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

public partial class ProductionInputSearch : System.Web.UI.Page
{

    decimal dcTotalHours = 0;
    decimal dcTotalHoursOfficial = 0;
    decimal dcTotalHoursShort = 0;

    #region Subs
    private void RecordHours(string sJob)
    {//loop through the gridview and record hours..
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            Label lblEmployeeID;
            Label lblEmployee;
            TextBox txtHours;
            decimal dcHours = 0;
            dcTotalHours = 0;
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
                return;
            }
            dcTotalHoursOfficial = SharedFunctions.GetJobHours(sJob); //Get Official Hours...
            if (dcTotalHoursOfficial == 0)
            {
                lblError.Text = "**No Official Hours to record!";
                lblError.ForeColor = Color.Red;
                return;
            }
            dcTotalHoursShort = dcTotalHoursOfficial - dcTotalHours;
            if (dcTotalHours < dcTotalHoursOfficial)//Short Hours!!!
            {
                lblError.Text = "**You do not have enough hours to record, you are short <font color='Blue'><b>" + dcTotalHoursShort.ToString("0.00") + "</b></font> hours!";
                lblError.ForeColor = Color.Red;
                return;
            }
            if (dcTotalHours > dcTotalHoursOfficial)//Short Hours!!!
            {
                lblError.Text = "**You are trying to record <font color='Blue'><b>" + dcTotalHoursShort.ToString("0.00").Replace("-", "") + "</b></font> hours more that is needed!";
                lblError.ForeColor = Color.Red;
                return;
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
                    if (RecordExistsInTable(sJob, iEmployeeID) == true)//Update
                    {
                        //Update record for employee hours in loop...
                        WipEmployeeJobHours jh = db.WipEmployeeJobHours.Single(p => p.Job == sJob && p.EmployeeID == iEmployeeID);
                        jh.Hours = dcHours;
                        db.SubmitChanges();
                    }
                    else//Add
                    {
                        //Add record for employee in loop...
                        WipEmployeeJobHours jh = new WipEmployeeJobHours();
                        jh.Job = sJob;
                        jh.EmployeeID = iEmployeeID;
                        jh.Hours = dcHours;
                        db.WipEmployeeJobHours.InsertOnSubmit(jh);
                        db.SubmitChanges();
                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Debug.WriteLine("Error with " + sEmployee);
                }

            }//End loop...

            Int64 iJob = Convert.ToInt64(sJob);
            BindEmployees(iJob);
            lblError.Text = "**All hours recorded successfully!";
            lblError.ForeColor = Color.Green;
        }
    }
    private void BindEmployees(Int64 iJob)
    {

        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from ea in db.WipEmployeeAssigns
                         where
                           ea.ProLineID ==
                             ((from wipproductionlineassigns in db.WipProductionLineAssigns
                               where
                              Convert.ToInt64(wipproductionlineassigns.Job) == iJob
                               select new
                               {
                                   wipproductionlineassigns.ProLineID
                               }).First().ProLineID)
                         select new
                         {
                             EmployeeID = (System.Int32?)ea.WipEmployees.EmployeeID,
                             Employee = (ea.WipEmployees.FirstName + " " + (ea.WipEmployees.MiddleName ?? "") + " " + ea.WipEmployees.LastName).Replace("  ", " "),
                             Hours = (System.Decimal?)
                               ((from wipemployeejobhours in db.WipEmployeeJobHours
                                 where
                                   wipemployeejobhours.EmployeeID == ea.WipEmployees.EmployeeID &&
                                    Convert.ToInt64(wipemployeejobhours.Job) == iJob
                                 select new
                                 {
                                     wipemployeejobhours.Hours
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
    private void GetHoursData()
    {
        if (txtSearch.Text.Trim() != "")
        {
            string sJob = "";

            sJob = txtSearch.Text.Trim();
            Int64 iJob = 0;
            if (sJob != "")
            {
                iJob = Convert.ToInt64(sJob);
            }
            Session["Job"] = sJob;             
            BindEmployees(iJob);

            DataTable dt = SharedFunctions.GetJobDescriptionAndStockCode(iJob);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["StockCode"] != null)
                {
                    lblJob.Text = "Stk#: " + dt.Rows[0]["StockCode"].ToString() + " : " + dt.Rows[0]["JobDescription"].ToString() + " - Job: (" + sJob + ") PL:" + dt.Rows[0]["LineName"].ToString();
                }
            }
            dt.Dispose();
        }
    }

    #endregion

    #region Functions
    private bool RecordExistsInTable(string sJob, int iEmployeeID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from jh in db.WipEmployeeJobHours
                         where jh.Job == sJob
                         && jh.EmployeeID == iEmployeeID
                         select jh);


            if (query.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
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
        
 
        
        if (!Page.IsPostBack)
        {

        }
      
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        dcTotalHours = 0;
        string sJob = "";
        if (Session["Job"]  != null)
        {
            sJob = Session["Job"].ToString();
        }
        RecordHours(sJob);

    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        GetHoursData();
        
    }
    protected void gvRecord_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      
        lblError.Text = "";
        string sJob = "";
        if (Session["Job"] != null)
        {
            sJob = Session["Job"].ToString();
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
            lblTotalHours.Text = "Total hours Recorded: <font color='Blue'><b>" + dcTotalHours.ToString("0.00") + "</b></font>";    

        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetHoursData();
    }

    
    #endregion

    #region WebMethods
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListJobs(string prefixText, int count, string contextKey)
    {
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {
            // Your LINQ to SQL query goes here 
            using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
            {
                string[] list = db.WipJobAssigns.Where(w => w.Job != null).OrderBy(w => w.Job).Select(w => Convert.ToInt64(w.Job.ToUpper()).ToString()).Take(100).ToArray();

                //var query = (from a in
                //                 (
                //                     ((
                //                     (from cities in db.Cities
                //                      select new
                //                      {
                //                          Result = cities.City
                //                      }).Distinct()
                //                     ).Union
                //                     (
                //                     (from cities in db.Cities
                //                      select new
                //                      {
                //                          Result = cities.County
                //                      }).Distinct()
                //                     )))
                //             orderby
                //               a.Result
                //             select new
                //             {
                //                 a.Result
                //             });

                //List<string> stringArr = new List<string>();

                //stringArr = query.Select(i => i.ToString()).ToList();

                //string[] list = stringArr.ToArray();

                string[] lResult = null;


                try
                {
                    lResult = (from d in list where d.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select d).Take(count).ToArray();
                }
                catch (Exception ex)
                {
                    lResult = new string[] { "None found" };
                    Debug.WriteLine(ex.ToString());

                }
                return lResult;
            }
        }
    }


    #endregion



}