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

public partial class PriceChangesUpdateSystem : System.Web.UI.Page
{


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


    #region Step 1
    #region Subs
    private void SaveFileToDatabaseStep1(DataTable dtExcel, string sFilePath)
    {
        //Create Connection to Excel work book 
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        string sStockCode = "";
        string sA = "";
        string sB = "";
        string sC = "";
        string sD = "";
        string sE = "";
        string sPound = "";
        string sStatus = "";
        string sContract = "";
        db.ExecuteCommand("TRUNCATE TABLE ArNewPriceHolding");
        try
        {
            int iBlankRows = 0;
            if (dtExcel.Rows.Count > 0)
            {

                foreach (DataRow Row in dtExcel.Rows)
                {

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

                    sA = Row["A"].ToString().Trim();
                    sB = Row["B"].ToString().Trim();
                    sC = Row["C"].ToString().Trim();
                    sD = Row["D"].ToString().Trim();
                    sE = Row["E"].ToString().Trim();
                    sPound = Row["Pound"].ToString().Trim();
                    sStatus = Row["Status"].ToString().Trim();
                    sContract = Row["Contract"].ToString().Trim();
                    ArNewPriceHolding n = new ArNewPriceHolding();
                    n.StockCode = sStockCode;
                    n.A = Convert.ToDouble(sA);
                    n.B = Convert.ToDouble(sB);
                    n.C = Convert.ToDouble(sC);
                    n.D = Convert.ToDouble(sD);
                    n.E = Convert.ToDouble(sE);
                    n.Pound = Convert.ToDouble(sPound);
                    n.Status = sStatus;
                    if (!String.IsNullOrEmpty(sContract))
                    {
                        n.Contract = Convert.ToDouble(sContract);
                    }
                    db.ArNewPriceHolding.InsertOnSubmit(n);
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
            btnConvertToSQLStep1.Enabled = false;
            lblErrorSQLStep1.Text = "**Import Complete( " + Path.GetFileName(sFilePath) + " ) " + dtExcel.Rows.Count + " rows imported!";
            lblErrorSQLStep1.ForeColor = Color.Green;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            lblErrorSQLStep1.Text = "**Error Importing to SQL!!!";
            lblErrorSQLStep1.ForeColor = Color.Red;
        }
        finally
        {
            dtExcel.Dispose();

        }
    }
    private void UploadExcelFileStep1(string saveDirectory, FileUpload fileUploadControl)
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
            lblErrorUploadStep1.Text = "**Please Select a file to upload!";
            lblErrorUploadStep1.ForeColor = Color.Red;
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
            lblErrorUploadStep1.Text = sMsg;
            lblErrorUploadStep1.ForeColor = Color.Red;
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
            lblErrorUploadStep1.Text = "**Excel file upload complete!";
            lblErrorUploadStep1.ForeColor = Color.Green;
            btnConvertToSQLStep1.Enabled = true;
        }
        catch (FileNotFoundException fe)
        {
            Debug.WriteLine(fe);
            lblErrorUploadStep1.Text = "**Excel file upload Failed:File Not Found!";
            lblErrorUploadStep1.ForeColor = Color.Red;
        }
        catch (AccessViolationException av)
        {
            Debug.WriteLine(av);
            lblErrorUploadStep1.Text = "**Excel file upload Failed:Access Violation!";
            lblErrorUploadStep1.ForeColor = Color.Red;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            lblErrorUploadStep1.Text = "**Excel file upload Failed!";
            lblErrorUploadStep1.ForeColor = Color.Red;
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
    private void GetNewPriceData()
    {
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        DataTable dt = new DataTable();
        sSQL = "EXEC spExportNewPriceTable";

        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");
        if (dt.Rows.Count > 0)
        {
            Session["dtCustomerReport"] = dt;


        }
        else
        {
            lblErrorSQLStep1.Text = "No results found!!";
            lblErrorSQLStep1.ForeColor = Color.Red;

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
            string ExcelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml; HDR=YES; IMEX=1'", sFullPath);//Updated 11-12-2020...

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
    private string ValidateExcelDataStep1(DataTable dt, string sMsg)
    {
        try
        {

            //Validate Column...
            if (dt.Columns[0].ColumnName != "StockCode")
            {
                sMsg += "**Column 1 name must be Stock Code! <br>";
            }
            if (dt.Columns[1].ColumnName != "A")
            {
                sMsg += "**Column 2 name must be A! <br>";
            }
            if (dt.Columns[2].ColumnName != "B")
            {
                sMsg += "**Column 3 name must be B! <br>";
            }
            if (dt.Columns[3].ColumnName != "C")
            {
                sMsg += "**Column 4 name must be C! <br>";
            }
            if (dt.Columns[4].ColumnName != "D")
            {
                sMsg += "**Column 5 name must be D! <br>";
            }
            if (dt.Columns[5].ColumnName != "E")
            {
                sMsg += "**Column 6 name must be E! <br>";
            }
            if (dt.Columns[6].ColumnName.ToUpper() != "POUND")
            {
                sMsg += "**Column 7 name must be Pound! <br>";
            }
            if (dt.Columns[7].ColumnName.ToUpper() != "STATUS")
            {
                sMsg += "**Column 8 name must be Status! <br>";
            }
            if (dt.Columns[8].ColumnName.ToUpper() != "CONTRACT")
            {
                sMsg += "**Column 9 name must be Contract! <br>";
            }

            string sStockCode = "";
            string sA = "";
            string sB = "";
            string sC = "";
            string sD = "";
            string sE = "";
            string sPound = "";
            string sStatus = "";
            string sContract = "";
            //validate data...
            int iBlankRows = 0;

            for (int row = 0; row < dt.Rows.Count; row++)
            {
                try
                {


                    sStockCode = dt.Rows[row]["StockCode"].ToString().Trim();
                    sA = dt.Rows[row]["A"].ToString().Trim();
                    sB = dt.Rows[row]["B"].ToString().Trim();
                    sC = dt.Rows[row]["C"].ToString().Trim();
                    sD = dt.Rows[row]["D"].ToString().Trim();
                    sE = dt.Rows[row]["E"].ToString().Trim();
                    sPound = dt.Rows[row]["Pound"].ToString().Trim();
                    sStatus = dt.Rows[row]["Status"].ToString().Trim();
                    sContract = dt.Rows[row]["Contract"].ToString().Trim();
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


                if (sStockCode == "")
                {
                    sMsg += "**StockCode value in  Row: " + (row + 2) + " was empty! <br>";
                }
                if (sA == "")
                {
                    sMsg += "**A value in  Row: " + (row + 2) + " was empty! <br>";
                }
                else
                {
                    if (!SharedFunctions.IsNumeric(sA))
                    {
                        sMsg += "**A must be a  numeric value in Row: " + (row + 2) + "! <br>";
                    }
                }
                if (sB == "")
                {
                    sMsg += "**B value in  Row: " + (row + 2) + " was empty! <br>";
                }
                else
                {
                    if (!SharedFunctions.IsNumeric(sB))
                    {
                        sMsg += "**B must be a  numeric value in Row: " + (row + 2) + "! <br>";
                    }
                }
                if (sC == "")
                {
                    sMsg += "**C value in  Row: " + (row + 2) + " was empty! <br>";
                }
                else
                {
                    if (!SharedFunctions.IsNumeric(sC))
                    {
                        sMsg += "**C must be a  numeric value in Row: " + (row + 2) + "! <br>";
                    }
                }
                if (sD == "")
                {
                    sMsg += "**D value in  Row: " + (row + 2) + " was empty! <br>";
                }
                else
                {
                    if (!SharedFunctions.IsNumeric(sD))
                    {
                        sMsg += "**D must be a  numeric value in Row: " + (row + 2) + "! <br>";
                    }
                }
                if (sE == "")
                {
                    sMsg += "**E value in  Row: " + (row + 2) + " was empty! <br>";
                }
                else
                {
                    if (!SharedFunctions.IsNumeric(sE))
                    {
                        sMsg += "**E must be a  numeric value in Row: " + (row + 2) + "! <br>";
                    }
                }
                if (sPound == "")
                {
                    sMsg += "**Pound value in  Row: " + (row + 2) + " was empty! <br>";
                }
                else
                {
                    if (!SharedFunctions.IsNumeric(sPound))
                    {
                        sMsg += "**Pound must be a  numeric value in Row: " + (row + 2) + "! <br>";
                    }
                }
                if (sContract != "")//Optional...
                {       
                    if (!SharedFunctions.IsNumeric(sContract))
                    {
                        sMsg += "**Contract must be a  numeric value in Row: " + (row + 2) + "! <br>";
                    }
                }
                if (sStatus == "")
                {
                    sMsg += "**Status value in  Row: " + (row + 2) + " was empty! <br>";
                }

                if (DupsEntryInColumn(dt, sStockCode))
                {
                    sMsg += "**Duplicate StockCode value in  Row: " + (row + 2) + " was Dup! <br>";
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
    private bool DupsEntryInColumn(DataTable dt, string sStockCode)
    {
        var query = (from d in dt.AsEnumerable()
                     where d["StockCode"].ToString() == sStockCode
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


    #endregion

    #region Events

    protected void btnUploadStep1_Click(object sender, EventArgs e)
    {
        lblErrorUploadStep1.Text = "";
        // string sDirectory = "";
        string filePath = MapPath("~/images/Excel/");
        UploadExcelFileStep1(filePath, FileUploadExcelStep1);

    }
    protected void btnConvertToSQLStep1_Click(object sender, EventArgs e)
    {
        lblErrorSQLStep1.Text = "";
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
            lblErrorSQLStep1.Text = "**File Path has become diminished!";
            lblErrorSQLStep1.ForeColor = Color.Red;
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
            lblErrorSQLStep1.ForeColor = Color.Red;
            lblErrorSQLStep1.Text = "**Update Failed!<br/>" + sMsg;
            return;	//error...your out of here...
        }


        sMsg = ValidateExcelDataStep1(dtExcel, sMsg);

        if (sMsg.Length > 0)//error in xls file format...
        {
            lblErrorSQLStep1.ForeColor = Color.Red;
            lblErrorSQLStep1.Text = "**Update Failed!<br/>" + sMsg;
            return;	//error...your out of here...
        }



        sExtension = Path.GetExtension(sFullPath);

        SaveFileToDatabaseStep1(dtExcel, sFullPath);


        dtExcel.Dispose();
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        lblErrorSQLStep1.Text = "";
        GetNewPriceData();//Put data into DataTable in session variable...

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";
        if (Session["dtCustomerReport"] == null)
        {
            lblErrorSQLStep1.Text = "**No Data in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtCustomerReport"];

        dt.TableName = "dtCustomerReport";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "NewCustomerPricelist" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    #endregion
    #endregion


    #region Step 2  

    #region Subs
    private void SaveFileToDatabaseStep2(DataTable dtExcel, string sFilePath)
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
            btnConvertToSQLStep2.Enabled = false;
            lblErrorSQLStep2.Text = "**Import Complete( " + Path.GetFileName(sFilePath) + " ) " + dtExcel.Rows.Count + " rows imported!";
            lblErrorSQLStep2.ForeColor = Color.Green;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            lblErrorSQLStep2.Text = "**Error Importing to SQL!!!";
            lblErrorSQLStep2.ForeColor = Color.Red;
        }
        finally
        {
            dtExcel.Dispose();

        }
    }
    private void UploadExcelFileStep2(string saveDirectory, FileUpload fileUploadControl)
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
            lblErrorUploadStep2.Text = "**Please Select a file to upload!";
            lblErrorUploadStep2.ForeColor = Color.Red;
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
            lblErrorUploadStep2.Text = sMsg;
            lblErrorUploadStep2.ForeColor = Color.Red;
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
            lblErrorUploadStep2.Text = "**Excel file upload complete!";
            lblErrorUploadStep2.ForeColor = Color.Green;
            btnConvertToSQLStep2.Enabled = true;
        }
        catch (FileNotFoundException fe)
        {
            Debug.WriteLine(fe);
            lblErrorUploadStep2.Text = "**Excel file upload Failed:File Not Found!";
            lblErrorUploadStep2.ForeColor = Color.Red;
        }
        catch (AccessViolationException av)
        {
            Debug.WriteLine(av);
            lblErrorUploadStep2.Text = "**Excel file upload Failed:Access Violation!";
            lblErrorUploadStep2.ForeColor = Color.Red;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            lblErrorUploadStep2.Text = "**Excel file upload Failed!";
            lblErrorUploadStep2.ForeColor = Color.Red;
        }
    }

    #endregion

    #region Functions

    private string ValidateExcelDataStep2(DataTable dt, string sMsg)
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
                    sStatus = dt.Rows[row]["Status"].ToString().Trim();
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
 
    protected void btnUploadStep2_Click(object sender, EventArgs e)
    {
        lblErrorUploadStep2.Text = "";
        // string sDirectory = "";
        string filePath = MapPath("~/images/Excel/");
        UploadExcelFileStep2(filePath, FileUploadExcelStep2);

    }
    protected void btnConvertToSQLStep2_Click(object sender, EventArgs e)
    {
        lblErrorSQLStep2.Text = "";
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
            lblErrorSQLStep2.Text = "**File Path has become diminished!";
            lblErrorSQLStep2.ForeColor = Color.Red;
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
            lblErrorSQLStep2.ForeColor = Color.Red;
            lblErrorSQLStep2.Text = "**Update Failed!<br/>" + sMsg;
            return;	//error...your out of here...
        }


        sMsg = ValidateExcelDataStep2(dtExcel, sMsg);

        if (sMsg.Length > 0)//error in xls file format...
        {
            lblErrorSQLStep2.ForeColor = Color.Red;
            lblErrorSQLStep2.Text = "**Update Failed!<br/>" + sMsg;
            return;	//error...your out of here...
        }



        sExtension = Path.GetExtension(sFullPath);

        SaveFileToDatabaseStep2(dtExcel, sFullPath);


        dtExcel.Dispose();
    }
    #endregion
    #endregion
}