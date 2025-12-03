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

public partial class UnitCostReport : System.Web.UI.Page
{
    decimal dcActualUnitCostC = 0;
    decimal dcWiUnitCostC = 0;
    decimal dcWiLabCostC = 0;
    decimal dcWiMatCostC = 0;
    decimal dcLabCostMatC = 0;
    decimal dcLabCostPkgC = 0;
    decimal dcTotalLabCostC = 0;
    decimal dcNewTotalC = 0;

    decimal dcActualUnitCostI = 0;
    decimal dcWiUnitCostI = 0;
    decimal dcWiLabCostI = 0;
    decimal dcWiMatCostI = 0;
    decimal dcLabCostMatI = 0;
    decimal dcLabCostPkgI = 0;
    decimal dcTotalLabCostI = 0;
    decimal dcNewTotalI = 0;

    #region Subs

    private void SyncGridViews()
    {
        decimal dcNewAmtTotal = 0;
        decimal dcQtyPer = 0;


        foreach (GridViewRow grvRow in gvIngredients.Rows)
        {
            if (grvRow.RowType == DataControlRowType.DataRow)
            {

                decimal dcNewAmt = 0;
                Label lblNewCostLabel = (Label)grvRow.FindControl("lblNewCost");
                if (lblNewCostLabel.Text != "")
                {
                    dcNewAmt = Convert.ToDecimal(lblNewCostLabel.Text);
                    dcNewAmtTotal += dcNewAmt;
                }

            }
        }
        try
        {
            gvIngredients.FooterRow.Cells[8].Text = dcNewAmtTotal.ToString("0.00");
        }
        catch (Exception)
        {


        }


        foreach (GridViewRow grvRow in gvReportCondensed.Rows)
        {
            if (grvRow.RowType == DataControlRowType.DataRow)
            {
                // decimal dcNewAmt = 0;
                Label lblQtyPerSummary = (Label)grvRow.FindControl("lblQtyPer");
                dcQtyPer = Convert.ToDecimal(lblQtyPerSummary.Text);
                //dcNewAmt = dcQtyPer * dcNewAmtTotal;//QtyPer * Total Of Recipt Grid New UOM Cost...
                Label lblActualUnitCost = (Label)grvRow.FindControl("lblActualUnitCost");
                Label lblPackagingCost = (Label)grvRow.FindControl("lblPackagingCost");
                // lblActualUnitCost.Text = dcNewAmt.ToString("0.00");//Override the ActualCost from spGetUnitCostReport...
                Label lblTotal = (Label)grvRow.FindControl("lblTotal");
                if (lblPackagingCost.Text != "" && lblActualUnitCost.Text != "")
                {
                    lblTotal.Text = (Convert.ToDecimal(lblActualUnitCost.Text) + Convert.ToDecimal(lblPackagingCost.Text)).ToString();

                }

            }
        }
    }
    private void Calculate()
    {//Called on submit...
        //Formulas: Margin = (Price - TotalCost) / Price
        //Markup = (Price - TotalCost) / TotalCost
        //Shrinkage = (Material + Pkg) * Shrinkage %
        decimal dcQtyPer = 0;
        decimal dcPrice = 0;
        decimal dcTotalCost = 0;
        decimal dcMargin = 0;
        decimal dcMarkup = 0;
        decimal dcShrinkAgePercentage = 0;
        decimal dcShrinkAge = 0;
        decimal dcRecipeCost = 0;
        decimal dcLaborCost = 0;
        decimal dcPackagingCost = 0;
        decimal dcShrinkagePlusLabor = 0;
        if (txtShrinkagePercentage.Text.Trim() == "")
        {
            dcShrinkAgePercentage = 2;
        }

        dcShrinkAgePercentage = Convert.ToDecimal(txtShrinkagePercentage.Text.Trim());

        if (txtPrice.Text.Trim() == "")
        {//Use default if blank...
            dcPrice = Convert.ToDecimal(Session["Price"]);
        }
        dcPrice = Convert.ToDecimal(txtPrice.Text.Trim());  //Could be user input...

        foreach (GridViewRow grvRow in gvReportCondensed.Rows)
        {//Summary grid...
            if (grvRow.RowType == DataControlRowType.DataRow)
            {
                //decimal dcNewAmt = 0;
                Label lblQtyPerSummary = (Label)grvRow.FindControl("lblQtyPer");
                dcQtyPer = Convert.ToDecimal(lblQtyPerSummary.Text);
                //dcNewAmt = dcTotalCost;//Don't multiply by QtyPer for Packaging...
                Label lblActualUnitCost = (Label)grvRow.FindControl("lblActualUnitCost");
                Label lblPackagingCost = (Label)grvRow.FindControl("lblPackagingCost");
                Label lblTotalLaborCost = (Label)grvRow.FindControl("lblTotalLaborCost");
                if (lblTotalLaborCost.Text != "")
                {
                    dcLaborCost = Convert.ToDecimal(lblTotalLaborCost.Text);
                }
                Label lblTotal = (Label)grvRow.FindControl("lblTotal");
                if (lblActualUnitCost.Text != "" && lblPackagingCost.Text != "" && lblTotalLaborCost.Text != "")
                {
                    lblTotal.Text = (Convert.ToDecimal(lblActualUnitCost.Text) + Convert.ToDecimal(lblPackagingCost.Text) + Convert.ToDecimal(lblTotalLaborCost.Text)).ToString();
                }
                if (lblTotal.Text != "")
                {
                    dcTotalCost = Convert.ToDecimal(lblTotal.Text);
                }
                if (lblActualUnitCost.Text != "")
                {
                    dcRecipeCost = Convert.ToDecimal(lblActualUnitCost.Text);
                }
                if (lblPackagingCost.Text != "")
                {
                    dcPackagingCost = Convert.ToDecimal(lblPackagingCost.Text);
                }

            }
        }


        dcShrinkAge = (((dcRecipeCost + dcPackagingCost) * dcShrinkAgePercentage) / 100) + (dcRecipeCost + dcPackagingCost);

        dcShrinkagePlusLabor = dcShrinkAge + dcLaborCost;
        dcTotalCost = dcShrinkagePlusLabor;
        dcMargin = ((dcPrice - dcTotalCost) / dcPrice) * 100;
        lblMargin.Text = dcMargin.ToString("0");

        dcMarkup = ((dcPrice - dcTotalCost) / dcTotalCost) * 100;
        lblMarkup.Text = dcMarkup.ToString("0");

        lblShrinkage.Text = dcShrinkagePlusLabor.ToString("#,0.00");

    }
    private void Calculate(decimal dcActualRecipeCost, decimal dcActualPackagingCost, decimal dcActualLaborCost, decimal dcTotalCost)
    {//called when user changes the Packaging or Recipe cost...
        //Formulas: Margin = (Price - TotalCost) / Price
        //Markup = (Price - TotalCost) / TotalCost
        //Shrinkage = (Material + Pkg) * Shrinkage %
        decimal dcQtyPer = 0;
        decimal dcPrice = 0;

        decimal dcMargin = 0;
        decimal dcMarkup = 0;
        decimal dcShrinkAgePercentage = 0;
        decimal dcShrinkAge = 0;
        decimal dcShrinkagePlusLabor = 0;

        if (txtShrinkagePercentage.Text.Trim() == "")
        {
            dcShrinkAgePercentage = 2;
        }
        else
        {
            dcShrinkAgePercentage = Convert.ToDecimal(txtShrinkagePercentage.Text.Trim());
        }


        if (txtPrice.Text.Trim() == "")
        {//Use default if blank...
            dcPrice = Convert.ToDecimal(Session["Price"]);
        }
        dcPrice = Convert.ToDecimal(txtPrice.Text.Trim());  //Could be user input...



        dcShrinkAge = (((dcActualRecipeCost + dcActualPackagingCost) * dcShrinkAgePercentage) / 100) + (dcActualRecipeCost + dcActualPackagingCost);

        dcShrinkagePlusLabor = dcShrinkAge + dcActualLaborCost;
        dcTotalCost = dcShrinkagePlusLabor;
        if (dcTotalCost == 0)
        {
            dcTotalCost = 1;
        }

        dcMargin = ((dcPrice - dcTotalCost) / dcPrice) * 100;
        lblMargin.Text = dcMargin.ToString("0.0");

        dcMarkup = ((dcPrice - dcTotalCost) / dcTotalCost) * 100;
        lblMarkup.Text = dcMarkup.ToString("0.00");

        lblShrinkage.Text = dcShrinkagePlusLabor.ToString("#,0.00");

    }
    //Cost Summary

    #endregion


    #region Functions
    //private DataRow AddWghtSumNsRow(DataTable NewSummary, decimal TotalParentQtyManuf, decimal TotalRecipeMatCostTotUsed, decimal TotalParentMatCostTotAdj, decimal TotalTotLabCost, decimal TotalTotCost)
    //{//WEIGHTED AVERAGE TOTAL
    //    DataRow dr = NewSummary.Rows.Add("A", "TOTALS", string.Empty, DateTime.MinValue, string.Empty, string.Empty, TotalParentQtyManuf,
    //     TotalRecipeMatCostTotUsed, TotalParentMatCostTotAdj, TotalTotLabCost, TotalTotCost,
    //     TotalRecipeMatCostTotUsed.ToString("#,0.00"), TotalParentMatCostTotAdj.ToString("#,0.00"), TotalTotLabCost.ToString("#,0.00"), TotalTotCost.ToString("#,0.00"));
    //    // dr.SetParentCompleteDateNull();
    //    return dr;
    //}
    //private DataRow AddPerUnitNsRow(DataTable NewSummary, decimal TotalParentQtyManuf, decimal TotalRecipeMatCostTotUsed, decimal TotalParentMatCostTotAdj, decimal TotalTotLabCost, decimal TotalTotCost)
    //{//COST PER UNIT...
    //    if (TotalParentQtyManuf == 0M)
    //    {
    //        lblError.Text = "Warning: The total parent qty manufactured is zero, so the 'COST PER UNIT' summary row may be inaccurate.";
    //        TotalParentQtyManuf = 1M;
    //    }
    //    decimal dUnitRecipeMatCostTotUsed = TotalRecipeMatCostTotUsed / TotalParentQtyManuf;
    //    decimal dUnitParentMatCostTotAdj = TotalParentMatCostTotAdj / TotalParentQtyManuf;
    //    decimal dUnitTotLabCost = TotalTotLabCost / TotalParentQtyManuf;
    //    decimal dUnitTotCost = TotalTotCost / TotalParentQtyManuf;
    //    DataRow dr = NewSummary.Rows.Add("U", "AVERAGE cost per unit", string.Empty, DateTime.MinValue, string.Empty, string.Empty, 0M,
    //     dUnitRecipeMatCostTotUsed, dUnitParentMatCostTotAdj, dUnitTotLabCost, dUnitTotCost,
    //     dUnitRecipeMatCostTotUsed.ToString("#,0.00"), dUnitParentMatCostTotAdj.ToString("#,0.00"), dUnitTotLabCost.ToString("#,0.00"), dUnitTotCost.ToString("#,0.00"));
    //    // dr.SetParentCompleteDateNull();
    //    // dr.SetParentQtyManufNull();
    //    return dr;
    //}
    //private DataRow AddWicpuRow(DataTable NewSummary, decimal TotalParentQtyManuf, decimal TotalWiRecipeMatCostTotUsed, decimal TotalWiParentMatCostTotAdj, decimal TotalWiTotLabCost, decimal TotalWiTotCost, int iRowCount)
    //{//WHAT-IF COST PER UNIT...Divide By to get Cost Per Unit...
    //    if (TotalParentQtyManuf == 0M)
    //    {
    //        lblError.Text = "Warning: The total parent qty manufactured is zero, so the 'WHAT-IF COST PER UNIT' summary row may be inaccurate.";
    //        TotalParentQtyManuf = 1M;
    //    }
    //    decimal dWiUnitRecipeMatCostTotUsed = TotalWiRecipeMatCostTotUsed / TotalParentQtyManuf;//Recipe...
    //    decimal dWiUnitParentMatCostTotAdj = TotalWiParentMatCostTotAdj / TotalParentQtyManuf;//Pkg...        
    //    decimal dWiUnitTotLabCost = TotalWiTotLabCost / TotalParentQtyManuf;//Labor...
    //    decimal dWiUnitTotCost = dWiUnitRecipeMatCostTotUsed + dWiUnitParentMatCostTotAdj + dWiUnitTotLabCost; //TotalWiTotCost / TotalParentQtyManuf;//Changed 7-8-14
    //    DataRow dr = NewSummary.Rows.Add("W", "WHAT-IF cost per unit", string.Empty, DateTime.MinValue, string.Empty, string.Empty, 0M,
    //     dWiUnitRecipeMatCostTotUsed, dWiUnitParentMatCostTotAdj, dWiUnitTotLabCost, dWiUnitTotCost,
    //     dWiUnitRecipeMatCostTotUsed.ToString("#,0.00"), dWiUnitParentMatCostTotAdj.ToString("#,0.00"), dWiUnitTotLabCost.ToString("#,0.00"), dWiUnitTotCost.ToString("#,0.00"));
    //    // dr.SetParentCompleteDateNull();
    //    //dr.SetParentQtyManufNull();
    //    return dr;
    //}
    //private DataRow AddVcpuRow(DataTable NewSummary, decimal TotalParentQtyManuf, decimal TotalVarRecipeMatCostTotUsed, decimal TotalVarParentMatCostTotAdj, decimal TotalVarTotLabCost, decimal TotalVarTotCost)
    //{//VARIANCE COST PER UNIT...Divide By to get Cost Per Unit...
    //    if (TotalParentQtyManuf == 0M)
    //    {
    //        lblError.Text = "Warning: The total parent qty manufactured is zero, so the 'VARIANCE COST PER UNIT' summary row may be inaccurate.";
    //        TotalParentQtyManuf = 1M;
    //    }
    //    decimal dVarUnitRecipeMatCostTotUsed = TotalVarRecipeMatCostTotUsed / TotalParentQtyManuf;
    //    decimal dVarUnitParentMatCostTotAdj = TotalVarParentMatCostTotAdj / TotalParentQtyManuf;
    //    decimal dVarUnitTotLabCost = TotalVarTotLabCost / TotalParentQtyManuf;
    //    decimal dVarUnitTotCost = TotalVarTotCost / TotalParentQtyManuf;
    //    DataRow dr = NewSummary.Rows.Add("V", "VARIANCE cost per unit", string.Empty, DateTime.MinValue, string.Empty, string.Empty, 0M,
    //     dVarUnitRecipeMatCostTotUsed, dVarUnitParentMatCostTotAdj, dVarUnitTotLabCost, dVarUnitTotCost,
    //     dVarUnitRecipeMatCostTotUsed.ToString("#,0.00"), dVarUnitParentMatCostTotAdj.ToString("#,0.00"), dVarUnitTotLabCost.ToString("#,0.00"), dVarUnitTotCost.ToString("#,0.00"));
    //    //dr.SetParentCompleteDateNull();
    //    //dr.SetParentQtyManufNull();
    //    return dr;
    //}
    //private DataRow AddBlankNsRow(DataTable NewSummary)
    //{
    //    DataRow dr = NewSummary.Rows.Add("B", "SPACE", string.Empty, DateTime.MinValue, string.Empty, string.Empty, 0M, 0M, 0M, 0M, 0M, string.Empty, string.Empty, string.Empty, string.Empty);
    //    return dr;
    //}
    private string GetPrice(string sStockCode)
    {
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        decimal dcPrice = 0;
        var query = (from p in db.InvPrice
                     where p.PriceCode == "A"
                     && p.StockCode == sStockCode
                     select new { p.SellingPrice });
        foreach (var a in query)
        {
            dcPrice = (decimal)a.SellingPrice;
        }
        if (dcPrice == 0)
        {
            var query1 = (from p in db.InvPrice
                          where p.PriceCode == "B"
                          && p.StockCode == sStockCode
                          select new { p.SellingPrice });
            foreach (var a in query1)
            {
                dcPrice = (decimal)a.SellingPrice;
            }
        }
        if (dcPrice == 0)
        {
            var query1 = (from p in db.InvPrice
                          where p.PriceCode == "C"
                          && p.StockCode == sStockCode
                          select new { p.SellingPrice });
            foreach (var a in query1)
            {
                dcPrice = (decimal)a.SellingPrice;
            }
        }
        if (dcPrice == 0)
        {
            var query1 = (from p in db.InvPrice
                          where p.PriceCode == "D"
                          && p.StockCode == sStockCode
                          select new { p.SellingPrice });
            foreach (var a in query1)
            {
                dcPrice = (decimal)a.SellingPrice;
            }
        }

        return dcPrice.ToString("#,0.00");
    }
    private string RunReport(string sStockCode)
    {
        string sMsg = "";


        DataSet ds = new DataSet();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        string sStartDate = "NULL";
        if (txtStartDate.Text.Trim() != "")
        {
            sStartDate = "'" + txtStartDate.Text.Trim() + "'";
        }
        string sEndDate = "NULL";
        if (txtEndDate.Text.Trim() != "")
        {
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
            return sMsg;
        }


        sSQL = "EXEC spGetUnitCost ";
        sSQL += "@FromDate=" + sStartDate + ",";
        sSQL += "@ToDate=" + sEndDate + ",";
        sSQL += "@StockCode=" + sStockCode;


        Debug.WriteLine(sSQL);


        ds = SharedFunctions.getDataSet(sSQL, conn, "ds");
        DataTable dt = ds.Tables[0];
        //foreach (DataRow Row in dt.Rows)
        //{
        //    foreach (DataColumn c in dt.Columns)
        //    {
        //        Debug.WriteLine(c.ColumnName + ": " + Row[c.ColumnName].ToString());
        //    }
        //    Debug.WriteLine("----------------------------------------------------");
        //}

        gvReportCondensed.DataSource = ds.Tables[0];
        gvReportCondensed.DataBind();

        gvComponents.DataSource = ds.Tables[2];
        gvComponents.DataBind();

        gvIngredients.DataSource = ds.Tables[1];
        gvIngredients.DataBind();

        try
        {

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            ds.Dispose();
        }
        return "";
    }
    private string RunEstimatedCostReport(string sStockCode)
    {
        string sMsg = "";
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        string sStartDate = "NULL";
        if (txtStartDate.Text.Trim() != "")
        {
            sStartDate = "'" + txtStartDate.Text.Trim() + "'";
        }
        string sEndDate = "NULL";
        if (txtEndDate.Text.Trim() != "")
        {
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
            return sMsg;
        }


        sSQL = "EXEC spGetEstimateCostForUnitCostReport ";
        sSQL += "@FromDate=" + sStartDate + ",";
        sSQL += "@ToDate=" + sEndDate + ",";
        sSQL += "@StockCode=" + sStockCode;


        Debug.WriteLine(sSQL);
        DataTable dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
        //foreach (DataRow Row in dt.Rows)
        //{
        //    foreach (DataColumn c in dt.Columns)
        //    {
        //        Debug.WriteLine(c.ColumnName + ": " + Row[c.ColumnName].ToString());
        //    }
        //    Debug.WriteLine("----------------------------------------------------");
        //}

        gvEstimatedCost.DataSource = dt;
        gvEstimatedCost.DataBind();


        try
        {

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            dt.Dispose();
        }
        return "";
    }
    private string RunCostReport(string sStockCode)
    {
        string sMsg = "";
        string sStartDate = "NULL";
        if (txtStartDate.Text.Trim() != "")
        {
            sStartDate = "'" + txtStartDate.Text.Trim() + "'";
        }
        string sEndDate = "NULL";
        if (txtEndDate.Text.Trim() != "")
        {
            sEndDate = "'" + txtEndDate.Text.Trim() + "'";
        }


        DataTable dtJd = new DataTable();
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
            return sMsg;
        }
        //switch (ddlPeriod.SelectedValue)
        //{
        //    case "All":
        //        sStartDate = "'" + DateTime.Now.AddYears(-5).ToShortDateString() + "'";//Five Years back...
        //        sEndDate = "'" + DateTime.Now.ToShortDateString() + "'";//today...
        //        break;
        //    case "Range":
        //        if (txtStartDate.Text.Trim() == "" && txtEndDate.Text.Trim() == "")
        //        {//Should ever get here...
        //            sStartDate = "'" + DateTime.Now.AddDays(-7).ToShortDateString() + "'";//one day back...
        //            sEndDate = "'" + DateTime.Now.ToShortDateString() + "'";//today...
        //        }
        //        else
        //        {
        //            sStartDate = "'" + txtStartDate.Text.Trim() + "'";
        //            sEndDate = "'" + txtEndDate.Text.Trim() + "'";
        //        }
        //        break;
        //    default://All Others...
        //        sStartDate = "'" + txtStartDate.Text.Trim() + "'";
        //        sEndDate = "'" + txtEndDate.Text.Trim() + "'";
        //        break;
        //}

        sSQL = "EXEC spGetWipCostAnalysis ";
        sSQL += "@FromDate=" + sStartDate + ",";
        sSQL += "@ToDate=" + sEndDate + ",";
        sSQL += "@FromStockCode=" + sStockCode + ",";
        sSQL += "@ToStockCode=null";


        Debug.WriteLine(sSQL);

        dtJd = SharedFunctions.getDataTable(sSQL, conn, "dtReportJd");
        Session["dtJd"] = dtJd;
        DataTable dtJs = new DataTable();
        // DataView dv = null;


        dtJs.Columns.Add("ParentJob", typeof(string));
        dtJs.Columns.Add("ParentPart", typeof(string));
        dtJs.Columns.Add("ParentDescription", typeof(string));
        dtJs.Columns.Add("ParentWh", typeof(string));
        dtJs.Columns.Add("ParentCompleteDate", typeof(DateTime));
        dtJs.Columns.Add("ParentIsComplete", typeof(bool));
        dtJs.Columns.Add("ParentQtyToMake", typeof(decimal));
        dtJs.Columns.Add("ParentQtyManuf", typeof(decimal));
        dtJs.Columns.Add("ParentConfirmed", typeof(bool));
        dtJs.Columns.Add("ParentOnHold", typeof(bool));
        dtJs.Columns.Add("ParentMatCostTotUnadj", typeof(decimal));
        dtJs.Columns.Add("WiParentMatCostTotUnadj", typeof(decimal));
        dtJs.Columns.Add("WiParentMatCostTotUnadjPkg", typeof(decimal));//Added 10-5-2014...    
        dtJs.Columns.Add("ParentMatCostTotAdj", typeof(decimal));
        dtJs.Columns.Add("WiParentMatCostTotAdj", typeof(decimal));
        dtJs.Columns.Add("WiParentMatCostTotAdjPkg", typeof(decimal));//Added 10-5-2014...
        dtJs.Columns.Add("ParentMatCostUnitUnadj", typeof(decimal));
        dtJs.Columns.Add("ParentMatCostUnitAdj", typeof(decimal));
        dtJs.Columns.Add("ParentLabCostTot", typeof(decimal));
        dtJs.Columns.Add("WiParentLabCostTot", typeof(decimal));
        dtJs.Columns.Add("ParentLabCostUnit", typeof(decimal));
        dtJs.Columns.Add("ParentUomFlag", typeof(string));
        dtJs.Columns.Add("ParentUom", typeof(string));
        dtJs.Columns.Add("RecipeMatCostTotUsed", typeof(decimal));
        dtJs.Columns.Add("WiRecipeMatCostTotUsed", typeof(decimal));
        dtJs.Columns.Add("WiRecipeMatCostTotUsedPkg", typeof(decimal));//Added 10-5-2014...
        dtJs.Columns.Add("RecipeLabCostTotUsed", typeof(decimal));
        dtJs.Columns.Add("WiRecipeLabCostTotUsed", typeof(decimal));
        dtJs.Columns.Add("WiRecipeLabCostTotUsedPkg", typeof(decimal));//Added 10-5-2014...
        dtJs.Columns.Add("RecipeMatCostUnitUsed", typeof(decimal));
        dtJs.Columns.Add("WiRecipeMatCostUnitUsed", typeof(decimal));
        dtJs.Columns.Add("WiRecipeMatCostUnitUsedPkg", typeof(decimal));//Added 10-5-2014...
        dtJs.Columns.Add("RecipeLabCostUnitUsed", typeof(decimal));
        dtJs.Columns.Add("WiRecipeLabCostUnitUsed", typeof(decimal));
        dtJs.Columns.Add("WiRecipeLabCostUnitUsedPkg", typeof(decimal));//Added 10-5-2014...
        dtJs.Columns.Add("ParentWhatIfMatCost", typeof(decimal));
        dtJs.Columns.Add("ParentWhatIfLabCost", typeof(decimal));
        dtJs.Columns.Add("ParentUnitConvFact", typeof(decimal));
        dtJs.Columns.Add("IngredMatCostTotUsed", typeof(decimal));
        dtJs.Columns.Add("WiIngredMatCostTotUsed", typeof(decimal));
        dtJs.Columns.Add("IngredLabCostTotUsed", typeof(decimal));
        dtJs.Columns.Add("WiIngredLabCostTotUsed", typeof(decimal));
        dtJs.Columns.Add("IngredMatCostUnitUsed", typeof(decimal));
        dtJs.Columns.Add("WiIngredMatCostUnitUsed", typeof(decimal));
        dtJs.Columns.Add("IngredLabCostUnitUsed", typeof(decimal));
        dtJs.Columns.Add("WiIngredLabCostUnitUsed", typeof(decimal));
        //Added 10-5-2014...
        dtJs.Columns.Add("RmIssUnitWhatIfMatCost", typeof(decimal));
        dtJs.Columns.Add("RmIssUnitWhatIfMatCostRecipe", typeof(decimal));
        dtJs.Columns.Add("RmIssUnitWhatIfMatCostPkg", typeof(decimal));
        dtJs.Columns.Add("RecipeTrnValueTot", typeof(decimal));//Add 7-13-2021 RSS... [Packaging Cost]...      
        dtJs.Columns.Add("FgUnitLabor", typeof(decimal));

        DataTable dtIng = new DataTable();

        dtIng.Columns.Add("RecipeJob", typeof(string));
        dtIng.Columns.Add("RecipeStockCode", typeof(string));
        dtIng.Columns.Add("RecipeDescription", typeof(string));
        dtIng.Columns.Add("RecipeWh", typeof(string));
        dtIng.Columns.Add("RecipeCompleteDate", typeof(DateTime));
        dtIng.Columns.Add("RecipeIsComplete", typeof(bool));
        dtIng.Columns.Add("RecipeQtyToMake", typeof(decimal));
        dtIng.Columns.Add("RecipeQtyManuf", typeof(decimal));
        dtIng.Columns.Add("RecipeConfirmed", typeof(bool));
        dtIng.Columns.Add("RecipeOnHold", typeof(bool));
        dtIng.Columns.Add("RecipeMatCostTotUnadj", typeof(decimal));
        dtIng.Columns.Add("RecipeMatCostUnitUnadj", typeof(decimal));
        dtIng.Columns.Add("RecipeLabCostTot", typeof(decimal));
        dtIng.Columns.Add("RecipeLabCostUnit", typeof(decimal));
        dtIng.Columns.Add("RecipeUomFlag", typeof(string));
        dtIng.Columns.Add("RecipeUom", typeof(string));
        dtIng.Columns.Add("IngredStockCode", typeof(string));
        dtIng.Columns.Add("IngredTrnValueTot", typeof(decimal));
        dtIng.Columns.Add("IngredTrnValueUnit", typeof(decimal));
        dtIng.Columns.Add("IngredUom", typeof(string));
        dtIng.Columns.Add("IngredWh", typeof(string));
        dtIng.Columns.Add("IngredJob", typeof(string));
        dtIng.Columns.Add("IngredDescription", typeof(string));
        dtIng.Columns.Add("IngredMatCostWhole", typeof(decimal));
        dtIng.Columns.Add("WiIngredMatCostWhole", typeof(decimal));
        dtIng.Columns.Add("IngredLabCostWhole", typeof(decimal));
        dtIng.Columns.Add("WiIngredLabCostWhole", typeof(decimal));
        dtIng.Columns.Add("IngredTotCostWhole", typeof(decimal));
        dtIng.Columns.Add("WiIngredTotCostWhole", typeof(decimal));
        dtIng.Columns.Add("IngredUsedFactor", typeof(decimal));
        dtIng.Columns.Add("IngredMatCostUsedTot", typeof(decimal));
        dtIng.Columns.Add("WiIngredMatCostUsedTot", typeof(decimal));
        dtIng.Columns.Add("IngredMatCostUsedUnit", typeof(decimal));
        dtIng.Columns.Add("WiIngredMatCostUsedUnit", typeof(decimal));
        dtIng.Columns.Add("IngredLabCostUsedTot", typeof(decimal));
        dtIng.Columns.Add("WiIngredLabCostUsedTot", typeof(decimal));
        dtIng.Columns.Add("IngredLabCostUsedUnit", typeof(decimal));
        dtIng.Columns.Add("WiIngredLabCostUsedUnit", typeof(decimal));
        dtIng.Columns.Add("RecipeWhatIfMatCost", typeof(decimal));
        dtIng.Columns.Add("RecipeWhatIfLabCost", typeof(decimal));
        dtIng.Columns.Add("RecipeUnitConvFact", typeof(decimal));
        dtIng.Columns.Add("IngredWhatIfMatCost", typeof(decimal));
        dtIng.Columns.Add("IngredWhatIfLabCost", typeof(decimal));
        dtIng.Columns.Add("IngredQtyManufactured", typeof(decimal));
        dtIng.Columns.Add("IngredQtyIssued", typeof(decimal));
        //Added 10-5-2014...
        dtIng.Columns.Add("RmIssUnitWhatIfMatCost", typeof(decimal));
        dtIng.Columns.Add("RmIssUnitWhatIfMatCostRecipe", typeof(decimal));
        dtIng.Columns.Add("RmIssUnitWhatIfMatCostPkg", typeof(decimal));
        dtIng.Columns.Add("FgUnitLabor", typeof(decimal));


        DataTable dtNs = new DataTable();
        dtNs.Columns.Add("RowType", typeof(string));//1.
        dtNs.Columns.Add("ParentPart", typeof(string));//2.
        dtNs.Columns.Add("ParentDescription", typeof(string));//3.
        dtNs.Columns.Add("ParentCompleteDate", typeof(DateTime));//4.
        dtNs.Columns.Add("ParentJob", typeof(string));//5.
        dtNs.Columns.Add("ParentUom", typeof(string));//6.
        dtNs.Columns.Add("ParentQtyManuf", typeof(decimal));//7.
        dtNs.Columns.Add("RecipeMatCostTotUsed", typeof(decimal));//8.
        dtNs.Columns.Add("ParentMatCostTotAdj", typeof(decimal));//9.
        dtNs.Columns.Add("TotLabCost", typeof(decimal));//10.
        dtNs.Columns.Add("TotCost", typeof(decimal));//11.
        dtNs.Columns.Add("RecipeMatCostTotUsedDisp", typeof(string));//12.
        dtNs.Columns.Add("ParentMatCostTotAdjDisp", typeof(string));//13.
        dtNs.Columns.Add("TotLabCostDisp", typeof(string));//14.
        dtNs.Columns.Add("TotCostDisp", typeof(string));//15.
        //Js = Job Summary...
        bool bPcdIsNull = false;


        decimal dcRecipeMatCostUsedTot = 0;
        decimal dcWiRecipeMatCostUsedTot = 0;
        decimal dcWiRecipeMatCostUsedTotPkg = 0;
        decimal dcWiRecipeLabCostUsedTotPkg = 0;
        decimal dcRecipeLabCostUsedTot = 0;
        decimal dcWiRecipeLabCostUsedTot = 0;
        decimal dcIngredMatCostUsedTot = 0;
        decimal dcIngredLabCostUsedTot = 0;
        decimal dRecipeMatCostUsedTot = 0;
        decimal dRecipeLabCostUsedTot = 0;
        decimal dcRecipeTrnValueTot = 0;
        string sLastParentJob = "";

        try
        {


            int iCountRecipeJobs = 1;

            DataView dv = new DataView(dtJd);
            dv.Sort = "ParentJob";
            dtJd = dv.ToTable();

            foreach (DataRow drJd in dtJd.Rows)//Loop through each row in main query...
            {


                //WcrDs.JobSumRow drJs = Db.JobSum.Select(dtJs, drJd.ParentJob);
                DataRow drJs = (from a in dtJs.Rows.Cast<DataRow>() where a.Field<string>("ParentJob") == drJd["ParentJob"].ToString() select a).FirstOrDefault();
                if (drJs == null)
                {
                    if (drJd["ParentCompleteDate"] == DBNull.Value)
                    {
                        bPcdIsNull = true;
                    }
                    else
                    {
                        bPcdIsNull = false;
                    }
                    //Add new dtJs Row...
                    drJs = dtJs.Rows.Add(
                    drJd["ParentJob"],
                    drJd["ParentPart"],
                    drJd["ParentDescription"],
                    drJd["ParentWh"],
                    bPcdIsNull ? DateTime.MinValue : drJd["ParentCompleteDate"],
                    drJd["ParentIsComplete"],
                    drJd["ParentQtyToMake"],
                    drJd["ParentQtyManuf"],
                    drJd["ParentConfirmed"],
                    drJd["ParentOnHold"],
                    drJd["ParentMatCostTotUnadj"],
                    drJd["WiParentMatCostTotUnadj"],
                    drJd["WiParentMatCostTotUnadjPkg"],
                    0M /*ParentMatCostTotAdj*/,
                    0M /*WiParentMatCostTotAdj*/,
                    0M /*WiParentMatCostTotAdjPkg*/,
                    0M/*  drJd["ParentMatCostUnitUnadj"], 10-15-2015 RSS Commented out...*/,
                    0M /*ParentMatCostUnitAdj*/,
                    drJd["ParentLabCostTot"],
                    0M /*WiParentLabCostTot*/,
                    drJd["ParentLabCostUnit"],
                    drJd["ParentUomFlag"],
                    drJd["ParentUom"],
                    0M /*RecipeMatCostTotUsed*/,
                    0M /*WiRecipeMatCostTotUsed*/,
                    0M /*WiRecipeMatCostTotUsedPkg*/,
                    0M /*RecipeLabCostTotUsed*/,
                    0M /*WiRecipeLabCostTotUsed*/,
                    0M/*WiRecipeLabCostTotUsedPkg*/,
                    0M /*RecipeMatCostUnitUsed*/,
                    0M /*WiRecipeMatCostUnitUsed*/,
                    0M/*WiRecipeMatCostUnitUsedPkg*/,
                    0M /*RecipeLabCostUnitUsed*/,
                    0M /*WiRecipeLabCostUnitUsed*/,
                    0M /*WiRecipeLabCostUnitUsedPkg*/,
                    drJd["ParentWhatIfMatCost"],
                    drJd["ParentWhatIfLabCost"],
                    drJd["ParentUnitConvFact"],
                    0M /*IngredMatCostTotUsed*/,
                    0M /*WiIngredMatCostTotUsed*/,
                    0M /*IngredLabCostTotUsed*/,
                    0M /*WiIngredLabCostTotUsed*/,
                    0M /*IngredMatCostUnitUsed*/,
                    0M /*WiIngredMatCostUnitUsed*/,
                    0M /*IngredLabCostUnitUsed*/,
                    0M /*WiIngredLabCostUnitUsed*/,
                    drJd["RmIssUnitWhatIfMatCost"],
                    drJd["RmIssUnitWhatIfMatCostRecipe"],
                    drJd["RmIssUnitWhatIfMatCostPkg"],
                    drJd["FgUnitLabor"]
                    );
                    //if (bPcdIsNull)
                    //{
                    //    drJs["ParentCompleteDateColumn"] = DBNull.Value;
                    //}
                }
                // Accumulate ingredient level for the recipe.
                drJd["IngredMatCostWholeTot"] = 0M;
                drJd["IngredLabCostWholeTot"] = 0M;

                //Debug.WriteLine(drJd["RecipeJob"].ToString());
                if (drJd["RecipeJob"].ToString().Length > 0)
                {//JobRecipe is not null...
                    ////  Db.JobDet.Load(scon, dtIng, 'S', drJd["RecipeJob, string.Empty, 'A', string.Empty, string.Empty, 'A', string.Empty, string.Empty);

                    //dv = new DataView(dtJd);
                    //dv.RowFilter = "RecipeJob = '" + drJd["RecipeJob"].ToString() + "'";
                    //dtIng = dv.ToTable();

                    sSQL = "EXEC spGetWipCostAnalysis ";
                    sSQL += "@FromJob=" + drJd["RecipeJob"].ToString();

                    // Debug.WriteLine(sSQL);

                    dtIng = SharedFunctions.getDataTable(sSQL, conn, "dtReportIng");
                    //Debug.WriteLine(dtIng.Rows.Count);

                    Session["dtIng"] = dtIng;
                }

                //Debug.WriteLine(dtIng.Rows.Count);
                if (drJd["ParentJob"].ToString() != sLastParentJob && sLastParentJob != "")
                {//If not same parentjob then reset values...
                    dcRecipeMatCostUsedTot = 0;
                    dcWiRecipeMatCostUsedTot = 0;
                    dcWiRecipeMatCostUsedTotPkg = 0;
                    dcWiRecipeLabCostUsedTotPkg = 0;
                    dcRecipeLabCostUsedTot = 0;
                    dcWiRecipeLabCostUsedTot = 0;
                    dcIngredMatCostUsedTot = 0;
                    dcIngredLabCostUsedTot = 0;
                    dRecipeMatCostUsedTot = 0;
                    dRecipeLabCostUsedTot = 0;
                    dcRecipeTrnValueTot = 0;
                    iCountRecipeJobs = 1;
                }

                foreach (DataRow drIng in dtIng.Rows)
                {
                    dRecipeMatCostUsedTot += (decimal)drIng["RecipeMatCostUsedTot"];
                    dRecipeLabCostUsedTot += (decimal)drIng["RecipeLabCostUsedTot"];
                    drJd["IngredMatCostWholeTot"] = dRecipeMatCostUsedTot;
                    drJd["IngredLabCostWholeTot"] = dRecipeLabCostUsedTot;
                }


                drJd["IngredMatCostUsedTot"] = (decimal)drJd["IngredMatCostWholeTot"] * (decimal)drJd["RecipeUsedFactor"];//Material Cost...
                //// TI_MOD Change per Brian START
                ////drJd["IngredMatCostUsedUnit = drJd["IngredMatCostUsedTot / drJd["ParentUnitConvFact;
                drJd["IngredMatCostUsedUnit"] = (decimal)drJd["IngredMatCostUsedTot"] / (decimal)drJd["RecipeQtyIssued"];//Material Cost...
                //// TI_MOD Change per Brian END
                drJd["IngredLabCostUsedTot"] = (decimal)drJd["IngredLabCostWholeTot"] * (decimal)drJd["RecipeUsedFactor"];//Labor Cost...
                //// TI_MOD Change per Brian START
                ////drJd["IngredLabCostUsedUnit = drJd["IngredLabCostUsedTot / drJd["ParentUnitConvFact;
                drJd["IngredLabCostUsedUnit"] = (decimal)drJd["IngredLabCostUsedTot"] / (decimal)drJd["RecipeQtyIssued"];//Labor Cost...
                //// TI_MOD Change per Brian END
                //// Accumulate receipe level.


                //Total up Recipe Cost...
                dcRecipeMatCostUsedTot += (decimal)drJd["RecipeMatCostUsedTot"];
                drJs["RecipeMatCostTotUsed"] = dcRecipeMatCostUsedTot;

                //Debug.WriteLine("First: " + drJd["ParentJob"] + "  - " + iCountRecipeJobs);
                //Debug.WriteLine(drJd["RecipeMatCostUsedTot"]);//Returned correct number of rows...
                //Debug.WriteLine(dcRecipeMatCostUsedTot);

                dcWiRecipeMatCostUsedTot += (decimal)drJd["WiRecipeMatCostUsedTot"];
                drJs["WiRecipeMatCostTotUsed"] = dcWiRecipeMatCostUsedTot;

                dcWiRecipeMatCostUsedTotPkg += (decimal)drJd["WiRecipeMatCostUsedTotPkg"];//Added 10-5-2014...
                drJs["WiRecipeMatCostTotUsedPkg"] = dcWiRecipeMatCostUsedTotPkg;

                dcWiRecipeLabCostUsedTotPkg += (decimal)drJd["WiRecipeLabCostUsedTotPkg"];//Added 10-5-2014...
                drJs["WiRecipeLabCostTotUsedPkg"] = dcWiRecipeLabCostUsedTotPkg;

                dcRecipeLabCostUsedTot += (decimal)drJd["RecipeLabCostUsedTot"];
                drJs["RecipeLabCostTotUsed"] = dcRecipeLabCostUsedTot;

                dcWiRecipeLabCostUsedTot += (decimal)drJd["WiRecipeLabCostUsedTot"];
                drJs["WiRecipeLabCostTotUsed"] = dcWiRecipeLabCostUsedTot;

                dcIngredMatCostUsedTot += (decimal)drJd["IngredMatCostUsedTot"];
                drJs["IngredMatCostTotUsed"] = dcIngredMatCostUsedTot;

                ////drJs["WiIngredMatCostTotUsed += drJd["WiIngredMatCostUsedTot; // this was not implemented at job detail level

                dcIngredLabCostUsedTot += (decimal)drJd["IngredLabCostUsedTot"];
                drJs["IngredLabCostTotUsed"] = dcIngredLabCostUsedTot;
                ////drJs["WiIngredLabCostTotUsed += drJd["WiIngredLabCostUsedTot; // this was not implemented at job detail level

                sLastParentJob = drJd["ParentJob"].ToString();//Set to last parent job...

                //Added 7-13-2021...
                dcRecipeTrnValueTot += (decimal)drJd["RecipeTrnValueTot"];
                drJs["RecipeTrnValueTot"] = dcRecipeTrnValueTot;

                iCountRecipeJobs++;
            } // foreach drJd in dtJd
            //// Compute recipe-level summary fields that require completed accumulation from detail above.
            foreach (DataRow drJs in dtJs.Rows)
            {

                drJs["ParentMatCostTotAdj"] = (decimal)drJs["RecipeTrnValueTot"];//Changed 7-13-2021 RSS... (decimal)drJs["ParentMatCostTotUnadj"] - (decimal)drJs["RecipeMatCostTotUsed"] - (decimal)drJs["RecipeLabCostTotUsed"]; [Packaging Cost]...                
                drJs["WiParentMatCostTotAdj"] = (decimal)drJs["WiParentMatCostTotUnadj"] - (decimal)drJs["WiRecipeMatCostTotUsed"] - (decimal)drJs["WiRecipeLabCostTotUsed"];
                drJs["WiParentMatCostTotAdjPkg"] = (decimal)drJs["WiParentMatCostTotUnadjPkg"] - (decimal)drJs["WiRecipeMatCostTotUsedPkg"] - (decimal)drJs["WiRecipeLabCostTotUsedPkg"];//Added 10-5-2014...
                drJs["ParentMatCostUnitAdj"] = Math.Round((decimal)drJs["ParentMatCostTotAdj"], 6, MidpointRounding.AwayFromZero);
                drJs["RecipeMatCostUnitUsed"] = Math.Round((decimal)drJs["RecipeMatCostTotUsed"] / (decimal)drJs["ParentUnitConvFact"], 6, MidpointRounding.AwayFromZero);
                drJs["WiRecipeMatCostUnitUsed"] = Math.Round((decimal)drJs["WiRecipeMatCostTotUsed"] / (decimal)drJs["ParentUnitConvFact"], 6, MidpointRounding.AwayFromZero);
                drJs["WiRecipeMatCostUnitUsedPkg"] = Math.Round((decimal)drJs["WiRecipeMatCostTotUsedPkg"] / (decimal)drJs["ParentUnitConvFact"], 6, MidpointRounding.AwayFromZero);//Added 10-5-2014...
                drJs["RecipeLabCostUnitUsed"] = Math.Round((decimal)drJs["RecipeLabCostTotUsed"] / (decimal)drJs["ParentUnitConvFact"], 6, MidpointRounding.AwayFromZero);
                drJs["WiRecipeLabCostUnitUsed"] = Math.Round((decimal)drJs["WiRecipeLabCostTotUsed"] / (decimal)drJs["ParentUnitConvFact"], 6, MidpointRounding.AwayFromZero);
                drJs["WiRecipeLabCostUnitUsedPkg"] = Math.Round((decimal)drJs["WiRecipeLabCostTotUsedPkg"] / (decimal)drJs["ParentUnitConvFact"], 6, MidpointRounding.AwayFromZero);//Added 10-5-2014...
                drJs["IngredMatCostUnitUsed"] = Math.Round((decimal)drJs["IngredMatCostTotUsed"] / (decimal)drJs["ParentUnitConvFact"], 6, MidpointRounding.AwayFromZero);
                drJs["WiIngredMatCostUnitUsed"] = Math.Round((decimal)drJs["WiIngredMatCostTotUsed"] / (decimal)drJs["ParentUnitConvFact"], 6, MidpointRounding.AwayFromZero);
                drJs["IngredLabCostUnitUsed"] = Math.Round((decimal)drJs["IngredLabCostTotUsed"] / (decimal)drJs["ParentUnitConvFact"], 6, MidpointRounding.AwayFromZero);
                drJs["WiIngredLabCostUnitUsed"] = Math.Round((decimal)drJs["WiIngredLabCostTotUsed"] / (decimal)drJs["ParentUnitConvFact"], 6, MidpointRounding.AwayFromZero);
            } // foreach drJs in dtJs


            //What If CPU..
            // New job summary format.
            decimal dTotalParentQtyManuf = 0M;
            decimal dTotalRecipeMatCostTotUsed = 0M;
            decimal dTotalWiRecipeMatCostTotUsed = 0M;
            decimal dTotalParentMatCostTotAdj = 0M;
            decimal dTotalWiParentMatCostTotAdj = 0M;
            decimal dTotalTotLabCost = 0M;
            decimal dTotalWiTotLabCost = 0M;
            decimal dTotalTotCost = 0M;
            decimal dTotalWiTotCost = 0M;
            decimal dTotalExtParentWhatIfMatCost = 0M;
            decimal dTotalExtParentWhatIfLabCost = 0M;
            DateTime? dtParentCompleteDate = null;
            int iCount = 0;
            decimal dTotalWiParentMatCostTotAdjPkg = 0M;//added 10-4-2014...
            foreach (DataRow drJs in dtJs.Rows)
            {

                if (drJs["ParentCompleteDate"] == DBNull.Value)
                {
                    dtParentCompleteDate = DateTime.MinValue;
                }
                else
                {
                    dtParentCompleteDate = (DateTime?)drJs["ParentCompleteDate"];
                }


                dTotalParentQtyManuf += (decimal)drJs["ParentQtyManuf"];
                //Total up Recipe Cost...
                dTotalRecipeMatCostTotUsed += (decimal)drJs["RecipeMatCostTotUsed"];
                //Debug.WriteLine("Second: " + drJs["ParentJob"]);
                //Debug.WriteLine(drJs["RecipeMatCostTotUsed"]);
                //Debug.WriteLine(dTotalRecipeMatCostTotUsed);

                dTotalWiRecipeMatCostTotUsed += (Convert.ToDecimal(drJs["RmIssUnitWhatIfMatCostRecipe"]) * Convert.ToDecimal(drJs["ParentQtyManuf"]));//drJs["WiRecipeMatCostTotUsed"];
                dTotalParentMatCostTotAdj += (decimal)drJs["ParentMatCostTotAdj"];
                dTotalWiParentMatCostTotAdj += (decimal)drJs["WiParentMatCostTotAdj"];
                dTotalWiParentMatCostTotAdjPkg += ((decimal)drJs["RmIssUnitWhatIfMatCostPkg"] * (decimal)drJs["ParentQtyManuf"]);//Added 10-5-2014...Used to send value to summary...
                dTotalTotLabCost += (decimal)drJs["ParentLabCostTot"] + (decimal)drJs["RecipeLabCostTotUsed"];//Changed 7-8-14
                //// dTotalWiTotLabCost += (decimal)drJs["ParentWhatIfLabCost"];//+= (decimal)drJs["WiParentLabCostTot"] + (decimal)drJs["WiRecipeLabCostTotUsed"];
                decimal dcFgLabor = 0;
                if (drJs["FgUnitLabor"] != DBNull.Value)
                {
                    dcFgLabor = Convert.ToDecimal(drJs["FgUnitLabor"]);
                }
                dTotalWiTotLabCost += (dcFgLabor * Convert.ToDecimal(drJs["ParentQtyManuf"]));//Changed 10-5-14

                dTotalTotCost += (decimal)drJs["RecipeMatCostTotUsed"] + (decimal)drJs["ParentMatCostTotAdj"] + (decimal)drJs["ParentLabCostTot"] + (decimal)drJs["RecipeLabCostTotUsed"];
                dTotalWiTotCost += (((decimal)drJs["RmIssUnitWhatIfMatCost"] + dcFgLabor) * (decimal)drJs["ParentQtyManuf"]); //Changed 10-5-14
                ////(decimal)drJs["WiRecipeMatCostTotUsed"] + (decimal)drJs["WiParentMatCostTotAdj"] + (decimal)drJs["WiParentLabCostTot"] + (decimal)drJs["WiRecipeLabCostTotUsed"];

                dTotalExtParentWhatIfMatCost += (decimal)drJs["ParentWhatIfMatCost"] * (decimal)drJs["ParentQtyManuf"];
                dTotalExtParentWhatIfLabCost += (decimal)drJs["ParentWhatIfLabCost"] * (decimal)drJs["ParentQtyManuf"];

                //Debug.WriteLine(drJs["RecipeMatCostTotUsed"]);
                // Debug.WriteLine(iCount);

                //Add a new dtNs row...
                DataRow drNs = dtNs.Rows.Add("D",//RowType[1]...
                drJs["ParentPart"],//ParentPart[2]...
                drJs["ParentDescription"],//ParentDescription[3]...
                Convert.ToDateTime(dtParentCompleteDate),//ParentCompleteDate[4]...
                drJs["ParentJob"],//ParentJob[5]...
                drJs["ParentUom"],//ParentUom[6]...
                drJs["ParentQtyManuf"],//ParentQtyManuf[7]...
                drJs["RecipeMatCostTotUsed"],//RecipeMatCostTotUsed[8]...
                drJs["ParentMatCostTotAdj"],//ParentMatCostTotAdj[9]...
                 (decimal)drJs["ParentLabCostTot"] + (decimal)drJs["RecipeLabCostTotUsed"], //TotLabCost[10]...
                 (decimal)drJs["RecipeMatCostTotUsed"] + (decimal)drJs["ParentMatCostTotAdj"] + (decimal)drJs["ParentLabCostTot"] + (decimal)drJs["RecipeLabCostTotUsed"],//TotCost[11]...
                Convert.ToDecimal(drJs["RecipeMatCostTotUsed"]).ToString("#,0.00"),//RecipeMatCostTotUsedDisp[12]...
                Convert.ToDecimal(drJs["ParentMatCostTotAdj"]).ToString("#,0.00"),//ParentMatCostTotAdjDisp[13]...
                Convert.ToDecimal((decimal)drJs["ParentLabCostTot"] + Convert.ToDecimal(drJs["RecipeLabCostTotUsed"])).ToString("#,0.00"),//TotLabCostDisp[14]...
                Convert.ToDecimal((decimal)drJs["RecipeMatCostTotUsed"] + (decimal)drJs["ParentMatCostTotAdj"] + (decimal)drJs["ParentLabCostTot"] + (decimal)drJs["RecipeLabCostTotUsed"]).ToString("#,0.00"));//TotCostDisp[15]...

                iCount++;
            }

            dtJs.AcceptChanges();

            if (dtJs.Rows.Count == 0)
            {
                return "No matching records found.";
            }

            /////RSS Comment dTotalParentQtyManuf = Qty, dTotalRecipeMatCostTotUsed = Recipe Cost, dTotalParentMatCostTotAdj = Pkg Cost, dTotalTotLabCost = Labor/OH, dTotalTotCost = TotalCost
            // AddBlankNsRow(dtNs);
            //AddWghtSumNsRow(dtNs, dTotalParentQtyManuf, dTotalRecipeMatCostTotUsed, dTotalParentMatCostTotAdj, dTotalTotLabCost, dTotalTotCost);
            //AddBlankNsRow(dtNs);
            //AddPerUnitNsRow(dtNs, dTotalParentQtyManuf, dTotalRecipeMatCostTotUsed, dTotalParentMatCostTotAdj, dTotalTotLabCost, dTotalTotCost);
            //// AddBlankNsRow(dtNs);
            ////Pass in totals to be divided by...
            //AddWicpuRow(dtNs, dTotalParentQtyManuf, dTotalWiRecipeMatCostTotUsed, dTotalWiParentMatCostTotAdjPkg, dTotalWiTotLabCost, dTotalWiTotCost, iCount);//Changed 10-5-2014...
            //AddBlankNsRow(dtNs);
            //Variance CPU...
            //decimal dTotalVarRecipeMatCostTotUsed = dTotalRecipeMatCostTotUsed - dTotalWiRecipeMatCostTotUsed;

            //decimal dTotalVarParentMatCostTotAdj = 0;// dTotalParentMatCostTotAdj - dTotalWiParentMatCostTotAdj;
            //dTotalVarParentMatCostTotAdj = dTotalParentMatCostTotAdj - dTotalWiParentMatCostTotAdjPkg;
            //decimal dTotalVarTotLabCost = dTotalTotLabCost - dTotalWiTotLabCost;
            //decimal dTotalVarTotCost = dTotalTotCost - dTotalWiTotCost;
            ////Pass in totals to be divided by...
            //AddVcpuRow(dtNs, dTotalParentQtyManuf, dTotalVarRecipeMatCostTotUsed, dTotalVarParentMatCostTotAdj, dTotalVarTotLabCost, dTotalVarTotCost);

            dtJd.AcceptChanges();
            // dtNs.AcceptChanges();

            // m_dtJobSum.Clear();
            // m_dtJobSum.Merge(dtJs);
            //m_dtJobSum.AcceptChanges();

            ////Below is for debugging...
            //Console.WriteLine("****************Short Details********************");
            //Console.WriteLine(dtJs.Rows.Count);
            //foreach (DataColumn dc in dtJs.Columns)
            //{

            //    foreach (DataRow dr in dtJs.Rows)
            //    {
            //        Debug.WriteLine(dc.ColumnName);
            //        Debug.WriteLine(dr[dc.ColumnName].ToString());
            //    }
            //}

            // m_dtJobDet.Clear();
            // m_dtJobDet.Merge(dtJd);
            // m_dtJobDet.AcceptChanges();
            ////Below is for debugging...
            //Console.WriteLine("**************Long******************");
            //Console.WriteLine(dtJd.Rows.Count);
            //foreach (DataColumn dc in dtJd.Columns)
            //{

            //    foreach (DataRow dr in dtJd.Rows)
            //    {
            //        Debug.WriteLine(dc.ColumnName);
            //        Debug.WriteLine(dr[dc.ColumnName].ToString());
            //    }
            //}
            //  m_dtNewSum.Clear();
            // m_dtNewSum.Merge(dtNs);
            // m_dtNewSum.AcceptChanges();

            ////Below is for debugging...
            //Console.WriteLine(dtNs.Rows.Count);
            //Console.WriteLine("********************Summary***********************");
            //foreach (DataColumn dc in dtNs.Columns)
            //{

            //    foreach (DataRow dr in dtNs.Rows)
            //    {
            //        Debug.WriteLine(dc.ColumnName);
            //        Debug.WriteLine(dr[dc.ColumnName].ToString());
            //    }
            //}

            dtNs.Columns.Add("NullEmptyCheck", typeof(int), "ParentJob is Null OR ParentJob = ''");
            dtNs.DefaultView.Sort = "NullEmptyCheck asc, ParentJob asc";

            dtJs.Columns.Add("NullEmptyCheck", typeof(int), "ParentJob is Null OR ParentJob = ''");
            dtJs.DefaultView.Sort = "NullEmptyCheck asc, ParentJob asc";


            var query = (from a in
                        (from a in (
                            (from f in dtNs.AsEnumerable()
                             select new
                             {
                                 RecipeCost = (Convert.ToDecimal(f["RecipeMatCostTotUsedDisp"]) / Convert.ToDecimal(f["ParentQtyManuf"])),
                                 PkgCost = (Convert.ToDecimal(f["ParentMatCostTotAdjDisp"]) / Convert.ToDecimal(f["ParentQtyManuf"])),
                                 LaborOH = (Convert.ToDecimal(f["TotLabCostDisp"]) / Convert.ToDecimal(f["ParentQtyManuf"])),
                                 Total = (Convert.ToDecimal(f["TotCostDisp"]) / Convert.ToDecimal(f["ParentQtyManuf"]))
                             }))
                         select new
                         {
                             a.RecipeCost,
                             a.PkgCost,
                             a.LaborOH,
                             a.Total,
                             Dummy = "x"
                         })
                         group a by new { a.Dummy } into g
                         select new
                         {
                             RecipeCost = (decimal?)Convert.ToDecimal(g.Average(p => p.RecipeCost)),
                             PkgCost = (decimal?)Convert.ToDecimal(g.Average(p => p.PkgCost)),
                             LaborOH = (decimal?)Convert.ToDecimal(g.Average(p => p.LaborOH)),
                             Total = (decimal?)Convert.ToDecimal(g.Average(p => p.Total))
                         });

            dtNs = SharedFunctions.LINQToDataTable(query);

            Session["dtNs"] = dtNs;
            gvReportWipCost.DataSource = dtNs;
            gvReportWipCost.DataBind();


        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return ex.ToString();
        }
        finally
        {
            dtJs.Dispose();
            dtJd.Dispose();
            dtNs.Dispose();
        }
        return "";
    }
    private bool EstimateIngredientCostExists(string sStockCode)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from est in db.InvEstimatedIngredientCost
                         where est.StockCode == sStockCode
                         select est);
            if (query.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private bool EstimatePackagingCostExists(string sStockCode)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from est in db.InvEstimatedPackagingCost
                         where est.StockCode == sStockCode
                         select est);
            if (query.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        int iUserID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        else
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
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
            //txtStartDate.Text = DateTime.Now.AddMonths(-12).ToShortDateString();
            //txtEndDate.Text = DateTime.Now.ToShortDateString();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isPostBack", "isPostBack();", true);
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        string sMsg = "";
        string sStockCode = "";
        if (txtStockCode.Text.Trim() == "")
        {
            lblError.Text = "**No Stock Code selected!";
            lblError.ForeColor = Color.Red;
            return;
        }
        else
        {
            sStockCode = txtStockCode.Text.Trim();
        }
        sMsg = RunCostReport(sStockCode);
        sMsg = RunReport(sStockCode);
        sMsg = RunEstimatedCostReport(sStockCode);

        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }

        decimal dcQtyPer = 0;
        decimal dcPrice = 0;
        decimal dcTotalCost = 0;
        decimal dcMargin = 0;
        decimal dcMarkup = 0;
        decimal dcShrinkAgePercentage = 0;
        decimal dcShrinkAge = 0;
        decimal dcRecipeCost = 0;
        decimal dcPackagingCost = 0;
        decimal dcLaborCost = 0;
        decimal dcShrinkagePlusLabor = 0;

        if (txtShrinkagePercentage.Text.Trim() == "")
        {
            dcShrinkAgePercentage = 2;
        }

        dcShrinkAgePercentage = Convert.ToDecimal(txtShrinkagePercentage.Text.Trim());

        string sPrice = GetPrice(sStockCode);
        dcPrice = Convert.ToDecimal(sPrice);


        SyncGridViews();

        foreach (GridViewRow grvRow in gvReportCondensed.Rows)
        {//Summary grid...
            if (grvRow.RowType == DataControlRowType.DataRow)
            {

                Label lblQtyPerSummary = (Label)grvRow.FindControl("lblQtyPer");
                dcQtyPer = Convert.ToDecimal(lblQtyPerSummary.Text);

                Label lblActualUnitCost = (Label)grvRow.FindControl("lblActualUnitCost");
                Label lblPackagingCost = (Label)grvRow.FindControl("lblPackagingCost");
                Label lblTotalLaborCost = (Label)grvRow.FindControl("lblTotalLaborCost");
                Label lblTotal = (Label)grvRow.FindControl("lblTotal");

                decimal dcActualUnitCost1 = 0;

                decimal dcPackagingCost1 = 0;

                if (lblActualUnitCost.Text != "")
                {
                    dcActualUnitCost1 = Convert.ToDecimal(lblActualUnitCost.Text);
                }
                if (lblTotalLaborCost.Text != "")
                {
                    dcLaborCost = Convert.ToDecimal(lblTotalLaborCost.Text);
                }
                if (lblPackagingCost.Text != "")
                {
                    dcPackagingCost1 = Convert.ToDecimal(lblPackagingCost.Text);
                }

                lblTotal.Text = (dcActualUnitCost1 + dcPackagingCost1 + dcLaborCost).ToString();
                dcTotalCost = Convert.ToDecimal(lblTotal.Text);
                dcRecipeCost = Convert.ToDecimal(lblActualUnitCost.Text);
                if (lblPackagingCost.Text != "")
                {
                    dcPackagingCost = Convert.ToDecimal(lblPackagingCost.Text);
                }

            }
        }

        dcShrinkAge = (((dcRecipeCost + dcPackagingCost) * dcShrinkAgePercentage) / 100) + (dcRecipeCost + dcPackagingCost);

        dcShrinkagePlusLabor = dcShrinkAge + dcLaborCost;
        dcTotalCost = dcShrinkagePlusLabor;

        if (dcPrice == 0)
        {
            dcPrice = 1;
        }
        if (dcPrice != 0)
        {
            dcMargin = ((dcPrice - dcTotalCost) / dcPrice) * 100;
        }
        lblMargin.Text = dcMargin.ToString("0");
        if (dcTotalCost != 0)
        {
            dcMarkup = ((dcPrice - dcTotalCost) / dcTotalCost) * 100;
        }
        lblMarkup.Text = dcMarkup.ToString("0");

        lblShrinkage.Text = dcShrinkagePlusLabor.ToString("#,0.00");


        Session["Price"] = sPrice;
        txtPrice.Text = sPrice;


    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Calculate();
    }
    protected void gvReportCondensed_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblActualUnitCost = (Label)e.Row.FindControl("lblActualUnitCost");
            Label lblWhatIfUnitCost = (Label)e.Row.FindControl("lblWhatIfUnitCost");

            Label lblTotalLaborCost = (Label)e.Row.FindControl("lblTotalLaborCost");
            Label lblWhatIfLaborCost = (Label)e.Row.FindControl("lblWhatIfLaborCost");

            Label lblPackagingCost = (Label)e.Row.FindControl("lblPackagingCost");
            Label lblWhatIfPkgCost = (Label)e.Row.FindControl("lblWhatIfPkgCost");

            Label lblTotal = (Label)e.Row.FindControl("lblTotal");
            Label lblWhatIfTotal = (Label)e.Row.FindControl("lblWhatIfTotal");

            Label lblQtyPer = (Label)e.Row.FindControl("lblQtyPer");
            if (lblQtyPer.Text != "")
            {
                Session["QtyPer"] = lblQtyPer.Text;
            }

            decimal dcTotal = 0;
            decimal dcWhatTotal = 0;
            decimal dcActualUnitCost = 0;
            decimal dcTotalLaborCost = 0;
            decimal dcPackagingCost = 0;

            decimal dcWhatIfUnitCost = 0;
            decimal dcWhatIfLaborCost = 0;
            decimal dcWhatIfPkgCost = 0;

            if (lblActualUnitCost.Text != "")
            {
                dcActualUnitCost = Convert.ToDecimal(lblActualUnitCost.Text);
            }
            if (lblTotalLaborCost.Text != "")
            {
                dcTotalLaborCost = Convert.ToDecimal(lblTotalLaborCost.Text);
            }
            if (lblPackagingCost.Text != "")
            {
                dcPackagingCost = Convert.ToDecimal(lblPackagingCost.Text);
            }

            if (lblActualUnitCost.Text != "" && lblWhatIfUnitCost.Text != "")
            {
                dcWhatIfUnitCost = Convert.ToDecimal(lblWhatIfUnitCost.Text);
            }
            if (lblTotalLaborCost.Text != "" && lblWhatIfLaborCost.Text != "")
            {
                dcWhatIfLaborCost = Convert.ToDecimal(lblWhatIfLaborCost.Text);
            }
            if (lblPackagingCost.Text != "" && lblWhatIfPkgCost.Text != "")
            {
                dcWhatIfPkgCost = Convert.ToDecimal(lblWhatIfPkgCost.Text);
            }

            dcTotal = dcActualUnitCost + dcTotalLaborCost + dcPackagingCost;
            dcWhatTotal = dcWhatIfUnitCost + dcWhatIfLaborCost + dcWhatIfPkgCost;

            lblTotal.Text = dcTotal.ToString();
            lblWhatIfTotal.Text = dcWhatTotal.ToString();
            Session["TotalCost"] = dcTotal;

        }
    }
    protected void gvComponents_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcActualUnitCost = 0;
        decimal dcWiUnitCost = 0;
        decimal dcWiLabCost = 0;
        decimal dcWiMatCost = 0;
        decimal dcLabCostMat = 0;
        decimal dcLabCostPkg = 0;
        decimal dcTotalLabCost = 0;
        decimal dcNewTotal = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblActualUnitCost = (Label)e.Row.FindControl("lblActualUnitCost");
            Label lblWhatIfUnitCost = (Label)e.Row.FindControl("lblWhatIfUnitCost");
            Label lblWhatIfLaborCost = (Label)e.Row.FindControl("lblWhatIfLaborCost");
            Label lblWhatIfMatCost = (Label)e.Row.FindControl("lblWhatIfMatCost");
            Label lblLaborCostMat = (Label)e.Row.FindControl("lblLaborCostMat");
            Label lblLaborCostPkg = (Label)e.Row.FindControl("lblLaborCostPkg");
            Label lblTotalLaborCost = (Label)e.Row.FindControl("lblTotalLaborCost");
            Label lblNewCost = (Label)e.Row.FindControl("lblNewCost");

            if (lblActualUnitCost.Text != "")
            {//Pkg Cost...
                dcActualUnitCost = Convert.ToDecimal(lblActualUnitCost.Text);
                dcActualUnitCostC += dcActualUnitCost;
                lblActualUnitCost.Text = Convert.ToDecimal(lblActualUnitCost.Text).ToString("0.00000");
            }
            if (lblWhatIfUnitCost.Text != "")
            {
                dcWiUnitCost = Convert.ToDecimal(lblWhatIfUnitCost.Text);
                dcWiUnitCostC += dcWiUnitCost;
                lblWhatIfUnitCost.Text = Convert.ToDecimal(lblWhatIfUnitCost.Text).ToString("0.00000");
            }
            if (lblWhatIfLaborCost.Text != "")
            {
                dcWiLabCost = Convert.ToDecimal(lblWhatIfLaborCost.Text);
                dcWiLabCostC = dcWiLabCost;
                lblWhatIfLaborCost.Text = Convert.ToDecimal(lblWhatIfLaborCost.Text).ToString("0.00");
            }
            if (lblWhatIfMatCost.Text != "")
            {
                dcWiMatCost = Convert.ToDecimal(lblWhatIfMatCost.Text);
                dcWiMatCostC += dcWiMatCost;
                lblWhatIfMatCost.Text = Convert.ToDecimal(lblWhatIfMatCost.Text).ToString("0.00");
            }
            if (lblLaborCostMat.Text != "")
            {
                dcLabCostMat = Convert.ToDecimal(lblLaborCostMat.Text);
                dcLabCostMatC = dcLabCostMat;
                lblLaborCostMat.Text = Convert.ToDecimal(lblLaborCostMat.Text).ToString("0.00");
            }
            if (lblLaborCostPkg.Text != "")
            {
                dcLabCostPkg = Convert.ToDecimal(lblLaborCostPkg.Text);
                dcLabCostPkgC = dcLabCostPkg;
                lblLaborCostPkg.Text = Convert.ToDecimal(lblLaborCostPkg.Text).ToString("0.00");
            }
            if (lblTotalLaborCost.Text != "")
            {
                dcTotalLabCost = Convert.ToDecimal(lblTotalLaborCost.Text);
                dcTotalLabCostC = dcTotalLabCost;
                lblTotalLaborCost.Text = Convert.ToDecimal(lblTotalLaborCost.Text).ToString("0.00");
            }
            if (lblNewCost.Text != "")
            {
                dcNewTotal = Convert.ToDecimal(lblNewCost.Text);
                dcNewTotalC += dcNewTotal;
                lblNewCost.Text = Convert.ToDecimal(lblNewCost.Text).ToString("0.00000");
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblActualUnitCostTotal = (Label)e.Row.FindControl("lblActualUnitCostTotal");
            Label lblWhatIfUnitCostTotal = (Label)e.Row.FindControl("lblWhatIfUnitCostTotal");
            Label lblWhatIfLaborCostTotal = (Label)e.Row.FindControl("lblWhatIfLaborCostTotal");
            Label lblWhatIfMatCostTotal = (Label)e.Row.FindControl("lblWhatIfMatCostTotal");
            Label lblLaborCostMatTotal = (Label)e.Row.FindControl("lblLaborCostMatTotal");
            Label lblLaborCostPkgTotal = (Label)e.Row.FindControl("lblLaborCostPkgTotal");
            Label lblTotalLaborCostTotal = (Label)e.Row.FindControl("lblTotalLaborCostTotal");
            Label lblNewCostTotal = (Label)e.Row.FindControl("lblNewCostTotal");

            lblActualUnitCostTotal.Text = dcActualUnitCostC.ToString("0.00");
            lblWhatIfUnitCostTotal.Text = dcWiUnitCostC.ToString("0.00");
            lblWhatIfLaborCostTotal.Text = "$" + dcWiLabCostC.ToString("0.00");
            lblWhatIfMatCostTotal.Text = "$" + dcWiMatCostC.ToString("0.00");
            lblLaborCostMatTotal.Text = dcLabCostMatC.ToString("0.00");
            lblLaborCostPkgTotal.Text = dcLabCostPkgC.ToString("0.00");
            lblTotalLaborCostTotal.Text = dcTotalLabCostC.ToString("0.00");
            lblNewCostTotal.Text = dcNewTotalC.ToString("0.00");
        }
    }
    protected void gvIngredients_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcActualUnitCost = 0;
        decimal dcWiUnitCost = 0;
        decimal dcWiLabCost = 0;
        //decimal dcWiMatCost = 0;
        decimal dcLabCostMat = 0;
        decimal dcLabCostPkg = 0;
        decimal dcTotalLabCost = 0;
        decimal dcNewTotal = 0;
        try
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblActualUnitCost = (Label)e.Row.FindControl("lblActualUnitCost");
                Label lblWhatIfUnitCost = (Label)e.Row.FindControl("lblWhatIfUnitCost");
                Label lblWhatIfLaborCost = (Label)e.Row.FindControl("lblWhatIfLaborCost");
                // Label lblWhatIfMatCost = (Label)e.Row.FindControl("lblWhatIfMatCost");
                Label lblLaborCostMat = (Label)e.Row.FindControl("lblLaborCostMat");
                Label lblLaborCostPkg = (Label)e.Row.FindControl("lblLaborCostPkg");
                Label lblTotalLaborCost = (Label)e.Row.FindControl("lblTotalLaborCost");
                Label lblNewCost = (Label)e.Row.FindControl("lblNewCost");

                if (lblActualUnitCost.Text != "")
                {//Recipe Cost...
                    dcActualUnitCost = Convert.ToDecimal(lblActualUnitCost.Text);
                    dcActualUnitCostI += dcActualUnitCost;
                    lblActualUnitCost.Text = Convert.ToDecimal(lblActualUnitCost.Text).ToString("0.00000");
                }
                if (lblWhatIfUnitCost.Text != "")
                {
                    dcWiUnitCost = Convert.ToDecimal(lblWhatIfUnitCost.Text);
                    dcWiUnitCostI += dcWiUnitCost;
                    lblWhatIfUnitCost.Text = Convert.ToDecimal(lblWhatIfUnitCost.Text).ToString("0.00000");
                }
                if (lblWhatIfLaborCost.Text != "")
                {
                    dcWiLabCost = Convert.ToDecimal(lblWhatIfLaborCost.Text);
                    dcWiLabCostI = dcWiLabCost;
                    lblWhatIfLaborCost.Text = Convert.ToDecimal(lblWhatIfLaborCost.Text).ToString("0.00");
                }
                //if (lblWhatIfMatCost.Text != "")
                //{
                //    dcWiMatCost = Convert.ToDecimal(lblWhatIfMatCost.Text);
                //    dcWiMatCostI += dcWiMatCost;
                //    lblWhatIfMatCost.Text =  Convert.ToDecimal(lblWhatIfMatCost.Text).ToString("0.00");
                //}
                if (lblLaborCostMat.Text != "")
                {
                    dcLabCostMat = Convert.ToDecimal(lblLaborCostMat.Text);
                    dcLabCostMatI = dcLabCostMat;
                    lblLaborCostMat.Text = Convert.ToDecimal(lblLaborCostMat.Text).ToString("0.00");
                }
                if (lblLaborCostPkg.Text != "")
                {
                    dcLabCostPkg = Convert.ToDecimal(lblLaborCostPkg.Text);
                    dcLabCostPkgI = dcLabCostPkg;
                    lblLaborCostPkg.Text = Convert.ToDecimal(lblLaborCostPkg.Text).ToString("0.00");
                }
                if (lblTotalLaborCost.Text != "")
                {
                    dcTotalLabCost = Convert.ToDecimal(lblTotalLaborCost.Text);
                    dcTotalLabCostI = dcTotalLabCost;
                    lblTotalLaborCost.Text = Convert.ToDecimal(lblTotalLaborCost.Text).ToString("0.00");
                }
                if (lblNewCost.Text != "")
                {
                    dcNewTotal = Convert.ToDecimal(lblNewCost.Text);
                    dcNewTotalI += dcNewTotal;
                    lblNewCost.Text = Convert.ToDecimal(lblNewCost.Text).ToString("0.00000");
                }
                TextBox txtMaterialCost = (TextBox)e.Row.FindControl("txtMaterialCost");
                Label lblMaterialCost = (Label)e.Row.FindControl("lblMaterialCost");
                if (txtMaterialCost.Text == "")
                {
                    txtMaterialCost.Text = lblMaterialCost.Text;
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblActualUnitCostTotal = (Label)e.Row.FindControl("lblActualUnitCostTotal");
                Label lblWhatIfUnitCostTotal = (Label)e.Row.FindControl("lblWhatIfUnitCostTotal");
                Label lblWhatIfLaborCostTotal = (Label)e.Row.FindControl("lblWhatIfLaborCostTotal");
                // Label lblWhatIfMatCostTotal = (Label)e.Row.FindControl("lblWhatIfMatCostTotal");
                Label lblLaborCostMatTotal = (Label)e.Row.FindControl("lblLaborCostMatTotal");
                Label lblLaborCostPkgTotal = (Label)e.Row.FindControl("lblLaborCostPkgTotal");
                Label lblTotalLaborCostTotal = (Label)e.Row.FindControl("lblTotalLaborCostTotal");
                Label lblNewCostTotal = (Label)e.Row.FindControl("lblNewCostTotal");

                lblActualUnitCostTotal.Text = dcActualUnitCostI.ToString("0.00");
                lblWhatIfUnitCostTotal.Text = dcWiUnitCostI.ToString("0.00");
                lblWhatIfLaborCostTotal.Text = "$" + dcWiLabCostI.ToString("0.00");
                //lblWhatIfMatCostTotal.Text = "$" + dcWiMatCostI.ToString("0.00");
                lblLaborCostMatTotal.Text = dcLabCostMatI.ToString("0.00");
                lblLaborCostPkgTotal.Text = dcLabCostPkgI.ToString("0.00");
                lblTotalLaborCostTotal.Text = dcTotalLabCostI.ToString("0.00");
                lblNewCostTotal.Text = dcNewTotalI.ToString("0.00");
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void txtStockCode_TextChanged(object sender, EventArgs e)
    {
        lblDescription.Text = SharedFunctions.GetStockCodeDesc(txtStockCode.Text.Trim());
        gvReportCondensed.DataSource = null;
        gvReportCondensed.DataBind();
        gvIngredients.DataSource = null;
        gvIngredients.DataBind();
        gvComponents.DataSource = null;
        gvComponents.DataBind();
        gvReportWipCost.DataSource = null;
        gvReportWipCost.DataBind();
        gvEstimatedCost.DataSource = null;
        gvEstimatedCost.DataBind();
        txtPrice.Text = "";
        txtShrinkagePercentage.Text = "2";
        lblShrinkage.Text = "";
        lblMargin.Text = "";
        lblMarkup.Text = "";
    }
    protected void txtMaterialCost_TextChanged(object sender, EventArgs e)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            decimal dcQtyPer = 0;
            decimal dcNewMaterialCost = 0;
            decimal dcNewCost = 0;
            TextBox txtMaterialCost = (TextBox)sender;
            Label lblNewCost = (Label)txtMaterialCost.Parent.FindControl("lblNewCost");
            Label lblQtyPer = (Label)txtMaterialCost.Parent.FindControl("lblQtyPer");
            Label lblStockCode = (Label)txtMaterialCost.Parent.FindControl("lblStockCode");
            CheckBox chkOverwrite = (CheckBox)txtMaterialCost.Parent.FindControl("chkOverwrite");

            if (txtMaterialCost.Text.Trim() != "")
            {
                dcNewMaterialCost = Convert.ToDecimal(txtMaterialCost.Text.Trim());
                if (chkOverwrite.Checked)
                {
                    //Add or Update...
                    bool bEstimateCostExists = false;
                    bEstimateCostExists = EstimateIngredientCostExists(lblStockCode.Text);
                    if (bEstimateCostExists)
                    {//Update...
                        InvEstimatedIngredientCost ie = db.InvEstimatedIngredientCost.Single(p => p.StockCode == lblStockCode.Text);
                        ie.EstimatedIngredientCost = dcNewMaterialCost;
                        ie.DateModified = DateTime.Now;
                        db.SubmitChanges();

                    }
                    else//Add...
                    {
                        InvEstimatedIngredientCost ie = new InvEstimatedIngredientCost();
                        ie.StockCode = lblStockCode.Text;
                        ie.EstimatedIngredientCost = dcNewMaterialCost;
                        ie.DateAdded = DateTime.Now;
                        db.InvEstimatedIngredientCost.InsertOnSubmit(ie);
                        db.SubmitChanges();
                    }
                }
            }
            dcQtyPer = Convert.ToDecimal(lblQtyPer.Text);

            dcNewCost = dcNewMaterialCost * dcQtyPer;

            lblNewCost.Text = dcNewCost.ToString("0.00000");
            decimal dcNewAmtTotal = 0;
            foreach (GridViewRow grvRow in gvIngredients.Rows)
            {
                if (grvRow.RowType == DataControlRowType.DataRow)
                {

                    decimal dcNewAmt = 0;
                    Label lblNewCostLabel = (Label)grvRow.FindControl("lblNewCost");
                    dcNewAmt = Convert.ToDecimal(lblNewCostLabel.Text);
                    dcNewAmtTotal += dcNewAmt;

                }
            }
            gvIngredients.FooterRow.Cells[8].Text = dcNewAmtTotal.ToString("0.00000");
            decimal dcActualRecipeCost = 0;
            decimal dcActualPackagingCost = 0;
            decimal dcTotalCost = 0;
            decimal dcLaborCost = 0;

            if (chkOverwrite.Checked == false)
            {
                foreach (GridViewRow grvRow in gvReportCondensed.Rows)
                {
                    if (grvRow.RowType == DataControlRowType.DataRow)
                    {//Summary Grid...OK to Recalcuate...
                        decimal dcNewAmt = 0;
                        Label lblQtyPerSummary = (Label)grvRow.FindControl("lblQtyPer");
                        dcQtyPer = Convert.ToDecimal(lblQtyPerSummary.Text);
                        dcNewAmt = dcQtyPer * dcNewAmtTotal;
                        Label lblActualUnitCost = (Label)grvRow.FindControl("lblActualUnitCost");
                        Label lblPackagingCost = (Label)grvRow.FindControl("lblPackagingCost");
                        Label lblTotalLaborCost = (Label)grvRow.FindControl("lblTotalLaborCost");
                        dcLaborCost = Convert.ToDecimal(lblTotalLaborCost.Text);
                        lblActualUnitCost.Text = dcNewAmt.ToString("0.00");
                        Label lblTotal = (Label)grvRow.FindControl("lblTotal");
                        lblTotal.Text = (Convert.ToDecimal(lblActualUnitCost.Text) + Convert.ToDecimal(lblPackagingCost.Text) + Convert.ToDecimal(lblTotalLaborCost.Text)).ToString();
                        dcActualRecipeCost = Convert.ToDecimal(lblActualUnitCost.Text);
                        dcActualPackagingCost = Convert.ToDecimal(lblPackagingCost.Text);
                        dcTotalCost = Convert.ToDecimal(lblTotal.Text);
                    }
                }
            }
            Calculate(dcActualRecipeCost, dcActualPackagingCost, dcLaborCost, dcTotalCost);
        }
        string sStockCode = "";
        if (txtStockCode.Text.Trim() == "")
        {
            lblError.Text = "**No Stock Code selected!";
            lblError.ForeColor = Color.Red;
            return;
        }
        else
        {
            sStockCode = txtStockCode.Text.Trim();
            RunEstimatedCostReport(sStockCode);
        }

    }
    protected void txtPackagingCost_TextChanged(object sender, EventArgs e)
    {//Packaging...
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            decimal dcQtyPer = 0;
            decimal dcNewPackagingCost = 0;
            decimal dcNewCost = 0;
            TextBox txtPackagingCost = (TextBox)sender;
            Label lblNewCost = (Label)txtPackagingCost.Parent.FindControl("lblNewCost");
            Label lblQtyPer = (Label)txtPackagingCost.Parent.FindControl("lblQtyPer");
            Label lblStockCode = (Label)txtPackagingCost.Parent.FindControl("lblStockCode");
            CheckBox chkOverwrite = (CheckBox)txtPackagingCost.Parent.FindControl("chkOverwrite");

            if (txtPackagingCost.Text.Trim() != "")
            {

                dcNewPackagingCost = Convert.ToDecimal(txtPackagingCost.Text.Trim());
                if (chkOverwrite.Checked)
                {
                    //Add or Update...
                    bool bEstimateCostExists = false;
                    bEstimateCostExists = EstimatePackagingCostExists(lblStockCode.Text);
                    if (bEstimateCostExists)
                    {//Update...
                        InvEstimatedPackagingCost ie = db.InvEstimatedPackagingCost.Single(p => p.StockCode == lblStockCode.Text);
                        ie.EstimatedPackagingCost = dcNewPackagingCost;
                        ie.DateModified = DateTime.Now;
                        db.SubmitChanges();

                    }
                    else//Add...
                    {
                        InvEstimatedPackagingCost ie = new InvEstimatedPackagingCost();
                        ie.StockCode = lblStockCode.Text;
                        ie.EstimatedPackagingCost = dcNewPackagingCost;
                        ie.DateAdded = DateTime.Now;
                        db.InvEstimatedPackagingCost.InsertOnSubmit(ie);
                        db.SubmitChanges();
                    }
                }
            }
            dcQtyPer = Convert.ToDecimal(lblQtyPer.Text);

            dcNewCost = dcNewPackagingCost * dcQtyPer;

            lblNewCost.Text = dcNewCost.ToString("0.00000");
            decimal dcNewAmtTotal = 0;
            foreach (GridViewRow grvRow in gvComponents.Rows)
            {
                if (grvRow.RowType == DataControlRowType.DataRow)
                {

                    decimal dcNewAmt = 0;
                    Label lblNewCostLabel = (Label)grvRow.FindControl("lblNewCost");
                    dcNewAmt = Convert.ToDecimal(lblNewCostLabel.Text);
                    dcNewAmtTotal += dcNewAmt;

                }
            }
            gvComponents.FooterRow.Cells[8].Text = dcNewAmtTotal.ToString("0.00");
            decimal dcActualRecipeCost = 0;
            decimal dcActualPackagingCost = 0;
            decimal dcTotalCost = 0;
            decimal dcLaborCost = 0;
            if (chkOverwrite.Checked == false)
            {
                foreach (GridViewRow grvRow in gvReportCondensed.Rows)
                {//Summary grid...
                    if (grvRow.RowType == DataControlRowType.DataRow)
                    {//OK to recalculate..
                        decimal dcNewAmt = 0;
                        Label lblQtyPerSummary = (Label)grvRow.FindControl("lblQtyPer");
                        dcQtyPer = Convert.ToDecimal(lblQtyPerSummary.Text);
                        dcNewAmt = dcNewAmtTotal;//Don't multiply by QtyPer for Packaging...
                        Label lblActualUnitCost = (Label)grvRow.FindControl("lblActualUnitCost");
                        Label lblPackagingCost = (Label)grvRow.FindControl("lblPackagingCost");
                        Label lblTotalLaborCost = (Label)grvRow.FindControl("lblTotalLaborCost");
                        dcLaborCost = Convert.ToDecimal(lblTotalLaborCost.Text);
                        lblPackagingCost.Text = dcNewAmt.ToString("0.00");

                        Label lblTotal = (Label)grvRow.FindControl("lblTotal");
                        lblTotal.Text = (Convert.ToDecimal(lblActualUnitCost.Text) + Convert.ToDecimal(lblPackagingCost.Text) + Convert.ToDecimal(lblTotalLaborCost.Text)).ToString();
                        dcActualRecipeCost = Convert.ToDecimal(lblActualUnitCost.Text);
                        dcActualPackagingCost = Convert.ToDecimal(lblPackagingCost.Text);
                        dcTotalCost = Convert.ToDecimal(lblTotal.Text);
                    }
                }
            }
            Calculate(dcActualRecipeCost, dcActualPackagingCost, dcLaborCost, dcTotalCost);
        }
        string sStockCode = "";
        if (txtStockCode.Text.Trim() == "")
        {
            lblError.Text = "**No Stock Code selected!";
            lblError.ForeColor = Color.Red;
            return;
        }
        else
        {
            sStockCode = txtStockCode.Text.Trim();
            RunEstimatedCostReport(sStockCode);
        }
    }
    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        DateTime dtFirstDayOfLastMonth = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
        DateTime dtLastDayOfLastMonth = Convert.ToDateTime(dtFirstDayOfLastMonth.AddMonths(1).AddDays(-1).ToShortDateString() + " 23:59:59");
        switch (ddlPeriod.SelectedIndex)
        {
            case 0://Range
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                break;
            case 1://Last Month.
                txtStartDate.Text = dtFirstDayOfLastMonth.ToShortDateString();
                txtEndDate.Text = dtLastDayOfLastMonth.ToShortDateString();
                break;
            case 2://30 days.
                txtStartDate.Text = DateTime.Now.AddDays(-30).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 3://60 days.
                txtStartDate.Text = DateTime.Now.AddDays(-60).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 4://3 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-3).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 5://6 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-6).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 6://9 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-9).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 7://12 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-12).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
        }
    }
    protected void gvReportCost_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Label lblStockCode = (Label)e.Row.FindControl("lblStockCode");
            //Label lblDate = (Label)e.Row.FindControl("lblDate");
            //Label lblDescription = (Label)e.Row.FindControl("lblDescription");
            //Label lblParentJob = (Label)e.Row.FindControl("lblParentJob");
            //Label lblUom = (Label)e.Row.FindControl("lblUom");
            //Label lblQty = (Label)e.Row.FindControl("lblQty");
            Label lblRecipeCost = (Label)e.Row.FindControl("lblRecipeCost");
            Label lblPkgCost = (Label)e.Row.FindControl("lblPkgCost");
            Label lblLaborCost = (Label)e.Row.FindControl("lblLaborCost");
            Label lblTotal = (Label)e.Row.FindControl("lblTotal");

            //switch (lblStockCode.Text)
            //{
            //    case "TOTALS":

            //        lblDate.Text = "";
            //        lblParentJob.Text = "";
            //        lblUom.Text = "";

            //        lblStockCode.Text = "";


            //        lblDescription.Text = "TOTALS:";
            //        lblDescription.ForeColor = Color.Black;
            //        lblDescription.Font.Bold = true;
            //        lblDescription.Font.Size = FontUnit.Point(10);

            //        lblRecipeCost.Font.Bold = true;
            //        lblPkgCost.Font.Bold = true;
            //        lblQty.Font.Bold = true;
            //        lblLaborCost.Font.Bold = true;
            //        break;
            //    case "AVERAGE cost per unit":
            //        lblDescription.Text = "&nbsp;&nbsp;AVERAGE";

            //        lblDate.Text = "";
            //        lblParentJob.Text = "";
            //        lblUom.Text = "";
            //        lblQty.Text = "";

            //        lblStockCode.Text = "";
            //        lblStockCode.Enabled = false;

            //        lblDescription.ForeColor = Color.Black;
            //        lblDescription.Font.Bold = true;
            //        lblDescription.Font.Italic = true;
            //        lblDescription.Font.Size = FontUnit.Point(10);
            //        break;
            //    case "WHAT-IF cost per unit":
            //        lblDescription.Text = "&nbsp;&nbsp;WHAT-IF";

            //        lblDate.Text = "";
            //        lblParentJob.Text = "";
            //        lblUom.Text = "";
            //        lblQty.Text = "";

            //        lblStockCode.Text = "";
            //        lblStockCode.Enabled = false;

            //        lblDescription.ForeColor = Color.Black;
            //        lblDescription.Font.Bold = true;
            //        lblDescription.Font.Italic = true;
            //        lblDescription.Font.Size = FontUnit.Point(10);
            //        break;
            //    case "VARIANCE cost per unit":
            //        lblDescription.Text = "&nbsp;&nbsp;VARIANCE";

            //        lblDate.Text = "";
            //        lblParentJob.Text = "";
            //        lblUom.Text = "";
            //        lblQty.Text = "";

            //        lblStockCode.Text = "";
            //        lblStockCode.Enabled = false;

            //        lblDescription.ForeColor = Color.Black;
            //        lblDescription.Font.Bold = true;
            //        lblDescription.Font.Italic = true;
            //        lblDescription.Font.Size = FontUnit.Point(10);
            //        break;
            //    case "SPACE":

            //        lblDate.Text = "";
            //        lblParentJob.Text = "";
            //        lblUom.Text = "";
            //        lblQty.Text = "";
            //        lblPkgCost.Text = "";
            //        lblLaborCost.Text = "";
            //        lblRecipeCost.Text = "";
            //        lblTotal.Text = "";

            //        lblStockCode.Text = "";
            //        lblStockCode.Enabled = false;

            //        lblDescription.Text = "COST PER UNIT:";
            //        lblDescription.ForeColor = Color.Black;
            //        lblDescription.Font.Bold = true;
            //        lblDescription.Font.Size = FontUnit.Point(10);
            //        break;
            //    default:
            //        lblStockCode.Font.Bold = true;
            //        break;
            //}

            //if (lblDate.Text != "")
            //{
            //    lblDate.Text = Convert.ToDateTime(lblDate.Text).ToShortDateString();
            //}
            //if (lblQty.Text != "")
            //{
            //    lblQty.Text = Convert.ToDecimal(lblQty.Text).ToString("0.0");
            //}
            if (lblRecipeCost.Text != "")
            {
                if (lblRecipeCost.Text.Trim().Contains("("))
                {
                    lblRecipeCost.Text = "$" + "(" + Convert.ToDecimal(lblRecipeCost.Text.Replace("$", "").Replace("(", "").Replace(")", "")).ToString("#,0.00") + ")";
                }
                else
                {
                    lblRecipeCost.Text = "$" + Convert.ToDecimal(lblRecipeCost.Text.Replace("$", "").Replace("(", "").Replace(")", "")).ToString("#,0.00");
                }
            }
            if (lblPkgCost.Text != "")
            {
                if (lblPkgCost.Text.Trim().Contains("("))
                {
                    lblPkgCost.Text = "$" + "(" + Convert.ToDecimal(lblPkgCost.Text.Replace("$", "").Replace("(", "").Replace(")", "")).ToString("#,0.00") + ")";
                }
                else
                {
                    lblPkgCost.Text = "$" + Convert.ToDecimal(lblPkgCost.Text.Replace("$", "").Replace("(", "").Replace(")", "")).ToString("#,0.00");
                }

            }
            if (lblLaborCost.Text != "")
            {
                if (lblLaborCost.Text.Trim().Contains("("))
                {
                    lblLaborCost.Text = "$" + "(" + Convert.ToDecimal(lblLaborCost.Text.Replace("$", "").Replace("(", "").Replace(")", "")).ToString("#,0.00") + ")";
                }
                else
                {
                    lblLaborCost.Text = "$" + Convert.ToDecimal(lblLaborCost.Text.Replace("$", "").Replace("(", "").Replace(")", "")).ToString("#,0.00");
                }

            }
            if (lblTotal.Text != "")
            {
                if (lblTotal.Text.Trim().Contains("("))
                {
                    lblTotal.Text = "$" + "(" + Convert.ToDecimal(lblTotal.Text.Replace("$", "").Replace("(", "").Replace(")", "")).ToString("#,0.00") + ")";
                }
                else
                {
                    lblTotal.Text = "$" + Convert.ToDecimal(lblTotal.Text.Replace("$", "").Replace("(", "").Replace(")", "")).ToString("#,0.00");
                }

            }

        }
    }
    protected void gvEstimatedCost_RowDataBound(object sender, GridViewRowEventArgs e)
    {

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