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

public partial class ComponentCostIncreaseReport : System.Web.UI.Page
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

    #endregion


    #region Functions
    private string RunReport(string sStockCode)
    {
        string sMsg = "";
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        //Validation...


        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return sMsg;
        }

        sSQL = "EXEC spGetComponentCostPriceIncrease ";
        sSQL += "@MStockCode='" + sStockCode + "'";

        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
   
        //foreach (DataRow Row in dt.Rows)
        //{
        //    foreach (DataColumn c in dt.Columns)
        //    {
        //        Debug.WriteLine(c.ColumnName + ": " + Row[c.ColumnName].ToString());
        //    }
        //    Debug.WriteLine("----------------------------------------------------");
        //}

        gvReportCondensed.DataSource = dt;
        gvReportCondensed.DataBind();

        Session["dtComp"] = dt;

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
            lblError.Text = "**No Stock Code entered!";
            lblError.ForeColor = Color.Red;
            return;
        }
        else
        {
            sStockCode = txtStockCode.Text.Trim();
        }

        sMsg = RunReport(sStockCode);

        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }     


    }
    protected void lbnExportExcel_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";

        if (Session["dtComp"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtComp"];

        dt.TableName = "dtComp"; 
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "ComponentCostIncreaseReport_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();


    }
    protected void gvReportCondensed_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


        }
    }
    protected void txtStockCode_TextChanged(object sender, EventArgs e)
    {
        lblDescription.Text = SharedFunctions.GetStockCodeDesc(txtStockCode.Text.Trim());
        gvReportCondensed.DataSource = null;
        gvReportCondensed.DataBind();
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
            string[] list = db.VwComponentStockCodesRSS.Where(w => w.StockCode != null ).OrderBy(w => w.StockCode).Select(w => (w.StockCode.ToString())).Distinct().ToArray();
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