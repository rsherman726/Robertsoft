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
using Microsoft.VisualBasic.FileIO;
using System.Text;
using System.Data.Linq.SqlClient;

public partial class SalesOrderTracker : System.Web.UI.Page
{
    static int TickNumber;

    //decimal dcLineTotal = 0;
    //decimal dcGrandTotal = 0;
    //decimal dcShortageTotal = 0;
    //decimal dcQtyTotal = 0;
    //decimal dcPlacedOrdersCountTotal = 0;
    //decimal dcPlacedOrdersTotal = 0;
    //decimal dcPlacedLinesCountTotal = 0;
    //decimal dcCurrentYTDTotal = 0;
    decimal dcLastMTDTotal = 0;
    //decimal dcCurrentMTDTotal = 0;
    //decimal dcReadyToInvoiceTotal = 0;
    //decimal dcOpenOrdersTotal = 0;

    //Top 15...
    decimal dcLineTotal = 0;
    decimal dcGrandTotal = 0;
    decimal dcShortageTotal = 0;
    decimal dcQtyTotal = 0;
    decimal dcPlacedOrdersCountTotal = 0;
    decimal dcPlacedOrdersTotal = 0;
    decimal dcPlacedLinesCountTotal = 0;

    decimal dcPlacedOrdersCountUnchangedTotal = 0;
    decimal dcPlacedOrdersUnchangedTotal = 0;
    decimal dcPlacedLinesCountUnchangedTotal = 0;

    decimal dcPlacedOrdersCountDiffTotal = 0;
    decimal dcPlacedOrdersDiffTotal = 0;
    decimal dcPlacedLinesCountDiffTotal = 0;

    decimal dcCurrentYTDTotal = 0;
    decimal dcLastYTDTotal = 0;
    decimal dcLastYTDMinusOneTotal = 0;


    decimal dcCurrentYTDEndOfMonthTotal = 0;
    decimal dcLastYTDEndOfMonthTotal = 0;
    decimal dcLastYTDMinusOneYearEndOfMonthAmountTotal = 0;

    decimal dcCurrentMTDTotal = 0;
    decimal dcLastYearCurrentMTDTotal = 0;

    decimal dcCurrentMonthTotal = 0;
    decimal dcCurrentMonthCostTotal = 0;
    decimal dcLastYearCurrentMonthTotal = 0;
    decimal dcPreviousMonthCostTotal = 0;

    decimal dcPreviousMonthTotal = 0;
    decimal dcLastYearPreviousMonthTotal = 0;


    decimal dcReadyToInvoiceTotal = 0;
    decimal dcOpenOrdersTotal = 0;

    decimal dcCombinedOpenOrdersAmount = 0;

    decimal dcYTDCostTotal = 0;
    decimal dcLastYTDCostTotal = 0;

    decimal dcYTDEndOfMonthCostTotal = 0;
    decimal dcLastYTDEndOfMonthCostTotal = 0;


    /// <summary>
    /// New 1-28-2020
    /// </summary>

    decimal dcYearToYearDiffPercentageWeighted = 0;
    decimal dcYearToYearDiffPercentageMinusOneWeighted = 0;

    decimal dcYearToYearDiffPercentageEndOfMonthWeighted = 0;
    decimal dcYearToYearDiffPercentageEndOfMonthMinusOneWeighted = 0;

    decimal dcCurrentYTDAmountPercentageOfTotalSum = 0;
    decimal dcLastYTDAmountPercentageOfTotalSum = 0;

    decimal dcCurrentYTDEndOfMonthAmountPercentageOfTotalSum = 0;
    decimal dcLastYTDEndOfMonthAmountPercentageOfTotalSum = 0;
    decimal dcQtyBreakdownTotal = 0;



    #region Properties
    private string GridViewSortDirection
    {
        get
        {
            return ViewState["SortDirection"] as string ?? "DESC";
        }
        set
        {
            ViewState["SortDirection"] = value;
        }
    }
    private bool bScanned
    {
        get
        {
            return (bool)ViewState["bScanned"];
        }
        set
        {
            ViewState["bScanned"] = value;
        }
    }
    private bool bAdvancedScanned
    {//Can un check a scanned checkbox...
        get
        {
            return (bool)ViewState["bAdvancedScanned"];
        }
        set
        {
            ViewState["bAdvancedScanned"] = value;
        }
    }
    private bool bStaged
    {
        get
        {
            return (bool)ViewState["bStaged"];
        }
        set
        {
            ViewState["bStaged"] = value;
        }
    }
    #endregion

    #region Subs

    private void EmailMessage(string sFromWho, string sToWho, string sToWhoCC, string sSubject, string sBody)
    {

        if (sToWho != "")
        {
            string sResult = "";
            string sMailServer = ConfigurationManager.AppSettings["SMTP"].ToString();
            string sEmailUserName = ConfigurationManager.AppSettings["SMTPUserName"].ToString();
            string sEmailPassword = ConfigurationManager.AppSettings["SMTPPassword"].ToString();

            try
            {
                if (Server.MachineName.ToUpper() == "BMW" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
                {
                    sResult = MyEmail.SendEmailWithCredentials(sFromWho, sToWho, sToWhoCC, sSubject, sBody, sMailServer, sEmailUserName, sEmailPassword);
                    if (sResult == "Email Sent!")
                    {
                        lblErrorNotes.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblErrorNotes.ForeColor = System.Drawing.Color.Red;
                    }

                    lblErrorNotes.Text = sResult;
                }
                else//Felbro...
                {
                    sResult = MyEmail.SendEmailWithCredentials(sFromWho, sToWho, sToWhoCC, sSubject, sBody, sMailServer, sEmailUserName, sEmailPassword);
                    if (sResult == "Email Sent!")
                    {
                        lblErrorNotes.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblErrorNotes.ForeColor = System.Drawing.Color.Red;
                    }
                    lblErrorNotes.Text = sResult;
                }


            }
            catch (Exception)
            {
                lblErrorNotes.ForeColor = System.Drawing.Color.Red;
                lblErrorNotes.Text = sResult;

            }

            // Any existing page can be used for the response redirect method
        }
    }


    private void GetSummary(int iRoleID, int iUserID)
    {
        DataTable dt = new DataTable();
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        string sSQL = "";
        if (iRoleID != 4)//Everybody Else...
        {
            sSQL = "EXEC spGetSalesOrderTrackerSummary @RoleID=" + iRoleID.ToString();
        }
        else//Client...
        {
            sSQL = "EXEC spGetSalesOrderTrackerSummary @RoleID=" + iRoleID.ToString();
            sSQL += " ,@UserID =" + iUserID.ToString();
        }
        dt = SharedFunctions.getDataTable(sSQL, conn, "Summary");
        dt = PivotTable.GetInversedDataTable(dt, "MonthYear");
        gvSummary.DataSource = dt;
        gvSummary.DataBind();
        dt.Dispose();
    }
    private void BindDeliveries(string sSalesOrderID)
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = ((from dm in db.DelMaster
                        where dm.SalesOrder == sSalesOrderID
                        select new
                        {
                            dm.DeliveryID,
                            dm.Customer,
                            dm.SalesOrder,
                            dm.CustomerPoNumber,
                            dm.DeliveryTypeID,
                            dm.QtyScheduled,
                            dm.QtyActual,
                            dm.DateScheduled,
                            dm.DateDelivered,
                            dm.DateAdded,
                            dm.AddedBy,
                            dm.DateModified,
                            dm.ModifiedBy,
                            dm.DriverID,
                            Truck = dm.DelVehicles.VehDescription,
                            dm.DeliveryStatus,
                            dm.CheckNumber,
                            dm.Amount,
                            dm.IsCOD,
                            dm.Comments,
                            DeliveryReceiptFlag =
                              (from DocScanUploadHistory in db.DocScanUploadHistory
                               where
                                 DocScanUploadHistory.DocType == "Delivery Receipt" &&
                                 DocScanUploadHistory.DocID == Convert.ToString(dm.DeliveryID)
                               select new
                               {
                                   DocScanUploadHistory.DocID
                               }).Count(p => p.DocID != null)
                        }));

            dt = SharedFunctions.ToDataTable(db, qry);

            gvDeliveries.DataSource = dt;
            gvDeliveries.DataBind();

            dt.Dispose();
        }
    }
    private void UpdateSalesOrderAcknowledgementValue()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sFile = "";

            if (Server.MachineName.ToUpper() == "POWERHOUSE" || Server.MachineName.ToUpper() == "POWERHOUSEJR" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
            {
                sFile = "c:\\temp\\ADMMSQ.DAT";
            }
            else
            {
                sFile = "E:\\SYSPRO7\\WORK\\ADMMSQ.DAT";
            }

            try
            {

                using (StreamReader sr = File.OpenText(sFile))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        // Process line
                        string sSalesOrder = "";
                        if (line.Contains("order acknowledgement by"))
                        {
                            int iPositionOfU = 0;
                            int iPostionOfComma = 0;
                            iPositionOfU = line.IndexOf("U");
                            iPostionOfComma = line.IndexOf(",");
                            sSalesOrder = line.Substring(iPositionOfU + 1, iPostionOfComma - (iPositionOfU + 1));
                            // Debug.WriteLine(sSalesOrder);

                            var query = (from s in db.SorMaster
                                         where s.SalesOrder == sSalesOrder
                                         && (s.OrderAck != "Y" || s.OrderAck == null)
                                         select s);
                            foreach (var a in query)
                            {
                                SorMaster sm = db.SorMaster.Single(p => p.SalesOrder == a.SalesOrder);
                                sm.OrderAck = "Y";
                                sm.OrderAckDateTime = DateTime.Now;
                                db.SubmitChanges();
                            }

                        }

                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void SendScannedEmail(string sSalesOrder, string sOrderDate, string sCustomer, string sPO)
    {
        string sSubject = "Customer Pickup Required: " + sCustomer + "/PO#" + sPO;
        string sBody = "";
        string sMyEmail = "logistics@felbro.com";

        string sCustomerEmail = SharedFunctions.GetCustomerEmail(sCustomer);

        sBody += "<p style='font-family:arial;font-size:12pt;color:black;text-align:justify'>";
        sBody += "Sales Order#: " + int.Parse(sSalesOrder).ToString() + "<br/>";
        sBody += "<br/>";
        sBody += "Order Date: " + sOrderDate + "<br/>";
        sBody += "<br/>";
        sBody += "Customer: " + sCustomer + "<br/>";
        sBody += "<br/>";
        sBody += "P.O.#" + sPO + "<br/>";
        sBody += "<br/>";
        sBody += "Please contact Felbro Logistics to arrange a pickup date and time.<br/>";
        sBody += "<br/>";
        sBody += "logistics@felbro.com<br/>";
        sBody += "<br/>";
        sBody += "</p>";
        //Send out Email...
        if (Server.MachineName.ToUpper() == "POWERHOUSE" || Server.MachineName.ToUpper() == "POWERHOUSEJR" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
        {
            EmailMessage(sMyEmail, "robert@robertsoftdev.com", "", sSubject, sBody);
        }
        else
        {
            EmailMessage(sMyEmail, sCustomerEmail, "orders@felbro.com", sSubject, sBody);
        }
        lblError.Text = "**EMAIL SENT!!";
        lblError.ForeColor = Color.Green;
        lblError0.Text = "**EMAIL SENT!!";
        lblError0.ForeColor = Color.Green;
    }
    private void SendCreditHoldEmail(string sSalesOrder, string sOrderDate, string sCustomer, string sPO)
    {
        string sSubject = "Credit Hold for: " + sCustomer + "/PO#" + sPO;
        string sBody = "";
        string sMyEmail = "logistics@felbro.com";
        string sFirstOperatorEmail = SharedFunctions.GetEmailOfFirstOperator(sSalesOrder);

        sBody += "<p style='font-family:arial;font-size:12pt;color:black;text-align:justify'>";
        sBody += "Sales Order#: " + int.Parse(sSalesOrder).ToString() + "<br/>";
        sBody += "<br/>";
        sBody += "Order Date: " + sOrderDate + "<br/>";
        sBody += "<br/>";
        sBody += "Customer: " + sCustomer + "<br/>";
        sBody += "<br/>";
        sBody += "P.O.#" + sPO + "<br/>";
        sBody += "<br/>";
        sBody += "Please contact Felbro Logistics to arrange a pickup date and time.<br/>";
        sBody += "<br/>";
        sBody += "logistics@felbro.com<br/>";
        sBody += "<br/>";
        sBody += "</p>";
        //Send out Email...
        if (Server.MachineName.ToUpper() == "POWERHOUSE" || Server.MachineName.ToUpper() == "POWERHOUSEJR" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
        {
            EmailMessage(sMyEmail, "robert@robertsoftdev.com", "", sSubject, sBody);
        }
        else
        {
            EmailMessage(sMyEmail, "logistics@felbro.com", sFirstOperatorEmail, sSubject, sBody);
        }
        lblError.Text = "**EMAIL SENT!!";
        lblError.ForeColor = Color.Green;
        lblError0.Text = "**EMAIL SENT!!";
        lblError0.ForeColor = Color.Green;
    }
    private void GetComments(string sSalesOrder)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = new DataTable();
            var query1 = (from ln in db.SorComments
                          join u in db.WipUsers on ln.AddedBy equals u.UserID
                          where Convert.ToInt32(ln.SalesOrder) == Convert.ToInt32(sSalesOrder)
                          orderby ln.DateAdded descending
                          select new
                          {
                              ln.SOCommentID,
                              ln.Comment,
                              ln.SalesOrder,
                              Name = (u.FirstName + " " + (u.MiddleName ?? "") + " " + u.LastName).Replace("  ", " "),
                              ln.DateAdded,
                              ln.Dept,

                          });

            dt = SharedFunctions.ToDataTable(db, query1);
            if (dt.Rows.Count > 0)
            {
                gvNotes.DataSource = dt;
                gvNotes.DataBind();
                Session["dtNotes"] = dt;
            }
            else
            {
                gvNotes.DataSource = null;
                gvNotes.DataBind();
            }

        }
    }
    private void AddComments()
    {
        string sCustomer = lblCustomerName.Text;
        string sCustomerNumber = lblCustomerNumber.Text.Trim();
        string sSalesPersonUserID = lblSalespersonUserID.Text.Trim();
        int iWipUserID = GetSalespersonWipUserID(sSalesPersonUserID);
        string sComment = txtSaleOrderComment.Text.Trim();
        if (sComment == "")
        {
            lblErrorNotes.Text = "**No Comment Entered!!";
            lblErrorNotes.ForeColor = Color.Red;
            return;
        }
        if (!chkCCtoCustomerService.Checked && !chkCCtoLogistics.Checked && !chkCCtoOperations.Checked && !chkCCtoProduction.Checked && !chkCCtoQualityControl.Checked && !chkCCtoAccountsReceivable.Checked && !chkCCtoTransfer.Checked)
        {
            lblErrorNotes.Text = "**You MUST selected at least one destination for your comment to be sent!!";
            lblErrorNotes.ForeColor = Color.Red;
            return;
        }

        int iUserID = 0;
        int iRoleID = 0;

        iUserID = Convert.ToInt32(Session["UserID"]);
        iRoleID = Convert.ToInt32(Session["RoleID"]);

        string sSalesOrder = "";
        sSalesOrder = lblSaleOrderDetailsHidden.Text;
        string sDept = SharedFunctions.GetDept(iUserID);
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            if (txtSaleOrderComment.Text.Trim() != "")
            {
                SorComments sc = new SorComments();
                sc.SalesOrder = sSalesOrder;
                sc.Comment = sComment.ToUpper();
                sc.AddedBy = iUserID;
                if (sDept != "")
                {
                    sc.Dept = sDept;
                }
                else
                {
                    sc.Dept = "P";
                }

                sc.DateAdded = DateTime.Now;
                db.SorComments.InsertOnSubmit(sc);
                db.SubmitChanges();
            }
        }
        // RunSearch();  Cant refresh SOT to show last note added, it takes too long...


        txtSaleOrderComment.Text = "";
        GetComments(sSalesOrder);
        if (sComment != "")
        {
            string sSubject = "Updated for " + sCustomer + " SO#" + int.Parse(sSalesOrder).ToString();
            string sBody = "";
            string sFirstOperatorEmail = SharedFunctions.GetEmailOfFirstOperator(sSalesOrder);
            string sAccountOwnerToEmail = SharedFunctions.GetUserEmail(iWipUserID);//House is blank so use new logic if blank 2-20-2019...        
            string sMyEmail = "DoNotReply@felbro.com";
            string sCC = "";
            string sCommentsBy = SharedFunctions.GetUserFullName(iUserID);
            sBody += "<p style='font-family:arial;font-size:12pt;color:black;text-align:justify'>";
            sBody += "Greetings,<br>";
            sBody += "<br>";
            sBody += "Sales Order# " + int.Parse(sSalesOrder).ToString() + "<br>";
            sBody += "<br>";
            sBody += "Customer: " + sCustomer + " - #" + sCustomerNumber + "<br>";
            sBody += "<br>";
            sBody += sComment;
            sBody += "";
            sBody += "<br>";
            sBody += "<br>";
            sBody += "Left by " + sCommentsBy + "<br><br>";
            sBody += "Left or updated on " + DateTime.Now.ToShortDateString();
            sBody += "<br>";
            sBody += "</p>";
            sBody += "<br/><br/><p style='font-family:arial;font-size:9pt'> CONFIDENTIALITY NOTICE: This E-mail contains confidential information intended only for the individual or entity named within the message. If the reader of this message is not the intended recipient, or the agent responsible to deliver it to the intended recipient, you are hereby notified that any review, dissemination or copying of this communication is prohibited. If this communication was received in error, please notify us by reply E-mail and delete the original message.</p><br/>";


            int iSalesOrder = Convert.ToInt32(sSalesOrder);
            List<string> lCCs = new List<string>();
            List<string> lGroupMembers = new List<string>();
            switch (sSalesPersonUserID.ToUpper())
            {
                case "1"://House WipUserID...
                    if (chkCCtoOperations.Checked)
                    {
                        lGroupMembers = SharedFunctions.GetMessageGroupEmails(1, 0);//Operations...
                        foreach (string sEmail in lGroupMembers)
                        {
                            lCCs.Add(sEmail);
                        }
                    }
                    if (chkCCtoLogistics.Checked)
                    {
                        lGroupMembers = SharedFunctions.GetMessageGroupEmails(2, 0);//Logistics...
                        foreach (string sEmail in lGroupMembers)
                        {
                            lCCs.Add(sEmail);
                        }
                    }
                    if (chkCCtoQualityControl.Checked)
                    {
                        lGroupMembers = SharedFunctions.GetMessageGroupEmails(5, 0);//QualityControl...
                        foreach (string sEmail in lGroupMembers)
                        {
                            lCCs.Add(sEmail);
                        }
                    }
                    if (chkCCtoProduction.Checked)
                    {
                        switch (iSalesOrder % 2)
                        {
                            case 0://Even...
                                lGroupMembers = SharedFunctions.GetMessageGroupEmails(3, 1);//Production even orders...
                                foreach (string sEmail in lGroupMembers)
                                {
                                    lCCs.Add(sEmail);
                                }
                                //Debug.WriteLine("even number");
                                break;
                            case 1://Odd...
                                lGroupMembers = SharedFunctions.GetMessageGroupEmails(3, 2);//Production odd orders...
                                foreach (string sEmail in lGroupMembers)
                                {
                                    lCCs.Add(sEmail);
                                }
                                //Debug.WriteLine("odd number");
                                break;
                        }
                        lGroupMembers = SharedFunctions.GetMessageGroupEmails(3, 0);//Production All...
                        foreach (string sEmail in lGroupMembers)
                        {
                            lCCs.Add(sEmail);
                        }
                    }
                    if (chkCCtoCustomerService.Checked)//Customer Service...
                    {
                        lGroupMembers = SharedFunctions.GetMessageGroupEmails(4, 0);//Customer Service...
                        foreach (string sEmail in lGroupMembers)
                        {
                            lCCs.Add(sEmail);
                        }
                    }
                    if (chkCCtoAccountsReceivable.Checked)//Accounts Receivable...
                    {
                        lGroupMembers = SharedFunctions.GetMessageGroupEmails(12, 0);//Accounts Receivable...
                        foreach (string sEmail in lGroupMembers)
                        {
                            lCCs.Add(sEmail);
                        }
                    }
                    if (chkCCtoTransfer.Checked)//Transfer...
                    {
                        lGroupMembers = SharedFunctions.GetMessageGroupEmails(13, 0);//Transfer...
                        foreach (string sEmail in lGroupMembers)
                        {
                            lCCs.Add(sEmail);
                        }
                    }
                    break;
                default://Sales Person other than house...
                    if (chkCCtoOperations.Checked)
                    {
                        if (String.IsNullOrEmpty(sAccountOwnerToEmail) == false)
                        {
                            lCCs.Add(sAccountOwnerToEmail);
                            lGroupMembers = SharedFunctions.GetMessageGroupEmails(1, 0);//Operations...
                            foreach (string sEmail in lGroupMembers)
                            {
                                lCCs.Add(sEmail);
                            }
                        }
                        else
                        {
                            lGroupMembers = SharedFunctions.GetMessageGroupEmails(1, 0);//Operations...
                            foreach (string sEmail in lGroupMembers)
                            {
                                lCCs.Add(sEmail);
                            }
                        }
                    }
                    if (chkCCtoLogistics.Checked)
                    {
                        if (String.IsNullOrEmpty(sAccountOwnerToEmail) == false)
                        {
                            lCCs.Add(sAccountOwnerToEmail);
                            lGroupMembers = SharedFunctions.GetMessageGroupEmails(2, 0);//Logistics...
                            foreach (string sEmail in lGroupMembers)
                            {
                                lCCs.Add(sEmail);
                            }
                        }
                        else
                        {
                            lGroupMembers = SharedFunctions.GetMessageGroupEmails(2, 0);//Logistics...
                            foreach (string sEmail in lGroupMembers)
                            {
                                lCCs.Add(sEmail);
                            }
                        }
                    }
                    if (chkCCtoQualityControl.Checked)
                    {
                        if (String.IsNullOrEmpty(sAccountOwnerToEmail) == false)
                        {
                            lCCs.Add(sAccountOwnerToEmail);
                            lGroupMembers = SharedFunctions.GetMessageGroupEmails(5, 0);//QualityControl...
                            foreach (string sEmail in lGroupMembers)
                            {
                                lCCs.Add(sEmail);
                            }
                        }
                        else
                        {
                            lGroupMembers = SharedFunctions.GetMessageGroupEmails(5, 0);//QualityControl...
                            foreach (string sEmail in lGroupMembers)
                            {
                                lCCs.Add(sEmail);
                            }
                        }
                    }
                    if (chkCCtoProduction.Checked)
                    {
                        switch (iSalesOrder % 2)
                        {
                            case 0://Even...
                                if (String.IsNullOrEmpty(sAccountOwnerToEmail) == false)
                                {
                                    lCCs.Add(sAccountOwnerToEmail);
                                    lGroupMembers = SharedFunctions.GetMessageGroupEmails(3, 1);//Production even orders...
                                    foreach (string sEmail in lGroupMembers)
                                    {
                                        lCCs.Add(sEmail);
                                    }
                                }
                                else
                                {
                                    lGroupMembers = SharedFunctions.GetMessageGroupEmails(3, 1);//Production even orders...
                                    foreach (string sEmail in lGroupMembers)
                                    {
                                        lCCs.Add(sEmail);
                                    }
                                }
                                //Debug.WriteLine("even number");
                                break;
                            case 1://Odd...
                                if (String.IsNullOrEmpty(sAccountOwnerToEmail) == false)
                                {
                                    lCCs.Add(sAccountOwnerToEmail);
                                    lGroupMembers = SharedFunctions.GetMessageGroupEmails(3, 2);//Production odd orders...
                                    foreach (string sEmail in lGroupMembers)
                                    {
                                        lCCs.Add(sEmail);
                                    }
                                }
                                else
                                {
                                    lGroupMembers = SharedFunctions.GetMessageGroupEmails(3, 2);//Production odd orders...
                                    foreach (string sEmail in lGroupMembers)
                                    {
                                        lCCs.Add(sEmail);
                                    }
                                }
                                //Debug.WriteLine("odd number");
                                break;
                        }
                        lGroupMembers = SharedFunctions.GetMessageGroupEmails(3, 0);//Production All...
                        foreach (string sEmail in lGroupMembers)
                        {
                            lCCs.Add(sEmail);
                        }
                    }
                    if (chkCCtoCustomerService.Checked)//Customer Service...
                    {
                        if (String.IsNullOrEmpty(sAccountOwnerToEmail) == false)
                        {
                            lCCs.Add(sAccountOwnerToEmail);
                            lGroupMembers = SharedFunctions.GetMessageGroupEmails(4, 0);//Customer Service...
                            foreach (string sEmail in lGroupMembers)
                            {
                                lCCs.Add(sEmail);
                            }
                        }
                        else
                        {
                            lGroupMembers = SharedFunctions.GetMessageGroupEmails(4, 0);//Customer Service...
                            foreach (string sEmail in lGroupMembers)
                            {
                                lCCs.Add(sEmail);
                            }
                        }
                    }

                    if (chkCCtoAccountsReceivable.Checked)//Accounts Receivable...
                    {
                        if (String.IsNullOrEmpty(sAccountOwnerToEmail) == false)
                        {
                            lCCs.Add(sAccountOwnerToEmail);
                            lGroupMembers = SharedFunctions.GetMessageGroupEmails(12, 0);//Accounts Receivable...
                            foreach (string sEmail in lGroupMembers)
                            {
                                lCCs.Add(sEmail);
                            }
                        }
                        else
                        {
                            lGroupMembers = SharedFunctions.GetMessageGroupEmails(12, 0);//Accounts Receivable...
                            foreach (string sEmail in lGroupMembers)
                            {
                                lCCs.Add(sEmail);
                            }
                        }
                    }
                    if (chkCCtoTransfer.Checked)//Transfer...
                    {
                        if (String.IsNullOrEmpty(sAccountOwnerToEmail) == false)
                        {
                            lCCs.Add(sAccountOwnerToEmail);
                            lGroupMembers = SharedFunctions.GetMessageGroupEmails(13, 0);//Transfer...
                            foreach (string sEmail in lGroupMembers)
                            {
                                lCCs.Add(sEmail);
                            }
                        }
                        else
                        {
                            lGroupMembers = SharedFunctions.GetMessageGroupEmails(13, 0);//Transfer...
                            foreach (string sEmail in lGroupMembers)
                            {
                                lCCs.Add(sEmail);
                            }
                        }
                    }
                    break;
            }
            lCCs = SharedFunctions.removeDuplicates(lCCs);
            foreach (string CC in lCCs)
            {
                sCC += CC + ",";
            }

            int iLastCommaInStringIdx = 0;
            if (sCC.Trim().EndsWith(","))
            {
                iLastCommaInStringIdx = sCC.LastIndexOf(",");
                sCC = sCC.Remove(iLastCommaInStringIdx).Trim();
            }

            //Debug.WriteLine(sCC);
            if (sCC != "")
            {
                //Send out Email...
                if (Server.MachineName.ToUpper() == "BMW" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
                {
                    EmailMessage(sMyEmail, "robert@robertsoftdev.com", "", sSubject, sBody);
                }
                else
                {
                    EmailMessage(sMyEmail, sMyEmail, sCC, sSubject, sBody);
                }
                lCCs.Clear();
            }

        }
    }
    private void LoadPlacedOrdersGrid(int iRoleID, int iUserID)
    {
        string sMsg = "";
        string sStartDate = "";
        if (txtStartDate.Text.Trim() != "")
        {//Not null then add quotes...
            sStartDate = "'" + txtStartDate.Text.Trim() + "'";
        }
        string sEndDate = "";
        if (txtEndDate.Text.Trim() != "")
        {//Not null then add quotes...
            sEndDate = "'" + txtEndDate.Text.Trim() + "'";
        }



        DataSet ds = new DataSet();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        //Validation...
        if (sStartDate == "")
        {
            sMsg += "**Start Date is Required to run Placed Orders Reports!<br/>";
        }
        if (sEndDate == "")
        {
            sMsg += "**End Date is Required to run Placed Orders Reports!<br/>";
        }
        if (SharedFunctions.IsDate(sStartDate) == true && SharedFunctions.IsDate(sEndDate) == true)
        {
            if (Convert.ToDateTime(sStartDate.Replace("'", "")) > Convert.ToDateTime(sEndDate.Replace("'", "")))
            {
                sMsg += "**End Date can not come before Start Date!<br/>";
            }
        }

        if (sMsg.Length > 0)
        {
            lblOrdersPlacedError.Text = sMsg;
            lblOrdersPlacedError.ForeColor = Color.Red;
            ModalPopupExtenderPlacedOrders.Show();
            return;
        }

        sSQL = "EXEC spGetSalesOrderTrackerSummaryPlacedOrdersNew ";
        sSQL += "@FromDate=" + sStartDate + ",";
        sSQL += "@ToDate=" + sEndDate + ",";
        sSQL += "@RoleID = " + iRoleID.ToString() + ",";
        sSQL += "@UserID =" + iUserID.ToString();


        Debug.WriteLine(sSQL);

        ds = SharedFunctions.getDataSet(sSQL, conn, "dsPlacedOrders");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[3].Rows.Count > 0)
            {
                Session["dtPlacedOrders"] = ds.Tables[3];
                gvPlacedOrders.DataSource = ds.Tables[3];
                gvPlacedOrders.DataBind();
            }
            else
            {
                lblOrdersPlacedError.Text = "No results found!!";
                lblOrdersPlacedError.ForeColor = Color.Red;
                gvPlacedOrders.DataSource = null;
                gvPlacedOrders.DataBind();
            }
        }
    }
    private void ExportPlacedOrders(int iRoleID, int iUserID)
    {
        string sMsg = "";
        string sStartDate = "";
        if (txtStartDate.Text.Trim() != "")
        {//Not null then add quotes...
            sStartDate = "'" + txtStartDate.Text.Trim() + "'";
        }
        string sEndDate = "";
        if (txtEndDate.Text.Trim() != "")
        {//Not null then add quotes...
            sEndDate = "'" + txtEndDate.Text.Trim() + "'";
        }

        DataSet ds = new DataSet();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        //Validation...
        if (sStartDate == "")
        {
            sMsg += "**Start Date is Required to run Placed Orders Reports!<br/>";
        }
        if (sEndDate == "")
        {
            sMsg += "**End Date is Required to run Placed Orders Reports!<br/>";
        }
        if (SharedFunctions.IsDate(sStartDate) == true && SharedFunctions.IsDate(sEndDate) == true)
        {
            if (Convert.ToDateTime(sStartDate.Replace("'", "")) > Convert.ToDateTime(sEndDate.Replace("'", "")))
            {
                sMsg += "**End Date can not come before Start Date!<br/>";
            }
        }

        if (sMsg.Length > 0)
        {
            lblOrdersPlacedError.Text = sMsg;
            lblOrdersPlacedError.ForeColor = Color.Red;
            ModalPopupExtenderPlacedOrders.Show();
            return;
        }

        sSQL = "EXEC spGetSalesOrderTrackerSummaryPlacedOrdersNew ";
        sSQL += "@FromDate=" + sStartDate + ",";
        sSQL += "@ToDate=" + sEndDate + ",";
        sSQL += "@RoleID = " + iRoleID.ToString() + ",";
        sSQL += "@UserID =" + iUserID.ToString();


        Debug.WriteLine(sSQL);

        ds = SharedFunctions.getDataSet(sSQL, conn, "dsPlacedOrders");
        DataTable dt = new DataTable();
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[3].Rows.Count > 0)
            {
                dt = ds.Tables[3];
                dt.TableName = "dtPlacedOrders";

                var query = (from d in dt.AsEnumerable()
                             select new
                             {
                                 Date = d["MonthDayYear"].ToString(),
                                 TotalValueOriginal = d["TotalValue"].ToString(),
                                 TotalValueCurrent = d["TotalValueUnchanged"].ToString(),
                                 TotalValueDiff = d["TotalValueDiff"].ToString(),
                                 LineCountOriginal = d["LineCount"].ToString(),
                                 LineCountCurrent = d["LineCountUnchanged"].ToString(),
                                 LineCountDiff = d["LineCountDiff"].ToString(),
                                 OrderCountOriginal = d["OrderCount"].ToString(),
                                 OrderCountCurrent = d["OrderCountUnchanged"].ToString(),
                                 OrderCountDiff = d["OrderCountDiff"].ToString(),

                             });
                dt = SharedFunctions.LINQToDataTable(query);

                string sFileName = "SalesOrderTackerPlacedOrdersReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                ExcelHelper.ToExcel(dt, sFileName, Page.Response);
            }
            else
            {
                lblOrdersPlacedError.Text = "No results found!!";
                lblOrdersPlacedError.ForeColor = Color.Red;
            }
        }


        //send session variable dtReport to Excel...
        dt.Dispose();
        ds.Dispose();

    }
    private void LoadPlaceOrderData(int iRoleID, int iUserID)
    {

        string sStartDate = "'01/01/2000'";
        string sEndDate = "'01/31/2000'";

        DataSet ds = new DataSet();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);


        sSQL = "EXEC spGetSalesOrderTrackerSummaryPlacedOrdersNew ";
        sSQL += "@FromDate=" + sStartDate + ",";
        sSQL += "@ToDate=" + sEndDate + ",";
        sSQL += "@RoleID = " + iRoleID.ToString() + ",";
        sSQL += "@UserID =" + iUserID.ToString();


        Debug.WriteLine(sSQL);

        ds = SharedFunctions.getDataSet(sSQL, conn, "dsPlacedOrders");
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0] != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {//Today...
                    string sTodaysOrderCount = "";
                    string sTodaysLineCount = "";
                    string sTodaysOrdersAmount = "";
                    sTodaysOrderCount = ds.Tables[0].Rows[0]["OrderCount"].ToString();
                    sTodaysLineCount = ds.Tables[0].Rows[0]["LineCount"].ToString();
                    sTodaysOrdersAmount = ds.Tables[0].Rows[0]["TotalValue"].ToString();

                    lblOrderAmountToday.Text = sTodaysOrdersAmount;
                    lblOrderCountToday.Text = sTodaysOrderCount;
                    lblLineCountToday.Text = sTodaysLineCount;
                }
                else
                {
                    lblOrderAmountToday.Text = "--";
                    lblOrderCountToday.Text = "--";
                    lblLineCountToday.Text = "--";
                }
            }
            if (ds.Tables[1] != null)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {//Yesterday...
                    string sYesterdaysOrderCount = "";
                    string sYesterdaysLineCount = "";
                    string sYesterdaysOrderAmount = "";
                    sYesterdaysOrderCount = ds.Tables[1].Rows[0]["OrderCount"].ToString();
                    sYesterdaysLineCount = ds.Tables[1].Rows[0]["LineCount"].ToString();
                    sYesterdaysOrderAmount = ds.Tables[1].Rows[0]["TotalValue"].ToString();

                    lblOrderAmountYesterday.Text = sYesterdaysOrderAmount;
                    lblOrderCountYesterday.Text = sYesterdaysOrderCount;
                    lblLineCountYesterday.Text = sYesterdaysLineCount;
                }
                else
                {
                    lblOrderAmountYesterday.Text = "--";
                    lblOrderCountYesterday.Text = "--";
                    lblLineCountYesterday.Text = "--";
                }
            }
            if (ds.Tables[2] != null)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {//This Month...
                    string sThisMonthsOrderCount = "";
                    string sThisMonthsLineCount = "";
                    string sThisMonthsOrderAmount = "";
                    sThisMonthsOrderCount = ds.Tables[2].Rows[0]["OrderCount"].ToString();
                    sThisMonthsLineCount = ds.Tables[2].Rows[0]["LineCount"].ToString();
                    sThisMonthsOrderAmount = ds.Tables[2].Rows[0]["TotalValue"].ToString();

                    lblOrderAmountThisMonth.Text = sThisMonthsOrderAmount;
                    lblOrderCountThisMonth.Text = sThisMonthsOrderCount;
                    lblLineCountThisMonth.Text = sThisMonthsLineCount;

                }
                else
                {
                    lblOrderAmountThisMonth.Text = "--";
                    lblOrderCountThisMonth.Text = "--";
                    lblLineCountThisMonth.Text = "--";

                }
            }
        }
    }
    private void ExportPlacedOrdersDetails()
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);

        lblOrdersPlacedError.Text = "";
        string sMsg = "";
        string sStartDate = "";
        if (txtStartDate.Text.Trim() != "")
        {//Not null then add quotes...
            sStartDate = "'" + txtStartDate.Text.Trim() + "'";
        }
        string sEndDate = "";
        if (txtEndDate.Text.Trim() != "")
        {//Not null then add quotes...
            sEndDate = "'" + txtEndDate.Text.Trim() + "'";
        }

        DataTable dt = new DataTable();
        string sFileName = "";
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        //Validation...
        if (sStartDate == "")
        {
            sMsg += "**Start Date is Required to run Placed Orders Reports!<br/>";
        }
        if (sEndDate == "")
        {
            sMsg += "**End Date is Required to run Placed Orders Reports!<br/>";
        }
        if (SharedFunctions.IsDate(sStartDate) == true && SharedFunctions.IsDate(sEndDate) == true)
        {
            if (Convert.ToDateTime(sStartDate.Replace("'", "")) > Convert.ToDateTime(sEndDate.Replace("'", "")))
            {
                sMsg += "**End Date can not come before Start Date!<br/>";
            }
        }

        if (sMsg.Length > 0)
        {
            lblOrdersPlacedError.Text = sMsg;
            lblOrdersPlacedError.ForeColor = Color.Red;
            ModalPopupExtenderPlacedOrders.Show();
            return;
        }

        sSQL = "EXEC spGetSalesOrderTrackerSummaryPlacedOrdersDetailsNew ";
        sSQL += "@FromDate=" + sStartDate + ",";
        sSQL += "@ToDate=" + sEndDate + ",";
        sSQL += "@RoleID = " + iRoleID.ToString() + ",";
        sSQL += "@UserID =" + iUserID.ToString();


        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dtPlacedOrdersDetails");
        foreach (DataRow row in dt.Rows)
        {//To get a numeric value with no leading zeros...
            string sSource = row["SalesOrder"].ToString();
            var sResult = int.Parse(sSource).ToString();
            row["SalesOrder"] = sResult;
        }
        dt.AcceptChanges();
        dt.TableName = "dtPlacedOrdersDetails";
        var query = (from d in dt.AsEnumerable()
                     select new
                     {
                         Date = d["MonthDayYear"].ToString(),
                         SalesOrder = d["SalesOrder"].ToString(),
                         Customer = d["Customer"].ToString(),
                         Name = d["Name"].ToString(),
                         TotalValueOriginal = d["TotalValue"].ToString(),
                         TotalValueCurrent = d["TotalValueUnchanged"].ToString(),
                         TotalValueDiff = d["TotalValueDiff"].ToString(),
                         LineCountOriginal = d["LineCount"].ToString(),
                         LineCountCurrent = d["LineCountUnchanged"].ToString(),
                         LineCountDiff = d["LineCountDiff"].ToString(),
                         Note = d["PlacedOrdersNote"].ToString(),
                     });
        dt = SharedFunctions.LINQToDataTable(query);

        sFileName = "SalesOrderTrackerPlacedOrdersDetailsReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExcelHelper.ToExcel(dt, sFileName, Page.Response);

        //send session variable dtReport to Excel...

        dt.Dispose();
    }
    private void LoadPlacedOrdersDetails()
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);

        lblOrdersPlacedError.Text = "";
        string sMsg = "";
        string sStartDate = "";
        if (txtStartDate.Text.Trim() != "")
        {//Not null then add quotes...
            sStartDate = "'" + txtStartDate.Text.Trim() + "'";
        }
        string sEndDate = "";
        if (txtEndDate.Text.Trim() != "")
        {//Not null then add quotes...
            sEndDate = "'" + txtEndDate.Text.Trim() + "'";
        }

        DataTable dt = new DataTable();
        string sFileName = "";
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        //Validation...
        if (sStartDate == "")
        {
            sMsg += "**Start Date is Required to run Placed Orders Reports!<br/>";
        }
        if (sEndDate == "")
        {
            sMsg += "**End Date is Required to run Placed Orders Reports!<br/>";
        }
        if (SharedFunctions.IsDate(sStartDate) == true && SharedFunctions.IsDate(sEndDate) == true)
        {
            if (Convert.ToDateTime(sStartDate.Replace("'", "")) > Convert.ToDateTime(sEndDate.Replace("'", "")))
            {
                sMsg += "**End Date can not come before Start Date!<br/>";
            }
        }

        if (sMsg.Length > 0)
        {
            lblOrdersPlacedError.Text = sMsg;
            lblOrdersPlacedError.ForeColor = Color.Red;
            ModalPopupExtenderPlacedOrders.Show();
            return;
        }

        sSQL = "EXEC spGetSalesOrderTrackerSummaryPlacedOrdersDetailsNew ";
        sSQL += "@FromDate=" + sStartDate + ",";
        sSQL += "@ToDate=" + sEndDate + ",";
        sSQL += "@RoleID = " + iRoleID.ToString() + ",";
        sSQL += "@UserID =" + iUserID.ToString();


        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dtPlacedOrdersDetails");
        if (dt.Rows.Count > 0)
        {
            Session["dtPlacedOrdersDetails"] = dt;
            gvPlacedOrderDetails.DataSource = dt;
            gvPlacedOrderDetails.DataBind();
        }
        else
        {
            lblOrdersPlacedError.Text = "No results found!!";
            lblOrdersPlacedError.ForeColor = Color.Red;
            gvPlacedOrderDetails.DataSource = null;
            gvPlacedOrderDetails.DataBind();
        }


        dt.Dispose();
    }
    private void ExportTopFifteenList()
    {
        DataTable dt = LoadTopFifteenData(rblTopTen.SelectedValue);

        string sLastYearMTD = DateTime.Now.AddYears(-1).ToShortDateString();

        DateTime dtFirstDayOfCurrentYear = new DateTime(DateTime.Now.Year, 1, 1);
        string sFirstDayOfCurrentYear = dtFirstDayOfCurrentYear.ToShortDateString();
        DateTime dtFirstDayOfLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);
        string sFirstDayOfLastYear = dtFirstDayOfLastYear.ToShortDateString();

        DateTime dtFirstDayOfLastYearMinusOne = new DateTime(DateTime.Now.AddYears(-2).Year, 1, 1);
        string sFirstDayOfLastYearMinusOne = dtFirstDayOfLastYearMinusOne.ToShortDateString();


        string sYTD = DateTime.Now.ToShortDateString();
        string sLYTD = DateTime.Now.AddDays(-365).ToShortDateString();
        string sLLYTD = DateTime.Now.AddDays(-730).ToShortDateString();
        string sMTD = DateTime.Now.ToShortDateString();
        DateTime dtFirstDayOfMonthCurrentYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        string sFirstDayOfMonthCurrentYear = dtFirstDayOfMonthCurrentYear.ToShortDateString();

        DateTime dtFirstDayOfLastMonthCurrentYear = dtFirstDayOfMonthCurrentYear.AddMonths(-1);//First Day of Last Month Period!
        string sFirstDayOfLastMonthCurrentYear = dtFirstDayOfLastMonthCurrentYear.ToShortDateString();

        //New 1-8-2020...
        DateTime dtFirstDayOfLastMonth = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1);
        string sFirstDayOfLastMonth = dtFirstDayOfLastMonth.ToShortDateString();
        DateTime dtLastDayOfLastMonth = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month));
        string sLastDayOfLastMonth = dtLastDayOfLastMonth.ToShortDateString();

        DateTime dtLastDayOfLastMonthCurrentYear = new DateTime(dtFirstDayOfMonthCurrentYear.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month));//Last Day of Last Month period!
        string sLastDayOfLastMonthCurrentYear = dtLastDayOfLastMonthCurrentYear.ToShortDateString();

        DateTime dtFirstDayOfMonthLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1).Month, 1);
        string sFirstDayOfMonthLastYear = dtFirstDayOfMonthLastYear.ToShortDateString();


        DateTime dtFirstDayOfLastMonthLastYear = new DateTime(DateTime.Now.AddMonths(-1).AddYears(-1).Year, DateTime.Now.AddMonths(-1).AddYears(-1).Month, 1);
        string sFirstDayOfLastMonthLastYear = dtFirstDayOfLastMonthLastYear.ToShortDateString();


        DateTime dtLastDayOfMonthCurrentYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        string sLastDayOfMonthCurrentYear = dtLastDayOfMonthCurrentYear.ToShortDateString();

        DateTime dtLastDayOfMonthLastYear = dtLastDayOfMonthCurrentYear.AddYears(-1);
        string sLastDayOfMonthLastYear = dtLastDayOfMonthLastYear.ToShortDateString();

        //Fixed 1-8-2020...


        int iYear = DateTime.Now.Year;
        int iLastMonthsLastYear = DateTime.Now.AddMonths(-1).AddYears(-1).Year;
        int iLastYearsLastMonth = DateTime.Now.AddMonths(-1).Month;
        int iYear1 = DateTime.Now.AddYears(-1).Year;
        int iYear2 = DateTime.Now.AddYears(-2).Year;
        int iMonth1 = DateTime.Now.AddMonths(-1).Month;
        int iLastYearLastMonthDay = DateTime.DaysInMonth(iYear1, iMonth1);

        DateTime dtLastDayOfLastMonthLastYear = new DateTime(iLastMonthsLastYear, iLastYearsLastMonth, iLastYearLastMonthDay);


        string sLastDayOfLastMonthLastYear = dtLastDayOfLastMonthLastYear.ToShortDateString();

        string sLastDayOfLastMonthLastYearMinusOne = dtLastDayOfLastMonthLastYear.AddYears(-1).ToShortDateString();
        DateTime dtLastDayOfCurrentYear = new DateTime(iYear, 12, 31);

        //New 2020...
        DateTime dtLastDayofCurrentYear = dtLastDayOfCurrentYear;
        string sLastDayofCurrentYear = dtLastDayofCurrentYear.ToShortDateString();

        DateTime dtLastDayOfLastYear = dtLastDayOfCurrentYear.AddYears(-1);
        string sLastDayOfLastYear = dtLastDayOfLastYear.ToShortDateString();

        DateTime dtLastDayOfLastYearMinusOne = dtLastDayOfCurrentYear.AddYears(-2);
        string sLastDayOfLastYearMinusOne = dtLastDayOfLastYearMinusOne.ToShortDateString();

        var query1 = (from dtSum in dt.AsEnumerable()
                      select new
                      {
                          Name = dtSum["Name"],

                          LAST_MONTH = dtSum["PreviousMonthAmountD"],
                          LAST_MONTH_LAST_YEAR = dtSum["LastYearPreviousMonthAmountD"],

                          MTD = dtSum["CurrentMTDAmountD"],
                          LAST_YEAR_MTD = dtSum["LastYearCurrentMTDAmountD"],
                          CURRENT_MONTH = dtSum["CurrentMonthAmountD"],
                          LAST_YEAR_CURRENT_MONTH = dtSum["LastYearCurrentMonthAmountD"],

                          YTD = dtSum["CurrentYTDAmountD"],
                          LYTD = dtSum["LastYTDAmountD"],
                          LYTD_MINUS_ONE = dtSum["LastYTDMinusOneAmountD"],
                          DIFF = dtSum["YearToYearDiffPercentage"],
                          DIFF_MINUS_ONE = dtSum["YearToYearDiffPercentageMinusOne"],

                          YTD_MARGIN = dtSum["CurrentYTDMargin"],
                          LYTD_MARGIN = dtSum["LastYTDMargin"],

                          CURRENT_MONTH_MARGIN = dtSum["CurrentMonthMargin"],
                          LAST_MONTH_MARGIN = dtSum["LastMonthMargin"],

                          PERCENTAGE_OF_TOTAL_YTD = dtSum["CurrentYTDAmountPercentageOfTotal"],
                          PERCENTAGE_OF_TOTAL_LYTD = dtSum["LastYTDAmountPercentageOfTotal"],

                          YTD_END_OF_MONTH = dtSum["CurrentYTDEndOfMonthAmountD"],
                          LYTD_END_OF_MONTH = dtSum["LastYTDEndOfMonthAmountD"],
                          LYTD_MINUS_ONE_END_OF_MONTH = dtSum["LastYTDMinusOneYearEndOfMonthAmountD"],
                          DIFF_END_OF_MONTH = dtSum["YearToYearDiffPercentageEndOfMonth"],
                          DIFF_END_OF_MONTH_MINUS_ONE = dtSum["YearToYearDiffPercentageEndOfMonthMinusOne"],

                          YTD_MARGIN_END_OF_MONTH = dtSum["CurrentYTDEndOfMonthMargin"],
                          LYTD_MARGIN_END_OF_MONTH = dtSum["LastYTDEndOfMonthMargin"],
                          PERCENTAGE_OF_TOTAL_YTD_END_OF_MONTH = dtSum["CurrentYTDEndOfMonthAmountPercentageOfTotal"],
                          PERCENTAGE_OF_TOTAL_LYTD_END_OF_MONTH = dtSum["LastYTDEndOfMonthAmountPercentageOfTotal"],

                          OPEN_ORDERS = dtSum["OpenOrdersAmountD"],
                          READY_TO_INVOICE = dtSum["ReadyToInvoiceAmountD"],

                      });
        dt = SharedFunctions.LINQToDataTable(query1);

        //foreach (DataRow Row in dt.Rows)
        //{
        //    foreach (DataColumn c in dt.Columns)
        //    {
        //        Debug.WriteLine(c.ColumnName + ": " + Row[c.ColumnName].ToString());
        //    }
        //    Debug.WriteLine("----------------------------------------------------");
        //}

        try
        {
            foreach (DataColumn col in dt.Columns)
            {
                switch (col.ColumnName)
                {
                    case "LAST_MONTH":
                        col.ColumnName = sFirstDayOfLastMonth + " to " + sLastDayOfLastMonth;//Rename column...
                        break;
                    case "LAST_MONTH_LAST_YEAR":
                        col.ColumnName = "[" + sFirstDayOfLastMonthLastYear + " to " + sLastDayOfLastMonthLastYear + "]";//Rename column...
                        break;
                    case "MTD":
                        col.ColumnName = sFirstDayOfMonthCurrentYear + " to " + sMTD + "";//Rename column...
                        break;
                    case "LAST_YEAR_MTD":
                        col.ColumnName = sFirstDayOfMonthLastYear + " to " + sLastYearMTD;//Rename column...
                        break;
                    case "CURRENT_MONTH":
                        col.ColumnName = sFirstDayOfMonthCurrentYear + " to " + sLastDayOfMonthCurrentYear;//Rename column...
                        break;
                    case "LAST_YEAR_CURRENT_MONTH":
                        col.ColumnName = sFirstDayOfMonthLastYear + " to " + sLastDayOfMonthLastYear;//Rename column...
                        break;
                    case "YTD":
                        col.ColumnName = "[" + sFirstDayOfCurrentYear + " to " + sYTD + "]";//Rename column...
                        break;
                    case "LYTD":
                        col.ColumnName = "[" + sFirstDayOfLastYear + " to " + sLYTD + "]";//Rename column...
                        break;
                    case "LYTD_MINUS_ONE":
                        col.ColumnName = "[" + sFirstDayOfLastYearMinusOne + " to " + sLLYTD + "]";//Rename column...
                        break;
                    case "DIFF":
                        col.ColumnName = DateTime.Now.Year.ToString() + " vs " + DateTime.Now.AddYears(-1).Year.ToString();//Rename column...
                        break;
                    case "DIFF_MINUS_ONE":
                        col.ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " vs " + DateTime.Now.AddYears(-2).Year.ToString();//Rename column...
                        break;
                    case "YTD_MARGIN":
                        col.ColumnName = "[" + sFirstDayOfCurrentYear + " to " + sYTD + " MARGIN" + "]";//Rename column...
                        break;
                    case "LYTD_MARGIN":
                        col.ColumnName = "[" + sFirstDayOfLastYear + " to " + sLYTD + " MARGIN" + "]";//Rename column...
                        break;
                    case "CURRENT_MONTH_MARGIN":
                        col.ColumnName = "[" + sFirstDayOfMonthCurrentYear + " to " + sLastDayOfMonthCurrentYear + " MARGIN" + "]";//Rename column...
                        break;
                    case "LAST_MONTH_MARGIN":
                        col.ColumnName = "[" + sFirstDayOfLastMonthCurrentYear + " to " + sLastDayOfLastMonthCurrentYear + " MARGIN" + "]";//Rename column...
                        break;
                    case "PERCENTAGE_OF_TOTAL_YTD":
                        col.ColumnName = "[" + sFirstDayOfCurrentYear + " to " + sYTD + " % of GT" + "]";//Rename column...
                        break;
                    case "PERCENTAGE_OF_TOTAL_LYTD":
                        col.ColumnName = "[" + sFirstDayOfLastYear + " to " + sLYTD + " % of GT" + "]";//Rename column...
                        break;
                    case "YTD_END_OF_MONTH":
                        col.ColumnName = "[" + sFirstDayOfCurrentYear + " to " + sLastDayOfLastMonthCurrentYear + "]";//Rename column...
                        break;
                    case "LYTD_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfLastYear + " to " + sLastDayOfLastMonthLastYear;//Rename column...
                        break;
                    case "LYTD_MINUS_ONE_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfLastYearMinusOne + " to " + sLastDayOfLastMonthLastYearMinusOne;//Rename column...
                        break;
                    case "DIFF_END_OF_MONTH":
                        col.ColumnName = DateTime.Now.Year.ToString() + " vs " + DateTime.Now.AddYears(-1).Year.ToString() + " Throught End of Last Month";//Rename column...
                        break;
                    case "DIFF_END_OF_MONTH_MINUS_ONE":
                        col.ColumnName = DateTime.Now.AddYears(-1).Year.ToString() + " vs " + DateTime.Now.AddYears(-2).Year.ToString() + " Throught End of Last Month";//Rename column...
                        break;
                    case "YTD_MARGIN_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfCurrentYear + " to " + sLastDayofCurrentYear + " MARGIN";//Rename column...
                        break;
                    case "LYTD_MARGIN_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfLastYear + " to " + sLastDayOfLastYear + " MARGIN";//Rename column...
                        break;
                    case "PERCENTAGE_OF_TOTAL_YTD_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfCurrentYear + " to " + sLastDayofCurrentYear + " % of GT";//Rename column...
                        break;
                    case "PERCENTAGE_OF_TOTAL_LYTD_END_OF_MONTH":
                        col.ColumnName = sFirstDayOfLastYear + " to " + sLastDayOfLastYear + " % of GT";//Rename column...
                        break;
                    case "OPEN_ORDERS":
                        col.ColumnName = "OPEN ORDERS";//Rename column...
                        break;
                    case "READY_TO_INVOICE":
                        col.ColumnName = "READY TO INVOICE";//Rename column...
                        break;
                }


            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }

        string sFileName = "TopFifteenReport_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExcelHelper.ToExcel(dt, sFileName, Page.Response);
    }
    //New Compton Stuff...
    private void LoadPickers(DropDownList ddl)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from u in db.WipUsers
                         orderby u.FirstName, u.LastName
                         where u.Picker == "Y"
                         select new
                         {
                             u.FirstName,
                             u.MiddleName,
                             u.LastName,
                             u.UserID,
                         });

            foreach (var a in query)
            {
                string sFullName = a.FirstName + " " + (a.MiddleName ?? "") + " " + a.LastName;
                sFullName = sFullName.Replace("  ", " ") + " (" + a.UserID.ToString() + ")";
                ddl.Items.Add(new ListItem(sFullName, a.UserID.ToString()));
            }
            ddl.Items.Insert(0, new ListItem("Pickers", "0"));
        }
    }
    private void LoadShipVia(DropDownList ddl)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from s in db.SorShipper
                         where s.Status == 1
                         orderby s.ShipVia
                         select new
                         {
                             s.ShipVia,
                             s.ShipperID,
                         });

            foreach (var a in query)
            {

                ddl.Items.Add(new ListItem(a.ShipVia, a.ShipperID.ToString()));
            }
            ddl.Items.Insert(0, new ListItem("Shippers", "0"));
        }
    }
    private void RunSearch()
    {
        dcGrandTotal = 0;
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
        string sInput = "NULL";
        if (txtSearch.Text.Trim() != "")
        {
            sInput = "'" + txtSearch.Text.Trim().Replace("'", "''") + "'";
        }

        DataTable dt = GetOpenOrders(iRoleID, iUserID, sInput);
        ViewState["Search"] = txtSearch.Text.Trim();
        gvRecord.DataSource = dt;
        gvRecord.DataBind();
    }
    private void UpdateSOT()
    {
        lblError.Text = "";
        lblError0.Text = "";
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            string sSalesOrder = "";
            //Validation...
            for (int i = 0; i < gvRecord.Rows.Count; i++)
            {
                TextBox txtPickDate = (TextBox)gvRecord.Rows[i].FindControl("txtPickDate");
                TextBox txtCasesPicked = (TextBox)gvRecord.Rows[i].FindControl("txtCasesPicked");
                TextBox txtActualDeliveryDate = (TextBox)gvRecord.Rows[i].FindControl("txtActualDeliveryDate");
                TextBox txtAppointmentDateTime = (TextBox)gvRecord.Rows[i].FindControl("txtAppointmentDateTime");
                DropDownList ddlStartHours = (DropDownList)gvRecord.Rows[i].FindControl("ddlStartHours");
                DropDownList ddlStartMinutes = (DropDownList)gvRecord.Rows[i].FindControl("ddlStartMinutes");
                DropDownList ddlEndHours = (DropDownList)gvRecord.Rows[i].FindControl("ddlEndHours");
                DropDownList ddlEndMinutes = (DropDownList)gvRecord.Rows[i].FindControl("ddlEndMinutes");
                DropDownList ddlAppointmentTimeMinutes = (DropDownList)gvRecord.Rows[i].FindControl("ddlAppointmentTimeMinutes");
                DropDownList ddlAppointmentTimeHours = (DropDownList)gvRecord.Rows[i].FindControl("ddlAppointmentTimeHours");
                LinkButton lbnSalesOrder = (LinkButton)gvRecord.Rows[i].FindControl("lbnSalesOrder");
                CheckBox chkScanned = (CheckBox)gvRecord.Rows[i].FindControl("chkScanned");
                TextBox txtStaged = (TextBox)gvRecord.Rows[i].FindControl("txtStaged");
                sSalesOrder = lbnSalesOrder.Text.Trim();


                if (txtPickDate.Text.Trim() != "")
                {
                    if (!SharedFunctions.IsDate(txtPickDate.Text.Trim()))
                    {
                        sMsg += "**Pick Date is not a valid date for Sales Order " + sSalesOrder + "!!<br>";
                        txtPickDate.BackColor = Color.Red;
                    }
                }
                if (txtCasesPicked.Text.Trim() != "")
                {
                    if (!SharedFunctions.IsNumeric(txtCasesPicked.Text.Trim()))
                    {
                        sMsg += "**Cases Picked is not a valid number for Sales Order " + sSalesOrder + "!!<br>";
                        txtCasesPicked.BackColor = Color.Red;
                    }
                }
                if (txtActualDeliveryDate.Text.Trim() != "")
                {
                    if (!SharedFunctions.IsDate(txtActualDeliveryDate.Text.Trim()))
                    {
                        sMsg += "**Actual Delivery Date is not a valid date for Sales Order " + sSalesOrder + "!!<br>";
                        txtActualDeliveryDate.BackColor = Color.Red;
                    }
                }
                if (ddlStartHours.SelectedIndex != 0 && ddlStartMinutes.SelectedIndex == 0)
                {
                    sMsg += "**If Start Hours are selected then Start Minutes must also be selected for Sales Order " + sSalesOrder + "!!<br>";
                    ddlStartMinutes.BackColor = Color.Red;
                }
                if (ddlStartHours.SelectedIndex == 0 && ddlStartMinutes.SelectedIndex != 0)
                {
                    sMsg += "**If Start Minutes are selected then Start Hours must also be selected for Sales Order " + sSalesOrder + "!!<br>";
                    ddlStartMinutes.BackColor = Color.Red;
                }

                if (ddlEndHours.SelectedIndex != 0 && ddlEndMinutes.SelectedIndex == 0)
                {
                    sMsg += "**If End Hours are selected then End Minutes must also be selected for Sales Order " + sSalesOrder + "!!<br>";
                    ddlEndMinutes.BackColor = Color.Red;
                }
                if (ddlEndHours.SelectedIndex == 0 && ddlEndMinutes.SelectedIndex != 0)
                {
                    sMsg += "**If End Minutes are selected then End Hours must also be selected for Sales Order " + sSalesOrder + "!!<br>";
                    ddlEndHours.BackColor = Color.Red;
                }

                if (ddlAppointmentTimeHours.SelectedIndex != 0 && ddlAppointmentTimeMinutes.SelectedIndex == 0)
                {
                    sMsg += "**If Appointment Hours are selected then Appointment Minutes must also be selected for Sales Order " + sSalesOrder + "!!<br>";
                    ddlAppointmentTimeMinutes.BackColor = Color.Red;
                }
                if (ddlAppointmentTimeHours.SelectedIndex == 0 && ddlAppointmentTimeMinutes.SelectedIndex != 0)
                {
                    sMsg += "**If Appointment Minutes are selected then Appointment Hours must also be selected for Sales Order " + sSalesOrder + "!!<br>";
                    ddlAppointmentTimeHours.BackColor = Color.Red;
                }
                if (txtAppointmentDateTime.Text.Trim() != "")
                {//If Not Blank...
                    if (!SharedFunctions.IsDate(txtAppointmentDateTime.Text.Trim()))
                    {//See if valid date...
                        sMsg += "**Appointment Date is not a valid date for Sales Order " + sSalesOrder + "!!<br>";
                        txtAppointmentDateTime.BackColor = Color.Red;
                    }
                    else
                    {
                        if (ddlAppointmentTimeHours.SelectedIndex == 0 && ddlAppointmentTimeMinutes.SelectedIndex == 0)
                        {//If no date but a time is selected...

                            sMsg += "**If you enter a date for an appointment, you must select an Appointment Time for Sales Order " + sSalesOrder + "!!<br>";
                            ddlAppointmentTimeHours.BackColor = Color.Red;
                            ddlAppointmentTimeMinutes.BackColor = Color.Red;
                        }
                    }
                }
                else//Appointment Data is blank...
                {
                    if (ddlAppointmentTimeHours.SelectedIndex != 0 && ddlAppointmentTimeMinutes.SelectedIndex != 0)
                    {//If no date but a time is selected...
                        sMsg += "**If you select a Time for an appointment, it must have an Appointment Date for Sales Order " + sSalesOrder + "!!<br>";
                        txtAppointmentDateTime.BackColor = Color.Red;
                    }
                }

                if (txtStaged.Text.Trim() != "")
                {
                    if (txtStaged.Text.Trim() != "P")
                    {
                        if (txtStaged.Text.Trim() != "F")
                        {
                            sMsg += "**Staged MUST be either a P for Partial or a F for Full for Sales Order " + sSalesOrder + "!!<br>";
                            txtStaged.Text = "";
                            txtStaged.BackColor = Color.Red;
                        }
                    }
                }

            }//End Validation Loop...

            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError0.Text = sMsg;
                lblError.ForeColor = Color.Red;
                lblError0.ForeColor = Color.Red;
                return;
            }


            for (int i = 0; i < gvRecord.Rows.Count; i++)
            {



                DropDownList ddlPicker = (DropDownList)gvRecord.Rows[i].FindControl("ddlPicker");
                LinkButton lbnSalesOrder = (LinkButton)gvRecord.Rows[i].FindControl("lbnSalesOrder");
                TextBox txtPickDate = (TextBox)gvRecord.Rows[i].FindControl("txtPickDate");
                TextBox txtCasesPicked = (TextBox)gvRecord.Rows[i].FindControl("txtCasesPicked");
                TextBox txtActualDeliveryDate = (TextBox)gvRecord.Rows[i].FindControl("txtActualDeliveryDate");
                TextBox txtAppointmentDateTime = (TextBox)gvRecord.Rows[i].FindControl("txtAppointmentDateTime");
                DropDownList ddlStartHours = (DropDownList)gvRecord.Rows[i].FindControl("ddlStartHours");
                DropDownList ddlStartMinutes = (DropDownList)gvRecord.Rows[i].FindControl("ddlStartMinutes");
                DropDownList ddlEndHours = (DropDownList)gvRecord.Rows[i].FindControl("ddlEndHours");
                DropDownList ddlEndMinutes = (DropDownList)gvRecord.Rows[i].FindControl("ddlEndMinutes");
                DropDownList ddlAppointmentTimeMinutes = (DropDownList)gvRecord.Rows[i].FindControl("ddlAppointmentTimeMinutes");
                DropDownList ddlAppointmentTimeHours = (DropDownList)gvRecord.Rows[i].FindControl("ddlAppointmentTimeHours");
                DropDownList ddlShipVia = (DropDownList)gvRecord.Rows[i].FindControl("ddlShipVia");
                Label lblStdPickingTimeInMinutes = (Label)gvRecord.Rows[i].FindControl("lblStdPickingTimeInMinutes");
                Label lblStdCasesPerMinute = (Label)gvRecord.Rows[i].FindControl("lblStdCasesPerMinute");
                Label lblTotalQuantity = (Label)gvRecord.Rows[i].FindControl("lblTotalQuantity");
                CheckBox chkScanned = (CheckBox)gvRecord.Rows[i].FindControl("chkScanned");
                TextBox txtStaged = (TextBox)gvRecord.Rows[i].FindControl("txtStaged");

                sSalesOrder = lbnSalesOrder.Text.Trim();
                int iOrderQuantity = 0;
                if (lblTotalQuantity.Text != "")
                {
                    iOrderQuantity = Convert.ToInt32(lblTotalQuantity.Text);
                }


                SorMaster ca = db.SorMaster.Single(p => Convert.ToInt32(p.SalesOrder) == Convert.ToInt32(sSalesOrder));
                if (ddlPicker.SelectedIndex != 0)
                {
                    ca.PickerUserID = Convert.ToInt32(ddlPicker.SelectedValue);
                }
                else
                {
                    ca.PickerUserID = null;
                }
                if (txtPickDate.Text.Trim() != "")
                {
                    ca.PickDate = Convert.ToDateTime(txtPickDate.Text.Trim());
                }
                else
                {
                    ca.PickDate = null;
                }

                if (txtCasesPicked.Text.Trim() != "")
                {
                    ca.CasesPicked = Convert.ToInt32(txtCasesPicked.Text.Trim());
                }
                else
                {
                    ca.CasesPicked = null;
                }

                //START...
                if (ddlStartHours.SelectedIndex != 0 && ddlStartMinutes.SelectedIndex != 0)
                {
                    ca.StartPickTime = Convert.ToDateTime(DateTime.Today.ToShortDateString() + " " + ddlStartHours.SelectedValue + ":" + ddlStartMinutes.SelectedValue);
                }
                else
                {
                    ca.StartPickTime = null;
                }

                ca.StdCasesPerMinute = Convert.ToDecimal(lblStdCasesPerMinute.Text.Trim());//From New Table...
                ca.StdPickingTimeInMinutes = Convert.ToInt32(lblStdPickingTimeInMinutes.Text.Trim());//Calc... OrderQty / Standard Cases Per Minute...

                //END...
                if (ddlEndHours.SelectedIndex != 0 && ddlEndMinutes.SelectedIndex != 0)
                {
                    ca.EndPickTime = Convert.ToDateTime(DateTime.Today.ToShortDateString() + " " + ddlEndHours.SelectedValue + ":" + ddlEndMinutes.SelectedValue);
                }
                else
                {
                    ca.EndPickTime = null;
                }

                if (txtActualDeliveryDate.Text.Trim() != "")
                {
                    ca.ActualDeliveryDate = Convert.ToDateTime(txtActualDeliveryDate.Text.Trim());
                }
                else
                {
                    ca.ActualDeliveryDate = null;
                }

                if (ddlAppointmentTimeMinutes.SelectedIndex != 0 && ddlAppointmentTimeHours.SelectedIndex != 0)
                {
                    ca.AppointmentTime = Convert.ToDateTime(txtAppointmentDateTime.Text.Trim() + " " + ddlAppointmentTimeHours.SelectedValue + ":" + ddlAppointmentTimeMinutes.SelectedValue);
                }
                else
                {
                    ca.AppointmentTime = null;
                }

                if (ddlShipVia.SelectedIndex != 0)
                {
                    ca.ActualShipViaID = Convert.ToInt32(ddlShipVia.SelectedValue);
                }
                else
                {
                    ca.ActualShipViaID = null;
                }

                if (chkScanned.Checked)
                {
                    ca.ReadyForPickup = DateTime.Now;
                }
                else
                {
                    ca.ReadyForPickup = null;
                }
                if (txtStaged.Text.Trim() != "")
                {
                    ca.Staged = txtStaged.Text.Trim().ToUpper();
                }
                else
                {
                    ca.Staged = null;
                }

                db.SubmitChanges();//Update SorMaster.....

                if (ddlStartMinutes.SelectedIndex != 0 && ddlStartHours.SelectedIndex != 0 && ddlEndMinutes.SelectedIndex != 0 && ddlEndHours.SelectedIndex != 0)
                {
                    DateTime dtStartDateTime = Convert.ToDateTime(ddlStartHours.SelectedValue + ":" + ddlStartMinutes.SelectedValue);
                    DateTime dtEndDateTime = Convert.ToDateTime(ddlEndHours.SelectedValue + ":" + ddlEndMinutes.SelectedValue);
                    TimeSpan tsActualPickingTime = new TimeSpan();

                    tsActualPickingTime = dtEndDateTime - dtStartDateTime;

                    SorMaster ca1 = db.SorMaster.Single(p => Convert.ToInt32(p.SalesOrder) == Convert.ToInt32(sSalesOrder));
                    ca1.ActualPickingTimeInMinutes = Convert.ToInt32(tsActualPickingTime.TotalMinutes);
                    int iCasesPicked = GetCasesPicked(Convert.ToInt32(sSalesOrder));
                    if (iCasesPicked != 0)
                    {
                        ca1.ActualCasesPerMinutes = iCasesPicked / Convert.ToInt32(tsActualPickingTime.TotalMinutes);
                    }
                    ca1.ProjectedFinishTime = dtStartDateTime.AddMinutes(Convert.ToDouble(lblStdPickingTimeInMinutes.Text.Trim())).ToString("HH:mm:ss");
                    db.SubmitChanges();

                }
                else if (ddlStartMinutes.SelectedIndex != 0 && ddlStartHours.SelectedIndex != 0 && ddlEndMinutes.SelectedIndex == 0 && ddlEndHours.SelectedIndex == 0)
                {//No End Time...
                    SorMaster ca1 = db.SorMaster.Single(p => Convert.ToInt32(p.SalesOrder) == Convert.ToInt32(sSalesOrder));
                    ca1.ProjectedFinishTime = Convert.ToDateTime(ddlStartHours.SelectedValue + ":" + ddlStartMinutes.SelectedValue).AddMinutes(Convert.ToDouble(lblStdPickingTimeInMinutes.Text.Trim())).ToString("HH:mm:ss");
                    db.SubmitChanges();
                }
                else if (ddlStartMinutes.SelectedIndex == 0 && ddlStartHours.SelectedIndex == 0 && ddlEndMinutes.SelectedIndex == 0 && ddlEndHours.SelectedIndex == 0)
                {//No end or start time...
                    SorMaster ca1 = db.SorMaster.Single(p => Convert.ToInt32(p.SalesOrder) == Convert.ToInt32(sSalesOrder));
                    ca1.ActualPickingTimeInMinutes = null;
                    ca1.ActualCasesPerMinutes = null;
                    ca1.ProjectedFinishTime = null;
                    db.SubmitChanges();
                }
                else
                {
                    //Ignore...
                }


            }//End Loop...


            RunSearch();

            lblError.Text = "**Sales Order Tracker data was successfully updated!";
            lblError0.Text = "Sales Order Tracker data was successfully updated!";
            lblError.ForeColor = Color.Green;
            lblError0.ForeColor = Color.Green;
        }
    }
    private void AddGroupMembersToDeptCheckBoxes()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sCustomerService = "";
            string sLogistics = "";
            string sOperations = "";
            string sProduction = "";
            string sQualityControl = "";
            string sAccountReceivable = "";
            string sTransfer = "";

            var queryCS = (from mg in db.WipMessageGroupAssignments
                           where mg.WipMessageGroupID == 4//Customer Service...
                           select new
                           {
                               FullName = (mg.WipUsers.FirstName + " " + (mg.WipUsers.MiddleName ?? "") + " " + mg.WipUsers.LastName).Replace("  ", " "),
                           });
            foreach (var a in queryCS)
            {
                sCustomerService += a.FullName + "\n";
            }
            var queryLogistics = (from mg in db.WipMessageGroupAssignments
                                  where mg.WipMessageGroupID == 2//Logistics...
                                  select new
                                  {
                                      FullName = (mg.WipUsers.FirstName + " " + (mg.WipUsers.MiddleName ?? "") + " " + mg.WipUsers.LastName).Replace("  ", " "),
                                  });
            foreach (var a in queryLogistics)
            {
                sLogistics += a.FullName + "\n";
            }
            var queryOperations = (from mg in db.WipMessageGroupAssignments
                                   where mg.WipMessageGroupID == 1//Operations...
                                   select new
                                   {
                                       FullName = (mg.WipUsers.FirstName + " " + (mg.WipUsers.MiddleName ?? "") + " " + mg.WipUsers.LastName).Replace("  ", " "),
                                   });
            foreach (var a in queryOperations)
            {
                sOperations += a.FullName + "\n";
            }
            var queryProduction = (from mg in db.WipMessageGroupAssignments
                                   where mg.WipMessageGroupID == 3//Production...
                                   select new
                                   {
                                       FullName = (mg.WipUsers.FirstName + " " + (mg.WipUsers.MiddleName ?? "") + " " + mg.WipUsers.LastName).Replace("  ", " "),
                                   });
            foreach (var a in queryProduction)
            {
                sProduction += a.FullName + "\n";
            }
            var queryQualityControl = (from mg in db.WipMessageGroupAssignments
                                       where mg.WipMessageGroupID == 5//Quality Control...
                                       select new
                                       {
                                           FullName = (mg.WipUsers.FirstName + " " + (mg.WipUsers.MiddleName ?? "") + " " + mg.WipUsers.LastName).Replace("  ", " "),
                                       });
            foreach (var a in queryQualityControl)
            {
                sQualityControl += a.FullName + "\n";
            }
            var queryAccountsReceivable = (from mg in db.WipMessageGroupAssignments
                                           where mg.WipMessageGroupID == 12//Accounts Receivable...
                                           select new
                                           {
                                               FullName = (mg.WipUsers.FirstName + " " + (mg.WipUsers.MiddleName ?? "") + " " + mg.WipUsers.LastName).Replace("  ", " "),
                                           });
            foreach (var a in queryAccountsReceivable)
            {
                sAccountReceivable += a.FullName + "\n";
            }
            var queryTransfer = (from mg in db.WipMessageGroupAssignments
                                 where mg.WipMessageGroupID == 13//Transfer...
                                 select new
                                 {
                                     FullName = (mg.WipUsers.FirstName + " " + (mg.WipUsers.MiddleName ?? "") + " " + mg.WipUsers.LastName).Replace("  ", " "),
                                 });
            foreach (var a in queryTransfer)
            {
                sTransfer += a.FullName + "\n";
            }


            chkCCtoCustomerService.Attributes.Add("title", sCustomerService);
            chkCCtoLogistics.Attributes.Add("title", sLogistics);
            chkCCtoOperations.Attributes.Add("title", sOperations);
            chkCCtoProduction.Attributes.Add("title", sProduction);
            chkCCtoQualityControl.Attributes.Add("title", sQualityControl);
            chkCCtoAccountsReceivable.Attributes.Add("title", sAccountReceivable);
            chkCCtoTransfer.Attributes.Add("title", sTransfer);
        }

    }
    private void UpdatePlacedOrdersDetails()
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sSalesOrder = "";
            string sSorPlacedOrdersNotesCommentOptionsID = "";
            try
            {
                for (int idx = 0; idx < gvPlacedOrderDetails.Rows.Count; idx++)
                {
                    Label lblSalesOrderLong = (Label)gvPlacedOrderDetails.Rows[idx].FindControl("lblSalesOrderLong");
                    DropDownList ddlPlaceOrdersNote = (DropDownList)gvPlacedOrderDetails.Rows[idx].FindControl("ddlPlaceOrdersNote");
                    sSalesOrder = lblSalesOrderLong.Text;
                    sSorPlacedOrdersNotesCommentOptionsID = ddlPlaceOrdersNote.SelectedValue;
                    if (PlacedOrderNoteExist(sSalesOrder))
                    {//Update...

                        if (ddlPlaceOrdersNote.SelectedIndex != 0)
                        {
                            SorPlacedOrdersNotes n = db.SorPlacedOrdersNotes.Single(p => p.SalesOrder == sSalesOrder);
                            n.SorPlacedOrdersNotesCommentOptionsID = Convert.ToInt32(sSorPlacedOrdersNotesCommentOptionsID);
                            db.SubmitChanges();
                        }
                        else
                        {
                            var query = (from n in db.SorPlacedOrdersNotes where n.SalesOrder == sSalesOrder select n);
                            foreach (var a in query)
                            {
                                SorPlacedOrdersNotes n = db.SorPlacedOrdersNotes.Single(p => p.PlacedOrdersNoteID == a.PlacedOrdersNoteID);
                                db.SorPlacedOrdersNotes.DeleteOnSubmit(n);
                                db.SubmitChanges();
                            }
                        }
                    }
                    else//Add...
                    {
                        if (ddlPlaceOrdersNote.SelectedIndex != 0)
                        {
                            SorPlacedOrdersNotes n = new SorPlacedOrdersNotes();
                            n.SorPlacedOrdersNotesCommentOptionsID = Convert.ToInt32(sSorPlacedOrdersNotesCommentOptionsID);
                            n.SalesOrder = sSalesOrder;
                            n.UserID = Convert.ToInt32(Session["UserID"]);
                            n.DateAdded = DateTime.Now;
                            db.SorPlacedOrdersNotes.InsertOnSubmit(n);
                            db.SubmitChanges();
                        }
                    }
                }

                LoadPlacedOrdersDetails();

                lblOrdersPlacedError.Text = "**Place Orders Details Updated Successfully!";
                lblOrdersPlacedError.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblOrdersPlacedError.Text = "**Updating Place Orders Details Failed!";
                lblOrdersPlacedError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }


        }
    }
    private void LoadSorPlacedOrdersNotesCommentOptions(DropDownList ddl)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from u in db.SorPlacedOrdersNotesCommentOptions
                         orderby u.CommentOption
                         select new
                         {
                             u.CommentOption,
                             u.SorPlacedOrdersNotesCommentOptionsID
                         });

            foreach (var a in query)
            {
                ddl.Items.Add(new ListItem(a.CommentOption, a.SorPlacedOrdersNotesCommentOptionsID.ToString()));
            }
            ddl.Items.Insert(0, new ListItem("Comments", "0"));
        }
    }
    private void SetupHeaderWidths(int iUserID)
    {
        if (rblCompton.SelectedIndex == 1)//Compton View...
        {
            btnUpdate.Visible = true;
            bool bCompton = SharedFunctions.IsCompton(iUserID);
            if (bCompton || rblCompton.SelectedIndex == 1)
            {
                HeaderTable.Width = Unit.Pixel(2400);
                gvRecord.Width = Unit.Pixel(2400);
                pnlGridView.Width = Unit.Pixel(2425);
                int iCellIndex = 0;
                foreach (TableRow row in HeaderTable.Rows)
                {
                    foreach (TableCell headerCell in row.Cells)
                    {
                        switch (iCellIndex)
                        {
                            case 0://#
                                headerCell.Width = Unit.Pixel(37);
                                headerCell.Visible = true;
                                break;
                            case 1://Fist/Last Operator...
                                headerCell.Width = Unit.Pixel(63);
                                headerCell.Visible = true;
                                break;
                            case 2://Cust ID...
                                headerCell.Width = Unit.Pixel(55);
                                headerCell.Visible = true;
                                break;
                            case 3://Customer...
                                headerCell.Width = Unit.Pixel(225);
                                headerCell.Visible = true;
                                break;
                            case 4://Notes Icon...
                                headerCell.Width = Unit.Pixel(23);
                                headerCell.Visible = true;
                                break;
                            case 5://Ack Icon...
                                headerCell.Width = Unit.Pixel(20);
                                headerCell.Visible = false;
                                break;
                            case 6://Order Date...
                                headerCell.Width = Unit.Pixel(80);
                                headerCell.Visible = true;
                                break;
                            case 7://Sales Order...
                                headerCell.Width = Unit.Pixel(58);
                                headerCell.Visible = true;
                                break;
                            case 8://PO...
                                headerCell.Width = Unit.Pixel(155);
                                headerCell.Visible = true;
                                break;
                            case 9://Total Qty...
                                headerCell.Width = Unit.Pixel(50);
                                headerCell.Visible = false;//Hidden...
                                break;
                            case 10://Total Value...
                                headerCell.Width = Unit.Pixel(130);
                                headerCell.Visible = true;
                                break;
                            case 11://Order Status...
                                headerCell.Width = Unit.Pixel(120);
                                headerCell.Visible = true;
                                break;
                            case 12://Assigned picker...
                                headerCell.Width = Unit.Pixel(110);
                                headerCell.Visible = true;
                                break;
                            case 13://Assigned Pick Date...
                                headerCell.Width = Unit.Pixel(100);
                                headerCell.Visible = true;
                                break;
                            case 14://Start Pick Time...
                                headerCell.Width = Unit.Pixel(95);
                                headerCell.Visible = true;
                                break;
                            case 15://End Pick Time
                                headerCell.Width = Unit.Pixel(95);
                                headerCell.Visible = true;
                                break;
                            case 16://Std Pick Time...
                                headerCell.Width = Unit.Pixel(77);
                                headerCell.Visible = true;
                                break;
                            case 17://Std Cases per Min...
                                headerCell.Width = Unit.Pixel(77);
                                headerCell.Visible = true;
                                break;
                            case 18://Actual Pick Time...
                                headerCell.Width = Unit.Pixel(55);
                                headerCell.Visible = true;
                                break;
                            case 19://Actual Cases per minute
                                headerCell.Width = Unit.Pixel(57);
                                headerCell.Visible = true;
                                break;
                            case 20://Cases Picked...
                                headerCell.Width = Unit.Pixel(52);
                                headerCell.Visible = true;
                                break;
                            case 21://Projected Finish Time...
                                headerCell.Width = Unit.Pixel(77);
                                headerCell.Visible = true;
                                break;
                            case 22://Fel Ship Date...
                                headerCell.Width = Unit.Pixel(80);
                                headerCell.Visible = true;
                                break;
                            case 23://Default Ship Via...
                                headerCell.Width = Unit.Pixel(100);
                                headerCell.Visible = true;
                                break;
                            case 24://Sales Person icon...
                                headerCell.Width = Unit.Pixel(20);
                                headerCell.Visible = false;
                                break;
                            case 25://Appointment Time - new 11-16-2021
                                headerCell.Width = Unit.Pixel(20);
                                headerCell.Visible = false;
                                break;
                            case 26://Staged
                                headerCell.Width = Unit.Pixel(20);
                                headerCell.Visible = true;
                                headerCell.Font.Size = FontUnit.Point(7);
                                break;
                            case 27://Pallets Required
                                headerCell.Width = Unit.Pixel(30);
                                headerCell.Visible = true;
                                headerCell.Font.Size = FontUnit.Point(7);
                                break;
                            case 28://Scanned
                                headerCell.Width = Unit.Pixel(35);
                                headerCell.Visible = true;
                                break;
                            case 29://HOLD CREDIT...
                                headerCell.Width = Unit.Pixel(35);
                                headerCell.Visible = true;
                                break;
                            case 30://Sched Del Date...
                                headerCell.Width = Unit.Pixel(75);
                                headerCell.Visible = true;
                                break;
                            case 31://Actual Del Date...
                                headerCell.Width = Unit.Pixel(75);
                                headerCell.Visible = true;
                                break;
                            case 32://Lead Times Icon...
                                headerCell.Width = Unit.Pixel(23);
                                headerCell.Visible = true;
                                break;
                            case 33://Appoinment Date...
                                headerCell.Width = Unit.Pixel(90);
                                headerCell.Visible = true;
                                break;
                            case 34://Appoinment Time...
                                headerCell.Width = Unit.Pixel(90);
                                headerCell.Visible = true;
                                break;
                            case 35://Actual Ship Via...
                                headerCell.Width = Unit.Pixel(90);
                                headerCell.Visible = true;
                                break;
                            case 36://Del UnScheduled Icon...
                                headerCell.Width = Unit.Pixel(20);
                                headerCell.Visible = false;//Hidden...
                                break;
                            case 37://Del Scheduled Icon...
                                headerCell.Width = Unit.Pixel(20);
                                headerCell.Visible = false;//Hidden...
                                break;
                            case 38://Special Instructions...
                                headerCell.Width = Unit.Pixel(20);
                                headerCell.Visible = false;
                                break;

                        }

                        iCellIndex++;

                    }
                }
            }
        }
        else//Not Compton View...
        {
            HeaderTable.Width = Unit.Pixel(1480);
            gvRecord.Width = Unit.Pixel(1480);
            pnlGridView.Width = Unit.Pixel(1500);
            btnUpdate.Visible = false;
            int iCellIndex = 0;
            foreach (TableRow row in HeaderTable.Rows)
            {
                foreach (TableCell headerCell in row.Cells)
                {
                    switch (iCellIndex)
                    {
                        case 0://#
                            headerCell.Width = Unit.Pixel(35);
                            headerCell.Visible = true;
                            break;
                        case 1://First/Last Operator...
                            headerCell.Width = Unit.Pixel(66);
                            headerCell.Visible = true;
                            break;
                        case 2://Cust ID...
                            headerCell.Width = Unit.Pixel(53);
                            headerCell.Visible = true;
                            break;
                        case 3://Customer...
                            headerCell.Width = Unit.Pixel(177);
                            headerCell.Visible = true;
                            break;
                        case 4://Comment Icon...
                            headerCell.Width = Unit.Pixel(20);
                            headerCell.Visible = true;
                            break;
                        case 5://Ack Icon...
                            headerCell.Width = Unit.Pixel(20);
                            headerCell.Visible = true;
                            break;
                        case 6://Order Date...
                            headerCell.Width = Unit.Pixel(70);
                            headerCell.Visible = true;
                            break;
                        case 7://Sales Order...
                            headerCell.Width = Unit.Pixel(55);
                            headerCell.Visible = true;
                            break;
                        case 8://PO...
                            headerCell.Width = Unit.Pixel(140);
                            headerCell.Visible = true;
                            break;
                        case 9://Total Qty...
                            headerCell.Width = Unit.Pixel(50);
                            headerCell.Visible = false;//Hidden...
                            break;
                        case 10://Total Value...
                            headerCell.Width = Unit.Pixel(110);
                            headerCell.Visible = true;
                            break;
                        case 11://Order Status...
                            headerCell.Width = Unit.Pixel(105);
                            headerCell.Visible = true;
                            break;
                        case 12://Assigned picker...
                            headerCell.Width = Unit.Pixel(100);
                            headerCell.Visible = false;
                            break;
                        case 13://Assigned Pick Date...
                            headerCell.Width = Unit.Pixel(90);
                            headerCell.Visible = false;
                            break;
                        case 14://Start Pick Time...
                            headerCell.Width = Unit.Pixel(84);
                            headerCell.Visible = false;
                            break;
                        case 15://End Pick Time
                            headerCell.Width = Unit.Pixel(84);
                            headerCell.Visible = false;
                            break;
                        case 16://Std Pick Time in Minutes...
                            headerCell.Width = Unit.Pixel(70);
                            headerCell.Visible = false;
                            break;
                        case 17://Std Cases per Min...
                            headerCell.Width = Unit.Pixel(70);
                            headerCell.Visible = false;
                            break;
                        case 18://Actual Pick Time...
                            headerCell.Width = Unit.Pixel(50);
                            headerCell.Visible = false;
                            break;
                        case 19://Actual Cases per minute
                            headerCell.Width = Unit.Pixel(52);
                            headerCell.Visible = false;
                            break;
                        case 20://Cases Picked...
                            headerCell.Width = Unit.Pixel(52);
                            headerCell.Visible = false;
                            break;
                        case 21://Projected Finish Time...
                            headerCell.Width = Unit.Pixel(52);
                            headerCell.Visible = false;
                            break;
                        case 22://Fel Ship Date...
                            headerCell.Width = Unit.Pixel(80);
                            headerCell.Visible = true;
                            break;
                        case 23://Default Ship Via...
                            headerCell.Width = Unit.Pixel(110);
                            headerCell.Visible = true;
                            break;
                        case 24://Sales Person icon...
                            headerCell.Width = Unit.Pixel(20);
                            headerCell.Visible = false;//Hidden...
                            break;
                        case 25://Appointment Time - new 11-16-2021
                            headerCell.Width = Unit.Pixel(20);
                            headerCell.Visible = true;
                            break;
                        case 26://Staged
                            headerCell.Width = Unit.Pixel(20);
                            headerCell.Visible = true;
                            headerCell.Font.Size = FontUnit.Point(7);
                            break;
                        case 27://Pallets Required
                            headerCell.Width = Unit.Pixel(30);
                            headerCell.Visible = true;
                            headerCell.Font.Size = FontUnit.Point(7);
                            break;
                        case 28://Scanned
                            headerCell.Width = Unit.Pixel(35);
                            headerCell.Visible = true;
                            break;
                        case 29://HOLD CREDIT...
                            headerCell.Width = Unit.Pixel(35);
                            headerCell.Visible = true;
                            break;
                        case 30://Sched Del Date...
                            headerCell.Width = Unit.Pixel(75);
                            headerCell.Visible = true;
                            break;
                        case 31://Actual Del Date...
                            headerCell.Width = Unit.Pixel(75);
                            headerCell.Visible = false;
                            break;
                        case 32://Lead Times Icon...
                            headerCell.Width = Unit.Pixel(23);
                            headerCell.Visible = false;
                            break;
                        case 33://Appoinment Date...
                            headerCell.Width = Unit.Pixel(90);
                            headerCell.Visible = false;
                            break;
                        case 34://Appoinment Time...
                            headerCell.Width = Unit.Pixel(90);
                            headerCell.Visible = false;
                            break;
                        case 35://Actual Ship Via...
                            headerCell.Width = Unit.Pixel(90);
                            headerCell.Visible = false;
                            break;
                        case 36://Del UnScheduled Icon...
                            headerCell.Width = Unit.Pixel(20);
                            headerCell.Visible = false;//Hidden...
                            break;
                        case 37://Del Scheduled Icon...
                            headerCell.Width = Unit.Pixel(20);
                            headerCell.Visible = false;//Hidden...
                            break;
                        case 38://Special Instructions...
                            headerCell.Width = Unit.Pixel(20);
                            headerCell.Visible = false;
                            break;

                    }

                    iCellIndex++;

                }
            }
        }
    }
    #endregion

    #region Functions
    private DataTable GetOpenOrders(int iRoleID, int iUserID, string sInput)
    {//Default...(HEADER)
        DataTable dt = new DataTable();

        try
        {

            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            string sSQL = "";
            if (iRoleID != 4)//Everybody Else...
            {
                sSQL = "EXEC spGetSalesOrderTracker @RoleID=" + iRoleID.ToString() + ",@Input=" + sInput;
            }
            else//Client...
            {
                sSQL = "EXEC spGetSalesOrderTracker @RoleID=" + iRoleID.ToString();
                sSQL += " ,@UserID =" + iUserID.ToString() + ",@Input=" + sInput;
            }

            Debug.WriteLine(sSQL);
            dt = SharedFunctions.getDataTable(sSQL, conn, "JobHeader");


            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            dt.Columns.Add(column);

            //Set values for existing rows...
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["ID"] = i + 1;
            }

            dt.AcceptChanges();


            DataView dv = new DataView(dt);
            if (ddlSort1.SelectedIndex != 0 && ddlSort2.SelectedIndex != 0 && ddlSort3.SelectedIndex != 0)
            {//All Select...
                dv.Sort = ddlSort1.SelectedValue + "," + ddlSort2.SelectedValue + "," + ddlSort3.SelectedValue;
            }
            else if (ddlSort1.SelectedIndex != 0 && ddlSort2.SelectedIndex != 0 && ddlSort3.SelectedIndex == 0)
            {//First two selected...
                dv.Sort = ddlSort1.SelectedValue + "," + ddlSort2.SelectedValue;
            }
            else if (ddlSort1.SelectedIndex == 0 && ddlSort2.SelectedIndex != 0 && ddlSort3.SelectedIndex != 0)
            {//Last two selected...
                dv.Sort = ddlSort2.SelectedValue + "," + ddlSort3.SelectedValue;
            }
            else if (ddlSort1.SelectedIndex != 0 && ddlSort2.SelectedIndex == 0 && ddlSort3.SelectedIndex != 0)
            {//First & Last selected...
                dv.Sort = ddlSort1.SelectedValue + "," + ddlSort3.SelectedValue;
            }
            else if (ddlSort1.SelectedIndex != 0 && ddlSort2.SelectedIndex == 0 && ddlSort3.SelectedIndex == 0)
            {//First one selected...
                dv.Sort = ddlSort1.SelectedValue;
            }
            else if (ddlSort1.SelectedIndex == 0 && ddlSort2.SelectedIndex != 0 && ddlSort3.SelectedIndex == 0)
            {//Second one selected...
                dv.Sort = ddlSort2.SelectedValue;
            }
            else if (ddlSort1.SelectedIndex == 0 && ddlSort2.SelectedIndex == 0 && ddlSort3.SelectedIndex != 0)
            {//Third One Selected...
                dv.Sort = ddlSort3.SelectedValue;
            }
            else
            {
                //Ignore...
            }


            dt = dv.ToTable();
            Session["dtRecordSalesOrderTracker"] = dt;
            lblPageNo.Text = "Current Page #: 1";

        }
        catch (Exception)
        {
            return null;
        }
        finally
        {
            dt.Dispose();
        }
        return dt;
        //TODO: Sub to Populate the Timeline...

    }
    private DataTable LoadTopFifteenData(string sListMode)
    {

        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        sSQL = "EXEC spGetTopFifteenList ";
        sSQL += "@ListMode ='" + sListMode + "'";


        Debug.WriteLine(sSQL);

        dt = SharedFunctions.getDataTable(sSQL, conn, "dtTopFifteen");
        gvTopFifteen.DataSource = dt;
        gvTopFifteen.DataBind();
        Session["dtTopFifteen"] = dt;
        dt.Dispose();

        return dt;


    }
    private string[] ParseHelper(String line, int lineRead)
    {
        MemoryStream mem = new MemoryStream(ASCIIEncoding.Default.GetBytes(line));
        TextFieldParser ReaderTemp = new TextFieldParser(mem);
        ReaderTemp.TextFieldType = FieldType.Delimited;
        ReaderTemp.SetDelimiters(new string[] { "\t", "," });
        ReaderTemp.HasFieldsEnclosedInQuotes = true;
        ReaderTemp.TextFieldType = FieldType.Delimited;
        ReaderTemp.TrimWhiteSpace = true;
        try
        {
            return ReaderTemp.ReadFields();
        }
        catch (MalformedLineException ex)
        {
            throw new MalformedLineException(String.Format(
                "Line {0} is not valid and will be skipped: {1}\r\n\r\n{2}",
                lineRead, ReaderTemp.ErrorLine, ex));
        }
    }
    //private bool HasShortage(string sSalesOrder, string sStockCode, int iRoleID, int iUserID)
    //{
    //    DataTable dt = new DataTable();
    //    string sSQL = "";
    //    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
    //    if (sStockCode == "")
    //    {
    //        sSQL = "EXEC spGetInventoryShortageYN @SalesOrder='" + sSalesOrder + "',";
    //        sSQL += "@RoleID = " + iRoleID.ToString() + ",";
    //        sSQL += "@UserID =" + iUserID.ToString();
    //    }
    //    else
    //    {
    //        sSQL = "EXEC spGetInventoryShortageYN @SalesOrder='" + sSalesOrder + "',@StockCode='" + sStockCode + "',";
    //        sSQL += "@RoleID = " + iRoleID.ToString() + ",";
    //        sSQL += "@UserID =" + iUserID.ToString();
    //    }
    //    Debug.WriteLine(sSQL);
    //    dt = SharedFunctions.getDataTable(sSQL, conn, "Shortage");

    //    if (dt.Rows.Count > 0)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    private string GetSortDirection()
    {
        switch (GridViewSortDirection)
        {
            case "ASC":
                GridViewSortDirection = "DESC";
                break;
            case "DESC":
                GridViewSortDirection = "ASC";
                break;
        }
        return GridViewSortDirection;
    }
    private DataTable GetDetails(string sSalesOrder, GridView gv)
    {//DETAILS GRID Create Dynamically...
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        try
        {

            sSQL = "spGetSalesOrderTrackerDetails @SalesOrder =" + sSalesOrder;

            Debug.WriteLine(sSQL);
            dt = SharedFunctions.getDataTable(sSQL, conn, "Details");


            //Shortage Data show now comes from SorReadyStatusAlerts table...7-31-2019...
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "RowID";
            dt.Columns.Add(column);
            //Set values for existing rows...
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["RowID"] = i + 1;
            }


            if (dt.Rows.Count > 0)
            {
                gv.DataSource = dt;
                gv.DataBind();
                Session["dtDetails"] = dt;
            }
            else
            {
                gv.DataSource = null;
                gv.DataBind();
            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            if (dt != null)
            {
                dt.Dispose();
            }
        }

        return dt;

    }
    private DataTable GetProductionMatrix(string sSalesOrder, string sStockCode)
    {//Production Matrix GRID Create Dynamically...
        DataTable dt = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        try
        {
            sSQL = "spGetProductionMatrix @SalesOrder ='" + sSalesOrder + "',@StockCode='" + sStockCode + "'";

            Debug.WriteLine(sSQL);
            dt = SharedFunctions.getDataTable(sSQL, conn, "ProductionMatrix");
            DataColumn column = new DataColumn();
            column.DataType = System.Type.GetType("System.Int32");
            column.ColumnName = "ID";
            dt.Columns.Add(column);
            //Set values for existing rows...
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["ID"] = i + 1;
            }

            Session["dtProductionMatrix"] = dt;

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            if (dt != null)
            {
                dt.Dispose();
            }
        }

        return dt;

    }
    private bool HasDeliveryRecords(string sSalesOrder)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var qry = (from dm in db.DelMaster
                   where dm.SalesOrder == sSalesOrder
                   select dm);
        if (qry.Count() > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool SalesOrderCommentsExist(string sSalesOrder)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var qry = (from dm in db.SorComments
                   where dm.SalesOrder == sSalesOrder
                   select dm);
        if (qry.Count() > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private string GetSalesOrderComments(string sSalesOrder)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sComments = "";

            var qry = (from dm in db.SorComments
                       join u in db.WipUsers on dm.AddedBy equals u.UserID
                       where dm.SalesOrder == sSalesOrder
                       select new
                       {
                           dm.Comment,
                           Name = (u.FirstName + " " + (u.MiddleName ?? "") + " " + u.LastName).Replace("  ", " ")
                       });
            foreach (var a in qry)
            {
                sComments = a.Name + " - " + a.Comment;
            }
            return sComments;
        }
    }
    private int GetSalespersonWipUserID(string sSalesPersonID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            int iWipUserID = 0;


            var qry = (from wp in db.WipUsers
                       where wp.Salesperson == sSalesPersonID
                       && wp.Salesperson != null
                       select wp);
            foreach (var a in qry)
            {
                iWipUserID = a.UserID;
            }

            return iWipUserID;
        }

    }
    private int GetWipUserID(string sSalesPersonID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            int iWipUserID = 0;


            var qry = (from wp in db.WipUsers
                       where wp.Salesperson == sSalesPersonID
                       select wp);
            foreach (var a in qry)
            {
                iWipUserID = a.UserID;
            }

            return iWipUserID;
        }

    }
    private int GetCasesPicked(int iSalesOrder)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            int iCasesPicked = 0;

            var qry = (from sm in db.SorMaster
                       where Convert.ToInt32(sm.SalesOrder) == iSalesOrder
                       select new
                       {
                           sm.CasesPicked
                       });
            foreach (var a in qry)
            {
                if (a.CasesPicked.HasValue)
                {
                    iCasesPicked = (int)a.CasesPicked;
                }
            }
            return iCasesPicked;
        }
    }
    private DateTime GetStartPickTime(int iSalesOrder)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DateTime dtStartPickTime = new DateTime(2000, 1, 1);

            var qry = (from sm in db.SorMaster
                       where Convert.ToInt32(sm.SalesOrder) == iSalesOrder
                       select new
                       {
                           sm.StartPickTime
                       });
            foreach (var a in qry)
            {
                dtStartPickTime = (DateTime)a.StartPickTime;
            }
            return dtStartPickTime;
        }
    }
    private DateTime GetEndPickTime(int iSalesOrder)
    {

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DateTime dtEndPickTime = new DateTime(2000, 1, 1);
            var qry = (from sm in db.SorMaster
                       where Convert.ToInt32(sm.SalesOrder) == iSalesOrder
                       select new
                       {
                           sm.EndPickTime
                       });
            foreach (var a in qry)
            {
                dtEndPickTime = (DateTime)a.EndPickTime;
            }
            return dtEndPickTime;
        }
    }
    private bool PlacedOrderNoteExist(string sSalesOrder)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var qry = (from dm in db.SorPlacedOrdersNotes
                   where dm.SalesOrder == sSalesOrder
                   select dm);
        if (qry.Count() > 0)
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

            if (Server.MachineName.ToUpper() == "BMW" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
            {
                txtSearch.Text = "Innovative Beverage Concepts";
            }

            List<string> lOKToUse = SharedFunctions.GetSecurityGroupMembers(2);//Scanned...      
            if (!lOKToUse.Contains(iUserID.ToString()))
            {
                bScanned = false;
            }
            else
            {
                bScanned = true;
            }

            lOKToUse = SharedFunctions.GetSecurityGroupMembers(3);//Advanced Scanned...      
            if (!lOKToUse.Contains(iUserID.ToString()))
            {
                bAdvancedScanned = false;
            }
            else
            {
                bAdvancedScanned = true;
            }

            lOKToUse = SharedFunctions.GetSecurityGroupMembers(1);//Staged...            
            if (!lOKToUse.Contains(iUserID.ToString()))
            {
                bStaged = false;
            }
            else
            {
                bStaged = true;
            }

            rblCompton.SelectedIndex = 0;
            if (Session["SOT_TimerChecked"] != null)
            {
                if ((bool)Session["SOT_TimerChecked"] == true)
                {
                    chkTimerOnOff.Checked = true;
                    Timer1.Enabled = true;
                }
                else
                {
                    chkTimerOnOff.Checked = false;
                    Timer1.Enabled = false;
                }
            }
            else
            {
                chkTimerOnOff.Checked = true;
                Timer1.Enabled = true;
            }
            if (Session["Interval"] != null)
            {
                ddlInterval.SelectedValue = Session["Interval"].ToString();
            }

            UpdateSalesOrderAcknowledgementValue();

            Session["dtRecordSalesOrderTracker"] = null;

            RunSearch();

            GetSummary(iRoleID, iUserID);

            lblOrdersPlacedError.Text = "";
            LoadPlaceOrderData(iRoleID, iUserID);

            lblToday.Text = DateTime.Now.ToShortDateString();
            lblYesterday.Text = DateTime.Now.AddDays(-1).ToShortDateString();
            lblThisMonth.Text = DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture) + " " + DateTime.Now.Year.ToString();

            int iDaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);

            txtStartDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString();
            txtEndDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, iDaysInMonth).ToShortDateString();

            LoadTopFifteenData("Y");
            switch (iRoleID)
            {
                case 1://Admin...
                    pnlTopFifteen.Visible = true;
                    rblCompton.Visible = true;
                    pnlPlacedOrdersSummary.Visible = true;
                    break;
                case 2://Supervisor...
                    pnlTopFifteen.Visible = true;
                    pnlPlacedOrdersSummary.Visible = true;
                    rblCompton.Visible = false;
                    break;
                case 8://Credit manager...
                    pnlTopFifteen.Visible = true;
                    pnlPlacedOrdersSummary.Visible = true;
                    rblCompton.Visible = true;
                    break;
                default:
                    pnlTopFifteen.Visible = false;
                    pnlPlacedOrdersSummary.Visible = false;
                    rblCompton.Visible = false;
                    break;
            }

            bool bCompton = SharedFunctions.IsCompton(iUserID);
            if (bCompton)
            {
                rblCompton.SelectedIndex = 1;
                btnUpdate.Visible = true;
            }
            else
            {
                btnUpdate.Visible = false;
            }

            SetupHeaderWidths(iUserID);

        }
        if (Page.IsPostBack)
        {
            //Use to Maintain jQuery during postbacks...
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isPostBack", "$(document).ready(isPostBack);", true);
        }
    }
    protected void gvRecord_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvRecord.PageIndex = e.NewPageIndex;
        lblPageNo.Text = "Current Page #: " + (gvRecord.PageIndex + 1).ToString();
        gvRecord.DataSource = (DataTable)Session["dtRecordSalesOrderTracker"];
        gvRecord.DataBind();
    }
    protected void gvRecord_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {

                decimal dcTotalValue = 0;
                if (e.Row.RowType == DataControlRowType.Header)
                {

                    bool bCompton = SharedFunctions.IsCompton(iUserID);
                    if (bCompton || rblCompton.SelectedIndex == 1)//Is compton employee or admnin set page to compton view...
                    {
                        e.Row.Cells[5].Visible = false;//Acknowledged
                        e.Row.Cells[25].Visible = false;//Appointment Date Time Label...                              
                        //e.Row.Cells[33].Visible = false;//UnScheduled Delivery...
                        //e.Row.Cells[34].Visible = false;//Scheduled Delivery...   
                    }
                    else//Not Compton View...
                    {
                        e.Row.Cells[12].Visible = false;//Assigned Picker...
                        e.Row.Cells[13].Visible = false;//Assigned Pick Date...
                        e.Row.Cells[14].Visible = false;//Start Pick Time...
                        e.Row.Cells[15].Visible = false;//End Pick Time...
                        e.Row.Cells[16].Visible = false;//Standard Pick Time...
                        e.Row.Cells[17].Visible = false;//Standard Cases Per Minute...
                        e.Row.Cells[18].Visible = false;//Actual Picking Time...
                        e.Row.Cells[19].Visible = false;//Actual Cases Per Minute...
                        e.Row.Cells[20].Visible = false;//Cased Picked...
                        e.Row.Cells[21].Visible = false;//Projected Finish Time...                        
                        e.Row.Cells[31].Visible = false;//Actual Delivery Date...
                        e.Row.Cells[32].Visible = false;//Lead Times Icon...
                        e.Row.Cells[33].Visible = false;//Appointment Date...
                        e.Row.Cells[34].Visible = false;//Appointment Time...
                        e.Row.Cells[35].Visible = false;//Actual Ship Via...                        
                    }
                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    //new 7-7-2021...
                    Label lblSpecialInstrs = (Label)e.Row.FindControl("lblSpecialInstrs");
                    HyperLink imgViewSpecialInstrs = (HyperLink)e.Row.FindControl("imgViewSpecialInstrs");
                    if (lblSpecialInstrs.Text.Trim() == "")
                    {
                        imgViewSpecialInstrs.Visible = false;
                    }


                    Label lblSalesOrder = (Label)e.Row.FindControl("lblSalesOrder");//First Always...
                    LinkButton lbnSalesOrder = (LinkButton)e.Row.FindControl("lbnSalesOrder");
                    lbnSalesOrder.Text = int.Parse(lbnSalesOrder.Text).ToString();

                    CheckBox chkScanned = (CheckBox)e.Row.FindControl("chkScanned");
                    //chkScanned.Attributes.Add("onclick", "if (!confirm('Are you sure you want to mark this order as Scanned? This cannot be undone!')) return false;");

                    Label lblScanned = (Label)e.Row.FindControl("lblScanned");
                    TextBox txtStaged = (TextBox)e.Row.FindControl("txtStaged");

                    Label lblTotalValue = (Label)e.Row.FindControl("lblTotalValue");
                    if (lblTotalValue.Text != "")
                    {
                        dcTotalValue = Convert.ToDecimal(lblTotalValue.Text.Replace("$", ""));
                        dcGrandTotal += dcTotalValue;
                        lblTotalValue.Text = "$" + lblTotalValue.Text;
                    }

                    Label lblOrderDate = (Label)e.Row.FindControl("lblOrderDate");
                    Label lblShipDate = (Label)e.Row.FindControl("lblShipDate");
                    Label lblCustomerDeliveryDate = (Label)e.Row.FindControl("lblCustomerDeliveryDate");
                    if (lblOrderDate.Text != "")
                    {
                        lblOrderDate.ToolTip = Convert.ToDateTime(lblOrderDate.Text).ToShortTimeString();
                        lblOrderDate.Text = Convert.ToDateTime(lblOrderDate.Text).ToShortDateString();
                        lblOrderDate.Style.Add("cursor", "pointer");
                    }
                    if (lblShipDate.Text != "")
                    {
                        lblShipDate.Text = Convert.ToDateTime(lblShipDate.Text).ToShortDateString();
                    }
                    if (lblCustomerDeliveryDate.Text != "")
                    {
                        lblCustomerDeliveryDate.Text = Convert.ToDateTime(lblCustomerDeliveryDate.Text).ToShortDateString();
                    }

                    System.Web.UI.WebControls.Image imgDeliveryInfoScheduled = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgDeliveryInfoScheduled");
                    System.Web.UI.WebControls.Image imgDeliveryInfoUnScheduled = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgDeliveryInfoUnScheduled");
                    System.Web.UI.WebControls.Image imgFlaggedPO = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgFlaggedPO");


                    DropDownList ddlPicker = (DropDownList)e.Row.FindControl("ddlPicker");
                    Label lblPickerUserID = (Label)e.Row.FindControl("lblPickerUserID");
                    LoadPickers(ddlPicker);
                    if (lblPickerUserID.Text != "")
                    {
                        ddlPicker.SelectedValue = lblPickerUserID.Text;
                    }


                    Label lblDefaultShipVia = (Label)e.Row.FindControl("lblDefaultShipVia");//23
                    TextBox txtActualDeliveryDate = (TextBox)e.Row.FindControl("txtActualDeliveryDate");//28
                    Label lblAppointmentTime = (Label)e.Row.FindControl("lblAppointmentTime");//229
                    DropDownList ddlShipVia = (DropDownList)e.Row.FindControl("ddlShipVia");//30
                    Label lblActualShipViaID = (Label)e.Row.FindControl("lblActualShipViaID");
                    LoadShipVia(ddlShipVia);
                    if (lblActualShipViaID.Text != "")
                    {
                        ddlShipVia.SelectedValue = lblActualShipViaID.Text;
                    }


                    if (lblAppointmentTime.Text != "")
                    {
                        lblAppointmentTime.Text = Convert.ToDateTime(lblAppointmentTime.Text).ToString("");
                    }
                    ////Updated 12-25-2019...
                    if (txtActualDeliveryDate.Text != "" || lblAppointmentTime.Text != "")//Change to Delivery Info Complete?
                    {
                        imgDeliveryInfoUnScheduled.Visible = false;
                        imgDeliveryInfoScheduled.Visible = true;
                        if (txtActualDeliveryDate.Text != "")
                        {
                            imgDeliveryInfoScheduled.ToolTip = "Actual Ship Date: - " + txtActualDeliveryDate.Text.Trim();
                        }
                        else
                        {
                            imgDeliveryInfoScheduled.ToolTip = "Scheduled Delivery Date: - " + lblAppointmentTime.Text;
                        }

                    }
                    else
                    {
                        imgDeliveryInfoUnScheduled.Visible = true;
                        imgDeliveryInfoScheduled.Visible = false;
                    }


                    DropDownList ddlStartMinutes = (DropDownList)e.Row.FindControl("ddlStartMinutes");
                    DropDownList ddlStartHours = (DropDownList)e.Row.FindControl("ddlStartHours");
                    Label lblStartPickTime = (Label)e.Row.FindControl("lblStartPickTime");

                    if (lblStartPickTime.Text != "")
                    {
                        DateTime dt = DateTime.Parse(lblStartPickTime.Text);
                        string sStandardHour = dt.ToString("HH");
                        ddlStartHours.SelectedValue = sStandardHour;
                        ddlStartMinutes.SelectedValue = dt.ToString("mm");
                    }


                    DropDownList ddlEndMinutes = (DropDownList)e.Row.FindControl("ddlEndMinutes");
                    DropDownList ddlEndHours = (DropDownList)e.Row.FindControl("ddlEndHours");
                    Label lblEndPickTime = (Label)e.Row.FindControl("lblEndPickTime");

                    if (lblEndPickTime.Text != "")
                    {
                        DateTime dt = DateTime.Parse(lblEndPickTime.Text);
                        string sStandardHour = dt.ToString("HH");
                        ddlEndHours.SelectedValue = sStandardHour;
                        ddlEndMinutes.SelectedValue = dt.ToString("mm");
                    }

                    DropDownList ddlAppointmentTimeMinutes = (DropDownList)e.Row.FindControl("ddlAppointmentTimeMinutes");
                    DropDownList ddlAppointmentTimeHours = (DropDownList)e.Row.FindControl("ddlAppointmentTimeHours");


                    if (lblAppointmentTime.Text != "")
                    {
                        DateTime dt = DateTime.Parse(lblAppointmentTime.Text);
                        string sStandardHour = dt.ToString("HH");
                        ddlAppointmentTimeHours.SelectedValue = sStandardHour;
                        ddlAppointmentTimeMinutes.SelectedValue = dt.ToString("mm");
                    }

                    Label lblActualCasesPerMinutes = (Label)e.Row.FindControl("lblActualCasesPerMinutes");
                    if (lblActualCasesPerMinutes.Text != "")
                    {
                        lblActualCasesPerMinutes.Text = Convert.ToDecimal(lblActualCasesPerMinutes.Text).ToString("0.00");
                    }


                    Label lblAck = (Label)e.Row.FindControl("lblAck");
                    Label lblAckDateTime = (Label)e.Row.FindControl("lblAckDateTime");
                    System.Web.UI.WebControls.Image imgAck = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgAck");
                    if (lblAck.Text.Trim() != "")
                    {
                        if (lblAck.Text.Trim() == "Y")
                        {//Show Order Ack if value is 1...
                            imgAck.Visible = true;
                            imgAck.ToolTip = "Sales Order Confirmed " + lblAckDateTime.Text;
                        }
                        else
                        {
                            imgAck.Visible = false;
                        }
                    }
                    else
                    {
                        imgAck.Visible = false;
                    }


                    GridView gvComments = (GridView)e.Row.FindControl("gvComments");
                    DataTable dtComments = new DataTable();
                    dtComments = SharedFunctions.GetSaleOrderComments(Convert.ToInt32(lbnSalesOrder.Text.Trim()));
                    gvComments.DataSource = dtComments;
                    gvComments.DataBind();
                    System.Web.UI.WebControls.Image imgNotes = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgNotes");

                    Label lblDept = (Label)e.Row.FindControl("lblDept");
                    Panel pnlNotes = (Panel)e.Row.FindControl("pnlNotes");
                    if (dtComments.Rows.Count > 0)
                    {
                        pnlNotes.Visible = true;
                        imgNotes.Visible = true;
                        if (lblDept.Text != "")
                        {
                            switch (lblDept.Text)
                            {
                                case "C"://Customer Service...
                                    imgNotes.ImageUrl = "images/InfoCustomerService.png";
                                    break;
                                case "L"://Logistics...
                                    imgNotes.ImageUrl = "images/InfoLogistics.png";
                                    break;
                                case "O"://Operations...
                                    imgNotes.ImageUrl = "images/InfoOrders.png";
                                    break;
                                case "P"://Production...
                                    imgNotes.ImageUrl = "images/InfoProduction.png";
                                    break;
                                case "Q"://Quality Control...
                                    imgNotes.ImageUrl = "images/InfoQualityControl.png";
                                    break;
                                case "S"://System...
                                    imgNotes.ImageUrl = "images/InfoSystem.png";
                                    break;
                            }
                        }
                    }
                    else
                    {
                        pnlNotes.Visible = false;
                        imgNotes.Visible = false;
                    }



                    System.Web.UI.WebControls.Image imgLeadTimes = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgLeadTimes");
                    Label lblContactType = (Label)e.Row.FindControl("lblContactType");
                    Label lblLeadTimeValue = (Label)e.Row.FindControl("lblLeadTimeValue");
                    Label lblLeadTimeValueType = (Label)e.Row.FindControl("lblLeadTimeValueType");
                    Label lblLeadTimeSource = (Label)e.Row.FindControl("lblLeadTimeSource");
                    HyperLink hlAppointmentInfo = (HyperLink)e.Row.FindControl("hlAppointmentInfo");
                    Panel pnlLeadTimes = (Panel)e.Row.FindControl("pnlLeadTimes");
                    Label lblAddressCode = (Label)e.Row.FindControl("lblAddressCode");

                    if (lblContactType.Text != "")
                    {
                        pnlLeadTimes.Visible = true;
                        imgLeadTimes.Visible = true;
                        pnlLeadTimes.Width = Unit.Pixel(125);
                        if (lblCustomerDeliveryDate.Text != "")
                        {
                            bool bWithinWindow = false;
                            DateTime dtCustomerDeliveryDate = Convert.ToDateTime(lblCustomerDeliveryDate.Text);
                            int iLeadTimeValue = 0;
                            switch (lblLeadTimeValueType.Text)
                            {
                                case "DAYS":
                                    iLeadTimeValue = Convert.ToInt32(lblLeadTimeValue.Text);
                                    break;
                                case "WEEKS":
                                    iLeadTimeValue = Convert.ToInt32(lblLeadTimeValue.Text);
                                    iLeadTimeValue = iLeadTimeValue * 7;
                                    break;
                                case "MONTHS":
                                    iLeadTimeValue = Convert.ToInt32(lblLeadTimeValue.Text);
                                    int iDaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                                    iLeadTimeValue = iLeadTimeValue * iDaysInMonth;
                                    break;

                            }

                            DateTime dtCustomerDeliveryDateLeadtime = dtCustomerDeliveryDate.AddDays(-iLeadTimeValue); //i.e. 3/1/2020 - 2 days = 2/27/2020

                            int iDateDiff = SqlMethods.DateDiffDay(DateTime.Now, dtCustomerDeliveryDateLeadtime);
                            //Could be a negative number the delivery date passes...
                            if (iDateDiff < 1 && ddlAppointmentTimeHours.SelectedIndex == 0)//Your within the time range and there is no appointment set...
                            {
                                bWithinWindow = true;
                            }
                            if (bWithinWindow)
                            {
                                //imgLeadTimes.Style.Add("border", " 2px solid red");
                                e.Row.Cells[29].BackColor = Color.Red;
                            }
                            else
                            {
                                //imgLeadTimes.Style.Remove("border");
                                e.Row.Cells[29].BackColor = Color.Transparent;
                            }

                            switch (lblContactType.Text)
                            {
                                case "EMAIL":
                                    hlAppointmentInfo.NavigateUrl = "mailto:" + lblLeadTimeSource.Text + "?subject=Appointment";
                                    hlAppointmentInfo.Text = "EMAIL";
                                    hlAppointmentInfo.ToolTip = "Click to Launch Email Client (" + lblLeadTimeSource.Text + ")";
                                    break;
                                case "WEBSITE":
                                    hlAppointmentInfo.NavigateUrl = "http://" + lblLeadTimeSource.Text;
                                    hlAppointmentInfo.Text = "WEBSITE";
                                    hlAppointmentInfo.ToolTip = "Click to go to Website: " + lblLeadTimeSource.Text;
                                    hlAppointmentInfo.Target = "_Blank";
                                    break;
                                case "PHONE":
                                    hlAppointmentInfo.NavigateUrl = "";
                                    hlAppointmentInfo.ToolTip = "Call this Phone Number to set an Appointment";
                                    hlAppointmentInfo.Text = SharedFunctions.GetPhoneFormat(lblLeadTimeSource.Text);
                                    break;
                            }


                            lblLeadTimeValueType.Text = "CODE: " + lblAddressCode.Text + "<br>" + lblLeadTimeValue.Text + " " + lblLeadTimeValueType.Text + "<br/> NOTICE <br />(" + dtCustomerDeliveryDateLeadtime.ToShortDateString() + ")";
                            lblLeadTimeValue.Visible = false;

                        }

                    }
                    else
                    {
                        pnlLeadTimes.Visible = false;
                        imgLeadTimes.Visible = false;
                    }
                    //For Testing...
                    ////if (Convert.ToInt32(lblSalesOrder.Text) == 57244)
                    ////{
                    ////    Debug.WriteLine(lblShipDate.Text);
                    ////}


                    Label lblShortage = (Label)e.Row.FindControl("lblShortage");
                    Label lblOrderStatus = (Label)e.Row.FindControl("lblOrderStatus");
                    if (lblOrderStatus.Text.ToUpper().Contains("SUSPENDED"))
                    {
                        e.Row.BackColor = Color.Orange;
                        e.Row.ForeColor = Color.Black;
                    }
                    else
                    {
                        if (lblShortage.Text != "")
                        {
                            if (lblShortage.Text == "Y")
                            {
                                if (lblShipDate.Text != "")
                                {
                                    DateTime dtShipDate = Convert.ToDateTime(lblShipDate.Text);
                                    DateTime? dtMaxProductioDate = SharedFunctions.GetMaxProductionDateBySalesOrderNew(Convert.ToInt32(lblSalesOrder.Text));
                                    bool bContainsTBDs = false;
                                    bContainsTBDs = SharedFunctions.DoesOrderContainTBDs(Convert.ToInt32(lblSalesOrder.Text));
                                    if (dtMaxProductioDate != null)
                                    {
                                        dtMaxProductioDate = ((DateTime)dtMaxProductioDate);
                                        //TODO It also Must not contain TBDs!!!!

                                        if ((dtShipDate >= dtMaxProductioDate) && bContainsTBDs == false)
                                        {
                                            e.Row.BackColor = Color.LemonChiffon;
                                            e.Row.ForeColor = Color.Black;
                                        }
                                        else
                                        {
                                            if (bContainsTBDs)
                                            {
                                                e.Row.BackColor = Color.LightBlue;
                                                e.Row.ForeColor = Color.Black;
                                            }
                                            else
                                            {
                                                e.Row.BackColor = Color.Pink;
                                                e.Row.ForeColor = Color.Black;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (bContainsTBDs)
                                        {
                                            e.Row.BackColor = Color.LightBlue;
                                            e.Row.ForeColor = Color.Black;
                                        }
                                        else
                                        {
                                            e.Row.BackColor = Color.Pink;
                                            e.Row.ForeColor = Color.Black;
                                        }
                                    }
                                }
                                else//Every single order has a ship date...
                                {
                                    e.Row.BackColor = Color.Pink;
                                    e.Row.ForeColor = Color.Black;
                                }
                            }
                            else
                            {
                                e.Row.BackColor = Color.LightGreen;
                                e.Row.ForeColor = Color.Black;
                            }
                        }
                        else
                        {
                            e.Row.BackColor = Color.LightGreen;
                            e.Row.ForeColor = Color.Black;
                        }
                    }
                    Label lblFirstOperator = (Label)e.Row.FindControl("lblFirstOperator");
                    Label lblLastOperator = (Label)e.Row.FindControl("lblLastOperator");

                    if (lblLastOperator.Text != "")
                    {
                        if (lblLastOperator.Text != lblFirstOperator.Text)
                        {
                            if (lblFirstOperator.Text != "")
                            {
                                lblFirstOperator.Text = lblFirstOperator.Text + " <span style='color:black'>" + lblLastOperator.Text + "</span>";
                                lblFirstOperator.ToolTip = "First and Last Operator";
                            }
                            else
                            {
                                lblFirstOperator.Text = "<span style='color:black'>" + lblLastOperator.Text + "</span>";
                                lblFirstOperator.ToolTip = "Last Operator";
                            }
                        }
                        else//No Last Operator
                        {
                            lblFirstOperator.ToolTip = "First Operator";
                        }
                    }
                    else
                    {
                        lblFirstOperator.ToolTip = "First Operator";
                    }
                    lblFirstOperator.Style.Add("cursor", "pointer");


                    //11-14-2019...
                    CheckBox chkCreditHold = (CheckBox)e.Row.FindControl("chkCreditHold");
                    //chkCreditHold.Attributes.Add("onclick", "if (!confirm('Are you sure you want to mark this order as On or Off Credit Hold?')) return false;");

                    Label lblCreditHold = (Label)e.Row.FindControl("lblCreditHold");

                    if (lblCreditHold.Text != "")
                    {
                        chkCreditHold.Checked = true;
                        chkCreditHold.ToolTip = Convert.ToDateTime(lblCreditHold.Text).ToString();
                        e.Row.BackColor = Color.Red;//Will change color must be after all other color changes...
                    }
                    else
                    {
                        chkCreditHold.Checked = false;
                    }

                    int iRoleID = Convert.ToInt32(Session["RoleID"]);

                    if (iRoleID == 1 || iRoleID == 8)//Admin or Credit manager...
                    {
                        chkCreditHold.Enabled = true;
                    }
                    else
                    {
                        chkCreditHold.Enabled = false;
                    }

                    Label lblProjectedFinishTime = (Label)e.Row.FindControl("lblProjectedFinishTime");
                    if (lblProjectedFinishTime.Text != "")
                    {
                        lblProjectedFinishTime.Text = Convert.ToDateTime(lblProjectedFinishTime.Text).ToString("HH:mm:ss"); //To Military Time...
                    }


                    bool bCompton = SharedFunctions.IsCompton(iUserID);

                    if (bCompton || rblCompton.SelectedIndex == 1)//Is compton employee or admnin set page to compton view...
                    {

                        if (!bScanned)
                        {
                            chkScanned.Enabled = false;
                            chkScanned.BackColor = Color.LightGray;
                        }
                        else
                        {
                            if (chkScanned.Checked)
                            {
                                if (!bAdvancedScanned)
                                {//Can't uncheck a checked scan...
                                    chkScanned.Enabled = false;
                                    chkScanned.BackColor = Color.LightGray;
                                }
                                else
                                {
                                    chkScanned.Enabled = true;
                                    chkScanned.BackColor = Color.LemonChiffon;
                                }
                            }
                            else//Not checked...
                            {
                                chkScanned.Enabled = true;
                                chkScanned.BackColor = Color.LemonChiffon;
                            }
                        }

                        if (!bStaged)
                        {
                            txtStaged.Enabled = false;
                            txtStaged.BackColor = Color.LightGray;
                        }
                        else
                        {
                            txtStaged.Enabled = true;
                            txtStaged.BackColor = Color.LemonChiffon;
                        }
                        e.Row.Cells[5].Visible = false;//Acknowledged
                        e.Row.Cells[25].Visible = false;//Appointment Date Time Label...
                        //e.Row.Cells[33].Visible = false;//UnScheduled Delivery...
                        //e.Row.Cells[34].Visible = false;//Scheduled Delivery...   
                    }
                    else//Not Compton View...
                    {
                        chkScanned.Enabled = false;
                        txtStaged.Enabled = false;
                        e.Row.Cells[12].Visible = false;//Assigned Picker...
                        e.Row.Cells[13].Visible = false;//Assigned Pick Date...
                        e.Row.Cells[14].Visible = false;//Start Pick Time...
                        e.Row.Cells[15].Visible = false;//End Pick Time...
                        e.Row.Cells[16].Visible = false;//Standard Pick Time...
                        e.Row.Cells[17].Visible = false;//Standard Cases Per Minute...
                        e.Row.Cells[18].Visible = false;//Actual Picking Time...
                        e.Row.Cells[19].Visible = false;//Actual Cases Per Minute...
                        e.Row.Cells[20].Visible = false;//Cased Picked...
                        e.Row.Cells[21].Visible = false;//Projected Finish Time...
                        e.Row.Cells[31].Visible = false;//Actual Delivery Date...
                        e.Row.Cells[32].Visible = false;//Lead Times Icon...
                        e.Row.Cells[33].Visible = false;//Appointment Date...
                        e.Row.Cells[34].Visible = false;//Appointment Time...
                        e.Row.Cells[35].Visible = false;//Actual Ship Via...                       
                    }
                    if (lblScanned.Text != "")
                    {
                        chkScanned.Checked = true;
                        chkScanned.ToolTip = Convert.ToDateTime(lblScanned.Text).ToShortDateString();
                        if (chkScanned.Checked)
                        {
                            if (!bAdvancedScanned)
                            {//Can't uncheck a checked scan...
                                chkScanned.Enabled = false;
                                chkScanned.BackColor = Color.LightGray;
                            }
                            else
                            {
                                chkScanned.Enabled = true;
                                chkScanned.BackColor = Color.LemonChiffon;
                            }
                        }
                    }
                    else
                    {
                        chkScanned.Checked = false;
                    }

                    //New 6-24-2020...
                    GridView gvTotalQtyBreakdown = (GridView)e.Row.FindControl("gvTotalQtyBreakdown");
                    DataTable dtTotalQtyBreakdown = new DataTable();
                    dtTotalQtyBreakdown = SharedFunctions.GetTotalQtyBreakdown(Convert.ToInt32(lbnSalesOrder.Text.Trim()));
                    gvTotalQtyBreakdown.DataSource = dtTotalQtyBreakdown;
                    gvTotalQtyBreakdown.DataBind();
                    dtTotalQtyBreakdown.Dispose();
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {


                    bool bCompton = SharedFunctions.IsCompton(iUserID);
                    if (bCompton || rblCompton.SelectedIndex == 1)//Is compton employee or admnin set page to compton view...
                    {
                        e.Row.Cells[5].Visible = false;//Acknowledged                        
                        e.Row.Cells[25].Visible = false;//Appointment Date Time Label...
                        //e.Row.Cells[33].Visible = false;//UnScheduled Delivery...
                        //e.Row.Cells[34].Visible = false;//Scheduled Delivery...   
                    }
                    else//Not Compton View...
                    {
                        e.Row.Cells[12].Visible = false;//Assigned Picker...
                        e.Row.Cells[13].Visible = false;//Assigned Pick Date...
                        e.Row.Cells[14].Visible = false;//Start Pick Time...
                        e.Row.Cells[15].Visible = false;//End Pick Time...
                        e.Row.Cells[16].Visible = false;//Standard Pick Time...
                        e.Row.Cells[17].Visible = false;//Standard Cases Per Minute...
                        e.Row.Cells[18].Visible = false;//Actual Picking Time...
                        e.Row.Cells[19].Visible = false;//Actual Cases Per Minute...
                        e.Row.Cells[20].Visible = false;//Cased Picked...
                        e.Row.Cells[21].Visible = false;//Projected Finish Time...
                        e.Row.Cells[31].Visible = false;//Actual Delivery Date...
                        e.Row.Cells[32].Visible = false;//Lead Times Icon...
                        e.Row.Cells[33].Visible = false;//Appointment Date...
                        e.Row.Cells[34].Visible = false;//Appointment Time...
                        e.Row.Cells[35].Visible = false;//Actual Ship Via...                          
                    }



                    Label lblGrandTotal = (Label)e.Row.FindControl("lblGrandTotal");
                    lblGrandTotal.Text = "$" + dcGrandTotal.ToString("#,0.00");
                    lblGrandTotalSummary.Text = "Grand Total of Open Orders in Main Grid Below: $" + dcGrandTotal.ToString("#,0.00");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    protected void gvRecord_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblErrorNotes.Text = "";

        int idx = 0;
        string sSalesOrder = "";
        DataTable dt = new DataTable();
        LinkButton lbnSalesOrder;
        Label lblPurchaseOrder;
        Label lblSalesOrder;
        Label lblQueryStatus;
        Label lblSalesPersonID;
        Label lblCustomer;
        Label lblCustomerID;
        Label lblShipDate;
        switch (e.CommandName)
        {

            case "ViewDetails":
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...
                lblPurchaseOrder = (Label)gvRecord.Rows[idx].FindControl("lblPurchaseOrder");
                lbnSalesOrder = (LinkButton)gvRecord.Rows[idx].FindControl("lbnSalesOrder");
                lblSalesOrder = (Label)gvRecord.Rows[idx].FindControl("lblSalesOrder");
                lblQueryStatus = (Label)gvRecord.Rows[idx].FindControl("lblQueryStatus");
                lblSalesPersonID = (Label)gvRecord.Rows[idx].FindControl("lblSalesPersonID");
                lblCustomer = (Label)gvRecord.Rows[idx].FindControl("lblCustomer");
                lblCustomerID = (Label)gvRecord.Rows[idx].FindControl("lblCustomerID");
                lblShipDate = (Label)gvRecord.Rows[idx].FindControl("lblShipDate");
                sSalesOrder = lblSalesOrder.Text;
                //New Comments...2016
                lblSaleOrderDetails.Text = lbnSalesOrder.Text;
                lblSaleOrderDetailsHidden.Text = lblSalesOrder.Text;
                txtSaleOrderComment.Text = "";
                lblSalespersonUserID.Text = lblSalesPersonID.Text;
                lblCustomerName.Text = lblCustomer.Text;
                lblCustomerNumber.Text = lblCustomerID.Text;
                lblFelbroShipDateDetails.Text = lblShipDate.Text;
                lblPO.Text = lblPurchaseOrder.Text;
                //Rebind gvDash...
                GetDetails(sSalesOrder, gvDetails);
                GetComments(sSalesOrder);
                if (gvDetails.Rows.Count == 0)
                {
                    lblQueryStatus.Text = "No items found";
                }
                else
                {
                    lblQueryStatus.Text = "";
                }
                Timer1.Enabled = false;//Turn Off...

                AddGroupMembersToDeptCheckBoxes();

                ModalPopupExtenderPopUp.Show();
                break;
        }

    }
    protected void gvRecord_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvRecord.Rows)
        {
            if (row.RowIndex == gvRecord.SelectedIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("Red");
            }
            else
            {
                Label lblShortage = (Label)row.FindControl("lblShortage");
                if (lblShortage.Text != "")
                {
                    if (lblShortage.Text == "Y")
                    {
                        row.BackColor = Color.Pink;
                        row.ForeColor = Color.Black;
                    }
                }
                else
                {
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                }
            }


        }
    }
    protected void gvRecord_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtRecordSalesOrderTracker"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvRecord.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvRecord.DataSource = m_DataView;
            gvRecord.DataBind();
            gvRecord.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
    }
    protected void gvDetails_Sorting(object sender, GridViewSortEventArgs e)
    {
        GridView gvDetails = (GridView)sender;
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtDetails"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvDetails.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvDetails.DataSource = m_DataView;
            gvDetails.DataBind();
            gvDetails.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
        ModalPopupExtenderPopUp.Show();
    }
    protected void gvDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        decimal dcPrice = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblSalesOrder = (Label)e.Row.FindControl("lblSalesOrder");


            Label lblSalesOrderHidden = (Label)e.Row.FindControl("lblSalesOrderHidden");
            Label lblSalesOrderNotes = (Label)e.Row.FindControl("lblSalesOrderNotes");
            Panel pnlNotes0 = (Panel)e.Row.FindControl("pnlNotes0");
            if (lblSalesOrderNotes.Text != "")
            {
                pnlNotes0.Visible = true;
                lblSalesOrder.Style.Add("cursor", "pointer");
            }
            else
            {
                pnlNotes0.Visible = false;
            }

            //Must be last!!!
            string sSource = lblSalesOrder.Text;
            var sResult = int.Parse(sSource).ToString();
            lblSalesOrder.Text = sResult;

            System.Web.UI.WebControls.Image imgNotes2 = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgNotes2");
            Panel pnlNotes2 = (Panel)e.Row.FindControl("pnlNotes2");
            Label lblNotes = (Label)e.Row.FindControl("lblNotes");
            if (lblNotes.Text != "")
            {
                pnlNotes2.Visible = true;
                imgNotes2.Visible = true;
                imgNotes2.Style.Add("cursor", "pointer");
            }
            else
            {
                pnlNotes2.Visible = false;
                imgNotes2.Visible = false;
            }

            HyperLink imgViewSpecialInstrs = (HyperLink)e.Row.FindControl("imgViewSpecialInstrs");
            HyperLink imgViewShippingInstrs = (HyperLink)e.Row.FindControl("imgViewShippingInstrs");

            Label lblSpecialInstrs = (Label)e.Row.FindControl("lblSpecialInstrs");
            Label lblShippingInstrs = (Label)e.Row.FindControl("lblShippingInstrs");
            Label lblExtendedPrice = (Label)e.Row.FindControl("lblExtendedPrice");
            Label lblPrice = (Label)e.Row.FindControl("lblPrice");
            if (lblPrice.Text != "")
            {
                lblPrice.Text = "$" + lblPrice.Text;
            }
            if (lblExtendedPrice.Text != "")
            {
                dcPrice = Convert.ToDecimal(lblExtendedPrice.Text.Replace("$", ""));
                dcLineTotal += dcPrice;
                lblExtendedPrice.Text = "$" + lblExtendedPrice.Text;
            }

            if (lblSpecialInstrs.Text == "")
            {
                imgViewSpecialInstrs.Visible = false;
            }
            if (lblShippingInstrs.Text == "")
            {
                imgViewShippingInstrs.Visible = false;
            }

            Label lblShortage = (Label)e.Row.FindControl("lblShortage");
            Label lblStockCode = (Label)e.Row.FindControl("lblStockCode");
            Label lblLineItemOrderStatus = (Label)e.Row.FindControl("lblLineItemOrderStatus");
            Label lblOrderStatus = (Label)e.Row.FindControl("lblOrderStatus");
            //Shortage Data show now comes from SorReadyStatusAlerts table...7-31-2019...
            if (lblShortage.Text != "")
            {
                if (lblShortage.Text == "Y")
                {
                    lblStockCode.ForeColor = Color.Red;
                    lblStockCode.ToolTip = "SHORTAGE";
                    lblStockCode.Style.Add("cursor", "pointer");
                    if (SharedFunctions.IsDate(lblLineItemOrderStatus.Text))
                    {
                        lblLineItemOrderStatus.Text = Convert.ToDateTime(lblLineItemOrderStatus.Text).ToShortDateString();
                        lblLineItemOrderStatus.ToolTip = Convert.ToDateTime(lblLineItemOrderStatus.Text).DayOfWeek.ToString();
                        lblLineItemOrderStatus.Style.Add("Cursor", "pointer");
                        lblLineItemOrderStatus.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblLineItemOrderStatus.Text = "TBD";
                        lblLineItemOrderStatus.ForeColor = Color.Red;
                    }
                }
                else//No Shortage...
                {
                    lblLineItemOrderStatus.Text = "READY";
                    lblLineItemOrderStatus.ForeColor = Color.Green;
                }
            }
            if (lblOrderStatus.Text.ToUpper() == "SUSPENDED")
            {
                lblLineItemOrderStatus.Text = "SUSPENDED";
                lblLineItemOrderStatus.ForeColor = Color.Orange;
            }
            if (lblStockCode.Text.Trim() != null)
            {
                GridView gvProductionSchedule = (GridView)e.Row.FindControl("gvProductionSchedule");
                DataTable dtProductionSchedule = new DataTable();
                if (chkLoadSchedules.Checked)
                {
                    dtProductionSchedule = SharedFunctions.GetProductionScheduleUnGrouped(lblStockCode.Text.Trim());
                    gvProductionSchedule.DataSource = dtProductionSchedule;
                    gvProductionSchedule.DataBind();
                    dtProductionSchedule.Dispose();
                }
                else
                {
                    gvProductionSchedule.DataSource = null;
                    gvProductionSchedule.DataBind();
                }

                HyperLink lbnNotes = (HyperLink)e.Row.FindControl("lbnNotes");

                Panel pnlNotes = (Panel)e.Row.FindControl("pnlNotes");
                if (dtProductionSchedule.Rows.Count != 0)
                {
                    pnlNotes.Visible = true;
                    lbnNotes.Visible = true;
                    lbnNotes.Style.Add("cursor", "pointer");
                }
                else
                {
                    pnlNotes.Visible = false;
                    lbnNotes.Visible = false;
                }


                HyperLink lbnMatrix = (HyperLink)e.Row.FindControl("lbnMatrix");
                Panel pnlMatrix = (Panel)e.Row.FindControl("pnlMatrix");
                string[] ExcludedStockCodes = new string[] { "704033", "704056", "704055" };//no Pallets...  
                if (lblStockCode.Text.Trim() != "MISC" && !ExcludedStockCodes.Contains(lblStockCode.Text.Trim()))
                {

                    GridView gvProductionMatrix = (GridView)e.Row.FindControl("gvProductionMatrix");
                    DataTable dtProductionMatrix = new DataTable();
                    if (chkLoadMatrixes.Checked)
                    {
                        dtProductionMatrix = GetProductionMatrix(lblSalesOrderHidden.Text.Trim(), lblStockCode.Text.Trim());
                        if (dtProductionMatrix.Rows.Count > 0)
                        {
                            gvProductionMatrix.DataSource = dtProductionMatrix;
                            gvProductionMatrix.DataBind();
                            if (dtProductionMatrix.Rows.Count != 0)
                            {
                                pnlMatrix.Visible = true;
                                lbnMatrix.Visible = true;
                                lbnMatrix.Style.Add("cursor", "pointer");
                            }
                            else
                            {
                                pnlMatrix.Visible = false;
                                lbnMatrix.Visible = false;
                            }
                            dtProductionMatrix.Dispose();
                        }
                        else
                        {
                            pnlMatrix.Visible = false;
                            lbnMatrix.Visible = false;
                        }
                        dtProductionMatrix.Dispose();

                    }
                    else//Dont Run...
                    {
                        gvProductionMatrix.DataSource = null;
                        gvProductionMatrix.DataBind();
                        pnlMatrix.Visible = false;
                        lbnMatrix.Visible = false;
                    }
                }
                else
                {
                    pnlMatrix.Visible = false;
                    lbnMatrix.Visible = false;
                }
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblPriceTotal = (Label)e.Row.FindControl("lblPriceTotal");
            lblPriceTotal.Text = "$" + dcLineTotal.ToString("#,0.00");
        }
    }
    protected void gvProductionMatrix_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblSelectedOrder = (Label)e.Row.FindControl("lblSelectedOrder");
            Label lblOrderDate = (Label)e.Row.FindControl("lblOrderDate");
            Label lblScheduledDate = (Label)e.Row.FindControl("lblScheduledDate");
            Label lblSalesOrder = (Label)e.Row.FindControl("lblSalesOrder");
            //Must be last!!!
            string sSource = lblSalesOrder.Text;
            var sResult = int.Parse(sSource).ToString();
            lblSalesOrder.Text = sResult;

            if (lblScheduledDate.Text != "")
            {
                lblScheduledDate.Text = Convert.ToDateTime(lblScheduledDate.Text).ToShortDateString();
                lblScheduledDate.ToolTip = Convert.ToDateTime(lblScheduledDate.Text).DayOfWeek.ToString();
                lblScheduledDate.Style.Add("Cursor", "pointer");
            }
            if (lblOrderDate.Text != "")
            {
                lblOrderDate.Text = Convert.ToDateTime(lblOrderDate.Text).ToShortDateString();
                lblOrderDate.ToolTip = Convert.ToDateTime(lblOrderDate.Text).DayOfWeek.ToString();
                lblOrderDate.Style.Add("Cursor", "pointer");
            }
            if (lblSelectedOrder.Text != "")
            {
                if (lblSelectedOrder.Text == "Y")
                {
                    e.Row.BackColor = Color.LemonChiffon;
                }
            }

        }

    }
    protected void ddlDisplayCount_SelectedIndexChanged(object sender, EventArgs e)
    {
        RunSearch();

    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        Timer1.Interval = Convert.ToInt32(ddlInterval.SelectedValue);
        DataTable dt = new DataTable();

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
        }
        switch (TickNumber)
        {
            case 1:
                RunSearch();

                GetSummary(iRoleID, iUserID);
                LoadPlaceOrderData(iRoleID, iUserID);
                LoadTopFifteenData(rblTopTen.SelectedValue);
                lblToday.Text = DateTime.Now.ToShortDateString();
                lblYesterday.Text = DateTime.Now.AddDays(-1).ToShortDateString();
                lblThisMonth.Text = DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture) + " " + DateTime.Now.Year.ToString();

                Debug.WriteLine("Full Insert: " + TickNumber);
                lblTimeUpdated.Text = "Data Last Updated: " + DateTime.Now.ToString();
                TickNumber = 0;//Reset counter...
                break;
            default:
                Debug.WriteLine("Just Display: " + TickNumber);
                RunSearch();

                GetSummary(iRoleID, iUserID);
                LoadPlaceOrderData(iRoleID, iUserID);
                LoadTopFifteenData(rblTopTen.SelectedValue);
                lblToday.Text = DateTime.Now.ToShortDateString();
                lblYesterday.Text = DateTime.Now.AddDays(-1).ToShortDateString();
                lblThisMonth.Text = DateTime.Now.ToString("MMMM", System.Globalization.CultureInfo.InvariantCulture) + " " + DateTime.Now.Year.ToString();
                lblTimeUpdated.Text = "Data Last Updated: " + DateTime.Now.ToString();
                break;
        }

        TickNumber++;

        //TODO reset the TickNumber to 0 after a while

        if (TickNumber == 9999)
        {
            TickNumber = 0;
        }
        dt.Dispose();
    }
    protected void chkTimerOnOff_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTimerOnOff.Checked)
        {
            Timer1.Enabled = true;
            Timer1.Interval = Convert.ToInt32(ddlInterval.SelectedValue);
            Session["SOT_TimerChecked"] = true;
        }
        else
        {
            Timer1.Enabled = false;
            Session["SOT_TimerChecked"] = false;
        }
    }
    protected void ddlInterval_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Interval"] = Convert.ToInt32(ddlInterval.SelectedValue);
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        lblErrorNotes.Text = "";
        //Reset checkbox...
        chkCCtoLogistics.Checked = false;
    }
    protected void btnCancel2_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderDeliveries.Hide();
        Timer1.Enabled = true;

        gvDeliveries.DataSource = null;
        gvDeliveries.DataBind();

    }
    protected void gvDeliveries_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblDateScheduled = (Label)e.Row.FindControl("lblDateScheduled");
            Label lblDateDelivered = (Label)e.Row.FindControl("lblDateDelivered");

            if (lblDateScheduled.Text != "")
            {
                lblDateScheduled.Text = Convert.ToDateTime(lblDateScheduled.Text).ToShortDateString();
            }
            else
            {
                lblDateScheduled.Text = "--";
            }
            if (lblDateDelivered.Text != "")
            {
                lblDateDelivered.Text = Convert.ToDateTime(lblDateDelivered.Text).ToShortDateString();
            }
            else
            {
                lblDateDelivered.Text = "--";
            }

            Label lblDeliveryTypeID = (Label)e.Row.FindControl("lblDeliveryTypeID");
            if (lblDeliveryTypeID.Text != "")
            {
                if (lblDeliveryTypeID.Text == "1")//Pickup
                {
                    lblDeliveryTypeID.Text = "Pick Up";
                }
                else if (lblDeliveryTypeID.Text == "2")
                {
                    lblDeliveryTypeID.Text = "Delivery";
                }
                else if (lblDeliveryTypeID.Text == "3")
                {
                    lblDeliveryTypeID.Text = "Will Call";
                }
            }

            Label lblDeliveryStatus = (Label)e.Row.FindControl("lblDeliveryStatus");
            if (lblDeliveryStatus.Text != "")
            {
                switch (lblDeliveryStatus.Text)
                {
                    case "1":
                        lblDeliveryStatus.Text = "PickUp Completed";
                        break;
                    case "2":
                        lblDeliveryStatus.Text = "Delivery/Will Call Completed";
                        break;
                    case "3":
                        lblDeliveryStatus.Text = "PickUp Scheduled";
                        break;
                    case "4":
                        lblDeliveryStatus.Text = "Delivery/Will Call Scheduled";
                        break;
                    default:
                        lblDeliveryStatus.Text = "Not Set";
                        break;
                }
            }
            else
            {
                lblDeliveryStatus.Text = "Not Set";
            }

            System.Web.UI.WebControls.Image imgFlaggedDR = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgFlaggedDR");
            Label lblDRFlag = (Label)e.Row.FindControl("lblDRFlag");
            if (lblDRFlag.Text != "")
            {
                if (lblDRFlag.Text == "0")
                {//Show Flag if value is zero...
                    imgFlaggedDR.Visible = true;
                    imgFlaggedDR.ToolTip = "Missing Delivery Receipt Document";
                }
                else
                {
                    imgFlaggedDR.Visible = false;
                }
            }

            Label lblCOD = (Label)e.Row.FindControl("lblCOD");
            if (lblCOD.Text != "")
            {
                if (lblCOD.Text == "1")
                {
                    lblCOD.Text = "YES";
                }
                else
                {
                    lblCOD.Text = "NO";
                }
            }

            Label lblAmount = (Label)e.Row.FindControl("lblAmount");

            if (lblAmount.Text != "")
            {
                lblAmount.Text = Convert.ToDecimal(lblAmount.Text).ToString("c");
            }

        }
    }
    protected void gvDeliveries_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int idx = 0;
        Label lblDeliveryID;
        Label lblSalesOrder;
        switch (e.CommandName)
        {

            case "ViewDocs"://Goes to DocSearch...
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...
                lblDeliveryID = (Label)gvDeliveries.Rows[idx].FindControl("lblDeliveryID");
                Response.Redirect("DocSearch.aspx?id=" + lblDeliveryID.Text + "&type=DR");
                break;
            case "Scan"://Goes to pre scan...
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...
                lblDeliveryID = (Label)gvDeliveries.Rows[idx].FindControl("lblDeliveryID");
                lblSalesOrder = (Label)gvDeliveries.Rows[idx].FindControl("lblSalesOrder");
                Response.Redirect("PreScan.aspx?id=" + lblDeliveryID.Text + "&type=DR&so=" + lblSalesOrder.Text);
                break;
            case "Edit"://Goes to Delivery Admin...
                idx = Convert.ToInt32(e.CommandArgument);//bound in html code...
                lblDeliveryID = (Label)gvDeliveries.Rows[idx].FindControl("lblDeliveryID");
                Response.Redirect("DeliveryAdmin.aspx?id=" + lblDeliveryID.Text + "&type=DR");
                break;
        }
    }
    protected void gvProductionSchedule_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblScheduledQty = (Label)e.Row.FindControl("lblScheduledQty");
            Label lblScheduledDate = (Label)e.Row.FindControl("lblScheduledDate");
            if (lblScheduledQty.Text != "")
            {
                lblScheduledQty.Text = Convert.ToDecimal(lblScheduledQty.Text.Trim()).ToString("0.00");
            }
            if (lblScheduledDate.Text != "")
            {
                lblScheduledDate.Text = Convert.ToDateTime(lblScheduledDate.Text.Trim()).ToShortDateString();
            }
        }
    }
    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        DataTable dtNew = new DataTable();
        DataTable dt = new DataTable();
        string sFileName = "";

        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(SharedFunctions.GetRole(iUserID));
        string sInput = "NULL";
        if (txtSearch.Text.Trim() != "")
        {
            sInput = "'" + txtSearch.Text.Trim() + "'";
        }

        dt = GetOpenOrders(iRoleID, iUserID, sInput);
        dt.TableName = "dtRecordSalesOrderTracker";
        if (rblCompton.SelectedIndex == 0)
        {
            var query = (from dtSum in dt.AsEnumerable()
                         select new
                         {
                             Operator = dtSum["LastOperator"].ToString(),
                             Customer = dtSum["CustomerID"].ToString(),
                             CustomerName = dtSum["Customer"].ToString(),
                             Ack = dtSum["Ack"].ToString(),
                             OrderDate = dtSum["OrderDate"].ToString(),
                             SalesOrder = dtSum["SalesOrder"],
                             CustomerPO = dtSum["CustomerPONumber"].ToString(),
                             TotalQuantity = dtSum["TotalQuantity"].ToString(),
                             TotalValue = dtSum["TotalValue"].ToString(),
                             OrderStatus = dtSum["OrderStatus"].ToString(),
                             ShipDate = dtSum["ReqShipDate"].ToString(),
                             DefaultShipVia = dtSum["ShipViaDescription"].ToString(),
                             Salesperson = dtSum["SalesPerson"].ToString(),
                             Scan = dtSum["ReadyForPickup"].ToString(),
                             CreditHold = dtSum["CreditHold"].ToString(),
                             CustomerDeliveryDate = dtSum["CustomerDeliveryDate"].ToString(),
                             CustomerDeliverySchedule = dtSum["CustomerDeliverySchedule"].ToString(),
                             Comment = dtSum["Comment"].ToString(),
                         });
            dt = SharedFunctions.LINQToDataTable(query);
            DataRow row = null;
            dtNew.Columns.Add("Operator");
            dtNew.Columns.Add("Customer");
            dtNew.Columns.Add("CustomerName");
            dtNew.Columns.Add("Ack");
            dtNew.Columns.Add("OrderDate");
            dtNew.Columns.Add("SalesOrder");
            dtNew.Columns.Add("CustomerPO");
            dtNew.Columns.Add("TotalQuantity");
            dtNew.Columns.Add("TotalValue");
            dtNew.Columns.Add("OrderStatus");
            dtNew.Columns.Add("ShipDate");
            dtNew.Columns.Add("DefaultShipVia");
            dtNew.Columns.Add("SalesPerson");
            dtNew.Columns.Add("Scan");
            dtNew.Columns.Add("CreditHold");
            dtNew.Columns.Add("CustomerDeliveryDate");
            dtNew.Columns.Add("CustomerDeliverySchedule");
            dtNew.Columns.Add("Comment");


            foreach (var a in query)
            {
                row = dtNew.NewRow();

                row["Operator"] = a.Operator;
                row["Customer"] = a.Customer;
                row["CustomerName"] = a.CustomerName;
                row["Ack"] = a.Ack;
                row["OrderDate"] = Convert.ToDateTime(a.OrderDate).ToShortDateString();
                row["SalesOrder"] = Convert.ToInt32(a.SalesOrder);
                row["CustomerPO"] = a.CustomerPO;
                row["TotalQuantity"] = a.TotalQuantity;
                row["TotalValue"] = a.TotalValue;
                row["OrderStatus"] = a.OrderStatus;
                if (!string.IsNullOrEmpty(a.ShipDate.ToString()))
                {
                    row["ShipDate"] = Convert.ToDateTime(a.ShipDate).ToShortDateString();
                }
                row["DefaultShipVia"] = a.DefaultShipVia;
                row["SalesPerson"] = a.Salesperson;
                if (a.Scan != "")
                {
                    row["Scan"] = "YES";
                }
                if (a.CreditHold != "")
                {
                    row["CreditHold"] = "YES";
                }
                if (!string.IsNullOrEmpty(a.CustomerDeliveryDate.ToString()))
                {
                    row["CustomerDeliveryDate"] = Convert.ToDateTime(a.CustomerDeliveryDate).ToShortDateString();
                }
                row["CustomerDeliverySchedule"] = a.CustomerDeliverySchedule;
                row["Comment"] = a.Comment;
                dtNew.Rows.Add(row);

            }
        }
        else//Compton...
        {
            var query = (from dtSum in dt.AsEnumerable()
                         select new
                         {
                             Operator = dtSum["LastOperator"].ToString(),
                             Customer = dtSum["CustomerID"].ToString(),
                             CustomerName = dtSum["Customer"].ToString(),
                             Ack = dtSum["Ack"].ToString(),
                             OrderDate = dtSum["OrderDate"].ToString(),
                             SalesOrder = dtSum["SalesOrder"].ToString(),
                             CustomerPO = dtSum["CustomerPONumber"].ToString(),
                             TotalQuantity = dtSum["TotalQuantity"].ToString(),
                             OrderStatus = dtSum["OrderStatus"].ToString(),
                             Picker = dtSum["Picker"].ToString(),
                             PickDate = dtSum["PickDate"].ToString(),
                             StartPickTime = dtSum["StartPickTime"].ToString(),
                             EndPickTime = dtSum["EndPickTime"].ToString(),
                             StdPickingTimeInMinutes = dtSum["StdPickingTimeInMinutes"].ToString(),
                             StdCasesPerMinute = dtSum["StdCasesPerMinute"].ToString(),
                             ActualPickingTimeInMinutes = dtSum["ActualPickingTimeInMinutes"].ToString(),
                             ActualCasesPerMinutes = dtSum["ActualCasesPerMinutes"].ToString(),
                             CasesPicked = dtSum["CasesPicked"].ToString(),
                             ProjectedFinishTime = dtSum["ProjectedFinishTime"].ToString(),
                             ShipDate = dtSum["ReqShipDate"].ToString(),
                             DefaultShipVia = dtSum["ShipViaDescription"].ToString(),
                             Salesperson = dtSum["SalesPerson"].ToString(),
                             Scan = dtSum["ReadyForPickup"].ToString(),
                             CreditHold = dtSum["CreditHold"].ToString(),
                             CustomerDeliveryDate = dtSum["CustomerDeliveryDate"].ToString(),
                             ActualDeliveryDate = dtSum["ActualDeliveryDate"].ToString(),
                             AppointmentTime = dtSum["AppointmentTime"].ToString(),
                             ActualShipVia = dtSum["ActualShipVia"].ToString(),
                             Comment = dtSum["Comment"].ToString(),
                         });

            dt = SharedFunctions.LINQToDataTable(query);

            DataRow row = null;
            dtNew.Columns.Add("Operator");
            dtNew.Columns.Add("Customer");
            dtNew.Columns.Add("CustomerName");
            dtNew.Columns.Add("Ack");
            dtNew.Columns.Add("OrderDate");
            dtNew.Columns.Add("SalesOrder");
            dtNew.Columns.Add("CustomerPO");
            dtNew.Columns.Add("TotalQuantity");
            dtNew.Columns.Add("OrderStatus");
            dtNew.Columns.Add("Picker");
            dtNew.Columns.Add("PickDate");
            dtNew.Columns.Add("StartPickTime", typeof(string));
            dtNew.Columns.Add("EndPickTime", typeof(string));
            dtNew.Columns.Add("StdPickingTimeInMinutes");
            dtNew.Columns.Add("StdCasesPerMinute");
            dtNew.Columns.Add("ActualPickingTimeInMinutes");
            dtNew.Columns.Add("ActualCasesPerMinutes");
            dtNew.Columns.Add("CasesPicked");
            dtNew.Columns.Add("ProjectedFinishTime");
            dtNew.Columns.Add("ShipDate");
            dtNew.Columns.Add("DefaultShipVia");
            dtNew.Columns.Add("SalesPerson");
            dtNew.Columns.Add("Scan");
            dtNew.Columns.Add("CreditHold");
            dtNew.Columns.Add("CustomerDeliveryDate");
            dtNew.Columns.Add("ActualDeliveryDate");
            dtNew.Columns.Add("AppointmentTime", typeof(string));
            dtNew.Columns.Add("ActualShipVia");
            dtNew.Columns.Add("Comment");
            foreach (var a in query)
            {
                row = dtNew.NewRow();

                row["Operator"] = a.Operator;
                row["Customer"] = a.Customer;
                row["CustomerName"] = a.CustomerName;
                row["Ack"] = a.Ack;
                row["OrderDate"] = Convert.ToDateTime(a.OrderDate).ToShortDateString();
                row["SalesOrder"] = Convert.ToInt32(a.SalesOrder);
                row["CustomerPO"] = a.CustomerPO;
                row["TotalQuantity"] = a.TotalQuantity;
                row["OrderStatus"] = a.OrderStatus;
                row["Picker"] = a.Picker;
                row["PickDate"] = a.PickDate;
                if (!string.IsNullOrEmpty(a.StartPickTime.ToString()))
                {
                    string sStart = Convert.ToDateTime(a.StartPickTime.ToString()).ToString("HH:mm:ss");
                    row["StartPickTime"] = sStart;
                }
                if (!string.IsNullOrEmpty(a.EndPickTime.ToString()))
                {
                    string sEnd = Convert.ToDateTime(a.EndPickTime.ToString()).ToString("HH:mm:ss");
                    row["EndPickTime"] = sEnd;
                }
                row["StdPickingTimeInMinutes"] = a.StdPickingTimeInMinutes;
                row["StdCasesPerMinute"] = a.StdCasesPerMinute;
                row["ActualPickingTimeInMinutes"] = a.StdPickingTimeInMinutes;
                row["ActualCasesPerMinutes"] = a.ActualCasesPerMinutes;
                row["CasesPicked"] = a.CasesPicked;
                row["ProjectedFinishTime"] = a.ProjectedFinishTime;
                if (!string.IsNullOrEmpty(a.ShipDate.ToString()))
                {
                    row["ShipDate"] = Convert.ToDateTime(a.ShipDate).ToShortDateString();
                }
                row["DefaultShipVia"] = a.DefaultShipVia;
                row["SalesPerson"] = a.Salesperson;
                if (a.Scan != "")
                {
                    row["Scan"] = "YES";
                }
                if (a.CreditHold != "")
                {
                    row["CreditHold"] = "YES";
                }
                if (!string.IsNullOrEmpty(a.CustomerDeliveryDate.ToString()))
                {
                    row["CustomerDeliveryDate"] = Convert.ToDateTime(a.CustomerDeliveryDate).ToShortDateString();
                }
                row["ActualDeliveryDate"] = a.ActualDeliveryDate;
                if (!string.IsNullOrEmpty(a.AppointmentTime.ToString()))
                {
                    string sAppointmentTime = Convert.ToDateTime(a.AppointmentTime.ToString()).ToString("HH:mm:ss");
                    row["AppointmentTime"] = sAppointmentTime;
                }
                row["ActualShipVia"] = a.ActualShipVia;
                row["Comment"] = a.Comment;
                dtNew.Rows.Add(row);

            }
        }

        sFileName = "SalesOrderTackerReport" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
        ExcelHelper.ToExcel(dtNew, sFileName, Page.Response);
        //send session variable dtReport to Excel...

        dt.Dispose();
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {//Reset checkbox...
        if (chkTimerOnOff.Checked)
        {
            Timer1.Enabled = true;
            Timer1.Interval = Convert.ToInt32(ddlInterval.SelectedValue);
            Session["SOT_TimerChecked"] = true;
        }
        else
        {
            Timer1.Enabled = false;
            Session["SOT_TimerChecked"] = false;
        }
        chkCCtoLogistics.Checked = false;
        chkCCtoProduction.Checked = false;
        chkCCtoQualityControl.Checked = false;
        chkCCtoCustomerService.Checked = false;
        chkCCtoOperations.Checked = false;
        chkCCtoAccountsReceivable.Checked = false;
        chkCCtoTransfer.Checked = false;
        chkLoadSchedules.Checked = false;
        chkLoadMatrixes.Checked = false;
        ModalPopupExtenderPopUp.Hide();

    }
    protected void btnPlacedOrdersByDateRange_Click(object sender, EventArgs e)
    {
        dcPlacedOrdersCountTotal = 0;
        dcPlacedOrdersTotal = 0;
        dcPlacedLinesCountTotal = 0;

        dcPlacedOrdersCountUnchangedTotal = 0;
        dcPlacedOrdersUnchangedTotal = 0;
        dcPlacedLinesCountUnchangedTotal = 0;

        dcPlacedOrdersCountDiffTotal = 0;
        dcPlacedOrdersDiffTotal = 0;
        dcPlacedLinesCountDiffTotal = 0;
        lblOrdersPlacedError.Text = "";
        ModalPopupExtenderPlacedOrders.Show();
        Timer1.Enabled = false;
    }

    protected void gvPlacedOrders_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcCount = 0;
        decimal dcLineCount = 0;
        decimal dcAmount = 0;
        decimal dcCountUnchanged = 0;
        decimal dcLineCountUnchanged = 0;
        decimal dcAmountUnchanged = 0;
        decimal dcCountDiff = 0;
        decimal dcLineCountDiff = 0;
        decimal dcAmountDiff = 0;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {


            //Normal...
            Label lblTotalValue = (Label)e.Row.FindControl("lblTotalValue");
            if (lblTotalValue.Text != "")
            {
                dcAmount = Convert.ToDecimal(lblTotalValue.Text.Replace("$", ""));
                dcPlacedOrdersTotal += dcAmount;
                lblTotalValue.Text = "$" + Convert.ToDecimal(lblTotalValue.Text).ToString("#,0");
            }

            Label lblOrderCount = (Label)e.Row.FindControl("lblOrderCount");
            if (lblOrderCount.Text != "")
            {
                dcCount = Convert.ToDecimal(lblOrderCount.Text);
                dcPlacedOrdersCountTotal += dcCount;
                lblOrderCount.Text = dcCount.ToString("#,0");
            }
            Label lblLineCount = (Label)e.Row.FindControl("lblLineCount");
            if (lblLineCount.Text != "")
            {
                dcLineCount = Convert.ToDecimal(lblLineCount.Text);
                dcPlacedLinesCountTotal += dcLineCount;
                lblLineCount.Text = dcLineCount.ToString("#,0");
            }
            //Unchanged
            Label lblTotalValueUnchanged = (Label)e.Row.FindControl("lblTotalValueUnchanged");
            if (lblTotalValueUnchanged.Text != "")
            {
                dcAmountUnchanged = Convert.ToDecimal(lblTotalValueUnchanged.Text.Replace("$", ""));
                dcPlacedOrdersUnchangedTotal += dcAmountUnchanged;
                lblTotalValueUnchanged.Text = "$" + Convert.ToDecimal(lblTotalValueUnchanged.Text).ToString("#,0");
            }

            Label lblOrderCountUnchanged = (Label)e.Row.FindControl("lblOrderCountUnchanged");
            if (lblOrderCountUnchanged.Text != "")
            {
                dcCountUnchanged = Convert.ToDecimal(lblOrderCountUnchanged.Text);
                dcPlacedOrdersCountUnchangedTotal += dcCountUnchanged;
                lblOrderCountUnchanged.Text = dcCountUnchanged.ToString("#,0");
            }
            Label lblLineCountUnchanged = (Label)e.Row.FindControl("lblLineCountUnchanged");
            if (lblLineCountUnchanged.Text != "")
            {
                dcLineCountUnchanged = Convert.ToDecimal(lblLineCountUnchanged.Text);
                dcPlacedLinesCountUnchangedTotal += dcLineCountUnchanged;
                lblLineCountUnchanged.Text = dcLineCountUnchanged.ToString("#,0");
            }

            //Diff
            Label lblTotalValueDiff = (Label)e.Row.FindControl("lblTotalValueDiff");
            if (lblTotalValueDiff.Text != "")
            {
                dcAmountDiff = Convert.ToDecimal(lblTotalValueDiff.Text.Replace("$", ""));
                dcPlacedOrdersDiffTotal += dcAmountDiff;
                lblTotalValueDiff.Text = "$" + Convert.ToDecimal(lblTotalValueDiff.Text).ToString("#,0");
            }

            Label lblOrderCountDiff = (Label)e.Row.FindControl("lblOrderCountDiff");
            if (lblOrderCount.Text != "")
            {
                dcCountDiff = Convert.ToDecimal(lblOrderCountDiff.Text);
                dcPlacedOrdersCountDiffTotal += dcCountDiff;
                lblOrderCountDiff.Text = dcCountDiff.ToString("#,0");
            }
            Label lblLineCountDiff = (Label)e.Row.FindControl("lblLineCountDiff");
            if (lblLineCountDiff.Text != "")
            {
                dcLineCountDiff = Convert.ToDecimal(lblLineCountDiff.Text);
                dcPlacedLinesCountDiffTotal += dcLineCountDiff;
                lblLineCountDiff.Text = dcLineCountDiff.ToString("#,0");
            }



        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            //Normal...
            Label lblTotalValueTotal = (Label)e.Row.FindControl("lblTotalValueTotal");
            lblTotalValueTotal.Text = "$" + dcPlacedOrdersTotal.ToString("#,0");

            Label lblLineCountTotal = (Label)e.Row.FindControl("lblLineCountTotal");
            lblLineCountTotal.Text = dcPlacedLinesCountTotal.ToString("#,0");

            Label lblOrderCountTotal = (Label)e.Row.FindControl("lblOrderCountTotal");
            lblOrderCountTotal.Text = dcPlacedOrdersCountTotal.ToString("#,0");

            //Unchanged...
            Label lblTotalValueUnchangedTotal = (Label)e.Row.FindControl("lblTotalValueUnchangedTotal");
            lblTotalValueUnchangedTotal.Text = "$" + dcPlacedOrdersUnchangedTotal.ToString("#,0");

            Label lblLineCountUnchangedTotal = (Label)e.Row.FindControl("lblLineCountUnchangedTotal");
            lblLineCountUnchangedTotal.Text = dcPlacedLinesCountUnchangedTotal.ToString("#,0");

            Label lblOrderCountUnchangedTotal = (Label)e.Row.FindControl("lblOrderCountUnchangedTotal");
            lblOrderCountUnchangedTotal.Text = dcPlacedOrdersCountUnchangedTotal.ToString("#,0");
            //Diff...
            Label lblTotalValueDiffTotal = (Label)e.Row.FindControl("lblTotalValueDiffTotal");
            lblTotalValueDiffTotal.Text = "$" + dcPlacedOrdersDiffTotal.ToString("#,0");

            Label lblLineCountDiffTotal = (Label)e.Row.FindControl("lblLineCountDiffTotal");
            lblLineCountDiffTotal.Text = dcPlacedLinesCountDiffTotal.ToString("#,0");

            Label lblOrderCountDiffTotal = (Label)e.Row.FindControl("lblOrderCountDiffTotal");
            lblOrderCountDiffTotal.Text = dcPlacedOrdersCountDiffTotal.ToString("#,0");


        }
    }
    protected void gvPlacedOrders_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtPlacedOrders"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvPlacedOrders.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvPlacedOrders.DataSource = m_DataView;
            gvPlacedOrders.DataBind();
            gvPlacedOrders.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
        ModalPopupExtenderPlacedOrders.Show();
    }
    protected void bntCancel4_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderPlacedOrders.Hide();
        if (chkTimerOnOff.Checked)
        {
            Timer1.Enabled = true;
        }
    }
    protected void btnRunPlacedOrders_Click(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
        lblOrdersPlacedError.Text = "";
        LoadPlacedOrdersGrid(iRoleID, iUserID);
        ModalPopupExtenderPlacedOrders.Show();
    }
    protected void btnExportExcelPlacedOrdersSummary_Click(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
        lblOrdersPlacedError.Text = "";

        ExportPlacedOrders(iRoleID, iUserID);
        LoadPlacedOrdersGrid(iRoleID, iUserID);
        ModalPopupExtenderPlacedOrders.Show();

    }
    //Place orders Details...
    protected void btnRunPlacedOrdersDetails_Click(object sender, EventArgs e)
    {
        lblOrdersPlacedError.Text = "";
        LoadPlacedOrdersDetails();
        ModalPopupExtenderPlacedOrders.Show();
    }
    protected void btnExportExcelPlacedOrdersDetails_Click(object sender, EventArgs e)
    {
        lblOrdersPlacedError.Text = "";
        ExportPlacedOrdersDetails();
        ModalPopupExtenderPlacedOrders.Show();
    }
    protected void gvPlacedOrderDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddlPlaceOrdersNote = (DropDownList)e.Row.FindControl("ddlPlaceOrdersNote");
            Label lblSorPlacedOrdersNotesCommentOptionsID = (Label)e.Row.FindControl("lblSorPlacedOrdersNotesCommentOptionsID");
            LoadSorPlacedOrdersNotesCommentOptions(ddlPlaceOrdersNote);
            if (lblSorPlacedOrdersNotesCommentOptionsID.Text != "")
            {
                ddlPlaceOrdersNote.SelectedValue = lblSorPlacedOrdersNotesCommentOptionsID.Text;
                ddlPlaceOrdersNote.BackColor = Color.Yellow;
            }
        }
    }
    protected void gvPlacedOrderDetails_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        DataTable dt = new DataTable();
        dt = (DataTable)Session["dtPlacedOrdersDetails"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvPlacedOrderDetails.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvPlacedOrderDetails.DataSource = m_DataView;
            gvPlacedOrderDetails.DataBind();
            gvPlacedOrderDetails.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
        ModalPopupExtenderPlacedOrders.Show();
    }
    protected void lbnUpdateNotes_Click(object sender, EventArgs e)
    {
        lblOrdersPlacedError.Text = "";
        UpdatePlacedOrdersDetails();
        ModalPopupExtenderPlacedOrders.Show();
    }
    protected void ddlPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblOrdersPlacedError.Text = "";
        gvPlacedOrders.DataSource = null;
        gvPlacedOrders.DataBind();
        int iDaysInMonth = 0;


        switch (ddlPeriod.SelectedIndex)
        {

            case 0://Range
                txtStartDate.Text = "";
                txtEndDate.Text = "";
                break;
            case 1://Today
                txtStartDate.Text = DateTime.Now.ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 2://Yesterday
                txtStartDate.Text = DateTime.Now.AddDays(-1).ToShortDateString();
                txtEndDate.Text = DateTime.Now.AddDays(-1).ToShortDateString();
                break;
            case 3://Current Month
                iDaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                txtStartDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString();
                txtEndDate.Text = new DateTime(DateTime.Now.Year, DateTime.Now.Month, iDaysInMonth).ToShortDateString();
                break;
            case 4://Last Month
                iDaysInMonth = DateTime.DaysInMonth(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month);
                txtStartDate.Text = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, 1).ToShortDateString();
                txtEndDate.Text = new DateTime(DateTime.Now.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, iDaysInMonth).ToShortDateString();
                break;
            case 5://3 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-3).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;
            case 6://6 mo.
                txtStartDate.Text = DateTime.Now.AddMonths(-6).ToShortDateString();
                txtEndDate.Text = DateTime.Now.ToShortDateString();
                break;

        }
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);
        if (txtStartDate.Text.Trim() != "" && txtEndDate.Text.Trim() != "")
        {
            LoadPlacedOrdersGrid(iRoleID, iUserID);
        }
        ModalPopupExtenderPlacedOrders.Show();

    }
    //protected void chkScanned_CheckedChanged(object sender, EventArgs e)
    //{
    //    lblError.Text = "";
    //    lblError0.Text = "";
    //    using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
    //    {
    //        try
    //        {

    //            string sSalesOrder = "";
    //            string sOrderDate = "";
    //            string sCustomer = "";
    //            string sPO = "";
    //            CheckBox chkScanned = (CheckBox)sender;

    //            GridViewRow gvr = (GridViewRow)chkScanned.Parent.Parent;
    //            LinkButton lbnSalesOrder = (LinkButton)gvr.FindControl("lbnSalesOrder");
    //            Label lblCustomer = (Label)gvr.FindControl("lblCustomer");
    //            Label lblOrderDate = (Label)gvr.FindControl("lblOrderDate");
    //            Label lblPurchaseOrder = (Label)gvr.FindControl("lblPurchaseOrder");

    //            int iUserID = Convert.ToInt32(Session["UserID"]);
    //            int iRoleID = Convert.ToInt32(Session["RoleID"]);

    //            List<string> lOKToUse = SharedFunctions.GetSecurityGroupMembers(2);//Scanned...                
    //            if (!lOKToUse.Contains(iUserID.ToString()))
    //            {
    //                lblError.Text = "**You are not authorized to mark order as Scanned!!";
    //                lblError.ForeColor = Color.Red;
    //                lblError0.Text = "**You are not authorized to mark order as Scanned!!";
    //                lblError0.ForeColor = Color.Red;
    //                chkScanned.Checked = false;
    //                return;
    //            }


    //            sSalesOrder = lbnSalesOrder.Text.Trim();
    //            sOrderDate = lblOrderDate.Text.Trim();
    //            sCustomer = lblCustomer.Text.Trim();
    //            sPO = lblPurchaseOrder.Text.Trim();
    //            if (chkScanned.Checked)
    //            {
    //                SorMaster ca = db.SorMaster.Single(p => Convert.ToInt32(p.SalesOrder) == Convert.ToInt32(sSalesOrder));
    //                ca.ReadyForPickup = DateTime.Now;
    //                db.SubmitChanges();
    //            }


    //            if (chkScanned.Checked)
    //            {
    //                //Send Email...
    //                //   SendScannedEmail(sSalesOrder, sOrderDate, sCustomer, sPO);//Commented out per Brian...9-24-2021...
    //            }

    //            RunSearch();

    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.WriteLine(ex.ToString());
    //        }
    //    }
    //}

    protected void chkCreditHold_CheckedChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        lblError0.Text = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {

                string sSalesOrder = "";
                string sOrderDate = "";
                string sCustomer = "";
                string sPO = "";
                CheckBox chkCreditHold = (CheckBox)sender;

                GridViewRow gvr = (GridViewRow)chkCreditHold.Parent.Parent;
                LinkButton lbnSalesOrder = (LinkButton)gvr.FindControl("lbnSalesOrder");
                Label lblCustomer = (Label)gvr.FindControl("lblCustomer");
                Label lblOrderDate = (Label)gvr.FindControl("lblOrderDate");
                Label lblPurchaseOrder = (Label)gvr.FindControl("lblPurchaseOrder");

                int iUserID = Convert.ToInt32(Session["UserID"]);
                int iRoleID = Convert.ToInt32(Session["RoleID"]);


                if (iRoleID == 1 || iRoleID == 8)//Admin or Credit manager...
                {
                    //OK to edit...
                }
                else
                {
                    lblError.Text = "**You are not authorized to mark order as Credit Hold!!";
                    lblError.ForeColor = Color.Red;
                    lblError0.Text = "**You are not authorized to mark order as Credit Hold!!";
                    lblError0.ForeColor = Color.Red;
                    chkCreditHold.Checked = false;
                    return;
                }


                sSalesOrder = lbnSalesOrder.Text.Trim();
                sOrderDate = lblOrderDate.Text.Trim();
                sCustomer = lblCustomer.Text.Trim();
                sPO = lblPurchaseOrder.Text.Trim();
                SorMaster ca = db.SorMaster.Single(p => Convert.ToInt32(p.SalesOrder) == Convert.ToInt32(sSalesOrder));
                if (chkCreditHold.Checked)
                {
                    ca.CreditHold = DateTime.Now;
                }
                else
                {
                    ca.CreditHold = null;
                }
                db.SubmitChanges();

                RunSearch();

                if (chkCreditHold.Checked)
                {
                    //Send Email...
                    SendCreditHoldEmail(sSalesOrder, sOrderDate, sCustomer, sPO);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    //protected void txtStaged_TextChanged(object sender, EventArgs e)
    //{
    //    lblError.Text = "";
    //    lblError0.Text = "";
    //    using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
    //    {
    //        try
    //        {

    //            string sSalesOrder = "";
    //            string sOrderDate = "";
    //            string sCustomer = "";
    //            string sPO = "";
    //            TextBox txtStaged = (TextBox)sender;

    //            GridViewRow gvr = (GridViewRow)txtStaged.Parent.Parent;
    //            LinkButton lbnSalesOrder = (LinkButton)gvr.FindControl("lbnSalesOrder");
    //            Label lblCustomer = (Label)gvr.FindControl("lblCustomer");
    //            Label lblOrderDate = (Label)gvr.FindControl("lblOrderDate");
    //            Label lblPurchaseOrder = (Label)gvr.FindControl("lblPurchaseOrder");

    //            int iUserID = Convert.ToInt32(Session["UserID"]);
    //            int iRoleID = Convert.ToInt32(Session["RoleID"]);
    //            List<string> lOKToUse = SharedFunctions.GetSecurityGroupMembers(1);//Staged...            
    //            if (!lOKToUse.Contains(iUserID.ToString()))
    //            {
    //                lblError.Text = "**You are not authorized to mark order as Staged!!";
    //                lblError.ForeColor = Color.Red;
    //                lblError0.Text = "**You are not authorized to mark order as Staged!!";
    //                lblError0.ForeColor = Color.Red;
    //                txtStaged.Text = "";
    //                return;
    //            }
    //            if (txtStaged.Text.Trim() != "")
    //            {
    //                if (txtStaged.Text.Trim() != "P")
    //                {
    //                    if (txtStaged.Text.Trim() != "F")
    //                    {
    //                        lblError.Text = "**Staged MUST be either a P for Partial or a F for Full!!";
    //                        lblError.ForeColor = Color.Red;
    //                        lblError0.Text = "**Staged MUST be either a P for Partial or a F for Full!!";
    //                        lblError0.ForeColor = Color.Red;
    //                        txtStaged.Text = "";
    //                        return;
    //                    }
    //                }
    //                else
    //                {

    //                }
    //            }
    //            sSalesOrder = lbnSalesOrder.Text.Trim();
    //            sOrderDate = lblOrderDate.Text.Trim();
    //            sCustomer = lblCustomer.Text.Trim();
    //            sPO = lblPurchaseOrder.Text.Trim();
    //            SorMaster ca = db.SorMaster.Single(p => Convert.ToInt32(p.SalesOrder) == Convert.ToInt32(sSalesOrder));
    //            if (txtStaged.Text.Trim() != "")
    //            {
    //                ca.Staged = txtStaged.Text.Trim().ToUpper();
    //            }
    //            else
    //            {
    //                ca.Staged = null;
    //            }
    //            db.SubmitChanges();

    //            RunSearch();

    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.WriteLine(ex.ToString());
    //        }
    //    }
    //}

    protected void btnAddNotes_Click(object sender, EventArgs e)
    {
        string sMsg = "";

        if (sMsg.Length > 0)
        {
            lblErrorNotes.Text = sMsg;
            lblErrorNotes.ForeColor = Color.Red;
            ModalPopupExtenderPopUp.Show();
            return;
        }
        AddComments();
        ModalPopupExtenderPopUp.Show();
    }
    protected void gvNotes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (gvNotes.EditIndex != -1)//In Edit Mode...
                {

                    if (gvNotes.EditIndex == e.Row.RowIndex)//edited row...
                    {



                    }
                    else if (gvNotes.EditIndex != e.Row.RowIndex)//only for row that are not in edit mode while editing...
                    {
                        string sUserName = Session["UserName"].ToString();
                        if (sUserName == "rsherman")
                        {
                            e.Row.Cells[0].Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[0].Visible = false;
                        }
                    }
                }
                else//Not in Edit Mode...
                {
                    string sUserName = Session["UserName"].ToString();
                    if (sUserName == "rsherman")
                    {
                        e.Row.Cells[0].Visible = true;
                    }
                    else
                    {
                        e.Row.Cells[0].Visible = false;
                    }
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)//the footer row...
            {

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }
    protected void gvNotes_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvNotes.EditIndex = e.NewEditIndex;
        gvNotes.DataSource = (DataTable)Session["dtNotes"];
        gvNotes.DataBind();
        ModalPopupExtenderPopUp.Show();

    }
    protected void gvNotes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvNotes.EditIndex = -1;
        gvNotes.DataSource = (DataTable)Session["dtNotes"];
        gvNotes.DataBind();
        ModalPopupExtenderPopUp.Show();
    }
    protected void gvNotes_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        gvNotes.EditIndex = -1;
        GetComments(lblSaleOrderDetails.Text);
        ModalPopupExtenderPopUp.Show();
    }
    protected void gvNotes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            lblErrorNotes.Text = "";


            int i = 0;
            int iSOCommentID = 0;
            Label lblSOCommentID;
            TextBox txtNote;

            string sFname = Session["FirstName"].ToString();
            string sMname = "";
            string sLname = Session["LastName"].ToString();
            if (Session["MiddleName"] != null)
            {
                sMname = Session["MiddleName"].ToString();
            }

            string sFullName = (sFname + " " + (sMname ?? "") + " " + sLname).Replace("  ", " ");



            switch (e.CommandName)
            {

                case "Update":
                    i = Convert.ToInt32(e.CommandArgument);
                    lblErrorNotes.Text = "";
                    lblSOCommentID = (Label)gvNotes.Rows[i].FindControl("lblSOCommentID");
                    iSOCommentID = Convert.ToInt32(lblSOCommentID.Text);
                    txtNote = (TextBox)gvNotes.Rows[i].FindControl("txtNote");
                    if (txtNote.Text.Trim() == "")
                    {
                        sMsg += "**No note!!";
                    }

                    if (sMsg.Length > 0)
                    {
                        lblErrorNotes.Text = sMsg;
                        lblErrorNotes.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    //Update...
                    try
                    {
                        SorComments ln = db.SorComments.Single(p => p.SOCommentID == iSOCommentID);
                        ln.Comment = txtNote.Text.Trim().ToUpper();
                        db.SubmitChanges();

                        lblErrorNotes.Text = "*** Notes Updated ***";
                        lblErrorNotes.ForeColor = System.Drawing.Color.Green;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        lblErrorNotes.Text = "**Update Failed!";
                        lblErrorNotes.ForeColor = System.Drawing.Color.Red;
                    }
                    break;
                case "Delete":
                    i = Convert.ToInt32(e.CommandArgument);
                    lblErrorNotes.Text = "";
                    lblSOCommentID = (Label)gvNotes.Rows[i].FindControl("lblSOCommentID");
                    iSOCommentID = Convert.ToInt32(lblSOCommentID.Text);


                    if (sMsg.Length > 0)
                    {
                        lblErrorNotes.Text = sMsg;
                        lblErrorNotes.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    //Update...
                    try
                    {
                        SorComments ln = db.SorComments.Single(p => p.SOCommentID == iSOCommentID);
                        db.SorComments.DeleteOnSubmit(ln);
                        db.SubmitChanges();

                        lblErrorNotes.Text = "*** Notes Deleted ***";
                        lblErrorNotes.ForeColor = System.Drawing.Color.Green;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.ToString());
                        lblErrorNotes.Text = "**Note Delete Failed!";
                        lblErrorNotes.ForeColor = System.Drawing.Color.Red;
                    }
                    break;

            }
        }//Close DB Connection...
    }
    protected void gvNotes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GetComments(lblSaleOrderDetails.Text);
        ModalPopupExtenderPopUp.Show();
    }
    //Top 15...

    protected void rblTopTen_SelectedIndexChanged(object sender, EventArgs e)
    {
        dcLineTotal = 0;
        dcGrandTotal = 0;
        dcShortageTotal = 0;
        dcQtyTotal = 0;
        dcPlacedOrdersCountTotal = 0;
        dcPlacedOrdersTotal = 0;
        dcPlacedLinesCountTotal = 0;

        dcCurrentYTDTotal = 0;
        dcLastYTDTotal = 0;
        dcLastYTDMinusOneTotal = 0;

        dcCurrentYTDEndOfMonthTotal = 0;
        dcLastYTDEndOfMonthTotal = 0;
        dcLastYTDMinusOneYearEndOfMonthAmountTotal = 0;

        dcCurrentMTDTotal = 0;
        dcLastYearCurrentMTDTotal = 0;

        dcCurrentMonthTotal = 0;
        dcLastYearCurrentMonthTotal = 0;

        dcPreviousMonthTotal = 0;
        dcLastYearPreviousMonthTotal = 0;


        dcReadyToInvoiceTotal = 0;
        dcOpenOrdersTotal = 0;

        dcYTDCostTotal = 0;
        dcLastYTDCostTotal = 0;

        dcYTDEndOfMonthCostTotal = 0;
        dcLastYTDEndOfMonthCostTotal = 0;


        dcYearToYearDiffPercentageWeighted = 0;
        dcYearToYearDiffPercentageMinusOneWeighted = 0;

        dcYearToYearDiffPercentageEndOfMonthWeighted = 0;
        dcYearToYearDiffPercentageEndOfMonthMinusOneWeighted = 0;

        dcCurrentYTDAmountPercentageOfTotalSum = 0;
        dcLastYTDAmountPercentageOfTotalSum = 0;

        dcCurrentYTDEndOfMonthAmountPercentageOfTotalSum = 0;
        dcLastYTDEndOfMonthAmountPercentageOfTotalSum = 0;


        if (rblTopTen.SelectedIndex == 1)
        {
            chkShowYTDthroughLastMonth.Checked = true;
        }
        else
        {
            chkShowYTDthroughLastMonth.Checked = false;
        }
        LoadTopFifteenData(rblTopTen.SelectedValue);
    }
    protected void gvTopFifteen_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcCurrentYTD = 0;
        decimal dcLastYTD = 0;
        decimal dcLastYTDMinusOne = 0;

        decimal dcCurrentYTDEndOfMonth = 0;
        decimal dcLastYTDEndOfMonth = 0;
        decimal dcLastYTDMinusOneYearEndOfMonth = 0;

        decimal dcCurrentMTD = 0;
        decimal dcLastYearCurrentMTD = 0;
        decimal dcCurrentMonthAmount = 0;
        decimal dcCurrentMonthCost = 0;
        decimal dcLastYearCurrentMonth = 0;

        decimal dcPreviousMonthAmount = 0;
        decimal dcPreviousMonthCost = 0;
        decimal dcLastYearLastMonth = 0;

        decimal dcReadyToInvoice = 0;
        decimal dcOpenOrders = 0;

        decimal dcYearToYearDiffPercentage = 0;
        decimal dcYearToYearDiffPercentageMinusOne = 0;
        decimal dcYearToYearDiffPercentageEndOfMonth = 0;
        decimal dcYearToYearDiffPercentageEndOfMonthMinusOne = 0;

        decimal dcYTDCost = 0;
        decimal dcLastYTDCost = 0;
        decimal dcYTDEndOfMonthCost = 0;
        decimal dcLastYTDEndOfMonthCost = 0;

        decimal dcYTDWeightedMargin = 0;
        decimal dcLastYTDWeightedMargin = 0;

        decimal dcYTDEndOfMonthWeightedMargin = 0;
        decimal dcLastYTDEndOfMonthWeightedMargin = 0;

        decimal dcCurrentYTDAmountPercentageOfTotal = 0;
        decimal dcLastYTDAmountPercentageOfTotal = 0;

        decimal dcCurrentYTDEndOfMonthAmountPercentageOfTotal = 0;
        decimal dcLastYTDEndOfMonthAmountPercentageOfTotal = 0;

        //Possible Future weighted avg...
        decimal dcCurrentMonthWeightedMargin = 0;
        decimal dcLastMonthWeightedMargin = 0;



        if (e.Row.RowType == DataControlRowType.Header)
        {
            DateTime dtToday = DateTime.Today;
            DateTime dtCurrentMonthFirstDay = new DateTime(dtToday.Year, dtToday.Month, 1);
            DateTime dtCurrentMonthFirstDayLastYear = new DateTime(dtToday.AddYears(-1).Year, dtToday.Month, 1);
            DateTime dtCurrentMonthLastDay = new DateTime(dtToday.Year, dtToday.Month, DateTime.DaysInMonth(dtToday.Year, dtToday.Month));
            DateTime dtCurrentMonthCurrentDay = new DateTime(dtToday.Year, dtToday.Month, dtToday.Day);
            DateTime dtCurrentMonthCurrentDayLastYear = new DateTime(2000, 1, 1);
            try
            {
                dtCurrentMonthCurrentDayLastYear = new DateTime(dtToday.AddYears(-1).Year, dtToday.Month, dtToday.Day);
            }
            catch (Exception)
            {//Leap Year Fix...
                dtCurrentMonthCurrentDayLastYear = new DateTime(dtToday.AddYears(-1).Year, dtToday.Month, 28);
            }


            DateTime dtFirstDayOfCurrentYear = new DateTime(DateTime.Now.Year, 1, 1);
            string sFirstDayOfCurrentYear = dtFirstDayOfCurrentYear.ToShortDateString();
            DateTime dtFirstDayOfLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, 1, 1);
            string sFirstDayOfLastYear = dtFirstDayOfLastYear.ToShortDateString();
            DateTime dtFirstDayOfLastYearMinusOne = new DateTime(DateTime.Now.AddYears(-2).Year, 1, 1);
            string sFirstDayOfLastYearMinusOne = dtFirstDayOfLastYearMinusOne.ToShortDateString();
            string sYTD = DateTime.Now.ToShortDateString();
            string sLYTD = DateTime.Now.AddDays(-365).ToShortDateString();
            string sMTD = DateTime.Now.ToShortDateString();
            DateTime dtFirstDayOfMonthCurrentYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            string sFirstDayOfMonthCurrentYear = dtFirstDayOfMonthCurrentYear.ToShortDateString();

            DateTime dtFirstDayOfLastMonthCurrentYear = dtFirstDayOfMonthCurrentYear.AddMonths(-1);//First Day of Last Month Period!
            string sFirstDayOfLastMonthCurrentYear = dtFirstDayOfLastMonthCurrentYear.ToShortDateString();

            DateTime dtLastDayOfLastMonthCurrentYear = new DateTime(dtFirstDayOfMonthCurrentYear.AddMonths(-1).Year, DateTime.Now.AddMonths(-1).Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.AddMonths(-1).Month));//Last Day of Last Month period!
            string sLastDayOfLastMonthCurrentYear = dtLastDayOfLastMonthCurrentYear.ToShortDateString();

            DateTime dtFirstDayOfMonthLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddYears(-1).Month, 1);
            string sFirstDayOfMonthLastYear = dtFirstDayOfMonthLastYear.ToShortDateString();


            DateTime dtFirstDayOfLastMonthLastYear = new DateTime(DateTime.Now.AddYears(-1).Year, DateTime.Now.AddMonths(-1).AddYears(-1).Month, 1);
            string sFirstDayOfLastMonthLastYear = dtFirstDayOfLastMonthLastYear.ToShortDateString();

            DateTime dtLastDayOfMonthCurrentYear = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
            string sLastDayOfMonthCurrentYear = dtLastDayOfMonthCurrentYear.ToShortDateString();
            DateTime dtLastDayOfMonthLastYear = dtLastDayOfMonthCurrentYear.AddYears(-1);
            string sLastDayOfMonthLastYear = dtLastDayOfMonthLastYear.ToShortDateString();

            DateTime dtLastDayOfLastMonthLastYear = dtLastDayOfLastMonthCurrentYear.AddYears(-1);
            string sLastDayOfLastMonthLastYear = dtLastDayOfLastMonthLastYear.ToShortDateString();

            DateTime dtLastDayOfLastMonthLastYearMinusOne = dtLastDayOfLastMonthCurrentYear.AddYears(-2);
            string sLastDayOfLastMonthLastYearMinusOne = dtLastDayOfLastMonthLastYearMinusOne.ToShortDateString();

            string sLastYearMTD = DateTime.Now.AddYears(-1).ToShortDateString();

            int iYear = DateTime.Now.Year;
            DateTime dtLastDayOfCurrentYear = new DateTime(iYear, 12, 31);
            DateTime dtLastMonthLastDay = new DateTime(iYear, DateTime.Now.AddMonths(-1).Month, DateTime.DaysInMonth(iYear, DateTime.Now.AddMonths(-1).Month));


            DateTime dtLastMonthCurrentYear = dtLastMonthLastDay;
            string sLastMonthCurrentYear = dtLastMonthLastDay.ToShortDateString();

            DateTime dtLastMonthLastYear = dtLastMonthLastDay.AddYears(-1);
            string sLastMonthLastYear = dtLastMonthLastYear.ToShortDateString();

            DateTime dtLastMonthLastYearMinusOne = dtLastMonthLastDay.AddYears(-2);
            string sLastMonthLastYearMinusOne = dtLastMonthLastYearMinusOne.ToShortDateString();


            DateTime dtLastDayofCurrentYear = dtLastDayOfCurrentYear;
            string sLastDayofCurrentYear = dtLastDayofCurrentYear.ToShortDateString();

            DateTime dtLastDayOfLastYear = dtLastDayOfCurrentYear.AddYears(-1);
            string sLastDayOfLastYear = dtLastDayOfLastYear.ToShortDateString();

            DateTime dtLastDayOfLastYearMinusOne = dtLastDayOfCurrentYear.AddYears(-2);
            string sLastDayOfLastYearMinusOne = dtLastDayOfLastYearMinusOne.ToShortDateString();

            LinkButton btnSort = (LinkButton)e.Row.Cells[1].Controls[0];//LM
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddMonths(-1).ToString("MMM yyyy").ToUpper().Replace(" ", "<br>");
            btnSort = (LinkButton)e.Row.Cells[2].Controls[0];//LMLY
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddMonths(-1).AddYears(-1).ToString("MMM yyyy").ToUpper().Replace(" ", "<br>");
            btnSort = (LinkButton)e.Row.Cells[3].Controls[0];//CM
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = dtCurrentMonthFirstDay.ToShortDateString() + "<br>to<br>" + dtCurrentMonthCurrentDay.ToShortDateString();
            btnSort = (LinkButton)e.Row.Cells[4].Controls[0];//CMLY
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = dtCurrentMonthFirstDayLastYear.ToShortDateString() + "<br>to<br>" + dtCurrentMonthCurrentDayLastYear.ToShortDateString();
            btnSort = (LinkButton)e.Row.Cells[5].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.ToString("MMM").ToUpper() + "<br>" + DateTime.Now.Year.ToString();
            btnSort = (LinkButton)e.Row.Cells[6].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.ToString("MMM").ToUpper() + "<br>" + DateTime.Now.AddYears(-1).Year.ToString();
            btnSort = (LinkButton)e.Row.Cells[7].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.Year.ToString() + " YTD";
            btnSort = (LinkButton)e.Row.Cells[8].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddYears(-1).Year.ToString() + " YTD";
            btnSort = (LinkButton)e.Row.Cells[9].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddYears(-2).Year.ToString() + " YTD";
            btnSort = (LinkButton)e.Row.Cells[10].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.Year.ToString() + "<br> to <br>" + DateTime.Now.AddYears(-1).Year.ToString() + "<br> DIFF %" + " YTD";
            btnSort = (LinkButton)e.Row.Cells[11].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddYears(-1).Year.ToString() + "<br> to <br>" + DateTime.Now.AddYears(-2).Year.ToString() + "<br> DIFF %" + " YTD"; //New Column 2 Years back...
            btnSort = (LinkButton)e.Row.Cells[12].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.Year.ToString() + "<br> MARGIN" + " YTD";
            btnSort = (LinkButton)e.Row.Cells[13].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddYears(-1).Year.ToString() + "<br> MARGIN" + " YTD";
            btnSort = (LinkButton)e.Row.Cells[14].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.Year.ToString() + " %" + "<br>YTD";
            btnSort = (LinkButton)e.Row.Cells[15].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddYears(-1).Year.ToString() + " %" + "<br>YTD";
            btnSort = (LinkButton)e.Row.Cells[16].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfCurrentYear + "<br>to<br>" + sLastMonthCurrentYear;
            btnSort = (LinkButton)e.Row.Cells[17].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfLastYear + "<br>to<br>" + sLastMonthLastYear;
            btnSort = (LinkButton)e.Row.Cells[18].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfLastYearMinusOne + "<br>to<br>" + sLastMonthLastYearMinusOne;//New 2 years back...
            btnSort = (LinkButton)e.Row.Cells[19].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.Year.ToString() + "<br>vs<br>" + DateTime.Now.AddYears(-1).Year.ToString() + "<br>Thru End of<br>Last Month";
            btnSort = (LinkButton)e.Row.Cells[20].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = DateTime.Now.AddYears(-1).Year.ToString() + "<br>vs<br>" + DateTime.Now.AddYears(-2).Year.ToString() + "<br>Thru End of<br>Last Month";//New column 2 years back...
            //New 11-10-2021...
            btnSort = (LinkButton)e.Row.Cells[21].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfMonthCurrentYear + "<br>to<br>" + sLastDayOfMonthCurrentYear + "<br>MARGIN";
            btnSort = (LinkButton)e.Row.Cells[22].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfLastMonthCurrentYear + "<br>to<br>" + sLastDayOfLastMonthCurrentYear + "<br>MARGIN";

            btnSort = (LinkButton)e.Row.Cells[23].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfCurrentYear + "<br>to<br>" + sLastDayofCurrentYear + "<br>MARGIN";
            btnSort = (LinkButton)e.Row.Cells[24].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfLastYear + "<br>to<br>" + sLastDayOfLastYear + "<br>MARGIN";
            btnSort = (LinkButton)e.Row.Cells[25].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfCurrentYear + "<br>to<br>" + sLastDayofCurrentYear + "<br>PERCENT";
            btnSort = (LinkButton)e.Row.Cells[26].Controls[0];
            btnSort.ToolTip = "Click to Sort";
            btnSort.Text = sFirstDayOfLastYear + "<br>to<br>" + sLastDayOfLastYear + "<br>PERCENT";

            e.Row.Cells[1].Visible = true;//LM
            e.Row.Cells[2].Visible = true;//LMLY

            if (!chkShowYTDthroughLastMonth.Checked)
            {

                e.Row.Cells[3].Visible = false;//LMTD
                e.Row.Cells[4].Visible = false;//LYLMTD
                e.Row.Cells[5].Visible = true;//MTD
                e.Row.Cells[6].Visible = true;//LYMTD

                //<% --YTD-- %>
                e.Row.Cells[7].Visible = true;//CM
                e.Row.Cells[8].Visible = true;//LYCM
                e.Row.Cells[9].Visible = true;//YTD
                e.Row.Cells[10].Visible = true;//LYTD
                e.Row.Cells[11].Visible = true;//LLYTD - New 1-28-2020
                e.Row.Cells[12].Visible = false;//YR to YR Diff -- Hide 11-10-2021
                e.Row.Cells[13].Visible = false;//YR to YR Diff Minus One -- Hide 11-10-2021
                e.Row.Cells[14].Visible = true;//YTD MARGIN
                e.Row.Cells[15].Visible = true;//LYTD MARGIN

                //<% --YTD End of Month-- %>
                e.Row.Cells[16].Visible = false;//YTDEM
                e.Row.Cells[17].Visible = false;//LYTDEM
                e.Row.Cells[18].Visible = false;//LLYTDEM
                e.Row.Cells[19].Visible = false;//YR to YR Diff EM -- Hide 11-10-2021
                e.Row.Cells[20].Visible = false;//YR to YR Diff EM Minus One -- Hide 11-10-2021

                e.Row.Cells[21].Visible = true;//CUR MO MARGIN -- new 11-10-2021...
                e.Row.Cells[22].Visible = true;//LAST MO MARGIN -- new 11-10-2021...

                e.Row.Cells[23].Visible = false;//YTD MARGIN EM
                e.Row.Cells[24].Visible = false;//LYTD MARGIN EM
                e.Row.Cells[25].Visible = false;//YTD % EM
                e.Row.Cells[26].Visible = false;//LYTD % EM
            }
            else//ShowYTDthroughLastMonth
            {
                e.Row.Cells[3].Visible = false;//LMTD
                e.Row.Cells[4].Visible = false;//LYLMTD
                e.Row.Cells[5].Visible = false;//MTD
                e.Row.Cells[6].Visible = false;//LYMTD

                //<% --YTD-- %>
                e.Row.Cells[7].Visible = false;//CM
                e.Row.Cells[8].Visible = false;//LYCM
                e.Row.Cells[9].Visible = false;//YTD
                e.Row.Cells[10].Visible = false;//LYTD
                e.Row.Cells[11].Visible = false;//LLYTD - New 1-28-2020
                e.Row.Cells[12].Visible = false;//YR to YR Diff -- Hide 11-10-2021
                e.Row.Cells[13].Visible = false;//YR to YR Diff Minus One -- Hide 11-10-2021
                e.Row.Cells[14].Visible = false;//YTD MARGIN
                e.Row.Cells[15].Visible = false;//LYTD MARGIN
                //<% --YTD End of Month-- %>
                e.Row.Cells[16].Visible = true;//YTDEM
                e.Row.Cells[17].Visible = true;//LYTDEM
                e.Row.Cells[18].Visible = true;//LLYTDEM
                e.Row.Cells[19].Visible = false;//YR to YR Diff EM -- Hide 11-10-2021
                e.Row.Cells[20].Visible = false;//YR to YR Diff EM Minus One -- Hide 11-10-2021

                e.Row.Cells[21].Visible = true;//CUR MO MARGIN -- new 11-10-2021...
                e.Row.Cells[22].Visible = true;//LAST MO MARGIN -- new 11-10-2021...

                e.Row.Cells[23].Visible = true;//YTD MARGIN EM
                e.Row.Cells[24].Visible = true;//LYTD MARGIN EM
                e.Row.Cells[25].Visible = true;//YTD % EM
                e.Row.Cells[26].Visible = true;//LYTD % EM
            }

            if (!chkShowLastColumns.Checked)
            {
                e.Row.Cells[27].Visible = false;
                e.Row.Cells[28].Visible = false;
                e.Row.Cells[29].Visible = false;
            }

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //YTD...
            Label lblCurrentYTDAmountD = (Label)e.Row.FindControl("lblCurrentYTDAmountD");
            if (lblCurrentYTDAmountD.Text != "")
            {
                dcCurrentYTD = Convert.ToDecimal(lblCurrentYTDAmountD.Text.Replace("$", ""));
                dcCurrentYTDTotal += dcCurrentYTD;
                lblCurrentYTDAmountD.Text = "$" + dcCurrentYTD.ToString("#,0");
            }
            //Last Year...
            Label lblLastYTDAmountD = (Label)e.Row.FindControl("lblLastYTDAmountD");
            if (lblLastYTDAmountD.Text != "")
            {
                dcLastYTD = Convert.ToDecimal(lblLastYTDAmountD.Text.Replace("$", ""));
                dcLastYTDTotal += dcLastYTD;
                lblLastYTDAmountD.Text = "$" + dcLastYTD.ToString("#,0");
            }
            //Two years back...
            Label lblLastYTDMinusOneAmountD = (Label)e.Row.FindControl("lblLastYTDMinusOneAmountD");
            if (lblLastYTDMinusOneAmountD.Text != "")
            {
                dcLastYTDMinusOne = Convert.ToDecimal(lblLastYTDMinusOneAmountD.Text.Replace("$", ""));
                dcLastYTDMinusOneTotal += dcLastYTDMinusOne;
                lblLastYTDMinusOneAmountD.Text = "$" + dcLastYTDMinusOne.ToString("#,0");
            }

            Label lblYearToYearDiffPercentage = (Label)e.Row.FindControl("lblYearToYearDiffPercentage");
            if (lblYearToYearDiffPercentage.Text != "")
            {
                dcYearToYearDiffPercentage = Convert.ToDecimal(lblYearToYearDiffPercentage.Text);
                lblYearToYearDiffPercentage.Text = lblYearToYearDiffPercentage.Text + "%";
                if (dcYearToYearDiffPercentage < 0)
                {
                    lblYearToYearDiffPercentage.ForeColor = Color.Red;
                }
            }
            Label lblYearToYearDiffPercentageMinusOne = (Label)e.Row.FindControl("lblYearToYearDiffPercentageMinusOne");
            if (lblYearToYearDiffPercentage.Text != "")
            {
                dcYearToYearDiffPercentageMinusOne = Convert.ToDecimal(lblYearToYearDiffPercentageMinusOne.Text);
                lblYearToYearDiffPercentageMinusOne.Text = lblYearToYearDiffPercentageMinusOne.Text + "%";
            }

            //New 11-11-2021...
            Label lblCurrentMonthMargin = (Label)e.Row.FindControl("lblCurrentMonthMargin");
            lblCurrentMonthMargin.Text = lblCurrentMonthMargin.Text + "%";

            Label lblLastMonthMargin = (Label)e.Row.FindControl("lblLastMonthMargin");
            lblLastMonthMargin.Text = lblLastMonthMargin.Text + "%";

            //YTD End of Month...
            Label lblCurrentYTDEndOfMonthAmountD = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthAmountD");
            if (lblCurrentYTDEndOfMonthAmountD.Text != "")
            {
                dcCurrentYTDEndOfMonth = Convert.ToDecimal(lblCurrentYTDEndOfMonthAmountD.Text.Replace("$", ""));
                dcCurrentYTDEndOfMonthTotal += dcCurrentYTDEndOfMonth;
                lblCurrentYTDEndOfMonthAmountD.Text = "$" + dcCurrentYTDEndOfMonth.ToString("#,0");
            }
            Label lblLastYTDEndOfMonthAmountD = (Label)e.Row.FindControl("lblLastYTDEndOfMonthAmountD");
            if (lblCurrentYTDEndOfMonthAmountD.Text != "")
            {
                dcLastYTDEndOfMonth = Convert.ToDecimal(lblLastYTDEndOfMonthAmountD.Text.Replace("$", ""));
                dcLastYTDEndOfMonthTotal += dcLastYTDEndOfMonth;
                lblLastYTDEndOfMonthAmountD.Text = "$" + dcLastYTDEndOfMonth.ToString("#,0");
            }

            Label lblLastYTDMinusOneYearEndOfMonthAmountD = (Label)e.Row.FindControl("lblLastYTDMinusOneYearEndOfMonthAmountD");
            if (lblLastYTDMinusOneYearEndOfMonthAmountD.Text != "")
            {
                dcLastYTDMinusOneYearEndOfMonth = Convert.ToDecimal(lblLastYTDMinusOneYearEndOfMonthAmountD.Text.Replace("$", ""));
                dcLastYTDMinusOneYearEndOfMonthAmountTotal += dcLastYTDMinusOneYearEndOfMonth;
                lblLastYTDMinusOneYearEndOfMonthAmountD.Text = "$" + dcLastYTDMinusOneYearEndOfMonth.ToString("#,0");
            }

            Label lblYearToYearDiffPercentageEndOfMonth = (Label)e.Row.FindControl("lblYearToYearDiffPercentageEndOfMonth");
            if (lblYearToYearDiffPercentageEndOfMonth.Text != "")
            {
                dcYearToYearDiffPercentageEndOfMonth = Convert.ToDecimal(lblYearToYearDiffPercentageEndOfMonth.Text);
                lblYearToYearDiffPercentageEndOfMonth.Text = lblYearToYearDiffPercentageEndOfMonth.Text + "%";
                if (dcYearToYearDiffPercentageEndOfMonth < 0)
                {
                    lblYearToYearDiffPercentageEndOfMonth.ForeColor = Color.Red;
                }
            }
            Label lblYearToYearDiffPercentageEndOfMonthMinusOne = (Label)e.Row.FindControl("lblYearToYearDiffPercentageEndOfMonthMinusOne");
            if (lblYearToYearDiffPercentageEndOfMonthMinusOne.Text != "")
            {
                dcYearToYearDiffPercentageEndOfMonthMinusOne = Convert.ToDecimal(lblYearToYearDiffPercentageEndOfMonthMinusOne.Text);
                lblYearToYearDiffPercentageEndOfMonthMinusOne.Text = lblYearToYearDiffPercentageEndOfMonthMinusOne.Text + "%";

            }


            //MTD...
            Label lblCurrentMTDAmountD = (Label)e.Row.FindControl("lblCurrentMTDAmountD");
            if (lblCurrentMTDAmountD.Text != "")
            {
                dcCurrentMTD = Convert.ToDecimal(lblCurrentMTDAmountD.Text.Replace("$", ""));
                dcCurrentMTDTotal += dcCurrentMTD;
                lblCurrentMTDAmountD.Text = "$" + dcCurrentMTD.ToString("#,0");
            }

            Label lblLastYearCurrentMTDAmountD = (Label)e.Row.FindControl("lblLastYearCurrentMTDAmountD");
            if (lblLastYearCurrentMTDAmountD.Text != "")
            {
                dcLastYearCurrentMTD = Convert.ToDecimal(lblLastYearCurrentMTDAmountD.Text.Replace("$", ""));
                dcLastYearCurrentMTDTotal += dcLastYearCurrentMTD;
                lblLastYearCurrentMTDAmountD.Text = "$" + dcLastYearCurrentMTD.ToString("#,0");
            }

            //Entire Current Month...
            Label lblCurrentMonthAmountD = (Label)e.Row.FindControl("lblCurrentMonthAmountD");
            if (lblCurrentMonthAmountD.Text != "")
            {
                dcCurrentMonthAmount = Convert.ToDecimal(lblCurrentMonthAmountD.Text.Replace("$", ""));
                dcCurrentMonthTotal += dcCurrentMonthAmount;
                lblCurrentMonthAmountD.Text = "$" + dcCurrentMonthAmount.ToString("#,0");
            }

            Label lblLastYearCurrentMonthAmountD = (Label)e.Row.FindControl("lblLastYearCurrentMonthAmountD");
            if (lblLastYearCurrentMonthAmountD.Text != "")
            {
                dcLastYearCurrentMonth = Convert.ToDecimal(lblLastYearCurrentMonthAmountD.Text.Replace("$", ""));
                dcLastYearCurrentMonthTotal += dcLastYearCurrentMonth;
                lblLastYearCurrentMonthAmountD.Text = "$" + dcLastYearCurrentMonth.ToString("#,0");
            }

            //Entire Previous Month...
            Label lblPreviousMonthAmountD = (Label)e.Row.FindControl("lblPreviousMonthAmountD");
            if (lblPreviousMonthAmountD.Text != "")
            {
                dcPreviousMonthAmount = Convert.ToDecimal(lblPreviousMonthAmountD.Text.Replace("$", ""));
                dcPreviousMonthTotal += dcPreviousMonthAmount;
                lblPreviousMonthAmountD.Text = "$" + dcPreviousMonthAmount.ToString("#,0");
            }

            //New 11-10-2021...
            Label lblCurrentMonthCost = (Label)e.Row.FindControl("lblCurrentMonthCost");
            if (lblCurrentMonthCost.Text != "")
            {
                dcCurrentMonthCost = Convert.ToDecimal(lblCurrentMonthCost.Text.Replace("$", ""));
                dcCurrentMonthCostTotal += dcCurrentMonthCost;
                lblCurrentMonthCost.Text = "$" + dcCurrentMonthCost.ToString("#,0");
            }
            Label lblPreviousMonthCost = (Label)e.Row.FindControl("lblPreviousMonthCost");
            if (lblPreviousMonthCost.Text != "")
            {
                dcPreviousMonthCost = Convert.ToDecimal(lblPreviousMonthCost.Text.Replace("$", ""));
                dcPreviousMonthCostTotal += dcPreviousMonthCost;
                lblPreviousMonthCost.Text = "$" + dcPreviousMonthCost.ToString("#,0");
            }



            Label lblLastYearPreviousMonthAmountD = (Label)e.Row.FindControl("lblLastYearPreviousMonthAmountD");
            if (lblLastYearPreviousMonthAmountD.Text != "")
            {
                dcLastYearLastMonth = Convert.ToDecimal(lblLastYearPreviousMonthAmountD.Text.Replace("$", ""));
                dcLastYearPreviousMonthTotal += dcLastYearLastMonth;
                lblLastYearPreviousMonthAmountD.Text = "$" + dcLastYearLastMonth.ToString("#,0");
            }


            //Other...
            Label lblReadyToInvoiceAmountD = (Label)e.Row.FindControl("lblReadyToInvoiceAmountD");
            if (lblReadyToInvoiceAmountD.Text != "")
            {
                dcReadyToInvoice = Convert.ToDecimal(lblReadyToInvoiceAmountD.Text.Replace("$", ""));
                dcReadyToInvoiceTotal += dcReadyToInvoice;
                lblReadyToInvoiceAmountD.Text = "$" + dcReadyToInvoice.ToString("#,0");
            }
            Label lblOpenOrdersAmountD = (Label)e.Row.FindControl("lblOpenOrdersAmountD");
            if (lblOpenOrdersAmountD.Text != "")
            {
                dcOpenOrders = Convert.ToDecimal(lblOpenOrdersAmountD.Text.Replace("$", ""));
                dcOpenOrdersTotal += dcOpenOrders;
                lblOpenOrdersAmountD.Text = "$" + dcOpenOrders.ToString("#,0");
            }
            Label lblCombinedOpenOrdersAmountD = (Label)e.Row.FindControl("lblCombinedOpenOrdersAmountD");
            lblCombinedOpenOrdersAmountD.Text = "$" + lblCombinedOpenOrdersAmountD.Text;
            Label lblGrandTotalCombinedOpenOrdersAmount = (Label)e.Row.FindControl("lblGrandTotalCombinedOpenOrdersAmount");
            if (lblGrandTotalCombinedOpenOrdersAmount.Text != "")
            {
                dcCombinedOpenOrdersAmount = Convert.ToDecimal(lblGrandTotalCombinedOpenOrdersAmount.Text.Replace("$", ""));

                //lblOpenOrdersAmountD.Text = "$" + dcOpenOrders.ToString("#,0");
            }



            //YTD...
            Label lblCurrentYTDCost = (Label)e.Row.FindControl("lblCurrentYTDCost");
            if (lblCurrentYTDCost.Text != "")
            {
                dcYTDCost = Convert.ToDecimal(lblCurrentYTDCost.Text.Replace("$", ""));
                dcYTDCostTotal += dcYTDCost;
            }

            Label lblLastYTDCost = (Label)e.Row.FindControl("lblLastYTDCost");
            if (lblLastYTDCost.Text != "")
            {
                dcLastYTDCost = Convert.ToDecimal(lblLastYTDCost.Text.Replace("$", ""));
                dcLastYTDCostTotal += dcLastYTDCost;
            }

            //YTD End of Month...
            Label lblCurrentYTDEndOfMonthCost = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthCost");
            if (lblCurrentYTDEndOfMonthCost.Text != "")
            {
                dcYTDEndOfMonthCost = Convert.ToDecimal(lblCurrentYTDEndOfMonthCost.Text.Replace("$", ""));
                dcYTDEndOfMonthCostTotal += dcYTDEndOfMonthCost;
            }

            Label lblLastYTDEndOfMonthCost = (Label)e.Row.FindControl("lblLastYTDEndOfMonthCost");
            if (lblLastYTDEndOfMonthCost.Text != "")
            {
                dcLastYTDEndOfMonthCost = Convert.ToDecimal(lblLastYTDEndOfMonthCost.Text.Replace("$", ""));
                dcLastYTDEndOfMonthCostTotal += dcLastYTDEndOfMonthCost;
            }

            Label lblCurrentYTDMargin = (Label)e.Row.FindControl("lblCurrentYTDMargin");
            lblCurrentYTDMargin.Text = lblCurrentYTDMargin.Text + "%";
            Label lblLastYTDMargin = (Label)e.Row.FindControl("lblLastYTDMargin");
            lblLastYTDMargin.Text = lblLastYTDMargin.Text + "%";

            Label lblCurrentYTDEndOfMonthMargin = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthMargin");
            lblCurrentYTDEndOfMonthMargin.Text = lblCurrentYTDEndOfMonthMargin.Text + "%";
            Label lblLastYTDEndOfMonthMargin = (Label)e.Row.FindControl("lblLastYTDEndOfMonthMargin");
            lblLastYTDEndOfMonthMargin.Text = lblLastYTDEndOfMonthMargin.Text + "%";

            Label lblCurrentYTDAmountPercentageOfTotal = (Label)e.Row.FindControl("lblCurrentYTDAmountPercentageOfTotal");
            dcCurrentYTDAmountPercentageOfTotal = Convert.ToDecimal(lblCurrentYTDAmountPercentageOfTotal.Text);
            dcCurrentYTDAmountPercentageOfTotalSum += dcCurrentYTDAmountPercentageOfTotal;
            lblCurrentYTDAmountPercentageOfTotal.Text = lblCurrentYTDAmountPercentageOfTotal.Text + "%";

            Label lblLastYTDAmountPercentageOfTotal = (Label)e.Row.FindControl("lblLastYTDAmountPercentageOfTotal");
            dcLastYTDAmountPercentageOfTotal = Convert.ToDecimal(lblLastYTDAmountPercentageOfTotal.Text);
            dcLastYTDAmountPercentageOfTotalSum += dcLastYTDAmountPercentageOfTotal;
            lblLastYTDAmountPercentageOfTotal.Text = lblLastYTDAmountPercentageOfTotal.Text + "%";

            Label lblCurrentYTDEndOfMonthAmountPercentageOfTotal = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthAmountPercentageOfTotal");
            dcCurrentYTDEndOfMonthAmountPercentageOfTotal = Convert.ToDecimal(lblCurrentYTDEndOfMonthAmountPercentageOfTotal.Text);
            dcCurrentYTDEndOfMonthAmountPercentageOfTotalSum += dcCurrentYTDEndOfMonthAmountPercentageOfTotal;
            lblCurrentYTDEndOfMonthAmountPercentageOfTotal.Text = lblCurrentYTDEndOfMonthAmountPercentageOfTotal.Text + "%";

            Label lblLastYTDEndOfMonthAmountPercentageOfTotal = (Label)e.Row.FindControl("lblLastYTDEndOfMonthAmountPercentageOfTotal");
            dcLastYTDEndOfMonthAmountPercentageOfTotal = Convert.ToDecimal(lblLastYTDEndOfMonthAmountPercentageOfTotal.Text);
            dcLastYTDEndOfMonthAmountPercentageOfTotalSum += dcLastYTDEndOfMonthAmountPercentageOfTotal;
            lblLastYTDEndOfMonthAmountPercentageOfTotal.Text = lblLastYTDEndOfMonthAmountPercentageOfTotal.Text + "%";


            e.Row.Cells[1].Visible = true;//LM
            e.Row.Cells[2].Visible = true;//LMLY

            if (!chkShowYTDthroughLastMonth.Checked)
            {

                e.Row.Cells[3].Visible = false;//LMTD
                e.Row.Cells[4].Visible = false;//LYLMTD
                e.Row.Cells[5].Visible = true;//MTD
                e.Row.Cells[6].Visible = true;//LYMTD

                //<% --YTD-- %>
                e.Row.Cells[7].Visible = true;//CM
                e.Row.Cells[8].Visible = true;//LYCM
                e.Row.Cells[9].Visible = true;//YTD
                e.Row.Cells[10].Visible = true;//LYTD
                e.Row.Cells[11].Visible = true;//LLYTD - New 1-28-2020
                e.Row.Cells[12].Visible = false;//YR to YR Diff -- Hide 11-10-2021
                e.Row.Cells[13].Visible = false;//YR to YR Diff Minus One -- Hide 11-10-2021
                e.Row.Cells[14].Visible = true;//YTD MARGIN
                e.Row.Cells[15].Visible = true;//LYTD MARGIN

                //<% --YTD End of Month-- %>
                e.Row.Cells[16].Visible = false;//YTDEM
                e.Row.Cells[17].Visible = false;//LYTDEM
                e.Row.Cells[18].Visible = false;//LLYTDEM
                e.Row.Cells[19].Visible = false;//YR to YR Diff EM -- Hide 11-10-2021
                e.Row.Cells[20].Visible = false;//YR to YR Diff EM Minus One -- Hide 11-10-2021

                e.Row.Cells[21].Visible = true;//CUR MO MARGIN -- new 11-10-2021...
                e.Row.Cells[22].Visible = true;//LAST MO MARGIN -- new 11-10-2021...

                e.Row.Cells[23].Visible = false;//YTD MARGIN EM
                e.Row.Cells[24].Visible = false;//LYTD MARGIN EM
                e.Row.Cells[25].Visible = false;//YTD % EM
                e.Row.Cells[26].Visible = false;//LYTD % EM
            }
            else//ShowYTDthroughLastMonth
            {
                e.Row.Cells[3].Visible = false;//LMTD
                e.Row.Cells[4].Visible = false;//LYLMTD
                e.Row.Cells[5].Visible = false;//MTD
                e.Row.Cells[6].Visible = false;//LYMTD

                //<% --YTD-- %>
                e.Row.Cells[7].Visible = false;//CM
                e.Row.Cells[8].Visible = false;//LYCM
                e.Row.Cells[9].Visible = false;//YTD
                e.Row.Cells[10].Visible = false;//LYTD
                e.Row.Cells[11].Visible = false;//LLYTD - New 1-28-2020
                e.Row.Cells[12].Visible = false;//YR to YR Diff -- Hide 11-10-2021
                e.Row.Cells[13].Visible = false;//YR to YR Diff Minus One -- Hide 11-10-2021
                e.Row.Cells[14].Visible = false;//YTD MARGIN
                e.Row.Cells[15].Visible = false;//LYTD MARGIN
                //<% --YTD End of Month-- %>
                e.Row.Cells[16].Visible = true;//YTDEM
                e.Row.Cells[17].Visible = true;//LYTDEM
                e.Row.Cells[18].Visible = true;//LLYTDEM
                e.Row.Cells[19].Visible = false;//YR to YR Diff EM -- Hide 11-10-2021
                e.Row.Cells[20].Visible = false;//YR to YR Diff EM Minus One -- Hide 11-10-2021

                e.Row.Cells[21].Visible = true;//CUR MO MARGIN -- new 11-10-2021...
                e.Row.Cells[22].Visible = true;//LAST MO MARGIN -- new 11-10-2021...

                e.Row.Cells[23].Visible = true;//YTD MARGIN EM
                e.Row.Cells[24].Visible = true;//LYTD MARGIN EM
                e.Row.Cells[25].Visible = true;//YTD % EM
                e.Row.Cells[26].Visible = true;//LYTD % EM
            }

            if (!chkShowLastColumns.Checked)
            {
                e.Row.Cells[27].Visible = false;
                e.Row.Cells[28].Visible = false;
                e.Row.Cells[29].Visible = false;
            }


            Label lblName = (Label)e.Row.FindControl("lblName");
            switch (lblName.Text.Trim())
            {
                case "SMASHBURGER":
                    lblName.ForeColor = Color.Red;
                    break;
                case "KRISPY KREME":
                    lblName.ForeColor = Color.Red;
                    break;
                case "IN & OUT":
                    lblName.ForeColor = Color.Blue;
                    break;
                case "BAKEMARK":
                    lblName.ForeColor = Color.Blue;
                    break;
                case "SYSCO":
                    lblName.ForeColor = Color.Blue;
                    break;
                case "ARYZTA":
                    lblName.ForeColor = Color.Blue;
                    break;
                case "US FOODS":
                    lblName.ForeColor = Color.Blue;
                    break;
                default:
                    break;
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            //YTD...
            Label lblCurrentYTDAmountTotal = (Label)e.Row.FindControl("lblCurrentYTDAmountTotal");
            lblCurrentYTDAmountTotal.Text = "$" + dcCurrentYTDTotal.ToString("#,0");

            Label lblLastYTDAmountTotal = (Label)e.Row.FindControl("lblLastYTDAmountTotal");
            lblLastYTDAmountTotal.Text = "$" + dcLastYTDTotal.ToString("#,0");

            Label lblLastYTDMinusOneAmountTotal = (Label)e.Row.FindControl("lblLastYTDMinusOneAmountTotal");
            lblLastYTDMinusOneAmountTotal.Text = "$" + dcLastYTDMinusOneTotal.ToString("#,0");

            //YTD End of Month...
            Label lblCurrentYTDEndOfMonthAmountTotal = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthAmountTotal");
            lblCurrentYTDEndOfMonthAmountTotal.Text = "$" + dcCurrentYTDEndOfMonthTotal.ToString("#,0");

            Label lblLastYTDEndOfMonthAmountTotal = (Label)e.Row.FindControl("lblLastYTDEndOfMonthAmountTotal");
            lblLastYTDEndOfMonthAmountTotal.Text = "$" + dcLastYTDEndOfMonthTotal.ToString("#,0");

            Label lblLastYTDMinusOneYearEndOfMonthAmountTotal = (Label)e.Row.FindControl("lblLastYTDMinusOneYearEndOfMonthAmountTotal");
            lblLastYTDMinusOneYearEndOfMonthAmountTotal.Text = "$" + dcLastYTDMinusOneYearEndOfMonthAmountTotal.ToString("#,0");

            //MTD...
            Label lblCurrentMTDAmountTotal = (Label)e.Row.FindControl("lblCurrentMTDAmountTotal");
            lblCurrentMTDAmountTotal.Text = "$" + dcCurrentMTDTotal.ToString("#,0");

            Label lblLastYearCurrentMTDAmountTotal = (Label)e.Row.FindControl("lblLastYearCurrentMTDAmountTotal");
            lblLastYearCurrentMTDAmountTotal.Text = "$" + dcLastYearCurrentMTDTotal.ToString("#,0");

            //Entire Current Month...
            Label lblCurrentMonthAmountTotal = (Label)e.Row.FindControl("lblCurrentMonthAmountTotal");
            lblCurrentMonthAmountTotal.Text = "$" + dcCurrentMonthTotal.ToString("#,0");

            Label lblLastYearCurrentMonthAmountTotal = (Label)e.Row.FindControl("lblLastYearCurrentMonthAmountTotal");
            lblLastYearCurrentMonthAmountTotal.Text = "$" + dcLastYearCurrentMonthTotal.ToString("#,0");


            //Entire Previous Month...
            Label lblPreviousMonthAmountTotal = (Label)e.Row.FindControl("lblPreviousMonthAmountTotal");
            lblPreviousMonthAmountTotal.Text = "$" + dcPreviousMonthTotal.ToString("#,0");

            Label lblLastYearPreviousMonthAmountTotal = (Label)e.Row.FindControl("lblLastYearPreviousMonthAmountTotal");
            lblLastYearPreviousMonthAmountTotal.Text = "$" + dcLastYearPreviousMonthTotal.ToString("#,0");



            //Other...
            Label lblReadyToInvoiceAmountTotal = (Label)e.Row.FindControl("lblReadyToInvoiceAmountTotal");
            lblReadyToInvoiceAmountTotal.Text = "$" + dcReadyToInvoiceTotal.ToString("#,0");
            Label lblOpenOrdersAmountTotal = (Label)e.Row.FindControl("lblOpenOrdersAmountTotal");
            lblOpenOrdersAmountTotal.Text = "$" + dcOpenOrdersTotal.ToString("#,0");

            Label lblCombinedOpenOrdersAmountDTotal = (Label)e.Row.FindControl("lblCombinedOpenOrdersAmountDTotal");
            lblCombinedOpenOrdersAmountDTotal.Text = "$" + dcCombinedOpenOrdersAmount.ToString("#,0");


            //New 11-10-2021...
            //Current Month Margin...
            Label lblCurrentMonthMarginWeighted = (Label)e.Row.FindControl("lblCurrentMonthMarginWeighted");
            if (dcCurrentMonthTotal == 0)
            {
                dcCurrentMonthTotal = 1;
            }
            dcCurrentMonthWeightedMargin = ((dcCurrentMonthTotal - dcCurrentMonthCostTotal) / dcCurrentMonthTotal) * 100;
            lblCurrentMonthMarginWeighted.Text = dcCurrentMonthWeightedMargin.ToString("0.0") + "%";

            //Last Month Margin...
            Label lblLastMonthMarginWeighted = (Label)e.Row.FindControl("lblLastMonthMarginWeighted");
            if (dcPreviousMonthTotal == 0)
            {
                dcPreviousMonthTotal = 1;
            }
            dcLastMonthWeightedMargin = ((dcPreviousMonthTotal - dcPreviousMonthCostTotal) / dcPreviousMonthTotal) * 100;
            lblLastMonthMarginWeighted.Text = dcLastMonthWeightedMargin.ToString("0.0") + "%";


            //YTD...
            Label lblCurrentYTDMarginWeighted = (Label)e.Row.FindControl("lblCurrentYTDMarginWeighted");
            if (dcCurrentYTDTotal == 0)
            {
                dcCurrentYTDTotal = 1;
            }
            dcYTDWeightedMargin = ((dcCurrentYTDTotal - dcYTDCostTotal) / dcCurrentYTDTotal) * 100;
            lblCurrentYTDMarginWeighted.Text = dcYTDWeightedMargin.ToString("0") + "%";

            Label lblLastYTDAmountMarginWeighted = (Label)e.Row.FindControl("lblLastYTDAmountMarginWeighted");
            if (dcLastYTDTotal == 0)
            {
                dcLastYTDTotal = 1;
            }

            dcLastYTDWeightedMargin = ((dcLastYTDTotal - dcLastYTDCostTotal) / dcLastYTDTotal) * 100;
            lblLastYTDAmountMarginWeighted.Text = dcLastYTDWeightedMargin.ToString("0") + "%";

            //YTD End of Month...
            Label lblCurrentYTDEndOfMonthMarginWeighted = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthMarginWeighted");
            if (dcCurrentYTDEndOfMonthTotal == 0)
            {
                dcCurrentYTDEndOfMonthTotal = 1;
            }
            dcYTDEndOfMonthWeightedMargin = ((dcCurrentYTDEndOfMonthTotal - dcYTDEndOfMonthCostTotal) / dcCurrentYTDEndOfMonthTotal) * 100;
            lblCurrentYTDEndOfMonthMarginWeighted.Text = dcYTDEndOfMonthWeightedMargin.ToString("0.0") + "%";

            Label lblLastYTDEndOfMonthAmountMarginWeighted = (Label)e.Row.FindControl("lblLastYTDEndOfMonthAmountMarginWeighted");
            if (dcLastYTDEndOfMonthTotal == 0)
            {
                dcLastYTDEndOfMonthTotal = 1;
            }

            dcLastYTDEndOfMonthWeightedMargin = ((dcLastYTDEndOfMonthTotal - dcLastYTDEndOfMonthCostTotal) / dcLastYTDEndOfMonthTotal) * 100;
            lblLastYTDEndOfMonthAmountMarginWeighted.Text = dcLastYTDEndOfMonthWeightedMargin.ToString("0.0") + "%";

            //Weighted Year to Year...
            Label lblYearToYearDiffPercentageWeighted = (Label)e.Row.FindControl("lblYearToYearDiffPercentageWeighted");
            dcYearToYearDiffPercentageWeighted = ((dcCurrentYTDTotal - dcLastYTDTotal) / dcLastYTDTotal) * 100;
            lblYearToYearDiffPercentageWeighted.Text = dcYearToYearDiffPercentageWeighted.ToString("0") + "%";

            Label lblYearToYearDiffPercentageMinusOneWeighted = (Label)e.Row.FindControl("lblYearToYearDiffPercentageMinusOneWeighted");
            if (dcLastYTDMinusOneTotal == 0)
            {
                dcLastYTDMinusOneTotal = 1;
            }
            dcYearToYearDiffPercentageMinusOneWeighted = ((dcLastYTDTotal - dcLastYTDMinusOneTotal) / dcLastYTDMinusOneTotal) * 100;
            lblYearToYearDiffPercentageMinusOneWeighted.Text = dcYearToYearDiffPercentageMinusOneWeighted.ToString("0") + "%";

            //Weighted Year to Year End of Month...
            Label lblYearToYearDiffPercentageEndOfMonthWeighted = (Label)e.Row.FindControl("lblYearToYearDiffPercentageEndOfMonthWeighted");
            if (dcLastYTDEndOfMonthTotal == 0)
            {
                dcLastYTDEndOfMonthTotal = 1;
            }
            dcYearToYearDiffPercentageEndOfMonthWeighted = ((dcCurrentYTDEndOfMonthTotal - dcLastYTDEndOfMonthTotal) / dcLastYTDEndOfMonthTotal) * 100;
            lblYearToYearDiffPercentageEndOfMonthWeighted.Text = dcYearToYearDiffPercentageEndOfMonthWeighted.ToString("0") + "%";

            if (dcLastYTDMinusOneYearEndOfMonthAmountTotal == 0)
            {
                dcLastYTDMinusOneYearEndOfMonthAmountTotal = 1;
            }

            Label lblYearToYearDiffPercentageEndOfMonthMinusOneWeighted = (Label)e.Row.FindControl("lblYearToYearDiffPercentageEndOfMonthMinusOneWeighted");
            dcYearToYearDiffPercentageEndOfMonthMinusOneWeighted = ((dcLastYTDEndOfMonthTotal - dcLastYTDMinusOneYearEndOfMonthAmountTotal) / dcLastYTDMinusOneYearEndOfMonthAmountTotal) * 100;
            lblYearToYearDiffPercentageEndOfMonthMinusOneWeighted.Text = dcYearToYearDiffPercentageEndOfMonthMinusOneWeighted.ToString("0") + "%";

            //Totals sums...

            Label lblCurrentYTDAmountPercentageOfTotalSum = (Label)e.Row.FindControl("lblCurrentYTDAmountPercentageOfTotalSum");
            if (dcCurrentYTDAmountPercentageOfTotalSum > 98)
            {
                dcCurrentYTDAmountPercentageOfTotalSum = 100;
            }
            lblCurrentYTDAmountPercentageOfTotalSum.Text = dcCurrentYTDAmountPercentageOfTotalSum.ToString("0") + "%";

            if (dcLastYTDAmountPercentageOfTotalSum > 98)
            {
                dcLastYTDAmountPercentageOfTotalSum = 100;
            }
            Label lblLastYTDAmountPercentageOfTotalSum = (Label)e.Row.FindControl("lblLastYTDAmountPercentageOfTotalSum");
            lblLastYTDAmountPercentageOfTotalSum.Text = dcLastYTDAmountPercentageOfTotalSum.ToString("0") + "%";
            if (dcCurrentYTDEndOfMonthAmountPercentageOfTotalSum > 98)
            {
                dcCurrentYTDEndOfMonthAmountPercentageOfTotalSum = 100;
            }

            Label lblCurrentYTDEndOfMonthAmountPercentageOfTotalSum = (Label)e.Row.FindControl("lblCurrentYTDEndOfMonthAmountPercentageOfTotalSum");
            lblCurrentYTDEndOfMonthAmountPercentageOfTotalSum.Text = dcCurrentYTDEndOfMonthAmountPercentageOfTotalSum.ToString("0") + "%";

            if (dcLastYTDEndOfMonthAmountPercentageOfTotalSum > 98)
            {
                dcLastYTDEndOfMonthAmountPercentageOfTotalSum = 100;
            }
            Label lblLastYTDEndOfMonthAmountPercentageOfTotalSum = (Label)e.Row.FindControl("lblLastYTDEndOfMonthAmountPercentageOfTotalSum");
            lblLastYTDEndOfMonthAmountPercentageOfTotalSum.Text = dcLastYTDEndOfMonthAmountPercentageOfTotalSum.ToString("0") + "%";

            e.Row.Cells[1].Visible = true;//LM
            e.Row.Cells[2].Visible = true;//LMLY

            if (!chkShowYTDthroughLastMonth.Checked)
            {

                e.Row.Cells[3].Visible = false;//LMTD
                e.Row.Cells[4].Visible = false;//LYLMTD
                e.Row.Cells[5].Visible = true;//MTD
                e.Row.Cells[6].Visible = true;//LYMTD

                //<% --YTD-- %>
                e.Row.Cells[7].Visible = true;//CM
                e.Row.Cells[8].Visible = true;//LYCM
                e.Row.Cells[9].Visible = true;//YTD
                e.Row.Cells[10].Visible = true;//LYTD
                e.Row.Cells[11].Visible = true;//LLYTD - New 1-28-2020
                e.Row.Cells[12].Visible = false;//YR to YR Diff -- Hide 11-10-2021
                e.Row.Cells[13].Visible = false;//YR to YR Diff Minus One -- Hide 11-10-2021
                e.Row.Cells[14].Visible = true;//YTD MARGIN
                e.Row.Cells[15].Visible = true;//LYTD MARGIN

                //<% --YTD End of Month-- %>
                e.Row.Cells[16].Visible = false;//YTDEM
                e.Row.Cells[17].Visible = false;//LYTDEM
                e.Row.Cells[18].Visible = false;//LLYTDEM
                e.Row.Cells[19].Visible = false;//YR to YR Diff EM -- Hide 11-10-2021
                e.Row.Cells[20].Visible = false;//YR to YR Diff EM Minus One -- Hide 11-10-2021

                e.Row.Cells[21].Visible = true;//CUR MO MARGIN -- new 11-10-2021...
                e.Row.Cells[22].Visible = true;//LAST MO MARGIN -- new 11-10-2021...

                e.Row.Cells[23].Visible = false;//YTD MARGIN EM
                e.Row.Cells[24].Visible = false;//LYTD MARGIN EM
                e.Row.Cells[25].Visible = false;//YTD % EM
                e.Row.Cells[26].Visible = false;//LYTD % EM
            }
            else//ShowYTDthroughLastMonth
            {
                e.Row.Cells[3].Visible = false;//LMTD
                e.Row.Cells[4].Visible = false;//LYLMTD
                e.Row.Cells[5].Visible = false;//MTD
                e.Row.Cells[6].Visible = false;//LYMTD

                //<% --YTD-- %>
                e.Row.Cells[7].Visible = false;//CM
                e.Row.Cells[8].Visible = false;//LYCM
                e.Row.Cells[9].Visible = false;//YTD
                e.Row.Cells[10].Visible = false;//LYTD
                e.Row.Cells[11].Visible = false;//LLYTD - New 1-28-2020
                e.Row.Cells[12].Visible = false;//YR to YR Diff -- Hide 11-10-2021
                e.Row.Cells[13].Visible = false;//YR to YR Diff Minus One -- Hide 11-10-2021
                e.Row.Cells[14].Visible = false;//YTD MARGIN
                e.Row.Cells[15].Visible = false;//LYTD MARGIN
                //<% --YTD End of Month-- %>
                e.Row.Cells[16].Visible = true;//YTDEM
                e.Row.Cells[17].Visible = true;//LYTDEM
                e.Row.Cells[18].Visible = true;//LLYTDEM
                e.Row.Cells[19].Visible = false;//YR to YR Diff EM -- Hide 11-10-2021
                e.Row.Cells[20].Visible = false;//YR to YR Diff EM Minus One -- Hide 11-10-2021

                e.Row.Cells[21].Visible = true;//CUR MO MARGIN -- new 11-10-2021...
                e.Row.Cells[22].Visible = true;//LAST MO MARGIN -- new 11-10-2021...

                e.Row.Cells[23].Visible = true;//YTD MARGIN EM
                e.Row.Cells[24].Visible = true;//LYTD MARGIN EM
                e.Row.Cells[25].Visible = true;//YTD % EM
                e.Row.Cells[26].Visible = true;//LYTD % EM
            }

            if (!chkShowLastColumns.Checked)
            {
                e.Row.Cells[27].Visible = false;
                e.Row.Cells[28].Visible = false;
                e.Row.Cells[29].Visible = false;
            }

        }
    }
    protected void gvTopFifteen_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        dtSortTable = (DataTable)Session["dtTopFifteen"];
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvTopFifteen.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            Session["Sort"] = e.SortExpression + " " + m_SortDirection;
            gvTopFifteen.DataSource = m_DataView;
            gvTopFifteen.DataBind();
            gvTopFifteen.PageIndex = m_PageIndex;
            Session["dtSortAdd"] = m_DataTable;
        }
    }
    protected void btnExportTopFifteenList_Click(object sender, EventArgs e)
    {
        ExportTopFifteenList();
    }

    protected void chkShowLastColumns_CheckedChanged(object sender, EventArgs e)
    {
        LoadTopFifteenData(rblTopTen.SelectedValue);
    }
    protected void chkMonthToDate_CheckedChanged(object sender, EventArgs e)
    {
        LoadTopFifteenData(rblTopTen.SelectedValue);
    }
    protected void chkShowYTDthroughLastMonth_CheckedChanged(object sender, EventArgs e)
    {
        LoadTopFifteenData(rblTopTen.SelectedValue);
    }
    //Compton Stuff 12-25-2019...
    //Picking...

    protected void rblCompton_SelectedIndexChanged(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        int iRoleID = Convert.ToInt32(Session["RoleID"]);

        lblError.Text = "";
        lblError0.Text = "";

        SetupHeaderWidths(iUserID);

        RunSearch();

    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        UpdateSOT();

    }

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        RunSearch();
    }
    protected void lbnSearch_Click(object sender, EventArgs e)
    {
        int iUserID = Convert.ToInt32(Session["UserID"]);
        RunSearch();
        SetupHeaderWidths(iUserID);
    }
    protected void lbnReset_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        ddlSort1.SelectedIndex = 0;
        ddlSort2.SelectedIndex = 0;
        ddlSort3.SelectedIndex = 0;
        RunSearch();
    }

    protected void gvTotalQtyBreakdown_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        decimal dcQtyBreakDown = 0;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblQty = (Label)e.Row.FindControl("lblQty");
            if (lblQty.Text != "")
            {
                dcQtyBreakDown = Convert.ToDecimal(lblQty.Text);
                dcQtyBreakdownTotal += dcQtyBreakDown;
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblQtyTotal = (Label)e.Row.FindControl("lblQtyTotal");
            lblQtyTotal.Text = dcQtyBreakdownTotal.ToString("0.00");
            dcQtyBreakdownTotal = 0;
        }

    }
    protected void chkLoadSchedules_CheckedChanged(object sender, EventArgs e)
    {
        string sSalesOrder = lblSaleOrderDetailsHidden.Text;
        GetDetails(sSalesOrder, gvDetails);
        ModalPopupExtenderPopUp.Show();
    }
    protected void chkLoadMatrixes_CheckedChanged(object sender, EventArgs e)
    {
        string sSalesOrder = lblSaleOrderDetailsHidden.Text;
        GetDetails(sSalesOrder, gvDetails);
        ModalPopupExtenderPopUp.Show();
    }

    //FrozenHeaderRow...
    // LinkButtons are used to dynamically create the links necessary
    // for paging.
    protected void HeaderLink_Click(object sender, System.EventArgs e)
    {
        LinkButton lnkHeader = (LinkButton)sender;
        SortDirection direction = SortDirection.Ascending;

        // the CommandArgument of each linkbutton contains the sortexpression
        // for the column that was clicked.
        if (gvRecord.SortExpression == lnkHeader.CommandArgument)
        {
            if (gvRecord.SortDirection == SortDirection.Ascending)
            {
                direction = SortDirection.Descending;
            }

        }

        gvRecord.Sort(lnkHeader.CommandArgument, direction);
    }

    ////create the table for the fixed header in Init
    protected void Page_Init(object sender, EventArgs e)
    {
        TableRow headerRow = new TableRow();

        for (int x = 0; x < gvRecord.Columns.Count; x++)
        {
            DataControlField col = gvRecord.Columns[x];
            gvRecord.ShowHeader = false;
            TableCell headerCell = new TableCell();
            headerCell.BorderStyle = BorderStyle.Solid;
            headerCell.BorderWidth = 0;//Hide vertical grid lines on header...
            headerCell.Font.Bold = true;
            headerCell.HorizontalAlign = HorizontalAlign.Center;

            // if the column has a SortExpression, we want to allow
            // sorting by that column. Therefore, we create a linkbutton
            // on those columns.
            if (col.SortExpression != "")
            {
                LinkButton lnkHeader = new LinkButton();

                // *** Comment the line below if using with AJAX
                //// lnkHeader.PostBackUrl = HttpContext.Current.Request.Url.LocalPath;

                lnkHeader.CommandArgument = col.SortExpression;
                lnkHeader.ForeColor = System.Drawing.Color.White;
                lnkHeader.Text = col.HeaderText.Replace(" ", "<br/>");

                // *** Uncomment this line for AJAX 
                lnkHeader.ID = "Sort" + col.HeaderText;

                lnkHeader.Click += new EventHandler(HeaderLink_Click);
                headerCell.Controls.Add(lnkHeader);
            }
            else
            {
                headerCell.Text = col.HeaderText;
            }


            //headerCell.Width = col.ItemStyle.Width;
            headerRow.Cells.Add(headerCell);

        }

        HeaderTable.Rows.Add(headerRow);
    }
    #endregion


    #region WebMethods
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListSalesOrders(string prefixText, int count, string contextKey)
    {//...


        // Your LINQ to SQL query goes here 
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {


            string[] listCustomerNumbers = db.ArCustomer.Where(w => w.Customer != null).OrderBy(w => w.Customer).Select(w => (w.Customer).Replace("  ", " ")).Distinct().ToArray();
            string[] listCustomerNames = db.ArCustomer.Where(w => w.Name != null).OrderBy(w => w.Name).Select(w => (w.Name).Replace("  ", " ")).Distinct().ToArray();
            string[] listSalesOrders = db.SorMaster.Where(w => w.SalesOrder != null && !w.OrderStatus.Contains("\\") && !w.OrderStatus.Contains("*") && !w.OrderStatus.Contains("9"))
            .Join(db.SorDetail,
                 sm => sm.SalesOrder,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
                 sd => sd.SalesOrder,   // Select the foreign key (the second part of the "on" clause)
                (sm, sd) => new { SM = sm, SD = sd })
                .OrderBy(p => (Convert.ToInt32(p.SM.SalesOrder)).ToString()).Select(p => (Convert.ToInt32(p.SM.SalesOrder)).ToString()).Distinct().ToArray();
            //string[] listPONumbers = db.SorMaster.Where(w => w.CustomerPoNumber != null && !w.OrderStatus.Contains("\\") && !w.OrderStatus.Contains("*") && !w.OrderStatus.Contains("9"))
            //    .Join(db.SorDetail,
            //     sm => sm.SalesOrder,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
            //     sd => sd.SalesOrder,   // Select the foreign key (the second part of the "on" clause)
            //    (sm, sd) => new { SM = sm, SD = sd })
            //    .OrderBy(p => p.SM.CustomerPoNumber).Select(p => (p.SM.CustomerPoNumber).Replace("  ", " ")).Distinct().ToArray();
            //string[] list = listPONumbers.Union(listSalesOrders.Union(listCustomerNames.Union(listCustomerNumbers))).ToArray();
            string[] list = listSalesOrders.Union(listCustomerNames.Union(listCustomerNumbers)).ToArray();
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