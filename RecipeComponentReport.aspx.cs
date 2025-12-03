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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


public partial class RecipeComponentReport : System.Web.UI.Page
{
    #region Variables
    private string _stockCode = "";
    private string _description = "";
    string scboStockCodeText = "";
    string scboStockCodeValue = "";

    #endregion

    #region Properties

    public string StockCode
    {
        get
        {
            return _stockCode;
        }
        set
        {
            _stockCode = value;
        }
    }
    public string Description
    {
        get
        {
            return _description;
        }
        set
        {
            _description = value;
        }
    }


    #endregion


    #region Subs

    private void LoadStockCodes()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            lbParentStockCode.Items.Clear();

            var query = (from w in db.InvMaster
                         group w by new
                         {
                             w.StockCode,
                             w.Description
                         } into g
                         orderby
                           g.Key.StockCode
                         select new
                         {
                             g.Key.StockCode,
                             g.Key.Description
                         });


            int iCount = query.Count();
            foreach (var a in query)
            {
                lbParentStockCode.Items.Add(new ListItem(a.StockCode + " - " + a.Description, a.StockCode));
            }
        }
    }
    protected void ExportPDF(string sFormat)
    {
        if (Session["dt"] == null)
        {
            lblError.Text = "**No Respondent Selected!";
            return;
        }
        DataTable dtDoc = (DataTable)Session["dt"];

        ReportDocument crystalReport = GetReportNew(dtDoc);
        ExportFormatType formatType = ExportFormatType.NoFormat;
        switch (sFormat)
        {
            case "Word":
                formatType = ExportFormatType.WordForWindows;
                break;
            case "PDF":
                formatType = ExportFormatType.PortableDocFormat;
                break;
            case "Excel":
                formatType = ExportFormatType.Excel;
                break;
            case "CSV":
                formatType = ExportFormatType.CharacterSeparatedValues;
                break;
        }
        string sFileName = "";
        sFileName = "RecipeComponentReport_" + DateTime.Now.ToString();
        crystalReport.ExportToHttpResponse(formatType, Response, true, sFileName);
        Response.End();
    }
    private void RunReport()
    {
        DataTable dt = new DataTable();
        if (lbParentStockCode.SelectedIndex == -1)
        {
            lblError.Text = "No Stock Code Selected!";
            lblError.ForeColor = Color.Red;
            return;
        }

        StockCode = lbParentStockCode.SelectedValue;
        Description = lbParentStockCode.SelectedItem.Text;

        string sSQL = "EXEC spGetRecipeComponentReport @StockCode=" + _stockCode;
        SqlConnection conn = null;

        conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dtRecipeComponentReport");
        if (dt.Rows.Count > 0)
        {

            Session["dt"] = dt;//First before SetPageIndex to clear values, since setPageIndex fires PageIndexChanging...

            imgbtnPDF.Visible = true;
            imgbtnWord.Visible = true;
            imgbtnExcel.Visible = true;
            lblOptions.Visible = true;
            GetReportNew(dt);
            dt.Dispose();
        }
        else
        {
            imgbtnPDF.Visible = false;
            imgbtnWord.Visible = false;
            imgbtnExcel.Visible = false;
            lblOptions.Visible = false;
        }
    }
    #endregion

    #region Functions
    private ReportDocument GetReportNew(DataTable dt)
    {
        try
        {
            try
            {
                ReportDocument report = new ReportDocument();

                if (report != null)
                {
                    report.Close();
                }


                report.Load(Server.MapPath(@"CrystalReports/rptRecipeComponentReport.rpt"));
                report.FileName = Server.MapPath(@"CrystalReports/rptRecipeComponentReport.rpt");
               

                CrystalDecisions.Shared.ConnectionInfo connectionInfo = new CrystalDecisions.Shared.ConnectionInfo();

                if (Server.MachineName.ToUpper() == "BMW" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
                {
                    connectionInfo.ServerName = "RSPSERVER2";
                    connectionInfo.DatabaseName = "SysproErp1";
                    connectionInfo.UserID = "rsherman";
                    connectionInfo.Password = "toyota1961";
                }
                else
                {
                    ////connectionInfo.ServerName = "SERVERNAME\\SERVER_INSTANCE";
                    ////connectionInfo.DatabaseName = "EnergyPro5";
                    ////connectionInfo.UserID = "EPADMIN";
                    ////connectionInfo.Password = "admin";
                }


                if (dt.Rows.Count > 0)
                {

                    report.SetDataSource(dt);


                    CrystalDecisions.CrystalReports.Engine.TextObject oStockCode;
                    oStockCode = (CrystalDecisions.CrystalReports.Engine.TextObject)report.ReportDefinition.ReportObjects["txtStockCode"];
                    oStockCode.Text = StockCode;


                    CrystalDecisions.CrystalReports.Engine.TextObject oDescription;
                    oDescription = (CrystalDecisions.CrystalReports.Engine.TextObject)report.ReportDefinition.ReportObjects["txtDescription"];
                    oDescription.Text = Description;


                    CrystalDecisions.CrystalReports.Engine.Database database = report.Database;
                    Tables tables = database.Tables;

                    foreach (CrystalDecisions.CrystalReports.Engine.Table table in tables)
                    {
                        CrystalDecisions.Shared.TableLogOnInfo tableLogOnInfo = table.LogOnInfo;
                        tableLogOnInfo.ConnectionInfo = connectionInfo;
                        table.ApplyLogOnInfo(tableLogOnInfo);
                    }






                    CrystalReportViewer1.ReportSource = report;
                    CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
                    CrystalReportViewer1.HasToggleGroupTreeButton = false;
                    CrystalReportViewer1.HasToggleParameterPanelButton = false;
                    CrystalReportViewer1.HasPageNavigationButtons = true;
                    CrystalReportViewer1.HasGotoPageButton = true;
                    CrystalReportViewer1.HasCrystalLogo = true;
                    CrystalReportViewer1.DisplayStatusbar = false;
                    CrystalReportViewer1.HasDrillUpButton = false;
                    CrystalReportViewer1.HasDrilldownTabs = false;
                    CrystalReportViewer1.HasSearchButton = false;
                    CrystalReportViewer1.HasZoomFactorList = false;
                    CrystalReportViewer1.HasExportButton = false;
                    CrystalReportViewer1.HasPrintButton = false;
                    CrystalReportViewer1.SeparatePages = false;
                    CrystalReportViewer1.CssClass = "CRViewer";
                    CrystalReportViewer1.Visible = true;
                    CrystalReportViewer1.Height = Unit.Pixel(500);
                    CrystalReportViewer1.DataBind();
                    return report;
                }
                else
                {
                    ////CrystalReportViewer1.Visible = false;
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
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return null;
        }
        return null;
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
            lblError.Text = "";            
            imgbtnPDF.Visible = false;
            imgbtnWord.Visible = false;
            imgbtnExcel.Visible = false;
            lblOptions.Visible = false;
            LoadStockCodes();
        }
    }

    protected void btnPreview_Click(object sender, EventArgs e)
    {
        RunReport();
    }
    protected void lbParentStockCode_SelectedIndexChanged(object sender, EventArgs e)
    {
        RunReport();
    }
    protected void imgbtnExcel_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";
        ExportPDF("Excel");
    }
    protected void imgbtnPDF_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";
        ExportPDF("PDF");
    }
    protected void imgbtnWord_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";
        ExportPDF("Word");
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
    #endregion



}