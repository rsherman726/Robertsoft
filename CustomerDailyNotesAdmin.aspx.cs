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

public partial class CustomerDailyNotesAdmin : System.Web.UI.Page
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
    private void LoadCustomersList(string sSearch, string sSalesperson)
    {
        try
        {

            using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
            {
                lbCustomers.Items.Clear();
                var query = (from c in db.ArCustomer
                             where c.Salesperson == sSalesperson
                             && c.Customer != "test"
                                   &&
                                (
                               c.Name.Contains(sSearch) ||
                               c.Email.Contains(sSearch) ||
                               sSearch == null)
                             orderby c.Name
                             select new
                             {
                                 c.Name,
                                 c.Contact,
                                 Address = c.SoldToAddr1,
                                 City = c.SoldToAddr4,
                                 State = c.SoldToAddr5,
                                 PostalCode = c.SoldPostalCode,
                                 Phone = c.Telephone,
                                 c.TelephoneExtn,
                                 c.Email,
                                 c.Customer,

                             });
                if (query.Count() > 0)
                {
                    foreach (var a in query)
                    {

                        string sDisplay = "";

                        sDisplay = a.Name.ToUpper() + " - " + a.Customer;

                        ListItem li = new ListItem();
                        li.Text = sDisplay;
                        li.Value = a.Customer.Trim();
                        lbCustomers.Items.Add(li);
                    }
                }
                else
                {
                    lblError.Text = "**No Records found!";
                    lblError0.Text = "**No Records found!";
                    lblError.ForeColor = Color.Red;
                    lblError0.ForeColor = Color.Red;

                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    private void BindNotesGrid(int iUserID, List<string> lCustomers, string sStartDate, string sEndDate)
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from n in db.WipUserCustomerNotes
                         join c in db.ArCustomer on n.Customer equals c.Customer
                         join u in db.WipUsers on n.WipUserID equals u.UserID
                         where n.WipUserID == iUserID
                          && lCustomers.Contains(n.Customer.Trim())
                         && (n.NoteDate >= Convert.ToDateTime(sStartDate + " 00:00:00") && n.NoteDate <= Convert.ToDateTime(sEndDate + " 23:59:59"))
                         orderby n.NoteDate descending
                         select new
                         {
                             n.NoteID,
                             Salesperson = (u.FirstName + " " + (u.MiddleName ?? "") + " " + u.LastName).Replace("  ", " "),
                             c.Name,
                             c.Contact,
                             Address = c.SoldToAddr1,
                             City = c.SoldToAddr4,
                             State = c.SoldToAddr5,
                             PostalCode = c.SoldPostalCode,
                             Phone = c.Telephone,
                             c.TelephoneExtn,
                             c.Email,
                             n.NoteDate,
                             n.Notes,
                             c.Customer,
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
    private void AddNotes(int iWipUserID, string sCustomer, string sNoteDate, string sNotes)
    {

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {


                WipUserCustomerNotes n = new WipUserCustomerNotes();
                n.WipUserID = iWipUserID;
                n.Customer = sCustomer;
                n.Notes = sNotes;
                n.NoteDate = Convert.ToDateTime(sNoteDate);
                n.DateAdded = DateTime.Now;
                db.WipUserCustomerNotes.InsertOnSubmit(n);
                db.SubmitChanges();

                lblErrorNotes.Text = "**Note Inserted Successfully!";
                lblErrorNotes.ForeColor = Color.Green;

                //Load Notes...
                int iUserID = 0;
                int iRoleID = Convert.ToInt32(Session["RoleID"]);
                if (iRoleID == 1)//Admin
                {
                    if (ddlSalesPerson.SelectedIndex != 0)
                    {
                        iUserID = GetWipUserID(ddlSalesPerson.SelectedValue);
                    }
                }
                else//Salesperson...
                {
                    iUserID = Convert.ToInt32(Session["UserID"]);
                }
                if (lbCustomers.SelectedIndex == -1)
                {
                    lblErrorNotes.Text = "**No Customer Selected!";
                    lblErrorNotes.ForeColor = Color.Red;
                    return;
                }
                //Load Notes...            
                GetBindNotesGrid(iUserID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                lblErrorNotes.Text = "**Note Insert Failed!";
                lblErrorNotes.ForeColor = Color.Red;
                return;
            }
        }

    }
    private void GetBindNotesGrid(int iUserID)
    {
            //Load Notes...            
            string sStartDate = txtStartDate.Text.Trim();
            string sEndDate = txtEndDate.Text.Trim();
            List<string> lCustomer = new List<string>();
            foreach (ListItem liCustomer in lbCustomers.Items)
            {
                if (liCustomer.Selected)
                {
                    lCustomer.Add(liCustomer.Value.Trim());
                }
            }
            BindNotesGrid(iUserID, lCustomer, sStartDate, sEndDate);
    }
    private void LoadSalesPersons()
    {
        //ddlSalesPerson.Items.Clear();
        //FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        //var query = (from ss in db.SalSalesperson
        //             where
        //                   ((from c in db.ArCustomer
        //                     select new
        //                     {
        //                         c.Salesperson
        //                     }).Distinct()).Contains(new { Salesperson = ss.Salesperson.ToString() })
        //             select new
        //             {
        //                 ss.Name,
        //                 ss.Salesperson
        //             });
        //foreach (var a in query)
        //{
        //    ddlSalesPerson.Items.Add(new ListItem(a.Name, a.Salesperson));
        //}
        ddlSalesPerson.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from c in db.WipUsers
                     where c.RoleID == 4//Employee...
                     && c.Salesperson != null
                     orderby c.FirstName, c.LastName
                     select new
                     {
                         Name = (c.FirstName + " " + (c.MiddleName ?? "") + " " + c.LastName).Replace("  ", " "),
                         c.UserID,
                         c.Salesperson

                     });
        foreach (var a in query)
        {
            string sName = "";

            if (a.Name == "Hou Se")
            {
                sName = "HOUSE";
            }
            else
            {
                sName = a.Name;
            }

            ddlSalesPerson.Items.Add(new ListItem(sName + " - " + a.UserID.ToString() + " SP: " + a.Salesperson, a.Salesperson));
        }
        ddlSalesPerson.Items.Insert(0, new ListItem("Select a Salesperson", "0"));
    }

    #endregion

    #region Functions
    public static bool NotesDateAlreadyExists(int iWipUserID, string sCustomerID, string sNoteDate)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from n in db.WipUserCustomerNotes
                       where n.WipUserID == iWipUserID
                       && n.Customer == sCustomerID
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
    private int GetWipUserID(string sSalesperson)
    {
        int iWipUserID = 0;
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from n in db.WipUsers
                       where n.Salesperson == sSalesperson
                       select n);
            foreach (var a in qry)
            {
                iWipUserID = a.UserID;
            }
            return iWipUserID;
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
        iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);

        if (!Page.IsPostBack)
        {
            txtNoteDate.Text = DateTime.Now.ToShortDateString();
            txtStartDate.Text = DateTime.Now.Month.ToString() + "/01/" + DateTime.Now.Year.ToString();
            txtEndDate.Text = DateTime.Now.Month.ToString() + "/" + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + "/" + DateTime.Now.Year.ToString();

            ddlSalesPerson.Visible = true;
            LoadSalesPersons();
        }

    }
    protected void ibnSearch_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblError0.Text = "";
        string sSalesperson = ddlSalesPerson.SelectedValue;

        if (ddlSalesPerson.SelectedIndex != 0)
        {
            LoadCustomersList("", sSalesperson);
        }


        LoadCustomersList(txtSearch.Text.Trim(), sSalesperson);
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblError0.Text = "";
        string sSalesperson = ddlSalesPerson.SelectedValue;

        if (ddlSalesPerson.SelectedIndex != 0)
        {
            LoadCustomersList("", sSalesperson);
        }
        LoadCustomersList(txtSearch.Text.Trim(), sSalesperson);
    }
    protected void lbCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrorNotes.Text = "";
        lblError.Text = "";
        lblError0.Text = "";
        if (lbCustomers.SelectedIndex != -1)
        {
            int iUserID = 0;

            if (ddlSalesPerson.SelectedIndex != 0)
            {
                iUserID = GetWipUserID(ddlSalesPerson.SelectedValue);
            }

            GetBindNotesGrid(iUserID);
            lbnAdd.Enabled = true;
            tblNotes.Visible = true;
        }
        else
        {
            lbnAdd.Enabled = false;
            tblNotes.Visible = false;
        }
    }
    protected void lbnAdd_Click(object sender, EventArgs e)
    {// Add Notes

        int iUserID = 0;

        if (ddlSalesPerson.SelectedIndex != 0)
        {
            iUserID = GetWipUserID(ddlSalesPerson.SelectedValue);
        }

        if (lbCustomers.SelectedIndex == -1)
        {
            lblErrorNotes.Text = "**No Customer Selected!";
            lblErrorNotes.ForeColor = Color.Red;
            return;
        }
        string sCustomer = lbCustomers.SelectedValue;
        string sNoteDate = txtNoteDate.Text.Trim();
        string sNotes = txtNotes.Text;
        if (NotesDateAlreadyExists(iUserID, sCustomer, sNoteDate))
        {
            lblErrorNotes.Text = "**Date already exists for this note!";
            lblErrorNotes.ForeColor = Color.Red;
            return;
        }
        if (lbCustomers.GetSelectedIndices().Count() > 1)
        {
            lblErrorNotes.Text = "**You must select a single company to add a note!";
            lblErrorNotes.ForeColor = Color.Red;
            return;
        }
        if (sNoteDate != "" && sNotes != "")
        {
            AddNotes(iUserID, sCustomer, sNoteDate, sNotes);
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

                        WipUserCustomerNotes n = db.WipUserCustomerNotes.Single(p => p.NoteID == iNoteID);
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
                        WipUserCustomerNotes d = db.WipUserCustomerNotes.Single(p => p.NoteID == iNoteID);
                        db.WipUserCustomerNotes.DeleteOnSubmit(d);
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
        if (ddlSalesPerson.SelectedIndex != 0)
        {
            iUserID = GetWipUserID(ddlSalesPerson.SelectedValue);
        }
        if (lbCustomers.SelectedIndex == -1)
        {
            lblErrorNotes.Text = "**No Customer Selected!";
            lblErrorNotes.ForeColor = Color.Red;
            return;
        }
        int iCustomerID = Convert.ToInt32(lbCustomers.SelectedValue);
        string sStartDate = txtStartDate.Text.Trim();
        string sEndDate = txtEndDate.Text.Trim();
        List<string> lCustomer = new List<string>();
        foreach (ListItem liCustomer in lbCustomers.Items)
        {
            if (liCustomer.Selected)
            {
                lCustomer.Add(liCustomer.Value.Trim());
            }
        }
        BindNotesGrid(iUserID, lCustomer, sStartDate, sEndDate);
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

        if (ddlSalesPerson.SelectedIndex != 0)
        {
            iUserID = GetWipUserID(ddlSalesPerson.SelectedValue);
        }

        if (lbCustomers.SelectedIndex == -1)
        {
            lblErrorNotes.Text = "**No Customer Selected!";
            lblErrorNotes.ForeColor = Color.Red;
            return;
        }
        int iCustomerID = Convert.ToInt32(lbCustomers.SelectedValue);
        string sStartDate = txtStartDate.Text.Trim();
        string sEndDate = txtEndDate.Text.Trim();
        List<string> lCustomer = new List<string>();
        foreach (ListItem liCustomer in lbCustomers.Items)
        {
            if (liCustomer.Selected)
            {
                lCustomer.Add(liCustomer.Value.Trim());
            }
        }
        BindNotesGrid(iUserID, lCustomer, sStartDate, sEndDate);
    }
    protected void ddlSalesPerson_SelectedIndexChanged(object sender, EventArgs e)
    {//Admins only...
        if (ddlSalesPerson.SelectedIndex != 0)
        {
            string sSalesperson = ddlSalesPerson.SelectedValue;

            if (ddlSalesPerson.SelectedIndex != 0)
            {
                LoadCustomersList("", sSalesperson);
            }
            tblNotes.Visible = false;

        }
        else
        {
            lbCustomers.Items.Clear();
            tblNotes.Visible = false;
        }
    }
    protected void imgExportExcel_Click(object sender, ImageClickEventArgs e)
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        string sFilesName = "";
        if (Session["dtNotes"] == null)
        {
            lblError.Text = "**No Report in memory or data for select month to export!";
            return;
        }
        dt = (DataTable)Session["dtNotes"];

        dt.TableName = "dtNotes";
        if (ds.Tables.Count == 0)
        {
            ds.Tables.Add(dt.Copy());
        }

        sFilesName = "CustomerNotes" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExportToExcel(ds, sFilesName);

        //send session variable dtReport to Excel...

        ds.Dispose();
    }
    protected void btnRun_Click(object sender, EventArgs e)
    {
        int iUserID = 0;
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
        if (iRoleID == 1)//Admin
        {
            if (ddlSalesPerson.SelectedIndex != 0)
            {
                iUserID = GetWipUserID(ddlSalesPerson.SelectedValue);
            }
        }
        else//Salesperson...
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
        }
        if (lbCustomers.SelectedIndex == -1)
        {
            lblErrorNotes.Text = "**No Customer Selected!";
            lblErrorNotes.ForeColor = Color.Red;
            return;
        }
        //Load Notes...            
        GetBindNotesGrid(iUserID);
    }
    protected void lbSelectAll_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbCustomers.Items)
        {
            li.Selected = true;
        }
        lbCustomers_SelectedIndexChanged(lbCustomers, null);
    }
    protected void lbClearAll_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbCustomers.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
        lbCustomers_SelectedIndexChanged(lbCustomers, null);
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
            string[] list = db.ArCustomer.Where(w => w.Name != null).OrderBy(w => w.Name).Select(w => w.Name).Distinct().ToArray();
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