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

public partial class CustomerReportsForSalesperson : System.Web.UI.Page
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
        lStockCodeDescListReport = GetNonCustomerStockCodes(iNonCustomerID);
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
    private void LoadCustomer(DropDownList ddl, string sSortBy, string sSalespersonID)
    {
        ddl.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        if (sSortBy == "Name")
        {
            var query = (from c in db.ArCustomer
                         where c.Customer != "test"
                         && c.Salesperson == sSalespersonID
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
                         && c.Salesperson == sSalespersonID
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
    private void LoadNonCustomers(DropDownList ddl, string sSalespersonID)
    {
        ddl.Items.Clear();
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from c in db.ArNonCustomer
                     where c.Salesperson == sSalespersonID
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
    private void LoadCustomerCities(ListBox ddl, string sSalespersonID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddl.Items.Clear();
            var query = (from ac in db.ArCustomer
                         where ac.Salesperson == sSalespersonID &&
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
    private void LoadCustomerZips(ListBox ddl, string sSalespersonID)
    {

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddl.Items.Clear();
            var query = (from ac in db.ArCustomer
                         where ac.Salesperson == sSalespersonID &&
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

    private void RunReportSummaryOld(string sSalespersonUserID)
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

        string sToMargin = "NULL";

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
            lStockCodes = GetNoCustomerStockCodes(iNonCustomerID);
            int iIndexOfPipe = 0;
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
        if (Mode == "Single")
        {//Single...
            if (sStockCode != "")//Selected Stock Codes...
            {

                sSQL = "EXEC spGetCustomerReportSummary ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@FromMargin=" + sFromMargin + ",";
                sSQL += "@ToMargin=" + sToMargin + ",";
                sSQL += "@StockCodes=" + sStockCode + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@Year=" + sYear + ",";
                sSQL += "@ProductClass=" + sProductClass + ",";
                sSQL += "@SalesPerson=" + sSalespersonUserID;
            }
            else//All Stock Codes...
            {
                sSQL = "EXEC spGetCustomerReportSummary ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@FromMargin=" + sFromMargin + ",";
                sSQL += "@ToMargin=" + sToMargin + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@Year=" + sYear + ",";
                sSQL += "@ProductClass=" + sProductClass + ",";
                sSQL += "@SalesPerson=" + sSalespersonUserID;
            }
        }
        else if (Mode == "Range")//Stock Code Range...
        {
            sSQL = "EXEC spGetCustomerReportSummaryByRange ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@FromMargin=" + sFromMargin + ",";
            sSQL += "@ToMargin=" + sToMargin + ",";
            sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
            sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
            sSQL += "@Customer=" + sCustomer + ",";
            sSQL += "@Year=" + sYear + ",";
            sSQL += "@ProductClass=" + sProductClass + ",";
            sSQL += "@IncludeEndCustomer=0,";
            sSQL += "@IncludeGrouping=0,";
            sSQL += "@SalesPerson=" + sSalespersonUserID;
        }
        else if (Mode == "EndCustomer")//End Customer...
        {
            sSQL = "EXEC spGetCustomerReportSummary ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@FromMargin=" + sFromMargin + ",";
            sSQL += "@ToMargin=" + sToMargin + ",";
            sSQL += "@StockCodes=" + sStockCode + ",";
            sSQL += "@Customer=" + sCustomer + ",";
            sSQL += "@Year=" + sYear + ",";
            sSQL += "@ProductClass=" + sProductClass + ",";
            sSQL += "@SalesPerson=" + sSalespersonUserID;
        }
        else
        {
            sSQL = "EXEC spGetCustomerReportSummary ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@FromMargin=" + sFromMargin + ",";
            sSQL += "@ToMargin=" + sToMargin + ",";
            sSQL += "@StockCodes=" + sStockCode + ",";
            sSQL += "@Customer=" + sCustomer + ",";
            sSQL += "@Year=" + sYear + ",";
            sSQL += "@ProductClass=" + sProductClass + ",";
            sSQL += "@SalesPerson=" + sSalespersonUserID;
        }



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
                    headerCell.Width = Unit.Pixel(88);
                    break;
                case 1://Customer#
                    headerCell.Width = Unit.Pixel(62);
                    break;
                case 2://SalesPerson
                    headerCell.Width = Unit.Pixel(88);
                    break;
                case 3://StockCode
                    headerCell.Width = Unit.Pixel(77);
                    break;
                case 4://Decription
                    headerCell.Width = Unit.Pixel(90);
                    break;
                case 5://ProductClass
                    headerCell.Width = Unit.Pixel(90);
                    break;
                case 6://Uom
                    headerCell.Width = col.ItemStyle.Width;// Unit.Pixel(40);
                    break;
                case 7://Qty
                    headerCell.Width = col.ItemStyle.Width;// Unit.Pixel(35);
                    break;
                case 8://Amount
                    headerCell.Width = col.ItemStyle.Width;// Unit.Pixel(45);
                    break;
                case 9://PriceCode
                    headerCell.Width = col.ItemStyle.Width;// Unit.Pixel(25);
                    break;
                case 10://Price
                    headerCell.Width = col.ItemStyle.Width;// Unit.Pixel(50);
                    break;
                case 11://LastChangeDate
                    headerCell.Width = col.ItemStyle.Width;// Unit.Pixel(45);
                    break;
                case 12://YTD
                    headerCell.Width = col.ItemStyle.Width;// Unit.Pixel(50);
                    break;
                case 13://YearMinus1
                    headerCell.Text = DateTime.Now.AddYears(-1).Year.ToString();
                    headerCell.Width = col.ItemStyle.Width;// Unit.Pixel(60);
                    break;
                case 14://YearMinus2
                    headerCell.Text = DateTime.Now.AddYears(-2).Year.ToString();
                    headerCell.Width = col.ItemStyle.Width;// Unit.Pixel(60);
                    break;
                case 15://YearMinus3
                    headerCell.Text = DateTime.Now.AddYears(-3).Year.ToString();
                    headerCell.Width = col.ItemStyle.Width;// Unit.Pixel(60);
                    break;
                case 16://YearMinus4
                    headerCell.Text = DateTime.Now.AddYears(-4).Year.ToString();
                    headerCell.Width = col.ItemStyle.Width;// Unit.Pixel(60);
                    break;
                case 17://YearMinus5
                    headerCell.Text = DateTime.Now.AddYears(-5).Year.ToString();
                    headerCell.Width = col.ItemStyle.Width;// Unit.Pixel(60);
                    break;

            }


            //headerCell.Width = col.ItemStyle.Width;
            headerRow.Cells.Add(headerCell);

        }

        tblHeaderTable.Rows.Add(headerRow);
    }

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
        lblError.Text = "";
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

        if (sStartDate == "" && sEndDate != "")
        {
            sMsg += "**If there is an End Date there must be a Start Date!<br/>";
        }
        if (sStartDate != "" && sEndDate == "")
        {
            sMsg += "**If there is an Start Date there must be a End Date!<br/>";
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
        if (ddlCustomers.SelectedIndex != 0)
        {
            sCustomer = ddlCustomers.SelectedValue;
        }
        else
        {
            sCustomer = "";
        }

        string sUserID = Session["UserID"].ToString();



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
        int iUserID = Convert.ToInt32(Session["UserID"]);
        string sSalesperson = "";
        sSalesperson = GetSalesperson(iUserID);
        RunTrendsReportDollars(sStartDate, sEndDate, sStockCodeFrom, sStockCodeTo, sCustomer, sEndCustomer, sUserID, sSalesperson);
        RunTrendsReportQuantity(sStartDate, sEndDate, sStockCodeFrom, sStockCodeTo, sCustomer, sEndCustomer, sUserID, sSalesperson);


    }
    private void GetTrendsReportsSummary()
    {
        lblError.Text = "";
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

        if (sStartDate == "" && sEndDate != "")
        {
            sMsg += "**If there is an End Date there must be a Start Date!<br/>";
        }
        if (sStartDate != "" && sEndDate == "")
        {
            sMsg += "**If there is an Start Date there must be a End Date!<br/>";
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
        if (ddlCustomers.SelectedIndex != 0)
        {
            sCustomer = ddlCustomers.SelectedValue;
        }
        else
        {
            sCustomer = "";
        }

        string sUserID = Session["UserID"].ToString();



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
        int iUserID = Convert.ToInt32(Session["UserID"]);
        string sSalesperson = "";
        sSalesperson = GetSalesperson(iUserID);
        RunTrendsReportDollarsSummary(sStartDate, sEndDate, sStockCodeFrom, sStockCodeTo, sCustomer, sEndCustomer, sUserID, sSalesperson);
        RunTrendsReportQuantitySummary(sStartDate, sEndDate, sStockCodeFrom, sStockCodeTo, sCustomer, sEndCustomer, sUserID, sSalesperson);


    }
    private void RunTrendsReportDollars(string sStartDate, string sEndDate, string sStockCodeFrom, string sStockCodeTo, string sCustomer, string sEndCustomer, string sUserID, string sSalesPerson)
    {//REQUIRES USER USERID!!!
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        if (sEndCustomer == "")
        {
            if (sCustomer != "")//With Customer...
            {
                sSQL = "EXEC spGetTrendsReport ";
                sSQL += "@UserID='" + sUserID + "',";
                sSQL += "@FromDate='" + sStartDate + "',";
                sSQL += "@ToDate='" + sEndDate + "',";
                sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                sSQL += "@SalesPerson='" + sSalesPerson + "',";
                sSQL += "@Customer='" + sCustomer + "'";
            }

            else if (sCustomer == "")//StockCode Range Only...
            {
                sSQL = "EXEC spGetTrendsReport ";
                sSQL += "@UserID='" + sUserID + "',";
                sSQL += "@FromDate='" + sStartDate + "',";
                sSQL += "@ToDate='" + sEndDate + "',";
                sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                sSQL += "@SalesPerson='" + sSalesPerson + "',";
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
            sSQL += "@UserID='" + sUserID + "',";
            sSQL += "@FromDate='" + sStartDate + "',";
            sSQL += "@ToDate='" + sEndDate + "',";
            sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
            sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
            sSQL += "@SalesPerson='" + sSalesPerson + "',";
            sSQL += "@EndCustomer='" + sEndCustomer + "'";
        }
        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
        if (dt.Rows.Count > 0)
        {
            Session["dtTrendsReportDollars"] = dt;
        }

    }
    private void RunTrendsReportDollarsSummary(string sStartDate, string sEndDate, string sStockCodeFrom, string sStockCodeTo, string sCustomer, string sEndCustomer, string sUserID, string sSalesPerson)
    {//REQUIRES USER USERID!!!
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        if (sEndCustomer == "")
        {
            if (sCustomer != "")//With Customer...
            {
                sSQL = "EXEC spGetTrendsReportSummary ";
                sSQL += "@UserID='" + sUserID + "',";
                sSQL += "@FromDate='" + sStartDate + "',";
                sSQL += "@ToDate='" + sEndDate + "',";
                sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                sSQL += "@SalesPerson='" + sSalesPerson + "',";
                sSQL += "@Customer='" + sCustomer + "'";
            }

            else if (sCustomer == "")//StockCode Range Only...
            {
                sSQL = "EXEC spGetTrendsReportSummary ";
                sSQL += "@UserID='" + sUserID + "',";
                sSQL += "@FromDate='" + sStartDate + "',";
                sSQL += "@ToDate='" + sEndDate + "',";
                sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                sSQL += "@SalesPerson='" + sSalesPerson + "',";
                sSQL += "@Customer=''";
            }
            else
            {
                //Should Never be here...
            }
        }
        else//End Customer...
        {
            sSQL = "EXEC spGetTrendsReportSummary ";
            sSQL += "@UserID='" + sUserID + "',";
            sSQL += "@FromDate='" + sStartDate + "',";
            sSQL += "@ToDate='" + sEndDate + "',";
            sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
            sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
            sSQL += "@SalesPerson='" + sSalesPerson + "',";
            sSQL += "@EndCustomer='" + sEndCustomer + "'";
        }
        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
        if (dt.Rows.Count > 0)
        {
            Session["dtTrendsReportDollarsSummary"] = dt;
        }

    }
    private void RunTrendsReportQuantity(string sStartDate, string sEndDate, string sStockCodeFrom, string sStockCodeTo, string sCustomer, string sEndCustomer, string sUserID, string sSalesPerson)
    {//REQUIRES USER USERID!!!
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        if (sEndCustomer == "")
        {
            if (sCustomer != "")//Customer...
            {
                sSQL = "EXEC spGetTrendsReportByQuantity ";
                sSQL += "@UserID='" + sUserID + "',";
                sSQL += "@FromDate='" + sStartDate + "',";
                sSQL += "@ToDate='" + sEndDate + "',";
                sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                sSQL += "@SalesPerson='" + sSalesPerson + "',";
                sSQL += "@Customer='" + sCustomer + "'";
            }
            else if (sCustomer == "")//StockCode Range Only...
            {
                sSQL = "EXEC spGetTrendsReportByQuantity ";
                sSQL += "@UserID='" + sUserID + "',";
                sSQL += "@FromDate='" + sStartDate + "',";
                sSQL += "@ToDate='" + sEndDate + "',";
                sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                sSQL += "@SalesPerson='" + sSalesPerson + "',";
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
            sSQL += "@UserID='" + sUserID + "',";
            sSQL += "@FromDate='" + sStartDate + "',";
            sSQL += "@ToDate='" + sEndDate + "',";
            sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
            sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
            sSQL += "@SalesPerson='" + sSalesPerson + "',";
            sSQL += "@EndCustomer='" + sEndCustomer + "'";
        }
        Debug.WriteLine(sSQL);
        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
        if (dt.Rows.Count > 0)
        {
            Session["dtTrendsReportQuantity"] = dt;
        }

    }
    private void RunTrendsReportQuantitySummary(string sStartDate, string sEndDate, string sStockCodeFrom, string sStockCodeTo, string sCustomer, string sEndCustomer, string sUserID, string sSalesPerson)
    {//REQUIRES USER USERID!!!
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        if (sEndCustomer == "")
        {
            if (sCustomer != "")//Customer...
            {
                sSQL = "EXEC spGetTrendsReportByQuantitySummary ";
                sSQL += "@UserID='" + sUserID + "',";
                sSQL += "@FromDate='" + sStartDate + "',";
                sSQL += "@ToDate='" + sEndDate + "',";
                sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                sSQL += "@SalesPerson='" + sSalesPerson + "',";
                sSQL += "@Customer='" + sCustomer + "'";
            }
            else if (sCustomer == "")//StockCode Range Only...
            {
                sSQL = "EXEC spGetTrendsReportByQuantitySummary ";
                sSQL += "@UserID='" + sUserID + "',";
                sSQL += "@FromDate='" + sStartDate + "',";
                sSQL += "@ToDate='" + sEndDate + "',";
                sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
                sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
                sSQL += "@SalesPerson='" + sSalesPerson + "',";
                sSQL += "@Customer=''";
            }
            else
            {
                //Should Never be here...
            }
        }
        else//End Customer...
        {
            sSQL = "EXEC spGetTrendsReportByQuantitySummary ";
            sSQL += "@UserID='" + sUserID + "',";
            sSQL += "@FromDate='" + sStartDate + "',";
            sSQL += "@ToDate='" + sEndDate + "',";
            sSQL += "@StockCodeFrom='" + sStockCodeFrom + "',";
            sSQL += "@StockCodeTo='" + sStockCodeTo + "',";
            sSQL += "@SalesPerson='" + sSalesPerson + "',";
            sSQL += "@EndCustomer='" + sEndCustomer + "'";
        }
        Debug.WriteLine(sSQL);
        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
        if (dt.Rows.Count > 0)
        {
            Session["dtTrendsReportQuantitySummary"] = dt;
        }

    }
    private void ExportFullReport(string sSalespersonUserID)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";


        dt = RunReport(sSalespersonUserID);
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

        try
        {
            dt.Columns.Remove("Margin");
            dt.Columns.Remove("CostValue");
        }
        catch (Exception)
        {
            //Ignore... 
        }

        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "CustomerReportFull" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    private void ExportSummary(string sSalespersonUserID)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";

        dt = RunReportSummary(sSalespersonUserID);
        dt.TableName = "dtReportSummary";
        string sYearMinus1 = DateTime.Now.AddYears(-1).Year.ToString();
        string sYearMinus2 = DateTime.Now.AddYears(-2).Year.ToString();
        string sYearMinus3 = DateTime.Now.AddYears(-3).Year.ToString();
        string sYearMinus4 = DateTime.Now.AddYears(-4).Year.ToString();
        string sYearMinus5 = DateTime.Now.AddYears(-5).Year.ToString();
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
                                  YearMinus1 = dtSum["YearMinus1"],                                 
                                  PercentageOfTotal = dtSum["PercentageOfTotal"],
                                  PercentageOfTotalPriorYearRange = dtSum["PercentageOfTotalPriorYearRange"],
                                  PercentageOfTotalCurrentYTD = dtSum["PercentageOfTotalCurrentYTD"],
                                  PercentageOfTotalYearMinus1 = dtSum["PercentageOfTotalYearMinus1"],

                              });
                dt = SharedFunctions.LINQToDataTable(query1);

                try
                {

                    dt.Columns["YearMinus1"].ColumnName = sYearMinus1;//Rename column...                 
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
                                  YearMinus1 = dtSum["YearMinus1"],
                                  YearMinus2 = dtSum["YearMinus2"],
                                  PercentageOfTotal = dtSum["PercentageOfTotal"],
                                  PercentageOfTotalPriorYearRange = dtSum["PercentageOfTotalPriorYearRange"],
                                  PercentageOfTotalCurrentYTD = dtSum["PercentageOfTotalCurrentYTD"],
                                  PercentageOfTotalYearMinus1 = dtSum["PercentageOfTotalYearMinus1"],
                                  PercentageOfTotalYearMinus2 = dtSum["PercentageOfTotalYearMinus2"],

                              });
                dt = SharedFunctions.LINQToDataTable(query2);
                try
                {

                    dt.Columns["YearMinus1"].ColumnName = sYearMinus1;//Rename column...
                    dt.Columns["YearMinus2"].ColumnName = sYearMinus2;//Rename column...
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
                                  YearMinus1 = dtSum["YearMinus1"],
                                  YearMinus2 = dtSum["YearMinus2"],
                                  YearMinus3 = dtSum["YearMinus3"],
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

                    dt.Columns["YearMinus1"].ColumnName = sYearMinus1;//Rename column...
                    dt.Columns["YearMinus2"].ColumnName = sYearMinus2;//Rename column...
                    dt.Columns["YearMinus3"].ColumnName = sYearMinus3;//Rename column...                  
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
                                  YearMinus1 = dtSum["YearMinus1"],
                                  YearMinus2 = dtSum["YearMinus2"],
                                  YearMinus3 = dtSum["YearMinus3"],
                                  YearMinus4 = dtSum["YearMinus4"],                                 
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

                    dt.Columns["YearMinus1"].ColumnName = sYearMinus1;//Rename column...
                    dt.Columns["YearMinus2"].ColumnName = sYearMinus2;//Rename column...
                    dt.Columns["YearMinus3"].ColumnName = sYearMinus3;//Rename column...
                    dt.Columns["YearMinus4"].ColumnName = sYearMinus4;//Rename column...
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
                                  YearMinus1 = dtSum["YearMinus1"],
                                  YearMinus2 = dtSum["YearMinus2"],
                                  YearMinus3 = dtSum["YearMinus3"],
                                  YearMinus4 = dtSum["YearMinus4"],
                                  YearMinus5 = dtSum["YearMinus5"],
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

                    dt.Columns["YearMinus1"].ColumnName = sYearMinus1;//Rename column...
                    dt.Columns["YearMinus2"].ColumnName = sYearMinus2;//Rename column...
                    dt.Columns["YearMinus3"].ColumnName = sYearMinus3;//Rename column...
                    dt.Columns["YearMinus4"].ColumnName = sYearMinus4;//Rename column...
                    dt.Columns["YearMinus5"].ColumnName = sYearMinus5;//Rename column...
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
    #endregion

    #region Functions

    private DataTable RunReportSummary(string sSalespersonUserID)
    {
        int iIndexOfPipe = 0;
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
            lStockCodes = GetNoCustomerStockCodes(iNonCustomerID);
            iIndexOfPipe = 0;
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
            sSQL = "EXEC spGetCustomerReportSummaryByRange ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
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
            sSQL += "@SalesPerson=" + sSalespersonUserID;

        }
        else if (Mode == "EndCustomer")//End Customer...
        {
            sSQL = "EXEC spGetCustomerReportSummary ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@StockCodes=" + sStockCode + ",";
            sSQL += "@Customer=" + sCustomer + ",";
            sSQL += "@Year=" + sYear + ",";
            sSQL += "@ProductClass=" + sProductClass + ",";
            sSQL += "@SalesPerson=" + sSalespersonUserID;
        }
        else
        {
            sSQL = "EXEC spGetCustomerReportSummary ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@StockCodes=" + sStockCode + ",";
            sSQL += "@Customer=" + sCustomer + ",";
            sSQL += "@Year=" + sYear + ",";
            sSQL += "@ProductClass=" + sProductClass + ",";
            sSQL += "@SalesPerson=" + sSalespersonUserID;
        }



        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");


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
    private DataTable RunReport(string sSalespersonUserID)
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

        string sToMargin = "NULL";

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
                sSQL += "@SalesPerson=" + sSalespersonUserID;
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
                sSQL += "@SalesPerson=" + sSalespersonUserID;
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
            sSQL += "@SalesPerson=" + sSalespersonUserID;
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
            sSQL += "@SalesPerson=" + sSalespersonUserID;
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
            sSQL += "@SalesPerson=" + sSalespersonUserID;
        }

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
        try
        {

        }
        catch (Exception)
        {


        }
        finally
        {
            dt.Dispose();
        }
        return dt;
    }
    private string GetSalesperson(int iUserID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        string sSalesperson = "";
        var query = (from s in db.SalSalesperson
                     join u in db.WipUsers on s.Salesperson equals u.Salesperson
                     where u.UserID == iUserID
                     select new { s.Salesperson });

        if (query.Count() > 0)
        {
            foreach (var a in query)
            {
                sSalesperson = a.Salesperson.Trim();
            }
        }
        return sSalesperson;
    }
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
            Mode = "Range";
            btnStockCodeRangePanel.CssClass = "btn btn-success";
            pnlStockCodeRange.Visible = true;
            LoadProductClass(ddlProductClass);

            LoadStockCodes(lbStockCode0);
            string sSalesperson = "";
            sSalesperson = GetSalesperson(iUserID);
            LoadCustomer(ddlCustomers, "Name", sSalesperson);
            LoadCustomer(ddlCustomers0, "Name", sSalesperson);
            LoadNonCustomers(ddlEndCustomers, sSalesperson);
            LoadCustomerCities(lbCities, sSalesperson);
            LoadCustomerZips(lbZips, sSalesperson);
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



        }//End postback

    }
 
    protected void btnStockCodeRangePanel_Click(object sender, EventArgs e)
    {
         
        pnlStockCodeRange.Visible = true;
        pnlEndCustomer.Visible = false;
        ddlEndCustomers.SelectedIndex = 0;
        lbParentStockCodeEndCustomer.Items.Clear();
 
        btnStockCodeRangePanel.CssClass = "btn btn-success";
        btnEndCustomerPanel.CssClass = "btn btn-info";

        Mode = "Range";
    }
    protected void btnEndCustomerPanel_Click(object sender, EventArgs e)
    {
         
        pnlStockCodeRange.Visible = false;
        pnlEndCustomer.Visible = true;
       
        btnStockCodeRangePanel.CssClass = "btn btn-info";
        btnEndCustomerPanel.CssClass = "btn btn-success";
        Mode = "EndCustomer";
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
            e.Row.Cells[13].Text = DateTime.Now.AddYears(-1).Year.ToString();
            e.Row.Cells[14].Text = DateTime.Now.AddYears(-2).Year.ToString();
            e.Row.Cells[15].Text = DateTime.Now.AddYears(-3).Year.ToString();
            e.Row.Cells[16].Text = DateTime.Now.AddYears(-4).Year.ToString();
            e.Row.Cells[17].Text = DateTime.Now.AddYears(-5).Year.ToString();
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

            //100 * ((s.InvoiceValue - s.CostValue) /  (CASE WHEN s.InvoiceValue = 0 THEN 1 ELSE s.InvoiceValue END))

            if (dcTotalAmount == 0)
            {
                dcTotalAmount = 1;
            }



            lblAmountTotal.Text = "$" + dcTotalAmount.ToString("#,0.00");

            lblPriceTotal.Text = "Price Total: " + lblAmountTotal.Text;


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
                e.Row.Cells[5].Text = DateTime.Now.AddYears(-1).Year.ToString();
                e.Row.Cells[6].Text = DateTime.Now.AddYears(-2).Year.ToString();
                e.Row.Cells[7].Text = DateTime.Now.AddYears(-3).Year.ToString();
                e.Row.Cells[8].Text = DateTime.Now.AddYears(-4).Year.ToString();
                e.Row.Cells[9].Text = DateTime.Now.AddYears(-5).Year.ToString();

                switch (ddlYearsBack.SelectedValue)
                {
                    case "1":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1200);
                        break;
                    case "2":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;

                        gvCustomerSummary.Width = Unit.Pixel(1300);
                        break;
                    case "3":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;

                        gvCustomerSummary.Width = Unit.Pixel(1400);
                        break;
                    case "4":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = false;

                        gvCustomerSummary.Width = Unit.Pixel(1500);
                        break;
                    case "5":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = true;

                        gvCustomerSummary.Width = Unit.Pixel(1600);
                        break;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblAmount = (Label)e.Row.FindControl("lblAmount");

                Label lblYTD = (Label)e.Row.FindControl("lblYTD");
                Label lblYearMinus1 = (Label)e.Row.FindControl("lblYearMinus1");
                Label lblYearMinus2 = (Label)e.Row.FindControl("lblYearMinus2");
                Label lblYearMinus3 = (Label)e.Row.FindControl("lblYearMinus3");
                Label lblYearMinus4 = (Label)e.Row.FindControl("lblYearMinus4");
                Label lblYearMinus5 = (Label)e.Row.FindControl("lblYearMinus5");

                switch (ddlYearsBack.SelectedValue)
                {
                    case "1":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1200);
                        break;
                    case "2":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;

                        gvCustomerSummary.Width = Unit.Pixel(1300);
                        break;
                    case "3":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;

                        gvCustomerSummary.Width = Unit.Pixel(1400);
                        break;
                    case "4":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = false;

                        gvCustomerSummary.Width = Unit.Pixel(1500);
                        break;
                    case "5":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = true;

                        gvCustomerSummary.Width = Unit.Pixel(1600);
                        break;
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
                Label lblCustomer = (Label)e.Row.FindControl("lblCustomer");
                Label lblName = (Label)e.Row.FindControl("lblName");
                lblName.ToolTip = GetCustomerDetails(lblCustomer.Text);
                lblName.Style.Add("cursor", "pointer");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblAmountTotal = (Label)e.Row.FindControl("lblAmountTotal");

                Label lblYTDTotal = (Label)e.Row.FindControl("lblYTDTotal");
                Label lblYearMinus1Total = (Label)e.Row.FindControl("lblYearMinus1Total");
                Label lblYearMinus2Total = (Label)e.Row.FindControl("lblYearMinus2Total");
                Label lblYearMinus3Total = (Label)e.Row.FindControl("lblYearMinus3Total");
                Label lblYearMinus4Total = (Label)e.Row.FindControl("lblYearMinus4Total");
                Label lblYearMinus5Total = (Label)e.Row.FindControl("lblYearMinus5Total");


                lblAmountTotal.Text = "$" + dcTotalAmountSummary.ToString("#,0.00");
                lblYTDTotal.Text = "$" + dcYTDTotalSummary.ToString("#,0.00");
                lblYearMinus1Total.Text = "$" + dcYearMinus1TotalSummary.ToString("#,0.00");
                lblYearMinus2Total.Text = "$" + dcYearMinus2TotalSummary.ToString("#,0.00");
                lblYearMinus3Total.Text = "$" + dcYearMinus3TotalSummary.ToString("#,0.00");
                lblYearMinus4Total.Text = "$" + dcYearMinus4TotalSummary.ToString("#,0.00");
                lblYearMinus5Total.Text = "$" + dcYearMinus5TotalSummary.ToString("#,0.00");

                switch (ddlYearsBack.SelectedValue)
                {
                    case "1":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = false;
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;
                        gvCustomerSummary.Width = Unit.Pixel(1200);
                        break;
                    case "2":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = false;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;

                        gvCustomerSummary.Width = Unit.Pixel(1300);
                        break;
                    case "3":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = false;
                        e.Row.Cells[9].Visible = false;

                        gvCustomerSummary.Width = Unit.Pixel(1400);
                        break;
                    case "4":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = false;

                        gvCustomerSummary.Width = Unit.Pixel(1500);
                        break;
                    case "5":
                        e.Row.Cells[5].Visible = true;
                        e.Row.Cells[6].Visible = true;
                        e.Row.Cells[7].Visible = true;
                        e.Row.Cells[8].Visible = true;
                        e.Row.Cells[9].Visible = true;

                        gvCustomerSummary.Width = Unit.Pixel(1600);
                        break;
                }



                lblPriceTotal.Text = "Price Total: " + lblAmountTotal.Text;

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

        int iUserID = Convert.ToInt32(Session["UserID"]);
        string sSalesperson = "";
        sSalesperson = GetSalesperson(iUserID);
        RunReport(sSalesperson);
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
    }
    protected void btnRunReportSummary_Click(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        string sSalesperson = "";
        sSalesperson = GetSalesperson(iUserID);
        RunReportSummary(sSalesperson);
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
        ddlEndCustomers.SelectedIndex = 0;
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

        int iUserID = Convert.ToInt32(Session["UserID"]);
        string sSalesperson = "";
        sSalesperson = GetSalesperson(iUserID);
        LoadCustomer(ddlCustomers, "Name", sSalesperson);
        LoadCustomer(ddlCustomers0, "Name", sSalesperson);
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
    protected void lbnSyncStockCodeLists_Click(object sender, EventArgs e)
    {
        if (txtStockCodeFrom.Text.Trim() == "")
        {
            foreach (ListItem li2 in lbStockCode0.Items)
            {
                if (txtStockCodeFrom.Text.Trim() == li2.Value)
                {
                    li2.Selected = true;
                }
            }
        }

    }
    protected void rblSort0_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnExportFullReport_Click(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        string sSalesperson = "";
        sSalesperson = GetSalesperson(iUserID);
        ExportFullReport(sSalesperson);
    }
    protected void btnExportSummary_Click(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        string sSalesperson = "";
        sSalesperson = GetSalesperson(iUserID);
        ExportSummary(sSalesperson);
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
 
    protected void ddlEndCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlCustomers.SelectedIndex = 0;
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

    protected void ddlYearsBack_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnRunReportSummary_Click(btnRunReportSummary, null);
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

        txtStockCodeFrom.Text = "";
        txtStockCodeTo.Text = "";

        ddlProductClass.SelectedIndex = 0;

        ddlPeriod.SelectedIndex = 0;
        ddlEndCustomers.SelectedIndex = 0;
        ddlYear.SelectedIndex = 0;

        lbParentStockCodeEndCustomer.Items.Clear();
        ddlCustomers.SelectedIndex = 0;

        lblStockCodeList.Text = "";
        PanelSetup();
    }
    protected void rblSingleOrStockCodeRange_SelectedIndexChanged(object sender, EventArgs e)
    {
        PanelSetup();
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