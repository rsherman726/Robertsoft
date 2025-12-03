using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Data.Linq;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Drawing;

public partial class PriceAnalysis : System.Web.UI.Page
{
    decimal dcSalesByDateRangeTotal = 0;
    decimal dcSalesLast12MonthsTotal = 0;

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

    private void LoadStockCodesWipMaster()
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lbParentStockCode.Items.Clear();


        var sevenYearsAgo = DateTime.Now.AddYears(-7).Year;

        var query = (from w in db.WipMaster
                     where w.ActCompleteDate.HasValue &&
                           w.ActCompleteDate.Value.Year >= sevenYearsAgo
                     group w by new { w.StockCode, w.StockDescription } into g
                     orderby g.Key.StockCode
                     select new
                     {
                         StockCode = g.Key.StockCode,
                         StockDescription = g.Key.StockDescription
                     }).ToList();


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
    private void LoadParentStockCodesIngredients(ListBox lb, string sIngredientStockCode)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
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
            int iCount = query.Count();
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
            lblStockCodeList.Text = "StockCodes: " + iCount.ToString();
        }
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
        lblStockCodeList.Text = "StockCodes: " + lb.Items.Count.ToString();
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
    private void GetReport()
    {
        DataTable dtReport = new DataTable();
        int iLastPipeInStringIdx = 0;

        string sStockCodeList = "";
        string sStartDate = "";
        string sEndDate = "";
        sStartDate = txtStartDate.Text.Trim();
        sEndDate = txtEndDate.Text.Trim();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        lblError.Text = "";
        string sMsg = "";

        //If date are blank they must be nulled out!!!
        if (sStartDate != "")
        {
            if (SharedFunctions.IsDate(sStartDate.Replace("'", "")) == false)
            {
                sMsg += "**Start Date is not a valid date!<br/>";
            }
        }
        else
        {
            sMsg += "**Start Date is cannot be blank!<br/>";
        }

        if (sEndDate != "")
        {
            if (SharedFunctions.IsDate(sEndDate.Replace("'", "")) == false)
            {
                sMsg += "**End Date is not a valid date!<br/>";
            }
        }
        else
        {
            sMsg += "**End Date is cannot be blank!<br/>";
        }

        if (sStartDate != "" && sEndDate != "")
        {
            if (SharedFunctions.IsDate(sStartDate.Replace("'", "")) == true && SharedFunctions.IsDate(sEndDate.Replace("'", "")) == true)
            {
                if (Convert.ToDateTime(sStartDate.Replace("'", "")) > Convert.ToDateTime(sEndDate.Replace("'", "")))
                {
                    sMsg += "**End Date can not come before Start Date!<br/>";
                }
            }
        }
        if (lbParentStockCode.SelectedIndex == -1)
        {
            sMsg += "**No Stock Codes Selected!<br/>";
        }
        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            lblError.ForeColor = Color.Red;
            return;
        }


        //Stock Codes...
        foreach (ListItem li in lbParentStockCode.Items)
        {
            if (li.Selected)
            {
                sStockCodeList += li.Value.Trim() + "|";
            }
        }
        if (sStockCodeList.Trim().EndsWith("|"))
        {
            iLastPipeInStringIdx = sStockCodeList.LastIndexOf("|");
            sStockCodeList = sStockCodeList.Remove(iLastPipeInStringIdx).Trim();
        }

        if (rblCostPerUnitDataSource.SelectedIndex == 0)
        {//Cost Report...

            sSQL = "EXEC spGetPriceAnalysis @StockCodes ='" + sStockCodeList + "',@FromDate='" + sStartDate + "' ,@ToDate='" + sEndDate + "' ";
        }
        else//Unit Cost Report...
        {
            sSQL = "EXEC spGetPriceAnalysisFromUnitCostReport @StockCodes ='" + sStockCodeList + "',@FromDate='" + sStartDate + "' ,@ToDate='" + sEndDate + "' ";

        }

        Debug.WriteLine(sSQL);
        dtReport = SharedFunctions.getDataTable(sSQL, conn, "dtReport");
        Session["dtReport"] = dtReport;
        if (dtReport.Rows.Count > 0)
        {
            pnlGridView.Visible = true;
            HeaderTable.Visible = true;
            gvReportCondensed.DataSource = dtReport;
            gvReportCondensed.DataBind();
            lblRecordCount.Text = "Stock Codes: " + dtReport.Rows.Count.ToString();
            lblRecordCount.ForeColor = Color.Blue;

            switch (dtReport.Rows.Count)
            {
                case 1:
                    pnlGridView.Height = Unit.Pixel(50);
                    break;
                case 2:
                    pnlGridView.Height = Unit.Pixel(85);
                    break;
                case 3:
                    pnlGridView.Height = Unit.Pixel(105);
                    break;
                case 4:
                    pnlGridView.Height = Unit.Pixel(125);
                    break;
                case 5:
                    pnlGridView.Height = Unit.Pixel(145);
                    break;
                case 6:
                    pnlGridView.Height = Unit.Pixel(165);
                    break;
                case 7:
                    pnlGridView.Height = Unit.Pixel(185);
                    break;
                case 8:
                    pnlGridView.Height = Unit.Pixel(210);
                    break;
                case 9:
                    pnlGridView.Height = Unit.Pixel(235);
                    break;
                case 10:
                    pnlGridView.Height = Unit.Pixel(260);
                    break;
                case 11:
                    pnlGridView.Height = Unit.Pixel(285);
                    break;
                case 12:
                    pnlGridView.Height = Unit.Pixel(310);
                    break;
                case 13:
                    pnlGridView.Height = Unit.Pixel(335);
                    break;
                case 14:
                    pnlGridView.Height = Unit.Pixel(360);
                    break;
                case 15:
                    pnlGridView.Height = Unit.Pixel(385);
                    break;
                case 16:
                    pnlGridView.Height = Unit.Pixel(410);
                    break;
                case 17:
                    pnlGridView.Height = Unit.Pixel(435);
                    break;
                case 18:
                    pnlGridView.Height = Unit.Pixel(460);
                    break;
                case 19:
                    pnlGridView.Height = Unit.Pixel(485);
                    break;
                case 20:
                    pnlGridView.Height = Unit.Pixel(510);
                    break;
                default:
                    pnlGridView.Height = Unit.Pixel(510);
                    break;

            }

        }
        else
        {
            pnlGridView.Visible = false;
            HeaderTable.Visible = false;
            gvReportCondensed.DataSource = null;
            gvReportCondensed.DataBind();
            lblRecordCount.Text = "No data found!";
            lblRecordCount.ForeColor = Color.Red;
        }
        dtReport.Dispose();
    }
    private void GetReportStockCodeRange(string sStockCodeFrom, string sStockCodeTo)
    {
        DataTable dtReport = new DataTable();

        string sStartDate = "";
        string sEndDate = "";
        sStartDate = txtStartDate.Text.Trim();
        sEndDate = txtEndDate.Text.Trim();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        lblError.Text = "";
        string sMsg = "";

        //If date are blank they must be nulled out!!!
        if (sStartDate != "")
        {
            if (SharedFunctions.IsDate(sStartDate.Replace("'", "")) == false)
            {
                sMsg += "**Start Date is not a valid date!<br/>";
            }
        }
        else
        {
            sMsg += "**Start Date is cannot be blank!<br/>";
        }

        if (sEndDate != "")
        {
            if (SharedFunctions.IsDate(sEndDate.Replace("'", "")) == false)
            {
                sMsg += "**End Date is not a valid date!<br/>";
            }
        }
        else
        {
            sMsg += "**End Date is cannot be blank!<br/>";
        }

        if (sStartDate != "" && sEndDate != "")
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
            lblError.ForeColor = Color.Red;
            return;
        }
        if (rblCostPerUnitDataSource.SelectedIndex == 0)
        {//Cost Report...
            sSQL = "EXEC spGetPriceAnalysisStockCodeRange @StockCodeFrom =" + sStockCodeFrom + ", @StockCodeTo =" + sStockCodeTo + ",@FromDate='" + sStartDate + "' ,@ToDate='" + sEndDate + "' ";
        }
        else//Unit Cost Report...
        {
            sSQL = "EXEC spGetPriceAnalysisStockCodeRangeFromUnitCostReport @StockCodeFrom =" + sStockCodeFrom + ", @StockCodeTo =" + sStockCodeTo + ",@FromDate='" + sStartDate + "' ,@ToDate='" + sEndDate + "' ";
        }


        Debug.WriteLine(sSQL);
        dtReport = SharedFunctions.getDataTable(sSQL, conn, "dtReport");
        Session["dtReport"] = dtReport;
        if (dtReport.Rows.Count > 0)
        {
            pnlGridView.Visible = true;
            HeaderTable.Visible = true;
            gvReportCondensed.DataSource = dtReport;
            gvReportCondensed.DataBind();
            lblRecordCount.Text = "Stock Codes: " + dtReport.Rows.Count.ToString();
            lblRecordCount.ForeColor = Color.Blue;

            switch (dtReport.Rows.Count)
            {
                case 1:
                    pnlGridView.Height = Unit.Pixel(50);
                    break;
                case 2:
                    pnlGridView.Height = Unit.Pixel(85);
                    break;
                case 3:
                    pnlGridView.Height = Unit.Pixel(105);
                    break;
                case 4:
                    pnlGridView.Height = Unit.Pixel(125);
                    break;
                case 5:
                    pnlGridView.Height = Unit.Pixel(145);
                    break;
                case 6:
                    pnlGridView.Height = Unit.Pixel(165);
                    break;
                case 7:
                    pnlGridView.Height = Unit.Pixel(185);
                    break;
                case 8:
                    pnlGridView.Height = Unit.Pixel(210);
                    break;
                case 9:
                    pnlGridView.Height = Unit.Pixel(235);
                    break;
                case 10:
                    pnlGridView.Height = Unit.Pixel(260);
                    break;
                case 11:
                    pnlGridView.Height = Unit.Pixel(285);
                    break;
                case 12:
                    pnlGridView.Height = Unit.Pixel(310);
                    break;
                case 13:
                    pnlGridView.Height = Unit.Pixel(335);
                    break;
                case 14:
                    pnlGridView.Height = Unit.Pixel(360);
                    break;
                case 15:
                    pnlGridView.Height = Unit.Pixel(385);
                    break;
                case 16:
                    pnlGridView.Height = Unit.Pixel(410);
                    break;
                case 17:
                    pnlGridView.Height = Unit.Pixel(435);
                    break;
                case 18:
                    pnlGridView.Height = Unit.Pixel(460);
                    break;
                case 19:
                    pnlGridView.Height = Unit.Pixel(485);
                    break;
                case 20:
                    pnlGridView.Height = Unit.Pixel(510);
                    break;
                default:
                    pnlGridView.Height = Unit.Pixel(510);
                    break;

            }

        }
        else
        {
            pnlGridView.Visible = false;
            HeaderTable.Visible = false;
            gvReportCondensed.DataSource = null;
            gvReportCondensed.DataBind();
            lblRecordCount.Text = "No data found!";
            lblRecordCount.ForeColor = Color.Red;
        }
        dtReport.Dispose();
    }
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
            LoadStockCodesWipMaster();
        }
        else
        {
            //Postback...
            //Use to Maintain jQuery during postbacks...
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isPostBack", "$(document).ready(isPostBack);", true);
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        string sMsg = "";
        if (rblSingleOrStockCodeRange.SelectedIndex == 0)
        {//List...
            GetReport();
        }
        else//Range...
        {

            string sStockCodeFrom = "";
            string sStockCodeTo = "";
            if (txtStockCodeFrom.Text.Trim() != "")
            {
                sStockCodeFrom = "'" + txtStockCodeFrom.Text.Trim() + "'";
            }
            else
            {
                sMsg += "**You selected RANGE: a From Stock Code is Required!<br/>";
            }
            if (txtStockCodeTo.Text.Trim() != "")
            {
                sStockCodeTo = "'" + txtStockCodeTo.Text.Trim() + "'";
            }
            else
            {
                sMsg += "**You selected RANGE: a To Stock Code is Required!<br/>";
            }

            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                return;
            }


            //Run Range Report...
            GetReportStockCodeRange(sStockCodeFrom, sStockCodeTo);
        }


    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (txtStockCodeChartIngredientComponent.Text.Trim() != "")
        {
            lblStockCodeDescIngredientComponent.Text = GetDescriptionFromIngredientStockCode(txtStockCodeChartIngredientComponent.Text.Trim());
            LoadParentStockCodesIngredients(lbParentStockCode, txtStockCodeChartIngredientComponent.Text.Trim());
        }
        else
        {
            lbParentStockCode.Items.Clear();
        }
    }
    protected void txtStockCodeChartIngredient_TextChanged(object sender, EventArgs e)
    {
        if (txtStockCodeChartIngredientComponent.Text.Trim() != "")
        {
            lblStockCodeDescIngredientComponent.Text = GetDescriptionFromIngredientStockCode(txtStockCodeChartIngredientComponent.Text.Trim());
            LoadParentStockCodesIngredients(lbParentStockCode, txtStockCodeChartIngredientComponent.Text.Trim());
        }
        else
        {
            lbParentStockCode.Items.Clear();
        }

    }
    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcSalesByDateRange = 0;
        decimal dcSalesLast12Months = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblSalesByDateRange = (Label)e.Row.FindControl("lblSalesByDateRange");
            Label lblSalesLast12Months = (Label)e.Row.FindControl("lblSalesLast12Months");
            if (lblSalesByDateRange.Text != "")
            {
                dcSalesByDateRange = Convert.ToDecimal(lblSalesByDateRange.Text);
                dcSalesByDateRangeTotal += dcSalesByDateRange;
            }
            if (lblSalesLast12Months.Text != "")
            {
                dcSalesLast12Months = Convert.ToDecimal(lblSalesLast12Months.Text);
                dcSalesLast12MonthsTotal += dcSalesLast12Months;
            }

            LinkButton lbnStockCode = (LinkButton)e.Row.FindControl("lbnStockCode");

            string sStockCode = lbnStockCode.Text.Trim();
            string sDateFrom = txtStartDate.Text.Trim();
            string sDateTo = txtEndDate.Text.Trim();
            string sURL = "";
            sURL = "CustomerReportPopup.aspx?sc=" + sStockCode;

            if (sDateFrom != "")
            {
                sURL += "&start=" + sDateFrom;
            }
            if (sDateTo != "")
            {
                sURL += "&end=" + sDateTo;
            }

            lbnStockCode.ToolTip = "Click to launch the Customer Report Page";
            lbnStockCode.Attributes.Add("onclick", "javascript: window.open('" + sURL + "', 'window','HEIGHT=800,WIDTH=1200,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
            lbnStockCode.Style.Add("Cursor", "pointer");

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblSalesByDateRangeTotal = (Label)e.Row.FindControl("lblSalesByDateRangeTotal");
            Label lblSalesLast12MonthsTotal = (Label)e.Row.FindControl("lblSalesLast12MonthsTotal");
            lblSalesByDateRangeTotal.Text = dcSalesByDateRangeTotal.ToString("#,0.00");
            lblSalesLast12MonthsTotal.Text = dcSalesLast12MonthsTotal.ToString("#,0.00");
        }

    }
    protected void gvReport_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();

        DataTable dt = (DataTable)Session["dtReport"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvReportCondensed.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvReportCondensed.DataSource = m_DataView;
            gvReportCondensed.DataBind();
            gvReportCondensed.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
    }

    protected void imgExportExcel_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";
        lblError.Text = "";
        string sMsg = "";
        //if (rblSingleOrStockCodeRange.SelectedIndex == 0)
        //{//List...
        //    GetReport();
        //}
        //else//Range...
        //{

        //    string sStockCodeFrom = "";
        //    string sStockCodeTo = "";
        //    if (txtStockCodeFrom.Text.Trim() != "")
        //    {
        //        sStockCodeFrom = "'" + txtStockCodeFrom.Text.Trim() + "'";
        //    }
        //    else
        //    {
        //        sMsg += "**You selected RANGE: a From Stock Code is Required!<br/>";
        //    }
        //    if (txtStockCodeTo.Text.Trim() != "")
        //    {
        //        sStockCodeTo = "'" + txtStockCodeTo.Text.Trim() + "'";
        //    }
        //    else
        //    {
        //        sMsg += "**You selected RANGE: a To Stock Code is Required!<br/>";
        //    }

        //    if (sMsg.Length > 0)
        //    {
        //        lblError.Text = sMsg;
        //        return;
        //    }


        //    //Run Range Report...
        //    GetReportStockCodeRange(sStockCodeFrom, sStockCodeTo);
        //}
        if (Session["dtReport"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtReport"];
        ////Filter only displayed rows...
        //var query = (from r in dt.AsEnumerable()
        //             select new
        //             {
        //                 StockCode = r.Field<string>("StockCode"),
        //                 Description = r.Field<string>("Description"),
        //                 UOM = r.Field<string>("UOM"),
        //                 QtySold = r.Field<decimal>("QtySold"),
        //                 SalesByDateRange =  Convert.ToDecimal(r.Field<string>("SalesDateRange")),
        //                 SalesLast12Months = Convert.ToDecimal(r.Field<string>("SalesLast12Months")),
        //                 AvgPrice = r.Field<decimal>("AvgPrice"),
        //                 Margin = r.Field<decimal>("Margin"),
        //                 RecipeCostPerUnit = r.Field<string>("RecipeCostPerUnit"),
        //                 PkgCostPerUnit = r.Field<string>("PkgCostPerUnit"),
        //                 LaborCostPerUnit = r.Field<string>("LaborCostPerUnit"),
        //                 TotalCostPerUnit = r.Field<string>("TotalCostPerUnit"),
        //                 WiRecipeCostPerUnit = r.Field<string>("WiRecipeCostPerUnit"),
        //                 WiPkgCostPerUnit = r.Field<string>("WiPkgCostPerUnit"),
        //                 WiLaborCostPerUnit = r.Field<string>("WiLaborCostPerUnit"),
        //                 WiTotalCostPerUnit = r.Field<string>("WiTotalCostPerUnit"),
        //                 LastPriceIncreaseDate = r.Field<string>("LastPriceIncreaseDate"),
        //             });

        //dt = SharedFunctions.LINQToDataTable(query);

        dt.TableName = "dtReport";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "PriceAnalysisReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void lbnSelectAllStockCode_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbParentStockCode.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearStockCode_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbParentStockCode.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        DateTime dtFirstDayOfLastMonth = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
        DateTime dtLastDayOfLastMonth = Convert.ToDateTime(dtFirstDayOfLastMonth.AddMonths(1).AddDays(-1).ToShortDateString() + " 23:59:59");

        switch (ddlPeriod.SelectedIndex)
        {
            case 0:
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                break;
            case 1://Range...
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                break;
            case 2://Last Month...
                txtStartDate.Text = dtFirstDayOfLastMonth.ToShortDateString();
                txtEndDate.Text = dtLastDayOfLastMonth.ToShortDateString();
                break;
            case 3://3 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-3).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 4://6 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-6).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 5://9 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-9).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 6://12 Mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-12).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 7://Previous Year              
                txtStartDate.Text = "01/01/" + DateTime.Now.AddYears(-1).Year.ToString();
                txtEndDate.Text = "12/31/" + DateTime.Now.AddYears(-1).Year.ToString();
                break;
            case 8://Current Year                
                txtStartDate.Text = "01/01/" + DateTime.Now.Year.ToString();
                txtEndDate.Text = "12/31/" + DateTime.Now.Year.ToString();
                break;
            case 9://Last 5 years
                txtStartDate.Text = DateTime.Now.AddYears(-5).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
        }
    }
    protected void rblSourceOfStockCodes_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblSourceOfStockCodes.SelectedIndex == 0)
        {//WipMaster...
            LoadStockCodesWipMaster();
        }
        else
        {//InvMaster
            LoadStockCodesInvMaster(lbParentStockCode);
        }
        txtStockCodeChartIngredientComponent.Text = "";
    }
    protected void btnLoadAll_Click(object sender, EventArgs e)
    {
        LoadParentStockCodesIngredients(lbParentStockCode);
    }
    #endregion

    //FrozenHeaderRow...
    // LinkButtons are used to dynamically create the links necessary
    // for paging.
    protected void HeaderLink_Click(object sender, System.EventArgs e)
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
        TableRow headerRow = new TableRow();

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
                case 0://StockCode
                    headerCell.Width = Unit.Pixel(65);
                    break;
                case 1://Description
                    headerCell.Width = Unit.Pixel(233);
                    break;
                case 2://UOM
                    headerCell.Width = Unit.Pixel(38);
                    break;
                case 3://QtySold
                    headerCell.Width = Unit.Pixel(33);
                    break;
                case 4://Sales Date Range
                    headerCell.Width = Unit.Pixel(90);
                    break;
                case 5://Sales Last 12 months
                    headerCell.Width = Unit.Pixel(88);
                    break;
                case 6://Avg Price
                    headerCell.Width = Unit.Pixel(52);
                    break;
                case 7://Margin
                    headerCell.Width = Unit.Pixel(38);
                    break;
                case 8://Recipe CPU
                    headerCell.Width = Unit.Pixel(51);
                    headerCell.BackColor = Color.Firebrick;
                    break;
                case 9://Pkg CPU
                    headerCell.Width = Unit.Pixel(51);
                    headerCell.BackColor = Color.Firebrick;
                    break;
                case 10://Labor CPU
                    headerCell.Width = Unit.Pixel(51);
                    headerCell.BackColor = Color.Firebrick;
                    break;
                case 11://Total CPU
                    headerCell.Width = Unit.Pixel(51);
                    headerCell.BackColor = Color.Firebrick;
                    break;
                case 12://Recipe CPU WI
                    headerCell.Width = Unit.Pixel(50);
                    headerCell.BackColor = Color.Navy;
                    break;
                case 13://Pkg CPU WI
                    headerCell.Width = Unit.Pixel(50);
                    headerCell.BackColor = Color.Navy;
                    break;
                case 14://Labor CPU WI
                    headerCell.Width = Unit.Pixel(50);
                    headerCell.BackColor = Color.Navy;
                    break;
                case 15://Total CPU WIL
                    headerCell.Width = Unit.Pixel(50);
                    headerCell.BackColor = Color.Navy;
                    break;
                case 16://LastPriceIncreaseDateA
                    headerCell.Width = Unit.Pixel(60);
                    headerCell.BackColor = Color.Teal;
                    break;
                case 17://LastPriceIncreaseDateB
                    headerCell.Width = Unit.Pixel(60);
                    headerCell.BackColor = Color.Teal;
                    break;
                case 18://LastPriceIncreaseDateC
                    headerCell.Width = Unit.Pixel(60);
                    headerCell.BackColor = Color.Teal;
                    break;
                case 19://LastPriceIncreaseDateD
                    headerCell.Width = Unit.Pixel(60);
                    headerCell.BackColor = Color.Teal;
                    break;

            }


            //headerCell.Width = col.ItemStyle.Width;
            headerRow.Cells.Add(headerCell);

        }

        HeaderTable.Rows.Add(headerRow);
    }



}