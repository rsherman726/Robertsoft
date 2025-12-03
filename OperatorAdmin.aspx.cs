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

public partial class OperatorAdmin : System.Web.UI.Page
{
    #region Subs
    private void LoadUserList(string sSearch, List<string> lStatus)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            sSearch = sSearch.ToUpper();
            lbOperators.Items.Clear();
            var query = (from o in db.AdmOperator_
                         orderby o.FullName
                         where
                          (o.FullName.ToUpper().Contains(sSearch) || o.Operator.ToUpper().Contains(sSearch) || sSearch == null)
                          && lStatus.Contains(o.Status.ToString())
                         select new
                         {
                             o.Operator,
                             o.FullName,

                         });

            foreach (var a in query)
            {
                lbOperators.Items.Add(new ListItem(a.FullName + " - " + a.Operator, a.Operator));
            }
        }
    }
    private void BindProfile(string sOperator)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            var query = (from users in db.AdmOperator_
                         where users.Operator == sOperator
                         select users);
            foreach (var a in query)
            {
                txtFullname.Text = a.FullName;
                txtOperator.Text = a.Operator;
                txtEmail.Text = a.Email;
                if (a.Status == 1)
                {//Active
                    ddlStatus.SelectedIndex = 0;
                }
                else
                {//Not active...
                    ddlStatus.SelectedIndex = 1;
                }
            }
        }
    }
    private void AddUserProfile()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            string sFullName = txtFullname.Text.Trim();
            string sOperator = txtOperator.Text.Trim();
            string sEmail = txtEmail.Text.Trim();

            //Check to see if username already exists
            if (DoesOperatorExist(sOperator))
            {
                sMsg += "**Operator already exists, please use another one!<br/>";
            }

            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }


            try
            {
                AdmOperator_ u = new AdmOperator_();
                u.FullName = sFullName;
                u.Operator = sOperator;

                if (sEmail != "")
                {
                    u.Email = sEmail;
                }
                else
                {
                    u.Email = null;
                }
                if (ddlStatus.SelectedIndex == 0)
                {//Active...
                    u.Status = 1;
                }
                else//Not Active...
                {
                    u.Status = 0;
                }

                u.DateAdded = DateTime.Now;

                db.AdmOperator_.InsertOnSubmit(u);
                db.SubmitChanges();

                lblError.Text = "**Operator Added successfully!";
                lblError.ForeColor = Color.Green;

                Reset();
            }
            catch (Exception ex)
            {
                lblError.Text = "**Operator Added failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void UpdateProfile(string sOperator)
    {
        if (Page.IsValid == false)
        {
            return;
        }
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            string sFullName = txtFullname.Text.Trim();
            string sEmail = txtEmail.Text.Trim();

            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }


            try
            {

                AdmOperator_ u = db.AdmOperator_.Single(p => p.Operator == sOperator);
                u.FullName = sFullName;
                if (ddlStatus.SelectedIndex == 0)
                {//Active...
                    u.Status = 1;
                }
                else//Not Active...
                {
                    u.Status = 0;
                }
                if (txtEmail.Text.Trim() != "")
                {
                    u.Email = sEmail;
                }
                else
                {
                    u.Email = null;
                }
                db.SubmitChanges();

                List<string> lStatus = new List<string>();
                foreach (ListItem li in cblStatus.Items)
                {
                    if (li.Selected)
                    {
                        lStatus.Add(li.Value);
                    }
                }
                LoadUserList(txtSearch.Text.Trim(), lStatus);

                foreach (ListItem li in lbOperators.Items)
                {
                    if (li.Value == sOperator)
                    {
                        lbOperators.SelectedValue = sOperator;
                    }
                }


                BindProfile(sOperator);

                lblError.Text = "**Operator updated successfully!";
                lblError.ForeColor = Color.Green;

            }
            catch (Exception ex)
            {
                lblError.Text = "**Operator updated failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void DeleteProfile(string sOperator)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        try
        {
            AdmOperator_ u = db.AdmOperator_.Single(p => p.Operator == sOperator);
            db.AdmOperator_.DeleteOnSubmit(u);
            db.SubmitChanges();

            lblError.Text = "**Operator Deleted successfully!";
            lblError.ForeColor = Color.Green;

            Reset();

            List<string> lStatus = new List<string>();
            foreach (ListItem li in cblStatus.Items)
            {
                if (li.Selected)
                {
                    lStatus.Add(li.Value);
                }
            }
            LoadUserList(txtSearch.Text.Trim(), lStatus);
        }
        catch (Exception ex)
        {
            lblError.Text = "**Operator Delete failed!(You can't delete an Operator who is associated with another table! i.e. Orders)";
            lblError.ForeColor = Color.Red;
            Debug.WriteLine(ex.ToString());
        }
    }
    private void Reset()
    {
        txtFullname.Text = "";
        txtOperator.Text = "";
        txtEmail.Text = "";
    }

    #endregion

    #region Functions
    private bool DoesOperatorExist(string sOperator)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from u in db.AdmOperator_
                         where u.Operator == sOperator
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
            List<string> lStatus = new List<string>();
            foreach (ListItem li in cblStatus.Items)
            {
                if (li.Selected)
                {
                    lStatus.Add(li.Value);
                }
            }
            LoadUserList(txtSearch.Text.Trim(), lStatus);
        }

    }
    protected void ibnSave_Click(object sender, EventArgs e)
    {
        //Save Profile...
        lblError.Text = "";
        string sOperator = "";
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (lbOperators.SelectedIndex == -1)
        {
            lblError.Text = "**Please selected a Operator.";
            lblError.ForeColor = Color.Red;
            return;
        }

        sOperator = lbOperators.SelectedValue;
        UpdateProfile(sOperator);
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
        string sOperator = "";
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (lbOperators.SelectedIndex == 0)
        {
            lblError.Text = "**Please selected a Operator.";
            lblError.ForeColor = Color.Red;
            return;
        }

        sOperator = lbOperators.SelectedValue;
        DeleteProfile(sOperator);
    }
    protected void ibnSearch_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        List<string> lStatus = new List<string>();
        foreach (ListItem li in cblStatus.Items)
        {
            if (li.Selected)
            {
                lStatus.Add(li.Value);
            }
        }
        LoadUserList(txtSearch.Text.Trim(), lStatus);
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        List<string> lStatus = new List<string>();
        foreach (ListItem li in cblStatus.Items)
        {
            if (li.Selected)
            {
                lStatus.Add(li.Value);
            }
        }
        LoadUserList(txtSearch.Text.Trim(), lStatus);
    }
    protected void lbOperators_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        string sOperator = "";
        if (lbOperators.SelectedIndex != -1)
        {
            sOperator = lbOperators.SelectedValue;
            ibnSave.Enabled = true;
            ibnDelete.Enabled = true;
            BindProfile(sOperator);
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
        string sOperator = "";
        if (rblMode.SelectedIndex == 0)//Add...
        {
            Reset();
            ibnAdd.Enabled = true;
            ibnSave.Enabled = false;
            ibnDelete.Enabled = false;

            lbOperators.Visible = false;
            txtSearch.Visible = false;
            ibnSearch.Visible = false;
            txtOperator.Enabled = true;
            txtOperator.ReadOnly = false;
            lblView.Visible = false;
            cblStatus.Visible = false;
        }
        else//Edit...
        {
            List<string> lStatus = new List<string>();
            foreach (ListItem li in cblStatus.Items)
            {
                if (li.Selected)
                {
                    lStatus.Add(li.Value);
                }
            }
            LoadUserList(txtSearch.Text.Trim(), lStatus);
            ibnAdd.Enabled = false;
            lbOperators.Visible = true;
            txtSearch.Visible = true;
            ibnSearch.Visible = true;
            txtOperator.ReadOnly = true;
            lblView.Visible = true;
            cblStatus.Visible = true;
            if (lbOperators.SelectedIndex != -1)
            {
                sOperator = lbOperators.SelectedValue;
                ibnSave.Enabled = true;
                ibnDelete.Enabled = true;
                BindProfile(sOperator);
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
        List<string> lStatus = new List<string>();
        foreach (ListItem li in cblStatus.Items)
        {
            if (li.Selected)
            {
                lStatus.Add(li.Value);
            }
        }
        LoadUserList(txtSearch.Text.Trim(), lStatus);
    }

    #endregion

    #region WebMethods
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListUserName(string prefixText, int count, string contextKey)
    {//...
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {

            // Your LINQ to SQL query goes here 
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            string[] list = db.AdmOperator_.Where(w => w.FullName != null).OrderBy(w => w.FullName).Select(w => (w.FullName)).Distinct().ToArray();
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