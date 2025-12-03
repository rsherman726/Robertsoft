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


public partial class TrendsReportByQtyPopupSummary : System.Web.UI.Page
{
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

    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string sStartDate = "";
            string sEndDate = "";

            if (Session["dtTrendsReportQuantitySummary"] != null)
            {
                DataTable dt = (DataTable)Session["dtTrendsReportQuantitySummary"];
                gvPurchasingTrends.DataSource = dt;
                gvPurchasingTrends.DataBind();

                sStartDate = Convert.ToDateTime(dt.Rows[0]["DateFrom"]).ToShortDateString();
                sEndDate = Convert.ToDateTime(dt.Rows[0]["DateTo"]).ToShortDateString();

                lblDateRange.Text = "From " + sStartDate + " to " + sEndDate;

                dt.Dispose();
            }
            else
            {
                gvPurchasingTrends.DataSource = null;
                gvPurchasingTrends.DataBind();
            }

        }

    }
    protected void gvPurchasingTrends_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        DataTable dt = (DataTable)Session["dtTrendsReportQuantitySummary"];
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[0].CssClass = "CenterAligner";
            e.Row.Cells[1].CssClass = "CenterAligner";
            e.Row.Cells[2].CssClass = "CenterAligner";
            e.Row.Cells[3].CssClass = "CenterAligner";
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            DataColumnCollection columns = dt.Columns;
            string sColumnName = "EndCustomer";
            if (columns.Contains(sColumnName))
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;


                //overwrite cell text with new label...
                Label lblName = new Label();
                lblName.Text = e.Row.Cells[0].Text;
                e.Row.Cells[0].Text = "";//Clear value...
                e.Row.Cells[0].Controls.Add(lblName);
                lblName.Width = Unit.Pixel(210);



                //overwrite cell text with new label...
                Label lblDateFrom = new Label();
                lblDateFrom.Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToShortDateString();
                e.Row.Cells[3].Text = "";//Clear value...
                e.Row.Cells[3].Controls.Add(lblDateFrom);

                //overwrite cell text with new label...
                Label lblDateTo = new Label();
                lblDateTo.Text = Convert.ToDateTime(e.Row.Cells[4].Text).ToShortDateString();
                e.Row.Cells[4].Text = "";//Clear value...
                e.Row.Cells[4].Controls.Add(lblDateTo);


                for (int i = 5; i < dt.Columns.Count; i++)
                {
                    e.Row.Cells[i].BorderColor = Color.Gray;
                    e.Row.Cells[i].BorderStyle = BorderStyle.Solid;
                    e.Row.Cells[i].BorderWidth = Unit.Pixel(1);
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                    if (e.Row.Cells[i].Text == "&nbsp;")
                    {
                        if (chkShowColor.Checked)
                        {
                            e.Row.Cells[i].BackColor = Color.Red;
                        }

                    }
                    else
                    {
                        if (e.Row.Cells[i].Text == "0.0")
                        {
                            if (chkShowColor.Checked)
                            {
                                e.Row.Cells[i].BackColor = Color.Red;
                            }
                            e.Row.Cells[i].Text = "";
                        }
                        else//Is numeric...
                        {
                            if (chkShowColor.Checked)
                            {
                                e.Row.Cells[i].BackColor = Color.LightGreen;
                            }

                            e.Row.Cells[i].Text = Convert.ToDecimal(e.Row.Cells[i].Text).ToString("#,0.0");

                        }
                    }
                }
            }
            else//No End Customer Column...
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;

                //overwrite cell text with new label...
                Label lblName = new Label();
                lblName.Text = e.Row.Cells[0].Text;
                e.Row.Cells[0].Text = "";//Clear value...
                e.Row.Cells[0].Controls.Add(lblName);
                lblName.Width = Unit.Pixel(210);

                Label lblCustomer = new Label();
                lblCustomer.Text = e.Row.Cells[1].Text;
                e.Row.Cells[1].Text = "";//Clear value...
                e.Row.Cells[1].Controls.Add(lblCustomer);
                lblCustomer.Width = Unit.Pixel(75);

                List<string> lEndCustomers = new List<string>();
                lEndCustomers = SharedFunctions.GetEndCustomer();
                List<string> lGroupMembers = new List<string>();
                lGroupMembers = SharedFunctions.GetGrouping();

                if (lEndCustomers.Contains(lblCustomer.Text))
                {
                    lblName.ForeColor = Color.Red;
                    lblName.Font.Bold = true;
                }
                if (lGroupMembers.Contains(lblCustomer.Text))
                {
                    lblName.ForeColor = Color.Blue;
                    lblName.Font.Bold = true;
                }



                //overwrite cell text with new label...
                Label lblDateFrom = new Label();
                lblDateFrom.Text = Convert.ToDateTime(e.Row.Cells[2].Text).ToShortDateString();
                e.Row.Cells[2].Text = "";//Clear value...
                e.Row.Cells[2].Controls.Add(lblDateFrom);

                //overwrite cell text with new label...
                Label lblDateTo = new Label();
                lblDateTo.Text = Convert.ToDateTime(e.Row.Cells[3].Text).ToShortDateString();
                e.Row.Cells[3].Text = "";//Clear value...
                e.Row.Cells[3].Controls.Add(lblDateTo);



                for (int i = 4; i < dt.Columns.Count; i++)
                {
                    e.Row.Cells[i].BorderColor = Color.Gray;
                    e.Row.Cells[i].BorderStyle = BorderStyle.Solid;
                    e.Row.Cells[i].BorderWidth = Unit.Pixel(1);
                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                    if (e.Row.Cells[i].Text == "&nbsp;")
                    {
                        if (chkShowColor.Checked)
                        {
                            e.Row.Cells[i].BackColor = Color.Red;
                        }

                    }
                    else
                    {
                        if (e.Row.Cells[i].Text == "0.0")
                        {
                            if (chkShowColor.Checked)
                            {
                                e.Row.Cells[i].BackColor = Color.Red;
                            }
                            e.Row.Cells[i].Text = "";
                        }
                        else//Is numeric...
                        {
                            if (chkShowColor.Checked)
                            {
                                e.Row.Cells[i].BackColor = Color.LightGreen;
                            }

                            e.Row.Cells[i].Text = Convert.ToDecimal(e.Row.Cells[i].Text).ToString("#,0.0");

                        }
                    }
                }
            }



        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {//Runs once...

            if (Request.QueryString["endcust"] != null)
            {
                /********** Setting Totals for Dynamically created Gridview columns********************/
                double sum = 0.00;
                double value = 0.00;
                string sValue = "";
                for (int i = 3; i < dt.Columns.Count; i++)
                {
                    sum = 0.00;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        try
                        {
                            sValue = gvPurchasingTrends.Rows[j].Cells[i].Text.Trim().Replace("$", "");

                            if (sValue == "")
                            {
                                value = 0;
                            }
                            else if (sValue == "&nbsp;")
                            {
                                value = 0;
                            }
                            else
                            {
                                value = double.Parse(gvPurchasingTrends.Rows[j].Cells[i].Text.Trim().Replace("$", ""));
                            }
                            sum += value;
                        }
                        catch (Exception)
                        {
                            Debug.WriteLine(gvPurchasingTrends.Rows[j].Cells[i].Text.Trim());
                        }
                    }

                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;

                    Label lblName = new Label();
                    lblName.Text = "Totals:";
                    e.Row.Cells[0].Text = "";//Clear value...
                    e.Row.Cells[0].Controls.Add(lblName);
                    lblName.Width = Unit.Pixel(210);

                    Label lbl = new Label();
                    lbl.Text = sum.ToString("#,0.0");
                    lbl.Font.Bold = true;
                    e.Row.Cells[i].Text = "";//Clear value...
                    e.Row.Cells[i].Controls.Add(lbl);
                }
            }
            else//End Customer...
            {
                /********** Setting Totals for Dynamically created Gridview columns********************/
                double sum = 0.00;
                double value = 0.00;
                string sValue = "";
                for (int i = 5; i < dt.Columns.Count; i++)
                {
                    sum = 0.00;
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        try
                        {
                            sValue = gvPurchasingTrends.Rows[j].Cells[i].Text.Trim().Replace("$", "");

                            if (sValue == "")
                            {
                                value = 0;
                            }
                            else if (sValue == "&nbsp;")
                            {
                                value = 0;
                            }
                            else
                            {
                                value = double.Parse(gvPurchasingTrends.Rows[j].Cells[i].Text.Trim().Replace("$", ""));
                            }
                            sum += value;
                        }
                        catch (Exception)
                        {
                            Debug.WriteLine(gvPurchasingTrends.Rows[j].Cells[i].Text.Trim());
                        }
                    }

                    e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Right;

                    Label lblName = new Label();
                    lblName.Text = "Totals:";
                    e.Row.Cells[0].Text = "";//Clear value...
                    e.Row.Cells[0].Controls.Add(lblName);
                    lblName.Width = Unit.Pixel(210);

                    Label lbl = new Label();
                    lbl.Text = sum.ToString("#,0.0");
                    lbl.Font.Bold = true;
                    e.Row.Cells[i].Text = "";//Clear value...
                    e.Row.Cells[i].Controls.Add(lbl);
                }
            }

        }

    }
    protected void gvPurchasingTrends_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();

        DataTable dt = (DataTable)Session["dtTrendsReportQuantitySummary"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvPurchasingTrends.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvPurchasingTrends.DataSource = m_DataView;
            gvPurchasingTrends.DataBind();
            gvPurchasingTrends.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
    }
    protected void chkShowColor_CheckedChanged(object sender, EventArgs e)
    {

        string sStartDate = "";
        string sEndDate = "";

        if (Session["dtTrendsReportQuantitySummary"] != null)
        {
            DataTable dt = (DataTable)Session["dtTrendsReportQuantitySummary"];
            gvPurchasingTrends.DataSource = dt;
            gvPurchasingTrends.DataBind();

            sStartDate = Convert.ToDateTime(dt.Rows[0]["DateFrom"]).ToShortDateString();
            sEndDate = Convert.ToDateTime(dt.Rows[0]["DateTo"]).ToShortDateString();

            lblDateRange.Text = "From " + sStartDate + " to " + sEndDate;

            dt.Dispose();
        }
        else
        {
            gvPurchasingTrends.DataSource = null;
            gvPurchasingTrends.DataBind();
        }


    }
    protected void imgExportExcel1_Click(object sender, ImageClickEventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        DataTable dtNew = new DataTable();
        string sFilesName = "";
        if (Session["dtTrendsReportQuantitySummary"] == null)
        {
            lblError.Text = "**No Report in memory to export!";
            return;
        }
        dt = (DataTable)Session["dtTrendsReportQuantitySummary"];
        dt.TableName = "dtCustomerReportQuantitySummary";
        dtNew = dt.Copy();
        try
        {
            dtNew.Columns.Remove("DateFrom");
            dtNew.Columns.Remove("DateTo");
        }
        catch (Exception)
        { }
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dtNew);
        }

        sFilesName = "PurchasingTrendsReportByQtySummary" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }

    #endregion
}