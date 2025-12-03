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

public partial class CustomerLeadTimesAdmin : System.Web.UI.Page
{
    #region Subs
    //Admin

    private void LoadCustomers(DropDownList ddl)
    {
        ddl.Items.Clear();
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.ArCustomer
                         orderby c.Name
                         select c);
            if (query.Count() > 0)
            {
                foreach (var a in query)
                {
                    ddl.Items.Add(new ListItem(a.Name + " - " + a.Customer.ToString(), a.Customer.ToString()));
                }
                ddl.Items.Insert(0, new ListItem("SELECT", "0"));
            }
            else
            {
                ddl.Items.Insert(0, new ListItem("No Customers found...", "0"));
            }
        }
    }
    private void LoadAddressCodes(DropDownList ddl, string sCustomer)
    {
        ddl.Items.Clear();
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.ArMultAddress
                         where c.Customer == sCustomer
                         orderby c.AddrCode
                         select c);
            if (query.Count() > 0)
            {
                foreach (var a in query)
                {
                    ddl.Items.Add(new ListItem(a.AddrCode, a.AddrCode.ToString()));
                }
                ddl.Items.Insert(0, new ListItem("SELECT", "0"));                
            }
            else
            {
                ddl.Items.Insert(0, new ListItem("SELECT", "-1"));
                ddl.Items.Insert(1, new ListItem("N/A", "0"));
            }
        }
    }
    private void LoadLeadTimesGrid()
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = new DataTable();

            var query = (from lt in db.ArCustomerLeadTimes
                         join c in db.ArCustomer on lt.Customer equals c.Customer
                         orderby
                           c.Name
                         select new
                         {
                             c.Name,
                             lt.CustomerLeadTimeID,
                             lt.Customer,
                             lt.AddrCode,
                             lt.ContactType,
                             lt.LeadTimeValue,
                             lt.LeadTimeValueType,
                             lt.LeadTimeSource,
                             lt.DateAdded

                         });


            pnlLeadTimes.ScrollBars = ScrollBars.Vertical;
            pnlLeadTimes.Height = Unit.Pixel(500);


            dt = SharedFunctions.ToDataTable(db, query);

            gvLeadTimes.DataSource = dt;
            gvLeadTimes.DataBind();
            Session["dtLeadTimes"] = dt;
            dt.Dispose();
        }
    }


    #endregion

    #region Functions

    private bool CustomerExists(string sCustomer, string sAddressCode)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.ArCustomerLeadTimes
                         where c.Customer == sCustomer
                         && c.AddrCode == sAddressCode
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
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = 0;
        iRoleID = Convert.ToInt32(SharedFunctions.GetRole(iUserID));
        if (iRoleID != 1)//Admin
        {
            Response.Redirect("Default.aspx");
        }
        if (!Page.IsPostBack)
        {
            LoadLeadTimesGrid();
        }
    }

    protected void gvLeadTimes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvLeadTimes.EditIndex != -1)//In Edit Mode...
                {

                    if (gvLeadTimes.EditIndex == e.Row.RowIndex)//edited row...
                    {
                        DropDownList ddlContactTypes = (DropDownList)e.Row.FindControl("ddlContactTypes");
                        DropDownList ddlLeadTimeValueTypes = (DropDownList)e.Row.FindControl("ddlLeadTimeValueTypes");
                        Label lblContactType = (Label)e.Row.FindControl("lblContactType");
                        Label lblLeadTimeValueType = (Label)e.Row.FindControl("lblLeadTimeValueType");
                        ddlContactTypes.SelectedValue = lblContactType.Text;
                        ddlLeadTimeValueTypes.SelectedValue = lblLeadTimeValueType.Text; 

                    }
                    else if (gvLeadTimes.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
                    {

                    }
                }
                else
                {

                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)//the footer row...
            {
                DropDownList ddlCustomers = (DropDownList)e.Row.FindControl("ddlCustomers");
                LoadCustomers(ddlCustomers);

                DropDownList ddlAddressCodes = (DropDownList)e.Row.FindControl("ddlAddressCodes");
                ddlAddressCodes.Items.Clear();
                ddlAddressCodes.Items.Add(new ListItem("Select Customer 1st", "0"));

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvLeadTimes_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvLeadTimes.EditIndex = e.NewEditIndex;
        gvLeadTimes.DataSource = (DataTable)Session["dtLeadTimes"];
        gvLeadTimes.DataBind();

    }
    protected void gvLeadTimes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvLeadTimes.EditIndex = -1;
        gvLeadTimes.DataSource = (DataTable)Session["dtLeadTimes"];
        gvLeadTimes.DataBind();
    }
    protected void gvLeadTimes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvLeadTimes.EditIndex = -1;
        LoadLeadTimesGrid();
    }
    protected void gvLeadTimes_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvLeadTimes.EditIndex = -1;
        LoadLeadTimesGrid();
    }
    protected void gvLeadTimes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            lblError.Text = "";

            int iUserID = 0;

            int i = 0;
            int iCustomerLeadTimeID = 0;
            Label lblCustomerLeadTimeID;
            DropDownList ddlCustomers;
            DropDownList ddlAddressCodes;
            DropDownList ddlContactTypes;
            DropDownList ddlLeadTimeValueTypes;
            TextBox txtLeadTimeValue;
            TextBox txtLeadTimeSource;
            if (Session["UserID"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            iUserID = Convert.ToInt32(Session["UserID"]);


            switch (e.CommandName)
            {
                case "Add":
                    lblError.Text = "";
                    ddlCustomers = (DropDownList)gvLeadTimes.FooterRow.FindControl("ddlCustomers");
                    ddlAddressCodes = (DropDownList)gvLeadTimes.FooterRow.FindControl("ddlAddressCodes");
                    ddlContactTypes = (DropDownList)gvLeadTimes.FooterRow.FindControl("ddlContactTypes");
                    ddlLeadTimeValueTypes = (DropDownList)gvLeadTimes.FooterRow.FindControl("ddlLeadTimeValueTypes");
                    txtLeadTimeValue = (TextBox)gvLeadTimes.FooterRow.FindControl("txtLeadTimeValue");
                    txtLeadTimeSource = (TextBox)gvLeadTimes.FooterRow.FindControl("txtLeadTimeSource");
                    if (ddlCustomers.SelectedIndex == 0)
                    {
                        sMsg += "**Please select a Customer!<br/>";
                    }
                    if (ddlAddressCodes.SelectedIndex == 0)
                    {
                        sMsg += "**Please select a Address Code!<br/>";
                    }
                    if (ddlContactTypes.SelectedIndex == 0)
                    {
                        sMsg += "**Please select a Contact Type!<br/>";
                    }
                    if (ddlLeadTimeValueTypes.SelectedIndex == 0)
                    {
                        sMsg += "**Please select a Lead Time Value Type!<br/>";
                    }
                    if (txtLeadTimeValue.Text.Trim() == "")
                    {
                        sMsg += "**Lead Time Value is required!<br/>";
                    }
                    else
                    {
                        if (!SharedFunctions.IsNumeric(txtLeadTimeValue.Text.Trim()))
                        {
                            sMsg += "**Lead Time Value must be a numeric value!<br/>";
                        }
                    }
                    if (txtLeadTimeSource.Text.Trim() == "")
                    {
                        sMsg += "**Lead Time Source is required!<br/>";
                    }
                    else
                    {
                        if (ddlContactTypes.SelectedIndex != 0)
                        {
                            switch (ddlContactTypes.SelectedValue)
                            {
                                case "EMAIL":
                                    if (SharedFunctions.IsEmail(txtLeadTimeSource.Text.Trim()) == false)
                                    {
                                        sMsg += "**If Email is selected as contact type you must enter a valid email!<br/>";
                                    }
                                    break;
                                case "PHONE":
                                    if (SharedFunctions.IsNumeric(txtLeadTimeSource.Text.Trim()) == false)
                                    {
                                        sMsg += "**If phone is selected as contact type you must enter a valid phone number without dashes, dots, brackets or spaces!<br/>";
                                    }
                                    break;
                                case "WEBSITE":
                                    try
                                    {
                                        using (System.Net.WebClient client = new System.Net.WebClient())
                                        {//Check to bad URL...
                                            string s2 = client.DownloadString("http://" + txtLeadTimeSource.Text.Trim());
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        sMsg += "**If website is selected as contact type you must enter a valid website URL!<br/>";
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            sMsg += "**Please select a Contact Type!<br/>";
                        }

                    }
                    if (ddlCustomers.SelectedIndex != 0)
                    {
                        if (CustomerExists(ddlCustomers.SelectedValue, ddlAddressCodes.SelectedValue))
                        {
                            sMsg += "**Customer Name already exists in the Lead Times Table with the selected Address Code!<br/>";
                        }
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
                        ArCustomerLeadTimes s = new ArCustomerLeadTimes();
                        s.Customer = ddlCustomers.SelectedValue;
                        s.AddrCode = ddlAddressCodes.SelectedValue;
                        s.ContactType = ddlContactTypes.SelectedValue;
                        s.LeadTimeValueType = ddlLeadTimeValueTypes.SelectedValue;
                        s.LeadTimeValue = Convert.ToInt32(txtLeadTimeValue.Text);
                        s.LeadTimeSource = txtLeadTimeSource.Text.Trim();
                        s.DateAdded = DateTime.Now;
                        db.ArCustomerLeadTimes.InsertOnSubmit(s);
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
                    LoadLeadTimesGrid();//Refresh...                
                    break;
                case "Update":
                    lblError.Text = "";

                    i = Convert.ToInt32(e.CommandArgument);
                    lblError.Text = "";
                    ddlContactTypes = (DropDownList)gvLeadTimes.Rows[i].FindControl("ddlContactTypes");
                    ddlLeadTimeValueTypes = (DropDownList)gvLeadTimes.Rows[i].FindControl("ddlLeadTimeValueTypes");
                    txtLeadTimeValue = (TextBox)gvLeadTimes.Rows[i].FindControl("txtLeadTimeValue");
                    txtLeadTimeSource = (TextBox)gvLeadTimes.Rows[i].FindControl("txtLeadTimeSource");
                    lblCustomerLeadTimeID = (Label)gvLeadTimes.Rows[i].FindControl("lblCustomerLeadTimeID");
                    iCustomerLeadTimeID = Convert.ToInt32(lblCustomerLeadTimeID.Text);

                    if (ddlContactTypes.SelectedIndex == 0)
                    {
                        sMsg += "**Please select a Contact Type!<br/>";
                    }
                    if (ddlLeadTimeValueTypes.SelectedIndex == 0)
                    {
                        sMsg += "**Please select a Lead Time Value Type!<br/>";
                    }
                    if (txtLeadTimeValue.Text.Trim() == "")
                    {
                        sMsg += "**Lead Time Value is required!<br/>";
                    }
                    else
                    {
                        if (!SharedFunctions.IsNumeric(txtLeadTimeValue.Text.Trim()))
                        {
                            sMsg += "**Lead Time Value must be a numeric value!<br/>";
                        }
                    }
                    if (txtLeadTimeSource.Text.Trim() == "")
                    {
                        sMsg += "**Lead Time Source is required!<br/>";
                    }
                    else
                    {
                        if (ddlContactTypes.SelectedIndex != 0)
                        {
                            switch (ddlContactTypes.SelectedValue)
                            {
                                case "EMAIL":
                                    if (SharedFunctions.IsEmail(txtLeadTimeSource.Text.Trim()) == false)
                                    {
                                        sMsg += "**If Email is selected as contact type you must enter a valid email!<br/>";
                                    }
                                    break;
                                case "PHONE":
                                    if (SharedFunctions.IsNumeric(txtLeadTimeSource.Text.Trim()) == false)
                                    {
                                        sMsg += "**If phone is selected as contact type you must enter a valid phone number without dashes, dots, brackets or spaces!<br/>";
                                    }
                                    break;
                                case "WEBSITE":
                                    try
                                    {
                                        using (System.Net.WebClient client = new System.Net.WebClient())
                                        {//Check to bad URL...
                                            string s2 = client.DownloadString("http://" + txtLeadTimeSource.Text.Trim());
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        sMsg += "**If website is selected as contact type you must enter a valid website URL!<br/>";
                                    }
                                    break;
                            }
                        }
                        else
                        {
                            sMsg += "**Please select a Contact Type!<br/>";
                        }

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
                        ArCustomerLeadTimes s = db.ArCustomerLeadTimes.Single(p => p.CustomerLeadTimeID == iCustomerLeadTimeID);
                        s.ContactType = ddlContactTypes.SelectedValue;
                        s.LeadTimeValueType = ddlLeadTimeValueTypes.SelectedValue;
                        s.LeadTimeValue = Convert.ToInt32(txtLeadTimeValue.Text);
                        s.LeadTimeSource = txtLeadTimeSource.Text.Trim();
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
                        lblCustomerLeadTimeID = (Label)gvLeadTimes.Rows[i].FindControl("lblCustomerLeadTimeID");
                        iCustomerLeadTimeID = Convert.ToInt32(lblCustomerLeadTimeID.Text);
                        var query = (from d in db.ArCustomerLeadTimes select d);
                        if (query.Count() == 1)
                        {
                            lblError.Text = "**You cannot delete the last record in the table.";
                            lblError.ForeColor = Color.Red;
                            return;
                        }

                        ArCustomerLeadTimes s = db.ArCustomerLeadTimes.Single(p => p.CustomerLeadTimeID == iCustomerLeadTimeID);
                        db.ArCustomerLeadTimes.DeleteOnSubmit(s);
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
    }
    protected void ddlCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList ddlCustomers = (DropDownList)sender;
        GridViewRow gvr = (GridViewRow)ddlCustomers.Parent.Parent;
        DropDownList ddlAddressCodes = (DropDownList)gvr.FindControl("ddlAddressCodes");
        if (ddlCustomers.SelectedIndex != 0)
        {
            LoadAddressCodes(ddlAddressCodes, ddlCustomers.SelectedValue);
        }
        else
        {
            ddlAddressCodes.Items.Clear();
            ddlAddressCodes.Items.Add(new ListItem("Select Customer 1st","0"));
        }
    }




    #endregion


}