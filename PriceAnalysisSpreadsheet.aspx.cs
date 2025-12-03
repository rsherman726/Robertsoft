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
using System.Data.Linq.SqlClient;
// Add references to Soap and Binary formatters. 
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public partial class PriceAnalysisSpreadsheet : System.Web.UI.Page
{

    #region Subs
    private void LoadStockCodesIngredients(ListBox lb)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lb.Items.Clear();
        var query = (from w in db.InvLookupTableIngredientsRSS
                     group w by new
                     {
                         w.MStockCode,
                         w.MDescription
                     } into g
                     orderby
                       g.Key.MStockCode
                     select new
                     {
                         g.Key.MStockCode,
                         g.Key.MDescription
                     });
        if (query.Count() > 0)
        {
            foreach (var a in query)
            {
                lb.Items.Add(new ListItem(a.MStockCode + " - " + a.MDescription, a.MStockCode));
            }
        }
        else
        {
            lb.Items.Insert(0, new ListItem("No Ingredient Stock Codes found", "0"));
        }
        lblStockCodeList.Text = "Key Ingredient StockCodes: " + lb.Items.Count.ToString();
    }
    private void RunReport()
    {
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

        //Stock Codes...
        string sKeyIngredientStockCode = "NULL";
        if (lbKeyIngredientStockCodes.SelectedIndex != -1)
        {
            sKeyIngredientStockCode = "'" + lbKeyIngredientStockCodes.SelectedValue.Trim() + "'";
        }


        DataTable dt = new DataTable();
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
        else
        {
            sMsg += "**Start Date can't be left blank!<br/>";
        }

        if (sEndDate != "NULL")
        {
            if (SharedFunctions.IsDate(sEndDate.Replace("'", "")) == false)
            {
                sMsg += "**End Date is not a valid date!<br/>";
            }
        }
        else
        {
            sMsg += "**End Date can't be left blank!<br/>";
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

        if (sKeyIngredientStockCode == null)
        {
            sSQL = "EXEC spGetPriceAnalysisComplete2016 ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate;
        }
        else
        {
            sSQL = "EXEC spGetPriceAnalysisComplete2016 ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@KeyIngredientStockCode =" + sKeyIngredientStockCode;// '633180'--HIGH FRUCTOSE 80
        }


        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");


        if (dt.Rows.Count > 0)
        {
            Session["dtSpreadsheet"] = dt;

            DataTable dtNew = new DataTable();
            dt = (DataTable)Session["dtSpreadsheet"];
            dtNew = dt.Copy();
            try
            {
                dtNew.Columns["IncludesKeyIngredient"].ColumnName = "Includes Key Ingredient";//Rename column...
                dtNew.Columns["LastPriceChangeDateA"].ColumnName = "Last Price Change Date A";//Rename column...
                dtNew.Columns["LastPriceChangeDateB"].ColumnName = "Last Price Change Date B";//Rename column...
                dtNew.Columns["LastPriceChangeDateC"].ColumnName = "Last Price Change Date C";//Rename column...
                dtNew.Columns["LastPriceChangeDateD"].ColumnName = "Last Price Change Date D";//Rename column...
                dtNew.Columns["SalesYTD"].ColumnName = DateTime.Now.Year.ToString() + " Sales YTD  to " + DateTime.Now.ToShortDateString();//Rename column...
                dtNew.Columns["MarginDateRange"].ColumnName = "Date Range Gross Margin %";//Rename column...
                dtNew.Columns["MarginCurrentYear"].ColumnName = DateTime.Now.Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginLastYear"].ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginMinus1"].ColumnName = DateTime.Now.AddYears(-2).Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginMinus2"].ColumnName = DateTime.Now.AddYears(-3).Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginAvgPreviousThreeYears"].ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " to " + DateTime.Now.AddYears(-3).Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginDiffCurrentYearVersusPreviousThreeYears"].ColumnName = DateTime.Now.Year.ToString() + " vs Avg Diff %";//Rename column...

            }
            catch (Exception)
            {
                //Ignore...
            }


            gvSpreadsheet.DataSource = dtNew;
            gvSpreadsheet.DataBind();
            lblRecordCount.Text = "RECORDS: " + dt.Rows.Count.ToString();

        }
        else
        {
            lblError.Text = "No results found!!";
            lblError.ForeColor = Color.Red;
            gvSpreadsheet.Visible = false;

            gvSpreadsheet.DataSource = null;
            gvSpreadsheet.DataBind();
        }



    }
    private void RunReportWithCost()
    {
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

        //Stock Codes...
        string sKeyIngredientStockCode = "NULL";
        if (lbKeyIngredientStockCodes.SelectedIndex != -1)
        {
            sKeyIngredientStockCode = "'" + lbKeyIngredientStockCodes.SelectedValue.Trim() + "'";
        }

        string sShrinkagePercentage = "NULL";
        if (txtShrinkagePercentage.Text.Trim() != "")
        {
            sShrinkagePercentage = txtShrinkagePercentage.Text.Trim();
        }

        string sLaborPercentage = "NULL";
        if (txtLaborPercentage.Text.Trim() != "")
        {
            sLaborPercentage = txtLaborPercentage.Text.Trim();
        }

        DataTable dt = new DataTable();
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
        else
        {
            sMsg += "**Start Date can't be left blank!<br/>";
        }

        if (sEndDate != "NULL")
        {
            if (SharedFunctions.IsDate(sEndDate.Replace("'", "")) == false)
            {
                sMsg += "**End Date is not a valid date!<br/>";
            }
        }
        else
        {
            sMsg += "**End Date can't be left blank!<br/>";
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

        if (sKeyIngredientStockCode == null)
        {
            sSQL = "EXEC spGetPriceAnalysisComplete2016WithCost ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@ShrinkagePercentage=" + sShrinkagePercentage + ",";
            sSQL += "@LaborPercentage=" + sLaborPercentage;
        }
        else
        {
            sSQL = "EXEC spGetPriceAnalysisComplete2016WithCost ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@ShrinkagePercentage=" + sShrinkagePercentage + ",";
            sSQL += "@LaborPercentage=" + sLaborPercentage + ",";
            sSQL += "@KeyIngredientStockCode =" + sKeyIngredientStockCode;// '633180'--HIGH FRUCTOSE 80
        }

        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");

        if (dt.Rows.Count > 0)
        {
            Session["dtSpreadsheet"] = dt;

            DataTable dtNew = new DataTable();
            dt = (DataTable)Session["dtSpreadsheet"];
            dtNew = dt.Copy();
            try
            {
                dtNew.Columns["IncludesKeyIngredient"].ColumnName = "Includes Key Ingredient";//Rename column...
                dtNew.Columns["LastPriceChangeDateA"].ColumnName = "Last Price Change Date A";//Rename column...
                dtNew.Columns["LastPriceChangeDateB"].ColumnName = "Last Price Change Date B";//Rename column...
                dtNew.Columns["LastPriceChangeDateC"].ColumnName = "Last Price Change Date C";//Rename column...
                dtNew.Columns["LastPriceChangeDateD"].ColumnName = "Last Price Change Date D";//Rename column...
                dtNew.Columns["SalesYTD"].ColumnName = DateTime.Now.Year.ToString() + " Sales YTD  to " + DateTime.Now.ToShortDateString();//Rename column...
                dtNew.Columns["MarginDateRange"].ColumnName = "Date Range Gross Margin %";//Rename column...
                dtNew.Columns["MarginCurrentYear"].ColumnName = DateTime.Now.Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginLastYear"].ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginMinus1"].ColumnName = DateTime.Now.AddYears(-2).Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginMinus2"].ColumnName = DateTime.Now.AddYears(-3).Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginAvgPreviousThreeYears"].ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " to " + DateTime.Now.AddYears(-3).Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginDiffCurrentYearVersusPreviousThreeYears"].ColumnName = DateTime.Now.Year.ToString() + " vs Avg Diff %";//Rename column...

                dtNew.Columns["LaborCost"].ColumnName = "Labor Cost";//Rename column...
                dtNew.Columns["EstimatedPackagingCost"].ColumnName = "Estimated  Packaging Cost";//Rename column...
                dtNew.Columns["EstimatedRecipeCost"].ColumnName = "Estimated Recipe Cost";//Rename column...

                dtNew.Columns["EstimateCostTotal"].ColumnName = "Estimate Cost Total";//Rename column...
                dtNew.Columns["EstimatedMargin"].ColumnName = "Estimated Margin";//Rename column...
                dtNew.Columns["SalesDateRange"].ColumnName = "Sales Date Range";//Rename column...

            }
            catch (Exception)
            {
                //Ignore...
            }


            gvSpreadsheet.DataSource = dtNew;
            gvSpreadsheet.DataBind();
            lblRecordCount.Text = "RECORDS: " + dt.Rows.Count.ToString();

        }
        else
        {
            lblError.Text = "No results found!!";
            lblError.ForeColor = Color.Red;
            gvSpreadsheet.Visible = false;

            gvSpreadsheet.DataSource = null;
            gvSpreadsheet.DataBind();
        }
    }
    private void RunReportWithCustomer(string sEndCustomer)
    {
        string sMsg = "";
        int iIndexOfPipe = 0;
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
        string sCustomer = "NULL";
        if (lbCustomers.SelectedIndex != -1)//Selected Companies...
        {//Not null then add quotes...
            sCustomer = "";
            foreach (ListItem li in lbCustomers.Items)
            {
                if (li.Selected)
                {
                    sCustomer += li.Value.Trim() + "|";
                }
            }
            if (sCustomer.Trim().EndsWith("|"))
            {
                iIndexOfPipe = sCustomer.Trim().LastIndexOf("|");
                sCustomer = sCustomer.Remove(iIndexOfPipe).Trim();
                sCustomer = "'" + sCustomer + "'";
            }
        }
        //Stock Codes...
        string sKeyIngredientStockCode = "NULL";
        if (lbKeyIngredientStockCodes.SelectedIndex != -1)
        {
            sKeyIngredientStockCode = "'" + lbKeyIngredientStockCodes.SelectedValue.Trim() + "'";
        }

        string sStockCode = "NULL";
        if (ddlEndCustomers.SelectedIndex != 0)
        {
            if (lbParentStockCode.SelectedIndex == -1)
            {
                sMsg += "**You selected Non Customer: a Stock Code(s) Selection is Required!<br/>";
            }
        }

        if (lbParentStockCode.SelectedIndex != -1)
        {
            sStockCode = "";
            foreach (ListItem li in lbParentStockCode.Items)
            {
                if (li.Selected)
                {
                    sStockCode += li.Value.Trim() + "|";
                }
            }
            if (sStockCode.Trim().EndsWith("|"))
            {
                iIndexOfPipe = sStockCode.Trim().LastIndexOf("|");
                sStockCode = sStockCode.Remove(iIndexOfPipe).Trim();
                sStockCode = "'" + sStockCode + "'";
            }
        }
        DataSet ds = new DataSet();
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
        else
        {
            sMsg += "**Start Date can't be left blank!<br/>";
        }
        if (sEndDate != "NULL")
        {
            if (SharedFunctions.IsDate(sEndDate.Replace("'", "")) == false)
            {
                sMsg += "**End Date is not a valid date!<br/>";
            }
        }
        else
        {
            sMsg += "**End Date can't be left blank!<br/>";
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

        if (sEndCustomer == "YES")
        {//Use End Customer...
            if (sKeyIngredientStockCode == null)
            {
                sSQL = "EXEC spGetPriceAnalysisComplete2016WithCustomers ";
                sSQL += "@EndCustomerYN=1,";
                sSQL += "@StockCodesEndCustomer=" + sStockCode + ",";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate;
            }
            else
            {
                sSQL = "EXEC spGetPriceAnalysisComplete2016WithCustomers ";
                sSQL += "@EndCustomerYN=1,";
                sSQL += "@StockCodesEndCustomer=" + sStockCode + ",";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@KeyIngredientStockCode =" + sKeyIngredientStockCode;// '633180'--HIGH FRUCTOSE 80
            }
        }
        else//NO END CUSTOMER...
        {
            if (sKeyIngredientStockCode == null)
            {
                sSQL = "EXEC spGetPriceAnalysisComplete2016WithCustomers ";
                sSQL += "@Customers=" + sCustomer + ",";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate;
            }
            else
            {
                sSQL = "EXEC spGetPriceAnalysisComplete2016WithCustomers ";
                sSQL += "@Customers=" + sCustomer + ",";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@KeyIngredientStockCode =" + sKeyIngredientStockCode;// '633180'--HIGH FRUCTOSE 80
            }
        }




        Debug.WriteLine(sSQL);

        ds = SharedFunctions.getDataSet(sSQL, conn, "dt");


        if (ds.Tables.Count > 0)
        {
            lblRecordCount.Text = "**PRICE ANALYSIS WITH CUSTOMERS IS READY FOR EXPORT!";
            Session["dsSpreadsheet"] = ds;
            gvSpreadsheet.Visible = false;
            gvSpreadsheet.DataSource = null;
            gvSpreadsheet.DataBind();
        }
        else
        {
            lblError.Text = "No results found!!";
            lblError.ForeColor = Color.Red;
            gvSpreadsheet.Visible = false;
            gvSpreadsheet.DataSource = null;
            gvSpreadsheet.DataBind();
        }
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
    private void LoadCustomer(ListBox ddl, string sSortBy)
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
                ddl.Items.Add(new ListItem(a.Name + " - " + a.Customer, a.Customer));
            }
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
    private void LoadStockCodesOfEndCustomer(int iNonCustomerID)
    {
        List<StockCodeDescriptionList.StockCodeDescription> lStockCodeDescListReport = new List<StockCodeDescriptionList.StockCodeDescription>();
        lStockCodeDescListReport = GetNonCustomerStockCodes(iNonCustomerID);
        lbParentStockCode.Items.Clear();
        foreach (StockCodeDescriptionList.StockCodeDescription a in lStockCodeDescListReport)
        {
            lbParentStockCode.Items.Add(new ListItem(a.StockCode + " - " + a.Desc, a.StockCode));
        }
        lblStockCodeList.Text = "End Customer Stock Codes: " + lbParentStockCode.Items.Count.ToString();
    }
    #endregion


    #region Functions
    private List<StockCodeDescriptionList.StockCodeDescription> GetNonCustomerStockCodes(int iNonCustomerID)
    {

        List<StockCodeDescriptionList.StockCodeDescription> lStockCodeDescListReport = new List<StockCodeDescriptionList.StockCodeDescription>();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        StockCodeDescriptionList.StockCodeDescription row = null;
        var query = (from c in db.ArNonCustomerStockCodeAssignments
                     join im in db.InvMaster on c.StockCode equals im.StockCode
                     where c.NonCustomerID == iNonCustomerID
                     select new
                     {
                         c.StockCode,
                         im.Description
                     });
        foreach (var a in query)
        {
            row = new StockCodeDescriptionList.StockCodeDescription();
            row.StockCode = a.StockCode;
            row.Desc = a.Description;
            lStockCodeDescListReport.Add(row);
        }
        return lStockCodeDescListReport;
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

            LoadStockCodesIngredients(lbKeyIngredientStockCodes);
            LoadCustomer(lbCustomers, "Name");
            LoadNonCustomers(ddlEndCustomers);
            //string sStockCode = "";
            string sStartDate = "";
            string sEndDate = "";
            //if (Request.QueryString["sc"] != null)
            //{
            //    sStockCode = Request.QueryString["sc"].ToString();

            //    txtStockCode.Text = sStockCode;
            //    lbStockCode0.SelectedIndex = SharedFunctions.GetSelIndex(sStockCode, lbStockCode0, "Value");
            //    txtStockCodeChartIngredient.Text = sStockCode;

            //}
            if (Request.QueryString["start"] != null)
            {
                sStartDate = Request.QueryString["start"].ToString();
                txtStartDate.Text = sStartDate;

            }
            if (Request.QueryString["end"] != null)
            {
                sEndDate = Request.QueryString["end"].ToString();
                txtEndDate.Text = sEndDate;

            }

        }//End postback
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isPostBack", "isPostBack();", true);
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        RunReport();
    }
    protected void btnPreviewWithCost_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        RunReportWithCost();
    }
    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlPeriod.SelectedIndex)
        {

            case 0://Range
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                break;
            case 1://3 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-3).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 2://6 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-6).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 3://9 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-9).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 4://12 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-12).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
        }
    }
    protected void btnCustomerReport_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (chkEndCustomer.Checked)
        {
            RunReportWithCustomer("YES");
        }
        else
        {
            RunReportWithCustomer("NO");
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        ddlPeriod.SelectedIndex = 0;
        lbKeyIngredientStockCodes.SelectedIndex = -1;
        lbCustomers.SelectedIndex = 0;
        ddlEndCustomers.SelectedIndex = 0;
        lbParentStockCode.Items.Clear();
        lbCustomers.SelectedIndex = -1;
        chkEndCustomer.Checked = false;
        txtShrinkagePercentage.Text = "";
        txtLaborPercentage.Text = "";
    }
    protected void imgExportExcelPriceAnalysis_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable dtNew = new DataTable();
        string sFilesName = "";
        if (Session["dtSpreadsheet"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtSpreadsheet"];
        dtNew = dt.Copy();
        try
        {
            dtNew.Columns["IncludesKeyIngredient"].ColumnName = "Includes Key Ingredient";//Rename column...
            dtNew.Columns["LastPriceChangeDateA"].ColumnName = "Last Price Change Date A";//Rename column...
            dtNew.Columns["LastPriceChangeDateB"].ColumnName = "Last Price Change Date B";//Rename column...
            dtNew.Columns["LastPriceChangeDateC"].ColumnName = "Last Price Change Date C";//Rename column...
            dtNew.Columns["LastPriceChangeDateD"].ColumnName = "Last Price Change Date D";//Rename column...
            dtNew.Columns["SalesYTD"].ColumnName = DateTime.Now.Year.ToString() + " Sales YTD  to " + DateTime.Now.ToShortDateString();//Rename column...
            dtNew.Columns["MarginDateRange"].ColumnName = "Date Range Gross Margin %";//Rename column...
            dtNew.Columns["MarginCurrentYear"].ColumnName = DateTime.Now.Year.ToString() + " Gross Margin %";//Rename column...
            dtNew.Columns["MarginLastYear"].ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " Gross Margin %";//Rename column...
            dtNew.Columns["MarginMinus1"].ColumnName = DateTime.Now.AddYears(-2).Year.ToString() + " Gross Margin %";//Rename column...
            dtNew.Columns["MarginMinus2"].ColumnName = DateTime.Now.AddYears(-3).Year.ToString() + " Gross Margin %";//Rename column...
            dtNew.Columns["MarginAvgPreviousThreeYears"].ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " to " + DateTime.Now.AddYears(-3).Year.ToString() + " Gross Margin %";//Rename column...
            dtNew.Columns["MarginDiffCurrentYearVersusPreviousThreeYears"].ColumnName = DateTime.Now.Year.ToString() + " vs Avg Diff %";//Rename column...

        }
        catch (Exception)
        {
            //Ignore...
        }
        dtNew.TableName = "dtSpreadsheet";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dtNew.Copy());
        }

        sFilesName = "PriceAnalysisSpreadsheet" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void imgExportExcelPriceAnalysisWithCost_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable dtNew = new DataTable();
        string sFilesName = "";
        if (Session["dtSpreadsheet"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtSpreadsheet"];
        dtNew = dt.Copy();
        try
        {
            dtNew.Columns["IncludesKeyIngredient"].ColumnName = "Includes Key Ingredient";//Rename column...
            dtNew.Columns["LastPriceChangeDateA"].ColumnName = "Last Price Change Date A";//Rename column...
            dtNew.Columns["LastPriceChangeDateB"].ColumnName = "Last Price Change Date B";//Rename column...
            dtNew.Columns["LastPriceChangeDateC"].ColumnName = "Last Price Change Date C";//Rename column...
            dtNew.Columns["LastPriceChangeDateD"].ColumnName = "Last Price Change Date D";//Rename column...
            dtNew.Columns["SalesYTD"].ColumnName = DateTime.Now.Year.ToString() + " Sales YTD  to " + DateTime.Now.ToShortDateString();//Rename column...
            dtNew.Columns["MarginDateRange"].ColumnName = "Date Range Gross Margin %";//Rename column...
            dtNew.Columns["MarginCurrentYear"].ColumnName = DateTime.Now.Year.ToString() + " Gross Margin %";//Rename column...
            dtNew.Columns["MarginLastYear"].ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " Gross Margin %";//Rename column...
            dtNew.Columns["MarginMinus1"].ColumnName = DateTime.Now.AddYears(-2).Year.ToString() + " Gross Margin %";//Rename column...
            dtNew.Columns["MarginMinus2"].ColumnName = DateTime.Now.AddYears(-3).Year.ToString() + " Gross Margin %";//Rename column...
            dtNew.Columns["MarginAvgPreviousThreeYears"].ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " to " + DateTime.Now.AddYears(-3).Year.ToString() + " Gross Margin %";//Rename column...
            dtNew.Columns["MarginDiffCurrentYearVersusPreviousThreeYears"].ColumnName = DateTime.Now.Year.ToString() + " vs Avg Diff %";//Rename column...

            dtNew.Columns["LaborCost"].ColumnName = "Labor Cost";//Rename column...
            dtNew.Columns["EstimatedPackagingCost"].ColumnName = "Estimated  Packaging Cost";//Rename column...
            dtNew.Columns["EstimatedRecipeCost"].ColumnName = "Estimated Recipe Cost";//Rename column...

            dtNew.Columns["EstimateCostTotal"].ColumnName = "Estimate Cost Total";//Rename column...
            dtNew.Columns["EstimatedMargin"].ColumnName = "Estimated Margin";//Rename column...
            dtNew.Columns["SalesDateRange"].ColumnName = "Sales Date Range";//Rename column...
        }
        catch (Exception)
        {
            //Ignore...
        }
        dtNew.TableName = "dtSpreadsheet";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dtNew.Copy());
        }

        sFilesName = "PriceAnalysisSpreadsheetWithCost" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void imgExportExcelPriceAnalysisWithCustomer_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataSet dsNew = new DataSet();
        DataTable dtNew = new DataTable();
        string sFilesName = "";
        if (Session["dsSpreadsheet"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        ds = (DataSet)Session["dsSpreadsheet"];
        //Handles Huge datasets...
        ds.RemotingFormat = SerializationFormat.Binary;
        // Save using the Runtime Object Serialization
        try
        {
            FileStream fs1 = new FileStream(@"c:\inetpub\wwwroot\Felbro_B_V7\images\data_ser.dat", FileMode.Create, FileAccess.ReadWrite);
            BinaryFormatter bin = new BinaryFormatter();
            bin.Serialize(fs1, ds);
            fs1.Close();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            lblError.Text = "**FILE ACCESS ERROR!";
            lblError.ForeColor = Color.Red;
            return;
        }



        string sCustomerName = "";
        string sEndCustomerName = "";
        foreach (DataTable dt in ds.Tables)
        { //Rename the tables in the DataSet with the Customer so they appear on the Excel individual tabs...

            sCustomerName = dt.Rows[0]["Name"].ToString().Replace(".", "").Replace("'", "").Replace("/", "-").Replace("  ", " ");
            if (sCustomerName.Length > 29)
            {
                sCustomerName = sCustomerName.Substring(0, 30);
            }

            if (chkEndCustomer.Checked)
            {//Use End Customer for tabs...
                sEndCustomerName = dt.Rows[0]["EndCustomer"].ToString().Replace(".", "").Replace("'", "").Replace("/", "-").Replace("  ", " ");
                if (sCustomerName.Length > 29)
                {
                    sEndCustomerName = sEndCustomerName.Substring(0, 30);
                }

                dt.TableName = sEndCustomerName;
            }
            else//Customer Customer Name for Tabs...
            {
                dt.TableName = sCustomerName;
            }


            Debug.WriteLine(sCustomerName);

            dtNew = dt.Copy();

            ////foreach(DataColumn col in dtNew.Columns)
            ////{
            ////    Debug.WriteLine(col.ColumnName);
            ////}
            try
            {
                dtNew.Columns["IncludesKeyIngredient"].ColumnName = "Includes Key Ingredient";//Rename column...

                dtNew.Columns["SalesYTD"].ColumnName = DateTime.Now.Year.ToString() + " Sales YTD  to " + DateTime.Now.ToShortDateString();//Rename column...
                dtNew.Columns["SalesRange"].ColumnName = "Date Range Sales "  + txtStartDate.Text.Trim() + " to " + txtEndDate.Text.Trim();//Rename column...
                dtNew.Columns["MarginDateRange"].ColumnName = "Date Range Gross Margin %";//Rename column...
                dtNew.Columns["MarginCurrentYear"].ColumnName = DateTime.Now.Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginLastYear"].ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginMinus1"].ColumnName = DateTime.Now.AddYears(-2).Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginMinus2"].ColumnName = DateTime.Now.AddYears(-3).Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginAvgPreviousThreeYears"].ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " to " + DateTime.Now.AddYears(-3).Year.ToString() + " Gross Margin %";//Rename column...
                dtNew.Columns["MarginDiffCurrentYearVersusPreviousThreeYears"].ColumnName = DateTime.Now.Year.ToString() + " vs Avg Diff %";//Rename column...

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                //Ignore...
            }

            dsNew.Tables.Add(dtNew.Copy());

        }



        sFilesName = "PriceAnalysisSpreadsheetWithCustomers" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(dsNew, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
        dsNew.Dispose();
    }
    protected void ddlEndCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblRecordCount.Text = "";
        lbParentStockCode.Items.Clear();
        lbCustomers.SelectedIndex = 0;
        LoadStockCodesOfEndCustomer(Convert.ToInt32(ddlEndCustomers.SelectedValue));
        foreach (ListItem li in lbParentStockCode.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblRecordCount.Text = "";
        ddlEndCustomers.SelectedIndex = 0;
    }
    protected void rblSort_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer(lbCustomers, rblSort.SelectedValue);
    }
    protected void chkEndCustomer_CheckedChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblRecordCount.Text = "";
    }
    protected void lbClearAll_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbKeyIngredientStockCodes.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    protected void lbSelectAllCustomer_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbCustomers.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbClearAllCustomer_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbCustomers.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    #endregion





}