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

public partial class CalendarAdmin : System.Web.UI.Page
{
    #region Subs
    private void LoadHolidayCalendar()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = new DataTable();

            var query = (from d in db.HolidayCalendar
                         orderby
                           d.HolidayDate descending
                         select new
                         {

                             d.HolidayCalendarID,
                             d.Holiday,
                             d.HolidayDate
                         });
            dt = SharedFunctions.ToDataTable(db, query);
            gvCalendar.DataSource = dt;
            gvCalendar.DataBind();
            Session["dtHolidayCalendar"] = dt;
            dt.Dispose();
        }
    }


    #endregion

    #region Functions

    private bool HolidayExists(string sHoliday, string sCalendateDate)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.HolidayCalendar
                         where c.Holiday == sHoliday
                         && c.HolidayDate == Convert.ToDateTime(sCalendateDate)
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
    private bool HolidayExistsForUpdate(string sHoliday, string sCalendateDate, int iHolidayCalendarID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.HolidayCalendar
                         where c.Holiday == sHoliday
                         && c.HolidayDate == Convert.ToDateTime(sCalendateDate)
                         && c.HolidayCalendarID != iHolidayCalendarID
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
            LoadHolidayCalendar();
            lblPageNo.Text = "Current Page #: 1";
        }
    }
    protected void gvCalendar_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (gvCalendar.EditIndex != -1)//In Edit Mode...
            {

                if (gvCalendar.EditIndex == e.Row.RowIndex)//edited row...
                {

                    TextBox txtHolidayDate = (TextBox)e.Row.FindControl("txtHolidayDate");
                    if (txtHolidayDate.Text != "")
                    {
                        txtHolidayDate.Text = Convert.ToDateTime(txtHolidayDate.Text).ToShortDateString();
                    }                 
                    
                }
                else if (gvCalendar.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
                {
                    Label lblHolidayDate = (Label)e.Row.FindControl("lblHolidayDate");
                    if (lblHolidayDate.Text != "")
                    {
                        lblHolidayDate.Text = Convert.ToDateTime(lblHolidayDate.Text).ToShortDateString();
                    }
                }

            }
            else
            {
                Label lblHolidayDate = (Label)e.Row.FindControl("lblHolidayDate");
                if (lblHolidayDate.Text != "")
                {
                    lblHolidayDate.Text = Convert.ToDateTime(lblHolidayDate.Text).ToShortDateString();
                }
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)//the footer row...
        {
           
        }
    }
    protected void gvCalendar_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvCalendar.EditIndex = e.NewEditIndex;
        gvCalendar.DataSource = (DataTable)Session["dtHolidayCalendar"];
        gvCalendar.DataBind();

    }
    protected void gvCalendar_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvCalendar.EditIndex = -1;
        gvCalendar.DataSource = (DataTable)Session["dtHolidayCalendar"];
        gvCalendar.DataBind();
    }
    protected void gvCalendar_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvCalendar.EditIndex = -1;
        LoadHolidayCalendar();
    }
    protected void gvCalendar_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvCalendar.EditIndex = -1;
        LoadHolidayCalendar();
    }
    protected void gvCalendar_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCalendar.PageIndex = e.NewPageIndex;
        lblPageNo.Text = "Current Page #: " + (gvCalendar.PageIndex + 1).ToString();
        gvCalendar.DataSource = (DataTable)Session["dtHolidayCalendar"];
        gvCalendar.DataBind();
    }
    protected void gvCalendar_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sMsg = "";
        lblError.Text = "";
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        int iUserID = 0;
        int iHolidayCalendarID = 0;

        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        int i = 0;
        Label lblHolidayCalendarID;
        TextBox txtHolidayDate;
        TextBox txtHoliday;
        
        TextBox txtHolidayDateAdd;
        TextBox txtHolidayAdd;
        
        string sHoliday = "";

        switch (e.CommandName)
        {
            case "Add":
                lblError.Text = "";
                txtHolidayDateAdd = (TextBox)gvCalendar.FooterRow.FindControl("txtHolidayDateAdd");
                txtHolidayAdd = (TextBox)gvCalendar.FooterRow.FindControl("txtHolidayAdd");                
                sHoliday = txtHolidayAdd.Text.Trim();
                if (txtHolidayDateAdd.Text.Trim() == "")
                {
                    lblError.Text = "**Holiday Date Required!<br/>";
                    lblError.ForeColor = Color.Red;
                    return;
                }
                //Validate data...
                if (txtHolidayAdd.Text.Trim() == "")
                {
                    sMsg += "**Please fill in Holiday Name!<br/>";
                }
                if (HolidayExists(txtHolidayAdd.Text.Trim(), txtHolidayDateAdd.Text.Trim()))
                {
                    sMsg += "**Holiday with that date already exists for date entered!<br/>";
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
                    HolidayCalendar dl = new HolidayCalendar();
                    dl.Holiday = sHoliday;
                    dl.HolidayDate = Convert.ToDateTime(txtHolidayDateAdd.Text.Trim());                  
                    db.HolidayCalendar.InsertOnSubmit(dl);                    
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
                LoadHolidayCalendar();//Refresh...                
                break;
            case "Update":
                lblError.Text = "";

                i = Convert.ToInt32(e.CommandArgument);
                lblError.Text = "";
                txtHolidayDate = (TextBox)gvCalendar.Rows[i].FindControl("txtHolidayDate");
                txtHoliday = (TextBox)gvCalendar.Rows[i].FindControl("txtHoliday");
                lblHolidayCalendarID = (Label)gvCalendar.Rows[i].FindControl("lblHolidayCalendarID");               
                iHolidayCalendarID = Convert.ToInt32(lblHolidayCalendarID.Text);
                sHoliday = txtHoliday.Text.Trim();
                if (txtHolidayDate.Text.Trim() == "")
                {
                    sMsg += "**Holiday Date Required!<br/>";
                }
               
                //Validate data...
                if (txtHoliday.Text.Trim() == "")
                {
                    sMsg += "**Please fill in Holiday Name!<br/>";
                }
                if (HolidayExistsForUpdate(txtHoliday.Text.Trim(), txtHolidayDate.Text.Trim(), iHolidayCalendarID))
                {
                    sMsg += "**Holiday already exists with date entered!<br/>";
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
                    HolidayCalendar dl = db.HolidayCalendar.Single(p => p.HolidayCalendarID == iHolidayCalendarID);
                    dl.HolidayDate = Convert.ToDateTime(txtHolidayDate.Text.Trim());
                    dl.Holiday = sHoliday;                    
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
                    lblHolidayCalendarID = (Label)gvCalendar.Rows[i].FindControl("lblHolidayCalendarID");
                    iHolidayCalendarID = Convert.ToInt32(lblHolidayCalendarID.Text);
                    var query = (from d in db.HolidayCalendar select d);
                    if (query.Count() == 1)
                    {
                        lblError.Text = "**You cannot delete the last record in the table.";
                        lblError.ForeColor = Color.Red;
                        return;
                    }

                    HolidayCalendar dl = db.HolidayCalendar.Single(p => p.HolidayCalendarID == iHolidayCalendarID);
                    db.HolidayCalendar.DeleteOnSubmit(dl);                    
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