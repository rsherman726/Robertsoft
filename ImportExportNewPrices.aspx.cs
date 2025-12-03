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

public partial class ImportExportNewPrices : System.Web.UI.Page
{
    #region Subs
    private void SaveFileToDatabase(DataTable dtExcel, string sFilePath)
    {
        //Create Connection to Excel work book 
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        string sStockCode = "";
        string sA = "";
        string sB = "";
        string sC = "";
        string sD = "";
        string sPound = "";
        string sStatus = "";
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
                    sPound = Row["Pound"].ToString().Trim();
                    sStatus = Row["Status"].ToString().Trim();
                    ArNewPriceHolding n = new ArNewPriceHolding();
                    n.StockCode = sStockCode;              
                    n.A = Convert.ToDouble(sA);
                    n.B = Convert.ToDouble(sB);
                    n.C = Convert.ToDouble(sC);
                    n.D = Convert.ToDouble(sD);
                    n.Pound = Convert.ToDouble(sPound);
                    n.Status = sStatus;
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

        ExcelHelper.ToExcel(ds, sFileName , Page.Response);

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
            lblErrorSQL.Text = "No results found!!";
            lblErrorSQL.ForeColor = Color.Red;
            
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
            if (dt.Columns[5].ColumnName.ToUpper() != "POUND")
            {
                sMsg += "**Column 6 name must be Pound! <br>";
            }
            if (dt.Columns[6].ColumnName.ToUpper() != "STATUS")
            {
                sMsg += "**Column 7 name must be Status! <br>";
            }

            string sStockCode = "";
            string sA = "";
            string sB = "";
            string sC = "";
            string sD = "";
            string sPound = "";
            string sStatus = "";
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
                    sPound = dt.Rows[row]["Pound"].ToString().Trim();
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
                        sMsg += "**A must be a  numeric value in Row: " + (row + 2) + " was empty! <br>";
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
                        sMsg += "**B must be a  numeric value in Row: " + (row + 2) + " was empty! <br>";
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
                        sMsg += "**C must be a  numeric value in Row: " + (row + 2) + " was empty! <br>";
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
                        sMsg += "**D must be a  numeric value in Row: " + (row + 2) + " was empty! <br>";
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
                        sMsg += "**Pound must be a  numeric value in Row: " + (row + 2) + " was empty! <br>";
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
    protected void btnExport_Click(object sender, EventArgs e)
    {
        lblErrorSQL.Text = "";
        GetNewPriceData();//Put data into DataTable in session variable...

        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";
        if (Session["dtCustomerReport"] == null)
        {
            lblErrorSQL.Text = "**No Data in memory to export!";
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


}