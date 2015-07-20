using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Error_handling1.Models;

namespace Error_handling1
{
    public partial class course : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetDepartments();

                //get the course if editing
                if (!String.IsNullOrEmpty(Request.QueryString["CourseID"]))
                {
                    GetCourse();
                }
            }
        }

        protected void GetCourse()
        {
            //populate the existing course for editing
            try
            {
                using (DefaultConnectionEF db = new DefaultConnectionEF())
                {
                    Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);

                    Course objC = (from c in db.Courses
                                   where c.CourseID == CourseID
                                   select c).FirstOrDefault();

                    //populate the form
                    txtTitle.Text = objC.Title;
                    txtCredits.Text = objC.Credits.ToString();
                    ddlDepartment.SelectedValue = objC.DepartmentID.ToString();

                    var objE = (from en in db.Enrollments
                                join d in db.Students on en.StudentID equals d.StudentID
                                join e in db.Courses on en.CourseID equals e.CourseID
                                where en.CourseID == CourseID
                                select new { en.EnrollmentID, d.LastName, d.FirstMidName });

                    grdStudents.DataSource = objE.ToList();
                    grdStudents.DataBind();

                    ddlStudent.ClearSelection();

                    var studs = from d in db.Students select d;
                    ddlStudent.DataSource = studs.ToList();
                    ddlStudent.DataBind();

                    ListItem newItem = new ListItem("-Select-", "0");
                    ddlStudent.Items.Insert(0, newItem);

                }
            }
            catch (Exception)
            {
                Response.Redirect("/error.aspx");
            }
        }

        protected void GetDepartments()
        {
            try
            {
                using (DefaultConnectionEF db = new DefaultConnectionEF())
                {
                    var deps = (from d in db.Departments
                                orderby d.Name
                                select d);

                    ddlDepartment.DataSource = deps.ToList();
                    ddlDepartment.DataBind();
                }
            }
            catch (Exception)
            {
                Response.Redirect("/error.aspx");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //do insert or update
                using (DefaultConnectionEF db = new DefaultConnectionEF())
                {
                    Course objC = new Course();

                    if (!String.IsNullOrEmpty(Request.QueryString["CourseID"]))
                    {
                        Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                        objC = (from c in db.Courses
                                where c.CourseID == CourseID
                                select c).FirstOrDefault();
                    }

                    //populate the course from the input form
                    objC.Title = txtTitle.Text;
                    objC.Credits = Convert.ToInt32(txtCredits.Text);
                    objC.DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);

                    if (String.IsNullOrEmpty(Request.QueryString["CourseID"]))
                    {
                        //add
                        db.Courses.Add(objC);
                    }

                    //save and redirect
                    db.SaveChanges();
                    Response.Redirect("courses.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("/error.aspx");
            }
        }

        protected void grdStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //get the selected EnrollmentID
            Int32 EnrollmentID = Convert.ToInt32(grdStudents.DataKeys[e.RowIndex].Values["EnrollmentID"]);
            try
            {
                using (DefaultConnectionEF db = new DefaultConnectionEF())
                {
                    Enrollment objE = (from en in db.Enrollments
                                       where en.EnrollmentID == EnrollmentID
                                       select en).FirstOrDefault();

                    db.Enrollments.Remove(objE);
                    db.SaveChanges();

                    //repopulate the page
                    GetCourse();
                }
            }
            catch (Exception)
            {
                Response.Redirect("/error.aspx");
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (DefaultConnectionEF db = new DefaultConnectionEF())
                {
                    //get the values needed
                    Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
                    Int32 StudentID = Convert.ToInt32(ddlStudent.SelectedValue);

                    //populate the new enrollment object
                    Enrollment objE = new Enrollment();
                    objE.StudentID = StudentID;
                    objE.CourseID = CourseID;

                    //save
                    db.Enrollments.Add(objE);
                    db.SaveChanges();

                    //refresh
                    GetCourse();
                }
            }
            catch (Exception)
            {
                Response.Redirect("/error.aspx");
            }
        }
    }
}