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


public partial class DeliveryReport : System.Web.UI.Page
{
    static int TickNumber;

    decimal dcLineTotal = 0;
     
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
 
        sSQL = "EXEC spGetDeliveryReport";        

        Debug.WriteLine(sSQL);
        dt = SharedFunctions.getDataTable(sSQL, conn, "JobHeader");      
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
            if (!SharedFunctions.IsAccessGranted(iAdminID, sText))
            {
                Response.Redirect("Default.aspx");
            }
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
       
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
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

            Label lblDateScheduled = (Label)e.Row.FindControl("lblDateScheduled");
            Label lblDateDelivered = (Label)e.Row.FindControl("lblDateDelivered");

            if (lblDateScheduled.Text != "")
            {
                lblDateScheduled.Text = Convert.ToDateTime(lblDateScheduled.Text).ToShortDateString();
            }
            else
            {
                lblDateScheduled.Text = "--";
            }
            if (lblDateDelivered.Text != "")
            {
                lblDateDelivered.Text = Convert.ToDateTime(lblDateDelivered.Text).ToShortDateString();
            }
            else
            {
                lblDateDelivered.Text = "--";
            }

            Label lblDeliveryTypeID = (Label)e.Row.FindControl("lblDeliveryTypeID");
            if (lblDeliveryTypeID.Text != "")
            {
                if (lblDeliveryTypeID.Text == "1")//Pickup
                {
                    lblDeliveryTypeID.Text = "Pick Up";
                }
                else if (lblDeliveryTypeID.Text == "2")
                {
                    lblDeliveryTypeID.Text = "Delivery";
                }
                else if(lblDeliveryTypeID.Text == "3")
                {
                    lblDeliveryTypeID.Text = "Will Call";
                }
            }

            Label lblDeliveryStatus = (Label)e.Row.FindControl("lblDeliveryStatus");
            if (lblDeliveryStatus.Text != "")
            {
                switch (lblDeliveryStatus.Text)
                {
                    case "1":
                        lblDeliveryStatus.Text = "PickUp Completed";
                        break;
                    case "2":
                        lblDeliveryStatus.Text = "Delivery/Will Call Completed";
                        break;
                    case "3":
                        lblDeliveryStatus.Text = "PickUp Scheduled";
                        break;
                    case "4":                        
                        lblDeliveryStatus.Text = "Delivery/Will Call Scheduled";
                        break;
                    default:
                        lblDeliveryStatus.Text = "Not Set";
                        break;
                }
            }
            else
            {
                lblDeliveryStatus.Text = "Not Set";
            }

            System.Web.UI.WebControls.Image imgFlaggedDR = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgFlaggedDR");
            Label lblDRFlag = (Label)e.Row.FindControl("lblDRFlag");
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

            Label lblCOD = (Label)e.Row.FindControl("lblCOD");
            if (lblCOD.Text != "")
            {
                if (lblCOD.Text == "1")
                {
                    lblCOD.Text = "YES";
                }
                else
                {
                    lblCOD.Text = "NO";
                }
            }

            Label lblAmount = (Label)e.Row.FindControl("lblAmount");

            if (lblAmount.Text != "")
            {
                lblAmount.Text = Convert.ToDecimal(lblAmount.Text).ToString("c");
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
 
        }

    }
    protected void gvRecord_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int idx = 0;
        int iSalesOrder = 0;
        DataTable dt = new DataTable();
        LinkButton lbnSalesOrder;        
        Label lblDeliveryID;
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
            case "Scan"://Goes to pre scan...
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...
                lblDeliveryID = (Label)gvRecord.Rows[idx].FindControl("lblDeliveryID");
                Response.Redirect("PreScan.aspx?id=" + lblDeliveryID.Text + "&type=DR");
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
                lnkHeader.Font.Size = FontUnit.Point(7);

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
                case 0://SO#
                    headerCell.Width = Unit.Pixel(51);
                    break;
                case 1://Customer
                    headerCell.Width = Unit.Pixel(148);
                    break;
                case 2://CustID
                    headerCell.Width = Unit.Pixel(56);
                    break;
                case 3://OrderDate
                    headerCell.Width = Unit.Pixel(75);
                    break;
                case 4://OrderStatus
                    headerCell.Width = Unit.Pixel(74);
                    break;
                case 5://Request Ship Date
                    headerCell.Width = Unit.Pixel(73);
                    break;
                case 6://DeliveryID
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 7://Type
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 8://Date Scheduled
                    headerCell.Width = Unit.Pixel(72);
                    break;
                case 9://Date Delivered
                    headerCell.Width = Unit.Pixel(71);
                    break;
                case 10://Delivery Status
                    headerCell.Width = Unit.Pixel(73);
                    break;
                case 11://COD
                    headerCell.Width = Unit.Pixel(26);
                    break;
                case 12://Check Number
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 13://Amount
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 14://Truck
                    headerCell.Width = Unit.Pixel(51);
                    break;
                case 15://Comments
                    headerCell.Width = Unit.Pixel(73);
                    break;
                case 16://Flag
                    headerCell.Width = Unit.Pixel(25);
                    break;
            }


            //headerCell.Width = col.ItemStyle.Width;
            headerRow.Cells.Add(headerCell);

        }

        HeaderTable.Rows.Add(headerRow);
    }

    #endregion
}