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

public partial class CustomerReportShorted : System.Web.UI.Page
{



    //Full Report...
    decimal dcOrderQtyTotal = 0;
    decimal dcShipQtyTotal = 0;
    decimal dcShortedQtyTotal = 0;
    //Summary...
    decimal dcOrderQtyTotalSummary = 0;
    decimal dcShipQtyTotalSummary = 0;
    decimal dcShortedQtyTotalSummary = 0;

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
            ddlSalesPerson.Items.Add(new ListItem(a.Name.ToUpper() + " - " + a.Salesperson, a.Salesperson));
        }

        rsListbox.Sort(ref ddlSalesPerson, rsListbox.SortOrder.Ascending);
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

        //Stock Codes...

        string sStockCode = "NULL";
        string sStockCodeFrom = "";
        string sStockCodeTo = "";
        if (Mode == "Single")
        {//Single Stock Code...
            if (txtStockCodeSingle.Text.Trim() != "")
            {//Not null then add quotes...
                sStockCode = "'" + txtStockCodeSingle.Text.Trim() + "'";
            }
        }
        else if (Mode == "Range")//Stock Code Range...
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
            return;
        }


        if (Mode == "Single")
        {//Single...

            if (sStockCode != "")//Selected Stock Codes...
            {

                sSQL = "EXEC spGetInventoryShortedReport ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@StockCodes=" + sStockCode + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@Year=" + sYear + ",";
                sSQL += "@ProductClass=" + sProductClass + ",";
                sSQL += "@SalesPerson=" + sSalesPerson;
            }
            else//All Stock Codes...
            {
                sSQL = "EXEC spGetInventoryShortedReport ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@Year=" + sYear + ",";
                sSQL += "@ProductClass=" + sProductClass + ",";
                sSQL += "@SalesPerson=" + sSalesPerson;
            }

        }
        else if (Mode == "Range")//Stock Code Range...
        {
            sSQL = "EXEC spGetInventoryShortedReportByRange ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
            sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
            sSQL += "@Customer=" + sCustomer + ",";
            sSQL += "@Year=" + sYear + ",";
            sSQL += "@ProductClass=" + sProductClass + ",";
            sSQL += "@SalesPerson=" + sSalesPerson;
        }
        else if (Mode == "EndCustomer")//End Customer...
        {
            sSQL = "EXEC spGetInventoryShortedReport ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@StockCodes=" + sStockCode + ",";
            sSQL += "@Customer=" + sCustomer + ",";
            sSQL += "@Year=" + sYear + ",";
            sSQL += "@ProductClass=" + sProductClass + ",";
            sSQL += "@SalesPerson=" + sSalesPerson;
        }
        else
        {
            sSQL = "EXEC spGetInventoryShortedReport ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
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
            Session["dtReport"] = dt;
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
            pnlGridView.Width = Unit.Pixel(1490);
            tblHeaderTable.Width = Unit.Pixel(1450);
            gvCustomerSummary.Width = Unit.Pixel(1574);
        }
        else
        {
            pnlGridView.ScrollBars = ScrollBars.None;
            pnlGridView.Width = Unit.Pixel(1320);
            tblHeaderTable.Width = Unit.Pixel(1454);
            gvCustomerSummary.Width = Unit.Pixel(1430);
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

        string sStockCode = "NULL";
        string sStockCodeFrom = "";
        string sStockCodeTo = "";
        if (Mode == "Single")
        {//Single Stock Code...
            if (txtStockCodeSingle.Text.Trim() != "")
            {//Not null then add quotes...
                sStockCode = "'" + txtStockCodeSingle.Text.Trim() + "'";
            }
        }
        else if (Mode == "Range")//Stock Code Range...
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

                sSQL = "EXEC spGetInventoryShortedReportSummary ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@StockCodes=" + sStockCode + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@Year=" + sYear + ",";
                sSQL += "@ProductClass=" + sProductClass + ",";
                sSQL += "@SalesPerson=" + sSalesPerson;
            }
            else//All Stock Codes...
            {
                sSQL = "EXEC spGetInventoryShortedReportSummary ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@Year=" + sYear + ",";
                sSQL += "@ProductClass=" + sProductClass + ",";
                sSQL += "@SalesPerson=" + sSalesPerson;
            }
        }
        else if (Mode == "Range")//Stock Code Range...
        {
            sSQL = "EXEC spGetInventoryShortedReportByRangeSummary ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
            sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
            sSQL += "@Customer=" + sCustomer + ",";
            sSQL += "@Year=" + sYear + ",";
            sSQL += "@ProductClass=" + sProductClass + ",";
            sSQL += "@SalesPerson=" + sSalesPerson;
        }
        else if (Mode == "EndCustomer")//End Customer...
        {
            sSQL = "EXEC spGetInventoryShortedReportSummary ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
            sSQL += "@StockCodes=" + sStockCode + ",";
            sSQL += "@Customer=" + sCustomer + ",";
            sSQL += "@Year=" + sYear + ",";
            sSQL += "@ProductClass=" + sProductClass + ",";
            sSQL += "@SalesPerson=" + sSalesPerson;
        }
        else
        {
            sSQL = "EXEC spGetInventoryShortedReportSummary ";
            sSQL += "@FromDate=" + sStartDate + ",";
            sSQL += "@ToDate=" + sEndDate + ",";
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
            Session["dtReport"] = dt;
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
            headerCell.Font.Bold = false;
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
                    headerCell.Width = Unit.Pixel(165);
                    break;
                case 1://Customer#
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 2://SalesPerson
                    headerCell.Width = Unit.Pixel(85);
                    break;
                case 3://StockCode
                    headerCell.Width = Unit.Pixel(74);
                    break;
                case 4://Decription
                    headerCell.Width = Unit.Pixel(80);
                    break;
                case 5://ProductClass
                    headerCell.Width = Unit.Pixel(80);
                    break;
                case 6://Uom
                    headerCell.Width = Unit.Pixel(50);
                    break;
                case 7://PC
                    headerCell.Width = Unit.Pixel(35);
                    break;
                case 8://OrderQty
                    headerCell.Width = Unit.Pixel(60);
                    break;
                case 9://ShipQty
                    headerCell.Width = Unit.Pixel(60);
                    break;
                case 10://ShortedQty
                    headerCell.Width = Unit.Pixel(60);
                    break;
                case 11://Invoice#
                    headerCell.Width = Unit.Pixel(60);
                    break;
                case 12://InvoiceDate
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 13://SalesOrder
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 14://OrderDate
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 15://OrderStatus
                    headerCell.Width = Unit.Pixel(55);
                    break;
                case 16://Dispostion
                    headerCell.Width = Unit.Pixel(40);
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
            case "Single"://Single Entry

                lblStockCodeList.Text = "";
                break;
            case "Range"://Stock Code Range

                lblStockCodeList.Text = "";
                break;
            case "EndCustomer"://End Customer  

                lblStockCodeDesc.Text = "";
                lbParentStockCodeEndCustomer.Items.Clear();
                lbParentStockCodeEndCustomer.Items.Add(new ListItem("First Select an End Customer", "0"));
                lblStockCodeList.Text = "";
                break;
            case "List"://Stock Code List               
                lblStockCodeDesc.Text = "";
                txtStockCodeFrom.Text = "";
                txtStockCodeTo.Text = "";

                break;
        }
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
        if (Mode == "Single")
        {
            sStockCode = txtStockCodeSingle.Text.Trim();
        }
        else
        {
            sStockCode = "";
        }

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
        if (Mode == "EndCustomer")//End Customer...
        {
            if (ddlEndCustomers.SelectedIndex != 0)
            {
                sCustomer = ddlEndCustomers.SelectedValue;
                sURL += "&endcust=" + sCustomer;
            }
            else
            {

            }

        }
        else
        {
            sURL += "&cus=" + sCustomer;
        }

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
        if (Mode == "EndCustomer")//End Customer...
        {
            if (ddlEndCustomers.SelectedIndex != 0)
            {
                sCustomer = ddlEndCustomers.SelectedValue;
                sURL2 += "&endcust=" + sCustomer;
            }
            else
            {

            }

        }
        else
        {
            sURL2 += "&cus=" + sCustomer;
        }



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
            Mode = "Single";
            LoadProductClass(ddlProductClass);
            LoadSalesPersons();

            LoadCustomer(ddlCustomers, "Name");

            LoadNonCustomers(ddlEndCustomers);
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


            btnStockCodeRangePanel_Click(btnSingleStockCodePanel, null);

        }//End postback

    }
    protected void btnSingleStockCodePanel_Click(object sender, EventArgs e)
    {
        pnlSingleStockCode.Visible = true;
        pnlStockCodeRange.Visible = false;
        pnlEndCustomer.Visible = false;


        btnSingleStockCodePanel.CssClass = "btn btn-success";
        btnStockCodeRangePanel.CssClass = "btn btn-info";
        btnEndCustomerPanel.CssClass = "btn btn-info";


        Mode = "Single";
    }
    protected void btnStockCodeRangePanel_Click(object sender, EventArgs e)
    {
        pnlSingleStockCode.Visible = false;
        pnlStockCodeRange.Visible = true;
        pnlEndCustomer.Visible = false;


        btnSingleStockCodePanel.CssClass = "btn btn-info";
        btnStockCodeRangePanel.CssClass = "btn btn-success";
        btnEndCustomerPanel.CssClass = "btn btn-info";


        Mode = "Range";
    }
    protected void btnEndCustomerPanel_Click(object sender, EventArgs e)
    {
        pnlSingleStockCode.Visible = false;
        pnlStockCodeRange.Visible = false;
        pnlEndCustomer.Visible = true;


        btnSingleStockCodePanel.CssClass = "btn btn-info";
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
        decimal dcOrderQty = 0;
        decimal dcShipQty = 0;
        decimal dcShortedQty = 0;
        int iRoleID = 0;
        iRoleID = Convert.ToInt32(Session["RoleID"]);


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
            Label lblInvoiceDate = (Label)e.Row.FindControl("lblInvoiceDate");
            Label lblOrderDate = (Label)e.Row.FindControl("lblOrderDate");
            Label lblOrderQty = (Label)e.Row.FindControl("lblOrderQty");
            Label lblShipQty = (Label)e.Row.FindControl("lblShipQty");
            Label lblShortedQty = (Label)e.Row.FindControl("lblShortedQty");

            if (lblOrderQty.Text != "")
            {
                dcOrderQty = Convert.ToDecimal(lblOrderQty.Text.Trim());
                dcOrderQtyTotal += dcOrderQty;
                lblOrderQty.Text = Convert.ToDecimal(lblOrderQty.Text).ToString("#,0.0");
            }

            if (lblShipQty.Text != "")
            {
                dcShipQty = Convert.ToDecimal(lblShipQty.Text.Trim());
                dcShipQtyTotal += dcShipQty;
                lblShipQty.Text = Convert.ToDecimal(lblShipQty.Text).ToString("#,0.0");
            }
            if (lblShortedQty.Text != "")
            {
                dcShortedQty = Convert.ToDecimal(lblShortedQty.Text.Trim());
                dcShortedQtyTotal += dcShortedQty;
                lblShortedQty.Text = Convert.ToDecimal(lblShortedQty.Text).ToString("#,0.0");
            }
            if (lblInvoiceDate.Text != "")
            {
                lblInvoiceDate.Text = Convert.ToDateTime(lblInvoiceDate.Text).ToShortDateString();
            }
            if (lblOrderDate.Text != "")
            {
                lblOrderDate.Text = Convert.ToDateTime(lblOrderDate.Text).ToShortDateString();
            }
            Label lblSalesOrder = (Label)e.Row.FindControl("lblSalesOrder");
            string sSource = lblSalesOrder.Text;
            var sResult = int.Parse(sSource).ToString();
            lblSalesOrder.Text = sResult;

            Label lblInvoice = (Label)e.Row.FindControl("lblInvoice");
            sSource = lblInvoice.Text;
            sResult = int.Parse(sSource).ToString();
            lblInvoice.Text = sResult;

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblOrderQtyTotal = (Label)e.Row.FindControl("lblOrderQtyTotal");
            Label lblShipQtyTotal = (Label)e.Row.FindControl("lblShipQtyTotal");
            Label lblShortedQtyTotal = (Label)e.Row.FindControl("lblShortedQtyTotal");

            lblOrderQtyTotal.Text = dcOrderQtyTotal.ToString("#,0.0");
            lblShipQtyTotal.Text = dcShipQtyTotal.ToString("#,0.0");
            lblShortedQtyTotal.Text = dcShortedQtyTotal.ToString("#,0.0");

        }

    }
    protected void gvReportCondensed_Sorting(object sender, GridViewSortEventArgs e)
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
    protected void gvCustomerSummary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcOrderQty = 0;
        decimal dcShipQty = 0;
        decimal dcShortedQty = 0;
        try
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblOrderQty = (Label)e.Row.FindControl("lblOrderQty");
                Label lblShipQty = (Label)e.Row.FindControl("lblShipQty");
                Label lblShortedQty = (Label)e.Row.FindControl("lblShortedQty");

                if (lblOrderQty.Text != "")
                {
                    dcOrderQty = Convert.ToDecimal(lblOrderQty.Text.Trim());
                    dcOrderQtyTotalSummary += dcOrderQty;
                    lblOrderQty.Text = Convert.ToDecimal(lblOrderQty.Text).ToString("#,0.0");
                }

                if (lblShipQty.Text != "")
                {
                    dcShipQty = Convert.ToDecimal(lblShipQty.Text.Trim());
                    dcShipQtyTotalSummary += dcShipQty;
                    lblShipQty.Text = Convert.ToDecimal(lblShipQty.Text).ToString("#,0.0");
                }
                if (lblShortedQty.Text != "")
                {
                    dcShortedQty = Convert.ToDecimal(lblShortedQty.Text.Trim());
                    dcShortedQtyTotalSummary += dcShortedQty;
                    lblShortedQty.Text = Convert.ToDecimal(lblShortedQty.Text).ToString("#,0.0");
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblOrderQtyTotal = (Label)e.Row.FindControl("lblOrderQtyTotal");
                Label lblShipQtyTotal = (Label)e.Row.FindControl("lblShipQtyTotal");
                Label lblShortedQtyTotal = (Label)e.Row.FindControl("lblShortedQtyTotal");

                lblOrderQtyTotal.Text = dcOrderQtyTotalSummary.ToString("#,0.0");
                lblShipQtyTotal.Text = dcShipQtyTotalSummary.ToString("#,0.0");
                lblShortedQtyTotal.Text = dcShortedQtyTotalSummary.ToString("#,0.0");
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

        DataTable dt = (DataTable)Session["dtReport"];
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

    protected void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlEndCustomers.SelectedIndex = 0;
    }

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
    protected void rblSort0_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void imgExportExcel1_Click(object sender, EventArgs e)
    {
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

        int iRoleID = 0;
        iRoleID = Convert.ToInt32(Session["RoleID"]);
        switch (iRoleID)
        {
            case 1://Admin...
                try
                {
                    dt.Columns.Remove("Margin2");
                    dt.Columns.Remove("GrandTotal");
                }
                catch (Exception)
                { }
                break;
            case 2://Supervisor...
                try
                {
                    dt.Columns.Remove("Margin2");
                    dt.Columns.Remove("GrandTotal");
                }
                catch (Exception)
                { }
                break;
            default:
                try
                {
                    dt.Columns.Remove("Margin");
                    dt.Columns.Remove("Margin2");
                }
                catch (Exception)
                { }
                break;
        }

        try
        {
            string sYearMinus1 = DateTime.Now.AddYears(-1).Year.ToString();
            string sYearMinus2 = DateTime.Now.AddYears(-2).Year.ToString();
            string sYearMinus3 = DateTime.Now.AddYears(-3).Year.ToString();
            string sYearMinus4 = DateTime.Now.AddYears(-4).Year.ToString();
            string sYearMinus5 = DateTime.Now.AddYears(-5).Year.ToString();

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
            foreach (DataRow row in dt.Rows)
            {
                string sSource = row["SalesOrder"].ToString();
                var sResult = int.Parse(sSource).ToString();
                row["SalesOrder"] = sResult;
                sSource = row["Invoice"].ToString();
                sResult = int.Parse(sSource).ToString();
                row["Invoice"] = sResult;
            }
            dt.AcceptChanges();
        }
        catch (Exception)
        { }


        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt);
        }

        sFilesName = "CustomerReport_Shorted" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void imgExportExcel2_Click(object sender, EventArgs e)
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


        int iRoleID = 0;
        iRoleID = Convert.ToInt32(Session["RoleID"]);
        switch (iRoleID)
        {
            case 1://Admin...
                try
                {
                    dt.Columns.Remove("Margin2");
                    dt.Columns.Remove("GrandTotal");
                }
                catch (Exception)
                { }
                break;
            case 2://Supervisor...
                try
                {
                    dt.Columns.Remove("Margin2");
                    dt.Columns.Remove("GrandTotal");
                }
                catch (Exception)
                { }
                break;
            default:
                try
                {
                    dt.Columns.Remove("Margin");
                    dt.Columns.Remove("Margin2");
                }
                catch (Exception)
                { }
                break;
        }
        try
        {
            string sYearMinus1 = DateTime.Now.AddYears(-1).Year.ToString();
            string sYearMinus2 = DateTime.Now.AddYears(-2).Year.ToString();
            string sYearMinus3 = DateTime.Now.AddYears(-3).Year.ToString();
            string sYearMinus4 = DateTime.Now.AddYears(-4).Year.ToString();
            string sYearMinus5 = DateTime.Now.AddYears(-5).Year.ToString();

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
            ds.Tables.Add(dt);
        }

        sFilesName = "PriceChangesReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void txtStockCodeSingle_TextChanged(object sender, EventArgs e)
    {
        if (txtStockCodeSingle.Text.Trim() != "")
        {
            lblStockCodeDesc.Text = SharedFunctions.GetStockCodeDesc(txtStockCodeSingle.Text.Trim());
        }
        else
        {
            lblStockCodeDesc.Text = "**ALL STOCK CODES USED!!";
        }
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
    protected void txtStockCodeFrom_TextChanged(object sender, EventArgs e)
    {
        lblStockCodeDesc.Text = SharedFunctions.GetStockCodeDesc(txtStockCodeFrom.Text.Trim());
    }


    // create the table for the fixed header in Init
    protected void Page_Init(object sender, EventArgs e)
    {
        SetupHeaderTable1();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtStockCodeSingle.Text = "";
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        txtStockCodeFrom.Text = "";
        txtStockCodeTo.Text = "";

        ddlProductClass.SelectedIndex = 0;
        ddlSalesPerson.SelectedIndex = 0;
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