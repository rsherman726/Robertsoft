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

public partial class ManageInvMovementsOther : System.Web.UI.Page
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
                          where (Convert.ToInt32(a.StockCode) > 100000)
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

    private void SaveData()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            try
            {
                for (int i = 0; i < gvForm.Rows.Count; i++)
                {
                    TextBox txtFreight = (TextBox)gvForm.Rows[i].FindControl("txtFreight");
                    TextBox txtTariff = (TextBox)gvForm.Rows[i].FindControl("txtTariff");
                    TextBox txtMisc = (TextBox)gvForm.Rows[i].FindControl("txtMisc");
                    Label lblStockCode = (Label)gvForm.Rows[i].FindControl("lblStockCode");

                    InvMovementsOther inv = db.InvMovementsOther.Single(p => p.StockCode == lblStockCode.Text);
                    inv.Freight = Math.Round( Convert.ToDecimal(txtFreight.Text.Trim()),3);
                    inv.Tariff = Math.Round(Convert.ToDecimal(txtTariff.Text.Trim()), 3);
                    inv.Misc = Math.Round(Convert.ToDecimal(txtMisc.Text.Trim()), 3);
                    inv.DateAdded = DateTime.Now;
                    db.SubmitChanges();


                    lblError.Text = "**Updated successfully!";
                    lblError.ForeColor = Color.Green;

                }
            }
            catch (Exception ex)
            {
                lblError.Text = "**Update Failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }


        }
    }
    private void CreateForm()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lblError.Text = "";
            if (lbParentStockCode.SelectedIndex == -1)
            {
                lblError.Text = "**Please select StockCode(s)!!";
                lblError.ForeColor = Color.Red;
                return;
            }
            List<string> lStockCodes = new List<string>();
            DataTable dt = new DataTable();
            string sStockCode = "";
            foreach (ListItem li in lbParentStockCode.Items)
            {
                if (li.Selected)
                {
                    sStockCode = li.Value;
                    lStockCodes.Add(sStockCode);
                }
            }

            var query = (from inv in db.InvMovementsOther
                         join m in db.InvMaster on inv.StockCode equals m.StockCode
                         where lStockCodes.Contains(inv.StockCode)
                         select new
                         {
                             m.Description,
                             inv.StockCode,
                             inv.Freight,
                             inv.Tariff,
                             inv.Misc,
                         });


            dt = SharedFunctions.ToDataTable(db, query);

            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            dt.Columns.Add(column);
            //Set values for existing rows...
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["ID"] = i + 1;
            }

            gvForm.DataSource = dt;
            gvForm.DataBind();
        }
    }


    #endregion

    #region Functions
    private DataTable RunReport()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            //Stock Codes...

            string sStockCode = "";
            List<string> lStockCodes = new List<string>();

            foreach (ListItem li in lbParentStockCode.Items)
            {
                if (li.Selected)
                {
                    sStockCode = li.Value;
                    lStockCodes.Add(sStockCode);
                }
            }
            DataTable dt = new DataTable();

            var query = (from inv in db.InvMovementsOther
                         join m in db.InvMaster on inv.StockCode equals m.StockCode
                         where lStockCodes.Contains(inv.StockCode)
                         select new
                         {                             
                             m.Description,
                             inv.StockCode,
                             inv.Freight,
                             inv.Tariff,
                             inv.Misc,
                         });


            dt = SharedFunctions.ToDataTable(db, query);

            //foreach (DataRow Row in dt.Rows)
            //{
            //    foreach (DataColumn c in dt.Columns)
            //    {
            //        Debug.WriteLine(c.ColumnName + ": " + Row[c.ColumnName].ToString());
            //    }
            //    Debug.WriteLine("----------------------------------------------------");
            //}
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            dt.Columns.Add(column);
            //Set values for existing rows...
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["ID"] = i + 1;
            }

            if (dt.Rows.Count > 0)
            {
                Session["dtInvMovementsOther"] = dt;
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
    }
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
        CreateForm();
    }



    protected void btnSave_Click(object sender, EventArgs e)
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
            lblError.Text = "**StockCode(s) Form is not generated!!";
            lblError.ForeColor = Color.Red;
            return;
        }
        for (int i = 0; i < gvForm.Rows.Count; i++)
        {
            TextBox txtFreight = (TextBox)gvForm.Rows[i].FindControl("txtFreight");
            TextBox txtTariff = (TextBox)gvForm.Rows[i].FindControl("txtTariff");
            TextBox txtMisc = (TextBox)gvForm.Rows[i].FindControl("txtMisc");
            Label lblStockCode = (Label)gvForm.Rows[i].FindControl("lblStockCode");
            if (txtFreight.Text.Trim() == "")
            {
                lblError.Text += "**StockCode: " + lblStockCode.Text + " Freight was left blank!!<br>";
                lblError.ForeColor = Color.Red;
            }
            else
            {
                if (SharedFunctions.IsNumeric(txtFreight.Text.Trim()) == false)
                {
                    lblError.Text += "**StockCode: " + lblStockCode.Text + " Freight must be numeric!!<br>";
                    lblError.ForeColor = Color.Red;
                }
            }
            if (txtTariff.Text.Trim() == "")
            {
                lblError.Text += "**StockCode: " + lblStockCode.Text + " Tariff was left blank!!<br>";
                lblError.ForeColor = Color.Red;
            }
            else
            {
                if (SharedFunctions.IsNumeric(txtTariff.Text.Trim()) == false)
                {
                    lblError.Text += "**StockCode: " + lblStockCode.Text + " Tariff must be numeric!!<br>";
                    lblError.ForeColor = Color.Red;
                }
            }
            if (txtMisc.Text.Trim() == "")
            {
                lblError.Text += "**StockCode: " + lblStockCode.Text + " Misc was left blank!!<br>";
                lblError.ForeColor = Color.Red;
            }
            else
            {
                if (SharedFunctions.IsNumeric(txtMisc.Text.Trim()) == false)
                {
                    lblError.Text += "**StockCode: " + lblStockCode.Text + " Misc must be numeric!!<br>";
                    lblError.ForeColor = Color.Red;
                }
            }

        }
        if (lblError.Text.Length > 0)
        {
            return;
        }
        SaveData();
        RunReport();

    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        gvForm.DataSource = null;
        gvForm.DataBind();
        gvReport.DataSource = null;
        gvReport.DataBind();
        lbParentStockCode.SelectedIndex = -1;

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

            dt = RunReport();
 

            dt.TableName = "dt";


            sFilesName = "InvMovementsOther_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
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

        
        if (e.Row.RowType == DataControlRowType.Header)
        {
            
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

             

        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {//Runs once...


        }
    }
    protected void gvForm_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }


    #endregion
}