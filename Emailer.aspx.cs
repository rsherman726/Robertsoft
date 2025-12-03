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
using System.IO;

public partial class Emailer : System.Web.UI.Page
{
    #region Member Variables


    #endregion

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
    #endregion

    #region Subs
    private void EmailMessage(string sFromWho, string sToWho, string sToWhoCC, string sSubject, string sBody, string sFullPath)
    {

        if (sToWho != "")
        {
            string sResult = "";
            string sMailServer = ConfigurationManager.AppSettings["SMTP"].ToString();
            string sEmailUserName = ConfigurationManager.AppSettings["SMTPUserName"].ToString();
            string sEmailPassword = ConfigurationManager.AppSettings["SMTPPassword"].ToString();

            try
            {
                if (Server.MachineName.ToUpper() == "POWERHOUSE")
                {
                    sResult = MyEmail.SendEmailWithCredentialsWithAttachment(sFromWho, sToWho, sToWhoCC, "", sSubject, sBody, sMailServer, sEmailUserName, sEmailPassword, sFullPath);
                    if (sResult == "Email Sent!")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                    lblMessage.Text = sResult;
                }
                else//Felbro...
                {
                    sResult = MyEmail.SendEmailWithCredentialsWithAttachment(sFromWho, sToWho, sToWhoCC, "", sSubject, sBody, sMailServer, sEmailUserName, sEmailPassword, sFullPath);
                    if (sResult == "Email Sent!")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                    lblMessage.Text = sResult;
                }

            }
            catch (Exception)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "<b>Email send failed with errors!!</b><br> ";

            }

            // Any existing page can be used for the response redirect method
        }
    }
    private void EmailMessage(string sFromWho, string sToWho, string sToWhoCC, string sSubject, string sBody, MemoryStream msFile, string sAttachmentFileName)
    {

        if (sToWho != "")
        {
            string sResult = "";
            string sMailServer = ConfigurationManager.AppSettings["SMTP"].ToString();
            string sEmailUserName = ConfigurationManager.AppSettings["SMTPUserName"].ToString();
            string sEmailPassword = ConfigurationManager.AppSettings["SMTPPassword"].ToString();

            try
            {
                if (Server.MachineName.ToUpper() == "POWERHOUSE")
                {
                    sResult = MyEmail.SendEmailWithCredentialsWithAttachment(sFromWho, sToWho, sToWhoCC, "", sSubject, sBody, sMailServer, sEmailUserName, sEmailPassword, msFile, sAttachmentFileName);
                    if (sResult == "Email Sent!")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                    lblMessage.Text = sResult;
                }
                else//Felbro...
                {
                    sResult = MyEmail.SendEmailWithCredentialsWithAttachment(sFromWho, sToWho, sToWhoCC, "", sSubject, sBody, sMailServer, sEmailUserName, sEmailPassword, msFile, sAttachmentFileName);
                    if (sResult == "Email Sent!")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                    lblMessage.Text = sResult;
                }

            }
            catch (Exception)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "<b>Email send failed with errors!!</b><br> ";

            }

            // Any existing page can be used for the response redirect method
        }
    }
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
                if (Server.MachineName.ToUpper() == "POWERHOUSE")
                {
                    sResult = MyEmail.SendEmailWithCredentials(sFromWho, sToWho, sToWhoCC, sSubject, sBody, sMailServer, sEmailUserName, sEmailPassword);
                    if (sResult == "Email Sent!")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }

                    lblMessage.Text = sResult;
                }
                else//Felbro...
                {
                    sResult = MyEmail.SendEmailWithCredentials(sFromWho, sToWho, sToWhoCC, sSubject, sBody, sMailServer, sEmailUserName, sEmailPassword);
                    if (sResult == "Email Sent!")
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                    }
                    else
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                    }
                    lblMessage.Text = sResult;
                }


            }
            catch (Exception)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = sResult;

            }

            // Any existing page can be used for the response redirect method
        }
    }
    private void LoadLetters()
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from wd in db.WordDocs
                     select new { wd.DocName });
        foreach (var a in query)
        {
            ddlLetters.Items.Add(new ListItem(a.DocName, a.DocName));
        }
        ddlLetters.Items.Insert(0, new ListItem("Select a Letter...", "0"));
    }
    private void LoadEmails(string sSource)
    {
        string sStartingLetter = "";
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        sStartingLetter = ddlStartingLetter.SelectedValue;

        if (sSource == "ALL")
        {
            if (sStartingLetter == "ALL")
            {
                var query =
                        (from a in
                             (from c in db.ArCustomer
                              where c.Email.Trim().Contains("@")
                              select new
                              {
                                  c.Email,
                                  c.Name,
                                  c.Customer,
                              }).Distinct()
                         orderby a.Name ascending
                         select new
                         {
                             a.Email,
                             a.Name,
                             a.Customer,
                         });
                cblEmailAddresses.Items.Clear();
                foreach (var a in query)
                {
                    string sEmail = a.Email.Trim().IndexOf(';') > 0 ? a.Email.Trim().Substring(0, a.Email.Trim().IndexOf(';')).ToLower() : a.Email.Trim().ToLower();
                    cblEmailAddresses.Items.Add(new ListItem(SharedFunctions.PCase(a.Name) + " - " + a.Customer + " -  " + sEmail, a.Customer));
                }
                lblEmailAddressCount.Text = "Email Addessess (" + query.Count().ToString() + " addresses)";
            }
            else
            {
                var query =
                    (from a in
                         (from c in db.ArCustomer
                          where c.Name.Trim().StartsWith(sStartingLetter)
                            && c.Email.Trim().Contains("@")
                          select new
                          {
                              c.Email,
                              c.Name,
                              c.Customer,
                          }).Distinct()
                     orderby a.Name ascending
                     select new { a.Email, a.Name, a.Customer });
                cblEmailAddresses.Items.Clear();
                foreach (var a in query)
                {
                    string sEmail = a.Email.Trim().IndexOf(';') > 0 ? a.Email.Trim().Substring(0, a.Email.Trim().IndexOf(';')).ToLower() : a.Email.Trim().ToLower();
                    cblEmailAddresses.Items.Add(new ListItem(SharedFunctions.PCase(a.Name) + " - " + a.Customer + " -  " + sEmail, a.Customer));
                }
                lblEmailAddressCount.Text = "Email Addessess (" + query.Count().ToString() + " addresses)";
            }
        }
        else if (sSource == "")//New Price Table Customers
        {
            if (sStartingLetter == "ALL")
            {
                var query =
            (from n in db.ArNewSalesPrice
             join c in db.ArCustomer on n.Customer.Trim() equals c.Customer.Trim()
             where c.Email.Trim().Contains("@")
             group c by new
             {
                 c.Name,
                 c.Email,
                 c.Customer,
             } into g
             orderby g.Key.Name
             select new
             {
                 g.Key.Name,
                 g.Key.Customer,
                 Email =
                 g.Key.Email.Trim() == "" ? "NO EMAIL" : g.Key.Email
             });
                cblEmailAddresses.Items.Clear();
                foreach (var a in query)
                {
                    string sEmail = a.Email.Trim().IndexOf(';') > 0 ? a.Email.Trim().Substring(0, a.Email.Trim().IndexOf(';')).ToLower() : a.Email.Trim().ToLower();
                    cblEmailAddresses.Items.Add(new ListItem(SharedFunctions.PCase(a.Name) + " - " + a.Customer + " -  " + sEmail, a.Customer));
                }
                lblEmailAddressCount.Text = "Email Addessess (" + query.Count().ToString() + " addresses)";
            }
            else
            {
                var query = (from n in db.ArNewSalesPrice
                             join c in db.ArCustomer on n.Customer.Trim() equals c.Customer.Trim()
                             where c.Name.Trim().StartsWith(sStartingLetter) && c.Email.Trim().Contains("@")
                             group c by new
                             {
                                 c.Name,
                                 c.Email,
                                 c.Customer,
                             } into g
                             orderby g.Key.Name
                             select new
                             {
                                 g.Key.Name,
                                 g.Key.Customer,
                                 Email = g.Key.Email.Trim() == "" ? "NO EMAIL" : g.Key.Email
                             });


                cblEmailAddresses.Items.Clear();
                foreach (var a in query)
                {
                    string sEmail = a.Email.Trim().IndexOf(';') > 0 ? a.Email.Trim().Substring(0, a.Email.Trim().IndexOf(';')).ToLower() : a.Email.Trim().ToLower();
                    cblEmailAddresses.Items.Add(new ListItem(SharedFunctions.PCase(a.Name) + " - " + a.Customer + " -  " + sEmail, a.Customer));
                }
                lblEmailAddressCount.Text = "Email Addessess (" + query.Count().ToString() + " addresses)";
            }
        }
        else if (sSource == "VENDORS")//Vendors...
        {
            if (sStartingLetter == "ALL")
            {
                var query = (from s in db.ApSupplier
                             where s.Email.Trim().Contains("@")
                             && s.Email.Trim() != null && s.Email.Trim() != ""
                             && (new string[] { "R", "P" }).Contains(s.SupplierClass)
                             && s.LastPurchDate > Convert.ToDateTime(DateTime.Now).AddYears(-2)
                             group s by new
                             {
                                 s.Supplier,
                                 s.SupplierName,
                                 Email = s.Email.Trim(),
                             } into g
                             orderby g.Key.SupplierName
                             select new
                             {
                                 g.Key.Supplier,
                                 Name = g.Key.SupplierName,
                                 Email = g.Key.Email.Trim() == "" ? "NO EMAIL" : g.Key.Email
                             });
                cblEmailAddresses.Items.Clear();
                foreach (var a in query)
                {
                    string sEmail = a.Email.Trim().IndexOf(';') > 0 ? a.Email.Trim().Substring(0, a.Email.Trim().IndexOf(';')).ToLower() : a.Email.Trim().ToLower();
                    cblEmailAddresses.Items.Add(new ListItem(a.Supplier + " - " + SharedFunctions.PCase(a.Name) + " -  " + sEmail, a.Supplier));
                }
                lblEmailAddressCount.Text = "Email Addessess (" + query.Count().ToString() + " addresses)";
            }
            else//by letter...
            {
                var query = (from s in db.ApSupplier
                             where s.SupplierName.Trim().StartsWith(sStartingLetter) && s.Email.Trim().Contains("@")
                             && s.Email.Trim() != null && s.Email.Trim() != ""
                             && (new string[] { "R", "P" }).Contains(s.SupplierClass)
                             && s.LastPurchDate > Convert.ToDateTime(DateTime.Now).AddYears(-2)
                             group s by new
                             {
                                 s.SupplierName,
                                 s.Supplier,
                                 Email = s.Email.Trim(),
                             } into g
                             select new
                             {
                                 g.Key.Supplier,
                                 Name = g.Key.SupplierName,
                                 Email = g.Key.Email.Trim() == "" ? "NO EMAIL" : g.Key.Email
                             });


                cblEmailAddresses.Items.Clear();
                foreach (var a in query)
                {
                    string sEmail = a.Email.Trim().IndexOf(';') > 0 ? a.Email.Trim().Substring(0, a.Email.Trim().IndexOf(';')).ToLower() : a.Email.Trim().ToLower();
                    cblEmailAddresses.Items.Add(new ListItem(a.Supplier + " - " + SharedFunctions.PCase(a.Name) + " -  " + sEmail, a.Supplier));
                }
                lblEmailAddressCount.Text = "Email Addessess (" + query.Count().ToString() + " addresses)";
            }
        }
        else//MISC...
        {
            if (sStartingLetter == "ALL")
            {
                var query = (from s in db.WipContacts
                             where s.Email.Trim().Contains("@")
                             && s.Email.Trim() != null && s.Email.Trim() != ""
                             group s by new
                             {
                                 Name = "Misc",
                                 Email = s.Email.Trim()
                             } into g
                             orderby g.Key.Email
                             select new
                             {
                                 g.Key.Name,
                                 Email = g.Key.Email.Trim() == "" ? "NO EMAIL" : g.Key.Email.Replace(";", "").ToLower()
                             });
                cblEmailAddresses.Items.Clear();
                foreach (var a in query)
                {
                    cblEmailAddresses.Items.Add(new ListItem(SharedFunctions.PCase(a.Name) + " -  " + a.Email, a.Email));
                }
                lblEmailAddressCount.Text = "Email Addessess (" + query.Count().ToString() + " addresses)";
            }
            else//by letter...
            {
                var query = (from s in db.WipContacts
                             where s.Email.Trim().StartsWith(sStartingLetter) && s.Email.Trim().Contains("@")
                             && s.Email.Trim() != null && s.Email.Trim() != ""
                             group s by new
                             {
                                 Name = "Misc",
                                 Email = s.Email.Trim()
                             } into g
                             orderby g.Key.Email
                             select new
                             {
                                 g.Key.Name,
                                 Email = g.Key.Email.Trim() == "" ? "NO EMAIL" : g.Key.Email.Replace(";", "")
                             });


                cblEmailAddresses.Items.Clear();
                foreach (var a in query)
                {
                    cblEmailAddresses.Items.Add(new ListItem(SharedFunctions.PCase(a.Name) + " -  " + a.Email, a.Email));
                }
                lblEmailAddressCount.Text = "Email Addessess (" + query.Count().ToString() + " addresses)";
            }
        }

    }
    private void SubmitEmail()
    {
        string sMsg = "";
        DataTable dt = new DataTable();
        dt.Columns.Add("ContactName");
        dt.Columns.Add("Customer"); //Cust #
        dt.Columns.Add("CustomerName");
        dt.Columns.Add("Address");
        dt.Columns.Add("CSZ");
        DataRow Row;
        //Validate values...
        if (ddlLetters.SelectedIndex == 0)
        {
            lblMessage.Text = "**Please select a Letter to proceed.";
            return;
        }
        string sTestEmail = txtTestEmailReciepient.Text.Trim();

        string sMyEmail = "DoNotReply@felbro.com";

        string sSubject = "";
        ////string sEmailAddresses = "";
        if (txtSubject.Text.Trim() != "")
        {
            sSubject = txtSubject.Text.Trim();
        }
        else
        {
            sSubject = "Correspondence from Felbro Foods!!!";
        }

        string sBody = "";
        string sFileName = "";



        if (cblEmailAddresses.SelectedIndex == -1)
        {
            sMsg += "**Email Addresses can not be left blank, try again!<br/>";
        }
        if (chkTesting.Checked)
        {
            if (sTestEmail == "")
            {
                sMsg += "**Test Email Address can not be left blank if the testing checkbox is checked, try again!<br/>";
            }
        }

        if (sMsg.Length > 0)
        {
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Text = sMsg;
            return;
        }

        decimal dcFileSize = 0;
        Stream stFileContent = null;

        string filePath = MapPath("~/images/Docs/");
        if (fuAttachment.HasFile)
        {

            dcFileSize = fuAttachment.PostedFile.ContentLength;
            dcFileSize = dcFileSize / 2048;
            dcFileSize = Convert.ToDecimal(dcFileSize.ToString("0.00"));
            if (dcFileSize > 2048)
            {
                lblMessage.Text = "**You have exceeded the maximum size of 2048kb, the size of your file was " + dcFileSize.ToString() + " kb, please try another file!";
                lblMessage.ForeColor = Color.Red;
                return;
            }
            sFileName = Path.GetFileName(fuAttachment.PostedFile.FileName);
            stFileContent = fuAttachment.FileContent;


            if (File.Exists(filePath + sFileName))
            {
                File.Delete(filePath + sFileName);
            }

            fuAttachment.SaveAs(filePath + sFileName);

        }

        //no errors then send message...

        ////OLD
        ////int iLastPipeInStringIdx = 0;
        ////if (cblEmailAddresses.Items.Count > 0)
        ////{
        ////    foreach (ListItem li in cblEmailAddresses.Items)
        ////    {
        ////        if (li.Selected)
        ////        {
        ////            sEmailAddresses += li.Value + ",";
        ////        }
        ////    }
        ////    //Remove last comma from Cities if exists...
        ////    if (sEmailAddresses.Trim().EndsWith(","))
        ////    {
        ////        iLastPipeInStringIdx = sEmailAddresses.LastIndexOf(",");
        ////        sEmailAddresses = sEmailAddresses.Remove(iLastPipeInStringIdx).Trim();
        ////    }
        ////}


        ////string[] EmailAddresses = sEmailAddresses.Split(',');
        //Loop through all email in listbox and send out one at a time...
        string sCustomer = "";//Cust #
        string sContactName = "";
        string sCustomerName = "";
        string sAddress = "";
        string sCSZ = "";
        string sPostalCode = "";
        DataTable dtCustomer = new DataTable();
        string sToEmail = "";
        bool bNewPrices = false;
        // foreach (string sDestinationEmail in EmailAddresses)
        string sDestinationEmail = "";
        string sSupplier = "";
        foreach (ListItem li in cblEmailAddresses.Items)
        {
            if (li.Selected)
            {

                sSupplier = li.Text.Substring(0, li.Text.IndexOf(" "));

                try
                {//Note The ColumnNames from Database...


                    switch (rblCustomerSource.SelectedIndex)
                    {
                        case 0://All Customers...

                            sDestinationEmail = GetCustomerEmail(li.Value);
                            dtCustomer = GetCustomerInfo(li.Value);

                            sCustomer = dtCustomer.Rows[0]["Customer"].ToString().Trim();
                            sContactName = dtCustomer.Rows[0]["Contact"].ToString().Trim();
                            sCustomerName = dtCustomer.Rows[0]["Name"].ToString().Trim();
                            sAddress = dtCustomer.Rows[0]["SoldToAddr1"].ToString().Trim();
                            if (dtCustomer.Rows[0]["SoldPostalCode"] != DBNull.Value)
                            {
                                if (dtCustomer.Rows[0]["SoldPostalCode"].ToString().Trim() != "")
                                {
                                    if (dtCustomer.Rows[0]["SoldPostalCode"].ToString().Trim().Length > 4)
                                    {
                                        sPostalCode = dtCustomer.Rows[0]["SoldPostalCode"].ToString().Trim().Substring(0, 5);
                                    }
                                }
                                else
                                {
                                    sPostalCode = "";
                                }
                            }
                            else
                            {
                                sPostalCode = "";
                            }
                            sCSZ = (dtCustomer.Rows[0]["SoldToAddr4"].ToString().Trim() + " " + dtCustomer.Rows[0]["SoldToAddr5"].ToString().Trim() + " " + sPostalCode).Trim();
                            bNewPrices = false;
                            break;
                        case 1://New List...
                            sDestinationEmail = GetCustomerEmail(li.Value);
                            dtCustomer = GetCustomerInfo(li.Value);

                            sCustomer = dtCustomer.Rows[0]["Customer"].ToString().Trim();
                            sContactName = dtCustomer.Rows[0]["Contact"].ToString().Trim();
                            sCustomerName = dtCustomer.Rows[0]["Name"].ToString().Trim();
                            sAddress = dtCustomer.Rows[0]["SoldToAddr1"].ToString().Trim();
                            if (dtCustomer.Rows[0]["SoldPostalCode"] != DBNull.Value)
                            {
                                if (dtCustomer.Rows[0]["SoldPostalCode"].ToString().Trim() != "")
                                {
                                    if (dtCustomer.Rows[0]["SoldPostalCode"].ToString().Trim().Length > 4)
                                    {
                                        sPostalCode = dtCustomer.Rows[0]["SoldPostalCode"].ToString().Trim().Substring(0, 5);
                                    }
                                }
                                else
                                {
                                    sPostalCode = "";
                                }
                            }
                            else
                            {
                                sPostalCode = "";
                            }
                            sCSZ = (dtCustomer.Rows[0]["SoldToAddr4"].ToString().Trim() + " " + dtCustomer.Rows[0]["SoldToAddr5"].ToString().Trim() + " " + sPostalCode).Trim();
                            if (chkSendNewPrices.Checked)
                            {
                                bNewPrices = true;
                            }
                            else
                            {
                                bNewPrices = false;
                            }
                            break;
                        case 2://Vendors...

                            dtCustomer = GetVendorInfo(sSupplier);
                            sDestinationEmail = GetVendorEmail(li.Value);
                            sCustomer = dtCustomer.Rows[0]["Supplier"].ToString().Trim();
                            sContactName = "";
                            sCustomerName = dtCustomer.Rows[0]["Name"].ToString().Trim();
                            sAddress = dtCustomer.Rows[0]["SupAddr1"].ToString().Trim();
                            if (dtCustomer.Rows[0]["SupPostalCode"] != DBNull.Value)
                            {
                                if (dtCustomer.Rows[0]["SupPostalCode"].ToString().Trim() != "")
                                {
                                    if (dtCustomer.Rows[0]["SupPostalCode"].ToString().Trim().Length > 4)
                                    {
                                        sPostalCode = dtCustomer.Rows[0]["SupPostalCode"].ToString().Trim().Substring(0, 5);
                                    }
                                }
                                else
                                {
                                    sPostalCode = "";
                                }
                            }
                            else
                            {
                                sPostalCode = "";
                            }
                            sCSZ = (dtCustomer.Rows[0]["SupAddr4"].ToString().Trim() + " " + dtCustomer.Rows[0]["SupAddr5"].ToString().Trim() + " " + sPostalCode).Trim();
                            bNewPrices = false;
                            break;
                        case 3://Misc...
                            bNewPrices = false;
                            sDestinationEmail = li.Value;
                            //no information...
                            break;
                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                try
                {
                    dt.Rows.Clear();
                    Row = dt.NewRow();
                    Row["ContactName"] = sContactName;
                    Row["Customer"] = sCustomer;//Cust #
                    Row["CustomerName"] = sCustomerName;
                    Row["Address"] = sAddress;
                    Row["CSZ"] = sCSZ;
                    dt.Rows.Add(Row);

                    sBody = WordDocuments.GetWordDoc(dt, "EMAIL", "E", ddlLetters.SelectedValue, bNewPrices);

                    string sNewSubject = "";
                    string sCC = "";

                    switch (rblCustomerSource.SelectedIndex)
                    {
                        case 0://All Customers...
                            if (sCustomerName == "")
                            {
                                sNewSubject = sSubject;
                            }
                            else
                            {
                                sNewSubject = sCustomerName + ": " + sSubject;
                            }
                            if (txtTestEmailReciepientCC.Text.Trim() != "")
                            {
                                sCC = txtTestEmailReciepientCC.Text.Trim();
                            }
                            else
                            {
                                sCC = "";
                            }

                            break;
                        case 1://New List...
                            if (sCustomerName == "")
                            {
                                sNewSubject = sSubject;
                            }
                            else
                            {
                                sNewSubject = sCustomerName + ": " + sSubject;
                            }
                            if (txtTestEmailReciepientCC.Text.Trim() != "")
                            {
                                sCC = txtTestEmailReciepientCC.Text.Trim();
                            }
                            else
                            {
                                sCC = "wil@felbro.com";
                            }
                            break;
                        case 2://Vendors...
                            sNewSubject = sSubject;
                            if (txtTestEmailReciepientCC.Text.Trim() != "")
                            {
                                sCC = txtTestEmailReciepientCC.Text.Trim();
                            }
                            else
                            {
                                sCC = "purchasing@felbro.com";
                            }

                            break;
                        case 3://Misc...
                            sNewSubject = sSubject;
                            if (txtTestEmailReciepientCC.Text.Trim() != "")
                            {
                                sCC = txtTestEmailReciepientCC.Text.Trim();
                            }
                            else
                            {
                                sCC = "";
                            }
                            break;
                    }



                    //////For Testing only...


                    sToEmail = sDestinationEmail.Trim();

                    if (sToEmail.IndexOf(";") != -1)
                    {
                        sToEmail = sToEmail.Substring(0, sToEmail.IndexOf(";"));
                    }
                    ////For Live server...
                    switch (rblCustomerSource.SelectedIndex)
                    {

                        case 1://New Price List (Customers)...
                            tblNewPrice.Visible = true;
                            //Check to see it body of email has New Price & Current Price er go it has the matrix so send it out for this customer in the loop...
                            if (sBody.Contains("New Price") && sBody.Contains("Current Price"))
                            {
                                if (rblEmailDestination.SelectedIndex == 0)
                                {//Sales Rep...
                                    if (txtSalesRepEmail.Text.Trim() == "")
                                    {
                                        lblEmailFormatError.Text = "**If you selected a Sales Rep Email option, you must fill in their email address in the Sales Rep Email Address Text Box!!";
                                        lblEmailFormatError.ForeColor = Color.Red;
                                        lblMessage.Text = "**If you selected a Sales Rep Email option, you must fill in their email address in the Sales Rep Email Address Text Box!!";
                                        lblMessage.ForeColor = Color.Red;
                                        return;
                                    }
                                    else//not blank...
                                    {
                                        if (SharedFunctions.IsEmail(txtSalesRepEmail.Text.Trim()) == false)
                                        {
                                            lblEmailFormatError.Text = "**The email address you entered for the Sales Rep is not a valid email address!!";
                                            lblEmailFormatError.ForeColor = Color.Red;
                                            lblMessage.Text = "**The email address you entered for the Sales Rep is not a valid email address!!";
                                            lblMessage.ForeColor = Color.Red;
                                            return;
                                        }
                                        else
                                        {
                                            if (chkTesting.Checked)
                                            {
                                                if (Server.MachineName.ToUpper() == "BMW" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
                                                {
                                                    EmailMessage(sMyEmail, "robert@robertsoftdev.com", sTestEmail, sNewSubject, sBody);
                                                }
                                                else
                                                {
                                                    //Testing...
                                                    EmailMessage(sMyEmail, "wil@felbro.com", sTestEmail, sNewSubject, sBody);
                                                }
                                            }
                                            else
                                            {
                                                if (stFileContent != null)
                                                {
                                                    if (Server.MachineName.ToUpper() == "BMW" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
                                                    {
                                                        EmailMessage(sMyEmail, "robert@robertsoftdev.com", sTestEmail, sNewSubject, sBody, filePath + sFileName);
                                                    }
                                                    else
                                                    {
                                                        EmailMessage(sMyEmail, txtSalesRepEmail.Text.Trim(), sCC, sNewSubject, sBody, filePath + sFileName);
                                                    }
                                                }
                                                else//No Attachment...
                                                {
                                                    if (Server.MachineName.ToUpper() == "BMW" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
                                                    {
                                                        EmailMessage(sMyEmail, "robert@robertsoftdev.com", sTestEmail, sNewSubject, sBody);
                                                    }
                                                    else
                                                    {
                                                        EmailMessage(sMyEmail, txtSalesRepEmail.Text.Trim(), sCC, sNewSubject, sBody);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else//Customer
                                {
                                    if (Server.MachineName.ToUpper() == "BMW" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
                                    {
                                        EmailMessage(sMyEmail, "robert@robertsoftdev.com", sTestEmail, sNewSubject, sBody);
                                    }
                                    else
                                    {
                                        if (chkTesting.Checked)
                                        {                                        //Testing...
                                            EmailMessage(sMyEmail, "wil@felbro.com", sTestEmail, sNewSubject, sBody);
                                        }
                                        else
                                        {
                                            if (stFileContent != null)
                                            {
                                                EmailMessage(sMyEmail, sToEmail, sCC, sNewSubject, sBody, filePath + sFileName);
                                            }
                                            else//No Attachment...
                                            {
                                                EmailMessage(sMyEmail, sToEmail, sCC, sNewSubject, sBody);
                                            }
                                        }
                                    }
                                }
                            }

                            break;
                        default://Vendor/All Customers...
                            string sAttachmentFileName = "Item Spec.xlsx";
                            if (Server.MachineName.ToUpper() == "BMW" || Server.MachineName.ToUpper() == "RSPSERVER2" || Server.MachineName.ToUpper() == "MERCEDES")
                            {
                                if (chkTesting.Checked)
                                {
                                    //Testing...
                                    if (chkSendItemSpecsToVendors.Checked)//Attachment
                                    {
                                        DataSet ds = GetVendorItemSpecs(sCustomer);
                                        MemoryStream MyMS = ExcelHelper.ToExcelMemoryStream(ds);
                                        EmailMessage(sMyEmail, "robert@robertsoftdev.com", sTestEmail, sNewSubject, sBody, MyMS, sAttachmentFileName);
                                    }
                                    else
                                    {
                                        EmailMessage(sMyEmail, "robert@robertsoftdev.com", sTestEmail, sNewSubject, sBody);
                                    }
                                }
                                else//No Testing...
                                {
                                    if (stFileContent != null) //Attachment
                                    {
                                        if (chkSendItemSpecsToVendors.Checked)// Vendor Attachment...
                                        {
                                            DataSet ds = GetVendorItemSpecs(sCustomer);
                                            MemoryStream MyMS = ExcelHelper.ToExcelMemoryStream(ds);
                                            EmailMessage(sMyEmail, "robert@robertsoftdev.com", "", sNewSubject, sBody, MyMS, sAttachmentFileName);
                                        }
                                        else//No Vendor Attachment...
                                        {
                                            EmailMessage(sMyEmail, "robert@robertsoftdev.com", "", sNewSubject, sBody, filePath + sFileName);
                                        }
                                    }
                                    else//No Attachment...
                                    {
                                        if (chkSendItemSpecsToVendors.Checked)//Vendor Attachment...
                                        {
                                            DataSet ds = GetVendorItemSpecs(sCustomer);
                                            MemoryStream MyMS = ExcelHelper.ToExcelMemoryStream(ds);
                                            EmailMessage(sMyEmail, "robert@robertsoftdev.com", "", sNewSubject, sBody, MyMS, sAttachmentFileName);
                                        }
                                        else
                                        {
                                            EmailMessage(sMyEmail, "robert@robertsoftdev.com", "", sNewSubject, sBody);
                                        }
                                    }
                                }
                            }
                            else//Live Server...
                            {
                                if (chkTesting.Checked)
                                {
                                    //Testing...  
                                    if (stFileContent != null)//Attachment...
                                    {
                                        if (chkSendItemSpecsToVendors.Checked)//Vendor Attachment...
                                        {
                                            DataSet ds = GetVendorItemSpecs(sCustomer);
                                            MemoryStream MyMS = ExcelHelper.ToExcelMemoryStream(ds);
                                            EmailMessage(sMyEmail, sTestEmail, sCC, sNewSubject, sBody, MyMS, sAttachmentFileName);
                                        }
                                        else//No Vendor attachment...
                                        {
                                            EmailMessage(sMyEmail, sTestEmail, sCC, sNewSubject, sBody, filePath + sFileName);
                                        }
                                    }
                                    else//No Attachment...
                                    {
                                        if (chkSendItemSpecsToVendors.Checked)//Vendor attachment...
                                        {
                                            DataSet ds = GetVendorItemSpecs(sCustomer);
                                            MemoryStream MyMS = ExcelHelper.ToExcelMemoryStream(ds);
                                            EmailMessage(sMyEmail, sTestEmail, sCC, sNewSubject, sBody, MyMS, sAttachmentFileName);
                                        }
                                        else//No Vendor attachment...
                                        {
                                            EmailMessage(sMyEmail, sTestEmail, sCC, sNewSubject, sBody);
                                        }
                                    }
                                }
                                else//No Testing...
                                {
                                    if (stFileContent != null)//Attachment...
                                    {
                                        if (chkSendItemSpecsToVendors.Checked)//Vendor Attachment...
                                        {
                                            DataSet ds = GetVendorItemSpecs(sCustomer);
                                            MemoryStream MyMS = ExcelHelper.ToExcelMemoryStream(ds);
                                            EmailMessage(sMyEmail, sToEmail, sCC, sNewSubject, sBody, MyMS, sAttachmentFileName);
                                        }
                                        else//No Vendor attachment...
                                        {
                                            EmailMessage(sMyEmail, sToEmail, sCC, sNewSubject, sBody, filePath + sFileName);
                                        }
                                    }
                                    else//No Attachment...
                                    {
                                        if (chkSendItemSpecsToVendors.Checked)//Vendor attachment...
                                        {
                                            DataSet ds = GetVendorItemSpecs(sCustomer);
                                            MemoryStream MyMS = ExcelHelper.ToExcelMemoryStream(ds);
                                            EmailMessage(sMyEmail, sToEmail, sCC, sNewSubject, sBody, MyMS, sAttachmentFileName);
                                        }
                                        else//No Vendor attachment...
                                        {
                                            EmailMessage(sMyEmail, sToEmail, sCC, sNewSubject, sBody);
                                        }
                                    }
                                }

                            }

                            break;
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }//Item Selected...
        }//End loop...

        ////if (File.Exists(filePath + sFileName))
        ////{
        ////    File.Delete(filePath + sFileName);
        ////}
    }
    private void Reset()
    {
        cblEmailAddresses.Text = "";
    }

    private void GetReportForPreviewGrid(string sCustomerList)
    {

        DataTable dt = new DataTable();
        try
        {

            dt = GetNewPriceReport(sCustomerList);
            gvPreview.DataSource = dt;
            gvPreview.DataBind();

            Session["dtReport"] = dt;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            dt.Dispose();
        }

    }
    #endregion

    #region Functions
    private static DataTable GetNewPriceReport(string sCustomer)
    {
        DataTable dtReport = new DataTable();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        sSQL = "EXEC spGetNewPriceEmailReport ";
        sSQL += "@Customer='" + sCustomer + "'";
        Debug.WriteLine(sSQL);

        dtReport = SharedFunctions.getDataTable(sSQL, conn, "dtReport");
        return dtReport;

    }
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
    private DataTable GetCustomerInfo(string sCustomer)
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.ArCustomer
                         where c.Customer == sCustomer
                         select c);

            try
            {
                dt = SharedFunctions.ToDataTable(db, query);
            }
            catch (Exception)
            {

            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
    }
    private DataTable GetVendorInfo(string sSupplier)
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            var query = (from s in db.ApSupplier
                         join sa in db.ApSupplierAddr on s.Supplier equals sa.Supplier
                         where s.Supplier == sSupplier
                         group s by new
                         {
                             s.SupplierName,
                             s.Supplier, //This is SupplierID...
                             Email = s.Email.Trim().IndexOf(';') > 0 ? s.Email.Trim().Substring(0, s.Email.Trim().IndexOf(';')) : s.Email.Trim(),
                             sa.SupAddr1,
                             sa.SupAddr4,
                             sa.SupAddr5,
                             sa.SupPostalCode
                         } into g
                         select new
                         {

                             Name = g.Key.SupplierName,
                             Email = g.Key.Email.Trim() == "" ? "NO EMAIL" : g.Key.Email.Replace(";", ""),
                             g.Key.Supplier,
                             g.Key.SupAddr1,
                             g.Key.SupAddr4,
                             g.Key.SupAddr5,
                             g.Key.SupPostalCode

                         });

            try
            {
                dt = SharedFunctions.ToDataTable(db, query);
            }
            catch (Exception)
            {

            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
    }
    private DataSet GetVendorItemSpecs(string sSupplier)
    {
        DataSet ds = new DataSet();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        try
        {
            sSQL = "EXEC spGetVendorItemSpecsTemplate @Supplier='" + sSupplier + "'";

            //Debug.WriteLine(sSQL);

            ds = SharedFunctions.getDataSet(sSQL, conn, "dsSupplier");
            ds.Tables[0].TableName = "Raw Material";
            ds.Tables[1].TableName = "Packaging";
            ds.Tables[2].TableName = "Labels";
            if (ds.Tables["Raw Material"].Rows.Count == 0)
            {
                ds.Tables.Remove(ds.Tables["Raw Material"]);
            }
            if (ds.Tables["Packaging"].Rows.Count == 0)
            {
                ds.Tables.Remove(ds.Tables["Packaging"]);
            }
            if (ds.Tables["Labels"].Rows.Count == 0)
            {
                ds.Tables.Remove(ds.Tables["Labels"]);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            ds.Dispose();
        }
        return ds;

    }
    private string GetCustomerEmail(string sCustomer)
    {
        string sEmail = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from c in db.ArCustomer
                         where c.Customer == sCustomer
                         group c by new
                         {
                             Email = c.Email.Trim(),
                         } into g
                         select new
                         {
                             Email = g.Key.Email.Trim(),
                         });
            foreach (var a in query)
            {
                sEmail = a.Email.Trim().IndexOf(';') > 0 ? a.Email.Trim().Substring(0, a.Email.Trim().IndexOf(';')).ToLower() : a.Email.Trim().ToLower();

            }
            return sEmail;
        }
    }
    private string GetVendorEmail(string sSupplier)
    {
        string sEmail = "";

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            var query = (from s in db.ApSupplier
                         join sa in db.ApSupplierAddr on s.Supplier equals sa.Supplier
                         where s.Supplier == sSupplier
                         group s by new
                         {
                             Email = s.Email.Trim(),
                         } into g
                         select new
                         {
                             Email = g.Key.Email.Trim(),
                         });

            foreach (var a in query)
            {
                sEmail = a.Email.Trim().IndexOf(';') > 0 ? a.Email.Trim().Substring(0, a.Email.Trim().IndexOf(';')).ToLower() : a.Email.Trim().ToLower();
                //Debug.WriteLine(sEmail);
            }

            return sEmail;
        }
    }
    #endregion

    #region Events
    protected void Page_Load(object sender, System.EventArgs e)
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
            LoadLetters();
            cblEmailAddresses.Attributes.Add("onchange", "javascript:CountItems(this);");
        }

    }
    protected void btnSubmit_Click(object sender, System.EventArgs e)
    {
        lblMessage.Text = "";
        lblEmailFormatError.Text = "";
        SubmitEmail();

    }
    protected void btnReset_Click(object sender, System.EventArgs e)
    {
        Reset();
    }
    protected void btnLoadEmails_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        switch (rblCustomerSource.SelectedIndex)
        {
            case 0:
                LoadEmails("ALL");
                break;
            case 1:
                LoadEmails("");//NEWPRICE Tbl...
                break;
            case 2:
                LoadEmails("VENDORS");//All Vendors...
                break;
            case 3:
                LoadEmails("MISC");//All Vendors...
                break;
        }

    }
    protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSelectAll.Checked)
        {
            foreach (ListItem li in cblEmailAddresses.Items)
            {
                li.Selected = true;
            }
            cblEmailAddresses_SelectedIndexChanged(null, null);
        }
        else
        {
            foreach (ListItem li in cblEmailAddresses.Items)
            {
                li.Selected = false;
            }
        }

    }
    protected void btnToEmailEditor_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmailEditor.aspx");
    }
    protected void ddlLetters_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dtContent = new DataTable();


            string sFileName = "";
            string sDocMode = "E";
            string sDocType = "";
            string sBody = "";
            if (ddlLetters.SelectedIndex == 0)
            {
                lblMessage.Text = "**Please select a Letter to proceed.";
                return;
            }


            if (ddlLetters.SelectedItem.Text.ToUpper().Contains("PRICE INCREASE"))
            {
                string sCustomerList = "";
                int iLastComma = 0;
                foreach (ListItem li in cblEmailAddresses.Items)
                {
                    if (li.Selected)
                    {
                        sCustomerList += li.Value + "|";
                    }
                }
                if (sCustomerList.Trim().EndsWith("|"))
                {
                    iLastComma = sCustomerList.LastIndexOf("|");
                    sCustomerList = sCustomerList.Remove(iLastComma).Trim();
                }

                GetReportForPreviewGrid(sCustomerList);
            }
            else
            {
                gvPreview.DataSource = null;
                gvPreview.DataBind();
            }
            sBody = "";


            sFileName = ddlLetters.SelectedValue;
            sDocMode = "ORIGINAL";

            sBody = WordDocuments.GetWordDoc(dtContent, sDocMode, sDocType, sFileName, false);

            if (String.IsNullOrEmpty(sBody))
            {
                sBody = "";
            }
            sBody = sBody.Replace("<HTML><BODY>", "");
            sBody = sBody.Replace("</HTML></BODY>", "");
            RSEditor.Text = sBody;

            dtContent.Dispose();
        }
    }
    protected void ddlStartingLetter_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        chkSelectAll.Checked = false;
        switch (rblCustomerSource.SelectedIndex)
        {
            case 0:
                LoadEmails("ALL");
                break;
            case 1:
                LoadEmails("");//NEWPRICE Tbl...
                break;
            case 2:
                LoadEmails("VENDORS");//All Vendors...
                break;
            case 3:
                LoadEmails("MISC");//All Vendors...
                break;
        }

    }
    protected void gvPreview_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dt = (DataTable)Session["dtReport"];
        if (e.Row.RowType == DataControlRowType.Header)
        {

        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblStatus = (Label)e.Row.FindControl("lblStatus");
            Label lblNewPrice = (Label)e.Row.FindControl("lblNewPrice");
            Label lblCurrentPrice = (Label)e.Row.FindControl("lblCurrentPrice");
            if (lblStatus.Text != "")
            {
                switch (lblStatus.Text.ToUpper())
                {
                    case "ACTIVE":
                        lblNewPrice.Text = Convert.ToDecimal(lblNewPrice.Text).ToString("c");
                        break;
                    case "TO BE DISCONTINUED":
                        lblNewPrice.Text = lblStatus.Text.ToUpper();
                        lblNewPrice.ForeColor = Color.Orange;
                        break;
                    case "INACTIVE":
                        lblNewPrice.Text = lblStatus.Text.ToUpper();
                        lblNewPrice.ForeColor = Color.Red;
                        break;
                    case "MARKET PRICE":
                        lblNewPrice.Text = lblStatus.Text.ToUpper();
                        lblNewPrice.ForeColor = Color.Blue;
                        break;
                }
            }
            if (lblCurrentPrice.Text != "")
            {
                lblCurrentPrice.Text = Convert.ToDecimal(lblCurrentPrice.Text).ToString("c");
            }
        }
    }
    protected void gvPreview_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();

        DataTable dt = (DataTable)Session["dtReport"];
        dtSortTable = dt;
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvPreview.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvPreview.DataSource = m_DataView;
            gvPreview.DataBind();
            gvPreview.PageIndex = m_PageIndex;
            Session["dtPropSort"] = m_DataTable;
        }
        dtSortTable.Dispose();
    }
    protected void cblEmailAddresses_SelectedIndexChanged(object sender, EventArgs e)
    {
        //string sEmailList = "";
        //int iLastComma = 0;
        //foreach (ListItem li in cblEmailAddresses.Items)
        //{
        //    if (li.Selected)
        //    {
        //        sEmailList += li.Value.Trim() + ",";
        //    }
        //}
        //if (sEmailList.Trim().EndsWith(","))
        //{
        //    iLastComma = sEmailList.LastIndexOf(",");
        //    sEmailList = sEmailList.Remove(iLastComma).Trim();
        //} 


    }
    protected void rblCustomerSource_SelectedIndexChanged(object sender, EventArgs e)
    {
        cblEmailAddresses.Items.Clear();
        chkSelectAll.Checked = false;
        switch (rblCustomerSource.SelectedIndex)
        {
            case 0://All Customers...
                tblNewPrice.Visible = false;
                chkSendItemSpecsToVendors.Visible = false;
                LabelSpecsToVender.Visible = false;
                break;
            case 1://New Price List (Customers)...
                tblNewPrice.Visible = true;
                chkSendItemSpecsToVendors.Visible = false;
                LabelSpecsToVender.Visible = false;
                break;
            case 2://Vendors...
                tblNewPrice.Visible = false;
                chkSendItemSpecsToVendors.Visible = true;
                LabelSpecsToVender.Visible = true;
                break;
            case 3://Misc...
                tblNewPrice.Visible = false;
                chkSendItemSpecsToVendors.Visible = false;
                LabelSpecsToVender.Visible = false;
                break;
        }
        btnLoadEmails_Click(btnLoadEmails, null);
    }
    protected void btnReloadPriceGrid_Click(object sender, EventArgs e)
    {
        string sCustomerList = "";
        int iLastComma = 0;
        foreach (ListItem li in cblEmailAddresses.Items)
        {
            if (li.Selected)
            {
                sCustomerList += li.Value + "|";
            }
        }
        if (sCustomerList.Trim().EndsWith("|"))
        {
            iLastComma = sCustomerList.LastIndexOf("|");
            sCustomerList = sCustomerList.Remove(iLastComma).Trim();
        }

        if (ddlLetters.SelectedItem.Text.ToUpper().Contains("PRICE INCREASE"))
        {
            GetReportForPreviewGrid(sCustomerList);
        }
        else
        {
            gvPreview.DataSource = null;
            gvPreview.DataBind();
        }
    }

    #endregion











}