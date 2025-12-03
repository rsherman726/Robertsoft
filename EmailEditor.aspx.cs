using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Diagnostics;
using System.IO;
using System.Text;
using ASP;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using System.Data.Linq;
using System.ComponentModel;
using System.Data.OleDb;
using System.Drawing;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections.Generic;

public partial class EmailEditor : System.Web.UI.Page
{
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
        gvButtons.DataSource = LoadButtonData();
        gvButtons.DataBind();
        txtInstructionForHTMLEditor.Text = "**Important: To Add a document, type the name of the document in the box next to the ADD button and press the Enter key (REQUIRED to initialized the HTML Editor). Next, start composing your document. When finished, Click the ADD button and confirm. Alternatively, you can load a previous document by Clicking the Load button next to the desired document. Edit it, and then Click the ADD button and confirm." + Environment.NewLine + Environment.NewLine;
        txtInstructionForHTMLEditor.Text += "For Updating documents First, load the desired document by Clicking the Load button. Next, start editing it in the HTML Editor below. When Finished, Click the Edit button for the desired document you wish to save the changes to. Lastly, Click the Update button and confirm.";
        }
        txtInstructionForHTMLEditor.Text = "**Important: To Add a document, type the name of the document in the box next to the ADD button and press the Enter key (REQUIRED to initialized the HTML Editor). Next, start composing your document. When finished, Click the ADD button and confirm. Alternatively, you can load a previous document by Clicking the Load button next to the desired document. Edit it, and then Click the ADD button and confirm." + Environment.NewLine + Environment.NewLine;
        txtInstructionForHTMLEditor.Text += "For Updating documents First, load the desired document by Clicking the Load button. Next, start editing it in the HTML Editor below. When Finished, Click the Edit button for the desired document you wish to save the changes to. Lastly, Click the Update button and confirm.";
    }
    protected void gvButtons_RowDataBound(object sender, GridViewRowEventArgs e)
    {
      
        if (e.Row.RowType == DataControlRowType.DataRow)//the DataRow row...
        {
          

            if (gvButtons.EditIndex != -1)//In Edit Mode...
            {
              
            }
            if (gvButtons.EditIndex == -1)//Not in edit mode...
            {

                Button btnDeleteRow = (Button)e.Row.FindControl("btnDeleteRow");    
                Label lDocName = (Label)e.Row.FindControl("lblDocName");

                if (lDocName.Text == "Demo")
                {
                    btnDeleteRow.Enabled = false;
                }
            }

        }
        if (e.Row.RowType == DataControlRowType.Footer)//the footer row...
        {
           
            
        }
    }
    protected void gvButtons_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        string sID = "";
        int iID = 0;
        DataTable dtContent = new DataTable();
        int index = 0;
        string sHTML = "";
        string sFileName = "";
        string sDocMode = "";
        string sDocType = "E";
        string sBody = "";

     //   dtContent = GetRespData();
        // Get the last name of the selected author from the appropriate
        // cell in the GridView control.
        GridViewRow SelectedRow;

        switch (e.CommandName)
        {
            case "Load":

                sBody = "";
                index = Convert.ToInt32(e.CommandArgument);
                SelectedRow = gvButtons.Rows[index];

                Label lDocNameLoad = (Label)SelectedRow.FindControl("lblDocName");              

                sFileName = lDocNameLoad.Text;
                sDocMode = "EDIT";

                sBody = WordDocuments.GetWordDoc(dtContent, sDocMode, sDocType, sFileName,false);

                if (String.IsNullOrEmpty(sBody))
                {
                    sBody = "";
                }
                sBody = sBody.Replace("<HTML><BODY>", "");
                sBody = sBody.Replace("</HTML></BODY>", "");
                RSEditor.Text  =sBody;

                dtContent.Dispose();
                break;
            
            case "Add":
                try
                {
                    ////ConfigureHTMLEditor(null);
                    string sMSG = "";
                    //Get the values stored in the text boxes
                    sID = gvButtons.DataKeys[0].Value.ToString();  //iCourseID is stored as DataKeyNames
                    string sDocName = ((TextBox)gvButtons.FooterRow.FindControl("txtAddDocName")).Text.Trim();

                    sHTML = RSEditor.Text.Trim();

                    //sHTML = sHTML.Replace("'", "''");

                    if (sDocName == "")
                    {
                        sMSG += "*Document Name text box can not be empty, try again.<br/>";
                    }
                    else
                    {
                        if (sDocName.Length > 50)
                        {
                            sMSG += "*Document Name text box is limited to 50 characters, try again.<br/>";
                        }
                    }
                    if (sHTML == "")
                    {
                        sMSG += "**HTML Editor can not be empty, try again.<br/>";
                    }


                    if (DocExists(sDocName))
                    {
                        sMSG += "**Document already exists, try again.<br/>";
                    }

                    if (sMSG.Length > 0)
                    {
                        lblErr.Text = sMSG;
                        return;
                    }

                    WordDocs worddocs = new WordDocs();

                    worddocs.LoadDocNavUrl = "~/Images/Load.png";
                    worddocs.CreateDocNavUrl = "~/Images/Gen.gif";
                    worddocs.DocName = sDocName;
                    worddocs.HTML = sHTML;
                    worddocs.DateAdded = DateTime.Now;
                    worddocs.Msrepl_tran_version = Guid.NewGuid();

                    db.WordDocs.InsertOnSubmit(worddocs);

                    db.SubmitChanges();


                    gvButtons.DataSource = LoadButtonData();
                    gvButtons.DataBind();
                    lblErr.ForeColor = System.Drawing.Color.Green;
                    lblErr.Text = "**Document added successfully!";

                    RSEditor.Text  ="";

                }
                catch (Exception ex)
                {
                    lblErr.ForeColor = System.Drawing.Color.Red;
                    lblErr.Text = "**Add Failed!";
                    Debug.WriteLine(ex.ToString());
                }
                break;
            case "Edit":


                break;
            case "Update":
                try
                {
                    string sMSG = "";

                    ///////////////////////////////////////////////////

                    index = Convert.ToInt32(e.CommandArgument);

                    // Get the last name of the selected author from the appropriate
                    // cell in the GridView control.
                    SelectedRow = gvButtons.Rows[index];

                    sID = ((Label)SelectedRow.FindControl("lblIDEdit")).Text;

                    iID = Convert.ToInt32(sID);

                    ////ConfigureHTMLEditor(index);

                    sHTML = RSEditor.Text.Trim();

                    //sHTML = sHTML.Replace("'", "''");

                    if (sHTML == "")
                    {
                        sMSG += "**HTML Editor can not be empty, try again.<br/>";
                    }


                    if (sMSG.Length > 0)
                    {
                        lblErr.Text = sMSG;
                        return;
                    }

                    WordDocs worddocs = db.WordDocs.Single(a => a.ID == iID);

                    worddocs.HTML = sHTML;

                    //submit changes to take effect
                    db.SubmitChanges();

                    gvButtons.DataSource = LoadButtonData();
                    gvButtons.DataBind();
                    lblErr.ForeColor = System.Drawing.Color.Green;
                    lblErr.Text = "**Document updated successfully!";
                }
                catch (Exception ex)
                {
                    lblErr.ForeColor = System.Drawing.Color.Red;
                    lblErr.Text = "**Update Failed!";
                    Debug.WriteLine(ex.ToString());
                }
                break;
            case "Cancel":
                break;
            case "Delete":

                index = Convert.ToInt32(e.CommandArgument);
                SelectedRow = gvButtons.Rows[index];

                sID = ((Label)SelectedRow.FindControl("lblID")).Text;


                iID = Convert.ToInt32(sID);

                WordDocs worddoc = db.WordDocs.Single(a => a.ID == iID);
                try
                {
                    db.WordDocs.DeleteOnSubmit(worddoc);
                    db.SubmitChanges();

                    gvButtons.DataSource = LoadButtonData();
                    gvButtons.DataBind();
                    lblErr.ForeColor = System.Drawing.Color.Green;
                    lblErr.Text = "**Document deleted successfully!";
                }
                catch (Exception ex)
                {
                    lblErr.ForeColor = System.Drawing.Color.Red;
                    lblErr.Text = "**Delete Failed!";
                    Debug.WriteLine(ex.ToString());
                }

                break;
            default:
                break;
        }
    }
    protected void gvButtons_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        DataTable dt = new DataTable();
        gvButtons.PageIndex = e.NewPageIndex;
        dt = LoadButtonData();
        gvButtons.DataSource = dt;
        gvButtons.DataBind();
    }
    protected void gvButtons_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        DataTable dt = new DataTable();
        gvButtons.EditIndex = -1;
        dt = LoadButtonData();
        gvButtons.DataSource = dt;
        gvButtons.DataBind();
    }
    protected void gvButtons_RowEditing(object sender, GridViewEditEventArgs e)
    {
        DataTable dt = new DataTable();
        gvButtons.EditIndex = e.NewEditIndex;
        dt = LoadButtonData();
        gvButtons.DataSource = dt;
        gvButtons.DataBind();
    }
    protected void gvButtons_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dt = new DataTable();
        dt = LoadButtonData();
        gvButtons.DataSource = dt;
        gvButtons.DataBind();
    }
    protected void gvButtons_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        DataTable dt = new DataTable();
        gvButtons.EditIndex = -1;
        dt = LoadButtonData();
        gvButtons.DataSource = dt;
        gvButtons.DataBind();
    }
    protected void txtAddDocName_TextChanged(object sender, EventArgs e)
    {
        //ConfigureHTMLEditor(null);
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        RSEditor.Text  ="";
    }
    protected void ContentChanged(object sender, EventArgs e)
    {
        ContentChangedLabel.Text = "<span style='color:red'>Content changed</span>";
    }
    public  bool DocExists(string sName)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (
                    from worddocs in db.WordDocs.OfType<WordDocs>()
                    where
                      worddocs.DocName == sName
                    select new
                    {
                        worddocs.DocName
                    }).ToArray();
        int iCount = query.Count();
        if (iCount > 0)
        {

            return true;
        }
        else
        {
            return false;
        }
    }
    private DataTable LoadButtonData()
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        DataTable dt = new DataTable();
        var query = (from worddocs in db.WordDocs
                     orderby worddocs.ID
                     select new
                     {
                         worddocs.ID,
                         worddocs.DocName,
                         worddocs.LoadDocNavUrl,
                         worddocs.CreateDocNavUrl
                     });

        dt = SharedFunctions.ToDataTable(db, query);

        return dt;
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Emailer.aspx");
    }
    protected void btnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}