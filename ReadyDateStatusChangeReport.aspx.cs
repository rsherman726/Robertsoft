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

public partial class ReadyDateStatusChangeReport : System.Web.UI.Page
{
    List<ProductionQuantities> lProductionQuantityAvailable = new List<ProductionQuantities>();

    decimal dcShortageTotal = 0;
    decimal dcQtyTotal = 0;

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



    #region Subs
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

        ExcelHelper.ToExcel(ds, sFileName, Page.Response);

    }
    private void LoadReadyDateStatusChangeReport()
    {
        string sMsg = "";
        string sStartDate = "NULL";
        if (txtStartDate.Text.Trim() != "")
        {//Not null then add quotes...
            sStartDate = "'" + txtStartDate.Text.Trim() + "'";
        }
        string sEndDate = "NULL";
        if (txtEndDate.Text.Trim() != "")
        {//Not null then add quotes...
            sEndDate = "'" + txtEndDate.Text.Trim() + "'";
        }
        //Stock Codes...        
        string sStockCodeFrom = "";
        string sStockCodeTo = "";

        if (txtStockCodeFrom.Text.Trim() != "")
        {
            sStockCodeFrom = "'" + txtStockCodeFrom.Text.Trim() + "'";
        }
        else
        {
            sMsg += "**From Stock Code is Required!<br/>";
        }
        if (txtStockCodeTo.Text.Trim() != "")
        {
            sStockCodeTo = "'" + txtStockCodeTo.Text.Trim() + "'";
        }
        else
        {
            sMsg += "**To Stock Code is Required! for Single StockCode use same StockCode in both To and From StockCode boxes!!<br/>";
        }


        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        //Validation...
        if (sStartDate != "NULL")
        {
            if (SharedFunctions.IsDate(sStartDate.Replace("'", "")) == false)
            {
                sMsg += "**Start Date is not a valid date!<br/>";
            }
        }

        if (sEndDate != "NULL")
        {
            if (SharedFunctions.IsDate(sEndDate.Replace("'", "")) == false)
            {
                sMsg += "**End Date is not a valid date!<br/>";
            }
        }


        if (sStartDate != "NULL" && sEndDate != "NULL")
        {
            if (SharedFunctions.IsDate(sStartDate.Replace("'", "")) == true && SharedFunctions.IsDate(sEndDate.Replace("'", "")) == true)
            {

                if (Convert.ToDateTime(sStartDate.Replace("'", "")) > Convert.ToDateTime(sEndDate.Replace("'", "")))
                {
                    sMsg += "**End Date can not come before Start Date!<br/>";
                }

            }
        }

        if (sStartDate == "NULL" && sEndDate != "NULL")
        {
            sMsg += "**If there is an End Date there must be a Start Date!<br/>";
        }
        if (sStartDate != "NULL" && sEndDate == "NULL")
        {
            sMsg += "**If there is an Start Date there must be a End Date!<br/>";
        }



        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }

        if (ddlPeriod.SelectedIndex == 0)
        {//All Time...
            if (chkCurrentTBDs.Checked)//Show TBDs Changed or not...
            {
                sSQL = "EXEC spGetReadyStatusChangeReportSummary ";
                sSQL += "@ShowAllTBDs='Y',";
                if (chkSHowAllOpenOrders.Checked)
                {
                    sSQL += "@OrderStatus='Filtered',";
                }
                if (chkShowSentAlerts.Checked)
                {
                    sSQL += "@ShowSent=1,";
                }
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo;
            }
            else
            {
                sSQL = "EXEC spGetReadyStatusChangeReportSummary ";
                if (chkTBD.Checked)
                {
                    sSQL += "@ReadyStatus='TBD',";
                }
                if (chkSHowAllOpenOrders.Checked)
                {
                    sSQL += "@OrderStatus='Filtered',";
                }
                if (chkShowSentAlerts.Checked)
                {
                    sSQL += "@ShowSent=1,";
                }
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo;
            }
        }
        else //Dates Supplied...
        {
            if (chkCurrentTBDs.Checked) //Show TBDs Changed or not...
            {
                sSQL = "EXEC spGetReadyStatusChangeReportSummary ";
                sSQL += "@ShowAllTBDs='Y',";
                if (chkSHowAllOpenOrders.Checked)
                {
                    sSQL += "@OrderStatus='Filtered',";
                }
                if (chkShowSentAlerts.Checked)
                {
                    sSQL += "@ShowSent=1,";
                }
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo;
            }
            else
            {
                sSQL = "EXEC spGetReadyStatusChangeReportSummary ";
                if (chkTBD.Checked)
                {
                    sSQL += "@ReadyStatus='TBD',";
                }
                if (chkSHowAllOpenOrders.Checked)
                {
                    sSQL += "@OrderStatus='Filtered',";
                }
                if (chkShowSentAlerts.Checked)
                {
                    sSQL += "@ShowSent=1,";
                }
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo;
            }
        }


        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");


        DataColumn column = new DataColumn();
        column.DataType = System.Type.GetType("System.Int32");
        column.ColumnName = "ID";
        dt.Columns.Add(column);
        if (dt.Rows.Count > 0)
        {
            //Set values for existing rows...
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["ID"] = i + 1;
            }
            Session["dtReadyStatusChanged"] = dt;
            gvReadyStatusDateChanged.DataSource = dt;
            gvReadyStatusDateChanged.DataBind();
            lblRecordCount.Text = "Record Count:  " + dt.Rows.Count.ToString();

        }
        else
        {
            gvReadyStatusDateChanged.DataSource = null;
            gvReadyStatusDateChanged.DataBind();
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
        int iRoleID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        else
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
            iRoleID = Convert.ToInt32(SharedFunctions.GetRole(iUserID));
            string sUrl = @"http://localhost" + Request.ServerVariables["URL"].ToString();//Just has to be a valid web path...
            Uri uri = new Uri(sUrl);
            string[] segments = uri.Segments;
            foreach (string s in segments)
            {
                sUrl = s;//Will hold the last segment...
            }
            string sText = SharedFunctions.GetMenuItemText(sUrl);
            int? iAdminID = SharedFunctions.GetAdminID(iUserID);

        }
        if (!Page.IsPostBack)
        {
            Session["dtReadyStatusChanged"] = null;
            ddlPeriod.SelectedIndex = 0;
            ddlPeriod_SelectedIndexChanged(ddlPeriod, null);
        }

    }
    protected void btnRunReport_Click(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
        lblError.Text = "";
        lblRecordCount.Text = "";
        LoadReadyDateStatusChangeReport();
    }
    protected void gvReadyStatusDateChanged_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblSalesOrder = (Label)e.Row.FindControl("lblSalesOrder");

            lblSalesOrder.Text = int.Parse(lblSalesOrder.Text).ToString();

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {

        }
    }
    protected void gvReadyStatusDateChanged_Sorting(object sender, GridViewSortEventArgs e)
    {

        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtReadyStatusChanged"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvReadyStatusDateChanged.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvReadyStatusDateChanged.DataSource = m_DataView;
            gvReadyStatusDateChanged.DataBind();
            gvReadyStatusDateChanged.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();

    }
    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlPeriod.SelectedIndex)
        {
            case 0://ALL...
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                break;
            case 1://Range
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                break;
            case 2://3 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-3).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 3://6 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-6).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 4://9 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-9).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 5://12 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-12).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
        }
    }
    protected void imgExportExcel_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";

        if (Session["dtReadyStatusChanged"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtReadyStatusChanged"];

        try
        {
            dt.Columns.Remove("ID");
        }
        catch (Exception)
        {

            //ignore...
        }

        dt.TableName = "dtReadyStatusChanged";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "ReadyStatusChanged" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtStockCodeFrom.Text = "000000";
        txtStockCodeTo.Text = "999999";
        chkSHowAllOpenOrders.Checked = true;
        chkTBD.Checked = false;
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        ddlPeriod.SelectedIndex = 0;
    }
    protected void chkTBD_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTBD.Checked)
        {
            chkCurrentTBDs.Checked = false;
        }
    }

    protected void chkCurrentTBDs_CheckedChanged(object sender, EventArgs e)
    {
        if (chkCurrentTBDs.Checked)
        {
            chkTBD.Checked = false;
        }
    }
    #endregion






}
