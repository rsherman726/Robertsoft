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
using ClosedXML.Excel;
using System.Data.OleDb;
using ExcelDataReader;


public partial class InventoryFlagReport : System.Web.UI.Page
{
    #region Subs
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
                          where !(Convert.ToInt32(a.StockCode) >= 700000 && Convert.ToInt32(a.StockCode) <= 799999)
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
    private void ExportToExcel(DataTable dt, string sFileName)
    {
        if (dt.Rows.Count > 0)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Reports");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + sFileName + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream, false);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

        }
    }
    private DataTable RunReport()
    {
        string sMsg = "";


        //Stock Codes...

        string sStockCode = "";
        string sQtyInputs = "";
        int iIndexOfPipe = 0;

        for (int i = 0; i < gvForm.Rows.Count; i++)
        {
            TextBox txtQuantity = (TextBox)gvForm.Rows[i].FindControl("txtQuantity");
            Label lblStockCode = (Label)gvForm.Rows[i].FindControl("lblStockCode");

            sStockCode += lblStockCode.Text.Trim() + "|";
            sQtyInputs += txtQuantity.Text.Trim() + "|";
        }


        if (sStockCode.Trim().EndsWith("|"))
        {
            iIndexOfPipe = sStockCode.Trim().LastIndexOf("|");
            sStockCode = sStockCode.Remove(iIndexOfPipe).Trim();
            sStockCode = "'" + sStockCode + "'";
        }
        if (sQtyInputs.Trim().EndsWith("|"))
        {
            iIndexOfPipe = sQtyInputs.Trim().LastIndexOf("|");
            sQtyInputs = sQtyInputs.Remove(iIndexOfPipe).Trim();
            sQtyInputs = "'" + sQtyInputs + "'";
        }


        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        //Validation... 


        if (sMsg.Length > 0)
        {
            lblErrorRange.Text = sMsg;
            return null;
        }

        sSQL = "EXEC spGetInventoryRequiredFlaggedReport @StockCodes =" + sStockCode + ",@QtyInputs =" + sQtyInputs;

        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");

        int iInitialColumnCount = 0;

        iInitialColumnCount = dt.Columns.Count;

        dt.Columns.Add("Total Req.");
        dt.Columns.Add("On Hand");
        dt.Columns.Add("Short");
        dt.Columns.Add("On POs");
        dt.Columns.Add("PO Date Required");


        string sMStockCode = "";
        int iWareHouse = 0;
        iWareHouse = Convert.ToInt32(rblWarehouse.SelectedValue);
        decimal dcQtyNeeded = 0;
        decimal dcQtyNeededTotal = 0;
        decimal dcOnHand = 0;
        decimal dcShort = 0;
        decimal dcOnPOs = 0;
        string sPODateRequired = "";
        foreach (DataRow row in dt.Rows)
        {
            dcQtyNeeded = 0;
            sMStockCode = row["MStockCode"].ToString();
            for (int i = 3; i < iInitialColumnCount; i++)
            {
                dcQtyNeeded += Convert.ToDecimal(row[i]);
            }
            dcQtyNeededTotal = dcQtyNeeded;
            row["Total Req."] = dcQtyNeededTotal;
            dcOnHand = GetOnHand(sMStockCode, iWareHouse);
            row["On Hand"] = dcOnHand;
            dcShort = dcQtyNeeded - dcOnHand;
            if (dcShort < 0)
            {
                dcShort = 0;
            }
            row["Short"] = dcShort;
            dcOnPOs = GetOnPOs(sMStockCode, iWareHouse);
            row["On POs"] = dcOnPOs;

            sPODateRequired = GetPODateRequired(sMStockCode);
            if (sPODateRequired == "1/1/2000")
            {
                sPODateRequired = "--";
            }
            row["PO Date Required"] = sPODateRequired;
        }

        foreach (DataRow Row in dt.Rows)
        {
            foreach (DataColumn c in dt.Columns)
            {
                Debug.WriteLine(c.ColumnName + ": " + Row[c.ColumnName].ToString());
            }
            Debug.WriteLine("----------------------------------------------------");
        }

        if (dt.Rows.Count > 0)
        {
            Session["dtInventoryRequirements"] = dt;
            gvReport.DataSource = dt;
            gvReport.DataBind();


        }
        else
        {
            lblError.Text = "No results found!!";
            lblError.ForeColor = Color.Red;


            gvReport.DataSource = null;
            gvReport.DataBind();
        }
        try
        {
            dt.Dispose();
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {

        }
        return dt;

    }
    //Import...
    private void UploadExcelFile(FileUpload fileUploadControl)
    {//Use new Excel Data Reader to use stream so you dont have to save file to drive!!!
        DataTable dtExcel = new DataTable();
        DataTable dt = new DataTable();
        string sStockCode = "";
        int iIndexOfPipe = 0;
        string sMsg = "";


        if (!fileUploadControl.HasFile)
        {
            lblImportRunError.Text = "**Please Select a file to upload!";
            lblImportRunError.ForeColor = Color.Red;
            return;
        }



        if (sMsg.Length > 0)
        {
            lblImportRunError.Text = sMsg;
            lblImportRunError.ForeColor = Color.Red;
            return;
        }
        try
        {
            MemoryStream memStream = new MemoryStream(fileUploadControl.FileBytes.ToArray());
            string fileName = fileUploadControl.FileName;
            string fileExtension = Path.GetExtension(fileUploadControl.FileName);



            if (fileExtension == ".xls" || fileExtension == ".xlsx")
            {
                ExcelDataReader.IExcelDataReader excelReader;
                if (fileExtension == ".xls")
                {
                    excelReader = ExcelDataReader.ExcelReaderFactory.CreateBinaryReader(memStream);
                }
                else
                {
                    excelReader = ExcelDataReader.ExcelReaderFactory.CreateOpenXmlReader(memStream);
                }

                var dataSet = excelReader.AsDataSet(new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true // Use first row is ColumnName here :D
                    }
                });
                if (dataSet.Tables.Count > 0)
                {
                    dtExcel = dataSet.Tables[0];
                }




            }
            else
            {
                lblImportRunError.Text = "**File must be an Excel file!";
                lblImportRunError.ForeColor = Color.Red;
                return;
            }



            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

            foreach (DataRow row in dtExcel.Rows)
            {
                sStockCode += row["StockCode"].ToString().Trim() + "|";
            }
            if (sStockCode.Trim().EndsWith("|"))
            {
                iIndexOfPipe = sStockCode.Trim().LastIndexOf("|");
                sStockCode = sStockCode.Remove(iIndexOfPipe).Trim();
                sStockCode = "'" + sStockCode + "'";
            }

            string sSQL = "";

            sSQL = "EXEC spGetIngredientsReport @StockCodes =" + sStockCode;

            Debug.WriteLine(sSQL);

            dt = SharedFunctions.getDataTable(sSQL, conn, "dt");

            if (dt.Rows.Count > 0)
            {
                Session["dtInventoryRequirements"] = dt;
                gvReport.DataSource = dt;
                gvReport.DataBind();


            }
            else
            {
                lblError.Text = "No results found!!";
                lblError.ForeColor = Color.Red;


                gvReport.DataSource = null;
                gvReport.DataBind();
            }
            dt.Dispose();

            lblImportRunError.Text = "**Excel file upload complete!";
            lblImportRunError.ForeColor = Color.Green;
            btnRun.Enabled = true;
        }
        catch (FileNotFoundException fe)
        {
            Debug.WriteLine(fe);
            lblImportRunError.Text = "**Excel file upload Failed:File Not Found!";
            lblImportRunError.ForeColor = Color.Red;
        }
        catch (AccessViolationException av)
        {
            Debug.WriteLine(av);
            lblImportRunError.Text = "**Excel file upload Failed:Access Violation!";
            lblImportRunError.ForeColor = Color.Red;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            lblImportRunError.Text = "**Excel file upload Failed!";
            lblImportRunError.ForeColor = Color.Red;
        }
    }


    #endregion

    #region Functions
    private decimal GetOnHand(string sIngredientStockCode, int iWarehouse)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            decimal dcOnHand = 0;
            var query = (from iw in db.InvWarehouse
                         where
                           iw.StockCode == sIngredientStockCode &&
                           iw.Warehouse == Convert.ToString(iWarehouse)
                         select new
                         {
                             OnHand = (iw.QtyOnHand + iw.QtyInTransit)
                         });
            if (query.Count() > 0)
            {
                foreach (var a in query)
                {
                    dcOnHand = Convert.ToDecimal(a.OnHand);
                }
            }
            return dcOnHand;
        }
    }
    private decimal GetOnPOs(string sIngredientStockCode, int iWarehouse)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            decimal dcOnPOs = 0;
            var query = (from iw in db.InvWarehouse
                         where
                           iw.StockCode == sIngredientStockCode &&
                           iw.Warehouse == Convert.ToString(iWarehouse)
                         select new
                         {
                             OnPOs = (iw.QtyOnOrder + iw.QtyOnBackOrder)
                         });
            if (query.Count() > 0)
            {
                foreach (var a in query)
                {
                    dcOnPOs = Convert.ToDecimal(a.OnPOs);
                }
            }
            return dcOnPOs;
        }
    }
    private string GetPODateRequired(string sIngredientStockCode)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DateTime dtPODateRequired = new DateTime(2000, 1, 1);
            var query = (from po in
                            (from po in db.PorMasterDetail
                             where
                                po.MStockCode.Trim() == sIngredientStockCode &&
                               (po.MCompleteFlag != "Y" &&
                               po.MCompleteFlag != "N")
                             select new
                             {
                                 po.MOrigDueDate,
                                 Dummy = "x"
                             })
                         group po by new { po.Dummy } into g
                         select new
                         {
                             PODateRequired = (DateTime?)g.Min(p => p.MOrigDueDate)
                         });
            if (query.Count() > 0)
            {
                foreach (var a in query)
                {
                    dtPODateRequired = Convert.ToDateTime(a.PODateRequired);
                }
            }
            return dtPODateRequired.ToShortDateString();
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

            LoadParentStockCodesIngredients(lbParentStockCode);


        }//End postback

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
    protected void lbnCreateAddFormProducts_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (lbParentStockCode.SelectedIndex == -1)
        {
            lblError.Text = "**Please select StockCode(s)!!";
            lblError.ForeColor = Color.Red;
            return;
        }

        DataTable dt = new DataTable();
        DataRow drRow = null;

        DataColumn column = new DataColumn();
        column.DataType = System.Type.GetType("System.Int32");
        column.AutoIncrement = true;
        column.AutoIncrementSeed = 1;
        column.AutoIncrementStep = 1;
        column.ColumnName = "ID";
        column.Unique = true;

        dt.Columns.Add(column);

        dt.Columns.Add("StockCode", typeof(string));
        dt.Columns.Add("Description", typeof(string));

        foreach (ListItem li in lbParentStockCode.Items)
        {
            if (li.Selected)
            {
                drRow = dt.NewRow();
                drRow["StockCode"] = li.Value;
                drRow["Description"] = li.Text;
                dt.Rows.Add(drRow);
            }

        }


        gvForm.DataSource = dt;
        gvForm.DataBind();
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (lbParentStockCode.SelectedIndex == -1)
        {
            lblError.Text = "**Please select StockCode(s)!!";
            lblError.ForeColor = Color.Red;
            return;
        }
        if (gvForm.Rows.Count == 0)
        {
            lblError.Text = "**StockCode(s) Quantities Form is not generated!!";
            lblError.ForeColor = Color.Red;
            return;
        }
        for (int i = 0; i < gvForm.Rows.Count; i++)
        {
            TextBox txtQuantity = (TextBox)gvForm.Rows[i].FindControl("txtQuantity");
            Label lblStockCode = (Label)gvForm.Rows[i].FindControl("lblStockCode");
            if (txtQuantity.Text.Trim() == "")
            {
                lblError.Text += "**StockCode: " + lblStockCode.Text + " quantity was left blank!!<br>";
                lblError.ForeColor = Color.Red;
            }
        }
        if (lblError.Text.Length > 0)
        {
            return;
        }

        RunReport();

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        gvForm.DataSource = null;
        gvForm.DataBind();
        gvReport.DataSource = null;
        gvReport.DataBind();
        lbParentStockCode.SelectedIndex = -1;
        rblWarehouse.SelectedIndex = 0;
    }
    protected void btnExportReport_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        try
        {
            if (gvReport.Rows.Count == 0)
            {
                lblError.Text = "**No report has ran!!";
                lblError.ForeColor = Color.Red;
                return;
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string sFilesName = "";


            //Add columns to DataTable.
            foreach (TableCell cell in gvReport.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }

            //Loop through the GridView and copy rows.
            foreach (GridViewRow row in gvReport.Rows)
            {
                dt.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dt.Rows[row.RowIndex][i] = row.Cells[i].Text;
                }
            }

            dt.TableName = "dt";


            sFilesName = "InventoryRequirements_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
            ExportToExcel(dt.Copy(), sFilesName);

            //send session variable dtReport to Excel...

            ds.Dispose();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcTotalRequired = 0;
        DataTable dt = (DataTable)Session["dtInventoryRequirements"];
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].CssClass = "CenterAligner";
            e.Row.Cells[1].CssClass = "CenterAligner";
            if (dt.Columns.Count > 5)
            {
                e.Row.Cells[2].CssClass = "CenterAligner";

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    e.Row.Cells[i].CssClass = "CenterAligner";
                }
            }
            else
            {
                e.Row.Cells[2].CssClass = "CenterAligner";
                e.Row.Cells[3].CssClass = "CenterAligner";
                e.Row.Cells[4].CssClass = "CenterAligner";
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DataColumnCollection columns = dt.Columns;

            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
            if (dt.Columns.Count > 5)
            {
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            }

            e.Row.Cells[0].Width = Unit.Pixel(100);
            e.Row.Cells[1].Width = Unit.Pixel(300);
            if (dt.Columns.Count > 5)
            {
                e.Row.Cells[2].Width = Unit.Pixel(50);
            }
            string sMStockCode = "";
            sMStockCode = e.Row.Cells[0].Text;
            int iWareHouse = 0;
            iWareHouse = Convert.ToInt32(rblWarehouse.SelectedValue);

            if (dt.Columns.Count > 5)
            {
                for (int i = 3; i < dt.Columns.Count - 1; i++)
                {
                    //e.Row.Cells[i].BorderColor = Color.Gray;
                    //e.Row.Cells[i].BorderStyle = BorderStyle.Solid;
                    //e.Row.Cells[i].BorderWidth = Unit.Pixel(1);
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                    e.Row.Cells[i].Width = Unit.Pixel(100);
                    if (e.Row.Cells[i].Text == "0.00")
                    {
                        e.Row.Cells[i].Text = "--";
                    }
                    else//Is numeric...
                    {
                        e.Row.Cells[i].Text = Convert.ToDecimal(e.Row.Cells[i].Text).ToString("#,0.00");
                    }
                }
            }
            else
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            }
            for (int i = dt.Columns.Count - 1; i < dt.Columns.Count; i++)
            {
                //e.Row.Cells[i].BorderColor = Color.Gray;
                //e.Row.Cells[i].BorderStyle = BorderStyle.Solid;
                //e.Row.Cells[i].BorderWidth = Unit.Pixel(1);
                e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;


            }


        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {//Runs once...


        }
    }
    protected void gvForm_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    //Import...
    protected void btnRun_Click(object sender, EventArgs e)
    {
        lblImportRunError.Text = "";
        UploadExcelFile(FileUploadExcel);
    }

    #endregion
}