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

public partial class EmployeeAdmin : System.Web.UI.Page
{
    #region Subs
    private void AddUserProfile(int iUserID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            string sFirstName = SharedFunctions.PCase(txtFirstName.Text.Trim());
            string sMiddleInt = SharedFunctions.PCase(txtMiddleInitial.Text.Trim());
            string sLastName = SharedFunctions.PCase(txtLastName.Text.Trim());
            string sStatus = ddlStatus.SelectedValue;



            if (ddlStatus.SelectedIndex == 0)
            {
                sMsg += "**Please select a Status!<br/>";
            }
            //Check to see if username already exists


            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }

            if (Session["UserID"] == null)
            {
                return;
            }


            try
            {


                WipEmployees u = new WipEmployees();
                u.FirstName = sFirstName;
                if (sMiddleInt != "")
                {
                    u.MiddleName = sMiddleInt;
                }
                else
                {
                    u.MiddleName = null;
                }

                u.LastName = sLastName;

                u.Status = Convert.ToInt32(sStatus);

                u.DateAdded = DateTime.Now;
                u.AddedBy = iUserID;

                db.WipEmployees.InsertOnSubmit(u);
                db.SubmitChanges();

                lblError.Text = "**Employees Added successfully!";
                lblError.ForeColor = Color.Green;

                Reset();
            }
            catch (Exception ex)
            {
                lblError.Text = "**Employees Added failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void UpdateProfile(int iUserID, int iSelectedEmployeeID)
    {
        if (Page.IsValid == false)
        {
            return;
        }
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sFirstName = SharedFunctions.PCase(txtFirstName.Text.Trim());
            string sMiddleInt = SharedFunctions.PCase(txtMiddleInitial.Text.Trim());
            string sLastName = SharedFunctions.PCase(txtLastName.Text.Trim());
            string sMsg = "";
            string sStatus = ddlStatus.SelectedValue;


            //Clean Phone...



            if (ddlStatus.SelectedIndex == 0)
            {
                sMsg += "**Please select a Status!<br/>";
            }

            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }

            if (Session["UserID"] == null)
            {
                return;
            }


            try
            {

                WipEmployees u = db.WipEmployees.Single(p => p.EmployeeID == iSelectedEmployeeID);
                u.FirstName = sFirstName;
                if (sMiddleInt != "")
                {
                    u.MiddleName = sMiddleInt;
                }
                else
                {
                    u.MiddleName = null;
                }

                u.LastName = sLastName;



                u.Status = Convert.ToInt32(sStatus);

                u.DateModified = DateTime.Now;
                u.ModifiedBy = iUserID;
                db.SubmitChanges();


                BindProfile(iSelectedEmployeeID);

                lblError.Text = "**Profile updated successfully!";
                lblError.ForeColor = Color.Green;

            }
            catch (Exception ex)
            {
                lblError.Text = "**Profile updated failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void DeleteProfile(int iUserID, int iSelectedEmployeeID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {
                WipEmployees u = db.WipEmployees.Single(p => p.EmployeeID == iSelectedEmployeeID);
                db.WipEmployees.DeleteOnSubmit(u);
                db.SubmitChanges();

                lblError.Text = "**Employee Deleted successfully!";
                lblError.ForeColor = Color.Green;

                Reset();
            }
            catch (Exception ex)
            {
                lblError.Text = "**Employee Delete failed!(You can't delete a user who is associated with another table.e.g. Orders)";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void BindProfile(int iSelectedEmployeeID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            var query = (from w in db.WipEmployees
                         where w.EmployeeID == iSelectedEmployeeID
                         select w);
            foreach (var a in query)
            {


                txtFirstName.Text = a.FirstName;
                txtMiddleInitial.Text = a.MiddleName;
                txtLastName.Text = a.LastName;
                ddlStatus.SelectedValue = a.Status.ToString();

            }

        }
    }
    private void LoadUserList(string sSearch)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sFullName = sSearch;
            string sFirstName = null;
            string sMiddleName = null;
            string sLastName = null;

            string[] name = sFullName.Split(' ');
            sFirstName = name[0].ToString();
            switch (name.Length)
            {
                case 1://Just First Name
                    sFirstName = name[0].ToString().ToUpper();
                    break;
                case 2://No Middle name
                    sFirstName = name[0].ToString().ToUpper();
                    sLastName = name[1].ToString().ToUpper();
                    break;
                case 3:
                    sFirstName = name[0].ToString().ToUpper();
                    sMiddleName = name[1].ToString().ToUpper();
                    sLastName = name[2].ToString().ToUpper();
                    break;
            }



            lbEmployees.Items.Clear();
            var query = (from users in db.WipEmployees
                         where
                          (users.FirstName.ToUpper().Contains(sFirstName) || sFirstName == null)
                          &&
                          (users.MiddleName.ToUpper().Contains(sMiddleName) || sMiddleName == null)
                          &&
                          (users.LastName.ToUpper().Contains(sLastName) || sLastName == null)
                         orderby users.FirstName, users.LastName
                         select new
                         {
                             users.FirstName,
                             users.MiddleName,
                             users.LastName,
                             users.EmployeeID
                         });
            if (query.Count() > 0)
            {
                foreach (var a in query)
                {
                    sFullName = a.FirstName + " " + (a.MiddleName ?? "") + " " + a.LastName;
                    sFullName = sFullName.Replace("  ", " ");
                    lbEmployees.Items.Add(new ListItem(sFullName + " - " + a.EmployeeID.ToString(), a.EmployeeID.ToString()));
                }                
            }
 
        }
    }

    private void Reset()
    {
        txtFirstName.Text = "";
        txtMiddleInitial.Text = "";
        txtLastName.Text = "";
        ddlStatus.SelectedIndex = 0;

    }

    #endregion

    #region Functions
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
            LoadUserList("");
        }

    }
    protected void ibnSave_Click(object sender, EventArgs e)
    {
        //Save Profile...
        lblError.Text = "";
        int iUserID = 0;
        int iSelectedUserID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (lbEmployees.SelectedIndex == 0)
        {
            lblError.Text = "**Please selected an employee!";
            lblError.ForeColor = Color.Red;
            return;
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        iSelectedUserID = Convert.ToInt32(lbEmployees.SelectedValue);
        UpdateProfile(iUserID, iSelectedUserID);
    }
    protected void ibnAdd_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iUserID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        if (Page.IsValid == false)
        {
            return;
        }

        AddUserProfile(iUserID);
    }
    protected void ibnDelete_Click(object sender, EventArgs e)
    {//Delete Profile...
        lblError.Text = "";
        int iUserID = 0;
        int iSelectedUserID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (lbEmployees.SelectedIndex == 0)
        {
            lblError.Text = "**Please selected a user.";
            lblError.ForeColor = Color.Red;
            return;
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        iSelectedUserID = Convert.ToInt32(lbEmployees.SelectedValue);
        DeleteProfile(iUserID, iSelectedUserID);
    }
    protected void ibnSearch_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        LoadUserList(txtSearch.Text.Trim());
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        LoadUserList(txtSearch.Text.Trim());
    }
    protected void lbEmployees_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iUserID = 0;
        if (lbEmployees.SelectedIndex != -1)
        {
            iUserID = Convert.ToInt32(lbEmployees.SelectedValue);
            ibnSave.Enabled = true;
            ibnDelete.Enabled = true;
            BindProfile(iUserID);
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
        int iUserID = 0;
        if (rblMode.SelectedIndex == 0)//Add...
        {
            Reset();
            ibnAdd.Enabled = true;
            ibnSave.Enabled = false;
            ibnDelete.Enabled = false;

            lbEmployees.Visible = false;
            txtSearch.Visible = false;
            ibnSearch.Visible = false;
        }
        else//Edit...
        {
            ibnAdd.Enabled = false;
            lbEmployees.Visible = true;
            txtSearch.Visible = true;
            ibnSearch.Visible = true;
            if (lbEmployees.SelectedIndex != -1)
            {
                iUserID = Convert.ToInt32(lbEmployees.SelectedValue);
                ibnSave.Enabled = true;
                ibnDelete.Enabled = true;
                BindProfile(iUserID);
            }
            else
            {
                Reset();
                ibnSave.Enabled = false;
                ibnDelete.Enabled = false;
            }

        }
    }



    #endregion

    #region WebMethods
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListUserName(string prefixText, int count, string contextKey)
    {//...
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {

            using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
            {
                string[] list = db.WipEmployees.Where(w => w.FirstName != null && w.LastName != null).OrderBy(w => w.LastName).Select(w => (w.FirstName + " " + (w.MiddleName ?? "") + " " + w.LastName).Replace("  ", " ")).Distinct().ToArray();
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