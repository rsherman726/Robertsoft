using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text;
using System.Data.Linq.SqlClient;
using System.Transactions;
using System.Data.SqlTypes;
using AjaxControlToolkit;
using System.Globalization;
using web = System.Web.UI.WebControls;
using Microsoft.VisualBasic.FileIO;

public partial class CommissionReportImporter : System.Web.UI.Page
{
    const string sDocPath = @"Images\Excel\";


    #region Subs

    private void TestCSV()
    {
        string sFile = "";
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        DataTable dt = new DataTable();
        string sRecType = "";
        string sYear = "";
        string sMonth = "";
        string sSalespersonName = "";
        string sSalespersonID = "";
        string sCustomerName = "";
        string sCustomerCode = "";
        string sInvoiceNumber = "";
        string sInvoiceDate = "";
        string sStockCode = "";
        string sStockDescription = "";
        string sQty = "";
        string sUoM = "";
        string sUnitPrice = "";
        string sLinePrice = "";
        string sTradeDiscAmt = "";
        string sCashDiscAmt = "";
        string sNetSaleAmt = "";
        string sComAmt = "";
        string sComPercentage = "";

        if (FileUpload1.HasFile)
        {
            sFile = FileUpload1.FileName;


            if (sFile.Substring(sFile.Length - 3).ToUpper() != "CSV")
            {
                lblErrorSQL.Text = "**You must select a file with a .csv extension.";
                lblErrorSQL.ForeColor = Color.Red;
                return;
            }


            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            int iTruncateTable = SharedFunctions.ExecuteQuery("TRUNCATE TABLE ArCommissionReport", conn);
            if (iTruncateTable == 0)
            {
                return;
            }

            string sAbsolutePath = "";


            //Upload File first...
            if (Server.MachineName.ToUpper() == "POWERHOUSE" || Server.MachineName.ToUpper() == "POWERHOUSEJR" || Server.MachineName.ToUpper() == "RSPSERVER2")
            {
                sAbsolutePath = MapPath(".\\") + sDocPath;
            }
            else//Hosting Company...
            {
                sAbsolutePath = MapPath(".\\") + sDocPath;
            }

            sFile = sAbsolutePath + sFile;
            //  TextReader tr1 = null;
            try
            {
                if (File.Exists(sFile))
                {
                    File.Delete(sFile);
                }
                FileUpload1.SaveAs(sFile);


                using (TextFieldParser parser = new TextFieldParser(sFile))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    parser.TrimWhiteSpace = true;
                    int lineRead = 0;
                    while (!parser.EndOfData)
                    {
                        try
                        {

                            //Process row
                            string[] fields = ParseHelper(parser.ReadLine(), lineRead++);
                            if (lineRead == 3)
                            {//This is the columns
                                foreach (string val in fields)
                                {                              
                                    //Debug.WriteLine(val);
                                    dt.Columns.Add(val);
                                }
                            }
                            else if (lineRead > 3)
                            {
                                // get the row data...
                                dt.Rows.Add(fields);
                            }
                            else
                            {

                            }

                        }
                        catch (MalformedLineException ex)
                        {
                            Debug.WriteLine(ex.ToString());
                        }
                    }
                }//End using...

                var Data = (from a in dt.AsEnumerable()
                            select new
                            {
                                RecType = a["Rec. Type"].ToString(),
                                Year = a["Year"].ToString(),
                                Month = a["Month"].ToString(),
                                SalespersonName = a["Salesperson Name"].ToString(),
                                SalespersonID = a["Salesperson ID"].ToString(),
                                CustomerName = a["Customer Name"].ToString(),
                                CustomerCode = a["Customer Code"].ToString(),
                                InvoiceNumber = a["Invoice Number"].ToString(),
                                InvoiceDate = a["Invoice Date"].ToString(),
                                StockCode = a["Stock Code"].ToString(),
                                StockDescription = a["Stock Descrip"].ToString(),
                                Qty = a["Qty"].ToString(),
                                UoM = a["U/M"].ToString(),
                                UnitPrice = a["Unit Price"].ToString(),
                                LinePrice = a["Line Price"].ToString(),
                                TradeDiscAmt = a["Trade Disc Amt"].ToString(),
                                CashDiscAmt = a["Cash Disc Amt"].ToString(),
                                NetSaleAmt = a["Net Sale Amt"].ToString(),
                                ComAmt = a["Com Amt"].ToString(),
                                ComPercentage = a["Com %"].ToString(),

                            });
                foreach (var a in Data)
                {

                    try
                    {

                        sRecType = a.RecType;
                        sYear = a.Year;
                        sMonth = a.Month;
                        sSalespersonName = a.SalespersonName;
                        sSalespersonID = a.SalespersonID;
                        sCustomerName = a.CustomerName;
                        sCustomerCode = a.CustomerCode;
                        sInvoiceNumber = a.InvoiceNumber;
                        sInvoiceDate = a.InvoiceDate;
                        sStockCode = a.StockCode;
                        sStockDescription = a.StockDescription;
                        sQty = a.Qty;
                        sUoM = a.UoM;
                        sUnitPrice = a.UnitPrice;
                        sLinePrice = a.LinePrice;
                        sTradeDiscAmt = a.TradeDiscAmt;
                        sCashDiscAmt = a.CashDiscAmt;
                        sNetSaleAmt = a.NetSaleAmt;
                        sComAmt = a.ComAmt;
                        sComPercentage = a.ComPercentage;



                        ArCommissionReport cr = new ArCommissionReport();

                        cr.RecType = Convert.ToInt32(a.RecType);
                        cr.Year = Convert.ToInt32(a.Year);
                        cr.Month = Convert.ToInt32(a.Month);
                        cr.SalespersonName = a.SalespersonName.Trim();
                        cr.SalespersonID = a.SalespersonID.Trim();
                        cr.CustomerName = a.CustomerName.Replace("\"", "");
                        cr.CustomerCode = a.CustomerCode.Trim();
                        cr.InvoiceNumber = a.InvoiceNumber.Trim();
                        cr.InvoiceDate = Convert.ToDateTime(a.InvoiceDate);
                        cr.StockCode = a.StockCode.Trim();
                        cr.StockDescription = a.StockDescription.Trim();
                        cr.Qty = Convert.ToDecimal(a.Qty);
                        cr.UoM = a.UoM.Trim();
                        cr.UnitPrice = Convert.ToDecimal(a.UnitPrice);
                        cr.LinePrice = Convert.ToDecimal(a.LinePrice);
                        cr.TradeDiscAmt = Convert.ToDecimal(a.TradeDiscAmt);
                        cr.CashDiscAmt = Convert.ToDouble(a.CashDiscAmt);
                        cr.NetSaleAmt = Convert.ToDouble(a.NetSaleAmt);
                        cr.ComAmt = Convert.ToDouble(a.ComAmt);
                        cr.ComPercentage = Convert.ToDecimal(a.ComPercentage);
                        cr.DataAdded = DateTime.Now;

                        db.ArCommissionReport.InsertOnSubmit(cr);
                        db.SubmitChanges();


                    }
                    catch (Exception)
                    {
                        continue;
                    }

                }

                //tr1.Close();
                lblErrorSQL.Text = "**Upload Complete!";
                lblErrorSQL.ForeColor = Color.Green;

                var query = (from cr in db.ArCommissionReport select cr);
                dt = SharedFunctions.ToDataTable(db, query);

                gvResults.DataSource = dt;
                gvResults.DataBind();

                if (File.Exists(sFile))
                {
                    File.Delete(sFile);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("RecType: " + sRecType);
                Debug.WriteLine("Year: " + sYear);
                Debug.WriteLine("Month: " + sMonth);
                Debug.WriteLine("SalespersonName: " + sSalespersonName);
                Debug.WriteLine("SalespersonID: " + sSalespersonID);
                Debug.WriteLine("CustomerName: " + sCustomerName);
                Debug.WriteLine("CustomerCode: " + sCustomerCode);
                Debug.WriteLine("InvoiceNumber: " + sInvoiceNumber);
                Debug.WriteLine("InvoiceDate: " + sInvoiceDate);
                Debug.WriteLine("StockCode: " + sStockCode);
                Debug.WriteLine("StockDescription: " + sStockDescription);
                Debug.WriteLine("Qty: " + sQty);
                Debug.WriteLine("UoM: " + sUoM);
                Debug.WriteLine("UnitPrice: " + sUnitPrice);
                Debug.WriteLine("LinePrice: " + sLinePrice);
                Debug.WriteLine("TradeDiscAmt: " + sTradeDiscAmt);
                Debug.WriteLine("CashDiscAmt: " + sCashDiscAmt);
                Debug.WriteLine("NetSaleAmt: " + sNetSaleAmt);
                Debug.WriteLine("ComAmt: " + sComAmt);
                Debug.WriteLine("ComPercentage: " + sComPercentage);

                Debug.WriteLine(ex.ToString());

                lblErrorSQL.Text = "**There is possibly a comma in the Customer Name! This is very bad for CSV files to import.";
                // tr1.Close();
            }
            finally
            {
                dt.Dispose();
            }

        }
    }

    #endregion

    #region Functions
    private string[] ParseHelper(String line, int lineRead)
    {
        MemoryStream mem = new MemoryStream(ASCIIEncoding.Default.GetBytes(line));
        TextFieldParser ReaderTemp = new TextFieldParser(mem);
        ReaderTemp.TextFieldType = FieldType.Delimited;
        ReaderTemp.SetDelimiters(new string[] { "\t", "," });
        ReaderTemp.HasFieldsEnclosedInQuotes = true;
        ReaderTemp.TextFieldType = FieldType.Delimited;
        ReaderTemp.TrimWhiteSpace = true;
        try
        {
            return ReaderTemp.ReadFields();
        }
        catch (MalformedLineException ex)
        {
            throw new MalformedLineException(String.Format(
                "Line {0} is not valid and will be skipped: {1}\r\n\r\n{2}",
                lineRead, ReaderTemp.ErrorLine, ex));
        }
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

        }

        Page.Form.Attributes.Add("enctype", "multipart/form-data");//Needed for uses with FileUpload and Update Panel!!!!



    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        lblErrorSQL.Text = "";

        TestCSV();

    }

    #endregion



}