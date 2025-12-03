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

public partial class CustomerPurchasingBehavior : System.Web.UI.Page
{


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
    private void GetTrendsReport()
    {
        lblError.Text = "";
        string sMsg = "";
        string sStartDate = txtStartDate.Text.Trim();
        string sEndDate = txtEndDate.Text.Trim();
        //Validation...
        if (sStartDate != "")
        {
            if (SharedFunctions.IsDate(sStartDate.Replace("'", "")) == false)
            {
                sMsg += "**Start Date is not a valid date!<br/>";
            }
        }

        if (sEndDate != "")
        {
            if (SharedFunctions.IsDate(sEndDate.Replace("'", "")) == false)
            {
                sMsg += "**End Date is not a valid date!<br/>";
            }
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

        if (sStartDate == "" && sEndDate != "")
        {
            sMsg += "**If there is an End Date there must be a Start Date!<br/>";
        }
        if (sStartDate != "" && sEndDate == "")
        {
            sMsg += "**If there is an Start Date there must be a End Date!<br/>";
        }

        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }


        string sStockCode = txtStockCode.Text.Trim();
        string sCustomer = "";
        if (ddlCustomers.SelectedIndex == 0)
        {
            sCustomer = "";
        }
        else
        {
            sCustomer = ddlCustomers.SelectedValue;
        }
        string sURL = "";
        sURL = "TrendsReportPopup.aspx?sc=" + sStockCode;
        switch (ddlPeriod.SelectedValue)
        {
            case "All":
                sStartDate = DateTime.Now.AddYears(-5).ToShortDateString();//Five Years back...
                sEndDate = DateTime.Now.ToShortDateString();//today...
                break;
            case "Range":
                if (txtStartDate.Text.Trim() == "" && txtEndDate.Text.Trim() == "")
                {//Should ever get here...
                    sStartDate = DateTime.Now.AddDays(-7).ToShortDateString();//one day back...
                    sEndDate = DateTime.Now.ToShortDateString();//today...
                }
                else
                {
                    sStartDate = txtStartDate.Text.Trim();
                    sEndDate = txtEndDate.Text.Trim();
                }
                break;
            default://All Others...
                sStartDate = txtStartDate.Text.Trim();
                sEndDate = txtEndDate.Text.Trim();
                break;
        }
        if (sStartDate != "")
        {
            sURL += "&start=" + sStartDate;
        }
        if (sEndDate != "")
        {
            sURL += "&end=" + sEndDate;
        }

        sURL += "&cus=" + sCustomer;

        lbnTrendsReport.ToolTip = "Click to launch the Purchasing Trends Report Page";
        lbnTrendsReport.Attributes.Add("onclick", "javascript: window.open('" + sURL + "', 'window','HEIGHT=800,WIDTH=1500,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
        lbnTrendsReport.Style.Add("Cursor", "pointer");
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

        ExcelHelper.ToExcel(ds, sFileName , Page.Response);

    }
    private void RunAlertReport(string sStockCode, string sCustomer)
    {
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        if (sStockCode != "" & sCustomer != "")//Both...
        {
            sSQL = "EXEC GetZeroMonthsReport ";           
            sSQL += "@StockCode='" + sStockCode + "',";
            sSQL += "@Customer='" + sCustomer + "'";
        }
        else if (sStockCode == "" & sCustomer == "")//Neither...
        {
            sSQL = "EXEC GetZeroMonthsReport ";           
            sSQL += "@StockCode='',";
            sSQL += "@Customer=''";
        }
        else if (sStockCode != "" & sCustomer == "")//StockCode Only...
        {
            sSQL = "EXEC GetZeroMonthsReport ";           
            sSQL += "@StockCode='" + sStockCode + "',";
            sSQL += "@Customer=''";
        }
        else if (sStockCode == "" & sCustomer != "")//Customer Only...
        {
            sSQL = "EXEC GetZeroMonthsReport ";          
            sSQL += "@StockCode='',";
            sSQL += "@Customer='" + sCustomer + "'";
        }
        else
        {
            //Should Never be here...
        }
        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");


        if (dt.Rows.Count > 0)
        {
            Session["dtReport"] = dt;
            gvPurchasingTrends.DataSource = dt;
            gvPurchasingTrends.DataBind();
        }
        else
        {
            gvPurchasingTrends.DataSource = null;
            gvPurchasingTrends.DataBind();
            lblError.Text = "No results found!!";
        }
        //Set up header column text...
        int iColumnCount = 0;
        iColumnCount = gvPurchasingTrends.HeaderRow.Cells.Count;
        for (int iColumnIndex = 0; iColumnIndex < iColumnCount; iColumnIndex++)
        {
            LinkButton SortButton = (LinkButton)gvPurchasingTrends.HeaderRow.Cells[iColumnIndex].Controls[0];

            switch (SortButton.Text)
            {
                case "LastSaleDate":
                    SortButton.Text = "Last Sale Date";
                    break;
                case "MonthsSinceLastSale":
                    SortButton.Text = "Months Since Last Sale";
                    break;
                case "ZeroAvg":
                    SortButton.Text = "Avg. Months Between Sales";
                    break;
                case "Flag"://Hide Column...
                    gvPurchasingTrends.HeaderRow.Cells[iColumnIndex].Visible = false;
                    for (int i = 0;  i < gvPurchasingTrends.Rows.Count; i++)
                    {
                      gvPurchasingTrends.Rows[i].Cells[iColumnIndex].Visible = false;
                    }
                    
                    break;
            }
             
        }

    }
    private void RunSalesTrendsReport(string sStockCode, string sCustomer, string sThreshold, string sFlag)
    {
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        if (sStockCode != "" & sCustomer != "")//Both...
        {
            sSQL = "EXEC GetRolling24MonthComparision ";
            sSQL += "@Threshold='" + sThreshold + "',";
            sSQL += "@Flag='" + sFlag + "',";
            sSQL += "@StockCode='" + sStockCode + "',";            
            sSQL += "@Customer='" + sCustomer + "'";
        }
        else if (sStockCode == "" & sCustomer == "")//Neither...
        {
            sSQL = "EXEC GetRolling24MonthComparision ";
            sSQL += "@Threshold='" + sThreshold + "',";
            sSQL += "@Flag='" + sFlag + "',";
            sSQL += "@StockCode='',";
            sSQL += "@Customer=''";
        }
        else if (sStockCode != "" & sCustomer == "")//StockCode Only...
        {
            sSQL = "EXEC GetRolling24MonthComparision ";
            sSQL += "@Threshold='" + sThreshold + "',";
            sSQL += "@Flag='" + sFlag + "',";
            sSQL += "@StockCode='" + sStockCode + "',";
            sSQL += "@Customer=''";
        }
        else if (sStockCode == "" & sCustomer != "")//Customer Only...
        {
            sSQL = "EXEC GetRolling24MonthComparision ";
            sSQL += "@Threshold='" + sThreshold + "',";
            sSQL += "@Flag='" + sFlag + "',";
            sSQL += "@StockCode='',";
            sSQL += "@Customer='" + sCustomer + "'";
        }
        else
        {
            //Should Never be here...
        }
        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");


        if (dt.Rows.Count > 0)
        {
            Session["dtReport"] = dt;
            gvPurchasingTrends.DataSource = dt;
            gvPurchasingTrends.DataBind();
        }
        else
        {
            gvPurchasingTrends.DataSource = null;
            gvPurchasingTrends.DataBind();
            lblError.Text = "No results found!!";

        }



        //Set up header column text...
        int iColumnCount = 0;
        iColumnCount = gvPurchasingTrends.HeaderRow.Cells.Count;
        for (int iColumnIndex = 0; iColumnIndex < iColumnCount; iColumnIndex++)
        {
            LinkButton SortButton = (LinkButton)gvPurchasingTrends.HeaderRow.Cells[iColumnIndex].Controls[0];

            switch (SortButton.Text)
            {
                case "Current12MonthTotal":
                    SortButton.Text = "Current 12 Months Total";
                    break;
                case "Previous12MonthTotal":
                    SortButton.Text = "Previous 12 Months Total";
                    break;
                case "Difference":
                    SortButton.Text = "Difference %";
                    break;
                
            }

        }
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

            
            LoadCustomer(ddlCustomers, "Name");
            
            ddlYear.SelectedValue = "All";
            string sStockCode = "";
            string sStartDate = "";
            string sEndDate = "";
            if (Request.QueryString["sc"] != null)
            {
                sStockCode = Request.QueryString["sc"].ToString();
                txtStockCode.Text = sStockCode;
            }
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
    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {

        //txtStartDateChart2.Text = txtStartDate.Text;
    }
    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {

        // txtEndDateChart2.Text = txtEndDate.Text;
    }
    protected void imgExportExcel_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";
        if (Session["dtReport"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtReport"];

        dt.TableName = "dtReport";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        switch (rblReport.SelectedIndex)
        {
            case 0://Alert Report...
               sFilesName = "AlertReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                break;
            case 1://Sales Trends Report...
                sFilesName = "SalesTrendsReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                break;
        }
       

        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        string sStockCode = "";
        string sCustomer = "";
        string sThreshold = "";
        string sFlag = "";
        lblError.Text = "";

        sStockCode = txtStockCode.Text.Trim();
        sThreshold = ddlThreshold.SelectedValue;
        if (rblIncreaseDecrease.SelectedIndex == 0)
        {//Increase
            sFlag = "1";
        }
        else//Decrease
        {
            sFlag = "0";
        }
        if (ddlCustomers.SelectedIndex != 0)//ALL...
        {//Not null then add quotes...
            sCustomer =  ddlCustomers.SelectedValue.Trim();
        }
        switch (rblReport.SelectedIndex)
        {
            case 0://Alert Report...
                RunAlertReport(sStockCode,sCustomer);
                break;
            case 1://Sales Trends Report...
                RunSalesTrendsReport(sStockCode, sCustomer, sThreshold, sFlag);
                break;
        }
        GetTrendsReport();//to initialize report...
    }
    protected void rblSort_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer(ddlCustomers, rblSort.SelectedValue); 
    }
    protected void lbnTrendsReport_Click(object sender, EventArgs e)
    {

    }
    protected void txtStockCode_TextChanged(object sender, EventArgs e)
    {
        lblStockCodeDesc.Text = SharedFunctions.GetStockCodeDesc(txtStockCode.Text.Trim());
    }
    protected void gvPurchasingTrends_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtReport"];
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 4; i < dt.Columns.Count; i++)
            {               
                e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                if (e.Row.Cells[i].Text == "&nbsp;")
                {  

                }
                else
                {
                    if (e.Row.Cells[i].Text == "0.00")
                    {

                        e.Row.Cells[i].Text = "";
                    }
                    else if (SharedFunctions.IsNumeric(e.Row.Cells[i].Text))//Is numeric...
                    {
                        e.Row.Cells[i].Text = Convert.ToDecimal(e.Row.Cells[i].Text).ToString("#,0");
                    }
                    else if (SharedFunctions.IsDate(e.Row.Cells[i].Text))//Is date...
                    {
                        e.Row.Cells[i].Text = Convert.ToDateTime(e.Row.Cells[i].Text).ToShortDateString();
                    }
                    else
                    {

                    }
                }
            }

        }
    }
    protected void gvPurchasingTrends_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();

        DataTable dt = (DataTable)Session["dtReport"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvPurchasingTrends.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvPurchasingTrends.DataSource = m_DataView;
            gvPurchasingTrends.DataBind();
            gvPurchasingTrends.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
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