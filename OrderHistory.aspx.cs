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


public partial class OrderHistory : System.Web.UI.Page
{

    const string sDocPath = @"Images\Documents\";

    private string _salesOrder = "";

    #region Properties
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
    public string SalesOrder
    {
        get
        {
            return _salesOrder;
        }
        set
        {
            _salesOrder = value;
        }
    }
    #endregion



    #region Subs
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
    private void LoadOrderHistory(int iRoleID, int iUserID)
    {
        string sMsg = "";
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
        string sStockCodeFrom = "NULL";
        string sStockCodeTo = "NULL";

        if (txtStockCodeFrom.Text.Trim() != "")
        {
            sStockCodeFrom = "'" + txtStockCodeFrom.Text.Trim() + "'";
        }
        if (txtStockCodeTo.Text.Trim() != "")
        {
            sStockCodeTo = "'" + txtStockCodeTo.Text.Trim() + "'";
        }

        string sCustomer = "NULL";
        if (ddlCustomers.SelectedIndex != 0)//ALL...
        {//Not null then add quotes...
            sCustomer = "'" + ddlCustomers.SelectedValue.Trim() + "'";
        }

        string sSalesOrder = "NULL";
        if (txtSaleOrder.Text.Trim() != "")//ALL...
        {//Not null then add quotes...
            if (!SharedFunctions.IsNumeric(txtSaleOrder.Text.Trim()))
            {
                sMsg += "**Sales Order MUST be a numeric value!<br/>";
            }
            else
            {
                sSalesOrder = "'" + int.Parse(txtSaleOrder.Text.Trim()).ToString() + "'";
            }
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
        else//No Start Date(From Date)...
        {
            switch (ddlPeriod.SelectedIndex)
            {
                case 0://Range
                    sMsg += "**Start Date is not a valid date!<br/>";
                    break;
                case 1://Single
                    sMsg += "**Start Date is not a valid date!<br/>";
                    break;
                case 2://Up To Date
                       //None...
                    break;
                case 3://ALL
                    //None...
                    break;

            }
        }
        //End Date...
        if (sEndDate != "NULL")
        {
            if (SharedFunctions.IsDate(sEndDate.Replace("'", "")) == false)
            {
                sMsg += "**End Date is not a valid date!<br/>";
            }
        }
        else//No End Date(To Date)...
        {
            switch (ddlPeriod.SelectedIndex)
            {
                case 0://Range
                    sMsg += "**End Date is not a valid date!<br/>";
                    break;
                case 1://Single
                       //None...
                    break;
                case 2://Up To Date
                    sMsg += "**End Date is not a valid date!<br/>";
                    break;
                case 3://ALL
                    //None...
                    break;
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


        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }

        switch (ddlPeriod.SelectedIndex)
        { //Runs if you pass Other it runs the Open Orders by Stock code and Customer Report...

            case 0://Date Range...
                sSQL = "EXEC spGetOrderHistoryReportByRange ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
                sSQL += "@SalesOrder=" + sSalesOrder + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@RoleID = " + iRoleID.ToString() + ",";
                sSQL += "@UserID =" + iUserID.ToString();

                break;
            case 1://Single Date...
                sSQL = "EXEC spGetOrderHistoryReportByRange ";
                sSQL += "@FromDate=" + sStartDate + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
                sSQL += "@SalesOrder=" + sSalesOrder + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@RoleID = " + iRoleID.ToString() + ",";
                sSQL += "@UserID =" + iUserID.ToString();

                break;
            case 2://Up To Date...
                sSQL = "EXEC spGetOrderHistoryReportByRange ";
                sSQL += "@ToDate=" + sEndDate + ",";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
                sSQL += "@SalesOrder=" + sSalesOrder + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@RoleID = " + iRoleID.ToString() + ",";
                sSQL += "@UserID =" + iUserID.ToString();

                break;
            case 3://ALL
                sSQL = "EXEC spGetOrderHistoryReportByRange ";
                sSQL += "@StockCodeFrom=" + sStockCodeFrom + ",";
                sSQL += "@StockCodeTo=" + sStockCodeTo + ",";
                sSQL += "@SalesOrder=" + sSalesOrder + ",";
                sSQL += "@Customer=" + sCustomer + ",";
                sSQL += "@RoleID = " + iRoleID.ToString() + ",";
                sSQL += "@UserID =" + iUserID.ToString();

                break;
        }

        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dt");


        DataView dv = new DataView(dt);
        dv.Sort = "OrderDate DESC";//Very Important for Color coding to be accurate...
        dt = dv.ToTable();

        DataColumn column = new DataColumn();
        column.DataType = System.Type.GetType("System.Int32");
        column.ColumnName = "ID";
        dt.Columns.Add(column);
        if (dt.Rows.Count > 0)
        {
            //Set values for existing rows...
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["ID"] = i + 1;
            }
            Session["dtOpenOrders"] = dt;
            gvOrderHistory.DataSource = dt;
            gvOrderHistory.DataBind();
            lblRecordCount.Text = "Record Count:  " + dt.Rows.Count.ToString();
            // pnlGridView.Visible = true;
            //tblHeaderTable.Visible = true;
        }
        else
        {
            lblError.Text = "No results found!!";
            lblError.ForeColor = Color.Red;
            // pnlGridView.Visible = false;
            // tblHeaderTable.Visible = false;

            gvOrderHistory.DataSource = null;
            gvOrderHistory.DataBind();
        }

        //if (dt.Rows.Count > 9)
        //{
        //    pnlGridView.ScrollBars = ScrollBars.Vertical;
        //    pnlGridView.Width = Unit.Pixel(1320);
        //    tblHeaderTable.Width = Unit.Pixel(1320);
        //}
        //else
        //{
        //    pnlGridView.ScrollBars = ScrollBars.None;
        //    pnlGridView.Width = Unit.Pixel(1100);
        //    tblHeaderTable.Width = Unit.Pixel(1324);
        //}



    }
    private void LoadCustomer(DropDownList ddl, string sSortBy)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddl.Items.Clear();

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
                    ddl.Items.Add(new ListItem(a.Name + " - " + a.Customer, a.Customer));
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
    }
    private void BindPDFData(string sSalesOrder)
    {

        string sFolder = rblFolderName.SelectedValue + "\\";

        string sFileName = "";
        string sDateCreated = "";
        string sPath = "";

        DataTable dt = new DataTable();
        DataRow drRow;

        dt.Columns.Add("FullPath", typeof(String));
        dt.Columns.Add("FileName", typeof(String));
        dt.Columns.Add("DateAdded", typeof(DateTime));
        dt.Columns.Add("DateCreated", typeof(String));
        dt.Columns.Add("Extension", typeof(String));
        dt.Columns.Add("Folder", typeof(String));
        dt.Columns.Add("FileSize", typeof(String));
        dt.Columns.Add("DocName", typeof(String));

        try
        {

            sPath = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + sDocPath + sFolder;
            if (!Directory.Exists(sPath))
            {
                gvPDFs.DataSource = null;
                gvPDFs.DataBind();

                btnDeleteAll.Visible = false;
                ModalPopupExtenderPopUp.Show();
                return;
            }
            Debug.WriteLine(sPath);
            DirectoryInfo diLienDocs = new DirectoryInfo(sPath);
            foreach (FileInfo fi in diLienDocs.GetFiles())
            {
                sFileName = fi.Name;
                if (sFileName.Substring(0, sFileName.IndexOf("_")) != sSalesOrder)
                {
                    continue;
                }

                sDateCreated = fi.CreationTime.ToShortDateString();
     
                Int64 iFileSize = 0;
                string sFileSize = "";
                iFileSize = fi.Length;


                if (iFileSize < 1024)
                {//bytes
                    sFileSize = iFileSize.ToString("#,0") + " bytes";
                }
                else if (iFileSize > 1024 && iFileSize < 1000000)
                {//KB
                    sFileSize = (iFileSize / 1024).ToString("#,0") + " kb";
                }
                else
                {//MB
                    sFileSize = (ConvertBytesToMegabytes(iFileSize)).ToString("#,0.0") + " mb";
                }

                drRow = dt.NewRow();
                drRow["FileName"] = sFileName;
                drRow["DateCreated"] = sDateCreated;
                drRow["FullPath"] = sDocPath + sFolder + sFileName;
                drRow["DateAdded"] = fi.LastWriteTime.ToString();
                drRow["Extension"] = fi.Extension;
                drRow["FileSize"] = sFileSize;
                drRow["Folder"] = sFolder.Replace("/", "");
                drRow["DocName"] = sFileName;
                dt.Rows.Add(drRow);
                
            }


            if (dt.Rows.Count > 0)
            {
                DataView dv = new DataView(dt);
                dv.Sort = "DateAdded Asc";
                dt = dv.ToTable();
                DataTable dtNew = dv.ToTable();

                DataColumn column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "ID";
                dtNew.Columns.Add(column);
                //Set values for existing rows...
                for (int i = 0; i < dtNew.Rows.Count; i++)
                {
                    dtNew.Rows[i]["ID"] = i + 1;
                }

                gvPDFs.DataSource = dtNew;
                gvPDFs.DataBind();



                ViewState["gvPDFs"] = dtNew;
                if (dt.Rows.Count > 0)
                {
                    btnDeleteAll.Visible = true;
                }
                else
                {
                    btnDeleteAll.Visible = false;
                }

            }
            else
            {
                gvPDFs.DataSource = null;
                gvPDFs.DataBind();

                btnDeleteAll.Visible = false;
                ModalPopupExtenderPopUp.Show();
                return;
            }

            gvPDFs.Visible = true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            Debug.WriteLine(ex.InnerException.ToString());
        }
        finally
        {
            dt.Dispose();
        }
    }
    private void UploadDocuments(string sSalesOrder, string sFolder)
    {

        string filePath = MapPath("~/images/Documents/" + sFolder + "/");
        lblError.Text = "";
        lblUploadError.Text = "";
        // string sAbsolutePath = "";
        string sNewName = "";

        try
        {
            if (fuDocuments.HasFile)
            {

                string sFileName = "";
                sFileName = fuDocuments.FileName;

                string[] AcceptableFileExtensions = new string[] { "APPLICATION/PDF", "IMAGE/PNG", "IMAGE/JPG", "IMAGE/GIF", "IMAGE/JPEG", "IMAGE/GIF", ".JPG", ".JPEG", ".GIF", ".PNG", ".PDF" };
                if (AcceptableFileExtensions.Contains(Path.GetExtension(sFileName).ToUpper()))
                {
                    if (File.Exists(filePath + sFileName))
                    {
                        File.Delete(filePath + sFileName);
                    }

                    fuDocuments.SaveAs(filePath + sFileName);
                    ClearContents(fuDocuments);


                    lblUploadError.Text = "**File uploaded successfully!";
                    lblUploadError.ForeColor = Color.Green;

                }
                else
                {
                    lblUploadError.Text = "**File Type Not Allowed!";
                    lblUploadError.ForeColor = Color.Red;
                }

            }
            else
            {
                lblUploadError.Text = "**No file selected!";
                lblUploadError.ForeColor = Color.Red;
            }
        }
        catch (FileNotFoundException fex)
        {
            Debug.WriteLine(fex.ToString());
            lblUploadError.Text = "**File not found!";
            lblUploadError.ForeColor = Color.Red;
        }
        catch (Exception ex)
        {
            if (File.Exists(filePath + sNewName))
            {
                File.Delete(filePath + sNewName);
            }
            Debug.WriteLine(ex.ToString());
            // lblUploadErr.Text = ex.Message;
        }
        finally
        {

            BindPDFData(sSalesOrder);
        }
    }
    private void ClearContents(Control control)
    {

        for (var i = 0; i < Session.Keys.Count; i++)
        {

            if (Session.Keys[i].Contains(control.ClientID))
            {

                Session.Remove(Session.Keys[i]);

                break;

            }

        }

    }
    #endregion

    #region Functions
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
    static double ConvertBytesToMegabytes(long bytes)
    {
        return (bytes / 1024f) / 1024f;
    }
    static double ConvertKilobytesToMegabytes(long kilobytes)
    {
        return kilobytes / 1024f;
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

        }
        if (!Page.IsPostBack)
        {
            LoadCustomer(ddlCustomers, "Name");

            Session["dtOpenOrders"] = null;
            ddlPeriod.SelectedIndex = 3;
            ddlPeriod_SelectedIndexChanged(ddlPeriod, null);

            ////txtSaleOrder.Text = "52892";
        }

    }
    protected void btnRunReport_Click(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
        lblError.Text = "";
        lblRecordCount.Text = "";
        LoadOrderHistory(iRoleID, iUserID);


    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        txtStartDate.Text = "";
        txtEndDate.Text = "";
        txtStockCodeFrom.Text = "";
        txtStockCodeTo.Text = "";
        txtSaleOrder.Text = "";
        ddlPeriod.SelectedIndex = 3;
        ddlCustomers.SelectedIndex = 0;

        txtStartDate.Text = "";
        txtEndDate.Text = "";


    }
    protected void rblSort_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomer(ddlCustomers, rblSort.SelectedValue);
    }
    protected void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnRunReport_Click(btnRunReport, null);

    }
    protected void gvOrderHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            try
            {
                Label lblOrderDate = (Label)e.Row.FindControl("lblOrderDate");
                if (lblOrderDate.Text != "")
                {
                    lblOrderDate.Text = Convert.ToDateTime(lblOrderDate.Text).ToShortDateString();
                }
                Label lblSalesOrder = (Label)e.Row.FindControl("lblSalesOrder");
                Label lblNotes = (Label)e.Row.FindControl("lblNotes");
                Label lblSalesOrderNotes = (Label)e.Row.FindControl("lblSalesOrderNotes");
                Panel pnlNotes0 = (Panel)e.Row.FindControl("pnlNotes0");
                if (lblSalesOrderNotes.Text != "")
                {
                    lblSalesOrderNotes.Text = lblSalesOrderNotes.Text.Replace("$%$", "<br />");
                    pnlNotes0.Visible = true;
                    lblNotes.Visible = true;
                    lblNotes.Style.Add("cursor", "pointer");
                }
                else
                {
                    pnlNotes0.Visible = false;
                    lblNotes.Visible = false;
                }

                string sSource = lblSalesOrder.Text;
                var sResult = int.Parse(sSource).ToString();
                lblSalesOrder.Text = sResult;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {

        }
    }
    protected void gvOrderHistory_Sorting(object sender, GridViewSortEventArgs e)
    {

        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtOpenOrders"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvOrderHistory.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvOrderHistory.DataSource = m_DataView;
            gvOrderHistory.DataBind();
            gvOrderHistory.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();

    }
    protected void gvOrderHistory_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        int iUserID = Convert.ToInt32(Session["UserID"]);
        int i = 0;

        Label lblSalesOrder;

        switch (e.CommandName)
        {
            case "ShowInvoice":
                i = Convert.ToInt32(e.CommandArgument);
                lblSalesOrder = (Label)gvOrderHistory.Rows[i].FindControl("lblSalesOrder");
                lblSalesOrderPopup.Text = lblSalesOrder.Text;
               // BindPDFData(lblSalesOrder.Text);
                ModalPopupExtenderPopUp.Show();
                 

                break;

        }


    }
    protected void txtStartDate_TextChanged(object sender, EventArgs e)
    {

        //txtStartDateChart2.Text = txtStartDate.Text;
    }
    protected void txtEndDate_TextChanged(object sender, EventArgs e)
    {

        // txtEndDateChart2.Text = txtEndDate.Text;
    }
    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblRecordCount.Text = "";
        gvOrderHistory.DataSource = null;
        gvOrderHistory.DataBind();

        switch (ddlPeriod.SelectedIndex)
        {

            case 0://Range
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                txtStartDate.Enabled = true;
                txtEndDate.Enabled = true;
                break;
            case 1://Single
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                txtStartDate.Enabled = true;
                txtEndDate.Enabled = false;

                break;
            case 2://Up To Date
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                txtStartDate.Enabled = false;
                txtEndDate.Enabled = true;
                break;
            case 3://ALL
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                txtStartDate.Enabled = false;
                txtEndDate.Enabled = false;
                break;
        }


    }

    protected void lbnExportExcel_Click(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";

        if (Session["dtOpenOrders"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtOpenOrders"];

        dt.TableName = "dtOpenOrders";
        try
        {
            dt.Columns.Remove("ID");
        }
        catch (Exception){}
        
        foreach (DataRow row in dt.Rows)
        {
            string sSource = row["SalesOrder"].ToString();
            var sResult = int.Parse(sSource).ToString();
            row["SalesOrder"] = sResult;
        }
        dt.AcceptChanges();

        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "OpenOrders" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();


    }
    protected void btnClose_Click(object sender, EventArgs e)
    {
        lblUploadError.Text = "";
        rblFolderName.SelectedIndex = -1;
        gvPDFs.Visible = false;
        ModalPopupExtenderPopUp.Hide();
    }
    protected void gvPDFs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int iRoleID = 0;
                iRoleID = Convert.ToInt32(Session["RoleID"]);
                string filePath = MapPath("~/images/Documents/");
                Label lblExtension = (Label)e.Row.FindControl("lblExtension");
                HyperLink hlView = (HyperLink)e.Row.FindControl("hlView");
                LinkButton lbnFileName = (LinkButton)e.Row.FindControl("lbnFileName");
                string sURL = "";
                if (lblExtension != null)
                {
                    if (lblExtension.Text != "")
                    {
                        switch (lblExtension.Text.ToUpper())
                        {
                            case ".JPG":                                 
                                hlView.ToolTip = "Click to view document";
                                hlView.Style.Add("Cursor", "pointer");
                                sURL = "Viewer.aspx?&folder=" + rblFolderName.SelectedValue + "&file=" + lbnFileName.Text;
                                hlView.Attributes.Add("onclick", "javascript: window.open('" + sURL + "', 'window','HEIGHT=800,WIDTH=1024,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
                                break;
                            case ".JPEG":                                
                                hlView.ToolTip = "Click to view document";
                                hlView.Style.Add("Cursor", "pointer");
                                sURL = "Viewer.aspx?&folder=" + rblFolderName.SelectedValue + "&file=" + lbnFileName.Text;
                                hlView.Attributes.Add("onclick", "javascript: window.open('" + sURL + "', 'window','HEIGHT=800,WIDTH=1024,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
                                break;
                            case ".GIF":                                 
                                hlView.ToolTip = "Click to view document";
                                hlView.Style.Add("Cursor", "pointer");
                                sURL = "Viewer.aspx?&folder=" + rblFolderName.SelectedValue + "&file=" + lbnFileName.Text;
                                hlView.Attributes.Add("onclick", "javascript: window.open('" + sURL + "', 'window','HEIGHT=800,WIDTH=1024,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
                                break;
                            case ".PNG":                                
                                hlView.ToolTip = "Click to view document";
                                hlView.Style.Add("Cursor", "pointer");
                                sURL = "Viewer.aspx?&folder=" + rblFolderName.SelectedValue + "&file=" + lbnFileName.Text;
                                hlView.Attributes.Add("onclick", "javascript: window.open('" + sURL + "', 'window','HEIGHT=800,WIDTH=1024,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
                                break;
                            case ".PDF":                                
                                hlView.ToolTip = "Click to view document";
                                hlView.Style.Add("Cursor", "pointer");
                                sURL = "Viewer.aspx?&folder=" + rblFolderName.SelectedValue + "&file=" + lbnFileName.Text;
                                hlView.Attributes.Add("onclick", "javascript: window.open('" + sURL + "', 'window','HEIGHT=800,WIDTH=1024,top=50,left=50,toolbar=no,scrollbars=yes,resizable=yes').focus();return true;");//Return true to fire both client and server events!!!
                                break;

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            Debug.WriteLine(ex.InnerException.ToString());
        }
    }
    protected void gvPDFs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string sSalesOrder = lblSalesOrderPopup.Text;
        BindPDFData(sSalesOrder);
    }
    protected void gvPDFs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblUploadError.Text = "";

        System.IO.FileInfo file = null;
        string Outgoingfile = "";
        string sContentType = "";
        string sFullPath = "";
      
        DataTable dt = new DataTable();
        GridViewRow selectedRow;
        int index = 0;
        string sFolder = rblFolderName.SelectedValue + "\\";
        string sFileName = "";
         
        string filePath = MapPath("~/images/Documents/");
        
        Label lblExtension;
        switch (e.CommandName)
        {
            case "Select":
                index = Convert.ToInt32(e.CommandArgument);

                LinkButton lbnFileName = (LinkButton)gvPDFs.Rows[index].FindControl("lbnFileName");
                lblExtension = (Label)gvPDFs.Rows[index].FindControl("lblExtension");                
                sFullPath = filePath + sFolder + "\\" + lbnFileName.Text;
                file = new System.IO.FileInfo(sFullPath);
                Outgoingfile = lbnFileName.Text;
                switch (lblExtension.Text.ToUpper())
                {
                    case ".TIFF":
                        sContentType = "image/TIFF";
                        break;
                    case ".TIF":
                        sContentType = "image/TIF";
                        break;
                    case ".CONTACT":
                        sContentType = "text/x-ms-contact";
                        break;
                    case ".EML":
                        sContentType = "text/plain";
                        break;
                    case ".RTF":
                        sContentType = "application/msword";
                        break;
                    case ".TXT":
                        sContentType = "text/plain";
                        break;
                    case ".JPG":
                        sContentType = "image/JPEG";
                        break;
                    case ".GIF":
                        sContentType = "image/GIF";
                        break;
                    case ".PNG":
                        sContentType = "image/PNG";
                        break;
                    case ".PDF":
                        sContentType = "application/pdf";
                        break;
                    case ".XLS":
                        sContentType = "application/vnd.ms-excel";
                        break;
                    case ".XLSX":
                        sContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case ".DOC":
                        sContentType = "application/ms-word";
                        break;
                    case ".DOCX":
                        sContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case ".ZIP":
                        sContentType = "application/zip";
                        break;
                    case ".MDB":
                        sContentType = "application/msaccess";
                        break;
                    case ".ACCDB":
                        sContentType = "application/msaccess";
                        break;
                    default:
                        sContentType = "application/octet-stream";
                        break;
                }
                try
                {
                    if (file.Exists)
                    {
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.Name + "\"");
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.ContentType = sContentType;
                        Response.Flush();
                        Response.TransmitFile(file.FullName);

                        break;
                    }
                    else
                    {
                        Response.Write("This file does not exist.");
                    }
                }
                catch (AccessViolationException iex)
                {
                    Debug.WriteLine(iex.ToString());
                    lblUploadError.Text = iex.Message;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    Response.End();//Remember to Add the Grid Postback triggers to avoid this not working...
                }

                ModalPopupExtenderPopUp.Show();
                break;
            case "Delete":
                index = Convert.ToInt32(e.CommandArgument);
                selectedRow = gvPDFs.Rows[index];
                sFileName = ((LinkButton)selectedRow.FindControl("lbnFileName")).Text;              
                sFullPath = filePath + sFolder + "\\" + sFileName;

                try
                {
                    if (File.Exists(sFullPath))
                    {
                        File.Delete(sFullPath);
                    }                     

                    lblUploadError.Text = "**Delete Complete!";
                    lblUploadError.ForeColor = System.Drawing.Color.Green;
                }
                catch (AccessViolationException iex)
                {
                    Debug.WriteLine(iex.ToString());
                    lblUploadError.Text = iex.Message;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    lblUploadError.Text = ex.Message;
                }

                ViewState["dtPDFs"] = dt;//DataTable with row removed...
                ModalPopupExtenderPopUp.Show();
                break;


        }
        dt.Dispose();
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblUploadError.Text = "";

        if (rblFolderName.SelectedIndex != -1)
        {
            string sSalesOrder = lblSalesOrderPopup.Text;
            UploadDocuments(sSalesOrder, rblFolderName.SelectedValue);
        }
        else
        {
            lblUploadError.Text = "**No Folder Selected!";
            lblUploadError.ForeColor = Color.Red;
        }
        ModalPopupExtenderPopUp.Show();
    }
    protected void btnDeleteAll_Click(object sender, EventArgs e)
    {

        lblUploadError.Text = "";

        string filePath = MapPath("~/images/Documents/");
        System.IO.FileInfo file = null;
        string sFullPath = "";


        for (int idx = 0; idx < gvPDFs.Rows.Count; idx++)
        {
            CheckBox chk = (CheckBox)((gvPDFs.Rows[idx].FindControl("chk"))); //first row check box...
            if (chk.Checked == true)
            {

                LinkButton lbnFileName = (LinkButton)gvPDFs.Rows[idx].FindControl("lbnFileName");
                Label lblExtension = (Label)gvPDFs.Rows[idx].FindControl("lblExtension");
                string sFolder = rblFolderName.SelectedValue + "\\";

                sFullPath = filePath + sFolder + lbnFileName.Text;

                file = new System.IO.FileInfo(sFullPath);

                try
                {
                    if (file.Exists)
                    {
                        File.Delete(sFullPath);
                    }
                    else
                    {
                        Response.Write("This file does not exist.");
                    }
                }
                catch (AccessViolationException iex)
                {
                    Debug.WriteLine(iex.ToString());
                    lblUploadError.Text = iex.Message;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

        }//End Loop...
        if (rblFolderName.SelectedIndex != -1)
        {
            string sSalesOrder = lblSalesOrderPopup.Text;
            BindPDFData(sSalesOrder);
            ModalPopupExtenderPopUp.Show();
        }
        ModalPopupExtenderPopUp.Show();
    }
    protected void chkAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chkAll = (CheckBox)((gvPDFs.HeaderRow.FindControl("chkAll"))); //first row check box...
        CheckBox ChkBoxes;

        if (chkAll.Checked == true)
        {
            for (int idx = 0; idx < gvPDFs.Rows.Count; idx++)
            {
                ChkBoxes = (CheckBox)((gvPDFs.Rows[idx].FindControl("chk")));
                ChkBoxes.Checked = true;
            }
        }
        else
        {
            for (int idx = 0; idx < gvPDFs.Rows.Count; idx++)
            {
                ChkBoxes = (CheckBox)((gvPDFs.Rows[idx].FindControl("chk")));
                ChkBoxes.Checked = false;
            }
        }
        ModalPopupExtenderPopUp.Show();
    }
    protected void rblFolderName_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblUploadError.Text = "";
        if (rblFolderName.SelectedIndex != -1)
        {
            string sSalesOrder = lblSalesOrderPopup.Text;
            BindPDFData(sSalesOrder);
            rblFolderName.Items[rblFolderName.SelectedIndex].Attributes.Add("style", "border:2px solid red;padding:12px;margin:12px;background-color:#ff4d4d;color:white");
            ModalPopupExtenderPopUp.Show();
        }
    }

    #endregion




}