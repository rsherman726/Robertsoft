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

public partial class ExceptionReport : System.Web.UI.Page
{
    static int TickNumber;

    decimal dcLineTotal = 0;
    decimal dcGrandTotal = 0;

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
    private void LoadGrid()
    {//Default...(HEADER)
        DataTable dt = new DataTable();

        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        string sSQL = "";

        sSQL = "EXEC spGetExceptionReport";
     

        Debug.WriteLine(sSQL);
        dt = SharedFunctions.getDataTable(sSQL, conn, "JobHeader");
        gvRecord.PageSize = Convert.ToInt32(ddlDisplayCount.SelectedValue);
        gvRecord.DataSource = dt;
        gvRecord.DataBind();


        Session["dtRecord"] = dt;
        lblPageNo.Text = "Current Page #: 1";
        //TODO: Sub to Populate the Timeline...

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
    private DataTable GetDetails(int iSalesOrder, GridView gv)
    {//DETAILS GRID Create Dynamically...
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        try
        {

            sSQL = "spGetSalesOrderTrackerDetails @SalesOrder =" + iSalesOrder;

            Debug.WriteLine(sSQL);
            dt = SharedFunctions.getDataTable(sSQL, conn, "Details");

            gv.DataSource = dt;
            gv.DataBind();
            Session["dtDetails"] = dt;

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
            LoadGrid(); 
        }

    }
    protected void gvRecord_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRecord.PageIndex = e.NewPageIndex;
        lblPageNo.Text = "Current Page #: " + (gvRecord.PageIndex + 1).ToString();
        gvRecord.DataSource = (DataTable)Session["dtRecord"];
        gvRecord.DataBind();
    }
    protected void gvRecord_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcTotalValue = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblTotalValue = (Label)e.Row.FindControl("lblTotalValue");
            if (lblTotalValue.Text != "")
            {
                dcTotalValue = Convert.ToDecimal(lblTotalValue.Text.Replace("$", ""));
                dcGrandTotal += dcTotalValue;
                lblTotalValue.Text = "$" + lblTotalValue.Text;
            }

            Label lblOrderDate = (Label)e.Row.FindControl("lblOrderDate");
            Label lblShipDate = (Label)e.Row.FindControl("lblShipDate");
            if (lblOrderDate.Text != "")
            {
                lblOrderDate.Text = Convert.ToDateTime(lblOrderDate.Text).ToShortDateString();
            }
            if (lblShipDate.Text != "")
            {
                lblShipDate.Text = Convert.ToDateTime(lblShipDate.Text).ToShortDateString();
            }
            Label lblDeliveryStatus = (Label)e.Row.FindControl("lblDeliveryStatus");
            ImageButton ibnDeliveryStatus = (ImageButton)e.Row.FindControl("ibnDeliveryStatus");

            switch (lblDeliveryStatus.Text)
            {
                case "--":
                    ibnDeliveryStatus.Visible = false;
                    break;
                case "1"://PickUp Completed
                    ibnDeliveryStatus.ImageUrl = "images/DeliveryComplete.jpg";
                    ibnDeliveryStatus.ToolTip = "PickUp Completed";
                    break;
                case "2"://Delivery Completed
                    ibnDeliveryStatus.ImageUrl = "images/DeliveryComplete.jpg";
                    ibnDeliveryStatus.ToolTip = "Delivery Completed";
                    break;
                case "3"://PickUp Confirmed
                    ibnDeliveryStatus.ImageUrl = "images/DeliveryScheduled.jpg";
                    ibnDeliveryStatus.ToolTip = "PickUp Scheduled";
                    break;
                case "4"://Delivery Confirmed
                    ibnDeliveryStatus.ImageUrl = "images/DeliveryScheduled.jpg";
                    ibnDeliveryStatus.ToolTip = "Delivery Scheduled";
                    break;
            }

            System.Web.UI.WebControls.Image imgFlaggedPO = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgFlaggedPO");
            System.Web.UI.WebControls.Image imgFlaggedDR = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgFlaggedDR");
            Label lblPOFlag = (Label)e.Row.FindControl("lblPOFlag");
            Label lblDRFlag = (Label)e.Row.FindControl("lblDRFlag");
            if (lblPOFlag.Text != "")
            {
                if (lblPOFlag.Text == "0")
                {//Show Flag if value is zero...
                    imgFlaggedPO.Visible = true;
                    imgFlaggedPO.ToolTip = "Missing Purchase Order Document";
                }
                else
                {
                    imgFlaggedPO.Visible = false;
                }
            }
            if (lblDRFlag.Text != "")
            {
                if (lblDRFlag.Text == "0")
                {//Show Flag if value is zero...
                    imgFlaggedDR.Visible = true;
                    imgFlaggedDR.ToolTip = "Missing Delivery Receipt Document";
                }
                else
                {
                    imgFlaggedDR.Visible = false;
                }
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblGrandTotal = (Label)e.Row.FindControl("lblGrandTotal");
            lblGrandTotal.Text = "$" + dcGrandTotal.ToString("#,0.00");
        }

    }
    protected void gvRecord_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int idx = 0;
        int iSalesOrder = 0;
        DataTable dt = new DataTable();
        LinkButton lbnSalesOrder;
        LinkButton lbnPurchaseOrder;
        LinkButton lbnDeliveryID;
        Label lblQueryStatus;
        switch (e.CommandName)
        {

            case "Select":
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...
                lbnSalesOrder = (LinkButton)gvRecord.Rows[idx].FindControl("lbnSalesOrder");
                lblQueryStatus = (Label)gvRecord.Rows[idx].FindControl("lblQueryStatus");
                iSalesOrder = Convert.ToInt32(lbnSalesOrder.Text);

                //Rebind gvDash...
                GetDetails(iSalesOrder, gvDetails);
                if (gvDetails.Rows.Count == 0)
                {
                    lblQueryStatus.Text = "No items found";
                }
                else
                {
                    lblQueryStatus.Text = "";
                }
                Timer1.Enabled = false;//Turn Off...
                ModalPopupExtenderPopUp.Show();
                break;

            case "View":
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...
                lbnSalesOrder = (LinkButton)gvRecord.Rows[idx].FindControl("lbnSalesOrder");
                Response.Redirect("DeliveryAdmin.aspx?so=" + lbnSalesOrder.Text);
                break;
            case "GetPO":
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...
                lbnPurchaseOrder = (LinkButton)gvRecord.Rows[idx].FindControl("lbnPurchaseOrder");
                Response.Redirect("DocSearch.aspx?id=" + lbnPurchaseOrder.Text.Trim().Replace(" ", "") + "&type=PO");
                break;
            case "GetSO":
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...
                lbnSalesOrder = (LinkButton)gvRecord.Rows[idx].FindControl("lbnSalesOrder");
                Response.Redirect("DocSearch.aspx?id=" + lbnSalesOrder.Text.Trim() + "&type=SO");
                break;
            case "GetDR":
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...
                lbnDeliveryID = (LinkButton)gvRecord.Rows[idx].FindControl("lbnDeliveryID");
                Response.Redirect("DocSearch.aspx?id=" + lbnDeliveryID.Text.Trim() + "&type=DR");
                break;
        }

    }
    protected void gvRecord_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtRecord"];
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

            System.Web.UI.WebControls.Image imgViewSpecialInstrs = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgViewSpecialInstrs");
            System.Web.UI.WebControls.Image imgViewShippingInstrs = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgViewShippingInstrs");

            Label lblSpecialInstrs = (Label)e.Row.FindControl("lblSpecialInstrs");
            Label lblShippingInstrs = (Label)e.Row.FindControl("lblShippingInstrs");
            Label lblExtendedPrice = (Label)e.Row.FindControl("lblExtendedPrice");
            Label lblPrice = (Label)e.Row.FindControl("lblPrice");
            if (lblPrice.Text != "")
            {
                lblPrice.Text = "$" + lblPrice.Text;
            }
            if (lblExtendedPrice.Text != "")
            {
                dcPrice = Convert.ToDecimal(lblExtendedPrice.Text.Replace("$", ""));
                dcLineTotal += dcPrice;
                lblExtendedPrice.Text = "$" + lblExtendedPrice.Text;
            }

            if (lblSpecialInstrs.Text == "")
            {
                imgViewSpecialInstrs.Visible = false;
            }
            if (lblShippingInstrs.Text == "")
            {
                imgViewShippingInstrs.Visible = false;
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblPriceTotal = (Label)e.Row.FindControl("lblPriceTotal");
            lblPriceTotal.Text = "$" + dcLineTotal.ToString("#,0.00");
        }
    }
    protected void ddlDisplayCount_SelectedIndexChanged(object sender, EventArgs e)
    {
        int iUserID = 0;

        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        else
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
        }

        LoadGrid();


    }
    protected void Timer1_Tick(object sender, EventArgs e)
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
        }
        switch (TickNumber)
        {
            case 1:
                LoadGrid();
                Debug.WriteLine("Full Insert: " + TickNumber);
                lblTimeUpdated.Text = "Data Last Updated: " + DateTime.Now.ToString();
                TickNumber = 0;//Reset counter...
                break;
            default:
                Debug.WriteLine("Just Display: " + TickNumber);
                LoadGrid();
                break;
        }

        TickNumber++;

        //TODO reset the TickNumber to 0 after a while

        if (TickNumber == 9999)
        {
            TickNumber = 0;
        }

    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderPopUp.Hide();
        Timer1.Enabled = true;

        gvDetails.DataSource = null;
        gvDetails.DataBind();

    }
    //FrozenHeaderRow...
    // LinkButtons are used to dynamically create the links necessary
    // for paging.
    protected void HeaderLink_Click(object sender, System.EventArgs e)
    {
        LinkButton lnkHeader = (LinkButton)sender;
        SortDirection direction = SortDirection.Ascending;

        // the CommandArgument of each linkbutton contains the sortexpression
        // for the column that was clicked.
        if (gvRecord.SortExpression == lnkHeader.CommandArgument)
        {
            if (gvRecord.SortDirection == SortDirection.Ascending)
            {
                direction = SortDirection.Descending;
            }

        }

        gvRecord.Sort(lnkHeader.CommandArgument, direction);
    }

    // create the table for the fixed header in Init
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
                case 0://Select
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 1://SO#
                    headerCell.Width = Unit.Pixel(50);
                    break;
                case 2://Customer
                    headerCell.Width = Unit.Pixel(225);
                    break;
                case 3://CustID
                    headerCell.Width = Unit.Pixel(61);
                    break;
                case 4://OrderDate
                    headerCell.Width = Unit.Pixel(80);
                    break;
                case 5://TotalValue
                    headerCell.Width = Unit.Pixel(90);
                    break;
                case 6://OrderStatus
                    headerCell.Width = Unit.Pixel(70);
                    break;
                case 7://RequestShipDate
                    headerCell.Width = Unit.Pixel(100);
                    break;
                case 8://SalesPerson
                    headerCell.Width = Unit.Pixel(133);
                    break;
                case 9://PurchaseOrder
                    headerCell.Width = Unit.Pixel(150);
                    break;
                case 10://PurchaseOrder Flag
                    headerCell.Width = Unit.Pixel(30);
                    break;
                case 11://Delivery
                    headerCell.Width = Unit.Pixel(50);
                    break;
                case 12://DeliveryID
                    headerCell.Width = Unit.Pixel(50);
                    break;
                case 13://DeliveryID Flag
                    headerCell.Width = Unit.Pixel(30);
                    break;
            }


            //headerCell.Width = col.ItemStyle.Width;
            headerRow.Cells.Add(headerCell);

        }

        HeaderTable.Rows.Add(headerRow);
    }

    #endregion

}