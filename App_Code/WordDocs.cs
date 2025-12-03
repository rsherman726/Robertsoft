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

/// <summary>
/// Summary description for WordDocs
/// </summary>
[Serializable()]
public sealed class WordDocuments
{
    private static FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

    public static string GetWordDoc(DataTable dt, string sMode, string sDocType, string sFileName, bool bNewPrices)
    {
        DataTable dtReport = new DataTable();
        string sBody = "";
        string sCustomer = "";//Cust#
        string sCustomerName = "";
        string sContactName = "";
        string sAddress = "";
        string sCSZ = "";
        string sTable = "";

        if (dt.Rows.Count > 0)
        {
            sCustomerName = dt.Rows[0]["CustomerName"].ToString();
            sCustomer = dt.Rows[0]["Customer"].ToString();//Cust#
            sContactName = dt.Rows[0]["ContactName"].ToString();
            sAddress = dt.Rows[0]["Address"].ToString();
            sCSZ = dt.Rows[0]["CSZ"].ToString();
        }
        sBody = "";

        if (sMode == "EDIT")//Loading...
        {
            if (sDocType == "E")
            {
                sBody = GetHTML(sFileName);
            }
            else
            {
                sBody = GetHTMLOriginal(sFileName);
            }
        }
        else
        {

            sBody = GetHTML(sFileName);//always load edited version for Word Doc...
            sBody = sBody.Replace("sCustomerName", sCustomerName);
            sBody = sBody.Replace("sContactName", sContactName);
            sBody = sBody.Replace("sAddress", sAddress);
            sBody = sBody.Replace("sCSZ", sCSZ);
            sBody = sBody.Replace("sDate", DateTime.Now.ToShortDateString());

            if (bNewPrices)
            {
                dtReport = GetReport(sCustomer);
                if (dtReport != null)
                {
                    if (dtReport.Rows.Count > 0)
                    {
                        sTable = "<table style='font-family:arial;border: medium solid #000000' width='100%'>";
                        sTable += "<tr style='font-family:arial;border: 1px solid #000000;color:white;background-color:navy'>";
                        sTable += "<td align='center' style='font-weight:bold;font-size:14pt;border: 1px solid #000000;'>Name</td><td align='center' style='font-weight:bold;font-size:14pt;border: 1px solid #000000'>Stock Code</td><td align='center' style='font-weight:bold;font-size:14pt;border: 1px solid #000000'>Description</td><td align='center' style='font-weight:bold;font-size:14pt;border: 1px solid #000000'>Current Price</td><td align='center' style='font-weight:bold;font-size:14pt;border: 1px solid #000000'>New Price</td>";
                        sTable += "</tr>";
                        int iRowIndex = 0;
                        foreach (DataRow row in dtReport.Rows)
                        {
                            string sNewPrice = "";

                            switch (row["Status"].ToString().ToUpper())
                            {
                                case "ACTIVE":
                                    sNewPrice = "$" + row["NewPrice"].ToString();
                                    break;
                                case "TO BE DISCONTINUED":
                                    sNewPrice = "<span style='color:orange'>" + row["Status"].ToString().ToUpper() + "</span>";
                                    break;
                                case "INACTIVE":
                                    sNewPrice = "<span style='color:red'>" + row["Status"].ToString().ToUpper() + "</span>";
                                    break;
                                case "MARKET PRICE":
                                    sNewPrice = "<span style='color:blue'>" + row["Status"].ToString().ToUpper() + "</span>";
                                    break;
                            }

                            if (iRowIndex % 2 == 0)
                            {

                                sTable += "<tr style='background-color: #EFF3FB;font-family:arial;border: 1px solid #000000'>";
                                sTable += "<td style='font-family:arial;border: 1px solid #000000'>" + row["Name"].ToString() + "</td>";
                                sTable += "<td style='font-family:arial;border: 1px solid #000000;text-align:center'>" + row["StockCode"].ToString() + "</td>";
                                sTable += "<td style='font-family:arial;border: 1px solid #000000'>" + row["Description"].ToString() + "</td>";
                                sTable += "<td align='right' style='font-family:arial;border: 1px solid #000000;text-align:center'>$" + row["CurrentPrice"].ToString() + "</td>";
                                sTable += "<td align='right' style='font-family:arial;border: 1px solid #000000;text-align:center'>" + sNewPrice + "</td>";
                                //sTable += "<td align='center' style='font-family:arial;border: 1px solid #000000'>" + row["Last Sale Date"].ToString() + "</td>";
                                sTable += "</tr>";
                            }
                            else
                            {
                                sTable += "<tr style='background-color: #fff;font-family:arial;border: 1px solid #000000'>";
                                sTable += "<td style='font-family:arial;border: 1px solid #000000'>" + row["Name"].ToString() + "</td>";
                                sTable += "<td style='font-family:arial;border: 1px solid #000000;text-align:center'>" + row["StockCode"].ToString() + "</td>";
                                sTable += "<td style='font-family:arial;border: 1px solid #000000'>" + row["Description"].ToString() + "</td>";
                                sTable += "<td align='right' style='font-family:arial;border: 1px solid #000000;text-align:center'>$" + row["CurrentPrice"].ToString() + "</td>";
                                sTable += "<td align='right' style='font-family:arial;border: 1px solid #000000;text-align:center'>" + sNewPrice + "</td>";
                                //sTable += "<td align='center' style='font-family:arial;border: 1px solid #000000'>" + row["Last Sale Date"].ToString() + "</td>";
                                sTable += "</tr>";

                            }



                            iRowIndex++;
                        }
                        sTable += "</table>";

                    }
                }
            }
            Char cDoubleQuotes = '"';
            string sDoubleQuotes = cDoubleQuotes.ToString();
            //Note:If no data then sTable is blank string...
            //sBody = sBody.Replace("’", "&rsquo;").Replace("‘", "&lsquo;").Replace("-", "&ndash;").Replace("—", "&mdash;").Replace("“", "&lsquo;").Replace("”", "&rdquo;").Replace(sDoubleQuotes, "&quot;");
            string sSignature = "";

            //sSignature += "<p style ='text-align:justify'><span style='font-size:14px'><span style='font-family:arial,helvetica,sans-serif'><span style='color:#222222'> Sincerely,</span></span></span></p>";
            //sSignature += "<p style ='text-align:justify'><span style='color:#222222; font-family:arial,helvetica,sans-serif; font-size:14px'> Felbro Food Products, Inc.</span></p>";
            //sSignature += "<p style ='text-align:justify'>&nbsp;</p>";
            sSignature += "<p style ='text-align:justify'><span style='font-family:arial,helvetica,sans-serif'><span style='font-size:14px'><em><strong><span style='color:#000000' > &nbsp; &nbsp; &nbsp;</span></strong></em></span></span></p>";
            sSignature += "<p style ='margin-left:40px'><span style='font-family:arial,helvetica,sans-serif'><span style='font-size:16px'> &nbsp;</span></span></p>";
            sSignature += "<p><span style='font-size:12px'><span style='font-family:arial,helvetica,sans-serif'> Felbro Food Products, Inc. | 5700 W.Adams Blvd. | Los Angeles, CA 90016 |</span></span></p>";
            sSignature += "<p><span style='font-size:12px'><span style='font-family:arial,helvetica,sans-serif'> Ph: 323.936.5266 | F: 323.936.5946 </span></span></p>";
            sSignature += "<p style='margin-left:40px'><span style='font-family:arial,helvetica,sans-serif'><span style='font-size:16px'><a href='http://www.felbro.com'> www.felbro.com</a></span></span></p>";
            sSignature += "<img src='http://www.robertsoftdev.com/Felbro_b/images/felbrologo.jpg' /></div>";
            sSignature += "<p> &nbsp;</p>";

            //Add table for report here...
            if (bNewPrices)
            {
                string sDisclaimer = "<p style='font-family:arial;font-size:9pt'> CONFIDENTIALITY NOTICE: This E-mail contains confidential information intended only for the individual or entity named within the message. If the reader of this message is not the intended recipient, or the agent responsible to deliver it to the intended recipient, you are hereby notified that any review, dissemination or copying of this communication is prohibited. If this communication was received in error, please notify us by reply E-mail and delete the original message.</p><br/>";
                sBody = "<html><body>" + sBody + "<p style='margin–bottom:'><br/><br/>" + sTable + sSignature + sDisclaimer + "</body></html>";
            }
        }

        dtReport.Dispose();

        return sBody;
    }
    private static string GetHTML(string sDocName)
    {
        string sHTML = "";

        var query = (
            from worddocs in db.WordDocs
            where
              worddocs.DocName == sDocName
            select new
            {
                worddocs.HTML
            });

        foreach (var r in query)
        {
            sHTML = r.HTML;
        };

        return sHTML;
    }
    private static string GetHTMLOriginal(string sDocName)
    {
        string sHTML = "";

        var query = (
            from worddocs in db.WordDocs
            where
              worddocs.DocName == sDocName
            select new
            {
                worddocs.OriginalHTML
            });

        foreach (var r in query)
        {
            sHTML = r.OriginalHTML;
        };

        return sHTML;
    }
    private static DataTable GetReport(string sCustomer)
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
}