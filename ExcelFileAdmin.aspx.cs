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

public partial class ExcelFileAdmin : System.Web.UI.Page
{


    private void UploadExcelFile(string saveDirectory, FileUpload fileUploadControl)
    {
        string filePath = Path.Combine(saveDirectory, fileUploadControl.FileName);
        string sMsg = "";
        string[] extArray = new string[] { ".xls", ".xlsx" };
        bool bTest = false;
        string sFileExtension1 = "";
        if (FileUploadExcel.HasFile)
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


            fileUploadControl.SaveAs(filePath);
            Session["FilePath"] = filePath;//Put file path in session variable for SQL Update...
            lblErrorUpload.Text = "**Excel file upload complete!";
            lblErrorUpload.ForeColor = Color.Green;
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
    private void BindPDFData()
    {
        string sFileName = "";
        string sDateCreated = "";
        string sPath = "";
        string sFileSize = "";
        DataTable dt = new DataTable();
        DataRow drRow;
        dt.Columns.Add("FullPath", typeof(String));
        dt.Columns.Add("FileName", typeof(String));
        dt.Columns.Add("DateCreated", typeof(String));
        dt.Columns.Add("FileSize", typeof(String));

        try
        {
            //foreach (string x in Request.ServerVariables)
            //{
            //    Debug.WriteLine(x.ToString());
            //    Debug.WriteLine(Request.ServerVariables[x].ToString());
            //}
                string sDocPath = @"Images\Excel\";
                sPath = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + sDocPath;    
                Debug.WriteLine(sPath);
                DirectoryInfo diDocs = new DirectoryInfo(sPath);
                foreach (FileInfo fi in diDocs.GetFiles())
                {
                    sFileName = fi.Name;

                    sFileSize = fi.Length < (1024 * 1000) ? fi.Length.ToString("#,0")+ " KB" : fi.Length >= (1024 * 1000) ? fi.Length.ToString("#,0") + " MB" : "";

                    sDateCreated = fi.CreationTime.ToShortDateString();
                    //Add to DataTable...
                    if (fi.Extension.ToUpper() == ".XLS" || fi.Extension.ToUpper() == ".XLSX")
                    {                      
                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FileSize"] = sFileSize;
                        drRow["FullPath"] = sDocPath + sFileName;

                        dt.Rows.Add(drRow);
                    }
                }

            

            if (dt.Rows.Count > 0)
            {
                gvPDFs.DataSource = dt;
                gvPDFs.DataBind();

                ViewState["gvPDFs"] = dt;
            }
            else
            {
                lblMessage.Text = "**You do not have any Excel files available to administer or view!";
            }

        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            dt.Dispose();
        }
    }

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
            BindPDFData();
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        lblErrorUpload.Text = "";
        // string sDirectory = "";
        string filePath = MapPath("~/images/Excel/");
        UploadExcelFile(filePath, FileUploadExcel);
        BindPDFData();

    }
    protected void gvPDFs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //Rebind data...
        DataTable dt = new DataTable();
        if (ViewState["gvPDFs"] != null)
        {
            dt = (DataTable)ViewState["gvPDFs"];
            gvPDFs.DataSource = dt;
            gvPDFs.DataBind();
        }

        dt.Dispose();
    }
    protected void gvPDFs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dt = new DataTable();
        GridViewRow selectedRow;
        int index = 0;
        string sMsg = "";

        switch (e.CommandName)
        {
            case "Delete":
                index = Convert.ToInt32(e.CommandArgument);
                selectedRow = gvPDFs.Rows[index];
                string sFileName = ((HyperLink)selectedRow.FindControl("lnkFileName")).Text;
                string sVirtualPath = ((Label)selectedRow.FindControl("lblFullPath")).Text;
                string sFullPath = "";
                string sDir = "images\\Excel\\";

               
              sFullPath = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + sDir + sFileName;
           

                if (File.Exists(sFullPath))
                {
                    File.Delete(sFullPath);
                }
                if (!File.Exists(sFullPath))
                {
                    //Remove from dataTable...
                    dt.Rows.RemoveAt(index);
                }

                ViewState["dtPDFs"] = dt;//DataTable with row removed...


                break;

        }
        dt.Dispose();
    }
    protected void gvPDFs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton DeleteButton = ((ImageButton)e.Row.FindControl("imgDelete"));
            string javascript = "javascript:return  confirm('Are you sure you want to Delete record ?')";
            DeleteButton.Attributes["onclick"] = javascript;


            string sFileName = "";
            HyperLink lnkFileName = (HyperLink)e.Row.FindControl("lnkFileName");
            Label lblFullPath = (Label)e.Row.FindControl("lblFullPath");
            sFileName = lnkFileName.Text;
            lnkFileName.NavigateUrl = lblFullPath.Text;

        }
    }
    protected void gvPDFs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPDFs.PageIndex = e.NewPageIndex;
        //Rebind data...
        DataTable dt = new DataTable();
        if (ViewState["gvPDFs"] != null)
        {
            dt = (DataTable)ViewState["gvPDFs"];
            gvPDFs.DataSource = dt;
            gvPDFs.DataBind();
        }


        dt.Dispose();
    }
}