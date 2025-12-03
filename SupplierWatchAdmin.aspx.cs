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

public partial class SupplierWatchAdmin : System.Web.UI.Page
{
    #region Subs
    //Admin

    private void LoadSuppliers(DropDownList ddl)
    {
        ddl.Items.Clear();
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from s in db.ApSupplier
                         where
                           s.LastPurchDate > Convert.ToDateTime(DateTime.Now).AddYears(-3)
                         orderby
                           s.SupplierName
                         select new
                         {
                             s.Supplier,
                             s.SupplierName
                         });
            if (query.Count() > 0)
            {
                foreach (var a in query)
                {
                    ddl.Items.Add(new ListItem(a.SupplierName + " - " + a.Supplier.ToString(), a.Supplier.ToString()));
                }
                ddl.Items.Insert(0, new ListItem("SELECT", "0"));
            }
            else
            {
                ddl.Items.Insert(0, new ListItem("No Suppliers found...", "0"));
            }
        }
    }
    private void LoadSuppliersWatchGrid()
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.ApSupplierWatch
                         join s in db.ApSupplier on c.Supplier equals s.Supplier
                         orderby s.SupplierName
                         select new
                         {
                             c.ApSupplierWatchID,
                             c.Supplier,
                             s.SupplierName
                         });
            dt = SharedFunctions.ToDataTable(db, query);

            if (query.Count() > 0)
            {


                DataColumn column = new DataColumn();
                column.DataType = System.Type.GetType("System.Int32");
                column.ColumnName = "ID";
                dt.Columns.Add(column);

                //Set values for existing rows...
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["ID"] = i + 1;
                }

                gvSupplierWatch.DataSource = dt;
                gvSupplierWatch.DataBind();
                Session["dt"] = dt;
            }
            else
            {
                gvSupplierWatch.DataSource = null;
                gvSupplierWatch.DataBind();
            }
        }
    }

    #endregion

    #region Functions

    private bool SupplierExists(string sSupplier)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.ApSupplierWatch
                         where c.Supplier == sSupplier
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
    private bool SupplierExistsForUpdate(string sSupplier, int ApSupplierWatchID)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.ApSupplierWatch
                         where c.Supplier == sSupplier
                         && c.ApSupplierWatchID != ApSupplierWatchID
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
            LoadSuppliersWatchGrid();
        }
    }

    protected void gvSupplierWatch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvSupplierWatch.EditIndex != -1)//In Edit Mode...
                {

                    if (gvSupplierWatch.EditIndex == e.Row.RowIndex)//edited row...
                    {
                        DropDownList ddlSuppliers = (DropDownList)e.Row.FindControl("ddlSuppliers");
                        LoadSuppliers(ddlSuppliers);
                        Label lblSuppliers = (Label)e.Row.FindControl("lblSuppliers");
                        if (lblSuppliers.Text != "")
                        {
                            ddlSuppliers.SelectedValue = lblSuppliers.Text;
                        }

                    }
                    else if (gvSupplierWatch.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
                    {

                    }
                }
                else
                {

                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)//the footer row...
            {
                DropDownList ddlSuppliers = (DropDownList)e.Row.FindControl("ddlSuppliers");
                LoadSuppliers(ddlSuppliers);

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvSupplierWatch_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvSupplierWatch.EditIndex = e.NewEditIndex;
        gvSupplierWatch.DataSource = (DataTable)Session["dtSupplierWatch"];
        gvSupplierWatch.DataBind();

    }
    protected void gvSupplierWatch_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvSupplierWatch.EditIndex = -1;
        gvSupplierWatch.DataSource = (DataTable)Session["dtSupplierWatch"];
        gvSupplierWatch.DataBind();
    }
    protected void gvSupplierWatch_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvSupplierWatch.EditIndex = -1;
        LoadSuppliersWatchGrid();
    }
    protected void gvSupplierWatch_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvSupplierWatch.EditIndex = -1;
        LoadSuppliersWatchGrid();
    }
    protected void gvSupplierWatch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            lblError.Text = "";


            int i = 0;
            int iApSupplierWatchID = 0;
            Label lblApSupplierWatchID;
            DropDownList ddlSuppliers;
            switch (e.CommandName)
            {
                case "Add":
                    lblError.Text = "";
                    ddlSuppliers = (DropDownList)gvSupplierWatch.FooterRow.FindControl("ddlSuppliers");

                    if (ddlSuppliers.SelectedIndex == 0)
                    {
                        sMsg += "**Please select a Supplier!<br/>";
                    }
                    if (SupplierExists(ddlSuppliers.SelectedValue))
                    {
                        sMsg += "**Supplier already exist in this list!<br/>";
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
                        ApSupplierWatch s = new ApSupplierWatch();
                        s.Supplier = ddlSuppliers.SelectedValue;
                        s.DateAdded = DateTime.Now;
                        db.ApSupplierWatch.InsertOnSubmit(s);
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
                    LoadSuppliersWatchGrid();//Refresh...                
                    break;              

                case "Delete":
                    i = Convert.ToInt32(e.CommandArgument);

                    try
                    {
                        lblApSupplierWatchID = (Label)gvSupplierWatch.Rows[i].FindControl("lblApSupplierWatchID");
                        iApSupplierWatchID = Convert.ToInt32(lblApSupplierWatchID.Text);
                        var query = (from d in db.ApSupplierWatch select d);
                        if (query.Count() == 1)
                        {
                            lblError.Text = "**You cannot delete the last record in the table.";
                            lblError.ForeColor = Color.Red;
                            return;
                        }

                        ApSupplierWatch s = db.ApSupplierWatch.Single(p => p.ApSupplierWatchID == iApSupplierWatchID);
                        db.ApSupplierWatch.DeleteOnSubmit(s);
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