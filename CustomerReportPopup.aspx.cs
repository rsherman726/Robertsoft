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

public partial class CustomerReportPopup : System.Web.UI.Page
{
    decimal dcCostValueTotal = 0;
    decimal dcTotalAmount = 0;

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
    private void RunReport(string sStartDate, string sEndDate, string sStockCode)
    {

        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        sSQL = "EXEC spGetCustomerReport ";
        sSQL += "@FromDate='" + sStartDate + "',";
        sSQL += "@ToDate='" + sEndDate + "',";
        sSQL += "@StockCodes='" + sStockCode + "'";


        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
        Session["dtCustomerReport"] = dt;
        gvReportCondensed.DataSource = dt;
        gvReportCondensed.DataBind();

        if (dt.Rows.Count > 0)
        {
            pnlGridView.Visible = true;
            tblHeaderTable.Visible = true;
        }
        else
        {
            pnlGridView.Visible = false;
            tblHeaderTable.Visible = false;
        }

        if (dt.Rows.Count > 9)
        {
            pnlGridView.ScrollBars = ScrollBars.Vertical;
            pnlGridView.Width = Unit.Pixel(1270);
            tblHeaderTable.Width = Unit.Pixel(1270);
        }
        else
        {
            pnlGridView.ScrollBars = ScrollBars.None;
            pnlGridView.Width = Unit.Pixel(1250);
            tblHeaderTable.Width = Unit.Pixel(1250);
        }

    }
    private void RunReport2(string sStartDate, string sEndDate, string sStockCode, string sCustomer)
    {
        //sStockCode for Exclusion purposes...
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        sSQL = "EXEC spGetCustomerReportAlsoBought ";
        sSQL += "@FromDate='" + sStartDate + "',";
        sSQL += "@ToDate='" + sEndDate + "',";
        sSQL += "@ExcludeStockCode='" + sStockCode + "',";
        sSQL += "@Customer='" + sCustomer + "'";


        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
        Session["dtAlsoBought"] = dt;
        gvAlsoBought.DataSource = dt;
        gvAlsoBought.DataBind();

        if (dt.Rows.Count > 0)
        {
            pnlGridView.Visible = true;
            tblHeaderTable.Visible = true;
            lblAlsoBought.Visible = true;
            imgExportExcel2.Visible = true;
        }
        else
        {
            pnlGridView.Visible = false;
            tblHeaderTable.Visible = false;
            lblAlsoBought.Visible = false;
            imgExportExcel2.Visible = false;
        }

        if (dt.Rows.Count > 9)
        {
            pnlGridView.ScrollBars = ScrollBars.Vertical;
            pnlGridView.Width = Unit.Pixel(1170);
            tblHeaderTable.Width = Unit.Pixel(1150);
        }
        else
        {
            pnlGridView.ScrollBars = ScrollBars.None;
            pnlGridView.Width = Unit.Pixel(1150);
            tblHeaderTable.Width = Unit.Pixel(1075);
        }

    }
    private void GetChart1(string sCustomer, string sStockCode)
    {
        DataTable dt = new DataTable();

        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        sSQL = "EXEC spGetCustomerReportChartsPopUp ";
        sSQL += "@StockCode='" + sStockCode + "',";
        sSQL += "@Customer='" + sCustomer + "'";


        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
        Session["dtChart1"] = dt;
        bool bCht1 = false;
        if (chk3D.Checked)
        {
            bCht1 = true;
        }
        else
        {
            bCht1 = false;
        }
        if (dt != null)
        {
            if (dt.Rows.Count > 0)
            {
                gv1.DataSource = dt;
                gv1.DataBind();

                dt.TableName = "dt";
                Chart1.DataSource = dt;
                Chart1.DataBind();
                CreateChartPriceMarginHistory(Chart1, bCht1, dt);
                Chart1.Visible = true;
                chk3D.Visible = true;
            }
            else
            {
                Chart1.Visible = false;
                chk3D.Visible = false;
            }
        }
    }
    private void SetupHeaderTable1()
    {
        TableRow headerRow = new TableRow();
        divHeader.Style.Add("width", "1100px");
        for (int x = 0; x < gvReportCondensed.Columns.Count; x++)
        {
            DataControlField col = gvReportCondensed.Columns[x];

            TableCell headerCell = new TableCell();
            headerCell.BorderStyle = BorderStyle.Solid;
            headerCell.BorderWidth = 0;//Hide vertical grid lines on header...
            headerCell.Font.Bold = true;
            headerCell.Font.Size = FontUnit.Point(10);
            headerCell.HorizontalAlign = HorizontalAlign.Center;
            headerCell.VerticalAlign = VerticalAlign.Bottom;

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
                case 0://Name
                    headerCell.Width = Unit.Pixel(145);
                    break;
                case 1://Customer
                    headerCell.Width = Unit.Pixel(48);
                    break;
                case 2://StockCode
                    headerCell.Width = Unit.Pixel(48);
                    break;
                case 3://Decription
                    headerCell.Width = Unit.Pixel(134);
                    break;
                case 4://ProductClass
                    headerCell.Width = Unit.Pixel(97);
                    break;
                case 5://CostUom
                    headerCell.Width = Unit.Pixel(30);
                    break;
                case 6://InvoiceQty
                    headerCell.Width = Unit.Pixel(46);
                    break;
                case 7://Amount
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 8://Margin
                    headerCell.Width = Unit.Pixel(45);
                    break;
                case 9://PriceCode
                    headerCell.Width = Unit.Pixel(25);
                    break;
                case 10://Price
                    headerCell.Width = Unit.Pixel(60);
                    break;
                case 11://LastChangeDate
                    headerCell.Width = Unit.Pixel(75);
                    break;

            }


            //headerCell.Width = col.ItemStyle.Width;
            headerRow.Cells.Add(headerCell);

        }

        tblHeaderTable.Rows.Add(headerRow);
    }

    //Charting...
    private void SetChartStyle(System.Web.UI.DataVisualization.Charting.Chart MyChart, string sChartStyle, int iSeries)
    {

        switch (sChartStyle)
        {
            case "Area":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Area;
                break;
            case "Bar":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Bar;
                break;
            case "BoxPlot":
                MyChart.Series[iSeries].ChartType = SeriesChartType.BoxPlot;
                break;
            case "Bubble":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Bubble;
                break;
            case "CandleStick":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Candlestick;
                break;
            case "Column":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Column;
                break;
            case "DoughNut":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Doughnut;
                break;
            case "ErrorBar":
                MyChart.Series[iSeries].ChartType = SeriesChartType.ErrorBar;
                break;
            case "FastLine":
                MyChart.Series[iSeries].ChartType = SeriesChartType.FastLine;
                break;
            case "FastPoint":
                MyChart.Series[iSeries].ChartType = SeriesChartType.FastPoint;
                break;
            case "Funnel":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Funnel;
                break;
            case "Line":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Line;
                break;
            case "Pie":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Pie;
                break;
            case "Point":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Point;
                break;
            case "Polar":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Polar;
                break;
            case "Pyramid":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Pyramid;
                break;
            case "Radar":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Radar;
                break;
            case "Range":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Range;
                break;
            case "RangeBar":
                MyChart.Series[iSeries].ChartType = SeriesChartType.RangeBar;
                break;
            case "RangeColumn":
                MyChart.Series[iSeries].ChartType = SeriesChartType.RangeColumn;
                break;
            case "Spline":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Spline;
                break;
            case "SplineArea":
                MyChart.Series[iSeries].ChartType = SeriesChartType.SplineArea;
                break;
            case "SplineRange":
                MyChart.Series[iSeries].ChartType = SeriesChartType.SplineRange;
                break;
            case "StackedArea":
                MyChart.Series[iSeries].ChartType = SeriesChartType.StackedArea;
                break;
            case "StackedArea100":
                MyChart.Series[iSeries].ChartType = SeriesChartType.StackedArea100;
                break;
            case "StackedBar":
                MyChart.Series[iSeries].ChartType = SeriesChartType.StackedBar;
                break;
            case "StackedBar100":
                MyChart.Series[iSeries].ChartType = SeriesChartType.StackedBar100;
                break;
            case "StackedColumn":
                MyChart.Series[iSeries].ChartType = SeriesChartType.StackedColumn;
                break;
            case "StackedColumn100":
                MyChart.Series[iSeries].ChartType = SeriesChartType.StackedColumn100;
                break;
            case "StepLine":
                MyChart.Series[iSeries].ChartType = SeriesChartType.StepLine;
                break;
            case "Stock":
                MyChart.Series[iSeries].ChartType = SeriesChartType.Stock;
                break;
            case "ThreeLineBreak":
                MyChart.Series[iSeries].ChartType = SeriesChartType.ThreeLineBreak;
                break;
            default:
                break;
        }
    }
    public void CreateChartPriceMarginHistory(System.Web.UI.DataVisualization.Charting.Chart MyChart, bool b3D, DataTable dt)
    {
        try
        {

            if (b3D)
            {
                MyChart.ChartAreas["ChartArea1"].Area3DStyle.PointGapDepth = 0;
                MyChart.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 5;
                MyChart.ChartAreas["ChartArea1"].Area3DStyle.Perspective = 10;
                MyChart.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 15;
                MyChart.ChartAreas["ChartArea1"].Area3DStyle.IsRightAngleAxes = false;
                MyChart.ChartAreas["ChartArea1"].Area3DStyle.WallWidth = 0;
                MyChart.ChartAreas["ChartArea1"].Area3DStyle.IsClustered = false;
                MyChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
            }
            else
            {
                MyChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
            }

            Font LabelFont = new Font("Arial", 12, FontStyle.Bold);

            /////////////////////////////////////////////////////
            MyChart.Series[0].YAxisType = AxisType.Primary;
            MyChart.Series[1].YAxisType = AxisType.Secondary;

            MyChart.ChartAreas[0].AxisY2.LineColor = Color.Transparent;
            MyChart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
            MyChart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
            MyChart.ChartAreas[0].AxisY2.IsStartedFromZero = MyChart.ChartAreas[0].AxisY.IsStartedFromZero;

            ///////////////////////////////////////////////////            


            MyChart.Series["Price"].XValueMember = "TrnDate";
            MyChart.Series["Margin"].XValueMember = "TrnDate";
            MyChart.Series["Amount"].XValueMember = "TrnDate";


            MyChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "yyyy";//Format X Label as Year... 
            MyChart.ChartAreas["ChartArea1"].AxisX.Interval = 350;

            //MyChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "MMM  yyyy";//Format X Label as Month/Year... 
            //MyChart.ChartAreas["ChartArea1"].AxisX.Interval = 75;
            //if (dt.Rows.Count > 25)
            //{
            //    MyChart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
            //}
            //else
            //{
            //    MyChart.ChartAreas[0].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;
            //}


            MyChart.ChartAreas["ChartArea1"].AxisX.Title = "Years";

            MyChart.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "$#,##0";
            MyChart.ChartAreas["ChartArea1"].AxisY2.LabelStyle.Format = "{d}%";

            MyChart.ChartAreas["ChartArea1"].AxisY.LabelStyle.Font = LabelFont;
            MyChart.ChartAreas["ChartArea1"].AxisY2.LabelStyle.Font = LabelFont;
            MyChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = LabelFont;

            MyChart.ChartAreas["ChartArea1"].AxisY.TitleFont = LabelFont;
            MyChart.ChartAreas["ChartArea1"].AxisY2.TitleFont = LabelFont;
            MyChart.ChartAreas["ChartArea1"].AxisX.TitleFont = LabelFont;
            MyChart.Legends[0].Font = LabelFont;


            //Set data points...
            MyChart.Series["Price"].YValueMembers = "Price";
            MyChart.Series["Margin"].YValueMembers = "Margin";
            MyChart.Series["Amount"].YValueMembers = "Amount";


            MyChart.Series["Price"].ToolTip = "#VALX{MMM yyyy}"; //this is for X Axis...
            //MyChart.Series["Series2"].ToolTip = "#AXISLABEL   Accounts Worked: #VAL{g}"; //g for general 
            //MyChart.Series["Series3"].ToolTip = "#AXISLABEL   Percentage of Accounts Worked: #VAL{g}"; //g for general 
            MyChart.Series["Margin"].IsValueShownAsLabel = true;
            MyChart.Series["Price"].IsValueShownAsLabel = true;
            MyChart.Series["Amount"].IsValueShownAsLabel = true;
            MyChart.Series["Price"].LabelFormat = "{c}";
            MyChart.Series["Amount"].LabelFormat = "$#,##0";
            MyChart.Series["Margin"].LabelFormat = "{d}%";



            MyChart.Series["Price"].Font = LabelFont;
            MyChart.Series["Amount"].Font = LabelFont;
            MyChart.Series["Margin"].Font = LabelFont;

            MyChart.Series["Price"].XValueType = ChartValueType.DateTime;
            MyChart.Series["Amount"].XValueType = ChartValueType.DateTime;
            MyChart.Series["Margin"].XValueType = ChartValueType.DateTime;

            MyChart.Series["Price"].YValueType = ChartValueType.Double;
            MyChart.Series["Amount"].YValueType = ChartValueType.Double;
            MyChart.Series["Margin"].YValueType = ChartValueType.Double;

            DateTime axisMinimum;
            DateTime axisMaximum;
            axisMinimum = DateTime.Now.AddYears(-3);
            axisMaximum = DateTime.Now;
            MyChart.ChartAreas[0].AxisX.Minimum = axisMinimum.ToOADate();
            MyChart.ChartAreas[0].AxisX.Maximum = axisMaximum.ToOADate();
        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.ToString());
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
            if (!SharedFunctions.IsAccessGranted(iAdminID, sText))
            {
                Response.Redirect("Default.aspx");
            }
        }

        if (!Page.IsPostBack)
        {

            //LoadCustomerForCharts(ddlCustomersChart2);


            string sStockCode = "";
            string sStartDate = "";
            string sEndDate = "";
            if (Request.QueryString["sc"] != null)
            {
                sStockCode = Request.QueryString["sc"].ToString();
            }
            if (Request.QueryString["start"] != null)
            {
                sStartDate = Request.QueryString["start"].ToString();
            }
            if (Request.QueryString["end"] != null)
            {
                sEndDate = Request.QueryString["end"].ToString();
            }

            RunReport(sStartDate, sEndDate, sStockCode);

        }//End postback

    }
    protected void gvReportCondensed_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int idx = 0;//bound in html code...
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        LinkButton lbnName;
        Label lblCustomer;
        Label lblStockCode;
        Label lblDescription;
        string sStockCode = "";
        string sStartDate = "";
        string sEndDate = "";
        if (Request.QueryString["start"] != null)
        {
            sStartDate = Request.QueryString["start"].ToString();
        }
        if (Request.QueryString["end"] != null)
        {
            sEndDate = Request.QueryString["end"].ToString();
        }
        switch (e.CommandName)
        {
            case "View":
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...                 
                lbnName = (LinkButton)gvReportCondensed.Rows[idx].FindControl("lbnName");
                lblCustomer = (Label)gvReportCondensed.Rows[idx].FindControl("lblCustomer");
                lblStockCode = (Label)gvReportCondensed.Rows[idx].FindControl("lblStockCode");
                lblDescription = (Label)gvReportCondensed.Rows[idx].FindControl("lblDescription");
                Session["Customer"] = lblCustomer.Text.Trim();
                GetChart1(lblCustomer.Text.Trim(), lblStockCode.Text.Trim());
                lblCustomerName.Text = lbnName.Text;
                lblStockCodeLabel.Text = lblStockCode.Text.Trim() + " - " + lblDescription.Text;
                sStockCode = lblStockCode.Text.Trim();

                RunReport2(sStartDate, sEndDate, sStockCode, lblCustomer.Text.Trim());
                break;
        }

    }
    protected void gvReportCondensed_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcCostValue = 0;
        decimal dcAmount = 0;//InvoiceValue...
        int iRoleID = 0;
        iRoleID = Convert.ToInt32(Session["RoleID"]);
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //LinkButton lbnStockCode = (LinkButton)e.Row.FindControl("lbnStockCode");
            Label lblTrnDate = (Label)e.Row.FindControl("lblTrnDate");
            Label lblQty = (Label)e.Row.FindControl("lblQty");
            Label lblAmount = (Label)e.Row.FindControl("lblAmount");
            Label lblCostValue = (Label)e.Row.FindControl("lblCostValue");
            Label lblMargin = (Label)e.Row.FindControl("lblMargin");
            if (lblQty.Text != "")
            {
                lblQty.Text = Convert.ToDecimal(lblQty.Text).ToString("0.0");
            }
            ////1   Admin
            ////2   Supervisor
            ////3   Manager
            ////4   Employee
            ////5   Customer Service Mgr
            ////6   Customer Service Employee
            ////7   Special1
            if (lblMargin.Text != "")
            {

                switch (iRoleID)
                {
                    case 1://Admin...
                        lblMargin.Text = Convert.ToDecimal(lblMargin.Text).ToString("0.0");
                        break;
                    case 2://Supervisor...
                        lblMargin.Text = Convert.ToDecimal(lblMargin.Text).ToString("0.0");
                        break;
                    default:
                        lblMargin.Text = "--";
                        break;
                }

            }
            if (lblAmount.Text != "")
            {
                dcAmount = Convert.ToDecimal(lblAmount.Text.Trim());
                dcTotalAmount += dcAmount;
                lblAmount.Text = Convert.ToDecimal(lblAmount.Text.Trim()).ToString("#,0.00");
            }
            if (lblCostValue.Text != "")
            {
                dcCostValue = Convert.ToDecimal(lblCostValue.Text.Trim());
                dcCostValueTotal += dcCostValue;
                lblCostValue.Text = Convert.ToDecimal(lblCostValue.Text.Trim()).ToString("0.00");
            }


        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblAmountTotal = (Label)e.Row.FindControl("lblAmountTotal");
            Label lblMarginAvgTotal = (Label)e.Row.FindControl("lblMarginAvgTotal");
            decimal dcWeightedAvg = 0;

            //100 * ((s.InvoiceValue - s.CostValue) /  (CASE WHEN s.InvoiceValue = 0 THEN 1 ELSE s.InvoiceValue END))
            dcWeightedAvg = (dcTotalAmount - dcCostValueTotal);
            if (dcTotalAmount == 0)
            {
                dcTotalAmount = 1;
            }
            dcWeightedAvg = dcWeightedAvg / dcTotalAmount;
            dcWeightedAvg = dcWeightedAvg * 100;
            lblMarginAvgTotal.Text = dcWeightedAvg.ToString("#,0.0") + "%";

            lblAmountTotal.Text = "$" + dcTotalAmount.ToString("#,0.00");
        }

    }
    protected void gvAlsoBought_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void gvAlsoBought_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcCostValue = 0;
        decimal dcAmount = 0;//InvoiceValue...

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblTrnDate = (Label)e.Row.FindControl("lblTrnDate");
            Label lblLastChangeDate = (Label)e.Row.FindControl("lblLastChangeDate");
            Label lblQty = (Label)e.Row.FindControl("lblQty");
            Label lblAmount = (Label)e.Row.FindControl("lblAmount");
            Label lblCostValue = (Label)e.Row.FindControl("lblCostValue");
            Label lblMargin = (Label)e.Row.FindControl("lblMargin");
            if (lblQty.Text != "")
            {
                lblQty.Text = Convert.ToDecimal(lblQty.Text).ToString("0.0");
            }
            if (lblMargin.Text != "")
            {
                lblMargin.Text = Convert.ToDecimal(lblMargin.Text).ToString("0.0");
            }
            if (lblAmount.Text != "")
            {
                dcAmount = Convert.ToDecimal(lblAmount.Text.Trim());
                dcTotalAmount += dcAmount;
                lblAmount.Text = Convert.ToDecimal(lblAmount.Text.Trim()).ToString("#,0.00");
            }
            if (lblCostValue.Text != "")
            {
                dcCostValue = Convert.ToDecimal(lblCostValue.Text.Trim());
                dcCostValueTotal += dcCostValue;
                lblCostValue.Text = Convert.ToDecimal(lblCostValue.Text.Trim()).ToString("0.00");
            }
            if (lblLastChangeDate.Text != "")
            {
                lblLastChangeDate.Text = Convert.ToDateTime(lblLastChangeDate.Text).ToShortDateString();
            }


        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblAmountTotal = (Label)e.Row.FindControl("lblAmountTotal");
            Label lblMarginAvgTotal = (Label)e.Row.FindControl("lblMarginAvgTotal");
            decimal dcWeightedAvg = 0;

            //100 * ((s.InvoiceValue - s.CostValue) /  (CASE WHEN s.InvoiceValue = 0 THEN 1 ELSE s.InvoiceValue END))
            dcWeightedAvg = (dcTotalAmount - dcCostValueTotal);
            if (dcTotalAmount == 0)
            {
                dcTotalAmount = 1;
            }
            dcWeightedAvg = dcWeightedAvg / dcTotalAmount;
            dcWeightedAvg = dcWeightedAvg * 100;
            lblMarginAvgTotal.Text = dcWeightedAvg.ToString("#,0.0") + "%";

            lblAmountTotal.Text = "$" + dcTotalAmount.ToString("#,0.00");
        }
    }
    protected void gvAlsoBought_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();

        DataTable dt = (DataTable)Session["dtAlsoBought"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvAlsoBought.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvAlsoBought.DataSource = m_DataView;
            gvAlsoBought.DataBind();
            gvAlsoBought.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
        if (Session["Customer"] != null && Request.QueryString["sc"] != null)
        {
            GetChart1(Session["Customer"].ToString(), Request.QueryString["sc"].ToString());
        }
    }
    //Chart...
    protected void chk3D_CheckedChanged(object sender, EventArgs e)
    {
        if (Session["Customer"] != null && Request.QueryString["sc"] != null)
        {
            GetChart1(Session["Customer"].ToString(), Request.QueryString["sc"].ToString());
        }

    }
    protected void imgExportExcel1_Click(object sender, ImageClickEventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";

        dt = (DataTable)Session["dtCustomerReport"];

        dt.TableName = "dtCustomerReport";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "CustomerReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void imgExportExcel2_Click(object sender, ImageClickEventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";

        dt = (DataTable)Session["dtAlsoBought"];

        dt.TableName = "dtAlsoBought";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "AlsoBoughtReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }

    //FrozenHeaderRow...
    // LinkButtons are used to dynamically create the links necessary
    // for paging.
    protected void HeaderLink1_Click(object sender, System.EventArgs e)
    {
        LinkButton lnkHeader = (LinkButton)sender;
        SortDirection direction = SortDirection.Ascending;

        // the CommandArgument of each linkbutton contains the sortexpression
        // for the column that was clicked.
        if (gvReportCondensed.SortExpression == lnkHeader.CommandArgument)
        {
            if (gvReportCondensed.SortDirection == SortDirection.Ascending)
            {
                direction = SortDirection.Descending;
            }

        }

        gvReportCondensed.Sort(lnkHeader.CommandArgument, direction);
    }

    // create the table for the fixed header in Init
    protected void Page_Init(object sender, EventArgs e)
    {
        SetupHeaderTable1();
    }


    #endregion

    #region WebMethods
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListStockCodes(string prefixText, int count, string contextKey)
    {//...
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {

            // Your LINQ to SQL query goes here 
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            string[] list = db.ArSalesMove.Where(w => w.StockCode != null).OrderBy(w => w.StockCode).Select(w => (w.StockCode.Trim())).Distinct().ToArray();
            string[] lResult = null;
            try
            {
                lResult = (from d in list where d.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select d).Take(count).ToArray();
            }
            catch (Exception ex)
            {
                lResult = new string[] { "None found" };
                Debug.WriteLine(ex.ToString());

            }
            return lResult;
        }
    }

    #endregion
}