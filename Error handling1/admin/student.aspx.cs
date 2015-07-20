using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//model references for EF
using Error_handling1.Models;
using System.Web.ModelBinding;

namespace Error_handling1
{
    public partial class student : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if save wasn't clicked AND we have a StudentID in the url
            if ((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                GetStudent();
            }
        }

        protected void GetStudent()
        {
            //populate form with existing student record
            Int32 StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);
            try
            {
                //connect to db via EF
                using (DefaultConnectionEF db = new DefaultConnectionEF())
                {
                    //populate a student instance with the StudentID from the URL parameter
                    Student s = (from objS in db.Students
                                 where objS.StudentID == StudentID
                                 select objS).FirstOrDefault();

                    //map the student properties to the form controls if we found a match
                    if (s != null)
                    {
                        txtLastName.Text = s.LastName;
                        txtFirstMidName.Text = s.FirstMidName;
                        txtEnrollmentDate.Text = s.EnrollmentDate.ToString("yyyy-MM-dd");
                    }

                    var objE = (from en in db.Enrollments
                                join c in db.Courses on en.CourseID equals c.CourseID
                                join d in db.Departments on c.DepartmentID equals d.DepartmentID
                                where en.StudentID == StudentID
                                select new { en.Grade, en.EnrollmentID, c.Title, d.Name });

                    grdCourses.DataSource = objE.ToList();
                    grdCourses.DataBind();

                    //clear the dropdown list
                    ddlDepartment.ClearSelection();
                    ddlCourse.ClearSelection();

                    //fill the departments to dropdown
                    var deps = from d in db.Departments select d;
                    ddlDepartment.DataSource = deps.ToList();
                    ddlDepartment.DataBind();

                    //add default options to the 2 dropdowns
                    ListItem newItem = new ListItem("-Select-", "0");
                    ddlDepartment.Items.Insert(0, newItem);
                    ddlCourse.Items.Insert(0, newItem);

                    //show the courses
                    grdCourses.Visible = true;
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
                //use EF to connect to SQL Server
                using (DefaultConnectionEF db = new DefaultConnectionEF())
                {

                    //use the Student model to save the new record
                    Student s = new Student();
                    Int32 StudentID = 0;

                    //check the querystring for an id so we can determine add / update
                    if (Request.QueryString["StudentID"] != null)
                    {
                        //get the id from the url
                        StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

                        //get the current student from EF
                        s = (from objS in db.Students
                             where objS.StudentID == StudentID
                             select objS).FirstOrDefault();
                    }

                    s.LastName = txtLastName.Text;
                    s.FirstMidName = txtFirstMidName.Text;
                    s.EnrollmentDate = Convert.ToDateTime(txtEnrollmentDate.Text);

                    //call add only if we have no student ID
                    if (StudentID == 0)
                    {
                        db.Students.Add(s);
                    }

                    //run the update or insert
                    db.SaveChanges();

                    //redirect to the updated students page
                    Response.Redirect("students.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("/error.aspx");
            }
        }

        protected void grdCourses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //get the selected EnrollmentID
                Int32 EnrollmentID = Convert.ToInt32(grdCourses.DataKeys[e.RowIndex].Values["EnrollmentID"]);

                using (DefaultConnectionEF db = new DefaultConnectionEF())
                {
                    Enrollment objE = (from en in db.Enrollments
                                       where en.EnrollmentID == EnrollmentID
                                       select en).FirstOrDefault();

                    db.Enrollments.Remove(objE);
                    db.SaveChanges();

                    //repopulate the page
                    GetStudent();
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
                    Int32 StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);
                    Int32 CourseID = Convert.ToInt32(ddlCourse.SelectedValue);

                    //populate the new enrollment object
                    Enrollment objE = new Enrollment();
                    objE.StudentID = StudentID;
                    objE.CourseID = CourseID;

                    //save
                    db.Enrollments.Add(objE);
                    db.SaveChanges();

                    //refresh
                    GetStudent();
                }
            }
            catch (Exception)
            {
                Response.Redirect("/error.aspx");
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (DefaultConnectionEF db = new DefaultConnectionEF())
                {
                    //store the selected departmentID
                    Int32 DepartmentID = Convert.ToInt32(ddlDepartment.SelectedValue);

                    var objC = (from c in db.Courses
                                where c.DepartmentID == DepartmentID
                                orderby c.Title
                                select c);

                    //bind to course dropdrown
                    ddlCourse.DataSource = objC.ToList();
                    ddlCourse.DataBind();

                    ListItem newItem = new ListItem("-Select-", "0");
                    ddlCourse.Items.Insert(0, newItem);
                }
            }
            catch (Exception)
            {
                Response.Redirect("/error.aspx");
            }
        }
    }
}