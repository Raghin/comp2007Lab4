using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//reference models
using Error_handling1.Models;
using System.Web.ModelBinding;

namespace Error_handling1
{
    public partial class department : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if the page isn't posted back, check the url for an id to see know add or edit
            if (!IsPostBack)
            {
                if (Request.QueryString.Keys.Count > 0)
                {
                    //we have a url parameter if the count is > 0 so populate the form
                    GetDepartment();
                }
            }
        }

        protected void GetDepartment()
        {
            try
            {
                //connect
                using (DefaultConnectionEF conn = new DefaultConnectionEF())
                {
                    //get id from url parameter and store in a variable
                    Int32 DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

                    var d = (from dep in conn.Departments
                             where dep.DepartmentID == DepartmentID
                             select dep).FirstOrDefault();

                    //populate the form from our department object
                    txtName.Text = d.Name;
                    txtBudget.Text = d.Budget.ToString();

                    var objE = (from en in conn.Courses
                                join c in conn.Departments on
                                 en.DepartmentID equals c.DepartmentID
                                where en.DepartmentID == DepartmentID
                                select new { en.Title, en.DepartmentID });

                    grdCourses.DataSource = objE.ToList();
                    grdCourses.DataBind();

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
                //connect
                using (DefaultConnectionEF conn = new DefaultConnectionEF())
                {
                    //instantiate a new deparment object in memory
                    Department d = new Department();

                    //decide if updating or adding, then save
                    if (Request.QueryString.Count > 0)
                    {
                        Int32 DepartmentID = Convert.ToInt32(Request.QueryString["DepartmentID"]);

                        d = (from dep in conn.Departments
                             where dep.DepartmentID == DepartmentID
                             select dep).FirstOrDefault();
                    }

                    //fill the properties of our object from the form inputs
                    d.Name = txtName.Text;
                    d.Budget = Convert.ToDecimal(txtBudget.Text);

                    if (Request.QueryString.Count == 0)
                    {
                        conn.Departments.Add(d);
                    }
                    conn.SaveChanges();

                    //redirect to updated departments page
                    Response.Redirect("departments.aspx");
                }
            }
            catch (Exception)
            {
                Response.Redirect("/error.aspx");
            }
        }

        protected void grdCourses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //get the selected Course
            Int32 DepartmentID = Convert.ToInt32(grdCourses.DataKeys[e.RowIndex].Values["DepartmentID"]);
            try
            {
                using (DefaultConnectionEF db = new DefaultConnectionEF())
                {
                    Course objE = (from en in db.Courses
                                   where en.DepartmentID == DepartmentID
                                   select en).FirstOrDefault();

                    db.Courses.Remove(objE);
                    db.SaveChanges();

                    //repopulate the page
                    GetDepartment();
                }
            }
            catch (Exception)
            {
                Response.Redirect("/error.aspx");
            }
        }
    }
}