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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class IngredientCostTrendsReport : System.Web.UI.Page
{

    private string GridViewSortDirection
    {
        get
        {
            return Session["SortDirection"] as string ?? "DESC";
        }
        set
        {
            Session["SortDirection"] = value;
        }
    }

    #region Subs

    private void GetIngredientCostTrendsReport()
    {

        string sMsg = "";

        //Stock Codes...
        string sStockCodeFrom = "";
        string sStockCodeTo = "";

        if (txtStockCodeFrom.Text.Trim() != "")
        {
            sStockCodeFrom = "'" + txtStockCodeFrom.Text.Trim() + "'";
        }
        else
        {
            sMsg += "**From Stock Code is Required!<br/>";
        }
        if (txtStockCodeTo.Text.Trim() != "")
        {
            sStockCodeTo = "'" + txtStockCodeTo.Text.Trim() + "'";
        }


        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        //Validation...
        if (sMsg.Length > 0)
        {
            lblErrorRange.Text = sMsg;
            return;
        }

        if (sStockCodeTo != "")
        {
            sSQL = "EXEC spGetRawIngredientsTrendsReport ";
            sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
            sSQL += "@StockCodeTo=" + sStockCodeTo;
        }
        else //No To Stock Code entered...
        {
            sSQL = "EXEC spGetRawIngredientsTrendsReport ";
            sSQL += "@StockCodeFrom=" + sStockCodeFrom;
        }


        Debug.WriteLine(sSQL);


        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");


        if (dt.Rows.Count > 0)
        {
            DataColumn columnID = new DataColumn();
            columnID.DataType = System.Type.GetType("System.Int32");
            columnID.ColumnName = "#";
            dt.Columns.Add(columnID);

            //Set values for existing rows...
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["#"] = i + 1;
            }

            // Move RowID column to the first position
            dt.Columns["#"].SetOrdinal(0);

            dt.AcceptChanges();

            DataView dv = new DataView(dt);
            dv.Sort = "[Stock Code] ASC";

            Session["dtReport"] = dt;

            gvIngredientCostTrends.DataSource = dv;
            gvIngredientCostTrends.DataBind();
        }
        else
        {
            lblError.Text = "No results found!!";
            lblError.ForeColor = Color.Red;

            gvIngredientCostTrends.DataSource = null;
            gvIngredientCostTrends.DataBind();
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

        }


        if (!Page.IsPostBack)
        {
            if (Server.MachineName.ToUpper() == "BMW" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
            {
                txtStockCodeFrom.Text = "603012";
                txtStockCodeTo.Text = "603517";
            }
            Session["dtReport"] = null;

        }
        if (IsPostBack)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isPostBack", "isPostBack();", true);
        }
    }

    protected void gvIngredientCostTrends_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        // Target the header row
        if (e.Row.RowType == DataControlRowType.Header && Session["dtReport"] != null)
        {
            DataTable dt = (DataTable)Session["dtReport"];

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];

                if (dt != null && i < dt.Columns.Count)
                {
                    string ColumnName = dt.Columns[i].ColumnName;
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    cell.CssClass = "CenterAligner";
                    if (ColumnName == "#")
                    {
                        cell.Width = Unit.Pixel(50);
                    }
                    else if (ColumnName == "Stock Code")
                    {
                        cell.Width = Unit.Pixel(150);
                    }
                    else if (ColumnName == "Description")
                    {
                        cell.Width = Unit.Pixel(500);
                    }
                    else if (ColumnName == "Uom")
                    {
                        cell.Width = Unit.Pixel(75);
                    }
                    else if (ColumnName.StartsWith("Volume"))
                    {
                        cell.Width = Unit.Pixel(100);
                    }
                    else if (ColumnName.StartsWith("AvgCost"))
                    {
                        cell.Width = Unit.Pixel(100);
                    }
                    else if (ColumnName == "Current Price")
                    {
                        cell.Width = Unit.Pixel(100);
                    }
                    else if (ColumnName == "Current Price Date")
                    {
                        cell.Width = Unit.Pixel(125);
                    }
                    else if (ColumnName == "1st Yr +/- % Diff")
                    {
                        cell.Width = Unit.Pixel(150);
                    }
                    else if (ColumnName == "2nd Yr +/- % Diff")
                    {
                        cell.Width = Unit.Pixel(150);
                    }

                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow && Session["dtReport"] != null)
        {
            DataTable dt = (DataTable)Session["dtReport"];

            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                // Fallback to DataTable column names if HeaderRow is blank
                string columnName = gvIngredientCostTrends.HeaderRow != null &&
                                    !string.IsNullOrEmpty(gvIngredientCostTrends.HeaderRow.Cells[i].Text)
                                    ? gvIngredientCostTrends.HeaderRow.Cells[i].Text
                                    : dt.Columns[i].ColumnName;

                // Debugging: Log column name and data
                //Debug.WriteLine("Column: " + columnName + ", Data: " + e.Row.Cells[i].Text);

                // Apply formatting based on column name
                if (columnName.StartsWith("Volume") || columnName.Contains("Avg") || columnName == "Curren tPrice" || columnName == "1st Yr +/- % Diff" || columnName == "2nd Yr +/- % Diff")
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right; // Right-justify numeric columns
                    e.Row.Cells[i].Width = Unit.Pixel(100); // Set a fixed width (adjust as needed)                    
                }
                else if (columnName == "Current Price Date")
                {
                    e.Row.Cells[i].Text = Convert.ToDateTime(e.Row.Cells[i].Text).ToShortDateString();
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[i].Width = Unit.Pixel(125);
                }
                else if (columnName.StartsWith("Uom"))
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                }
                else if (columnName.StartsWith("Stock Code"))
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[i].Font.Bold = true;
                }
                else if (columnName.StartsWith("Description"))
                {
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[i].Width = Unit.Pixel(500);
                }
            }
        }
    }
    protected void gvIngredientCostTrends_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (Session["dtReport"] != null)
        {
            // Retrieve the DataTable from the session
            DataTable dt = (DataTable)Session["dtReport"];

            if (dt != null)
            {
                // Preserve the current page index
                int currentPageIndex = gvIngredientCostTrends.PageIndex;

                // Determine the sort direction
                string sortDirection = GetSortDirection();

                // Create a DataView to sort the DataTable
                DataView dv = new DataView(dt);
                dv.Sort = e.SortExpression + " " + sortDirection;

                // Bind the sorted DataView to the GridView
                gvIngredientCostTrends.DataSource = dv;
                gvIngredientCostTrends.DataBind();

                // Restore the page index
                gvIngredientCostTrends.PageIndex = currentPageIndex;

                // Save the sorted DataTable to the session (optional)
                Session["dtReport"] = dt;
            }
        }
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblErrorRange.Text = "";
        GetIngredientCostTrendsReport();

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblErrorRange.Text = "";
        txtStockCodeFrom.Text = "";
        txtStockCodeTo.Text = "";
        gvIngredientCostTrends.DataSource = null;
        gvIngredientCostTrends.DataBind();
    }
    protected void imgExportExcel1_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblErrorRange.Text = "";
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
            ds.Tables.Add(dt);
        }

        sFilesName = "GetIngredientCostTrendsReport_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    #endregion

    #region WebMethods
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListStockCodes(string prefixText, int count, string contextKey)
    {
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {
            using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
            {

                try
                {
                    // Perform all filtering and grouping in SQL
                    var stockCodeList = db.PorMasterDetail
                        .Where(pmd =>
                            pmd.MStockCode != null &&                           // Ensure MStockCode is not null
                            pmd.MStockCode.StartsWith(prefixText) &&           // Use SQL's LIKE equivalent
                            (
                                (string.Compare(pmd.MStockCode, "600000") >= 0 && string.Compare(pmd.MStockCode, "649999") <= 0) ||
                                (string.Compare(pmd.MStockCode, "700000") >= 0 && string.Compare(pmd.MStockCode, "759999") <= 0)
                            )
                        )
                        .GroupBy(pmd => pmd.MStockCode)                         // Group by MStockCode
                        .Select(g => g.Key.Trim())                              // Select distinct, trimmed keys
                        .OrderBy(stockCode => stockCode)                        // Order results
                        .Take(count)                                            // Limit results to 'count'
                        .ToArray();                                             // Execute the query and return an array

                    return stockCodeList;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    return new string[] { "None found" };
                }
            }
        }
    }

    #endregion



}
