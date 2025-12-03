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

public partial class ProductionScheduleAdmin : System.Web.UI.Page
{
    #region Subs


    //Assignments...
    private void LoadProductionScheduleGrid()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = new DataTable();

            var query = (from d in db.WipProductionSchedule
                         orderby
                             d.ScheduledDate descending, d.StockCode ascending
                         select new
                         {
                             d.ProductionScheduleID,
                             d.StockCode,
                             d.StockDescription,
                             d.Line,
                             d.Quantity,
                             d.Comment,
                             d.ScheduledDate,
                             d.DateAdded

                         });
            dt = SharedFunctions.ToDataTable(db, query);
            if (ddlDisplayCount.SelectedValue != "ALL")
            {
                gvProductionSchedule.PageSize = Convert.ToInt32(ddlDisplayCount.SelectedValue);
            }
            else
            {
                gvProductionSchedule.PageSize = dt.Rows.Count;
            }
            gvProductionSchedule.DataSource = dt;
            gvProductionSchedule.DataBind();
            Session["dtProductionSchedule"] = dt;
            dt.Dispose();
        }
    }
    private void LoadStockCode(DropDownList ddl)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddl.Items.Clear();

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
    }

    #endregion

    #region Functions

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

            LoadProductionScheduleGrid();
            lblPageNo.Text = "Current Page #: 1";
        }
    }


    //Assignments...
    protected void gvProductionSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvProductionSchedule.EditIndex != -1)//In Edit Mode...
                {

                    if (gvProductionSchedule.EditIndex == e.Row.RowIndex)//edited row...
                    {
                        Label lblStockCode = (Label)e.Row.FindControl("lblStockCode");
                        Label lblStockDescription = (Label)e.Row.FindControl("lblStockDescription");
                        TextBox txtScheduledDate = (TextBox)e.Row.FindControl("txtScheduledDate");
                        if (txtScheduledDate.Text.Trim() != "")
                        {
                            txtScheduledDate.Text = Convert.ToDateTime(txtScheduledDate.Text.Trim()).ToShortDateString();
                        }

                        string sStockCodePlusDesc = "";
                        sStockCodePlusDesc = lblStockCode.Text.Trim() + " - " + lblStockDescription.Text.Trim();
                        DropDownList ddlStockCodes = (DropDownList)e.Row.FindControl("ddlStockCodes");
                        LoadStockCode(ddlStockCodes);
                        ddlStockCodes.SelectedValue = lblStockCode.Text.Trim();


                    }
                    else if (gvProductionSchedule.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
                    {
                        Label lblScheduledDate = (Label)e.Row.FindControl("lblScheduledDate");
                        if (lblScheduledDate.Text.Trim() != "")
                        {
                            lblScheduledDate.Text = Convert.ToDateTime(lblScheduledDate.Text.Trim()).ToShortDateString();
                        }
                    }
                }
                else
                {
                    Label lblScheduledDate = (Label)e.Row.FindControl("lblScheduledDate");
                    if (lblScheduledDate.Text.Trim() != "")
                    {
                        lblScheduledDate.Text = Convert.ToDateTime(lblScheduledDate.Text.Trim()).ToShortDateString();
                    }
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
    protected void gvProductionSchedule_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvProductionSchedule.EditIndex = e.NewEditIndex;
        gvProductionSchedule.DataSource = (DataTable)Session["dtProductionSchedule"];
        gvProductionSchedule.DataBind();

    }
    protected void gvProductionSchedule_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvProductionSchedule.EditIndex = -1;
        gvProductionSchedule.DataSource = (DataTable)Session["dtProductionSchedule"];
        gvProductionSchedule.DataBind();
    }
    protected void gvProductionSchedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvProductionSchedule.EditIndex = -1;
        LoadProductionScheduleGrid();
    }
    protected void gvProductionSchedule_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvProductionSchedule.EditIndex = -1;
        LoadProductionScheduleGrid();
    }
    protected void gvProductionSchedule_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvProductionSchedule.PageIndex = e.NewPageIndex;
        lblPageNo.Text = "Current Page #: " + (gvProductionSchedule.PageIndex + 1).ToString();
        gvProductionSchedule.DataSource = (DataTable)Session["dtProductionSchedule"];
        gvProductionSchedule.DataBind();
    }
    protected void gvProductionSchedule_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            lblError.Text = "";

            int iUserID = 0;
            int iProductionScheduleID = 0;

            string sStockDescription = "";
            int i = 0;

            Label lblProductionScheduleID;
            DropDownList ddlStockCodes;
            DropDownList ddlStockCodesAdd;
            TextBox txtLine;
            TextBox txtQuantity;
            TextBox txtComment;
            TextBox txtScheduledDate;

            TextBox txtLineAdd;
            TextBox txtQuantityAdd;
            TextBox txtCommentAdd;
            TextBox txtScheduledDateAdd;

            if (Session["UserID"] == null)
            {
                Response.Redirect("Default.aspx");
            }

            iUserID = Convert.ToInt32(Session["UserID"]);


            switch (e.CommandName)
            {
                case "Add":
                    lblError.Text = "";
                    ddlStockCodesAdd = (DropDownList)gvProductionSchedule.FooterRow.FindControl("ddlStockCodesAdd");
                    txtLineAdd = (TextBox)gvProductionSchedule.FooterRow.FindControl("txtLineAdd");
                    txtQuantityAdd = (TextBox)gvProductionSchedule.FooterRow.FindControl("txtQuantityAdd");
                    txtCommentAdd = (TextBox)gvProductionSchedule.FooterRow.FindControl("txtCommentAdd");
                    txtScheduledDateAdd = (TextBox)gvProductionSchedule.FooterRow.FindControl("txtScheduledDateAdd");

                    if (ddlStockCodesAdd.SelectedIndex == 0)
                    {
                        sMsg += "**Please select a Stock Code!";
                    }
                    else
                    {
                        int iIndexOf = 0;
                        int iLengthMinus = 0;

                        iIndexOf = ddlStockCodesAdd.SelectedItem.Text.IndexOf("-") + 1;
                        iLengthMinus = ddlStockCodesAdd.SelectedItem.Text.Length - iIndexOf;
                        sStockDescription = ddlStockCodesAdd.SelectedItem.Text.Substring(iIndexOf, iLengthMinus);
                    }
                    if (txtLineAdd.Text.Trim() == "")
                    {
                        sMsg += "**Please input a Line!";
                    }
                    else
                    {
                        if (!SharedFunctions.IsNumeric(txtLineAdd.Text.Trim()))
                        {
                            sMsg += "**Line must be a numeric value!";
                        }
                    }
                    if (txtQuantityAdd.Text.Trim() == "")
                    {
                        sMsg += "**Please input a Quantity!";
                    }
                    else
                    {
                        if (!SharedFunctions.IsNumeric(txtQuantityAdd.Text.Trim()))
                        {
                            sMsg += "**Quantity must be a numeric value!";
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
                        WipProductionSchedule dl = new WipProductionSchedule();
                        dl.StockCode = ddlStockCodesAdd.SelectedValue;
                        dl.StockDescription = sStockDescription;
                        dl.Line = Convert.ToDecimal(txtLineAdd.Text.Trim());
                        dl.Quantity = Convert.ToDecimal(txtQuantityAdd.Text.Trim());

                        if (txtCommentAdd.Text.Trim() != "")
                        {
                            dl.Comment = txtCommentAdd.Text.Trim();
                        }
                        else
                        {
                            dl.Comment = null;
                        }
                        if (txtScheduledDateAdd.Text.Trim() != "")
                        {
                            dl.ScheduledDate = Convert.ToDateTime(txtScheduledDateAdd.Text.Trim());
                        }
                        else
                        {
                            dl.ScheduledDate = null;
                        }
                        dl.DateAdded = DateTime.Now;

                        db.WipProductionSchedule.InsertOnSubmit(dl);
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
                    LoadProductionScheduleGrid();//Refresh...                
                    break;
                case "Update":
                    lblError.Text = "";

                    i = Convert.ToInt32(e.CommandArgument);
                    lblError.Text = "";
                    ddlStockCodes = (DropDownList)gvProductionSchedule.Rows[i].FindControl("ddlStockCodes");
                    txtLine = (TextBox)gvProductionSchedule.Rows[i].FindControl("txtLine");
                    txtQuantity = (TextBox)gvProductionSchedule.Rows[i].FindControl("txtQuantity");
                    txtComment = (TextBox)gvProductionSchedule.Rows[i].FindControl("txtComment");
                    txtScheduledDate = (TextBox)gvProductionSchedule.Rows[i].FindControl("txtScheduledDate");

                    lblProductionScheduleID = (Label)gvProductionSchedule.Rows[i].FindControl("lblProductionScheduleID");
                    iProductionScheduleID = Convert.ToInt32(lblProductionScheduleID.Text);
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
                    if (txtLine.Text.Trim() == "")
                    {
                        sMsg += "**Please input a Line!";
                    }
                    else
                    {
                        if (!SharedFunctions.IsNumeric(txtLine.Text.Trim()))
                        {
                            sMsg += "**Line must be a numeric value!";
                        }
                    }
                    if (txtQuantity.Text.Trim() == "")
                    {
                        sMsg += "**Please input a Quantity!";
                    }
                    else
                    {
                        if (!SharedFunctions.IsNumeric(txtQuantity.Text.Trim()))
                        {
                            sMsg += "**Quantity must be a numeric value!";
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

                        WipProductionSchedule dl = db.WipProductionSchedule.Single(p => p.ProductionScheduleID == iProductionScheduleID);
                        dl.StockCode = ddlStockCodes.SelectedValue;
                        dl.StockDescription = sStockDescription;
                        dl.Line = Convert.ToDecimal(txtLine.Text.Trim());
                        dl.Quantity = Convert.ToDecimal(txtQuantity.Text.Trim());



                        if (txtComment.Text.Trim() != "")
                        {
                            dl.Comment = txtComment.Text.Trim();
                        }
                        else
                        {
                            dl.Comment = null;
                        }
                        if (txtScheduledDate.Text.Trim() != "")
                        {
                            dl.ScheduledDate = Convert.ToDateTime(txtScheduledDate.Text.Trim());
                        }
                        else
                        {
                            dl.ScheduledDate = null;
                        }
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
                        lblProductionScheduleID = (Label)gvProductionSchedule.Rows[i].FindControl("lblProductionScheduleID");
                        iProductionScheduleID = Convert.ToInt32(lblProductionScheduleID.Text);
                        var query = (from d in db.WipProductionSchedule select d);
                        if (query.Count() == 1)
                        {
                            lblError.Text = "**You cannot delete the last record in the table.";
                            lblError.ForeColor = Color.Red;
                            return;
                        }

                        WipProductionSchedule dl = db.WipProductionSchedule.Single(p => p.ProductionScheduleID == iProductionScheduleID);
                        db.WipProductionSchedule.DeleteOnSubmit(dl);
                        db.SubmitChanges();

                        lblError.Text = "**Delete was successful!";
                        lblError.ForeColor = Color.Green;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "**Delete Failed";
                        lblError.ForeColor = Color.Red;
                        Debug.WriteLine(ex.ToString());
                    }
                    break;

            }
        }
    }
    protected void ddlDisplayCount_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadProductionScheduleGrid();

    }


    #endregion
}