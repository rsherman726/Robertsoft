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

public partial class PriceMarginHistory : System.Web.UI.Page
{

    #region Subs
    private void LoadParentStockCodesIngredients(ListBox lb, string sIngredientStockCode)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lb.Items.Clear();
        var query = (from w in db.InvLookupTableIngredientsRSS
                     where w.MStockCode.Trim() == sIngredientStockCode
                     group w by new
                     {
                         w.FinishedStockCode,
                         w.FinishedDescription
                     } into g
                     orderby
                       g.Key.FinishedStockCode
                     select new
                     {
                         g.Key.FinishedStockCode,
                         g.Key.FinishedDescription
                     });
        if (query.Count() > 0)
        {
            foreach (var a in query)
            {
                lb.Items.Add(new ListItem(a.FinishedStockCode + " - " + a.FinishedDescription, a.FinishedStockCode));
            }
        }
        else
        {
            lb.Items.Insert(0, new ListItem("No Finished Stock Codes found for this Ingredient Stock Code", "0"));
        }
        lblStockCodeList.Text = "StockCodes: " + lbParentStockCode.Items.Count.ToString();
    }
    private void LoadParentStockCodesIngredients(ListBox lb)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lb.Items.Clear();
        var query = (from w in db.InvLookupTableIngredientsRSS
                     group w by new
                     {
                         w.FinishedStockCode,
                         w.FinishedDescription
                     } into g
                     orderby
                       g.Key.FinishedStockCode
                     select new
                     {
                         g.Key.FinishedStockCode,
                         g.Key.FinishedDescription
                     });
        if (query.Count() > 0)
        {
            foreach (var a in query)
            {
                lb.Items.Add(new ListItem(a.FinishedStockCode + " - " + a.FinishedDescription, a.FinishedStockCode));
            }
        }
        else
        {
            lb.Items.Insert(0, new ListItem("No Finished Stock Codes found for this Ingredient Stock Code", "0"));
        }
        lblStockCodeList.Text = "StockCodes: " + lbParentStockCode.Items.Count.ToString();
    }
    private void LoadParentStockCodesComponents(ListBox lb, string sComponentStockCode)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lb.Items.Clear();
        var query = (from w in db.InvLookupTableComponentsRSS
                     where w.MStockCode.Trim() == sComponentStockCode
                     group w by new
                     {
                         w.FinishedStockCode,
                         w.FinishedDescription
                     } into g
                     orderby
                       g.Key.FinishedStockCode
                     select new
                     {
                         g.Key.FinishedStockCode,
                         g.Key.FinishedDescription
                     });
        if (query.Count() > 0)
        {
            foreach (var a in query)
            {
                lb.Items.Add(new ListItem(a.FinishedStockCode + " - " + a.FinishedDescription, a.FinishedStockCode));
            }
        }
        else
        {
            lb.Items.Insert(0, new ListItem("No Finished Stock Codes found for this Component Stock Code", "0"));
        }
        lblStockCodeList.Text = "StockCodes: " + lbParentStockCode.Items.Count.ToString();
    }
    private void LoadParentStockCodesComponents(ListBox lb)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lb.Items.Clear();
        var query = (from w in db.InvLookupTableComponentsRSS
                     group w by new
                     {
                         w.FinishedStockCode,
                         w.FinishedDescription
                     } into g
                     orderby
                       g.Key.FinishedStockCode
                     select new
                     {
                         g.Key.FinishedStockCode,
                         g.Key.FinishedDescription
                     });
        if (query.Count() > 0)
        {
            foreach (var a in query)
            {
                lb.Items.Add(new ListItem(a.FinishedStockCode + " - " + a.FinishedDescription, a.FinishedStockCode));
            }
        }
        else
        {
            lb.Items.Insert(0, new ListItem("No Finished Stock Codes found for this Component Stock Code", "0"));
        }
        lblStockCodeList.Text = "StockCodes: " + lbParentStockCode.Items.Count.ToString();
    }
    private void LoadStockCodesWipMaster()
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lbParentStockCode.Items.Clear();
        var query = (from w in db.WipMaster
                     group w by new
                     {
                         w.StockCode,
                         w.StockDescription
                     } into g
                     orderby
                       g.Key.StockCode
                     select new
                     {
                         g.Key.StockCode,
                         g.Key.StockDescription
                     });
        foreach (var a in query)
        {
            lbParentStockCode.Items.Add(new ListItem(a.StockCode + " - " + a.StockDescription, a.StockCode));
        }
        lblStockCodeList.Text = "StockCodes: " + lbParentStockCode.Items.Count.ToString();
    }
    private void LoadStockCodesInvMaster(ListBox lb)
    {

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lb.Items.Clear();

            var queryUnfiltered = (
                            from im in db.InvMaster
                            where
                              !
                                (from BomStructure in db.BomStructure
                                 group BomStructure by new
                                 {
                                     BomStructure.Component
                                 } into g
                                 select new
                                 {
                                     g.Key.Component
                                 }).Contains(new { Component = im.StockCode }) &&
                              !(new string[] { "L" }).Contains((im.StockCode.Substring(1 - 1, 1)).ToUpper()) &&
                              !(new string[] { "BCL" }).Contains((im.StockCode.Substring(1 - 1, 3)).ToUpper()) &&
                              !(new string[] { "BAGS" }).Contains((im.StockCode.Substring(1 - 1, 4)).ToUpper())
                            orderby im.StockCode
                            group im by new
                            {
                                im.StockCode,
                                im.Description
                            } into g
                            select new
                            {
                                StockCode = g.Key.StockCode,
                                Description = g.Key.Description
                            });

            DataTable dt = SharedFunctions.ToDataTable(db, queryUnfiltered);
            int result_ignored;
            var query1 = (from a in (
                            from im in dt.AsEnumerable()
                            where int.TryParse(im["StockCode"].ToString(), out result_ignored)
                            select new
                            {
                                StockCode = im["StockCode"].ToString(),
                                Description = im["Description"].ToString()
                            })
                          where !(Convert.ToInt32(a.StockCode) >= 600000 && Convert.ToInt32(a.StockCode) <= 799999)
                          && (Convert.ToInt32(a.StockCode) > 100000)
                          orderby a.StockCode
                          select new
                          {
                              a.StockCode,
                              a.Description
                          });


            var query2 = (from im in db.InvMaster
                          where
                            !
                              (from BomStructure in db.BomStructure
                               group BomStructure by new
                               {
                                   BomStructure.Component
                               } into g
                               select new
                               {
                                   g.Key.Component
                               }).Contains(new { Component = im.StockCode }) &&
                            (
                            im.StockCode.ToUpper().Contains("A") ||
                            im.StockCode.ToUpper().Contains("B") ||
                            im.StockCode.ToUpper().Contains("C") ||
                            im.StockCode.ToUpper().Contains("D") ||
                            im.StockCode.ToUpper().Contains("E") ||
                            im.StockCode.ToUpper().Contains("F") ||
                            im.StockCode.ToUpper().Contains("G") ||
                            im.StockCode.ToUpper().Contains("H") ||
                            im.StockCode.ToUpper().Contains("I") ||
                            im.StockCode.ToUpper().Contains("J") ||
                            im.StockCode.ToUpper().Contains("K") ||
                            im.StockCode.ToUpper().Contains("L") ||
                            im.StockCode.ToUpper().Contains("M") ||
                            im.StockCode.ToUpper().Contains("N") ||
                            im.StockCode.ToUpper().Contains("O") ||
                            im.StockCode.ToUpper().Contains("P") ||
                            im.StockCode.ToUpper().Contains("Q") ||
                            im.StockCode.ToUpper().Contains("R") ||
                            im.StockCode.ToUpper().Contains("S") ||
                            im.StockCode.ToUpper().Contains("T") ||
                            im.StockCode.ToUpper().Contains("U") ||
                            im.StockCode.ToUpper().Contains("V") ||
                            im.StockCode.ToUpper().Contains("W") ||
                            im.StockCode.ToUpper().Contains("X") ||
                            im.StockCode.ToUpper().Contains("Y") ||
                            im.StockCode.ToUpper().Contains("Z")) &&
                            !(new string[] { "L" }).Contains((im.StockCode.Substring(1 - 1, 1)).ToUpper()) &&
                            !(new string[] { "BCL" }).Contains((im.StockCode.Substring(1 - 1, 3)).ToUpper()) &&
                            !(new string[] { "BAGS" }).Contains((im.StockCode.Substring(1 - 1, 4)).ToUpper())
                          group im by new
                          {
                              im.StockCode,
                              im.Description
                          } into g
                          orderby
                            g.Key.StockCode
                          select new
                          {
                              g.Key.StockCode,
                              g.Key.Description
                          });
            int iCount = query2.Count();
            var Union = query1.Union(query2).OrderBy(p => p.StockCode);//Sort to alpha stockcode in proper order...

            if (dt.Rows.Count > 0)
            {
                foreach (var a in Union)
                {
                    lb.Items.Add(new ListItem(a.StockCode + " - " + a.Description, a.StockCode.ToString()));
                }
            }
            else
            {
                lb.Items.Insert(0, new ListItem("No Finished Stock Codes found for this Ingredient Stock Code", "0"));
            }
            lblStockCodeList.Text = "StockCodes: " + lbParentStockCode.Items.Count.ToString();
        }
    }
    private void LoadStockCodesInvMaster()
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lbParentStockCode.Items.Clear();
        var query = (from w in db.InvMaster
                     group w by new
                     {
                         w.StockCode,
                         w.Description
                     } into g
                     orderby
                       g.Key.StockCode
                     select new
                     {
                         g.Key.StockCode,
                         g.Key.Description
                     });
        foreach (var a in query)
        {
            lbParentStockCode.Items.Add(new ListItem(a.StockCode + " - " + a.Description, a.StockCode));
        }
        lblStockCodeList.Text = "StockCodes: " + lbParentStockCode.Items.Count.ToString();
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

            //if (chkGroupByYearChart1.Checked)
            //{
            MyChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "yyyy";//Format X Label as Year... 
            MyChart.ChartAreas["ChartArea1"].AxisX.Interval = 350;
            // }
            ////else
            ////{
            ////    MyChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "MMM  yyyy";//Format X Label as Month/Year... 
            ////    MyChart.ChartAreas["ChartArea1"].AxisX.Interval = 75;
            ////    if (dt.Rows.Count > 25)
            ////    {
            ////        MyChart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
            ////    }
            ////    else
            ////    {
            ////        MyChart.ChartAreas[0].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;
            ////    }

            ////}

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
    //public void CreateChartSalesMarginHistory(System.Web.UI.DataVisualization.Charting.Chart MyChart, bool b3D, DataTable dt)
    //{
    //    try
    //    {

    //        if (b3D)
    //        {
    //            MyChart.ChartAreas["ChartArea1"].Area3DStyle.PointGapDepth = 0;
    //            MyChart.ChartAreas["ChartArea1"].Area3DStyle.Rotation = 5;
    //            MyChart.ChartAreas["ChartArea1"].Area3DStyle.Perspective = 10;
    //            MyChart.ChartAreas["ChartArea1"].Area3DStyle.Inclination = 15;
    //            MyChart.ChartAreas["ChartArea1"].Area3DStyle.IsRightAngleAxes = false;
    //            MyChart.ChartAreas["ChartArea1"].Area3DStyle.WallWidth = 0;
    //            MyChart.ChartAreas["ChartArea1"].Area3DStyle.IsClustered = false;

    //            MyChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;
    //        }
    //        else
    //        {
    //            MyChart.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = false;
    //        }
    //        /////////////////////////////////////////////////////
    //        MyChart.Series[0].YAxisType = AxisType.Primary;
    //        MyChart.Series[1].YAxisType = AxisType.Secondary;

    //        MyChart.ChartAreas[0].AxisY2.LineColor = Color.Transparent;
    //        MyChart.ChartAreas[0].AxisY2.MajorGrid.Enabled = false;
    //        MyChart.ChartAreas[0].AxisY2.Enabled = AxisEnabled.True;
    //        MyChart.ChartAreas[0].AxisY2.IsStartedFromZero = MyChart.ChartAreas[0].AxisY.IsStartedFromZero;

    //        ///////////////////////////////////////////////////


    //        MyChart.Series["Sales"].XValueMember = "TrnDate";
    //        MyChart.Series["Margin"].XValueMember = "TrnDate";
    //        if (chkGroupByYearChart2.Checked)
    //        {
    //            MyChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "yyyy";//Format X Label as Year... 
    //            MyChart.ChartAreas["ChartArea1"].AxisX.Interval = 350;
    //        }
    //        else
    //        {
    //            MyChart.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "MMM  yyyy";//Format X Label as Month/Year... 
    //            MyChart.ChartAreas["ChartArea1"].AxisX.Interval = 75;
    //            if (dt.Rows.Count > 25)
    //            {
    //                MyChart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
    //            }
    //            else
    //            {
    //                MyChart.ChartAreas[0].AxisX.LabelAutoFitStyle = LabelAutoFitStyles.LabelsAngleStep90;
    //            }
    //        }

    //        MyChart.Series["Sales"].YValueMembers = "Amount";
    //        MyChart.Series["Margin"].YValueMembers = "Margin";


    //        MyChart.Series["Sales"].ToolTip = "#VALX{MMM yyyy}"; //this is for X Axis...
    //        //MyChart.Series["Series2"].ToolTip = "#AXISLABEL   Accounts Worked: #VAL{g}"; //g for general 
    //        //MyChart.Series["Series3"].ToolTip = "#AXISLABEL   Percentage of Accounts Worked: #VAL{g}"; //g for general 

    //        MyChart.Series["Sales"].IsValueShownAsLabel = true;
    //        MyChart.Series["Margin"].IsValueShownAsLabel = true;
    //        MyChart.Series["Sales"].LabelFormat = "{c}";
    //        MyChart.Series["Sales"].XValueType = ChartValueType.DateTime;
    //        MyChart.Series["Margin"].XValueType = ChartValueType.DateTime;
    //        MyChart.Series["Sales"].YValueType = ChartValueType.Double;
    //        MyChart.Series["Margin"].YValueType = ChartValueType.Double;

    //    }
    //    catch (Exception ex)
    //    {

    //        Debug.WriteLine(ex.ToString());
    //    }
    //}
    private void GetChart1(string sStockCode)
    {
        DataTable dt = new DataTable();

        string sYear = "'0'";

        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        sSQL = "EXEC spGetCustomerReportCharts ";
        sSQL += "@StockCode=" + sStockCode + ",";
        sSQL += "@Year=" + sYear;

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
                lblErrorChart1.Text = "No results found!!";
                lblErrorChart1.ForeColor = Color.Red;
                Chart1.Visible = false;
                chk3D.Visible = false;
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
            
             

        }//End postback

    }
    //Chart...
    protected void chk3D_CheckedChanged(object sender, EventArgs e)
    {
        lblErrorChart1.Text = "";
        string sStockCode = "";
        string sMsg = "";
        if (lbParentStockCode.SelectedIndex != -1)
        {//Not null then add quotes...
            sStockCode = "'" + lbParentStockCode.SelectedValue.Trim() + "'";
            GetChart1(sStockCode);
        }
        else
        {
            sMsg += "**StockCode is required!!";
            lblErrorChart1.Text = sMsg;
            return;
        }
    }
    protected void chk3D0_CheckedChanged1(object sender, EventArgs e)
    {
        lblErrorChart1.Text = "";
        string sStockCode = "";
        string sMsg = "";
        if (lbParentStockCode.SelectedIndex != -1)
        {//Not null then add quotes...
            sStockCode = "'" + lbParentStockCode.SelectedValue.Trim() + "'";
            GetChart1(sStockCode);
        }
        else
        {
            sMsg += "**StockCode is required!!";
            lblErrorChart1.Text = sMsg;
            return;
        }
    }
    protected void btnPreviewChart1_Click(object sender, EventArgs e)
    {
        lblErrorChart1.Text = "";
        string sStockCode = "";
        string sMsg = "";
        if (lbParentStockCode.SelectedIndex != -1)
        {//Not null then add quotes...
            sStockCode = "'" + lbParentStockCode.SelectedValue.Trim() + "'";
            GetChart1(sStockCode);
        }
        else
        {
            sMsg += "**StockCode is required!!";
            lblErrorChart1.Text = sMsg;
            return;
        }
    }
    protected void btnPreviewChart2_Click(object sender, EventArgs e)
    {
        lblErrorChart1.Text = "";
        string sStockCode = "";
        string sMsg = "";
        if (lbParentStockCode.SelectedIndex != -1)
        {//Not null then add quotes...
            sStockCode = "'" + lbParentStockCode.SelectedValue.Trim() + "'";
            GetChart1(sStockCode);
        }
        else
        {
            sMsg += "**StockCode is required!!";
            lblErrorChart1.Text = sMsg;
            return;
        }
    }
    protected void txtStockCodeChartIngredient_TextChanged(object sender, EventArgs e)
    {
        if (txtStockCodeChartIngredientComponent.Text.Trim() != "")
        {
            if (rblIngredientComponent.SelectedIndex == 0)//Ingredient...
            {
                lblStockCodeDescIngredientComponent.Text = GetDescriptionFromIngredientStockCode(txtStockCodeChartIngredientComponent.Text.Trim());
                LoadParentStockCodesIngredients(lbParentStockCode, txtStockCodeChartIngredientComponent.Text.Trim());
            }
            else//Component...
            {
                lblStockCodeDescIngredientComponent.Text = GetDescriptionFromIngredientStockCode(txtStockCodeChartIngredientComponent.Text.Trim());
                LoadParentStockCodesComponents(lbParentStockCode, txtStockCodeChartIngredientComponent.Text.Trim());
            }
        }
        else
        {
            lbParentStockCode.Items.Clear();
        }

    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtStockCodeChartIngredientComponent.Text.Trim() != "")
        {
            if (rblIngredientComponent.SelectedIndex == 0)//Ingredient...
            {
                lblStockCodeDescIngredientComponent.Text = GetDescriptionFromIngredientStockCode(txtStockCodeChartIngredientComponent.Text.Trim());
                LoadParentStockCodesIngredients(lbParentStockCode, txtStockCodeChartIngredientComponent.Text.Trim());
            }
            else//Component...
            {
                lblStockCodeDescIngredientComponent.Text = GetDescriptionFromIngredientStockCode(txtStockCodeChartIngredientComponent.Text.Trim());
                LoadParentStockCodesComponents(lbParentStockCode, txtStockCodeChartIngredientComponent.Text.Trim());
            }
        }
        else
        {
            lbParentStockCode.Items.Clear();
        }
    }
    protected void btnLoadAll_Click(object sender, EventArgs e)
    {
        if (rblIngredientComponent.SelectedIndex == 0)//Ingredient...
        {
            LoadParentStockCodesIngredients(lbParentStockCode);
        }
        else//Component...
        {
            LoadParentStockCodesComponents(lbParentStockCode);
        }
    }
    protected void rblSourceOfStockCodes_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblSourceOfStockCodes.SelectedIndex != -1)
        {
            if (rblSourceOfStockCodes.SelectedIndex == 0)
            {//WipMaster...
                LoadStockCodesWipMaster();
            }
            else
            {//InvMaster
                LoadStockCodesInvMaster(lbParentStockCode);
            }
        }

    }
    protected void lbParentStockCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrorChart1.Text = "";
        string sStockCode = "";
        string sMsg = "";
        if (lbParentStockCode.SelectedIndex != -1)
        {//Not null then add quotes...
            sStockCode = "'" + lbParentStockCode.SelectedValue.Trim() + "'";
            GetChart1(sStockCode);
        }
        else
        {
            sMsg += "**StockCode is required!!";
            lblErrorChart1.Text = sMsg;
            return;
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListIngredientStockCodes(string prefixText, int count, string contextKey)
    {//...
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {

            // Your LINQ to SQL query goes here 
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            string[] list = db.InvLookupTableIngredientsRSS.Where(w => w.MStockCode != null).OrderBy(w => w.MStockCode.Trim()).Select(w => (w.MStockCode.Trim())).Distinct().ToArray();
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