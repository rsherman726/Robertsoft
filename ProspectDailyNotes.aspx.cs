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
using ClosedXML.Excel;

public partial class ProspectDailyNotes : System.Web.UI.Page
{
    #region Subs
    private void ExportToExcel(DataSet ds, string sFileName)
    {

        if (ds != null)
        {

            if (ds.Tables.Count == 0)
            {
                return;
            }
        }
        else
        {
            return;
        }

        ExcelHelper.ToExcel(ds, sFileName, Page.Response);

    }
    private void BindProfile(int iWipProspectID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {

                var query = (from u in db.WipProspects
                             where u.WipProspectID == iWipProspectID
                             select u);
                foreach (var a in query)
                {
                    txtCompanyName.Text = a.CompanyName;
                    txtAddress.Text = a.Address;
                    txtCity.Text = a.City;
                    if (a.State != null)
                    {
                        ddlState.SelectedValue = a.State;
                    }
                    else
                    {
                        ddlState.SelectedIndex = 0;
                    }
                    txtPostalCode.Text = a.PostalCode;
                    txtContact.Text = a.Contact;
                    if (a.Phone != null)
                    {
                        if (a.Phone.Trim().Length > 9)
                        {
                            txtPhone.Text = SharedFunctions.GetPhoneFormat(a.Phone);
                        }
                    }
                    if (a.AltPhone != null)
                    {
                        if (a.AltPhone.Trim().Length > 9)
                        {
                            txtAltPhone.Text = SharedFunctions.GetPhoneFormat(a.AltPhone);
                        }
                    }
                    txtEmail.Text = a.Email;
                    txtExtension.Text = a.Extension;

                    lblProspectID.Text = a.WipProspectID.ToString();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }
    }
    private void AddUserProfile(int iUserID)
    {
        string sCompanyName = SharedFunctions.PCase(txtCompanyName.Text.Trim());
        string sAddress = SharedFunctions.PCase(txtAddress.Text.Trim());
        string sCity = SharedFunctions.PCase(txtCity.Text.Trim());
        string sState = ddlState.SelectedValue;
        string sPostalCode = txtPostalCode.Text.Trim();
        string sPhone = txtPhone.Text.Trim();
        string sAltPhone = txtAltPhone.Text.Trim();
        string sEmail = txtEmail.Text.Trim();
        string sContact = txtContact.Text.Trim();
        string sExtension = txtExtension.Text.Trim();
        //Clean Phone...

        sPhone = sPhone.Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Trim();
        sAltPhone = sAltPhone.Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Trim();
        string sMsg = "";
        //Validate data...

        if (sPhone != "")
        {
            if (SharedFunctions.IsNumeric(sPhone) == false)
            {
                sMsg += "**Phone number contains non numeric values!<br/>";
            }
        }
        if (sAltPhone != "")
        {
            if (SharedFunctions.IsNumeric(sAltPhone) == false)
            {
                sMsg += "**Alt Phone number contains non numeric values!<br/>";
            }
        }
        if (sExtension.Length > 20)
        {
            sMsg += "**Extesion is limited to 20 characters!<br/>";
        }
        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            lblError.ForeColor = Color.Red;
            lblError0.Text = sMsg;
            lblError0.ForeColor = Color.Red;
            return;
        }




        try
        {

            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            WipProspects u = new WipProspects();


            u.CompanyName = sCompanyName;
            u.WipUserID = iUserID;

            if (sAddress != "")
            {
                u.Address = sAddress;
            }
            else
            {
                u.Address = null;
            }
            if (sCity != "")
            {
                u.City = sCity;
            }
            else
            {
                u.City = null;
            }
            if (sState != "")
            {
                u.State = sState;
            }
            else
            {
                u.State = null;
            }
            if (sPostalCode != "")
            {
                u.PostalCode = sPostalCode;
            }
            else
            {
                u.PostalCode = null;
            }
            if (sEmail != "")
            {
                u.Email = sEmail;
            }
            else
            {
                u.Email = null;
            }
            if (sPhone != "")
            {
                u.Phone = sPhone;
            }
            else
            {
                u.Phone = null;
            }
            if (sExtension != "")
            {
                u.Extension = sExtension;
            }
            else
            {
                u.Extension = null;
            }
            if (sAltPhone != "")
            {
                u.AltPhone = sAltPhone;
            }
            else
            {
                u.AltPhone = null;
            }
            if (sContact != "")
            {
                u.Contact = sContact;
            }
            else
            {
                u.Contact = null;
            }
            u.DateAdded = DateTime.Now;
            db.WipProspects.InsertOnSubmit(u);
            db.SubmitChanges();



            lblError.Text = "**Record Added successfully!";
            lblError.ForeColor = Color.Green;
            lblError0.Text = "**Record Added successfully!";
            lblError0.ForeColor = Color.Green;

            Reset();
        }
        catch (Exception ex)
        {
            lblError.Text = "**Record Added failed!";
            lblError.ForeColor = Color.Red;
            lblError0.Text = "**Record Added failed!";
            lblError0.ForeColor = Color.Red;
            Debug.WriteLine(ex.ToString());
        }
    }
    private void UpdateProfile(int iWipProspectID)
    {
        if (Page.IsValid == false)
        {
            return;
        }

        string sCompanyName = SharedFunctions.PCase(txtCompanyName.Text.Trim());
        string sAddress = SharedFunctions.PCase(txtAddress.Text.Trim());
        string sCity = SharedFunctions.PCase(txtCity.Text.Trim());
        string sState = ddlState.SelectedValue;
        string sPostalCode = txtPostalCode.Text.Trim();
        string sPhone = txtPhone.Text.Trim();
        string sAltPhone = txtAltPhone.Text.Trim();
        string sEmail = txtEmail.Text.Trim();
        string sContact = txtContact.Text.Trim();
        string sExtension = txtExtension.Text.Trim();
        //Clean Phone...

        sPhone = sPhone.Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Trim();
        sAltPhone = sAltPhone.Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Trim();

        string sMsg = "";
        //Validate data...

        if (sPhone != "")
        {
            if (SharedFunctions.IsNumeric(sPhone) == false)
            {
                sMsg += "**Phone number contains non numeric values!<br/>";
            }
        }
        if (sAltPhone != "")
        {
            if (SharedFunctions.IsNumeric(sAltPhone) == false)
            {
                sMsg += "**Alt Phone number contains non numeric values!<br/>";
            }
        }
        if (sExtension.Length > 20)
        {
            sMsg += "**Extesion is limited to 20 characters!<br/>";
        }
        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            lblError.ForeColor = Color.Red;
            lblError0.Text = sMsg;
            lblError0.ForeColor = Color.Red;
            return;
        }

        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            lblError.ForeColor = Color.Red;
            lblError0.Text = sMsg;
            lblError0.ForeColor = Color.Red;
            return;
        }

        if (Session["UserID"] == null)
        {
            return;
        }


        try
        {

            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            WipProspects u = db.WipProspects.Single(p => p.WipProspectID == iWipProspectID);
            u.CompanyName = sCompanyName;


            if (sAddress != "")
            {
                u.Address = sAddress;
            }
            else
            {
                u.Address = null;
            }
            if (sCity != "")
            {
                u.City = sCity;
            }
            else
            {
                u.City = null;
            }
            if (sState != "")
            {
                u.State = sState;
            }
            else
            {
                u.State = null;
            }
            if (sPostalCode != "")
            {
                u.PostalCode = sPostalCode;
            }
            else
            {
                u.PostalCode = null;
            }
            if (sEmail != "")
            {
                u.Email = sEmail;
            }
            else
            {
                u.Email = null;
            }
            if (sPhone != "")
            {
                u.Phone = sPhone;
            }
            else
            {
                u.Phone = null;
            }
            if (sExtension != "")
            {
                u.Extension = sExtension;
            }
            else
            {
                u.Extension = null;
            }
            if (sAltPhone != "")
            {
                u.AltPhone = sAltPhone;
            }
            else
            {
                u.AltPhone = null;
            }
            if (sContact != "")
            {
                u.Contact = sContact;
            }
            else
            {
                u.Contact = null;
            }
            db.SubmitChanges();
            int iUserID = 0;
            int iRoleID = Convert.ToInt32(Session["RoleID"]);
             if (iRoleID == 1  || iRoleID == 5)//Salesperson...
            {
                if (ddlSalesPerson.SelectedIndex != 0)
                {
                    iUserID = Convert.ToInt32(ddlSalesPerson.SelectedValue);
                }
            }
            else//Salesperson...
            {
                iUserID = Convert.ToInt32(Session["UserID"]);
            }
            LoadProspectsList(txtSearch.Text.Trim(), iUserID);
            try
            {
                lbProspects.SelectedValue = iWipProspectID.ToString();
            }
            catch (Exception)
            {
                //ignore...they may not be in list anymore...
            }


            BindProfile(iWipProspectID);

            lblError.Text = "**Record updated successfully!<br/>";
            lblError.ForeColor = Color.Green;
            lblError0.Text = "**Record updated successfully!<br/>";
            lblError0.ForeColor = Color.Green;

        }
        catch (Exception ex)
        {
            lblError.Text = "**Record updated failed!";
            lblError.ForeColor = Color.Red;
            lblError0.Text = "**Record updated failed!";
            lblError0.ForeColor = Color.Red;
            Debug.WriteLine(ex.ToString());
        }
    }
    private void DeleteProfile(int iWipProspectID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        try
        {
            WipProspects u = db.WipProspects.Single(p => p.WipProspectID == iWipProspectID);
            db.WipProspects.DeleteOnSubmit(u);

            db.SubmitChanges();

            lblError.Text = "**Record Deleted successfully!";
            lblError.ForeColor = Color.Green;
            lblError0.Text = "**Record Deleted successfully!";
            lblError0.ForeColor = Color.Green;

            Reset();
            int iUserID = 0;
            int iRoleID = Convert.ToInt32(Session["RoleID"]);
             if (iRoleID == 1  || iRoleID == 5)//Salesperson...
            {
                if (ddlSalesPerson.SelectedIndex != 0)
                {
                    iUserID = Convert.ToInt32(ddlSalesPerson.SelectedValue);
                }
            }
            else//Salesperson...
            {
                iUserID = Convert.ToInt32(Session["UserID"]);
            }
            LoadProspectsList("", iUserID);
        }
        catch (Exception ex)
        {
            lblError.Text = "**Record Delete failed!(You can't delete a record who is associated with another table.e.g. Orders)";
            lblError.ForeColor = Color.Red;
            lblError0.Text = "**Record Delete failed!(You can't delete a record who is associated with another table.e.g. Orders)";
            lblError0.ForeColor = Color.Red;
            Debug.WriteLine(ex.ToString());
        }
    }
    private void LoadProspectsList(string sSearch, int iUserID)
    {
        try
        {

            using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
            {
                lbProspects.Items.Clear();
                var query = (from c in db.WipProspects
                             where c.WipUserID == iUserID &&
                              (
                               c.CompanyName.Contains(sSearch) ||
                               c.Email.Contains(sSearch) ||
                               sSearch == null)
                             orderby c.CompanyName
                             select new
                             {
                                 c.CompanyName,
                                 c.Contact,
                                 c.WipProspectID,

                             });
                if (query.Count() > 0)
                {
                    foreach (var a in query)
                    {

                        string sDisplay = "";
                        if (a.Contact != null)
                        {
                            sDisplay = a.CompanyName + " - " + a.Contact;
                        }
                        else
                        {
                            sDisplay = a.CompanyName;
                        }
                        lbProspects.Items.Add(new ListItem(sDisplay, a.WipProspectID.ToString()));
                    }
                }
                else
                {
                    lblError.Text = "**No Records found!";
                    lblError0.Text = "**No Records found!";
                    lblError.ForeColor = Color.Red;
                    lblError0.ForeColor = Color.Red;
                    Reset();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    private void Reset()
    {
        txtCompanyName.Text = "";
        txtAddress.Text = "";
        txtCity.Text = "";
        ddlState.SelectedIndex = 0;
        txtPostalCode.Text = "";
        txtPhone.Text = "";
        txtAltPhone.Text = "";
        txtEmail.Text = "";
        txtContact.Text = "";
        txtExtension.Text = "";
    }
    private void BindNotesGrid(int iUserID, int iProspectID, string sStartDate, string sEndDate)
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from n in db.WipUserProspectNotes
                         join p in db.WipProspects on n.WipProspectID equals p.WipProspectID
                         where n.WipUserID == iUserID
                         && n.WipProspectID == iProspectID
                         && (n.NoteDate >= Convert.ToDateTime(sStartDate + " 00:00:00") && n.NoteDate <= Convert.ToDateTime(sEndDate + " 23:59:59"))
                         orderby n.NoteDate descending
                         select new
                         {

                             n.NoteID,
                             p.CompanyName,
                             p.Contact,
                             p.Address,
                             p.City,
                             p.State,
                             p.PostalCode,
                             Phone = p.Phone == null ? "" : "(" + p.Phone.Substring(0, 3) + ")" + p.Phone.Substring(3, 3) + "-" + p.Phone.Substring(6, 4),
                             p.Extension,
                             AltPhone = p.AltPhone == null ? "" : "(" + p.AltPhone.Substring(0, 3) + ")" + p.AltPhone.Substring(3, 3) + "-" + p.AltPhone.Substring(6, 4),
                             p.Email,
                             n.NoteDate,
                             n.Notes,

                         });

            dt = SharedFunctions.ToDataTable(db, query);
            if (dt.Rows.Count > 0)
            {
                gvNotes.DataSource = dt;
                gvNotes.DataBind();
                Session["dtNotes"] = dt;
            }
            else
            {
                gvNotes.DataSource = null;
                gvNotes.DataBind();
            }
        }
    }
    private void AddNotes(int iWipUserID, int iProspectID, string sNoteDate, string sNotes)
    {

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {


                WipUserProspectNotes n = new WipUserProspectNotes();
                n.WipUserID = iWipUserID;
                n.WipProspectID = iProspectID;
                n.Notes = sNotes;
                n.NoteDate = Convert.ToDateTime(sNoteDate);
                n.DateAdded = DateTime.Now;
                db.WipUserProspectNotes.InsertOnSubmit(n);
                db.SubmitChanges();

                lblErrorNotes.Text = "**Note Inserted Successfully!";
                lblErrorNotes.ForeColor = Color.Green;

                //Load Notes...
                int iUserID = 0;
                int iRoleID = Convert.ToInt32(Session["RoleID"]);
                 if (iRoleID == 1  || iRoleID == 5)//Salesperson...
                {
                    if (ddlSalesPerson.SelectedIndex != 0)
                    {
                        iUserID = Convert.ToInt32(ddlSalesPerson.SelectedValue);
                    }
                }
                else//Salesperson...
                {
                    iUserID = Convert.ToInt32(Session["UserID"]);
                }
                if (lbProspects.SelectedIndex == -1)
                {
                    lblErrorNotes.Text = "**No Prospect Selected!";
                    lblErrorNotes.ForeColor = Color.Red;
                    return;
                }
                string sStartDate = txtStartDate.Text.Trim();
                string sEndDate = txtEndDate.Text.Trim();

                BindNotesGrid(iUserID, iProspectID, sStartDate, sEndDate);

                List<string> lProspects = new List<string>();
                foreach (ListItem liProspect in lbProspects.Items)
                {
                    lProspects.Add(liProspect.Value.Trim());
                }
                GetNotesForSalespersonForDateRange(iUserID, lProspects, sStartDate, sEndDate);
            }
            catch (Exception)
            {
                lblErrorNotes.Text = "**Note Insert Failed!";
                lblErrorNotes.ForeColor = Color.Red;
                return;
            }
        }

    }
    private void LoadSalesPersons()
    {
        ddlSalesPerson.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from c in db.WipUsers
                     where c.RoleID == 4//Employee...
                     orderby c.FirstName, c.LastName
                     select new
                     {
                         Name = (c.FirstName + " " + (c.MiddleName ?? "") + " " + c.LastName).Replace("  ", " "),
                         c.UserID

                     });
        foreach (var a in query)
        {
            ddlSalesPerson.Items.Add(new ListItem(a.Name, a.UserID.ToString()));
        }
        ddlSalesPerson.Items.Insert(0, new ListItem("Select a Salesperson", "0"));
    }
    private void GetNotesForSalespersonForDateRange(int iUserID, List<string> lProspects, string sStartDate, string sEndDate)
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from n in db.WipUserProspectNotes
                         join p in db.WipProspects on n.WipProspectID equals p.WipProspectID
                         join u in db.WipUsers on n.WipUserID equals u.UserID
                         where n.WipUserID == iUserID
                          && lProspects.Contains(n.WipProspectID.ToString())
                         && (n.NoteDate >= Convert.ToDateTime(sStartDate + " 00:00:00") && n.NoteDate <= Convert.ToDateTime(sEndDate + " 23:59:59"))
                         orderby n.NoteDate descending
                         select new
                         {

                             n.NoteID,
                             Salesperson = (u.FirstName + " " + (u.MiddleName ?? "") + " " + u.LastName).Replace("  ", " "),
                             p.CompanyName,
                             p.Contact,
                             p.Address,
                             p.City,
                             p.State,
                             p.PostalCode,
                             Phone = p.Phone == null ? "" : "(" + p.Phone.Substring(0, 3) + ")" + p.Phone.Substring(3, 3) + "-" + p.Phone.Substring(6, 4),
                             p.Extension,
                             AltPhone = p.AltPhone == null ? "" : "(" + p.AltPhone.Substring(0, 3) + ")" + p.AltPhone.Substring(3, 3) + "-" + p.AltPhone.Substring(6, 4),
                             p.Email,
                             n.NoteDate,
                             n.Notes,

                         });


            dt = SharedFunctions.ToDataTable(db, query);
            if (dt.Rows.Count > 0)
            {
                gvNotes.DataSource = dt;
                gvNotes.DataBind();
                Session["dtNotesExport"] = dt;
            }
            else
            {
                gvNotes.DataSource = null;
                gvNotes.DataBind();
                Session["dtNotesExport"] = null;
            }
        }
    }
    #endregion

    #region Functions
    public static bool NotesDateAlreadyExists(int iWipUserID, int iProspectID, string sNoteDate)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from n in db.WipUserProspectNotes
                       where n.WipUserID == iWipUserID
                       && n.WipProspectID == iProspectID
                       && n.NoteDate == Convert.ToDateTime(sNoteDate)
                       select n);
            if (qry.Count() > 0)
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

        int iRoleID = 0;
        iRoleID = Convert.ToInt32(Session["RoleID"]);



        if (!Page.IsPostBack)
        {
             if (iRoleID == 4)//Salesperson...
            {
                ddlSalesPerson.Visible = false;
                iUserID = Convert.ToInt32(Session["UserID"]);
                LoadProspectsList("", iUserID);
            }
            else//Admin...
            {
                LoadSalesPersons();
                ddlSalesPerson.Visible = true;

            }
            ibnAdd.Enabled = false;
            ibnDelete.Enabled = false;
            ibnSave.Enabled = false;

            txtNoteDate.Text = DateTime.Now.ToShortDateString();
            txtStartDate.Text = DateTime.Now.Month.ToString() + "/01/" + DateTime.Now.Year.ToString();
            txtEndDate.Text = DateTime.Now.Month.ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + "/" + DateTime.Now.Year.ToString();

        }

    }
    protected void ibnSave_Click(object sender, EventArgs e)
    {
        //Save Profile...
        lblError.Text = "";
        lblError0.Text = "";
        int iWipProspectID = 0;

        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (lbProspects.SelectedIndex == -1)
        {
            lblError.Text = "**Please selected a user.";
            lblError.ForeColor = Color.Red;
            return;
        }

        iWipProspectID = Convert.ToInt32(lbProspects.SelectedValue);
        UpdateProfile(iWipProspectID);
    }
    protected void ibnAdd_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblError0.Text = "";
        int iWipProspectID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        iWipProspectID = Convert.ToInt32(Session["UserID"]);
        if (Page.IsValid == false)
        {
            return;
        }

        AddUserProfile(iWipProspectID);
    }
    protected void ibnDelete_Click(object sender, EventArgs e)
    {//Delete Profile...
        lblError.Text = "";
        lblError0.Text = "";
        int iWipProspectID = 0;

        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (lbProspects.SelectedIndex == -1)
        {
            lblError.Text = "**Please selected a user.";
            lblError.ForeColor = Color.Red;
            return;
        }

        iWipProspectID = Convert.ToInt32(lbProspects.SelectedValue);
        DeleteProfile(iWipProspectID);
    }
    protected void ibnSearch_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblError0.Text = "";
        Reset();
        int iUserID = 0;
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
         if (iRoleID == 1  || iRoleID == 5)//Salesperson...
        {
            if (ddlSalesPerson.SelectedIndex != 0)
            {
                iUserID = Convert.ToInt32(ddlSalesPerson.SelectedValue);
            }
        }
        else//Salesperson...
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
        }
        LoadProspectsList(txtSearch.Text.Trim(), iUserID);
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblError0.Text = "";
        Reset();
        int iUserID = 0;
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
         if (iRoleID == 1  || iRoleID == 5)//Salesperson...
        {
            if (ddlSalesPerson.SelectedIndex != 0)
            {
                iUserID = Convert.ToInt32(ddlSalesPerson.SelectedValue);
            }
        }
        else//Salesperson...
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
        }
        LoadProspectsList(txtSearch.Text.Trim(), iUserID);
    }
    protected void lbProspects_SelectedIndexChanged(object sender, EventArgs e)
    {

        lblError.Text = "";
        lblError0.Text = "";
        int iProspectID = 0;
        if (lbProspects.SelectedIndex != -1)
        {
            iProspectID = Convert.ToInt32(lbProspects.SelectedValue);
            ibnSave.Enabled = true;
            ibnDelete.Enabled = true;
            Reset();
            BindProfile(iProspectID);
            lbnAdd.Enabled = true;
            //Load Notes...
            int iUserID = 0;
            int iRoleID = Convert.ToInt32(Session["RoleID"]);
             if (iRoleID == 1  || iRoleID == 5)//Salesperson...
            {
                if (ddlSalesPerson.SelectedIndex != 0)
                {
                    iUserID = Convert.ToInt32(ddlSalesPerson.SelectedValue);
                }
            }
            else//Salesperson...
            {
                iUserID = Convert.ToInt32(Session["UserID"]);
            }
            string sStartDate = txtStartDate.Text.Trim();
            string sEndDate = txtEndDate.Text.Trim();
            BindNotesGrid(iUserID, iProspectID, sStartDate, sEndDate);

            List<string> lProspects = new List<string>();
            foreach (ListItem liProspect in lbProspects.Items)
            {
                lProspects.Add(liProspect.Value.Trim());
            }
            GetNotesForSalespersonForDateRange(iUserID, lProspects, sStartDate, sEndDate);
            tblNotes.Visible = true;
        }
        else
        {
            Reset();
            ibnSave.Enabled = false;
            ibnDelete.Enabled = false;
            lbnAdd.Enabled = false;
            tblNotes.Visible = false;
        }
    }
    protected void rblMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblError0.Text = "";
        int iWipProspectID = 0;
        if (rblMode.SelectedIndex == 0)//Add...
        {
            Reset();

            ibnAdd.Enabled = true;
            ibnSave.Enabled = false;
            ibnDelete.Enabled = false;
            lbProspects.Visible = false;
            txtSearch.Visible = false;
            ibnSearch.Visible = false;
            LabelUserID.Visible = false;
            lblProspectID.Visible = false;
            tblNotes.Visible = false;
        }
        else//Edit...
        {
            ibnAdd.Enabled = false;
            lbProspects.Visible = true;
            txtSearch.Visible = true;
            ibnSearch.Visible = true;
            LabelUserID.Visible = true;
            lblProspectID.Visible = true;
            int iUserID = 0;
            int iRoleID = Convert.ToInt32(Session["RoleID"]);
             if (iRoleID == 1  || iRoleID == 5)//Salesperson...
            {
                if (ddlSalesPerson.SelectedIndex != 0)
                {
                    iUserID = Convert.ToInt32(ddlSalesPerson.SelectedValue);
                }
            }
            else//Salesperson...
            {
                iUserID = Convert.ToInt32(Session["UserID"]);
            }
            LoadProspectsList("", iUserID);
            if (lbProspects.SelectedIndex != -1)
            {
                iWipProspectID = Convert.ToInt32(lbProspects.SelectedValue);
                ibnSave.Enabled = true;
                ibnDelete.Enabled = true;
                BindProfile(iWipProspectID);
                tblNotes.Visible = true;
            }
            else
            {
                Reset();
                ibnSave.Enabled = false;
                ibnDelete.Enabled = false;

            }

        }
    }
    protected void lbnAdd_Click(object sender, EventArgs e)
    {// Add Notes

        int iUserID = 0;
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
         if (iRoleID == 1  || iRoleID == 5)//Salesperson...
        {
            if (ddlSalesPerson.SelectedIndex != 0)
            {
                iUserID = Convert.ToInt32(ddlSalesPerson.SelectedValue);
            }
        }
        else//Salesperson...
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
        }
        if (lbProspects.SelectedIndex == -1)
        {
            lblErrorNotes.Text = "**No Prospect Selected!";
            lblErrorNotes.ForeColor = Color.Red;
            return;
        }
        int iProspectID = Convert.ToInt32(lbProspects.SelectedValue);
        string sNoteDate = txtNoteDate.Text.Trim();
        string sNotes = txtNotes.Text;
        if (NotesDateAlreadyExists(iUserID, iProspectID, sNoteDate))
        {
            lblErrorNotes.Text = "**Date already exists for this note!";
            lblErrorNotes.ForeColor = Color.Red;
            return;
        }
        if (sNoteDate != "" && sNotes != "")
        {
            AddNotes(iUserID, iProspectID, sNoteDate, sNotes);
            txtNotes.Text = "";
        }
        else
        {
            lblErrorNotes.Text = "**No note date or note entered!";
            lblErrorNotes.ForeColor = Color.Red;
        }

    }
    protected void gvNotes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvNotes.EditIndex = -1;
        gvNotes.DataSource = (DataTable)Session["dtNotes"];
        gvNotes.DataBind();
    }
    protected void gvNotes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int i = 0;
        int iNoteID = 0;
        Label lblNoteID;
        TextBox txtNotes;
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            switch (e.CommandName)
            {


                case "Update":
                    i = Convert.ToInt32(e.CommandArgument);
                    txtNotes = (TextBox)gvNotes.Rows[i].FindControl("txtNotes");
                    lblNoteID = (Label)gvNotes.Rows[i].FindControl("lblNoteID");
                    iNoteID = Convert.ToInt32(lblNoteID.Text);
                    try
                    {

                        WipUserProspectNotes n = db.WipUserProspectNotes.Single(p => p.NoteID == iNoteID);
                        n.Notes = txtNotes.Text.Trim();
                        db.SubmitChanges();


                        lblErrorNotes.Text = "**Note Updated Successfully!";
                        lblErrorNotes.ForeColor = Color.Green;
                    }
                    catch (Exception)
                    {
                        lblErrorNotes.Text = "**Note Update Failed!";
                        lblErrorNotes.ForeColor = Color.Red;
                        return;
                    }

                    break;
                case "Delete":
                    i = Convert.ToInt32(e.CommandArgument);
                    lblNoteID = (Label)gvNotes.Rows[i].FindControl("lblNoteID");
                    iNoteID = Convert.ToInt32(lblNoteID.Text);
                    try
                    {
                        WipUserProspectNotes d = db.WipUserProspectNotes.Single(p => p.NoteID == iNoteID);
                        db.WipUserProspectNotes.DeleteOnSubmit(d);
                        db.SubmitChanges();


                        lblErrorNotes.Text = "**Note Deleted Successfully!";
                        lblErrorNotes.ForeColor = Color.Green;
                    }
                    catch (Exception)
                    {
                        lblErrorNotes.Text = "**Note Deletion Failed!";
                        lblErrorNotes.ForeColor = Color.Red;
                        return;
                    }

                    break;
            }

        }

    }
    protected void gvNotes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (gvNotes.EditIndex != -1)//In Edit Mode...
                {

                    if (gvNotes.EditIndex == e.Row.RowIndex)//edited row...
                    {

                        Label lblNoteDate = (Label)e.Row.FindControl("lblNoteDate");
                        if (lblNoteDate.Text != "")
                        {
                            lblNoteDate.Text = Convert.ToDateTime(lblNoteDate.Text).ToShortDateString();
                        }

                    }
                    else//All Rows not in edit mode while a row is in edit mode...
                    {
                        Label lblNoteDate = (Label)e.Row.FindControl("lblNoteDate");
                        if (lblNoteDate.Text != "")
                        {
                            lblNoteDate.Text = Convert.ToDateTime(lblNoteDate.Text).ToShortDateString();
                        }
                    }
                }
                else//Not Edit Mode...
                {

                    Label lblNoteDate = (Label)e.Row.FindControl("lblNoteDate");
                    if (lblNoteDate.Text != "")
                    {
                        lblNoteDate.Text = Convert.ToDateTime(lblNoteDate.Text).ToShortDateString();
                    }
                }//End Not in Edit Mode..

            }



            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvNotes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

        int iUserID = 0;
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
         if (iRoleID == 1  || iRoleID == 5)//Salesperson...
        {
            if (ddlSalesPerson.SelectedIndex != 0)
            {
                iUserID = Convert.ToInt32(ddlSalesPerson.SelectedValue);
            }
        }
        else//Salesperson...
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
        }
        if (lbProspects.SelectedIndex == -1)
        {
            lblErrorNotes.Text = "**No Prospect Selected!";
            lblErrorNotes.ForeColor = Color.Red;
            return;
        }
        int iProspectID = Convert.ToInt32(lbProspects.SelectedValue);
        string sStartDate = txtStartDate.Text.Trim();
        string sEndDate = txtEndDate.Text.Trim();

        BindNotesGrid(iUserID, iProspectID, sStartDate, sEndDate);

        List<string> lProspects = new List<string>();
        foreach (ListItem liProspect in lbProspects.Items)
        {
            lProspects.Add(liProspect.Value.Trim());
        }
        GetNotesForSalespersonForDateRange(iUserID, lProspects, sStartDate, sEndDate);
    }
    protected void gvNotes_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvNotes.EditIndex = e.NewEditIndex;
        gvNotes.DataSource = (DataTable)Session["dtNotes"];
        gvNotes.DataBind();
    }
    protected void gvNotes_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvNotes.EditIndex = -1;
        int iUserID = 0;
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
         if (iRoleID == 1  || iRoleID == 5)//Salesperson...
        {
            if (ddlSalesPerson.SelectedIndex != 0)
            {
                iUserID = Convert.ToInt32(ddlSalesPerson.SelectedValue);
            }
        }
        else//Salesperson...
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
        }
        if (lbProspects.SelectedIndex == -1)
        {
            lblErrorNotes.Text = "**No Prospect Selected!";
            lblErrorNotes.ForeColor = Color.Red;
            return;
        }
        int iProspectID = Convert.ToInt32(lbProspects.SelectedValue);
        string sStartDate = txtStartDate.Text.Trim();
        string sEndDate = txtEndDate.Text.Trim();

        BindNotesGrid(iUserID, iProspectID, sStartDate, sEndDate);

        List<string> lProspects = new List<string>();
        foreach (ListItem liProspect in lbProspects.Items)
        {
            lProspects.Add(liProspect.Value.Trim());
        }
        GetNotesForSalespersonForDateRange(iUserID, lProspects, sStartDate, sEndDate);
    }
    protected void ddlSalesPerson_SelectedIndexChanged(object sender, EventArgs e)
    {
        int iUserID = 0;
        if (ddlSalesPerson.SelectedIndex != 0)
        {
            iUserID = Convert.ToInt32(ddlSalesPerson.SelectedValue);
            LoadProspectsList("", iUserID);

            string sStartDate = txtStartDate.Text.Trim();
            string sEndDate = txtEndDate.Text.Trim();
            List<string> lProspects = new List<string>();
            foreach (ListItem liProspect in lbProspects.Items)
            {
                lProspects.Add(liProspect.Value.Trim());
            }
            GetNotesForSalespersonForDateRange(iUserID, lProspects, sStartDate, sEndDate);
        }
        else
        {
            Reset();
        }
    }
    protected void imgExportExcel_Click(object sender, ImageClickEventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";
        if (Session["dtNotesExport"] == null)
        {
            lblError.Text = "**No Report in memory or no data for selected month to export!";
            return;
        }
        dt = (DataTable)Session["dtNotesExport"];

        dt.TableName = "dtNotesExport";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "ProspectsNotes" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void btnRun_Click(object sender, EventArgs e)
    {
        //Load Notes...
        int iUserID = 0;
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
         if (iRoleID == 1  || iRoleID == 5)//Salesperson...
        {
            if (ddlSalesPerson.SelectedIndex != 0)
            {
                iUserID = Convert.ToInt32(ddlSalesPerson.SelectedValue);
            }
        }
        else//Salesperson...
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
        }
        if (lbProspects.SelectedIndex == -1)
        {
            lblErrorNotes.Text = "**No Prospect Selected!";
            lblErrorNotes.ForeColor = Color.Red;
            return;
        }
        string sStartDate = txtStartDate.Text.Trim();
        string sEndDate = txtEndDate.Text.Trim();
        int iProspectID = Convert.ToInt32(lbProspects.SelectedValue);
        BindNotesGrid(iUserID, iProspectID, sStartDate, sEndDate);

        List<string> lProspects = new List<string>();
        foreach (ListItem liProspect in lbProspects.Items)
        {
            lProspects.Add(liProspect.Value.Trim());
        }
        GetNotesForSalespersonForDateRange(iUserID, lProspects, sStartDate, sEndDate);
    }
    #endregion

    #region WebMethods
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListUserName(string prefixText, int count, string contextKey)
    {//...
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {

            // Your LINQ to SQL query goes here 
            FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            string[] list = db.WipProspects.Where(w => w.CompanyName != null).OrderBy(w => w.CompanyName).Select(w => w.CompanyName).Distinct().ToArray();
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