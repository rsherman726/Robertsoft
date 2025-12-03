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

public partial class AssignEmployees : System.Web.UI.Page
{
    #region Subs
    private void LoadProductionLines()
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        ddlProductionLines.Items.Clear();
        var query = (from p in db.WipProductionLines
                     select new
                     {
                         p.ProLineID,
                         p.LineName
                     });
        foreach (var a in query)
        {
            ddlProductionLines.Items.Add(new ListItem(a.LineName, a.ProLineID.ToString()));
        }
        ddlProductionLines.Items.Insert(0, new ListItem("--Select a production line--", "0"));
    }
    private void LoadAssignedEmployees(int iProLineID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lbAssignedEmployees.Items.Clear();
        var query = (from ea in db.WipEmployeeAssigns
                     where
                       ea.ProLineID == iProLineID                      
                     select new
                     {
                          ea.EmployeeID,
                          FullName = (ea.WipEmployees.FirstName + " " + (ea.WipEmployees.MiddleName??"") + " "  +ea.WipEmployees.LastName).Replace("  "," ")
                     });
        foreach (var a in query)
        {
            lbAssignedEmployees.Items.Add(new ListItem(a.FullName, a.EmployeeID.ToString()));
        }

    }
    private void LoadAvailableEmployees(int iProlineID)
    {//NOTE:Criteria could change..Uses first day of month until today...
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        lbAvailableEmployees.Items.Clear();
        var query = (from e in db.WipEmployees
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

    private void AddSingleFunction(string sEmployee, int iEmployeeID, int iManagerID, int iProlineID)
    {//For add mode...
        //put  item(s) into cart list box... 
        try
        {
            ListBox lbNoDups = new ListBox();
            this.lbAssignedEmployees.Items.Add(new ListItem(sEmployee, iEmployeeID.ToString()));

            SharedFunctions.RemoveDupsFromListBox(lbAssignedEmployees);
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            //Add Single manager to job Assignments...
            if (!RecordAlreadyinTable(iEmployeeID,iProlineID))
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
    private void RemoveSingleFunction(int iProlineID, int iEmployeeID)
    {

        try
        {


            if (lbAssignedEmployees.Items.Count == 0) { return; }
            if (lbAssignedEmployees.SelectedIndex == -1) { return; }

            if (HaveHoursBeenRecorded( iProlineID, iEmployeeID))
            {
                lblError.Text = "**You can't remove a Employee from a Production line if they have hours recorded while working on the selected production line!";
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
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
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


    #endregion

    #region Functions
    private bool RecordAlreadyinTable(int iEmployeeID,int iProLineID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
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
    private bool HaveHoursBeenRecorded(int iProLine, int iEmployeeID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
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
        }
    }
    protected void ddlProductionLines_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbAssignedEmployees.Items.Clear();
        int iProlineID = 0;

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
        LoadAvailableEmployees(iProlineID);
        LoadAssignedEmployees(iProlineID);
        if (lbAssignedEmployees.Items.Count > 0)
        {
            btnRemoveAll.Enabled = true;
            btnRemoveOne.Enabled = true;
        }
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
        if (lbAvailableEmployees.SelectedIndex == -1)
        {
            lblError.Text = "**You have not selected a employee(s) to add, try again.";
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
            lblError.Text = "**You have not selected a job(s)!";
            return;
        }
        else
        {
            lblError.Text = "";
        }
       
        int iEmployeeID = Convert.ToInt32(lbAvailableEmployees.SelectedValue);
        string sEmployee = lbAvailableEmployees.SelectedItem.Text;
        int iProlineID = 0;
        if (ddlProductionLines.SelectedIndex == 0)
        {
            return;
        }
        iProlineID = Convert.ToInt32(ddlProductionLines.SelectedValue);
       

        AddSingleFunction(sEmployee, iEmployeeID, iUserID, iProlineID);


    }
    protected void btnAddAll_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
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
        if (lbAvailableEmployees.SelectedIndex == -1)
        {
            lblError.Text = "**You have not selected a employee(s) to Add, try again.";
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
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            int iEmployeeID = 0;
            foreach (ListItem li in lbAssignedEmployees.Items)
            {
                iEmployeeID = Convert.ToInt32(li.Value);
                //Add Single manager to job Assignments...
                if (!RecordAlreadyinTable(iEmployeeID,iProlineID))
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
    protected void btnRemoveOne_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iProlineID = 0;
        if (ddlProductionLines.SelectedIndex == 0)
        {
            return;
        }
        iProlineID = Convert.ToInt32(ddlProductionLines.SelectedValue);
        int iEmployeeID = Convert.ToInt32(lbAssignedEmployees.SelectedValue);
        RemoveSingleFunction(iProlineID, iEmployeeID);
    }
    protected void btnRemoveAll_Click(object sender, EventArgs e)
    {
        lblError.Text = "";       
        int iProlineID = 0;
        if (ddlProductionLines.SelectedIndex == 0)
        {
            return;
        }
        iProlineID = Convert.ToInt32(ddlProductionLines.SelectedValue);

        //Delete all items in Assigned listbox...
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

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
                        lblError.Text += "**You can't remove Employee: <font color='blue'>" + SharedFunctions.GetEmployeeName(iEmployeeID) + "</font> they have hours recorded while working on the selected production line!<br/>";
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

    protected void lbnSelectAllAssign_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAvailableEmployees.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearAssign_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAvailableEmployees.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    protected void lbnSelectAllAssigned_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAssignedEmployees.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearAssigned_Click(object sender, EventArgs e)
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

}