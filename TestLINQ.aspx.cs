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


public partial class TestLINQ : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //DataTable dt = new DataTable();
          
        //// Your LINQ to SQL query goes here 
        //FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        //var qry = from c in db.SorMaster
        //              where c.CustomerPoNumber.Trim() != ""
        //              && c.CustomerPoNumber != null
        //              && c.OrderDate > Convert.ToDateTime("01/01/2013")
        //              &&  db.Udf_IsNumeric(c.CustomerPoNumber) == 1
        //              select new { Item =c.CustomerPoNumber + " - " + c.SalesOrder };

 


        //dt = SharedFunctions.ToDataTable(db, qry);
        //GridView1.DataSource =  dt;
        //GridView1.DataBind();
        //dt.Dispose();

    }

    #region WebMethods
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListPOs(string prefixText, int count, string contextKey)
    {//...


        // Your LINQ to SQL query goes here 
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        string[] list = db.SorMaster.Where(w => w.CustomerPoNumber != null  && w.CustomerPoNumber.Trim() != ""  && w.OrderDate > Convert.ToDateTime("01/01/2013") && db.Udf_IsNumeric(w.CustomerPoNumber) == 1)//Check for Numeric...
            .OrderBy(w => w.CustomerPoNumber).Select(w => (w.CustomerPoNumber + " - " + w.SalesOrder))
            .Take(100).ToArray();
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
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListUserName(string prefixText, int count, string contextKey)
    {//...
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {

            // Your LINQ to SQL query goes here 
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            string[] list = db.WipUsers.Where(w => w.FirstName != null && w.LastName != null).OrderBy(w => w.LastName).Select(w => (w.FirstName + " " + (w.MiddleName ?? "") + " " + w.LastName).Replace("  ", " ")).Distinct().ToArray();
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
    protected void txtUpload_TextChanged(object sender, EventArgs e)
    {

    }
}