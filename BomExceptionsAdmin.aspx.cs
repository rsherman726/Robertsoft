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

public partial class BomExceptionsAdmin : System.Web.UI.Page
{
    #region Subs
    //Admin
 
    //Assignments...
    private void LoadBomExceptionsGrid()
    {
        DataTable dt = new DataTable();
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from c in db.BomExceptions
                     join pp in db.InvMaster on c.ParentPart equals pp.StockCode
                     join com in db.InvMaster on c.Component equals com.StockCode
                     select new
                     {
                         c.BomExceptionsID,
                         c.ParentPart,
                         c.Component,
                         ParentPartDescription = pp.Description,
                         ComponentDescription = com.Description,
                     });
        dt = SharedFunctions.ToDataTable(db, query);
        if (ddlDisplayCount.SelectedValue != "ALL")
        {
            gvExceptions.PageSize = Convert.ToInt32(ddlDisplayCount.SelectedValue);
        }
        else
        {
            gvExceptions.PageSize = dt.Rows.Count;
        }
        gvExceptions.DataSource = dt;
        gvExceptions.DataBind();
        Session["dtBomExceptions"] = dt;
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

    private bool NameStockCodeComboExists(string sParentPart, string sComponent)
    {
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from c in db.BomExceptions
                     where c.ParentPart == sParentPart
                     && c.Component == sComponent
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
    private bool NameStockCodeComboExistsForUpdate(string sParentPart, string sComponent, int iBomExceptionsID)
    {
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from c in db.BomExceptions
                     where c.ParentPart == sParentPart
                     && c.Component == sComponent
                     && c.BomExceptionsID != iBomExceptionsID
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
 
            LoadBomExceptionsGrid();
            lblPageNo.Text = "Current Page #: 1";
        }
    }    
    //Assignments...
    protected void gvExceptions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvExceptions.EditIndex != -1)//In Edit Mode...
                {

                    if (gvExceptions.EditIndex == e.Row.RowIndex)//edited row...
                    {
                        Label lblStockCodePP = (Label)e.Row.FindControl("lblStockCodePP");                         
                        DropDownList ddlStockCodesPP = (DropDownList)e.Row.FindControl("ddlStockCodesPP");
                        LoadStockCode(ddlStockCodesPP);
                        ddlStockCodesPP.SelectedValue = lblStockCodePP.Text.Trim();

                        Label lblStockCodeC = (Label)e.Row.FindControl("lblStockCodeC");
                        DropDownList ddlStockCodesC = (DropDownList)e.Row.FindControl("ddlStockCodesC");
                        LoadStockCode(ddlStockCodesC);
                        ddlStockCodesC.SelectedValue = lblStockCodeC.Text.Trim();


                    }
                    else if (gvExceptions.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
                    {

                    }
                }
                else
                {

                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)//the footer row...
            {
                DropDownList ddlStockCodesAddPP = (DropDownList)e.Row.FindControl("ddlStockCodesAddPP");
                LoadStockCode(ddlStockCodesAddPP);

                DropDownList ddlStockCodesAddC = (DropDownList)e.Row.FindControl("ddlStockCodesAddC");
                LoadStockCode(ddlStockCodesAddC);

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvExceptions_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvExceptions.EditIndex = e.NewEditIndex;
        gvExceptions.DataSource = (DataTable)Session["dtBomExceptions"];
        gvExceptions.DataBind();

    }
    protected void gvExceptions_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvExceptions.EditIndex = -1;
        gvExceptions.DataSource = (DataTable)Session["dtBomExceptions"];
        gvExceptions.DataBind();
    }
    protected void gvExceptions_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvExceptions.EditIndex = -1;
        LoadBomExceptionsGrid();
    }
    protected void gvExceptions_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvExceptions.EditIndex = -1;
        LoadBomExceptionsGrid();
    }
    protected void gvExceptions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvExceptions.PageIndex = e.NewPageIndex;
        lblPageNo.Text = "Current Page #: " + (gvExceptions.PageIndex + 1).ToString();
        gvExceptions.DataSource = (DataTable)Session["dtBomExceptions"];
        gvExceptions.DataBind();
    }
    protected void gvExceptions_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sMsg = "";
        lblError.Text = "";
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        int iUserID = 0;
        int iBomExceptionsID = 0;        
        
        int i = 0;
         
        DropDownList ddlStockCodesPP;
        DropDownList ddlStockCodesC;
        DropDownList ddlStockCodesAddPP;
        DropDownList ddlStockCodesAddC;
        Label lblBomExceptionsID;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        iUserID = Convert.ToInt32(Session["UserID"]);


        switch (e.CommandName)
        {
            case "Add":
                lblError.Text = "";
                ddlStockCodesAddPP = (DropDownList)gvExceptions.FooterRow.FindControl("ddlStockCodesAddPP");
                ddlStockCodesAddC = (DropDownList)gvExceptions.FooterRow.FindControl("ddlStockCodesAddC");

                if (ddlStockCodesAddPP.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Stock Code for Parent Part.";
                }
 
                if (ddlStockCodesAddC.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Stock Code for Component.";
                }
 


                if (ddlStockCodesAddPP.SelectedIndex != 0 && ddlStockCodesAddC.SelectedIndex != 0)
                {
                    if (NameStockCodeComboExists(ddlStockCodesAddPP.SelectedValue, ddlStockCodesAddC.SelectedValue))
                    {
                        sMsg += "**Parent Part with this Component already exists!<br/>";
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
                    
                    BomExceptions b = new BomExceptions();
                    b.ParentPart = ddlStockCodesAddPP.SelectedValue;
                    b.Component = ddlStockCodesAddC.SelectedValue;                     
                    b.DateAdded = DateTime.Now;
                    db.BomExceptions.InsertOnSubmit(b);
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
                LoadBomExceptionsGrid();//Refresh...                
                break;
            case "Update":
                lblError.Text = "";

                i = Convert.ToInt32(e.CommandArgument);
                lblError.Text = "";
                ddlStockCodesPP = (DropDownList)gvExceptions.Rows[i].FindControl("ddlStockCodesPP");
                ddlStockCodesC = (DropDownList)gvExceptions.Rows[i].FindControl("ddlStockCodesC");
                lblBomExceptionsID = (Label)gvExceptions.Rows[i].FindControl("lblBomExceptionsID");                 
                iBomExceptionsID = Convert.ToInt32(lblBomExceptionsID.Text);
                if (ddlStockCodesPP.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Stock Code for Parent Part.";
                }

                if (ddlStockCodesC.SelectedIndex == 0)
                {
                    sMsg += "**Please select a Stock Code for Component.";
                }



                if (ddlStockCodesPP.SelectedIndex != 0 && ddlStockCodesC.SelectedIndex != 0)
                {
                    if (NameStockCodeComboExistsForUpdate(ddlStockCodesPP.SelectedValue, ddlStockCodesC.SelectedValue, iBomExceptionsID))
                    {
                        sMsg += "**Parent Part with this Component already exists!<br/>";
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
                    BomExceptions dl = db.BomExceptions.Single(p => p.BomExceptionsID == iBomExceptionsID);
                    dl.ParentPart = ddlStockCodesPP.SelectedValue;
                    dl.Component = ddlStockCodesC.SelectedValue;                     
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
                    lblBomExceptionsID = (Label)gvExceptions.Rows[i].FindControl("lblBomExceptionsID");
                    iBomExceptionsID = Convert.ToInt32(lblBomExceptionsID.Text);
                    var query = (from d in db.BomExceptions select d);
                    if (query.Count() == 1)
                    {
                        lblError.Text = "**You cannot delete the last record in the table.";
                        lblError.ForeColor = Color.Red;
                        return;
                    }

                    BomExceptions dl = db.BomExceptions.Single(p => p.BomExceptionsID == iBomExceptionsID);
                    db.BomExceptions.DeleteOnSubmit(dl);
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
        LoadBomExceptionsGrid();

    }




    #endregion
}