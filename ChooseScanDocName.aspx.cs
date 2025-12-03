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

public partial class ChooseScanDocName : System.Web.UI.Page
{
    #region Subs
    private void LoadDocsNames(int iDocTypeID)
    {
        rblDocs.Items.Clear();
        switch (iDocTypeID)
        {
            case 0:
                rblDocs.Items.Add(new ListItem("&nbsp;Delivery Receipt", "Delivery Receipt"));
                rblDocs.SelectedIndex = 0;
                txtCustomName.Visible = false;
                break;
            case 1:
                rblDocs.Items.Add(new ListItem("&nbsp;Pickup", "Pickup"));
                txtCustomName.Visible = false;
                rblDocs.SelectedIndex = 0;
                break;
            case 2:
                rblDocs.Items.Add(new ListItem("&nbsp;Purchase Order", "Purchase Order"));
                txtCustomName.Visible = false;
                break;
            case 3:
                rblDocs.Items.Add(new ListItem("&nbsp;Custom Name", "Custom Name"));
                txtCustomName.Visible = true;
                rblDocs.SelectedIndex = 0;
                break;
        }   
    }
    private void SearchIDs(string sSearch)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
       
        DataTable dtSearchIDs = new DataTable();
        switch (rblDocTypeIDSearch.SelectedIndex)
        {
            case 0://Delivery/Will Call...
                var qry = (from dm in db.DelMaster
                           join c in db.ArCustomer on dm.Customer equals c.Customer into c_join
                           from c in c_join.DefaultIfEmpty()
                           where
                                (new string[] { "2", "3" }).Contains(dm.DeliveryTypeID.ToString()) &&
                            (dm.DeliveryID.ToString() == sSearch ||
                             dm.SalesOrder.Trim() == sSearch ||
                             dm.CustomerPoNumber.Trim().Replace(" ", "").ToUpper().Contains(sSearch) ||
                             (c.Name.Trim().ToUpper().Contains(sSearch.ToUpper())  ) ||
                             (c.Customer.Trim().Contains(sSearch.ToUpper())  ) ||
                             (dm.DateDelivered.Value.Month.ToString() + "/" + dm.DateDelivered.Value.Day.ToString() + "/" + dm.DateDelivered.Value.Year.ToString() == sSearch))
                            
                           select new
                           {
                               ID = dm.DeliveryID,
                               Company = c.Name,
                               Date = dm.DateDelivered,
                               SalesOrder = dm.SalesOrder.Trim(),
                               CustomerPoNumber = dm.CustomerPoNumber.Trim()
                           }).Take(100);
                // int iCount = qry.Count();
                dtSearchIDs = SharedFunctions.ToDataTable(db, qry);
                gvResults.DataSource = dtSearchIDs;
                gvResults.DataBind();
                Session["dtSearchIDs"] = dtSearchIDs;
                break;
            case 1://Pickup...
                var qry0 = (from dm in db.DelMaster
                            join c in db.ArCustomer on dm.Customer equals c.Customer into c_join
                            from c in c_join.DefaultIfEmpty()
                            where dm.DeliveryTypeID == 1  
                            &&                            
                            (c.Name.Trim().ToUpper().Contains(sSearch.ToUpper())  
                             || dm.DeliveryID.ToString() == sSearch
                             || c.Customer.Trim().Contains(sSearch.ToUpper()) ) 
                            ||(dm.DateDelivered.Value.Month.ToString() + "/" + dm.DateDelivered.Value.Day.ToString() + "/" + dm.DateDelivered.Value.Year.ToString() == sSearch)
                            
                           select new
                           {
                               ID = dm.DeliveryID,
                               Company = c.Name,
                               Date = dm.DateDelivered,
                               SalesOrder = dm.SalesOrder.Trim(),
                               CustomerPoNumber = dm.CustomerPoNumber.Trim()
                           }).Take(100).Distinct();
                // int iCount = qry.Count();
                dtSearchIDs = SharedFunctions.ToDataTable(db, qry0);
                gvResults.DataSource = dtSearchIDs;
                gvResults.DataBind();
                Session["dtSearchIDs"] = dtSearchIDs;
                break;
            case 2://Purchase ORders...                 
                var qry1 = (from sm in db.SorMaster
                            join c in db.ArCustomer on sm.Customer equals c.Customer
                            where                             
                              sm.SalesOrder.Trim() == sSearch ||
                              sm.CustomerPoNumber.Trim().Replace(" ", "").ToUpper().Contains(sSearch) ||
                             (c.Name.Trim().ToUpper().Contains(sSearch.ToUpper()) || sSearch == "") ||
                             (c.Customer.Trim().Contains(sSearch.ToUpper()) || sSearch == "") ||
                             ((sm.OrderDate.Value.Month.ToString() + "/" + sm.OrderDate.Value.Day.ToString() + "/" + sm.OrderDate.Value.Year.ToString()) == sSearch
                             || sSearch == "")
                            orderby sm.OrderDate descending
                            select new
                            {
                                ID = sm.CustomerPoNumber,
                                Company = c.Name,
                                Date = sm.OrderDate,
                                SalesOrder = sm.SalesOrder.Trim(),
                                CustomerPoNumber = sm.CustomerPoNumber.Trim()
                            }).Take(100);
                dtSearchIDs = SharedFunctions.ToDataTable(db, qry1);
                gvResults.DataSource = dtSearchIDs;
                gvResults.DataBind();
                Session["dtSearchIDs"] = dtSearchIDs;
                break;
            case 3://Other...
                var qry2 = (from sm in db.SorMaster
                            join c in db.ArCustomer on sm.Customer equals c.Customer
                            where
                              sm.SalesOrder.Trim() == sSearch ||
                              sm.CustomerPoNumber.Trim().Replace(" ", "").ToUpper().Contains(sSearch) ||
                             (c.Name.Trim().ToUpper().Contains(sSearch.ToUpper()) || sSearch == "") ||
                             (c.Customer.Trim().Contains(sSearch.ToUpper()) || sSearch == "") ||
                             ((sm.OrderDate.Value.Month.ToString() + "/" + sm.OrderDate.Value.Day.ToString() + "/" + sm.OrderDate.Value.Year.ToString()) == sSearch
                             || sSearch == "")
                            orderby sm.OrderDate descending
                            select new
                            {
                                ID = sm.SalesOrder,
                                Company = c.Name,
                                Date = sm.OrderDate,
                                SalesOrder = sm.SalesOrder.Trim(),
                                CustomerPoNumber = sm.CustomerPoNumber.Trim()
                            }).Take(100);
                dtSearchIDs = SharedFunctions.ToDataTable(db, qry2);
                gvResults.DataSource = dtSearchIDs;
                gvResults.DataBind();
                Session["dtSearchIDs"] = dtSearchIDs;
                break;
        }


        dtSearchIDs.Dispose();
    }

    #endregion

    #region Functions
    private DateTime? GetScheduledDeliveryDate(int iDeliveryID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        DateTime? dtScheduledDeliveryDate = (DateTime?)db.DelMaster.Where(p => p.DeliveryID == iDeliveryID).FirstOrDefault().DateScheduled;
        return dtScheduledDeliveryDate;
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

        }

        if (!Page.IsPostBack)
        {
            LoadDocsNames(0);//Default is Delivery Receipt...
            
        }
    
    }
 
    protected void gvResults_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            Label lblDate = (Label)e.Row.FindControl("lblDate");

            if (lblDate.Text != "")
            {

                lblDate.Text = Convert.ToDateTime(lblDate.Text).ToShortDateString();
            }
        }
    }
    protected void gvResults_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string sID = "0";

        switch (e.CommandName)
        {
            case "Select":
                sID = e.CommandArgument.ToString().Trim();
                if (sID != "0")
                {


                    Session["ID"] = sID;
                    //Bind Details View here...
                    lblID.Text = sID;

                    if (rblDocTypeIDSearch.SelectedIndex == 0)//Delivery Receipt
                    {
                        int iDeliveryID = 0;
                        iDeliveryID = Convert.ToInt32(lblID.Text);
                        lblID.Text = iDeliveryID.ToString();
                        lblDocType.Text = "Delivery/Will Call Receipt";

                        DateTime? dtScheduledDeliveryDate = Convert.ToDateTime(GetScheduledDeliveryDate(iDeliveryID));
                        if (dtScheduledDeliveryDate != null && dtScheduledDeliveryDate != Convert.ToDateTime("1/1/0001"))
                        {
                            lblScheduledDeliveryDate.Text = Convert.ToDateTime(dtScheduledDeliveryDate).ToShortDateString();

                            HttpContext.Current.Session["DateDelivered"] = dtScheduledDeliveryDate;//Load scheduled date into the session variable...
                        }
                        else
                        {
                            lblScheduledDeliveryDate.Text = "";
                            lblError.Text = "**No Scheduled delivery date! Go back and enter one before you can scan the delivery receipt";
                            lblError.ForeColor = Color.Red;
                        }
                    }    
                    else if (rblDocTypeIDSearch.SelectedIndex == 1)//Pickup Receipt
                    {
                        int iDeliveryID = 0;
                        iDeliveryID = Convert.ToInt32(lblID.Text);
                        lblID.Text = iDeliveryID.ToString();
                        lblDocType.Text = "Pick Up";

                        DateTime? dtScheduledDeliveryDate = Convert.ToDateTime(GetScheduledDeliveryDate(iDeliveryID));
                        if (dtScheduledDeliveryDate != null && dtScheduledDeliveryDate != Convert.ToDateTime("1/1/0001"))
                        {
                            lblScheduledDeliveryDate.Text = Convert.ToDateTime(dtScheduledDeliveryDate).ToShortDateString();

                            HttpContext.Current.Session["DateDelivered"] = dtScheduledDeliveryDate;//Load scheduled date into the session variable...
                        }
                        else
                        {
                            lblScheduledDeliveryDate.Text = "";
                            lblError.Text = "**No Scheduled delivery date! Go back and enter one before you can scan the delivery receipt";
                            lblError.ForeColor = Color.Red;
                        }
                    }
                    else if (rblDocTypeIDSearch.SelectedIndex == 2)
                    {
                        lblDocType.Text = "Purchase Order";
                        lblScheduledDeliveryDate.Text = "N/A";
                    }
                    else
                    {
                        lblScheduledDeliveryDate.Text = "N/A";
                        lblDocType.Text = "Other";
                    }
                
                }
                else
                {


                }

                break;
        }
    }
    protected void gvResults_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvResults.PageIndex = e.NewPageIndex;
        //use Datatable from memory...
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtSearchIDs"];
        gvResults.DataSource = dt;
        gvResults.DataBind();
    }
    protected void btnSearchIDs_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblID.Text = "";
        lblDocType.Text = "";
        lblScheduledDeliveryDate.Text = "";
        SearchIDs(txtSearchIDs.Text.Trim());
    }
    protected void txtSearchIDs_TextChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblID.Text = "";
        lblScheduledDeliveryDate.Text = "";
        SearchIDs(txtSearchIDs.Text.Trim());
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        string sNewName = "";
        switch (rblDocTypeIDSearch.SelectedIndex)
        {
            case 0://Delivery Receipts
                switch (rblDocs.SelectedIndex)
                {
                    case 0:
                        sNewName = "Delivery_Receipt";
                        break;
                }
                break;
            case 1://Pickup Receipts
                switch (rblDocs.SelectedIndex)
                {
                    case 0:
                        sNewName = "Pickup";
                        break;
                }
                break;
            case 2://Purchase Orders
                switch (rblDocs.SelectedIndex)
                {
                    //Rename the doc here...
                    case 0: 
                        sNewName = "Purchase_Order";
                        break;                   
                    case 1:
                        if (lblID.Text.Trim() == "")
                        {
                            lblError.Text = "**If you select Custom Name, you must fill in the textbox below it!<br/>";
                            return;
                        }
                        sNewName = txtCustomName.Text.Trim().Replace(" ", "_").Replace("-", "_").Replace("/", "_").Replace("\\", "_");
                        break;
                }
                break;            
        }

        if (lblID.Text == "")
        {
            lblError.Text = "**No Document Name!!";
            return;
        }
        string sDocID = lblID.Text.Trim().Replace(" ", "_").Replace("-", "_").Replace("/", "_").Replace("\\", "_") + "_" + sNewName;
        //Validation...
        //Does file already Exists?...
        if (SharedFunctions.DocumentAlreadyExists(Path.GetFileNameWithoutExtension(sDocID)))
        {
            lblError.Text = "**Document already exists for this ID and Document Type!!";
            lblError.ForeColor = Color.Red;
            return;
        }
        if (rblDocTypeIDSearch.SelectedIndex == 0 || rblDocTypeIDSearch.SelectedIndex == 1)//Delivery/Pickup Receipt
        {
            if (lblScheduledDeliveryDate.Text == "")
            {
                lblError.Text = "**No Scheduled delivery/pickup date! Go back and enter one before you can scan the delivery receipt";
                lblError.ForeColor = Color.Red;
                return;
            }

            if (txtDateDelivered.Text.Trim() != "")
            {//Overwrite DateScheduled in session variable with user input...
                HttpContext.Current.Session["DateDelivered"] = Convert.ToDateTime(txtDateDelivered.Text.Trim());
            }
            else
            {
                if (HttpContext.Current.Session["DateDelivered"] == null)
                {//Not good...
                    lblError.Text ="**ERROR with delivery date session variable!!";
                    lblError.ForeColor = Color.Red;
                    return;
                }
            }
        }
        if (HttpContext.Current.Session["DateDelivered"] != null)
        {
            Cache["DateDelivered"] = HttpContext.Current.Session["DateDelivered"].ToString();//Use the Cache...
        }
        Session["DocID"] = sDocID;
        Response.Redirect("ScanDocs.aspx");

    }
    protected void rblDocTypeIDSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDocsNames(rblDocTypeIDSearch.SelectedIndex);
    }


    #endregion


}