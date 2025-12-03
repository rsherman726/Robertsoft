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

public partial class RunRatesAdmin : System.Web.UI.Page
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


    private void LoadRunRatesGrid()
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = new DataTable();

            var query = (from d in db.SorRunRates
                         join c in db.ArCustomer on d.Customer equals c.Customer
                         orderby
                           c.Name
                         select new
                         {
                             d.SorRunRatesID,
                             d.Customer,
                             c.Name,
                             d.StdCasesPerMinute,

                         });
            dt = SharedFunctions.ToDataTable(db, query);
            if (query.Count() > 10)
            {
                pnlRunRates.ScrollBars = ScrollBars.Vertical;
                pnlRunRates.Height = Unit.Pixel(400);
            }
            else
            {
                pnlRunRates.ScrollBars = ScrollBars.None;
                pnlRunRates.Height = Unit.Pixel(400);
            }

            gvRunRates.DataSource = dt;
            gvRunRates.DataBind();
            Session["dtRunRates"] = dt;
            dt.Dispose();
        }
    }


    #endregion

    #region Functions

    private bool CustomerExists(string sCustomer)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.SorRunRates
                         where c.Customer == sCustomer
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
            LoadRunRatesGrid();
        }
    }

    protected void gvRunRates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvRunRates.EditIndex != -1)//In Edit Mode...
                {

                    if (gvRunRates.EditIndex == e.Row.RowIndex)//edited row...
                    { 
 


                    }
                    else if (gvRunRates.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
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

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvRunRates_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvRunRates.EditIndex = e.NewEditIndex;
        gvRunRates.DataSource = (DataTable)Session["dtRunRates"];
        gvRunRates.DataBind();

    }
    protected void gvRunRates_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvRunRates.EditIndex = -1;
        gvRunRates.DataSource = (DataTable)Session["dtRunRates"];
        gvRunRates.DataBind();
    }
    protected void gvRunRates_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvRunRates.EditIndex = -1;
        LoadRunRatesGrid();
    }
    protected void gvRunRates_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvRunRates.EditIndex = -1;
        LoadRunRatesGrid();
    } 
    protected void gvRunRates_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            lblError.Text = "";

            int iUserID = 0;
           
            int i = 0;
            int iSorRunRatesID = 0;
            Label lblSorRunRatesID;
            DropDownList ddlCustomers;
            TextBox txtStdCasesPerMinute;

            if (Session["UserID"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            iUserID = Convert.ToInt32(Session["UserID"]);


            switch (e.CommandName)
            {
                case "Add":
                    lblError.Text = "";
                    ddlCustomers = (DropDownList)gvRunRates.FooterRow.FindControl("ddlCustomers");
                    txtStdCasesPerMinute = (TextBox)gvRunRates.FooterRow.FindControl("txtStdCasesPerMinute");

                    if (ddlCustomers.SelectedIndex == 0)
                    {
                        sMsg += "**Please select a Customer!";
                    }
                    if (txtStdCasesPerMinute.Text.Trim() == "")
                    {
                        sMsg += "**Standard Cases Per Minute is required!<br/>";
                    }
                    else
                    {
                        if (!SharedFunctions.IsNumeric(txtStdCasesPerMinute.Text.Trim()))
                        {
                            sMsg += "**Standard Cases Per Minute must be a numeric value!<br/>";
                        }
                    }

                    if (ddlCustomers.SelectedIndex != 0 )
                    {
                        if (CustomerExists(ddlCustomers.SelectedValue))
                        {
                            sMsg += "**Customer Name already exists in the Run Rates Table!<br/>";
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
                        SorRunRates s = new SorRunRates();
                        s.Customer = ddlCustomers.SelectedValue;
                        s.StdCasesPerMinute = Convert.ToDecimal(txtStdCasesPerMinute.Text);
                        db.SorRunRates.InsertOnSubmit(s);
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
                    LoadRunRatesGrid();//Refresh...                
                    break;
                case "Update":
                    lblError.Text = "";

                    i = Convert.ToInt32(e.CommandArgument);
                    lblError.Text = "";
                   
                    txtStdCasesPerMinute = (TextBox)gvRunRates.Rows[i].FindControl("txtStdCasesPerMinute");
                    lblSorRunRatesID = (Label)gvRunRates.Rows[i].FindControl("lblSorRunRatesID");
                    iSorRunRatesID = Convert.ToInt32(lblSorRunRatesID.Text);

                    if (txtStdCasesPerMinute.Text.Trim() == "")
                    {
                        sMsg += "**Standard Cases Per Minute is required!<br/>";
                    }
                    else
                    {
                        if (!SharedFunctions.IsNumeric(txtStdCasesPerMinute.Text.Trim()))
                        {
                            sMsg += "**Standard Cases Per Minute must be a numeric value!<br/>";
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
                        SorRunRates s = db.SorRunRates.Single(p => p.SorRunRatesID == iSorRunRatesID);                        
                        s.StdCasesPerMinute = Convert.ToDecimal(txtStdCasesPerMinute.Text);
                        s.DateModified = DateTime.Now;
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
                        lblSorRunRatesID = (Label)gvRunRates.Rows[i].FindControl("lblSorRunRatesID");
                        iSorRunRatesID = Convert.ToInt32(lblSorRunRatesID.Text);
                        var query = (from d in db.SorRunRates select d);
                        if (query.Count() == 1)
                        {
                            lblError.Text = "**You cannot delete the last record in the table.";
                            lblError.ForeColor = Color.Red;
                            return;
                        }

                        SorRunRates s = db.SorRunRates.Single(p => p.SorRunRatesID == iSorRunRatesID);
                        db.SorRunRates.DeleteOnSubmit(s);
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
 




    #endregion
}