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

public partial class Manager : System.Web.UI.Page
{

    private string GridViewSortDirection
    {
        get
        {
            return ViewState["SortDirection"] as string ?? "DESC";
        }
        set
        {
            ViewState["SortDirection"] = value;
        }
    }

    #region Subs
    private void BindManagerJobsTable(int iUserID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = new DataTable();

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
                             ja.Job,
                             JobNumber = Convert.ToInt64(ja.Job).ToString(),
                             wm.StockCode,
                             JobDescription = wm.JobDescription,
                             ja.DateAdded,
                             ProductionLine = pa.WipProductionLines.LineName == null ? "Not Assigned" : pa.WipProductionLines.LineName,
                             Status =
                               (from wipemployeejobhours in db.WipEmployeeJobHours
                                where
                                  wipemployeejobhours.Job == ja.Job
                                select new
                                {
                                    wipemployeejobhours
                                }).Count() == 0 ? "Record Hours" : "Completed"
                         });

            dt = SharedFunctions.ToDataTable(db, query);
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
                dtDummy.Columns.Add("Job");
                dtDummy.Columns.Add("JobDescription");
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


    #region Functions
    private string GetSortDirection()
    {
        switch (GridViewSortDirection)
        {
            case "ASC":
                GridViewSortDirection = "DESC";
                break;
            case "DESC":
                GridViewSortDirection = "ASC";
                break;
        }
        return GridViewSortDirection;
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
            BindManagerJobsTable(iUserID);
        }
        if (IsPostBack)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isPostBack", "isPostBack();", true);
        }
    }
    protected void gvRecord_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRecord.PageIndex = e.NewPageIndex;
        gvRecord.DataSource = (DataTable)Session["dtRecord"];
        gvRecord.DataBind();
    }
    protected void gvRecord_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int i = 0;
        switch (e.CommandName)
        {


        }
    }
    protected void gvRecord_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HyperLink hlRecord = (HyperLink)e.Row.FindControl("hlRecord");
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            Label lblJob = (Label)e.Row.FindControl("lblJob");

            hlRecord.ToolTip = "Add New Lead";
            hlRecord.Style.Add("Cursor", "pointer");
            string code = "window.open('ProductionInput.aspx?job=" + lblJob.Text + "','_blank','left=500, top=5, height=990, width=1024, status=no, resizable= no, scrollbars= yes, toolbar= no,location= no, menubar= no');";
            hlRecord.Attributes.Add("onclick", code);


            if (lblStatus.Text == "Completed")
            {
                hlRecord.Text = "Completed";
                hlRecord.Enabled = false;
            }
            else
            {
                hlRecord.Text = "Record Hours";
                hlRecord.Enabled = true;
            }

        }

    }
    protected void gvRecord_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        dtSortTable = (DataTable)Session["dtRecord"];
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvRecord.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvRecord.DataSource = m_DataView;
            gvRecord.DataBind();
            gvRecord.PageIndex = m_PageIndex;
            Session["dtSort"] = m_DataTable;
        }
    }
    protected void fromPopUp(object sender, EventArgs e)
    {//Call back from child page...
        if (Session["Event"] != null)
        {
            if ((int)Session["Event"] == 1)
            {//Fire get Data...
                int iUserID = Convert.ToInt32(Session["UserID"]);
                BindManagerJobsTable(iUserID);
            }
            else
            {//Fire Redirect...

            }
        }

    }
    #endregion




}