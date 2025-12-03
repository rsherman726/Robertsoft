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
using System.Data.Linq.SqlClient;
// Add references to Soap and Binary formatters. 
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public partial class BE_Report : System.Web.UI.Page
{
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

        string sDir = "images\\Excel\\";
        string sFullPathWithoutFile = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + sDir;
        string sFullPath = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + sDir + sFileName + ".xlsx";
        System.IO.DirectoryInfo root = new System.IO.DirectoryInfo(sFullPathWithoutFile);
        System.IO.FileInfo[] files = null;        
        files = root.GetFiles("*.*");
        foreach (System.IO.FileInfo fi in files)
        {
            if (fi.FullName.Contains("BE_Report"))
            {
                if (File.Exists(fi.FullName))
                {
                    File.Delete(fi.FullName);
                }
            }
        }


        ExcelHelper.ToExcel(ds, sFileName, Page.Response, sFullPath);
        if (File.Exists(sFullPath))
        {
            hlExcel.NavigateUrl = sDir + sFileName + ".xlsx";
            hlExcel.Text = "<i class='fas fa-download'></i>&nbsp;" + sFileName;
            //hlExcel.Target = "_Blank";
        }
    }

    #endregion
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



            string sDir = "images\\Excel\\";
            string sFullPathWithoutFile = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + sDir;             
            System.IO.DirectoryInfo root = new System.IO.DirectoryInfo(sFullPathWithoutFile);
            System.IO.FileInfo[] files = null;
            files = root.GetFiles("*.*");
            foreach (System.IO.FileInfo fi in files)
            {
                if (fi.FullName.Contains("BE_Report"))
                {//Get the last File created..
                    if (File.Exists(fi.FullName))
                    {
                        hlExcel.NavigateUrl = sDir + fi.Name;
                        hlExcel.Text = "<i class='fas fa-download'></i>&nbsp;" + fi.Name;
                        //hlExcel.Target = "_Blank";
                    }
                }
            }           

        }
    }

    protected void imgExportExcel1_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        DataSet ds = new DataSet();
        DataSet dsNew = new DataSet();
        DataTable dtNew = new DataTable();
        string sFilesName = "";

        sSQL = "EXEC spGetBE_Report";

        Debug.WriteLine(sSQL);


        ds = SharedFunctions.getDataSet(sSQL, conn, "ds");
        //Handles Huge datasets...
        ds.RemotingFormat = SerializationFormat.Binary;
        // Save using the Runtime Object Serialization
        try
        {
            FileStream fs1 = new FileStream(@"c:\inetpub\wwwroot\Felbro_B_V7\images\data_ser.dat", FileMode.Create, FileAccess.ReadWrite);
            BinaryFormatter bin = new BinaryFormatter();
            bin.Serialize(fs1, ds);
            fs1.Close();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            lblError.Text = "**FILE ACCESS ERROR!";
            lblError.ForeColor = Color.Red;
            return;
        }


        string sTabName = "";
        int iTableIndex = 0;
        foreach (DataTable dt in ds.Tables)
        {

            switch (iTableIndex)
            {
                case 0:
                    sTabName = "FinishedProducts";
                    break;
                case 1:
                    sTabName = "FinishedProductsPackaging";
                    break;
                case 2:
                    sTabName = "Recipes";
                    break;
                case 3:
                    sTabName = "FinishedProductsWithRecipes";
                    break;
                case 4:
                    sTabName = "PackagingWithRecipes";
                    break;
            }

            dt.TableName = sTabName;

            Debug.WriteLine(sTabName);

            dtNew = dt.Copy();

            dsNew.Tables.Add(dtNew.Copy());
            iTableIndex++;

        }
        HttpContext.Current.Session["ds"] = ds;
        dtNew.Dispose();
        ds.Dispose();
        dsNew.Dispose();
        sFilesName = "BE_Report" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        // Response.Redirect("DownloadFile.ashx?fn=" + sFilesName);


        ExportToExcel(dsNew, sFilesName);
       


    }
}