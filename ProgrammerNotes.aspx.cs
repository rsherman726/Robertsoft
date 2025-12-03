using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.IO;
using System.Text;
using ASP;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using System.Data.Linq;
using System.ComponentModel;
using System.Data.OleDb;
using System.Drawing;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections.Generic;
public partial class ProgrammerNotes : System.Web.UI.Page
{
    private void LoadNotes()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            var query = (from a in db.AbcA_ProgrammerNotes select a);
            foreach (var a in query)
            {
                txtNotes.Text = a.Notes;
                lblModifiedDate.Text = "Last Updated: " + Convert.ToDateTime(a.DateModified).ToString();
            }


        }
    }
    private void UpdateNotes()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {
                AbcA_ProgrammerNotes n = db.AbcA_ProgrammerNotes.Single(p => p.ProNotesID == 1);
                if (txtNotes.Text.Trim() != "")
                {
                    n.Notes = txtNotes.Text.Trim();
                }
                else
                {
                    n.Notes = null;
                }
                n.DateModified = DateTime.Now;
                db.SubmitChanges();

                LoadNotes();

                lblErrorNotes.Text = "**Update sucessfully!!";
                lblErrorNotes.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                lblErrorNotes.Text = "**Update failed!!";
                lblErrorNotes.ForeColor = Color.Red;
            }



        }
    }
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
            LoadNotes();
        }
    }

    protected void lbnSave_Click(object sender, EventArgs e)
    {
        lblErrorNotes.Text = "";
        UpdateNotes();
    }
}