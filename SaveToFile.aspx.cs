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


public partial class SaveToFile : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string sDocID = "";
        string sDocType = "";
        string sDocName = "";
        string sExtension = "";
        string sFullPath = "";
        try
        {



            String strImageName;
            HttpFileCollection files = HttpContext.Current.Request.Files;
            HttpPostedFile uploadfile = files["RemoteFile"];
            strImageName = uploadfile.FileName;
            sFullPath = Server.MapPath(".") + "\\Images\\Docs\\" + strImageName;
            uploadfile.SaveAs(sFullPath);
            if (File.Exists(sFullPath))
            {

                int iHyphenPosition = 0;
                int iPeriodPosition = 0;
                string sFirstPartOfFileName = "";
                string sSecondPartOfName = "";
                iHyphenPosition = strImageName.IndexOf("_");
                iPeriodPosition = strImageName.IndexOf(".");
                sFirstPartOfFileName = strImageName.Substring(0, iHyphenPosition);
                sSecondPartOfName = Path.GetFileNameWithoutExtension(strImageName).Replace(sFirstPartOfFileName, "");
                sDocID = sFirstPartOfFileName;
                sDocType = sSecondPartOfName.Trim().Replace("_", " ").Trim();
                sDocName = Path.GetFileNameWithoutExtension(strImageName);
                sExtension = Path.GetExtension(strImageName);

                //Records in ScanUpload History Table..
                FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
                DocScanUploadHistory ds = new DocScanUploadHistory();
                ds.DocID = sDocID;
                ds.DocType = sDocType;
                ds.DocName = sDocName;
                ds.Extension = sExtension;
                ds.DateAdded = DateTime.Now;
                db.DocScanUploadHistory.InsertOnSubmit(ds);
                db.SubmitChanges();


                if (sDocType == "Delivery Receipt")
                {
                    //When all else fails use the cache!!!!
                    if (Cache["DateDelivered"] != null)
                    {
                        DateTime dtDateDelivered = Convert.ToDateTime(Cache["DateDelivered"]);
                        DelMaster dm = db.DelMaster.Single(p => p.DeliveryID == Convert.ToInt32(sDocID));
                        dm.DeliveryStatus = 2;//Delivered...
                        dm.DateDelivered = dtDateDelivered;
                        db.SubmitChanges();
                    }
                    else
                    {
                        DelMaster dm = db.DelMaster.Single(p => p.DeliveryID == Convert.ToInt32(sDocID));
                        dm.DeliveryStatus = 2;//Delivered...
                        dm.DateDelivered = DateTime.Now;
                        db.SubmitChanges();
                    }
                }
                else if (sDocType == "Pickup")
                {
                    if (HttpContext.Current.Session["DateDelivered"] != null)
                    {
                        DateTime dtDateDelivered = Convert.ToDateTime(HttpContext.Current.Session["DateDelivered"]);
                        DelMaster dm = db.DelMaster.Single(p => p.DeliveryID == Convert.ToInt32(sDocID));
                        dm.DeliveryStatus = 1;//PickedUp...
                        dm.DateDelivered = dtDateDelivered;
                        db.SubmitChanges();
                    }
                    else
                    {
                        DelMaster dm = db.DelMaster.Single(p => p.DeliveryID == Convert.ToInt32(sDocID));
                        dm.DeliveryStatus = 1;//PickedUp...
                        dm.DateDelivered = DateTime.Now;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            if (File.Exists(sFullPath))
            {
                File.Delete(sFullPath);
            }
        }
    }
}