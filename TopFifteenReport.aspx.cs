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

public partial class TopFifteenReport : System.Web.UI.Page
{

    decimal dcLineTotal = 0;
    decimal dcGrandTotal = 0;
    decimal dcShortageTotal = 0;
    decimal dcQtyTotal = 0;
    decimal dcPlacedOrdersCountTotal = 0;
    decimal dcPlacedOrdersTotal = 0;
    decimal dcPlacedLinesCountTotal = 0;

    decimal dcCurrentYTDTotal = 0;
    decimal dcLastYTDTotal = 0;
    decimal dcLastYTDMinusOneTotal = 0;

    decimal dcCurrentYTDEndOfMonthTotal = 0;
    decimal dcLastYTDEndOfMonthTotal = 0;
    decimal dcLastYTDMinusOneYearEndOfMonthAmountTotal = 0;

    decimal dcCurrentMTDTotal = 0;
    decimal dcLastYearCurrentMTDTotal = 0;

    decimal dcCurrentMonthTotal = 0;
    decimal dcCurrentMonthCostTotal = 0;
    decimal dcLastYearCurrentMonthTotal = 0;

    decimal dcPreviousMonthTotal = 0;
    decimal dcPreviousMonthCostTotal = 0;
    decimal dcLastYearPreviousMonthTotal = 0;


    decimal dcReadyToInvoiceTotal = 0;
    decimal dcOpenOrdersTotal = 0;

    decimal dcCombinedOpenOrdersAmount = 0;

    decimal dcYTDCostTotal = 0;
    decimal dcLastYTDCostTotal = 0;

    //Goal Pace....
    decimal dcCurrentYTDTotalGoalPace = 0;
    decimal dcLastYTDTotalGoalPace = 0;
    decimal dcLastYTDMinusOneTotalGoalPace = 0;

    decimal dcCurrentMTDTotalGoalPace = 0;
    decimal dcLastYearCurrentMTDTotalGoalPace = 0;

    decimal dcCurrentMonthTotalGoalPace = 0;
    decimal dcCurrentMonthCostTotalGoalPace = 0;
    decimal dcLastYearCurrentMonthTotalGoalPace = 0;

    decimal dcPreviousMonthTotalGoalPace = 0;
    decimal dcPreviousMonthCostTotalGoalPace = 0;
    decimal dcLastYearPreviousMonthTotalGoalPace = 0;

    decimal dcYTDCostTotalGoalPace = 0;
    decimal dcLastYTDCostTotalGoalPace = 0;

    decimal dcYearToYearDiffPercentageWeightedGoalPace = 0;
    decimal dcYearToYearDiffPercentageMinusOneWeightedGoalPace = 0;

    decimal dcYearToYearDiffPercentageEndOfMonthWeightedGoalPace = 0;
    decimal dcYearToYearDiffPercentageEndOfMonthMinusOneWeightedGoalPace = 0;

    decimal dcCurrentYTDAmountPercentageOfTotalSumGoalPace = 0;
    decimal dcLastYTDAmountPercentageOfTotalSumGoalPace = 0;

    decimal dcCurrentMonthWeightedMarginTotalGoalPace = 0;
    decimal dcLastMonthWeightedMarginTotalGoalPace = 0;


    //Thru End of Month..
    decimal dcYTDEndOfMonthCostTotal = 0;
    decimal dcLastYTDEndOfMonthCostTotal = 0;

    /// <summary>
    /// New 1-28-2020
    /// </summary>

    decimal dcYearToYearDiffPercentageWeighted = 0;
    decimal dcYearToYearDiffPercentageMinusOneWeighted = 0;

    decimal dcYearToYearDiffPercentageEndOfMonthWeighted = 0;
    decimal dcYearToYearDiffPercentageEndOfMonthMinusOneWeighted = 0;

    decimal dcCurrentYTDAmountPercentageOfTotalSum = 0;
    decimal dcLastYTDAmountPercentageOfTotalSum = 0;

    decimal dcCurrentYTDEndOfMonthAmountPercentageOfTotalSum = 0;
    decimal dcLastYTDEndOfMonthAmountPercentageOfTotalSum = 0;

    decimal dcCurrentMonthWeightedMarginTotal = 0;
    decimal dcLastMonthWeightedMarginTotal = 0;

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
    private void ExportTopFifteenList()
    {
        DataTable dt = LoadTopFifteenData(rblTopTen.SelectedValue);

        string sLastYearMTD = DateTime.Now.AddYears(-1).ToShortDateString();

        DateTime dtFirstDayOfCurrentYear = new DateTime(DateTime.Now.Year, 1, 1);
        string sFirstDayOfCurrentYear = dtFirstDayOfCurrentYear.ToShortDateString();
        DateTime dtFirstDayOfLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);
        string sFirstDayOfLastYear = dtFirstDayOfLastYear.ToShortDateString();

        DateTime dtFirstDayOfLastYearMinusOne = new DateTime(DateTime.Now.AddYears(-2).Year, 1, 1);
        string sFirstDayOfLastYearMinusOne = dtFirstDayOfLastYearMinusOne.ToShortDateString();


        string sYTD = DateTime.Now.ToShortDateString();
        string sLYTD = DateTime.Now.AddDays(-365).ToShortDateString();
        string sLLYTD = DateTime.Now.AddDays(-730).ToShortDateString();
        string sMTD = DateTime.Now.ToShortDateString();
        DateTime dtFirstDayOfMonthCurrentYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        string sFirstDayOfMonthCurrentYear = dtFirstDayOfMonthCurrentYear.ToShortDateString();

        DateTime dtFirstDayOfLastMonthCurrentYear = dtFirstDayOfMonthCurrentYear.AddMonths(-1);//First Day of Last Month Period!
        string sFirstDayOfLastMonthCurrentYear = dtFirstDayOfLastMonthCurrentYear.ToShortDateString();

        //New 1-8-2020...
        DateTime dtFirstDayOfLastMonth = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
        string sFirstDayOfLastMonth = dtFirstDayOfLastMonth.ToShortDateString();
        DateTime dtLastDayOfLastMonth = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month));
        string sLastDayOfLastMonth = dtLastDayOfLastMonth.ToShortDateString();

        DateTime dtLastDayOfLastMonthCurrentYear = new DateTime(dtFirstDayOfMonthCurrentYear.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month));//Last Day of Last Month period!
        string sLastDayOfLastMonthCurrentYear = dtLastDayOfLastMonthCurrentYear.ToShortDateString();

        DateTime dtFirstDayOfMonthLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1).Month, 1);
        string sFirstDayOfMonthLastYear = dtFirstDayOfMonthLastYear.ToShortDateString();


        DateTime dtFirstDayOfLastMonthLastYear = new DateTime(DateTime.Now.AddMonths(-1).AddYears(-1).Year, DateTime.Now.AddMonths(-1).AddYears(-1).Month, 1);
        string sFirstDayOfLastMonthLastYear = dtFirstDayOfLastMonthLastYear.ToShortDateString();


        DateTime dtLastDayOfMonthCurrentYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        string sLastDayOfMonthCurrentYear = dtLastDayOfMonthCurrentYear.ToShortDateString();

        DateTime dtLastDayOfMonthLastYear = dtLastDayOfMonthCurrentYear.AddYears(-1);
        string sLastDayOfMonthLastYear = dtLastDayOfMonthLastYear.ToShortDateString();

        //Fixed 1-8-2020...


        int iYear = DateTime.Now.Year;
        int iLastMonthsLastYear = DateTime.Now.AddMonths(-1).AddYears(-1).Year;
        int iLastYearsLastMonth = DateTime.Now.AddMonths(-1).Month;
        int iYear1 = DateTime.Now.AddYears(-1).Year;
        int iYear2 = DateTime.Now.AddYears(-2).Year;
        int iMonth1 = DateTime.Now.AddMonths(-1).Month;
        int iLastYearLastMonthDay = DateTime.DaysInMonth(iYear1, iMonth1);

        DateTime dtLastDayOfLastMonthLastYear = new DateTime(iLastMonthsLastYear, iLastYearsLastMonth, iLastYearLastMonthDay);


        string sLastDayOfLastMonthLastYear = dtLastDayOfLastMonthLastYear.ToShortDateString();

        string sLastDayOfLastMonthLastYearMinusOne = dtLastDayOfLastMonthLastYear.AddYears(-1).ToShortDateString();
        DateTime dtLastDayOfCurrentYear = new DateTime(iYear, 12, 31);

        //New 2020...
        DateTime dtLastDayofCurrentYear = dtLastDayOfCurrentYear;
        string sLastDayofCurrentYear = dtLastDayofCurrentYear.ToShortDateString();

        DateTime dtLastDayOfLastYear = dtLastDayOfCurrentYear.AddYears(-1);
        string sLastDayOfLastYear = dtLastDayOfLastYear.ToShortDateString();

        DateTime dtLastDayOfLastYearMinusOne = dtLastDayOfCurrentYear.AddYears(-2);
        string sLastDayOfLastYearMinusOne = dtLastDayOfLastYearMinusOne.ToShortDateString();

        var query1 = (from dtSum in dt.AsEnumerable()
                      select new
                      {
                          Name = dtSum["Name"],

                          LAST_MONTH = dtSum["PreviousMonthAmountD"],
                          LAST_MONTH_LAST_YEAR = dtSum["LastYearPreviousMonthAmountD"],

                          MTD = dtSum["CurrentMTDAmountD"],
                          LAST_YEAR_MTD = dtSum["LastYearCurrentMTDAmountD"],
                          CURRENT_MONTH = dtSum["CurrentMonthAmountD"],
                          LAST_YEAR_CURRENT_MONTH = dtSum["LastYearCurrentMonthAmountD"],

                          YTD = dtSum["CurrentYTDAmountD"],
                          LYTD = dtSum["LastYTDAmountD"],
                          LYTD_MINUS_ONE = dtSum["LastYTDMinusOneAmountD"],
                          DIFF = dtSum["YearToYearDiffPercentage"],
                          DIFF_MINUS_ONE = dtSum["YearToYearDiffPercentageMinusOne"],

                          YTD_MARGIN = dtSum["CurrentYTDMargin"],
                          LYTD_MARGIN = dtSum["LastYTDMargin"],

                          CURRENT_MONTH_MARGIN = dtSum["CurrentMonthMargin"],
                          LAST_MONTH_MARGIN = dtSum["LastMonthMargin"],

                          PERCENTAGE_OF_TOTAL_YTD = dtSum["CurrentYTDAmountPercentageOfTotal"],
                          PERCENTAGE_OF_TOTAL_LYTD = dtSum["LastYTDAmountPercentageOfTotal"],

                          YTD_END_OF_MONTH = dtSum["CurrentYTDEndOfMonthAmountD"],
                          LYTD_END_OF_MONTH = dtSum["LastYTDEndOfMonthAmountD"],
                          LYTD_MINUS_ONE_END_OF_MONTH = dtSum["LastYTDMinusOneYearEndOfMonthAmountD"],
                          DIFF_END_OF_MONTH = dtSum["YearToYearDiffPercentageEndOfMonth"],
                          DIFF_END_OF_MONTH_MINUS_ONE = dtSum["YearToYearDiffPercentageEndOfMonthMinusOne"],

                          YTD_MARGIN_END_OF_MONTH = dtSum["CurrentYTDEndOfMonthMargin"],
                          LYTD_MARGIN_END_OF_MONTH = dtSum["LastYTDEndOfMonthMargin"],
                          PERCENTAGE_OF_TOTAL_YTD_END_OF_MONTH = dtSum["CurrentYTDEndOfMonthAmountPercentageOfTotal"],
                          PERCENTAGE_OF_TOTAL_LYTD_END_OF_MONTH = dtSum["LastYTDEndOfMonthAmountPercentageOfTotal"],

                          OPEN_ORDERS = dtSum["OpenOrdersAmountD"],
                          READY_TO_INVOICE = dtSum["ReadyToInvoiceAmountD"],

                      });
        dt = SharedFunctions.LINQToDataTable(query1);

        //foreach (DataRow Row in dt.Rows)
        //{
        //    foreach (DataColumn c in dt.Columns)
        //    {
        //        Debug.WriteLine(c.ColumnName + ": " + Row[c.ColumnName].ToString());
        //    }
        //    Debug.WriteLine("----------------------------------------------------");
        //}

        try
        {
            foreach (DataColumn col in dt.Columns)
            {
                switch (col.ColumnName)
                {
                    case "LAST_MONTH":
                        col.ColumnName = sFirstDayOfLastMonth + " to " + sLastDayOfLastMonth;//Rename column...
                        break;
                    case "LAST_MONTH_LAST_YEAR":
                        col.ColumnName = "[" + sFirstDayOfLastMonthLastYear + " to " + sLastDayOfLastMonthLastYear + "]";//Rename column...
                        break;
                    case "MTD":
                        col.ColumnName = sFirstDayOfMonthCurrentYear + " to " + sMTD + "";//Rename column...
                        break;
                    case "LAST_YEAR_MTD":
                        col.ColumnName = sFirstDayOfMonthLastYear + " to " + sLastYearMTD;//Rename column...
                        break;
                    case "CURRENT_MONTH":
                        col.ColumnName = sFirstDayOfMonthCurrentYear + " to " + sLastDayOfMonthCurrentYear;//Rename column...
                        break;
                    case "LAST_YEAR_CURRENT_MONTH":
                        col.ColumnName = sFirstDayOfMonthLastYear + " to " + sLastDayOfMonthLastYear;//Rename column...
                        break;
                    case "YTD":
                        col.ColumnName = "[" + sFirstDayOfCurrentYear + " to " + sYTD + "]";//Rename column...
                        break;
                    case "LYTD":
                        col.ColumnName = "[" + sFirstDayOfLastYear + " to " + sLYTD + "]";//Rename column...
                        break;
                    case "LYTD_MINUS_ONE":
                        col.ColumnName = "[" + sFirstDayOfLastYearMinusOne + " to " + sLLYTD + "]";//Rename column...
                        break;
                    case "DIFF":
                        col.ColumnName = DateTime.Now.Year.ToString() + " vs " + DateTime.Now.AddYears(-1).Year.ToString();//Rename column...
                        break;
                    case "DIFF_MINUS_ONE":
                        col.ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " vs " + DateTime.Now.AddYears(-2).Year.ToString();//Rename column...
                        break;
                    case "YTD_MARGIN":
                        col.ColumnName = "[" + sFirstDayOfCurrentYear + " to " + sYTD + " MARGIN" + "]";//Rename column...
                        break;
                    case "LYTD_MARGIN":
                        col.ColumnName = "[" + sFirstDayOfLastYear + " to " + sLYTD + " MARGIN" + "]";//Rename column...
                        break;
                    case "CURRENT_MONTH_MARGIN":
                        col.ColumnName = "[" + sFirstDayOfMonthCurrentYear + " to " + sLastDayOfMonthCurrentYear + " MARGIN" + "]";//Rename column...
                        break;
                    case "LAST_MONTH_MARGIN":
                        col.ColumnName = "[" + sFirstDayOfLastMonthCurrentYear + " to " + sLastDayOfLastMonthCurrentYear + " MARGIN" + "]";//Rename column...
                        break;
                    case "PERCENTAGE_OF_TOTAL_YTD":
                        col.ColumnName = "[" + sFirstDayOfCurrentYear + " to " + sYTD + " % of GT" + "]";//Rename column...
                        break;
                    case "PERCENTAGE_OF_TOTAL_LYTD":
                        col.ColumnName = "[" + sFirstDayOfLastYear + " to " + sLYTD + " % of GT" + "]";//Rename column...
                        break;
                    case "YTD_END_OF_MONTH":
                        col.ColumnName = "[" + sFirstDayOfCurrentYear + " to " + sLastDayOfLastMonthCurrentYear + "]";//Rename column...
                        break;
                    case "LYTD_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfLastYear + " to " + sLastDayOfLastMonthLastYear;//Rename column...
                        break;
                    case "LYTD_MINUS_ONE_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfLastYearMinusOne + " to " + sLastDayOfLastMonthLastYearMinusOne;//Rename column...
                        break;
                    case "DIFF_END_OF_MONTH":
                        col.ColumnName = DateTime.Now.Year.ToString() + " vs " + DateTime.Now.AddYears(-1).Year.ToString() + " Throught End of Last Month";//Rename column...
                        break;
                    case "DIFF_END_OF_MONTH_MINUS_ONE":
                        col.ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " vs " + DateTime.Now.AddYears(-2).Year.ToString() + " Throught End of Last Month";//Rename column...
                        break;
                    case "YTD_MARGIN_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfCurrentYear + " to " + sLastDayofCurrentYear + " MARGIN";//Rename column...
                        break;
                    case "LYTD_MARGIN_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfLastYear + " to " + sLastDayOfLastYear + " MARGIN";//Rename column...
                        break;
                    case "PERCENTAGE_OF_TOTAL_YTD_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfCurrentYear + " to " + sLastDayofCurrentYear + " % of GT";//Rename column...
                        break;
                    case "PERCENTAGE_OF_TOTAL_LYTD_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfLastYear + " to " + sLastDayOfLastYear + " % of GT";//Rename column...
                        break;
                    case "OPEN_ORDERS":
                        col.ColumnName = "OPEN ORDERS";//Rename column...
                        break;
                    case "READY_TO_INVOICE":
                        col.ColumnName = "READY TO INVOICE";//Rename column...
                        break;
                }


            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }

        string sFileName = "TopFifteenReport_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExcelHelper.ToExcel(dt, sFileName, Page.Response);
    }
    private void ExportGoalPaceList()
    {
        DataTable dt = LoadGoalPaceData();

        string sLastYearMTD = DateTime.Now.AddYears(-1).ToShortDateString();

        DateTime dtFirstDayOfCurrentYear = new DateTime(DateTime.Now.Year, 1, 1);
        string sFirstDayOfCurrentYear = dtFirstDayOfCurrentYear.ToShortDateString();
        DateTime dtFirstDayOfLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);
        string sFirstDayOfLastYear = dtFirstDayOfLastYear.ToShortDateString();

        DateTime dtFirstDayOfLastYearMinusOne = new DateTime(DateTime.Now.AddYears(-2).Year, 1, 1);
        string sFirstDayOfLastYearMinusOne = dtFirstDayOfLastYearMinusOne.ToShortDateString();


        string sYTD = DateTime.Now.ToShortDateString();
        string sLYTD = DateTime.Now.AddDays(-365).ToShortDateString();
        string sLLYTD = DateTime.Now.AddDays(-730).ToShortDateString();
        string sMTD = DateTime.Now.ToShortDateString();
        DateTime dtFirstDayOfMonthCurrentYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        string sFirstDayOfMonthCurrentYear = dtFirstDayOfMonthCurrentYear.ToShortDateString();

        DateTime dtFirstDayOfLastMonthCurrentYear = dtFirstDayOfMonthCurrentYear.AddMonths(-1);//First Day of Last Month Period!
        string sFirstDayOfLastMonthCurrentYear = dtFirstDayOfLastMonthCurrentYear.ToShortDateString();

        //New 1-8-2020...
        DateTime dtFirstDayOfLastMonth = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
        string sFirstDayOfLastMonth = dtFirstDayOfLastMonth.ToShortDateString();
        DateTime dtLastDayOfLastMonth = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month));
        string sLastDayOfLastMonth = dtLastDayOfLastMonth.ToShortDateString();

        DateTime dtLastDayOfLastMonthCurrentYear = new DateTime(dtFirstDayOfMonthCurrentYear.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month));//Last Day of Last Month period!
        string sLastDayOfLastMonthCurrentYear = dtLastDayOfLastMonthCurrentYear.ToShortDateString();

        DateTime dtFirstDayOfMonthLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1).Month, 1);
        string sFirstDayOfMonthLastYear = dtFirstDayOfMonthLastYear.ToShortDateString();


        DateTime dtFirstDayOfLastMonthLastYear = new DateTime(DateTime.Now.AddMonths(-1).AddYears(-1).Year, DateTime.Now.AddMonths(-1).AddYears(-1).Month, 1);
        string sFirstDayOfLastMonthLastYear = dtFirstDayOfLastMonthLastYear.ToShortDateString();


        DateTime dtLastDayOfMonthCurrentYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        string sLastDayOfMonthCurrentYear = dtLastDayOfMonthCurrentYear.ToShortDateString();

        DateTime dtLastDayOfMonthLastYear = dtLastDayOfMonthCurrentYear.AddYears(-1);
        string sLastDayOfMonthLastYear = dtLastDayOfMonthLastYear.ToShortDateString();

        //Fixed 1-8-2020...


        int iYear = DateTime.Now.Year;
        int iLastMonthsLastYear = DateTime.Now.AddMonths(-1).AddYears(-1).Year;
        int iLastYearsLastMonth = DateTime.Now.AddMonths(-1).Month;
        int iYear1 = DateTime.Now.AddYears(-1).Year;
        int iYear2 = DateTime.Now.AddYears(-2).Year;
        int iMonth1 = DateTime.Now.AddMonths(-1).Month;
        int iLastYearLastMonthDay = DateTime.DaysInMonth(iYear1, iMonth1);

        DateTime dtLastDayOfLastMonthLastYear = new DateTime(iLastMonthsLastYear, iLastYearsLastMonth, iLastYearLastMonthDay);


        string sLastDayOfLastMonthLastYear = dtLastDayOfLastMonthLastYear.ToShortDateString();

        string sLastDayOfLastMonthLastYearMinusOne = dtLastDayOfLastMonthLastYear.AddYears(-1).ToShortDateString();
        DateTime dtLastDayOfCurrentYear = new DateTime(iYear, 12, 31);

        //New 2020...
        DateTime dtLastDayofCurrentYear = dtLastDayOfCurrentYear;
        string sLastDayofCurrentYear = dtLastDayofCurrentYear.ToShortDateString();

        DateTime dtLastDayOfLastYear = dtLastDayOfCurrentYear.AddYears(-1);
        string sLastDayOfLastYear = dtLastDayOfLastYear.ToShortDateString();

        DateTime dtLastDayOfLastYearMinusOne = dtLastDayOfCurrentYear.AddYears(-2);
        string sLastDayOfLastYearMinusOne = dtLastDayOfLastYearMinusOne.ToShortDateString();

        var query1 = (from dtSum in dt.AsEnumerable()
                      select new
                      {
                          Name = dtSum["Name"],

                          GOAL_PACE_DIFF = dtSum["GoalPaceAmountDiff"],
                          GOAL_PACE_AMT = dtSum["GoalPaceAmount"],

                          LAST_MONTH = dtSum["PreviousMonthAmount"],
                          LAST_MONTH_LAST_YEAR = dtSum["LastYearPreviousMonthAmount"],

                          CURRENT_MONTH = dtSum["CurrentMonthAmount"],
                          LAST_YEAR_CURRENT_MONTH = dtSum["LastYearCurrentMonthAmount"],

                          YTD = dtSum["CurrentYTDAmount"],
                          LYTD = dtSum["LastYTDAmount"],
                          LYTD_MINUS_ONE = dtSum["LastYTDMinusOneAmount"],

                          PERCENTAGE_OF_TOTAL_YTD = dtSum["CurrentYTDAmountPercentageOfTotal"],
                          PERCENTAGE_OF_TOTAL_LYTD = dtSum["LastYTDAmountPercentageOfTotal"],

                          CURRENT_MONTH_MARGIN = dtSum["CurrentMonthMargin"],
                          LAST_MONTH_MARGIN = dtSum["LastMonthMargin"],

                      });
        dt = SharedFunctions.LINQToDataTable(query1);

        try
        {
            foreach (DataColumn col in dt.Columns)
            {
                switch (col.ColumnName)
                {
                    case "LAST_MONTH":
                        col.ColumnName = sFirstDayOfLastMonth + " to " + sLastDayOfLastMonth;//Rename column...
                        break;
                    case "LAST_MONTH_LAST_YEAR":
                        col.ColumnName = "[" + sFirstDayOfLastMonthLastYear + " to " + sLastDayOfLastMonthLastYear + "]";//Rename column...
                        break;
                    case "MTD":
                        col.ColumnName = sFirstDayOfMonthCurrentYear + " to " + sMTD + "";//Rename column...
                        break;
                    case "LAST_YEAR_MTD":
                        col.ColumnName = sFirstDayOfMonthLastYear + " to " + sLastYearMTD;//Rename column...
                        break;
                    case "CURRENT_MONTH":
                        col.ColumnName = sFirstDayOfMonthCurrentYear + " to " + sLastDayOfMonthCurrentYear;//Rename column...
                        break;
                    case "LAST_YEAR_CURRENT_MONTH":
                        col.ColumnName = sFirstDayOfMonthLastYear + " to " + sLastDayOfMonthLastYear;//Rename column...
                        break;
                    case "YTD":
                        col.ColumnName = "[" + sFirstDayOfCurrentYear + " to " + sYTD + "]";//Rename column...
                        break;
                    case "LYTD":
                        col.ColumnName = "[" + sFirstDayOfLastYear + " to " + sLYTD + "]";//Rename column...
                        break;
                    case "LYTD_MINUS_ONE":
                        col.ColumnName = "[" + sFirstDayOfLastYearMinusOne + " to " + sLLYTD + "]";//Rename column...
                        break;
                    case "DIFF":
                        col.ColumnName = DateTime.Now.Year.ToString() + " vs " + DateTime.Now.AddYears(-1).Year.ToString();//Rename column...
                        break;
                    case "DIFF_MINUS_ONE":
                        col.ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " vs " + DateTime.Now.AddYears(-2).Year.ToString();//Rename column...
                        break;
                    case "YTD_MARGIN":
                        col.ColumnName = "[" + sFirstDayOfCurrentYear + " to " + sYTD + " MARGIN" + "]";//Rename column...
                        break;
                    case "LYTD_MARGIN":
                        col.ColumnName = "[" + sFirstDayOfLastYear + " to " + sLYTD + " MARGIN" + "]";//Rename column...
                        break;
                    case "CURRENT_MONTH_MARGIN":
                        col.ColumnName = "[" + sFirstDayOfMonthCurrentYear + " to " + sLastDayOfMonthCurrentYear + " MARGIN" + "]";//Rename column...
                        break;
                    case "LAST_MONTH_MARGIN":
                        col.ColumnName = "[" + sFirstDayOfLastMonthCurrentYear + " to " + sLastDayOfLastMonthCurrentYear + " MARGIN" + "]";//Rename column...
                        break;
                    case "PERCENTAGE_OF_TOTAL_YTD":
                        col.ColumnName = "[" + sFirstDayOfCurrentYear + " to " + sYTD + " % of GT" + "]";//Rename column...
                        break;
                    case "PERCENTAGE_OF_TOTAL_LYTD":
                        col.ColumnName = "[" + sFirstDayOfLastYear + " to " + sLYTD + " % of GT" + "]";//Rename column...
                        break;
                    case "YTD_END_OF_MONTH":
                        col.ColumnName = "[" + sFirstDayOfCurrentYear + " to " + sLastDayOfLastMonthCurrentYear + "]";//Rename column...
                        break;
                    case "LYTD_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfLastYear + " to " + sLastDayOfLastMonthLastYear;//Rename column...
                        break;
                    case "LYTD_MINUS_ONE_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfLastYearMinusOne + " to " + sLastDayOfLastMonthLastYearMinusOne;//Rename column...
                        break;
                    case "DIFF_END_OF_MONTH":
                        col.ColumnName = DateTime.Now.Year.ToString() + " vs " + DateTime.Now.AddYears(-1).Year.ToString() + " Throught End of Last Month";//Rename column...
                        break;
                    case "DIFF_END_OF_MONTH_MINUS_ONE":
                        col.ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " vs " + DateTime.Now.AddYears(-2).Year.ToString() + " Throught End of Last Month";//Rename column...
                        break;
                    case "YTD_MARGIN_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfCurrentYear + " to " + sLastDayofCurrentYear + " MARGIN";//Rename column...
                        break;
                    case "LYTD_MARGIN_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfLastYear + " to " + sLastDayOfLastYear + " MARGIN";//Rename column...
                        break;
                    case "PERCENTAGE_OF_TOTAL_YTD_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfCurrentYear + " to " + sLastDayofCurrentYear + " % of GT";//Rename column...
                        break;
                    case "PERCENTAGE_OF_TOTAL_LYTD_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfLastYear + " to " + sLastDayOfLastYear + " % of GT";//Rename column...
                        break;
                }


            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }

        string sFileName = "GoalPace250kReport_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExcelHelper.ToExcel(dt, sFileName, Page.Response);
    }
    private DataTable LoadTopFifteenData(string sListMode)
    {

        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        sSQL = "EXEC spGetTopFifteenList ";
        sSQL += "@ListMode ='" + sListMode + "'";


        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dtTopFifteen");
        gvTopFifteen.DataSource = dt;
        gvTopFifteen.DataBind();
        Session["dtTopFifteen"] = dt;
        dt.Dispose();

        return dt;


    }
    private DataTable LoadGoalPaceData()
    {

        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        sSQL = "EXEC spGetTopAveragers ";


        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dtGoalPace");

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
            gvGoalPace.DataSource = dt;
            gvGoalPace.DataBind();
            Session["dtGoalPace"] = dt;
            dt.Dispose();
        }
        return dt;


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
            LoadTopFifteenData("Y");
            LoadGoalPaceData();
        }
    }
    protected void rblTopTen_SelectedIndexChanged(object sender, EventArgs e)
    {
        dcLineTotal = 0;
        dcGrandTotal = 0;
        dcShortageTotal = 0;
        dcQtyTotal = 0;
        dcPlacedOrdersCountTotal = 0;
        dcPlacedOrdersTotal = 0;
        dcPlacedLinesCountTotal = 0;

        dcCurrentYTDTotal = 0;
        dcLastYTDTotal = 0;
        dcLastYTDMinusOneTotal = 0;

        dcCurrentYTDEndOfMonthTotal = 0;
        dcLastYTDEndOfMonthTotal = 0;
        dcLastYTDMinusOneYearEndOfMonthAmountTotal = 0;

        dcCurrentMTDTotal = 0;
        dcLastYearCurrentMTDTotal = 0;

        dcCurrentMonthTotal = 0;
        dcLastYearCurrentMonthTotal = 0;

        dcPreviousMonthTotal = 0;
        dcLastYearPreviousMonthTotal = 0;


        dcReadyToInvoiceTotal = 0;
        dcOpenOrdersTotal = 0;

        dcYTDCostTotal = 0;
        dcLastYTDCostTotal = 0;

        dcYTDEndOfMonthCostTotal = 0;
        dcLastYTDEndOfMonthCostTotal = 0;


        dcYearToYearDiffPercentageWeighted = 0;
        dcYearToYearDiffPercentageMinusOneWeighted = 0;

        dcYearToYearDiffPercentageEndOfMonthWeighted = 0;
        dcYearToYearDiffPercentageEndOfMonthMinusOneWeighted = 0;

        dcCurrentYTDAmountPercentageOfTotalSum = 0;
        dcLastYTDAmountPercentageOfTotalSum = 0;

        dcCurrentYTDEndOfMonthAmountPercentageOfTotalSum = 0;
        dcLastYTDEndOfMonthAmountPercentageOfTotalSum = 0;


        if (rblTopTen.SelectedIndex == 1)
        {
            chkShowYTDthroughLastMonth.Checked = true;
        }
        else
        {
            chkShowYTDthroughLastMonth.Checked = false;
        }
        LoadTopFifteenData(rblTopTen.SelectedValue);
    }
    protected void gvTopFifteen_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcCurrentYTD = 0;
        decimal dcLastYTD = 0;
        decimal dcLastYTDMinusOne = 0;

        decimal dcCurrentYTDEndOfMonth = 0;
        decimal dcLastYTDEndOfMonth = 0;
        decimal dcLastYTDMinusOneYearEndOfMonth = 0;

        decimal dcCurrentMTD = 0;
        decimal dcLastYearCurrentMTD = 0;
        decimal dcCurrentMonthAmount = 0;
        decimal dcCurrentMonthCost = 0;
        decimal dcLastYearCurrentMonth = 0;

        decimal dcPreviousMonthAmount = 0;
        decimal dcPreviousMonthCost = 0;
        decimal dcLastYearLastMonth = 0;

        decimal dcReadyToInvoice = 0;
        decimal dcOpenOrders = 0;

        decimal dcYearToYearDiffPercentage = 0;
        decimal dcYearToYearDiffPercentageMinusOne = 0;
        decimal dcYearToYearDiffPercentageEndOfMonth = 0;
        decimal dcYearToYearDiffPercentageEndOfMonthMinusOne = 0;

        decimal dcYTDCost = 0;
        decimal dcLastYTDCost = 0;
        decimal dcYTDEndOfMonthCost = 0;
        decimal dcLastYTDEndOfMonthCost = 0;

        decimal dcYTDWeightedMargin = 0;
        decimal dcLastYTDWeightedMargin = 0;

        decimal dcYTDEndOfMonthWeightedMargin = 0;
        decimal dcLastYTDEndOfMonthWeightedMargin = 0;

        decimal dcCurrentYTDAmountPercentageOfTotal = 0;
        decimal dcLastYTDAmountPercentageOfTotal = 0;

        decimal dcCurrentYTDEndOfMonthAmountPercentageOfTotal = 0;
        decimal dcLastYTDEndOfMonthAmountPercentageOfTotal = 0;

        //Possible Future weighted avg...
        decimal dcCurrentMonthWeightedMargin = 0;
        decimal dcLastMonthWeightedMargin = 0;



        if (e.Row.RowType == DataControlRowType.Header)
        {
            DateTime dtToday = DateTime.Today;
            DateTime dtCurrentMonthFirstDay = new DateTime(dtToday.Year, dtToday.Month, 1);
            DateTime dtCurrentMonthFirstDayLastYear = new DateTime(dtToday.AddYears(-1).Year, dtToday.Month, 1);
            DateTime dtCurrentMonthLastDay = new DateTime(dtToday.Year, dtToday.Month, DateTime.DaysInMonth(dtToday.Year, dtToday.Month));
            DateTime dtCurrentMonthCurrentDay = new DateTime(dtToday.Year, dtToday.Month, dtToday.Day);
            DateTime dtCurrentMonthCurrentDayLastYear = new DateTime(2000, 1, 1);
            try
            {
                dtCurrentMonthCurrentDayLastYear = new DateTime(dtToday.AddYears(-1).Year, dtToday.Month, dtToday.Day);
            }
            catch (Exception)
            {//Leap Year Fix...
                dtCurrentMonthCurrentDayLastYear = new DateTime(dtToday.AddYears(-1).Year, dtToday.Month, 28);
            }

            DateTime dtFirstDayOfCurrentYear = new DateTime(DateTime.Now.Year, 1, 1);
            string sFirstDayOfCurrentYear = dtFirstDayOfCurrentYear.ToShortDateString();
            DateTime dtFirstDayOfLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);
            string sFirstDayOfLastYear = dtFirstDayOfLastYear.ToShortDateString();
            DateTime dtFirstDayOfLastYearMinusOne = new DateTime(DateTime.Now.AddYears(-2).Year, 1, 1);
            string sFirstDayOfLastYearMinusOne = dtFirstDayOfLastYearMinusOne.ToShortDateString();
            string sYTD = DateTime.Now.ToShortDateString();
            string sLYTD = DateTime.Now.AddDays(-365).ToShortDateString();
            string sMTD = DateTime.Now.ToShortDateString();
            DateTime dtFirstDayOfMonthCurrentYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            string sFirstDayOfMonthCurrentYear = dtFirstDayOfMonthCurrentYear.ToShortDateString();

            DateTime dtFirstDayOfLastMonthCurrentYear = dtFirstDayOfMonthCurrentYear.AddMonths(-1);//First Day of Last Month Period!
            string sFirstDayOfLastMonthCurrentYear = dtFirstDayOfLastMonthCurrentYear.ToShortDateString();

            DateTime dtLastDayOfLastMonthCurrentYear = new DateTime(dtFirstDayOfMonthCurrentYear.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month));//Last Day of Last Month period!
            string sLastDayOfLastMonthCurrentYear = dtLastDayOfLastMonthCurrentYear.ToShortDateString();

            DateTime dtFirstDayOfMonthLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1).Month, 1);
            string sFirstDayOfMonthLastYear = dtFirstDayOfMonthLastYear.ToShortDateString();


            DateTime dtFirstDayOfLastMonthLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddMonths(-1).AddYears(-1).Month, 1);
            string sFirstDayOfLastMonthLastYear = dtFirstDayOfLastMonthLastYear.ToShortDateString();

            DateTime dtLastDayOfMonthCurrentYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            string sLastDayOfMonthCurrentYear = dtLastDayOfMonthCurrentYear.ToShortDateString();
            DateTime dtLastDayOfMonthLastYear = dtLastDayOfMonthCurrentYear.AddYears(-1);
            string sLastDayOfMonthLastYear = dtLastDayOfMonthLastYear.ToShortDateString();

            DateTime dtLastDayOfLastMonthLastYear = dtLastDayOfLastMonthCurrentYear.AddYears(-1);
            string sLastDayOfLastMonthLastYear = dtLastDayOfLastMonthLastYear.ToShortDateString();

            DateTime dtLastDayOfLastMonthLastYearMinusOne = dtLastDayOfLastMonthCurrentYear.AddYears(-2);
            string sLastDayOfLastMonthLastYearMinusOne = dtLastDayOfLastMonthLastYearMinusOne.ToShortDateString();

            string sLastYearMTD = DateTime.Now.AddYears(-1).ToShortDateString();

            int iYear = DateTime.Now.Year;
            DateTime dtLastDayOfCurrentYear = new DateTime(iYear, 12, 31);
            DateTime dtLastMonthLastDay = new DateTime(iYear, DateTime.Now.AddMonths(-1).Month, DateTime.DaysInMonth(iYear, DateTime.Now.AddMonths(-1).Month));


            DateTime dtLastMonthCurrentYear = dtLastMonthLastDay;
            string sLastMonthCurrentYear = dtLastMonthLastDay.ToShortDateString();

            DateTime dtLastMonthLastYear = dtLastMonthLastDay.AddYears(-1);
            string sLastMonthLastYear = dtLastMonthLastYear.ToShortDateString();

            DateTime dtLastMonthLastYearMinusOne = dtLastMonthLastDay.AddYears(-2);
            string sLastMonthLastYearMinusOne = dtLastMonthLastYearMinusOne.ToShortDateString();


            DateTime dtLastDayofCurrentYear = dtLastDayOfCurrentYear;
            string sLastDayofCurrentYear = dtLastDayofCurrentYear.ToShortDateString();

            DateTime dtLastDayOfLastYear = dtLastDayOfCurrentYear.AddYears(-1);
            string sLastDayOfLastYear = dtLastDayOfLastYear.ToShortDateString();

            DateTime dtLastDayOfLastYearMinusOne = dtLastDayOfCurrentYear.AddYears(-2);
            string sLastDayOfLastYearMinusOne = dtLastDayOfLastYearMinusOne.ToShortDateString();

            LinkButton btnSort = (LinkButton)e.Row.Cells[1].Controls[0];//LM
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddMonths(-1).ToString("MMM yyyy").ToUpper().Replace(" ", "<br>");
            btnSort = (LinkButton)e.Row.Cells[2].Controls[0];//LMLY
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddMonths(-1).AddYears(-1).ToString("MMM yyyy").ToUpper().Replace(" ", "<br>");
            btnSort = (LinkButton)e.Row.Cells[3].Controls[0];//CM
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = dtCurrentMonthFirstDay.ToShortDateString() + "<br>to<br>" + dtCurrentMonthCurrentDay.ToShortDateString();
            btnSort = (LinkButton)e.Row.Cells[4].Controls[0];//CMLY
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = dtCurrentMonthFirstDayLastYear.ToShortDateString() + "<br>to<br>" + dtCurrentMonthCurrentDayLastYear.ToShortDateString();
            btnSort = (LinkButton)e.Row.Cells[5].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.ToString("MMM").ToUpper() + "<br>" + DateTime.Now.Year.ToString();
            btnSort = (LinkButton)e.Row.Cells[6].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.ToString("MMM").ToUpper() + "<br>" + DateTime.Now.AddYears(-1).Year.ToString();
            btnSort = (LinkButton)e.Row.Cells[7].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.Year.ToString() + " YTD";
            btnSort = (LinkButton)e.Row.Cells[8].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddYears(-1).Year.ToString() + " YTD";
            btnSort = (LinkButton)e.Row.Cells[9].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddYears(-2).Year.ToString() + " YTD";
            btnSort = (LinkButton)e.Row.Cells[10].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.Year.ToString() + "<br> to <br>" + DateTime.Now.AddYears(-1).Year.ToString() + "<br> DIFF %" + " YTD";
            btnSort = (LinkButton)e.Row.Cells[11].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddYears(-1).Year.ToString() + "<br> to <br>" + DateTime.Now.AddYears(-2).Year.ToString() + "<br> DIFF %" + " YTD"; //New Column 2 Years back...
            btnSort = (LinkButton)e.Row.Cells[12].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.Year.ToString() + "<br> MARGIN" + " YTD";
            btnSort = (LinkButton)e.Row.Cells[13].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddYears(-1).Year.ToString() + "<br> MARGIN" + " YTD";
            btnSort = (LinkButton)e.Row.Cells[14].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.Year.ToString() + " %" + "<br>YTD";
            btnSort = (LinkButton)e.Row.Cells[15].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddYears(-1).Year.ToString() + " %" + "<br>YTD";
            btnSort = (LinkButton)e.Row.Cells[16].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfCurrentYear + "<br>to<br>" + sLastMonthCurrentYear;
            btnSort = (LinkButton)e.Row.Cells[17].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfLastYear + "<br>to<br>" + sLastMonthLastYear;
            btnSort = (LinkButton)e.Row.Cells[18].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfLastYearMinusOne + "<br>to<br>" + sLastMonthLastYearMinusOne;//New 2 years back...
            btnSort = (LinkButton)e.Row.Cells[19].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.Year.ToString() + "<br>vs<br>" + DateTime.Now.AddYears(-1).Year.ToString() + "<br>Thru End of<br>Last Month";
            btnSort = (LinkButton)e.Row.Cells[20].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddYears(-1).Year.ToString() + "<br>vs<br>" + DateTime.Now.AddYears(-2).Year.ToString() + "<br>Thru End of<br>Last Month";//New column 2 years back...
            //New 11-10-2021...
            btnSort = (LinkButton)e.Row.Cells[21].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfMonthCurrentYear + "<br>to<br>" + sLastDayOfMonthCurrentYear + "<br>MARGIN";
            btnSort = (LinkButton)e.Row.Cells[22].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfLastMonthCurrentYear + "<br>to<br>" + sLastDayOfLastMonthCurrentYear + "<br>MARGIN";

            btnSort = (LinkButton)e.Row.Cells[23].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfCurrentYear + "<br>to<br>" + sLastDayofCurrentYear + "<br>MARGIN";
            btnSort = (LinkButton)e.Row.Cells[24].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfLastYear + "<br>to<br>" + sLastDayOfLastYear + "<br>MARGIN";
            btnSort = (LinkButton)e.Row.Cells[25].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfCurrentYear + "<br>to<br>" + sLastDayofCurrentYear + "<br>PERCENT";
            btnSort = (LinkButton)e.Row.Cells[26].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfLastYear + "<br>to<br>" + sLastDayOfLastYear + "<br>PERCENT";

            e.Row.Cells[1].Visible = true;//LM
            e.Row.Cells[2].Visible = true;//LMLY

            if (!chkShowYTDthroughLastMonth.Checked)
            {

                e.Row.Cells[3].Visible = false;//LMTD
                e.Row.Cells[4].Visible = false;//LYLMTD
                e.Row.Cells[5].Visible = true;//MTD
                e.Row.Cells[6].Visible = true;//LYMTD

                //<% --YTD-- %>
                e.Row.Cells[7].Visible = true;//CM
                e.Row.Cells[8].Visible = true;//LYCM
                e.Row.Cells[9].Visible = true;//YTD
                e.Row.Cells[10].Visible = true;//LYTD
                e.Row.Cells[11].Visible = true;//LLYTD - New 1-28-2020
                e.Row.Cells[12].Visible = false;//YR to YR Diff -- Hide 11-10-2021
                e.Row.Cells[13].Visible = false;//YR to YR Diff Minus One -- Hide 11-10-2021
                e.Row.Cells[14].Visible = true;//YTD MARGIN
                e.Row.Cells[15].Visible = true;//LYTD MARGIN

                //<% --YTD End of Month-- %>
                e.Row.Cells[16].Visible = false;//YTDEM
                e.Row.Cells[17].Visible = false;//LYTDEM
                e.Row.Cells[18].Visible = false;//LLYTDEM
                e.Row.Cells[19].Visible = false;//YR to YR Diff EM -- Hide 11-10-2021
                e.Row.Cells[20].Visible = false;//YR to YR Diff EM Minus One -- Hide 11-10-2021

                e.Row.Cells[21].Visible = true;//CUR MO MARGIN -- new 11-10-2021...
                e.Row.Cells[22].Visible = true;//LAST MO MARGIN -- new 11-10-2021...

                e.Row.Cells[23].Visible = false;//YTD MARGIN EM
                e.Row.Cells[24].Visible = false;//LYTD MARGIN EM
                e.Row.Cells[25].Visible = false;//YTD % EM
                e.Row.Cells[26].Visible = false;//LYTD % EM
            }
            else//ShowYTDthroughLastMonth
            {
                e.Row.Cells[3].Visible = false;//LMTD
                e.Row.Cells[4].Visible = false;//LYLMTD
                e.Row.Cells[5].Visible = false;//MTD
                e.Row.Cells[6].Visible = false;//LYMTD

                //<% --YTD-- %>
                e.Row.Cells[7].Visible = false;//CM
                e.Row.Cells[8].Visible = false;//LYCM
                e.Row.Cells[9].Visible = false;//YTD
                e.Row.Cells[10].Visible = false;//LYTD
                e.Row.Cells[11].Visible = false;//LLYTD - New 1-28-2020
                e.Row.Cells[12].Visible = false;//YR to YR Diff -- Hide 11-10-2021
                e.Row.Cells[13].Visible = false;//YR to YR Diff Minus One -- Hide 11-10-2021
                e.Row.Cells[14].Visible = false;//YTD MARGIN
                e.Row.Cells[15].Visible = false;//LYTD MARGIN
                //<% --YTD End of Month-- %>
                e.Row.Cells[16].Visible = true;//YTDEM
                e.Row.Cells[17].Visible = true;//LYTDEM
                e.Row.Cells[18].Visible = true;//LLYTDEM
                e.Row.Cells[19].Visible = false;//YR to YR Diff EM -- Hide 11-10-2021
                e.Row.Cells[20].Visible = false;//YR to YR Diff EM Minus One -- Hide 11-10-2021

                e.Row.Cells[21].Visible = true;//CUR MO MARGIN -- new 11-10-2021...
                e.Row.Cells[22].Visible = true;//LAST MO MARGIN -- new 11-10-2021...

                e.Row.Cells[23].Visible = true;//YTD MARGIN EM
                e.Row.Cells[24].Visible = true;//LYTD MARGIN EM
                e.Row.Cells[25].Visible = true;//YTD % EM
                e.Row.Cells[26].Visible = true;//LYTD % EM
            }

            if (!chkShowLastColumns.Checked)
            {
                e.Row.Cells[27].Visible = false;
                e.Row.Cells[28].Visible = false;
                e.Row.Cells[29].Visible = false;
            }

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //YTD...
            Label lblCurrentYTDAmountD = (Label)e.Row.FindControl("lblCurrentYTDAmountD");
            if (lblCurrentYTDAmountD.Text != "")
            {
                dcCurrentYTD = Convert.ToDecimal(lblCurrentYTDAmountD.Text.Replace("$", ""));
                dcCurrentYTDTotal += dcCurrentYTD;
                lblCurrentYTDAmountD.Text = "$" + dcCurrentYTD.ToString("#,0");
            }
            //Last Year...
            Label lblLastYTDAmountD = (Label)e.Row.FindControl("lblLastYTDAmountD");
            if (lblLastYTDAmountD.Text != "")
            {
                dcLastYTD = Convert.ToDecimal(lblLastYTDAmountD.Text.Replace("$", ""));
                dcLastYTDTotal += dcLastYTD;
                lblLastYTDAmountD.Text = "$" + dcLastYTD.ToString("#,0");
            }
            //Two years back...
            Label lblLastYTDMinusOneAmountD = (Label)e.Row.FindControl("lblLastYTDMinusOneAmountD");
            if (lblLastYTDMinusOneAmountD.Text != "")
            {
                dcLastYTDMinusOne = Convert.ToDecimal(lblLastYTDMinusOneAmountD.Text.Replace("$", ""));
                dcLastYTDMinusOneTotal += dcLastYTDMinusOne;
                lblLastYTDMinusOneAmountD.Text = "$" + dcLastYTDMinusOne.ToString("#,0");
            }

            Label lblYearToYearDiffPercentage = (Label)e.Row.FindControl("lblYearToYearDiffPercentage");
            if (lblYearToYearDiffPercentage.Text != "")
            {
                dcYearToYearDiffPercentage = Convert.ToDecimal(lblYearToYearDiffPercentage.Text);
                lblYearToYearDiffPercentage.Text = lblYearToYearDiffPercentage.Text + "%";
                if (dcYearToYearDiffPercentage < 0)
                {
                    lblYearToYearDiffPercentage.ForeColor = Color.Red;
                }
            }
            Label lblYearToYearDiffPercentageMinusOne = (Label)e.Row.FindControl("lblYearToYearDiffPercentageMinusOne");
            if (lblYearToYearDiffPercentage.Text != "")
            {
                dcYearToYearDiffPercentageMinusOne = Convert.ToDecimal(lblYearToYearDiffPercentageMinusOne.Text);
                lblYearToYearDiffPercentageMinusOne.Text = lblYearToYearDiffPercentageMinusOne.Text + "%";
            }

            //New 11-11-2021...
            Label lblCurrentMonthMargin = (Label)e.Row.FindControl("lblCurrentMonthMargin");
            lblCurrentMonthMargin.Text = lblCurrentMonthMargin.Text + "%";

            Label lblLastMonthMargin = (Label)e.Row.FindControl("lblLastMonthMargin");
            lblLastMonthMargin.Text = lblLastMonthMargin.Text + "%";

            //YTD End of Month...
            Label lblCurrentYTDEndOfMonthAmountD = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthAmountD");
            if (lblCurrentYTDEndOfMonthAmountD.Text != "")
            {
                dcCurrentYTDEndOfMonth = Convert.ToDecimal(lblCurrentYTDEndOfMonthAmountD.Text.Replace("$", ""));
                dcCurrentYTDEndOfMonthTotal += dcCurrentYTDEndOfMonth;
                lblCurrentYTDEndOfMonthAmountD.Text = "$" + dcCurrentYTDEndOfMonth.ToString("#,0");
            }
            Label lblLastYTDEndOfMonthAmountD = (Label)e.Row.FindControl("lblLastYTDEndOfMonthAmountD");
            if (lblCurrentYTDEndOfMonthAmountD.Text != "")
            {
                dcLastYTDEndOfMonth = Convert.ToDecimal(lblLastYTDEndOfMonthAmountD.Text.Replace("$", ""));
                dcLastYTDEndOfMonthTotal += dcLastYTDEndOfMonth;
                lblLastYTDEndOfMonthAmountD.Text = "$" + dcLastYTDEndOfMonth.ToString("#,0");
            }

            Label lblLastYTDMinusOneYearEndOfMonthAmountD = (Label)e.Row.FindControl("lblLastYTDMinusOneYearEndOfMonthAmountD");
            if (lblLastYTDMinusOneYearEndOfMonthAmountD.Text != "")
            {
                dcLastYTDMinusOneYearEndOfMonth = Convert.ToDecimal(lblLastYTDMinusOneYearEndOfMonthAmountD.Text.Replace("$", ""));
                dcLastYTDMinusOneYearEndOfMonthAmountTotal += dcLastYTDMinusOneYearEndOfMonth;
                lblLastYTDMinusOneYearEndOfMonthAmountD.Text = "$" + dcLastYTDMinusOneYearEndOfMonth.ToString("#,0");
            }

            Label lblYearToYearDiffPercentageEndOfMonth = (Label)e.Row.FindControl("lblYearToYearDiffPercentageEndOfMonth");
            if (lblYearToYearDiffPercentageEndOfMonth.Text != "")
            {
                dcYearToYearDiffPercentageEndOfMonth = Convert.ToDecimal(lblYearToYearDiffPercentageEndOfMonth.Text);
                lblYearToYearDiffPercentageEndOfMonth.Text = lblYearToYearDiffPercentageEndOfMonth.Text + "%";
                if (dcYearToYearDiffPercentageEndOfMonth < 0)
                {
                    lblYearToYearDiffPercentageEndOfMonth.ForeColor = Color.Red;
                }
            }
            Label lblYearToYearDiffPercentageEndOfMonthMinusOne = (Label)e.Row.FindControl("lblYearToYearDiffPercentageEndOfMonthMinusOne");
            if (lblYearToYearDiffPercentageEndOfMonthMinusOne.Text != "")
            {
                dcYearToYearDiffPercentageEndOfMonthMinusOne = Convert.ToDecimal(lblYearToYearDiffPercentageEndOfMonthMinusOne.Text);
                lblYearToYearDiffPercentageEndOfMonthMinusOne.Text = lblYearToYearDiffPercentageEndOfMonthMinusOne.Text + "%";

            }


            //MTD...
            Label lblCurrentMTDAmountD = (Label)e.Row.FindControl("lblCurrentMTDAmountD");
            if (lblCurrentMTDAmountD.Text != "")
            {
                dcCurrentMTD = Convert.ToDecimal(lblCurrentMTDAmountD.Text.Replace("$", ""));
                dcCurrentMTDTotal += dcCurrentMTD;
                lblCurrentMTDAmountD.Text = "$" + dcCurrentMTD.ToString("#,0");
            }

            Label lblLastYearCurrentMTDAmountD = (Label)e.Row.FindControl("lblLastYearCurrentMTDAmountD");
            if (lblLastYearCurrentMTDAmountD.Text != "")
            {
                dcLastYearCurrentMTD = Convert.ToDecimal(lblLastYearCurrentMTDAmountD.Text.Replace("$", ""));
                dcLastYearCurrentMTDTotal += dcLastYearCurrentMTD;
                lblLastYearCurrentMTDAmountD.Text = "$" + dcLastYearCurrentMTD.ToString("#,0");
            }

            //Entire Current Month...
            Label lblCurrentMonthAmountD = (Label)e.Row.FindControl("lblCurrentMonthAmountD");
            if (lblCurrentMonthAmountD.Text != "")
            {
                dcCurrentMonthAmount = Convert.ToDecimal(lblCurrentMonthAmountD.Text.Replace("$", ""));
                dcCurrentMonthTotal += dcCurrentMonthAmount;
                lblCurrentMonthAmountD.Text = "$" + dcCurrentMonthAmount.ToString("#,0");
            }

            Label lblLastYearCurrentMonthAmountD = (Label)e.Row.FindControl("lblLastYearCurrentMonthAmountD");
            if (lblLastYearCurrentMonthAmountD.Text != "")
            {
                dcLastYearCurrentMonth = Convert.ToDecimal(lblLastYearCurrentMonthAmountD.Text.Replace("$", ""));
                dcLastYearCurrentMonthTotal += dcLastYearCurrentMonth;
                lblLastYearCurrentMonthAmountD.Text = "$" + dcLastYearCurrentMonth.ToString("#,0");
            }

            //Entire Previous Month...
            Label lblPreviousMonthAmountD = (Label)e.Row.FindControl("lblPreviousMonthAmountD");
            if (lblPreviousMonthAmountD.Text != "")
            {
                dcPreviousMonthAmount = Convert.ToDecimal(lblPreviousMonthAmountD.Text.Replace("$", ""));
                dcPreviousMonthTotal += dcPreviousMonthAmount;
                lblPreviousMonthAmountD.Text = "$" + dcPreviousMonthAmount.ToString("#,0");
            }

            //New 11-10-2021...
            Label lblCurrentMonthCost = (Label)e.Row.FindControl("lblCurrentMonthCost");
            if (lblCurrentMonthCost.Text != "")
            {
                dcCurrentMonthCost = Convert.ToDecimal(lblCurrentMonthCost.Text.Replace("$", ""));
                dcCurrentMonthCostTotal += dcCurrentMonthCost;
                lblCurrentMonthCost.Text = "$" + dcCurrentMonthCost.ToString("#,0");
            }
            Label lblPreviousMonthCost = (Label)e.Row.FindControl("lblPreviousMonthCost");
            if (lblPreviousMonthCost.Text != "")
            {
                dcPreviousMonthCost = Convert.ToDecimal(lblPreviousMonthCost.Text.Replace("$", ""));
                dcPreviousMonthCostTotal += dcPreviousMonthCost;
                lblPreviousMonthCost.Text = "$" + dcPreviousMonthCost.ToString("#,0");
            }



            Label lblLastYearPreviousMonthAmountD = (Label)e.Row.FindControl("lblLastYearPreviousMonthAmountD");
            if (lblLastYearPreviousMonthAmountD.Text != "")
            {
                dcLastYearLastMonth = Convert.ToDecimal(lblLastYearPreviousMonthAmountD.Text.Replace("$", ""));
                dcLastYearPreviousMonthTotal += dcLastYearLastMonth;
                lblLastYearPreviousMonthAmountD.Text = "$" + dcLastYearLastMonth.ToString("#,0");
            }


            //Other...
            Label lblReadyToInvoiceAmountD = (Label)e.Row.FindControl("lblReadyToInvoiceAmountD");
            if (lblReadyToInvoiceAmountD.Text != "")
            {
                dcReadyToInvoice = Convert.ToDecimal(lblReadyToInvoiceAmountD.Text.Replace("$", ""));
                dcReadyToInvoiceTotal += dcReadyToInvoice;
                lblReadyToInvoiceAmountD.Text = "$" + dcReadyToInvoice.ToString("#,0");
            }
            Label lblOpenOrdersAmountD = (Label)e.Row.FindControl("lblOpenOrdersAmountD");
            if (lblOpenOrdersAmountD.Text != "")
            {
                dcOpenOrders = Convert.ToDecimal(lblOpenOrdersAmountD.Text.Replace("$", ""));
                dcOpenOrdersTotal += dcOpenOrders;
                lblOpenOrdersAmountD.Text = "$" + dcOpenOrders.ToString("#,0");
            }
            Label lblCombinedOpenOrdersAmountD = (Label)e.Row.FindControl("lblCombinedOpenOrdersAmountD");
            lblCombinedOpenOrdersAmountD.Text = "$" + lblCombinedOpenOrdersAmountD.Text;
            Label lblGrandTotalCombinedOpenOrdersAmount = (Label)e.Row.FindControl("lblGrandTotalCombinedOpenOrdersAmount");
            if (lblGrandTotalCombinedOpenOrdersAmount.Text != "")
            {
                dcCombinedOpenOrdersAmount = Convert.ToDecimal(lblGrandTotalCombinedOpenOrdersAmount.Text.Replace("$", ""));

                //lblOpenOrdersAmountD.Text = "$" + dcOpenOrders.ToString("#,0");
            }



            //YTD...
            Label lblCurrentYTDCost = (Label)e.Row.FindControl("lblCurrentYTDCost");
            if (lblCurrentYTDCost.Text != "")
            {
                dcYTDCost = Convert.ToDecimal(lblCurrentYTDCost.Text.Replace("$", ""));
                dcYTDCostTotal += dcYTDCost;
            }

            Label lblLastYTDCost = (Label)e.Row.FindControl("lblLastYTDCost");
            if (lblLastYTDCost.Text != "")
            {
                dcLastYTDCost = Convert.ToDecimal(lblLastYTDCost.Text.Replace("$", ""));
                dcLastYTDCostTotal += dcLastYTDCost;
            }

            //YTD End of Month...
            Label lblCurrentYTDEndOfMonthCost = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthCost");
            if (lblCurrentYTDEndOfMonthCost.Text != "")
            {
                dcYTDEndOfMonthCost = Convert.ToDecimal(lblCurrentYTDEndOfMonthCost.Text.Replace("$", ""));
                dcYTDEndOfMonthCostTotal += dcYTDEndOfMonthCost;
            }

            Label lblLastYTDEndOfMonthCost = (Label)e.Row.FindControl("lblLastYTDEndOfMonthCost");
            if (lblLastYTDEndOfMonthCost.Text != "")
            {
                dcLastYTDEndOfMonthCost = Convert.ToDecimal(lblLastYTDEndOfMonthCost.Text.Replace("$", ""));
                dcLastYTDEndOfMonthCostTotal += dcLastYTDEndOfMonthCost;
            }

            Label lblCurrentYTDMargin = (Label)e.Row.FindControl("lblCurrentYTDMargin");
            lblCurrentYTDMargin.Text = lblCurrentYTDMargin.Text + "%";
            Label lblLastYTDMargin = (Label)e.Row.FindControl("lblLastYTDMargin");
            lblLastYTDMargin.Text = lblLastYTDMargin.Text + "%";

            Label lblCurrentYTDEndOfMonthMargin = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthMargin");
            lblCurrentYTDEndOfMonthMargin.Text = lblCurrentYTDEndOfMonthMargin.Text + "%";
            Label lblLastYTDEndOfMonthMargin = (Label)e.Row.FindControl("lblLastYTDEndOfMonthMargin");
            lblLastYTDEndOfMonthMargin.Text = lblLastYTDEndOfMonthMargin.Text + "%";

            Label lblCurrentYTDAmountPercentageOfTotal = (Label)e.Row.FindControl("lblCurrentYTDAmountPercentageOfTotal");
            dcCurrentYTDAmountPercentageOfTotal = Convert.ToDecimal(lblCurrentYTDAmountPercentageOfTotal.Text);
            dcCurrentYTDAmountPercentageOfTotalSum += dcCurrentYTDAmountPercentageOfTotal;
            lblCurrentYTDAmountPercentageOfTotal.Text = lblCurrentYTDAmountPercentageOfTotal.Text + "%";

            Label lblLastYTDAmountPercentageOfTotal = (Label)e.Row.FindControl("lblLastYTDAmountPercentageOfTotal");
            dcLastYTDAmountPercentageOfTotal = Convert.ToDecimal(lblLastYTDAmountPercentageOfTotal.Text);
            dcLastYTDAmountPercentageOfTotalSum += dcLastYTDAmountPercentageOfTotal;
            lblLastYTDAmountPercentageOfTotal.Text = lblLastYTDAmountPercentageOfTotal.Text + "%";

            Label lblCurrentYTDEndOfMonthAmountPercentageOfTotal = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthAmountPercentageOfTotal");
            dcCurrentYTDEndOfMonthAmountPercentageOfTotal = Convert.ToDecimal(lblCurrentYTDEndOfMonthAmountPercentageOfTotal.Text);
            dcCurrentYTDEndOfMonthAmountPercentageOfTotalSum += dcCurrentYTDEndOfMonthAmountPercentageOfTotal;
            lblCurrentYTDEndOfMonthAmountPercentageOfTotal.Text = lblCurrentYTDEndOfMonthAmountPercentageOfTotal.Text + "%";

            Label lblLastYTDEndOfMonthAmountPercentageOfTotal = (Label)e.Row.FindControl("lblLastYTDEndOfMonthAmountPercentageOfTotal");
            dcLastYTDEndOfMonthAmountPercentageOfTotal = Convert.ToDecimal(lblLastYTDEndOfMonthAmountPercentageOfTotal.Text);
            dcLastYTDEndOfMonthAmountPercentageOfTotalSum += dcLastYTDEndOfMonthAmountPercentageOfTotal;
            lblLastYTDEndOfMonthAmountPercentageOfTotal.Text = lblLastYTDEndOfMonthAmountPercentageOfTotal.Text + "%";


            e.Row.Cells[1].Visible = true;//LM
            e.Row.Cells[2].Visible = true;//LMLY

            if (!chkShowYTDthroughLastMonth.Checked)
            {

                e.Row.Cells[3].Visible = false;//LMTD
                e.Row.Cells[4].Visible = false;//LYLMTD
                e.Row.Cells[5].Visible = true;//MTD
                e.Row.Cells[6].Visible = true;//LYMTD

                //<% --YTD-- %>
                e.Row.Cells[7].Visible = true;//CM
                e.Row.Cells[8].Visible = true;//LYCM
                e.Row.Cells[9].Visible = true;//YTD
                e.Row.Cells[10].Visible = true;//LYTD
                e.Row.Cells[11].Visible = true;//LLYTD - New 1-28-2020
                e.Row.Cells[12].Visible = false;//YR to YR Diff -- Hide 11-10-2021
                e.Row.Cells[13].Visible = false;//YR to YR Diff Minus One -- Hide 11-10-2021
                e.Row.Cells[14].Visible = true;//YTD MARGIN
                e.Row.Cells[15].Visible = true;//LYTD MARGIN

                //<% --YTD End of Month-- %>
                e.Row.Cells[16].Visible = false;//YTDEM
                e.Row.Cells[17].Visible = false;//LYTDEM
                e.Row.Cells[18].Visible = false;//LLYTDEM
                e.Row.Cells[19].Visible = false;//YR to YR Diff EM -- Hide 11-10-2021
                e.Row.Cells[20].Visible = false;//YR to YR Diff EM Minus One -- Hide 11-10-2021

                e.Row.Cells[21].Visible = true;//CUR MO MARGIN -- new 11-10-2021...
                e.Row.Cells[22].Visible = true;//LAST MO MARGIN -- new 11-10-2021...

                e.Row.Cells[23].Visible = false;//YTD MARGIN EM
                e.Row.Cells[24].Visible = false;//LYTD MARGIN EM
                e.Row.Cells[25].Visible = false;//YTD % EM
                e.Row.Cells[26].Visible = false;//LYTD % EM
            }
            else//ShowYTDthroughLastMonth
            {
                e.Row.Cells[3].Visible = false;//LMTD
                e.Row.Cells[4].Visible = false;//LYLMTD
                e.Row.Cells[5].Visible = false;//MTD
                e.Row.Cells[6].Visible = false;//LYMTD

                //<% --YTD-- %>
                e.Row.Cells[7].Visible = false;//CM
                e.Row.Cells[8].Visible = false;//LYCM
                e.Row.Cells[9].Visible = false;//YTD
                e.Row.Cells[10].Visible = false;//LYTD
                e.Row.Cells[11].Visible = false;//LLYTD - New 1-28-2020
                e.Row.Cells[12].Visible = false;//YR to YR Diff -- Hide 11-10-2021
                e.Row.Cells[13].Visible = false;//YR to YR Diff Minus One -- Hide 11-10-2021
                e.Row.Cells[14].Visible = false;//YTD MARGIN
                e.Row.Cells[15].Visible = false;//LYTD MARGIN
                //<% --YTD End of Month-- %>
                e.Row.Cells[16].Visible = true;//YTDEM
                e.Row.Cells[17].Visible = true;//LYTDEM
                e.Row.Cells[18].Visible = true;//LLYTDEM
                e.Row.Cells[19].Visible = false;//YR to YR Diff EM -- Hide 11-10-2021
                e.Row.Cells[20].Visible = false;//YR to YR Diff EM Minus One -- Hide 11-10-2021

                e.Row.Cells[21].Visible = true;//CUR MO MARGIN -- new 11-10-2021...
                e.Row.Cells[22].Visible = true;//LAST MO MARGIN -- new 11-10-2021...

                e.Row.Cells[23].Visible = true;//YTD MARGIN EM
                e.Row.Cells[24].Visible = true;//LYTD MARGIN EM
                e.Row.Cells[25].Visible = true;//YTD % EM
                e.Row.Cells[26].Visible = true;//LYTD % EM
            }

            if (!chkShowLastColumns.Checked)
            {
                e.Row.Cells[27].Visible = false;
                e.Row.Cells[28].Visible = false;
                e.Row.Cells[29].Visible = false;
            }


            Label lblName = (Label)e.Row.FindControl("lblName");
            switch (lblName.Text.Trim())
            {
                case "SMASHBURGER":
                    lblName.ForeColor = Color.Red;
                    break;
                case "KRISPY KREME":
                    lblName.ForeColor = Color.Red;
                    break;
                case "IN & OUT":
                    lblName.ForeColor = Color.Blue;
                    break;
                case "BAKEMARK":
                    lblName.ForeColor = Color.Blue;
                    break;
                case "SYSCO":
                    lblName.ForeColor = Color.Blue;
                    break;
                case "ARYZTA":
                    lblName.ForeColor = Color.Blue;
                    break;
                case "US FOODS":
                    lblName.ForeColor = Color.Blue;
                    break;
                default:
                    break;
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            //YTD...
            Label lblCurrentYTDAmountTotal = (Label)e.Row.FindControl("lblCurrentYTDAmountTotal");
            lblCurrentYTDAmountTotal.Text = "$" + dcCurrentYTDTotal.ToString("#,0");

            Label lblLastYTDAmountTotal = (Label)e.Row.FindControl("lblLastYTDAmountTotal");
            lblLastYTDAmountTotal.Text = "$" + dcLastYTDTotal.ToString("#,0");

            Label lblLastYTDMinusOneAmountTotal = (Label)e.Row.FindControl("lblLastYTDMinusOneAmountTotal");
            lblLastYTDMinusOneAmountTotal.Text = "$" + dcLastYTDMinusOneTotal.ToString("#,0");

            //YTD End of Month...
            Label lblCurrentYTDEndOfMonthAmountTotal = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthAmountTotal");
            lblCurrentYTDEndOfMonthAmountTotal.Text = "$" + dcCurrentYTDEndOfMonthTotal.ToString("#,0");

            Label lblLastYTDEndOfMonthAmountTotal = (Label)e.Row.FindControl("lblLastYTDEndOfMonthAmountTotal");
            lblLastYTDEndOfMonthAmountTotal.Text = "$" + dcLastYTDEndOfMonthTotal.ToString("#,0");

            Label lblLastYTDMinusOneYearEndOfMonthAmountTotal = (Label)e.Row.FindControl("lblLastYTDMinusOneYearEndOfMonthAmountTotal");
            lblLastYTDMinusOneYearEndOfMonthAmountTotal.Text = "$" + dcLastYTDMinusOneYearEndOfMonthAmountTotal.ToString("#,0");

            //MTD...
            Label lblCurrentMTDAmountTotal = (Label)e.Row.FindControl("lblCurrentMTDAmountTotal");
            lblCurrentMTDAmountTotal.Text = "$" + dcCurrentMTDTotal.ToString("#,0");

            Label lblLastYearCurrentMTDAmountTotal = (Label)e.Row.FindControl("lblLastYearCurrentMTDAmountTotal");
            lblLastYearCurrentMTDAmountTotal.Text = "$" + dcLastYearCurrentMTDTotal.ToString("#,0");

            //Entire Current Month...
            Label lblCurrentMonthAmountTotal = (Label)e.Row.FindControl("lblCurrentMonthAmountTotal");
            lblCurrentMonthAmountTotal.Text = "$" + dcCurrentMonthTotal.ToString("#,0");

            Label lblLastYearCurrentMonthAmountTotal = (Label)e.Row.FindControl("lblLastYearCurrentMonthAmountTotal");
            lblLastYearCurrentMonthAmountTotal.Text = "$" + dcLastYearCurrentMonthTotal.ToString("#,0");


            //Entire Previous Month...
            Label lblPreviousMonthAmountTotal = (Label)e.Row.FindControl("lblPreviousMonthAmountTotal");
            lblPreviousMonthAmountTotal.Text = "$" + dcPreviousMonthTotal.ToString("#,0");

            Label lblLastYearPreviousMonthAmountTotal = (Label)e.Row.FindControl("lblLastYearPreviousMonthAmountTotal");
            lblLastYearPreviousMonthAmountTotal.Text = "$" + dcLastYearPreviousMonthTotal.ToString("#,0");



            //Other...
            Label lblReadyToInvoiceAmountTotal = (Label)e.Row.FindControl("lblReadyToInvoiceAmountTotal");
            lblReadyToInvoiceAmountTotal.Text = "$" + dcReadyToInvoiceTotal.ToString("#,0");
            Label lblOpenOrdersAmountTotal = (Label)e.Row.FindControl("lblOpenOrdersAmountTotal");
            lblOpenOrdersAmountTotal.Text = "$" + dcOpenOrdersTotal.ToString("#,0");

            Label lblCombinedOpenOrdersAmountDTotal = (Label)e.Row.FindControl("lblCombinedOpenOrdersAmountDTotal");
            lblCombinedOpenOrdersAmountDTotal.Text = "$" + dcCombinedOpenOrdersAmount.ToString("#,0");


            //New 11-10-2021...
            //Current Month Margin...
            Label lblCurrentMonthMarginWeighted = (Label)e.Row.FindControl("lblCurrentMonthMarginWeighted");
            if (dcCurrentMonthTotal == 0)
            {
                dcCurrentMonthTotal = 1;
            }
            dcCurrentMonthWeightedMargin = ((dcCurrentMonthTotal - dcCurrentMonthCostTotal) / dcCurrentMonthTotal) * 100;
            lblCurrentMonthMarginWeighted.Text = dcCurrentMonthWeightedMargin.ToString("0.0") + "%";

            //Last Month Margin...
            Label lblLastMonthMarginWeighted = (Label)e.Row.FindControl("lblLastMonthMarginWeighted");
            if (dcPreviousMonthTotal == 0)
            {
                dcPreviousMonthTotal = 1;
            }
            dcLastMonthWeightedMargin = ((dcPreviousMonthTotal - dcPreviousMonthCostTotal) / dcPreviousMonthTotal) * 100;
            lblLastMonthMarginWeighted.Text = dcLastMonthWeightedMargin.ToString("0.0") + "%";


            //YTD...
            Label lblCurrentYTDMarginWeighted = (Label)e.Row.FindControl("lblCurrentYTDMarginWeighted");
            if (dcCurrentYTDTotal == 0)
            {
                dcCurrentYTDTotal = 1;
            }
            dcYTDWeightedMargin = ((dcCurrentYTDTotal - dcYTDCostTotal) / dcCurrentYTDTotal) * 100;
            lblCurrentYTDMarginWeighted.Text = dcYTDWeightedMargin.ToString("0") + "%";

            Label lblLastYTDAmountMarginWeighted = (Label)e.Row.FindControl("lblLastYTDAmountMarginWeighted");
            if (dcLastYTDTotal == 0)
            {
                dcLastYTDTotal = 1;
            }

            dcLastYTDWeightedMargin = ((dcLastYTDTotal - dcLastYTDCostTotal) / dcLastYTDTotal) * 100;
            lblLastYTDAmountMarginWeighted.Text = dcLastYTDWeightedMargin.ToString("0") + "%";

            //YTD End of Month...
            Label lblCurrentYTDEndOfMonthMarginWeighted = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthMarginWeighted");
            if (dcCurrentYTDEndOfMonthTotal == 0)
            {
                dcCurrentYTDEndOfMonthTotal = 1;
            }
            dcYTDEndOfMonthWeightedMargin = ((dcCurrentYTDEndOfMonthTotal - dcYTDEndOfMonthCostTotal) / dcCurrentYTDEndOfMonthTotal) * 100;
            lblCurrentYTDEndOfMonthMarginWeighted.Text = dcYTDEndOfMonthWeightedMargin.ToString("0.0") + "%";

            Label lblLastYTDEndOfMonthAmountMarginWeighted = (Label)e.Row.FindControl("lblLastYTDEndOfMonthAmountMarginWeighted");
            if (dcLastYTDEndOfMonthTotal == 0)
            {
                dcLastYTDEndOfMonthTotal = 1;
            }

            dcLastYTDEndOfMonthWeightedMargin = ((dcLastYTDEndOfMonthTotal - dcLastYTDEndOfMonthCostTotal) / dcLastYTDEndOfMonthTotal) * 100;
            lblLastYTDEndOfMonthAmountMarginWeighted.Text = dcLastYTDEndOfMonthWeightedMargin.ToString("0.0") + "%";

            //Weighted Year to Year...
            Label lblYearToYearDiffPercentageWeighted = (Label)e.Row.FindControl("lblYearToYearDiffPercentageWeighted");
            dcYearToYearDiffPercentageWeighted = ((dcCurrentYTDTotal - dcLastYTDTotal) / dcLastYTDTotal) * 100;
            lblYearToYearDiffPercentageWeighted.Text = dcYearToYearDiffPercentageWeighted.ToString("0") + "%";

            Label lblYearToYearDiffPercentageMinusOneWeighted = (Label)e.Row.FindControl("lblYearToYearDiffPercentageMinusOneWeighted");
            if (dcLastYTDMinusOneTotal == 0)
            {
                dcLastYTDMinusOneTotal = 1;
            }
            dcYearToYearDiffPercentageMinusOneWeighted = ((dcLastYTDTotal - dcLastYTDMinusOneTotal) / dcLastYTDMinusOneTotal) * 100;
            lblYearToYearDiffPercentageMinusOneWeighted.Text = dcYearToYearDiffPercentageMinusOneWeighted.ToString("0") + "%";

            //Weighted Year to Year End of Month...
            Label lblYearToYearDiffPercentageEndOfMonthWeighted = (Label)e.Row.FindControl("lblYearToYearDiffPercentageEndOfMonthWeighted");
            if (dcLastYTDEndOfMonthTotal == 0)
            {
                dcLastYTDEndOfMonthTotal = 1;
            }
            dcYearToYearDiffPercentageEndOfMonthWeighted = ((dcCurrentYTDEndOfMonthTotal - dcLastYTDEndOfMonthTotal) / dcLastYTDEndOfMonthTotal) * 100;
            lblYearToYearDiffPercentageEndOfMonthWeighted.Text = dcYearToYearDiffPercentageEndOfMonthWeighted.ToString("0") + "%";

            if (dcLastYTDMinusOneYearEndOfMonthAmountTotal == 0)
            {
                dcLastYTDMinusOneYearEndOfMonthAmountTotal = 1;
            }

            Label lblYearToYearDiffPercentageEndOfMonthMinusOneWeighted = (Label)e.Row.FindControl("lblYearToYearDiffPercentageEndOfMonthMinusOneWeighted");
            dcYearToYearDiffPercentageEndOfMonthMinusOneWeighted = ((dcLastYTDEndOfMonthTotal - dcLastYTDMinusOneYearEndOfMonthAmountTotal) / dcLastYTDMinusOneYearEndOfMonthAmountTotal) * 100;
            lblYearToYearDiffPercentageEndOfMonthMinusOneWeighted.Text = dcYearToYearDiffPercentageEndOfMonthMinusOneWeighted.ToString("0") + "%";

            //Totals sums...

            Label lblCurrentYTDAmountPercentageOfTotalSum = (Label)e.Row.FindControl("lblCurrentYTDAmountPercentageOfTotalSum");
            if (dcCurrentYTDAmountPercentageOfTotalSum > 98)
            {
                dcCurrentYTDAmountPercentageOfTotalSum = 100;
            }
            lblCurrentYTDAmountPercentageOfTotalSum.Text = dcCurrentYTDAmountPercentageOfTotalSum.ToString("0") + "%";

            if (dcLastYTDAmountPercentageOfTotalSum > 98)
            {
                dcLastYTDAmountPercentageOfTotalSum = 100;
            }
            Label lblLastYTDAmountPercentageOfTotalSum = (Label)e.Row.FindControl("lblLastYTDAmountPercentageOfTotalSum");
            lblLastYTDAmountPercentageOfTotalSum.Text = dcLastYTDAmountPercentageOfTotalSum.ToString("0") + "%";
            if (dcCurrentYTDEndOfMonthAmountPercentageOfTotalSum > 98)
            {
                dcCurrentYTDEndOfMonthAmountPercentageOfTotalSum = 100;
            }

            Label lblCurrentYTDEndOfMonthAmountPercentageOfTotalSum = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthAmountPercentageOfTotalSum");
            lblCurrentYTDEndOfMonthAmountPercentageOfTotalSum.Text = dcCurrentYTDEndOfMonthAmountPercentageOfTotalSum.ToString("0") + "%";

            if (dcLastYTDEndOfMonthAmountPercentageOfTotalSum > 98)
            {
                dcLastYTDEndOfMonthAmountPercentageOfTotalSum = 100;
            }
            Label lblLastYTDEndOfMonthAmountPercentageOfTotalSum = (Label)e.Row.FindControl("lblLastYTDEndOfMonthAmountPercentageOfTotalSum");
            lblLastYTDEndOfMonthAmountPercentageOfTotalSum.Text = dcLastYTDEndOfMonthAmountPercentageOfTotalSum.ToString("0") + "%";

            e.Row.Cells[1].Visible = true;//LM
            e.Row.Cells[2].Visible = true;//LMLY

            if (!chkShowYTDthroughLastMonth.Checked)
            {

                e.Row.Cells[3].Visible = false;//LMTD
                e.Row.Cells[4].Visible = false;//LYLMTD
                e.Row.Cells[5].Visible = true;//MTD
                e.Row.Cells[6].Visible = true;//LYMTD

                //<% --YTD-- %>
                e.Row.Cells[7].Visible = true;//CM
                e.Row.Cells[8].Visible = true;//LYCM
                e.Row.Cells[9].Visible = true;//YTD
                e.Row.Cells[10].Visible = true;//LYTD
                e.Row.Cells[11].Visible = true;//LLYTD - New 1-28-2020
                e.Row.Cells[12].Visible = false;//YR to YR Diff -- Hide 11-10-2021
                e.Row.Cells[13].Visible = false;//YR to YR Diff Minus One -- Hide 11-10-2021
                e.Row.Cells[14].Visible = true;//YTD MARGIN
                e.Row.Cells[15].Visible = true;//LYTD MARGIN

                //<% --YTD End of Month-- %>
                e.Row.Cells[16].Visible = false;//YTDEM
                e.Row.Cells[17].Visible = false;//LYTDEM
                e.Row.Cells[18].Visible = false;//LLYTDEM
                e.Row.Cells[19].Visible = false;//YR to YR Diff EM -- Hide 11-10-2021
                e.Row.Cells[20].Visible = false;//YR to YR Diff EM Minus One -- Hide 11-10-2021

                e.Row.Cells[21].Visible = true;//CUR MO MARGIN -- new 11-10-2021...
                e.Row.Cells[22].Visible = true;//LAST MO MARGIN -- new 11-10-2021...

                e.Row.Cells[23].Visible = false;//YTD MARGIN EM
                e.Row.Cells[24].Visible = false;//LYTD MARGIN EM
                e.Row.Cells[25].Visible = false;//YTD % EM
                e.Row.Cells[26].Visible = false;//LYTD % EM
            }
            else//ShowYTDthroughLastMonth
            {
                e.Row.Cells[3].Visible = false;//LMTD
                e.Row.Cells[4].Visible = false;//LYLMTD
                e.Row.Cells[5].Visible = false;//MTD
                e.Row.Cells[6].Visible = false;//LYMTD

                //<% --YTD-- %>
                e.Row.Cells[7].Visible = false;//CM
                e.Row.Cells[8].Visible = false;//LYCM
                e.Row.Cells[9].Visible = false;//YTD
                e.Row.Cells[10].Visible = false;//LYTD
                e.Row.Cells[11].Visible = false;//LLYTD - New 1-28-2020
                e.Row.Cells[12].Visible = false;//YR to YR Diff -- Hide 11-10-2021
                e.Row.Cells[13].Visible = false;//YR to YR Diff Minus One -- Hide 11-10-2021
                e.Row.Cells[14].Visible = false;//YTD MARGIN
                e.Row.Cells[15].Visible = false;//LYTD MARGIN
                //<% --YTD End of Month-- %>
                e.Row.Cells[16].Visible = true;//YTDEM
                e.Row.Cells[17].Visible = true;//LYTDEM
                e.Row.Cells[18].Visible = true;//LLYTDEM
                e.Row.Cells[19].Visible = false;//YR to YR Diff EM -- Hide 11-10-2021
                e.Row.Cells[20].Visible = false;//YR to YR Diff EM Minus One -- Hide 11-10-2021

                e.Row.Cells[21].Visible = true;//CUR MO MARGIN -- new 11-10-2021...
                e.Row.Cells[22].Visible = true;//LAST MO MARGIN -- new 11-10-2021...

                e.Row.Cells[23].Visible = true;//YTD MARGIN EM
                e.Row.Cells[24].Visible = true;//LYTD MARGIN EM
                e.Row.Cells[25].Visible = true;//YTD % EM
                e.Row.Cells[26].Visible = true;//LYTD % EM
            }

            if (!chkShowLastColumns.Checked)
            {
                e.Row.Cells[27].Visible = false;
                e.Row.Cells[28].Visible = false;
                e.Row.Cells[29].Visible = false;
            }

        }
    }
    protected void gvTopFifteen_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        dtSortTable = (DataTable)Session["dtTopFifteen"];
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvTopFifteen.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            Session["Sort"] = e.SortExpression + " " + m_SortDirection;
            gvTopFifteen.DataSource = m_DataView;
            gvTopFifteen.DataBind();
            gvTopFifteen.PageIndex = m_PageIndex;
            Session["dtSortAdd"] = m_DataTable;
        }
    }
    protected void gvGoalPace_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcCurrentYTD = 0;


        decimal dcLastYTD = 0;
        decimal dcLastYTDMinusOne = 0;

        decimal dcCurrentMonthAmount = 0;
        decimal dcCurrentMonthCost = 0;
        decimal dcLastYearCurrentMonth = 0;

        decimal dcPreviousMonthAmount = 0;
        decimal dcPreviousMonthCost = 0;
        decimal dcLastYearLastMonth = 0;

        decimal dcYTDCost = 0;
        decimal dcLastYTDCost = 0;

        decimal dcYTDWeightedMargin = 0;
        decimal dcLastYTDWeightedMargin = 0;

        decimal dcCurrentYTDAmountPercentageOfTotal = 0;
        decimal dcLastYTDAmountPercentageOfTotal = 0;

        //Possible Future weighted avg...
        decimal dcCurrentMonthWeightedMarginGoalPace = 0;
        decimal dcLastMonthWeightedMarginGoalPace = 0;

        if (e.Row.RowType == DataControlRowType.Header)
        {

            DateTime dtToday = DateTime.Today;
            DateTime dtCurrentMonthFirstDay = new DateTime(dtToday.Year, dtToday.Month, 1);
            DateTime dtCurrentMonthFirstDayLastYear = new DateTime(dtToday.AddYears(-1).Year, dtToday.Month, 1);
            DateTime dtCurrentMonthLastDay = new DateTime(dtToday.Year, dtToday.Month, DateTime.DaysInMonth(dtToday.Year, dtToday.Month));
            DateTime dtCurrentMonthCurrentDay = new DateTime(dtToday.Year, dtToday.Month, dtToday.Day);
            DateTime dtCurrentMonthCurrentDayLastYear = new DateTime(2000, 1, 1);
            try
            {
                dtCurrentMonthCurrentDayLastYear = new DateTime(dtToday.AddYears(-1).Year, dtToday.Month, dtToday.Day);
            }
            catch (Exception)
            {//Leap Year Fix...
                dtCurrentMonthCurrentDayLastYear = new DateTime(dtToday.AddYears(-1).Year, dtToday.Month, 28);
            }

            DateTime dtFirstDayOfCurrentYear = new DateTime(DateTime.Now.Year, 1, 1);
            string sFirstDayOfCurrentYear = dtFirstDayOfCurrentYear.ToShortDateString();
            DateTime dtFirstDayOfLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);
            string sFirstDayOfLastYear = dtFirstDayOfLastYear.ToShortDateString();
            DateTime dtFirstDayOfLastYearMinusOne = new DateTime(DateTime.Now.AddYears(-2).Year, 1, 1);
            string sFirstDayOfLastYearMinusOne = dtFirstDayOfLastYearMinusOne.ToShortDateString();
            string sYTD = DateTime.Now.ToShortDateString();
            string sLYTD = DateTime.Now.AddDays(-365).ToShortDateString();
            string sMTD = DateTime.Now.ToShortDateString();
            DateTime dtFirstDayOfMonthCurrentYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            string sFirstDayOfMonthCurrentYear = dtFirstDayOfMonthCurrentYear.ToShortDateString();

            DateTime dtFirstDayOfLastMonthCurrentYear = dtFirstDayOfMonthCurrentYear.AddMonths(-1);//First Day of Last Month Period!
            string sFirstDayOfLastMonthCurrentYear = dtFirstDayOfLastMonthCurrentYear.ToShortDateString();

            DateTime dtLastDayOfLastMonthCurrentYear = new DateTime(dtFirstDayOfMonthCurrentYear.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month));//Last Day of Last Month period!
            string sLastDayOfLastMonthCurrentYear = dtLastDayOfLastMonthCurrentYear.ToShortDateString();

            DateTime dtFirstDayOfMonthLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1).Month, 1);
            string sFirstDayOfMonthLastYear = dtFirstDayOfMonthLastYear.ToShortDateString();


            DateTime dtFirstDayOfLastMonthLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddMonths(-1).AddYears(-1).Month, 1);
            string sFirstDayOfLastMonthLastYear = dtFirstDayOfLastMonthLastYear.ToShortDateString();

            DateTime dtLastDayOfMonthCurrentYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            string sLastDayOfMonthCurrentYear = dtLastDayOfMonthCurrentYear.ToShortDateString();
            DateTime dtLastDayOfMonthLastYear = dtLastDayOfMonthCurrentYear.AddYears(-1);
            string sLastDayOfMonthLastYear = dtLastDayOfMonthLastYear.ToShortDateString();

            DateTime dtLastDayOfLastMonthLastYear = dtLastDayOfLastMonthCurrentYear.AddYears(-1);
            string sLastDayOfLastMonthLastYear = dtLastDayOfLastMonthLastYear.ToShortDateString();

            DateTime dtLastDayOfLastMonthLastYearMinusOne = dtLastDayOfLastMonthCurrentYear.AddYears(-2);
            string sLastDayOfLastMonthLastYearMinusOne = dtLastDayOfLastMonthLastYearMinusOne.ToShortDateString();

            string sLastYearMTD = DateTime.Now.AddYears(-1).ToShortDateString();

            int iYear = DateTime.Now.Year;
            DateTime dtLastDayOfCurrentYear = new DateTime(iYear, 12, 31);
            DateTime dtLastMonthLastDay = new DateTime(iYear, DateTime.Now.AddMonths(-1).Month, DateTime.DaysInMonth(iYear, DateTime.Now.AddMonths(-1).Month));


            DateTime dtLastMonthCurrentYear = dtLastMonthLastDay;
            string sLastMonthCurrentYear = dtLastMonthLastDay.ToShortDateString();

            DateTime dtLastMonthLastYear = dtLastMonthLastDay.AddYears(-1);
            string sLastMonthLastYear = dtLastMonthLastYear.ToShortDateString();

            DateTime dtLastMonthLastYearMinusOne = dtLastMonthLastDay.AddYears(-2);
            string sLastMonthLastYearMinusOne = dtLastMonthLastYearMinusOne.ToShortDateString();


            DateTime dtLastDayofCurrentYear = dtLastDayOfCurrentYear;
            string sLastDayofCurrentYear = dtLastDayofCurrentYear.ToShortDateString();

            DateTime dtLastDayOfLastYear = dtLastDayOfCurrentYear.AddYears(-1);
            string sLastDayOfLastYear = dtLastDayOfLastYear.ToShortDateString();

            DateTime dtLastDayOfLastYearMinusOne = dtLastDayOfCurrentYear.AddYears(-2);
            string sLastDayOfLastYearMinusOne = dtLastDayOfLastYearMinusOne.ToShortDateString();

            //Amounts...
            Label lblPreviousMonthAmountHeader = (Label)e.Row.FindControl("lblPreviousMonthAmountHeader");//LM            
            lblPreviousMonthAmountHeader.Text = DateTime.Now.AddMonths(-1).ToString("MMM yyyy").ToUpper().Replace(" ", "<br>");

            Label lbLastYearPreviousMonthAmountHeader = (Label)e.Row.FindControl("lbLastYearPreviousMonthAmountHeader");//LMLY            
            lbLastYearPreviousMonthAmountHeader.Text = DateTime.Now.AddMonths(-1).AddYears(-1).ToString("MMM yyyy").ToUpper().Replace(" ", "<br>");

            Label lblCurrentMonthAmountHeader = (Label)e.Row.FindControl("lblCurrentMonthAmountHeader");//CM            
            lblCurrentMonthAmountHeader.Text = DateTime.Now.AddMonths(0).ToString("MMM yyyy").ToUpper().Replace(" ", "<br>");

            Label lblLastYearCurrentMonthAmountHeader = (Label)e.Row.FindControl("lblLastYearCurrentMonthAmountHeader");//CMLY             
            lblLastYearCurrentMonthAmountHeader.Text = DateTime.Now.AddMonths(0).AddYears(-1).ToString("MMM yyyy").ToUpper().Replace(" ", "<br>");

            //YTDs
            Label lblCurrentYTDAmountHeader = (Label)e.Row.FindControl("lblCurrentYTDAmountHeader");//YTD           
            lblCurrentYTDAmountHeader.Text = DateTime.Now.Year.ToString() + " YTD";

            Label lblLastYTDAmountHeader = (Label)e.Row.FindControl("lblLastYTDAmountHeader");//LYYTD
            lblLastYTDAmountHeader.Text = DateTime.Now.AddYears(-1).Year.ToString() + " YTD";

            Label lblLastYTDMinusOneAmountHeader = (Label)e.Row.FindControl("lblLastYTDMinusOneAmountHeader");//2 Years Ago YTD
            lblLastYTDMinusOneAmountHeader.Text = DateTime.Now.AddYears(-2).Year.ToString() + " YTD";

            //% of Total
            Label lblCurrentYTDAmountPercentageOfTotalHeader = (Label)e.Row.FindControl("lblCurrentYTDAmountPercentageOfTotalHeader");
            lblCurrentYTDAmountPercentageOfTotalHeader.Text = DateTime.Now.Year.ToString() + " %" + "<br>YTD";

            Label lblLastYTDAmountPercentageOfTotalHeader = (Label)e.Row.FindControl("lblLastYTDAmountPercentageOfTotalHeader");
            lblLastYTDAmountPercentageOfTotalHeader.Text = DateTime.Now.AddYears(-1).Year.ToString() + " %" + "<br>YTD";

            //Margins...
            Label lblCurrentMonthMarginHeader = (Label)e.Row.FindControl("lblCurrentMonthMarginHeader");
            lblCurrentMonthMarginHeader.Text = sFirstDayOfMonthCurrentYear + "<br>to<br>" + sLastDayOfMonthCurrentYear + "<br>MARGIN";

            Label lblLastMonthMarginHeader = (Label)e.Row.FindControl("lblLastMonthMarginHeader");
            lblLastMonthMarginHeader.Text = sFirstDayOfLastMonthCurrentYear + "<br>to<br>" + sLastDayOfLastMonthCurrentYear + "<br>MARGIN";

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //YTD...
            Label lblCurrentYTDAmountD = (Label)e.Row.FindControl("lblCurrentYTDAmountD");
            if (lblCurrentYTDAmountD.Text != "")
            {
                dcCurrentYTD = Convert.ToDecimal(lblCurrentYTDAmountD.Text.Replace("$", ""));
                dcCurrentYTDTotalGoalPace += dcCurrentYTD;
                lblCurrentYTDAmountD.Text = "$" + dcCurrentYTD.ToString("#,0");
            }

            //Last Year...
            Label lblLastYTDAmountD = (Label)e.Row.FindControl("lblLastYTDAmountD");
            if (lblLastYTDAmountD.Text != "")
            {
                dcLastYTD = Convert.ToDecimal(lblLastYTDAmountD.Text.Replace("$", ""));
                dcLastYTDTotalGoalPace += dcLastYTD;
                lblLastYTDAmountD.Text = "$" + dcLastYTD.ToString("#,0");
            }
            //Two years back...
            Label lblLastYTDMinusOneAmountD = (Label)e.Row.FindControl("lblLastYTDMinusOneAmountD");
            if (lblLastYTDMinusOneAmountD.Text != "")
            {
                dcLastYTDMinusOne = Convert.ToDecimal(lblLastYTDMinusOneAmountD.Text.Replace("$", ""));
                dcLastYTDMinusOneTotalGoalPace += dcLastYTDMinusOne;
                lblLastYTDMinusOneAmountD.Text = "$" + dcLastYTDMinusOne.ToString("#,0");
            }

            //New 11-11-2021...
            Label lblCurrentMonthMargin = (Label)e.Row.FindControl("lblCurrentMonthMargin");
            lblCurrentMonthMargin.Text = lblCurrentMonthMargin.Text + "%";

            Label lblLastMonthMargin = (Label)e.Row.FindControl("lblLastMonthMargin");
            lblLastMonthMargin.Text = lblLastMonthMargin.Text + "%";



            //Entire Current Month...
            Label lblCurrentMonthAmountD = (Label)e.Row.FindControl("lblCurrentMonthAmountD");
            if (lblCurrentMonthAmountD.Text != "")
            {
                dcCurrentMonthAmount = Convert.ToDecimal(lblCurrentMonthAmountD.Text.Replace("$", ""));
                dcCurrentMonthTotalGoalPace += dcCurrentMonthAmount;
                lblCurrentMonthAmountD.Text = "$" + dcCurrentMonthAmount.ToString("#,0");
            }

            Label lblLastYearCurrentMonthAmountD = (Label)e.Row.FindControl("lblLastYearCurrentMonthAmountD");
            if (lblLastYearCurrentMonthAmountD.Text != "")
            {
                dcLastYearCurrentMonth = Convert.ToDecimal(lblLastYearCurrentMonthAmountD.Text.Replace("$", ""));
                dcLastYearCurrentMonthTotalGoalPace += dcLastYearCurrentMonth;
                lblLastYearCurrentMonthAmountD.Text = "$" + dcLastYearCurrentMonth.ToString("#,0");
            }

            //Entire Previous Month...
            Label lblPreviousMonthAmountD = (Label)e.Row.FindControl("lblPreviousMonthAmountD");
            if (lblPreviousMonthAmountD.Text != "")
            {
                dcPreviousMonthAmount = Convert.ToDecimal(lblPreviousMonthAmountD.Text.Replace("$", ""));
                dcPreviousMonthTotalGoalPace += dcPreviousMonthAmount;
                lblPreviousMonthAmountD.Text = "$" + dcPreviousMonthAmount.ToString("#,0");
            }

            //New 11-10-2021...
            Label lblCurrentMonthCost = (Label)e.Row.FindControl("lblCurrentMonthCost");
            if (lblCurrentMonthCost.Text != "")
            {
                dcCurrentMonthCost = Convert.ToDecimal(lblCurrentMonthCost.Text.Replace("$", ""));
                dcCurrentMonthCostTotalGoalPace += dcCurrentMonthCost;
                lblCurrentMonthCost.Text = "$" + dcCurrentMonthCost.ToString("#,0");
            }
            Label lblPreviousMonthCost = (Label)e.Row.FindControl("lblPreviousMonthCost");
            if (lblPreviousMonthCost.Text != "")
            {
                dcPreviousMonthCost = Convert.ToDecimal(lblPreviousMonthCost.Text.Replace("$", ""));
                dcPreviousMonthCostTotalGoalPace += dcPreviousMonthCost;
                lblPreviousMonthCost.Text = "$" + dcPreviousMonthCost.ToString("#,0");
            }

            Label lblLastYearPreviousMonthAmountD = (Label)e.Row.FindControl("lblLastYearPreviousMonthAmountD");
            if (lblLastYearPreviousMonthAmountD.Text != "")
            {
                dcLastYearLastMonth = Convert.ToDecimal(lblLastYearPreviousMonthAmountD.Text.Replace("$", ""));
                dcLastYearPreviousMonthTotalGoalPace += dcLastYearLastMonth;
                lblLastYearPreviousMonthAmountD.Text = "$" + dcLastYearLastMonth.ToString("#,0");
            }

            //YTD...
            Label lblCurrentYTDCost = (Label)e.Row.FindControl("lblCurrentYTDCost");
            if (lblCurrentYTDCost.Text != "")
            {
                dcYTDCost = Convert.ToDecimal(lblCurrentYTDCost.Text.Replace("$", ""));
                dcYTDCostTotalGoalPace += dcYTDCost;
            }

            Label lblLastYTDCost = (Label)e.Row.FindControl("lblLastYTDCost");
            if (lblLastYTDCost.Text != "")
            {
                dcLastYTDCost = Convert.ToDecimal(lblLastYTDCost.Text.Replace("$", ""));
                dcLastYTDCostTotalGoalPace += dcLastYTDCost;
            }

            Label lblCurrentYTDMargin = (Label)e.Row.FindControl("lblCurrentYTDMargin");
            lblCurrentYTDMargin.Text = lblCurrentYTDMargin.Text + "%";
            Label lblLastYTDMargin = (Label)e.Row.FindControl("lblLastYTDMargin");
            lblLastYTDMargin.Text = lblLastYTDMargin.Text + "%";

            Label lblCurrentYTDAmountPercentageOfTotal = (Label)e.Row.FindControl("lblCurrentYTDAmountPercentageOfTotal");
            dcCurrentYTDAmountPercentageOfTotal = Convert.ToDecimal(lblCurrentYTDAmountPercentageOfTotal.Text);
            dcCurrentYTDAmountPercentageOfTotalSumGoalPace += dcCurrentYTDAmountPercentageOfTotal;
            lblCurrentYTDAmountPercentageOfTotal.Text = lblCurrentYTDAmountPercentageOfTotal.Text + "%";

            Label lblLastYTDAmountPercentageOfTotal = (Label)e.Row.FindControl("lblLastYTDAmountPercentageOfTotal");
            dcLastYTDAmountPercentageOfTotal = Convert.ToDecimal(lblLastYTDAmountPercentageOfTotal.Text);
            dcLastYTDAmountPercentageOfTotalSumGoalPace += dcLastYTDAmountPercentageOfTotal;
            lblLastYTDAmountPercentageOfTotal.Text = lblLastYTDAmountPercentageOfTotal.Text + "%";



            Label lblName = (Label)e.Row.FindControl("lblName");
            switch (lblName.Text.Trim())
            {
                case "SMASHBURGER":
                    lblName.ForeColor = Color.Red;
                    break;
                case "KRISPY KREME":
                    lblName.ForeColor = Color.Red;
                    break;
                case "IN & OUT":
                    lblName.ForeColor = Color.Blue;
                    break;
                case "BAKEMARK":
                    lblName.ForeColor = Color.Blue;
                    break;
                case "SYSCO":
                    lblName.ForeColor = Color.Blue;
                    break;
                case "ARYZTA":
                    lblName.ForeColor = Color.Blue;
                    break;
                case "US FOODS":
                    lblName.ForeColor = Color.Blue;
                    break;
                default:
                    break;
            }
            Label lblGoalPaceAmountDiffD = (Label)e.Row.FindControl("lblGoalPaceAmountDiffD");
            lblGoalPaceAmountDiffD.Text = Convert.ToDecimal(lblGoalPaceAmountDiffD.Text).ToString("c");
            Label lblGoalPaceAmountD = (Label)e.Row.FindControl("lblGoalPaceAmountD");
            lblGoalPaceAmountD.Text = Convert.ToDecimal(lblGoalPaceAmountD.Text).ToString("c");

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            //YTD...
            Label lblCurrentYTDAmountTotal = (Label)e.Row.FindControl("lblCurrentYTDAmountTotal");
            lblCurrentYTDAmountTotal.Text = "$" + dcCurrentYTDTotalGoalPace.ToString("#,0");

            Label lblLastYTDAmountTotal = (Label)e.Row.FindControl("lblLastYTDAmountTotal");
            lblLastYTDAmountTotal.Text = "$" + dcLastYTDTotalGoalPace.ToString("#,0");

            Label lblLastYTDMinusOneAmountTotal = (Label)e.Row.FindControl("lblLastYTDMinusOneAmountTotal");
            lblLastYTDMinusOneAmountTotal.Text = "$" + dcLastYTDMinusOneTotalGoalPace.ToString("#,0");



            //Entire Current Month...
            Label lblCurrentMonthAmountTotal = (Label)e.Row.FindControl("lblCurrentMonthAmountTotal");
            lblCurrentMonthAmountTotal.Text = "$" + dcCurrentMonthTotalGoalPace.ToString("#,0");

            Label lblLastYearCurrentMonthAmountTotal = (Label)e.Row.FindControl("lblLastYearCurrentMonthAmountTotal");
            lblLastYearCurrentMonthAmountTotal.Text = "$" + dcLastYearCurrentMonthTotalGoalPace.ToString("#,0");


            //Entire Previous Month...
            Label lblPreviousMonthAmountTotal = (Label)e.Row.FindControl("lblPreviousMonthAmountTotal");
            lblPreviousMonthAmountTotal.Text = "$" + dcPreviousMonthTotalGoalPace.ToString("#,0");

            Label lblLastYearPreviousMonthAmountTotal = (Label)e.Row.FindControl("lblLastYearPreviousMonthAmountTotal");
            lblLastYearPreviousMonthAmountTotal.Text = "$" + dcLastYearPreviousMonthTotalGoalPace.ToString("#,0");

            //New 11-10-2021...
            //Current Month Margin...
            Label lblCurrentMonthMarginWeighted = (Label)e.Row.FindControl("lblCurrentMonthMarginWeighted");
            if (dcCurrentMonthTotalGoalPace == 0)
            {
                dcCurrentMonthTotalGoalPace = 1;
            }
            dcCurrentMonthWeightedMarginGoalPace = ((dcCurrentMonthTotalGoalPace - dcCurrentMonthCostTotalGoalPace) / dcCurrentMonthTotalGoalPace) * 100;
            lblCurrentMonthMarginWeighted.Text = dcCurrentMonthWeightedMarginGoalPace.ToString("0.0") + "%";

            //Last Month Margin...
            Label lblLastMonthMarginWeighted = (Label)e.Row.FindControl("lblLastMonthMarginWeighted");
            if (dcPreviousMonthTotalGoalPace == 0)
            {
                dcPreviousMonthTotalGoalPace = 1;
            }
            dcLastMonthWeightedMarginGoalPace = ((dcPreviousMonthTotalGoalPace - dcPreviousMonthCostTotalGoalPace) / dcPreviousMonthTotalGoalPace) * 100;
            lblLastMonthMarginWeighted.Text = dcLastMonthWeightedMarginGoalPace.ToString("0.0") + "%";


            //YTD...
            Label lblCurrentYTDMarginWeighted = (Label)e.Row.FindControl("lblCurrentYTDMarginWeighted");
            if (dcCurrentYTDTotalGoalPace == 0)
            {
                dcCurrentYTDTotalGoalPace = 1;
            }
            dcYTDWeightedMargin = ((dcCurrentYTDTotalGoalPace - dcYTDCostTotalGoalPace) / dcCurrentYTDTotalGoalPace) * 100;
            lblCurrentYTDMarginWeighted.Text = dcYTDWeightedMargin.ToString("0") + "%";

            Label lblLastYTDAmountMarginWeighted = (Label)e.Row.FindControl("lblLastYTDAmountMarginWeighted");
            if (dcLastYTDTotalGoalPace == 0)
            {
                dcLastYTDTotalGoalPace = 1;
            }

            dcLastYTDWeightedMargin = ((dcLastYTDTotalGoalPace - dcLastYTDCostTotalGoalPace) / dcLastYTDTotalGoalPace) * 100;
            lblLastYTDAmountMarginWeighted.Text = dcLastYTDWeightedMargin.ToString("0") + "%";

            Label lblYearToYearDiffPercentageMinusOneWeighted = (Label)e.Row.FindControl("lblYearToYearDiffPercentageMinusOneWeighted");
            if (dcLastYTDMinusOneTotalGoalPace == 0)
            {
                dcLastYTDMinusOneTotalGoalPace = 1;
            }
            //Totals sums...

            Label lblCurrentYTDAmountPercentageOfTotalSum = (Label)e.Row.FindControl("lblCurrentYTDAmountPercentageOfTotalSum");
            if (dcCurrentYTDAmountPercentageOfTotalSumGoalPace > 98)
            {
                dcCurrentYTDAmountPercentageOfTotalSumGoalPace = 100;
            }
            lblCurrentYTDAmountPercentageOfTotalSum.Text = dcCurrentYTDAmountPercentageOfTotalSumGoalPace.ToString("0") + "%";

            if (dcLastYTDAmountPercentageOfTotalSumGoalPace > 98)
            {
                dcLastYTDAmountPercentageOfTotalSumGoalPace = 100;
            }
            Label lblLastYTDAmountPercentageOfTotalSum = (Label)e.Row.FindControl("lblLastYTDAmountPercentageOfTotalSum");
            lblLastYTDAmountPercentageOfTotalSum.Text = dcLastYTDAmountPercentageOfTotalSumGoalPace.ToString("0") + "%";

        }
    }
    protected void gvGoalPace_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        dtSortTable = (DataTable)Session["dtGoalPace"];
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvTopFifteen.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            Session["Sort"] = e.SortExpression + " " + m_SortDirection;
            gvTopFifteen.DataSource = m_DataView;
            gvTopFifteen.DataBind();
            gvTopFifteen.PageIndex = m_PageIndex;
            Session["dtSortAddGoalPace"] = m_DataTable;
        }
    }
    protected void btnExportSummary_Click(object sender, EventArgs e)
    {
        ExportTopFifteenList();

    }
    protected void btnExportSummaryGoalPace_Click(object sender, EventArgs e)
    {
        ExportGoalPaceList();
    }
    protected void chkShowLastColumns_CheckedChanged(object sender, EventArgs e)
    {
        LoadTopFifteenData(rblTopTen.SelectedValue);
    }
    protected void chkMonthToDate_CheckedChanged(object sender, EventArgs e)
    {
        LoadTopFifteenData(rblTopTen.SelectedValue);
    }
    protected void chkShowYTDthroughLastMonth_CheckedChanged(object sender, EventArgs e)
    {
        LoadTopFifteenData(rblTopTen.SelectedValue);
    }
    #endregion





}