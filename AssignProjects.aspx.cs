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

public partial class AssignProjects : System.Web.UI.Page
{

    #region Subs
    private void LoadManagers()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddlManagers.Items.Clear();
            var query = (from u in db.WipUsers
                         where u.RoleID == 3 //Managers
                         select new
                         {
                             u.UserID,
                             FullName = (u.FirstName + " " + (u.MiddleName ?? "") + " " + u.LastName).Replace("  ", " ")
                         });
            foreach (var a in query)
            {
                ddlManagers.Items.Add(new ListItem(a.FullName, a.UserID.ToString()));
            }
            ddlManagers.Items.Insert(0, new ListItem("--Select a manager--", "0"));
        }
    }
    private void LoadAssignedJobs(int iUserID, string sAssignedDate)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbAssignedJobs.Items.Clear();
            var query = (from ja in db.WipJobAssigns
                         join wm in db.WipMaster on ja.Job equals wm.Job into wm_join
                         from wm in wm_join.DefaultIfEmpty()
                         where
                           ja.UserID == iUserID &&
                           ja.DateAdded >= Convert.ToDateTime(sAssignedDate + " 00:00:00") && ja.DateAdded <= Convert.ToDateTime(sAssignedDate + " 23:59:59")
                         select new
                         {
                             ja.Job,
                             wm.JobDescription,
                             wm.StockCode,
                             wm.ActCompleteDate,
                             wm.Warehouse
                         });
            foreach (var a in query)
            {
                string sJob = int.Parse(a.Job).ToString();
                lbAssignedJobs.Items.Add(new ListItem("stk# " + a.StockCode + " - Job#: " + sJob + " - " + a.JobDescription + " -  CompDate: " + Convert.ToDateTime(a.ActCompleteDate).ToShortDateString() + " (" + a.Warehouse.Trim() + ") ", a.Job.ToString()));
            }
        }
    }
    private void LoadAvailableJobs(string sWarehouse, bool bShowUnAssignedJobs)
    {//NOTE:Criteria could change..Uses first day of month until today...
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbAvailableJobs.Items.Clear();

            if (sWarehouse == "0")//AllWhere Houses...
            {
                if (bShowUnAssignedJobs)
                {
                    var query = (from m in db.WipMaster
                                 where
                                   !
                                     (from wipjobassigns in db.WipJobAssigns
                                      select new
                                      {
                                          wipjobassigns.Job
                                      }).Contains(new { m.Job }) &&
                                   m.ActCompleteDate >= DateTime.Now.AddDays(-180)
                                   && m.ActCompleteDate <= Convert.ToDateTime(DateTime.Now)
                                 orderby m.ActCompleteDate descending
                                 select new
                                 {
                                     m.Job,
                                     m.JobDescription,
                                     m.StockCode,
                                     m.ActCompleteDate,
                                     m.Warehouse
                                 });
                    foreach (var a in query)
                    {
                        string sJob = int.Parse(a.Job).ToString();
                        lbAvailableJobs.Items.Add(new ListItem("stk# " + a.StockCode + " - Job#: " + sJob + " - " + a.JobDescription + " -  CompDate: " + Convert.ToDateTime(a.ActCompleteDate).ToShortDateString() + " (" + a.Warehouse.Trim() + ") ", a.Job.ToString()));
                    }
                }
                else
                {
                    var query = (from m in db.WipMaster
                                 where
                                   !
                                     (from wipjobassigns in db.WipJobAssigns
                                      select new
                                      {
                                          wipjobassigns.Job
                                      }).Contains(new { m.Job }) &&
                                   m.ActCompleteDate >= Convert.ToDateTime(SharedFunctions.GetFirstDayOfMonth(DateTime.Now).ToShortDateString() + " 00:00:00")
                                   && m.ActCompleteDate <= Convert.ToDateTime(DateTime.Now)
                                 orderby m.ActCompleteDate descending
                                 select new
                                 {
                                     m.Job,
                                     m.JobDescription,
                                     m.StockCode,
                                     m.ActCompleteDate,
                                     m.Warehouse

                                 });
                    foreach (var a in query)
                    {
                        string sJob = int.Parse(a.Job).ToString();
                        lbAvailableJobs.Items.Add(new ListItem("stk# " + a.StockCode + " - Job#: " + sJob + " - " + a.JobDescription + " -  CompDate: " + Convert.ToDateTime(a.ActCompleteDate).ToShortDateString() + " (" + a.Warehouse.Trim() + ") ", a.Job.ToString()));
                    }
                }
            }
            else//Selected Warehouse...
            {
                if (bShowUnAssignedJobs)
                {
                    var query = (from m in db.WipMaster
                                 where m.Warehouse == sWarehouse &&
                                   !
                                     (from wipjobassigns in db.WipJobAssigns
                                      select new
                                      {
                                          wipjobassigns.Job
                                      }).Contains(new { m.Job }) &&
                                   m.ActCompleteDate >= DateTime.Now.AddDays(-180)
                                   && m.ActCompleteDate <= Convert.ToDateTime(DateTime.Now)
                                 orderby m.ActCompleteDate descending
                                 select new
                                 {
                                     m.Job,
                                     m.JobDescription,
                                     m.StockCode,
                                     m.ActCompleteDate,
                                     m.Warehouse
                                 });
                    foreach (var a in query)
                    {
                        string sJob = int.Parse(a.Job).ToString();
                        lbAvailableJobs.Items.Add(new ListItem("stk# " + a.StockCode + " - Job#: " + sJob + " - " + a.JobDescription + " -  CompDate: " + Convert.ToDateTime(a.ActCompleteDate).ToShortDateString() + " (" + a.Warehouse.Trim() + ") ", a.Job.ToString()));
                    }
                }
                else
                {
                    var query = (from m in db.WipMaster
                                 where m.Warehouse == sWarehouse &&
                                   !
                                     (from wipjobassigns in db.WipJobAssigns
                                      select new
                                      {
                                          wipjobassigns.Job
                                      }).Contains(new { m.Job }) &&
                                   m.ActCompleteDate >= Convert.ToDateTime(SharedFunctions.GetFirstDayOfMonth(DateTime.Now).ToShortDateString() + " 00:00:00")
                                   && m.ActCompleteDate <= Convert.ToDateTime(DateTime.Now)
                                 orderby m.ActCompleteDate descending
                                 select new
                                 {
                                     m.Job,
                                     m.JobDescription,
                                     m.StockCode,
                                     m.ActCompleteDate,
                                     m.Warehouse
                                 });
                    foreach (var a in query)
                    {
                        string sJob = int.Parse(a.Job).ToString();
                        lbAvailableJobs.Items.Add(new ListItem("stk# " + a.StockCode + " - Job#: " + sJob + " - " + a.JobDescription + " -  CompDate: " + Convert.ToDateTime(a.ActCompleteDate).ToShortDateString() + " (" + a.Warehouse.Trim() + ") ", a.Job.ToString()));
                    }
                }
            }
        }
    }
    private void GetLoadAvailableJobs()
    {
        lblError.Text = "";
        bool bShowUnAssignedJobs = false;

        if (chkShowUnAssigned.Checked)
        {
            bShowUnAssignedJobs = true;
        }
        else
        {
            bShowUnAssignedJobs = false;
        }
        string sWareHouse = "";

        sWareHouse = ddlWareHouses.SelectedValue;
        LoadAvailableJobs(sWareHouse, bShowUnAssignedJobs);
    }
    private void AddSingleFunction(string sjobDesc, string sJob, int iSuperID, int iManagerID, string sAssignedDate)
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
                    WipJobAssigns ja = new WipJobAssigns();
                    ja.Job = sJob;
                    ja.UserID = iManagerID;
                    ja.AddedBy = iSuperID;
                    ja.DateAdded = DateTime.Now;
                    db.WipJobAssigns.InsertOnSubmit(ja);
                    db.SubmitChanges();
                }

                LoadAssignedJobs(iManagerID, sAssignedDate);

                rsListbox.Sort(ref lbNoDups, rsListbox.SortOrder.Ascending);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void RemoveSingleFunction(int iManagerID, string sAssignedDate, string sJob)
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
                WipJobAssigns ja = db.WipJobAssigns.Single(p => p.Job == sJob && p.UserID == iManagerID);
                db.WipJobAssigns.DeleteOnSubmit(ja);
                db.SubmitChanges();

                LoadAssignedJobs(iManagerID, sAssignedDate);

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
            var query = (from ja in db.WipJobAssigns
                         where ja.Job == sJob
                         select ja);
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
            LoadManagers();
            GetLoadAvailableJobs();
            txtAssignedDate.Text = DateTime.Now.ToShortDateString();
            
        }
        //LoadAvailableJobs();//Load outside postback so it refreshes on all updates...
    }
    protected void ddlManagers_SelectedIndexChanged(object sender, EventArgs e)
    {

        lbAssignedJobs.Items.Clear();
        int iUserID = 0;
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
        if (ddlManagers.SelectedIndex == 0)
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
        iUserID = Convert.ToInt32(ddlManagers.SelectedValue);

        LoadAssignedJobs(iUserID, sAssignedDate);
        if (lbAssignedJobs.Items.Count > 0)
        {
            btnRemoveAll.Enabled = true;
            btnRemoveOne.Enabled = true;
        }
 
    }
    protected void txtAssignedDate_TextChanged(object sender, EventArgs e)
    {
        int iUserID = 0;
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
        if (ddlManagers.SelectedIndex == 0)
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
        iUserID = Convert.ToInt32(ddlManagers.SelectedValue);

        LoadAssignedJobs(iUserID, sAssignedDate);

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
        string sAssignedDate="";
        string sJob = lbAvailableJobs.SelectedValue;
        string sJobDesc = lbAvailableJobs.SelectedItem.Text;
        int iManagerID = 0;
        if(ddlManagers.SelectedIndex==0)
        {
            return;
        }
        iManagerID = Convert.ToInt32(ddlManagers.SelectedValue);
        if(txtAssignedDate.Text.Trim()=="")
        {
            lblError.Text="**Assigned Date Cannot be blank!";
            return;
        }
        else
        {
            sAssignedDate = txtAssignedDate.Text.Trim();
        }

        AddSingleFunction(sJobDesc, sJob, iUserID, iManagerID, sAssignedDate);

        GetLoadAvailableJobs();
    }
    protected void btnAddAll_Click(object sender, EventArgs e)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lblError.Text = "";
            string sAssignedDate = "";
            string sJob = "";
            int iSuperID = 0;
            if (Session["UserID"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            iSuperID = Convert.ToInt32(Session["UserID"]);
            int iManagerID = 0;
            if (ddlManagers.SelectedIndex == 0)
            {
                lblError.Text = "**You have not selected a manager, try again.";
                return;
            }
            iManagerID = Convert.ToInt32(ddlManagers.SelectedValue);
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
                        WipJobAssigns ja = new WipJobAssigns();
                        ja.Job = sJob;
                        ja.UserID = iManagerID;
                        ja.AddedBy = iSuperID;
                        ja.DateAdded = DateTime.Now;
                        db.WipJobAssigns.InsertOnSubmit(ja);
                        db.SubmitChanges();
                    }
                }
                LoadAssignedJobs(iManagerID, sAssignedDate);
                GetLoadAvailableJobs();
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
        int iManagerID = 0;
        if (ddlManagers.SelectedIndex == 0)
        {
            return;
        }
        iManagerID = Convert.ToInt32(ddlManagers.SelectedValue);
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
        RemoveSingleFunction(iManagerID, sAssignedDate, sJob);
        GetLoadAvailableJobs();
    }
    protected void btnRemoveAll_Click(object sender, EventArgs e)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lblError.Text = "";
            string sJob = "";
            int iManagerID = 0;
            if (ddlManagers.SelectedIndex == 0)
            {
                return;
            }
            iManagerID = Convert.ToInt32(ddlManagers.SelectedValue);
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

                        WipJobAssigns ja = db.WipJobAssigns.Single(p => p.Job == sJob && p.UserID == iManagerID);
                        db.WipJobAssigns.DeleteOnSubmit(ja);
                        db.SubmitChanges();
                    }

                }
                LoadAssignedJobs(iManagerID, sAssignedDate);
                GetLoadAvailableJobs();
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
    protected void chkShowUnAssigned_CheckedChanged(object sender, EventArgs e)
    {
        GetLoadAvailableJobs();
    }
    protected void ddlWareHouses_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetLoadAvailableJobs();
    }

    #endregion







}