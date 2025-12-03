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
using System.Data.Linq.SqlClient;

public partial class DocSearch : System.Web.UI.Page
{
    const string sDocPath = @"Images\Docs\";

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

    #region Subs
    private void BindPDFData(List<string> lIDs)
    {
        string sCompanyName = "";
        string sPurchaseOrder = "";
        string sSalesOrder = "";
        string sDeliveryDate = "";
        string sFileName = "";
        string sDateCreated = "";
        string sPath = "";
        string sExtension = "";
        int iHyphenPosition = 0;
        DataTable dt = new DataTable();
        DataRow drRow;
        dt.Columns.Add("FullPath", typeof(String));
        dt.Columns.Add("FileName", typeof(String));
        dt.Columns.Add("Company", typeof(String));
        dt.Columns.Add("SalesOrder", typeof(String));
        dt.Columns.Add("CustomerPoNumber", typeof(String));
        dt.Columns.Add("DateDelivered", typeof(String));
        dt.Columns.Add("DateCreated", typeof(String));
        dt.Columns.Add("Extension", typeof(String));

        try
        {
            ////foreach (string x in Request.ServerVariables)
            ////{
            ////    Debug.WriteLine(x.ToString());
            ////    Debug.WriteLine(Request.ServerVariables[x].ToString());
            ////}



            sPath = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + sDocPath;
            Debug.WriteLine(sPath);
            DirectoryInfo diLienDocs = new DirectoryInfo(sPath);


            foreach (FileInfo fi in diLienDocs.GetFiles())
            {
                 
                sFileName = fi.Name;
                sExtension = fi.Extension;
                sDateCreated = fi.CreationTime.ToShortDateString();
                //Add to DataTable...                
                //if files starts with contract number then add to datatable...
                string sList = ".JPG,.PNG,.GIF,.DOC,.DOCX,XLS,.XLSX,.PDF,.TXT,.ACCDB,.MDB";
                string[] Ext = sList.Split(',');
                if (Ext.Contains(fi.Extension.ToUpper()))
                {
                    string sFirstPartOfFileName = "";
                    iHyphenPosition = sFileName.IndexOf("_");
                    sFirstPartOfFileName = sFileName.Substring(0, iHyphenPosition);
                    int iDeliveryID = 0;

                   
                   
                   // Debug.WriteLine(lIDs[0].ToString());
                    if (lIDs.Contains(sFirstPartOfFileName))
                    {
                        switch (rblDocType.SelectedIndex)
                        {
                            case 0://Delivery...
                                iDeliveryID = Convert.ToInt32(sFirstPartOfFileName);
                                sCompanyName = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["Name"].ToString().Trim();
                                sSalesOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["SalesOrder"].ToString().Trim();
                                sPurchaseOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["CustomerPoNumber"].ToString().Trim();
                                sDeliveryDate = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["DateDelivered"].ToString().Trim();
                                break;
                            case 1://Purchase Order...
                                sCompanyName = SharedFunctions.GetCompanyInfoFromPurchaseOrder(sFirstPartOfFileName).Rows[0]["Name"].ToString().Trim();
                                sSalesOrder = SharedFunctions.GetCompanyInfoFromPurchaseOrder(sFirstPartOfFileName).Rows[0]["SalesOrder"].ToString().Trim();
                                sPurchaseOrder = SharedFunctions.GetCompanyInfoFromPurchaseOrder(sFirstPartOfFileName).Rows[0]["CustomerPoNumber"].ToString().Trim();
                                sDeliveryDate = "";
                                break;
                            case 2://Sales Order...
                                sCompanyName = SharedFunctions.GetCompanyInfoFromSalesOrder(sFirstPartOfFileName).Rows[0]["Name"].ToString().Trim();
                                sSalesOrder = SharedFunctions.GetCompanyInfoFromSalesOrder(sFirstPartOfFileName).Rows[0]["SalesOrder"].ToString().Trim();
                                sPurchaseOrder = SharedFunctions.GetCompanyInfoFromSalesOrder(sFirstPartOfFileName).Rows[0]["CustomerPoNumber"].ToString().Trim();
                                sDeliveryDate = "";
                                break;
                        }
                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["Company"] = sCompanyName;
                        drRow["SalesOrder"] = sSalesOrder;
                        drRow["CustomerPoNumber"] = sPurchaseOrder;
                        drRow["DateDelivered"] = sDeliveryDate;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FullPath"] = sDocPath + sFileName;
                        drRow["Extension"] = sExtension;
                        dt.Rows.Add(drRow);
                    }
                }
            }

            if (dt.Rows.Count > 0)
            {
                gvPDFs.DataSource = dt;
                gvPDFs.DataBind();

                ViewState["gvPDFs"] = dt;
            }
            else//No records then create a dummy record to make Gridview still show up...
            {
                //Add Cols and MOAdd a blank row to the dataset
                DataTable dtDummy = new DataTable();
                dtDummy.Columns.Add("FullPath", typeof(String));
                dtDummy.Columns.Add("FileName", typeof(String));
                dtDummy.Columns.Add("Company", typeof(String));
                dtDummy.Columns.Add("SalesOrder", typeof(String));
                dtDummy.Columns.Add("CustomerPoNumber", typeof(String));
                dtDummy.Columns.Add("DateDelivered", typeof(String));
                dtDummy.Columns.Add("DateCreated", typeof(String));
                dtDummy.Columns.Add("Extension", typeof(String));

                dtDummy.Rows.Add(dtDummy.NewRow());
                //Bind the DataSet to the GridView
                gvPDFs.DataSource = dtDummy;
                gvPDFs.DataBind();
                //Get the number of columns to know what the Column Span should be
                int columnCount = gvPDFs.Rows[0].Cells.Count;
                //Call the clear method to clear out any controls that you use in the columns.  I use a dropdown list in one of the column so this was necessary.
                gvPDFs.Rows[0].Cells.Clear();
                gvPDFs.Rows[0].Cells.Add(new TableCell());
                gvPDFs.Rows[0].Cells[0].ColumnSpan = columnCount;
                gvPDFs.Rows[0].Cells[0].Text = "No Records Found.";
                gvPDFs.Rows[0].Cells[0].ForeColor = Color.Red;
                gvPDFs.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                dtDummy.Dispose();
            }
            dt.Dispose();
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
    private void BindPDFData(DataTable dt)
    {
        try
        {
         if (dt.Rows.Count > 0)
            {
                gvPDFs.DataSource = dt;
                gvPDFs.DataBind();

                ViewState["gvPDFs"] = dt;
            }
            else//No records then create a dummy record to make Gridview still show up...
            {
                //Add Cols and MOAdd a blank row to the dataset
                DataTable dtDummy = new DataTable();
                dtDummy.Columns.Add("FullPath", typeof(String));
                dtDummy.Columns.Add("FileName", typeof(String));
                dtDummy.Columns.Add("Company", typeof(String));
                dtDummy.Columns.Add("SalesOrder", typeof(String));
                dtDummy.Columns.Add("CustomerPoNumber", typeof(String));
                dtDummy.Columns.Add("DateDelivered", typeof(String));
                dtDummy.Columns.Add("DateCreated", typeof(String));
                dtDummy.Columns.Add("Extension", typeof(String));
                dtDummy.Columns.Add("DocType", typeof(String));
                dtDummy.Columns.Add("DeliveryID", typeof(String));
                dtDummy.Rows.Add(dtDummy.NewRow());
                //Bind the DataSet to the GridView
                gvPDFs.DataSource = dtDummy;
                gvPDFs.DataBind();
                //Get the number of columns to know what the Column Span should be
                int columnCount = gvPDFs.Rows[0].Cells.Count;
                //Call the clear method to clear out any controls that you use in the columns.  I use a dropdown list in one of the column so this was necessary.
                gvPDFs.Rows[0].Cells.Clear();
                gvPDFs.Rows[0].Cells.Add(new TableCell());
                gvPDFs.Rows[0].Cells[0].ColumnSpan = columnCount;
                gvPDFs.Rows[0].Cells[0].Text = "No Records Found.";
                gvPDFs.Rows[0].Cells[0].ForeColor = Color.Red;
                gvPDFs.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                dtDummy.Dispose();
            }
            dt.Dispose();
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
    private void BindPDFDataUploads(string sID)
    {
        string sFileName = "";
        string sDateCreated = "";
        string sPath = "";
        string sExtension = "";
        int iHyphenPosition = 0;
        DataTable dt = new DataTable();
        DataRow drRow;
        dt.Columns.Add("FullPath", typeof(String));
        dt.Columns.Add("FileName", typeof(String));
        dt.Columns.Add("DateCreated", typeof(String));
        dt.Columns.Add("Extension", typeof(String));

        try
        {
            ////foreach (string x in Request.ServerVariables)
            ////{
            ////    Debug.WriteLine(x.ToString());
            ////    Debug.WriteLine(Request.ServerVariables[x].ToString());
            ////}

            sPath = Request.ServerVariables["APPL_PHYSICAL_PATH"].ToString() + sDocPath;
            Debug.WriteLine(sPath);
            DirectoryInfo diLienDocs = new DirectoryInfo(sPath);


            foreach (FileInfo fi in diLienDocs.GetFiles())
            {
                sFileName = fi.Name;
                sExtension = fi.Extension;
                sDateCreated = fi.CreationTime.ToShortDateString();
                //Add to DataTable...                
                //if files starts with contract number then add to datatable...
                string sList = ".JPG,.PNG,.GIF,.DOC,.DOCX,XLS,.XLSX,.PDF,.TXT,.ACCDB,.MDB";
                string[] Ext = sList.Split(',');
                if (Ext.Contains(fi.Extension.ToUpper()))
                {
                    string sFirstPartOfFileName = "";
                    iHyphenPosition = sFileName.IndexOf("_");
                    sFirstPartOfFileName = sFileName.Substring(0, iHyphenPosition);
                    if (sFirstPartOfFileName == sID)
                    {
                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FullPath"] = sDocPath + sFileName;
                        drRow["Extension"] = sExtension;
                        dt.Rows.Add(drRow);
                    }
                }
            }

            if (dt.Rows.Count > 0)
            {
                gvPDFsUploads.DataSource = dt;
                gvPDFsUploads.DataBind();

                ViewState["gvPDFsUploads"] = dt;
            }
            else
            {
                gvPDFsUploads.DataSource = null;
                gvPDFsUploads.DataBind();

            }
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
                rblDocs.SelectedIndex = 0;
                txtCustomName.Visible = false;
                break;
            case 2:
                rblDocs.Items.Add(new ListItem("&nbsp;Purchase Order", "Purchase Order"));
                rblDocs.SelectedIndex = 0;
                txtCustomName.Visible = false;
                break;
            case 3:
                rblDocs.Items.Add(new ListItem("&nbsp;Custom Name", "Custom Name"));
                rblDocs.SelectedIndex = 0;
                txtCustomName.Visible = true;
                break;
        }
    }
    private void UploadDocuments(string sID)
    {
 

        lblUploadErr.Text = "";
        string sAbsolutePath = "";
        string sNewName = "";
        if (Server.MachineName.ToUpper() == "POWERHOUSE" || Server.MachineName.ToUpper() == "POWERHOUSEJR" || Server.MachineName.ToUpper() == "RSPSERVER")
        {
            sAbsolutePath = MapPath("..\\Felbro_B\\") + sDocPath;
        }
        else//Hosting Company...
        {
            sAbsolutePath = "d:\\inetpub\\wwwroot\\Felbro_B\\" + sDocPath;
        }
        try
        {
            if (fuDocuments.HasFile)
            {
                if (rblDocs.SelectedIndex == -1)
                {
                    lblUploadErr.Text = "**Please select a name for your document!<br/>";
                    return;
                }

                string sFileName = "";
                
                string sFirstPartOfFileName = "";
                sFileName = fuDocuments.FileName;

                switch (rblDocTypeIDSearch.SelectedIndex)
                {
                    case 0:
                        sNewName = "Delivery Receipt";
                        break;
                    case 1: 
                        sNewName = "Pickup";
                        break;
                    case 2:
                        sNewName = "Purchase_Order";
                        break;
                    case 3:
                        if (lblID.Text.Trim() == "")
                        {
                            lblMessageUpload.Text = "**If you select Custom Name, you must fill in the textbox below it!<br/>";
                            return;
                        }
                        sNewName = txtCustomName.Text.Trim().Replace(" ", "_").Replace("-", "_").Replace("/", "_").Replace("\\", "_");
                        break;
                }


                string sNameToReplace = "";
                int iIndexOfPeriod = 0;
                iIndexOfPeriod = sFileName.IndexOf(".");
                sNameToReplace = sFileName.Substring(0, iIndexOfPeriod);
                sNewName = sFileName.Replace(sNameToReplace, sNewName);
                sFirstPartOfFileName = sID + "_";
                sNewName = sFirstPartOfFileName + sNewName;

                //Validation...
                //Does file aleady Exists?...
                if (SharedFunctions.DocumentAlreadyExists(Path.GetFileNameWithoutExtension(sNewName)))
                {
                    lblMessageUpload.Text = "**Document already exists for this ID and Document Type!!";
                    lblMessageUpload.ForeColor = Color.Red;
                    return;
                }

                if (File.Exists(sAbsolutePath + sNewName))
                {
                    File.Delete(sAbsolutePath + sNewName);
                }
                 
                fuDocuments.SaveAs(sAbsolutePath + sNewName);
                ClearContents(fuDocuments);

                if (File.Exists(sAbsolutePath + sNewName))
                {


                    string sDocID = "";
                    string sDocType = "";
                    string sDocName = "";
                    string sExtension = "";
                    int iHyphenPosition = 0;
                    int iPeriodPosition = 0;

                    string sSecondPartOfName = "";
                    iHyphenPosition = sNewName.IndexOf("_");
                    iPeriodPosition = sNewName.IndexOf(".");
                    sFirstPartOfFileName = sNewName.Substring(0, iHyphenPosition);
                    if (rblDocs.SelectedIndex == 1)
                    {
                        sSecondPartOfName = txtCustomName.Text.Trim();
                    }
                    else
                    {
                        sSecondPartOfName = rblDocs.SelectedItem.Text.Replace("&nbsp;", "");
                    }

                    sDocID = sFirstPartOfFileName;
                    sDocType = sSecondPartOfName.Replace("_", " ").Trim();
                    sDocName = Path.GetFileNameWithoutExtension(sNewName);
                    sExtension = Path.GetExtension(sNewName);

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


                    if (rblDocTypeIDSearch.SelectedIndex == 0)//Delivery Receipt
                    {
                        if (txtDateDelivered.Text.Trim() != "")
                        {//Overwrite DateScheduled in session variable with user input...
                            HttpContext.Current.Session["DateDelivered"] = Convert.ToDateTime(txtDateDelivered.Text.Trim());
                        }
                        else
                        {
                            if (HttpContext.Current.Session["DateDelivered"] == null)
                            {//Not good...
                                lblUploadErr.Text = "**ERROR with delivery date session variable!!";
                                lblUploadErr.ForeColor = Color.Red;
                                return;
                            }
                        }
                    }

                    if (sDocType == "Delivery Receipt")
                    {
                        //When all else fails use the cache!!!!
                        if (HttpContext.Current.Session["DateDelivered"] != null)
                        {
                            DateTime dtDateDelivered = Convert.ToDateTime(HttpContext.Current.Session["DateDelivered"]);
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

                }//End if file exists...
            }
            else
            {
                lblUploadErr.Text = "**No file selected!";
                lblMessageUpload.ForeColor = Color.Red;
            }
        }
        catch (FileNotFoundException fex)
        {
            Debug.WriteLine(fex.ToString());
            lblUploadErr.Text = "**File not found!";
            lblMessageUpload.ForeColor = Color.Red;
        }
        catch (Exception ex)
        {
            if (File.Exists(sAbsolutePath + sNewName))
            {
                File.Delete(sAbsolutePath + sNewName);
            }
            Debug.WriteLine(ex.ToString());
            // lblUploadErr.Text = ex.Message;
        }
        finally
        {
            BindPDFDataUploads(sID);
        }
    }
    private void ClearContents(Control control)
    {

        for (var i = 0; i < Session.Keys.Count; i++)
        {

            if (Session.Keys[i].Contains(control.ClientID))
            {

                Session.Remove(Session.Keys[i]);

                break;

            }

        }

    }
    private void SearchIDs(string sSearch)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        sSearch = sSearch.Trim().Replace(" ", "").ToUpper();
        DataTable dtSearchIDs = new DataTable();
        switch (rblDocTypeIDSearch.SelectedIndex)
        {
            case 0://Delivery/Will Call...
                var qry = (from dm in db.DelMaster
                           join c in db.ArCustomer on dm.Customer equals c.Customer into c_join
                           from c in c_join.DefaultIfEmpty()
                           where  (new string[] { "2", "3" }).Contains(dm.DeliveryTypeID.ToString()) &&
                             dm.SalesOrder.Trim() == sSearch ||
                             dm.CustomerPoNumber.Trim().Replace(" ", "").ToUpper().Contains(sSearch) ||
                             (c.Name.Trim().ToUpper().Contains(sSearch.ToUpper()) || sSearch == "") ||
                             (c.Customer.Trim().Contains(sSearch.ToUpper()) || sSearch == "") ||
                             ((dm.DateDelivered.Value.Month.ToString() + "/" + dm.DateDelivered.Value.Day.ToString() + "/" + dm.DateDelivered.Value.Year.ToString()) == sSearch
                             || sSearch == "")
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
                              || c.Customer.Trim().Contains(sSearch.ToUpper()))
                             || (dm.DateDelivered.Value.Month.ToString() + "/" + dm.DateDelivered.Value.Day.ToString() + "/" + dm.DateDelivered.Value.Year.ToString() == sSearch)

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
    private List<string> SearchIDsMain(string sSearch)
    {
        sSearch = sSearch.Trim().Replace(" ", "").ToUpper();
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        List<string> ArrIDs = new List<string>();
        switch (rblDocType.SelectedIndex)
        {
            case 0://Delivery...
                var qry = (from dm in db.DelMaster
                           join c in db.ArCustomer on dm.Customer equals c.Customer into c_join
                           from c in c_join.DefaultIfEmpty()

                           where
                             dm.SalesOrder.Trim() == sSearch ||
                             dm.CustomerPoNumber.Trim().Replace(" ", "").ToUpper().Contains(sSearch) ||
                             (c.Name.Trim().ToUpper().Contains(sSearch.ToUpper()) || sSearch == "") ||
                             (c.Customer.Trim().Contains(sSearch.ToUpper()) || sSearch == "") 
                             ||
                             (dm.DateDelivered.Value.Month.ToString() + "/" + dm.DateDelivered.Value.Day.ToString() + "/" + dm.DateDelivered.Value.Year.ToString()) == sSearch
                             || sSearch == ""
                           select new
                           {
                               ID = dm.DeliveryID
                           }).Take(100);
               // int iCount = qry.Count();

                List<string> stringArr = new List<string>();
                stringArr = qry.Select(i => i.ID.ToString()).ToList();               
                ArrIDs = stringArr;
                Session["dtSearchIDs"] = stringArr;
                break;
            case 1://Purchase ORders...
                var qry1 = (from sm in db.SorMaster
                            join c in db.ArCustomer on sm.Customer equals c.Customer
                            where
                              sm.SalesOrder.Trim() == sSearch ||
                              sm.CustomerPoNumber.Trim().Replace(" ", "").ToUpper().Contains(sSearch) ||
                              c.Name.Trim().ToUpper().Contains(sSearch.ToUpper())  ||
                              c.Customer.Trim().Contains(sSearch.ToUpper())  ||
                             (sm.OrderDate.Value.Month.ToString() + "/" + sm.OrderDate.Value.Day.ToString() + "/" + sm.OrderDate.Value.Year.ToString()) == sSearch
                             || sSearch == ""
                            select new
                            {
                                ID = sm.CustomerPoNumber.Trim()
                            }).Take(100);
                List<string> stringArr1 = new List<string>();
                stringArr1 = qry1.Select(i => i.ID).ToList();
                ArrIDs = stringArr1;
                Session["dtSearchIDs"] = stringArr1;
                break;
            case 2://Sales Orders...
                var qry2 = (from sm in db.SorMaster
                            join c in db.ArCustomer on sm.Customer equals c.Customer
                            where
                              sm.SalesOrder.Trim() == sSearch ||
                              sm.CustomerPoNumber.Trim().Replace(" ", "").ToUpper().Contains(sSearch) ||
                              c.Name.Trim().ToUpper().Contains(sSearch.ToUpper()) ||
                              c.Customer.Trim().Contains(sSearch.ToUpper()) ||
                             (sm.OrderDate.Value.Month.ToString() + "/" + sm.OrderDate.Value.Day.ToString() + "/" + sm.OrderDate.Value.Year.ToString()) == sSearch
                             || sSearch == ""
                            select new
                            {
                                ID = sm.SalesOrder.Trim()
                            }).Take(100);

                List<string> stringArr2 = new List<string>();
                stringArr2 = qry2.Select(i => i.ID).ToList();
                ArrIDs = stringArr2;
                Session["dtSearchIDs"] = stringArr2;

                break;

        }


        return ArrIDs;
    }
    private DataTable SearchIDsMainDataTable(string sSearch, string sStartDate,string sEndDate)
    {//Sarching for Scanned or Uploaded Docs...       
        int iDeliveryID = 0;
        string sCompanyName = "";
        string sPurchaseOrder = "";
        string sSalesOrder = "";
        string sDeliveryDate = "";
        string sFileName = "";
        string sDateCreated = "";
        string sDocType = "";
        string sExtension = "";
        DataTable dtAllDocs = new DataTable();
        DataTable dt = new DataTable();
        DataRow drRow;
        dt.Columns.Add("FullPath", typeof(String));
        dt.Columns.Add("FileName", typeof(String));
        dt.Columns.Add("Company", typeof(String));
        dt.Columns.Add("SalesOrder", typeof(String));
        dt.Columns.Add("CustomerPoNumber", typeof(String));
        dt.Columns.Add("DateDelivered", typeof(String));
        dt.Columns.Add("DateCreated", typeof(String));
        dt.Columns.Add("Extension", typeof(String));
        dt.Columns.Add("DocType", typeof(String));
        dt.Columns.Add("DeliveryID", typeof(String));
        
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        try
        {
            switch (rblDocType.SelectedIndex)
            {
                case 0://Delivery...
                    var qry = (from dh in db.DocScanUploadHistory
                               join dm in db.DelMaster on new { DocID = dh.DocID } equals new { DocID = dm.DeliveryID.ToString() }
                               join c in db.ArCustomer on dm.Customer equals c.Customer into c_join
                               from c in c_join.DefaultIfEmpty()
                               where
                                 dh.DocType == "Delivery Receipt" &&
                                 dh.DateAdded >= Convert.ToDateTime(sStartDate + " 00:00:00") && dh.DateAdded <= Convert.ToDateTime(sEndDate+" 23:59:59") &&
                                 (c.Name.ToUpper().Contains(sSearch.ToUpper()) ||
                                 c.Customer.Contains(sSearch) ||
                                 dm.SalesOrder == sSearch ||
                                 dm.CustomerPoNumber.ToUpper().Trim() == sSearch.ToUpper() ||
                                 dm.DeliveryID.ToString() == sSearch )
                               select new
                               {
                                   dm.DeliveryID,
                                   dh.ScanUploadID,
                                   dh.DocID,
                                   dh.DocType,
                                   dh.DocName,
                                   dh.Extension,
                                   dh.DateAdded
                               }).Take(250);
                   
                    foreach (var a in qry)
                    {

                        iDeliveryID = Convert.ToInt32(a.DocID);
                        sCompanyName = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["Name"].ToString().Trim();
                        sSalesOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["SalesOrder"].ToString().Trim();
                        sPurchaseOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["CustomerPoNumber"].ToString().Trim();
                        sDeliveryDate = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["DateDelivered"].ToString().Trim();
                        sDateCreated = a.DateAdded.ToShortDateString();
                        sExtension = a.Extension;
                        sFileName = a.DocName + a.Extension;
                        sDocType = a.DocType;

                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["Company"] = sCompanyName;
                        drRow["SalesOrder"] = sSalesOrder;
                        drRow["CustomerPoNumber"] = sPurchaseOrder;
                        drRow["DateDelivered"] = sDeliveryDate;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FullPath"] = sDocPath + sFileName;
                        drRow["Extension"] = sExtension;
                        drRow["DocType"] = sDocType;
                        drRow["DeliveryID"] = iDeliveryID.ToString();
                        dt.Rows.Add(drRow);
                    }

                    ViewState["gvPDFs"] = dt;
                    break;
                case 1://Pickup...
                    var qry0 = (from dh in db.DocScanUploadHistory                                
                                join dm in db.DelMaster on new { DocID = dh.DocID } equals new { DocID = dm.DeliveryID.ToString() }
                                join c in db.ArCustomer on dm.Customer equals c.Customer into c_join
                                from c in c_join.DefaultIfEmpty()
                                where
                                 dh.DocType == "Pickup" &&
                                 dh.DateAdded >= Convert.ToDateTime(sStartDate + " 00:00:00") && dh.DateAdded <= Convert.ToDateTime(sEndDate + " 23:59:59") &&
                                 (c.Name.ToUpper().Contains(sSearch.ToUpper()) ||
                                 c.Customer.Contains(sSearch) ||
                                 dm.SalesOrder == sSearch ||
                                 dm.CustomerPoNumber.ToUpper().Trim() == sSearch.ToUpper() ||
                                 dm.DeliveryID.ToString() == sSearch)
                               select new
                               {
                                   dm.DeliveryID,
                                   dh.ScanUploadID,
                                   dh.DocID,
                                   dh.DocType,
                                   dh.DocName,
                                   dh.Extension,
                                   dh.DateAdded
                               }).Take(250);

                    foreach (var a in qry0)
                    {

                        iDeliveryID = Convert.ToInt32(a.DocID);
                        sCompanyName = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["Name"].ToString().Trim();
                        sSalesOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["SalesOrder"].ToString().Trim();
                        sPurchaseOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["CustomerPoNumber"].ToString().Trim();
                        sDeliveryDate = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["DateDelivered"].ToString().Trim();
                        sDateCreated = a.DateAdded.ToShortDateString();
                        sExtension = a.Extension;
                        sFileName = a.DocName + a.Extension;
                        sDocType = a.DocType;

                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["Company"] = sCompanyName;
                        drRow["SalesOrder"] = sSalesOrder;
                        drRow["CustomerPoNumber"] = sPurchaseOrder;
                        drRow["DateDelivered"] = sDeliveryDate;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FullPath"] = sDocPath + sFileName;
                        drRow["Extension"] = sExtension;
                        drRow["DocType"] = sDocType;
                        drRow["DeliveryID"] = iDeliveryID.ToString();
                        dt.Rows.Add(drRow);
                    }

                    ViewState["gvPDFs"] = dt;
                    break;
                case 2://Purchase Orders...    
                    var qry1 = (
                                from a in
                                    (
                                        ((from dh in db.DocScanUploadHistory
                                          join sm in db.SorMaster on dh.DocID.Trim() equals sm.CustomerPoNumber.Trim().Replace(" ", "")
                                          join c in db.ArCustomer on sm.Customer equals c.Customer
                                          where
                                            dh.DocType == "Purchase Order" &&
                                       dh.DateAdded >= Convert.ToDateTime(sStartDate + " 00:00:00") && dh.DateAdded <= Convert.ToDateTime(sEndDate + " 23:59:59") &&
                                    (sm.CustomerPoNumber.Trim().ToUpper().Trim().Replace(" ", "").Contains(sSearch.ToUpper().Trim().Replace(" ", "")) || sm.SalesOrder.Trim() == sSearch
                                    || c.Name.Trim().ToUpper().Contains(sSearch.ToUpper())
                                    || c.Customer.Trim().Contains(sSearch.ToUpper()))

                                          select new
                                          {
                                              dh.ScanUploadID,
                                              dh.DocID,
                                              dh.DocType,
                                              dh.DocName,
                                              dh.Extension,
                                              dh.DateAdded
                                          }).Take(250)))
                                group a by new
                                {
                                    a.DocID
                                } into g
                                select new
                                {
                                    DocID = g.Min(p => p.DocID),
                                    DocType = g.Min(p => p.DocType),
                                    DocName = g.Min(p => p.DocName),
                                    ScanUploadID = g.Min(p => Convert.ToString(p.ScanUploadID)),
                                    Extension = g.Min(p => p.Extension),
                                    DateAdded = Convert.ToDateTime(g.Min(p => p.DateAdded))
                                }).Take(250);                    
                   
                    foreach (var a in qry1)
                    {

                        sCompanyName = SharedFunctions.GetCompanyInfoFromPurchaseOrder(a.DocID).Rows[0]["Name"].ToString().Trim();
                        sSalesOrder = SharedFunctions.GetCompanyInfoFromPurchaseOrder(a.DocID).Rows[0]["SalesOrder"].ToString().Trim();
                        sPurchaseOrder = SharedFunctions.GetCompanyInfoFromPurchaseOrder(a.DocID).Rows[0]["CustomerPoNumber"].ToString().Trim();
                        sDeliveryDate = "";
                        sDateCreated = a.DateAdded.ToShortDateString();
                        sExtension = a.Extension;
                        sFileName = a.DocName + a.Extension;
                        sDocType = a.DocType;

                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["Company"] = sCompanyName;
                        drRow["SalesOrder"] = sSalesOrder;
                        drRow["CustomerPoNumber"] = sPurchaseOrder;
                        drRow["DateDelivered"] = sDeliveryDate;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FullPath"] = sDocPath + sFileName;
                        drRow["Extension"] = sExtension;
                        drRow["DocType"] = sDocType;
                        drRow["DeliveryID"] = DBNull.Value;
                        dt.Rows.Add(drRow);
                    }
                    ViewState["gvPDFs"] = dt;
                    break;

                case 3://All Docs
                    string sSQL = "EXEC spGetAllDocuments @Search ='" + sSearch.Replace("'","''") + "',@StartDate='" + sStartDate + "',@EndDate='" + sEndDate + "'";
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
                    Debug.WriteLine(sSQL);
                    dtAllDocs = SharedFunctions.getDataTable(sSQL, conn, "dtAllDocs");

                    int iCount = dtAllDocs.Rows.Count;
                    foreach (DataRow r in dtAllDocs.Rows)
                    {
                        switch (r["DocType"].ToString())
                        {
                            case "Delivery Receipt":
                                iDeliveryID = Convert.ToInt32(r["DocID"]);
                                sCompanyName = r["Name"].ToString().Trim();
                                sSalesOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["SalesOrder"].ToString().Trim();
                                sPurchaseOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["CustomerPoNumber"].ToString().Trim();
                                sDeliveryDate = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["DateDelivered"].ToString().Trim();
                                break;
                            case "Purchase Order":
                                sCompanyName = r["Name"].ToString().Trim();
                                sSalesOrder = SharedFunctions.GetCompanyInfoFromPurchaseOrder(r["DocID"].ToString()).Rows[0]["SalesOrder"].ToString().Trim();
                                sPurchaseOrder = SharedFunctions.GetCompanyInfoFromPurchaseOrder(r["DocID"].ToString()).Rows[0]["CustomerPoNumber"].ToString().Trim();
                                sDeliveryDate = "";
                                break;
                            case "Sales Order":
                                sCompanyName = r["Name"].ToString().Trim();
                                sSalesOrder = SharedFunctions.GetCompanyInfoFromSalesOrder(r["DocID"].ToString()).Rows[0]["SalesOrder"].ToString().Trim();
                                sPurchaseOrder = SharedFunctions.GetCompanyInfoFromSalesOrder(r["DocID"].ToString()).Rows[0]["CustomerPoNumber"].ToString().Trim();
                                sDeliveryDate = "";
                                break;
                            default:
                                sCompanyName = r["Name"].ToString().Trim();
                                sSalesOrder = SharedFunctions.GetCompanyInfoFromAllDocs(r["DocID"].ToString()).Rows[0]["SalesOrder"].ToString().Trim();
                                sPurchaseOrder = SharedFunctions.GetCompanyInfoFromAllDocs(r["DocID"].ToString()).Rows[0]["CustomerPoNumber"].ToString().Trim();
                                sDeliveryDate = "";
                                break;
                        }
                        sDocType = r["DocType"].ToString();
                        sDateCreated = Convert.ToDateTime(r["DateAdded"]).ToShortDateString();
                        sExtension = r["Extension"].ToString();
                        sFileName = r["DocName"].ToString() + r["Extension"].ToString();

                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["Company"] = sCompanyName;
                        drRow["SalesOrder"] = sSalesOrder;
                        drRow["CustomerPoNumber"] = sPurchaseOrder;
                        drRow["DateDelivered"] = sDeliveryDate;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FullPath"] = sDocPath + sFileName;
                        drRow["Extension"] = sExtension;
                        drRow["DocType"] = sDocType;
                        drRow["DeliveryID"] = r["DeliveryID"].ToString();
                        dt.Rows.Add(drRow);
                    }
                    ViewState["gvPDFs"] = dt;
                    break;

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            dt.Dispose();
            dtAllDocs.Dispose();
        }

        return dt;
    }
    private DataTable SearchIDsMainDataTableLinkClick(string sSearch, string sStartDate, string sEndDate, string sColumn)
    {
        //NOTE: here, DR id both Delivery and Pickup since the DeliveryID is for both
        int iDeliveryID = 0;
        string sCompanyName = "";
        string sPurchaseOrder = "";
        string sSalesOrder = "";
        string sDeliveryDate = "";
        string sFileName = "";
        string sDateCreated = "";
        string sDocType = "";
        string sExtension = "";
        DataTable dtAllDocs = new DataTable();
        DataTable dt = new DataTable();
        DataRow drRow;
        dt.Columns.Add("FullPath", typeof(String));
        dt.Columns.Add("FileName", typeof(String));
        dt.Columns.Add("Company", typeof(String));
        dt.Columns.Add("SalesOrder", typeof(String));
        dt.Columns.Add("CustomerPoNumber", typeof(String));
        dt.Columns.Add("DateDelivered", typeof(String));
        dt.Columns.Add("DateCreated", typeof(String));
        dt.Columns.Add("Extension", typeof(String));
        dt.Columns.Add("DocType", typeof(String));
        dt.Columns.Add("DeliveryID", typeof(String));

        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        try
        {
            switch (rblDocType.SelectedIndex)
            {
                case 0://Delivery...
                    var qry = (from dh in db.DocScanUploadHistory
                               join dm in db.DelMaster on new { DocID = dh.DocID } equals new { DocID = dm.DeliveryID.ToString() }
                               join c in db.ArCustomer on dm.Customer equals c.Customer into c_join
                               from c in c_join.DefaultIfEmpty()
                               where
                                 dh.DocType == "Delivery Receipt" &&
                                  dh.DateAdded >= Convert.ToDateTime(sStartDate + " 00:00:00") && dh.DateAdded <= Convert.ToDateTime(sEndDate + " 23:59:59") &&
                                (sColumn == "CO" ? c.Name.Trim().ToUpper().Contains(sSearch.ToUpper()) :
                                 sColumn == "SO" ? dm.SalesOrder.Trim() == sSearch :
                                 sColumn == "PO" ? dm.CustomerPoNumber.ToUpper().Trim() == sSearch.ToUpper() :
                                 sColumn == "DR" ? dm.DeliveryID.ToString() == sSearch : sSearch == null )
                               select new
                               {
                                   dm.DeliveryID,
                                   dh.ScanUploadID,
                                   dh.DocID,
                                   dh.DocType,
                                   dh.DocName,
                                   dh.Extension,
                                   dh.DateAdded
                               }).Take(250);

                    foreach (var a in qry)
                    {

                        iDeliveryID = Convert.ToInt32(a.DocID);
                        sCompanyName = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["Name"].ToString().Trim();
                        sSalesOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["SalesOrder"].ToString().Trim();
                        sPurchaseOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["CustomerPoNumber"].ToString().Trim();
                        sDeliveryDate = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["DateDelivered"].ToString().Trim();
                        sDateCreated = a.DateAdded.ToShortDateString();
                        sExtension = a.Extension;
                        sFileName = a.DocName + a.Extension;
                        sDocType = a.DocType;

                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["Company"] = sCompanyName;
                        drRow["SalesOrder"] = sSalesOrder;
                        drRow["CustomerPoNumber"] = sPurchaseOrder;
                        drRow["DateDelivered"] = sDeliveryDate;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FullPath"] = sDocPath + sFileName;
                        drRow["Extension"] = sExtension;
                        drRow["DocType"] = sDocType;
                        drRow["DeliveryID"] = iDeliveryID.ToString();
                        dt.Rows.Add(drRow);
                    }

                    ViewState["gvPDFs"] = dt;
                    break;
                case 1://Pickup...
                    var qry0 = (from dh in db.DocScanUploadHistory
                               join dm in db.DelMaster on new { DocID = dh.DocID } equals new { DocID = dm.DeliveryID.ToString() }
                                join c in db.ArCustomer on dm.Customer equals c.Customer into c_join
                                from c in c_join.DefaultIfEmpty()
                                where
                                 dh.DocType == "Pickup" &&
                                  dh.DateAdded >= Convert.ToDateTime(sStartDate + " 00:00:00") && dh.DateAdded <= Convert.ToDateTime(sEndDate + " 23:59:59") &&
                                (sColumn == "CO" ? c.Name.Trim().ToUpper().Contains(sSearch.ToUpper()) :
                                 sColumn == "SO" ? dm.SalesOrder.Trim() == sSearch :
                                 sColumn == "PO" ? dm.CustomerPoNumber.ToUpper().Trim() == sSearch.ToUpper() :
                                 sColumn == "DR" ? dm.DeliveryID.ToString() == sSearch : sSearch == null)
                               select new
                               {
                                   dm.DeliveryID,
                                   dh.ScanUploadID,
                                   dh.DocID,
                                   dh.DocType,
                                   dh.DocName,
                                   dh.Extension,
                                   dh.DateAdded
                               }).Take(250);

                    foreach (var a in qry0)
                    {

                        iDeliveryID = Convert.ToInt32(a.DocID);
                        sCompanyName = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["Name"].ToString().Trim();
                        sSalesOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["SalesOrder"].ToString().Trim();
                        sPurchaseOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["CustomerPoNumber"].ToString().Trim();
                        sDeliveryDate = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["DateDelivered"].ToString().Trim();
                        sDateCreated = a.DateAdded.ToShortDateString();
                        sExtension = a.Extension;
                        sFileName = a.DocName + a.Extension;
                        sDocType = a.DocType;

                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["Company"] = sCompanyName;
                        drRow["SalesOrder"] = sSalesOrder;
                        drRow["CustomerPoNumber"] = sPurchaseOrder;
                        drRow["DateDelivered"] = sDeliveryDate;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FullPath"] = sDocPath + sFileName;
                        drRow["Extension"] = sExtension;
                        drRow["DocType"] = sDocType;
                        drRow["DeliveryID"] = iDeliveryID.ToString();
                        dt.Rows.Add(drRow);
                    }

                    ViewState["gvPDFs"] = dt;
                    break;
                case 2://Purchase Orders...    
                    var qry1 = (from dh in db.DocScanUploadHistory
                                join sm in db.SorMaster on dh.DocID.Trim() equals (sm.CustomerPoNumber).Trim().Replace(" ", "")
                                join c in db.ArCustomer on sm.Customer equals c.Customer
                                where
                                  dh.DocType == "Purchase Order" &&
                                  dh.DateAdded >= Convert.ToDateTime(sStartDate + " 00:00:00") && dh.DateAdded <= Convert.ToDateTime(sEndDate + " 23:59:59") &&                                   
                                (sColumn == "CO" ? c.Name.ToUpper().Contains(sSearch.ToUpper()) :
                                 sColumn == "SO" ? sm.SalesOrder.Trim() == sSearch :
                                 sColumn == "PO" ? sm.CustomerPoNumber.Trim().ToUpper().Trim().Replace(" ", "").Contains(sSearch.ToUpper().Trim().Replace(" ", "")) :
                                 sSearch == null ) 
                                select new
                                {
                                    dh.ScanUploadID,
                                    dh.DocID,
                                    dh.DocType,
                                    dh.DocName,
                                    dh.Extension,
                                    dh.DateAdded
                                }).Take(250);



                    foreach (var a in qry1)
                    {

                        sCompanyName = SharedFunctions.GetCompanyInfoFromPurchaseOrder(a.DocID).Rows[0]["Name"].ToString().Trim();
                        sSalesOrder = SharedFunctions.GetCompanyInfoFromPurchaseOrder(a.DocID).Rows[0]["SalesOrder"].ToString().Trim();
                        sPurchaseOrder = SharedFunctions.GetCompanyInfoFromPurchaseOrder(a.DocID).Rows[0]["CustomerPoNumber"].ToString().Trim();
                        sDeliveryDate = "";
                        sDateCreated = a.DateAdded.ToShortDateString();
                        sExtension = a.Extension;
                        sFileName = a.DocName + a.Extension;
                        sDocType = a.DocType;

                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["Company"] = sCompanyName;
                        drRow["SalesOrder"] = sSalesOrder;
                        drRow["CustomerPoNumber"] = sPurchaseOrder;
                        drRow["DateDelivered"] = sDeliveryDate;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FullPath"] = sDocPath + sFileName;
                        drRow["Extension"] = sExtension;
                        drRow["DocType"] = sDocType;
                        drRow["DeliveryID"] = DBNull.Value;
                        dt.Rows.Add(drRow);
                    }
                    ViewState["gvPDFs"] = dt;
                    break;              
                case 3://All Docs
                    string sSQL = "EXEC spGetAllDocuments @Search ='" + sSearch.Replace("'", "''") + "',@StartDate='" + sStartDate + "',@EndDate='" + sEndDate + "'";
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
                    Debug.WriteLine(sSQL);
                    dtAllDocs = SharedFunctions.getDataTable(sSQL, conn, "dtAllDocs");

                    int iCount = dtAllDocs.Rows.Count;
                    foreach (DataRow r in dtAllDocs.Rows)
                    {
                        switch (r["DocType"].ToString())
                        {
                            case "Delivery Receipt":
                                iDeliveryID = Convert.ToInt32(r["DocID"]);
                                sCompanyName = r["Name"].ToString().Trim();
                                sSalesOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["SalesOrder"].ToString().Trim();
                                sPurchaseOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["CustomerPoNumber"].ToString().Trim();
                                sDeliveryDate = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["DateDelivered"].ToString().Trim();
                                break;
                            case "Purchase Order":
                                sCompanyName = r["Name"].ToString().Trim();
                                sSalesOrder = SharedFunctions.GetCompanyInfoFromPurchaseOrder(r["DocID"].ToString()).Rows[0]["SalesOrder"].ToString().Trim();
                                sPurchaseOrder = SharedFunctions.GetCompanyInfoFromPurchaseOrder(r["DocID"].ToString()).Rows[0]["CustomerPoNumber"].ToString().Trim();
                                sDeliveryDate = "";
                                break;
                            case "Sales Order":
                                sCompanyName = r["Name"].ToString().Trim();
                                sSalesOrder = SharedFunctions.GetCompanyInfoFromSalesOrder(r["DocID"].ToString()).Rows[0]["SalesOrder"].ToString().Trim();
                                sPurchaseOrder = SharedFunctions.GetCompanyInfoFromSalesOrder(r["DocID"].ToString()).Rows[0]["CustomerPoNumber"].ToString().Trim();
                                sDeliveryDate = "";
                                break;
                            default:
                                sCompanyName = r["Name"].ToString().Trim();
                                sSalesOrder = SharedFunctions.GetCompanyInfoFromAllDocs(r["DocID"].ToString()).Rows[0]["SalesOrder"].ToString().Trim();
                                sPurchaseOrder = SharedFunctions.GetCompanyInfoFromAllDocs(r["DocID"].ToString()).Rows[0]["CustomerPoNumber"].ToString().Trim();
                                sDeliveryDate = "";
                                break;
                        }
                        sDocType = r["DocType"].ToString();
                        sDateCreated = Convert.ToDateTime(r["DateAdded"]).ToShortDateString();
                        sExtension = r["Extension"].ToString();
                        sFileName = r["DocName"].ToString() + r["Extension"].ToString();

                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["Company"] = sCompanyName;
                        drRow["SalesOrder"] = sSalesOrder;
                        drRow["CustomerPoNumber"] = sPurchaseOrder;
                        drRow["DateDelivered"] = sDeliveryDate;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FullPath"] = sDocPath + sFileName;
                        drRow["Extension"] = sExtension;
                        drRow["DocType"] = sDocType;
                        drRow["DeliveryID"] = r["DeliveryID"].ToString();
                        dt.Rows.Add(drRow);
                    }
                    ViewState["gvPDFs"] = dt;
                    break;

            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            dt.Dispose();
            dtAllDocs.Dispose();
        }

        return dt;
    }
    private DataTable SearchIDsMainDataTableFromSalesOrderTracker(string sSearch)
    {
        //NO PICKUP in Sales Order Tracker...
        int iDeliveryID = 0;
        string sCompanyName = "";
        string sPurchaseOrder = "";
        string sSalesOrder = "";
        string sDeliveryDate = "";
        string sFileName = "";
        string sDateCreated = "";
        string sExtension = "";
        string sDocType = "";
        DataTable dtAllDocs = new DataTable();
        DataTable dt = new DataTable();
        DataRow drRow;
        dt.Columns.Add("FullPath", typeof(String));
        dt.Columns.Add("FileName", typeof(String));
        dt.Columns.Add("Company", typeof(String));
        dt.Columns.Add("SalesOrder", typeof(String));
        dt.Columns.Add("CustomerPoNumber", typeof(String));
        dt.Columns.Add("DateDelivered", typeof(String));
        dt.Columns.Add("DateCreated", typeof(String));
        dt.Columns.Add("Extension", typeof(String));
        dt.Columns.Add("DocType", typeof(String));
        dt.Columns.Add("DeliveryID", typeof(String));

        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        try
        {
            switch (rblDocType.SelectedIndex)
            {
                case 0://Delivery...
                    var qry = (from dh in db.DocScanUploadHistory
                               join dm in db.DelMaster on new { DocID = dh.DocID } equals new { DocID = dm.DeliveryID.ToString() }
                               where
                                 dm.DeliveryID.ToString() == sSearch
                                 && dh.DocType =="Delivery Receipt"
                               select new
                               {
                                   dm.DeliveryID,
                                   dh.ScanUploadID,
                                   dh.DocID,
                                   dh.DocType,
                                   dh.DocName,
                                   dh.Extension,
                                   dh.DateAdded
                               }).Take(250);

                    foreach (var a in qry)
                    {

                        iDeliveryID = Convert.ToInt32(a.DeliveryID);
                        sCompanyName = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["Name"].ToString().Trim();
                        sSalesOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["SalesOrder"].ToString().Trim();
                        sPurchaseOrder = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["CustomerPoNumber"].ToString().Trim();
                        sDeliveryDate = SharedFunctions.GetCompanyInfoFromDeliveryIDInDeliveries(iDeliveryID).Rows[0]["DateDelivered"].ToString().Trim();
                        sDateCreated = a.DateAdded.ToShortDateString();
                        sExtension = a.Extension;
                        sFileName = a.DocName + a.Extension;
                        sDocType = a.DocType;

                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["Company"] = sCompanyName;
                        drRow["SalesOrder"] = sSalesOrder;
                        drRow["CustomerPoNumber"] = sPurchaseOrder;
                        drRow["DateDelivered"] = sDeliveryDate;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FullPath"] = sDocPath + sFileName;
                        drRow["Extension"] = sExtension;
                        drRow["DocType"] = sDocType;
                        drRow["DeliveryID"] = iDeliveryID.ToString();

                        dt.Rows.Add(drRow);
                    }

                    ViewState["gvPDFs"] = dt;
                    break;
                case 1://Purchase Orders...    
                    var qry1 = (from dh in db.DocScanUploadHistory
                                join sm in db.SorMaster on dh.DocID.Trim().Trim().Replace(" ", "") equals (sm.CustomerPoNumber).Trim().Replace(" ", "")
                                where
                                  sm.CustomerPoNumber.Trim().ToUpper().Trim().Replace(" ", "").Contains(sSearch.ToUpper())
                                select new
                                {
                                    dh.ScanUploadID,
                                    dh.DocID,
                                    dh.DocType,
                                    dh.DocName,
                                    dh.Extension,
                                    dh.DateAdded
                                }).Take(250);

                    foreach (var a in qry1)
                    {

                        sCompanyName = SharedFunctions.GetCompanyInfoFromPurchaseOrder(a.DocID).Rows[0]["Name"].ToString().Trim();
                        sSalesOrder = SharedFunctions.GetCompanyInfoFromPurchaseOrder(a.DocID).Rows[0]["SalesOrder"].ToString().Trim();
                        sPurchaseOrder = SharedFunctions.GetCompanyInfoFromPurchaseOrder(a.DocID).Rows[0]["CustomerPoNumber"].ToString().Trim();
                        sDeliveryDate = "";
                        sDateCreated = a.DateAdded.ToShortDateString();
                        sExtension = a.Extension;
                        sFileName = a.DocName + a.Extension;
                        sDocType = a.DocType;

                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["Company"] = sCompanyName;
                        drRow["SalesOrder"] = sSalesOrder;
                        drRow["CustomerPoNumber"] = sPurchaseOrder;
                        drRow["DateDelivered"] = sDeliveryDate;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FullPath"] = sDocPath + sFileName;
                        drRow["Extension"] = sExtension;
                        drRow["DocType"] = sDocType;
                        drRow["DeliveryID"] = DBNull.Value;
                        dt.Rows.Add(drRow);
                    }
                    ViewState["gvPDFs"] = dt;
                    break;
                case 2://Sales Orders...
                    var qry2 = (from sm in db.SorMaster
                                join dh in db.DocScanUploadHistory on sm.SalesOrder equals dh.DocID
                                where
                                  sm.SalesOrder.Trim().ToUpper().Contains(sSearch.ToUpper())
                                select new
                                {
                                    dh.ScanUploadID,
                                    dh.DocID,
                                    dh.DocName,
                                    dh.DocType,
                                    dh.Extension,
                                    dh.DateAdded
                                }).Take(250);

                    foreach (var a in qry2)
                    {
                        sCompanyName = SharedFunctions.GetCompanyInfoFromSalesOrder(a.DocID).Rows[0]["Name"].ToString().Trim();
                        sSalesOrder = SharedFunctions.GetCompanyInfoFromSalesOrder(a.DocID).Rows[0]["SalesOrder"].ToString().Trim();
                        sPurchaseOrder = SharedFunctions.GetCompanyInfoFromSalesOrder(a.DocID).Rows[0]["CustomerPoNumber"].ToString().Trim();
                        sDeliveryDate = "";
                        sDateCreated = a.DateAdded.ToShortDateString();
                        sExtension = a.Extension;
                        sFileName = a.DocName + a.Extension;
                        sDocType = a.DocType;

                        drRow = dt.NewRow();
                        drRow["FileName"] = sFileName;
                        drRow["Company"] = sCompanyName;
                        drRow["SalesOrder"] = sSalesOrder;
                        drRow["CustomerPoNumber"] = sPurchaseOrder;
                        drRow["DateDelivered"] = sDeliveryDate;
                        drRow["DateCreated"] = sDateCreated;
                        drRow["FullPath"] = sDocPath + sFileName;
                        drRow["Extension"] = sExtension;
                        drRow["DocType"] = sDocType;
                        drRow["DeliveryID"] = DBNull.Value;
                        dt.Rows.Add(drRow);
                    }
                    ViewState["gvPDFs"] = dt;
                    break;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            dt.Dispose();
        }

        return dt;
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
            LoadDocsNames(0);//Default is Delivery Receipt...
            txtStartDate.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            txtEndDate.Text = DateTime.Now.ToShortDateString();            
            if (Request.QueryString["id"] != null)
            {
                txtSearch.Text = Request.QueryString["id"].ToString();
                switch (Request.QueryString["type"])
                {
                    case "SO":
                        rblDocType.SelectedIndex = 2;
                        break;
                    case "PO":
                        rblDocType.SelectedIndex = 1;
                        break;
                    case "DR":
                        rblDocType.SelectedIndex = 0;
                        break;
                }           
                 
                DataTable dt = new DataTable();       
                dt = SearchIDsMainDataTableFromSalesOrderTracker(txtSearch.Text.Trim());
                BindPDFData(dt);
                dt.Dispose();
            }
    
        }
 
    }
    protected void gvPDFs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

 
           
            Label lblExtension = (Label)e.Row.FindControl("lblExtension");
            System.Web.UI.WebControls.Image imgExtension = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgExtension");
            Label lblDateDelivered = (Label)e.Row.FindControl("lblDateDelivered");
            if (lblDateDelivered.Text != "")
            {
                lblDateDelivered.Text = Convert.ToDateTime(lblDateDelivered.Text).ToShortDateString();
            }
            else
            {
                lblDateDelivered.Text = "Not Assigned";
            }
            if (lblExtension != null)
            {
                if (lblExtension.Text != "")
                {
                    switch (lblExtension.Text.ToUpper())
                    {
                        case ".JPG":
                            imgExtension.ImageUrl = "Images\\Icons\\img.gif";
                            break;
                        case ".GIF":
                            imgExtension.ImageUrl = "Images\\Icons\\img.gif";
                            break;
                        case ".PNG":
                            imgExtension.ImageUrl = "Images\\Icons\\img.gif";
                            break;
                        case ".PDF":
                            imgExtension.ImageUrl = "Images\\Icons\\pdf.gif";
                            break;
                        case ".XLS":
                            imgExtension.ImageUrl = "Images\\Icons\\Excel.gif";
                            break;
                        case ".XLSX":
                            imgExtension.ImageUrl = "Images\\Icons\\Excel.gif";
                            break;
                        case ".DOC":
                            imgExtension.ImageUrl = "Images\\Icons\\word.gif";
                            break;
                        case ".DOCX":
                            imgExtension.ImageUrl = "Images\\Icons\\word.gif";
                            break;
                        case ".ZIP":
                            imgExtension.ImageUrl = "Images\\Icons\\Zip.gif";
                            break;
                        case ".MDB":
                            imgExtension.ImageUrl = "Images\\Icons\\ms_access.gif";
                            break;
                        case ".ACCDB":
                            imgExtension.ImageUrl = "Images\\Icons\\ms_access.gif";
                            break;
                        default:
                            break;
                    }
                }
            }


        }
    }
    protected void gvPDFs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //Rebind data...
        DataTable dt = new DataTable();

        string sStartDate = "";
        string sEndDate = "";
        sStartDate = txtStartDate.Text.Trim();
        sEndDate = txtEndDate.Text.Trim();
        string sID = lblID.Text;
        dt = SearchIDsMainDataTable(txtSearch.Text.Trim(), sStartDate, sEndDate);
        BindPDFData(dt);
        gvPDFs.DataSource = dt;
        gvPDFs.DataBind();


        dt.Dispose();
    }
    protected void gvPDFs_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblUploadErr.Text = "";
        lblMessageUpload.Text = "";

        System.IO.FileInfo file = null;
        string Outgoingfile = "";
        string sContentType = "";
        string sFullPath = "";
        string sAbsolutePath = "";
        DataTable dt = new DataTable();
        GridViewRow selectedRow;
        int index = 0;
        string sStartDate = "";
        string sEndDate = "";
        sStartDate = txtStartDate.Text.Trim();
        sEndDate = txtEndDate.Text.Trim();


        switch (e.CommandName)
        {
            case "Select":
                index = Convert.ToInt32(e.CommandArgument);
                selectedRow = gvPDFs.Rows[index];
                LinkButton lbnFileName = (LinkButton)selectedRow.FindControl("lbnFileName");
                Label lblExtension = (Label)selectedRow.FindControl("lblExtension");

                if (Server.MachineName.ToUpper() == "POWERHOUSE" || Server.MachineName.ToUpper() == "POWERHOUSEJR" || Server.MachineName.ToUpper() == "RSPSERVER")
                {
                    sAbsolutePath = MapPath("..\\Felbro_B\\") + sDocPath;
                }
                else//Hosting Company...
                {
                    sAbsolutePath = "d:\\inetpub\\wwwroot\\Felbro_B\\" + sDocPath;
                }

                sFullPath = sAbsolutePath + lbnFileName.Text;

                file = new System.IO.FileInfo(sFullPath);
                Outgoingfile = lbnFileName.Text;
                switch (lblExtension.Text.ToUpper())
                {
                    case ".TXT":
                        sContentType = "text/plain";
                        break;
                    case ".JPG":
                        sContentType = "image/JPEG";
                        break;
                    case ".GIF":
                        sContentType = "image/GIF";
                        break;
                    case ".PNG":
                        sContentType = "image/PNG";
                        break;
                    case ".PDF":
                        sContentType = "application/pdf";
                        break;
                    case ".XLS":
                        sContentType = "application/vnd.ms-excel";
                        break;
                    case ".XLSX":
                        sContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case ".DOC":
                        sContentType = "application/ms-word";
                        break;
                    case ".DOCX":
                        sContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case ".ZIP":
                        sContentType = "application/zip";
                        break;
                    case ".MDB":
                        sContentType = "application/msaccess";
                        break;
                    case ".ACCDB":
                        sContentType = "application/octet-stream";
                        break;
                    default:
                        sContentType = "application/octet-stream";
                        break;
                }
                try
                {
                    if (file.Exists)
                    {
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.Name + "\"");
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.ContentType = sContentType;
                        Response.Flush();
                        Response.TransmitFile(file.FullName);

                        break;
                    }
                    else
                    {
                        Response.Write("This file does not exist.");
                    }
                }
                catch (AccessViolationException iex)
                {
                    Debug.WriteLine(iex.ToString());
                    lblMessageUpload.Text = iex.Message;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    Response.End();//Remember to Add the Grid Postback triggers to avoid this not working...
                }


                break;
            case "Delete":
                index = Convert.ToInt32(e.CommandArgument);
                selectedRow = gvPDFs.Rows[index];
                string sFileName = ((LinkButton)selectedRow.FindControl("lbnFileName")).Text;
                string sVirtualPath = ((Label)selectedRow.FindControl("lblFullPath")).Text;
                sFullPath = "";
                sAbsolutePath = "";
                if (Server.MachineName.ToUpper() == "POWERHOUSE" || Server.MachineName.ToUpper() == "POWERHOUSEJR" || Server.MachineName.ToUpper() == "RSPSERVER")
                {
                    sAbsolutePath = MapPath("..\\Felbro_B\\") + sDocPath;
                }
                else//Hosting Company...
                {
                    sAbsolutePath = "d:\\inetpub\\wwwroot\\Felbro_B\\" + sDocPath;
                }

                sFullPath = sAbsolutePath + sFileName;

                try
                {
                    if (File.Exists(sFullPath))
                    {
                        File.Delete(sFullPath);
                    }
                    if (!File.Exists(sFullPath))
                    {
                        //Remove from dataTable...
                        dt.Rows.RemoveAt(index);
                    }

                    string sNewName = Path.GetFileNameWithoutExtension(sFileName);

                    int iHyphenPosition = 0;

                    string sFirstPartOfFileName = "";
                    iHyphenPosition = sNewName.IndexOf("_");
                    sFirstPartOfFileName = sNewName.Substring(0, iHyphenPosition);
                    FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
                    DocScanUploadHistory sd = db.DocScanUploadHistory.Single(p => p.DocID == sFirstPartOfFileName && p.DocName == sNewName);
                    db.DocScanUploadHistory.DeleteOnSubmit(sd);
                    db.SubmitChanges();

                    lblMessageUpload.Text = "**Delete Complete!";
                    lblMessageUpload.ForeColor = Color.Green;
                }
                catch (AccessViolationException iex)
                {
                    Debug.WriteLine(iex.ToString());
                    lblMessageUpload.Text = iex.Message;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    lblMessageUpload.Text = ex.Message;
                }


                ViewState["dtPDFs"] = dt;//DataTable with row removed...

                break;
            case "Company":
                index = Convert.ToInt32(e.CommandArgument);
                selectedRow = gvPDFs.Rows[index];
                //The DocType RadioButtonList does not change index here...
                LinkButton lbnCompany = (LinkButton)selectedRow.FindControl("lbnCompany");
                dt = new DataTable();
                dt = SearchIDsMainDataTableLinkClick(lbnCompany.Text.Trim(), sStartDate, sEndDate,"CO");
                BindPDFData(dt);
                dt.Dispose();
                txtSearch.Text = lbnCompany.Text.Trim();
                break;
            case "SalesOrder":
                index = Convert.ToInt32(e.CommandArgument);
                selectedRow = gvPDFs.Rows[index];
                //The DocType RadioButtonList does not change index here...
                LinkButton lbnSalesOrder = (LinkButton)selectedRow.FindControl("lbnSalesOrder");
                dt = new DataTable();
                dt = SearchIDsMainDataTableLinkClick(lbnSalesOrder.Text.Trim(), sStartDate, sEndDate,"SO");
                BindPDFData(dt);
                dt.Dispose();
                txtSearch.Text = lbnSalesOrder.Text.Trim();
                break;
            case "PurchaseOrder":
                index = Convert.ToInt32(e.CommandArgument);
                selectedRow = gvPDFs.Rows[index];
                //The DocType RadioButtonList does not change index here...
                LinkButton lbnPurchaseOrder = (LinkButton)selectedRow.FindControl("lbnPurchaseOrder");
                dt = new DataTable();
                dt = SearchIDsMainDataTableLinkClick(lbnPurchaseOrder.Text.Trim(), sStartDate, sEndDate,"PO");
                BindPDFData(dt);
                dt.Dispose();
                txtSearch.Text = lbnPurchaseOrder.Text.Trim();
                break;
            case "Delivery":
                index = Convert.ToInt32(e.CommandArgument);
                selectedRow = gvPDFs.Rows[index];
                //The DocType RadioButtonList does not change index here...
                LinkButton lbnDeliveryID = (LinkButton)selectedRow.FindControl("lbnDeliveryID");
                dt = new DataTable();
                dt = SearchIDsMainDataTableLinkClick(lbnDeliveryID.Text.Trim(), sStartDate, sEndDate,"DR");
                BindPDFData(dt);
                dt.Dispose();
                txtSearch.Text = lbnDeliveryID.Text.Trim();
                break;

        }
        dt.Dispose();
    }
    protected void gvPDFs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPDFs.PageIndex = e.NewPageIndex;
        //Rebind data...
        DataTable dt = new DataTable();
        if (ViewState["gvPDFs"] != null)
        {
            dt = (DataTable)ViewState["gvPDFs"];
            gvPDFs.DataSource = dt;
            gvPDFs.DataBind();
        }


        dt.Dispose();
    }
    protected void gvPDFs_Sorting(object sender, GridViewSortEventArgs e)
    {
        string m_SortDirection = "";
        DataTable dtSortTable = new DataTable();
        dtSortTable = (DataTable)ViewState["gvPDFs"];
        DataTable m_DataTable = dtSortTable;
        if (m_DataTable != null)
        {
            int m_PageIndex = gvPDFs.PageIndex;
            m_SortDirection = GetSortDirection();
            DataView m_DataView = new DataView(m_DataTable);
            m_DataView.Sort = e.SortExpression + " " + m_SortDirection;
            gvPDFs.DataSource = m_DataView;
            gvPDFs.DataBind();
            gvPDFs.PageIndex = m_PageIndex;
            ViewState["gvPDFs"] = m_DataTable;
        }
        dtSortTable.Dispose();
    }
    //Uploads
    protected void gvPDFsUploads_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblExtension = (Label)e.Row.FindControl("lblExtension");
            System.Web.UI.WebControls.Image imgExtension = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgExtension");
            if (lblExtension != null)
            {
                if (lblExtension.Text != "")
                {
                    switch (lblExtension.Text.ToUpper())
                    {
                        case ".JPG":
                            imgExtension.ImageUrl = "Images\\Icons\\img.gif";
                            break;
                        case ".GIF":
                            imgExtension.ImageUrl = "Images\\Icons\\img.gif";
                            break;
                        case ".PNG":
                            imgExtension.ImageUrl = "Images\\Icons\\img.gif";
                            break;
                        case ".PDF":
                            imgExtension.ImageUrl = "Images\\Icons\\pdf.gif";
                            break;
                        case ".XLS":
                            imgExtension.ImageUrl = "Images\\Icons\\Excel.gif";
                            break;
                        case ".XLSX":
                            imgExtension.ImageUrl = "Images\\Icons\\Excel.gif";
                            break;
                        case ".DOC":
                            imgExtension.ImageUrl = "Images\\Icons\\word.gif";
                            break;
                        case ".DOCX":
                            imgExtension.ImageUrl = "Images\\Icons\\word.gif";
                            break;
                        case ".ZIP":
                            imgExtension.ImageUrl = "Images\\Icons\\Zip.gif";
                            break;
                        case ".MDB":
                            imgExtension.ImageUrl = "Images\\Icons\\ms_access.gif";
                            break;
                        case ".ACCDB":
                            imgExtension.ImageUrl = "Images\\Icons\\ms_access.gif";
                            break;
                        default:
                            break;
                    }
                }
            }


        }
    }
    protected void gvPDFsUploads_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        //Rebind data...
        DataTable dt = new DataTable();

        string sID = lblID.Text;
        BindPDFDataUploads(sID);
        gvPDFs.DataSource = dt;
        gvPDFs.DataBind();


        dt.Dispose();
    }
    protected void gvPDFsUploads_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblUploadErr.Text = "";
        lblMessageUpload.Text = "";
        System.IO.FileInfo file = null;
        string Outgoingfile = "";
        string sContentType = "";
        string sFullPath = "";
        string sAbsolutePath = "";
        DataTable dt = new DataTable();
        GridViewRow selectedRow;
        int index = 0;
        //    string sMsg = "";

        if (ViewState["gvPDFsUploads"] != null)
        {
            dt = (DataTable)ViewState["gvPDFsUploads"];
            gvPDFsUploads.DataSource = dt;
            gvPDFsUploads.DataBind();
        }

        switch (e.CommandName)
        {
            case "Select":
                index = Convert.ToInt32(e.CommandArgument);
                selectedRow = gvPDFsUploads.Rows[index];
                LinkButton lbnFileName = (LinkButton)selectedRow.FindControl("lbnFileName");
                Label lblExtension = (Label)selectedRow.FindControl("lblExtension");

                if (Server.MachineName.ToUpper() == "POWERHOUSE" || Server.MachineName.ToUpper() == "POWERHOUSEJR" || Server.MachineName.ToUpper() == "RSPSERVER")
                {
                    sAbsolutePath = MapPath("..\\Felbro_B\\") + sDocPath;
                }
                else//Hosting Company...
                {
                    sAbsolutePath = "d:\\inetpub\\wwwroot\\Felbro_B\\" + sDocPath;
                }

                sFullPath = sAbsolutePath + lbnFileName.Text;

                file = new System.IO.FileInfo(sFullPath);
                Outgoingfile = lbnFileName.Text;
                switch (lblExtension.Text.ToUpper())
                {
                    case ".TXT":
                        sContentType = "text/plain";
                        break;
                    case ".JPG":
                        sContentType = "image/JPEG";
                        break;
                    case ".GIF":
                        sContentType = "image/GIF";
                        break;
                    case ".PNG":
                        sContentType = "image/PNG";
                        break;
                    case ".PDF":
                        sContentType = "application/pdf";
                        break;
                    case ".XLS":
                        sContentType = "application/vnd.ms-excel";
                        break;
                    case ".XLSX":
                        sContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        break;
                    case ".DOC":
                        sContentType = "application/ms-word";
                        break;
                    case ".DOCX":
                        sContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                        break;
                    case ".ZIP":
                        sContentType = "application/zip";
                        break;
                    case ".MDB":
                        sContentType = "application/msaccess";
                        break;
                    case ".ACCDB":
                        sContentType = "application/octet-stream";
                        break;
                    default:
                        sContentType = "application/octet-stream";
                        break;
                }
                try
                {
                    if (file.Exists)
                    {
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + file.Name + "\"");
                        Response.AddHeader("Content-Length", file.Length.ToString());
                        Response.ContentType = sContentType;
                        Response.Flush();
                        Response.TransmitFile(file.FullName);

                        break;
                    }
                    else
                    {
                        Response.Write("This file does not exist.");
                    }
                }
                catch (AccessViolationException iex)
                {
                    Debug.WriteLine(iex.ToString());
                    lblMessageUpload.Text = iex.Message;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    Response.End();//Remember to Add the Grid Postback triggers to avoid this not working...
                }


                break;
            case "Delete":
                index = Convert.ToInt32(e.CommandArgument);
                selectedRow = gvPDFsUploads.Rows[index];
                string sFileName = ((LinkButton)selectedRow.FindControl("lbnFileName")).Text;
                string sVirtualPath = ((Label)selectedRow.FindControl("lblFullPath")).Text;
                if (Server.MachineName.ToUpper() == "POWERHOUSE" || Server.MachineName.ToUpper() == "POWERHOUSEJR" || Server.MachineName.ToUpper() == "RSPSERVER")
                {
                    sAbsolutePath = MapPath("..\\Felbro_B\\") + sDocPath;
                }
                else//Hosting Company...
                {
                    sAbsolutePath = "d:\\inetpub\\wwwroot\\Felbro_B\\" + sDocPath;
                }

                sFullPath = sAbsolutePath + sFileName;

                try
                {
                    if (File.Exists(sFullPath))
                    {
                        File.Delete(sFullPath);
                    }
                    if (!File.Exists(sFullPath))
                    {
                        //Remove from dataTable...
                        dt.Rows.RemoveAt(index);
                    }

                    string sNewName = Path.GetFileNameWithoutExtension(sFileName);

                    int iHyphenPosition = 0;

                    string sFirstPartOfFileName = "";
                    iHyphenPosition = sNewName.IndexOf("_"); 
                    sFirstPartOfFileName = sNewName.Substring(0, iHyphenPosition);
                    FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
                    DocScanUploadHistory sd = db.DocScanUploadHistory.Single(p => p.DocID == sFirstPartOfFileName && p.DocName == sNewName);
                    db.DocScanUploadHistory.DeleteOnSubmit(sd);
                    db.SubmitChanges();

                    lblMessageUpload.Text = "**Delete Complete!";
                    lblMessageUpload.ForeColor = Color.Green;                    

                }
                catch (AccessViolationException iex)
                {
                    Debug.WriteLine(iex.ToString());
                    lblMessageUpload.Text = iex.Message;
                    lblMessageUpload.ForeColor = Color.Red;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    lblMessageUpload.Text = ex.Message;
                    lblMessageUpload.ForeColor = Color.Red;
                }


                ViewState["dtPDFsUploads"] = dt;//DataTable with row removed...

                break;

        }
        dt.Dispose();
    }
    protected void gvPDFsUploads_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPDFs.PageIndex = e.NewPageIndex;
        //Rebind data...
        DataTable dt = new DataTable();
        if (ViewState["gvPDFsUploads"] != null)
        {
            dt = (DataTable)ViewState["gvPDFsUploads"];
            gvPDFs.DataSource = dt;
            gvPDFs.DataBind();
        }


        dt.Dispose();
    }
    protected void ibnSearch_Click(object sender, EventArgs e)
    {
        lblMessageUpload.Text = "";
        DataTable dt = new DataTable();
        string sMsg = "";
        string sStartDate = "";
        string sEndDate = "";
        sStartDate = txtStartDate.Text.Trim();
        sEndDate = txtEndDate.Text.Trim();
        if (txtSearch.Text.Trim() == "")
        {
            sMsg += "**Search box can't be blank!!<br/>";
        }
        if (sStartDate != "")
        {
            if (SharedFunctions.IsDate(sStartDate) == false)
            {
                sMsg += "**Start Date is not a valid date!<br/>";
            }
        }

        if (sEndDate != "")
        {
            if (SharedFunctions.IsDate(sEndDate) == false)
            {
                sMsg += "**End Date is not a valid date!<br/>";
            }
        }

        if (sStartDate != "" && sEndDate != "")
        {
            if (SharedFunctions.IsDate(sStartDate) == true && SharedFunctions.IsDate(sEndDate) == true)
            {
                if (Convert.ToDateTime(sStartDate) > Convert.ToDateTime(sEndDate))
                {
                    sMsg += "**End Date can not come before Start Date!<br/>";
                }

            }
        }
        if (sStartDate == "" && sEndDate != "")
        {
            sMsg += "**If there is an End Date there must be a Start Date!<br/>";
        }
        if (sStartDate != "" && sEndDate == "")
        {
            sMsg += "**If there is an Start Date there must be a End Date!<br/>";
        }
        if (sMsg.Length > 0)
        {
            gvPDFs.DataSource = null;
            gvPDFs.DataBind();
            lblMessageUpload.Text = sMsg;
            lblMessageUpload.ForeColor = Color.Red;
            return;
        }
        dt = SearchIDsMainDataTable(txtSearch.Text.Trim(), sStartDate, sEndDate);
        BindPDFData(dt);
       // List<string> lIDs = SearchIDsMain(txtSearch.Text.Trim());
        //BindPDFData(lIDs);

    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        lblMessageUpload.Text = "";
        DataTable dt = new DataTable();
        string sMsg = "";
        string sStartDate = "";
        string sEndDate = "";
        sStartDate = txtStartDate.Text.Trim();
        sEndDate = txtEndDate.Text.Trim();
        if (sStartDate != "")
        {
            if (SharedFunctions.IsDate(sStartDate) == false)
            {
                sMsg += "**Start Date is not a valid date!<br/>";
            }
        }

        if (sEndDate != "")
        {
            if (SharedFunctions.IsDate(sEndDate) == false)
            {
                sMsg += "**End Date is not a valid date!<br/>";
            }
        }

        if (sStartDate != "" && sEndDate != "")
        {
            if (SharedFunctions.IsDate(sStartDate) == true && SharedFunctions.IsDate(sEndDate) == true)
            {
                if (Convert.ToDateTime(sStartDate) > Convert.ToDateTime(sEndDate))
                {
                    sMsg += "**End Date can not come before Start Date!<br/>";
                }

            }
        }
        if (sStartDate == "" && sEndDate != "")
        {
            sMsg += "**If there is an End Date there must be a Start Date!<br/>";
        }
        if (sStartDate != "" && sEndDate == "")
        {
            sMsg += "**If there is an Start Date there must be a End Date!<br/>";
        }
        if (sMsg.Length > 0)
        {
            lblMessageUpload.Text = sMsg;
            lblMessageUpload.ForeColor = Color.Red;
            return;
        }
        dt = SearchIDsMainDataTable(txtSearch.Text.Trim(), sStartDate, sEndDate);
        BindPDFData(dt);
       // List<string> lIDs = SearchIDsMain(txtSearch.Text.Trim());
        //BindPDFData(lIDs);
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        lblMessageUpload.Text = "";
        lblUploadErr.Text = "";
        string sID = "0";
        if(lblID.Text=="")
        {
            lblMessageUpload.Text = "**Nothing Selected!!";
            lblMessageUpload.ForeColor = Color.Red;
            return;
        }
        sID = lblID.Text.Trim().Replace(" ","");//Clean Up PO...
        UploadDocuments(sID);
    }
    protected void btnSearchIDs_Click(object sender, EventArgs e)
    {
        SearchIDs(txtSearchIDs.Text.Trim());
    }
    protected void txtSearchIDs_TextChanged(object sender, EventArgs e)
    {
        SearchIDs(txtSearchIDs.Text.Trim());
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

                    if (rblDocTypeIDSearch.SelectedIndex == 0 || rblDocTypeIDSearch.SelectedIndex == 1)//Delivery/Pickup Receipt
                    {
                        DateTime? dtScheduledDeliveryDate = Convert.ToDateTime(GetScheduledDeliveryDate(Convert.ToInt32(sID)));
                        if (dtScheduledDeliveryDate != null)
                        {
                            lblScheduledDeliveryDate.Text = Convert.ToDateTime(dtScheduledDeliveryDate).ToShortDateString();

                            HttpContext.Current.Session["DateDelivered"] = dtScheduledDeliveryDate;//Load scheduled date into the session variable...
                        }
                        else
                        {
                            lblScheduledDeliveryDate.Text = "";
                        }
                    }
                    else
                    {
                        lblScheduledDeliveryDate.Text = "N/A";
                    }

                    BindPDFDataUploads(sID);
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
    protected void rblDocTypeIDSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadDocsNames(rblDocTypeIDSearch.SelectedIndex);
    }

    #endregion

    #region WebMethods
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListIDs(string prefixText, int count, string contextKey)
    {
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {
            // Your LINQ to SQL query goes here 
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);


            var query = ((
                      from ar in db.ArCustomer
                        select new
                        {
                            ID = ar.Name.Trim()
                        }
                    ).Union
                    (
                      from ar in db.SorMaster
                      select new
                      {
                          ID = ar.CustomerPoNumber.Trim().Replace(" ","")
                      }
                    ).Union
                    (
                        from sm in db.SorMaster
                        select new
                        {
                            ID = sm.SalesOrder.Trim()
                        }
                    )

                    .OrderBy(p => p.ID));

            int iCount = query.Count();

            List<string> stringArr = new List<string>();

            stringArr = query.Select(i => i.ID).ToList();

            string[] list = stringArr.ToArray();

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



    


