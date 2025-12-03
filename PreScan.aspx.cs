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

public partial class PreScan : System.Web.UI.Page
{
    private int iDeliveryID = 0;

    #region Subs


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
        string sDocType = "";
        string sPurchaseOrder = "";
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
            if (Request.QueryString["id"] != null)
            {
                if (Request.QueryString["type"] != null)
                {
                    sDocType = Request.QueryString["type"].ToString();
                    if (sDocType == "PO")
                    {
                       
                        string sSO = "";
                        if (Request.QueryString["so"] != null)
                        {
                            sSO = Request.QueryString["so"].ToString();
                            lblSO.Text = sSO;
                        }
                        DataTable dt = SharedFunctions.GetCompanyInfoFromSalesOrder(sSO);
                        sPurchaseOrder = Request.QueryString["id"].ToString();
                        lblID.Text = sPurchaseOrder;
                        lblDocType.Text = "Purchase Order";
                        lblScheduledDeliveryDate.Text = "Not Applicable";
                        lblPO.Text = sPurchaseOrder;
                        lblCustomer.Text = SharedFunctions.GetCustomerName(dt.Rows[0]["Customer"].ToString());
                        dt.Dispose();
                    }
                    else//DR
                    {
                        iDeliveryID = Convert.ToInt32(Request.QueryString["id"]);
                        DataTable dt = SharedFunctions.GetDeliveryInfo(iDeliveryID);
                        lblID.Text = iDeliveryID.ToString();
                        if (sDocType == "DR")
                        {
                            lblDocType.Text = "Delivery/Will Call Receipt";
                            LabelSalesOrder.Visible = true;
                            lblSO.Visible = true;
                            lblPO.Text = dt.Rows[0]["CustomerPoNumber"].ToString();
                        }
                        else
                        {
                            lblDocType.Text = "Pickup";
                            LabelSalesOrder.Visible = false;
                            lblSO.Visible = false;
                            LabelPO.Text = "Internal Purchase Order";
                            lblPO.Text = dt.Rows[0]["InternalPoNumber"].ToString();

                        }
                        lblSO.Text = dt.Rows[0]["SalesOrder"].ToString();                        
                        lblCustomer.Text = SharedFunctions.GetCustomerName(dt.Rows[0]["Customer"].ToString());
                        DateTime? dtScheduledDeliveryDate = Convert.ToDateTime(GetScheduledDeliveryDate(iDeliveryID));
                        if (dtScheduledDeliveryDate != null && dtScheduledDeliveryDate != Convert.ToDateTime("1/1/0001"))
                        {
                            lblScheduledDeliveryDate.Text = Convert.ToDateTime(dtScheduledDeliveryDate).ToShortDateString();

                            HttpContext.Current.Session["DateDelivered"] = dtScheduledDeliveryDate;//Load scheduled date into the session variable...
                        }
                        else
                        {
                            lblScheduledDeliveryDate.Text = "";
                            lblError.Text = "**No Scheduled delivery/Pickup date! Go back and enter one before you can scan the delivery receipt";
                            lblError.ForeColor = Color.Red;
                        }
                        dt.Dispose();
                    }
                }

            }
            
        }

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblError.Text = "";



        string sNewName = "";
        if (lblDocType.Text == "Delivery/Will Call Receipt" || lblDocType.Text == "Pickup")
        {
            if (lblScheduledDeliveryDate.Text == "")
            {
                lblError.Text = "**No Scheduled delivery/pick date! Go back and enter one before you can scan the delivery receipt";
                lblError.ForeColor = Color.Red;
                return;
            }

            sNewName = "Delivery_Receipt";
            if (txtDateDelivered.Text.Trim() != "")
            {//Overwrite DateScheduled in session variable with user input...
                HttpContext.Current.Session["DateDelivered"] = Convert.ToDateTime(txtDateDelivered.Text.Trim());
            }
            else
            {
                if (HttpContext.Current.Session["DateDelivered"] == null)
                {//Not good...
                    lblError.Text = "**ERROR with delivery date session variable!!";
                    lblError.ForeColor = Color.Red;
                    return;
                }
            }
        }
        else//PO
        {
            sNewName = "Purchase_Order";
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
        if (HttpContext.Current.Session["DateDelivered"] != null)
        {
            Cache["DateDelivered"] = HttpContext.Current.Session["DateDelivered"].ToString();//Use the Cache...
        }
        Session["DocID"] = sDocID;
        Response.Redirect("ScanDocs.aspx");

    }


    #endregion
}