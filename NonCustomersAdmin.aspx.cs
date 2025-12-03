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

public partial class NonCustomersAdmin : System.Web.UI.Page
{
    #region Subs
    //Admin
    private void LoadSalesPersons()
    {
        ddlSalesPerson.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from c in db.SalSalesperson
                     orderby c.Name
                     select new
                     {
                         c.Name,
                         c.Salesperson

                     });
        foreach (var a in query)
        {
            ddlSalesPerson.Items.Add(new ListItem(a.Name, a.Salesperson));
        }
        ddlSalesPerson.Items.Insert(0, new ListItem("SELECT", ""));
    }
    private void LoadNonCustomers(DropDownList ddl)
    {
        ddl.Items.Clear();
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from c in db.ArNonCustomer
                     orderby c.Name
                     select c);
        if (query.Count() > 0)
        {
            foreach (var a in query)
            {
                ddl.Items.Add(new ListItem(a.Name, a.NonCustomerID.ToString()));
            }
            ddl.Items.Insert(0, new ListItem("SELECT", "0"));
        }
        else
        {
            ddl.Items.Insert(0, new ListItem("No End Customers found...", "0"));
        }
    }
    private void AddProfile(int iUserID)
    {
        string sSalesperson = ddlSalesPerson.SelectedValue;
        string sNonCustomerName = txtNonCustomerName.Text.Trim();
        string sMsg = "";
        //Validate data...


        if (txtNonCustomerName.Text.Trim() == "")
        {
            sMsg += "**Please input a Customer name!<br/>";
        }
        if (ddlSalesPerson.SelectedIndex == 0)
        {
            sMsg += "**Please select a Salesperson!<br/>";
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
            //create a contestant's account...
            FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            ArNonCustomer u = new ArNonCustomer();
            u.Name = sNonCustomerName.ToUpper();
            u.Salesperson = sSalesperson;
            u.DateAdded = DateTime.Now;

            db.ArNonCustomer.InsertOnSubmit(u);
            db.SubmitChanges();


            lblError.Text = "**Client Added successfully!";
            lblError.ForeColor = Color.Green;
            LoadNonCustomerGrid();
            Reset();
        }
        catch (Exception ex)
        {
            lblError.Text = "**Client Added failed!";
            lblError.ForeColor = Color.Red;
            Debug.WriteLine(ex.ToString());
        }
    }
    private void UpdateProfile(int iUserID, int iSelectedNonCustomerID)
    {
        if (Page.IsValid == false)
        {
            return;
        }

        string sNonCustomerName = txtNonCustomerName.Text.Trim();
        string sSalesperson = ddlSalesPerson.SelectedValue;
        string sMsg = "";
        //Validate data...

        if (ddlNonCustomersMain.SelectedIndex == 0)
        {
            sMsg += "**Please select a Customer!<br/>";
        }
        if (txtNonCustomerName.Text.Trim() == "")
        {
            sMsg += "**Please input a Customer name!<br/>";
        }
        if (ddlSalesPerson.SelectedIndex == 0)
        {
            sMsg += "**Please select a Salesperson!<br/>";
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

        int iNonCustomerID = 0;
        iNonCustomerID = Convert.ToInt32(ddlNonCustomersMain.SelectedValue);
        try
        {
            //create a contestant's account...
            FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            ArNonCustomer u = db.ArNonCustomer.Single(p => p.NonCustomerID == iSelectedNonCustomerID);  
            u.Name = sNonCustomerName.ToUpper();
            u.Salesperson = sSalesperson;
            db.SubmitChanges();


            BindProfile(iSelectedNonCustomerID);
            LoadNonCustomers(ddlNonCustomersMain);
            ddlNonCustomersMain.SelectedIndex = SharedFunctions.GetSelIndex(txtNonCustomerName.Text.Trim().ToUpper(), ddlNonCustomersMain, "Text");

            LoadNonCustomerGrid();
            lblError.Text = "**Client updated successfully!";
            lblError.ForeColor = Color.Green;

        }
        catch (Exception ex)
        {
            lblError.Text = "**Client updated failed!";
            lblError.ForeColor = Color.Red;
            Debug.WriteLine(ex.ToString());
        }
    }
    private void DeleteProfile(int iUserID, int iSelectedNonCustomerID)
    {
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        try
        {
            ArNonCustomer u = db.ArNonCustomer.Single(p => p.NonCustomerID == iSelectedNonCustomerID);
            db.ArNonCustomer.DeleteOnSubmit(u);
            db.SubmitChanges();
            LoadNonCustomers(ddlNonCustomersMain);

            lblError.Text = "**Client Deleted successfully!";
            lblError.ForeColor = Color.Green;
            LoadNonCustomerGrid();
            Reset();
        }
        catch (Exception ex)
        {
            lblError.Text = "**Client Delete failed!(You can't delete a user who is associated with another table.e.g. Properties)";
            lblError.ForeColor = Color.Red;
            Debug.WriteLine(ex.ToString());
        }
    }
    private void BindProfile(int iNonCustomerID)
    {
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from c in db.ArNonCustomer
                     where c.NonCustomerID == iNonCustomerID
                     select c);
        foreach (var a in query)
        {

            txtNonCustomerName.Text = a.Name;
            lblNonCustomerIDDisplay.Text = a.NonCustomerID.ToString();
            ddlSalesPerson.SelectedValue = a.Salesperson;
        }
    }
    private void Reset()
    {
        txtNonCustomerName.Text = "";
        lblNonCustomerIDDisplay.Text = "";
        ddlSalesPerson.SelectedIndex = 0;
    }
    //Assignments...
    private void LoadNonCustomerGrid()
    {
        DataTable dt = new DataTable();
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from d in db.ArNonCustomerStockCodeAssignments
                     orderby
                       d.ArNonCustomer.Name, d.StockCode
                     select new
                     {
                         d.NonCustomerStockCodeAssignID,
                         d.NonCustomerID,
                         d.ArNonCustomer.Name,
                         d.StockCode,
                         d.StockDescription,

                     });
        dt = SharedFunctions.ToDataTable(db, query);
        if (ddlDisplayCount.SelectedValue != "ALL")
        {
            gvNonCustomer.PageSize = Convert.ToInt32(ddlDisplayCount.SelectedValue);
        }
        else
        {
            gvNonCustomer.PageSize = dt.Rows.Count;
        }
        gvNonCustomer.DataSource = dt;
        gvNonCustomer.DataBind();
        Session["dtNonCustomer"] = dt;
        dt.Dispose();
    }
    private void LoadStockCode(DropDownList ddl)
    {
        ddl.Items.Clear();
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from w in db.InvMaster
                     group w by new
                     {
                         w.StockCode,
                         w.Description
                     } into g
                     orderby
                       g.Key.StockCode
                     select new
                     {
                         g.Key.StockCode,
                         g.Key.Description
                     });
        foreach (var a in query)
        {
            ddl.Items.Add(new ListItem(a.StockCode.Trim() + " - " + a.Description.Trim(), a.StockCode.Trim()));
        }
        ddl.Items.Insert(0, new ListItem("Select", "0"));
    }

    #endregion

    #region Functions

    private bool NameStockCodeComboExists(int iNonCustomerID, string sStockCode)
    {
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from c in db.ArNonCustomerStockCodeAssignments
                     where c.NonCustomerID == iNonCustomerID
                     && c.StockCode == sStockCode
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
    private bool NameStockCodeComboExistsForUpdate(int iNonCustomerID, string sStockCode, int iNonCustomerStockCodeAssignID)
    {
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from c in db.ArNonCustomerStockCodeAssignments
                     where c.NonCustomerID == iNonCustomerID
                     && c.StockCode == sStockCode
                     && c.NonCustomerStockCodeAssignID != iNonCustomerStockCodeAssignID
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
            LoadSalesPersons();
            LoadNonCustomers(ddlNonCustomersMain);
            LoadNonCustomerGrid();
            lblPageNo.Text = "Current Page #: 1";
        }
    }
    //Admin....
    protected void ibnSave_Click(object sender, EventArgs e)
    {
        //Save Profile...
        lblError.Text = "";
        int iUserID = 0;
        int iSelectedNonCustomerID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (ddlNonCustomersMain.SelectedIndex == 0)
        {
            lblError.Text = "**Please selected a Client.";
            lblError.ForeColor = Color.Red;
            return;
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        iSelectedNonCustomerID = Convert.ToInt32(ddlNonCustomersMain.SelectedValue);
        UpdateProfile(iUserID, iSelectedNonCustomerID);
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

        AddProfile(iUserID);
    }
    protected void ibnDelete_Click(object sender, EventArgs e)
    {//Delete Profile...
        lblError.Text = "";
        int iUserID = 0;
        int iSelectedNonCustomerID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (ddlNonCustomersMain.SelectedIndex == 0)
        {
            lblError.Text = "**Please selected a Client.";
            lblError.ForeColor = Color.Red;
            return;
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        iSelectedNonCustomerID = Convert.ToInt32(ddlNonCustomersMain.SelectedValue);
        DeleteProfile(iUserID, iSelectedNonCustomerID);
    }
    protected void ddlNonCustomers_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iNonCustomerID = 0;
        if (ddlNonCustomersMain.SelectedIndex != 0)
        {
            iNonCustomerID = Convert.ToInt32(ddlNonCustomersMain.SelectedValue);
            ibnSave.Enabled = true;
            ibnDelete.Enabled = true;
            Reset();
            BindProfile(iNonCustomerID);
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
        int iNonCustomerID = 0;
        if (rblMode.SelectedIndex == 0)//Add...
        {
            Reset();
            ibnAdd.Enabled = true;
            ibnSave.Enabled = false;
            ibnDelete.Enabled = false;
            ddlNonCustomersMain.Visible = false;


        }
        else//Edit...
        {
            ibnAdd.Enabled = false;
            ddlNonCustomersMain.Visible = true;
            LoadNonCustomers(ddlNonCustomersMain);
            if (ddlNonCustomersMain.SelectedIndex != 0)
            {
                iNonCustomerID = Convert.ToInt32(ddlNonCustomersMain.SelectedValue);
                ibnSave.Enabled = true;
                ibnDelete.Enabled = true;
                BindProfile(iNonCustomerID);
            }
            else
            {
                Reset();
                ibnSave.Enabled = false;
                ibnDelete.Enabled = false;
            }

        }
    }

    //Assignments...
    protected void gvNonCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvNonCustomer.EditIndex != -1)//In Edit Mode...
                {

                    if (gvNonCustomer.EditIndex == e.Row.RowIndex)//edited row...
                    {
                        Label lblStockCode = (Label)e.Row.FindControl("lblStockCode");                        
                        Label lblStockDescription = (Label)e.Row.FindControl("lblStockDescription");

                        DropDownList ddlNonCustomers = (DropDownList)e.Row.FindControl("ddlNonCustomers");
                        Label lblNonCustomerID = (Label)e.Row.FindControl("lblNonCustomerID");
                        LoadNonCustomers(ddlNonCustomers);
                        ddlNonCustomers.SelectedValue = lblNonCustomerID.Text;

                        string sStockCodePlusDesc = "";
                        sStockCodePlusDesc = lblStockCode.Text.Trim() + " - " + lblStockDescription.Text.Trim();
                        DropDownList ddlStockCodes = (DropDownList)e.Row.FindControl("ddlStockCodes");
                        LoadStockCode(ddlStockCodes);
                        ddlStockCodes.SelectedValue = lblStockCode.Text.Trim();                     


                    }
                    else if (gvNonCustomer.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
                    {

                    }
                }
                else
                {

                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)//the footer row...
            {
                DropDownList ddlStockCodesAdd = (DropDownList)e.Row.FindControl("ddlStockCodesAdd");
                LoadStockCode(ddlStockCodesAdd);

                DropDownList ddlNonCustomersAdd = (DropDownList)e.Row.FindControl("ddlNonCustomersAdd");
                LoadNonCustomers(ddlNonCustomersAdd);

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvNonCustomer_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvNonCustomer.EditIndex = e.NewEditIndex;
        gvNonCustomer.DataSource = (DataTable)Session["dtNonCustomer"];
        gvNonCustomer.DataBind();

    }
    protected void gvNonCustomer_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvNonCustomer.EditIndex = -1;
        gvNonCustomer.DataSource = (DataTable)Session["dtNonCustomer"];
        gvNonCustomer.DataBind();
    }
    protected void gvNonCustomer_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvNonCustomer.EditIndex = -1;
        LoadNonCustomerGrid();
    }
    protected void gvNonCustomer_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvNonCustomer.EditIndex = -1;
        LoadNonCustomerGrid();
    }
    protected void gvNonCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvNonCustomer.PageIndex = e.NewPageIndex;
        lblPageNo.Text = "Current Page #: " + (gvNonCustomer.PageIndex + 1).ToString();
        gvNonCustomer.DataSource = (DataTable)Session["dtNonCustomer"];
        gvNonCustomer.DataBind();
    }
    protected void gvNonCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sMsg = "";
        lblError.Text = "";
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        int iUserID = 0;
        int iNonCustomerID = 0;
        int iNonCustomerStockCodeAssignID = 0;
        string sStockDescription = "";
        int i = 0;
        Label lblNonCustomerStockCodeAssignID;
        Label lblNonCustomerID;
        DropDownList ddlStockCodes;
        DropDownList ddlNonCustomers;
        DropDownList ddlStockCodesAdd;
        DropDownList ddlNonCustomersAdd;
 
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        iUserID = Convert.ToInt32(Session["UserID"]);


        switch (e.CommandName)
        {
            case "Add":
                lblError.Text = "";
                ddlStockCodesAdd = (DropDownList)gvNonCustomer.FooterRow.FindControl("ddlStockCodesAdd");
                ddlNonCustomersAdd = (DropDownList)gvNonCustomer.FooterRow.FindControl("ddlNonCustomersAdd");

                if (ddlStockCodesAdd.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Stock Code.";         
                }
                else
                {
                    int iIndexOf = 0;
                    int iLengthMinus = 0;

                    iIndexOf = ddlStockCodesAdd.SelectedItem.Text.IndexOf("-") + 1;
                    iLengthMinus = ddlStockCodesAdd.SelectedItem.Text.Length - iIndexOf;
                    sStockDescription = ddlStockCodesAdd.SelectedItem.Text.Substring(iIndexOf, iLengthMinus).Trim();
                }
                if (ddlNonCustomersAdd.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Non Customer!";                    
                }
                else
                {
                    iNonCustomerID = Convert.ToInt32(ddlNonCustomersAdd.SelectedValue);
                }

                if (ddlStockCodesAdd.SelectedIndex != 0 && ddlNonCustomersAdd.SelectedIndex != 0)
                {
                    if (NameStockCodeComboExists(iNonCustomerID, ddlStockCodesAdd.SelectedValue))
                    {
                        sMsg += "**Non Customer Name with this Stock Code already exists with this StockCode!<br/>";
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

                    iNonCustomerID = Convert.ToInt32(ddlNonCustomersAdd.SelectedValue);
                    ArNonCustomerStockCodeAssignments dl = new ArNonCustomerStockCodeAssignments();
                    dl.StockCode = ddlStockCodesAdd.SelectedValue;
                    dl.StockDescription = sStockDescription;
                    dl.NonCustomerID = iNonCustomerID;
                    dl.DateAdded = DateTime.Now;

                    db.ArNonCustomerStockCodeAssignments.InsertOnSubmit(dl);
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
                LoadNonCustomerGrid();//Refresh...                
                break;
            case "Update":
                lblError.Text = "";

                i = Convert.ToInt32(e.CommandArgument);
                lblError.Text = "";
                ddlStockCodes = (DropDownList)gvNonCustomer.Rows[i].FindControl("ddlStockCodes");
                ddlNonCustomers = (DropDownList)gvNonCustomer.Rows[i].FindControl("ddlNonCustomers");
                lblNonCustomerID = (Label)gvNonCustomer.Rows[i].FindControl("lblNonCustomerID");
                lblNonCustomerStockCodeAssignID = (Label)gvNonCustomer.Rows[i].FindControl("lblNonCustomerStockCodeAssignID");
                iNonCustomerStockCodeAssignID = Convert.ToInt32(lblNonCustomerStockCodeAssignID.Text);
                if (ddlStockCodes.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Stock Code!!";                   
                }
                else
                {
                    int iIndexOf = 0;
                    int iLengthMinus = 0;

                    iIndexOf = ddlStockCodes.SelectedItem.Text.IndexOf("-") + 1;
                    iLengthMinus = ddlStockCodes.SelectedItem.Text.Length - iIndexOf;
                    sStockDescription = ddlStockCodes.SelectedItem.Text.Substring(iIndexOf, iLengthMinus);
                }
                if (ddlNonCustomers.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Non Customer!"; 
                }
                else
                {
                    iNonCustomerID = Convert.ToInt32(ddlNonCustomers.SelectedValue);
                }
                if (ddlStockCodes.SelectedIndex != 0 && ddlNonCustomers.SelectedIndex != 0)
                {
                    if (NameStockCodeComboExistsForUpdate(iNonCustomerID, ddlStockCodes.SelectedValue, iNonCustomerStockCodeAssignID))
                    {
                        sMsg += "**Non Customer Name with this Stock Code already exists!<br/>";
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
                  

                    ArNonCustomerStockCodeAssignments dl = db.ArNonCustomerStockCodeAssignments.Single(p => p.NonCustomerStockCodeAssignID == iNonCustomerStockCodeAssignID);
                    dl.StockCode = ddlStockCodes.SelectedValue;
                    dl.StockDescription = sStockDescription;
                    dl.NonCustomerID = iNonCustomerID;
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

                    lblNonCustomerStockCodeAssignID = (Label)gvNonCustomer.Rows[i].FindControl("lblNonCustomerStockCodeAssignID");
                    iNonCustomerStockCodeAssignID = Convert.ToInt32(lblNonCustomerStockCodeAssignID.Text);
                    var query = (from d in db.ArNonCustomerStockCodeAssignments select d);
                    if (query.Count() == 1)
                    {
                        lblError.Text = "**You cannot delete the last record in the table.";
                        lblError.ForeColor = Color.Red;
                        return;
                    }

                    ArNonCustomerStockCodeAssignments dl = db.ArNonCustomerStockCodeAssignments.Single(p => p.NonCustomerStockCodeAssignID == iNonCustomerStockCodeAssignID);
                    db.ArNonCustomerStockCodeAssignments.DeleteOnSubmit(dl);
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
    protected void ddlDisplayCount_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadNonCustomerGrid();

    }




    #endregion
}