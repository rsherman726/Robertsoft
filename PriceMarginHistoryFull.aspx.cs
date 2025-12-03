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

public partial class PriceMarginHistoryFull : System.Web.UI.Page
{
    //Full Report...
    decimal dcAmountYTDTotal = 0;
    decimal dcAmountYearMinus1Total = 0;
    decimal dcAmountYearMinus2Total = 0;
    decimal dcAmountYearMinus3Total = 0;

    decimal dcPriceYTDTotal = 0;
    decimal dcPriceYearMinus1Total = 0;
    decimal dcPriceYearMinus2Total = 0;
    decimal dcPriceYearMinus3Total = 0;




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
    private void RunReportSummary()
    {
        string sMsg = "";
        string sStockCode = "";

        int iIndexOfPipe = 0;
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


        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }

        sSQL = "EXEC spGetPriceMarginHistory ";
        sSQL += "@StockCodes=" + sStockCode;

        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");


         if (dt.Rows.Count > 0)
        {
            Session["dtCustomerReport"] = dt;
            gvReportCondensed.DataSource = dt;
            gvReportCondensed.DataBind();

            pnlGridView.Visible = true;
            tblHeaderTable.Visible = true;
        }
        else
        {
            lblError.Text = "No results found!!";
            lblError.ForeColor = Color.Red;
            pnlGridView.Visible = false;
            tblHeaderTable.Visible = false;

            gvReportCondensed.DataSource = null;
            gvReportCondensed.DataBind();
        }

        if (dt.Rows.Count > 9)
        {
            pnlGridView.ScrollBars = ScrollBars.Vertical;
            pnlGridView.Width = Unit.Pixel(1320);
            tblHeaderTable.Width = Unit.Pixel(1320);
        }
        else
        {
            pnlGridView.ScrollBars = ScrollBars.None;
            pnlGridView.Width = Unit.Pixel(1250);
            tblHeaderTable.Width = Unit.Pixel(1325);
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
            LoadStockCodesWipMaster();


        }//End postback

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
    protected void gvReportCondensed_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        int iRoleID = 0;
        iRoleID = Convert.ToInt32(Session["RoleID"]);

        decimal dcAmountYTD = 0;
        decimal dcAmountYearMinus1 = 0;
        decimal dcAmountYearMinus2 = 0;
        decimal dcAmountYearMinus3 = 0;

        decimal dcPriceYTD = 0;
        decimal dcPriceYearMinus1 = 0;
        decimal dcPriceYearMinus2 = 0;
        decimal dcPriceYearMinus3 = 0;

        try
        {



            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Text = "Margin<br/>" + DateTime.Now.AddYears(-1).Year.ToString();
                e.Row.Cells[4].Text = "Margin<br/>" + DateTime.Now.AddYears(-2).Year.ToString();
                e.Row.Cells[5].Text = "Margin<br/>" + DateTime.Now.AddYears(-3).Year.ToString();

                e.Row.Cells[7].Text = "Amount<br/>" + DateTime.Now.AddYears(-1).Year.ToString();
                e.Row.Cells[8].Text = "Amount<br/>" + DateTime.Now.AddYears(-2).Year.ToString();
                e.Row.Cells[9].Text = "Amount<br/>" + DateTime.Now.AddYears(-3).Year.ToString();


                e.Row.Cells[11].Text = "Price<br/>" + DateTime.Now.AddYears(-1).Year.ToString();
                e.Row.Cells[12].Text = "Price<br/>" + DateTime.Now.AddYears(-2).Year.ToString();
                e.Row.Cells[13].Text = "Price<br/>" + DateTime.Now.AddYears(-3).Year.ToString();

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {



                Label lblAmountYTD = (Label)e.Row.FindControl("lblAmountYTD");
                Label lblAmountYearMinus1 = (Label)e.Row.FindControl("lblAmountYearMinus1");
                Label lblAmountYearMinus2 = (Label)e.Row.FindControl("lblAmountYearMinus2");
                Label lblAmountYearMinus3 = (Label)e.Row.FindControl("lblAmountYearMinus3");

                Label lblPriceYTD = (Label)e.Row.FindControl("lblPriceYTD");
                Label lblPriceYearMinus1 = (Label)e.Row.FindControl("lblPriceYearMinus1");
                Label lblPriceYearMinus2 = (Label)e.Row.FindControl("lblPriceYearMinus2");
                Label lblPriceYearMinus3 = (Label)e.Row.FindControl("lblPriceYearMinus3");


                ////1   Admin
                ////2   Supervisor
                ////3   Manager
                ////4   Employee
                ////5   Customer Service Mgr
                ////6   Customer Service Employee
                ////7   Special1


                if (lblAmountYTD.Text != "")
                {
                    dcAmountYTD = Convert.ToDecimal(lblAmountYTD.Text.Trim());
                    dcAmountYTDTotal += dcAmountYTD;
                    lblAmountYTD.Text = Convert.ToDecimal(lblAmountYTD.Text).ToString("#,0.00");
                }
                if (lblAmountYearMinus1.Text != "")
                {
                    dcAmountYearMinus1 = Convert.ToDecimal(lblAmountYearMinus1.Text.Trim());
                    dcAmountYearMinus1Total += dcAmountYearMinus1;
                    lblAmountYearMinus1.Text = Convert.ToDecimal(lblAmountYearMinus1.Text).ToString("#,0.00");
                }
                if (lblAmountYearMinus2.Text != "")
                {
                    dcAmountYearMinus2 = Convert.ToDecimal(lblAmountYearMinus2.Text.Trim());
                    dcAmountYearMinus2Total += dcAmountYearMinus2;
                    lblAmountYearMinus2.Text = Convert.ToDecimal(lblAmountYearMinus2.Text).ToString("#,0.00");
                }
                if (lblAmountYearMinus3.Text != "")
                {
                    dcAmountYearMinus3 = Convert.ToDecimal(lblAmountYearMinus3.Text.Trim());
                    dcAmountYearMinus3Total += dcAmountYearMinus3;
                    lblAmountYearMinus3.Text = Convert.ToDecimal(lblAmountYearMinus3.Text).ToString("#,0.00");
                }

                if (lblPriceYTD.Text != "")
                {
                    dcPriceYTD = Convert.ToDecimal(lblPriceYTD.Text.Trim());
                    dcPriceYTDTotal += dcPriceYTD;
                    lblPriceYTD.Text = Convert.ToDecimal(lblPriceYTD.Text).ToString("#,0.00");
                }
                if (lblPriceYearMinus1.Text != "")
                {
                    dcPriceYearMinus1 = Convert.ToDecimal(lblPriceYearMinus1.Text.Trim());
                    dcPriceYearMinus1Total += dcPriceYearMinus1;
                    lblPriceYearMinus1.Text = Convert.ToDecimal(lblPriceYearMinus1.Text).ToString("#,0.00");
                }
                if (lblPriceYearMinus2.Text != "")
                {
                    dcPriceYearMinus2 = Convert.ToDecimal(lblPriceYearMinus2.Text.Trim());
                    dcPriceYearMinus2Total += dcPriceYearMinus2;
                    lblPriceYearMinus2.Text = Convert.ToDecimal(lblPriceYearMinus2.Text).ToString("#,0.00");
                }
                if (lblPriceYearMinus3.Text != "")
                {
                    dcPriceYearMinus3 = Convert.ToDecimal(lblPriceYearMinus3.Text.Trim());
                    dcPriceYearMinus3Total += dcPriceYearMinus3;
                    lblPriceYearMinus3.Text = Convert.ToDecimal(lblPriceYearMinus3.Text).ToString("#,0.00");
                }


            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {


                Label lblAmountYTDTotal = (Label)e.Row.FindControl("lblAmountYTDTotal");
                Label lblAmountYearMinus1Total = (Label)e.Row.FindControl("lblAmountYearMinus1Total");
                Label lblAmountYearMinus2Total = (Label)e.Row.FindControl("lblAmountYearMinus2Total");
                Label lblAmountYearMinus3Total = (Label)e.Row.FindControl("lblAmountYearMinus3Total");

                Label lblPriceYTDTotal = (Label)e.Row.FindControl("lblPriceYTDTotal");
                Label lblPriceYearMinus1Total = (Label)e.Row.FindControl("lblPriceYearMinus1Total");
                Label lblPriceYearMinus2Total = (Label)e.Row.FindControl("lblPriceYearMinus2Total");
                Label lblPriceYearMinus3Total = (Label)e.Row.FindControl("lblPriceYearMinus3Total");

                lblAmountYTDTotal.Text = "$" + dcAmountYTDTotal.ToString("#,0.00");
                lblAmountYearMinus1Total.Text = "$" + dcAmountYearMinus1Total.ToString("#,0.00");
                lblAmountYearMinus2Total.Text = "$" + dcAmountYearMinus2Total.ToString("#,0.00");
                lblAmountYearMinus3Total.Text = "$" + dcAmountYearMinus3Total.ToString("#,0.00");

                lblPriceYTDTotal.Text = "$" + dcPriceYTDTotal.ToString("#,0.00");
                lblPriceYearMinus1Total.Text = "$" + dcPriceYearMinus1Total.ToString("#,0.00");
                lblPriceYearMinus2Total.Text = "$" + dcPriceYearMinus2Total.ToString("#,0.00");
                lblPriceYearMinus3Total.Text = "$" + dcPriceYearMinus3Total.ToString("#,0.00");

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvReportCondensed_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();

        DataTable dt = (DataTable)Session["dtPriceMarginHistory"];
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
    protected void btnPreviewChart1_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        string sMsg = "";
        if (lbParentStockCode.SelectedIndex != -1)
        {
            RunReportSummary();
        }
        else
        {
            sMsg += "**StockCode is required!!";
            lblError.Text = sMsg;
            return;
        }
    }

    protected void HeaderLink1_Click(object sender, System.EventArgs e)
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
    protected void Page_Init(object sender, EventArgs e)
    {
        TableRow headerRow = new TableRow();

        divHeader.Style.Add("width", "1200px");
        for (int x = 0; x < gvReportCondensed.Columns.Count; x++)
        {
            DataControlField col = gvReportCondensed.Columns[x];

            TableCell headerCell = new TableCell();
            headerCell.BorderStyle = BorderStyle.Solid;
            headerCell.BorderWidth = 0;//Hide vertical grid lines on header...
            headerCell.Font.Bold = true;
            headerCell.Font.Size = FontUnit.Point(10);
            headerCell.HorizontalAlign = HorizontalAlign.Center;

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

                lnkHeader.Click += new EventHandler(HeaderLink1_Click);
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
                    headerCell.Width = Unit.Pixel(60);
                    break;
                case 1://Description
                    headerCell.Width = Unit.Pixel(95);
                    break;
                case 2://MarginYTD
                    headerCell.Width = Unit.Pixel(50);
                    break;
                case 3://Margin -1
                    headerCell.Width = Unit.Pixel(50);
                    headerCell.Text = "Margin<br/>" + DateTime.Now.AddYears(-1).Year.ToString();
                    break;
                case 4://Margin -2
                    headerCell.Width = Unit.Pixel(50);
                    headerCell.Text = "Margin<br/>" + DateTime.Now.AddYears(-2).Year.ToString();
                    break;
                case 5://Margin -3
                    headerCell.Width = Unit.Pixel(50);
                    headerCell.Text = "Margin<br/>" + DateTime.Now.AddYears(-3).Year.ToString();
                    break;
                case 6://AmountYTD
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 7://Amount -1
                    headerCell.Width = Unit.Pixel(40);
                    headerCell.Text = "Amount<br/>" + DateTime.Now.AddYears(-1).Year.ToString();
                    break;
                case 8://Amount -2
                    headerCell.Width = Unit.Pixel(40);
                    headerCell.Text = "Amount<br/>" + DateTime.Now.AddYears(-2).Year.ToString();
                    break;
                case 9://Amount -3
                    headerCell.Width = Unit.Pixel(40);
                    headerCell.Text = "Amount<br/>" + DateTime.Now.AddYears(-3).Year.ToString();
                    break;
                case 10://PriceYTD
                    headerCell.Width = Unit.Pixel(50);
                    break;
                case 11://Price -1
                    headerCell.Width = Unit.Pixel(50);
                    headerCell.Text = "Price<br/>" + DateTime.Now.AddYears(-1).Year.ToString();
                    break;
                case 12://Price -2
                    headerCell.Width = Unit.Pixel(50);
                    headerCell.Text = "Price<br/>" + DateTime.Now.AddYears(-2).Year.ToString();
                    break;
                case 13://Price -3
                    headerCell.Width = Unit.Pixel(70);
                    headerCell.Text = "Price<br/>" + DateTime.Now.AddYears(-3).Year.ToString();
                    break;

            }


            //headerCell.Width = col.ItemStyle.Width;
            headerRow.Cells.Add(headerCell);

        }

        tblHeaderTable.Rows.Add(headerRow);
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