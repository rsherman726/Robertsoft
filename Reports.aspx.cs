using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Data.Linq;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Drawing;

public partial class Reports : System.Web.UI.Page
{
    private string sDate = "";
    private string sProductionLine = "";
    private string sStockCode = "";
    private string sStockDescription = "";
    private string sQtyManufactured = "";
    private string sJob = "";
    private string sUOM = "";

    private decimal dcTotalHours = 0;

    #region Properties
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

    #endregion

    #region   Subs
    private void LoadEmployees()
    {//NOTE:Criteria could change..Uses first day of month until today...
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lbEmployees.Items.Clear();
        var query = (from e in db.WipEmployees 
                     orderby e.FirstName,e.LastName
                     select new
                     {
                         e.EmployeeID,
                         FullName = (e.FirstName + " " + (e.MiddleName ?? "") + " " + e.LastName).Replace("  ", " ")
                     });
        foreach (var a in query)
        {
            lbEmployees.Items.Add(new ListItem(a.FullName, a.EmployeeID.ToString()));
        }

    }
    private void LoadProductionLines()
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lbProductionLine.Items.Clear();
        var query = (from p in db.WipProductionLines
                     orderby p.LineName
                     select new
                     {
                         p.ProLineID,
                         p.LineName
                     });
        foreach (var a in query)
        {
            lbProductionLine.Items.Add(new ListItem(a.LineName, a.ProLineID.ToString()));
        }        
    }
    private void LoadStockCodes()
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lbStockCode.Items.Clear();
        var query = (from a in
                         (
                             from ja in db.WipJobAssigns
                             join jh in db.WipEmployeeJobHours on ja.Job equals jh.Job
                             join wm in db.WipMaster on ja.Job equals wm.Job into wm_join
                             from wm in wm_join.DefaultIfEmpty()
                             where
                               jh.Hours != null
                             group new { ja, wm } by new
                             {
                                 wm.JobDescription,
                                 wm.StockCode
                             } into g
                             select new
                             {
                                 g.Key.JobDescription,
                                 g.Key.StockCode
                             })
                             orderby a.StockCode
                     select new { a.JobDescription,a.StockCode});
        foreach (var a in query)
        {
            lbStockCode.Items.Add(new ListItem("Stock Code: " + a.StockCode + " - " + a.JobDescription , a.StockCode));
        }
    }
    private void ExportToExcel(DataSet ds, string sFileName)
    {

        if (ds != null)
        {

            if (ds.Tables.Count == 0)
            {
                return;
            }
        }
        else
        {
            return;
        }

        ExcelHelper.ToExcel(ds, sFileName , Page.Response);

    }

    //private void GetProductivityReportData()
    //{
    //    DataSet ds = new DataSet();
    //    string sSQL = "";
    //    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        
    //    //TODO: AutoMate ExchangeRate...
    //    sSQL = "EXEC spGetProductivityReport";

    //    ds = SharedFunctions.getDataSet(sSQL, conn);

    //    if (ds.Tables.Count > 0)
    //    {
    //        gvReports.DataSource = ds.Tables[0];
    //        gvReports.DataBind();
    //        Session["dsReports"] = ds;
    //        lblRecordCount.Text = ds.Tables[0].Rows.Count + " records";
    //    }
    //    else//No records then create a dummy record to make Gridview still show up...
    //    {
    //        DataTable dtDummy = new DataTable();
    //        //Add a blank row to the dataset
    //        dtDummy.Rows.Add(dtDummy.NewRow());
    //        //Bind the DataSet to the GridView
    //        gvReports.DataSource = dtDummy;
    //        gvReports.DataBind();
    //        //Get the number of columns to know what the Column Span should be
    //        int columnCount = gvReports.Rows[0].Cells.Count;
    //        //Call the clear method to clear out any controls that you use in the columns.  I use a dropdown list in one of the column so this was necessary.
    //        gvReports.Rows[0].Cells.Clear();
    //        gvReports.Rows[0].Cells.Add(new TableCell());
    //        gvReports.Rows[0].Cells[0].ColumnSpan = columnCount;
    //        gvReports.Rows[0].Cells[0].Text = "No Records Found.";
    //        dtDummy.Dispose();
    //    }

    //    ds.Dispose();
        
    //}
    private void GetProductivityReportData(int iGroup)
    {
        DataSet ds = new DataSet();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lblError.Text = "";
        string sMsg = "";
        string sStartDate = "";
        string sEndDate = "";
        //If date are blank they must be nulled out!!!
        if (txtStartDate.Text.Trim() == "")
        {
            sStartDate = null;
        }
        else
        {
            sStartDate = txtStartDate.Text.Trim();
        }
        if (txtEndDate.Text.Trim() == "")
        {
            sEndDate = null;
        }
        else
        {
            sEndDate = txtEndDate.Text.Trim();
        }
        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            lblError.ForeColor = Color.Red;
            return;
        }


        int iLastPipeInStringIdx = 0;
        string sProductionLines = "";
        string sEmployeeIDs = "";
        string sStockCodes = "";

        //Production Lines...
        for (int i = 0; i < lbProductionLine.Items.Count; i++)
        {
            if (lbProductionLine.Items[i].Selected)
            {
                sProductionLines += lbProductionLine.Items[i].Value.Trim() + "|";
            }
        }
        if (sProductionLines.Trim().EndsWith("|"))
        {
            iLastPipeInStringIdx = sProductionLines.LastIndexOf("|");
            sProductionLines = sProductionLines.Remove(iLastPipeInStringIdx).Trim();
        }
        //Employees...
        for (int i = 0; i < lbEmployees.Items.Count; i++)
        {
            if (lbEmployees.Items[i].Selected)
            {
                sEmployeeIDs += lbEmployees.Items[i].Value.Trim() + "|";
            }
        }
        if (sEmployeeIDs.Trim().EndsWith("|"))
        {
            iLastPipeInStringIdx = sEmployeeIDs.LastIndexOf("|");
            sEmployeeIDs = sEmployeeIDs.Remove(iLastPipeInStringIdx).Trim();
        }
        //Stock Codes...
        for (int i = 0; i < lbStockCode.Items.Count; i++)
        {
            if (lbStockCode.Items[i].Selected)
            {
                sStockCodes += lbStockCode.Items[i].Value.Trim() + "|";
            }
        }
        if (sStockCodes.Trim().EndsWith("|"))
        {
            iLastPipeInStringIdx = sStockCodes.LastIndexOf("|");
            sStockCodes = sStockCodes.Remove(iLastPipeInStringIdx).Trim();
        }

        if (sStartDate == null && sEndDate == null)
        {
            sSQL = "EXEC spGetProductivityReport  @ProductionLines='" + sProductionLines + "',@FromDate=null,@ToDate=null,@EmployeeIDs='" + sEmployeeIDs + "',@StockCodes='" + sStockCodes + "',@Group=" + iGroup;
        }
        else
        {
            sSQL = "EXEC spGetProductivityReport  @ProductionLines='" + sProductionLines + "',@FromDate='" + sStartDate + "',@ToDate='" + sEndDate + "',@EmployeeIDs='" + sEmployeeIDs + "',@StockCodes='" + sStockCodes + "',@Group=" + iGroup;
        }
        Debug.WriteLine(sSQL);
        ds = SharedFunctions.getDataSet(sSQL, conn, "Employees");

        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            dt.Columns[0].ColumnName = "StockCode";
            dt.Columns[1].ColumnName = "Stock Description";
            dt.Columns[2].ColumnName = "Job";
            dt.Columns[3].ColumnName = "Qty";
            dt.Columns[4].ColumnName = "Material Cost";
            dt.Columns[5].ColumnName = "Labor";
            dt.Columns[6].ColumnName = "OH";
            dt.Columns[7].ColumnName = "Total Cost";
            dt.Columns[8].ColumnName = "Material CPU";
            dt.Columns[9].ColumnName = "Labor CPU";
            dt.Columns[10].ColumnName = "OH CPU";
            dt.Columns[11].ColumnName = "COGS CPU";
            dt.Columns.Remove("AssignDate");
            dt.Columns.Remove("ProLine");
   
            gvReports.PageSize = Convert.ToInt32(ddlDisplayCount.SelectedValue);
            gvReports.DataSource = dt;
            gvReports.DataBind();
            
            Session["dsReports"] = ds;            

            gvReports.AllowSorting = true;
            lblRecordCount.Text = ds.Tables[0].Rows.Count + " records";
        }
        else//No records then create a dummy record to make Gridview still show up...
        {
            //Add Cols and MOAdd a blank row to the dataset
            DataTable dtDummy = new DataTable();
            dtDummy.Columns.Add("StockCode");
            dtDummy.Columns.Add("StockDescription");
            dtDummy.Columns.Add("Job");
            dtDummy.Columns.Add("QtyManufactured");
            dtDummy.Columns.Add("MaterialCost");
            dtDummy.Columns.Add("Labor");
            dtDummy.Columns.Add("OH");
            dtDummy.Columns.Add("TotalCost");
            dtDummy.Columns.Add("MaterialCPU");
            dtDummy.Columns.Add("LaborCPU");
            dtDummy.Columns.Add("OHCPU");
            dtDummy.Columns.Add("COGSCPU");
           
            dtDummy.Rows.Add(dtDummy.NewRow());
            //Bind the DataSet to the GridView
            gvReports.DataSource = dtDummy;
            gvReports.DataBind();
            //Get the number of columns to know what the Column Span should be
            int columnCount = gvReports.Rows[0].Cells.Count;
            //Call the clear method to clear out any controls that you use in the columns.  I use a dropdown list in one of the column so this was necessary.
            gvReports.Rows[0].Cells.Clear();
            gvReports.Rows[0].Cells.Add(new TableCell());
            gvReports.Rows[0].Cells[0].ColumnSpan = columnCount;
            gvReports.Rows[0].Cells[0].Text = "No Records Found.";
            dtDummy.Dispose();
        }

        ds.Dispose();
    }
    private void GetLaborHoursReportData(int iGroup)
    {
        DataSet ds = new DataSet();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lblError.Text = "";
        string sMsg = "";
        string sStartDate = "";
        string sEndDate = "";
        //If date are blank they must be nulled out!!!
        if (txtStartDate.Text.Trim() == "")
        {
            sStartDate = null;
        }
        else
        {
            sStartDate = txtStartDate.Text.Trim();
        }
        if (txtEndDate.Text.Trim() == "")
        {
            sEndDate = null;
        }
        else
        {
            sEndDate = txtEndDate.Text.Trim();
        }
        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            lblError.ForeColor = Color.Red;
            return;
        }


        int iLastPipeInStringIdx = 0;
        string sProductionLines = "";
        string sEmployeeIDs = "";
        string sStockCodes = "";

        //Production Lines...
        for (int i = 0; i < lbProductionLine.Items.Count; i++)
        {
            if (lbProductionLine.Items[i].Selected)
            {
                sProductionLines += lbProductionLine.Items[i].Value.Trim() + "|";
            }
        }
        if (sProductionLines.Trim().EndsWith("|"))
        {
            iLastPipeInStringIdx = sProductionLines.LastIndexOf("|");
            sProductionLines = sProductionLines.Remove(iLastPipeInStringIdx).Trim();
        }
        //Employees...
        for (int i = 0; i < lbEmployees.Items.Count; i++)
        {
            if (lbEmployees.Items[i].Selected)
            {
                sEmployeeIDs += lbEmployees.Items[i].Value.Trim() + "|";
            }
        }
        if (sEmployeeIDs.Trim().EndsWith("|"))
        {
            iLastPipeInStringIdx = sEmployeeIDs.LastIndexOf("|");
            sEmployeeIDs = sEmployeeIDs.Remove(iLastPipeInStringIdx).Trim();
        }
        //Stock Codes...
        for (int i = 0; i < lbStockCode.Items.Count; i++)
        {
            if (lbStockCode.Items[i].Selected)
            {
                sStockCodes += lbStockCode.Items[i].Value.Trim() + "|";
            }
        }
        if (sStockCodes.Trim().EndsWith("|"))
        {
            iLastPipeInStringIdx = sStockCodes.LastIndexOf("|");
            sStockCodes = sStockCodes.Remove(iLastPipeInStringIdx).Trim();
        }

        if (sStartDate == null && sEndDate == null)
        {
            sSQL = "EXEC spGetLaborHoursReport  @ProductionLines='" + sProductionLines + "',@FromDate=null,@ToDate=null,@EmployeeIDs='" + sEmployeeIDs + "',@StockCodes='" + sStockCodes + "',@Group=" + iGroup;
        }
        else
        {
            sSQL = "EXEC spGetLaborHoursReport  @ProductionLines='" + sProductionLines + "',@FromDate='" + sStartDate + "',@ToDate='" + sEndDate + "',@EmployeeIDs='" + sEmployeeIDs + "',@StockCodes='" + sStockCodes + "',@Group=" + iGroup;
        }
        Debug.WriteLine(sSQL);
        ds = SharedFunctions.getDataSet(sSQL, conn, "Employees");

        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            dt.Columns[0].ColumnName = "Date";
            dt.Columns[1].ColumnName = "ProductionLine";
            dt.Columns[2].ColumnName = "StockCode";
            dt.Columns[3].ColumnName = "StockDescription";
            dt.Columns[4].ColumnName = "QtyManufactured";
            dt.Columns[5].ColumnName = "Job";
            dt.Columns[6].ColumnName = "UOM";
            dt.Columns[7].ColumnName = "EmployeeName";
            dt.Columns[8].ColumnName = "Hours";

            gvReportsLaborHours.PageSize = Convert.ToInt32(ddlDisplayCount.SelectedValue);
            gvReportsLaborHours.DataSource = dt;
            gvReportsLaborHours.DataBind();
          
            Session["dsReportsLaborHours"] = ds;
            lblRecordCount.Text = ds.Tables[0].Rows.Count + " records";
        }
        else//No records then create a dummy record to make Gridview still show up...
        {
            //Add Cols and MOAdd a blank row to the dataset
            DataTable dtDummy = new DataTable();
            dtDummy.Columns.Add("Date"); 
            dtDummy.Columns.Add("ProductionLine");
            dtDummy.Columns.Add("StockCode");
            dtDummy.Columns.Add("StockDescription");
            dtDummy.Columns.Add("QtyManufactured");
            dtDummy.Columns.Add("Job");
            dtDummy.Columns.Add("UOM");
            dtDummy.Columns.Add("EmployeeName");
            dtDummy.Columns.Add("Hours"); 

            dtDummy.Rows.Add(dtDummy.NewRow());
            //Bind the DataSet to the GridView
            gvReportsLaborHours.DataSource = dtDummy;
            gvReportsLaborHours.DataBind();
            //Get the number of columns to know what the Column Span should be
            int columnCount = gvReportsLaborHours.Rows[0].Cells.Count;
            //Call the clear method to clear out any controls that you use in the columns.  I use a dropdown list in one of the column so this was necessary.
            gvReportsLaborHours.Rows[0].Cells.Clear();
            gvReportsLaborHours.Rows[0].Cells.Add(new TableCell());
            gvReportsLaborHours.Rows[0].Cells[0].ColumnSpan = columnCount;
            gvReportsLaborHours.Rows[0].Cells[0].Text = "No Records Found.";
            dtDummy.Dispose();
        }

        ds.Dispose();
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
            LoadEmployees();
            LoadProductionLines();
            LoadStockCodes();
        }

    }
    //protected void ddlReports_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    gvReports.DataSource = null;
    //    gvReports.DataBind();

    //    lbEmployees.SelectedIndex = -1;
    //    lbProductionLine.SelectedIndex = -1;
    //    lbStockCode.SelectedIndex = -1;

    //}

    protected void btnRun_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (ddlReports.SelectedIndex == 0)
        {
            lblError.Text = "**No report selected!";
            return;
        }

        DataTable dt = new DataTable();
        DataTable dtSummary = new DataTable();
        string sMsg = "";
        string sStartDate = "";
        string sEndDate = "";
        sStartDate = txtStartDate.Text.Trim();
        sEndDate = txtEndDate.Text.Trim();

        switch (ddlReports.SelectedIndex)
        {
            case 1:
                if (sStartDate != "")
                {
                    if (SharedFunctions.IsDate(sStartDate) == false)
                    {
                        sMsg += "**Start Date is not a valid date!<br/>";
                    }
                }

                if (sEndDate != "")
                {
                    if (SharedFunctions.IsDate(sEndDate) == false)
                    {
                        sMsg += "**End Date is not a valid date!<br/>";
                    }
                }

                if (sStartDate != "" && sEndDate != "")
                {
                    if (SharedFunctions.IsDate(sStartDate) == true && SharedFunctions.IsDate(sEndDate) == true)
                    {
                        if (Convert.ToDateTime(sStartDate) > Convert.ToDateTime(sEndDate))
                        {
                            sMsg += "**End Date can not come before Start Date!<br/>";
                        }

                    }
                }
                if (sStartDate == "" && sEndDate != "")
                {
                    sMsg += "**If there is an End Date there must be a Start Date!<br/>";
                }
                if (sStartDate != "" && sEndDate == "")
                {
                    sMsg += "**If there is an Start Date there must be a End Date!<br/>";
                }
                if (chkGroup.Checked)
                {
                    if (lbEmployees.SelectedIndex == -1)
                    {
                        sMsg += "**You have select Employee group option but you did not select any employees.<br/>";
                    }
                }
                break;
            case 2:

                //Valadation...
                if (sStartDate != "")
                {
                    if (SharedFunctions.IsDate(sStartDate) == false)
                    {
                        sMsg += "**Start Date is not a valid date!<br/>";
                    }
                }

                if (sEndDate != "")
                {
                    if (SharedFunctions.IsDate(sEndDate) == false)
                    {
                        sMsg += "**End Date is not a valid date!<br/>";
                    }
                }

                if (sStartDate != "" && sEndDate != "")
                {
                    if (SharedFunctions.IsDate(sStartDate) == true && SharedFunctions.IsDate(sEndDate) == true)
                    {
                        if (Convert.ToDateTime(sStartDate) > Convert.ToDateTime(sEndDate))
                        {
                            sMsg += "**End Date can not come before Start Date!<br/>";
                        }

                    }
                }
                if (sStartDate == "" && sEndDate != "")
                {
                    sMsg += "**If there is an End Date there must be a Start Date!<br/>";
                }
                if (sStartDate != "" && sEndDate == "")
                {
                    sMsg += "**If there is an Start Date there must be a End Date!<br/>";
                }
                if (chkGroup.Checked)
                {
                    if (lbEmployees.SelectedIndex == -1)
                    {
                        sMsg += "**You have select Employee group option but you did not select any employees.<br/>";
                    }
                }
                break;

            default:
                break;
        }
        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }

        switch (ddlReports.SelectedIndex)
        {
            case 0:
                pnlLaborControls.Visible = false;
                lblError.Text = "**No report Selected!";
                break;
            case 1://Referral Report...
                pnlLaborControls.Visible = true;
                if (chkGroup.Checked)
                {
                    GetProductivityReportData(1);
                }
                else
                {
                    GetProductivityReportData(0);
                }
                break;
            case 2:
                pnlLaborControls.Visible = true;
                if (chkGroup.Checked)
                {
                    GetLaborHoursReportData(1);
                }
                else
                {
                    GetLaborHoursReportData(0);
                }
                break;
 
        }      


    }
    protected void lbnSelectAllStockCode_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbStockCode.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearStockCode_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbStockCode.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    protected void lbnSelectAllEmployee_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbEmployees.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearEmployee_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbEmployees.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    protected void lbnSelectAllProLine_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbProductionLine.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearProLine_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbProductionLine.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    protected void gvReports_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReports.PageIndex = e.NewPageIndex;
        //use data in memory...
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        ds = (DataSet)Session["dsReports"];
        dt = ds.Tables[0];
        gvReports.DataSource = dt;
        gvReports.DataBind();
    }
    protected void gvReports_Sorting(object sender, GridViewSortEventArgs e)
    {

        SharedFunctions.Check_Session("dsReports");
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        DataSet ds = new DataSet();
        ds =  (DataSet)Session["dsReports"];
        dtSortTable = ds.Tables[0];
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvReports.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvReports.DataSource = m_DataView;
            gvReports.DataBind();
            gvReports.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
    }
    protected void gvReports_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            switch (ddlReports.SelectedIndex)
            {
                case 1:
                    e.Row.Cells[0].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[1].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[2].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[3].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[4].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[5].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[6].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[7].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[8].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[9].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[10].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[11].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    break;
                case 2:

                    e.Row.Cells[0].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[1].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[2].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[3].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[4].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[5].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[6].ToolTip = "Click Once to sort acending and a second time to sort descending...";
                    e.Row.Cells[7].ToolTip = "Click Once to sort acending and a second time to sort descending...";

                    break;
            }

        }
    }
    protected void imgExportExcel_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        string sFilesName = "";



        switch (ddlReports.SelectedIndex)
        {
            case 1:
                if (Session["dsReports"] == null)
                {
                    lblError.Text = "**No Report in memory to export!";
                    return;
                }
                ds = (DataSet)Session["dsReports"];
                sFilesName = "LaborProductionReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                ExportToExcel(ds, sFilesName);
                break;
            case 2:

                if (Session["dsReportsLaborHours"] == null)
                {
                    lblError.Text = "**No Report in memory to export!";
                    return;
                }
                ds = (DataSet)Session["dsReportsLaborHours"];

                string sEmployeeName = "";
                foreach (DataTable dt in ds.Tables)
                { //Rename the tables in the DataSet with the Officers fullname so they appear on the Excel individual tabs...

                    if (chkGroup.Checked)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            sEmployeeName = (dt.Rows[0]["EmployeeName"].ToString());
                            dt.TableName = sEmployeeName;
                        }
                    }
                }

                sFilesName = "LaborHoursReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                ExportToExcel(ds, sFilesName);
                break;
        }
        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void gvReportsLaborHours_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcHours = 0;

        try
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblDate = (Label)e.Row.FindControl("lblDate");
                Label lblProductionLine = (Label)e.Row.FindControl("lblProductionLine");
                Label lblStockCode = (Label)e.Row.FindControl("lblStockCode");
                Label lblStockDescription = (Label)e.Row.FindControl("lblStockDescription");
                Label lblQtyManufactured = (Label)e.Row.FindControl("lblQtyManufactured");
                Label lblJob = (Label)e.Row.FindControl("lblJob");
                Label lblUOM = (Label)e.Row.FindControl("lblUOM");
                Label lblHours = (Label)e.Row.FindControl("lblHours");

                if (lblHours.Text != "")
                {
                    dcHours = Convert.ToDecimal(lblHours.Text);
                    dcTotalHours += dcHours;
                }

                if (lblDate.Text.Trim() == sDate && lblProductionLine.Text.Trim() == sProductionLine && lblStockCode.Text.Trim() == sStockCode &&
                    lblStockDescription.Text.Trim() == sStockDescription && lblQtyManufactured.Text.Trim() == sQtyManufactured && lblJob.Text.Trim() == sJob
                    && lblUOM.Text.Trim() == sUOM)
                {
                    lblDate.Text = "";
                    lblProductionLine.Text = "";
                    lblStockCode.Text = "";
                    lblStockDescription.Text = "";
                    lblQtyManufactured.Text = "";
                    lblJob.Text = "";
                    lblUOM.Text = "";
                }

                sDate = lblDate.Text.Trim();
                sProductionLine = lblProductionLine.Text.Trim();
                sStockCode = lblStockCode.Text.Trim();
                sStockDescription = lblStockDescription.Text.Trim();
                sQtyManufactured = lblQtyManufactured.Text.Trim();
                sJob = lblJob.Text.Trim();
                sUOM = lblUOM.Text.Trim();
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotalHours = (Label)e.Row.FindControl("lblTotalHours");
                lblTotalHours.Text = dcTotalHours.ToString("0.00");
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvReportsLaborHours_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReportsLaborHours.PageIndex = e.NewPageIndex;
        //use data in memory...
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        ds = (DataSet)Session["dsReportsLaborHours"];
        dt = ds.Tables[0];
        gvReportsLaborHours.DataSource = dt;
        gvReportsLaborHours.DataBind();
    }
    protected void ddlDisplayCount_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlReports.SelectedIndex)
        {
            case 0:
                pnlLaborControls.Visible = false;
                lblError.Text = "**No report Selected!";
                break;
            case 1://Productivity Report...
                pnlLaborControls.Visible = true;
                if (chkGroup.Checked)
                {
                    GetProductivityReportData(1);
                }
                else
                {
                    GetProductivityReportData(0);
                }
                break;
            case 2://Labor Hours Report...
                pnlLaborControls.Visible = true;
                if (chkGroup.Checked)
                {
                    GetLaborHoursReportData(1);
                }
                else
                {
                    GetLaborHoursReportData(0);
                }
                break;
        }
    }
    protected void ddlReports_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlReports.SelectedIndex)
        {
            case 0:
                pnlLaborControls.Visible = false;
                lblError.Text = "**No report Selected!";
                break;
            case 1://Productivity Report...
                pnlLaborControls.Visible = true;
                gvReportsLaborHours.DataSource = null;
                gvReportsLaborHours.DataBind();
                break;
            case 2://Labor Hours Report...
                pnlLaborControls.Visible = true;
                gvReports.DataSource = null;
                gvReports.DataBind();
                break;
        }
    }
    #endregion











}