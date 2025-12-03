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

public partial class AssignProductionLine : System.Web.UI.Page
{
    #region Subs
    private void LoadProductionLines()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddlProductionLines.Items.Clear();
            var query = (from p in db.WipProductionLines
                         orderby p.LineName
                         select new
                         {
                             p.ProLineID,
                             p.LineName
                         });
            foreach (var a in query)
            {
                ddlProductionLines.Items.Add(new ListItem(a.LineName + " - #" + a.ProLineID.ToString(), a.ProLineID.ToString()));
            }
            ddlProductionLines.Items.Insert(0, new ListItem("--Select a production line--", "0"));
        }
    }
    private void LoadAssignedJobs(int iProLineID, string sAssignedDate)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbAssignedJobs.Items.Clear();
            var query = (from pa in db.WipProductionLineAssigns
                         join wm in db.WipMaster on pa.Job equals wm.Job into wm_join
                         from wm in wm_join.DefaultIfEmpty()
                         where
                           pa.ProLineID == iProLineID &&
                           pa.DateAdded >= Convert.ToDateTime(sAssignedDate + " 00:00:00") && pa.DateAdded <= Convert.ToDateTime(sAssignedDate + " 23:59:59")
                         select new
                         {
                             pa.Job,
                             wm.JobDescription,
                             wm.StockCode,
                             wm.ActCompleteDate
                         });
            foreach (var a in query)
            {
                string sJob = int.Parse(a.Job).ToString();
                lbAssignedJobs.Items.Add(new ListItem("stk#" + a.StockCode + " - Job#: " + sJob + " - " + a.JobDescription + " -  CompDate: " + Convert.ToDateTime(a.ActCompleteDate).ToShortDateString(), a.Job.ToString()));
            }
        }
    }
    private void LoadAvailableJobs(int iManagerID, string sStockCode)
    {//NOTE:Criteria could change..Uses first day of month until today...
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbAvailableJobs.Items.Clear();

            if (sStockCode == "0")//All Jobs
            {
                var query = (from ja in db.WipJobAssigns
                             join wm in db.WipMaster on ja.Job equals wm.Job into wm_join
                             from wm in wm_join.DefaultIfEmpty()
                             where
                             ja.UserID == iManagerID &&
                               !
                                 (from pa in db.WipProductionLineAssigns
                                  select new
                                  {
                                      pa.Job
                                  }).Contains(new { ja.Job })
                             orderby wm.ActCompleteDate descending
                             select new
                             {
                                 ja.Job,
                                 wm.JobDescription,
                                 wm.StockCode,
                                 wm.ActCompleteDate
                             });
                foreach (var a in query)
                {
                    string sJob = int.Parse(a.Job).ToString();
                    lbAvailableJobs.Items.Add(new ListItem("Stock Code: " + a.StockCode + " - Job#: " + sJob + " - " + a.JobDescription + " -  CompDate: " + Convert.ToDateTime(a.ActCompleteDate).ToShortDateString(), a.Job.ToString()));
                }

            }
            else//Selected By StockCode...
            {
                var query = (from ja in db.WipJobAssigns
                             join wm in db.WipMaster on ja.Job equals wm.Job into wm_join
                             from wm in wm_join.DefaultIfEmpty()
                             where wm.StockCode == sStockCode &&
                             ja.UserID == iManagerID &&
                               !
                                 (from pa in db.WipProductionLineAssigns
                                  select new
                                  {
                                      pa.Job
                                  }).Contains(new { ja.Job })
                             orderby wm.ActCompleteDate descending
                             select new
                             {
                                 ja.Job,
                                 wm.JobDescription,
                                 wm.StockCode,
                                 wm.ActCompleteDate
                             });
                foreach (var a in query)
                {
                    string sJob = int.Parse(a.Job).ToString();
                    lbAvailableJobs.Items.Add(new ListItem("Stock Code: " + a.StockCode + " - Job#: " + sJob + " - " + a.JobDescription + " -  CompDate: " + Convert.ToDateTime(a.ActCompleteDate).ToShortDateString(), a.Job.ToString()));
                }
            }
        }
    }
    private void LoadAvailableStockCodes(int iManagerID)
    {//NOTE:Criteria could change..Uses first day of month until today...
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbAvailableJobs.Items.Clear();
            var query = (from ja in db.WipJobAssigns
                         join wm in db.WipMaster on ja.Job equals wm.Job into wm_join
                         from wm in wm_join.DefaultIfEmpty()
                         where
                           ja.UserID == iManagerID &&
                           !
                             (from wipproductionlineassigns in db.WipProductionLineAssigns
                              select new
                              {
                                  wipproductionlineassigns.Job
                              }).Contains(new { wm.Job })

                         group wm by new
                         {
                             wm.StockCode,
                             wm.JobDescription
                         } into g
                         orderby
                           g.Key.JobDescription
                         select new
                         {
                             g.Key.StockCode,
                             g.Key.JobDescription

                         });
            foreach (var a in query)
            {
                ddlStockCodes.Items.Add(new ListItem(a.StockCode + " - " + a.JobDescription, a.StockCode));
            }
            ddlStockCodes.Items.Insert(0, new ListItem("ALL STOCK CODES", "0"));
        }
    }
    private void AddSingleFunction(string sjobDesc, string sJob, int iManagerID, int iProlineID, string sAssignedDate)
    {//For add mode...
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            //put  item(s) into cart list box... 
            try
            {
                ListBox lbNoDups = new ListBox();
                this.lbAssignedJobs.Items.Add(new ListItem(sjobDesc, sJob));

                SharedFunctions.RemoveDupsFromListBox(lbAssignedJobs);

                //Add Single manager to job Assignments...
                if (!RecordAlreadyinTable(sJob))
                {
                    WipProductionLineAssigns pa = new WipProductionLineAssigns();
                    pa.Job = sJob;
                    pa.ProLineID = iProlineID;
                    pa.AddedBy = iManagerID;
                    pa.DateAdded = DateTime.Now;
                    db.WipProductionLineAssigns.InsertOnSubmit(pa);
                    db.SubmitChanges();
                }

                LoadAssignedJobs(iProlineID, sAssignedDate);
                LoadAvailableJobs(iManagerID, ddlStockCodes.SelectedValue);
                rsListbox.Sort(ref lbNoDups, rsListbox.SortOrder.Ascending);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void RemoveSingleFunction(int iProlineID, string sAssignedDate, string sJob)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            try
            {

                if (lbAssignedJobs.Items.Count == 0) { return; }
                if (lbAssignedJobs.SelectedIndex == -1) { return; }

                if (HaveHoursBeenRecorded(sJob))
                {
                    lblError.Text = "**You can't remove a Job that has hours recorded!";
                    return;
                }

                //  this.ddlAgentsAdd.Items.Add(new ListItem(sAgent, iAgentID.ToString()));//Add to included role...

                // Remove item(s) from Available listbox...
                foreach (ListItem li in lbAvailableJobs.Items)
                {
                    if (sJob == li.Value)
                    {
                        lbAssignedJobs.Items.RemoveAt(lbAssignedJobs.SelectedIndex);//remove from available items list box...
                    }
                }
                //Remove Dups...
                SharedFunctions.RemoveDupsFromListBox(lbAssignedJobs);

                //remove agent from group assignments...
                WipProductionLineAssigns ja = db.WipProductionLineAssigns.Single(p => p.Job == sJob && p.ProLineID == iProlineID);
                db.WipProductionLineAssigns.DeleteOnSubmit(ja);
                db.SubmitChanges();

                int iManagerID = 0;
                if (Session["UserID"] == null)
                {
                    return;
                }
                iManagerID = Convert.ToInt32(Session["UserID"]);
                LoadAssignedJobs(iProlineID, sAssignedDate);
                LoadAvailableJobs(iManagerID, ddlStockCodes.SelectedValue);
                rsListbox.Sort(ref lbAssignedJobs, rsListbox.SortOrder.Ascending);
                // rsListbox.Sort(ref ddlAgentsAdd, rsListbox.SortOrder.Ascending);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }


    #endregion

    #region Functions
    private bool RecordAlreadyinTable(string sJob)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from pa in db.WipProductionLineAssigns
                         where pa.Job == sJob
                         select pa);
            int iCount = query.Count();
            if (iCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private bool HaveHoursBeenRecorded(string sJob)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from jh in db.WipEmployeeJobHours
                         where jh.Job == sJob
                         select jh);
            int iCount = query.Count();
            if (iCount > 0)
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
            LoadProductionLines();
            LoadProductionLinesEmployees();
            txtAssignedDate.Text = DateTime.Now.ToShortDateString();
            LoadAvailableStockCodes(iUserID);
            LoadAvailableJobs(iUserID, ddlStockCodes.SelectedValue);
        }

    }
    protected void ddlProductionLines_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbAssignedJobs.Items.Clear();
        int iProlineID = 0;
        string sAssignedDate = "";
        if (txtAssignedDate.Text.Trim() != "")
        {
            sAssignedDate = txtAssignedDate.Text.Trim();
        }
        else
        {
            lblError.Text = "**Assigned Date was blank, try again.";
            return;
        }
        if (ddlProductionLines.SelectedIndex == 0)
        {
            btnAddAll.Enabled = false;
            btnAddOne.Enabled = false;
            btnRemoveAll.Enabled = false;
            btnRemoveOne.Enabled = false;
            return;
        }
        else
        {
            btnAddAll.Enabled = true;
            btnAddOne.Enabled = true;

        }
        iProlineID = Convert.ToInt32(ddlProductionLines.SelectedValue);
        int iManagerID = 0;
        if (Session["UserID"] == null)
        {
            return;
        }
        iManagerID = Convert.ToInt32(Session["UserID"]);
        LoadAssignedJobs(iProlineID, sAssignedDate);
        LoadAvailableJobs(iManagerID, ddlStockCodes.SelectedValue); 
        if (lbAssignedJobs.Items.Count > 0)
        {
            btnRemoveAll.Enabled = true;
            btnRemoveOne.Enabled = true;
        }
    }
    protected void txtAssignedDate_TextChanged(object sender, EventArgs e)
    {
        int iProlineID = 0;
        string sAssignedDate = "";
        if (txtAssignedDate.Text.Trim() != "")
        {
            sAssignedDate = txtAssignedDate.Text.Trim();
        }
        else
        {
            lblError.Text = "**Assigned Date was blank, try again.";
            return;
        }
        if (ddlProductionLines.SelectedIndex == 0)
        {
            btnAddAll.Enabled = false;
            btnAddOne.Enabled = false;
            btnRemoveAll.Enabled = false;
            btnRemoveOne.Enabled = false;
            return;
        }
        else
        {
            btnAddAll.Enabled = true;
            btnAddOne.Enabled = true;
            if (lbAssignedJobs.Items.Count > 0)
            {
                btnRemoveAll.Enabled = true;
                btnRemoveOne.Enabled = true;
            }
        }
        iProlineID = Convert.ToInt32(ddlProductionLines.SelectedValue);
        int iManagerID = 0;
        if (Session["UserID"] == null)
        {
            return;
        }
        iManagerID = Convert.ToInt32(Session["UserID"]);
        LoadAssignedJobs(iProlineID, sAssignedDate);
        LoadAvailableJobs(iManagerID, ddlStockCodes.SelectedValue); 

    }

    protected void btnAddOne_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iUserID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        if (lbAvailableJobs.SelectedIndex == -1)
        {
            lblError.Text = "**You have not selected a job(s) to add, try again.";
            return;
        }
        if (lbAvailableJobs.Items.Count == 0) { return; }
        if (lbAvailableJobs.SelectedIndex == -1) { return; }

        bool bSelected = false;
        foreach (ListItem li in lbAvailableJobs.Items)
        {
            if (li.Selected == false)
            {
                bSelected = false;
            }
            else
            {
                bSelected = true;
                break;
            }
        }
        if (bSelected == false)
        {
            lblError.Text = "**You have not selected a job(s)!";
            return;
        }
        else
        {
            lblError.Text = "";
        }
        string sAssignedDate = "";
        string sJob = lbAvailableJobs.SelectedValue;
        string sJobDesc = lbAvailableJobs.SelectedItem.Text;
        int iProlineID = 0;
        if (ddlProductionLines.SelectedIndex == 0)
        {
            return;
        }
        iProlineID = Convert.ToInt32(ddlProductionLines.SelectedValue);
        if (txtAssignedDate.Text.Trim() == "")
        {
            lblError.Text = "**Assigned Date Cannot be blank!";
            return;
        }
        else
        {
            sAssignedDate = txtAssignedDate.Text.Trim();
        }

        AddSingleFunction(sJobDesc, sJob, iUserID, iProlineID, sAssignedDate);


    }
    protected void btnAddAll_Click(object sender, EventArgs e)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lblError.Text = "";
            string sAssignedDate = "";
            string sJob = "";
            int iManagerID = 0;
            if (Session["UserID"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            iManagerID = Convert.ToInt32(Session["UserID"]);
            int iProlineID = 0;
            if (ddlProductionLines.SelectedIndex == 0)
            {
                lblError.Text = "**You have not selected a production line, try again.";
                return;
            }
            iProlineID = Convert.ToInt32(ddlProductionLines.SelectedValue);
            if (lbAvailableJobs.SelectedIndex == -1)
            {
                lblError.Text = "**You have not selected a job(s) to Add, try again.";
                return;
            }
            if (txtAssignedDate.Text.Trim() == "")
            {
                lblError.Text = "**Assigned Date Cannot be blank!";
                return;
            }
            else
            {
                sAssignedDate = txtAssignedDate.Text.Trim();
            }
            try
            {
                ListItemCollection lsc = new ListItemCollection();
                for (int i = 0; i < lbAvailableJobs.Items.Count; i++)
                {
                    if (lbAvailableJobs.Items[i].Selected == true)
                    {
                        string value = lbAvailableJobs.Items[i].Value;
                        string text = lbAvailableJobs.Items[i].Text;
                        ListItem lst = new ListItem();
                        lst.Text = text;
                        lst.Value = value;
                        lbAssignedJobs.Items.Add(lst);
                        lsc.Add(lst);
                    }
                }
                foreach (ListItem ls in lsc)
                {

                }
                //Remove Dups...
                SharedFunctions.RemoveDupsFromListBox(lbAssignedJobs);

                //Add all items in Assigned listbox...


                foreach (ListItem li in lbAssignedJobs.Items)
                {
                    sJob = li.Value;
                    //Add Single manager to job Assignments...
                    if (!RecordAlreadyinTable(sJob))
                    {
                        WipProductionLineAssigns pa = new WipProductionLineAssigns();
                        pa.Job = sJob;
                        pa.ProLineID = iProlineID;
                        pa.AddedBy = iManagerID;
                        pa.DateAdded = DateTime.Now;
                        db.WipProductionLineAssigns.InsertOnSubmit(pa);
                        db.SubmitChanges();
                    }
                }
                LoadAssignedJobs(iProlineID, sAssignedDate);
                LoadAvailableJobs(iManagerID, ddlStockCodes.SelectedValue);
                rsListbox.Sort(ref lbAssignedJobs, rsListbox.SortOrder.Ascending);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    protected void btnRemoveOne_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iProlineID = 0;
        if (ddlProductionLines.SelectedIndex == 0)
        {
            return;
        }
        iProlineID = Convert.ToInt32(ddlProductionLines.SelectedValue);
        string sAssignedDate = "";
        if (txtAssignedDate.Text.Trim() == "")
        {
            lblError.Text = "**Assigned Date Cannot be blank!";
            return;
        }
        else
        {
            sAssignedDate = txtAssignedDate.Text.Trim();
        }
        string sJob = lbAssignedJobs.SelectedValue;
        RemoveSingleFunction(iProlineID, sAssignedDate, sJob);
    }
    protected void btnRemoveAll_Click(object sender, EventArgs e)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lblError.Text = "";
            string sJob = "";
            int iProlineID = 0;
            if (ddlProductionLines.SelectedIndex == 0)
            {
                return;
            }
            iProlineID = Convert.ToInt32(ddlProductionLines.SelectedValue);
            string sAssignedDate = "";
            if (txtAssignedDate.Text.Trim() == "")
            {
                lblError.Text = "**Assigned Date Cannot be blank!";
                return;
            }
            else
            {
                sAssignedDate = txtAssignedDate.Text.Trim();
            }

            //Delete all items in Assigned listbox...


            try
            {

                foreach (ListItem li in lbAssignedJobs.Items)
                {
                    sJob = li.Value;
                    //Delete Single Agent from Group Assignments...
                    if (li.Selected)
                    {
                        if (HaveHoursBeenRecorded(sJob))
                        {
                            lblError.Text += "**You can't remove Job: <font color='blue'>" + sJob + "</font> it has hours recorded!<br/>";
                            continue;
                        }

                        WipProductionLineAssigns pa = db.WipProductionLineAssigns.Single(p => p.Job == sJob && p.ProLineID == iProlineID);
                        db.WipProductionLineAssigns.DeleteOnSubmit(pa);
                        db.SubmitChanges();
                    }

                }
                int iManagerID = 0;
                if (Session["UserID"] == null)
                {
                    return;
                }
                iManagerID = Convert.ToInt32(Session["UserID"]);
                LoadAssignedJobs(iProlineID, sAssignedDate);
                LoadAvailableJobs(iManagerID, ddlStockCodes.SelectedValue);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }

    protected void lbnSelectAllAssign_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAvailableJobs.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearAssign_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAvailableJobs.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    protected void lbnSelectAllAssigned_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAssignedJobs.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearAssigned_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAssignedJobs.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }

    protected void ddlStockCodes_SelectedIndexChanged(object sender, EventArgs e)
    {
        int iUserID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        LoadAvailableJobs(iUserID, ddlStockCodes.SelectedValue);
    }

    #endregion

    #region  AssignEmployees

    #region Subs
    private void LoadProductionLinesEmployees()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddlProductionLinesEmployees.Items.Clear();
            var query = (from p in db.WipProductionLines
                         orderby p.LineName
                         select new
                         {
                             p.ProLineID,
                             p.LineName
                         });
            foreach (var a in query)
            {
                ddlProductionLinesEmployees.Items.Add(new ListItem(a.LineName + " - #" + a.ProLineID.ToString(), a.ProLineID.ToString()));
            }
            ddlProductionLinesEmployees.Items.Insert(0, new ListItem("--Select a production line--", "0"));
        }
    }
    private void LoadAssignedEmployees(int iProLineID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbAssignedEmployees.Items.Clear();
            var query = (from ea in db.WipEmployeeAssigns
                         where
                           ea.ProLineID == iProLineID
                         orderby ea.WipEmployees.FirstName
                         select new
                         {
                             ea.EmployeeID,
                             FullName = (ea.WipEmployees.FirstName + " " + (ea.WipEmployees.MiddleName ?? "") + " " + ea.WipEmployees.LastName).Replace("  ", " ")
                         });
            foreach (var a in query)
            {
                lbAssignedEmployees.Items.Add(new ListItem(a.FullName, a.EmployeeID.ToString()));
            }
        }
    }
    private void LoadAvailableEmployees(int iProlineID)
    {//NOTE:Criteria could change..Uses first day of month until today...
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbAvailableEmployees.Items.Clear();
            var query = (from e in db.WipEmployees
                         orderby e.FirstName
                         where
                           !
                             (from ea in db.WipEmployeeAssigns
                              where ea.ProLineID == iProlineID
                              select new
                              {
                                  ea.EmployeeID
                              }).Contains(new { e.EmployeeID })
                         select new
                         {
                             e.EmployeeID,
                             FullName = (e.FirstName + " " + (e.MiddleName ?? "") + " " + e.LastName).Replace("  ", " ")
                         });
            foreach (var a in query)
            {
                lbAvailableEmployees.Items.Add(new ListItem(a.FullName, a.EmployeeID.ToString()));
            }
        }
    }

    private void AddSingleFunction(string sEmployee, int iEmployeeID, int iManagerID, int iProlineID)
    {//For add mode...
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            //put  item(s) into cart list box... 
            try
            {
                ListBox lbNoDups = new ListBox();
                this.lbAssignedEmployees.Items.Add(new ListItem(sEmployee, iEmployeeID.ToString()));

                SharedFunctions.RemoveDupsFromListBox(lbAssignedEmployees);

                //Add Single manager to job Assignments...
                if (!RecordAlreadyinTable(iEmployeeID, iProlineID))
                {
                    WipEmployeeAssigns ea = new WipEmployeeAssigns();
                    ea.EmployeeID = iEmployeeID;
                    ea.ProLineID = iProlineID;
                    ea.AddedBy = iManagerID;
                    ea.DateAdded = DateTime.Now;
                    db.WipEmployeeAssigns.InsertOnSubmit(ea);
                    db.SubmitChanges();
                }

                LoadAssignedEmployees(iProlineID);
                LoadAvailableEmployees(iProlineID);

                rsListbox.Sort(ref lbNoDups, rsListbox.SortOrder.Ascending);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void RemoveSingleFunction(int iProlineID, int iEmployeeID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {


                if (lbAssignedEmployees.Items.Count == 0) { return; }
                if (lbAssignedEmployees.SelectedIndex == -1) { return; }

                if (HaveHoursBeenRecorded(iProlineID, iEmployeeID))
                {
                    lblErrorEmployees.Text = "**You can't remove a Employee from a Production line if they have hours recorded while working on the selected production line!";
                    return;
                }

                //  this.ddlAgentsAdd.Items.Add(new ListItem(sAgent, iAgentID.ToString()));//Add to included role...

                // Remove item(s) from Available listbox...
                foreach (ListItem li in lbAvailableEmployees.Items)
                {
                    if (iEmployeeID == Convert.ToInt32(li.Value))
                    {
                        lbAssignedEmployees.Items.RemoveAt(lbAssignedEmployees.SelectedIndex);//remove from available items list box...
                    }
                }
                //Remove Dups...
                SharedFunctions.RemoveDupsFromListBox(lbAssignedEmployees);

                //remove agent from group assignments...
                WipEmployeeAssigns ea = db.WipEmployeeAssigns.Single(p => p.EmployeeID == iEmployeeID && p.ProLineID == iProlineID);
                db.WipEmployeeAssigns.DeleteOnSubmit(ea);
                db.SubmitChanges();

                LoadAssignedEmployees(iProlineID);
                LoadAvailableEmployees(iProlineID);

                rsListbox.Sort(ref lbAssignedEmployees, rsListbox.SortOrder.Ascending);
                // rsListbox.Sort(ref ddlAgentsAdd, rsListbox.SortOrder.Ascending);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }


    #endregion

    #region Functions
    private bool RecordAlreadyinTable(int iEmployeeID, int iProLineID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from ea in db.WipEmployeeAssigns
                         where ea.EmployeeID == iEmployeeID
                         && ea.ProLineID == iProLineID
                         select ea);
            int iCount = query.Count();
            if (iCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private bool HaveHoursBeenRecorded(int iProLine, int iEmployeeID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from wipemployeejobhours in db.WipEmployeeJobHours
                         where
                             (from wipproductionlineassigns in db.WipProductionLineAssigns
                              where
                                wipproductionlineassigns.ProLineID == iProLine &&
                                wipemployeejobhours.EmployeeID == iEmployeeID
                              select new
                              {
                                  wipproductionlineassigns.Job
                              }).Contains(new { wipemployeejobhours.Job })
                         select new
                         {
                             wipemployeejobhours.JobHoursID,
                             wipemployeejobhours.Job,
                             wipemployeejobhours.EmployeeID,
                             wipemployeejobhours.Hours
                         });
            int iCount = query.Count();
            if (iCount > 0)
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

    protected void ddlProductionLinesEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbAssignedEmployees.Items.Clear();
        int iProlineID = 0;

        if (ddlProductionLinesEmployees.SelectedIndex == 0)
        {
            btnAddAllEmployees.Enabled = false;
            btnAddOneEmployees.Enabled = false;
            btnRemoveAllEmployees.Enabled = false;
            btnRemoveOneEmployees.Enabled = false;
            return;
        }
        else
        {
            btnAddAllEmployees.Enabled = true;
            btnAddOneEmployees.Enabled = true;

        }
        iProlineID = Convert.ToInt32(ddlProductionLinesEmployees.SelectedValue);
        LoadAvailableEmployees(iProlineID);
        LoadAssignedEmployees(iProlineID);
        if (lbAssignedEmployees.Items.Count > 0)
        {
            btnRemoveAllEmployees.Enabled = true;
            btnRemoveOneEmployees.Enabled = true;
        }
    }
    protected void btnAddOneEmployees_Click(object sender, EventArgs e)
    {
        lblErrorEmployees.Text = "";
        int iUserID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        if (lbAvailableEmployees.SelectedIndex == -1)
        {
            lblErrorEmployees.Text = "**You have not selected a employee(s) to add, try again.";
            return;
        }
        if (lbAvailableEmployees.Items.Count == 0) { return; }
        if (lbAvailableEmployees.SelectedIndex == -1) { return; }

        bool bSelected = false;
        foreach (ListItem li in lbAvailableEmployees.Items)
        {
            if (li.Selected == false)
            {
                bSelected = false;
            }
            else
            {
                bSelected = true;
                break;
            }
        }
        if (bSelected == false)
        {
            lblErrorEmployees.Text = "**You have not selected a job(s)!";
            return;
        }
        else
        {
            lblErrorEmployees.Text = "";
        }

        int iEmployeeID = Convert.ToInt32(lbAvailableEmployees.SelectedValue);
        string sEmployee = lbAvailableEmployees.SelectedItem.Text;
        int iProlineID = 0;
        if (ddlProductionLinesEmployees.SelectedIndex == 0)
        {
            return;
        }
        iProlineID = Convert.ToInt32(ddlProductionLinesEmployees.SelectedValue);


        AddSingleFunction(sEmployee, iEmployeeID, iUserID, iProlineID);


    }
    protected void btnAddAllEmployees_Click(object sender, EventArgs e)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lblErrorEmployees.Text = "";
            int iManagerID = 0;
            if (Session["UserID"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            iManagerID = Convert.ToInt32(Session["UserID"]);
            int iProlineID = 0;
            if (ddlProductionLinesEmployees.SelectedIndex == 0)
            {
                lblErrorEmployees.Text = "**You have not selected a production line, try again.";
                return;
            }
            iProlineID = Convert.ToInt32(ddlProductionLinesEmployees.SelectedValue);
            if (lbAvailableEmployees.SelectedIndex == -1)
            {
                lblErrorEmployees.Text = "**You have not selected a employee(s) to Add, try again.";
                return;
            }
            try
            {
                ListItemCollection lsc = new ListItemCollection();
                for (int i = 0; i < lbAvailableEmployees.Items.Count; i++)
                {
                    if (lbAvailableEmployees.Items[i].Selected == true)
                    {
                        string value = lbAvailableEmployees.Items[i].Value;
                        string text = lbAvailableEmployees.Items[i].Text;
                        ListItem lst = new ListItem();
                        lst.Text = text;
                        lst.Value = value;
                        lbAssignedEmployees.Items.Add(lst);
                        lsc.Add(lst);
                    }
                }
                foreach (ListItem ls in lsc)
                {

                }
                //Remove Dups...
                SharedFunctions.RemoveDupsFromListBox(lbAssignedEmployees);

                //Add all items in Assigned listbox...

                int iEmployeeID = 0;
                foreach (ListItem li in lbAssignedEmployees.Items)
                {
                    iEmployeeID = Convert.ToInt32(li.Value);
                    //Add Single manager to job Assignments...
                    if (!RecordAlreadyinTable(iEmployeeID, iProlineID))
                    {
                        WipEmployeeAssigns ea = new WipEmployeeAssigns();
                        ea.EmployeeID = iEmployeeID;
                        ea.ProLineID = iProlineID;
                        ea.AddedBy = iManagerID;
                        ea.DateAdded = DateTime.Now;
                        db.WipEmployeeAssigns.InsertOnSubmit(ea);
                        db.SubmitChanges();
                    }
                }
                LoadAssignedEmployees(iProlineID);
                LoadAvailableEmployees(iProlineID);
                rsListbox.Sort(ref lbAssignedEmployees, rsListbox.SortOrder.Ascending);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    protected void btnRemoveOneEmployees_Click(object sender, EventArgs e)
    {
        lblErrorEmployees.Text = "";
        int iProlineID = 0;
        if (ddlProductionLinesEmployees.SelectedIndex == 0)
        {
            return;
        }
        iProlineID = Convert.ToInt32(ddlProductionLinesEmployees.SelectedValue);
        int iEmployeeID = Convert.ToInt32(lbAssignedEmployees.SelectedValue);
        RemoveSingleFunction(iProlineID, iEmployeeID);
    }
    protected void btnRemoveAllEmployees_Click(object sender, EventArgs e)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lblErrorEmployees.Text = "";
            int iProlineID = 0;
            if (ddlProductionLinesEmployees.SelectedIndex == 0)
            {
                return;
            }
            iProlineID = Convert.ToInt32(ddlProductionLinesEmployees.SelectedValue);

            //Delete all items in Assigned listbox...


            try
            {
                int iEmployeeID = 0;
                foreach (ListItem li in lbAssignedEmployees.Items)
                {
                    iEmployeeID = Convert.ToInt32(li.Value);
                    //Delete Single Agent from Group Assignments...
                    if (li.Selected)
                    {

                        if (HaveHoursBeenRecorded(iProlineID, iEmployeeID))
                        {
                            lblErrorEmployees.Text += "**You can't remove Employee: <font color='blue'>" + SharedFunctions.GetEmployeeName(iEmployeeID) + "</font> they have hours recorded while working on the selected production line!<br/>";
                            continue;
                        }
                        WipEmployeeAssigns ea = db.WipEmployeeAssigns.Single(p => p.EmployeeID == iEmployeeID && p.ProLineID == iProlineID);
                        db.WipEmployeeAssigns.DeleteOnSubmit(ea);
                        db.SubmitChanges();
                    }

                }
                LoadAssignedEmployees(iProlineID);
                LoadAvailableEmployees(iProlineID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }

    protected void lbnSelectAllAssignEmployees_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAvailableEmployees.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearAssignEmployees_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAvailableEmployees.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    protected void lbnSelectAllAssignedEmployees_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAssignedEmployees.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearAssignedEmployees_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAssignedEmployees.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }



    #endregion

    #endregion


}