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
using Microsoft.VisualBasic.FileIO;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class PurchaseOrderTracker : System.Web.UI.Page
{
    static int TickNumber;
    string _purchaseOrder;
    string _supplier;
    string _PO_Date;
    decimal dcLineTotal = 0;
    decimal dcGrandTotal = 0;

    decimal dcQtyTotal = 0;
    decimal dcOpenOrdersTotal = 0;
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
    public string PurchaseOrder
    {
        get
        {
            return _purchaseOrder;
        }
        set
        {
            _purchaseOrder = value;
        }
    }
    public string PurchaseOrderDate
    {
        get
        {
            return _PO_Date;
        }
        set
        {
            _PO_Date = value;
        }
    }
    public string SupplierInfo
    {
        get
        {
            return _supplier;
        }
        set
        {
            _supplier = value;
        }
    }
    #endregion

    #region Subs
    protected void ExportPDF(string sFormat)
    {
        if (Session["dt"] == null)
        {
            lblError.Text = "**No Data in memory!";
            return;
        }
        DataTable dtDoc = (DataTable)Session["dt"];

        ReportDocument crystalReport = GetReportNew(dtDoc);
        ExportFormatType formatType = ExportFormatType.NoFormat;
        switch (sFormat)
        {
            case "Word":
                formatType = ExportFormatType.WordForWindows;
                break;
            case "PDF":
                formatType = ExportFormatType.PortableDocFormat;
                break;
            case "Excel":
                formatType = ExportFormatType.Excel;
                break;
            case "CSV":
                formatType = ExportFormatType.CharacterSeparatedValues;
                break;
        }
        string sFileName = "";
        sFileName = "PurchaseOrderDetailsReport_" + DateTime.Now.ToString();
        crystalReport.ExportToHttpResponse(formatType, Response, true, sFileName);
        Response.End();
    }
    private void RunPDFPurchaseOrderDetailsReport()
    {
        DataTable dt = new DataTable();



        string sSQL = "EXEC spGetPurchaseOrderTrackerDetailsForPO @PurchaseOrder='" + PurchaseOrder + "'";
        SqlConnection conn = null;

        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dtPurchaseOrderDetails");
        if (dt.Rows.Count > 0)
        {

            Session["dt"] = dt;//First before SetPageIndex to clear values, since setPageIndex fires PageIndexChanging...

            imgbtnPDF.Visible = true;
            imgbtnWord.Visible = true;
            imgbtnExcel.Visible = true;
            lblOptions.Visible = true;
            GetReportNew(dt);
            dt.Dispose();
        }
        else
        {
            imgbtnPDF.Visible = false;
            imgbtnWord.Visible = false;
            imgbtnExcel.Visible = false;
            lblOptions.Visible = false;
        }
    }
    #endregion

    #region Functions
    private DataTable GetOpenPurchaseOrders()
    {//Default...(HEADER)
        DataTable dt = new DataTable();
        try
        {

            string sDateFrom = txtStartDate.Text.Trim();
            string sDateTo = txtEndDate.Text.Trim();

            string sConfirmation = "NULL";
            string sDirectorApproved = "NULL";
            string sManageApproved = "NULL";
            string sNonStockReceived = "NULL";

            foreach (ListItem li in cblFilters.Items)
            {
                if (li.Selected)
                {
                    if (li.Value == "Confirmation")
                    {
                        sConfirmation = "1";
                    }
                    if (li.Value == "Director Approved")
                    {
                        sDirectorApproved = "1";
                    }
                    if (li.Value == "Manager Approved")
                    {
                        sManageApproved = "1";
                    }
                    if (li.Value == "Non Stock Received")
                    {
                        sNonStockReceived = "1";
                    }
                }
            }


            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            string sSQL = "";

            if (sDateFrom == "" && sDateTo == "")
            {
                if (chkAllSuppliers.Checked == false)
                {
                    sSQL = "EXEC spGetPurchaseOrdersTracker @FromWatchList=1, @ConfirmFlag=" + sConfirmation + ",@DirectorApproved=" + sDirectorApproved + ",@ManagerApproved=" + sManageApproved + ",@NonStockReceived=" + sNonStockReceived;

                }
                else
                {
                    sSQL = "EXEC spGetPurchaseOrdersTracker @ConfirmFlag=" + sConfirmation + ",@DirectorApproved=" + sDirectorApproved + ",@ManagerApproved=" + sManageApproved + ",@NonStockReceived=" + sNonStockReceived;
                }
            }
            else
            {
                if (chkAllSuppliers.Checked == false)
                {
                    sSQL = "EXEC spGetPurchaseOrdersTracker ";
                    sSQL += " @FromWatchList = 1";
                    sSQL += ",@ConfirmFlag=" + sConfirmation;
                    sSQL += ",@DirectorApproved=" + sDirectorApproved;
                    sSQL += ",@ManagerApproved=" + sManageApproved;
                    sSQL += ",@FromDate='" + sDateFrom + "'";
                    sSQL += ",@ToDate='" + sDateTo + "'";
                    sSQL += ",@NonStockReceived=" + sNonStockReceived;
                }
                else
                {
                    sSQL = "EXEC spGetPurchaseOrdersTracker ";
                    sSQL += "@ConfirmFlag=" + sConfirmation;
                    sSQL += ",@DirectorApproved=" + sDirectorApproved;
                    sSQL += ",@ManagerApproved=" + sManageApproved;
                    sSQL += ",@FromDate='" + sDateFrom + "'";
                    sSQL += ",@ToDate='" + sDateTo + "'";
                    sSQL += ",@NonStockReceived=" + sNonStockReceived;
                }

            }

            Debug.WriteLine(sSQL);
            dt = SharedFunctions.getDataTable(sSQL, conn, "JobHeader");

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            dt.Columns.Add(column);

            //Set values for existing rows...
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["ID"] = i + 1;
            }

            dt.AcceptChanges();


            Session["dtPOs"] = dt;
            lblPageNo.Text = "Current Page #: 1";

        }
        catch (Exception)
        {
            return null;
        }
        finally
        {
            dt.Dispose();
        }
        return dt;
        //TODO: Sub to Populate the Timeline...

    }
    private ReportDocument GetReportNew(DataTable dt)
    {
        try
        {
            try
            {
                ReportDocument report = new ReportDocument();

                if (report != null)
                {
                    report.Close();
                }


                report.Load(Server.MapPath(@"CrystalReports/rptPurchaseOrderReport.rpt"));
                report.FileName = Server.MapPath(@"CrystalReports/rptPurchaseOrderReport.rpt");
                report.SetParameterValue("@PurchaseOrder", lblPurchaseOrderDetailsHidden.Text);


                CrystalDecisions.Shared.ConnectionInfo connectionInfo = new CrystalDecisions.Shared.ConnectionInfo();

                if (Server.MachineName.ToUpper() == "BMW" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
                {
                    connectionInfo.ServerName = "RSPSERVER2";
                    connectionInfo.DatabaseName = "SysproErp1";
                    connectionInfo.UserID = "rsherman";
                    connectionInfo.Password = "toyota1961";
                }
                else
                {
                    ////connectionInfo.ServerName = "SERVERNAME\\SERVER_INSTANCE";
                    ////connectionInfo.DatabaseName = "EnergyPro5";
                    ////connectionInfo.UserID = "EPADMIN";
                    ////connectionInfo.Password = "admin";
                }


                if (dt.Rows.Count > 0)
                {

                    report.SetDataSource(dt);


                    //CrystalDecisions.CrystalReports.Engine.TextObject oPurchaseOrder;
                    //oPurchaseOrder = (CrystalDecisions.CrystalReports.Engine.TextObject)report.ReportDefinition.ReportObjects["txtPurchaseOrder"];
                    //oPurchaseOrder.Text = PurchaseOrder;


                    //CrystalDecisions.CrystalReports.Engine.Database database = report.Database;
                    //Tables tables = database.Tables;

                    //foreach (CrystalDecisions.CrystalReports.Engine.Table table in tables)
                    //{
                    //    CrystalDecisions.Shared.TableLogOnInfo tableLogOnInfo = table.LogOnInfo;
                    //    tableLogOnInfo.ConnectionInfo = connectionInfo;
                    //    table.ApplyLogOnInfo(tableLogOnInfo);
                    //} 


                    CrystalReportViewer1.ReportSource = report;
                    CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                    CrystalReportViewer1.HasToggleGroupTreeButton = false;
                    CrystalReportViewer1.HasToggleParameterPanelButton = false;
                    CrystalReportViewer1.HasPageNavigationButtons = true;
                    CrystalReportViewer1.HasGotoPageButton = true;
                    CrystalReportViewer1.HasCrystalLogo = true;
                    CrystalReportViewer1.DisplayStatusbar = false;
                    CrystalReportViewer1.HasDrillUpButton = false;
                    CrystalReportViewer1.HasDrilldownTabs = false;
                    CrystalReportViewer1.HasSearchButton = false;
                    CrystalReportViewer1.HasZoomFactorList = false;
                    CrystalReportViewer1.HasExportButton = false;
                    CrystalReportViewer1.HasPrintButton = false;
                    CrystalReportViewer1.SeparatePages = false;
                    CrystalReportViewer1.CssClass = "CRViewer";
                    CrystalReportViewer1.Visible = true;
                    CrystalReportViewer1.Height = Unit.Pixel(500);
                    CrystalReportViewer1.DataBind();
                    return report;
                }
                else
                {
                    ////CrystalReportViewer1.Visible = false;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                dt.Dispose();
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return null;
        }
        return null;
    }
    private string[] ParseHelper(String line, int lineRead)
    {
        MemoryStream mem = new MemoryStream(ASCIIEncoding.Default.GetBytes(line));
        TextFieldParser ReaderTemp = new TextFieldParser(mem);
        ReaderTemp.TextFieldType = FieldType.Delimited;
        ReaderTemp.SetDelimiters(new string[] { "\t", "," });
        ReaderTemp.HasFieldsEnclosedInQuotes = true;
        ReaderTemp.TextFieldType = FieldType.Delimited;
        ReaderTemp.TrimWhiteSpace = true;
        try
        {
            return ReaderTemp.ReadFields();
        }
        catch (MalformedLineException ex)
        {
            throw new MalformedLineException(String.Format(
                "Line {0} is not valid and will be skipped: {1}\r\n\r\n{2}",
                lineRead, ReaderTemp.ErrorLine, ex));
        }
    }
    private bool HasShortage(string sSalesOrder, string sStockCode, int iRoleID, int iUserID)
    {
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        if (sStockCode == "")
        {
            sSQL = "EXEC spGetInventoryShortageYN @SalesOrder='" + sSalesOrder + "',";
            sSQL += "@RoleID = " + iRoleID.ToString() + ",";
            sSQL += "@UserID =" + iUserID.ToString();
        }
        else
        {
            sSQL = "EXEC spGetInventoryShortageYN @SalesOrder='" + sSalesOrder + "',@StockCode='" + sStockCode + "',";
            sSQL += "@RoleID = " + iRoleID.ToString() + ",";
            sSQL += "@UserID =" + iUserID.ToString();
        }
        Debug.WriteLine(sSQL);
        dt = SharedFunctions.getDataTable(sSQL, conn, "Shortage");

        if (dt.Rows.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
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
    private DataTable GetDetails(string sSalesOrder, GridView gv)
    {//DETAILS GRID Create Dynamically...
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        try
        {
            sSQL = "spGetPurchaseOrderTrackerDetails @PurchaseOrder='" + sSalesOrder + "'";

            Debug.WriteLine(sSQL);
            dt = SharedFunctions.getDataTable(sSQL, conn, "Details");

            if (dt.Rows.Count > 0)
            {
                gv.DataSource = dt;
                gv.DataBind();
                Session["dtDetails"] = dt;
            }
            else
            {
                gv.DataSource = null;
                gv.DataBind();
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            if (dt != null)
            {
                dt.Dispose();
            }
        }

        return dt;

    }
    private DataTable GetProductionMatrix(string sSalesOrder, string sStockCode)
    {//Production Matrix GRID Create Dynamically...
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        try
        {
            sSQL = "spGetProductionMatrix @SalesOrder ='" + sSalesOrder + "',@StockCode='" + sStockCode + "'";

            Debug.WriteLine(sSQL);
            dt = SharedFunctions.getDataTable(sSQL, conn, "ProductionMatrix");
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            dt.Columns.Add(column);
            //Set values for existing rows...
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["ID"] = i + 1;
            }

            Session["dtProductionMatrix"] = dt;

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            if (dt != null)
            {
                dt.Dispose();
            }
        }

        return dt;

    }
    private bool HasDeliveryRecords(string sSalesOrder)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var qry = (from dm in db.DelMaster
                   where dm.SalesOrder == sSalesOrder
                   select dm);
        if (qry.Count() > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool SalesOrderCommentsExist(string sSalesOrder)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from dm in db.SorComments
                       where dm.SalesOrder == sSalesOrder
                       select dm);
            if (qry.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private string GetSalesOrderComments(string sSalesOrder)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sComments = "";

            var qry = (from dm in db.SorComments
                       where dm.SalesOrder == sSalesOrder
                       select dm);
            foreach (var a in qry)
            {
                sComments = a.Comment;
            }
            return sComments;
        }
    }
    private int GetSalespersonWipUserID(string sSalesPersonID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            int iWipUserID = 0;


            var qry = (from wp in db.WipUsers
                       where wp.Salesperson == sSalesPersonID
                       && wp.Salesperson != null
                       select wp);
            foreach (var a in qry)
            {
                iWipUserID = a.UserID;
            }

            return iWipUserID;
        }

    }
    private int GetWipUserID(string sSalesPersonID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            int iWipUserID = 0;


            var qry = (from wp in db.WipUsers
                       where wp.Salesperson == sSalesPersonID
                       select wp);
            foreach (var a in qry)
            {
                iWipUserID = a.UserID;
            }

            return iWipUserID;
        }

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
            if (Session["SOT_TimerChecked"] != null)
            {
                if ((bool)Session["SOT_TimerChecked"] == true)
                {
                    chkTimerOnOff.Checked = true;
                    Timer1.Enabled = true;
                }
                else
                {
                    chkTimerOnOff.Checked = false;
                    Timer1.Enabled = false;
                }
            }
            else
            {
                chkTimerOnOff.Checked = true;
                Timer1.Enabled = true;
            }
            if (Session["Interval"] != null)
            {
                ddlInterval.SelectedValue = Session["Interval"].ToString();
            }


            Session["dtPOs"] = null;

            DataTable dt = GetOpenPurchaseOrders();

            gvRecord.DataSource = dt;
            gvRecord.DataBind();



            int iDaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            txtStartDate.Text = "";
            txtEndDate.Text = "";


        }
        if (Page.IsPostBack)
        {
            //Use to Maintain jQuery during postbacks...
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isPostBack", "$(document).ready(isPostBack);", true);
        }
    }
    protected void gvRecord_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRecord.PageIndex = e.NewPageIndex;
        lblPageNo.Text = "Current Page #: " + (gvRecord.PageIndex + 1).ToString();
        gvRecord.DataSource = (DataTable)Session["dtPOs"];
        gvRecord.DataBind();
    }
    protected void gvRecord_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            decimal dcTotalValue = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                LinkButton lbnPurchaseOrder = (LinkButton)e.Row.FindControl("lbnPurchaseOrder");
                lbnPurchaseOrder.Text = int.Parse(lbnPurchaseOrder.Text).ToString();


                CheckBox chkConfirmed = (CheckBox)e.Row.FindControl("chkConfirmed");


                CheckBox chkDirectorApproval = (CheckBox)e.Row.FindControl("chkDirectorApproval");


                CheckBox chkManagementApproval = (CheckBox)e.Row.FindControl("chkManagementApproval");


                CheckBox chkNonStockReceived = (CheckBox)e.Row.FindControl("chkNonStockReceived");


                Label lblConfirmer = (Label)e.Row.FindControl("lblConfirmer");
                Label lblApprovalDirector = (Label)e.Row.FindControl("lblApprovalDirector");
                Label lblApprovalManager = (Label)e.Row.FindControl("lblApprovalManager");
                Label lblNonStockReceivedUser = (Label)e.Row.FindControl("lblNonStockReceivedUser");

                if (lblConfirmer.Text != "")
                {
                    chkConfirmed.Checked = true;
                    chkConfirmed.ToolTip = lblConfirmer.Text;
                    chkConfirmed.Attributes.Add("onclick", "if (!confirm('Are you sure you want to Unmark this purchase order as confirmed?')) return false;");
                }
                else
                {
                    chkConfirmed.Checked = false;
                    chkConfirmed.Attributes.Add("onclick", "if (!confirm('Are you sure you want to mark this purchase order as confirmed?')) return false;");
                }
                if (lblApprovalDirector.Text != "")
                {
                    chkDirectorApproval.Checked = true;
                    chkDirectorApproval.ToolTip = lblApprovalDirector.Text;
                    chkDirectorApproval.Attributes.Add("onclick", "if (!confirm('Are you sure you want to Unmark this purchase order as Director Approved?')) return false;");
                }
                else
                {
                    chkDirectorApproval.Checked = false;
                    chkDirectorApproval.Attributes.Add("onclick", "if (!confirm('Are you sure you want to mark this purchase order as Director Approved?')) return false;");
                }
                if (lblApprovalManager.Text != "")
                {
                    chkManagementApproval.Checked = true;
                    chkManagementApproval.ToolTip = lblApprovalManager.Text;
                    chkManagementApproval.Attributes.Add("onclick", "if (!confirm('Are you sure you want to Unmark this purchase order as Management Approved?')) return false;");
                }
                else
                {
                    chkManagementApproval.Checked = false;
                    chkManagementApproval.Attributes.Add("onclick", "if (!confirm('Are you sure you want to mark this purchase order as Management Approved?')) return false;");
                }
                if (lblNonStockReceivedUser.Text != "")
                {
                    chkNonStockReceived.Checked = true;
                    chkNonStockReceived.ToolTip = lblNonStockReceivedUser.Text;
                    chkNonStockReceived.Attributes.Add("onclick", "if (!confirm('Are you sure you want to Unmark this purchase order as Non Stock Received?')) return false;");
                }
                else
                {
                    chkNonStockReceived.Checked = false;
                    chkNonStockReceived.Attributes.Add("onclick", "if (!confirm('Are you sure you want to mark this purchase order as Non Stock Received?')) return false;");
                }

                Label lblTotalValue = (Label)e.Row.FindControl("lblTotalValue");
                if (lblTotalValue.Text != "")
                {
                    dcTotalValue = Convert.ToDecimal(lblTotalValue.Text.Replace("$", ""));
                    dcGrandTotal += dcTotalValue;
                    lblTotalValue.Text = "$" + Convert.ToDecimal(lblTotalValue.Text).ToString("#,0.00");
                }

                Label lblPurchaseOrderDate = (Label)e.Row.FindControl("lblPurchaseOrderDate");
                Label lblOrderDueDate = (Label)e.Row.FindControl("lblOrderDueDate");
                Label lblShipDate = (Label)e.Row.FindControl("lblShipDate");
                Label lblCustomerDeliveryDate = (Label)e.Row.FindControl("lblCustomerDeliveryDate");
                if (lblPurchaseOrderDate.Text != "")
                {
                    lblPurchaseOrderDate.Text = Convert.ToDateTime(lblPurchaseOrderDate.Text).ToShortDateString();
                }
                if (lblOrderDueDate.Text != "")
                {
                    lblOrderDueDate.Text = Convert.ToDateTime(lblOrderDueDate.Text).ToShortDateString();
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblGrandTotal = (Label)e.Row.FindControl("lblGrandTotal");
                lblGrandTotal.Text = "$" + dcGrandTotal.ToString("#,0.00");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvRecord_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblPopUpError.Text = "";
        int idx = 0;
        string sPurchaseOrder = "";
        DataTable dt = new DataTable();
        LinkButton lbnPurchaseOrder;
        Label lblPurchaseOrder;
        Label lblSupplierName;
        Label lblSupplier;
        Label lblOrderDueDate;
        Label lblPurchaseOrderDate;
        Label lblApprovalDirector;
        Label lblApprovalManager;
        switch (e.CommandName)
        {

            case "ViewDetails":
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...
                lblApprovalDirector = (Label)gvRecord.Rows[idx].FindControl("lblApprovalDirector");
                lblApprovalManager = (Label)gvRecord.Rows[idx].FindControl("lblApprovalManager");

                lblPurchaseOrder = (Label)gvRecord.Rows[idx].FindControl("lblPurchaseOrder");
                lbnPurchaseOrder = (LinkButton)gvRecord.Rows[idx].FindControl("lbnPurchaseOrder");

                lblSupplierName = (Label)gvRecord.Rows[idx].FindControl("lblSupplierName");
                lblSupplier = (Label)gvRecord.Rows[idx].FindControl("lblSupplier");
                lblPurchaseOrderDate = (Label)gvRecord.Rows[idx].FindControl("lblPurchaseOrderDate");
                lblOrderDueDate = (Label)gvRecord.Rows[idx].FindControl("lblOrderDueDate");
                sPurchaseOrder = lblPurchaseOrder.Text;
                PurchaseOrder = sPurchaseOrder;//For PDF stuff... 

                lblPurchaseOrderDetails.Text = lbnPurchaseOrder.Text;
                lblPurchaseOrderDetailsHidden.Text = lblPurchaseOrder.Text;
                lblPurchaseOrderDateDetailsHidden.Text = lblPurchaseOrderDate.Text;
                lblDueDatePopup.Text = lblOrderDueDate.Text;
                lblSupplierNameHidden.Text = lblSupplierName.Text;
                lblSupplierNumberHidden.Text = lblSupplier.Text;

                //Rebind gvDash...
                GetDetails(sPurchaseOrder, gvDetails);

                RunPDFPurchaseOrderDetailsReport();
                Timer1.Enabled = false;//Turn Off...

                if (lblApprovalDirector.Text != "" && lblApprovalManager.Text != "")
                {
                    lbnSave.Visible = true;
                }
                else
                {
                    lbnSave.Visible = false;
                }

                ModalPopupExtenderPopUp.Show();
                break;

        }

    }
    //protected void gvRecord_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    foreach (GridViewRow row in gvRecord.Rows)
    //    {
    //        if (row.RowIndex == gvRecord.SelectedIndex)
    //        {
    //            row.BackColor = ColorTranslator.FromHtml("Red");
    //        }
    //        else
    //        {
    //            Label lblShortage = (Label)row.FindControl("lblShortage");
    //            if (lblShortage.Text != "")
    //            {
    //                if (lblShortage.Text == "Y")
    //                {
    //                    row.BackColor = Color.Pink;
    //                    row.ForeColor = Color.Black;
    //                }
    //            }
    //            else
    //            {
    //                row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
    //            }
    //        }


    //    }
    //}
    protected void gvRecord_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtPOs"];
        dtSortTable = dt;
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
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
    }
    //Details Grid...
    protected void gvDetails_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridView gvDetails = (GridView)sender;
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtDetails"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvDetails.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvDetails.DataSource = m_DataView;
            gvDetails.DataBind();
            gvDetails.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
        ModalPopupExtenderPopUp.Show();
    }
    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        decimal dcPrice = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblPurchaseOrder = (Label)e.Row.FindControl("lblPurchaseOrder");
            Label lblPurchaseOrderHidden = (Label)e.Row.FindControl("lblPurchaseOrderHidden");
            HyperLink hlStockCode = (HyperLink)e.Row.FindControl("hlStockCode");
            //Must be last!!!
            string sSource = lblPurchaseOrder.Text;
            var sResult = int.Parse(sSource).ToString();
            lblPurchaseOrder.Text = sResult;
            string sURL = "IngredientCostHistory.aspx?stockcode=" + hlStockCode.Text;
            hlStockCode.ForeColor = Color.Navy;
            hlStockCode.ToolTip = "Go to Ingredient Cost History Report";
            hlStockCode.Attributes.Add("onclick", "javascript: window.open('" + sURL + "', 'window','HEIGHT=800,WIDTH=1200,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
            hlStockCode.Style.Add("Cursor", "pointer");

            Label lblOrderValue = (Label)e.Row.FindControl("lblOrderValue");
            if (lblOrderValue.Text != "")
            {
                dcPrice = Convert.ToDecimal(lblOrderValue.Text.Replace("$", ""));
                dcLineTotal += dcPrice;
                lblOrderValue.Text = "$" + Convert.ToDecimal(lblOrderValue.Text).ToString("#,0.00");
            }
            Label lblFlag = (Label)e.Row.FindControl("lblFlag");
            Label lblPrice = (Label)e.Row.FindControl("lblPrice");
            Label lblLastPrice = (Label)e.Row.FindControl("lblLastPrice");
            Label lblUnitPriceInOrderUom = (Label)e.Row.FindControl("lblUnitPriceInOrderUom");
            if (lblFlag.Text == "0")
            {
                e.Row.BackColor = Color.Pink;
                e.Row.ForeColor = Color.Black;
                e.Row.ToolTip = "Last Price: " + lblLastPrice.Text + " P.O. Price: " + lblPrice.Text;
                lblPurchaseOrder.ToolTip = "Last Price: " + lblLastPrice.Text + " P.O. Price: " + lblPrice.Text;
                lblPurchaseOrder.Font.Bold = true;
                lblUnitPriceInOrderUom.Font.Bold = false;
                lblUnitPriceInOrderUom.ForeColor = Color.Navy;
                lblUnitPriceInOrderUom.Text = "Unit Price:" + lblUnitPriceInOrderUom.Text;
                lblLastPrice.Font.Bold = true;
                lblLastPrice.ForeColor = Color.Red;
                lblLastPrice.Visible = true;
                lblLastPrice.Text = "Last Price: " + lblLastPrice.Text;
                lblPurchaseOrder.Style.Add("cursor", "pointer");
                e.Row.Style.Add("cursor", "pointer");
            }
            else
            {
                e.Row.BackColor = Color.LightGreen;
                e.Row.ForeColor = Color.Black;
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblOrderValueTotal = (Label)e.Row.FindControl("lblOrderValueTotal");
            lblOrderValueTotal.Text = "$" + dcLineTotal.ToString("#,0.00");
        }
    }

    protected void Timer1_Tick(object sender, EventArgs e)
    {
        Timer1.Interval = Convert.ToInt32(ddlInterval.SelectedValue);
        DataTable dt = new DataTable();

        switch (TickNumber)
        {
            case 1:
                dt = GetOpenPurchaseOrders();

                gvRecord.DataSource = dt;
                gvRecord.DataBind();

                Debug.WriteLine("Full Insert: " + TickNumber);
                lblTimeUpdated.Text = "Data Last Updated: " + DateTime.Now.ToString();
                TickNumber = 0;//Reset counter...
                break;
            default:
                Debug.WriteLine("Just Display: " + TickNumber);
                dt = GetOpenPurchaseOrders();

                gvRecord.DataSource = dt;
                gvRecord.DataBind();

                lblTimeUpdated.Text = "Data Last Updated: " + DateTime.Now.ToString();
                break;
        }

        TickNumber++;

        //TODO reset the TickNumber to 0 after a while

        if (TickNumber == 9999)
        {
            TickNumber = 0;
        }
        dt.Dispose();
    }
    protected void chkTimerOnOff_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTimerOnOff.Checked)
        {
            Timer1.Enabled = true;
            Timer1.Interval = Convert.ToInt32(ddlInterval.SelectedValue);
            Session["SOT_TimerChecked"] = true;
        }
        else
        {
            Timer1.Enabled = false;
            Session["SOT_TimerChecked"] = false;
        }
    }
    protected void ddlInterval_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Interval"] = Convert.ToInt32(ddlInterval.SelectedValue);
    }
    protected void btnRunReport_Click(object sender, EventArgs e)
    {
        DataTable dt = GetOpenPurchaseOrders();

        gvRecord.DataSource = dt;
        gvRecord.DataBind();
    }
    protected void lbnExportExcel_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFileName = "";
        if (Session["dtPOs"] != null)
        {
            dt = (DataTable)Session["dtPOs"];
        }
        else
        {
            dt = GetOpenPurchaseOrders();
        }

        dt.TableName = "dtPOs";
        sFileName = "PurchaseOrderTrackerReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        if (dt.Rows.Count > 0)
        {
            ExcelHelper.ToExcel(dt, sFileName, Page.Response);
        }

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void btnClose_Click(object sender, EventArgs e)
    {//Reset checkbox...
        lblPopUpError.Text = "";
        if (chkTimerOnOff.Checked)
        {
            Timer1.Enabled = true;
            Timer1.Interval = Convert.ToInt32(ddlInterval.SelectedValue);
            Session["SOT_TimerChecked"] = true;
        }
        else
        {
            Timer1.Enabled = false;
            Session["SOT_TimerChecked"] = false;
        }
        ModalPopupExtenderPopUp.Hide();
    }
    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        DateTime dtMondayOfLastWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek - 6);
        dtMondayOfLastWeek = Convert.ToDateTime(dtMondayOfLastWeek.ToShortDateString());
        DateTime dtSundayOfLastWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
        dtSundayOfLastWeek = Convert.ToDateTime(dtSundayOfLastWeek.ToShortDateString() + " 23:59:59");

        DateTime dtMondayOfThisWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
        dtMondayOfThisWeek = Convert.ToDateTime(dtMondayOfThisWeek.ToShortDateString());
        DateTime dtSundayOfThisWeek = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 7);
        dtSundayOfThisWeek = Convert.ToDateTime(dtSundayOfThisWeek.ToShortDateString() + " 23:59:59");

        DateTime dtFirstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        DateTime dtLastDayOfMonth = Convert.ToDateTime(dtFirstDayOfMonth.AddMonths(1).AddDays(-1).ToShortDateString() + " 23:59:59");

        DateTime dtFirstDayOfLastMonth = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
        DateTime dtLastDayOfLastMonth = Convert.ToDateTime(dtFirstDayOfLastMonth.AddMonths(1).AddDays(-1).ToShortDateString() + " 23:59:59");

        DateTime dtFirstDayOfyear = new DateTime(DateTime.Now.Year, 1, 1);

        DateTime dtSevenDaysAgo = Convert.ToDateTime(DateTime.Now.AddDays(-7).ToShortDateString() + " 00:00:00");


        switch (ddlPeriod.SelectedIndex)
        {
            case 0://All
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                break;
            case 1://Range
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                break;
            case 2://Current Week
                txtStartDate.Text = dtMondayOfThisWeek.ToShortDateString();
                txtEndDate.Text = dtSundayOfThisWeek.ToShortDateString();
                break;
            case 3://Previous Week
                txtStartDate.Text = dtMondayOfLastWeek.ToShortDateString();
                txtEndDate.Text = dtSundayOfLastWeek.ToShortDateString();
                break;
        }

        if (txtStartDate.Text.Trim() != "" && txtEndDate.Text.Trim() != "")
        {

        }
    }
    protected void cblFilters_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = GetOpenPurchaseOrders();

        gvRecord.DataSource = dt;
        gvRecord.DataBind();

        dt.Dispose();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        cblFilters.SelectedIndex = -1;
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        DataTable dt = GetOpenPurchaseOrders();

        gvRecord.DataSource = dt;
        gvRecord.DataBind();

        dt.Dispose();
    }
    protected void chkConfirmed_CheckedChanged(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        CheckBox chkConfirmed = (CheckBox)sender;
        GridViewRow gvr = (GridViewRow)chkConfirmed.Parent.Parent;
        Label lblPurchaseOrder = (Label)gvr.FindControl("lblPurchaseOrder");
        string sPurchaseOrder = lblPurchaseOrder.Text;
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            if (chkConfirmed.Checked)
            {
                PorMasterHdr ca = db.PorMasterHdr.Single(p => p.PurchaseOrder == sPurchaseOrder);
                ca.ConfirmationUserID = iUserID;
                db.SubmitChanges();
            }
            else
            {
                PorMasterHdr ca = db.PorMasterHdr.Single(p => p.PurchaseOrder == sPurchaseOrder);
                ca.ConfirmationUserID = null;
                db.SubmitChanges();
            }
            DataTable dt = GetOpenPurchaseOrders();

            gvRecord.DataSource = dt;
            gvRecord.DataBind();
            dt.Dispose();
        }
    }
    protected void chkDirectorApproval_CheckedChanged(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        CheckBox chkDirectorApproval = (CheckBox)sender;
        GridViewRow gvr = (GridViewRow)chkDirectorApproval.Parent.Parent;
        Label lblPurchaseOrder = (Label)gvr.FindControl("lblPurchaseOrder");
        string sPurchaseOrder = lblPurchaseOrder.Text;
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            if (chkDirectorApproval.Checked)
            {
                PorMasterHdr ca = db.PorMasterHdr.Single(p => p.PurchaseOrder == sPurchaseOrder);
                ca.DirectorApprovalUserID = iUserID;
                db.SubmitChanges();
            }
            else
            {
                PorMasterHdr ca = db.PorMasterHdr.Single(p => p.PurchaseOrder == sPurchaseOrder);
                ca.DirectorApprovalUserID = null;
                db.SubmitChanges();
            }
            DataTable dt = GetOpenPurchaseOrders();

            gvRecord.DataSource = dt;
            gvRecord.DataBind();
            dt.Dispose();
        }
    }
    protected void chkManagementApproval_CheckedChanged(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        CheckBox chkManagementApproval = (CheckBox)sender;
        GridViewRow gvr = (GridViewRow)chkManagementApproval.Parent.Parent;
        Label lblPurchaseOrder = (Label)gvr.FindControl("lblPurchaseOrder");
        string sPurchaseOrder = lblPurchaseOrder.Text;
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            if (chkManagementApproval.Checked)
            {
                PorMasterHdr ca = db.PorMasterHdr.Single(p => p.PurchaseOrder == sPurchaseOrder);
                ca.ManagementApprovalUserID = iUserID;
                db.SubmitChanges();
            }
            else
            {
                PorMasterHdr ca = db.PorMasterHdr.Single(p => p.PurchaseOrder == sPurchaseOrder);
                ca.ManagementApprovalUserID = null;
                db.SubmitChanges();
            }
            DataTable dt = GetOpenPurchaseOrders();

            gvRecord.DataSource = dt;
            gvRecord.DataBind();
            dt.Dispose();
        }
    }
    protected void chkNonStockReceived_CheckedChanged(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        CheckBox chkNonStockReceived = (CheckBox)sender;
        GridViewRow gvr = (GridViewRow)chkNonStockReceived.Parent.Parent;
        Label lblPurchaseOrder = (Label)gvr.FindControl("lblPurchaseOrder");
        string sPurchaseOrder = lblPurchaseOrder.Text;
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            if (chkNonStockReceived.Checked)
            {
                PorMasterHdr ca = db.PorMasterHdr.Single(p => p.PurchaseOrder == sPurchaseOrder);
                ca.NonStockReceived = iUserID;
                db.SubmitChanges();
            }
            else
            {
                PorMasterHdr ca = db.PorMasterHdr.Single(p => p.PurchaseOrder == sPurchaseOrder);
                ca.NonStockReceived = null;
                db.SubmitChanges();
            }
            DataTable dt = GetOpenPurchaseOrders();

            gvRecord.DataSource = dt;
            gvRecord.DataBind();
            dt.Dispose();
        }
    }
    protected void imgbtnExcel_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";
        ExportPDF("Excel");
    }
    protected void imgbtnPDF_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";
        ExportPDF("PDF");
    }
    protected void imgbtnWord_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";
        ExportPDF("Word");
    }
    protected void lbnSave_Click(object sender, EventArgs e)
    {

        try
        {
            RunPDFPurchaseOrderDetailsReport();

            DataTable dt = (DataTable)Session["dt"];

            string sFileName = lblSupplierNameHidden.Text.Replace(" ", "_") + "_" + lblSupplierNumberHidden.Text + "_" + int.Parse(lblPurchaseOrderDetailsHidden.Text).ToString() + "_" + lblPurchaseOrderDateDetailsHidden.Text.Replace("/", "_");
            sFileName = sFileName.Replace(".", "").Replace(",", "") + ".pdf";
            string sPath = "~/Images/Docs/ApprovedPOs/";

            string sFullPath = MapPath(sPath) + sFileName;

            ReportDocument report = new ReportDocument();

            report.Load(Server.MapPath(@"CrystalReports/rptPurchaseOrderReport.rpt"));
            report.FileName = Server.MapPath(@"CrystalReports/rptPurchaseOrderReport.rpt");
            report.SetParameterValue("@PurchaseOrder", lblPurchaseOrderDetailsHidden.Text);


            CrystalDecisions.Shared.ConnectionInfo connectionInfo = new CrystalDecisions.Shared.ConnectionInfo();

            if (Server.MachineName.ToUpper() == "BMW" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
            {
                connectionInfo.ServerName = "RSPSERVER2";
                connectionInfo.DatabaseName = "SysproErp1";
                connectionInfo.UserID = "rsherman";
                connectionInfo.Password = "toyota1961";
            }
            else
            {
                ////connectionInfo.ServerName = "SERVERNAME\\SERVER_INSTANCE";
                ////connectionInfo.DatabaseName = "EnergyPro5";
                ////connectionInfo.UserID = "EPADMIN";
                ////connectionInfo.Password = "admin";
            }
            if (dt.Rows.Count == 0)
            {
                return;
            }

            report.SetDataSource(dt);

            var exportOptions = report.ExportOptions;
            exportOptions.ExportDestinationType = ExportDestinationType.NoDestination;
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            var req = new ExportRequestContext { ExportInfo = exportOptions };
            var stream = report.FormatEngine.ExportToStream(req);

            using (var fileStream = new FileStream(sFullPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
            lblPopUpError.Text = "**PDF Successfully created!";
            lblPopUpError.ForeColor = Color.Green;
        }
        catch (Exception ex)
        {
            lblPopUpError.Text = "**PDF creation failed!";
            lblPopUpError.ForeColor = Color.Red;
            Debug.WriteLine(ex.ToString());
        }
        ModalPopupExtenderPopUp.Show();

    }
    protected void chkAllSuppliers_CheckedChanged(object sender, EventArgs e)
    {
        DataTable dt = GetOpenPurchaseOrders();

        gvRecord.DataSource = dt;
        gvRecord.DataBind();
    }
    //FrozenHeaderRow...
    // LinkButtons are used to dynamically create the links necessary
    // for paging.
    protected void HeaderLink_Click(object sender, System.EventArgs e)
    {
        LinkButton lnkHeader = (LinkButton)sender;
        System.Web.UI.WebControls.SortDirection direction = System.Web.UI.WebControls.SortDirection.Ascending;

        // the CommandArgument of each linkbutton contains the sortexpression
        // for the column that was clicked.
        if (gvRecord.SortExpression == lnkHeader.CommandArgument)
        {
            if (gvRecord.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending)
            {
                direction = System.Web.UI.WebControls.SortDirection.Descending;
            }

        }

        gvRecord.Sort(lnkHeader.CommandArgument, direction);
    }

    ////create the table for the fixed header in Init
    protected void Page_Init(object sender, EventArgs e)
    {
        TableRow headerRow = new TableRow();

        for (int x = 0; x < gvRecord.Columns.Count; x++)
        {
            DataControlField col = gvRecord.Columns[x];

            TableCell headerCell = new TableCell();
            headerCell.BorderStyle = BorderStyle.Solid;
            headerCell.BorderWidth = 0;//Hide vertical grid lines on header...
            headerCell.Font.Bold = true;
            headerCell.HorizontalAlign = HorizontalAlign.Center;

            // if the column has a SortExpression, we want to allow
            // sorting by that column. Therefore, we create a linkbutton
            // on those columns.
            if (col.SortExpression != "")
            {
                LinkButton lnkHeader = new LinkButton();

                // *** Comment the line below if using with AJAX
                //// lnkHeader.PostBackUrl = HttpContext.Current.Request.Url.LocalPath;

                lnkHeader.CommandArgument = col.SortExpression;
                lnkHeader.ForeColor = System.Drawing.Color.White;
                lnkHeader.Text = col.HeaderText.Replace(" ", "<br/>");

                // *** Uncomment this line for AJAX 
                lnkHeader.ID = "Sort" + col.HeaderText;

                lnkHeader.Click += new EventHandler(HeaderLink_Click);
                headerCell.Controls.Add(lnkHeader);
            }
            else
            {
                headerCell.Text = col.HeaderText;
            }
            //We need to set the width of the column header customly...
            switch (x)
            {
                case 0://#
                    headerCell.Width = Unit.Pixel(35);
                    break;
                case 1://Select/PO#
                    headerCell.Width = Unit.Pixel(45);
                    break;
                case 2://Supplier#
                    headerCell.Width = Unit.Pixel(50);
                    break;
                case 3://Supplier Name
                    headerCell.Width = Unit.Pixel(340);
                    break;
                case 4://Memo Date
                    headerCell.Width = Unit.Pixel(75);
                    break;
                case 5://Buyer
                    headerCell.Width = Unit.Pixel(100);
                    break;
                case 6://PO Date
                    headerCell.Width = Unit.Pixel(75);
                    break;
                case 7://PO Date
                    headerCell.Width = Unit.Pixel(75);
                    break;
                case 8://Total Value
                    headerCell.Width = Unit.Pixel(75);
                    break;
                case 9://Confirmation
                    headerCell.Width = Unit.Pixel(50);
                    break;
                case 10://Confirmation
                    headerCell.Width = Unit.Pixel(250);
                    break;
                case 11://Director Approval
                    headerCell.Width = Unit.Pixel(50);
                    headerCell.Visible = false;
                    break;
                case 12://Management Approval
                    headerCell.Width = Unit.Pixel(50);
                    headerCell.Visible = false;
                    break;
                case 13://Non Stock Received
                    headerCell.Width = Unit.Pixel(50);
                    headerCell.Visible = false;
                    break;

            }


            //headerCell.Width = col.ItemStyle.Width;
            headerRow.Cells.Add(headerCell);

        }

        HeaderTable.Rows.Add(headerRow);
    }

    #endregion





}