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

public partial class IngredientCostHistory : System.Web.UI.Page
{
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
    private void LoadStockCodes(ListBox lb, string sSearch)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lb.Items.Clear();
            var query = (from m in db.InvMovements
                         where
                           (
                            m.StockCode.Trim().ToUpper() == sSearch.ToUpper() ||
                            m.InvMaster.Description.Trim().ToUpper().Contains(sSearch.ToUpper())
                           )
                         group new { m, m.InvMaster } by new
                         {
                             m.StockCode,
                             m.InvMaster.Description
                         } into g
                         orderby
                           g.Key.StockCode
                         select new
                         {
                             g.Key.StockCode,
                             g.Key.Description
                         });
            if (query.Count() > 0)
            {
                foreach (var a in query)
                {
                    lb.Items.Add(new ListItem(a.StockCode + " - " + a.Description, a.StockCode));
                }
            }
            else
            {
                lb.Items.Insert(0, new ListItem("No Stock Codes found for Input", "0"));
            }
        }
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
    public void CreateChartPriceMarginHistory(System.Web.UI.DataVisualization.Charting.Chart MyChart, bool b3D, DataTable dt, bool bLabelVisible)
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

            Font LabelFont = new Font("Arial", 9, FontStyle.Bold);

            /////////////////////////////////////////////////////
            MyChart.Series[0].YAxisType = AxisType.Primary;
            ///////////////////////////////////////////////////      

            MyChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "MMM yyyy";//Format X Label as Year... 
            MyChart.ChartAreas["ChartArea1"].AxisX.Interval = 1;
            MyChart.ChartAreas["ChartArea1"].AxisX.Title = "Date Range";
            MyChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Font = LabelFont;
            MyChart.ChartAreas["ChartArea1"].AxisX.TitleFont = LabelFont;
            MyChart.ChartAreas["ChartArea1"].AxisX.IntervalType = DateTimeIntervalType.Months;

            MyChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Angle = 90;
            MyChart.ChartAreas["ChartArea1"].AxisX.IsLabelAutoFit = false;

            MyChart.ChartAreas["ChartArea1"].AxisY.Title = "Unit Cost";
            //MyChart.ChartAreas["ChartArea1"].AxisY.Interval = 1;
            MyChart.ChartAreas["ChartArea1"].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            MyChart.ChartAreas["ChartArea1"].AxisY.LabelStyle.Format = "$#,0.00";
            MyChart.ChartAreas["ChartArea1"].AxisY.LabelStyle.Font = LabelFont;
            MyChart.ChartAreas["ChartArea1"].AxisY.TitleFont = LabelFont;

            //MyChart.ChartAreas["ChartArea1"].AxisY2.Title = "Last Cost";
            ////MyChart.ChartAreas["ChartArea1"].AxisY.Interval = 1;
            //MyChart.ChartAreas["ChartArea1"].AxisY2.IntervalAutoMode = IntervalAutoMode.VariableCount;
            //MyChart.ChartAreas["ChartArea1"].AxisY2.LabelStyle.Format = "$#,0.00";
            //MyChart.ChartAreas["ChartArea1"].AxisY2.LabelStyle.Font = LabelFont;
            //MyChart.ChartAreas["ChartArea1"].AxisY2.TitleFont = LabelFont;

            MyChart.Legends[0].Font = LabelFont;


            //Set data points...             
            MyChart.Series["UnitCost"].YValueMembers = "UnitCost";
            MyChart.Series["UnitCost"].XValueMember = "EntryDate";
            //MyChart.Series["UnitCost"].ToolTip = "#VALX{MMM yyyy}"; //this is for X Axis...
            MyChart.Series["UnitCost"].ToolTip = "#VALY{c}" + "/ #VALX{MMM dd yyyy}"; //this is for X Axis...

            MyChart.Series["LastCost"].YValueMembers = "LastCost";
            MyChart.Series["LastCost"].XValueMember = "EntryDate";
            //MyChart.Series["UnitCost"].ToolTip = "#VALX{MMM yyyy}"; //this is for X Axis...
            MyChart.Series["LastCost"].ToolTip = "#VALY{c}" + "/ #VALX{MMM dd yyyy}"; //this is for X Axis...


            if (bLabelVisible)
            {
                MyChart.Series["UnitCost"].IsValueShownAsLabel = true;
                MyChart.Series["LastCost"].IsValueShownAsLabel = true;
            }
            else
            {
                MyChart.Series["UnitCost"].IsValueShownAsLabel = false;
                MyChart.Series["LastCost"].IsValueShownAsLabel = false;
            }

            MyChart.Series["UnitCost"].LabelFormat = "{c}";
            MyChart.Series["UnitCost"].Font = LabelFont;
            MyChart.Series["UnitCost"].XValueType = ChartValueType.DateTime;
            MyChart.Series["UnitCost"].YValueType = ChartValueType.Double;

            MyChart.Series["LastCost"].LabelFormat = "{c}";
            MyChart.Series["LastCost"].Font = LabelFont;
            MyChart.Series["LastCost"].XValueType = ChartValueType.DateTime;
            MyChart.Series["LastCost"].YValueType = ChartValueType.Double;


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
    private void GetChart1(string sStockCode)
    {
        DataTable dt = new DataTable();

        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

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
            lblError.ForeColor = Color.Red;
            return;
        }

        if (ddlPeriod.SelectedIndex == 0)
        {//All Time...
            sSQL = "EXEC spRawIngredientTrendsReport ";
            sSQL += "@StockCode=" + sStockCode;

        }
        else //Dates Supplied...
        {
            sSQL = "EXEC spRawIngredientTrendsReport ";
            sSQL += "@StockCode=" + sStockCode + ",";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate;

        }



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
        bool bShowLabels = false;
        if (chkShowLabels.Checked)
        {
            bShowLabels = true;
        }
        else
        {
            bShowLabels = false;
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
                CreateChartPriceMarginHistory(Chart1, bCht1, dt, bShowLabels);
                Chart1.Visible = true;
                chk3D.Visible = true;
                chkShowLabels.Visible = true;
            }
            else
            {
                lblError.Text = "No results found!!";
                lblError.ForeColor = Color.Red;
                Chart1.Visible = false;
                chk3D.Visible = false;
                chkShowLabels.Visible = false;
                Chart1.DataSource = null;
                gv1.DataBind();
            }
        }
    }


    #endregion

    #region Functions
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
            if (Request.QueryString["stockcode"] != null)
            {
                txtSearch.Text = Request.QueryString["stockcode"].ToString();
                btnSearch_Click(btnSearch, null);
                lbStockCode.SelectedIndex = 0;
                lbStockCode_SelectedIndexChanged(lbStockCode, null);
            }


        }//End postback

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
            case 6://2 Years
                txtStartDate.Text = DateTime.Now.AddMonths(-24).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 7://3 Years mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-36).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
        }

        if (lbStockCode.Items.Count > 0)
        {
            lbStockCode.SelectedIndex = -1;
        }
        Chart1.Visible = false;
        gv1.DataSource = null;
        gv1.DataBind();
    }
    //Chart...
    protected void chk3D_CheckedChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        string sStockCode = "";
        string sMsg = "";
        if (lbStockCode.SelectedIndex != -1)
        {//Not null then add quotes...
            sStockCode = "'" + lbStockCode.SelectedValue.Trim() + "'";
            GetChart1(sStockCode);
        }
        else
        {
            sMsg += "**StockCode is required!!";
            lblError.Text = sMsg;
            return;
        }
    }
    protected void chkShowLabels_CheckedChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        string sStockCode = "";
        string sMsg = "";
        if (lbStockCode.SelectedIndex != -1)
        {//Not null then add quotes...
            sStockCode = "'" + lbStockCode.SelectedValue.Trim() + "'";
            GetChart1(sStockCode);
        }
        else
        {
            sMsg += "**StockCode is required!!";
            lblError.Text = sMsg;
            return;
        }
    }
    protected void btnPreviewChart1_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        string sStockCode = "";
        string sMsg = "";
        if (lbStockCode.SelectedIndex != -1)
        {//Not null then add quotes...
            sStockCode = "'" + lbStockCode.SelectedValue.Trim() + "'";
            GetChart1(sStockCode);
        }
        else
        {
            sMsg += "**StockCode is required!!";
            lblError.Text = sMsg;
            return;
        }
    }
    protected void txtStockCodeChartIngredient_TextChanged(object sender, EventArgs e)
    {
        if (lbStockCode.Items.Count > 0)
        {
            lbStockCode.SelectedIndex = -1;
        }
        Chart1.Visible = false;
        gv1.DataSource = null;
        gv1.DataBind();
        if (txtSearch.Text.Trim() != "")
        {
            //Search...
            LoadStockCodes(lbStockCode, txtSearch.Text.Trim());
        }
        else
        {
            lbStockCode.Items.Clear();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (lbStockCode.Items.Count > 0)
        {
            lbStockCode.SelectedIndex = -1;
        }
        Chart1.Visible = false;
        gv1.DataSource = null;
        gv1.DataBind();
        if (txtSearch.Text.Trim() != "")
        {
            //Search...
            LoadStockCodes(lbStockCode, txtSearch.Text.Trim());
        }
        else
        {
            lbStockCode.Items.Clear();
        }
    }
    protected void lbStockCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        string sStockCode = "";
        string sMsg = "";
        if (lbStockCode.SelectedIndex != -1)
        {//Not null then add quotes...
            sStockCode = "'" + lbStockCode.SelectedValue.Trim() + "'";
            GetChart1(sStockCode);
        }
        else
        {
            sMsg += "**StockCode is required!!";
            lblError.Text = sMsg;
            return;
        }
    }
    protected void imgExportExcel_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";

        if (Session["dtChart1"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtChart1"];

        try
        {
            dt.Columns.Remove("ID");
        }
        catch (Exception)
        {

            //ignore...
        }

        dt.TableName = "dtChart1";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "IngredientCostHistory_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void gv1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDate = (Label)e.Row.FindControl("lblDate");
            if (lblDate.Text != "")
            {
                lblDate.Text = Convert.ToDateTime(lblDate.Text).ToShortDateString();
            }
            Label lblTrnQty = (Label)e.Row.FindControl("lblTrnQty");
            if (lblTrnQty.Text != "")
            {
                lblTrnQty.Text = Convert.ToDecimal(lblTrnQty.Text).ToString("0");
            }
        }

    }
    protected void chkShowChart_CheckedChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        string sStockCode = "";
        string sMsg = "";
        if (lbStockCode.SelectedIndex != -1)
        {//Not null then add quotes...
            sStockCode = "'" + lbStockCode.SelectedValue.Trim() + "'";
            GetChart1(sStockCode);
        }
        else
        {
            sMsg += "**StockCode is required!!";
            lblError.Text = sMsg;
            return;
        }
        if (chkShowChart.Checked)
        {
            pnlChart.Visible = true;
        }
        else
        {
            pnlChart.Visible = false;
        }  
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
            string[] list = db.InvMovements.Where(w => w.StockCode != null).OrderBy(w => w.StockCode).Select(w => (w.StockCode.Trim())).Distinct().ToArray();
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