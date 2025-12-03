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

public partial class MiscEmailsAdmin : System.Web.UI.Page
{
    #region Subs
    private void LoadWipContacts()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = new DataTable();

            var query = (from d in db.WipContacts
                         orderby
                           d.Email ascending
                         select new
                         {

                             d.ContactID,
                             d.Email,                              
                         });
            dt = SharedFunctions.ToDataTable(db, query);
            gvMiscEmails.DataSource = dt;
            gvMiscEmails.DataBind();
            Session["dtWipContacts"] = dt;
            dt.Dispose();
        }
    }


    #endregion

    #region Functions

    private bool EmailExists(string sEmail)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.WipContacts
                         where c.Email == sEmail                         
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
    private bool EmailExistsForUpdate(string sEmail, int iContactID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.WipContacts
                         where c.Email == sEmail                          
                         && c.ContactID != iContactID
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
            if (!SharedFunctions.IsAccessGranted(iAdminID, sText))
            {
                Response.Redirect("Default.aspx");
            }
        }
        if (!Page.IsPostBack)
        {
            LoadWipContacts();
             
        }
    }
    protected void gvMiscEmails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (gvMiscEmails.EditIndex != -1)//In Edit Mode...
            {

                if (gvMiscEmails.EditIndex == e.Row.RowIndex)//edited row...
                {

                    

                }
                else if (gvMiscEmails.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
                {
                    
                }

            }
            else
            {
                 
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)//the footer row...
        {

        }
    }
    protected void gvMiscEmails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvMiscEmails.EditIndex = e.NewEditIndex;
        gvMiscEmails.DataSource = (DataTable)Session["dtWipContacts"];
        gvMiscEmails.DataBind();

    }
    protected void gvMiscEmails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvMiscEmails.EditIndex = -1;
        gvMiscEmails.DataSource = (DataTable)Session["dtWipContacts"];
        gvMiscEmails.DataBind();
    }
    protected void gvMiscEmails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvMiscEmails.EditIndex = -1;
        LoadWipContacts();
    }
    protected void gvMiscEmails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvMiscEmails.EditIndex = -1;
        LoadWipContacts();
    }
    protected void gvMiscEmails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sMsg = "";
        lblError.Text = "";
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        int iUserID = 0;
        int iContactID = 0;

        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        int i = 0;
        Label lblContactID; 
        TextBox txtEmail; 
        TextBox txtEmailAdd;

        string sEmail = "";

        switch (e.CommandName)
        {
            case "Add":
                lblError.Text = "";
                 
                txtEmailAdd = (TextBox)gvMiscEmails.FooterRow.FindControl("txtEmailAdd");
                sEmail = txtEmailAdd.Text.Trim();                 
                //Validate data...
                if (txtEmailAdd.Text.Trim() == "")
                {
                    sMsg += "**Please fill in Email Name!<br/>";
                }
                if (EmailExists(txtEmailAdd.Text.Trim()))
                {
                    sMsg += "**Email with that date already exists for date entered!<br/>";
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
                    WipContacts dl = new WipContacts();
                    dl.Email = sEmail;                    
                    db.WipContacts.InsertOnSubmit(dl);
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
                LoadWipContacts();//Refresh...                
                break;
            case "Update":
                lblError.Text = "";

                i = Convert.ToInt32(e.CommandArgument);
                lblError.Text = "";
                 
                txtEmail = (TextBox)gvMiscEmails.Rows[i].FindControl("txtEmail");
                lblContactID = (Label)gvMiscEmails.Rows[i].FindControl("lblContactID");
                iContactID = Convert.ToInt32(lblContactID.Text);
                sEmail = txtEmail.Text.Trim(); 
                 
                //Validate data...
                if (txtEmail.Text.Trim() == "")
                {
                    sMsg += "**Please fill in Email Name!<br/>";
                }
                if (EmailExistsForUpdate(txtEmail.Text.Trim(), iContactID))
                {
                    sMsg += "**Email already exists with date entered!<br/>";
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
                    WipContacts dl = db.WipContacts.Single(p => p.ContactID == iContactID);                    
                    dl.Email = sEmail;
                    db.SubmitChanges();

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
                    lblContactID = (Label)gvMiscEmails.Rows[i].FindControl("lblContactID");
                    iContactID = Convert.ToInt32(lblContactID.Text);
                    var query = (from d in db.WipContacts select d);
                    if (query.Count() == 1)
                    {
                        lblError.Text = "**You cannot delete the last record in the table.";
                        lblError.ForeColor = Color.Red;
                        return;
                    }

                    WipContacts dl = db.WipContacts.Single(p => p.ContactID == iContactID);
                    db.WipContacts.DeleteOnSubmit(dl);
                    db.SubmitChanges();

                    lblError.Text = "**Delete was successful!";
                    lblError.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    lblError.Text = "**Delete Failed!";
                    lblError.ForeColor = Color.Red;
                    Debug.WriteLine(ex.ToString());
                }
                break;
        }
    }





    #endregion
}