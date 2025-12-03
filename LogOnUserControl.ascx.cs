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

public partial class LogOnUserControl : System.Web.UI.UserControl
{
    #region Subs
    private void EmailMessageForgotPassword(string sFromWho, string sToWho, string sToWhoCC, string sSubject, string sBody)
    {

        if (sToWho != "")

        {
            try
            {
                string sCC = "";//ConfigurationManager.AppSettings["BCC"].ToString();
                string sMailServer = ConfigurationManager.AppSettings["SMTP"].ToString();
                string sEmailUserName = ConfigurationManager.AppSettings["SMTPUserName"].ToString();
                string sEmailPassword = ConfigurationManager.AppSettings["SMTPPassword"].ToString();

                if (HttpContext.Current.Server.MachineName.ToUpper() == "BMW" || HttpContext.Current.Server.MachineName.ToUpper() == "RSPSERVER2" || HttpContext.Current.Server.MachineName.ToUpper() == "MERCEDES")
                {

                    MyEmail.SendEmailWithCredentials(sFromWho, sToWho, sCC, sSubject, sBody, sMailServer, sEmailUserName, sEmailPassword);
                    lblResetPwdError.ForeColor = System.Drawing.Color.Green;
                    lblResetPwdError.Text = "**Email sent!!<br> ";
                }
                else
                {
                    //MyEmail.SendEmail(sFromWho, sToWho, sCC, sSubject, sBody, sMailServer);
                    MyEmail.SendEmailWithCredentials(sFromWho, sToWho, sCC, sSubject, sBody, sMailServer, sEmailUserName, sEmailPassword);
                    lblResetPwdError.ForeColor = System.Drawing.Color.Green;
                    lblResetPwdError.Text = "**Email sent!!<br> ";
                }


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                lblResetPwdError.ForeColor = System.Drawing.Color.Red;
                lblResetPwdError.Text = "**Email send failed with errors!!<br> ";
            }

            // Any existing page can be used for the response redirect method
        }
    }
    #endregion


    #region Functions
    private bool EmailFound(string sEmail)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from users in db.WipUsers
                         where users.Email == sEmail
                         select users);
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
    private string CheckLogin(string sUserName, string sPassword)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from users in db.WipUsers
                     where users.UserName == sUserName
                     && users.Password == sPassword//Case sensitive...
                     && users.Status == 1
                     select new
                     {
                         users.UserName,
                         users.UserID,
                         users.FirstName,
                         users.LastName,
                         users.RoleID,
                         users.WipUsersRoles.Role
                     });
        foreach (var a in query)
        {

            Session["UserName"] = a.UserName;
            Session["UserID"] = a.UserID;
            Session["FirstName"] = a.FirstName;
            Session["lastName"] = a.LastName;
            Session["RoleID"] = a.RoleID;
            Session["SecurityLevel"] = a.Role;
        }
        if (Session["UserName"] == null)
        {
            return @"**UserName and/or Password Not Found or account has been disabled.";
        }
        else
        {
            return "";//Success
        }

    }
    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            pnlSignOn.Visible = true;
            btnLogOff.Visible = false;
        }
        else
        {
            lblWelcome.Text = "Welcome, " + Session["FirstName"].ToString() + " " + Session["LastName"].ToString() + " (" + Session["UserID"].ToString() + ") - " + Session["SecurityLevel"].ToString();
            pnlSignOn.Visible = false;
            btnLogOff.Visible = true;
        }
        if (!Page.IsPostBack)
        {

            if (Request.Cookies["UNameFELBRO"] != null)
            {
                txtUname.Text = Request.Cookies["UNameFELBRO"].Value;
            }
            if (Request.Cookies["PWDFELBRO"] != null)
            {
                txtPwd.Attributes.Add("value", Request.Cookies["PWDFELBRO"].Value);
            }
            if (Request.Cookies["UNameFELBRO"] != null && Request.Cookies["PWDFELBRO"] != null)
            {
                chkRememberMe.Checked = true;
            }
        }

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        string sResult = "";
        string sUserName = "";
        string sPassword = "";

        sUserName = txtUname.Text.Trim();
        sPassword = txtPwd.Text.Trim();

        if (sPassword == "" || sUserName == "")
        {
            lblError.Text = "**UserName and/or Password can not be left blank, try again.";
            return;
        }

        sResult = CheckLogin(sUserName, sPassword);
        if (sResult != "")
        {
            lblError.Text = sResult;
            pnlSignOn.Visible = true;
            btnLogOff.Visible = false;
        }
        else
        {
            if (chkRememberMe.Checked == true)
            {
                Response.Cookies["UNameFELBRO"].Value = txtUname.Text;
                Response.Cookies["PWDFELBRO"].Value = txtPwd.Text;
                Response.Cookies["UNameFELBRO"].Expires = DateTime.Now.AddMonths(2);
                Response.Cookies["PWDFELBRO"].Expires = DateTime.Now.AddMonths(2);
            }
            else
            {
                Response.Cookies["UNameFELBRO"].Expires = DateTime.Now.AddMonths(-1);
                Response.Cookies["PWDFELBRO"].Expires = DateTime.Now.AddMonths(-1);
            }

            lblWelcome.Text = "Welcome, " + Session["FirstName"].ToString() + " " + Session["LastName"].ToString() + " - " + Session["SecurityLevel"].ToString();
            pnlSignOn.Visible = false;
            btnLogOff.Visible = true;
            //Redirect to either Admin or Merchant back office...
            //if Role =1 then (go to Admin back office) else if Role = 2 then (go to User Section)
            int iUserID = 0;
            if (Session["UserID"] != null)
            {
                iUserID = Convert.ToInt32(Session["UserID"]);

                switch (Convert.ToInt32(Session["RoleID"]))
                {

                    default:
                        Response.Redirect("Default.aspx");
                        break;
                }
            }
        }
    }
    protected void lbnForgotPassword_Click(object sender, EventArgs e)
    {
        ModalPopupExtenderForgotPwd.Show();
    }
    protected void lbnCancel0_Click(object sender, EventArgs e)
    {//Forget password...
        ModalPopupExtenderForgotPwd.Hide();
        lblResetPwdError.Text = "";
        txtResetEmail.Text = "";
    }
    protected void lbnResetPwd_Click(object sender, EventArgs e)
    {

        lblResetPwdError.Text = "";
        string sMsg = "";
        string sEmail = "";
        sEmail = txtResetEmail.Text.Trim();
        if (sEmail == "")
        {
            sMsg += "**No Email address was supplied, try again.<br/>";
        }
        else
        {
            if (!SharedFunctions.IsEmail(sEmail))
            {
                sMsg += "**Email format is not valid!<br/>";
            }
            else
            {
                if (!EmailFound(sEmail))
                {
                    sMsg += "**The Email address you entered was not found in our database, try again.<br/>";
                }
            }

        }


        if (sMsg.Length > 0)
        {
            lblResetPwdError.Text = sMsg;
            lblResetPwdError.ForeColor = Color.Red;
            ModalPopupExtenderForgotPwd.Show();
            return;
        }

        string sSender = ConfigurationManager.AppSettings["MainSender"].ToString();
        string sDestinationEmail = sEmail;
        string sSubject = "Login Credentials From Felbro Foods Intranet Portal";
        string sBody = "";
        string sFirstName = "";
        string sMiddleInt = "";
        string sLastName = "";
        string sUserName = "";
        string sPassword = "";

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            var query = (from users in db.WipUsers
                         where users.Email == sEmail
                         select users);
            foreach (var a in query)
            {
                sFirstName = a.FirstName;
                if (a.MiddleName != null)
                {
                    sMiddleInt = a.MiddleName;
                }
                sLastName = a.LastName;
                sUserName = a.UserName;
                sPassword = a.Password;



                string sName = (sFirstName + " " + sMiddleInt + " " + sLastName).Replace("  ", " ");

                sBody = "";
                sBody += "NAME:  " + sName + "<br/>";
                sBody += "YOUR USERNAME:  " + sUserName + "<br/>";
                sBody += "YOUR PASSWORD:  " + sPassword + "<br/><br/><br/>";

                sBody += "<p style='font-family:arial;font-size:9pt'> CONFIDENTIALITY NOTICE: This E-mail contains confidential information intended only for the individual or entity named within the message. If the reader of this message is not the intended recipient, or the agent responsible to deliver it to the intended recipient, you are hereby notified that any review, dissemination or copying of this communication is prohibited. If this communication was received in error, please notify us by reply E-mail and delete the original message.</p><br/>";

                try
                {
                    //no errors then send message...
                    if (Server.MachineName.ToUpper() == "BMW" || Server.MachineName.ToUpper() == "MERCEDES" || Server.MachineName.ToUpper() == "RSPSERVER2")
                    {
                        EmailMessageForgotPassword(sSender, "robert@robertsoftdev.com", "", sSubject, sBody);
                    }
                    else
                    {
                        EmailMessageForgotPassword(sSender, sDestinationEmail, "", sSubject, sBody);
                    }


                    lblResetPwdError.Text = "**Password emailed successfully!";
                    lblResetPwdError.ForeColor = Color.Green;
                }
                catch (Exception)
                {
                    lblResetPwdError.Text = "Error: Password not sent! Contact system administrator!";
                    lblResetPwdError.ForeColor = Color.Red;
                }

            }




            ModalPopupExtenderForgotPwd.Show();
        }
    }
    protected void btnLogOff_Click(object sender, EventArgs e)
    {

        pnlSignOn.Visible = true;
        btnLogOff.Visible = false;
        Session["UserName"] = null;
        Session["UserID"] = null;
        Session["FirstName"] = null;
        Session["lastName"] = null;
        Session["RoleID"] = null;
        Session["SecurityLevel"] = null;
        txtUname.Text = "";
        txtPwd.Text = "";
        lblWelcome.Text = "";
        Session.Abandon();
        Session.Clear();
        Response.Redirect("Default.aspx");



    }
    #endregion


}