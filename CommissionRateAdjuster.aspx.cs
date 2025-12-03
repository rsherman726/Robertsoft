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
using System.Data.SqlTypes;
using AjaxControlToolkit;
using System.Globalization;
using web = System.Web.UI.WebControls;

public partial class CommissionRateAdjuster : System.Web.UI.Page
{
    #region Subs
    //StockCodes...
    private void LoadStockcodeExclusionGrid()
    {
        DataTable dt = new DataTable();
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from d in db.ArCommStockCodeOverride
                     join im in db.InvMaster on d.StockCode equals im.StockCode
                     orderby
                        d.StockCode
                     select new
                     {
                         d.CommStockCodeOverrideID,
                         d.StockCode,
                         StockDescription = im.Description,
                         d.ComPercentage,

                     });
        dt = SharedFunctions.ToDataTable(db, query);
        if (ddlDisplayCount.SelectedValue != "ALL")
        {
            gvStockcodeExclusions.PageSize = Convert.ToInt32(ddlDisplayCount.SelectedValue);
        }
        else
        {
            gvStockcodeExclusions.PageSize = dt.Rows.Count;
        }
        gvStockcodeExclusions.DataSource = dt;
        gvStockcodeExclusions.DataBind();
        Session["dtStockcodeExclusions"] = dt;
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

    //Salesperson-Customer...
    private void LoadSalespersonCustomerExclusionGrid()
    {
        DataTable dt = new DataTable();
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from d in db.ArCommSalespersonCustomerOverride
                     join c in db.ArCustomer on d.Customer equals c.Customer
                     join s in db.SalSalesperson on d.Salesperson equals s.Salesperson
                     orderby s.Salesperson, c.Customer
                     select new
                     {
                         d.CommSalespersonCustomerOverrideID,
                         d.Customer,
                         CustomerName = c.Name,
                         d.Salesperson,
                         SalespersonName = s.Name,
                         d.ComPercentage,

                     });
        dt = SharedFunctions.ToDataTable(db, query);
        if (ddlDisplayCount.SelectedValue != "ALL")
        {
            gvSalespersonCustomerExclusions.PageSize = Convert.ToInt32(ddlDisplayCount.SelectedValue);
        }
        else
        {
            gvSalespersonCustomerExclusions.PageSize = dt.Rows.Count;
        }
        gvSalespersonCustomerExclusions.DataSource = dt;
        gvSalespersonCustomerExclusions.DataBind();
        Session["dtSalespersonCustomerExclusions"] = dt;
        dt.Dispose();
    }
    private void LoadSalesPersons(DropDownList ddl)
    {
        ddl.Items.Clear();
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
            ddl.Items.Add(new ListItem(a.Name.ToUpper().Trim(), a.Salesperson.Trim()));
        }
        ddl.Items.Insert(0, new ListItem("All", "All"));
    }
    private void LoadCustomer(DropDownList ddl)
    {
        ddl.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from c in db.ArCustomer
                     where c.Customer != "test"
                     orderby c.Name.Trim()
                     select new
                     {
                         Customer = c.Customer.Trim(),
                         Name = c.Name
                     });
        foreach (var a in query)
        {
            ddl.Items.Add(new ListItem(a.Name.ToUpper() + " - " + a.Customer.Trim(), a.Customer.Trim()));
        }
        ddl.Items.Insert(0, new ListItem("All", "All"));

    }

    #endregion

    #region Functions
    //StockCodes...
    private bool StockCodeExclusionExists(string sStockCode)
    {
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from c in db.ArCommStockCodeOverride
                     where c.StockCode == sStockCode
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
    private bool StockCodeExclusionExistsForUpdate(string sStockCode, int iCommStockCodeOverrideID)
    {
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from c in db.ArCommStockCodeOverride
                     where c.StockCode == sStockCode
                     && c.CommStockCodeOverrideID != iCommStockCodeOverrideID
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
    //Salesperson-Customer....
    private bool SalespersonCustomerExclusionExists(string sSalesperson, string sCustomer)
    {
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from c in db.ArCommSalespersonCustomerOverride
                     where c.Salesperson == sSalesperson
                     && c.Customer == sCustomer
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
    private bool SalespersonCustomerExclusionExistsForUpdate(string sSalesperson, string sCustomer, int iCommSalespersonCustomerOverrideID)
    {
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from c in db.ArCommSalespersonCustomerOverride
                     where c.Salesperson == sSalesperson
                     && c.Customer == sCustomer
                     && c.CommSalespersonCustomerOverrideID != iCommSalespersonCustomerOverrideID
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
            LoadStockcodeExclusionGrid();
            LoadSalespersonCustomerExclusionGrid();
            lblPageNo.Text = "Current Page #: 1";
        }
    }



    //StockCodes...
    protected void gvStockcodeExclusions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvStockcodeExclusions.EditIndex != -1)//In Edit Mode...
                {

                    if (gvStockcodeExclusions.EditIndex == e.Row.RowIndex)//edited row...
                    {
                        Label lblStockCode = (Label)e.Row.FindControl("lblStockCode");
                        Label lblStockDescription = (Label)e.Row.FindControl("lblStockDescription");

                        string sStockCodePlusDesc = "";
                        sStockCodePlusDesc = lblStockCode.Text.Trim() + " - " + lblStockDescription.Text.Trim();
                        DropDownList ddlStockCodes = (DropDownList)e.Row.FindControl("ddlStockCodes");
                        LoadStockCode(ddlStockCodes);
                        ddlStockCodes.SelectedValue = lblStockCode.Text.Trim();


                    }
                    else if (gvStockcodeExclusions.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
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

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvStockcodeExclusions_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvStockcodeExclusions.EditIndex = e.NewEditIndex;
        gvStockcodeExclusions.DataSource = (DataTable)Session["dtStockcodeExclusions"];
        gvStockcodeExclusions.DataBind();

    }
    protected void gvStockcodeExclusions_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvStockcodeExclusions.EditIndex = -1;
        gvStockcodeExclusions.DataSource = (DataTable)Session["dtStockcodeExclusions"];
        gvStockcodeExclusions.DataBind();
    }
    protected void gvStockcodeExclusions_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvStockcodeExclusions.EditIndex = -1;
        LoadStockcodeExclusionGrid();
    }
    protected void gvStockcodeExclusions_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvStockcodeExclusions.EditIndex = -1;
        LoadStockcodeExclusionGrid();
    }
    protected void gvStockcodeExclusions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvStockcodeExclusions.PageIndex = e.NewPageIndex;
        lblPageNo.Text = "Current Page #: " + (gvStockcodeExclusions.PageIndex + 1).ToString();
        gvStockcodeExclusions.DataSource = (DataTable)Session["dtStockcodeExclusions"];
        gvStockcodeExclusions.DataBind();
    }
    protected void gvStockcodeExclusions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sMsg = "";
        lblError.Text = "";
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        int iUserID = 0;

        int iCommStockCodeOverrideID = 0;
        string sStockDescription = "";
        int i = 0;
        Label lblCommStockCodeOverrideID;

        DropDownList ddlStockCodes;
        DropDownList ddlStockCodesAdd;
        TextBox txtComPercentage;
        TextBox txtComPercentageAdd;


        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        iUserID = Convert.ToInt32(Session["UserID"]);


        switch (e.CommandName)
        {
            case "Add":
                lblError.Text = "";
                ddlStockCodesAdd = (DropDownList)gvStockcodeExclusions.FooterRow.FindControl("ddlStockCodesAdd");
                txtComPercentageAdd = (TextBox)gvStockcodeExclusions.FooterRow.FindControl("txtComPercentageAdd");

                if (ddlStockCodesAdd.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Stock Code!<br/>";
                }
                else
                {
                    int iIndexOf = 0;
                    int iLengthMinus = 0;

                    iIndexOf = ddlStockCodesAdd.SelectedItem.Text.IndexOf("-") + 1;
                    iLengthMinus = ddlStockCodesAdd.SelectedItem.Text.Length - iIndexOf;
                    sStockDescription = ddlStockCodesAdd.SelectedItem.Text.Substring(iIndexOf, iLengthMinus).Trim();
                }
                if (txtComPercentageAdd.Text.Trim() == "")
                {
                    sMsg += "**Commission Percentage is required!<br/>";
                }
                else
                {
                    if (!SharedFunctions.IsNumeric(txtComPercentageAdd.Text.Trim()))
                    {
                        sMsg += "**Commission Percentage Must be a numeric value!<br/>";
                    }
                }

                if (ddlStockCodesAdd.SelectedIndex != 0)
                {
                    if (StockCodeExclusionExists(ddlStockCodesAdd.SelectedValue))
                    {
                        sMsg += "**Stock Code already exists!<br/>";
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


                    ArCommStockCodeOverride dl = new ArCommStockCodeOverride();
                    dl.StockCode = ddlStockCodesAdd.SelectedValue;
                    dl.ComPercentage = Convert.ToDecimal(txtComPercentageAdd.Text.Trim());
                    dl.DateAdded = DateTime.Now;

                    db.ArCommStockCodeOverride.InsertOnSubmit(dl);
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
                LoadStockcodeExclusionGrid();//Refresh...                
                break;
            case "Update":
                lblError.Text = "";

                i = Convert.ToInt32(e.CommandArgument);
                lblError.Text = "";
                ddlStockCodes = (DropDownList)gvStockcodeExclusions.Rows[i].FindControl("ddlStockCodes");
                txtComPercentage = (TextBox)gvStockcodeExclusions.Rows[i].FindControl("txtComPercentage");

                lblCommStockCodeOverrideID = (Label)gvStockcodeExclusions.Rows[i].FindControl("lblCommStockCodeOverrideID");
                iCommStockCodeOverrideID = Convert.ToInt32(lblCommStockCodeOverrideID.Text);
                if (ddlStockCodes.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Stock Code!!<br/>";
                }
                else
                {
                    int iIndexOf = 0;
                    int iLengthMinus = 0;

                    iIndexOf = ddlStockCodes.SelectedItem.Text.IndexOf("-") + 1;
                    iLengthMinus = ddlStockCodes.SelectedItem.Text.Length - iIndexOf;
                    sStockDescription = ddlStockCodes.SelectedItem.Text.Substring(iIndexOf, iLengthMinus);
                }
                if (txtComPercentage.Text.Trim() == "")
                {
                    sMsg += "**Commission Percentage is required!<br/>";
                }
                else
                {
                    if (!SharedFunctions.IsNumeric(txtComPercentage.Text.Trim()))
                    {
                        sMsg += "**Commission Percentage Must be a numeric value!<br/>";
                    }
                }
                if (ddlStockCodes.SelectedIndex != 0)
                {
                    if (StockCodeExclusionExistsForUpdate(ddlStockCodes.SelectedValue, iCommStockCodeOverrideID))
                    {
                        sMsg += "**Stock Code already exists!<br/>";
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


                    ArCommStockCodeOverride dl = db.ArCommStockCodeOverride.Single(p => p.CommStockCodeOverrideID == iCommStockCodeOverrideID);
                    dl.StockCode = ddlStockCodes.SelectedValue;
                    dl.ComPercentage = Convert.ToDecimal(txtComPercentage.Text.Trim());
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

                    lblCommStockCodeOverrideID = (Label)gvStockcodeExclusions.Rows[i].FindControl("lblCommStockCodeOverrideID");
                    iCommStockCodeOverrideID = Convert.ToInt32(lblCommStockCodeOverrideID.Text);
                    var query = (from d in db.ArCommStockCodeOverride select d);
                    if (query.Count() == 1)
                    {
                        lblError.Text = "**You cannot delete the last record in the table.";
                        lblError.ForeColor = Color.Red;
                        return;
                    }

                    ArCommStockCodeOverride dl = db.ArCommStockCodeOverride.Single(p => p.CommStockCodeOverrideID == iCommStockCodeOverrideID);
                    db.ArCommStockCodeOverride.DeleteOnSubmit(dl);
                    db.SubmitChanges();

                    lblError.Text = "**Delete was successful!";
                    lblError.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    lblError.Text = "**Delete Failed (Relationship exists with another table.)";
                    lblError.ForeColor = Color.Red;
                    Debug.WriteLine(ex.ToString());
                }
                break;

        }
    }
    protected void ddlDisplayCount_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadStockcodeExclusionGrid();

    }
    //Salesperson-Customer...
    protected void gvSalespersonCustomerExclusions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvSalespersonCustomerExclusions.EditIndex != -1)//In Edit Mode...
                {

                    if (gvSalespersonCustomerExclusions.EditIndex == e.Row.RowIndex)//edited row...
                    {
                        Label lblSalesperson = (Label)e.Row.FindControl("lblSalesperson");
                        DropDownList ddlSalesperson = (DropDownList)e.Row.FindControl("ddlSalesperson");
                        LoadSalesPersons(ddlSalesperson);
                        ddlSalesperson.SelectedValue = lblSalesperson.Text.Trim();

                        Label lblCustomer = (Label)e.Row.FindControl("lblCustomer");
                        DropDownList ddlCustomer = (DropDownList)e.Row.FindControl("ddlCustomer");
                        LoadCustomer(ddlCustomer);
                        ddlCustomer.SelectedValue = lblCustomer.Text.Trim();


                    }
                    else if (gvSalespersonCustomerExclusions.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
                    {

                    }
                }
                else
                {

                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)//the footer row...
            {
                DropDownList ddlSalespersonAdd = (DropDownList)e.Row.FindControl("ddlSalespersonAdd");
                LoadSalesPersons(ddlSalespersonAdd);

                DropDownList ddlCustomerAdd = (DropDownList)e.Row.FindControl("ddlCustomerAdd");
                LoadCustomer(ddlCustomerAdd);

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvSalespersonCustomerExclusions_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvSalespersonCustomerExclusions.EditIndex = e.NewEditIndex;
        gvSalespersonCustomerExclusions.DataSource = (DataTable)Session["dtSalespersonCustomerExclusions"];
        gvSalespersonCustomerExclusions.DataBind();

    }
    protected void gvSalespersonCustomerExclusions_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvSalespersonCustomerExclusions.EditIndex = -1;
        gvSalespersonCustomerExclusions.DataSource = (DataTable)Session["dtSalespersonCustomerExclusions"];
        gvSalespersonCustomerExclusions.DataBind();
    }
    protected void gvSalespersonCustomerExclusions_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvSalespersonCustomerExclusions.EditIndex = -1;
        LoadSalespersonCustomerExclusionGrid();
    }
    protected void gvSalespersonCustomerExclusions_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvSalespersonCustomerExclusions.EditIndex = -1;
        LoadSalespersonCustomerExclusionGrid();
    }
    protected void gvSalespersonCustomerExclusions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvSalespersonCustomerExclusions.PageIndex = e.NewPageIndex;
        lblPageNo.Text = "Current Page #: " + (gvSalespersonCustomerExclusions.PageIndex + 1).ToString();
        gvSalespersonCustomerExclusions.DataSource = (DataTable)Session["dtSalespersonCustomerExclusions"];
        gvSalespersonCustomerExclusions.DataBind();
    }
    protected void gvSalespersonCustomerExclusions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sMsg = "";
        lblError.Text = "";
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        int iUserID = 0;

        int iCommSalespersonCustomerOverrideID = 0;
       
        int i = 0;
        Label lblCommSalespersonCustomerOverrideID;

        DropDownList ddlSalesperson;
        DropDownList ddlSalespersonAdd;
        DropDownList ddlCustomer;
        DropDownList ddlCustomerAdd;
        TextBox txtComPercentage;
        TextBox txtComPercentageAdd;


        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        iUserID = Convert.ToInt32(Session["UserID"]);


        switch (e.CommandName)
        {
            case "Add":
                lblError.Text = "";
                ddlSalespersonAdd = (DropDownList)gvSalespersonCustomerExclusions.FooterRow.FindControl("ddlSalespersonAdd");
                ddlCustomerAdd = (DropDownList)gvSalespersonCustomerExclusions.FooterRow.FindControl("ddlCustomerAdd");
                txtComPercentageAdd = (TextBox)gvSalespersonCustomerExclusions.FooterRow.FindControl("txtComPercentageAdd");

                if (ddlSalespersonAdd.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Salesperson!<br/>";
                }
                if (ddlCustomerAdd.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Customer!<br/>";
                }
                if (txtComPercentageAdd.Text.Trim() == "")
                {
                    sMsg += "**Commission Percentage is required!<br/>";
                }
                else
                {
                    if (!SharedFunctions.IsNumeric(txtComPercentageAdd.Text.Trim()))
                    {
                        sMsg += "**Commission Percentage Must be a numeric value!<br/>";
                    }
                }

                if (ddlSalespersonAdd.SelectedIndex != 0 && ddlCustomerAdd.SelectedIndex != 0)
                {
                    if (SalespersonCustomerExclusionExists(ddlSalespersonAdd.SelectedValue, ddlCustomerAdd.SelectedValue))
                    {
                        sMsg += "**Salesperson/Customer combo already exists!<br/>";
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


                    ArCommSalespersonCustomerOverride dl = new ArCommSalespersonCustomerOverride();
                    dl.Salesperson = ddlSalespersonAdd.SelectedValue;
                    dl.Customer = ddlCustomerAdd.SelectedValue;
                    dl.ComPercentage = Convert.ToDecimal(txtComPercentageAdd.Text.Trim());
                    dl.DateAdded = DateTime.Now;

                    db.ArCommSalespersonCustomerOverride.InsertOnSubmit(dl);
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
                LoadSalespersonCustomerExclusionGrid();//Refresh...                
                break;
            case "Update":
                lblError.Text = "";

                i = Convert.ToInt32(e.CommandArgument);
                lblError.Text = "";
                ddlSalesperson = (DropDownList)gvSalespersonCustomerExclusions.Rows[i].FindControl("ddlSalesperson");
                ddlCustomer = (DropDownList)gvSalespersonCustomerExclusions.Rows[i].FindControl("ddlCustomer");
                txtComPercentage = (TextBox)gvSalespersonCustomerExclusions.Rows[i].FindControl("txtComPercentage");

                lblCommSalespersonCustomerOverrideID = (Label)gvSalespersonCustomerExclusions.Rows[i].FindControl("lblCommSalespersonCustomerOverrideID");
                iCommSalespersonCustomerOverrideID = Convert.ToInt32(lblCommSalespersonCustomerOverrideID.Text);
                if (ddlSalesperson.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Salesperson!<br/>";
                }
                if (ddlCustomer.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Customer!<br/>";
                }
                if (txtComPercentage.Text.Trim() == "")
                {
                    sMsg += "**Commission Percentage is required!<br/>";
                }
                else
                {
                    if (!SharedFunctions.IsNumeric(txtComPercentage.Text.Trim()))
                    {
                        sMsg += "**Commission Percentage Must be a numeric value!<br/>";
                    }
                }
                if (ddlSalesperson.SelectedIndex != 0 && ddlCustomer.SelectedIndex != 0)
                {
                    if (SalespersonCustomerExclusionExistsForUpdate(ddlSalesperson.SelectedValue, ddlCustomer.SelectedValue, iCommSalespersonCustomerOverrideID))
                    {
                        sMsg += "**Salesperson/Customer combo already exists!<br/>";
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


                    ArCommSalespersonCustomerOverride dl = db.ArCommSalespersonCustomerOverride.Single(p => p.CommSalespersonCustomerOverrideID == iCommSalespersonCustomerOverrideID);
                    dl.Salesperson = ddlSalesperson.SelectedValue;
                    dl.Customer = ddlCustomer.SelectedValue;
                    dl.ComPercentage = Convert.ToDecimal(txtComPercentage.Text.Trim());
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

                    lblCommSalespersonCustomerOverrideID = (Label)gvSalespersonCustomerExclusions.Rows[i].FindControl("lblCommSalespersonCustomerOverrideID");
                    iCommSalespersonCustomerOverrideID = Convert.ToInt32(lblCommSalespersonCustomerOverrideID.Text);
                    var query = (from d in db.ArCommSalespersonCustomerOverride select d);
                    if (query.Count() == 1)
                    {
                        lblError.Text = "**You cannot delete the last record in the table.";
                        lblError.ForeColor = Color.Red;
                        return;
                    }

                    ArCommSalespersonCustomerOverride dl = db.ArCommSalespersonCustomerOverride.Single(p => p.CommSalespersonCustomerOverrideID == iCommSalespersonCustomerOverrideID);
                    db.ArCommSalespersonCustomerOverride.DeleteOnSubmit(dl);
                    db.SubmitChanges();

                    lblError.Text = "**Delete was successful!";
                    lblError.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    lblError.Text = "**Delete Failed (Relationship exists with another table.)";
                    lblError.ForeColor = Color.Red;
                    Debug.WriteLine(ex.ToString());
                }
                break;

        }
    }
    protected void ddlDisplayCount0_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSalespersonCustomerExclusionGrid();

    }


    #endregion
}