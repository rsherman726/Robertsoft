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

public partial class DeliveryAdmin : System.Web.UI.Page
{
    #region Subs
    private void AddDelivery(int iUserID)
    {
        try
        {
            string sMsg = "";

            //Validate...
            if (ddlCustomer.SelectedIndex == 0)
            {
                sMsg += "**Please select a Customer!<br/>";

            }
            if (ddlDeliveryType.SelectedIndex == 0)
            {
                sMsg += "**Please select a delivery type!<br/>";
            }
            if (txtQtyActual.Text.Trim() != "")
            {
                if (!SharedFunctions.IsNumeric(txtQtyActual.Text.Trim()))
                {
                    sMsg += "**Qty Delivered/Picked Up must be a numeric value!<br/>";
                }
            }
            if (txtQtyScheduled.Text.Trim() != "")
            {
                if (!SharedFunctions.IsNumeric(txtQtyScheduled.Text.Trim()))
                {
                    sMsg += "**Qty scheduled must be a numeric value!<br/>";
                }
            }
            //Can have more than one delivery per SO!!!
            ////if (SalesOrderAlreadyExists(txtSalesOrder.Text.Trim()))
            ////{
            ////    sMsg += "**This Sales Order already exists in the delivery table on another record!<br/>";
            ////}

            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }




            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            DelMaster dm = new DelMaster();
            dm.Customer = ddlCustomer.SelectedValue;
            if (txtSalesOrder.Text.Trim() != "")
            {
                dm.SalesOrder = txtSalesOrder.Text.Trim();
            }
            else
            {
                dm.SalesOrder = null;
            }
            if (txtPurchaseOrder.Text.Trim() != "")
            {
                dm.CustomerPoNumber = txtPurchaseOrder.Text.Trim();
            }
            else
            {
                dm.CustomerPoNumber = null;
            }
            if (ddlDeliveryType.SelectedIndex != 0)
            {
                dm.DeliveryTypeID = Convert.ToInt32(ddlDeliveryType.SelectedValue);
            }
            else
            {
                dm.DeliveryTypeID = null;
            }
            if (ddlDeliveryStatus.SelectedIndex != 0)
            {
                dm.DeliveryStatus = Convert.ToInt32(ddlDeliveryStatus.SelectedValue);
            }
            else
            {
                dm.DeliveryStatus = null;
            }
            if (txtQtyActual.Text.Trim() != "")
            {
                dm.QtyActual = Convert.ToInt32(txtQtyActual.Text.Trim());
            }
            else
            {
                dm.QtyActual = null;
            }
            if (txtQtyScheduled.Text.Trim() != "")
            {
                dm.QtyScheduled = Convert.ToInt32(txtQtyScheduled.Text.Trim());
            }
            else
            {
                dm.QtyScheduled = null;
            }
            if (txtDateScheduled.Text.Trim() != "")
            {
                dm.DateScheduled = Convert.ToDateTime(txtDateScheduled.Text.Trim());
            }
            else
            {
                dm.DateScheduled = null;
            }
            if (txtDateDelivered.Text.Trim() != "")
            {
                dm.DateDelivered = Convert.ToDateTime(txtDateDelivered.Text.Trim());
            }
            else
            {
                dm.DateDelivered = null;
            }

            if (ddlVehicle.SelectedIndex != 0)
            {
                dm.VehicleID = Convert.ToInt32(ddlVehicle.SelectedValue);
            }
            else
            {
                dm.VehicleID = null;
            }
            if (txtCheckNumber.Text.Trim() != "")
            {
                dm.CheckNumber = Convert.ToInt32(txtCheckNumber.Text.Trim());
            }
            else
            {
                dm.CheckNumber = null;
            }
            if (txtCheckAmount.Text.Trim() != "")
            {
                dm.Amount = Convert.ToDecimal(txtCheckAmount.Text.Trim());
            }
            else
            {
                dm.Amount = null;
            }
            if (chkIsCOD.Checked)
            {
                dm.IsCOD = 1;
            }
            else
            {
                dm.IsCOD = 0;
            }
            if (txtComments.Text.Trim() != "")
            {
                dm.Comments = txtComments.Text.Trim();
            }
            else
            {
                dm.Comments = null;
            }
            if (txtTrackingNumber.Text.Trim() != "")
            {
                dm.TrackingNumber = txtTrackingNumber.Text.Trim();
            }
            else
            {
                dm.TrackingNumber = null;
            }
            if (txtInternalPO.Text.Trim() != "")
            {
                dm.InternalPoNumber = txtInternalPO.Text.Trim();
            }
            else
            {
                dm.InternalPoNumber = null;
            }

            dm.DateAdded = DateTime.Now;
            dm.AddedBy = iUserID;

            db.DelMaster.InsertOnSubmit(dm);
            db.SubmitChanges();
            lblError.Text = "**Delivery/Pickup Added successfully!";
            lblError.ForeColor = Color.Green;

            Reset();
        }
        catch (Exception ex)
        {
            lblError.Text = "**Delivery/Pickup Added failed!";
            lblError.ForeColor = Color.Red;
            Debug.WriteLine(ex.ToString());
        }
    }
    private void UpdateDelivery(int iUserID, int iSelectedDeliveryID)
    {
        string sMsg = "";

        //Validate...
        if (ddlCustomer.SelectedIndex == 0)
        {
            sMsg += "**Please select a Customer!<br/>";
        }
        if (ddlDeliveryType.SelectedIndex == 0)
        {
            sMsg += "**Please select a delivery type!<br/>";
        }
        if (txtQtyActual.Text.Trim() != "")
        {
            if (!SharedFunctions.IsNumeric(txtQtyActual.Text.Trim()))
            {
                sMsg += "**Qty Delivered/Picked Up must be a numeric value!<br/>";
            }
        }
        if (txtQtyScheduled.Text.Trim() != "")
        {
            if (!SharedFunctions.IsNumeric(txtQtyScheduled.Text.Trim()))
            {
                sMsg += "**Qty scheduled must be a numeric value!<br/>";
            }
        }

        //You can have multiple Deliveries for a single SO!!!
        ////if (SalesOrderAlreadyExistsForUpdate(txtSalesOrder.Text.Trim(),Convert.ToInt32(lblDeliveryID.Text)))
        ////{
        ////    sMsg += "**This Sales Order # already exists in the delivery table on another delivery record!<br/>";
        ////}

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
            
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            DelMaster dm = db.DelMaster.Single(p => p.DeliveryID == iSelectedDeliveryID);
            dm.Customer = ddlCustomer.SelectedValue;
            if(txtSalesOrder.Text.Trim() != "")
            {
                dm.SalesOrder = txtSalesOrder.Text.Trim();
            }
            else
            {
                dm.SalesOrder = null;
            }
            if (txtPurchaseOrder.Text.Trim() != "")
            {
                dm.CustomerPoNumber = txtPurchaseOrder.Text.Trim();
            }
            else
            {
                dm.CustomerPoNumber = null;
            }
            if (ddlDeliveryType.SelectedIndex != 0)
            {
                dm.DeliveryTypeID = Convert.ToInt32(ddlDeliveryType.SelectedValue);
            }
            else
            {
                dm.DeliveryTypeID = null;
            }
            if (ddlDeliveryStatus.SelectedIndex != 0)
            {
                dm.DeliveryStatus = Convert.ToInt32(ddlDeliveryStatus.SelectedValue);
            }
            else
            {
                dm.DeliveryStatus = null;
            }
            if (txtQtyActual.Text.Trim() != "")
            {
                dm.QtyActual = Convert.ToInt32(txtQtyActual.Text.Trim());
            }
            else
            {
                dm.QtyActual = null;
            }
            if (txtQtyScheduled.Text.Trim() != "")
            {
                dm.QtyScheduled = Convert.ToInt32(txtQtyScheduled.Text.Trim());
            }
            else
            {
                dm.QtyScheduled = null;
            }
            if (txtDateScheduled.Text.Trim() != "")
            {
                dm.DateScheduled = Convert.ToDateTime(txtDateScheduled.Text.Trim());
            }
            else
            {
                dm.DateScheduled = null;
            }
            if (txtDateDelivered.Text.Trim() != "")
            {
                dm.DateDelivered = Convert.ToDateTime(txtDateDelivered.Text.Trim());
            }
            else
            {
                dm.DateDelivered = null;
            }
            if (ddlVehicle.SelectedIndex != 0)
            {
                dm.VehicleID = Convert.ToInt32(ddlVehicle.SelectedValue);
            }
            else
            {
                dm.VehicleID = null;
            }
            if (txtCheckNumber.Text.Trim() != "")
            {
                dm.CheckNumber = Convert.ToInt32(txtCheckNumber.Text.Trim());
            }
            else
            {
                dm.CheckNumber = null;
            }
            if (txtCheckAmount.Text.Trim() != "")
            {
                dm.Amount = Convert.ToDecimal(txtCheckAmount.Text.Trim());
            }
            else
            {
                dm.Amount = null;
            }
            if (chkIsCOD.Checked)
            {
                dm.IsCOD = 1;
            }
            else
            {
                dm.IsCOD = 0;
            }
            if (txtComments.Text.Trim() != "")
            {
                dm.Comments = txtComments.Text.Trim();
            }
            else
            {
                dm.Comments = null;
            }
            if (txtTrackingNumber.Text.Trim() != "")
            {
                dm.TrackingNumber = txtTrackingNumber.Text.Trim();
            }
            else
            {
                dm.TrackingNumber = null;
            }
            if (txtInternalPO.Text.Trim() != "")
            {
                dm.InternalPoNumber = txtInternalPO.Text.Trim();
            }
            else
            {
                dm.InternalPoNumber = null;
            }


            dm.ModifiedBy = iUserID;
            dm.DateModified = DateTime.Now;
            db.SubmitChanges();

            BindDelivery(iSelectedDeliveryID);

            lblError.Text = "**Delivery/Pickup updated successfully!";
            lblError.ForeColor = Color.Green;

        }
        catch (Exception ex)
        {
            lblError.Text = "**Delivery/Pickup updated failed!";
            lblError.ForeColor = Color.Red;
            Debug.WriteLine(ex.ToString());
        }
    }
    private void DeleteDelivery(int iSelectedDeliveryID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        try
        {
            DelMaster dm = db.DelMaster.Single(p => p.DeliveryID == iSelectedDeliveryID);
            db.DelMaster.DeleteOnSubmit(dm);
            db.SubmitChanges();

            lblError.Text = "**Delivery Deleted successfully!";
            lblError.ForeColor = Color.Green;

            Reset();
        }
        catch (Exception ex)
        {
            lblError.Text = "**Delivery Delete failed!(You can't delete a user who is associated with another table.e.g. Sales Orders)";
            lblError.ForeColor = Color.Red;
            Debug.WriteLine(ex.ToString());
        }
    }
    private void BindDelivery(int iDeliveryID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from d in db.DelMaster
                     where d.DeliveryID == iDeliveryID
                     select d);
        foreach (var a in query)
        {
            lblDeliveryID.Text = a.DeliveryID.ToString();

            ddlCustomer.SelectedValue = a.Customer.Trim();


            if (a.DeliveryTypeID.HasValue)
            {
                ddlDeliveryType.SelectedValue = a.DeliveryTypeID.ToString();
            }
            if (a.DeliveryStatus.HasValue)
            {
                ddlDeliveryStatus.SelectedValue = a.DeliveryStatus.ToString();
            }
            txtSalesOrder.Text = a.SalesOrder;
            txtPurchaseOrder.Text = a.CustomerPoNumber;
            txtQtyScheduled.Text = a.QtyScheduled.ToString();
            txtQtyActual.Text = a.QtyActual.ToString();
            if (a.DateScheduled.HasValue)
            {
                txtDateScheduled.Text = Convert.ToDateTime(a.DateScheduled).ToShortDateString();
            }
            if (a.DateDelivered.HasValue)
            {
                txtDateDelivered.Text = Convert.ToDateTime(a.DateDelivered).ToShortDateString();
            }
            txtDateAdded.Text = a.DateAdded.ToShortDateString();
            if (a.DateModified.HasValue)
            {
                txtDateModified.Text = a.DateModified.ToString();
            }
            txtAddedBy.Text = SharedFunctions.GetUserFullName(Convert.ToInt32(a.AddedBy));
            if (a.ModifiedBy.HasValue)
            {
                txtModifiedBy.Text = SharedFunctions.GetUserFullName(Convert.ToInt32(a.ModifiedBy));
            }
            if(a.VehicleID.HasValue)
            {
                ddlVehicle.SelectedValue = a.VehicleID.ToString();
            }
  


            if (a.IsCOD == 0)
            {
                chkIsCOD.Checked = false;
                txtCheckNumber.Text = "";
                txtCheckAmount.Text = "";
            }
            else
            {
                chkIsCOD.Checked = true;
                if (a.CheckNumber.HasValue)
                {
                    txtCheckNumber.Text = a.CheckNumber.ToString();
                    ViewState["CheckNumber"] = a.CheckNumber;
                }
                if (a.Amount.HasValue)
                {
                    txtCheckAmount.Text = Convert.ToDecimal(a.Amount).ToString("#,0.00");
                    ViewState["CheckAmount"] = Convert.ToDecimal(a.Amount).ToString("#,0.00");
                }
            }
            if (chkIsCOD.Checked)
            {
                txtCheckAmount.Enabled = true;
                txtCheckNumber.Enabled = true;
                txtCheckAmount.BackColor = Color.LemonChiffon;
                txtCheckNumber.BackColor = Color.LemonChiffon;
            }
            else
            {
                txtCheckAmount.Enabled = false;
                txtCheckNumber.Enabled = false;
                txtCheckAmount.BackColor = Color.LightGray;
                txtCheckNumber.BackColor = Color.LightGray;
            }
            txtComments.Text = a.Comments;
            txtInternalPO.Text = a.InternalPoNumber;
            txtTrackingNumber.Text = a.TrackingNumber;
        }
    }
    private void LoadDeliveriesGrid(string sSearch)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        DataTable dt = new DataTable();
        string sFullName = sSearch;
        string sFirstName = null;
        string sMiddleName = null;
        string sLastName = null;

        string[] name = sFullName.Split(' ');
        sFirstName = name[0].ToString();
        switch (name.Length)
        {
            case 1://Just First Name
                sFirstName = name[0].ToString().ToUpper();
                break;
            case 2://No Middle name
                sFirstName = name[0].ToString().ToUpper();
                sLastName = name[1].ToString().ToUpper();
                break;
            case 3:
                sFirstName = name[0].ToString().ToUpper();
                sMiddleName = name[1].ToString().ToUpper();
                sLastName = name[2].ToString().ToUpper();
                break;
        }

        var qry = (from dm in db.DelMaster
                   join c in db.ArCustomer on dm.Customer equals c.Customer into c_join
                   from c in c_join.DefaultIfEmpty()
                   where (c.Name.Trim().ToUpper().Contains(sSearch.ToUpper())|| sSearch == "")
                   || ( dm.SalesOrder.Trim().Contains(sSearch) || sSearch == "")
                   || ( dm.CustomerPoNumber.Trim().Contains(sSearch) || sSearch == "")
                   || ( dm.DelVehicles.VehDescription.Contains(sSearch) || sSearch == "")
                   || (( dm.DateDelivered.Value.Month.ToString() + "/" + dm.DateDelivered.Value.Day.ToString() +"/"+ dm.DateDelivered.Value.Year.ToString()).Contains(sSearch.Trim()) || sSearch == "")                    
                   select new {

                       dm.DeliveryID,
                       dm.DateDelivered,
                       dm.DateScheduled,
                       Driver = ( dm.DelDrivers.FirstName + " " + ( dm.DelDrivers.MiddleName??"") + " " + dm.DelDrivers.LastName).Replace("  "," "),
                       Vehicle = dm.DelVehicles.VehDescription,
                       dm.SalesOrder,
                       dm.CustomerPoNumber,
                       dm.Customer,
                       c.Name
                   }).Take(50);

        dt = SharedFunctions.ToDataTable(db, qry);
        if (dt.Rows.Count > 0)
        {
            gvResults.DataSource = dt;
            gvResults.DataBind();
        }
        else
        {
            gvResults.DataSource = null;
            gvResults.DataBind();
        }
        Session["dtDeliveries"] = dt;

        dt.Dispose();


    }
    private void LoadCustomerList()
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        ddlCustomer.Items.Clear();
        var qry = (from ar in db.ArCustomer                  
                   orderby ar.Name
                   select ar);
        if (qry.Count() > 0)
        {
            foreach (var a in qry)
            {
                ddlCustomer.Items.Add(new ListItem(a.Name.Trim(), a.Customer.Trim()));
            }
            ddlCustomer.Items.Insert(0, new ListItem("--Select a Customer--", "0"));
        }
        else
        {
            ddlCustomer.Items.Insert(0, new ListItem("--No Customers Found--", "0"));
        }
    }
    private void LoadDeliveryStatus()
    {
        ddlDeliveryStatus.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from r in db.DelStatuses
                     orderby r.DeliveryStatus
                     select r);
        foreach (var a in query)
        {
            ddlDeliveryStatus.Items.Add(new ListItem(a.DeliveryStatus, a.DeliveryStatusID.ToString()));
        }
        ddlDeliveryStatus.Items.Insert(0, new ListItem("--Select a Status--", "0"));
    }
    private void LoadDeliveryTypes()
    {
        ddlDeliveryType.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from r in db.DelTypes
                     orderby r.DeliveryType
                     select r);
        foreach (var a in query)
        {
            ddlDeliveryType.Items.Add(new ListItem(a.DeliveryType, a.DeliveryTypeID.ToString()));
        }
        ddlDeliveryType.Items.Insert(0, new ListItem("--Select a Delivery Type--", "0"));
    }
    private void LoadVehicles()
    {
        ddlVehicle.Items.Clear();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from r in db.DelVehicles
                     select r);
        if (query.Count() > 0)
        {
            foreach (var a in query)
            {
                ddlVehicle.Items.Add(new ListItem(a.VehDescription, a.VehicleID.ToString()));
            }
            ddlVehicle.Items.Insert(0, new ListItem("--Select a Vehicle--", "0"));
        }
        else
        {
            ddlVehicle.Items.Insert(0, new ListItem("--No Vehicles Found--", "0"));
        }
    }
    private void Reset()
    {
        lblDeliveryID.Text = "";
        ddlCustomer.SelectedValue = "0";
        ddlDeliveryType.SelectedValue = "0";
        ddlDeliveryStatus.SelectedValue = "0";
        txtSalesOrder.Text = "";
        txtPurchaseOrder.Text = "";
        txtQtyScheduled.Text = "";
        txtQtyActual.Text = "";
        txtDateScheduled.Text = "";
        txtDateDelivered.Text = "";
        txtDateAdded.Text = "";
        txtDateModified.Text = "";
        txtAddedBy.Text = "";
        txtModifiedBy.Text = "";
        ddlVehicle.SelectedValue = "0";
        txtCheckNumber.Text = "";
        txtCheckAmount.Text = "";
        chkIsCOD.Checked = false;
        txtComments.Text = "";
        gvResults.DataSource = null;
        gvResults.DataBind();

    }

    #endregion

    #region Functions
    private bool isDeliveryComplete(int iUserID)
    {
        bool bComplete = false;
        string sAddress = "";
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from u in db.WipUsers
                     where u.UserID == iUserID
                     select new { u.Address });
        foreach (var a in query)
        {
            sAddress = a.Address;
        }
        if (String.IsNullOrEmpty(sAddress))
        {
            bComplete = false;
        }
        else
        {
            bComplete = true;
        }

        return bComplete;
    }
    private bool SalesOrderAlreadyExists(string  sSalesOrder)
    {    
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from d in db.DelMaster
                     where d.SalesOrder == sSalesOrder
                     select d);
        if (query.Count() > 0)
        {
            return true;
        }
        else
        {
            return false;
        }        
    }
    private bool SalesOrderAlreadyExistsForUpdate(string sSalesOrder, int iDeliveryID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from d in db.DelMaster
                     where d.SalesOrder == sSalesOrder
                     && d.DeliveryID != iDeliveryID//For Any other Delivery record?
                     select d);
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
        else
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
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
            LoadDeliveryStatus();
            LoadDeliveryTypes();
            LoadVehicles();
            LoadCustomerList();
            ibnAdd.Enabled = false;
            ibnDelete.Enabled = false;
            ibnSave.Enabled = false;
            string sDeliveryID = "";
            if (Request.QueryString["id"] != null)
            {//Comes From Sales Order Tracker Delivery Details Popup...
                sDeliveryID = Request.QueryString["id"].ToString();
                BindDelivery(Convert.ToInt32(sDeliveryID));
                ibnSave.Enabled = true;
            }
            string sSalesOrder = "";
            if (Request.QueryString["type"] != null && Request.QueryString["id"]!= null)
            {//Comes From Sales Order Tracker...
                if (Request.QueryString["type"].ToString() == "so")
                {
                    ddlDeliveryType.SelectedValue="2";//Delivery
                    txtPurchaseOrder.Visible = true;
                    LabelPO.Visible = true;
                    txtSalesOrder.Visible = true;
                    LabelSO.Visible = true;
                    ibnSearch.Visible = false;
                    txtSearch.Visible = false;
                    ibnSave.Enabled = false;
                    ibnDelete.Enabled = false;
                    ibnAdd.Enabled = true;
                    sSalesOrder = Request.QueryString["id"].ToString();
                    rblMode.SelectedIndex = 0;
                    //rblMode_SelectedIndexChanged(null, null);
                    txtSalesOrder.Text = sSalesOrder;
                    txtSalesOrder_TextChanged(null, null);
                }
            }

        }

    }
    protected void ibnSave_Click(object sender, EventArgs e)
    {
        //Save Delivery...
        lblError.Text = "";
        int iUserID = 0;
        int iSelectedDeliveryID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (lblDeliveryID.Text == "")
        {
            lblError.Text = "**No Delivery Selected!!";
            return;
        }

        iSelectedDeliveryID = Convert.ToInt32(lblDeliveryID.Text);
        UpdateDelivery(iUserID, iSelectedDeliveryID);
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

        AddDelivery(iUserID);
    }
    protected void ibnDelete_Click(object sender, EventArgs e)
    {//Delete Delivery...
        lblError.Text = "";
        int iSelectedDeliveryID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        if (lblDeliveryID.Text == "")
        {
            lblError.Text = "**No Delivery Selected!!";
            return;
        }

        iSelectedDeliveryID = Convert.ToInt32(lblDeliveryID.Text);
        DeleteDelivery(iSelectedDeliveryID);
    }
    protected void ibnSearch_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        LoadDeliveriesGrid(txtSearch.Text.Trim());
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        LoadDeliveriesGrid(txtSearch.Text.Trim());
    }
    protected void rblMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";

        if (rblMode.SelectedIndex == 0)//Add...
        {
            Reset();
            ibnAdd.Enabled = true;
            ibnSave.Enabled = false;
            ibnDelete.Enabled = false;
            LabelDeliveryID.Visible = false;

            txtSearch.Visible = false;
            ibnSearch.Visible = false;
            txtAddedBy.Visible = false;
            LabelAddedBy.Visible = false;
            txtDateAdded.Visible = false;
            LabelInputDate.Visible = false;
            txtModifiedBy.Visible = false;
            LabelDateModified.Visible = false;
            txtDateModified.Visible = false;
        }
        else//Edit...
        {
            ibnAdd.Enabled = false;
            txtSearch.Visible = true;
            ibnSearch.Visible = true;
            LabelDeliveryID.Visible = true;
            Reset();
            ibnSave.Enabled = false;
            ibnDelete.Enabled = false;

            txtAddedBy.Visible = true;
            LabelAddedBy.Visible = true;
            txtDateAdded.Visible = true;
            LabelInputDate.Visible = true;
            txtModifiedBy.Visible = true;
            LabelDateModified.Visible = true;
            txtDateModified.Visible = true;

        }
    }
    protected void txtSalesOrder_TextChanged(object sender, EventArgs e)
    {
        string sSalesOrder = "";
        if (txtSalesOrder.Text.Trim() != "")
        {
            sSalesOrder = txtSalesOrder.Text.Trim();
        }
        string sPurchaseOrder = "";
        sPurchaseOrder = SharedFunctions.GetPurchaseOrderWithSalesOrder(sSalesOrder);//If there is one...
        string sCustomerID = "";
        sCustomerID = SharedFunctions.GetCustomerIDWithSalesOrder(sSalesOrder);
        if (sCustomerID != "")
        {
            ddlCustomer.SelectedValue = sCustomerID;
        }

        
        txtPurchaseOrder.Text = sPurchaseOrder;
        if (sPurchaseOrder != "")
        {
            ddlDeliveryStatus.Focus();
        }
    }
    protected void gvResults_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            Label lblDateDelivered = (Label)e.Row.FindControl("lblDateDelivered");
            Label lblDateScheduled = (Label)e.Row.FindControl("lblDateScheduled");
            if (lblDateScheduled.Text != "")
            {

                lblDateScheduled.Text = Convert.ToDateTime(lblDateScheduled.Text).ToShortDateString();
            }
            else
            {
                lblDateScheduled.Text = "Not Assigned";
            }
            if (lblDateDelivered.Text != "")
            {

                lblDateDelivered.Text = Convert.ToDateTime(lblDateDelivered.Text).ToShortDateString();
            }
            else
            {
                lblDateDelivered.Text = "Not Assigned";
            }
            Label lblDriver = (Label)e.Row.FindControl("lblDriver");
            Label lblVehicle = (Label)e.Row.FindControl("lblVehicle");            
            Label lblPO = (Label)e.Row.FindControl("lblPO");
            Label lblSO = (Label)e.Row.FindControl("lblSO");
          
            if (lblVehicle.Text == "")
            {
                lblVehicle.Text = "Not Assigned";
            }
            if (lblDriver.Text == "")
            {
                lblDriver.Text = "Not Assigned";
            }           
            if (lblPO.Text == "")
            {
                lblPO.Text = "Not Assigned";
            }
            if (lblSO.Text == "")
            {
                lblSO.Text = "Not Assigned";
            }
        }
    }
    protected void gvResults_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int iDeliveryID = 0;

        switch (e.CommandName)
        {
            case "Select":
                iDeliveryID = Convert.ToInt32(e.CommandArgument);
                if (iDeliveryID != 0)
                {

                    ibnSave.Enabled = true;
                    ibnDelete.Enabled = true;
                    Session["DeliveryID"] = iDeliveryID;
                    //Bind Details View here...
                    BindDelivery(iDeliveryID);
                }
                else
                {
                    Reset();
                    ibnSave.Enabled = false;
                    ibnDelete.Enabled = false;
                }

               


                break;


        }
    }
    protected void chkIsCOD_CheckedChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        if (chkIsCOD.Checked)
        {
            txtCheckAmount.Enabled = true;
            txtCheckNumber.Enabled = true;
            txtCheckAmount.BackColor = Color.LightPink;
            txtCheckNumber.BackColor = Color.LightPink;
            if (ViewState["CheckNumber"] != null)
            {
                txtCheckNumber.Text = ViewState["CheckNumber"].ToString();
            }
            if (ViewState["CheckAmount"] != null)
            {
                txtCheckAmount.Text = Convert.ToDecimal(ViewState["CheckAmount"]).ToString("#,0.00");
            }
        }
        else
        {
            txtCheckAmount.Enabled = false;
            txtCheckNumber.Enabled = false;
            txtCheckAmount.BackColor = Color.LightGray;
            txtCheckNumber.BackColor = Color.LightGray;
            txtCheckAmount.Text = "";
            txtCheckNumber.Text = "";
        }
    }
    protected void ddlVehicle_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (ddlVehicle.SelectedItem.Text.ToUpper())
        {
            case "UPS":
                txtTrackingNumber.Visible = true;
                LabelTrack.Visible = true;
                break;
            case "FEDEX":
                txtTrackingNumber.Visible = true;
                LabelTrack.Visible = true;
                break;
            case "FREIGHT COLLECT":
                txtTrackingNumber.Visible = true;
                LabelTrack.Visible = true;
                break;
            default:
                txtTrackingNumber.Visible = false;
                LabelTrack.Visible = false;
                break;
        }
    }
    protected void ddlDeliveryType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlDeliveryType.SelectedIndex != 0)
        {
            if (ddlDeliveryType.SelectedValue =="1")
            {//Supplier Pickup...
                txtPurchaseOrder.Visible = false;
                LabelPO.Visible = false;
                txtSalesOrder.Visible = false;
                LabelSO.Visible = false;
                txtInternalPO.Visible = true;
                LabelInternalPO.Visible = true;
            }
            else if (ddlDeliveryType.SelectedValue == "2")//Delivery...
            {
                txtPurchaseOrder.Visible = true;
                LabelPO.Visible = true;
                txtSalesOrder.Visible = true;
                LabelSO.Visible = true;
                txtInternalPO.Visible = false;
                LabelInternalPO.Visible = false;
            }
            else//Will Call Pick...
            {
                txtPurchaseOrder.Visible = true;
                LabelPO.Visible = true;
                txtSalesOrder.Visible = true;
                LabelSO.Visible = true;
                txtInternalPO.Visible = false;
                LabelInternalPO.Visible = false;
            }
        }
        else
        {
            txtPurchaseOrder.Visible = false;
            LabelPO.Visible = false;
            txtSalesOrder.Visible = false;
            LabelSO.Visible = false;
            txtInternalPO.Visible = false;
            LabelInternalPO.Visible = false;
        }
    }

    #endregion

    #region WebMethods
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListDelivery(string prefixText, int count, string contextKey)
    {//...
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {

            // Your LINQ to SQL query goes here 
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            string[] list = db.DelMaster.Where(w => w.DriverID != null && w.VehicleID != null && w.DateDelivered != null).OrderBy(w => w.DateDelivered).Select(w => w.DateDelivered.ToString()).ToArray();
            string[] lResult = null;
            try
            {
                lResult = (from d in list where d.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select d).Take(count).ToArray();
            }
            catch (Exception ex)
            {
                lResult = new string[] { "None found" };
                Debug.WriteLine(ex.ToString());

            }
            return lResult;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListSalesOrders(string prefixText, int count, string contextKey)
    {//...
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {

            // Your LINQ to SQL query goes here 
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            string[] list = db.SorMaster.Where(w => w.SalesOrder != null).OrderBy(w => w.SalesOrder).Select(w => w.SalesOrder.ToString()).ToArray();
            string[] lResult = null;
            try
            {
                lResult = (from d in list where d.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select d).Take(count).ToArray();
            }
            catch (Exception ex)
            {
                lResult = new string[] { "None found" };
                Debug.WriteLine(ex.ToString());

            }
            return lResult;
        }
    }
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListPurchaseOrders(string prefixText, int count, string contextKey)
    {//...
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {

            // Your LINQ to SQL query goes here 
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            string[] list = db.SorMaster.Where(w => w.CustomerPoNumber != null).OrderBy(w => w.CustomerPoNumber).Select(w => w.CustomerPoNumber.ToString()).Distinct().ToArray();
            string[] lResult = null;
            try
            {
                lResult = (from d in list where d.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase) select d).Take(count).ToArray();
            }
            catch (Exception ex)
            {
                lResult = new string[] { "None found" };
                Debug.WriteLine(ex.ToString());

            }
            return lResult;
        }
    }
    #endregion








}