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
using System.Text;

public partial class CustomerReportsNew : System.Web.UI.Page
{
    decimal dcCostValueTotal = 0;
    decimal dcTotalAmount = 0;



    //Full Report...
    decimal dcYTDTotal = 0;
    decimal dcYearMinus1Total = 0;
    decimal dcYearMinus2Total = 0;
    decimal dcYearMinus3Total = 0;
    decimal dcYearMinus4Total = 0;
    decimal dcYearMinus5Total = 0;
    //Summary...
    decimal dcTotalAmountSummary = 0;
    decimal dcYTDTotalSummary = 0;
    decimal dcPriorYTDTotalSummary = 0;
    decimal dcTotalAmountPriorSummary = 0;
    decimal dcYearMinus1TotalSummary = 0;
    decimal dcYearMinus2TotalSummary = 0;
    decimal dcYearMinus3TotalSummary = 0;
    decimal dcYearMinus4TotalSummary = 0;
    decimal dcYearMinus5TotalSummary = 0;


    decimal dcCostRangeSummary = 0;
    decimal dcYTDCostSummary = 0;
    decimal dcTotalCostPriorSummary = 0;
    decimal dcYearMinus1CostSummary = 0;
    decimal dcYearMinus2CostSummary = 0;
    decimal dcYearMinus3CostSummary = 0;
    decimal dcYearMinus4CostSummary = 0;
    decimal dcYearMinus5CostSummary = 0;

    decimal dcMarginRangeSummaryAvg = 0;
    decimal dcYTDMarginSummaryAvg = 0;
    decimal dcTotalMarginPriorSummaryAvg = 0;
    decimal dcYearMinus1MarginSummaryAvg = 0;
    decimal dcYearMinus2MarginSummaryAvg = 0;
    decimal dcYearMinus3MarginSummaryAvg = 0;
    decimal dcYearMinus4MarginSummaryAvg = 0;
    decimal dcYearMinus5MarginSummaryAvg = 0;

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
    private string Mode
    {
        get
        {
            if (ViewState["Mode"] != null)
            {
                return ViewState["Mode"].ToString();
            }
            else
            {
                return "";
            }
        }
        set
        {
            ViewState["Mode"] = value;
        }
    }

    #region Subs

    private void LoadStockCodesOfEndCustomer(int iNonCustomerID)
    {
        List<StockCodeDescriptionList.StockCodeDescription> lStockCodeDescListReport = new List<StockCodeDescriptionList.StockCodeDescription>();
        lStockCodeDescListReport = GetNonCustomerStockCodesDescs(iNonCustomerID);
        lbParentStockCodeEndCustomer.Items.Clear();
        foreach (StockCodeDescriptionList.StockCodeDescription a in lStockCodeDescListReport)
        {
            lbParentStockCodeEndCustomer.Items.Add(new ListItem(a.StockCode + " - " + a.Desc, a.StockCode));
        }
        lblStockCodeList.Text = "End Customer Stock Codes: " + lbParentStockCodeEndCustomer.Items.Count.ToString();

        foreach (ListItem li in lbParentStockCodeEndCustomer.Items)
        {
            li.Selected = true;
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
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddl.Items.Clear();

            if (sSortBy == "Name")
            {
                var query = ((
                        from ar in db.ArCustomer
                        where
                          ar.Customer != "test" &&
                          ar.Customer != "9999999" &&
                          Convert.ToInt32(ar.Customer) != 1
                        select new
                        {
                            ar.Customer,
                            ar.Name
                        }
                    ).Union
                    (
                        from an in db.ArNonCustomer
                        select new
                        {
                            Customer = Convert.ToString(an.NonCustomerID),
                            Name = an.Name
                        }
                    ).Union
                    (
                        from ag in db.ArGroups
                        select new
                        {
                            Customer = Convert.ToString(ag.GroupID),
                            Name = ag.GroupName
                        }
                    )
                    .OrderBy(p => p.Name));
                foreach (var a in query)
                {
                    ddl.Items.Add(new ListItem(a.Name + " - " + a.Customer, a.Customer));
                }
                ddl.Items.Insert(0, new ListItem("All Customers and or End Customers/Groups", "All Customers and or End Customers/Groups"));

            }
            else
            {
                var query = ((
                            from ar in db.ArCustomer
                            where
                              ar.Customer != "test" &&
                              ar.Customer != "9999999" &&
                              Convert.ToInt32(ar.Customer) != 1
                            select new
                            {
                                ar.Customer,
                                ar.Name
                            }
                        ).Union
                        (
                            from an in db.ArNonCustomer
                            select new
                            {
                                Customer = Convert.ToString(an.NonCustomerID),
                                Name = an.Name
                            }
                        ).Union
                        (
                            from ag in db.ArGroups
                            select new
                            {
                                Customer = Convert.ToString(ag.GroupID),
                                Name = ag.GroupName
                            }
                        )
                        .OrderBy(p => p.Customer));
                foreach (var a in query)
                {
                    ddl.Items.Add(new ListItem(a.Customer + " - " + a.Name, a.Customer));
                }
                ddl.Items.Insert(0, new ListItem("All Customers and or End Customers/Groups", "All Customers and or End Customers/Groups"));
            }

            rsListbox.Sort(ref lbCustomers, rsListbox.SortOrder.Ascending);
            lbCustomers.SelectedIndex = -1;
        }
    }
    private void LoadNonCustomers(DropDownList ddl)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddl.Items.Clear();

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
    }
    private void LoadProductClass(DropDownList ddl)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddl.Items.Clear();


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
    }
    private void LoadCustomerCities(ListBox ddl)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddl.Items.Clear();
            var query = (from ac in db.ArCustomer
                         where
                           !ac.SoldToAddr4.Contains("0") &&
                           !ac.SoldToAddr4.Contains("1") &&
                           !ac.SoldToAddr4.Contains("2") &&
                           !ac.SoldToAddr4.Contains("3") &&
                           !ac.SoldToAddr4.Contains("4") &&
                           !ac.SoldToAddr4.Contains("5") &&
                           !ac.SoldToAddr4.Contains("6") &&
                           !ac.SoldToAddr4.Contains("7") &&
                           !ac.SoldToAddr4.Contains("8") &&
                           !ac.SoldToAddr4.Contains("9") &&
                           !ac.SoldToAddr4.Contains("@") &&
                           !ac.SoldToAddr4.Contains(",") &&
                           !ac.SoldToAddr4.Contains(".") &&
                           !ac.SoldToAddr4.Contains(":") &&
                           !(new string[] { "AZ", "CA", "NC", "NM", "NV", "SAN" }).Contains(ac.SoldToAddr4.Trim()) &&
                           ac.SoldToAddr4.Trim() != ""
                         group ac by new
                         {
                             ac.SoldToAddr4
                         } into g
                         orderby
                           g.Key.SoldToAddr4
                         select new
                         {
                             City = g.Key.SoldToAddr4.Trim().ToUpper()
                         });
            foreach (var a in query)
            {
                ddl.Items.Add(new ListItem(a.City, a.City));
            }
        }
    }
    private void LoadCustomerZips(ListBox ddl)
    {

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddl.Items.Clear();
            var query = (from ac in db.ArCustomer
                         where
                           ac.SoldPostalCode != "" &&
                           (int?)ac.SoldPostalCode.Length == 5
                         group ac by new
                         {
                             ac.SoldPostalCode
                         } into g
                         orderby
                           g.Key.SoldPostalCode
                         select new
                         {
                             g.Key.SoldPostalCode
                         });
            foreach (var a in query)
            {
                ddl.Items.Add(new ListItem(a.SoldPostalCode, a.SoldPostalCode));
            }
        }
    }
    private void LoadSalesPersons()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddlSalesPerson.Items.Clear();


            var query = (from c in db.SalSalesperson
                         orderby c.Name
                         select new
                         {
                             c.Name,
                             c.Salesperson

                         });
            foreach (var a in query)
            {
                ddlSalesPerson.Items.Add(new ListItem(a.Name.ToUpper() + " - " + a.Salesperson, a.Salesperson));
            }

            rsListbox.Sort(ref ddlSalesPerson, rsListbox.SortOrder.Ascending);
            ddlSalesPerson.Items.Insert(0, new ListItem("All", "All"));
        }
    }
    private void LoadCustomerForCharts(DropDownList ddl)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddl.Items.Clear();

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
    }
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

    private void LoadStockCodes(ListBox lb)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
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
    }
    private void PanelSetup()
    {
        switch (Mode)
        {
            case "Range"://Stock Code Range

                lblStockCodeList.Text = "";
                break;
            case "EndCustomer"://End Customer   

                lbParentStockCodeEndCustomer.Items.Clear();
                lbParentStockCodeEndCustomer.Items.Add(new ListItem("First Select an End Customer", "0"));
                lblStockCodeList.Text = "";
                break;

        }
    }

    private void GetTrendsReports()
    {
        string sMsg = "";
        string sStartDate = txtStartDate.Text.Trim();
        string sEndDate = txtEndDate.Text.Trim();

        string sStockCodeFrom = "";
        string sStockCodeTo = "";

        sStockCodeFrom = txtStockCodeFrom.Text.Trim();
        sStockCodeTo = txtStockCodeTo.Text.Trim();

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


        if (sStockCodeFrom == "")
        {
            sMsg += "**Stock Code From is Required to run Trends Reports!<br/>";
        }
        if (sStockCodeTo == "")
        {
            sMsg += "**Stock Code To is Required to run Trends Reports!<br/>";
        }
        if (sStartDate == "")
        {
            sMsg += "**Start Date is Required to run Trends Reports!<br/>";
        }
        if (sEndDate == "")
        {
            sMsg += "**End Date is Required to run Trends Reports!<br/>";
        }

        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }




        string sCustomer = "";
        string sEndCustomer = "";
        if (lbCustomers.SelectedIndex != -1)
        {
            sCustomer = lbCustomers.SelectedValue;
        }
        else
        {
            sCustomer = "";
        }


        if (Mode == "EndCustomer")//End Customer...
        {
            if (ddlEndCustomers.SelectedIndex != 0)
            {
                sEndCustomer = ddlEndCustomers.SelectedValue;
            }
            else
            {
                sEndCustomer = "";
            }

        }


        RunTrendsReportDollars(sStartDate, sEndDate, sStockCodeFrom, sStockCodeTo, sCustomer, sEndCustomer);
        RunTrendsReportQuantity(sStartDate, sEndDate, sStockCodeFrom, sStockCodeTo, sCustomer, sEndCustomer);



    }
    private void GetTrendsReportsSummary()
    {
        string sMsg = "";
        string sStartDate = txtStartDate.Text.Trim();
        string sEndDate = txtEndDate.Text.Trim();

        string sStockCodeFrom = "";
        string sStockCodeTo = "";

        sStockCodeFrom = txtStockCodeFrom.Text.Trim();
        sStockCodeTo = txtStockCodeTo.Text.Trim();

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


        if (sStockCodeFrom == "")
        {
            sMsg += "**Stock Code From is Required to run Trends Reports!<br/>";
        }
        if (sStockCodeTo == "")
        {
            sMsg += "**Stock Code To is Required to run Trends Reports!<br/>";
        }
        if (sStartDate == "")
        {
            sMsg += "**Start Date is Required to run Trends Reports!<br/>";
        }
        if (sEndDate == "")
        {
            sMsg += "**End Date is Required to run Trends Reports!<br/>";
        }

        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }




        string sCustomer = "";
        string sEndCustomer = "";
        if (lbCustomers.SelectedIndex != -1)
        {
            sCustomer = lbCustomers.SelectedValue;
        }
        else
        {
            sCustomer = "";
        }


        if (Mode == "EndCustomer")//End Customer...
        {
            if (ddlEndCustomers.SelectedIndex != 0)
            {
                sEndCustomer = ddlEndCustomers.SelectedValue;
            }
            else
            {
                sEndCustomer = "";
            }

        }


        RunTrendsReportDollarsSummary(sStartDate, sEndDate, sStockCodeFrom, sStockCodeTo, sCustomer, sEndCustomer);
        RunTrendsReportQuantitySummary(sStartDate, sEndDate, sStockCodeFrom, sStockCodeTo, sCustomer, sEndCustomer);



    }
    private void RunTrendsReportDollars(string sStartDate, string sEndDate, string sStockCodeFrom, string sStockCodeTo, string sCustomer, string sEndCustomer)
    {//DOES NOT USE USERID!!!
        DataTable dt = new DataTable();
        string sSQL = "";
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            if (sEndCustomer == "")
            {
                if (sCustomer != "")//With Customer...
                {
                    sSQL = "EXEC spGetTrendsReport ";
                    sSQL += "@FromDate='" + sStartDate + "',";
                    sSQL += "@ToDate='" + sEndDate + "',";
                    sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                    sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                    sSQL += "@Customer='" + sCustomer + "'";
                }

                else if (sCustomer == "")//StockCode Range Only...
                {
                    sSQL = "EXEC spGetTrendsReport ";
                    sSQL += "@FromDate='" + sStartDate + "',";
                    sSQL += "@ToDate='" + sEndDate + "',";
                    sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                    sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                    sSQL += "@Customer=''";
                }
                else
                {
                    //Should Never be here...
                }
            }
            else//End Customer...
            {
                sSQL = "EXEC spGetTrendsReport ";
                sSQL += "@FromDate='" + sStartDate + "',";
                sSQL += "@ToDate='" + sEndDate + "',";
                sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                sSQL += "@EndCustomer='" + sEndCustomer + "'";
            }
            Debug.WriteLine(sSQL);

            dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
            try
            {
                if (dt.Rows.Count > 0)
                {
                    Session["dtTrendsReportDollars"] = dt;
                }
            }
            catch (Exception)
            {
                lblError.Text = "**There was to data at the end of the range to run Trends Dollar Report, try an end that is older!";
                lblError.ForeColor = Color.Red;
            }

        }
    }
    private void RunTrendsReportDollarsSummary(string sStartDate, string sEndDate, string sStockCodeFrom, string sStockCodeTo, string sCustomer, string sEndCustomer)
    {//DOES NOT USE USERID!!!
        DataTable dt = new DataTable();
        string sSQL = "";
        using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            if (sEndCustomer == "")
            {
                if (sCustomer != "")//With Customer...
                {

                    if (chkIncludeEndCustomerInSummary.Checked && chkIncludeGroupingInSummary.Checked)
                    {//Both Checked...
                        sSQL = "EXEC spGetTrendsReportSummary2020 ";
                        sSQL += "@FromDate='" + sStartDate + "',";
                        sSQL += "@ToDate='" + sEndDate + "',";
                        sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                        sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                        sSQL += "@Customer='" + sCustomer + "',";
                        sSQL += "@IncludeEndCustomer=1,";
                        sSQL += "@IncludeGrouping=1";
                    }
                    if (chkIncludeEndCustomerInSummary.Checked && !chkIncludeGroupingInSummary.Checked)
                    {//End Customer Checked...
                        sSQL = "EXEC spGetTrendsReportSummary2020 ";
                        sSQL += "@FromDate='" + sStartDate + "',";
                        sSQL += "@ToDate='" + sEndDate + "',";
                        sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                        sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                        sSQL += "@Customer='" + sCustomer + "',";
                        sSQL += "@IncludeEndCustomer=1,";
                        sSQL += "@IncludeGrouping=0";
                    }
                    if (!chkIncludeEndCustomerInSummary.Checked && chkIncludeGroupingInSummary.Checked)
                    {//Grouping Checked...
                        sSQL = "EXEC spGetTrendsReportSummary2020 ";
                        sSQL += "@FromDate='" + sStartDate + "',";
                        sSQL += "@ToDate='" + sEndDate + "',";
                        sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                        sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                        sSQL += "@Customer='" + sCustomer + "',";
                        sSQL += "@IncludeEndCustomer=0,";
                        sSQL += "@IncludeGrouping=1";
                    }
                    if (!chkIncludeEndCustomerInSummary.Checked && !chkIncludeGroupingInSummary.Checked)
                    {//Both UnChecked...
                        sSQL = "EXEC spGetTrendsReportSummary2020 ";
                        sSQL += "@FromDate='" + sStartDate + "',";
                        sSQL += "@ToDate='" + sEndDate + "',";
                        sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                        sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                        sSQL += "@Customer='" + sCustomer + "',";
                        sSQL += "@IncludeEndCustomer=0,";
                        sSQL += "@IncludeGrouping=0";
                    }
                    else
                    {

                    }
                }
                else if (sCustomer == "")//StockCode Range Only...
                {
                    if (chkIncludeEndCustomerInSummary.Checked && chkIncludeGroupingInSummary.Checked)
                    {//Both Checked...
                        sSQL = "EXEC spGetTrendsReportSummary2020 ";
                        sSQL += "@FromDate='" + sStartDate + "',";
                        sSQL += "@ToDate='" + sEndDate + "',";
                        sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                        sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                        sSQL += "@Customer='',";
                        sSQL += "@IncludeEndCustomer=1,";
                        sSQL += "@IncludeGrouping=1";
                    }
                    if (chkIncludeEndCustomerInSummary.Checked && !chkIncludeGroupingInSummary.Checked)
                    {//End Customer Checked...
                        sSQL = "EXEC spGetTrendsReportSummary2020 ";
                        sSQL += "@FromDate='" + sStartDate + "',";
                        sSQL += "@ToDate='" + sEndDate + "',";
                        sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                        sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                        sSQL += "@Customer='',";
                        sSQL += "@IncludeEndCustomer=1,";
                        sSQL += "@IncludeGrouping=0";
                    }
                    if (!chkIncludeEndCustomerInSummary.Checked && chkIncludeGroupingInSummary.Checked)
                    {//Grouping Checked...
                        sSQL = "EXEC spGetTrendsReportSummary2020 ";
                        sSQL += "@FromDate='" + sStartDate + "',";
                        sSQL += "@ToDate='" + sEndDate + "',";
                        sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                        sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                        sSQL += "@Customer='',";
                        sSQL += "@IncludeEndCustomer=0,";
                        sSQL += "@IncludeGrouping=1";
                    }
                    if (!chkIncludeEndCustomerInSummary.Checked && !chkIncludeGroupingInSummary.Checked)
                    {//Both UnChecked...
                        sSQL = "EXEC spGetTrendsReportSummary2020 ";
                        sSQL += "@FromDate='" + sStartDate + "',";
                        sSQL += "@ToDate='" + sEndDate + "',";
                        sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                        sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                        sSQL += "@Customer='',";
                        sSQL += "@IncludeEndCustomer=0,";
                        sSQL += "@IncludeGrouping=0";
                    }
                    else
                    {

                    }
                }
                else
                {
                    //Should Never be here...
                }
            }
            else//End Customer...
            {
                sSQL = "EXEC spGetTrendsReportSummary2020 ";
                sSQL += "@FromDate='" + sStartDate + "',";
                sSQL += "@ToDate='" + sEndDate + "',";
                sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                sSQL += "@EndCustomer='" + sEndCustomer + "'";
            }
            Debug.WriteLine(sSQL);

            dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
            try
            {
                if (dt.Rows.Count > 0)
                {
                    Session["dtTrendsReportDollarsSummary"] = dt;
                }
            }
            catch (Exception)
            {
                lblError.Text = "**There was to data at the end of the range to run Trends Dollar Report Summary, try an end that is older!";
                lblError.ForeColor = Color.Red;
            }

        }
    }
    private void RunTrendsReportQuantity(string sStartDate, string sEndDate, string sStockCodeFrom, string sStockCodeTo, string sCustomer, string sEndCustomer)
    {//DOES NOT USE USERID!!!
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        if (sEndCustomer == "")
        {
            if (sCustomer != "")//Customer...
            {
                sSQL = "EXEC spGetTrendsReportByQuantity ";
                sSQL += "@FromDate='" + sStartDate + "',";
                sSQL += "@ToDate='" + sEndDate + "',";
                sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                sSQL += "@Customer='" + sCustomer + "'";
            }
            else if (sCustomer == "")//StockCode Range Only...
            {
                sSQL = "EXEC spGetTrendsReportByQuantity ";
                sSQL += "@FromDate='" + sStartDate + "',";
                sSQL += "@ToDate='" + sEndDate + "',";
                sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                sSQL += "@Customer=''";
            }
            else
            {
                //Should Never be here...
            }
        }
        else//End Customer...
        {
            sSQL = "EXEC spGetTrendsReportByQuantity ";
            sSQL += "@FromDate='" + sStartDate + "',";
            sSQL += "@ToDate='" + sEndDate + "',";
            sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
            sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
            sSQL += "@EndCustomer='" + sEndCustomer + "'";
        }
        Debug.WriteLine(sSQL);
        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
        try
        {
            if (dt.Rows.Count > 0)
            {
                Session["dtTrendsReportQuantity"] = dt;
            }
        }
        catch (Exception)
        {
            lblError.Text = "**There was to data at the end of the range to run Trends Quantity Report, try an end that is older!";
            lblError.ForeColor = Color.Red;
        }


    }
    private void RunTrendsReportQuantitySummary(string sStartDate, string sEndDate, string sStockCodeFrom, string sStockCodeTo, string sCustomer, string sEndCustomer)
    {//DOES NOT USE USERID!!!
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        if (sEndCustomer == "")
        {
            if (sCustomer != "")//With Customer...
            {

                if (chkIncludeEndCustomerInSummary.Checked && chkIncludeGroupingInSummary.Checked)
                {//Both Checked...
                    sSQL = "EXEC spGetTrendsReportByQuantitySummary2020 ";
                    sSQL += "@FromDate='" + sStartDate + "',";
                    sSQL += "@ToDate='" + sEndDate + "',";
                    sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                    sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                    sSQL += "@Customer='" + sCustomer + "',";
                    sSQL += "@IncludeEndCustomer=1,";
                    sSQL += "@IncludeGrouping=1";
                }
                if (chkIncludeEndCustomerInSummary.Checked && !chkIncludeGroupingInSummary.Checked)
                {//End Customer Checked...
                    sSQL = "EXEC spGetTrendsReportByQuantitySummary2020 ";
                    sSQL += "@FromDate='" + sStartDate + "',";
                    sSQL += "@ToDate='" + sEndDate + "',";
                    sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                    sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                    sSQL += "@Customer='" + sCustomer + "',";
                    sSQL += "@IncludeEndCustomer=1,";
                    sSQL += "@IncludeGrouping=0";
                }
                if (!chkIncludeEndCustomerInSummary.Checked && chkIncludeGroupingInSummary.Checked)
                {//Grouping Checked...
                    sSQL = "EXEC spGetTrendsReportByQuantitySummary2020 ";
                    sSQL += "@FromDate='" + sStartDate + "',";
                    sSQL += "@ToDate='" + sEndDate + "',";
                    sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                    sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                    sSQL += "@Customer='" + sCustomer + "',";
                    sSQL += "@IncludeEndCustomer=0,";
                    sSQL += "@IncludeGrouping=1";
                }
                if (!chkIncludeEndCustomerInSummary.Checked && !chkIncludeGroupingInSummary.Checked)
                {//Both UnChecked...
                    sSQL = "EXEC spGetTrendsReportByQuantitySummary2020 ";
                    sSQL += "@FromDate='" + sStartDate + "',";
                    sSQL += "@ToDate='" + sEndDate + "',";
                    sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                    sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                    sSQL += "@Customer='" + sCustomer + "',";
                    sSQL += "@IncludeEndCustomer=0,";
                    sSQL += "@IncludeGrouping=0";
                }
                else
                {

                }
            }
            else if (sCustomer == "")//StockCode Range Only...
            {
                if (chkIncludeEndCustomerInSummary.Checked && chkIncludeGroupingInSummary.Checked)
                {//Both Checked...
                    sSQL = "EXEC spGetTrendsReportByQuantitySummary2020 ";
                    sSQL += "@FromDate='" + sStartDate + "',";
                    sSQL += "@ToDate='" + sEndDate + "',";
                    sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                    sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                    sSQL += "@Customer='',";
                    sSQL += "@IncludeEndCustomer=1,";
                    sSQL += "@IncludeGrouping=1";
                }
                if (chkIncludeEndCustomerInSummary.Checked && !chkIncludeGroupingInSummary.Checked)
                {//End Customer Checked...
                    sSQL = "EXEC spGetTrendsReportByQuantitySummary2020 ";
                    sSQL += "@FromDate='" + sStartDate + "',";
                    sSQL += "@ToDate='" + sEndDate + "',";
                    sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                    sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                    sSQL += "@Customer='',";
                    sSQL += "@IncludeEndCustomer=1,";
                    sSQL += "@IncludeGrouping=0";
                }
                if (!chkIncludeEndCustomerInSummary.Checked && chkIncludeGroupingInSummary.Checked)
                {//Grouping Checked...
                    sSQL = "EXEC spGetTrendsReportByQuantitySummary2020 ";
                    sSQL += "@FromDate='" + sStartDate + "',";
                    sSQL += "@ToDate='" + sEndDate + "',";
                    sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                    sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                    sSQL += "@Customer='',";
                    sSQL += "@IncludeEndCustomer=0,";
                    sSQL += "@IncludeGrouping=1";
                }
                if (!chkIncludeEndCustomerInSummary.Checked && !chkIncludeGroupingInSummary.Checked)
                {//Both UnChecked...
                    sSQL = "EXEC spGetTrendsReportByQuantitySummary2020 ";
                    sSQL += "@FromDate='" + sStartDate + "',";
                    sSQL += "@ToDate='" + sEndDate + "',";
                    sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                    sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                    sSQL += "@Customer='',";
                    sSQL += "@IncludeEndCustomer=0,";
                    sSQL += "@IncludeGrouping=0";
                }
                else
                {

                }
            }
            else
            {
                //Should Never be here...
            }
        }
        else//End Customer...
        {
            sSQL = "EXEC spGetTrendsReportByQuantitySummary2020 ";
            sSQL += "@FromDate='" + sStartDate + "',";
            sSQL += "@ToDate='" + sEndDate + "',";
            sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
            sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
            sSQL += "@EndCustomer='" + sEndCustomer + "'";
        }
        Debug.WriteLine(sSQL);
        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
        try
        {
            if (dt.Rows.Count > 0)
            {
                Session["dtTrendsReportQuantitySummary"] = dt;
            }
        }
        catch (Exception)
        {
            lblError.Text = "**There was to data at the end of the range to run Trends Quantity Report Summary, try an end that is older!";
            lblError.ForeColor = Color.Red;
        }


    }
    private void ExportSummary()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";

        dt = RunReportSummary();
        dt.TableName = "dtReportSummary";

        string sRange = "";
        string sRangePrior = "";
        string sYTD = "";
        string sPriorYTD = "";
        if (ddlYear.SelectedIndex != 0)
        {//Use Year
            sRange = ddlYear.SelectedValue;
            sRangePrior = (Convert.ToInt32(ddlYear.SelectedValue) - 1).ToString();
        }
        else//Use Range
        {
            sRange = txtStartDate.Text + "-" + txtEndDate.Text;
            sRangePrior = Convert.ToDateTime(txtStartDate.Text).AddDays(-365).ToShortDateString() + "-" + Convert.ToDateTime(txtEndDate.Text).AddDays(-365).ToShortDateString();
        }
        sYTD = DateTime.Now.ToShortDateString();
        sPriorYTD = DateTime.Now.AddDays(-365).ToShortDateString();

        string sYearMinus1 = DateTime.Now.AddYears(-1).Year.ToString();
        string sYearMinus2 = DateTime.Now.AddYears(-2).Year.ToString();
        string sYearMinus3 = DateTime.Now.AddYears(-3).Year.ToString();
        string sYearMinus4 = DateTime.Now.AddYears(-4).Year.ToString();
        string sYearMinus5 = DateTime.Now.AddYears(-5).Year.ToString();
        string sYearMinus1Margin = DateTime.Now.AddYears(-1).Year.ToString() + " Margin";
        string sYearMinus2Margin = DateTime.Now.AddYears(-2).Year.ToString() + " Margin";
        string sYearMinus3Margin = DateTime.Now.AddYears(-3).Year.ToString() + " Margin";
        string sYearMinus4Margin = DateTime.Now.AddYears(-4).Year.ToString() + " Margin";
        string sYearMinus5Margin = DateTime.Now.AddYears(-5).Year.ToString() + " Margin";
        string sYearMinus1PercentageOfGrandTotal = DateTime.Now.AddYears(-1).Year.ToString() + " % of GT";
        string sYearMinus2PercentageOfGrandTotal = DateTime.Now.AddYears(-2).Year.ToString() + " % of GT";
        string sYearMinus3PercentageOfGrandTotal = DateTime.Now.AddYears(-3).Year.ToString() + " % of GT";
        string sYearMinus4PercentageOfGrandTotal = DateTime.Now.AddYears(-4).Year.ToString() + " % of GT";
        string sYearMinus5PercentageOfGrandTotal = DateTime.Now.AddYears(-5).Year.ToString() + " % of GT";

        //var query = (from dtSum in dt.AsEnumerable()
        //             select new
        //             {
        //                 Name = dtSum["Name"],
        //                 Customer = dtSum["Customer"],
        //                 RangeAmount = dtSum["RangeAmount"],
        //                 PriorYearRangeAmount = dtSum["PriorYearRangeAmount"],
        //                 CurrentYTD = dtSum["CurrentYTD"],
        //                 YearMinus1 = dtSum["YearMinus1"],
        //                 YearMinus2 = dtSum["YearMinus2"],
        //                 YearMinus3 = dtSum["YearMinus3"],
        //                 YearMinus4 = dtSum["YearMinus4"],
        //                 YearMinus5 = dtSum["YearMinus5"],
        //                 RangeMargin = dtSum["RangeMargin"],
        //                 PriorYearRangeMargin = dtSum["PriorYearRangeMargin"],
        //                 CurrentYTDMargin = dtSum["CurrentYTDMargin"],
        //                 YearMinus1Margin = dtSum["YearMinus1Margin"],
        //                 YearMinus2Margin = dtSum["YearMinus2Margin"],
        //                 YearMinus3Margin = dtSum["YearMinus3Margin"],
        //                 YearMinus4Margin = dtSum["YearMinus4Margin"],
        //                 YearMinus5Margin = dtSum["YearMinus5Margin"],                         
        //                 PercentageOfTotal = dtSum["PercentageOfTotal"],
        //                 PercentageOfTotalPriorYearRange = dtSum["PercentageOfTotalPriorYearRange"],
        //                 PercentageOfTotalCurrentYTD = dtSum["PercentageOfTotalCurrentYTD"],
        //                 PercentageOfTotalYearMinus1 = dtSum["PercentageOfTotalYearMinus1"],
        //                 PercentageOfTotalYearMinus2 = dtSum["PercentageOfTotalYearMinus2"],
        //                 PercentageOfTotalYearMinus3 = dtSum["PercentageOfTotalYearMinus3"],
        //                 PercentageOfTotalYearMinus4 = dtSum["PercentageOfTotalYearMinus4"],
        //                 PercentageOfTotalYearMinus5 = dtSum["PercentageOfTotalYearMinus5"],

        //             });

        switch (ddlYearsBack.SelectedValue)
        {
            case "1":
                var query1 = (from dtSum in dt.AsEnumerable()
                              select new
                              {
                                  Name = dtSum["Name"],
                                  Customer = dtSum["Customer"],
                                  RangeAmount = dtSum["RangeAmount"],
                                  PriorYearRangeAmount = dtSum["PriorYearRangeAmount"],
                                  CurrentYTD = dtSum["CurrentYTD"],
                                  PriorYTD = dtSum["PriorYTD"],
                                  YearMinus1 = dtSum["YearMinus1"],
                                  RangeMargin = dtSum["RangeMargin"],
                                  PriorYearRangeMargin = dtSum["PriorYearRangeMargin"],
                                  CurrentYTDMargin = dtSum["CurrentYTDMargin"],
                                  YearMinus1Margin = dtSum["YearMinus1Margin"],
                                  PercentageOfTotal = dtSum["PercentageOfTotal"],
                                  PercentageOfTotalPriorYearRange = dtSum["PercentageOfTotalPriorYearRange"],
                                  PercentageOfTotalCurrentYTD = dtSum["PercentageOfTotalCurrentYTD"],
                                  PercentageOfTotalYearMinus1 = dtSum["PercentageOfTotalYearMinus1"],

                              });
                dt = SharedFunctions.LINQToDataTable(query1);

                try
                {
                    dt.Columns["RangeAmount"].ColumnName = "Amount " + sRange + "";//Rename column...
                    dt.Columns["PriorYearRangeAmount"].ColumnName = "Amount Prior " + sRangePrior + "";//Rename column...
                    dt.Columns["CurrentYTD"].ColumnName = "YTD-up to " + sYTD;//Rename column...
                    dt.Columns["RangeMargin"].ColumnName = "Margin " + sRange;//Rename column...
                    dt.Columns["PriorYearRangeMargin"].ColumnName = "Margin Prior " + sRangePrior;//Rename column...
                    dt.Columns["CurrentYTDMargin"].ColumnName = "YTD Margin-up to " + sYTD;//Rename column...
                    dt.Columns["PriorYTD"].ColumnName = "YTD Prior-up to " + sPriorYTD;//Rename column...


                    dt.Columns["YearMinus1"].ColumnName = sYearMinus1;//Rename column...
                    dt.Columns["YearMinus1Margin"].ColumnName = sYearMinus1Margin;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus1"].ColumnName = sYearMinus1PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotal"].ColumnName = "% of GT Range";//Rename column...
                    dt.Columns["PercentageOfTotalCurrentYTD"].ColumnName = "% of GT Cur YTD";//Rename column...
                    dt.Columns["PercentageOfTotalPriorYearRange"].ColumnName = "% of GT Prior Yr";//Rename column...

                }
                catch (Exception)
                { }
                break;
            case "2":
                var query2 = (from dtSum in dt.AsEnumerable()
                              select new
                              {
                                  Name = dtSum["Name"],
                                  Customer = dtSum["Customer"],
                                  RangeAmount = dtSum["RangeAmount"],
                                  PriorYearRangeAmount = dtSum["PriorYearRangeAmount"],
                                  CurrentYTD = dtSum["CurrentYTD"],
                                  PriorYTD = dtSum["PriorYTD"],
                                  YearMinus1 = dtSum["YearMinus1"],
                                  YearMinus2 = dtSum["YearMinus2"],
                                  RangeMargin = dtSum["RangeMargin"],
                                  PriorYearRangeMargin = dtSum["PriorYearRangeMargin"],
                                  CurrentYTDMargin = dtSum["CurrentYTDMargin"],
                                  YearMinus1Margin = dtSum["YearMinus1Margin"],
                                  YearMinus2Margin = dtSum["YearMinus2Margin"],
                                  PercentageOfTotal = dtSum["PercentageOfTotal"],
                                  PercentageOfTotalPriorYearRange = dtSum["PercentageOfTotalPriorYearRange"],
                                  PercentageOfTotalCurrentYTD = dtSum["PercentageOfTotalCurrentYTD"],
                                  PercentageOfTotalYearMinus1 = dtSum["PercentageOfTotalYearMinus1"],
                                  PercentageOfTotalYearMinus2 = dtSum["PercentageOfTotalYearMinus2"],

                              });
                dt = SharedFunctions.LINQToDataTable(query2);
                try
                {
                    dt.Columns["RangeAmount"].ColumnName = "Amount " + sRange + "";//Rename column...
                    dt.Columns["PriorYearRangeAmount"].ColumnName = "Amount Prior " + sRangePrior + "";//Rename column...
                    dt.Columns["CurrentYTD"].ColumnName = "YTD-up to " + sYTD;//Rename column...
                    dt.Columns["RangeMargin"].ColumnName = "Margin " + sRange;//Rename column...
                    dt.Columns["PriorYearRangeMargin"].ColumnName = "Margin Prior " + sRangePrior;//Rename column...
                    dt.Columns["CurrentYTDMargin"].ColumnName = "YTD Margin-up to " + sYTD;//Rename column...
                    dt.Columns["PriorYTD"].ColumnName = "YTD Prior-up to " + sPriorYTD;//Rename column...

                    dt.Columns["YearMinus1"].ColumnName = sYearMinus1;//Rename column...
                    dt.Columns["YearMinus2"].ColumnName = sYearMinus2;//Rename column...
                    dt.Columns["YearMinus1Margin"].ColumnName = sYearMinus1Margin;//Rename column...
                    dt.Columns["YearMinus2Margin"].ColumnName = sYearMinus2Margin;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus1"].ColumnName = sYearMinus1PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus2"].ColumnName = sYearMinus2PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotal"].ColumnName = "% of GT Range";//Rename column...
                    dt.Columns["PercentageOfTotalCurrentYTD"].ColumnName = "% of GT Cur YTD";//Rename column...
                    dt.Columns["PercentageOfTotalPriorYearRange"].ColumnName = "% of GT Prior Yr";//Rename column...

                }
                catch (Exception)
                { }
                break;
            case "3":
                var query3 = (from dtSum in dt.AsEnumerable()
                              select new
                              {
                                  Name = dtSum["Name"],
                                  Customer = dtSum["Customer"],
                                  RangeAmount = dtSum["RangeAmount"],
                                  PriorYearRangeAmount = dtSum["PriorYearRangeAmount"],
                                  CurrentYTD = dtSum["CurrentYTD"],
                                  PriorYTD = dtSum["PriorYTD"],
                                  YearMinus1 = dtSum["YearMinus1"],
                                  YearMinus2 = dtSum["YearMinus2"],
                                  YearMinus3 = dtSum["YearMinus3"],
                                  RangeMargin = dtSum["RangeMargin"],
                                  PriorYearRangeMargin = dtSum["PriorYearRangeMargin"],
                                  CurrentYTDMargin = dtSum["CurrentYTDMargin"],
                                  YearMinus1Margin = dtSum["YearMinus1Margin"],
                                  YearMinus2Margin = dtSum["YearMinus2Margin"],
                                  YearMinus3Margin = dtSum["YearMinus3Margin"],
                                  PercentageOfTotal = dtSum["PercentageOfTotal"],
                                  PercentageOfTotalPriorYearRange = dtSum["PercentageOfTotalPriorYearRange"],
                                  PercentageOfTotalCurrentYTD = dtSum["PercentageOfTotalCurrentYTD"],
                                  PercentageOfTotalYearMinus1 = dtSum["PercentageOfTotalYearMinus1"],
                                  PercentageOfTotalYearMinus2 = dtSum["PercentageOfTotalYearMinus2"],
                                  PercentageOfTotalYearMinus3 = dtSum["PercentageOfTotalYearMinus3"],
                              });
                dt = SharedFunctions.LINQToDataTable(query3);
                try
                {
                    dt.Columns["RangeAmount"].ColumnName = "Amount " + sRange + "";//Rename column...
                    dt.Columns["PriorYearRangeAmount"].ColumnName = "Amount Prior " + sRangePrior + "";//Rename column...
                    dt.Columns["CurrentYTD"].ColumnName = "YTD-up to " + sYTD;//Rename column...
                    dt.Columns["RangeMargin"].ColumnName = "Margin " + sRange;//Rename column...
                    dt.Columns["PriorYearRangeMargin"].ColumnName = "Margin Prior " + sRangePrior;//Rename column...
                    dt.Columns["CurrentYTDMargin"].ColumnName = "YTD Margin-up to " + sYTD;//Rename column...
                    dt.Columns["PriorYTD"].ColumnName = "YTD Prior-up to " + sPriorYTD;//Rename column...

                    dt.Columns["YearMinus1"].ColumnName = sYearMinus1;//Rename column...
                    dt.Columns["YearMinus2"].ColumnName = sYearMinus2;//Rename column...
                    dt.Columns["YearMinus3"].ColumnName = sYearMinus3;//Rename column...
                    dt.Columns["YearMinus1Margin"].ColumnName = sYearMinus1Margin;//Rename column...
                    dt.Columns["YearMinus2Margin"].ColumnName = sYearMinus2Margin;//Rename column...
                    dt.Columns["YearMinus3Margin"].ColumnName = sYearMinus3Margin;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus1"].ColumnName = sYearMinus1PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus2"].ColumnName = sYearMinus2PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus3"].ColumnName = sYearMinus3PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotal"].ColumnName = "% of GT Range";//Rename column...
                    dt.Columns["PercentageOfTotalCurrentYTD"].ColumnName = "% of GT Cur YTD";//Rename column...
                    dt.Columns["PercentageOfTotalPriorYearRange"].ColumnName = "% of GT Prior Yr";//Rename column...

                }
                catch (Exception)
                { }
                break;
            case "4":
                var query4 = (from dtSum in dt.AsEnumerable()
                              select new
                              {
                                  Name = dtSum["Name"],
                                  Customer = dtSum["Customer"],
                                  RangeAmount = dtSum["RangeAmount"],
                                  PriorYearRangeAmount = dtSum["PriorYearRangeAmount"],
                                  CurrentYTD = dtSum["CurrentYTD"],
                                  PriorYTD = dtSum["PriorYTD"],
                                  YearMinus1 = dtSum["YearMinus1"],
                                  YearMinus2 = dtSum["YearMinus2"],
                                  YearMinus3 = dtSum["YearMinus3"],
                                  YearMinus4 = dtSum["YearMinus4"],
                                  RangeMargin = dtSum["RangeMargin"],
                                  PriorYearRangeMargin = dtSum["PriorYearRangeMargin"],
                                  CurrentYTDMargin = dtSum["CurrentYTDMargin"],
                                  YearMinus1Margin = dtSum["YearMinus1Margin"],
                                  YearMinus2Margin = dtSum["YearMinus2Margin"],
                                  YearMinus3Margin = dtSum["YearMinus3Margin"],
                                  YearMinus4Margin = dtSum["YearMinus4Margin"],
                                  PercentageOfTotal = dtSum["PercentageOfTotal"],
                                  PercentageOfTotalPriorYearRange = dtSum["PercentageOfTotalPriorYearRange"],
                                  PercentageOfTotalCurrentYTD = dtSum["PercentageOfTotalCurrentYTD"],
                                  PercentageOfTotalYearMinus1 = dtSum["PercentageOfTotalYearMinus1"],
                                  PercentageOfTotalYearMinus2 = dtSum["PercentageOfTotalYearMinus2"],
                                  PercentageOfTotalYearMinus3 = dtSum["PercentageOfTotalYearMinus3"],
                                  PercentageOfTotalYearMinus4 = dtSum["PercentageOfTotalYearMinus4"],

                              });
                dt = SharedFunctions.LINQToDataTable(query4);
                try
                {
                    dt.Columns["RangeAmount"].ColumnName = "Amount " + sRange + "";//Rename column...
                    dt.Columns["PriorYearRangeAmount"].ColumnName = "Amount Prior " + sRangePrior + "";//Rename column...
                    dt.Columns["CurrentYTD"].ColumnName = "YTD-up to " + sYTD;//Rename column...
                    dt.Columns["RangeMargin"].ColumnName = "Margin " + sRange;//Rename column...
                    dt.Columns["PriorYearRangeMargin"].ColumnName = "Margin Prior " + sRangePrior;//Rename column...
                    dt.Columns["CurrentYTDMargin"].ColumnName = "YTD Margin-up to " + sYTD;//Rename column...
                    dt.Columns["PriorYTD"].ColumnName = "YTD Prior-up to " + sPriorYTD;//Rename column...

                    dt.Columns["YearMinus1"].ColumnName = sYearMinus1;//Rename column...
                    dt.Columns["YearMinus2"].ColumnName = sYearMinus2;//Rename column...
                    dt.Columns["YearMinus3"].ColumnName = sYearMinus3;//Rename column...
                    dt.Columns["YearMinus4"].ColumnName = sYearMinus4;//Rename column...
                    dt.Columns["YearMinus1Margin"].ColumnName = sYearMinus1Margin;//Rename column...
                    dt.Columns["YearMinus2Margin"].ColumnName = sYearMinus2Margin;//Rename column...
                    dt.Columns["YearMinus3Margin"].ColumnName = sYearMinus3Margin;//Rename column...
                    dt.Columns["YearMinus4Margin"].ColumnName = sYearMinus4Margin;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus1"].ColumnName = sYearMinus1PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus2"].ColumnName = sYearMinus2PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus3"].ColumnName = sYearMinus3PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus4"].ColumnName = sYearMinus4PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotal"].ColumnName = "% of GT Range";//Rename column...
                    dt.Columns["PercentageOfTotalCurrentYTD"].ColumnName = "% of GT Cur YTD";//Rename column...
                    dt.Columns["PercentageOfTotalPriorYearRange"].ColumnName = "% of GT Prior Yr";//Rename column...

                }
                catch (Exception)
                { }
                break;
            case "5":
                var query5 = (from dtSum in dt.AsEnumerable()
                              select new
                              {
                                  Name = dtSum["Name"],
                                  Customer = dtSum["Customer"],
                                  RangeAmount = dtSum["RangeAmount"],
                                  PriorYearRangeAmount = dtSum["PriorYearRangeAmount"],
                                  CurrentYTD = dtSum["CurrentYTD"],
                                  PriorYTD = dtSum["PriorYTD"],
                                  YearMinus1 = dtSum["YearMinus1"],
                                  YearMinus2 = dtSum["YearMinus2"],
                                  YearMinus3 = dtSum["YearMinus3"],
                                  YearMinus4 = dtSum["YearMinus4"],
                                  YearMinus5 = dtSum["YearMinus5"],
                                  RangeMargin = dtSum["RangeMargin"],
                                  PriorYearRangeMargin = dtSum["PriorYearRangeMargin"],
                                  CurrentYTDMargin = dtSum["CurrentYTDMargin"],
                                  YearMinus1Margin = dtSum["YearMinus1Margin"],
                                  YearMinus2Margin = dtSum["YearMinus2Margin"],
                                  YearMinus3Margin = dtSum["YearMinus3Margin"],
                                  YearMinus4Margin = dtSum["YearMinus4Margin"],
                                  YearMinus5Margin = dtSum["YearMinus5Margin"],
                                  PercentageOfTotal = dtSum["PercentageOfTotal"],
                                  PercentageOfTotalPriorYearRange = dtSum["PercentageOfTotalPriorYearRange"],
                                  PercentageOfTotalCurrentYTD = dtSum["PercentageOfTotalCurrentYTD"],
                                  PercentageOfTotalYearMinus1 = dtSum["PercentageOfTotalYearMinus1"],
                                  PercentageOfTotalYearMinus2 = dtSum["PercentageOfTotalYearMinus2"],
                                  PercentageOfTotalYearMinus3 = dtSum["PercentageOfTotalYearMinus3"],
                                  PercentageOfTotalYearMinus4 = dtSum["PercentageOfTotalYearMinus4"],
                                  PercentageOfTotalYearMinus5 = dtSum["PercentageOfTotalYearMinus5"],

                              });
                dt = SharedFunctions.LINQToDataTable(query5);
                try
                {
                    dt.Columns["RangeAmount"].ColumnName = "Amount " + sRange + "";//Rename column...
                    dt.Columns["PriorYearRangeAmount"].ColumnName = "Amount Prior " + sRangePrior + "";//Rename column...
                    dt.Columns["CurrentYTD"].ColumnName = "YTD-up to " + sYTD;//Rename column...
                    dt.Columns["RangeMargin"].ColumnName = "Margin " + sRange;//Rename column...
                    dt.Columns["PriorYearRangeMargin"].ColumnName = "Margin Prior " + sRangePrior;//Rename column...
                    dt.Columns["CurrentYTDMargin"].ColumnName = "YTD Margin-up to " + sYTD;//Rename column...
                    dt.Columns["PriorYTD"].ColumnName = "YTD Prior-up to " + sPriorYTD;//Rename column...

                    dt.Columns["YearMinus1"].ColumnName = sYearMinus1;//Rename column...
                    dt.Columns["YearMinus2"].ColumnName = sYearMinus2;//Rename column...
                    dt.Columns["YearMinus3"].ColumnName = sYearMinus3;//Rename column...
                    dt.Columns["YearMinus4"].ColumnName = sYearMinus4;//Rename column...
                    dt.Columns["YearMinus5"].ColumnName = sYearMinus5;//Rename column...
                    dt.Columns["YearMinus1Margin"].ColumnName = sYearMinus1Margin;//Rename column...
                    dt.Columns["YearMinus2Margin"].ColumnName = sYearMinus2Margin;//Rename column...
                    dt.Columns["YearMinus3Margin"].ColumnName = sYearMinus3Margin;//Rename column...
                    dt.Columns["YearMinus4Margin"].ColumnName = sYearMinus4Margin;//Rename column...
                    dt.Columns["YearMinus5Margin"].ColumnName = sYearMinus5Margin;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus1"].ColumnName = sYearMinus1PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus2"].ColumnName = sYearMinus2PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus3"].ColumnName = sYearMinus3PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus4"].ColumnName = sYearMinus4PercentageOfGrandTotal;//Rename column...
                    dt.Columns["PercentageOfTotalYearMinus5"].ColumnName = sYearMinus5PercentageOfGrandTotal;//Rename column... 
                    dt.Columns["PercentageOfTotal"].ColumnName = "% of GT Range";//Rename column...
                    dt.Columns["PercentageOfTotalCurrentYTD"].ColumnName = "% of GT Cur YTD";//Rename column...
                    dt.Columns["PercentageOfTotalPriorYearRange"].ColumnName = "% of GT Prior Yr";//Rename column...

                }
                catch (Exception)
                { }
                break;
        }




        int iRoleID = 0;
        iRoleID = Convert.ToInt32(Session["RoleID"]);
        switch (iRoleID)
        {
            case 1://Admin...
                try
                {
                    dt.Columns.Remove("Margin2");
                    dt.Columns.Remove("GrandTotalCurrentYTD");
                    dt.Columns.Remove("GrandTotalYearMinus1");
                    dt.Columns.Remove("GrandTotalYearMinus2");
                    dt.Columns.Remove("GrandTotalYearMinus3");
                    dt.Columns.Remove("GrandTotalYearMinus4");
                    dt.Columns.Remove("GrandTotalYearMinus5");
                    dt.Columns.Remove("GrandTotalPriorYearRange");
                }
                catch (Exception)
                { }
                break;
            case 2://Supervisor...
                try
                {
                    dt.Columns.Remove("Margin2");
                    dt.Columns.Remove("GrandTotal");
                    dt.Columns.Remove("GrandTotalCurrentYTD");
                    dt.Columns.Remove("GrandTotalYearMinus1");
                    dt.Columns.Remove("GrandTotalYearMinus2");
                    dt.Columns.Remove("GrandTotalYearMinus3");
                    dt.Columns.Remove("GrandTotalYearMinus4");
                    dt.Columns.Remove("GrandTotalYearMinus5");
                    dt.Columns.Remove("GrandTotalPriorYearRange");
                }
                catch (Exception)
                { }
                break;
            default:
                try
                {
                    dt.Columns.Remove("Margin");
                    dt.Columns.Remove("Margin2");
                    dt.Columns.Remove("GrandTotalCurrentYTD");
                    dt.Columns.Remove("GrandTotalYearMinus1");
                    dt.Columns.Remove("GrandTotalYearMinus2");
                    dt.Columns.Remove("GrandTotalYearMinus3");
                    dt.Columns.Remove("GrandTotalYearMinus4");
                    dt.Columns.Remove("GrandTotalYearMinus5");
                    dt.Columns.Remove("GrandTotalPriorYearRange");
                }
                catch (Exception)
                { }
                break;
        }




        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "CustomerReportSummary" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    private void ExportFullReport()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";


        dt = RunReport();
        dt.TableName = "dtReportFull";


        string sYearMinus1 = DateTime.Now.AddYears(-1).Year.ToString();
        string sYearMinus2 = DateTime.Now.AddYears(-2).Year.ToString();
        string sYearMinus3 = DateTime.Now.AddYears(-3).Year.ToString();
        string sYearMinus4 = DateTime.Now.AddYears(-4).Year.ToString();
        string sYearMinus5 = DateTime.Now.AddYears(-5).Year.ToString();


        try
        {

            dt.Columns["YearMinus1"].ColumnName = sYearMinus1;//Rename column...
            dt.Columns["YearMinus2"].ColumnName = sYearMinus2;//Rename column...
            dt.Columns["YearMinus3"].ColumnName = sYearMinus3;//Rename column...
            dt.Columns["YearMinus4"].ColumnName = sYearMinus4;//Rename column...
            dt.Columns["YearMinus5"].ColumnName = sYearMinus5;//Rename column... 

        }
        catch (Exception)
        { }

        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "CustomerReportFull" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    #endregion

    #region Functions
    private DataTable RunReport()
    {
        string sMsg = "";

        string sCustomer = "NULL";
        if (lbCustomers.SelectedIndex != -1)//ALL...
        {//Not null then add quotes...
            sCustomer = "'" + lbCustomers.SelectedValue.Trim() + "'";
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
        string sStockCodeFrom = "";
        string sStockCodeTo = "";

        if (Mode == "Range")//Stock Code Range...
        {
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

        }
        else if (Mode == "EndCustomer")//End Customer...
        {
            sStockCode = "";
            if (ddlEndCustomers.SelectedIndex == 0)
            {
                sMsg += "**You selected Non Customer: a Non Customer Selection is Required!<br/>";
            }
            if (lbParentStockCodeEndCustomer.SelectedIndex == -1)
            {
                sMsg += "**You selected Non Customer: a Stock Code(s) Selection is Required!<br/>";
            }
            int iIndexOfPipe = 0;
            foreach (ListItem li in lbParentStockCodeEndCustomer.Items)
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
        else
        {

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
            return null;
        }


        if (Mode == "Single")
        {//Single...

            if (sStockCode != "")//Selected Stock Codes...
            {

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
            }
            else//All Stock Codes...
            {
                sSQL = "EXEC spGetCustomerReport ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@FromMargin=" + sFromMargin + ",";
                sSQL += "@ToMargin=" + sToMargin + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@Year=" + sYear + ",";
                sSQL += "@ProductClass=" + sProductClass + ",";
                sSQL += "@SalesPerson=" + sSalesPerson;
            }

        }
        else if (Mode == "Range")//Stock Code Range...
        {
            sSQL = "EXEC spGetCustomerReportByRange ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@FromMargin=" + sFromMargin + ",";
            sSQL += "@ToMargin=" + sToMargin + ",";
            sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
            sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
            sSQL += "@Customer=" + sCustomer + ",";
            sSQL += "@Year=" + sYear + ",";
            sSQL += "@ProductClass=" + sProductClass + ",";
            sSQL += "@SalesPerson=" + sSalesPerson;
        }
        else if (Mode == "EndCustomer")//End Customer...
        {
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
        }
        else
        {
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
        }

        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");


        if (dt.Rows.Count > 0)
        {
            Session["dtReportFull"] = dt;
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
        return dt;
    }
    private DataTable RunReportSummary()
    {
        int iIndexOfPipe = 0;
        string sMsg = "";

        string sCustomer = "NULL";
        if (lbCustomers.SelectedIndex != -1)//ALL...
        {//Not null then add quotes...
            sCustomer = "'" + lbCustomers.SelectedValue.Trim() + "'";
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
        string sStockCodeFrom = "";
        string sStockCodeTo = "";

        if (Mode == "Range")//Stock Code Range...
        {
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
        }
        else if (Mode == "EndCustomer")//End Customer...
        {
            sStockCode = "";
            int iNonCustomerID = 0;
            if (ddlEndCustomers.SelectedIndex == 0)
            {
                sMsg += "**You selected Non Customer: a Non Customer Selection is Required!<br/>";
            }
            else
            {
                iNonCustomerID = Convert.ToInt32(ddlEndCustomers.SelectedValue);
            }
            List<string> lStockCodes = new List<string>();
            lStockCodes = GetNonCustomerStockCodes(iNonCustomerID);

            foreach (string StockCode in lStockCodes)
            {
                sStockCode += StockCode + "|";
            }
            if (sStockCode.Trim().EndsWith("|"))
            {
                iIndexOfPipe = sStockCode.Trim().LastIndexOf("|");
                sStockCode = sStockCode.Remove(iIndexOfPipe).Trim();
                sStockCode = "'" + sStockCode + "'";
            }
        }
        else
        {

        }

        string sCities = "";
        foreach (ListItem li in lbCities.Items)
        {
            if (li.Selected)
            {
                sCities += li.Value + "|";
            }
        }
        iIndexOfPipe = 0;
        if (sCities.Trim().EndsWith("|"))
        {
            iIndexOfPipe = sCities.Trim().LastIndexOf("|");
            sCities = sCities.Remove(iIndexOfPipe).Trim();
            sCities = "'" + sCities + "'";
        }
        string sZips = "";
        foreach (ListItem li in lbZips.Items)
        {
            if (li.Selected)
            {
                sZips += li.Value + "|";
            }
        }
        iIndexOfPipe = 0;
        if (sZips.Trim().EndsWith("|"))
        {
            iIndexOfPipe = sZips.Trim().LastIndexOf("|");
            sZips = sZips.Remove(iIndexOfPipe).Trim();
            sZips = "'" + sZips + "'";
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
            return null;
        }

        if (Mode == "Range")//Stock Code Range...
        {
            if (chkIncludeEndCustomerInSummary.Checked && chkIncludeGroupingInSummary.Checked)
            {//Both Checked...
                sSQL = "EXEC spGetCustomerReportSummaryCombo ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@FromMargin=" + sFromMargin + ",";
                sSQL += "@ToMargin=" + sToMargin + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@Year=" + sYear + ",";
                sSQL += "@ProductClass=" + sProductClass + ",";
                if (sCities != "")
                {
                    sSQL += "@Cities=" + sCities + ",";
                }
                if (sZips != "")
                {
                    sSQL += "@Zips=" + sZips + ",";
                }
                sSQL += "@IncludeEndCustomer=1,";
                sSQL += "@IncludeGrouping=1,";
                sSQL += "@SalesPerson=" + sSalesPerson;
            }
            if (chkIncludeEndCustomerInSummary.Checked && !chkIncludeGroupingInSummary.Checked)
            {//End Customer Checked...
                sSQL = "EXEC spGetCustomerReportSummaryCombo ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@FromMargin=" + sFromMargin + ",";
                sSQL += "@ToMargin=" + sToMargin + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@Year=" + sYear + ",";
                sSQL += "@ProductClass=" + sProductClass + ",";
                if (sCities != "")
                {
                    sSQL += "@Cities=" + sCities + ",";
                }
                if (sZips != "")
                {
                    sSQL += "@Zips=" + sZips + ",";
                }
                sSQL += "@IncludeEndCustomer=1,";
                sSQL += "@IncludeGrouping=0,";
                sSQL += "@SalesPerson=" + sSalesPerson;
            }
            if (!chkIncludeEndCustomerInSummary.Checked && chkIncludeGroupingInSummary.Checked)
            {//Grouping Checked...
                sSQL = "EXEC spGetCustomerReportSummaryCombo ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@FromMargin=" + sFromMargin + ",";
                sSQL += "@ToMargin=" + sToMargin + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@Year=" + sYear + ",";
                sSQL += "@ProductClass=" + sProductClass + ",";
                if (sCities != "")
                {
                    sSQL += "@Cities=" + sCities + ",";
                }
                if (sZips != "")
                {
                    sSQL += "@Zips=" + sZips + ",";
                }
                sSQL += "@IncludeEndCustomer=0,";
                sSQL += "@IncludeGrouping=1,";
                sSQL += "@SalesPerson=" + sSalesPerson;
            }
            if (!chkIncludeEndCustomerInSummary.Checked && !chkIncludeGroupingInSummary.Checked)
            {//Both UnChecked...
                sSQL = "EXEC spGetCustomerReportSummaryCombo ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@FromMargin=" + sFromMargin + ",";
                sSQL += "@ToMargin=" + sToMargin + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@Year=" + sYear + ",";
                sSQL += "@ProductClass=" + sProductClass + ",";
                if (sCities != "")
                {
                    sSQL += "@Cities=" + sCities + ",";
                }
                if (sZips != "")
                {
                    sSQL += "@Zips=" + sZips + ",";
                }
                sSQL += "@IncludeEndCustomer=0,";
                sSQL += "@IncludeGrouping=0,";
                sSQL += "@SalesPerson=" + sSalesPerson;
            }
            else
            {

            }
        }
        else if (Mode == "EndCustomer")//End Customer...
        {
            if (sStockCode == "")
            {
                lblError.Text = "**No Stockcodes available for this End Customer!!";
                lblError.ForeColor = Color.Red;
                gvCustomerSummary.DataSource = null;
                gvCustomerSummary.DataBind();
                return null;
            }

            sSQL = "EXEC spGetCustomerReportSummaryCombo ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@FromMargin=" + sFromMargin + ",";
            sSQL += "@ToMargin=" + sToMargin + ",";
            sSQL += "@StockCodes=" + sStockCode + ",";
            sSQL += "@Customer=" + sCustomer + ",";
            sSQL += "@Year=" + sYear + ",";
            sSQL += "@ProductClass=" + sProductClass + ",";
            sSQL += "@IncludeEndCustomer=1,";
            sSQL += "@IncludeGrouping=0,";
            sSQL += "@SalesPerson=" + sSalesPerson;
        }
        else
        {
            sSQL = "EXEC spGetCustomerReportSummaryCombo ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@FromMargin=" + sFromMargin + ",";
            sSQL += "@ToMargin=" + sToMargin + ",";
            sSQL += "@StockCodes=" + sStockCode + ",";
            sSQL += "@Customer=" + sCustomer + ",";
            sSQL += "@Year=" + sYear + ",";
            sSQL += "@ProductClass=" + sProductClass + ",";
            sSQL += "@SalesPerson=" + sSalesPerson;
        }



        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
        DataView view = new DataView(dt);
        view.Sort = "CurrentYTD DESC";
        dt = view.ToTable();


        if (dt.Rows.Count > 0)
        {
            Session["dtReportSummary"] = dt;
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
        return dt;
    }
    private List<string> GetNonCustomerStockCodes(int iNonCustomerID)
    {
        List<string> lStockCodes = new List<string>();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

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
    }
    private List<StockCodeDescriptionList.StockCodeDescription> GetNonCustomerStockCodesDescs(int iNonCustomerID)
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
    private string GetCustomerDetails(string sCustomer)
    {
        string sCustomerDetails = "";
        StringBuilder sb = new StringBuilder();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from cu in db.ArCustomer
                         join t in db.TblArTerms on cu.TermsCode equals t.TermsCode
                         where
                           cu.Customer == sCustomer
                         select new
                         {
                             cu.Customer,
                             cu.Name,
                             cu.SoldToAddr1,
                             cu.SoldToAddr2,
                             cu.SoldToAddr3,
                             cu.SoldToAddr4,
                             cu.SoldToAddr5,
                             cu.SoldPostalCode,
                             cu.Contact,
                             cu.Email,
                             cu.Telephone,
                             Terms = t.Description,
                             cu.ShippingInstrs
                         });
            foreach (var a in query)
            {


                sCustomerDetails = "ADDRESS: " + a.SoldToAddr1.ToUpper().Trim() + " ";
                sb.Append(sCustomerDetails);
                sb.Append(Environment.NewLine);
                sCustomerDetails = "CITY: " + a.SoldToAddr4.ToUpper().Trim() + " ";
                sb.Append(sCustomerDetails);
                sb.Append(Environment.NewLine);
                sCustomerDetails = "STATE: " + a.SoldToAddr5.ToUpper().Trim() + " ";
                sb.Append(sCustomerDetails);
                sb.Append(Environment.NewLine);
                sCustomerDetails = "ZIP: " + a.SoldPostalCode.ToUpper().Trim() + " ";
                sb.Append(sCustomerDetails);
                sb.Append(Environment.NewLine);
                sCustomerDetails = "CONTACT: " + a.Contact.ToUpper().Trim() + " ";
                sb.Append(sCustomerDetails);
                sb.Append(Environment.NewLine);
                sCustomerDetails = "EMAIL: " + a.Email.ToUpper().Trim() + " ";
                sb.Append(sCustomerDetails);
                sb.Append(Environment.NewLine);
                sCustomerDetails = "PHONE: " + a.Telephone.ToUpper().Trim() + " ";
                sb.Append(sCustomerDetails);
                sb.Append(Environment.NewLine);
                sCustomerDetails = "TERMS: " + a.Terms.ToUpper().Trim() + " ";
                sb.Append(sCustomerDetails);
                sb.Append(Environment.NewLine);
                sCustomerDetails = "SHIPPING INSTR: " + a.ShippingInstrs.ToUpper().Trim();
                sb.Append(sCustomerDetails);
                sb.Append(Environment.NewLine);

                sCustomerDetails = sb.ToString();

            }
        }

        return sCustomerDetails;

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
            ////txtStartDate.Text = SharedFunctions.GetOldestTrnDateInDatabase();
            ////txtEndDate.Text = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
            ddlPeriod.SelectedIndex = 2;
            DateTime dtToday = DateTime.Today;
            DateTime dtCurrentMonthFirstDay = new DateTime(dtToday.Year, dtToday.Month, 1);
            DateTime dtCurrentMonthLastDay = new DateTime(dtToday.Year, dtToday.Month, DateTime.DaysInMonth(dtToday.Year, dtToday.Month));
            DateTime dtFirstDayOfLastMonth = dtCurrentMonthFirstDay.AddMonths(-1);
            DateTime dtLastDayOfLastMonth = dtCurrentMonthFirstDay.AddDays(-1);

            txtStartDate.Text = dtCurrentMonthFirstDay.ToShortDateString();
            txtEndDate.Text = dtCurrentMonthLastDay.ToShortDateString();

            ViewState["FullReportClicked"] = false;
            ViewState["SummaryReportClicked"] = false;


            Session["dtReportFull"] = null;
            Session["dtReportSummary"] = null;

            Mode = "Range";
            btnStockCodeRangePanel.CssClass = "btn btn-success";
            LoadProductClass(ddlProductClass);
            LoadSalesPersons();

            LoadCustomer(lbCustomers, "Name");

            LoadNonCustomers(ddlEndCustomers);
            LoadCustomerCities(lbCities);
            LoadCustomerZips(lbZips);
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

            pnlStockCodeRange.Visible = true;





        }//End postback
        else
        {
            //Postback...
            //Use to Maintain jQuery during postbacks...
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isPostBack", "$(document).ready(isPostBack);", true);
        }

    }

    protected void btnStockCodeRangePanel_Click(object sender, EventArgs e)
    {
        pnlSingleStockCode.Visible = false;
        pnlStockCodeRange.Visible = true;
        pnlEndCustomer.Visible = false;
        ddlEndCustomers.SelectedIndex = 0;
        lbParentStockCodeEndCustomer.Items.Clear();
        chkIncludeEndCustomerInSummary.Checked = false;

        btnStockCodeRangePanel.CssClass = "btn btn-success";
        btnEndCustomerPanel.CssClass = "btn btn-info";


        Mode = "Range";
    }
    protected void btnEndCustomerPanel_Click(object sender, EventArgs e)
    {
        pnlSingleStockCode.Visible = false;
        pnlStockCodeRange.Visible = false;
        pnlEndCustomer.Visible = true;
        chkIncludeEndCustomerInSummary.Checked = true;


        btnStockCodeRangePanel.CssClass = "btn btn-info";
        btnEndCustomerPanel.CssClass = "btn btn-success";

        Mode = "EndCustomer";
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
        Session["dtTrendsReportQuantity"] = null;
        Session["dtTrendsReportDollars"] = null;
        Session["dtTrendsReportQuantitySummary"] = null;
        Session["dtTrendsReportDollarsSummary"] = null;
        DateTime dtToday = DateTime.Today;
        DateTime dtCurrentMonthFirstDay = new DateTime(dtToday.Year, dtToday.Month, 1);
        DateTime dtCurrentMonthLastDay = new DateTime(dtToday.Year, dtToday.Month, DateTime.DaysInMonth(dtToday.Year, dtToday.Month));
        DateTime dtFirstDayOfLastMonth = dtCurrentMonthFirstDay.AddMonths(-1);
        DateTime dtLastDayOfLastMonth = dtCurrentMonthFirstDay.AddDays(-1);

        switch (ddlPeriod.SelectedIndex)
        {
            case 0://All
                txtStartDate.Text = SharedFunctions.GetOldestTrnDateInDatabase();
                txtEndDate.Text = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();
                break;
            case 1://Range
                txtStartDate.Text = DateTime.Now.AddDays(-7).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 2://Current Month...
                txtStartDate.Text = dtCurrentMonthFirstDay.ToShortDateString();
                txtEndDate.Text = dtCurrentMonthLastDay.ToShortDateString();
                break;
            case 3://Last Month...
                txtStartDate.Text = dtFirstDayOfLastMonth.ToShortDateString();
                txtEndDate.Text = dtLastDayOfLastMonth.ToShortDateString();
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

        //if ((bool)ViewState["FullReportClicked"] == true)
        //{
        //    btnPreview_Click(btnPreview, null);
        //}
        //if ((bool)ViewState["SummaryReportClicked"] == true)
        //{
        //    btnRunReportSummary_Click(btnRunReportSummary, null);
        //}
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


            if (lblMarginAvgTotal.Text != "")
            {
                switch (iRoleID)
                {
                    case 1://Admin...
                        lblMarginAvgTotal.Text = dcWeightedAvg.ToString("#,0.0") + "%";
                        break;
                    case 2://Supervisor...
                        lblMarginAvgTotal.Text = dcWeightedAvg.ToString("#,0.0") + "%";
                        break;
                    default:
                        lblMarginAvgTotal.Text = "--";
                        break;
                }
            }

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

        DataTable dt = (DataTable)Session["dtReportFull"];
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
        int iRoleID = 0;
        iRoleID = Convert.ToInt32(Session["RoleID"]);

        decimal dcAmount = 0;//InvoiceValue...
        decimal dcAmountPrior = 0;//InvoiceValue...
        decimal dcYTD = 0;
        decimal dcPriorYTD = 0;
        decimal dcYearMinus1 = 0;
        decimal dcYearMinus2 = 0;
        decimal dcYearMinus3 = 0;
        decimal dcYearMinus4 = 0;
        decimal dcYearMinus5 = 0;

        decimal dcCostRange = 0;
        decimal dcYTDCost = 0;
        decimal dcTotalCostPrior = 0;
        decimal dcYearMinus1Cost = 0;
        decimal dcYearMinus2Cost = 0;
        decimal dcYearMinus3Cost = 0;
        decimal dcYearMinus4Cost = 0;
        decimal dcYearMinus5Cost = 0;

        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                string sRange = "";
                string sRangePrior = "";
                string sYTD = "";
                string sPriorYTD = "";
                if (ddlYear.SelectedIndex != 0)
                {//Use Year
                    sRange = ddlYear.SelectedValue;
                    sRangePrior = (Convert.ToInt32(ddlYear.SelectedValue) - 1).ToString();
                }
                else//Use Range
                {
                    sRange = txtStartDate.Text + "-" + txtEndDate.Text;
                    sRangePrior = Convert.ToDateTime(txtStartDate.Text).AddDays(-365).ToShortDateString() + "-" + Convert.ToDateTime(txtEndDate.Text).AddDays(-365).ToShortDateString();
                }
                sYTD = DateTime.Now.ToShortDateString();
                sPriorYTD = DateTime.Now.AddDays(-365).ToShortDateString();
                e.Row.Cells[3].Text = "Amount<br />" + sRange + "";//Add range
                e.Row.Cells[4].Text = "Margin<br />" + sRange + "";//Add range
                //e.Row.Cells[5].Text = "Margin<br />(" + sRange + ")";//Add range
                e.Row.Cells[7].Text = "Amount Prior<br />" + sRangePrior + "";//Add range
                e.Row.Cells[8].Text = "Margin Prior<br />" + sRangePrior + "";//Add range
                e.Row.Cells[9].Text = "YTD<br />up to <br />" + sYTD;//Add Date Up to...
                e.Row.Cells[10].Text = "YTD Prior<br />up to <br />" + sPriorYTD;//Add Date Up to(prior year)...
                e.Row.Cells[12].Text = "YTD Margin<br />up to <br />" + sYTD; //Add Date Up to...


                e.Row.Cells[14].Text = DateTime.Now.AddYears(-1).Year.ToString();
                e.Row.Cells[15].Text = DateTime.Now.AddYears(-1).Year.ToString() + "<br> Margin";

                e.Row.Cells[16].Text = DateTime.Now.AddYears(-2).Year.ToString();
                e.Row.Cells[17].Text = DateTime.Now.AddYears(-2).Year.ToString() + "<br> Margin";

                e.Row.Cells[18].Text = DateTime.Now.AddYears(-3).Year.ToString();
                e.Row.Cells[19].Text = DateTime.Now.AddYears(-3).Year.ToString() + "<br> Margin";

                e.Row.Cells[20].Text = DateTime.Now.AddYears(-4).Year.ToString();
                e.Row.Cells[21].Text = DateTime.Now.AddYears(-4).Year.ToString() + "<br> Margin";

                e.Row.Cells[22].Text = DateTime.Now.AddYears(-5).Year.ToString();
                e.Row.Cells[23].Text = DateTime.Now.AddYears(-5).Year.ToString() + "<br> Margin";


                switch (ddlYearsBack.SelectedValue)
                {
                    case "1":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = false;
                        e.Row.Cells[17].Visible = false;
                        e.Row.Cells[18].Visible = false;
                        e.Row.Cells[19].Visible = false;
                        e.Row.Cells[20].Visible = false;
                        e.Row.Cells[21].Visible = false;
                        e.Row.Cells[22].Visible = false;
                        e.Row.Cells[23].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1200);
                        break;
                    case "2":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = true;
                        e.Row.Cells[17].Visible = true;
                        e.Row.Cells[18].Visible = false;
                        e.Row.Cells[19].Visible = false;
                        e.Row.Cells[20].Visible = false;
                        e.Row.Cells[21].Visible = false;
                        e.Row.Cells[22].Visible = false;
                        e.Row.Cells[23].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1300);
                        break;
                    case "3":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = true;
                        e.Row.Cells[17].Visible = true;
                        e.Row.Cells[18].Visible = true;
                        e.Row.Cells[19].Visible = true;
                        e.Row.Cells[20].Visible = false;
                        e.Row.Cells[21].Visible = false;
                        e.Row.Cells[22].Visible = false;
                        e.Row.Cells[23].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1400);
                        break;
                    case "4":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = true;
                        e.Row.Cells[17].Visible = true;
                        e.Row.Cells[18].Visible = true;
                        e.Row.Cells[19].Visible = true;
                        e.Row.Cells[20].Visible = true;
                        e.Row.Cells[21].Visible = true;
                        e.Row.Cells[22].Visible = false;
                        e.Row.Cells[23].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1500);
                        break;
                    case "5":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = true;
                        e.Row.Cells[17].Visible = true;
                        e.Row.Cells[18].Visible = true;
                        e.Row.Cells[19].Visible = true;
                        e.Row.Cells[20].Visible = true;
                        e.Row.Cells[21].Visible = true;
                        e.Row.Cells[22].Visible = true;
                        e.Row.Cells[23].Visible = true;
                        gvCustomerSummary.Width = Unit.Pixel(1600);
                        break;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                switch (ddlYearsBack.SelectedValue)
                {
                    case "1":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = false;
                        e.Row.Cells[17].Visible = false;
                        e.Row.Cells[18].Visible = false;
                        e.Row.Cells[19].Visible = false;
                        e.Row.Cells[20].Visible = false;
                        e.Row.Cells[21].Visible = false;
                        e.Row.Cells[22].Visible = false;
                        e.Row.Cells[23].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1200);
                        break;
                    case "2":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = true;
                        e.Row.Cells[17].Visible = true;
                        e.Row.Cells[18].Visible = false;
                        e.Row.Cells[19].Visible = false;
                        e.Row.Cells[20].Visible = false;
                        e.Row.Cells[21].Visible = false;
                        e.Row.Cells[22].Visible = false;
                        e.Row.Cells[23].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1300);
                        break;
                    case "3":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = true;
                        e.Row.Cells[17].Visible = true;
                        e.Row.Cells[18].Visible = true;
                        e.Row.Cells[19].Visible = true;
                        e.Row.Cells[20].Visible = false;
                        e.Row.Cells[21].Visible = false;
                        e.Row.Cells[22].Visible = false;
                        e.Row.Cells[23].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1400);
                        break;
                    case "4":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = true;
                        e.Row.Cells[17].Visible = true;
                        e.Row.Cells[18].Visible = true;
                        e.Row.Cells[19].Visible = true;
                        e.Row.Cells[20].Visible = true;
                        e.Row.Cells[21].Visible = true;
                        e.Row.Cells[22].Visible = false;
                        e.Row.Cells[23].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1500);
                        break;
                    case "5":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = true;
                        e.Row.Cells[17].Visible = true;
                        e.Row.Cells[18].Visible = true;
                        e.Row.Cells[19].Visible = true;
                        e.Row.Cells[20].Visible = true;
                        e.Row.Cells[21].Visible = true;
                        e.Row.Cells[22].Visible = true;
                        e.Row.Cells[23].Visible = true;
                        gvCustomerSummary.Width = Unit.Pixel(1600);
                        break;
                }

                Label lblAmount = (Label)e.Row.FindControl("lblAmount");
                Label lblPriorYearRangeAmount = (Label)e.Row.FindControl("lblPriorYearRangeAmount");

                Label lblMargin = (Label)e.Row.FindControl("lblMargin");
                Label lblYTD = (Label)e.Row.FindControl("lblYTD");
                Label lblPriorYTD = (Label)e.Row.FindControl("lblPriorYTD");
                Label lblChangePercentage = (Label)e.Row.FindControl("lblChangePercentage");
                Label lblYearMinus1 = (Label)e.Row.FindControl("lblYearMinus1");
                Label lblYearMinus2 = (Label)e.Row.FindControl("lblYearMinus2");
                Label lblYearMinus3 = (Label)e.Row.FindControl("lblYearMinus3");
                Label lblYearMinus4 = (Label)e.Row.FindControl("lblYearMinus4");
                Label lblYearMinus5 = (Label)e.Row.FindControl("lblYearMinus5");


                Label lblPriorYearRangeMargin = (Label)e.Row.FindControl("lblPriorYearRangeMargin");
                Label lblCurrentYTDMargin = (Label)e.Row.FindControl("lblCurrentYTDMargin");
                Label lblYearMinus1Margin = (Label)e.Row.FindControl("lblYearMinus1Margin");
                Label lblYearMinus2Margin = (Label)e.Row.FindControl("lblYearMinus2Margin");
                Label lblYearMinus3Margin = (Label)e.Row.FindControl("lblYearMinus3Margin");
                Label lblYearMinus4Margin = (Label)e.Row.FindControl("lblYearMinus4Margin");
                Label lblYearMinus5Margin = (Label)e.Row.FindControl("lblYearMinus5Margin");



                //Amount...
                if (lblAmount.Text != "")
                {
                    dcAmount = Convert.ToDecimal(lblAmount.Text.Trim());
                    dcTotalAmountSummary += dcAmount;
                    lblAmount.Text = "$" + Convert.ToDecimal(lblAmount.Text.Trim()).ToString("#,0");
                }
                if (lblPriorYearRangeAmount.Text != "")
                {
                    dcAmountPrior = Convert.ToDecimal(lblPriorYearRangeAmount.Text.Trim());
                    dcTotalAmountPriorSummary += dcAmountPrior;
                    lblPriorYearRangeAmount.Text = "$" + Convert.ToDecimal(lblPriorYearRangeAmount.Text.Trim()).ToString("#,0");
                }
                if (lblYTD.Text != "")
                {
                    dcYTD = Convert.ToDecimal(lblYTD.Text.Trim());
                    dcYTDTotalSummary += dcYTD;
                    lblYTD.Text = "$" + Convert.ToDecimal(lblYTD.Text).ToString("#,0");
                }
                if (lblYTD.Text != "$0")
                {
                    if (lblPriorYTD.Text != "")
                    {
                        if (lblPriorYTD.Text != "0")
                        {

                            dcPriorYTD = Convert.ToDecimal(lblPriorYTD.Text.Trim());
                            dcPriorYTDTotalSummary += dcPriorYTD;

                            decimal dcDiff = 0;

                            if (dcPriorYTD == 0)
                            {
                                dcPriorYTD = 1;
                            }
                            dcDiff = ((dcYTD - dcPriorYTD) / dcPriorYTD) * 100;
                            if (dcPriorYTD > dcYTD)
                            {//Loss from prior year...
                                lblChangePercentage.ForeColor = Color.Red;

                                if ((dcDiff * -1) > 10)
                                {
                                    lblChangePercentage.ForeColor = Color.Red;
                                    lblChangePercentage.Font.Bold = true;
                                }
                                lblChangePercentage.Text = "(" + (dcDiff * -1).ToString("#,0") + ")" + "%";
                            }
                            else
                            {//Better than last year...
                                lblChangePercentage.Text = ((dcDiff * -1) * -1).ToString("#,0") + "%";
                            }
                            lblPriorYTD.Text = "$" + Convert.ToDecimal(lblPriorYTD.Text).ToString("#,0");
                        }
                        else//Prior YTD = "$0"
                        {
                            lblPriorYTD.Text = "$0";
                            lblChangePercentage.Text = "--";
                        }
                    }
                }
                else
                {
                    if (lblYTD.Text == "$0")
                    {
                        if (lblPriorYTD.Text != "0")
                        {
                            dcPriorYTD = Convert.ToDecimal(lblPriorYTD.Text.Trim());
                            if (lblYTD.Text == "$0" && dcPriorYTD != 0)
                            {
                                lblChangePercentage.Text = "100%";
                            }
                        }
                        else//Both Zeros...
                        {
                            lblPriorYTD.Text = "$0";
                            lblChangePercentage.Text = "--";
                        }
                    }
                }
                if (lblYearMinus1.Text != "")
                {
                    dcYearMinus1 = Convert.ToDecimal(lblYearMinus1.Text.Trim());
                    dcYearMinus1TotalSummary += dcYearMinus1;
                    lblYearMinus1.Text = "$" + Convert.ToDecimal(lblYearMinus1.Text).ToString("#,0");
                }
                if (lblYearMinus2.Text != "")
                {
                    dcYearMinus2 = Convert.ToDecimal(lblYearMinus2.Text.Trim());
                    dcYearMinus2TotalSummary += dcYearMinus2;
                    lblYearMinus2.Text = "$" + Convert.ToDecimal(lblYearMinus2.Text).ToString("#,0");
                }
                if (lblYearMinus3.Text != "")
                {
                    dcYearMinus3 = Convert.ToDecimal(lblYearMinus3.Text.Trim());
                    dcYearMinus3TotalSummary += dcYearMinus3;
                    lblYearMinus3.Text = "$" + Convert.ToDecimal(lblYearMinus3.Text).ToString("#,0");
                }
                if (lblYearMinus4.Text != "")
                {
                    dcYearMinus4 = Convert.ToDecimal(lblYearMinus4.Text.Trim());
                    dcYearMinus4TotalSummary += dcYearMinus4;
                    lblYearMinus4.Text = "$" + Convert.ToDecimal(lblYearMinus4.Text).ToString("#,0");
                }
                if (lblYearMinus5.Text != "")
                {
                    dcYearMinus5 = Convert.ToDecimal(lblYearMinus5.Text.Trim());
                    dcYearMinus5TotalSummary += dcYearMinus5;
                    lblYearMinus5.Text = "$" + Convert.ToDecimal(lblYearMinus5.Text).ToString("#,0");
                }
                Label lblRangeCost = (Label)e.Row.FindControl("lblRangeCost");
                Label lblCurrentYTDCost = (Label)e.Row.FindControl("lblCurrentYTDCost");
                Label lblYearMinus1Cost = (Label)e.Row.FindControl("lblYearMinus1Cost");
                Label lblYearMinus2Cost = (Label)e.Row.FindControl("lblYearMinus2Cost");
                Label lblYearMinus3Cost = (Label)e.Row.FindControl("lblYearMinus3Cost");
                Label lblYearMinus4Cost = (Label)e.Row.FindControl("lblYearMinus4Cost");
                Label lblYearMinus5Cost = (Label)e.Row.FindControl("lblYearMinus5Cost");
                Label lblPriorYearRangeCost = (Label)e.Row.FindControl("lblPriorYearRangeCost");



                //Cost...
                if (lblRangeCost.Text != "")
                {
                    dcCostRange = Convert.ToDecimal(lblRangeCost.Text.Trim());
                    dcCostRangeSummary += dcCostRange;
                }
                if (lblPriorYearRangeCost.Text != "")
                {
                    dcTotalCostPrior = Convert.ToDecimal(lblPriorYearRangeCost.Text.Trim());
                    dcTotalCostPriorSummary += dcTotalCostPrior;
                }
                if (lblCurrentYTDCost.Text != "")
                {
                    dcYTDCost = Convert.ToDecimal(lblCurrentYTDCost.Text.Trim());
                    dcYTDCostSummary += dcYTDCost;
                }
                if (lblYearMinus1Cost.Text != "")
                {
                    dcYearMinus1Cost = Convert.ToDecimal(lblYearMinus1Cost.Text.Trim());
                    dcYearMinus1CostSummary += dcYearMinus1Cost;
                }
                if (lblYearMinus2Cost.Text != "")
                {
                    dcYearMinus2Cost = Convert.ToDecimal(lblYearMinus2Cost.Text.Trim());
                    dcYearMinus2CostSummary += dcYearMinus2Cost;
                }
                if (lblYearMinus3Cost.Text != "")
                {
                    dcYearMinus3Cost = Convert.ToDecimal(lblYearMinus3Cost.Text.Trim());
                    dcYearMinus3CostSummary += dcYearMinus3Cost;
                }
                if (lblYearMinus4Cost.Text != "")
                {
                    dcYearMinus4Cost = Convert.ToDecimal(lblYearMinus4Cost.Text.Trim());
                    dcYearMinus4CostSummary += dcYearMinus4Cost;
                }
                if (lblYearMinus5Cost.Text != "")
                {
                    dcYearMinus5Cost = Convert.ToDecimal(lblYearMinus5Cost.Text.Trim());
                    dcYearMinus5CostSummary += dcYearMinus5Cost;
                }


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
                            lblMargin.Text = Convert.ToDecimal(lblMargin.Text.Replace("%", "")).ToString("0.0") + "%";
                            if (lblMargin.Text != "")
                            {
                                lblMargin.Text = Convert.ToDecimal(lblMargin.Text.Replace("%", "")).ToString("0.0") + "%";
                            }
                            if (lblPriorYearRangeMargin.Text != "")
                            {
                                lblPriorYearRangeMargin.Text = Convert.ToDecimal(lblPriorYearRangeMargin.Text).ToString("0.0") + "%";
                            }
                            if (lblCurrentYTDMargin.Text != "")
                            {
                                lblCurrentYTDMargin.Text = Convert.ToDecimal(lblCurrentYTDMargin.Text).ToString("0.0") + "%";
                            }
                            if (lblYearMinus1Margin.Text != "")
                            {
                                lblYearMinus1Margin.Text = Convert.ToDecimal(lblYearMinus1Margin.Text).ToString("0.0") + "%";
                            }
                            if (lblYearMinus2Margin.Text != "")
                            {
                                lblYearMinus2Margin.Text = Convert.ToDecimal(lblYearMinus2Margin.Text).ToString("0.0") + "%";
                            }
                            if (lblYearMinus3Margin.Text != "")
                            {
                                lblYearMinus3Margin.Text = Convert.ToDecimal(lblYearMinus3Margin.Text).ToString("0.0") + "%";
                            }
                            if (lblYearMinus4Margin.Text != "")
                            {
                                lblYearMinus4Margin.Text = Convert.ToDecimal(lblYearMinus4Margin.Text).ToString("0.0") + "%";
                            }
                            if (lblYearMinus5Margin.Text != "")
                            {
                                lblYearMinus5Margin.Text = Convert.ToDecimal(lblYearMinus5Margin.Text).ToString("0.0") + "%";
                            }
                            break;
                        case 2://Supervisor...
                            lblMargin.Text = Convert.ToDecimal(lblMargin.Text.Replace("%", "")).ToString("0.0") + "%";
                            if (lblMargin.Text != "")
                            {
                                lblMargin.Text = Convert.ToDecimal(lblMargin.Text.Replace("%", "")).ToString("0.0") + "%";
                            }
                            if (lblPriorYearRangeMargin.Text != "")
                            {
                                lblPriorYearRangeMargin.Text = Convert.ToDecimal(lblPriorYearRangeMargin.Text).ToString("0.0") + "%";
                            }
                            if (lblCurrentYTDMargin.Text != "")
                            {
                                lblCurrentYTDMargin.Text = Convert.ToDecimal(lblCurrentYTDMargin.Text).ToString("0.0") + "%";
                            }
                            if (lblYearMinus1Margin.Text != "")
                            {
                                lblYearMinus1Margin.Text = Convert.ToDecimal(lblYearMinus1Margin.Text).ToString("0.0") + "%";
                            }
                            if (lblYearMinus2Margin.Text != "")
                            {
                                lblYearMinus2Margin.Text = Convert.ToDecimal(lblYearMinus2Margin.Text).ToString("0.0") + "%";
                            }
                            if (lblYearMinus3Margin.Text != "")
                            {
                                lblYearMinus3Margin.Text = Convert.ToDecimal(lblYearMinus3Margin.Text).ToString("0.0") + "%";
                            }
                            if (lblYearMinus4Margin.Text != "")
                            {
                                lblYearMinus4Margin.Text = Convert.ToDecimal(lblYearMinus4Margin.Text).ToString("0.0") + "%";
                            }
                            if (lblYearMinus5Margin.Text != "")
                            {
                                lblYearMinus5Margin.Text = Convert.ToDecimal(lblYearMinus5Margin.Text).ToString("0.0") + "%";
                            }
                            break;
                        default:
                            lblMargin.Text = "--";
                            if (lblMargin.Text != "")
                            {
                                lblMargin.Text = "--";
                            }
                            if (lblPriorYearRangeMargin.Text != "")
                            {
                                lblPriorYearRangeMargin.Text = "--";
                            }
                            if (lblCurrentYTDMargin.Text != "")
                            {
                                lblCurrentYTDMargin.Text = "--";
                            }
                            if (lblYearMinus1Margin.Text != "")
                            {
                                lblYearMinus1Margin.Text = "--";
                            }
                            if (lblYearMinus2Margin.Text != "")
                            {
                                lblYearMinus2Margin.Text = "--";
                            }
                            if (lblYearMinus3Margin.Text != "")
                            {
                                lblYearMinus3Margin.Text = "--";
                            }
                            if (lblYearMinus4Margin.Text != "")
                            {
                                lblYearMinus4Margin.Text = "--";
                            }
                            if (lblYearMinus5Margin.Text != "")
                            {
                                lblYearMinus5Margin.Text = "--";
                            }
                            break;
                    }

                }


                List<string> lEndCustomers = new List<string>();
                lEndCustomers = SharedFunctions.GetEndCustomer();
                List<string> lGroupMembers = new List<string>();
                lGroupMembers = SharedFunctions.GetGrouping();
                Label lblCustomer = (Label)e.Row.FindControl("lblCustomer");
                Label lblName = (Label)e.Row.FindControl("lblName");
                if (lEndCustomers.Contains(lblCustomer.Text))
                {
                    lblName.ForeColor = Color.Red;
                    lblName.Font.Bold = true;
                }
                if (lGroupMembers.Contains(lblCustomer.Text))
                {
                    lblName.ForeColor = Color.Blue;
                    lblName.Font.Bold = true;
                }
                lblName.ToolTip = GetCustomerDetails(lblCustomer.Text);
                lblName.Style.Add("cursor", "pointer");

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblAmountTotal = (Label)e.Row.FindControl("lblAmountTotal");
                Label lblMarginAvgTotal = (Label)e.Row.FindControl("lblMarginAvgTotal");
                Label lblPriorYearRangeAmountTotal = (Label)e.Row.FindControl("lblPriorYearRangeAmountTotal");
                Label lblYTDTotal = (Label)e.Row.FindControl("lblYTDTotal");
                Label lblPriorYTDTotal = (Label)e.Row.FindControl("lblPriorYTDTotal");
                Label lblYearMinus1Total = (Label)e.Row.FindControl("lblYearMinus1Total");
                Label lblYearMinus2Total = (Label)e.Row.FindControl("lblYearMinus2Total");
                Label lblYearMinus3Total = (Label)e.Row.FindControl("lblYearMinus3Total");
                Label lblYearMinus4Total = (Label)e.Row.FindControl("lblYearMinus4Total");
                Label lblYearMinus5Total = (Label)e.Row.FindControl("lblYearMinus5Total");

                lblPriorYearRangeAmountTotal.Text = "$" + dcTotalAmountPriorSummary.ToString("#,0");
                lblAmountTotal.Text = "$" + dcTotalAmountSummary.ToString("#,0");
                lblYTDTotal.Text = "$" + dcYTDTotalSummary.ToString("#,0");
                lblPriorYTDTotal.Text = "$" + dcPriorYTDTotalSummary.ToString("#,0");
                lblYearMinus1Total.Text = "$" + dcYearMinus1TotalSummary.ToString("#,0");
                lblYearMinus2Total.Text = "$" + dcYearMinus2TotalSummary.ToString("#,0");
                lblYearMinus3Total.Text = "$" + dcYearMinus3TotalSummary.ToString("#,0");
                lblYearMinus4Total.Text = "$" + dcYearMinus4TotalSummary.ToString("#,0");
                lblYearMinus5Total.Text = "$" + dcYearMinus5TotalSummary.ToString("#,0");
                lblPriceTotal.Text = "Price Total: " + lblAmountTotal.Text;

                Label lblMarginAvg = (Label)e.Row.FindControl("lblMarginAvg");
                Label lblPriorYearRangeMarginAvg = (Label)e.Row.FindControl("lblPriorYearRangeMarginAvg");
                Label lblCurrentYTDMarginAvg = (Label)e.Row.FindControl("lblCurrentYTDMarginAvg");
                Label lblYearMinus1MarginAvg = (Label)e.Row.FindControl("lblYearMinus1MarginAvg");
                Label lblYearMinus2MarginAvg = (Label)e.Row.FindControl("lblYearMinus2MarginAvg");
                Label lblYearMinus3MarginAvg = (Label)e.Row.FindControl("lblYearMinus3MarginAvg");
                Label lblYearMinus4MarginAvg = (Label)e.Row.FindControl("lblYearMinus4MarginAvg");
                Label lblYearMinus5MarginAvg = (Label)e.Row.FindControl("lblYearMinus5MarginAvg");


                if (dcTotalAmountSummary == 0) { dcTotalAmountSummary = 1; }
                dcMarginRangeSummaryAvg = ((dcTotalAmountSummary - dcCostRangeSummary) / dcTotalAmountSummary) * 100;
                if (dcMarginRangeSummaryAvg == 100) { dcMarginRangeSummaryAvg = 0; }

                if (dcYTDTotalSummary == 0) { dcYTDTotalSummary = 1; }
                dcYTDMarginSummaryAvg = ((dcYTDTotalSummary - dcYTDCostSummary) / dcYTDTotalSummary) * 100;
                if (dcYTDMarginSummaryAvg == 100) { dcYTDMarginSummaryAvg = 0; }

                if (dcTotalAmountPriorSummary == 0) { dcTotalAmountPriorSummary = 1; }
                dcTotalMarginPriorSummaryAvg = ((dcTotalAmountPriorSummary - dcTotalCostPriorSummary) / dcTotalAmountPriorSummary) * 100;
                if (dcTotalMarginPriorSummaryAvg == 100) { dcTotalMarginPriorSummaryAvg = 0; }

                if (dcYearMinus1TotalSummary == 0) { dcYearMinus1TotalSummary = 1; }
                dcYearMinus1MarginSummaryAvg = ((dcYearMinus1TotalSummary - dcYearMinus1CostSummary) / dcYearMinus1TotalSummary) * 100;
                if (dcYearMinus1MarginSummaryAvg == 100) { dcYearMinus1MarginSummaryAvg = 0; }

                if (dcYearMinus2TotalSummary == 0) { dcYearMinus2TotalSummary = 1; }
                dcYearMinus2MarginSummaryAvg = ((dcYearMinus2TotalSummary - dcYearMinus2CostSummary) / dcYearMinus2TotalSummary) * 100;
                if (dcYearMinus2MarginSummaryAvg == 100) { dcYearMinus2MarginSummaryAvg = 0; }

                if (dcYearMinus3TotalSummary == 0) { dcYearMinus3TotalSummary = 1; }
                dcYearMinus3MarginSummaryAvg = ((dcYearMinus3TotalSummary - dcYearMinus3CostSummary) / dcYearMinus3TotalSummary) * 100;
                if (dcYearMinus3MarginSummaryAvg == 100) { dcYearMinus3MarginSummaryAvg = 0; }

                if (dcYearMinus4TotalSummary == 0) { dcYearMinus4TotalSummary = 1; }
                dcYearMinus4MarginSummaryAvg = ((dcYearMinus4TotalSummary - dcYearMinus4CostSummary) / dcYearMinus4TotalSummary) * 100;
                if (dcYearMinus4MarginSummaryAvg == 100) { dcYearMinus4MarginSummaryAvg = 0; }

                if (dcYearMinus5TotalSummary == 0) { dcYearMinus5TotalSummary = 1; }
                dcYearMinus5MarginSummaryAvg = ((dcYearMinus5TotalSummary - dcYearMinus5CostSummary) / dcYearMinus5TotalSummary) * 100;
                if (dcYearMinus5MarginSummaryAvg == 100) { dcYearMinus5MarginSummaryAvg = 0; }

                lblPriorYearRangeMarginAvg.Text = dcTotalMarginPriorSummaryAvg.ToString("#,0") + "%";
                lblMarginAvg.Text = dcMarginRangeSummaryAvg.ToString("#,0") + "%";
                lblCurrentYTDMarginAvg.Text = dcYTDMarginSummaryAvg.ToString("#,0") + "%";
                lblYearMinus1MarginAvg.Text = dcYearMinus1MarginSummaryAvg.ToString("0.0") + "%";
                lblYearMinus2MarginAvg.Text = dcYearMinus2MarginSummaryAvg.ToString("0.0") + "%";
                lblYearMinus3MarginAvg.Text = dcYearMinus3MarginSummaryAvg.ToString("0.0") + "%";
                lblYearMinus4MarginAvg.Text = dcYearMinus4MarginSummaryAvg.ToString("0.0") + "%";
                lblYearMinus5MarginAvg.Text = dcYearMinus5MarginSummaryAvg.ToString("0.0") + "%";

                switch (ddlYearsBack.SelectedValue)
                {
                    case "1":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = false;
                        e.Row.Cells[17].Visible = false;
                        e.Row.Cells[18].Visible = false;
                        e.Row.Cells[19].Visible = false;
                        e.Row.Cells[20].Visible = false;
                        e.Row.Cells[21].Visible = false;
                        e.Row.Cells[22].Visible = false;
                        e.Row.Cells[23].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1200);
                        break;
                    case "2":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = true;
                        e.Row.Cells[17].Visible = true;
                        e.Row.Cells[18].Visible = false;
                        e.Row.Cells[19].Visible = false;
                        e.Row.Cells[20].Visible = false;
                        e.Row.Cells[21].Visible = false;
                        e.Row.Cells[22].Visible = false;
                        e.Row.Cells[23].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1300);
                        break;
                    case "3":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = true;
                        e.Row.Cells[17].Visible = true;
                        e.Row.Cells[18].Visible = true;
                        e.Row.Cells[19].Visible = true;
                        e.Row.Cells[20].Visible = false;
                        e.Row.Cells[21].Visible = false;
                        e.Row.Cells[22].Visible = false;
                        e.Row.Cells[23].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1400);
                        break;
                    case "4":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = true;
                        e.Row.Cells[17].Visible = true;
                        e.Row.Cells[18].Visible = true;
                        e.Row.Cells[19].Visible = true;
                        e.Row.Cells[20].Visible = true;
                        e.Row.Cells[21].Visible = true;
                        e.Row.Cells[22].Visible = false;
                        e.Row.Cells[23].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1500);
                        break;
                    case "5":
                        e.Row.Cells[14].Visible = true;
                        e.Row.Cells[15].Visible = true;
                        e.Row.Cells[16].Visible = true;
                        e.Row.Cells[17].Visible = true;
                        e.Row.Cells[18].Visible = true;
                        e.Row.Cells[19].Visible = true;
                        e.Row.Cells[20].Visible = true;
                        e.Row.Cells[21].Visible = true;
                        e.Row.Cells[22].Visible = true;
                        e.Row.Cells[23].Visible = true;
                        gvCustomerSummary.Width = Unit.Pixel(1600);
                        break;
                }
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

        DataTable dt = (DataTable)Session["dtReportSummary"];
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
        string sMsg = "";
        if (txtStartDate.Text.Trim() == "")
        {
            sMsg = "**Start Date can't be left blank!!<br>";
        }
        if (txtEndDate.Text.Trim() == "")
        {
            sMsg += "**End Date can't be left blank!!";
        }
        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            lblError.ForeColor = Color.Red;
            return;
        }
        lblError.Text = "";
        lblPriceTotal.Text = "";
        lblMarginWeightedAvg.Text = "";
        RunReport();
        gvCustomerSummary.DataSource = null;
        gvCustomerSummary.DataBind();
        trFullReport.Visible = true;
        trSummary.Visible = false;
        GetTrendsReports();

        string sURL = "";
        sURL = "TrendsReportPopup.aspx";

        lbnTrendsReport.ToolTip = "Click to launch the Purchasing Trends Report Page";
        lbnTrendsReport.Attributes.Add("onclick", "javascript: window.open('" + sURL + "', '_blank','HEIGHT=800,WIDTH=1500,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
        lbnTrendsReport.Style.Add("Cursor", "pointer");
        string sURL2 = "";
        sURL2 = "TrendsReportByQtyPopup.aspx";
        lbnTrendsReportByQty.ToolTip = "Click to launch the Purchasing Trends Report Page";
        lbnTrendsReportByQty.Attributes.Add("onclick", "javascript: window.open('" + sURL2 + "', '_blank','HEIGHT=800,WIDTH=1500,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
        lbnTrendsReportByQty.Style.Add("Cursor", "pointer");

        ViewState["FullReportClicked"] = true;
        ViewState["SummaryReportClicked"] = false;
    }
    protected void btnRunReportSummary_Click(object sender, EventArgs e)
    {
        string sMsg = "";
        if (txtStartDate.Text.Trim() == "")
        {
            sMsg = "**Start Date can't be left blank!!<br>";
        }
        if (txtEndDate.Text.Trim() == "")
        {
            sMsg += "**End Date can't be left blank!!";
        }
        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            lblError.ForeColor = Color.Red;
            return;
        }
        RunReportSummary();
        gvReportCondensed.DataSource = null;
        gvReportCondensed.DataBind();
        trFullReport.Visible = false;
        trSummary.Visible = true;
        GetTrendsReportsSummary();

        string sURL = "";
        sURL = "TrendsReportPopupSummary.aspx";

        lbnTrendsReport.ToolTip = "Click to launch the Purchasing Trends Report Page";
        lbnTrendsReport.Attributes.Add("onclick", "javascript: window.open('" + sURL + "', '_blank','HEIGHT=800,WIDTH=1500,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
        lbnTrendsReport.Style.Add("Cursor", "pointer");
        string sURL2 = "";
        sURL2 = "TrendsReportByQtyPopupSummary.aspx";
        lbnTrendsReportByQty.ToolTip = "Click to launch the Purchasing Trends Report Page";
        lbnTrendsReportByQty.Attributes.Add("onclick", "javascript: window.open('" + sURL2 + "', '_blank','HEIGHT=800,WIDTH=1500,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
        lbnTrendsReportByQty.Style.Add("Cursor", "pointer");

        ViewState["FullReportClicked"] = false;
        ViewState["SummaryReportClicked"] = true;
    }
    protected void btnPreview0_Click(object sender, EventArgs e)
    {
        lblErrorPrior.Text = "";
        //RunReport2();
        //RunReport();
    }
    protected void lbCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlEndCustomers.SelectedIndex = 0;
        if ((bool)ViewState["FullReportClicked"] == true)
        {
            btnPreview_Click(btnPreview, null);
        }
        if ((bool)ViewState["SummaryReportClicked"] == true)
        {
            btnRunReportSummary_Click(btnRunReportSummary, null);
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
    protected void rblSort_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer(lbCustomers, rblSort.SelectedValue);
    }
    protected void btnExportFullReport_Click(object sender, EventArgs e)
    {
        string sMsg = "";
        if (txtStartDate.Text.Trim() == "")
        {
            sMsg = "**Start Date can't be left blank!!<br>";
        }
        if (txtEndDate.Text.Trim() == "")
        {
            sMsg += "**End Date can't be left blank!!";
        }
        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            lblError.ForeColor = Color.Red;
            return;
        }
        ExportFullReport();
    }

    protected void btnExportSummary_Click(object sender, EventArgs e)
    {
        string sMsg = "";
        if (txtStartDate.Text.Trim() == "")
        {
            sMsg = "**Start Date can't be left blank!!<br>";
        }
        if (txtEndDate.Text.Trim() == "")
        {
            sMsg += "**End Date can't be left blank!!";
        }
        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            lblError.ForeColor = Color.Red;
            return;
        }
        ExportSummary();
    }

    protected void ddlEndCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbCustomers.SelectedIndex = -1;
        LoadStockCodesOfEndCustomer(Convert.ToInt32(ddlEndCustomers.SelectedValue));
    }
    protected void lbnSelectAllStockCodeEndCustomer_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbParentStockCodeEndCustomer.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearStockCodeEndCustomer_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbParentStockCodeEndCustomer.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
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
        GetTrendsReports();
    }
    protected void lbnTrendsReportByQty_Click(object sender, EventArgs e)
    {
        GetTrendsReports();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {

        txtStartDate.Text = "";
        txtEndDate.Text = "";
        txtMarginFrom.Text = "";
        txtMarginTo.Text = "";
        txtStockCodeFrom.Text = "000000";
        txtStockCodeTo.Text = "999999";

        ddlProductClass.SelectedIndex = 0;
        ddlSalesPerson.SelectedIndex = 0;
        ddlPeriod.SelectedIndex = 0;
        ddlEndCustomers.SelectedIndex = 0;
        ddlYear.SelectedIndex = 0;

        lbParentStockCodeEndCustomer.Items.Clear();
        lbCustomers.SelectedIndex = -1;
        lbCities.SelectedIndex = -1;
        lbZips.SelectedIndex = -1;
        lblStockCodeList.Text = "";

        txtStartDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString();
        txtEndDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month)).ToShortDateString();

        ddlPeriod.SelectedIndex = 2;//Current month...
        //txtStartDate.Text = SharedFunctions.GetOldestTrnDateInDatabase();
        //txtEndDate.Text = new DateTime(DateTime.Now.Year, 12, 31).ToShortDateString();

        PanelSetup();

        //if ((bool)ViewState["FullReportClicked"] == true)
        //{
        //    btnPreview_Click(btnPreview, null);
        //}
        //if ((bool)ViewState["SummaryReportClicked"] == true)
        //{
        //    btnRunReportSummary_Click(btnRunReportSummary, null);
        //}
    }
    protected void rblSingleOrStockCodeRange_SelectedIndexChanged(object sender, EventArgs e)
    {
        PanelSetup();
    }
    protected void ddlYearsBack_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnRunReportSummary_Click(btnRunReportSummary, null);
    }

    protected void lbClearAllCities_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbCities.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    protected void lbClearAllZips_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbZips.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
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
            // Step 1: Retrieve all StockCodes into an array
            string[] stockCodes = db.ArSalesMove
                .Where(w => w.StockCode != null)
                .Select(w => w.StockCode.Trim())
                .Distinct()
                .ToArray();

            // Step 2: Filter the array to include only numeric values
            string[] filteredStockCodes = stockCodes
                .Where(code => code.All(char.IsDigit))
                .OrderBy(code => code)
                .ToArray();

            string[] lResult = null;
            try
            {
                lResult = (from d in filteredStockCodes where d.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select d).Take(count).ToArray();
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