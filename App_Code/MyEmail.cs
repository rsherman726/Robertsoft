using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net.Mail;
using System.Diagnostics;
using System.Net;
using System.Net.Mime;
using System.Collections;
using System.IO;

/// <summary>
/// Summary description for MyEmail
/// </summary>
public sealed class MyEmail
{
    public MyEmail()
    {

    }
    public static string SendEmail(string sFromEmail, string sToEmail, string sCC, string sSubject, string sMessage, string sMailServer)
    {
        try
        {

            MailAddress oFrom = new MailAddress(sFromEmail);
            MailAddress oTo = new MailAddress(sToEmail);
            MailMessage oMessage = new MailMessage(oFrom, oTo);
            oMessage.Subject = sSubject;
            if (sCC.Contains(","))
            {
                string[] CC = sCC.Split(',');
                foreach (string scc in CC)
                {
                    MailAddress copy = new MailAddress(scc);
                    oMessage.CC.Add(copy);
                }
            }
            else
            {
                if (sCC.Length > 0)
                {
                    MailAddress copy = new MailAddress(sCC);
                    oMessage.CC.Add(copy);
                }
            }
            oMessage.Body = sMessage;
            oMessage.IsBodyHtml = true;
            if (HttpContext.Current.Server.MachineName.ToUpper() == "BMW" || HttpContext.Current.Server.MachineName.ToUpper() == "RSPSERVER2" || HttpContext.Current.Server.MachineName.ToUpper() == "MERCEDES")
            {
                SmtpClient client = new SmtpClient(sMailServer);
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                client.Send(oMessage);
                return "Email Sent!";
            }
            else
            {

                SmtpClient client = new SmtpClient(sMailServer, 587);
                client.EnableSsl = true;
                //NetworkCredential theCredential = new NetworkCredential(sUserName, sPassword);
                //client.Credentials = theCredential;
                client.Send(oMessage);
                return "Email Sent!";
            }

        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.ToString());
            return "ERROR: " + ex.ToString();
        }

    }
    public static string SendEmailWithCredentials(string sFromEmail, string sToEmail, string sCC, string sSubject, string sMessage, string sMailServer, string sUserName, string sPassword)
    {
        try
        {
            foreach (string address in sToEmail.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                NetworkCredential theCredential = new NetworkCredential(sUserName, sPassword);
                MailAddress oFrom = new MailAddress(sFromEmail);
                MailAddress oTo = new MailAddress(address);
                MailMessage oMessage = new MailMessage(oFrom, oTo);
                oMessage.Subject = sSubject;
                if (sCC != "")
                {
                    if (sCC.Contains(","))
                    {
                        string[] CC = sCC.Split(',');
                        foreach (string scc in CC)
                        {
                            MailAddress copy = new MailAddress(scc);
                            oMessage.CC.Add(copy);
                        }
                    }
                    else
                    {
                        MailAddress copy = new MailAddress(sCC);
                        oMessage.CC.Add(copy);
                    }
                }
                oMessage.Body = sMessage;
                oMessage.IsBodyHtml = true;
                SmtpClient client = new SmtpClient(sMailServer);
                client.Credentials = theCredential;
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                if (HttpContext.Current.Server.MachineName.ToUpper() == "BMW" || HttpContext.Current.Server.MachineName.ToUpper() == "RSPSERVER2" || HttpContext.Current.Server.MachineName.ToUpper() == "MERCEDES" || HttpContext.Current.Server.MachineName.ToUpper() == "S198-12-225-69")
                {
                    //Godaddy...
                }
                else
                {
                    client.EnableSsl = true;// Liquid Web only...
                }

                client.Send(oMessage);
            }
            return "Email Sent!";
        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.ToString());
            return "ERROR: " + ex.ToString();
        }
    }
    public static string SendEmailWithCredentialsWithAttachment(string sFromEmail,
    string sToEmail,
    string sCC,
    string sBCC,
    string sSubject,
    string sMessage,
    string sMailServer,
    string sUserName,
    string sPassword,
    string sFileName
    )
    {

        try
        {

            NetworkCredential theCredential = new NetworkCredential(sUserName, sPassword);
            MailAddress oFrom = new MailAddress(sFromEmail);//Must Be from the authorized from email for Office365
            MailAddress oTo = new MailAddress(sToEmail);
            MailMessage oMessage = new MailMessage(oFrom, oTo);


            // Create  the file attachment for this e-mail message.
            Attachment data = new Attachment(sFileName, MediaTypeNames.Application.Octet);
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(sFileName);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(sFileName);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(sFileName);
            if (sFileName != "")
            {
                // Add the file attachment to this e-mail message.
                oMessage.Attachments.Add(data);
            }

            oMessage.Subject = sSubject;
            if (sCC.Contains(","))
            {
                string[] CC = sCC.Split(',');
                foreach (string scc in CC)
                {
                    MailAddress copy = new MailAddress(scc);
                    oMessage.CC.Add(copy);
                }
            }
            else
            {
                if (sCC.Length > 0)
                {
                    MailAddress copy = new MailAddress(sCC);
                    oMessage.CC.Add(copy);
                }
            }
            oMessage.Body = sMessage;
            oMessage.IsBodyHtml = true;


            if (HttpContext.Current.Server.MachineName.ToUpper() == "BMW" || HttpContext.Current.Server.MachineName.ToUpper() == "RSPSERVER2" || HttpContext.Current.Server.MachineName.ToUpper() == "MERCEDES")
            {
                SmtpClient client = new SmtpClient(sMailServer);
                client.Credentials = theCredential;
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                client.Send(oMessage);

                // Display the values in the ContentDisposition for the attachment.
                ContentDisposition cd = data.ContentDisposition;
                Debug.WriteLine("Content disposition");
                Debug.WriteLine(cd.ToString());
                Debug.WriteLine("File {0}", cd.FileName);
                Debug.WriteLine("Size {0}", cd.Size);
                Debug.WriteLine("Creation {0}", cd.CreationDate);
                Debug.WriteLine("Modification {0}", cd.ModificationDate);
                Debug.WriteLine("Read {0}", cd.ReadDate);
                Debug.WriteLine("Inline {0}", cd.Inline);
                Debug.WriteLine("Parameters: {0}", cd.Parameters.Count);
                foreach (DictionaryEntry d in cd.Parameters)
                {
                    Console.WriteLine("{0} = {1}", d.Key, d.Value);
                }

                data.Dispose();
                return "Email Sent!";
            }
            else
            {

                SmtpClient client = new SmtpClient(sMailServer, 587);
                client.EnableSsl = true;
                //NetworkCredential theCredential = new NetworkCredential(sUserName, sPassword);
                client.Credentials = theCredential;
                client.Send(oMessage);

                // Display the values in the ContentDisposition for the attachment.
                ContentDisposition cd = data.ContentDisposition;
                Debug.WriteLine("Content disposition");
                Debug.WriteLine(cd.ToString());
                Debug.WriteLine("File {0}", cd.FileName);
                Debug.WriteLine("Size {0}", cd.Size);
                Debug.WriteLine("Creation {0}", cd.CreationDate);
                Debug.WriteLine("Modification {0}", cd.ModificationDate);
                Debug.WriteLine("Read {0}", cd.ReadDate);
                Debug.WriteLine("Inline {0}", cd.Inline);
                Debug.WriteLine("Parameters: {0}", cd.Parameters.Count);
                foreach (DictionaryEntry d in cd.Parameters)
                {
                    Console.WriteLine("{0} = {1}", d.Key, d.Value);
                }
                data.Dispose();

                return "Email Sent!";


            }




        }
        catch (SmtpException ex)
        {
            Debug.WriteLine(ex.ToString());
            return "ERROR: " + ex.ToString();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            Debug.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}", ex.ToString());
            return "ERROR: " + ex.ToString();
        }
    }
    public static string SendEmailWithCredentialsWithAttachment(string sFromEmail,
    string sToEmail,
    string sCC,
    string sBCC,
    string sSubject,
    string sMessage,
    string sMailServer,
    string sUserName,
    string sPassword,
    MemoryStream msFile,
    string sAttachmentFileName
    )
    {

        try
        {

            NetworkCredential theCredential = new NetworkCredential(sUserName, sPassword);
            MailAddress oFrom = new MailAddress(sUserName);//Must Be from the authorized from email for Office365
            MailAddress oTo = new MailAddress(sToEmail);
            MailMessage oMessage = new MailMessage(oFrom, oTo);


            // Create  the file attachment for this e-mail message.          
            Stream myStream = msFile;
            Attachment data = new Attachment(myStream, sAttachmentFileName);

            if (msFile != null)
            {
                // Add the file attachment to this e-mail message.
                oMessage.Attachments.Add(data);
            }

            oMessage.Subject = sSubject;
            if (sCC.Contains(","))
            {
                string[] CC = sCC.Split(',');
                foreach (string scc in CC)
                {
                    MailAddress copy = new MailAddress(scc);
                    oMessage.CC.Add(copy);
                }
            }
            else
            {
                if (sCC.Length > 0)
                {
                    MailAddress copy = new MailAddress(sCC);
                    oMessage.CC.Add(copy);
                }
            }
            oMessage.Body = sMessage;
            oMessage.IsBodyHtml = true;


            if (HttpContext.Current.Server.MachineName.ToUpper() == "BMW" || HttpContext.Current.Server.MachineName.ToUpper() == "RSPSERVER2" || HttpContext.Current.Server.MachineName.ToUpper() == "MERCEDES")
            {
                SmtpClient client = new SmtpClient(sMailServer);
                client.Credentials = theCredential;
                client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                client.Send(oMessage);

                // Display the values in the ContentDisposition for the attachment.
                ContentDisposition cd = data.ContentDisposition;
                Debug.WriteLine("Content disposition");
                Debug.WriteLine(cd.ToString());
                Debug.WriteLine("File {0}", cd.FileName);
                Debug.WriteLine("Size {0}", cd.Size);
                Debug.WriteLine("Creation {0}", cd.CreationDate);
                Debug.WriteLine("Modification {0}", cd.ModificationDate);
                Debug.WriteLine("Read {0}", cd.ReadDate);
                Debug.WriteLine("Inline {0}", cd.Inline);
                Debug.WriteLine("Parameters: {0}", cd.Parameters.Count);
                foreach (DictionaryEntry d in cd.Parameters)
                {
                    Console.WriteLine("{0} = {1}", d.Key, d.Value);
                }

                data.Dispose();
                return "Email Sent!";
            }
            else
            {

                SmtpClient client = new SmtpClient(sMailServer, 587);
                client.EnableSsl = true;
                //NetworkCredential theCredential = new NetworkCredential(sUserName, sPassword);
                client.Credentials = theCredential;
                client.Send(oMessage);

                // Display the values in the ContentDisposition for the attachment.
                ContentDisposition cd = data.ContentDisposition;
                Debug.WriteLine("Content disposition");
                Debug.WriteLine(cd.ToString());
                Debug.WriteLine("File {0}", cd.FileName);
                Debug.WriteLine("Size {0}", cd.Size);
                Debug.WriteLine("Creation {0}", cd.CreationDate);
                Debug.WriteLine("Modification {0}", cd.ModificationDate);
                Debug.WriteLine("Read {0}", cd.ReadDate);
                Debug.WriteLine("Inline {0}", cd.Inline);
                Debug.WriteLine("Parameters: {0}", cd.Parameters.Count);
                foreach (DictionaryEntry d in cd.Parameters)
                {
                    Console.WriteLine("{0} = {1}", d.Key, d.Value);
                }
                data.Dispose();

                return "Email Sent!";


            }

        }
        catch (SmtpException ex)
        {
            Debug.WriteLine(ex.ToString());
            return "ERROR: " + ex.ToString();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            Debug.WriteLine("Exception caught in CreateMessageWithAttachment(): {0}", ex.ToString());
            return "ERROR: " + ex.ToString();
        }
    }
}
