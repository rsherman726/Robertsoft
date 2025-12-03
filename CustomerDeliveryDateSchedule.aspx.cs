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

public partial class CustomerDeliveryDateSchedule : System.Web.UI.Page
{
    #region Subs
    private void LoadCustomerDeliverySchedule()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = new DataTable();

            var query = (from acs in db.ArCustomerDeliverySchedule
                         select new
                         {
                             acs.CustomerDeliveryScheduleID,
                             acs.Customer,
                             acs.ArCustomer.Name,
                             acs.Monday,
                             acs.Tuesday,
                             acs.Wednesday,
                             acs.Thursday,
                             acs.Friday,
                             acs.DateAdded,
                             acs.AddedBy,
                             acs.DateModified,
                             acs.ModifiedBy
                         });
            dt = SharedFunctions.ToDataTable(db, query);
            gvCustomerDeliverySchedule.DataSource = dt;
            gvCustomerDeliverySchedule.DataBind();
            Session["dtCustomerDeliverySchedule"] = dt;
            dt.Dispose();
        }
    }
    private void LoadCustomer(DropDownList ddl)
    {
        ddl.Items.Clear();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

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
                ddl.Items.Add(new ListItem(a.Name + " - " + a.Customer, a.Customer));
            }
            ddl.Items.Insert(0, new ListItem("SELECT", "0"));
        }
    }
    private void UpdateSchedules()
    {
        lblError.Text = "";
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iCustomerDeliveryScheduleID = 0;
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            for (int i = 0; i < gvCustomerDeliverySchedule.Rows.Count; i++)
            {

                Label lblCustomerDeliveryScheduleID = (Label)gvCustomerDeliverySchedule.Rows[i].FindControl("lblCustomerDeliveryScheduleID");
                iCustomerDeliveryScheduleID = Convert.ToInt32(lblCustomerDeliveryScheduleID.Text);
                CheckBoxList cblSchedule = (CheckBoxList)gvCustomerDeliverySchedule.Rows[i].FindControl("cblSchedule");

                ArCustomerDeliverySchedule dl = db.ArCustomerDeliverySchedule.Single(p => p.CustomerDeliveryScheduleID == iCustomerDeliveryScheduleID);

                if (cblSchedule.Items[0].Selected)
                {
                    dl.Monday = 1;
                }
                else
                {
                    dl.Monday = 0;
                }
                if (cblSchedule.Items[1].Selected)
                {
                    dl.Tuesday = 1;
                }
                else
                {
                    dl.Tuesday = 0;
                }
                if (cblSchedule.Items[2].Selected)
                {
                    dl.Wednesday = 1;
                }
                else
                {
                    dl.Wednesday = 0;
                }
                if (cblSchedule.Items[3].Selected)
                {
                    dl.Thursday = 1;
                }
                else
                {
                    dl.Thursday = 0;
                }
                if (cblSchedule.Items[4].Selected)
                {
                    dl.Friday = 1;
                }
                else
                {
                    dl.Friday = 0;
                }

                dl.DateModified = DateTime.Now;
                dl.ModifiedBy = iUserID;                
                db.SubmitChanges();

                

            }//End Loop


            LoadCustomerDeliverySchedule();

            lblError.Text = "**Update was successful!";
            lblError.ForeColor = Color.Green;
            lblError0.Text = "**Update was successful!";
            lblError0.ForeColor = Color.Green;

        }
    }

    #endregion

    #region Functions
    private bool CustomerScheduleExists(string sCustomer)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.ArCustomerDeliverySchedule
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
        int iRoleID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        else
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
            iRoleID = Convert.ToInt32(SharedFunctions.GetRole(iUserID));
            string sUrl = @"http://localhost" + Request.ServerVariables["URL"].ToString();//Just has to be a valid web path...
            Uri uri = new Uri(sUrl);
            string[] segments = uri.Segments;
            foreach (string s in segments)
            {
                sUrl = s;//Will hold the last segment...
            }
            string sText = SharedFunctions.GetMenuItemText(sUrl);
            int? iAdminID = SharedFunctions.GetAdminID(iUserID);
            if (!SharedFunctions.IsAccessGranted(iAdminID, sText))
            {
                Response.Redirect("Default.aspx");
            }
        }
        if (!Page.IsPostBack)
        {
            LoadCustomerDeliverySchedule();
        }
       
    }
    protected void gvCustomerDeliverySchedule_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            int i = 0;
            int iCustomerDeliveryScheduleID = 0;
            Label lblCustomerDeliveryScheduleID;
            DropDownList ddlCustomers;
            CheckBoxList cblSchedule;
            switch (e.CommandName)
            {
                case "Add":
                    lblError.Text = "";
                    ddlCustomers = (DropDownList)gvCustomerDeliverySchedule.FooterRow.FindControl("ddlCustomers");
                    cblSchedule = (CheckBoxList)gvCustomerDeliverySchedule.FooterRow.FindControl("cblSchedule");

                    if (ddlCustomers.SelectedIndex == 0)
                    {
                        lblError.Text = "**A customer is Required!<br/>";
                        lblError.ForeColor = Color.Red;
                        return;
                    }

                    if (CustomerScheduleExists(ddlCustomers.SelectedValue))
                    {
                        sMsg += "**Customer's Delivery Schedule already exists!<br/>";
                    }

                    if (sMsg.Length > 0)
                    {
                        lblError.Text = sMsg;
                        lblError.ForeColor = Color.Red;
                        lblError0.Text = sMsg;
                        lblError0.ForeColor = Color.Red;
                        return;
                    }
                    //Add...
                    try
                    {
                        ArCustomerDeliverySchedule dl = new ArCustomerDeliverySchedule();
                        dl.Customer = ddlCustomers.SelectedValue;
                        if (cblSchedule.Items[0].Selected)
                        {
                            dl.Monday = 1;
                        }
                        else
                        {
                            dl.Monday = 0;
                        }
                        if (cblSchedule.Items[1].Selected)
                        {
                            dl.Tuesday = 1;
                        }
                        else
                        {
                            dl.Tuesday = 0;
                        }
                        if (cblSchedule.Items[2].Selected)
                        {
                            dl.Wednesday = 1;
                        }
                        else
                        {
                            dl.Wednesday = 0;
                        }
                        if (cblSchedule.Items[3].Selected)
                        {
                            dl.Thursday = 1;
                        }
                        else
                        {
                            dl.Thursday = 0;
                        }
                        if (cblSchedule.Items[4].Selected)
                        {
                            dl.Friday = 1;
                        }
                        else
                        {
                            dl.Friday = 0;
                        }

                        dl.DateAdded = DateTime.Now;
                        dl.AddedBy = iUserID;
                        db.ArCustomerDeliverySchedule.InsertOnSubmit(dl);
                        db.SubmitChanges();

                        lblError.Text = "**Add was successful!";
                        lblError.ForeColor = Color.Green;
                        lblError0.Text = "**Add was successful!";
                        lblError0.ForeColor = Color.Green;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "**Add Failed!";
                        lblError.ForeColor = Color.Red;
                        lblError0.Text = "**Add Failed!";
                        lblError0.ForeColor = Color.Red;
                        Debug.WriteLine(ex.ToString());
                    }

                    //Refresh Grid...                
                    LoadCustomerDeliverySchedule();//Refresh...                
                    break;

                case "Delete":
                    i = Convert.ToInt32(e.CommandArgument);

                    try
                    {
                        lblCustomerDeliveryScheduleID = (Label)gvCustomerDeliverySchedule.Rows[i].FindControl("lblCustomerDeliveryScheduleID");
                        iCustomerDeliveryScheduleID = Convert.ToInt32(lblCustomerDeliveryScheduleID.Text);
                        var query = (from d in db.ArCustomerDeliverySchedule select d);
                        if (query.Count() == 1)
                        {
                            lblError.Text = "**You cannot delete the last record in the table.";
                            lblError.ForeColor = Color.Red;
                            lblError0.Text = "**You cannot delete the last record in the table.";
                            lblError0.ForeColor = Color.Red;
                            return;
                        }

                        ArCustomerDeliverySchedule dl = db.ArCustomerDeliverySchedule.Single(p => p.CustomerDeliveryScheduleID == iCustomerDeliveryScheduleID);
                        db.ArCustomerDeliverySchedule.DeleteOnSubmit(dl);
                        db.SubmitChanges();

                        lblError.Text = "**Delete was successful!";
                        lblError.ForeColor = Color.Green;
                        lblError0.Text = "**Delete was successful!";
                        lblError0.ForeColor = Color.Green;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "**Delete Failed!";
                        lblError.ForeColor = Color.Red;
                        lblError0.Text = "**Delete Failed!";
                        lblError0.ForeColor = Color.Red;
                        Debug.WriteLine(ex.ToString());
                    }
                    break;
            }
        }
    }
    protected void gvCustomerDeliverySchedule_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBoxList cblSchedule = (CheckBoxList)e.Row.FindControl("cblSchedule");

            Label lblMonday = (Label)e.Row.FindControl("lblMonday");
            Label lblTuesday = (Label)e.Row.FindControl("lblTuesday");
            Label lblWednesday = (Label)e.Row.FindControl("lblWednesday");
            Label lblThursday = (Label)e.Row.FindControl("lblThursday");
            Label lblFriday = (Label)e.Row.FindControl("lblFriday");

            if (lblMonday.Text == "1")
            {
                cblSchedule.Items[0].Selected = true;
            }
            else
            {
                cblSchedule.Items[0].Selected = false;
            }
            if (lblTuesday.Text == "1")
            {
                cblSchedule.Items[1].Selected = true;
            }
            else
            {
                cblSchedule.Items[1].Selected = false;
            }
            if (lblWednesday.Text == "1")
            {
                cblSchedule.Items[2].Selected = true;
            }
            else
            {
                cblSchedule.Items[2].Selected = false;
            }
            if (lblThursday.Text == "1")
            {
                cblSchedule.Items[3].Selected = true;
            }
            else
            {
                cblSchedule.Items[3].Selected = false;
            }
            if (lblFriday.Text == "1")
            {
                cblSchedule.Items[4].Selected = true;
            }
            else
            {
                cblSchedule.Items[4].Selected = false;
            }


        }
        if (e.Row.RowType == DataControlRowType.Footer)//the footer row...
        {
            DropDownList ddlCustomers = (DropDownList)e.Row.FindControl("ddlCustomers");
            LoadCustomer(ddlCustomers);
        }
    }
    protected void gvCustomerDeliverySchedule_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvCustomerDeliverySchedule.EditIndex = -1;
        LoadCustomerDeliverySchedule();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        UpdateSchedules();

    }





    #endregion




}