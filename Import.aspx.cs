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

public partial class Import : System.Web.UI.Page
{

    #region Subs
    private void SaveFileToDatabase(DataTable dtExcel, string sFilePath)
    {
        //Create Connection to Excel work book 
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        string sStockCode = "";
        string sCustomer = "";
        string sPriceCode = "";
        string sNewPrice = "";
        string sStatus = "";
        db.ExecuteCommand("TRUNCATE TABLE ArNewSalesPrice");
        try
        {
            int iBlankRows = 0;
            if (dtExcel.Rows.Count > 0)
            {

                foreach (DataRow Row in dtExcel.Rows)
                {
                    sCustomer = Row["Customer"].ToString().Trim();
                    sStockCode = Row["StockCode"].ToString();
                    if (sStockCode == "")
                    {//Blank Row...
                        if (iBlankRows > 2)
                        {
                            break;
                        }
                        iBlankRows++;
                        continue;//Skip Blank Rows...
                    }

                    sPriceCode = Row["PriceCode"].ToString().Trim();
                    sNewPrice = Row["NewPrice"].ToString().Trim();
                    sStatus = Row["Status"].ToString().Trim();
                    ArNewSalesPrice n = new ArNewSalesPrice();
                    n.Customer = sCustomer;
                    n.StockCode = sStockCode;
                    n.PriceCode = sPriceCode;
                    n.NewPrice = Convert.ToDecimal(sNewPrice);
                    n.DateAdded = DateTime.Now;
                    n.Status = sStatus;

                    db.ArNewSalesPrice.InsertOnSubmit(n);
                    db.SubmitChanges();

                }
            }
            else
            {
                return;
            }

            string NewFileName = "";
            NewFileName = sFilePath + ".done";
            //No Errors then Rename File to .done
            System.IO.File.Move(sFilePath, NewFileName);
            btnConvertToSQL.Enabled = false;
            lblErrorSQL.Text = "**Import Complete( " + Path.GetFileName(sFilePath) + " ) " + dtExcel.Rows.Count + " rows imported!";
            lblErrorSQL.ForeColor = Color.Green;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            lblErrorSQL.Text = "**Error Importing to SQL!!!";
            lblErrorSQL.ForeColor = Color.Red;
        }
        finally
        {
            dtExcel.Dispose();

        }
    }
    private void UploadExcelFile(string saveDirectory, FileUpload fileUploadControl)
    {
        string filePath = Path.Combine(saveDirectory, fileUploadControl.FileName);
        string sMsg = "";
        string[] extArray = new string[] { ".xls", ".xlsx" };
        bool bTest = false;
        string sFileExtension1 = "";
        if (fileUploadControl.HasFile)
        {
            sFileExtension1 = Path.GetExtension(fileUploadControl.FileName);
        }
        else
        {
            lblErrorUpload.Text = "**Please Select a file to upload!";
            lblErrorUpload.ForeColor = Color.Red;
            return;
        }

        if (sFileExtension1 != "")
        {
            for (int i = 0; i < extArray.Length; i++)
            {
                if (extArray[i].ToUpper() == sFileExtension1.ToUpper())
                {
                    bTest = true;
                }
            }

            if (bTest == false)
            {
                sMsg += "**Please select one of the following file extension for your upload: .xls or .xlsx !<br/>";
            }
        }


        if (sMsg.Length > 0)
        {
            lblErrorUpload.Text = sMsg;
            lblErrorUpload.ForeColor = Color.Red;
            return;
        }
        try
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            string sDoneFileName = "";
            sDoneFileName = filePath + ".done";
            if (File.Exists(sDoneFileName))
            {
                File.Delete(sDoneFileName);
            }

            fileUploadControl.SaveAs(filePath);
            Session["FilePath"] = filePath;//Put file path in session variable for SQL Update...
            lblErrorUpload.Text = "**Excel file upload complete!";
            lblErrorUpload.ForeColor = Color.Green;
            btnConvertToSQL.Enabled = true;
        }
        catch (FileNotFoundException fe)
        {
            Debug.WriteLine(fe);
            lblErrorUpload.Text = "**Excel file upload Failed:File Not Found!";
            lblErrorUpload.ForeColor = Color.Red;
        }
        catch (AccessViolationException av)
        {
            Debug.WriteLine(av);
            lblErrorUpload.Text = "**Excel file upload Failed:Access Violation!";
            lblErrorUpload.ForeColor = Color.Red;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            lblErrorUpload.Text = "**Excel file upload Failed!";
            lblErrorUpload.ForeColor = Color.Red;
        }
    }

    #endregion

    #region Functions

    private DataTable GetExcelWorkSheet(string sFullPath, int workSheetNumber)
    {  //Gets a single worksheet...
        DataTable dt = new DataTable();
        try
        {
            string ExcelConnString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFullPath + ";Extended Properties=Excel 8.0;";
            OleDbConnection ExcelConnection = new OleDbConnection(ExcelConnString);
            OleDbCommand ExcelCommand = new OleDbCommand();
            ExcelCommand.Connection = ExcelConnection;
            OleDbDataAdapter ExcelAdapter = new OleDbDataAdapter(ExcelCommand);

            ExcelConnection.Open();
            DataTable ExcelSheets = ExcelConnection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            string SpreadSheetName = "[" + ExcelSheets.Rows[workSheetNumber]["TABLE_NAME"].ToString() + "]";


            ExcelCommand.CommandText = @"SELECT  * FROM " + SpreadSheetName;

            ExcelAdapter.Fill(dt);
            ExcelConnection.Close();
            return dt;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            dt.Dispose();
            return null;
        }
    }
    private DataTable GetExcelWorkSheet2013(string sFullPath, int workSheetNumber)
    {  //Gets a single worksheet...

        DataTable dt = new DataTable();
        try
        {
            string ExcelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0\"", sFullPath);

            OleDbConnection ExcelConnection = new OleDbConnection(ExcelConnString);
            OleDbCommand ExcelCommand = new OleDbCommand();
            ExcelCommand.Connection = ExcelConnection;
            OleDbDataAdapter ExcelAdapter = new OleDbDataAdapter(ExcelCommand);

            ExcelConnection.Open();
            DataTable ExcelSheets = ExcelConnection.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            string SpreadSheetName = "[" + ExcelSheets.Rows[workSheetNumber]["TABLE_NAME"].ToString() + "]";

            ExcelCommand.CommandText = @"SELECT * FROM " + SpreadSheetName;

            ExcelAdapter.Fill(dt);
            ExcelConnection.Close();
            return dt;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            dt.Dispose();
            return null;
        }
    }
    private string ValidateExcelData(DataTable dt, string sMsg)
    {
        try
        {


            //Validate Column...
            if (dt.Columns[0].ColumnName != "Customer")
            {
                sMsg += "**Column 1 name must be Customer! <br>";
            }
            if (dt.Columns[1].ColumnName != "StockCode")
            {
                sMsg += "**Column 2 name must be StockCode! <br>";
            }
            if (dt.Columns[2].ColumnName != "PriceCode")
            {
                sMsg += "**Column 3 name must be PriceCode! <br>";
            }
            if (dt.Columns[3].ColumnName != "NewPrice")
            {
                sMsg += "**Column 4 name must be NewPrice! <br>";
            }
            if (dt.Columns[4].ColumnName != "Status")
            {
                sMsg += "**Column 5 name must be Status! <br>";
            }


            string sStockCode = "";
            string sCustomer = "";
            string sPriceCode = "";
            string sNewPrice = "";
            string sStatus = "";
            //validate data...
            int iBlankRows = 0;

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                try
                {

                    sCustomer = dt.Rows[row]["Customer"].ToString().Trim();
                    sStockCode = dt.Rows[row]["StockCode"].ToString().Trim();
                    sPriceCode = dt.Rows[row]["PriceCode"].ToString().Trim();
                    sNewPrice = dt.Rows[row]["NewPrice"].ToString().Trim();
                    sStatus =  dt.Rows[row]["Status"].ToString().Trim();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    sMsg += "**" + ex.Message + " <br>";
                    return sMsg;
                }


                if (sStockCode == "")
                {//Blank Row...
                    if (iBlankRows > 2)
                    {
                        break;
                    }
                    iBlankRows++;
                    continue;//Skip Blank Rows...
                }

                if (sCustomer == "")
                {
                    sMsg += "**Customer value in  Row: " + (row + 2) + " was empty! <br>";
                }
                else
                {
                    if (CustomerInDB(sCustomer) == false)
                    {
                        sMsg += "**Customer(" + sCustomer + ") value in  Row: " + (row + 2) + " not found in database! <br>";
                    }
                }
                if (sStockCode == "")
                {
                    sMsg += "**StockCode value in  Row: " + (row + 2) + " was empty! <br>";
                }
                if (sPriceCode == "")
                {
                    sMsg += "**PriceCode value in  Row: " + (row + 2) + " was empty! <br>";
                }

                if (sNewPrice == "")
                {
                    sMsg += "**NewPrice value in  Row: " + (row + 2) + " was empty! <br>";
                }
                else
                {
                    if (!SharedFunctions.IsNumeric(sNewPrice))
                    {
                        sMsg += "**New Price must be a  numeric value in Row: " + (row + 2) + " was empty! <br>";
                    }
                }
                if (sStatus == "")
                {
                    sMsg += "**Status value in  Row: " + (row + 2) + " was empty! <br>";
                }
                if (DupsEntryInColumn(dt, sStockCode, sCustomer))
                {
                    sMsg += "**Duplicate StockCode/Customer Combination value in  Row: " + (row + 2) + " was Dup! <br>";
                }

            }//end validate data loop...

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            sMsg = "**Major error with Excel file!!!";
        }

        return sMsg;
    }
    private bool DupsEntryInColumn(DataTable dt, string sStockCode, string sCustomer)
    {
        var query = (from d in dt.AsEnumerable()
                     where d["StockCode"].ToString() == sStockCode
                     && d["Customer"].ToString() == sCustomer
                     select d);
        if (query.Count() > 1)//one or more...
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    private bool CustomerInDB(string sCustomer)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from d in db.ArCustomer
                     where d.Customer.Trim() == sCustomer.Trim()
                     select d);
        if (query.Count() > 0)
        {
            return true;
        }
        else
        {
            return false;
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
        iUserID = Convert.ToInt32(Session["UserID"]);
        int iRole = 0;
        iRole = Convert.ToInt32(SharedFunctions.GetRole(iUserID));
        if (iRole != 1)
        {
            Response.Redirect("Default.aspx");
        }
        if (!Page.IsPostBack)
        {


        }
        Page.Form.Attributes.Add("enctype", "multipart/form-data");//Needed for uses with FileUpload and Update Panel!!!!
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        lblErrorUpload.Text = "";
        // string sDirectory = "";
        string filePath = MapPath("~/images/Excel/");
        UploadExcelFile(filePath, FileUploadExcel);

    }
    protected void btnConvertToSQL_Click(object sender, EventArgs e)
    {
        lblErrorSQL.Text = "";
        DataTable dtExcel = new DataTable();
        string sMsg = "";
        string sFullPath = "";
        string sExtension = "";

        if (Session["FilePath"] != null)
        {
            sFullPath = Session["FilePath"].ToString();
        }
        else
        {
            lblErrorSQL.Text = "**File Path has become diminished!";
            lblErrorSQL.ForeColor = Color.Red;
            return;
        }
        sExtension = Path.GetExtension(sFullPath);
        if (sExtension.ToUpper() == ".XLS")
        {
            dtExcel = GetExcelWorkSheet(sFullPath, 0);
        }
        else//XLSX...
        {
            dtExcel = GetExcelWorkSheet2013(sFullPath, 0);
        }

        if (dtExcel == null)
        {
            sMsg += "**An Major Error occurred in the Excel import, the file you are trying to import has major formatting issues which are beyond the ability of this application to handle!!";
        }
        if (sMsg.Length > 0)//error in xls file format...
        {
            lblErrorSQL.ForeColor = Color.Red;
            lblErrorSQL.Text = "**Update Failed!<br/>" + sMsg;
            return;	//error...your out of here...
        }


        sMsg = ValidateExcelData(dtExcel, sMsg);

        if (sMsg.Length > 0)//error in xls file format...
        {
            lblErrorSQL.ForeColor = Color.Red;
            lblErrorSQL.Text = "**Update Failed!<br/>" + sMsg;
            return;	//error...your out of here...
        }



        sExtension = Path.GetExtension(sFullPath);

        SaveFileToDatabase(dtExcel, sFullPath);


        dtExcel.Dispose();
    }

    #endregion

}