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

public partial class CustomItemsAdmin : System.Web.UI.Page
{
    #region Subs
    //Admin

    //Assignments...
    private void LoadCustomItemsGrid()
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = new DataTable();

            var query = (from c in db.InvCustomItemStockcodes
                         join pp in db.InvMaster on c.StockCode equals pp.StockCode
                         orderby c.StockCode
                         select new
                         {
                             c.CustomItemID,
                             c.StockCode,
                             pp.Description,
                         });
            dt = SharedFunctions.ToDataTable(db, query);
            if (ddlDisplayCount.SelectedValue != "ALL")
            {
                gvCustomItems.PageSize = Convert.ToInt32(ddlDisplayCount.SelectedValue);
            }
            else
            {
                gvCustomItems.PageSize = dt.Rows.Count;
            }
            gvCustomItems.DataSource = dt;
            gvCustomItems.DataBind();
            Session["dtInvCustomItemStockcodes"] = dt;
            dt.Dispose();
        }
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
    private bool NameStockCodeComboExists(string sStockCode)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.InvCustomItemStockcodes
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
    }
    private bool NameStockCodeComboExistsForUpdate(string sStockCode, int iCustomItemsID)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.InvCustomItemStockcodes
                         where c.StockCode == sStockCode
                         && c.CustomItemID != iCustomItemsID
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

            LoadCustomItemsGrid();
            lblPageNo.Text = "Current Page #: 1";
        }
    }
    //Assignments...
    protected void gvCustomItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvCustomItems.EditIndex != -1)//In Edit Mode...
                {

                    if (gvCustomItems.EditIndex == e.Row.RowIndex)//edited row...
                    {
                        Label lblStockCode = (Label)e.Row.FindControl("lblStockCode");
                        DropDownList ddlStockCodes = (DropDownList)e.Row.FindControl("ddlStockCodes");
                        LoadStockCode(ddlStockCodes);
                        ddlStockCodes.SelectedValue = lblStockCode.Text.Trim();

                    }
                    else if (gvCustomItems.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
                    {

                    }
                }
                else
                {

                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)//the footer row...
            {
                DropDownList ddlStockCodes = (DropDownList)e.Row.FindControl("ddlStockCodes");
                LoadStockCode(ddlStockCodes);

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvCustomItems_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvCustomItems.EditIndex = e.NewEditIndex;
        gvCustomItems.DataSource = (DataTable)Session["dtInvCustomItemStockcodes"];
        gvCustomItems.DataBind();

    }
    protected void gvCustomItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvCustomItems.EditIndex = -1;
        gvCustomItems.DataSource = (DataTable)Session["dtInvCustomItemStockcodes"];
        gvCustomItems.DataBind();
    }
    protected void gvCustomItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvCustomItems.EditIndex = -1;
        LoadCustomItemsGrid();
    }
    protected void gvCustomItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvCustomItems.EditIndex = -1;
        LoadCustomItemsGrid();
    }
    protected void gvCustomItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvCustomItems.PageIndex = e.NewPageIndex;
        lblPageNo.Text = "Current Page #: " + (gvCustomItems.PageIndex + 1).ToString();
        gvCustomItems.DataSource = (DataTable)Session["dtInvCustomItemStockcodes"];
        gvCustomItems.DataBind();
    }
    protected void gvCustomItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            lblError.Text = ""; 
            int iCustomItemID = 0;
            int i = 0;
            DropDownList ddlStockCodes;
            Label lblCustomItemID;
            if (Session["UserID"] == null)
            {
                Response.Redirect("Default.aspx");
            } 

            switch (e.CommandName)
            {
                case "Add":
                    lblError.Text = "";
                    ddlStockCodes = (DropDownList)gvCustomItems.FooterRow.FindControl("ddlStockCodes");

                    if (ddlStockCodes.SelectedIndex == 0)
                    {
                        sMsg += "**Please select a Stock Code!";
                    }

                    if (NameStockCodeComboExists(ddlStockCodes.SelectedValue))
                    {
                        sMsg += "**Stock Code already exists!<br/>";
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
                        InvCustomItemStockcodes b = new InvCustomItemStockcodes();
                        b.StockCode = ddlStockCodes.SelectedValue;
                        b.DateAdded = DateTime.Now;
                        db.InvCustomItemStockcodes.InsertOnSubmit(b);
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
                    LoadCustomItemsGrid();//Refresh...                
                    break;
                case "Update":
                    lblError.Text = "";

                    i = Convert.ToInt32(e.CommandArgument);
                    lblError.Text = "";
                    ddlStockCodes = (DropDownList)gvCustomItems.Rows[i].FindControl("ddlStockCodes");

                    lblCustomItemID = (Label)gvCustomItems.Rows[i].FindControl("lblCustomItemID");
                    iCustomItemID = Convert.ToInt32(lblCustomItemID.Text);
                    if (ddlStockCodes.SelectedIndex == 0)
                    {
                        sMsg += "**Please select a Stock Code!";
                    }
                    if (NameStockCodeComboExistsForUpdate(ddlStockCodes.SelectedValue, iCustomItemID))
                    {
                        sMsg += "**Stock Code already exists for another row!<br/>";
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
                        InvCustomItemStockcodes dl = db.InvCustomItemStockcodes.Single(p => p.CustomItemID == iCustomItemID);
                        dl.StockCode = ddlStockCodes.SelectedValue;
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
                        lblCustomItemID = (Label)gvCustomItems.Rows[i].FindControl("lblCustomItemID");
                        iCustomItemID = Convert.ToInt32(lblCustomItemID.Text);
                        var query = (from d in db.InvCustomItemStockcodes select d);
                        if (query.Count() == 1)
                        {
                            lblError.Text = "**You cannot delete the last record in the table.";
                            lblError.ForeColor = Color.Red;
                            return;
                        }

                        InvCustomItemStockcodes dl = db.InvCustomItemStockcodes.Single(p => p.CustomItemID == iCustomItemID);
                        db.InvCustomItemStockcodes.DeleteOnSubmit(dl);
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
    protected void ddlDisplayCount_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCustomItemsGrid();

    }




    #endregion
}