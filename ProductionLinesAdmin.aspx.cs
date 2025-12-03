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
public partial class ProductionLinesAdmin : System.Web.UI.Page
{
    #region Subs
    private void AddProLineProfile()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            string sLineName = txtLineName.Text.Trim();

            if (LineNameExists(sLineName) == true)
            {
                sMsg = "**Line name already exists, please use another name!";
            }


            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }

            try
            {

                WipProductionLines pl = new WipProductionLines();
                pl.LineName = sLineName;
                db.WipProductionLines.InsertOnSubmit(pl);
                db.SubmitChanges();

                lblError.Text = "**Production Line Added successfully!";
                lblError.ForeColor = Color.Green;

                Reset();
            }
            catch (Exception ex)
            {
                lblError.Text = "**Production Line Added failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void UpdateProfile(int iProLineID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";

            if (Page.IsValid == false)
            {
                return;
            }

            string sLineName = txtLineName.Text.Trim();


            //Clean Phone...


            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }


            try
            {
                
                WipProductionLines pl = db.WipProductionLines.Single(p => p.ProLineID == iProLineID);
                pl.LineName = sLineName;
                db.SubmitChanges();


                BindProfile(iProLineID);

                lblError.Text = "**Production Line updated successfully!";
                lblError.ForeColor = Color.Green;

            }
            catch (Exception ex)
            {
                lblError.Text = "**Production Line updated failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void DeleteProfile(int iProLineID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {
                WipProductionLines pl = db.WipProductionLines.Single(p => p.ProLineID == iProLineID);
                db.WipProductionLines.DeleteOnSubmit(pl);
                db.SubmitChanges();

                lblError.Text = "**Production Line Deleted successfully!";
                lblError.ForeColor = Color.Green;

                Reset();
            }
            catch (Exception ex)
            {
                lblError.Text = "**Production Line Delete failed!(You can't delete a user who is associated with another table.e.g. Orders)";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void BindProfile(int iProLineID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            var query = (from w in db.WipProductionLines
                         where w.ProLineID == iProLineID
                         select w);
            foreach (var a in query)
            {
                txtLineName.Text = a.LineName;
            }
        }
    }
    private void LoadProductionLinesList(string sSearch)
    {

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddlProductionLines.Items.Clear();
            var query = (from ProductionLines in db.WipProductionLines
                         where
                          (ProductionLines.LineName.ToUpper().Contains(sSearch.ToUpper()) || sSearch == null)
                          orderby ProductionLines.LineName
                         select new
                         {
                             ProductionLines.LineName,
                             ProductionLines.ProLineID
                         });
            if (query.Count() > 0)
            {
                foreach (var a in query)
                {
                    ddlProductionLines.Items.Add(new ListItem(a.LineName, a.ProLineID.ToString()));
                }
                ddlProductionLines.Items.Insert(0, new ListItem("--Select a ProductionLine--", "0"));
            }
            else
            {
                ddlProductionLines.Items.Insert(0, new ListItem("No ProductionLines found...", "0"));
            }
        }
    }
    private void Reset()
    {
        txtLineName.Text = "";
    }
    #endregion

    #region Functions
    public static bool LineNameExists(string sLineName)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from pl in db.WipProductionLines
                         where pl.LineName == sLineName
                         select pl);
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
            ibnAdd.Enabled = false;
            ibnDelete.Enabled = false;
            ibnSave.Enabled = false;
            LoadProductionLinesList("");
        }

    }
    protected void ibnSave_Click(object sender, EventArgs e)
    {
        //Save Profile...
        lblError.Text = "";
        int iProLineID = 0;
 

        if (ddlProductionLines.SelectedIndex == 0)
        {
            lblError.Text = "**Please selected an Production Line!";
            lblError.ForeColor = Color.Red;
            return;
        }
        
        iProLineID = Convert.ToInt32(ddlProductionLines.SelectedValue);
        UpdateProfile(iProLineID);
    }
    protected void ibnAdd_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (Page.IsValid == false)
        {
            return;
        }

        AddProLineProfile();
    }
    protected void ibnDelete_Click(object sender, EventArgs e)
    {//Delete Profile...
        lblError.Text = "";
        int iProLineID = 0;
        if (ddlProductionLines.SelectedIndex == 0)
        {
            lblError.Text = "**Please selected a Production Line.";
            lblError.ForeColor = Color.Red;
            return;
        }

        iProLineID = Convert.ToInt32(ddlProductionLines.SelectedValue);
        DeleteProfile(iProLineID);
    }
    protected void ibnSearch_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        LoadProductionLinesList(txtSearch.Text.Trim());
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        LoadProductionLinesList(txtSearch.Text.Trim());
    }
    protected void ddlProductionLines_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iProLineID = 0;
        if (ddlProductionLines.SelectedIndex != 0)
        {
            iProLineID = Convert.ToInt32(ddlProductionLines.SelectedValue);
            ibnSave.Enabled = true;
            ibnDelete.Enabled = true;
            BindProfile(iProLineID);
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
        int iProLineID = 0;
        if (rblMode.SelectedIndex == 0)//Add...
        {
            Reset();
            ibnAdd.Enabled = true;
            ibnSave.Enabled = false;
            ibnDelete.Enabled = false;

            ddlProductionLines.Visible = false;
            txtSearch.Visible = false;
            ibnSearch.Visible = false;
        }
        else//Edit...
        {
            ibnAdd.Enabled = false;
            ddlProductionLines.Visible = true;
            txtSearch.Visible = true;
            ibnSearch.Visible = true;
            if (ddlProductionLines.SelectedIndex != 0)
            {
                iProLineID = Convert.ToInt32(ddlProductionLines.SelectedValue);
                ibnSave.Enabled = true;
                ibnDelete.Enabled = true;
                BindProfile(iProLineID);
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
    public static string[] GetCompletionListLineName(string prefixText, int count, string contextKey)
    {//...
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {

            // Your LINQ to SQL query goes here 
            using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
            {
                string[] list = db.WipProductionLines.Where(w => w.LineName != null).OrderBy(w => w.LineName).Select(w => (w.LineName)).Distinct().ToArray();
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