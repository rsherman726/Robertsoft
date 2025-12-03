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
using AjaxControlToolkit;
using System.Reflection;

public partial class GroupsAdmin : System.Web.UI.Page
{
    #region Subs
    private void LoadGroupsList()
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from d in db.ArGroups
                         orderby d.GroupName
                         select new
                         {
                             d.GroupName,
                             d.GroupID,
                         });
            dt = SharedFunctions.ToDataTable(db, query);

            if (ddlDisplayCount.SelectedValue != "ALL")
            {
                gvGroups.PageSize = Convert.ToInt32(ddlDisplayCount.SelectedValue);
            }
            else
            {
                gvGroups.PageSize = dt.Rows.Count;
            }

            gvGroups.DataSource = dt;
            gvGroups.DataBind();


            Session["dtGroups"] = dt;
            dt.Dispose();
        }
    }
    private void LoadGroupNames(DropDownList ddl)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddl.Items.Clear();
            var query = (from r in db.ArGroups
                         orderby r.GroupName
                         select new
                         {
                             r.GroupName,
                             r.GroupID
                         });
            foreach (var a in query)
            {
                lbGroups.Items.Add(new ListItem(a.GroupName, a.GroupID.ToString()));
            }
            rsListbox.Sort(ref ddl, rsListbox.SortOrder.Ascending);
            ddl.Items.Insert(0, new ListItem("SELECT", "0"));
        }
    }
    private void LoadGroups()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbGroups.Items.Clear();
            var query = (from r in db.ArGroups
                         orderby r.GroupName
                         select new
                         {
                             r.GroupName,
                             r.GroupID
                         });
            foreach (var a in query)
            {
                lbGroups.Items.Add(new ListItem(a.GroupName, a.GroupID.ToString()));
            }
        }
    }
    private void LoadAssignedUsers(int iGroupID)
    {

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbAssigned.Items.Clear();
            var query = (from r in db.ArGroupAssignments
                         join u in db.ArCustomer on r.Customer equals u.Customer
                         where
                           r.GroupID == iGroupID
                         orderby u.Name
                         select new
                         {
                             u.Customer,
                             u.Name,

                         });
            foreach (var a in query)
            {

                lbAssigned.Items.Add(new ListItem(a.Name + " - " + a.Customer, a.Customer));
            }
        }
    }
    private void LoadAvailableUsers()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbAvailable.Items.Clear();
            var query = (

                             from u in db.ArCustomer
                             orderby u.Name
                             where  
                               !
                                 (from r in db.ArGroupAssignments
                                  select new
                                  {
                                      r.Customer
                                  }).Contains(new { Customer = u.Customer })
                             select new
                             {
                                 u.Name,
                                 u.Customer
                             });
            foreach (var a in query)
            {
                
                lbAvailable.Items.Add(new ListItem(a.Name + " - " + a.Customer, a.Customer));
            }
        }
    }
    private void AddSingleFunction(string sName, int iGroupID, string sCustomer)
    {

        try
        {
            ListBox lbNoDups = new ListBox();
            this.lbAssigned.Items.Add(new ListItem(sName, sCustomer.ToString()));

            SharedFunctions.RemoveDupsFromListBox(lbAssigned);
            FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

            if (!RecordAlreadyinTable(iGroupID, sCustomer))
            {
                ArGroupAssignments ea = new ArGroupAssignments();
                ea.GroupID = iGroupID;
                ea.Customer = sCustomer;
                ea.DateAdded = DateTime.Now;
                db.ArGroupAssignments.InsertOnSubmit(ea);                
                db.SubmitChanges();
            }

            rsListbox.Sort(ref lbNoDups, rsListbox.SortOrder.Ascending);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }

    }
    private void RemoveSingleFunction(string sCustomer, int iGroupID)
    {

        try
        {


            if (lbAssigned.Items.Count == 0) { return; }
            if (lbAssigned.SelectedIndex == -1) { return; }

            // Remove item(s) from Available listbox...
            foreach (ListItem li in lbAvailable.Items)
            {
                if (sCustomer ==li.Value)
                {
                    lbAssigned.Items.RemoveAt(lbAssigned.SelectedIndex);//remove from available items list box...
                }
            }
            //Remove Dups...
            SharedFunctions.RemoveDupsFromListBox(lbAssigned);
            FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            //remove agent from group assignments...
            int iGroupAssignmentID = db.ArGroupAssignments.Where(p => p.GroupID == iGroupID && p.Customer == sCustomer).FirstOrDefault().GroupAssignmentID;
            ArGroupAssignments ea = db.ArGroupAssignments.Single(p => p.GroupAssignmentID == iGroupAssignmentID);
            db.ArGroupAssignments.DeleteOnSubmit(ea);            
            db.SubmitChanges();

            LoadAssignedUsers(iGroupID);
            LoadAvailableUsers();

            rsListbox.Sort(ref lbAssigned, rsListbox.SortOrder.Ascending);
            // rsListbox.Sort(ref ddlDealsAdd, rsListbox.SortOrder.Ascending);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    private void AddGroup()
    {
        lblError.Text = "";
        string sMsg = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            


           string  sName = txtGroupName.Text.Trim().ToUpper();
            //Validate data...
            if (sName=="")
            {
                sMsg += "**Please enter a Group Name!<br/>";
            }

            if (GroupNameExists(sName))
            {
                sMsg += "** Groups Name already exists!<br/>";
            }
            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }
            //Add...
            try
            {

                ArGroups dl = new ArGroups();

                dl.GroupName = sName;
                dl.DateAdded = DateTime.Now;
                db.ArGroups.InsertOnSubmit(dl);

                db.SubmitChanges();

                lblError.Text = "**Add was successful!";
                lblError.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblError.Text = "**Add Failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }

            //Refresh Grid...       
            LoadGroupsList();//Refresh...   
            LoadGroups();
        }
    }
    #endregion

    #region Functions

    private bool GroupNameExists(string sName)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.ArGroups
                         where c.GroupName == sName
                         select c);
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
    private bool GroupNameExistsForUpdate(string sName, int iGroupID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.ArGroups
                         where c.GroupName == sName
                         && c.GroupID != iGroupID
                         select c);
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
    private bool RecordAlreadyinTable(int iGroupID, string sCustomer)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from ea in db.ArGroupAssignments
                         where ea.GroupID == iGroupID
                         && ea.Customer == sCustomer
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
            LoadGroupsList();
            lblPageNo.Text = "Current Page #: 1";
            LoadGroups();
        }
    }
    protected void gvGroups_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvGroups.EditIndex != -1)//In Edit Mode...
                {

                    if (gvGroups.EditIndex == e.Row.RowIndex)//edited row...
                    {



                        //Label lblGroupName = (Label)e.Row.FindControl("lblGroupName");
                        //DropDownList ddlGroupName = (DropDownList)e.Row.FindControl("ddlGroupName");
                        //LoadGroupNames(ddlGroupName);
                        //ddlGroupName.SelectedValue = lblGroupName.Text;


                    }
                    else if (gvGroups.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
                    {



                    }
                }
                else//Not Edit Mode...
                {

                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)//the footer row...
            { 
 

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvGroups_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvGroups.EditIndex = e.NewEditIndex;
        gvGroups.DataSource = (DataTable)Session["dtGroups"];
        gvGroups.DataBind();

    }
    protected void gvGroups_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvGroups.EditIndex = -1;
        gvGroups.DataSource = (DataTable)Session["dtGroups"];
        gvGroups.DataBind();
    }
    protected void gvGroups_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvGroups.EditIndex = -1;
        LoadGroupsList();
    }
    protected void gvGroups_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvGroups.EditIndex = -1;
        LoadGroupsList();
    }
    protected void gvGroups_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvGroups.PageIndex = e.NewPageIndex;
        lblPageNo.Text = "Current Page #: " + (gvGroups.PageIndex + 1).ToString();
        gvGroups.DataSource = (DataTable)Session["dtGroups"];
        gvGroups.DataBind();
    }
    protected void gvGroups_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sMsg = "";
        lblError.Text = "";
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        int iUserID = 0;

        int i = 0;
        Label lblGroupID;         
        TextBox txtGroupName;

        string sName = "";
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        iUserID = Convert.ToInt32(Session["UserID"]);


        switch (e.CommandName)
        {
            case "Add":
               
                break;
            case "Update":
                lblError.Text = "";

                i = Convert.ToInt32(e.CommandArgument);
                lblError.Text = "";

                txtGroupName = (TextBox)gvGroups.Rows[i].FindControl("txtGroupName");
                lblGroupID = (Label)gvGroups.Rows[i].FindControl("lblGroupID");
                sName = txtGroupName.Text.Trim().ToUpper();
                //Validate data...
                if (sName == "")
                {
                    sMsg += "**Please enter in Group Name Name!<br/>";
                }

                if (GroupNameExistsForUpdate(sName, Convert.ToInt32(lblGroupID.Text)))
                {
                    sMsg += "** Groups Name already exists!<br/>";
                }
                if (sMsg.Length > 0)
                {
                    lblError.Text = sMsg;
                    lblError.ForeColor = Color.Red;
                    return;
                }
                //Update...
                try
                {
                    int iPrimaryKeyID = Convert.ToInt32(lblGroupID.Text);
                    ArGroups dl = db.ArGroups.Single(p => p.GroupID == iPrimaryKeyID);
                    dl.GroupName = sName;                     
                    db.SubmitChanges();

                    LoadGroups();

                    lblError.Text = "**Update was successful!";
                    lblError.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    lblError.Text = "**Update Failed!";
                    lblError.ForeColor = Color.Red;
                    Debug.WriteLine(ex.ToString());
                }
                break;

            case "Delete":
                i = Convert.ToInt32(e.CommandArgument);

                try
                {
                    lblGroupID = (Label)gvGroups.Rows[i].FindControl("lblGroupID");

                    var query = (from d in db.ArGroups select d);
                    if (query.Count() == 1)
                    {
                        lblError.Text = "**Cannot delete the last record in the table.";
                        lblError.ForeColor = Color.Red;
                        return;
                    }

                    int iPrimaryKeyID = Convert.ToInt32(lblGroupID.Text);
                    ArGroups dl = db.ArGroups.Single(p => p.GroupID == iPrimaryKeyID);
                    db.ArGroups.DeleteOnSubmit(dl);                    
                    db.SubmitChanges();

                    LoadGroups();

                    lblError.Text = "**Delete was successful!";
                    lblError.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    lblError.Text = "**Delete Failed! You can't delete an GroupName that is associated with a Group Assignment!";
                    lblError.ForeColor = Color.Red;
                    Debug.WriteLine(ex.ToString());
                }
                break;
        }
    }
    protected void ddlDisplayCount_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGroupsList();

    }
    //Members
    protected void lbGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        lbAssigned.Items.Clear();
        int iGroupID = 0;

        if (lbGroups.SelectedIndex == -1)
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
            iGroupID = Convert.ToInt32(lbGroups.SelectedValue);
            LoadAvailableUsers();
            LoadAssignedUsers(iGroupID);
            if (lbAssigned.Items.Count > 0)
            {
                btnRemoveAll.Enabled = true;
                btnRemoveOne.Enabled = true;
            }
            else
            {
                btnRemoveAll.Enabled = false;
                btnRemoveOne.Enabled = false;
            }
            if (lbAvailable.Items.Count > 0)
            {
                btnAddAll.Enabled = true;
                btnAddOne.Enabled = true;
            }
            else
            {
                btnAddAll.Enabled = false;
                btnAddOne.Enabled = false;
            }
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
        if (lbAvailable.SelectedIndex == -1)
        {
            lblError.Text = "**You have not selected a Users(s) to add, try again.";
            return;
        }
        if (lbAvailable.Items.Count == 0) { return; }
        if (lbAvailable.SelectedIndex == -1) { return; }

        bool bSelected = false;
        foreach (ListItem li in lbAvailable.Items)
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
            lblError.Text = "**You have not selected a Users(s)!";
            return;
        }
        else
        {
            lblError.Text = "";
        }

        string sCustomer =lbAvailable.SelectedValue;
        string sName = lbAvailable.SelectedItem.Text;
        int iGroupID = 0;
        if (lbGroups.SelectedIndex == -1)
        {
            return;
        }
        iGroupID = Convert.ToInt32(lbGroups.SelectedValue);


        AddSingleFunction(sName, iGroupID, sCustomer);
        LoadAssignedUsers(iGroupID);
        LoadAvailableUsers();
        if (lbAssigned.Items.Count > 0)
        {
            btnRemoveAll.Enabled = true;
            btnRemoveOne.Enabled = true;
        }
        else
        {
            btnRemoveAll.Enabled = false;
            btnRemoveOne.Enabled = false;
        }
        if (lbAvailable.Items.Count > 0)
        {
            btnAddAll.Enabled = true;
            btnAddOne.Enabled = true;
        }
        else
        {
            btnAddAll.Enabled = false;
            btnAddOne.Enabled = false;
        }

    }
    protected void btnAddAll_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iUserID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        int iGroupID = 0;
        if (lbGroups.SelectedIndex == -1)
        {
            lblError.Text = "**You have not selected a Group, try again.";
            return;
        }
        iGroupID = Convert.ToInt32(lbGroups.SelectedValue);
        if (lbAvailable.SelectedIndex == -1)
        {
            lblError.Text = "**You have not selected a Underwriter(s) to Add, try again.";
            return;
        }
        try
        {
            ListItemCollection lsc = new ListItemCollection();
            for (int i = 0; i < lbAvailable.Items.Count; i++)
            {
                if (lbAvailable.Items[i].Selected == true)
                {
                    string value = lbAvailable.Items[i].Value;
                    string text = lbAvailable.Items[i].Text;
                    ListItem lst = new ListItem();
                    lst.Text = text;
                    lst.Value = value;
                    lbAssigned.Items.Add(lst);
                    lsc.Add(lst);
                }
            }
            foreach (ListItem ls in lsc)
            {

            }
            //Remove Dups...
            SharedFunctions.RemoveDupsFromListBox(lbAssigned);

            //Add all items in Assigned listbox...
            FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            string sCustomer = "";
            foreach (ListItem li in lbAssigned.Items)
            {
                sCustomer = li.Value;
                //Add Single manager to job Assignments...
                if (!RecordAlreadyinTable(iGroupID, sCustomer))
                {
                    ArGroupAssignments ea = new ArGroupAssignments();
                    ea.GroupID = iGroupID;
                    ea.Customer = sCustomer;
                    ea.DateAdded = DateTime.Now;
                    db.ArGroupAssignments.InsertOnSubmit(ea);
                    
                    db.SubmitChanges();
                }
            }
            LoadAssignedUsers(iGroupID);
            LoadAvailableUsers();
            rsListbox.Sort(ref lbAssigned, rsListbox.SortOrder.Ascending);

            if (lbAssigned.Items.Count > 0)
            {
                btnRemoveAll.Enabled = true;
                btnRemoveOne.Enabled = true;
            }
            else
            {
                btnRemoveAll.Enabled = false;
                btnRemoveOne.Enabled = false;
            }
            if (lbAvailable.Items.Count > 0)
            {
                btnAddAll.Enabled = true;
                btnAddOne.Enabled = true;
            }
            else
            {
                btnAddAll.Enabled = false;
                btnAddOne.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void btnRemoveOne_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iGroupID = 0;
        if (lbGroups.SelectedIndex == -1)
        {
            return;
        }
        iGroupID = Convert.ToInt32(lbGroups.SelectedValue);
        string sCustomer = lbAssigned.SelectedValue;
        RemoveSingleFunction(sCustomer, iGroupID);

        if (lbAssigned.Items.Count > 0)
        {
            btnRemoveAll.Enabled = true;
            btnRemoveOne.Enabled = true;
        }
        else
        {
            btnRemoveAll.Enabled = false;
            btnRemoveOne.Enabled = false;
        }
        if (lbAvailable.Items.Count > 0)
        {
            btnAddAll.Enabled = true;
            btnAddOne.Enabled = true;
        }
        else
        {
            btnAddAll.Enabled = false;
            btnAddOne.Enabled = false;
        }
    }
    protected void btnRemoveAll_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iGroupID = 0;
        if (lbGroups.SelectedIndex == -1)
        {
            return;
        }
        iGroupID = Convert.ToInt32(lbGroups.SelectedValue);

        //Delete all items in Assigned listbox...
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        try
        {
            string sCustomer = "";
            foreach (ListItem li in lbAssigned.Items)
            {
                sCustomer = li.Value;
                //Delete Single Agent from Group Assignments...
                if (li.Selected)
                {
                    int iGroupAssignmentID = db.ArGroupAssignments.Where(p => p.GroupID == iGroupID && p.Customer == sCustomer).FirstOrDefault().GroupAssignmentID;
                    ArGroupAssignments ea = db.ArGroupAssignments.Single(p => p.GroupAssignmentID == iGroupAssignmentID);
                    db.ArGroupAssignments.DeleteOnSubmit(ea);
                    
                    db.SubmitChanges();
                }

            }
            LoadAssignedUsers(iGroupID);
            LoadAvailableUsers();

            if (lbAssigned.Items.Count > 0)
            {
                btnRemoveAll.Enabled = true;
                btnRemoveOne.Enabled = true;
            }
            else
            {
                btnRemoveAll.Enabled = false;
                btnRemoveOne.Enabled = false;
            }
            if (lbAvailable.Items.Count > 0)
            {
                btnAddAll.Enabled = true;
                btnAddOne.Enabled = true;
            }
            else
            {
                btnAddAll.Enabled = false;
                btnAddOne.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void lbnSelectAllAssign_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAvailable.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearAssign_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAvailable.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    protected void lbnSelectAllAssigned_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAssigned.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearAssigned_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAssigned.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    protected void lbnAdd_Click(object sender, EventArgs e)
    {
        AddGroup();
    }

    #endregion


}