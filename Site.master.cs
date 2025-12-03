using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Transactions;
using System.Data;
using System.Linq;
using System.Data.Linq;
using System.Diagnostics;
public partial class SiteMaster : MasterPage
{
    private string CheckLogin(string sUserName, string sPassword)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
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
    }
    private string GetUserName(int iUserID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sUserName = "";

            var query = (from users in db.WipUsers
                         where users.UserID == iUserID
                         select new
                         {
                             users.UserName
                         });
            foreach (var a in query)
            {
                sUserName = a.UserName;
            }
            return sUserName;
        }
    }
    private string GetUserPassword(int iUserID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sPassword = "";

            var query = (from users in db.WipUsers
                         where users.UserID == iUserID
                         select new
                         {
                             users.Password
                         });
            foreach (var a in query)
            {
                sPassword = a.Password;
            }
            return sPassword;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        int iUserID = 0;
        if (Session["UserID"] != null)
        {
            iUserID = Convert.ToInt32(Session["UserID"]);
            if (Request.QueryString["page"] != null)
            {
                switch (Request.QueryString["page"].ToString())
                {
                    case "1"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=1
                        Response.Redirect("SalesOrderTracker.aspx");
                        break;
                    case "2"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=2
                        Response.Redirect("WipCostReport.aspx");
                        break;
                    case "3"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=3
                        Response.Redirect("CustomerReportsNew.aspx");
                        break;
                    case "4"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=4
                        Response.Redirect("UnitCostReport.aspx");
                        break;
                    case "5"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=5
                        Response.Redirect("IngredientCostHistory.aspx");
                        break;
                    case "6"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=6
                        Response.Redirect("RecipeComponentReport.aspx");
                        break;
                    case "7"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=7
                        Response.Redirect("PurchaseOrderTracker.aspx");
                        break;
                    case "8"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=8
                        Response.Redirect("BE_Report.aspx");
                        break;
                }
            }
        }
        else
        {
            if (Request.QueryString["userid"] != null)
            {

                iUserID = Convert.ToInt32(Request.QueryString["userid"]);//Brian is #4
                string sPassword = "";
                string sUserName = "";


                Label lblError = (Label)LogOnUserControl1.FindControl("lblError");
                Label lblWelcome = (Label)LogOnUserControl1.FindControl("lblWelcome");
                Panel pnlSignOn = (Panel)LogOnUserControl1.FindControl("pnlSignOn");
                Button btnLogOff = (Button)LogOnUserControl1.FindControl("btnLogOff");
                CheckBox chkRememberMe = (CheckBox)LogOnUserControl1.FindControl("chkRememberMe");
                TextBox txtUname = (TextBox)LogOnUserControl1.FindControl("txtUname");
                TextBox txtPwd = (TextBox)LogOnUserControl1.FindControl("txtPwd");

                txtUname.Text = GetUserName(iUserID);
                txtPwd.Text = GetUserPassword(iUserID);
                sUserName = txtUname.Text.Trim();
                sPassword = txtPwd.Text.Trim();

                string sResult = "";
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

                    lblWelcome.Text = "Welcome, " + Session["FirstName"].ToString() + " " + Session["LastName"].ToString() + " (" + Session["UserID"].ToString() + ") - " + Session["SecurityLevel"].ToString();
                    pnlSignOn.Visible = false;
                    btnLogOff.Visible = true;
                    //Redirect to either Admin or Merchant back office...
                    //if Role =1 then (go to Admin back office) else if Role = 2 then (go to User Section)


                    if (Request.QueryString["page"] != null)
                    {
                        switch (Request.QueryString["page"].ToString())
                        {
                            case "1"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=1
                                Response.Redirect("SalesOrderTracker.aspx");
                                break;
                            case "2"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=2
                                Response.Redirect("WipCostReport.aspx");
                                break;
                            case "3"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=3
                                Response.Redirect("CustomerReportsNew.aspx");
                                break;
                            case "4"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=4
                                Response.Redirect("UnitCostReport.aspx");
                                break;
                            case "5"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=5
                                Response.Redirect("IngredientCostHistory.aspx");
                                break;
                            case "6"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=6
                                Response.Redirect("RecipeComponentReport.aspx");
                                break;
                            case "7"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=7
                                Response.Redirect("PurchaseOrderTracker.aspx");
                                break;
                            case "8"://http://localhost/Felbro_B_V7/Default.aspx?userid=4&page=8
                                Response.Redirect("BE_Report.aspx");
                                break;
                        }
                    }


                }//End if result is not blank....
            }//End if Query string is not null...
        }//End if is session userid is null...



        try
        {
            using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
                {
                    var query = (from menu in db.Menu
                                 where menu.Show == 1
                                 orderby menu.SortOrder
                                 select new
                                 {
                                     menu.MenuID,
                                     menu.Text,
                                     menu.Description,
                                     menu.NavigateUrl,
                                     ImageUrl =
                                          ((from icons in db.Icons
                                            where
                                              icons.ImageID == menu.ImageID
                                            select new
                                            {
                                                icons.ImageUrl
                                            }).First().ImageUrl),
                                     menu.Target,
                                     menu.ParentID
                                 });

                    dt = SharedFunctions.ToDataTable(db, query);

                    ds.Tables.Add(dt);
                    ds.DataSetName = "Menus";
                    ds.Tables[0].TableName = "Menu";
                    DataRelation relation = new DataRelation("ParentChild", ds.Tables["Menu"].Columns["MenuID"], ds.Tables["Menu"].Columns["ParentID"], true);

                    relation.Nested = true;
                    ds.Relations.Add(relation);

                    xmlDataSource.Data = ds.GetXml();
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }

    }

    protected void MyMenu_MenuItemDataBound(object sender, System.Web.UI.WebControls.MenuEventArgs e)
    {
        SharedFunctions.SetupMenu(e);

    }

}