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

public partial class ProductionInput : System.Web.UI.Page
{
    decimal dcTotalHours = 0;
    decimal dcTotalHoursOfficial = 0;
    decimal dcTotalHoursShort = 0;

    #region Subs

    private void BindEmployees(Int64 iJob)
    {

        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from wea in db.WipEmployeeAssigns
                         join wpl in db.WipProductionLineAssigns on wea.ProLineID equals wpl.ProLineID
                         where
                           Convert.ToInt64(wpl.Job) == iJob
                         orderby
                           wea.WipEmployees.FirstName,
                           wea.WipEmployees.LastName
                         select new
                         {
                             wea.WipEmployees.EmployeeID,
                             Employee = (wea.WipEmployees.FirstName + " " + (wea.WipEmployees.MiddleName ?? "") + " " + wea.WipEmployees.LastName).Replace("  ", " "),
                             Hours =
                                ((from wji in db.WipEmployeeJobHours
                                  where
                                    wji.Job == wpl.Job &&
                                    wji.EmployeeID == wea.EmployeeID
                                  select new
                                  {
                                      wji.Hours
                                  }).First().Hours)
                         });
            dt = SharedFunctions.ToDataTable(db, query);

            if (dt.Rows.Count > 0)
            {
                dcTotalHours = 0;
                gvRecord.DataSource = dt;
                gvRecord.DataBind();
                Session["dtRecord"] = dt;
                btnSubmit.Visible = true;
            }
            else//No records then create a dummy record to make Gridview still show up...
            {
                //Add a blank row to the dataset
                dt.Rows.Add(dt.NewRow());
                //Bind the DataSet to the GridView
                gvRecord.DataSource = dt;
                gvRecord.DataBind();
                //Get the number of columns to know what the Column Span should be
                int columnCount = gvRecord.Rows[0].Cells.Count;
                //Call the clear method to clear out any controls that you use in the columns.  I use a dropdown list in one of the column so this was necessary.
                gvRecord.Rows[0].Cells.Clear();
                gvRecord.Rows[0].Cells.Add(new TableCell());
                gvRecord.Rows[0].Cells[0].ColumnSpan = columnCount;
                gvRecord.Rows[0].Cells[0].Text = "No production line with Employees assigned to this job!";
                btnSubmit.Visible = false;

            }
        }
    }


    #endregion

    #region Functions
    private int RecordHours(string sJob)
    {//loop through the gridview and record hours..
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            Label lblEmployeeID;
            Label lblEmployee;
            TextBox txtHours;
            decimal dcHours = 0;
            int iEmployeeID = 0;
            string sEmployee = "";
            int iLastInStringIdx = 0;

            //First check to see if all hours are recorded...
            for (int i = 0; i < gvRecord.Rows.Count; i++)
            {
                lblEmployeeID = (Label)gvRecord.Rows[i].FindControl("lblEmployeeID");
                lblEmployee = (Label)gvRecord.Rows[i].FindControl("lblEmployee");
                txtHours = (TextBox)gvRecord.Rows[i].FindControl("txtHours");
                if (txtHours.Text.Trim() == "")
                {
                    txtHours.Text = "0";
                }
                dcHours = Convert.ToDecimal(txtHours.Text);
                dcTotalHours += dcHours;

            }
            if (sMsg.Trim().EndsWith(","))
            {
                iLastInStringIdx = sMsg.LastIndexOf(",");
                sMsg = sMsg.Remove(iLastInStringIdx).Trim();
            }
            if (sMsg.Length > 0)
            {
                lblError.Text = "**You are missing hours for: <br/><font color='Blue'>" + sMsg + "</font> Please go back and fill them in!";
                lblError.ForeColor = Color.Red;
                return 0;
            }
            dcTotalHoursOfficial = SharedFunctions.GetJobHours(sJob);//Get Official Hours...
            if (dcTotalHoursOfficial == 0)
            {
                lblError.Text = "**No Official Hours to record!";
                lblError.ForeColor = Color.Red;
                return 0;
            }
            dcTotalHoursShort = dcTotalHoursOfficial - dcTotalHours;
            if (dcTotalHours < dcTotalHoursOfficial)//Short Hours!!!
            {
                lblError.Text = "**You do not have enough hours to record, you are short <font color='Blue'><b>" + dcTotalHoursShort.ToString("0.00") + "</b></font> hours!";
                lblError.ForeColor = Color.Red;
                return 0;
            }
            if (dcTotalHours > dcTotalHoursOfficial)//Short Hours!!!
            {
                lblError.Text = "**You are trying to record <font color='Blue'><b>" + dcTotalHoursShort.ToString("0.00").Replace("-", "") + "</b></font> hours more that is needed!";
                lblError.ForeColor = Color.Red;
                return 0;
            }

            //If no issues record hours..
            for (int i = 0; i < gvRecord.Rows.Count; i++)
            {
                lblEmployeeID = (Label)gvRecord.Rows[i].FindControl("lblEmployeeID");
                lblEmployee = (Label)gvRecord.Rows[i].FindControl("lblEmployee");
                txtHours = (TextBox)gvRecord.Rows[i].FindControl("txtHours");
                dcHours = Convert.ToDecimal(txtHours.Text);
                iEmployeeID = Convert.ToInt32(lblEmployeeID.Text);
                sEmployee = lblEmployee.Text;


                try
                {
                    //Add record for employee in loop...
                    WipEmployeeJobHours jh = new WipEmployeeJobHours();
                    jh.Job = sJob;
                    jh.EmployeeID = iEmployeeID;
                    jh.Hours = dcHours;
                    db.WipEmployeeJobHours.InsertOnSubmit(jh);
                    db.SubmitChanges();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Debug.WriteLine("Error with " + sEmployee);
                }

            }//End loop...

            // BindEmployees(sJob);//Dont bind for managers...

            lblError.Text = "**All recorded successfully!";
            lblError.ForeColor = Color.Green;
            return 1;
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
        string sJob = "";
        if (Request.QueryString["job"] != null)
        {
            sJob = Request.QueryString["job"].ToString();
        }
        Int64 iJob = 0;
        if (sJob != "")
        {
            iJob = Convert.ToInt64(sJob);
        }

        DataTable dt = SharedFunctions.GetJobDescriptionAndStockCode(iJob);
        if (!Page.IsPostBack)
        {
            BindEmployees(iJob);
            lblJob.Text = "Stk#: " + dt.Rows[0]["StockCode"].ToString() + " : " + dt.Rows[0]["JobDescription"].ToString() + " - Job: (" + sJob + ") PL:" + dt.Rows[0]["LineName"].ToString();
        }
        if (IsPostBack)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isPostBack", "isPostBack();", true);
        }
        dt.Dispose();
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        string sJob = "";
        if (Request.QueryString["job"] != null)
        {
            sJob = Request.QueryString["job"].ToString();
        }
        int iResult = RecordHours(sJob);
        //if (iResult == 1)
        //{
        //    string code = "<script>window.opener.document.getElementById('MainContent_popUpAnchor').click();</script>";

        //    //call back to parent page...
        //    if (!ClientScript.IsClientScriptBlockRegistered("someKey"))
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), "someKey", code);
        //    }
        //    if (!ClientScript.IsClientScriptBlockRegistered("Popup2"))
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "Popup2", "window.self.close(); ", true);
        //    }
        //    // Close window and call back to parent page...
        //    if (!ClientScript.IsClientScriptBlockRegistered("Popup"))
        //    {
        //        ClientScript.RegisterClientScriptBlock(this.GetType(), "Popup", "window.opener.location.reload();", true);
        //    }

        //    Session["Event"] = 1;
        //}
    }
    protected void gvRecord_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        lblError.Text = "";
        string sJob = "";
        if (Request.QueryString["Job"] != null)
        {
            sJob = Request.QueryString["Job"].ToString();
        }
        decimal dcHours = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TextBox txtHours = (TextBox)e.Row.FindControl("txtHours");

            if (txtHours.Text != "")
            {
                dcHours = Convert.ToDecimal(txtHours.Text);
                dcTotalHours += dcHours;
            }
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblTotalHours = (Label)e.Row.FindControl("lblTotalHours");
            Label lblTotalHoursOfficial = (Label)e.Row.FindControl("lblTotalHoursOfficial");
            dcTotalHoursOfficial = SharedFunctions.GetJobHours(sJob);
            lblTotalHoursOfficial.Text = "Official Hours: <font color='Blue'><b>" + dcTotalHoursOfficial.ToString("0.00") + "</b></font>";
            lblTotalHours.Text = "Total hours you Recorded: <font color='Blue'><b>" + dcTotalHours.ToString("0.00") + "</b></font>";

        }
    }

    #endregion






}