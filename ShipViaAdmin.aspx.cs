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
public partial class ShipViaAdmin : System.Web.UI.Page
{
    #region Subs
    private void LoadShipperList(string sSearch, List<string> lStatus)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            sSearch = sSearch.ToUpper();
            lbShipVia.Items.Clear();
            var query = (from o in db.SorShipper
                         orderby o.ShipVia
                         where
                          (o.ShipVia.ToUpper().Contains(sSearch) || sSearch == null)
                          && lStatus.Contains(o.Status.ToString())
                         select new
                         {
                             o.ShipperID,
                             o.ShipVia,


                         });

            foreach (var a in query)
            {
                lbShipVia.Items.Add(new ListItem(a.ShipVia + " - " + a.ShipperID.ToString(), a.ShipperID.ToString()));
            }
        }
    }
    private void BindProfile(int iShipperID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            var query = (from s in db.SorShipper
                         where s.ShipperID == iShipperID
                         select s);
            foreach (var a in query)
            {

                txtShipVia.Text = a.ShipVia;

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

            string sShipVia = txtShipVia.Text.Trim();


            //Check to see if username already exists
            if (DoesShipViaExist(sShipVia))
            {
                sMsg += "**ShipVia already exists, please use another one!<br/>";
            }

            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }


            try
            {
                SorShipper u = new SorShipper();

                u.ShipVia = sShipVia.ToUpper();


                if (ddlStatus.SelectedIndex == 0)
                {//Active...
                    u.Status = 1;
                }
                else//Not Active...
                {
                    u.Status = 0;
                }

                u.DateAdded = DateTime.Now;

                db.SorShipper.InsertOnSubmit(u);
                db.SubmitChanges();

                lblError.Text = "**ShipVia Added successfully!";
                lblError.ForeColor = Color.Green;

                Reset();
            }
            catch (Exception ex)
            {
                lblError.Text = "**ShipVia Added failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void UpdateProfile(int iShipperID)
    {
        if (Page.IsValid == false)
        {
            return;
        }
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            string sShipVia = txtShipVia.Text.Trim();

            if (DoesShipViaExistForUpdate(iShipperID, sShipVia))
            {
                sMsg += "**ShipVia already exists, please use another one!<br/>";
            }
            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }


            try
            {

                SorShipper u = db.SorShipper.Single(p => p.ShipperID == iShipperID);
                u.ShipVia = sShipVia.ToUpper();
                if (ddlStatus.SelectedIndex == 0)
                {//Active...
                    u.Status = 1;
                }
                else//Not Active...
                {
                    u.Status = 0;
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
                LoadShipperList(txtSearch.Text.Trim(), lStatus);

                //lbShipVia.SelectedValue = iShipperID.ToString();


                BindProfile(iShipperID);

                lblError.Text = "**ShipVia updated successfully!";
                lblError.ForeColor = Color.Green;

            }
            catch (Exception ex)
            {
                lblError.Text = "**ShipVia updated failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void DeleteProfile(int iShipperID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        try
        {
            SorShipper u = db.SorShipper.Single(p => p.ShipperID == iShipperID);
            db.SorShipper.DeleteOnSubmit(u);
            db.SubmitChanges();

            lblError.Text = "**ShipVia Deleted successfully!";
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
            LoadShipperList(txtSearch.Text.Trim(), lStatus);
        }
        catch (Exception ex)
        {
            lblError.Text = "**ShipVia Delete failed!(You can't delete an ShipVia who is associated with another table! i.e. Orders)";
            lblError.ForeColor = Color.Red;
            Debug.WriteLine(ex.ToString());
        }
    }
    private void Reset()
    {

        txtShipVia.Text = "";

    }

    #endregion

    #region Functions
    private bool DoesShipViaExist(string sShipVia)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from u in db.SorShipper
                         where u.ShipVia == sShipVia
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
    private bool DoesShipViaExistForUpdate(int iShipperID, string sShipVia)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from u in db.SorShipper
                         where u.ShipVia == sShipVia
                         && u.ShipperID != iShipperID

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
            LoadShipperList(txtSearch.Text.Trim(), lStatus);
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

        if (lbShipVia.SelectedIndex == -1)
        {
            lblError.Text = "**Please selected a ShipVia.";
            lblError.ForeColor = Color.Red;
            return;
        }

        int iShipperID = Convert.ToInt32(lbShipVia.SelectedValue);
        UpdateProfile(iShipperID);
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

        if (lbShipVia.SelectedIndex == 0)
        {
            lblError.Text = "**Please selected a ShipVia.";
            lblError.ForeColor = Color.Red;
            return;
        }

        int iShipperID = Convert.ToInt32(lbShipVia.SelectedValue);
        DeleteProfile(iShipperID);
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
        LoadShipperList(txtSearch.Text.Trim(), lStatus);
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
        LoadShipperList(txtSearch.Text.Trim(), lStatus);
    }
    protected void lbShipVia_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iShipperID = 0;
        if (lbShipVia.SelectedIndex != -1)
        {
            iShipperID = Convert.ToInt32(lbShipVia.SelectedValue);
            ibnSave.Enabled = true;
            ibnDelete.Enabled = true;
            BindProfile(iShipperID);
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
        int iShipperID = 0;
        if (rblMode.SelectedIndex == 0)//Add...
        {
            Reset();
            ibnAdd.Enabled = true;
            ibnSave.Enabled = false;
            ibnDelete.Enabled = false;

            lbShipVia.Visible = false;
            txtSearch.Visible = false;
            ibnSearch.Visible = false;
            txtShipVia.Enabled = true;
            txtShipVia.ReadOnly = false;
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
            LoadShipperList(txtSearch.Text.Trim(), lStatus);
            ibnAdd.Enabled = false;
            lbShipVia.Visible = true;
            txtSearch.Visible = true;
            ibnSearch.Visible = true;
            txtShipVia.ReadOnly = true;
            lblView.Visible = true;
            cblStatus.Visible = true;
            if (lbShipVia.SelectedIndex != -1)
            {
                iShipperID = Convert.ToInt32(lbShipVia.SelectedValue);
                ibnSave.Enabled = true;
                ibnDelete.Enabled = true;
                BindProfile(iShipperID);
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
        LoadShipperList(txtSearch.Text.Trim(), lStatus);
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
            string[] list = db.SorShipper.Where(w => w.ShipVia != null).OrderBy(w => w.ShipVia).Select(w => (w.ShipVia)).Distinct().ToArray();
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