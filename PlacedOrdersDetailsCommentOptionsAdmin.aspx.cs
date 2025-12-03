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

public partial class PlacedOrdersDetailsCommentOptionsAdmin : System.Web.UI.Page
{
    #region Subs
    private void LoadCommentOptionsList(string sSearch)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            sSearch = sSearch.ToUpper();
            lbCommentOptions.Items.Clear();
            var query = (from o in db.SorPlacedOrdersNotesCommentOptions
                         orderby o.CommentOption
                         where
                          (o.CommentOption.ToUpper().Contains(sSearch) || sSearch == null)
                          
                         select new
                         {
                             o.CommentOption,
                             o.SorPlacedOrdersNotesCommentOptionsID,
                         });

            foreach (var a in query)
            {
                lbCommentOptions.Items.Add(new ListItem(a.CommentOption + " - #" + a.SorPlacedOrdersNotesCommentOptionsID.ToString(), a.SorPlacedOrdersNotesCommentOptionsID.ToString()));
            }
        }
    }
    private void BindProfile(int iSorPlacedOrdersNotesCommentOptionsID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            var query = (from s in db.SorPlacedOrdersNotesCommentOptions
                         where s.SorPlacedOrdersNotesCommentOptionsID == iSorPlacedOrdersNotesCommentOptionsID
                         select s);
            foreach (var a in query)
            {
                txtCommentOptions.Text = a.CommentOption;               
            }
        }
    }
    private void AddUserProfile()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";

            string sCommentOption = txtCommentOptions.Text.Trim();


            //Check to see if username already exists
            if (DoesCommentOptionExist(sCommentOption))
            {
                sMsg += "**Comment option already exists, please use another one!<br/>";
            }

            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }


            try
            {
                SorPlacedOrdersNotesCommentOptions u = new SorPlacedOrdersNotesCommentOptions();

                u.CommentOption = sCommentOption.ToUpper(); 
                u.DateAdded = DateTime.Now;

                db.SorPlacedOrdersNotesCommentOptions.InsertOnSubmit(u);
                db.SubmitChanges();

                lblError.Text = "**Comment Option Added successfully!";
                lblError.ForeColor = Color.Green;

                Reset();
            }
            catch (Exception ex)
            {
                lblError.Text = "**Comment Option Added failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void UpdateProfile(int iSorPlacedOrdersNotesCommentOptionsID)
    {
        if (Page.IsValid == false)
        {
            return;
        }
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            string sCommentOption = txtCommentOptions.Text.Trim();

            if (DoesCommentOptionExistForUpdate(iSorPlacedOrdersNotesCommentOptionsID, sCommentOption))
            {
                sMsg += "**Comment Option already exists, please use another one!<br/>";
            }
            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }


            try
            {

                SorPlacedOrdersNotesCommentOptions u = db.SorPlacedOrdersNotesCommentOptions.Single(p => p.SorPlacedOrdersNotesCommentOptionsID == iSorPlacedOrdersNotesCommentOptionsID);
                u.CommentOption = sCommentOption.ToUpper();
                db.SubmitChanges();
                LoadCommentOptionsList(txtSearch.Text.Trim());

                //lbComment Option.SelectedValue = iSorPlacedOrdersNotesCommentOptionsID.ToString();


                BindProfile(iSorPlacedOrdersNotesCommentOptionsID);

                lblError.Text = "**Comment Option updated successfully!";
                lblError.ForeColor = Color.Green;

            }
            catch (Exception ex)
            {
                lblError.Text = "**Comment Option updated failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void DeleteProfile(int iSorPlacedOrdersNotesCommentOptionsID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        try
        {
            SorPlacedOrdersNotesCommentOptions u = db.SorPlacedOrdersNotesCommentOptions.Single(p => p.SorPlacedOrdersNotesCommentOptionsID == iSorPlacedOrdersNotesCommentOptionsID);
            db.SorPlacedOrdersNotesCommentOptions.DeleteOnSubmit(u);
            db.SubmitChanges();

            lblError.Text = "**Comment Option Deleted successfully!";
            lblError.ForeColor = Color.Green;

            Reset();

            LoadCommentOptionsList(txtSearch.Text.Trim());
        }
        catch (Exception ex)
        {
            lblError.Text = "**Comment Option Delete failed!(You can't delete an Comment Option who is associated with another table! i.e. Orders)";
            lblError.ForeColor = Color.Red;
            Debug.WriteLine(ex.ToString());
        }
    }
    private void Reset()
    {
        txtCommentOptions.Text = "";
    }

    #endregion

    #region Functions
    private bool DoesCommentOptionExist(string sCommentOption)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from u in db.SorPlacedOrdersNotesCommentOptions
                         where u.CommentOption == sCommentOption
                         select u);
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
    private bool DoesCommentOptionExistForUpdate(int iSorPlacedOrdersNotesCommentOptionsID, string sCommentOption)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from u in db.SorPlacedOrdersNotesCommentOptions
                         where u.CommentOption == sCommentOption
                         && u.SorPlacedOrdersNotesCommentOptionsID != iSorPlacedOrdersNotesCommentOptionsID
                         select u);
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

            ibnAdd.Enabled = false;
            ibnDelete.Enabled = false;
            ibnSave.Enabled = false;
            LoadCommentOptionsList(txtSearch.Text.Trim());
        }

    }
    protected void ibnSave_Click(object sender, EventArgs e)
    {
        //Save Profile...
        lblError.Text = "";

        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (lbCommentOptions.SelectedIndex == -1)
        {
            lblError.Text = "**Please selected a Comment Option.";
            lblError.ForeColor = Color.Red;
            return;
        }

        int iSorPlacedOrdersNotesCommentOptionsID = Convert.ToInt32(lbCommentOptions.SelectedValue);
        UpdateProfile(iSorPlacedOrdersNotesCommentOptionsID);
    }
    protected void ibnAdd_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (Page.IsValid == false)
        {
            return;
        }

        AddUserProfile();
    }
    protected void ibnDelete_Click(object sender, EventArgs e)
    {//Delete Profile...
        lblError.Text = "";

        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (lbCommentOptions.SelectedIndex == 0)
        {
            lblError.Text = "**Please selected a Comment Option.";
            lblError.ForeColor = Color.Red;
            return;
        }

        int iSorPlacedOrdersNotesCommentOptionsID = Convert.ToInt32(lbCommentOptions.SelectedValue);
        DeleteProfile(iSorPlacedOrdersNotesCommentOptionsID);
    }
    protected void ibnSearch_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        LoadCommentOptionsList(txtSearch.Text.Trim());
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        LoadCommentOptionsList(txtSearch.Text.Trim());
    }
    protected void lbCommentOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iSorPlacedOrdersNotesCommentOptionsID = 0;
        if (lbCommentOptions.SelectedIndex != -1)
        {
            iSorPlacedOrdersNotesCommentOptionsID = Convert.ToInt32(lbCommentOptions.SelectedValue);
            ibnSave.Enabled = true;
            ibnDelete.Enabled = true;
            BindProfile(iSorPlacedOrdersNotesCommentOptionsID);
        }
        else
        {
            Reset();
            ibnSave.Enabled = false;
            ibnDelete.Enabled = false;
        }
    }
    protected void rblMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iSorPlacedOrdersNotesCommentOptionsID = 0;
        if (rblMode.SelectedIndex == 0)//Add...
        {
            Reset();
            ibnAdd.Enabled = true;
            ibnSave.Enabled = false;
            ibnDelete.Enabled = false;

            lbCommentOptions.Visible = false;
            txtSearch.Visible = false;
            ibnSearch.Visible = false;

        }
        else//Edit...
        {
            LoadCommentOptionsList(txtSearch.Text.Trim());
            ibnAdd.Enabled = false;
            lbCommentOptions.Visible = true;
            txtSearch.Visible = true;
            ibnSearch.Visible = true;
            if (lbCommentOptions.SelectedIndex != -1)
            {
                iSorPlacedOrdersNotesCommentOptionsID = Convert.ToInt32(lbCommentOptions.SelectedValue);
                ibnSave.Enabled = true;
                ibnDelete.Enabled = true;
                BindProfile(iSorPlacedOrdersNotesCommentOptionsID);
            }
            else
            {
                Reset();
                ibnSave.Enabled = false;
                ibnDelete.Enabled = false;
            }

        }
    }
    protected void cblStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCommentOptionsList(txtSearch.Text.Trim());
    }

    #endregion

    #region WebMethods
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListCommentOption(string prefixText, int count, string contextKey)
    {//...
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {

            // Your LINQ to SQL query goes here 
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            string[] list = db.SorPlacedOrdersNotesCommentOptions.Where(w => w.CommentOption != null).OrderBy(w => w.CommentOption).Select(w => (w.CommentOption)).Distinct().ToArray();
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


    #endregion
}