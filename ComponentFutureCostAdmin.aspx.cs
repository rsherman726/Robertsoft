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

public partial class ComponentFutureCostAdmin : System.Web.UI.Page
{
    #region Subs
    private void AddIngredientCost()
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            lblError.Text = "";
            decimal dcNewIngredientCost = 0;
            bool bEstimateCostExists = false;

            if (txtStockCodeIngredient.Text.Trim() == "")
            {
                sMsg += "**StockCode Ingredient is Required!<br/>";
            }
            else
            {
                bEstimateCostExists = EstimateIngredientCostExists(txtStockCodeIngredient.Text);
                if (bEstimateCostExists)
                {
                    sMsg += "**StockCode Ingredient is Already in the Table!<br/>";
                }
                if (StockCodeExistsInInvMaster(txtStockCodeIngredient.Text.Trim()) == false)
                {
                    sMsg += "**StockCode Ingredient does not exist in InvMaster table yet!<br/>";
                }
            }
            if (txtIngredientCost.Text.Trim() == "")
            {
                sMsg += "**Estimated Ingredient Cost is Required!<br/>";
            }
            if (!StockCodeIsAnIngredient(txtStockCodeIngredient.Text.Trim()))
            {
                sMsg += "**You entered a packaging stock code in Ingredient!<br/>";
            }
            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }
            if (txtIngredientCost.Text.Trim() != "" && txtStockCodeIngredient.Text.Trim() != "")
            {
                dcNewIngredientCost = Convert.ToDecimal(txtIngredientCost.Text.Trim());

                InvEstimatedIngredientCost ie = new InvEstimatedIngredientCost();
                ie.StockCode = txtStockCodeIngredient.Text;
                ie.EstimatedIngredientCost = dcNewIngredientCost;
                ie.DateAdded = DateTime.Now;
                db.InvEstimatedIngredientCost.InsertOnSubmit(ie);
                db.SubmitChanges();

                lblError.Text = "**Ingredient Added Successfully(" + txtStockCodeIngredient.Text + ")!"; ;
                lblError.ForeColor = Color.Green;

                txtStockCodeIngredient.Text = "";
                txtIngredientCost.Text = "";

                string sStockCode = txtSearch.Text.Trim();
                RunReport(sStockCode);
            }
        }
    }
    private void AddPackagingCost()
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            lblError.Text = "";
            decimal dcNewPackagingCost = 0;
            bool bEstimateCostExists = false;
            if (txtStockCodePackaging.Text.Trim() == "")
            {
                sMsg += "**StockCode Packaging is Required!<br/>";
            }
            else
            {
                bEstimateCostExists = EstimatePackagingCostExists(txtStockCodePackaging.Text);
                if (bEstimateCostExists)
                {
                    sMsg += "**StockCode Packaging is Already in the Table!<br/>";
                }
                if (StockCodeExistsInInvMaster(txtStockCodePackaging.Text.Trim()) == false)
                {
                    sMsg += "**StockCode Packaging does not exist in InvMaster table yet!<br/>";
                }
            }
            if (txtPackagingCost.Text.Trim() == "")
            {
                sMsg += "**Estimated Packaging Cost is Required!<br/>";
            }
            if (StockCodeIsAnIngredient(txtStockCodePackaging.Text.Trim()))
            {
                sMsg += "**You entered an ingredient stock code in packaging!<br/>";
            }
            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }

            if (txtPackagingCost.Text.Trim() != "" && txtStockCodePackaging.Text.Trim() != "")
            {
                dcNewPackagingCost = Convert.ToDecimal(txtPackagingCost.Text.Trim());

                InvEstimatedPackagingCost ie = new InvEstimatedPackagingCost();
                ie.StockCode = txtStockCodePackaging.Text;
                ie.EstimatedPackagingCost = dcNewPackagingCost;
                ie.DateAdded = DateTime.Now;
                db.InvEstimatedPackagingCost.InsertOnSubmit(ie);
                db.SubmitChanges();

                lblError.Text = "**Packaging Added Successfully(" + txtStockCodePackaging.Text + ")!";
                lblError.ForeColor = Color.Green;

                txtStockCodePackaging.Text = "";
                txtPackagingCost.Text = "";

                string sStockCode = txtSearch.Text.Trim();
                RunReport(sStockCode);

            }
        }
    }
    private void ExportToExcel(DataTable dt, string sFileName)
    {

        if (dt != null)
        {

            if (dt.Rows.Count == 0)
            {
                return;
            }
        }
        else
        {
            return;
        }

        ExcelHelper.ToExcel(dt, sFileName, Page.Response);

    }

    #endregion


    #region Functions

    private string GetPrice(string sStockCode)
    {
        FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        decimal dcPrice = 0;
        var query = (from p in db.InvPrice
                     where p.PriceCode == "A"
                     && p.StockCode == sStockCode
                     select new { p.SellingPrice });
        foreach (var a in query)
        {
            dcPrice = (decimal)a.SellingPrice;
        }
        if (dcPrice == 0)
        {
            var query1 = (from p in db.InvPrice
                          where p.PriceCode == "B"
                          && p.StockCode == sStockCode
                          select new { p.SellingPrice });
            foreach (var a in query1)
            {
                dcPrice = (decimal)a.SellingPrice;
            }
        }
        if (dcPrice == 0)
        {
            var query1 = (from p in db.InvPrice
                          where p.PriceCode == "C"
                          && p.StockCode == sStockCode
                          select new { p.SellingPrice });
            foreach (var a in query1)
            {
                dcPrice = (decimal)a.SellingPrice;
            }
        }
        if (dcPrice == 0)
        {
            var query1 = (from p in db.InvPrice
                          where p.PriceCode == "D"
                          && p.StockCode == sStockCode
                          select new { p.SellingPrice });
            foreach (var a in query1)
            {
                dcPrice = (decimal)a.SellingPrice;
            }
        }

        return dcPrice.ToString("#,0.00");
    }
    private string RunReport(string sStockCode)
    {
        string sMsg = "";


        DataSet ds = new DataSet();
        string sSQL = "";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return sMsg;
        }

        if (sStockCode != "")
        {
            sSQL = "EXEC spGetComponentFuturePackagingAndRecipeValues ";
            sSQL += "@StockCode=" + sStockCode;
        }
        else
        {
            sSQL = "EXEC spGetComponentFuturePackagingAndRecipeValues";
        }


        Debug.WriteLine(sSQL);

        ds = SharedFunctions.getDataSet(sSQL, conn, "ds");
        //DataTable dt = ds.Tables[0];
        //foreach (DataRow Row in dt.Rows)
        //{
        //    foreach (DataColumn c in dt.Columns)
        //    {
        //        Debug.WriteLine(c.ColumnName + ": " + Row[c.ColumnName].ToString());
        //    }
        //    Debug.WriteLine("----------------------------------------------------");
        //}
        if (ds.Tables[0].Rows.Count > 0)
        {
            using (DataTable dataTable = ds.Tables[0])
            {
                gvIngredients.DataSource = dataTable;
                gvIngredients.DataBind();
                base.ViewState["dtIngredients"] = dataTable;
                pnlDetailsIngredientsGrid.Visible = true;
                if (dataTable.Rows.Count > 1)
                {
                    pnlGridIngredients.Height = Unit.Pixel(500);
                    pnlGridIngredients.ScrollBars = ScrollBars.Vertical;
                }
                else
                {
                    pnlGridIngredients.Height = Unit.Pixel(100);
                    pnlGridIngredients.ScrollBars = ScrollBars.None;
                }
            }
        }
        else
        {
            pnlDetailsIngredientsGrid.Visible = false;
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            using (DataTable dataTable = ds.Tables[1])
            {
                gvPackaging.DataSource = dataTable;
                gvPackaging.DataBind();
                base.ViewState["dtPackaging"] = dataTable;
                pnlPackagingGrid.Visible = true;
                if (dataTable.Rows.Count > 1)
                {
                    pnlGridPackaging.Height = Unit.Pixel(500);
                    pnlGridPackaging.ScrollBars = ScrollBars.Vertical;
                }
                else
                {
                    pnlGridPackaging.Height = Unit.Pixel(100);
                    pnlGridPackaging.ScrollBars = ScrollBars.None;
                }
            }
        }
        else
        {
            pnlPackagingGrid.Visible = false;
        }


        try
        {

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            ds.Dispose();
        }
        return "";
    }
    private bool EstimateIngredientCostExists(string sStockCode)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from est in db.InvEstimatedIngredientCost
                         where est.StockCode == sStockCode
                         select est);
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
    private bool EstimatePackagingCostExists(string sStockCode)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from est in db.InvEstimatedPackagingCost
                         where est.StockCode == sStockCode
                         select est);
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
    private bool StockCodeExistsInInvMaster(string sStockCode)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from est in db.InvMaster
                         where est.StockCode == sStockCode
                         select est);
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
    private bool StockCodeIsAnIngredient(string sStockCode)
    {
        if (SharedFunctions.IsNumeric(sStockCode))
        {
            int iStockCode = Convert.ToInt32(sStockCode);
            if (iStockCode >= 600000 && iStockCode <= 649999)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else//A Label stockcode...
        {
            return false;
        }
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
            //string sStockCode = txtSearch.Text.Trim();
            //RunReport(sStockCode);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "isPostBack", "isPostBack();", true);
        }
    }
    protected void lbnSearch_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        string sMsg = "";
        string sStockCode = "";

        sStockCode = txtSearch.Text.Trim();
        sMsg = RunReport(sStockCode);

        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }
    }
    protected void txtStockCode_TextChanged(object sender, EventArgs e)
    {
        lblDescription.Text = SharedFunctions.GetStockCodeDesc(txtSearch.Text.Trim());
        lblError.Text = "";
        string sMsg = "";
        string sStockCode = "";

        sStockCode = txtSearch.Text.Trim();

        sMsg = RunReport(sStockCode);

        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }
    }
    protected void lbnReset_Click(object sender, EventArgs e)
    {
        lblDescription.Text = "";
        txtSearch.Text = "";
        string sStockCode = txtSearch.Text.Trim();
        RunReport(sStockCode);
    }


    protected void gvPackaging_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {



        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {

        }
    }
    protected void gvIngredients_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        try
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {




            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
    }

    protected void txtMaterialCost_TextChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            decimal dcNewMaterialCost = 0;

            TextBox txtMaterialCost = (TextBox)sender;
            Label lblStockCode = (Label)txtMaterialCost.Parent.FindControl("lblStockCode");

            if (txtMaterialCost.Text.Trim() != "")
            {
                dcNewMaterialCost = Convert.ToDecimal(txtMaterialCost.Text.Trim());


                InvEstimatedIngredientCost ie = db.InvEstimatedIngredientCost.Single(p => p.StockCode == lblStockCode.Text);
                ie.EstimatedIngredientCost = dcNewMaterialCost;
                ie.DateModified = DateTime.Now;
                db.SubmitChanges();

                lblError.Text = "**Ingredient Updated Successfully(" + lblStockCode.Text + ")!";
                lblError.ForeColor = Color.Green;


            }
        }
    }
    protected void txtPackagingCost_TextChanged(object sender, EventArgs e)
    {//Packaging...
        lblError.Text = "";
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            decimal dcNewPackagingCost = 0;

            TextBox txtPackagingCost = (TextBox)sender;
            Label lblNewCost = (Label)txtPackagingCost.Parent.FindControl("lblNewCost");
            Label lblQtyPer = (Label)txtPackagingCost.Parent.FindControl("lblQtyPer");
            Label lblStockCode = (Label)txtPackagingCost.Parent.FindControl("lblStockCode");
            CheckBox chkOverwrite = (CheckBox)txtPackagingCost.Parent.FindControl("chkOverwrite");

            if (txtPackagingCost.Text.Trim() != "")
            {
                dcNewPackagingCost = Convert.ToDecimal(txtPackagingCost.Text.Trim());


                InvEstimatedPackagingCost ie = db.InvEstimatedPackagingCost.Single(p => p.StockCode == lblStockCode.Text);
                ie.EstimatedPackagingCost = dcNewPackagingCost;
                ie.DateModified = DateTime.Now;
                db.SubmitChanges();

                lblError.Text = "**Packaging Updated Successfully(" + lblStockCode.Text + ")!";
                lblError.ForeColor = Color.Green;

            }

        }
    }
    protected void lbnAddPackaging_Click(object sender, EventArgs e)
    {
        AddPackagingCost();
    }
    protected void lbnAddIngredient_Click(object sender, EventArgs e)
    {
        AddIngredientCost();
    }
    protected void btnExportIngredients_Click(object sender, EventArgs e)
    {
        if (ViewState["dtIngredients"] != null)
        {
            DataTable dt = new DataTable();
            string sFilesName = "";

            dt = (DataTable)ViewState["dtIngredients"];
            if (dt.Rows.Count > 0)
            {
                dt.TableName = "dtIngredients";

                sFilesName = "IngredientList_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                ExportToExcel(dt, sFilesName);
            }
        }
    }
    protected void btnExportPackaging_Click(object sender, EventArgs e)
    {
        if (ViewState["dtPackaging"] != null)
        {
            DataTable dt = new DataTable();
            string sFilesName = "";

            dt = (DataTable)ViewState["dtPackaging"];
            if (dt.Rows.Count > 0)
            {
                dt.TableName = "dtPackaging";

                sFilesName = "PackagingList_" + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString();
                ExportToExcel(dt, sFilesName);
            }
        }
    }
    protected void gvPackaging_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            lblError.Text = "";

            int iUserID = 0;
            int iEstimatedPackagingCostID = 0;

            if (Session["UserID"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            iUserID = Convert.ToInt32(Session["UserID"]);
            int i = 0;
            Label lblEstimatedPackagingCostID;


            switch (e.CommandName)
            {
                case "Delete":
                    i = Convert.ToInt32(e.CommandArgument);

                    try
                    {
                        lblEstimatedPackagingCostID = (Label)gvPackaging.Rows[i].FindControl("lblEstimatedPackagingCostID");
                        iEstimatedPackagingCostID = Convert.ToInt32(lblEstimatedPackagingCostID.Text);
                        var query = (from d in db.HolidayCalendar select d);
                        if (query.Count() == 1)
                        {
                            lblError.Text = "**You cannot delete the last record in the table.";
                            lblError.ForeColor = Color.Red;
                            return;
                        }

                        InvEstimatedPackagingCost dl = db.InvEstimatedPackagingCost.Single(p => p.EstimatedPackagingCostID == iEstimatedPackagingCostID);
                        db.InvEstimatedPackagingCost.DeleteOnSubmit(dl);
                        db.SubmitChanges();

                        lblError.Text = "**Delete was successful!";
                        lblError.ForeColor = Color.Green;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "**Delete Failed!";
                        lblError.ForeColor = Color.Red;
                        Debug.WriteLine(ex.ToString());
                    }
                    break;
            }
        }
    }
    protected void gvPackaging_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvPackaging.EditIndex = -1;
        lblError.Text = "";
        string sMsg = "";
        string sStockCode = "";

        sStockCode = txtSearch.Text.Trim();
        sMsg = RunReport(sStockCode);

        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }
    }
    protected void gvIngredients_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            lblError.Text = "";

            int iUserID = 0;
            int iEstimatedIngredientCostID = 0;

            if (Session["UserID"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            iUserID = Convert.ToInt32(Session["UserID"]);
            int i = 0;
            Label lblEstimatedIngredientCostID;


            switch (e.CommandName)
            {
                case "Delete":
                    i = Convert.ToInt32(e.CommandArgument);

                    try
                    {
                        lblEstimatedIngredientCostID = (Label)gvIngredients.Rows[i].FindControl("lblEstimatedIngredientCostID");
                        iEstimatedIngredientCostID = Convert.ToInt32(lblEstimatedIngredientCostID.Text);
                        var query = (from d in db.HolidayCalendar select d);
                        if (query.Count() == 1)
                        {
                            lblError.Text = "**You cannot delete the last record in the table.";
                            lblError.ForeColor = Color.Red;
                            return;
                        }

                        InvEstimatedIngredientCost dl = db.InvEstimatedIngredientCost.Single(p => p.EstimatedIngredientCostID == iEstimatedIngredientCostID);
                        db.InvEstimatedIngredientCost.DeleteOnSubmit(dl);
                        db.SubmitChanges();

                        lblError.Text = "**Delete was successful!";
                        lblError.ForeColor = Color.Green;
                    }
                    catch (Exception ex)
                    {
                        lblError.Text = "**Delete Failed!";
                        lblError.ForeColor = Color.Red;
                        Debug.WriteLine(ex.ToString());
                    }
                    break;
            }
        }
    }

    protected void gvIngredients_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        gvIngredients.EditIndex = -1;
        lblError.Text = "";
        string sMsg = "";
        string sStockCode = "";

        sStockCode = txtSearch.Text.Trim();
        sMsg = RunReport(sStockCode);

        if (sMsg.Length > 0)
        {
            lblError.Text = sMsg;
            return;
        }
    }
    #endregion

    #region WebMethods
    [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
    public static string[] GetCompletionListStockCodes(string prefixText, int count, string contextKey)
    {//...
        using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
        {

            // Your LINQ to SQL query goes here 
            FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
            string[] listRecipe = db.InvEstimatedIngredientCost.Where(w => w.StockCode != null).OrderBy(w => w.StockCode).Select(w => (w.StockCode.Trim())).Distinct().ToArray();
            string[] listPackaging = db.InvEstimatedPackagingCost.Where(w => w.StockCode != null).OrderBy(w => w.StockCode).Select(w => (w.StockCode.Trim())).Distinct().ToArray();
            string[] list = listPackaging.Union(listRecipe).ToArray();
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