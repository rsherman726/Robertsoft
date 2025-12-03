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

public partial class StockCodeListCustomerReport : System.Web.UI.Page
{
    decimal dcCostValueTotal = 0;
    decimal dcTotalAmount = 0;
    decimal dcTotalAmountSummary = 0;


    //Full Report...
    decimal dcYTDTotal = 0;
    decimal dcYearMinus1Total = 0;
    decimal dcYearMinus2Total = 0;
    decimal dcYearMinus3Total = 0;
    decimal dcYearMinus4Total = 0;
    decimal dcYearMinus5Total = 0;
    //Summary...
    decimal dcYTDTotalSummary = 0;
    decimal dcYearMinus1TotalSummary = 0;
    decimal dcYearMinus2TotalSummary = 0;
    decimal dcYearMinus3TotalSummary = 0;
    decimal dcYearMinus4TotalSummary = 0;
    decimal dcYearMinus5TotalSummary = 0;

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
    private void LoadParentStockCodesIngredients(ListBox lb)
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
    private void LoadParentStockCodesIngredientsOld(ListBox lb)
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
    private void LoadProductClass(DropDownList ddl)
    {
        ddl.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from c in db.SalProductClassDes
                     orderby c.Description
                     select new
                     {
                         c.Description,
                         c.ProductClass

                     });
        foreach (var a in query)
        {
            ddl.Items.Add(new ListItem(a.Description + " - " + a.ProductClass, a.ProductClass));
        }
        ddl.Items.Insert(0, new ListItem("All", "All"));
    }
    private void LoadSalesPersons()
    {
        ddlSalesPerson.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from c in db.SalSalesperson
                     orderby c.Name
                     select new
                     {
                         c.Name,
                         c.Salesperson

                     });
        foreach (var a in query)
        {
            ddlSalesPerson.Items.Add(new ListItem(a.Name, a.Salesperson));
        }
        ddlSalesPerson.Items.Insert(0, new ListItem("All", "All"));
    }
    private void LoadCustomerForCharts(DropDownList ddl)
    {
        ddl.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
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
        ddl.Items.Insert(0, new ListItem("Select", "0"));
    }
    private void RunReport()
    {
        string sMsg = "";

        string sCustomer = "NULL";
        if (ddlCustomers.SelectedIndex != 0)//ALL...
        {//Not null then add quotes...
            sCustomer = "'" + ddlCustomers.SelectedValue.Trim() + "'";
        }


        string sProductClass = "NULL";
        if (ddlProductClass.SelectedIndex != 0)//ALL...
        {//Not null then add quotes...
            sProductClass = "'" + ddlProductClass.SelectedValue.Trim() + "'";
        }

        string sSalesPerson = "NULL";
        if (ddlSalesPerson.SelectedIndex != 0)//ALL...
        {//Not null then add quotes...
            sSalesPerson = "'" + ddlSalesPerson.SelectedValue.Trim() + "'";
        }
        string sYear = "NULL";
        if (ddlYear.SelectedIndex != 0)//ALL...
        {//Not null then add quotes...
            sYear = "'" + ddlYear.SelectedValue.Trim() + "'";
        }
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
        string sFromMargin = "NULL";
        if (txtMarginFrom.Text.Trim() != "")
        {//Not null then add quotes...
            sFromMargin = "'" + txtMarginFrom.Text.Trim() + "'";
        }
        string sToMargin = "NULL";
        if (txtMarginTo.Text.Trim() != "")
        {//Not null then add quotes...
            sToMargin = "'" + txtMarginTo.Text.Trim() + "'";
        }
        //Stock Codes...

        string sStockCode = "NULL";

        int iIndexOfPipe = 0;
        if (lbParentStockCode.GetSelectedIndices().Count() > 0)
        {
            sStockCode = "";
        }
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
            lblErrorRange.Text = sMsg;
            return;
        }




        sSQL = "EXEC spGetCustomerReport ";
        sSQL += "@FromDate=" + sStartDate + ",";
        sSQL += "@ToDate=" + sEndDate + ",";
        sSQL += "@FromMargin=" + sFromMargin + ",";
        sSQL += "@ToMargin=" + sToMargin + ",";
        sSQL += "@StockCodes=" + sStockCode + ",";
        sSQL += "@Customer=" + sCustomer + ",";
        sSQL += "@Year=" + sYear + ",";
        sSQL += "@ProductClass=" + sProductClass + ",";
        sSQL += "@SalesPerson=" + sSalesPerson;


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
            pnlGridView.Width = Unit.Pixel(1620);
            tblHeaderTable.Width = Unit.Pixel(1620);
            gvCustomerSummary.Width = Unit.Pixel(1574);
        }
        else
        {
            pnlGridView.ScrollBars = ScrollBars.None;
            pnlGridView.Width = Unit.Pixel(1350);
            tblHeaderTable.Width = Unit.Pixel(1574);
            gvCustomerSummary.Width = Unit.Pixel(1350);
        }

    }
    private void RunReportSummary()
    {
        string sMsg = "";

        string sCustomer = "NULL";
        if (ddlCustomers.SelectedIndex != 0)//ALL...
        {//Not null then add quotes...
            sCustomer = "'" + ddlCustomers.SelectedValue.Trim() + "'";
        }


        string sProductClass = "NULL";
        if (ddlProductClass.SelectedIndex != 0)//ALL...
        {//Not null then add quotes...
            sProductClass = "'" + ddlProductClass.SelectedValue.Trim() + "'";
        }

        string sSalesPerson = "NULL";
        if (ddlSalesPerson.SelectedIndex != 0)//ALL...
        {//Not null then add quotes...
            sSalesPerson = "'" + ddlSalesPerson.SelectedValue.Trim() + "'";
        }
        string sYear = "NULL";
        if (ddlYear.SelectedIndex != 0)//ALL...
        {//Not null then add quotes...
            sYear = "'" + ddlYear.SelectedValue.Trim() + "'";
        }
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
        string sFromMargin = "NULL";
        if (txtMarginFrom.Text.Trim() != "")
        {//Not null then add quotes...
            sFromMargin = "'" + txtMarginFrom.Text.Trim() + "'";
        }
        string sToMargin = "NULL";
        if (txtMarginTo.Text.Trim() != "")
        {//Not null then add quotes...
            sToMargin = "'" + txtMarginTo.Text.Trim() + "'";
        }
        string sStockCode = "NULL";
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


        sSQL = "EXEC spGetCustomerReportSummary ";
        sSQL += "@FromDate=" + sStartDate + ",";
        sSQL += "@ToDate=" + sEndDate + ",";
        sSQL += "@FromMargin=" + sFromMargin + ",";
        sSQL += "@ToMargin=" + sToMargin + ",";
        sSQL += "@StockCodes=" + sStockCode + ",";
        sSQL += "@Customer=" + sCustomer + ",";
        sSQL += "@Year=" + sYear + ",";
        sSQL += "@ProductClass=" + sProductClass + ",";
        sSQL += "@SalesPerson=" + sSalesPerson;




        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");


        if (dt.Rows.Count > 0)
        {
            Session["dtCustomerReportSummary"] = dt;
            gvCustomerSummary.DataSource = dt;
            gvCustomerSummary.DataBind();
        }
        else
        {
            lblError.Text = "No results found!!";
            lblError.ForeColor = Color.Red;

            gvCustomerSummary.DataSource = null;
            gvCustomerSummary.DataBind();
        }

    }
    //private void RunReport2()
    //{

    //    int iLastPipeInStringIdx = 0;
    //    string sStockCodeList = "";
    //    string sCustomer = "NULL";
    //    DataTable dt = new DataTable();
    //    string sSQL = "";
    //    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

    //    if (ddlCustomers0.SelectedIndex != 0)//ALL...
    //    {//Not null then add quotes...
    //        sCustomer = "'" + ddlCustomers0.SelectedValue.Trim() + "'";
    //    }
    //    //Stock Codes...
    //    foreach (ListItem li in lbStockCode0.Items)
    //    {
    //        if (li.Selected)
    //        {
    //            sStockCodeList += li.Value.Trim() + "|";
    //        }
    //    }
    //    if (sStockCodeList.Trim().EndsWith("|"))
    //    {
    //        iLastPipeInStringIdx = sStockCodeList.LastIndexOf("|");
    //        sStockCodeList = sStockCodeList.Remove(iLastPipeInStringIdx).Trim();
    //    }


    //    if (sStockCodeList != "")//Selected Stock Codes...
    //    {
    //        sSQL = "EXEC spGetCustomerReportLastPriceIncrease ";
    //        sSQL += "@StockCodes='" + sStockCodeList + "',";
    //        sSQL += "@Customer=" + sCustomer;
    //    }

    //    Debug.WriteLine(sSQL);

    //    dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
    //    Session["dtDatePriorToPriceChange"] = dt;
    //    gvDatePriorToPriceChange.DataSource = dt;
    //    gvDatePriorToPriceChange.DataBind();

    //    if (dt.Rows.Count > 0)
    //    {
    //        pnlGridView2.Visible = true;
    //        HeaderTable2.Visible = true;
    //    }
    //    else
    //    {
    //        lblErrorPrior.Text = "No results found!!";
    //        lblErrorPrior.ForeColor = Color.Red;
    //        pnlGridView2.Visible = false;
    //        HeaderTable2.Visible = false;
    //    }

    //    if (dt.Rows.Count > 9)
    //    {
    //        pnlGridView2.ScrollBars = ScrollBars.Vertical;
    //        pnlGridView2.Width = Unit.Pixel(1050);
    //        HeaderTable2.Width = Unit.Pixel(1050);
    //    }
    //    else
    //    {
    //        pnlGridView2.ScrollBars = ScrollBars.None;
    //        pnlGridView2.Width = Unit.Pixel(1130);
    //        HeaderTable2.Width = Unit.Pixel(1010);
    //    }

    //}
    private void SetupHeaderTable1()
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
                case 0://Name
                    headerCell.Width = Unit.Pixel(65);
                    break;
                case 1://Customer#
                    headerCell.Width = Unit.Pixel(50);
                    break;
                case 2://SalesPerson
                    headerCell.Width = Unit.Pixel(60);
                    break;
                case 3://StockCode
                    headerCell.Width = Unit.Pixel(60);
                    break;
                case 4://Decription
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 5://ProductClass
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 6://Uom
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 7://Qty
                    headerCell.Width = Unit.Pixel(40);
                    break;
                case 8://Amount
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 9://Margin
                    headerCell.Width = Unit.Pixel(45);
                    break;
                case 10://PriceCode
                    headerCell.Width = Unit.Pixel(25);
                    break;
                case 11://Price
                    headerCell.Width = Unit.Pixel(50);
                    break;
                case 12://LastChangeDate
                    headerCell.Width = Unit.Pixel(50);
                    break;
                case 13://YTD
                    headerCell.Width = Unit.Pixel(60);
                    break;
                case 14://YearMinus1
                    headerCell.Text = DateTime.Now.AddYears(-1).Year.ToString();
                    headerCell.Width = Unit.Pixel(60);
                    break;
                case 15://YearMinus2
                    headerCell.Text = DateTime.Now.AddYears(-2).Year.ToString();
                    headerCell.Width = Unit.Pixel(60);
                    break;
                case 16://YearMinus3
                    headerCell.Text = DateTime.Now.AddYears(-3).Year.ToString();
                    headerCell.Width = Unit.Pixel(60);
                    break;
                case 17://YearMinus4
                    headerCell.Text = DateTime.Now.AddYears(-4).Year.ToString();
                    headerCell.Width = Unit.Pixel(60);
                    break;
                case 18://YearMinus5
                    headerCell.Text = DateTime.Now.AddYears(-5).Year.ToString();
                    headerCell.Width = Unit.Pixel(60);
                    break;

            }


            //headerCell.Width = col.ItemStyle.Width;
            headerRow.Cells.Add(headerCell);

        }

        tblHeaderTable.Rows.Add(headerRow);
    }
    //private void SetupHeaderTable2()
    //{
    //    TableRow headerRow = new TableRow();

    //    for (int x = 0; x < gvDatePriorToPriceChange.Columns.Count; x++)
    //    {
    //        DataControlField col = gvDatePriorToPriceChange.Columns[x];

    //        TableCell headerCell = new TableCell();
    //        headerCell.BorderStyle = BorderStyle.Solid;
    //        headerCell.BorderWidth = 0;//Hide vertical grid lines on header...
    //        headerCell.Font.Bold = true;
    //        headerCell.Font.Size = FontUnit.Point(10);
    //        headerCell.HorizontalAlign = HorizontalAlign.Center;

    //        // if the column has a SortExpression, we want to allow
    //        // sorting by that column. Therefore, we create a linkbutton
    //        // on those columns.
    //        if (col.SortExpression != "")
    //        {
    //            LinkButton lnkHeader = new LinkButton();

    //            // *** Comment the line below if using with AJAX
    //            //// lnkHeader.PostBackUrl = HttpContext.Current.Request.Url.LocalPath;

    //            lnkHeader.CommandArgument = col.SortExpression;
    //            lnkHeader.ForeColor = System.Drawing.Color.White;
    //            lnkHeader.Text = col.HeaderText.Replace(" ", "<br/>");

    //            // *** Uncomment this line for AJAX 
    //            lnkHeader.ID = "Sort" + col.HeaderText;

    //            lnkHeader.Click += new EventHandler(HeaderLink2_Click);
    //            headerCell.Controls.Add(lnkHeader);
    //        }
    //        else
    //        {
    //            headerCell.Text = col.HeaderText;
    //        }
    //        //We need to set the width of the column header customly...
    //        switch (x)
    //        {
    //            case 0://Name
    //                headerCell.Width = Unit.Pixel(145);
    //                break;
    //            case 1://Customer
    //                headerCell.Width = Unit.Pixel(55);
    //                break;
    //            case 2://StockCode
    //                headerCell.Width = Unit.Pixel(45);
    //                break;
    //            case 3://Decription
    //                headerCell.Width = Unit.Pixel(140);
    //                break;
    //            case 4://CostUom
    //                headerCell.Width = Unit.Pixel(25);
    //                break;
    //            case 5://UnitCost
    //                headerCell.Width = Unit.Pixel(42);
    //                break;
    //            case 6://Margin
    //                headerCell.Width = Unit.Pixel(20);
    //                break;
    //            case 7://LastPriceIncreaseDate
    //                headerCell.Width = Unit.Pixel(50);
    //                break;
    //        }


    //        //headerCell.Width = col.ItemStyle.Width;
    //        headerRow.Cells.Add(headerCell);

    //    }

    //    HeaderTable2.Rows.Add(headerRow);
    //}
    private void LoadStockCodes(ListBox lb)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lb.Items.Clear();
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
            lb.Items.Add(new ListItem(a.StockCode + " - " + a.StockDescription, a.StockCode));
        }
    }
    private void PanelSetup()
    {

        rblSourceOfStockCodes_SelectedIndexChanged(rblSourceOfStockCodes, null);


    }

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


        string sStockCode = "";




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


        //Second Trends Report...
        string sURL2 = "";
        sURL2 = "TrendsReportByQtyPopup.aspx?sc=" + sStockCode;
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
            sURL2 += "&start=" + sStartDate;
        }
        if (sEndDate != "")
        {
            sURL2 += "&end=" + sEndDate;
        }

        sURL2 += "&cus=" + sCustomer;



        lbnTrendsReport.ToolTip = "Click to launch the Purchasing Trends Report Page";
        lbnTrendsReport.Attributes.Add("onclick", "javascript: window.open('" + sURL + "', '_blank','HEIGHT=800,WIDTH=1500,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
        lbnTrendsReport.Style.Add("Cursor", "pointer");

        lbnTrendsReportByQty.ToolTip = "Click to launch the Purchasing Trends Report Page";
        lbnTrendsReportByQty.Attributes.Add("onclick", "javascript: window.open('" + sURL2 + "', '_blank','HEIGHT=800,WIDTH=1500,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
        lbnTrendsReportByQty.Style.Add("Cursor", "pointer");
    }
    #endregion

    #region Functions
    private List<string> GetNoCustomerStockCodes(int iNonCustomerID)
    {
        List<string> lStockCodes = new List<string>();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from c in db.ArNonCustomerStockCodeAssignments
                     where c.NonCustomerID == iNonCustomerID
                     select new
                     {
                         c.StockCode,
                     });
        foreach (var a in query)
        {
            lStockCodes.Add(a.StockCode.Trim());
        }
        return lStockCodes;
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
            txtStockCodeChartIngredientComponent.Attributes.Add("placeholder", "Enter Ingredient Stock Code");


            LoadProductClass(ddlProductClass);
            LoadSalesPersons();
            LoadStockCodes(lbStockCode0);
            LoadCustomer(ddlCustomers, "Name");
            LoadCustomer(ddlCustomers0, "Name");

            ddlYear.SelectedValue = "All";
            string sStockCode = "";
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

            LoadStockCodesWipMaster();


        }//End postback

    }


    protected void ddlMargins_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlMargins.SelectedIndex)
        {
            case 0://Single...

                txtMarginFrom.Enabled = true;
                txtMarginFrom.Focus();

                txtMarginTo.Enabled = false;
                break;
            case 1://Range...

                txtMarginFrom.Enabled = true;
                txtMarginFrom.Focus();
                txtMarginTo.Enabled = true;
                break;
            case 2://All...

                txtMarginFrom.Enabled = false;
                txtMarginTo.Enabled = false;
                txtMarginFrom.Text = "";
                txtMarginTo.Text = "";
                break;
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
    protected void gvReportCondensed_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcCostValue = 0;
        decimal dcAmount = 0;//InvoiceValue...
        int iRoleID = 0;
        iRoleID = Convert.ToInt32(Session["RoleID"]);
        decimal dcYTD = 0;
        decimal dcYearMinus1 = 0;
        decimal dcYearMinus2 = 0;
        decimal dcYearMinus3 = 0;
        decimal dcYearMinus4 = 0;
        decimal dcYearMinus5 = 0;
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[14].Text = DateTime.Now.AddYears(-1).Year.ToString();
            e.Row.Cells[15].Text = DateTime.Now.AddYears(-2).Year.ToString();
            e.Row.Cells[16].Text = DateTime.Now.AddYears(-3).Year.ToString();
            e.Row.Cells[17].Text = DateTime.Now.AddYears(-4).Year.ToString();
            e.Row.Cells[18].Text = DateTime.Now.AddYears(-5).Year.ToString();
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton lbnStockCode = (LinkButton)e.Row.FindControl("lbnStockCode");

            string sStockCode = lbnStockCode.Text.Trim();
            string sDateFrom = "";
            string sDateTo = "";
            if (ddlYear.SelectedIndex == 0)
            {
                switch (ddlPeriod.SelectedValue)
                {
                    case "All":
                        sDateFrom = DateTime.Now.AddYears(-5).ToShortDateString();//Five Years back...
                        sDateTo = DateTime.Now.ToShortDateString();//today...
                        break;
                    case "Range":
                        if (txtStartDate.Text.Trim() == "" && txtEndDate.Text.Trim() == "")
                        {//Should ever get here...
                            sDateFrom = DateTime.Now.AddDays(-7).ToShortDateString();//one day back...
                            sDateTo = DateTime.Now.ToShortDateString();//today...
                        }
                        else
                        {
                            sDateFrom = txtStartDate.Text.Trim();
                            sDateTo = txtEndDate.Text.Trim();
                        }
                        break;
                    default://All Others...
                        sDateFrom = txtStartDate.Text.Trim();
                        sDateTo = txtEndDate.Text.Trim();
                        break;
                }
            }
            else//a year is selected...
            {
                sDateFrom = "01/01/" + ddlYear.SelectedValue;
                sDateTo = "12/31/" + ddlYear.SelectedValue;
            }

            string sURL = "";
            sURL = "CostReportPopup.aspx?sc=" + sStockCode;

            if (sDateFrom != "")
            {
                sURL += "&start=" + sDateFrom;
            }
            if (sDateTo != "")
            {
                sURL += "&end=" + sDateTo;
            }
            sURL += "&period=" + ddlPeriod.SelectedValue;

            lbnStockCode.ToolTip = "Click to launch the Cost Report Page";
            lbnStockCode.Attributes.Add("onclick", "javascript: window.open('" + sURL + "', 'window','HEIGHT=800,WIDTH=1200,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
            lbnStockCode.Style.Add("Cursor", "pointer");


            //LinkButton lbnStockCode = (LinkButton)e.Row.FindControl("lbnStockCode");
            Label lblTrnDate = (Label)e.Row.FindControl("lblTrnDate");
            Label lblQty = (Label)e.Row.FindControl("lblQty");
            Label lblAmount = (Label)e.Row.FindControl("lblAmount");
            Label lblCostValue = (Label)e.Row.FindControl("lblCostValue");
            Label lblMargin = (Label)e.Row.FindControl("lblMargin");
            if (lblQty.Text != "")
            {
                lblQty.Text = Convert.ToDecimal(lblQty.Text).ToString("0.0");
            }
            Label lblYTD = (Label)e.Row.FindControl("lblYTD");
            Label lblYearMinus1 = (Label)e.Row.FindControl("lblYearMinus1");
            Label lblYearMinus2 = (Label)e.Row.FindControl("lblYearMinus2");
            Label lblYearMinus3 = (Label)e.Row.FindControl("lblYearMinus3");
            Label lblYearMinus4 = (Label)e.Row.FindControl("lblYearMinus4");
            Label lblYearMinus5 = (Label)e.Row.FindControl("lblYearMinus5");

            ////1   Admin
            ////2   Supervisor
            ////3   Manager
            ////4   Employee
            ////5   Customer Service Mgr
            ////6   Customer Service Employee
            ////7   Special1
            if (lblMargin.Text != "")
            {
                switch (iRoleID)
                {
                    case 1://Admin...
                        lblMargin.Text = Convert.ToDecimal(lblMargin.Text).ToString("0.0");
                        break;
                    case 2://Supervisor...
                        lblMargin.Text = Convert.ToDecimal(lblMargin.Text).ToString("0.0");
                        break;
                    default:
                        lblMargin.Text = "--";
                        break;
                }

            }
            if (lblAmount.Text != "")
            {
                dcAmount = Convert.ToDecimal(lblAmount.Text.Trim());
                dcTotalAmount += dcAmount;
                lblAmount.Text = Convert.ToDecimal(lblAmount.Text.Trim()).ToString("#,0.00");
            }
            if (lblCostValue.Text != "")
            {
                dcCostValue = Convert.ToDecimal(lblCostValue.Text.Trim());
                dcCostValueTotal += dcCostValue;
                lblCostValue.Text = Convert.ToDecimal(lblCostValue.Text.Trim()).ToString("0.00");
            }//Added 1-18-2015...
            if (lblYTD.Text != "")
            {
                dcYTD = Convert.ToDecimal(lblYTD.Text.Trim());
                dcYTDTotal += dcYTD;
                lblYTD.Text = Convert.ToDecimal(lblYTD.Text).ToString("#,0.00");
            }
            if (lblYearMinus1.Text != "")
            {
                dcYearMinus1 = Convert.ToDecimal(lblYearMinus1.Text.Trim());
                dcYearMinus1Total += dcYearMinus1;
                lblYearMinus1.Text = Convert.ToDecimal(lblYearMinus1.Text).ToString("#,0.00");
            }
            if (lblYearMinus2.Text != "")
            {
                dcYearMinus2 = Convert.ToDecimal(lblYearMinus2.Text.Trim());
                dcYearMinus2Total += dcYearMinus2;
                lblYearMinus2.Text = Convert.ToDecimal(lblYearMinus2.Text).ToString("#,0.00");
            }
            if (lblYearMinus3.Text != "")
            {
                dcYearMinus3 = Convert.ToDecimal(lblYearMinus3.Text.Trim());
                dcYearMinus3Total += dcYearMinus3;
                lblYearMinus3.Text = Convert.ToDecimal(lblYearMinus3.Text).ToString("#,0.00");
            }
            if (lblYearMinus4.Text != "")
            {
                dcYearMinus4 = Convert.ToDecimal(lblYearMinus4.Text.Trim());
                dcYearMinus4Total += dcYearMinus4;
                lblYearMinus4.Text = Convert.ToDecimal(lblYearMinus4.Text).ToString("#,0.00");
            }
            if (lblYearMinus5.Text != "")
            {
                dcYearMinus5 = Convert.ToDecimal(lblYearMinus5.Text.Trim());
                dcYearMinus5Total += dcYearMinus5;
                lblYearMinus5.Text = Convert.ToDecimal(lblYearMinus5.Text).ToString("#,0.00");
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblAmountTotal = (Label)e.Row.FindControl("lblAmountTotal");
            Label lblMarginAvgTotal = (Label)e.Row.FindControl("lblMarginAvgTotal");
            decimal dcWeightedAvg = 0;

            //100 * ((s.InvoiceValue - s.CostValue) /  (CASE WHEN s.InvoiceValue = 0 THEN 1 ELSE s.InvoiceValue END))
            dcWeightedAvg = (dcTotalAmount - dcCostValueTotal);
            if (dcTotalAmount == 0)
            {
                dcTotalAmount = 1;
            }
            dcWeightedAvg = dcWeightedAvg / dcTotalAmount;
            dcWeightedAvg = dcWeightedAvg * 100;
            lblMarginAvgTotal.Text = dcWeightedAvg.ToString("#,0.0") + "%";

            lblAmountTotal.Text = "$" + dcTotalAmount.ToString("#,0.00");

            lblPriceTotal.Text = "Price Total: " + lblAmountTotal.Text;
            lblMarginWeightedAvg.Text = " Margin Weighted Avg: " + lblMarginAvgTotal.Text;

            Label lblYTDTotal = (Label)e.Row.FindControl("lblYTDTotal");
            Label lblYearMinus1Total = (Label)e.Row.FindControl("lblYearMinus1Total");
            Label lblYearMinus2Total = (Label)e.Row.FindControl("lblYearMinus2Total");
            Label lblYearMinus3Total = (Label)e.Row.FindControl("lblYearMinus3Total");
            Label lblYearMinus4Total = (Label)e.Row.FindControl("lblYearMinus4Total");
            Label lblYearMinus5Total = (Label)e.Row.FindControl("lblYearMinus5Total");

            lblYTDTotal.Text = "$" + dcYTDTotal.ToString("#,0.00");
            lblYearMinus1Total.Text = "$" + dcYearMinus1Total.ToString("#,0.00");
            lblYearMinus2Total.Text = "$" + dcYearMinus2Total.ToString("#,0.00");
            lblYearMinus3Total.Text = "$" + dcYearMinus3Total.ToString("#,0.00");
            lblYearMinus4Total.Text = "$" + dcYearMinus4Total.ToString("#,0.00");
            lblYearMinus5Total.Text = "$" + dcYearMinus5Total.ToString("#,0.00");
        }

    }
    protected void gvReportCondensed_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();

        DataTable dt = (DataTable)Session["dtCustomerReport"];
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
    protected void gvCustomerSummary_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        decimal dcAmount = 0;//InvoiceValue...
        decimal dcYTD = 0;
        decimal dcYearMinus1 = 0;
        decimal dcYearMinus2 = 0;
        decimal dcYearMinus3 = 0;
        decimal dcYearMinus4 = 0;
        decimal dcYearMinus5 = 0;
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[6].Text = DateTime.Now.AddYears(-1).Year.ToString();
                e.Row.Cells[7].Text = DateTime.Now.AddYears(-2).Year.ToString();
                e.Row.Cells[8].Text = DateTime.Now.AddYears(-3).Year.ToString();
                e.Row.Cells[9].Text = DateTime.Now.AddYears(-4).Year.ToString();
                e.Row.Cells[10].Text = DateTime.Now.AddYears(-5).Year.ToString();
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblAmount = (Label)e.Row.FindControl("lblAmount");
                Label lblMargin = (Label)e.Row.FindControl("lblMargin");
                Label lblYTD = (Label)e.Row.FindControl("lblYTD");
                Label lblYearMinus1 = (Label)e.Row.FindControl("lblYearMinus1");
                Label lblYearMinus2 = (Label)e.Row.FindControl("lblYearMinus2");
                Label lblYearMinus3 = (Label)e.Row.FindControl("lblYearMinus3");
                Label lblYearMinus4 = (Label)e.Row.FindControl("lblYearMinus4");
                Label lblYearMinus5 = (Label)e.Row.FindControl("lblYearMinus5");


                if (lblMargin.Text != "")
                {
                    lblMargin.Text = Convert.ToDecimal(lblMargin.Text).ToString("0.0");
                }
                if (lblAmount.Text != "")
                {
                    dcAmount = Convert.ToDecimal(lblAmount.Text.Trim());
                    dcTotalAmountSummary += dcAmount;
                    lblAmount.Text = Convert.ToDecimal(lblAmount.Text.Trim()).ToString("#,0.00");
                }

                if (lblYTD.Text != "")
                {
                    dcYTD = Convert.ToDecimal(lblYTD.Text.Trim());
                    dcYTDTotalSummary += dcYTD;
                    lblYTD.Text = Convert.ToDecimal(lblYTD.Text).ToString("#,0.00");
                }
                if (lblYearMinus1.Text != "")
                {
                    dcYearMinus1 = Convert.ToDecimal(lblYearMinus1.Text.Trim());
                    dcYearMinus1TotalSummary += dcYearMinus1;
                    lblYearMinus1.Text = Convert.ToDecimal(lblYearMinus1.Text).ToString("#,0.00");
                }
                if (lblYearMinus2.Text != "")
                {
                    dcYearMinus2 = Convert.ToDecimal(lblYearMinus2.Text.Trim());
                    dcYearMinus2TotalSummary += dcYearMinus2;
                    lblYearMinus2.Text = Convert.ToDecimal(lblYearMinus2.Text).ToString("#,0.00");
                }
                if (lblYearMinus3.Text != "")
                {
                    dcYearMinus3 = Convert.ToDecimal(lblYearMinus3.Text.Trim());
                    dcYearMinus3TotalSummary += dcYearMinus3;
                    lblYearMinus3.Text = Convert.ToDecimal(lblYearMinus3.Text).ToString("#,0.00");
                }
                if (lblYearMinus4.Text != "")
                {
                    dcYearMinus4 = Convert.ToDecimal(lblYearMinus4.Text.Trim());
                    dcYearMinus4TotalSummary += dcYearMinus4;
                    lblYearMinus4.Text = Convert.ToDecimal(lblYearMinus4.Text).ToString("#,0.00");
                }
                if (lblYearMinus5.Text != "")
                {
                    dcYearMinus5 = Convert.ToDecimal(lblYearMinus5.Text.Trim());
                    dcYearMinus5TotalSummary += dcYearMinus5;
                    lblYearMinus5.Text = Convert.ToDecimal(lblYearMinus5.Text).ToString("#,0.00");
                }


            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblAmountTotal = (Label)e.Row.FindControl("lblAmountTotal");
                Label lblMarginAvgTotal = (Label)e.Row.FindControl("lblMarginAvgTotal");

                Label lblYTDTotal = (Label)e.Row.FindControl("lblYTDTotal");
                Label lblYearMinus1Total = (Label)e.Row.FindControl("lblYearMinus1Total");
                Label lblYearMinus2Total = (Label)e.Row.FindControl("lblYearMinus2Total");
                Label lblYearMinus3Total = (Label)e.Row.FindControl("lblYearMinus3Total");
                Label lblYearMinus4Total = (Label)e.Row.FindControl("lblYearMinus4Total");
                Label lblYearMinus5Total = (Label)e.Row.FindControl("lblYearMinus5Total");
                // decimal dcWeightedAvgSummary = 0;

                //100 * ((s.InvoiceValue - s.CostValue) /  (CASE WHEN s.InvoiceValue = 0 THEN 1 ELSE s.InvoiceValue END))
                //dcWeightedAvgSummary = (dcTotalAmountSummary - dcCostValueTotalSummary);
                //if (dcTotalAmountSummary == 0)
                //{
                //    dcTotalAmountSummary = 1;
                //}
                //dcWeightedAvgSummary = dcWeightedAvgSummary / dcTotalAmountSummary;
                //dcWeightedAvgSummary = dcWeightedAvgSummary * 100;
                //lblMarginAvgTotal.Text = dcWeightedAvgSummary.ToString("#,0.0") + "%";

                lblAmountTotal.Text = "$" + dcTotalAmountSummary.ToString("#,0.00");
                lblYTDTotal.Text = "$" + dcYTDTotalSummary.ToString("#,0.00");
                lblYearMinus1Total.Text = "$" + dcYearMinus1TotalSummary.ToString("#,0.00");
                lblYearMinus2Total.Text = "$" + dcYearMinus2TotalSummary.ToString("#,0.00");
                lblYearMinus3Total.Text = "$" + dcYearMinus3TotalSummary.ToString("#,0.00");
                lblYearMinus4Total.Text = "$" + dcYearMinus4TotalSummary.ToString("#,0.00");
                lblYearMinus5Total.Text = "$" + dcYearMinus5TotalSummary.ToString("#,0.00");



                lblPriceTotal.Text = "Price Total: " + lblAmountTotal.Text;
                // lblMarginWeightedAvg.Text = " Margin Weighted Avg: " + lblMarginAvgTotal.Text;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvCustomerSummary_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();

        DataTable dt = (DataTable)Session["dtCustomerReportSummary"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvCustomerSummary.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvCustomerSummary.DataSource = m_DataView;
            gvCustomerSummary.DataBind();
            gvCustomerSummary.PageIndex = m_PageIndex;
            Session["dtPropSortSummary"] = m_DataTable;
        }
        dtSortTable.Dispose();
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblPriceTotal.Text = "";
        lblMarginWeightedAvg.Text = "";
        RunReport();
        gvCustomerSummary.DataSource = null;
        gvCustomerSummary.DataBind();
        trFullReport.Visible = true;
        trSummary.Visible = false;
        GetTrendsReport();
    }
    protected void btnSummary_Click(object sender, EventArgs e)
    {
        RunReportSummary();
        gvReportCondensed.DataSource = null;
        gvReportCondensed.DataBind();
        trFullReport.Visible = false;
        trSummary.Visible = true;


    }
    protected void btnPreview0_Click(object sender, EventArgs e)
    {
        lblErrorPrior.Text = "";
        //RunReport2();
        //RunReport();
    }
    protected void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCustomers0.SelectedIndex = ddlCustomers.SelectedIndex;
    }
    //protected void gvDatePriorToPriceChange_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        Label lblLastPriceIncreaseDate = (Label)e.Row.FindControl("lblLastPriceIncreaseDate");
    //        if (lblLastPriceIncreaseDate.Text != "")
    //        {
    //            lblLastPriceIncreaseDate.Text = Convert.ToDateTime(lblLastPriceIncreaseDate.Text).ToShortDateString();
    //        }
    //        Label lblUnitCost = (Label)e.Row.FindControl("lblUnitCost");
    //        if (lblLastPriceIncreaseDate.Text != "")
    //        {
    //            lblUnitCost.Text = Convert.ToDecimal(lblUnitCost.Text).ToString("#,0.00");
    //        }
    //    }

    //}
    //protected void gvDatePriorToPriceChange_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    string m_SortDirection = "";
    //    DataTable dtSortTable = new DataTable();

    //    DataTable dt = (DataTable)Session["dtDatePriorToPriceChange"];
    //    dtSortTable = dt;
    //    DataTable m_DataTable = dtSortTable;
    //    if (m_DataTable != null)
    //    {
    //        int m_PageIndex = gvDatePriorToPriceChange.PageIndex;
    //        m_SortDirection = GetSortDirection();
    //        DataView m_DataView = new DataView(m_DataTable);
    //        m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
    //        gvDatePriorToPriceChange.DataSource = m_DataView;
    //        gvDatePriorToPriceChange.DataBind();
    //        gvDatePriorToPriceChange.PageIndex = m_PageIndex;
    //        Session["dtPropSort"] = m_DataTable;
    //    }
    //    dtSortTable.Dispose();
    //}
    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {

        //txtStartDateChart2.Text = txtStartDate.Text;
    }
    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {

        // txtEndDateChart2.Text = txtEndDate.Text;
    }
    protected void rblSort_SelectedIndexChanged(object sender, EventArgs e)
    {

        LoadCustomer(ddlCustomers, rblSort.SelectedValue);
        LoadCustomer(ddlCustomers0, rblSort.SelectedValue);
        //LoadCustomer(ddlCustomersChart2, rblSort.SelectedValue);
        if (rblSort.SelectedIndex == 1)
        {
            rblSort0.SelectedIndex = 1;
            // rblSortChart2.SelectedIndex = 1;
        }
        else
        {
            rblSort0.SelectedIndex = 0;
            //rblSortChart2.SelectedIndex = 0;
        }

    }

    //protected void rblSortChart2_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    LoadCustomer(ddlCustomersChart1, rblSortChart2.SelectedValue);
    //    LoadCustomer(ddlCustomersChart2, rblSortChart2.SelectedValue);
    //    if (rblSortChart2.SelectedIndex == 1)
    //    {
    //        rblSortChart1.SelectedIndex = 1;
    //        rblSortChart2.SelectedIndex = 1;
    //    }
    //    else
    //    {
    //        rblSortChart1.SelectedIndex = 0;
    //        rblSortChart2.SelectedIndex = 0;
    //    }
    //}
    //FrozenHeaderRow...
    // LinkButtons are used to dynamically create the links necessary
    // for paging.
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
    //protected void HeaderLink2_Click(object sender, System.EventArgs e)
    //{
    //    LinkButton lnkHeader = (LinkButton)sender;
    //    SortDirection direction = SortDirection.Ascending;

    //    // the CommandArgument of each linkbutton contains the sortexpression
    //    // for the column that was clicked.
    //    if (gvDatePriorToPriceChange.SortExpression == lnkHeader.CommandArgument)
    //    {
    //        if (gvDatePriorToPriceChange.SortDirection == SortDirection.Ascending)
    //        {
    //            direction = SortDirection.Descending;
    //        }

    //    }

    //    gvDatePriorToPriceChange.Sort(lnkHeader.CommandArgument, direction);
    //}
    protected void lbnSelectAllStockCode0_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbStockCode0.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearStockCode0_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbStockCode0.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }

    protected void rblSort0_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void imgExportExcel1_Click(object sender, ImageClickEventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";
        if (Session["dtCustomerReport"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtCustomerReport"];

        dt.TableName = "dtCustomerReport";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "CustomerReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void imgExportExcel2_Click(object sender, ImageClickEventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";
        if (Session["dtDatePriorToPriceChange"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtDatePriorToPriceChange"];

        dt.TableName = "dtDatePriorToPriceChange";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "PriceChangesReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
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
                LoadStockCodesInvMaster();
            }
        }

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

    // create the table for the fixed header in Init
    protected void Page_Init(object sender, EventArgs e)
    {
        SetupHeaderTable1();
        //SetupHeaderTable2();
    }
    protected void lbnTrendsReport_Click(object sender, EventArgs e)
    {

    }
    protected void lbnTrendsReportByQty_Click(object sender, EventArgs e)
    {

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {

        txtStartDate.Text = "";
        txtEndDate.Text = "";
        txtMarginFrom.Text = "";
        txtMarginTo.Text = "";

        txtStockCodeChartIngredientComponent.Text = "";
        ddlProductClass.SelectedIndex = 0;
        ddlSalesPerson.SelectedIndex = 0;
        ddlPeriod.SelectedIndex = 0;

        ddlYear.SelectedIndex = 0;
        rblIngredientComponent.SelectedIndex = 0;
        lbParentStockCode.Items.Clear();
        ddlCustomers.SelectedIndex = 0;
        lbParentStockCode.Text = "";
        lblStockCodeList.Text = "";
        lblStockCodeDescIngredientComponent.Text = "";
        PanelSetup();
    }
    protected void rblSingleOrStockCodeRange_SelectedIndexChanged(object sender, EventArgs e)
    {
        PanelSetup();
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

    protected void rblIngredientComponent_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblIngredientComponent.SelectedIndex == 0)
        {
            txtStockCodeChartIngredientComponent.Attributes.Add("placeholder", "Enter Ingredient Stock Code");
        }
        else
        {
            txtStockCodeChartIngredientComponent.Attributes.Add("placeholder", "Enter Component/Pkg Stock Code");
        }
    }


}