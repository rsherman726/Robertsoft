using System.Collections.Generic;
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Linq;
using System.Data.Linq;
using System.Xml.Linq;
using AjaxControlToolkit;
using System.IO;
using System.Transactions;

public partial class ManagerOverview : System.Web.UI.Page
{
    #region Subs
    private void LoadManagers()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddlManagers.Items.Clear();
            var query = (from u in db.WipUsers
                         where u.RoleID == 3 //Managers
                         select new
                         {
                             u.UserID,
                             FullName = (u.FirstName + " " + (u.MiddleName ?? "") + " " + u.LastName).Replace("  ", " ")
                         });
            foreach (var a in query)
            {
                ddlManagers.Items.Add(new ListItem(a.FullName, a.UserID.ToString()));
            }
            ddlManagers.Items.Insert(0, new ListItem("All", "0"));
        }
    }
    private void BindManagerJobsTable(int iUserID)
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            if (iUserID == 0)//All Managers
            {
                var query = (from ja in db.WipJobAssigns
                             join pa in db.WipProductionLineAssigns on ja.Job equals pa.Job into pa_join
                             from pa in pa_join.DefaultIfEmpty()
                             join wm in db.WipMaster on ja.Job equals wm.Job into wm_join
                             from wm in wm_join.DefaultIfEmpty()

                             orderby
                               ja.DateAdded descending
                             select new
                             {
                                 Manager = (ja.WipUsers.FirstName + "" + (ja.WipUsers.MiddleName ?? "") + " " + ja.WipUsers.LastName).Replace("  ", " "),
                                 ja.Job,
                                 JobDescription = "stk#: " + wm.StockCode + " " + wm.JobDescription + "Job# (" + ja.Job + ")",
                                 CompletedDate = wm.ActCompleteDate,
                                 ja.DateAdded,
                                 ProductionLine = pa.WipProductionLines.LineName == null ? "Not Assigned" : pa.WipProductionLines.LineName,
                                 Status =
                                   (from wipemployeejobhours in db.WipEmployeeJobHours
                                    where
                                      wipemployeejobhours.Job == ja.Job
                                    select new
                                    {
                                        wipemployeejobhours
                                    }).Count() == 0 ? "Pending" : "Completed"
                             });

                dt = SharedFunctions.ToDataTable(db, query);
            }
            else//Selected a manager...
            {
                var query = (from ja in db.WipJobAssigns
                             join pa in db.WipProductionLineAssigns on ja.Job equals pa.Job into pa_join
                             from pa in pa_join.DefaultIfEmpty()
                             join wm in db.WipMaster on ja.Job equals wm.Job into wm_join
                             from wm in wm_join.DefaultIfEmpty()

                             where
                               ja.UserID == iUserID
                             orderby
                               ja.DateAdded descending
                             select new
                             {
                                 Manager = (ja.WipUsers.FirstName + "" + (ja.WipUsers.MiddleName ?? "") + " " + ja.WipUsers.LastName).Replace("  ", " "),
                                 ja.Job,
                                 JobDescription = "stk#: " + wm.StockCode + " " + wm.JobDescription + "Job# (" + int.Parse(ja.Job).ToString() + ")",
                                 CompletedDate = wm.ActCompleteDate,
                                 ja.DateAdded,
                                 ProductionLine = pa.WipProductionLines.LineName == null ? "Not Assigned" : pa.WipProductionLines.LineName,
                                 Status =
                                   (from wipemployeejobhours in db.WipEmployeeJobHours
                                    where
                                      wipemployeejobhours.Job == ja.Job
                                    select new
                                    {
                                        wipemployeejobhours
                                    }).Count() == 0 ? "Pending" : "Completed"
                             });

                dt = SharedFunctions.ToDataTable(db, query);
            }

            if (dt.Rows.Count > 0)
            {
                gvRecord.DataSource = dt;
                gvRecord.DataBind();
                Session["dtRecord"] = dt;
            }
            else//No records then create a dummy record to make Gridview still show up...
            {

                //Add Cols and MOAdd a blank row to the dataset
                DataTable dtDummy = new DataTable();
                dtDummy.Columns.Add("Manager");
                dtDummy.Columns.Add("Job");
                dtDummy.Columns.Add("JobDescription");
                dtDummy.Columns.Add("CompletedDate");
                dtDummy.Columns.Add("DateAdded");
                dtDummy.Columns.Add("ProductionLine");
                dtDummy.Columns.Add("Status");

                dtDummy.Rows.Add(dtDummy.NewRow());
                //Bind the DataSet to the GridView
                gvRecord.DataSource = dtDummy;
                gvRecord.DataBind();

                //Get the number of columns to know what the Column Span should be
                int columnCount = gvRecord.Rows[0].Cells.Count;
                //Call the clear method to clear out any controls that you use in the columns.  I use a dropdown list in one of the column so this was necessary.
                gvRecord.Rows[0].Cells.Clear();
                gvRecord.Rows[0].Cells.Add(new TableCell());
                gvRecord.Rows[0].Cells[0].ColumnSpan = columnCount;
                gvRecord.Rows[0].Cells[0].Text = "No Records Found.";

            }
            dt.Dispose();
        }
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        int iUserID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        else
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
            string sUrl = @"http://localhost" + Request.ServerVariables["URL"].ToString();//Just has to be a valid web path...
            Uri uri = new Uri(sUrl);
            string[] segments = uri.Segments;
            foreach (string s in segments)
            {
                sUrl = s;//Will hold the last segment...
            }
            string sText = SharedFunctions.GetMenuItemText(sUrl);
            int? iAdminID = SharedFunctions.GetAdminID(iUserID);
            if (!SharedFunctions.IsAccessGranted(iAdminID, sText))
            {
                Response.Redirect("Default.aspx");
            }
        }
        if (!Page.IsPostBack)
        {
            LoadManagers();
            BindManagerJobsTable(0);
        }
      
    }
    protected void gvRecord_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRecord.PageIndex = e.NewPageIndex;
        gvRecord.DataSource = (DataTable)Session["dtRecord"];
        gvRecord.DataBind();
    }
    protected void gvRecord_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblRecord = (Label)e.Row.FindControl("lblRecord");
            Label lblCompletedDate = (Label)e.Row.FindControl("lblCompletedDate");
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");

            if (lblStatus.Text == "Completed")
            {
                lblRecord.Text = "Completed";                
            }
            else
            {
                lblRecord.Text = "Pending";                 
            }
            if (lblCompletedDate.Text != "")
            {
                lblCompletedDate.Text = Convert.ToDateTime(lblCompletedDate.Text).ToShortDateString();
            }

        }

    }
    protected void ddlManagers_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iUserID = 0;
        iUserID = Convert.ToInt32(ddlManagers.SelectedValue);
        BindManagerJobsTable(iUserID);
    }


    #endregion

}