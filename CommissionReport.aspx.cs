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
using System.Web.UI.DataVisualization.Charting;
using System.Data.Linq.SqlClient;

public partial class CommissionReport : System.Web.UI.Page
{
    decimal dcTotalSales = 0;
    decimal dcTotalCommission = 0;

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
    private void LoadCustomer(DropDownList ddl, string sSortBy)
    {
        ddl.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        if (sSortBy == "Name")
        {
            var query = (from c in db.ArCustomer
                         where c.Customer != "test"
                         orderby c.Name.Trim()
                         select new
                         {
                             Customer = c.Customer.Trim(),
                             Name = c.Name
                         });
            foreach (var a in query)
            {
                ddl.Items.Add(new ListItem(a.Customer + " - " + a.Name, a.Customer));
            }
            ddl.Items.Insert(0, new ListItem("All", "All"));

        }
        else
        {
            var query = (from c in db.ArCustomer
                         where c.Customer != "test"
                         orderby c.Customer.Trim()
                         select new
                         {
                             Customer = c.Customer.Trim(),
                             Name = c.Name
                         });
            foreach (var a in query)
            {
                ddl.Items.Add(new ListItem(a.Customer + " - " + a.Name, a.Customer));
            }
            ddl.Items.Insert(0, new ListItem("All", "All"));
        }




    }
    private void LoadNonCustomers(DropDownList ddl)
    {
        ddl.Items.Clear();
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from c in db.ArNonCustomer
                     orderby c.Name
                     select c);
        if (query.Count() > 0)
        {
            foreach (var a in query)
            {
                ddl.Items.Add(new ListItem(a.Name, a.NonCustomerID.ToString()));
            }
            ddl.Items.Insert(0, new ListItem("SELECT", "0"));
        }
        else
        {
            ddl.Items.Insert(0, new ListItem("No Non Customers found...", "0"));
        }
    }
    private void LoadProductClass(DropDownList ddl)
    {
        ddl.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from c in db.SalProductClassDes
                     orderby c.Description
                     select new
                     {
                         c.Description,
                         c.ProductClass

                     });
        foreach (var a in query)
        {
            ddl.Items.Add(new ListItem(a.Description + " - " + a.ProductClass, a.ProductClass));
        }
        ddl.Items.Insert(0, new ListItem("All", "All"));
    }
    private void LoadSalesPersons()
    {
        ddlSalesPerson.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from c in db.ArCommissionReport
                     orderby c.SalespersonName.Trim()
                     select new
                     {
                         c.SalespersonID,
                         c.SalespersonName

                     }).Distinct();
        foreach (var a in query)
        {
            ddlSalesPerson.Items.Add(new ListItem(a.SalespersonName.ToUpper(), a.SalespersonID));
        }
        rsListbox.Sort(ref ddlSalesPerson, rsListbox.SortOrder.Ascending);
        ddlSalesPerson.Items.Insert(0, new ListItem("All", "All"));
    }
    private void LoadCustomerForCharts(DropDownList ddl)
    {
        ddl.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from c in db.ArCustomer
                     where c.Customer != "test"
                     orderby c.Name.Trim()
                     select new
                     {
                         Customer = c.Customer.Trim(),
                         Name = c.Name
                     });
        foreach (var a in query)
        {
            ddl.Items.Add(new ListItem(a.Customer + " - " + a.Name, a.Customer));
        }
        ddl.Items.Insert(0, new ListItem("Select", "0"));
    }
    private void RunReportSummary()
    {
        string sMsg = "";

        string sSalesPerson = "NULL";
        if (ddlSalesPerson.SelectedIndex != 0)//ALL...
        {//Not null then add quotes...
            sSalesPerson = "'" + ddlSalesPerson.SelectedValue.Trim() + "'";
        }


        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);


        sSQL = "EXEC spGetCommissionReport ";
        sSQL += "@SalesPerson=" + sSalesPerson;


        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");


        if (dt.Rows.Count > 0)
        {
            Session["dtCommissionReportSummary"] = dt;
            gvReportSummary.DataSource = dt;
            gvReportSummary.DataBind();
            imgExportExcel1.Enabled = true;
            pnlGridView.Visible = true;
            tblHeaderTable.Visible = true;
        }
        else
        {
            lblError.Text = "No results found!!";
            lblError.ForeColor = Color.Red;
            pnlGridView.Visible = false;
            tblHeaderTable.Visible = false;
            imgExportExcel1.Enabled = false;
            gvReportSummary.DataSource = null;
            gvReportSummary.DataBind();
        }

        if (dt.Rows.Count > 9)
        {
            pnlGridView.ScrollBars = ScrollBars.Vertical;
            pnlGridView.Width = Unit.Pixel(1125);
            tblHeaderTable.Width = Unit.Pixel(1125);
        }
        else
        {
            pnlGridView.ScrollBars = ScrollBars.None;
            pnlGridView.Width = Unit.Pixel(1100);
            tblHeaderTable.Width = Unit.Pixel(1100);
        }

    }
    private void RunReportDetails()
    {
        string sMsg = "";

        string sSalesPerson = "NULL";
        if (ddlSalesPerson.SelectedIndex != 0)//ALL...
        {//Not null then add quotes...
            sSalesPerson = "'" + ddlSalesPerson.SelectedValue.Trim() + "'";
        }

        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        sSQL = "EXEC spGetCommissionReportDetails ";
        sSQL += "@SalesPerson=" + sSalesPerson;

        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");

        if (dt.Rows.Count > 0)
        {
            Session["dtCommissionReportDetails"] = dt;
            gvReportDetails.DataSource = dt;
            gvReportDetails.DataBind();
            imgExportExcel2.Enabled = true;
            pnlGridView2.Visible = true;
            tblHeaderTable2.Visible = false;//Hide for now...
            Label1.Text = "Details";
        }
        else
        {
            lblError.Text = "No results found!!";
            lblError.ForeColor = Color.Red;
            pnlGridView2.Visible = false;
            tblHeaderTable2.Visible = false;
            imgExportExcel2.Enabled = false;
            gvReportDetails.DataSource = null;
            gvReportDetails.DataBind();
            Label1.Text = "";
        }

        if (dt.Rows.Count > 9)
        {
            pnlGridView2.ScrollBars = ScrollBars.Vertical;
            pnlGridView2.Width = Unit.Pixel(1325);
            tblHeaderTable2.Width = Unit.Pixel(1325);
        }
        else
        {
            pnlGridView2.ScrollBars = ScrollBars.None;
            pnlGridView2.Width = Unit.Pixel(1300);
            tblHeaderTable2.Width = Unit.Pixel(1300);
        }

    }

    private void SetupHeaderTable1()
    {
        TableRow headerRow = new TableRow();

        divHeader.Style.Add("width", "1200px");
        for (int x = 0; x < gvReportSummary.Columns.Count; x++)
        {
            DataControlField col = gvReportSummary.Columns[x];

            TableCell headerCell = new TableCell();
            headerCell.BorderStyle = BorderStyle.Solid;
            headerCell.BorderWidth = 0;//Hide vertical grid lines on header...
            headerCell.Font.Bold = true;
            headerCell.Font.Size = FontUnit.Point(10);
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

                lnkHeader.Click += new EventHandler(HeaderLink1_Click);
                headerCell.Controls.Add(lnkHeader);
            }
            else
            {
                headerCell.Text = col.HeaderText;
            }
            //We need to set the width of the column header customly...
            switch (x)
            {
                case 0://Salesperson#
                    headerCell.Width = Unit.Pixel(45);
                    break;
                case 1://SalesPerson Name
                    headerCell.Width = Unit.Pixel(135);
                    break;
                case 2://Customer#
                    headerCell.Width = Unit.Pixel(50);
                    break;
                case 3://Customer Name
                    headerCell.Width = Unit.Pixel(135);
                    break;
                case 4://Sales
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 5://Commission
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 6://%
                    headerCell.Width = Unit.Pixel(40);
                    break;


            }


            //headerCell.Width = col.ItemStyle.Width;
            headerRow.Cells.Add(headerCell);

        }

        tblHeaderTable.Rows.Add(headerRow);
    }
    private void SetupHeaderTable2()
    {
        TableRow headerRow = new TableRow();

        divHeader2.Style.Add("width", "1200px");
        for (int x = 0; x < gvReportDetails.Columns.Count; x++)
        {
            DataControlField col = gvReportDetails.Columns[x];

            TableCell headerCell = new TableCell();
            headerCell.BorderStyle = BorderStyle.Solid;
            headerCell.BorderWidth = 1;//Hide vertical grid lines on header...
            headerCell.Font.Bold = true;
            headerCell.Font.Size = FontUnit.Point(10);
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

                lnkHeader.Click += new EventHandler(HeaderLink2_Click);
                headerCell.Controls.Add(lnkHeader);
            }
            else
            {
                headerCell.Text = col.HeaderText;
            }
            //We need to set the width of the column header customly...
            switch (x)
            {
                case 0://Salesperson#
                    headerCell.Width = Unit.Pixel(30);
                    break;
                case 1://SalesPerson Name
                    headerCell.Width = Unit.Pixel(90);
                    break;
                case 2://Customer#
                    headerCell.Width = Unit.Pixel(50);
                    break;
                case 3://Customer Name
                    headerCell.Width = Unit.Pixel(100);
                    break;
                case 4://Inv Number
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 5://Inv Date
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 6://StockCode
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 7://Stock Desc
                    headerCell.Width = Unit.Pixel(100);
                    break;
                case 8://Qty
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 9://Uom
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 10://UnitPrice
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 11://LinePrice
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 12://CashDiscAmt
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 13://NetSalAmt
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 14://ComAmt
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 15://Comm%
                    headerCell.Width = Unit.Pixel(40);
                    break;

            }


            //headerCell.Width = col.ItemStyle.Width;
            headerRow.Cells.Add(headerCell);

        }

        tblHeaderTable2.Rows.Add(headerRow);
       
    }

    #endregion

    #region Functions
    private List<string> GetNoCustomerStockCodes(int iNonCustomerID)
    {
        List<string> lStockCodes = new List<string>();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from c in db.ArNonCustomerStockCodeAssignments
                     where c.NonCustomerID == iNonCustomerID
                     select new
                     {
                         c.StockCode,
                     });
        foreach (var a in query)
        {
            lStockCodes.Add(a.StockCode.Trim());
        }
        return lStockCodes;
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
    private string GetDescriptionFromIngredientStockCode(string sIngredientStockCode)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        string sDesc = "";
        var query = (from w in db.InvLookupTableIngredientsRSS
                     where w.MStockCode.Trim() == sIngredientStockCode
                     group w by new
                     {
                         w.MDescription
                     } into g
                     orderby
                       g.Key.MDescription
                     select new
                     {

                         g.Key.MDescription
                     });
        if (query.Count() > 0)
        {
            foreach (var a in query)
            {
                sDesc = a.MDescription;
            }
        }
        return sDesc;
    }
    private List<string> GetNonCustomerStockCodes(int iNonCustomerID)
    {
        List<string> lStockCodes = new List<string>();
        lStockCodes.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from c in db.ArNonCustomerStockCodeAssignments
                     select new
                     {
                         c.StockCode,


                     });
        foreach (var a in query)
        {
            lStockCodes.Add(a.StockCode);
        }
        return lStockCodes;
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
            LoadSalesPersons();

        }//End postback

    }
    protected void gvReportSummary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcSales = 0;
        decimal dcCommission = 0;//InvoiceValue...
        int iRoleID = 0;
        iRoleID = Convert.ToInt32(Session["RoleID"]);



        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label lblSales = (Label)e.Row.FindControl("lblSales");
            Label lblCommission = (Label)e.Row.FindControl("lblCommission");

            if (lblSales.Text != "")
            {
                dcSales = Convert.ToDecimal(lblSales.Text.Trim());
                dcTotalSales += dcSales;
                lblSales.Text = Convert.ToDecimal(lblSales.Text).ToString("#,0.00");
            }
            if (lblCommission.Text != "")
            {
                dcCommission = Convert.ToDecimal(lblCommission.Text.Trim());
                dcTotalCommission += dcCommission;
                lblCommission.Text = Convert.ToDecimal(lblCommission.Text).ToString("#,0.00");
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblTotalSales = (Label)e.Row.FindControl("lblTotalSales");
            Label lblTotalCommission = (Label)e.Row.FindControl("lblTotalCommission");

            lblTotalSales.Text = "$" + dcTotalSales.ToString("#,0.00");
            lblTotalCommission.Text = "$" + dcTotalCommission.ToString("#,0.00");

            lblTotalSalesLabel.Text = "TOTAL SALES:  $" + dcTotalSales.ToString("#,0.00");
            lblTotalCommissionLabel.Text = " TOTAL COMM:  $" + dcTotalCommission.ToString("#,0.00");
        }

    }
    protected void gvReportSummary_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();

        DataTable dt = (DataTable)Session["dtCommissionReportSummary"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvReportSummary.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvReportSummary.DataSource = m_DataView;
            gvReportSummary.DataBind();
            gvReportSummary.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
    }
    protected void gvReportDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
 
        int iRoleID = 0;
        iRoleID = Convert.ToInt32(Session["RoleID"]);



        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            Label lblInvoiceDate = (Label)e.Row.FindControl("lblInvoiceDate");

            if (lblInvoiceDate.Text != "")
            {
                lblInvoiceDate.Text = Convert.ToDateTime(lblInvoiceDate.Text).ToShortDateString();
            }

            Label lblInvoiceNumber = (Label)e.Row.FindControl("lblInvoiceNumber");
            if (lblInvoiceNumber.Text != "")
            {
                lblInvoiceNumber.Text = Convert.ToInt32(lblInvoiceNumber.Text).ToString("");
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {

        }

    }
    protected void gvReportDetails_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();

        DataTable dt = (DataTable)Session["dtCommissionReportDetails"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvReportDetails.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvReportDetails.DataSource = m_DataView;
            gvReportDetails.DataBind();
            gvReportDetails.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        dcTotalSales = 0;
        dcTotalCommission = 0;
        lblError.Text = "";
        lblTotalSalesLabel.Text = "";
        lblTotalCommissionLabel.Text = "";
        RunReportSummary();

        trSummaryReport.Visible = true;

    }
    protected void btnPreviewDetails_Click(object sender, EventArgs e)
    {
        dcTotalSales = 0;
        dcTotalCommission = 0;
        lblError.Text = "";

        RunReportDetails();
    }

    protected void HeaderLink1_Click(object sender, System.EventArgs e)
    {
        LinkButton lnkHeader = (LinkButton)sender;
        SortDirection direction = SortDirection.Ascending;

        // the CommandArgument of each linkbutton contains the sortexpression
        // for the column that was clicked.
        if (gvReportSummary.SortExpression == lnkHeader.CommandArgument)
        {
            if (gvReportSummary.SortDirection == SortDirection.Ascending)
            {
                direction = SortDirection.Descending;
            }

        }

        gvReportSummary.Sort(lnkHeader.CommandArgument, direction);
    }
    protected void HeaderLink2_Click(object sender, System.EventArgs e)
    {
        LinkButton lnkHeader2 = (LinkButton)sender;
        SortDirection direction = SortDirection.Ascending;

        // the CommandArgument of each linkbutton contains the sortexpression
        // for the column that was clicked.
        if (gvReportDetails.SortExpression == lnkHeader2.CommandArgument)
        {
            if (gvReportDetails.SortDirection == SortDirection.Ascending)
            {
                direction = SortDirection.Descending;
            }

        }

        gvReportDetails.Sort(lnkHeader2.CommandArgument, direction);
    }

    protected void imgExportExcel1_Click(object sender, ImageClickEventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";
        if (Session["dtCommissionReportSummary"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtCommissionReportSummary"];

        dt.TableName = "dtCommissionReportSummary";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "CustomerReport_Summary_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void imgExportExcel2_Click(object sender, ImageClickEventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";
        if (Session["dtCommissionReportDetails"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtCommissionReportDetails"];

        dt.TableName = "dtCommissionReportDetails";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "CustomerReport_Details_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    // create the table for the fixed header in Init
    protected void Page_Init(object sender, EventArgs e)
    {
        SetupHeaderTable1();
        SetupHeaderTable2();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlSalesPerson.SelectedIndex = 0;
    }
    #endregion



}