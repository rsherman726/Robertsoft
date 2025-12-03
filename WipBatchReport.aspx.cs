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

public partial class WipBatchReport : System.Web.UI.Page
{

    decimal dcIngredUsedFactorTotal = 0;
    decimal dcIngredQtyManufacturedTotal = 0;
    decimal dcIngredQtyIssuedTotal = 0;
    decimal dcQtyMainTotal = 0;


    #region Subs
    private void RunReport(string sStockCode)
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
        string sJobCount = "NULL";
        if (txtJobCount.Text.Trim() != "")
        {
            sJobCount = "'" + txtJobCount.Text.Trim() + "'";
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
            lblError.Text = sMsg;
            return;
        }
        switch (ddlPeriod.SelectedValue)
        {
            case "All":
                sStartDate = "'" + DateTime.Now.AddYears(-5).ToShortDateString() + "'";//Five Years back...
                sEndDate = "'" + DateTime.Now.ToShortDateString() + "'";//today...
                break;
            case "Range":
                if (txtStartDate.Text.Trim() == "" && txtEndDate.Text.Trim() == "")
                {//Should ever get here...
                    sStartDate = "'" + DateTime.Now.AddDays(-7).ToShortDateString() + "'";//one day back...
                    sEndDate = "'" + DateTime.Now.ToShortDateString() + "'";//today...
                }
                else
                {
                    sStartDate = "'" + txtStartDate.Text.Trim() + "'";
                    sEndDate = "'" + txtEndDate.Text.Trim() + "'";
                }
                break;
            default://All Others...
                sStartDate = "'" + txtStartDate.Text.Trim() + "'";
                sEndDate = "'" + txtEndDate.Text.Trim() + "'";
                break;
        }
        if (sJobCount == "NULL")
        {
            sSQL = "EXEC spGetWipCostAnalysis ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@FromStockCode=" + sStockCode + ",";
            sSQL += "@ToStockCode=null";
        }
        else//Use Job Count...
        {
            sSQL = "EXEC spGetWipCostAnalysis ";
            sSQL += "@FromStockCode=" + sStockCode + ",";
            sSQL += "@JobCount=" + sJobCount;

        }

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
        dtNs.Columns.Add("RowType", typeof(string));
        dtNs.Columns.Add("ParentPart", typeof(string));
        dtNs.Columns.Add("ParentDescription", typeof(string));
        dtNs.Columns.Add("ParentCompleteDate", typeof(DateTime));
        dtNs.Columns.Add("ParentJob", typeof(string));
        dtNs.Columns.Add("ParentUom", typeof(string));
        dtNs.Columns.Add("ParentQtyManuf", typeof(decimal));
        dtNs.Columns.Add("RecipeMatCostTotUsed", typeof(decimal));
        dtNs.Columns.Add("ParentMatCostTotAdj", typeof(decimal));
        dtNs.Columns.Add("TotLabCost", typeof(decimal));
        dtNs.Columns.Add("TotCost", typeof(decimal));
        dtNs.Columns.Add("RecipeMatCostTotUsedDisp", typeof(string));
        dtNs.Columns.Add("ParentMatCostTotAdjDisp", typeof(string));
        dtNs.Columns.Add("TotLabCostDisp", typeof(string));
        dtNs.Columns.Add("TotCostDisp", typeof(string));
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

                iCountRecipeJobs++;
            } // foreach drJd in dtJd
            //// Compute recipe-level summary fields that require completed accumulation from detail above.
            foreach (DataRow drJs in dtJs.Rows)
            {

                drJs["ParentMatCostTotAdj"] = (decimal)drJs["ParentMatCostTotUnadj"] - (decimal)drJs["RecipeMatCostTotUsed"] - (decimal)drJs["RecipeLabCostTotUsed"];
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
                DataRow drNs = dtNs.Rows.Add("D",
                drJs["ParentPart"],
                drJs["ParentDescription"],
                Convert.ToDateTime(dtParentCompleteDate),
                drJs["ParentJob"],
                drJs["ParentUom"],
                drJs["ParentQtyManuf"],
                drJs["RecipeMatCostTotUsed"],
                drJs["ParentMatCostTotAdj"],
                 (decimal)drJs["ParentLabCostTot"] + (decimal)drJs["RecipeLabCostTotUsed"],
                 (decimal)drJs["RecipeMatCostTotUsed"] + (decimal)drJs["ParentMatCostTotAdj"] + (decimal)drJs["ParentLabCostTot"] + (decimal)drJs["RecipeLabCostTotUsed"],
                Convert.ToDecimal(drJs["RecipeMatCostTotUsed"]).ToString("#,0.00"),
                Convert.ToDecimal(drJs["ParentMatCostTotAdj"]).ToString("#,0.00"),
                Convert.ToDecimal((decimal)drJs["ParentLabCostTot"] + Convert.ToDecimal(drJs["RecipeLabCostTotUsed"])).ToString("#,0.00"),
                Convert.ToDecimal((decimal)drJs["RecipeMatCostTotUsed"] + (decimal)drJs["ParentMatCostTotAdj"] + (decimal)drJs["ParentLabCostTot"] + (decimal)drJs["RecipeLabCostTotUsed"]).ToString("#,0.00"));
                // if (drJs["IsParentCompleteDateNull())
                //   drNs.SetParentCompleteDateNull();
                iCount++;
            }

            dtJs.AcceptChanges();

            if (dtJs.Rows.Count == 0)
            {
                lblError.Text = "No matching records found.";
                return;
            }

            /////RSS Comment dTotalParentQtyManuf = Qty, dTotalRecipeMatCostTotUsed = Recipe Cost, dTotalParentMatCostTotAdj = Pkg Cost, dTotalTotLabCost = Labor/OH, dTotalTotCost = TotalCost
            // AddBlankNsRow(dtNs);
            AddWghtSumNsRow(dtNs, dTotalParentQtyManuf, dTotalRecipeMatCostTotUsed, dTotalParentMatCostTotAdj, dTotalTotLabCost, dTotalTotCost);
            AddBlankNsRow(dtNs);
            AddPerUnitNsRow(dtNs, dTotalParentQtyManuf, dTotalRecipeMatCostTotUsed, dTotalParentMatCostTotAdj, dTotalTotLabCost, dTotalTotCost);
            // AddBlankNsRow(dtNs);
            //Pass in totals to be divided by...
            AddWicpuRow(dtNs, dTotalParentQtyManuf, dTotalWiRecipeMatCostTotUsed, dTotalWiParentMatCostTotAdjPkg, dTotalWiTotLabCost, dTotalWiTotCost, iCount);//Changed 10-5-2014...
            //AddBlankNsRow(dtNs);
            //Variance CPU...
            decimal dTotalVarRecipeMatCostTotUsed = dTotalRecipeMatCostTotUsed - dTotalWiRecipeMatCostTotUsed;

            decimal dTotalVarParentMatCostTotAdj = 0;// dTotalParentMatCostTotAdj - dTotalWiParentMatCostTotAdj;
            dTotalVarParentMatCostTotAdj = dTotalParentMatCostTotAdj - dTotalWiParentMatCostTotAdjPkg;
            decimal dTotalVarTotLabCost = dTotalTotLabCost - dTotalWiTotLabCost;
            decimal dTotalVarTotCost = dTotalTotCost - dTotalWiTotCost;
            //Pass in totals to be divided by...
            AddVcpuRow(dtNs, dTotalParentQtyManuf, dTotalVarRecipeMatCostTotUsed, dTotalVarParentMatCostTotAdj, dTotalVarTotLabCost, dTotalVarTotCost);

            dtJd.AcceptChanges();
            dtNs.AcceptChanges();

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

 

            Session["dtNs"] = dtNs;
            gvReportCondensed.DataSource = dtNs;
            gvReportCondensed.DataBind();
 
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            dtJs.Dispose();
            dtJd.Dispose();
            dtNs.Dispose();
        }
    }
 
    #endregion


    #region Functions
    //Level 1...
    private DataRow AddWghtSumNsRow(DataTable NewSummary, decimal TotalParentQtyManuf, decimal TotalRecipeMatCostTotUsed, decimal TotalParentMatCostTotAdj, decimal TotalTotLabCost, decimal TotalTotCost)
    {//WEIGHTED AVERAGE TOTAL
        DataRow dr = NewSummary.Rows.Add("A", "TOTALS", string.Empty, DateTime.MinValue, string.Empty, string.Empty, TotalParentQtyManuf,
         TotalRecipeMatCostTotUsed, TotalParentMatCostTotAdj, TotalTotLabCost, TotalTotCost,
         TotalRecipeMatCostTotUsed.ToString("#,0.00"), TotalParentMatCostTotAdj.ToString("#,0.00"), TotalTotLabCost.ToString("#,0.00"), TotalTotCost.ToString("#,0.00"));
        // dr.SetParentCompleteDateNull();
        return dr;
    }
    private DataRow AddPerUnitNsRow(DataTable NewSummary, decimal TotalParentQtyManuf, decimal TotalRecipeMatCostTotUsed, decimal TotalParentMatCostTotAdj, decimal TotalTotLabCost, decimal TotalTotCost)
    {//COST PER UNIT...
        if (TotalParentQtyManuf == 0M)
        {
            lblError.Text = "Warning: The total parent qty manufactured is zero, so the 'COST PER UNIT' summary row may be inaccurate.";
            TotalParentQtyManuf = 1M;
        }
        decimal dUnitRecipeMatCostTotUsed = TotalRecipeMatCostTotUsed / TotalParentQtyManuf;
        decimal dUnitParentMatCostTotAdj = TotalParentMatCostTotAdj / TotalParentQtyManuf;
        decimal dUnitTotLabCost = TotalTotLabCost / TotalParentQtyManuf;
        decimal dUnitTotCost = TotalTotCost / TotalParentQtyManuf;
        DataRow dr = NewSummary.Rows.Add("U", "AVERAGE cost per unit", string.Empty, DateTime.MinValue, string.Empty, string.Empty, 0M,
         dUnitRecipeMatCostTotUsed, dUnitParentMatCostTotAdj, dUnitTotLabCost, dUnitTotCost,
         dUnitRecipeMatCostTotUsed.ToString("#,0.00"), dUnitParentMatCostTotAdj.ToString("#,0.00"), dUnitTotLabCost.ToString("#,0.00"), dUnitTotCost.ToString("#,0.00"));
        // dr.SetParentCompleteDateNull();
        // dr.SetParentQtyManufNull();
        return dr;
    }
    private DataRow AddWicpuRow(DataTable NewSummary, decimal TotalParentQtyManuf, decimal TotalWiRecipeMatCostTotUsed, decimal TotalWiParentMatCostTotAdj, decimal TotalWiTotLabCost, decimal TotalWiTotCost, int iRowCount)
    {//WHAT-IF COST PER UNIT...Divide By to get Cost Per Unit...
        if (TotalParentQtyManuf == 0M)
        {
            lblError.Text = "Warning: The total parent qty manufactured is zero, so the 'WHAT-IF COST PER UNIT' summary row may be inaccurate.";
            TotalParentQtyManuf = 1M;
        }
        decimal dWiUnitRecipeMatCostTotUsed = TotalWiRecipeMatCostTotUsed / TotalParentQtyManuf;//Recipe...
        decimal dWiUnitParentMatCostTotAdj = TotalWiParentMatCostTotAdj / TotalParentQtyManuf;//Pkg...        
        decimal dWiUnitTotLabCost = TotalWiTotLabCost / TotalParentQtyManuf;//Labor...
        decimal dWiUnitTotCost = dWiUnitRecipeMatCostTotUsed + dWiUnitParentMatCostTotAdj + dWiUnitTotLabCost; //TotalWiTotCost / TotalParentQtyManuf;//Changed 7-8-14
        DataRow dr = NewSummary.Rows.Add("W", "WHAT-IF cost per unit", string.Empty, DateTime.MinValue, string.Empty, string.Empty, 0M,
         dWiUnitRecipeMatCostTotUsed, dWiUnitParentMatCostTotAdj, dWiUnitTotLabCost, dWiUnitTotCost,
         dWiUnitRecipeMatCostTotUsed.ToString("#,0.00"), dWiUnitParentMatCostTotAdj.ToString("#,0.00"), dWiUnitTotLabCost.ToString("#,0.00"), dWiUnitTotCost.ToString("#,0.00"));
        // dr.SetParentCompleteDateNull();
        //dr.SetParentQtyManufNull();
        return dr;
    }
    private DataRow AddVcpuRow(DataTable NewSummary, decimal TotalParentQtyManuf, decimal TotalVarRecipeMatCostTotUsed, decimal TotalVarParentMatCostTotAdj, decimal TotalVarTotLabCost, decimal TotalVarTotCost)
    {//VARIANCE COST PER UNIT...Divide By to get Cost Per Unit...
        if (TotalParentQtyManuf == 0M)
        {
            lblError.Text = "Warning: The total parent qty manufactured is zero, so the 'VARIANCE COST PER UNIT' summary row may be inaccurate.";
            TotalParentQtyManuf = 1M;
        }
        decimal dVarUnitRecipeMatCostTotUsed = TotalVarRecipeMatCostTotUsed / TotalParentQtyManuf;
        decimal dVarUnitParentMatCostTotAdj = TotalVarParentMatCostTotAdj / TotalParentQtyManuf;
        decimal dVarUnitTotLabCost = TotalVarTotLabCost / TotalParentQtyManuf;
        decimal dVarUnitTotCost = TotalVarTotCost / TotalParentQtyManuf;
        DataRow dr = NewSummary.Rows.Add("V", "VARIANCE cost per unit", string.Empty, DateTime.MinValue, string.Empty, string.Empty, 0M,
         dVarUnitRecipeMatCostTotUsed, dVarUnitParentMatCostTotAdj, dVarUnitTotLabCost, dVarUnitTotCost,
         dVarUnitRecipeMatCostTotUsed.ToString("#,0.00"), dVarUnitParentMatCostTotAdj.ToString("#,0.00"), dVarUnitTotLabCost.ToString("#,0.00"), dVarUnitTotCost.ToString("#,0.00"));
        //dr.SetParentCompleteDateNull();
        //dr.SetParentQtyManufNull();
        return dr;
    }
    private DataRow AddBlankNsRow(DataTable NewSummary)
    {
        DataRow dr = NewSummary.Rows.Add("B", "SPACE", string.Empty, DateTime.MinValue, string.Empty, string.Empty, 0M, 0M, 0M, 0M, 0M, string.Empty, string.Empty, string.Empty, string.Empty);
        return dr;
    }


    //Level 2...
    private DataRow AddPkgLaborSumNsRowL2(DataTable NewSummary, decimal dcQuantityTotal, decimal dcPackagingLabor)
    {//Packaging Labor TOTAL
        if (dcQuantityTotal == 0M)
        {
            lblError.Text = "Warning: The total parent qty manufactured is zero, so the 'COST PER UNIT' summary row may be inaccurate.";
            dcQuantityTotal = 1M;
        }
        decimal dcPackagingLaborTotal = 0;
        dcPackagingLaborTotal = dcPackagingLabor / dcQuantityTotal;
        DataRow dr = NewSummary.Rows.Add("Packaging Labor", string.Empty, string.Empty, string.Empty, 0M, string.Empty, 0M, 0M, 0M, 0M, dcPackagingLaborTotal, dcPackagingLaborTotal, dcPackagingLaborTotal, dcPackagingLaborTotal, dcPackagingLaborTotal, dcPackagingLaborTotal, dcPackagingLaborTotal);
        return dr;
    }
    private DataRow AddWghtSumNsRowL2(DataTable NewSummary, decimal dcQuantityTotal, decimal dcPckTotal, decimal dcRecipeCost, decimal dcMatCost, decimal dcLaborCost)
    {//WEIGHTED AVERAGE TOTAL
        DataRow dr = NewSummary.Rows.Add("TOTALS", string.Empty, string.Empty, string.Empty, dcQuantityTotal, string.Empty, dcPckTotal, dcRecipeCost, 0M, dcMatCost, 0M, dcLaborCost, 0M, 0M, 0M, 0M, 0M, 0M);
        return dr;
    }
    private DataRow AddPerUnitNsRowL2(DataTable NewSummary, decimal dcQuantityTotal, decimal dcRecipeCostPerUom, decimal dcMatCostPerUom, decimal dcLaborCostPerUom)
    {//COST PER UNIT...
        if (dcQuantityTotal == 0M)
        {
            lblError.Text = "Warning: The total parent qty manufactured is zero, so the 'COST PER UNIT' summary row may be inaccurate.";
            dcQuantityTotal = 1M;
        }
        decimal dUnitIngredientCost = dcRecipeCostPerUom;
        decimal dUnitPkgCost = dcMatCostPerUom;
        decimal dUnitLaborCost = dcLaborCostPerUom;

        DataRow dr = NewSummary.Rows.Add("AVERAGE", string.Empty, string.Empty, string.Empty, 0M, 0M, 0M, dUnitIngredientCost, 0M, dUnitPkgCost, 0M, dUnitLaborCost, 0M, 0M, 0M, 0M, 0M, 0M);
        //dUnitIngredientCost.ToString("C3"), dUnitPkgCost.ToString("C3"), dUnitLaborCost.ToString("C3"));
        return dr;
    }
    private DataRow AddWicpuRowL2(DataTable NewSummary, decimal dcQuantityTotal, decimal dcWiRecipeCost, decimal dcWiPkgCost, decimal dcWiLaborCost)
    {//WHAT-IF COST PER UNIT...Divide By to get Cost Per Unit...
        if (dcQuantityTotal == 0M)
        {
            lblError.Text = "Warning: The total parent qty manufactured is zero, so the 'WHAT-IF COST PER UNIT' summary row may be inaccurate.";
            dcQuantityTotal = 1M;
        }
        decimal dWiUnitTotRecipeCost = dcWiRecipeCost;
        decimal dWiUnitTotPkgCost = dcWiPkgCost;
        decimal dWiUnitTotLabCost = dcWiLaborCost;

        DataRow dr = NewSummary.Rows.Add("WHAT-IF", string.Empty, string.Empty, string.Empty, 0M, 0M, 0M, dWiUnitTotRecipeCost, 0M, dWiUnitTotPkgCost, 0M, dWiUnitTotLabCost, 0M, 0M, 0M, 0M, 0M, 0M);
        //dWiUnitTotRecipeCost.ToString("C3"), dWiUnitTotPkgCost.ToString("C3"), dWiUnitTotLabCost.ToString("C3"));

        return dr;
    }
    private DataRow AddVcpuRowL2(DataTable NewSummary, decimal dcQuantityTotal, decimal dcTotalVarTotRecipeCost, decimal dcTotalVarTotPkgCost, decimal dcTotalVarTotLabCost)
    {//VARIANCE COST PER UNIT...Divide By to get Cost Per Unit...
        if (dcQuantityTotal == 0M)
        {
            lblError.Text = "Warning: The total parent qty manufactured is zero, so the 'VARIANCE COST PER UNIT' summary row may be inaccurate.";
            dcQuantityTotal = 1M;
        }
        decimal dVarUnitTotRecipeCost = dcTotalVarTotRecipeCost;
        decimal dVarUnitTotPkgCost = dcTotalVarTotPkgCost;
        decimal dVarUnitTotLabCost = dcTotalVarTotLabCost;

        DataRow dr = NewSummary.Rows.Add("VARIANCE", string.Empty, string.Empty, string.Empty, 0M, 0M, 0M, dVarUnitTotRecipeCost, 0M, dVarUnitTotPkgCost, 0M, dVarUnitTotLabCost, 0M, 0M, 0M, 0M, 0M, 0M);

        //dVarUnitRecipeMatCostTotUsed.ToString("C3"), dVarUnitParentMatCostTotAdj.ToString("C3"), dVarUnitTotLabCost.ToString("C3"));
        return dr;
    }
    private DataRow AddBlankNsRowL2(DataTable NewSummary)
    {
        DataRow dr = NewSummary.Rows.Add("SPACE", string.Empty, string.Empty, string.Empty, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M, 0M);
        return dr;
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


        }
    }

    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
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
        }
       
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
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
        if (txtJobCount.Text.Trim() != "")
        {
            if (!SharedFunctions.IsNumeric(txtJobCount.Text.Trim()))
            {
                lblError.Text = "**Job Count must be numeric!";
                lblError.ForeColor = Color.Red;
                return;
            }
        }
        RunReport(sStockCode);

    }
    protected void lbStockCode_SelectedIndexChanged(object sender, EventArgs e)
    {
         
    }
    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {
         
    }
    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {
         
    }
    protected void lbnInitializeStockCode_Click(object sender, EventArgs e)
    {
        
    }
    protected void gvReportCondensed_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbnStockCode = (LinkButton)e.Row.FindControl("lbnStockCode");
            Label lblDate = (Label)e.Row.FindControl("lblDate");
            Label lblDescription = (Label)e.Row.FindControl("lblDescription");
            Label lblParentJobTruncated = (Label)e.Row.FindControl("lblParentJobTruncated");
            Label lblUom = (Label)e.Row.FindControl("lblUom");
            Label lblQty = (Label)e.Row.FindControl("lblQty");
 

            switch (lbnStockCode.Text)
            {
                case "TOTALS":

                    lblDate.Text = "";
                    lblParentJobTruncated.Text = "";
                    lblUom.Text = "";

                    lbnStockCode.Text = "";


                    lblDescription.Text = "TOTALS:";
                    lblDescription.ForeColor = Color.Black;
                    lblDescription.Font.Bold = true;
                    lblDescription.Font.Size = FontUnit.Point(10);

 
                    lblQty.Font.Bold = true;
 
                    break;
                case "AVERAGE cost per unit":
                    lblDescription.Text = "";

                    lblDate.Text = "";
                    lblParentJobTruncated.Text = "";
                    lblUom.Text = "";
                    lblQty.Text = "";

                    lbnStockCode.Text = "";
                    lbnStockCode.Enabled = false;

                    lblDescription.ForeColor = Color.Black;
                    lblDescription.Font.Bold = true;
                    lblDescription.Font.Italic = true;
                    lblDescription.Font.Size = FontUnit.Point(10);
                    break;
                case "WHAT-IF cost per unit":
                    lblDescription.Text = "";

                    lblDate.Text = "";
                    lblParentJobTruncated.Text = "";
                    lblUom.Text = "";
                    lblQty.Text = "";

                    lbnStockCode.Text = "";
                    lbnStockCode.Enabled = false;

                    lblDescription.ForeColor = Color.Black;
                    lblDescription.Font.Bold = true;
                    lblDescription.Font.Italic = true;
                    lblDescription.Font.Size = FontUnit.Point(10);
                    break;
                case "VARIANCE cost per unit":
                    lblDescription.Text = "";

                    lblDate.Text = "";
                    lblParentJobTruncated.Text = "";
                    lblUom.Text = "";
                    lblQty.Text = "";

                    lbnStockCode.Text = "";
                    lbnStockCode.Enabled = false;

                    lblDescription.ForeColor = Color.Black;
                    lblDescription.Font.Bold = true;
                    lblDescription.Font.Italic = true;
                    lblDescription.Font.Size = FontUnit.Point(10);
                    break;
                case "SPACE":

                    lblDate.Text = "";
                    lblParentJobTruncated.Text = "";
                    lblUom.Text = "";
                    lblQty.Text = "";
 

                    lbnStockCode.Text = "";
                    lbnStockCode.Enabled = false;

                    lblDescription.Text = "";
                    lblDescription.ForeColor = Color.Black;
                    lblDescription.Font.Bold = true;
                    lblDescription.Font.Size = FontUnit.Point(10);
                    break;
                default:
                    lbnStockCode.Font.Bold = true;
                    string sSource = lblParentJobTruncated.Text;
                    var sResult = int.Parse(sSource).ToString();
                    lblParentJobTruncated.Text = sResult;
                    break;
            }

            if (lblDate.Text != "")
            {
                lblDate.Text = Convert.ToDateTime(lblDate.Text).ToShortDateString();
            }
            if (lblQty.Text != "")
            {
                lblQty.Text = Convert.ToDecimal(lblQty.Text).ToString("0.0");
            }

        }
    }
    protected void gvReportCondensed_RowCommand(object sender, GridViewCommandEventArgs e)
    {//1st Level...
        int idx = 0;//bound in html code...
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        DataTable dtLevel2 = new DataTable();
        DataTable dtJd = new DataTable();
        dtJd = (DataTable)Session["dtJd"];
        switch (e.CommandName)
        {
            case "Select":
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...
                Label lblParentJob = (Label)gvReportCondensed.Rows[idx].FindControl("lblParentJob");
                Label lblParentJobTruncated = (Label)gvReportCondensed.Rows[idx].FindControl("lblParentJobTruncated");
                LinkButton lbnStockCode = (LinkButton)gvReportCondensed.Rows[idx].FindControl("lbnStockCode");
                Label lblDescription = (Label)gvReportCondensed.Rows[idx].FindControl("lblDescription");
                Label lblDate = (Label)gvReportCondensed.Rows[idx].FindControl("lblDate");

                var query = (from jd in dtJd.AsEnumerable()
                             where jd.Field<string>("ParentJob") == lblParentJob.Text
                             orderby jd.Field<string>("RecipeStockCode")
                             select new
                             {
                                 RecipeStockCode = jd.Field<string>("RecipeStockCode"),
                                 RecipeDescription = jd.Field<string>("RecipeDescription"),
                                 RecipeJob = jd.Field<string>("RecipeJob"),
                                 RecipeUom = jd.Field<string>("RecipeUom"),
                                 RecipeQtyIssued = jd.Field<decimal>("RecipeQtyIssued"),
                                 StockUom = jd.Field<string>("StockUom"),
                                 PckQty = jd.Field<decimal>("ParentQtyManuf"),
                                 IngredientCost = jd.Field<decimal>("RecipeMatCostUsedTot"),
                                 IngredientCostPerUom = jd.Field<decimal>("RecipeMatCostUsedUnit"),
                                 PkgCost = jd.Field<decimal>("RecipeTrnValueTot"),//Pkg Cost
                                 PkgCostPerUom = jd.Field<decimal>("RecipeTrnValueUnit"),//Pkg Cost per Uom
                                 LaborCost = jd.Field<decimal>("RecipeLabCostUsedTot"),
                                 LaborCostPerUom = jd.Field<decimal>("RecipeLabCostUsedUnit"),
                                 RecipeWhatIfMatCost = jd.Field<decimal>("WiRecipeMatCostWhole"),
                                 RecipeWhatIfLabCost = jd.Field<decimal>("FgUnitLabor"),
                                 RecipeWhatIfPkgCost = jd.Field<decimal>("RecipeWhatIfPkgCost"),
                                 ParentLabCostTot = jd.Field<decimal>("ParentLabCostTot"),
                                 QtyPer = jd.Field<decimal>("QtyPer"),
                             });

                dtLevel2 = SharedFunctions.LINQToDataTable(query);

                decimal dcQuantityTotal = 0;
                decimal dcIngredientCostTotal = 0;
                decimal dcIngredientCostPerUnitTotal = 0;
                decimal dcPkgCostTotal = 0;
                decimal dcPkgCostPerUnitTotal = 0;
                decimal dcLaborCostTotal = 0;
                decimal dcLaborCostPerUnitTotal = 0;
                decimal dcPackagingLaborTotal = 0;
                decimal dcWiIngredientCostPerUnitTotal = 0;
                decimal dcWiPkgCostPerUnitTotal = 0;
                decimal dcWiLaborCostPerUnitTotal = 0;
                string sRecipeJob = "";
                decimal dcPkgQtyTotal = 0;
                decimal dcQtyPer = 0;
                int iPkgRowCount = 0;
                int iIngredRowCount = 0;
                int iTotalRows = 0;
                decimal dcPckTotal = 0;
                foreach (var a in query)
                {
                    sRecipeJob = a.RecipeJob;
                    dcQuantityTotal += a.RecipeQtyIssued;
                    dcIngredientCostTotal += a.IngredientCost;
                    dcIngredientCostPerUnitTotal += a.IngredientCostPerUom;
                    dcPkgCostTotal += a.PkgCost;
                    dcPkgCostPerUnitTotal += a.PkgCostPerUom;
                    dcLaborCostTotal += a.LaborCost;
                    dcLaborCostPerUnitTotal += a.LaborCostPerUom;
                    dcPackagingLaborTotal += a.ParentLabCostTot;
                    if (sRecipeJob != "")
                    {//Recipe 
                        dcWiIngredientCostPerUnitTotal += a.RecipeWhatIfMatCost;
                        dcPckTotal += a.PckQty;
                        dcQtyPer += a.QtyPer;
                        iIngredRowCount++;
                    }
                    else//sRecipeJob==""
                    {//get total of pkg,get qty of pkg, get rowcount of pkg...
                        dcWiPkgCostPerUnitTotal += a.RecipeWhatIfPkgCost;
                        dcPkgQtyTotal += a.RecipeQtyIssued;
                        iPkgRowCount++;
                    }

                    dcWiLaborCostPerUnitTotal += a.RecipeWhatIfLabCost;
                }
                if (iIngredRowCount == 0)
                {
                    iIngredRowCount = 1;
                }
                iTotalRows = iPkgRowCount + iIngredRowCount;
                if (dcQtyPer == 0)
                {
                    dcQtyPer = 1;
                }
                dcQtyPer = dcQtyPer / iIngredRowCount;
                if (dcPckTotal == 0)
                {
                    dcPckTotal = 1;
                }

                dcPckTotal = dcPckTotal / iIngredRowCount;

                AddPkgLaborSumNsRowL2(dtLevel2, iTotalRows, dcPackagingLaborTotal);

                dcLaborCostTotal = dcLaborCostTotal + (dcPackagingLaborTotal / iTotalRows);//Total up Labor Cost...

                AddWghtSumNsRowL2(dtLevel2, dcQuantityTotal, dcPckTotal, dcIngredientCostTotal, dcPkgCostTotal, dcLaborCostTotal);
                AddBlankNsRowL2(dtLevel2);
                dcLaborCostPerUnitTotal = dcLaborCostTotal / dcPckTotal;
                dcPkgCostPerUnitTotal = dcPkgCostTotal / dcPckTotal;
                dcIngredientCostPerUnitTotal = (dcIngredientCostTotal / dcPckTotal) / dcQtyPer;

                AddPerUnitNsRowL2(dtLevel2, iIngredRowCount, dcIngredientCostPerUnitTotal, dcPkgCostPerUnitTotal, dcLaborCostPerUnitTotal);

                //Pass in totals to be divided by...
                dcPkgQtyTotal = dcPkgQtyTotal / iPkgRowCount;
                dcWiPkgCostPerUnitTotal = dcWiPkgCostPerUnitTotal / dcPckTotal; //PkgWhatIf divided by PkgQtyTotal...
                dcWiIngredientCostPerUnitTotal = (dcWiIngredientCostPerUnitTotal / dcPckTotal) / dcQtyPer; //WhatIf divided by PkgQtyTotal...
                dcWiLaborCostPerUnitTotal = dcWiLaborCostPerUnitTotal / iTotalRows;
                //WhatIf Cost per unit...
                AddWicpuRowL2(dtLevel2, iIngredRowCount, dcWiIngredientCostPerUnitTotal, dcWiPkgCostPerUnitTotal, dcWiLaborCostPerUnitTotal);

                //Variance CPU...
                decimal dTotalVarTotRecipeCost = dcIngredientCostPerUnitTotal - dcWiIngredientCostPerUnitTotal;
                decimal dTotalVarTotPkgCost = dcPkgCostPerUnitTotal - dcWiPkgCostPerUnitTotal;
                decimal dTotalVarTotLabCost = dcLaborCostPerUnitTotal - dcWiLaborCostPerUnitTotal;

                //Pass in totals to be divided by...
                AddVcpuRowL2(dtLevel2, iIngredRowCount, dTotalVarTotRecipeCost, dTotalVarTotPkgCost, dTotalVarTotLabCost);

                dtLevel2.AcceptChanges();

                gvDetails1.DataSource = dtLevel2;
                gvDetails1.DataBind();

                //foreach (DataRow Row in dtLevel2.Rows)
                //{
                //    foreach (DataColumn c in dtLevel2.Columns)
                //    {
                //        Debug.WriteLine(c.ColumnName + ": " + Row[c.ColumnName].ToString());
                //    }
                //    Debug.WriteLine("----------------------------------------------------");
                //}

                ModalPopupExtenderDetails1.Show();
                pnlSearchCriteria.Visible = false;
                btnPreview.Visible = false;

                lblDetailsForJob.Text = "Details for Job <font color='Blue'><b>" + int.Parse(lblParentJobTruncated.Text).ToString() + "</b></font>  (<font color='Red'><b>" + lblDate.Text + "</b></font>) - (<font color='blue'><b>" + lbnStockCode.Text + "</font></b>) <font color='Navy'><b>" + lblDescription.Text + "</b></font>";

                dtLevel2.Dispose();
                dtJd.Dispose();
                break;
        }
    }
    protected void gvDetails1_RowCommand(object sender, GridViewCommandEventArgs e)
    {//2nd Level...
        int idx = 0;//bound in html code...
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        DataTable dt = new DataTable();
        DataTable dtIng = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        switch (e.CommandName)
        {
            case "Select":
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...
                LinkButton lbnStockCode = (LinkButton)gvDetails1.Rows[idx].FindControl("lbnStockCode");
                Label lblRecipeJobHidden = (Label)gvDetails1.Rows[idx].FindControl("lblRecipeJobHidden");
                Label lblRecipeJob = (Label)gvDetails1.Rows[idx].FindControl("lblRecipeJob");
                Label lblDescription = (Label)gvDetails1.Rows[idx].FindControl("lblDescription");
                Label lblQty = (Label)gvDetails1.Rows[idx].FindControl("lblQty");
                sSQL = "EXEC spGetWipCostAnalysis ";
                sSQL += "@FromJob='" + lblRecipeJobHidden.Text + "'";
                Debug.WriteLine(sSQL);
                dtIng = SharedFunctions.getDataTable(sSQL, conn, "dtReportIng");

                ////var query = (from jd in dtIng.AsEnumerable()
                ////             where jd.Field<string>("RecipeJob") == lblRecipeJob.Text
                ////             orderby jd.Field<string>("RecipeStockCode")
                ////             select new
                ////             {
                ////                 IngredStockCode = jd.Field<string>("RecipeStockCode"),
                ////                 IngredDescription = jd.Field<string>("RecipeDescription"),
                ////                 IngredTrnValueTot = jd.Field<decimal>("RecipeTrnValueTot"),
                ////                 IngredTrnValueUnit = jd.Field<decimal>("RecipeTrnValueUnit"),
                ////                 IngredUom = jd.Field<string>("RecipeUom"),
                ////                 IngredWh = jd.Field<string>("RecipeWh"),
                ////                 IngredJob = jd.Field<string>("RecipeJob"),
                ////                 IngredMatCostWhole = jd.Field<decimal>("RecipeMatCostWhole"),
                ////                 IngredLabCostWhole = jd.Field<decimal>("RecipeLabCostWhole"),
                ////                 IngredTotCostWhole = jd.Field<decimal>("RecipeTotCostWhole"),
                ////                 IngredUsedFactor = jd.Field<decimal>("RecipeUsedFactor"),
                ////                 IngredMatCostUsedTot = jd.Field<decimal>("RecipeMatCostUsedTot"),
                ////                 IngredMatCostUsedUnit = jd.Field<decimal>("RecipeMatCostUsedUnit"),
                ////                 IngredLabCostUsedTot = jd.Field<decimal>("RecipeLabCostUsedTot"),
                ////                 IngredLabCostUsedUnit = jd.Field<decimal>("RecipeLabCostUsedUnit"),
                ////                 RecipeWhatIfMatCost = jd.Field<decimal>("RecipeWhatIfMatCost"),
                ////                 RecipeWhatIfLabCost = jd.Field<decimal>("RecipeWhatIfLabCost"),
                ////                 RecipeUnitConvFact = jd.Field<decimal>("ParentUnitConvFact"),
                ////                 IngredWhatIfMatCost = jd.Field<decimal>("ParentWhatIfMatCost"),
                ////                 IngredWhatIfLabCost = jd.Field<decimal>("ParentWhatIfLabCost"),
                ////                 IngredQtyManufactured = jd.Field<decimal>("RecipeQtyManufactured"),
                ////                 IngredQtyIssued = jd.Field<decimal>("RecipeQtyIssued"),
                ////             });


                if (dtIng.Rows.Count > 0)
                {

                    gvDetails2.DataSource = dtIng;
                    gvDetails2.DataBind();

                    Label lblPanSize = (Label)gvDetails2.Rows[0].FindControl("lblPanSize");

                    lblJobQty.Text = lblQty.Text;
                    lblDetailsForJob2.Text = "Details for Job <font color='Blue'><b>" + int.Parse(lblRecipeJob.Text).ToString() + "</b></font> Qty: <font color='Red'><b>" + lblQty.Text + "</b></font> PanSize: <font color='Red'><b>" + lblPanSize.Text + "</b></font> - (<font color='blue'><b>" + lbnStockCode.Text + "</font></b>)  <font color='Navy'><b>" + lblDescription.Text + "</b></font>";

                    ModalPopupExtenderDetails2.Show();
                    ModalPopupExtenderDetails1.Hide();
                }
                else
                {
                    ModalPopupExtenderDetails1.Show();
                }

                dt.Dispose();
                dtIng.Dispose();
                break;
        }
    }
    protected void gvDetails1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbnStockCode = (LinkButton)e.Row.FindControl("lbnStockCode");
                Label lblDescription = (Label)e.Row.FindControl("lblDescription");
                Label lblRecipeJob = (Label)e.Row.FindControl("lblRecipeJob");
                Label lblUom = (Label)e.Row.FindControl("lblUom");                
                Label lblPckQty = (Label)e.Row.FindControl("lblPckQty");
                Label lblQty = (Label)e.Row.FindControl("lblQty");
                Label lblPkgUom = (Label)e.Row.FindControl("lblPkgUom");


                if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;

                }


                switch (lbnStockCode.Text)
                {
                    case "Packaging Labor":

                        lblUom.Text = "";
                        lblQty.Text = "";
                        
                        lblRecipeJob.Text = "";
                        lbnStockCode.Text = "";
                        lblPckQty.Text = "";
                        lblPkgUom.Text = "";

                        lbnStockCode.Font.Bold = false;

                        lblDescription.Text = "";
                        lblDescription.ForeColor = Color.Black;
                        lblDescription.Font.Bold = false;
                        lblDescription.Font.Size = FontUnit.Point(10);

                        lblQty.Font.Bold = false;
                        
                        break;
                    case "TOTALS":

                        lblUom.Text = "";

                        lbnStockCode.Text = "";
                        lblRecipeJob.Text = "";
                        lblQty.Text = "";
                        lblDescription.Text = "TOTALS:";
                        lblDescription.ForeColor = Color.Black;
                        lblDescription.Font.Bold = true;
                        lblDescription.Font.Size = FontUnit.Point(10);

                        lblPckQty.Font.Bold = true;
                        lblQty.Font.Bold = true;

                        break;
                    case "AVERAGE":
                        lblDescription.Text = "";

                        lblUom.Text = "";
                        lblQty.Text = "";
                        lblRecipeJob.Text = "";
                        lbnStockCode.Text = "";
                        lblPckQty.Text = "";
                        lblPkgUom.Text = "";
                        
                        lbnStockCode.Enabled = false;

                        lblDescription.ForeColor = Color.Black;
                        lblDescription.Font.Bold = true;
                        lblDescription.Font.Italic = true;
                        lblDescription.Font.Size = FontUnit.Point(10);
                        break;
                    case "WHAT-IF":
                        lblDescription.Text = "";

                        lblUom.Text = "";
                        lblQty.Text = "";
                        lblRecipeJob.Text = "";
                        lbnStockCode.Text = "";
                        lblPckQty.Text = "";
                        lblPkgUom.Text = "";
                      
                        lbnStockCode.Enabled = false;

                        lblDescription.ForeColor = Color.Black;
                        lblDescription.Font.Bold = true;
                        lblDescription.Font.Italic = true;
                        lblDescription.Font.Size = FontUnit.Point(10);
                        break;
                    case "VARIANCE":
                        lblDescription.Text = "";
                        lblUom.Text = "";
                        lblQty.Text = "";
                        lblRecipeJob.Text = "";
                        lbnStockCode.Text = "";
                        lblPckQty.Text = "";
                        lblPkgUom.Text = "";
                        
                        lbnStockCode.Enabled = false;

                        lblDescription.ForeColor = Color.Black;
                        lblDescription.Font.Bold = true;
                        lblDescription.Font.Italic = true;
                        lblDescription.Font.Size = FontUnit.Point(10);

                        break;
                    case "SPACE":

                        lblUom.Text = "";
                        lblQty.Text = "";
                       
                        lblRecipeJob.Text = "";
                        lbnStockCode.Text = "";
                        lblPckQty.Text = "";
                        lblPkgUom.Text = "";
                       
                        lbnStockCode.Enabled = false;

                        lblDescription.Text = "";
                        lblDescription.ForeColor = Color.Black;
                        lblDescription.Font.Bold = true;
                        lblDescription.Font.Size = FontUnit.Point(10);
                        break;
                    default:
                        lbnStockCode.Font.Bold = true;
                        lblPckQty.Text = "--";
                        break;
                }
                if (lblPckQty.Text != "" && lblPckQty.Text != "--")
                {
                    lblPckQty.Text = Convert.ToDecimal(lblPckQty.Text).ToString("#,0.0");
                }

                if (lblQty.Text != "")
                {
                    lblQty.Text = Convert.ToDecimal(lblQty.Text).ToString("0.0");
                }

                string sSource = lblRecipeJob.Text;
                var sResult = int.Parse(sSource).ToString();
                lblRecipeJob.Text = sResult;
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvDetails2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcIngredQtyManufactured = 0;
        decimal dcIngredQtyIssued = 0;

        decimal dcQtyMain = 0;
        decimal dcQtyVar = 0;
        decimal dcCheckQty = 0;


        try
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                Label lblParentQtyManuf = (Label)e.Row.FindControl("lblParentQtyManuf");
                Label lblCheckQty = (Label)e.Row.FindControl("lblCheckQty");
               
                Label lblPanSize = (Label)e.Row.FindControl("lblPanSize");
                
                Label lblIngredUsedFactor = (Label)e.Row.FindControl("lblIngredUsedFactor");

                Label lblIngredQtyManufactured = (Label)e.Row.FindControl("lblIngredQtyManufactured");
                Label lblIngredQtyIssued = (Label)e.Row.FindControl("lblIngredQtyIssued");

                Label lblRecipeUnitConvFact = (Label)e.Row.FindControl("lblRecipeUnitConvFact");



                //New 11-17-2014...
                if (lblParentQtyManuf != null)
                {
                    dcQtyMain = Convert.ToDecimal(lblParentQtyManuf.Text);
                    dcQtyMainTotal += dcQtyMain;
                }
                if (lblCheckQty.Text != "")
                {
                    dcCheckQty = Convert.ToDecimal(lblCheckQty.Text);
                    dcQtyVar = (dcQtyMain - dcCheckQty) / (dcQtyMain == 0 ? 1 : dcQtyMain);
                    if (dcQtyVar == 0)
                    {
                        lblCheckQty.Text = "--";
                    }
                    else
                    {
                        lblCheckQty.Text = dcQtyVar.ToString("0.000") + "%";
                    }
                }

                if (lblPanSize.Text != "")
                {
                    lblPanSize.Text = Convert.ToDecimal(lblPanSize.Text).ToString("0.0");
                }

                //////////////////////
                if (lblIngredQtyManufactured.Text != "")
                {
                    dcIngredQtyManufactured = Convert.ToDecimal(lblIngredQtyManufactured.Text);
                    dcIngredQtyManufacturedTotal += dcIngredQtyManufactured;
                    lblIngredQtyManufactured.Text = "$" + Convert.ToDecimal(lblIngredQtyManufactured.Text).ToString("0.00");
                }
                if (lblIngredQtyIssued.Text != "")
                {
                    dcIngredQtyIssued = Convert.ToDecimal(lblIngredQtyIssued.Text);
                    dcIngredQtyIssuedTotal += dcIngredQtyIssued;
                    lblIngredQtyIssued.Text = Convert.ToDecimal(lblIngredQtyIssued.Text).ToString("0.00");
                }


            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                dcQtyMainTotal = dcQtyMainTotal / gvDetails2.Rows.Count;
               

                Label lblIngredQtyManufacturedTotal = (Label)e.Row.FindControl("lblIngredQtyManufacturedTotal");
                Label lblIngredQtyIssuedTotal = (Label)e.Row.FindControl("lblIngredQtyIssuedTotal");

            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderDetails1.Hide();
        pnlSearchCriteria.Visible = true;
        btnPreview.Visible = true;
    }
    protected void btnCancel2_Click(object sender, EventArgs e)
    {
        dcIngredUsedFactorTotal = 0;
        dcIngredQtyManufacturedTotal = 0;
        dcIngredQtyIssuedTotal = 0;
        dcQtyMainTotal = 0;
        ModalPopupExtenderDetails2.Hide();
        ModalPopupExtenderDetails1.Show();

    }
    protected void btnCloseAll_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderDetails2.Hide();
        ModalPopupExtenderDetails1.Hide();
        pnlSearchCriteria.Visible = true;
        btnPreview.Visible = true;
    }
 
    protected void txtStockCode_TextChanged(object sender, EventArgs e)
    {       
        lblDescription.Text = SharedFunctions.GetStockCodeDesc(txtStockCode.Text.Trim());
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