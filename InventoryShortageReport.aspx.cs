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

public partial class InventoryShortageReport : System.Web.UI.Page
{

    decimal dcShortageTotal = 0;
    decimal dcQtyTotal = 0;
    decimal dcShortageTotalSummary = 0;
    List<ProductionQuantities> lProductionQuantityAvailable = new List<ProductionQuantities>();

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
    private void LoadInventoryShortageReport(int iRoleID, int iUserID)
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
        else//No Start Date(From Date)...
        {
            switch (ddlPeriod.SelectedIndex)
            {
                case 0://Range
                    sMsg += "**Start Date is not a valid date!<br/>";
                    break;
                case 1://Single
                    sMsg += "**Start Date is not a valid date!<br/>";
                    break;
                case 2://Up To Date
                       //None...
                    break;
                case 3://ALL
                    //None...
                    break;

            }
        }
        //End Date...
        if (sEndDate != "NULL")
        {
            if (SharedFunctions.IsDate(sEndDate.Replace("'", "")) == false)
            {
                sMsg += "**End Date is not a valid date!<br/>";
            }
        }
        else//No End Date(To Date)...
        {
            switch (ddlPeriod.SelectedIndex)
            {
                case 0://Range
                    sMsg += "**End Date is not a valid date!<br/>";
                    break;
                case 1://Single
                       //None...
                    break;
                case 2://Up To Date
                    sMsg += "**End Date is not a valid date!<br/>";
                    break;
                case 3://ALL
                    //None...
                    break;
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


        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }
        string sReportType = "";

        sReportType = "SHORTAGE";


        switch (ddlPeriod.SelectedIndex)
        {

            case 0://Date Range...
                sSQL = "EXEC spGetInventoryShortageReportByRange ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
                sSQL += "@RoleID = " + iRoleID.ToString() + ",";
                sSQL += "@UserID =" + iUserID.ToString() + ",";
                sSQL += "@ReportType='" + sReportType + "'";
                break;
            case 1://Single Date...
                sSQL = "EXEC spGetInventoryShortageReportByRange ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
                sSQL += "@RoleID = " + iRoleID.ToString() + ",";
                sSQL += "@UserID =" + iUserID.ToString() + ",";
                sSQL += "@ReportType='" + sReportType + "'";
                break;
            case 2://Up To Date...
                sSQL = "EXEC spGetInventoryShortageReportByRange ";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
                sSQL += "@RoleID = " + iRoleID.ToString() + ",";
                sSQL += "@UserID =" + iUserID.ToString() + ",";
                sSQL += "@ReportType='" + sReportType + "'";
                break;
            case 3://ALL
                sSQL = "EXEC spGetInventoryShortageReportByRange ";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
                sSQL += "@RoleID = " + iRoleID.ToString() + ",";
                sSQL += "@UserID =" + iUserID.ToString() + ",";
                sSQL += "@ReportType='" + sReportType + "'";
                break;
        }




        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
        DataView dv = new DataView(dt);
        dv.Sort = "MStockCode ASC, MLineShipDate ASC";//Very Important for Color coding to be accurate...
        dt = dv.ToTable();

        DataColumn column = new DataColumn();
        column.DataType = System.Type.GetType("System.Int32");
        column.ColumnName = "ID";
        dt.Columns.Add(column);
        if (dt.Rows.Count > 0)
        {            //Set values for existing rows...
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["ID"] = i + 1;
            }
            Session["dtInventoryShortageReport"] = dt;
            gvInventoryShortageReport.DataSource = dt;
            gvInventoryShortageReport.DataBind();
            lblRecordCount.Text = "Record Count:  " + dt.Rows.Count.ToString();
            // pnlGridView.Visible = true;
            //tblHeaderTable.Visible = true;
        }
        else
        {
            lblError.Text = "No results found!!";
            lblError.ForeColor = Color.Red;
            // pnlGridView.Visible = false;
            // tblHeaderTable.Visible = false;

            gvInventoryShortageReport.DataSource = null;
            gvInventoryShortageReport.DataBind();
        }

        //if (dt.Rows.Count > 9)
        //{
        //    pnlGridView.ScrollBars = ScrollBars.Vertical;
        //    pnlGridView.Width = Unit.Pixel(1320);
        //    tblHeaderTable.Width = Unit.Pixel(1320);
        //}
        //else
        //{
        //    pnlGridView.ScrollBars = ScrollBars.None;
        //    pnlGridView.Width = Unit.Pixel(1100);
        //    tblHeaderTable.Width = Unit.Pixel(1324);
        //}



    }
    private void LoadInventoryShortageReportSummary(int iRoleID, int iUserID)
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
        else//No Start Date(From Date)...
        {
            switch (ddlPeriod.SelectedIndex)
            {
                case 0://Range
                    sMsg += "**Start Date is not a valid date!<br/>";
                    break;
                case 1://Single
                    sMsg += "**Start Date is not a valid date!<br/>";
                    break;
                case 2://Up To Date
                       //None...
                    break;
                case 3://ALL
                    //None...
                    break;

            }
        }
        //End Date...
        if (sEndDate != "NULL")
        {
            if (SharedFunctions.IsDate(sEndDate.Replace("'", "")) == false)
            {
                sMsg += "**End Date is not a valid date!<br/>";
            }
        }
        else//No End Date(To Date)...
        {
            switch (ddlPeriod.SelectedIndex)
            {
                case 0://Range
                    sMsg += "**End Date is not a valid date!<br/>";
                    break;
                case 1://Single
                       //None...
                    break;
                case 2://Up To Date
                    sMsg += "**End Date is not a valid date!<br/>";
                    break;
                case 3://ALL
                    //None...
                    break;
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


        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }

        switch (ddlPeriod.SelectedIndex)
        {

            case 0://Date Range...
                sSQL = "EXEC spGetInventoryShortageReportByRangeSummary ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@RoleID = " + iRoleID.ToString() + ",";
                sSQL += "@UserID =" + iUserID.ToString() + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo;

                break;
            case 1://Single Date...
                sSQL = "EXEC spGetInventoryShortageReportByRangeSummary ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@RoleID = " + iRoleID.ToString() + ",";
                sSQL += "@UserID =" + iUserID.ToString() + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo;
                break;
            case 2://Up To Date...
                sSQL = "EXEC spGetInventoryShortageReportByRangeSummary ";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@RoleID = " + iRoleID.ToString() + ",";
                sSQL += "@UserID =" + iUserID.ToString() + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo;
                break;
            case 3://ALL
                sSQL = "EXEC spGetInventoryShortageReportByRangeSummary ";
                sSQL += "@RoleID = " + iRoleID.ToString() + ",";
                sSQL += "@UserID =" + iUserID.ToString() + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo;
                break;
        }


        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");


        if (dt.Rows.Count > 0)
        {
            Session["dtInventoryShortageReportSummary"] = dt;
            gvInventoryShortageReportSummary.DataSource = dt;
            gvInventoryShortageReportSummary.DataBind();
            lblRecordCount0.Text = "Record Count:  " + dt.Rows.Count.ToString();
            // pnlGridView.Visible = true;
            //tblHeaderTable.Visible = true;
        }
        else
        {
            lblError.Text = "No results found!!";
            lblError.ForeColor = Color.Red;
            // pnlGridView.Visible = false;
            // tblHeaderTable.Visible = false;

            gvInventoryShortageReportSummary.DataSource = null;
            gvInventoryShortageReportSummary.DataBind();
        }

    }
    private void SetUpNotInEditMode(object sender, GridViewRowEventArgs e)
    {
        decimal dcShortage = 0;
        decimal dcQty = 0;

        try
        {


            Label lblQty = (Label)e.Row.FindControl("lblQty");
            Label lblStockCode = (Label)e.Row.FindControl("lblStockCode");
            Label lblShipDate = (Label)e.Row.FindControl("lblShipDate");
            //Label lblScheduledDate = (Label)e.Row.FindControl("lblScheduledDate");
            //Label lblQty = (Label)e.Row.FindControl("lblQty");
            Label lblShortage = (Label)e.Row.FindControl("lblShortage");
            if (lblShipDate.Text != "")
            {
                lblShipDate.Text = Convert.ToDateTime(lblShipDate.Text).ToShortDateString();
            }
            Label lblOrderDate = (Label)e.Row.FindControl("lblOrderDate");
            if (lblOrderDate.Text != "")
            {
                lblOrderDate.Text = Convert.ToDateTime(lblOrderDate.Text).ToShortDateString();
            }
            //if (lblScheduledDate.Text != "")
            //{
            //    lblScheduledDate.Text = Convert.ToDateTime(lblScheduledDate.Text).ToShortDateString();
            //}
            //if (lblQty.Text != "")
            //{
            //    dcQty = Convert.ToDecimal(lblQty.Text);
            //    dcQtyTotal += dcQty;
            //    lblQty.Text = Convert.ToDecimal(lblQty.Text.Trim()).ToString("#,0.00");
            //}

            Label lblSalesOrder = (Label)e.Row.FindControl("lblSalesOrder");
            Label lblSalesOrderNotes = (Label)e.Row.FindControl("lblSalesOrderNotes");
            Panel pnlNotes0 = (Panel)e.Row.FindControl("pnlNotes0");
            if (lblSalesOrderNotes.Text != "")
            {
                pnlNotes0.Visible = true;
                lblSalesOrder.Style.Add("cursor", "pointer");
            }
            else
            {
                pnlNotes0.Visible = false;
            }
            string sSource = lblSalesOrder.Text;
            var sResult = int.Parse(sSource).ToString();
            lblSalesOrder.Text = sResult;

            if (lblShortage.Text != "")
            {
                dcShortage = Convert.ToDecimal(lblShortage.Text);
                dcShortageTotal += dcShortage;
                lblShortage.Text = Convert.ToDecimal(lblShortage.Text.Trim()).ToString("#,0.00");
            }
            if (lblQty.Text != "")
            {
                dcQty = Convert.ToDecimal(lblQty.Text);
                dcQtyTotal += dcQty;
                lblQty.Text = Convert.ToDecimal(lblQty.Text.Trim()).ToString("#,0.00");
            }
            if (lblStockCode.Text.Trim() != null)
            {
                GridView gvProductionSchedule = (GridView)e.Row.FindControl("gvProductionSchedule");
                DataTable dtProductionSchedule = new DataTable();
                dtProductionSchedule = SharedFunctions.GetProductionScheduleUnGrouped(lblStockCode.Text.Trim());
                gvProductionSchedule.DataSource = dtProductionSchedule;
                gvProductionSchedule.DataBind();
                dtProductionSchedule.Dispose();

                System.Web.UI.WebControls.Image imgNotes = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgNotes");
                Panel pnlNotes = (Panel)e.Row.FindControl("pnlNotes");

                if (dtProductionSchedule.Rows.Count != 0)
                {//Has a Production Schedule...
                    pnlNotes.Visible = true;
                    imgNotes.Visible = true;
                    imgNotes.Style.Add("cursor", "pointer");

                    decimal dcNewdcProductionQty = 0;
                    decimal dcProductionQty = 0;
                    dcProductionQty = Convert.ToDecimal(dtProductionSchedule.Rows[0]["ScheduledQty"]);//Most Recent Scheduled date...
                    DateTime dtProductionDate = Convert.ToDateTime(dtProductionSchedule.Rows[0]["ScheduledDate"]);
                    string sStockCode = lblStockCode.Text.Trim();
                    lProductionQuantityAvailable = lProductionQuantityAvailable.Where(p => p.StockCode.Trim() == sStockCode).ToList<ProductionQuantities>();

                    //Keep Running total of how much quantity we have in production that we are assigning out..
                    if (lProductionQuantityAvailable.Count() == 0)//Stock Code Does not Exists in our list...                             
                    {//Add...
                        ProductionQuantities pq = new ProductionQuantities();
                        pq.StockCode = sStockCode;
                        pq.ProductionQuantityAvailables = dcProductionQty;
                        lProductionQuantityAvailable.Add(pq);
                        dcNewdcProductionQty = dcProductionQty;
                    }
                    else
                    {
                        foreach (var t in lProductionQuantityAvailable)
                        {//Should only be one row of data per stockcode...
                            dcNewdcProductionQty = t.ProductionQuantityAvailables;//Get Adjusted Value...
                        }
                    }
                    bool bIsTBD = false;
                    bIsTBD = SharedFunctions.IsOrderStatusTBD(Convert.ToInt32(lblSalesOrder.Text), sStockCode);
                    if (dcNewdcProductionQty >= dcShortage)//dcShortage was set above...
                    {//We have enough to cover shortage...
                        if (dcShortage > 0)
                        {
                            DateTime dtShippingDate = Convert.ToDateTime(lblShipDate.Text);

                            if ((dtShippingDate > dtProductionDate.AddDays(3) || dtProductionDate.AddDays(3) > DateTime.Now))
                            {
                                lblStockCode.ForeColor = Color.Red;
                                lblStockCode.ToolTip = "SHORTAGE COVERABLE";
                                lblStockCode.Style.Add("cursor", "pointer");
                                e.Row.BackColor = Color.LemonChiffon;
                                e.Row.ForeColor = Color.Black;
                            }
                            else
                            {
                                lblStockCode.ForeColor = Color.Red;
                                lblStockCode.ToolTip = "SHORTAGE";
                                lblStockCode.Style.Add("cursor", "pointer");
                                e.Row.BackColor = Color.Pink;
                                e.Row.ForeColor = Color.Black;
                            }
                        }
                    }
                    else
                    {
                        lblStockCode.ForeColor = Color.Red;
                        lblStockCode.ToolTip = "SHORTAGE";
                        lblStockCode.Style.Add("cursor", "pointer");
                        e.Row.BackColor = Color.Pink;
                        e.Row.ForeColor = Color.Black;
                    }
                    if (lProductionQuantityAvailable.Count() > 0)//Stock Code Exists in our list...
                    {//First Time - Remove the Qty of the first Occurance...
                        foreach (var t in lProductionQuantityAvailable)
                        {//Should only be one row of data per stockcode...
                            t.ProductionQuantityAvailables = dcNewdcProductionQty - dcShortage;//Reduce the Qty for the next check...
                        }
                    }

                }
                else//No Production Schedule...
                {
                    lblStockCode.ForeColor = Color.Red;
                    lblStockCode.ToolTip = "SHORTAGE";
                    lblStockCode.Style.Add("cursor", "pointer");
                    e.Row.BackColor = Color.Pink;
                    e.Row.ForeColor = Color.Black;

                    pnlNotes.Visible = false;
                    imgNotes.Visible = false;
                }


                System.Web.UI.WebControls.Image imgNotes2 = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgNotes2");
                Panel pnlNotes2 = (Panel)e.Row.FindControl("pnlNotes2");
                Label lblNotes = (Label)e.Row.FindControl("lblNotes");
                if (lblNotes.Text != "")
                {
                    pnlNotes2.Visible = true;
                    imgNotes2.Visible = true;
                    imgNotes2.Style.Add("cursor", "pointer");
                }
                else
                {
                    pnlNotes2.Visible = false;
                    imgNotes2.Visible = false;
                }
            }

            Label lblCustomerDeliveryDate = (Label)e.Row.FindControl("lblCustomerDeliveryDate");
            if (lblCustomerDeliveryDate.Text != "")
            {
                lblCustomerDeliveryDate.Text = Convert.ToDateTime(lblCustomerDeliveryDate.Text).ToShortDateString();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
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
            Session["dtInventoryShortageReport"] = null;
            Session["dtInventoryShortageReportSummary"] = null;
            ddlPeriod.SelectedIndex = 2;
            ddlPeriod_SelectedIndexChanged(ddlPeriod, null);
            txtEndDate.Text = DateTime.Now.ToShortDateString();
        }

    }
    protected void btnRunReport_Click(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
        lblError.Text = "";
        lblRecordCount.Text = "";
        if (chkRunBoth.Checked)
        {
            LoadInventoryShortageReport(iRoleID, iUserID);
            LoadInventoryShortageReportSummary(iRoleID, iUserID);
        }
        else
        {
            LoadInventoryShortageReport(iRoleID, iUserID);
        }

    }
    protected void gvInventoryShortageReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            int iUserID = Convert.ToInt32(Session["UserID"]);
            int i = 0;
            string sStockCode = "";
            string sSalesOrder = "";
            string sNotes = "";
            TextBox txtNotes;
            Label lblSalesOrder;
            Label lblStockCode;
            CheckBox chkDuplicate;
            switch (e.CommandName)
            {
                case "Update":
                    i = Convert.ToInt32(e.CommandArgument);
                    chkDuplicate = (CheckBox)gvInventoryShortageReport.Rows[i].FindControl("chkDuplicate");
                    txtNotes = (TextBox)gvInventoryShortageReport.Rows[i].FindControl("txtNotes");
                    lblSalesOrder = (Label)gvInventoryShortageReport.Rows[i].FindControl("lblSalesOrder");
                    lblStockCode = (Label)gvInventoryShortageReport.Rows[i].FindControl("lblStockCode");
                    sStockCode = lblStockCode.Text.Trim();
                    sSalesOrder = lblSalesOrder.Text;
                    sNotes = txtNotes.Text.Trim();

                    if (chkDuplicate.Checked)
                    {

                        var qry1 = (from sn in db.SorCommentsForStockCodes
                                    where sn.StockCode.Trim() == sStockCode
                                    select sn);

                        if (qry1.Count() > 0)
                        {//Update..
                            var qrySC = (from sc in db.SorCommentsForStockCodes where sc.StockCode == sStockCode select sc);
                            foreach (var a in qrySC)
                            {
                                SorCommentsForStockCodes scs = db.SorCommentsForStockCodes.Single(p => p.SOCommentID == a.SOCommentID);
                                if (sNotes != "")
                                {
                                    scs.Comment = sNotes;
                                }
                                else
                                {
                                    scs.Comment = null;
                                }
                                db.SubmitChanges();
                            }
                        }
                        else
                        {//Add...
                            if (sNotes != "")
                            {
                                var qry2 = (from so in db.SorMaster
                                            join sd in db.SorDetail on so.SalesOrder equals sd.SalesOrder
                                            where sd.MStockCode.Trim() == sStockCode
                                            select so);
                                foreach (var a in qry2)
                                {
                                    SorCommentsForStockCodes scs = new SorCommentsForStockCodes();
                                    scs.AddedBy = iUserID;
                                    scs.DateAdded = DateTime.Now;
                                    scs.SalesOrder = a.SalesOrder;
                                    scs.StockCode = sStockCode;
                                    scs.Comment = sNotes;
                                    db.SorCommentsForStockCodes.InsertOnSubmit(scs);
                                    db.SubmitChanges();
                                }
                            }
                        }
                    }
                    else//Do not duplicate...
                    {
                        var qry = (from sn in db.SorCommentsForStockCodes
                                   where sn.StockCode.Trim() == sStockCode
                                   && sn.SalesOrder == sSalesOrder
                                   select sn);

                        if (qry.Count() > 0)
                        {//Update...
                            SorCommentsForStockCodes scs = db.SorCommentsForStockCodes.Single(p => p.StockCode == sStockCode && p.SalesOrder == sSalesOrder);
                            if (sNotes != "")
                            {
                                scs.Comment = sNotes;
                            }
                            else
                            {
                                scs.Comment = null;
                            }
                            db.SubmitChanges();
                        }
                        else//Add...
                        {
                            if (sNotes != "")
                            {
                                SorCommentsForStockCodes scs = new SorCommentsForStockCodes();
                                scs.AddedBy = iUserID;
                                scs.DateAdded = DateTime.Now;
                                scs.SalesOrder = sSalesOrder;
                                scs.StockCode = sStockCode;
                                scs.Comment = sNotes;
                                db.SorCommentsForStockCodes.InsertOnSubmit(scs);
                                db.SubmitChanges();
                            }
                        }
                    }

                    break;

            }

        }
    }
    protected void gvInventoryShortageReport_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvInventoryShortageReport.EditIndex = -1;
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
        LoadInventoryShortageReport(iRoleID, iUserID);
    }
    protected void gvInventoryShortageReport_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvInventoryShortageReport.EditIndex = e.NewEditIndex;
        gvInventoryShortageReport.DataSource = (DataTable)Session["dtInventoryShortageReport"];
        gvInventoryShortageReport.DataBind();
    }
    protected void gvInventoryShortageReport_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvInventoryShortageReport.EditIndex = -1;
        gvInventoryShortageReport.DataSource = (DataTable)Session["dtInventoryShortageReport"];
        gvInventoryShortageReport.DataBind();
    }
    protected void gvInventoryShortageReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (gvInventoryShortageReport.EditIndex != -1)//In Edit Mode...
            {
                if (gvInventoryShortageReport.EditIndex == e.Row.RowIndex)//edited row...
                {
                    Label lblShipDate = (Label)e.Row.FindControl("lblShipDate");
                    if (lblShipDate.Text != "")
                    {
                        lblShipDate.Text = Convert.ToDateTime(lblShipDate.Text).ToShortDateString();
                    }
                    Label lblCustomerDeliveryDate = (Label)e.Row.FindControl("lblCustomerDeliveryDate");
                    if (lblCustomerDeliveryDate.Text != "")
                    {
                        lblCustomerDeliveryDate.Text = Convert.ToDateTime(lblCustomerDeliveryDate.Text).ToShortDateString();
                    }
                    Label lblOrderDate = (Label)e.Row.FindControl("lblOrderDate");
                    if (lblOrderDate.Text != "")
                    {
                        lblOrderDate.Text = Convert.ToDateTime(lblOrderDate.Text).ToShortDateString();
                    }
                }
                else if (gvInventoryShortageReport.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
                {
                    SetUpNotInEditMode(sender, e);
                }
            }
            else//End not in Edit Mode...
            {
                SetUpNotInEditMode(sender, e);
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblShortageTotal = (Label)e.Row.FindControl("lblShortageTotal");
            lblShortageTotal.Text = dcShortageTotal.ToString("#,0.00");
            Label lblQtyTotal = (Label)e.Row.FindControl("lblQtyTotal");
            lblQtyTotal.Text = dcQtyTotal.ToString("#,0.00");
        }
    }
    protected void gvInventoryShortageReport_Sorting(object sender, GridViewSortEventArgs e)
    {

        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtInventoryShortageReport"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvInventoryShortageReport.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvInventoryShortageReport.DataSource = m_DataView;
            gvInventoryShortageReport.DataBind();
            gvInventoryShortageReport.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();

    }
    //Summary
    protected void gvInventoryShortageReportSummary_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gvInventoryShortageReportSummary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcShortage = 0;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblStockCode = (Label)e.Row.FindControl("lblStockCode");
            Label lblShortage = (Label)e.Row.FindControl("lblShortage");

            if (lblShortage.Text != "")
            {
                dcShortage = Convert.ToDecimal(lblShortage.Text);
                dcShortageTotalSummary += dcShortage;
                lblShortage.Text = Convert.ToDecimal(lblShortage.Text.Trim()).ToString("#,0.00");
            }

            if (lblStockCode.Text.Trim() != null)
            {
                GridView gvProductionSchedule = (GridView)e.Row.FindControl("gvProductionSchedule");
                DataTable dtProductionSchedule = new DataTable();
                dtProductionSchedule = SharedFunctions.GetProductionScheduleUnGrouped(lblStockCode.Text.Trim());
                gvProductionSchedule.DataSource = dtProductionSchedule;
                gvProductionSchedule.DataBind();
                dtProductionSchedule.Dispose();

                System.Web.UI.WebControls.Image imgNotes = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgNotes");

                Panel pnlNotes = (Panel)e.Row.FindControl("pnlNotes");
                if (dtProductionSchedule.Rows.Count != 0)
                {
                    pnlNotes.Visible = true;
                    imgNotes.Visible = true;
                    imgNotes.Style.Add("cursor", "pointer");
                }
                else
                {
                    pnlNotes.Visible = false;
                    imgNotes.Visible = false;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblShortageTotal = (Label)e.Row.FindControl("lblShortageTotal");
            lblShortageTotal.Text = dcShortageTotalSummary.ToString("#,0.00");

        }
    }
    protected void gvInventoryShortageReportSummary_Sorting(object sender, GridViewSortEventArgs e)
    {

        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtInventoryShortageReportSummary"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvInventoryShortageReportSummary.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvInventoryShortageReportSummary.DataSource = m_DataView;
            gvInventoryShortageReportSummary.DataBind();
            gvInventoryShortageReportSummary.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();

    }

    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {

        //txtStartDateChart2.Text = txtStartDate.Text;
    }
    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {

        // txtEndDateChart2.Text = txtEndDate.Text;
    }
    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblRecordCount.Text = "";
        gvInventoryShortageReport.DataSource = null;
        gvInventoryShortageReport.DataBind();

        switch (ddlPeriod.SelectedIndex)
        {

            case 0://Range
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                txtStartDate.Enabled = true;
                txtEndDate.Enabled = true;
                break;
            case 1://Single
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                txtStartDate.Enabled = true;
                txtEndDate.Enabled = false;

                break;
            case 2://Up To Date
                txtStartDate.Text = "";
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                txtStartDate.Enabled = false;
                txtEndDate.Enabled = true;
                break;
            case 3://ALL
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                txtStartDate.Enabled = false;
                txtEndDate.Enabled = false;
                break;
        }


    }
    protected void imgExportExcelShortage_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";

        if (Session["dtInventoryShortageReport"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtInventoryShortageReport"];

        dt.TableName = "InventoryShortageReport";
        foreach (DataRow row in dt.Rows)
        {
            string sSource = row["SalesOrder"].ToString();
            var sResult = int.Parse(sSource).ToString();
            row["SalesOrder"] = sResult;
        }
        dt.AcceptChanges();

        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "ShortageReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();


    }

    protected void imgExportExcelShortageSummary_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";

        if (Session["dtInventoryShortageReportSummary"] == null)
        {
            lblError.Text = "**No Summary Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtInventoryShortageReportSummary"];

        dt.TableName = "InventoryShortageReportSummary";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "ShortageReportSummary_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void gvProductionSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblScheduledQty = (Label)e.Row.FindControl("lblScheduledQty");
            Label lblScheduledDate = (Label)e.Row.FindControl("lblScheduledDate");
            if (lblScheduledQty.Text != "")
            {
                lblScheduledQty.Text = Convert.ToDecimal(lblScheduledQty.Text.Trim()).ToString("0.00");
            }
            if (lblScheduledDate.Text != "")
            {
                lblScheduledDate.Text = Convert.ToDateTime(lblScheduledDate.Text.Trim()).ToShortDateString();
            }
        }
    }
    protected void btnRunReportSummary_Click(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
        lblError.Text = "";
        lblRecordCount0.Text = "";
        LoadInventoryShortageReportSummary(iRoleID, iUserID);
    }

    #endregion






}