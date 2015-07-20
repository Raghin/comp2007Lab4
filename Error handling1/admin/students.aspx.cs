using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//reference the EF models
using Error_handling1.Models;
using System.Web.ModelBinding;
using System.Linq.Dynamic;

namespace Error_handling1
{
    public partial class students : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if loading the page for the first time, populate student grid
            if (!IsPostBack)
            {
                Session["SortDirection"] = "ASC";
                Session["SortColumn"] = "StudentID";
                GetStudents();
            }
        }

        protected void GetStudents()
        {
            try
            {
                //connect to EF
                using (DefaultConnectionEF db = new DefaultConnectionEF())
                {

                    //query the students table using EF and LINQ
                    var Students = from s in db.Students
                                   select s;

                    //append the current direction to the Sort Column
                    String Sort = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                    //bind the result to the gridview
                    grdStudents.DataSource = Students.AsQueryable().OrderBy(Sort).ToList();
                    grdStudents.DataBind();

                }
            }
            catch (Exception)
            {
                Response.Redirect("/error.aspx");
            }
        }

        protected void grdStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //store which row was clicked
                Int32 selectedRow = e.RowIndex;

                //get the selected StudentID using the grid's Data Key collection
                Int32 StudentID = Convert.ToInt32(grdStudents.DataKeys[selectedRow].Values["StudentID"]);

                //use EF to remove the selected student from the db
                using (DefaultConnectionEF db = new DefaultConnectionEF())
                {

                    Student s = (from objS in db.Students
                                 where objS.StudentID == StudentID
                                 select objS).FirstOrDefault();

                    //do the delete
                    db.Students.Remove(s);
                    db.SaveChanges();
                }


                //refresh the grid
                GetStudents();
            }
            catch (Exception)
            {
                Response.Redirect("/error.aspx");
            }
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            //set the page size and refresh the grid
            grdStudents.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            GetStudents();
        }

        protected void grdStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //set the page index and refresh the grid
            grdStudents.PageIndex = e.NewPageIndex;
            GetStudents();
        }

        protected void grdStudents_Sorting(object sender, GridViewSortEventArgs e)
        {
            //set the global sort column to column clicked on by the user
            Session["SortColumn"] = e.SortExpression;
            GetStudents();

            //toggle the direction
            if (Session["SortDirection"].ToString() == "ASC")
            {
                Session["SortDirection"] = "DESC";
            }
            else
            {
                Session["SortDirection"] = "ASC";
            }
        }

        protected void grdStudents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Image SortImage = new Image();

                    for (int i = 0; i <= grdStudents.Columns.Count - 1; i++)
                    {
                        if (grdStudents.Columns[i].SortExpression == Session["SortColumn"].ToString())
                        {
                            if (Session["SortDirection"].ToString() == "DESC")
                            {
                                SortImage.ImageUrl = "~/images/desc.jpg";
                                SortImage.AlternateText = "Sort Descending";
                            }
                            else
                            {
                                SortImage.ImageUrl = "~/images/asc.jpg";
                                SortImage.AlternateText = "Sort Ascending";
                            }

                            e.Row.Cells[i].Controls.Add(SortImage);

                        }
                    }
                }

            }
        }
    }
}