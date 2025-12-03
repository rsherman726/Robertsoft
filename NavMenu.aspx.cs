using System.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Linq;
using System.Data.Linq;
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
using System.Transactions;

public partial class NavMenu : System.Web.UI.Page
{
    #region Variables

    string gvUniqueID = String.Empty;
    // int gvNewPageIndex = 0;
    //    int gvEditIndex = -1;
    string gvSortExpr = String.Empty;

    #endregion

    #region Subs
    private void BindData(string sDDLValue)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            if (sDDLValue == "NONE" || sDDLValue == "")
            {
                var query = (from menu in db.Menu
                             select new
                             {

                                 menu.MenuID,
                                 menu.Text,
                                 menu.Description,
                                 menu.NavigateUrl,
                                 menu.ImageID,
                                 menu.SortOrder,
                                 menu.Show,
                                 ImageURL =
                                    ((from icons in db.Icons
                                      where
                                    icons.ImageID == menu.ImageID
                                      select new
                                      {
                                          icons.ImageUrl
                                      }).First().ImageUrl),
                                 menu.Target,
                                 menu.ParentID
                             }).Take(1);
                foreach (var a in query)
                {
                    txtRankingNumUpDown.Text = a.SortOrder.ToString();
                    txtMenuID.Text = a.MenuID.ToString();
                    txtText.Text = a.Text;
                    txtDescription.Text = a.Description;
                    txtNavigateUrl.Text = a.NavigateUrl;
                    txtTarget.Text = a.Target;
                    int iImageUrlID = 0;
                    if (a.ImageID.HasValue)
                    {
                        iImageUrlID = Convert.ToInt32(a.ImageID);
                    }
                    ddlImageUrl.SelectedIndex = SharedFunctions.GetSelIndex(iImageUrlID.ToString(), ddlImageUrl, "Value");
                    if (a.ImageURL != "")
                    {
                        imgIcon.ImageUrl = a.ImageURL;
                    }
                    else
                    {
                        imgIcon.ImageUrl = @"images\16.jpg";
                    }
                    string sParent = "";
                    sParent = a.ParentID.ToString();
                    ddlParentID.SelectedIndex = SharedFunctions.GetSelIndex(sParent, ddlParentID, "Value");
                    if (a.Show == 0)
                    {
                        chkShowOnMenu.Checked = false;
                    }
                    else//1
                    {
                        chkShowOnMenu.Checked = true;
                    }
                }
            }
            else
            {
                var query = (from menu in db.Menu
                             where menu.MenuID == Convert.ToInt32(sDDLValue)
                             select new
                             {
                                 menu.MenuID,
                                 menu.Text,
                                 menu.Description,
                                 menu.NavigateUrl,
                                 menu.ImageID,
                                 menu.SortOrder,
                                 menu.Show,
                                 ImageURL =
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

                foreach (var a in query)
                {
                    txtRankingNumUpDown.Text = a.SortOrder.ToString();
                    txtMenuID.Text = a.MenuID.ToString();
                    txtText.Text = a.Text;
                    txtDescription.Text = a.Description;
                    txtNavigateUrl.Text = a.NavigateUrl;
                    txtTarget.Text = a.Target;
                    int iImageUrlID = 0;
                    if (a.ImageID.HasValue)
                    {
                        iImageUrlID = Convert.ToInt32(a.ImageID);
                    }
                    ddlImageUrl.SelectedIndex = SharedFunctions.GetSelIndex(iImageUrlID.ToString(), ddlImageUrl, "Value");
                    if (a.ImageURL != "")
                    {
                        imgIcon.ImageUrl = a.ImageURL;
                    }
                    else
                    {
                        imgIcon.ImageUrl = @"images\16.jpg";
                    }
                    string sParent = "";
                    sParent = a.ParentID.ToString();
                    ddlParentID.SelectedIndex = SharedFunctions.GetSelIndex(sParent, ddlParentID, "Value");
                    if (a.Show == 0)
                    {
                        chkShowOnMenu.Checked = false;
                    }
                    else//1
                    {
                        chkShowOnMenu.Checked = true;
                    }
                }

            }
        }
    }
    private void LoadCurrent()
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            ddlCurrent.Items.Clear();

            var query = (from menu in db.Menu
                         orderby menu.Description
                         select new
                         {
                             menu.Description,
                             menu.MenuID
                         });

            if (query.Count() > 0)
            {
                ddlCurrent.Items.Add(new ListItem("Select a menu item to admin...", "NONE"));
                foreach (var a in query)
                {
                    ddlCurrent.Items.Add(new ListItem(a.Description, a.MenuID.ToString()));
                }
            }
            else
            {
                ddlCurrent.Items.Add(new ListItem("No menu items found, try again...", "NONE"));
                lblMessage.Text = "**There are no menu items in the database with that search criteria, try again!";
            }

            ddlCurrent.SelectedIndex = 0;
        }
    }
    private void LoadParentID()
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            ddlParentID.Items.Clear();

            var query = (from menu in db.Menu
                         orderby menu.Description
                         select new
                         {
                             menu.Description,
                             menu.MenuID
                         });

            if (query.Count() > 0)
            {
                
                foreach (var a in query)
                {
                    ddlParentID.Items.Add(new ListItem(a.Description, a.MenuID.ToString()));
                }

                ddlParentID.Items.Insert(0,new ListItem("NONE", "NONE"));
            }
            else
            {
                ddlParentID.Items.Add(new ListItem("No menu items found, try again...", "NONE"));
                lblMessage.Text = "**There are no menu items in the database with that search criteria, try again!";
            }

            ddlParentID.SelectedIndex = 0;

        }
    }
    private void LoadImageUrl()
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            ddlImageUrl.Items.Clear();

            var query = (from icons in db.Icons
                         where icons.ImageUrl != null
                         orderby icons.ImageUrl
                         select new
                         {
                             icons.ImageID,
                             icons.ImageUrl
                         });


            if (query.Count() > 0)
            {
                ddlImageUrl.Items.Add(new ListItem("NONE", "NONE"));

                foreach (var a in query)
                {
                    ddlImageUrl.Items.Add(new ListItem(a.ImageUrl, a.ImageID.ToString()));
                }
            }
            else
            {
                ddlImageUrl.Items.Add(new ListItem("No images found, try again...", "NONE"));
                lblMessage.Text = "**There are no images in the database with that search criteria, try again!";
            }

            ddlImageUrl.SelectedIndex = 0;

        }
    }
    private void AddRecord(object sender, System.EventArgs e)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            //make sure there are no validation errors...
            if (!Page.IsValid)
            {
                return;
            }
            lblMessage.Text = "";
            string msg = "";
            DataSet ds = new DataSet();

            //int iResult = 0;
            //validation...
            if (SharedFunctions.MenuItemAlreadyExists(txtText.Text.Trim()))
            {
                msg = "**Menu item already exists!";
            }
            string sMenuItem = txtMenuID.Text.Trim();
            int iMenuItem = 0;
            if (sMenuItem != "")
            {
                if (!SharedFunctions.IsNumeric(sMenuItem))
                {
                    msg = "**Menu item must be a numeric value, try again.";
                }
                else
                {
                    iMenuItem = Convert.ToInt32(sMenuItem);
                    if (SharedFunctions.MenuItemIDAlreadyExists(iMenuItem))
                    {
                        msg = "**Menu item already exists!";
                    }
                }
            }
            else
            {
                //Handled by validator...
            }
            if (lbIncluded.Items.Count == 0)
            {
                msg = "**You must select at least one security role from excluded roles listbox!";
            }
            if (txtNavigateUrl.Text.Trim().ToUpper().Contains("Images\\Excel".ToUpper()))
            {
                if (!txtNavigateUrl.Text.Trim().ToUpper().Contains(".XLSX") && !txtNavigateUrl.Text.Trim().ToUpper().Contains(".XLS"))
                {
                    msg += "**You are missing the .XLS or .XLSX extension!! <br/>";
                }
            }

            if (msg.Length > 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = msg;

                return;
            }

            //remove "'"...

            using (var transaction = new TransactionScope())
            {

                try
                {
                    Menu menu = new Menu();


                    menu.MenuID = iMenuItem;
                    menu.Text = "&nbsp;" + txtText.Text.Trim();
                    menu.Description = txtDescription.Text.Trim();

                    //Nullable items...
                    if (txtNavigateUrl.Text.Trim() != "")
                    {
                        menu.NavigateUrl = txtNavigateUrl.Text.Trim();
                    }
                    else
                    {
                        menu.NavigateUrl = null;
                    }

                    if (ddlImageUrl.SelectedIndex != 0)
                    {
                        menu.ImageID = Convert.ToInt32(ddlImageUrl.SelectedValue);
                    }
                    else
                    {
                        menu.ImageID = null;
                    }
                    if (txtTarget.Text.Trim() != "")
                    {
                        menu.Target = txtTarget.Text.Trim();
                    }
                    else
                    {
                        menu.Target = null;
                    }

                    if (ddlParentID.SelectedIndex != 0)
                    {
                        menu.ParentID = Convert.ToInt32(ddlParentID.SelectedValue);
                    }
                    else
                    {
                        menu.ParentID = null;
                    }
                    menu.SortOrder = Convert.ToInt32(txtRankingNumUpDown.Text);
                    if (chkShowOnMenu.Checked)
                    {
                        menu.Show = 1;
                    }
                    else
                    {
                        menu.Show = 0;
                    }
                    db.Menu.InsertOnSubmit(menu);

                    db.SubmitChanges();

                    //Add security Records to Menu Item Access table...
                    foreach (ListItem li in lbIncluded.Items)
                    {//Insert each role id from included list...
                        MenuItemAccess mia = new MenuItemAccess();

                        mia.MenuID = iMenuItem;
                        mia.AdminID = Convert.ToInt32(li.Value);

                        db.MenuItemAccess.InsertOnSubmit(mia);
                        //submit changes to take effect
                        db.SubmitChanges();

                    }

                    transaction.Complete();

                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "**Menu item Added successfully!";

                    ResetForm();

                }
                catch (Exception ex)
                {
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Text = "**Menu item Add failed!";
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    transaction.Dispose();
                    LoadExcluded();
                    txtMenuID.Text = MaxMenuID().ToString();
                }
            }
        }
    }
    private void UpdateRecord(object sender, System.EventArgs e)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            //make sure there are no validation errors...
            //if (!Page.IsValid)
            //{
            //    btnUpdate.Enabled = true;
            //    btnDelete.Enabled = true;
            //    return;
            //}
            string msg = "";

            lblMessage.Text = "";
            lblMessage.Text = "";

            //validation...

            string sMenuItem = txtMenuID.Text.Trim();
            int iMenuItemID = 0;
            int iParentID = 0;
            iMenuItemID = Convert.ToInt32(sMenuItem);

            if (ddlParentID.SelectedIndex != 0)
            {
                iParentID = Convert.ToInt32(ddlParentID.SelectedValue);
            }
            if (txtNavigateUrl.Text.Trim().ToUpper().Contains("Images\\Excel".ToUpper()))
            {
                if (!txtNavigateUrl.Text.Trim().ToUpper().Contains(".XLSX") && !txtNavigateUrl.Text.Trim().ToUpper().Contains(".XLS"))
                {
                    msg += "**You are missing the .XLS or .XLSX extension!! <br/>";
                }
            }
            //Check for cirular reference...
            //If ParentID and MenuID are equal throw error message...
            if (iMenuItemID == iParentID)
            {
                msg += "**You can't assign a menu item to itself!! <br/>";
            }
            if (IsCirularReference(iParentID, iMenuItemID))
            {
                msg += "**You can't assign this parentID to this Menu Item because it would create a cirular reference!! <br/>";
            }

            if (msg.Length > 0)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = msg;

                return;
            }

            try
            {

                Menu menu = db.Menu.Single(a => a.MenuID == iMenuItemID);
                if (txtText.Text.Trim().StartsWith("&nbsp;"))
                {
                    menu.Text = txtText.Text.Trim();
                }
                else
                {
                    menu.Text = "&nbsp;" + txtText.Text.Trim();
                }

                menu.Description = txtDescription.Text.Trim();
                //Nullable items...
                if (txtNavigateUrl.Text.Trim() != "")
                {
                    string sURL = "";
                    sURL = txtNavigateUrl.Text.Trim();
                    menu.NavigateUrl = sURL;
                }
                else
                {
                    menu.NavigateUrl = null;
                }

                if (ddlImageUrl.SelectedIndex != 0)
                {
                    menu.ImageID = Convert.ToInt32(ddlImageUrl.SelectedValue);
                }
                else
                {
                    menu.ImageID = null;
                }
                if (txtTarget.Text.Trim() != "")
                {
                    menu.Target = txtTarget.Text.Trim();
                }
                else
                {
                    menu.Target = null;
                }

                if (ddlParentID.SelectedIndex != 0)
                {
                    menu.ParentID = Convert.ToInt32(ddlParentID.SelectedValue);
                }
                else
                {
                    menu.ParentID = null;
                }
                menu.SortOrder = Convert.ToInt32(txtRankingNumUpDown.Text);
                if (chkShowOnMenu.Checked)
                {
                    menu.Show = 1;
                }
                else
                {
                    menu.Show = 0;
                }
                db.SubmitChanges();

                //Updating Security roles is a live action....

                LoadCurrent();

                ddlCurrent.SelectedIndex = SharedFunctions.GetSelIndex(txtMenuID.Text.Trim().ToUpper(), ddlCurrent, "Value");

                BindData(ddlCurrent.SelectedValue);
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "**Menu item updated successfully!";

            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "**Menu item Update failed!";
                Debug.WriteLine(ex.ToString());
            }

            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }
    }
    private void DeleteRecord()
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lblMessage.Text = "";
            using (var transaction = new TransactionScope())
            {
                try
                {
                    int iMenuItemID = Convert.ToInt32(ddlCurrent.SelectedValue);


                    //Now delete child security records from Menu Item Access Table...
                    foreach (ListItem li in lbIncluded.Items)
                    {//Delete each role id from included list...
                        MenuItemAccess mia = db.MenuItemAccess.Single(a => a.AdminID == Convert.ToInt32(li.Value) && a.MenuID == iMenuItemID);

                        db.MenuItemAccess.DeleteOnSubmit(mia);
                        //submit changes to take effect
                        db.SubmitChanges();
                    }

                    //Update all Children SET ParentID to null...
                    var query = (from mi in db.Menu
                                 where mi.ParentID == iMenuItemID
                                 select new { mi.MenuID });
                    foreach (var a in query)
                    {
                        Menu miUpdate = db.Menu.Single(p => p.MenuID == a.MenuID);
                        miUpdate.ParentID = null;
                        db.SubmitChanges();
                    }

                    //now delete parent security records from Menu table...
                    Menu menu = db.Menu.Single(a => a.MenuID == iMenuItemID);
                    db.Menu.DeleteOnSubmit(menu);
                    db.SubmitChanges();
                    transaction.Complete();//Complete...

                    lblMessage.ForeColor = System.Drawing.Color.Green;
                    lblMessage.Text = "**Record deleted!<br>";
                }
                catch (Exception ex)
                {
                    lblMessage.Text = "**Delete failed!";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    Debug.WriteLine(ex.ToString());
                }
                finally
                {
                    transaction.Dispose();
                }
            }
            //Rebind data...

            ddlCurrent.SelectedIndex = 1;

            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
        }
    }
    private void ResetForm()
    {
        txtText.Text = "";
        txtDescription.Text = "";

        if (rblMode.SelectedItem.Value == "1")//Add Mode...
        {
            lbExcluded.Items.Clear();
            lbIncluded.Items.Clear();
            //Load Excluded Roles now...

        }
        else//Edit...
        {
            txtMenuID.Text = "";
        }

        txtTarget.Text = "";
        txtNavigateUrl.Text = "";
        imgIcon.ImageUrl = @"images\16.jpg";
        ddlParentID.SelectedIndex = 0;
        ddlImageUrl.SelectedIndex = 0;
        txtRankingNumUpDown.Text = "1";

    }
    private void LoadIncluded(int iMenuID)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbIncluded.Items.Clear();//clean out list before loading...
            var query = (from mia in db.MenuItemAccess
                         where
                           mia.MenuID == iMenuID
                         select new
                         {
                             mia.AdminID,
                             Role =
                               ((from roles in db.WipUsersRoles
                                 where
                                   roles.RoleID == mia.AdminID
                                 select new
                                 {
                                     roles.Role
                                 }).First().Role)
                         });

            if (query.Count() > 0)
            {
                foreach (var a in query)
                {
                    lbIncluded.Items.Add(new ListItem(a.Role, a.AdminID.ToString()));
                }
            }
        }
    }
    private void LoadExcluded()
    {//For Add Mode...
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            lbExcluded.Items.Clear();//clear before loading...  
            var query = (from r in db.WipUsersRoles
                         orderby r.Role
                         select new
                         {
                             AdminID = r.RoleID,
                             r.Role
                         });

            if (query.Count() > 0)
            {
                foreach (var a in query)
                {
                    lbExcluded.Items.Add(new ListItem(a.Role, a.AdminID.ToString()));
                }
            }

            //No Included Roles yet...
            rsListbox.Sort(ref lbIncluded, rsListbox.SortOrder.Ascending);
            rsListbox.Sort(ref lbExcluded, rsListbox.SortOrder.Ascending);
        }
    }
    private void LoadExcluded(int iMenuID)
    {//For Edit Mode...

        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            lbIncluded.Items.Clear();//clear any left over items first...
            LoadIncluded(iMenuID);//reload before removing...
            lbExcluded.Items.Clear();//clear before loading...  
            //Excluded...
            var queryEx = (from r in db.WipUsersRoles
                           where
                             !
                               (from mia in db.MenuItemAccess
                                where mia.MenuID == iMenuID
                                select new
                                {
                                    mia.AdminID
                                }).Contains(new { AdminID = r.RoleID })
                           select new
                           {
                               r.Role,
                               r.RoleID
                           });
            lbExcluded.Items.Clear();
            if (queryEx.Count() > 0)
            {
                foreach (var a in queryEx)
                {
                    lbExcluded.Items.Add(new ListItem(a.Role, a.RoleID.ToString()));
                }
            }
            var queryIn = (from mia in db.MenuItemAccess
                           where
                             mia.MenuID == iMenuID
                           select new
                           {
                               mia.AdminID,
                               Role =
                                 ((from roles in db.WipUsersRoles
                                   where
                                     roles.RoleID == mia.AdminID
                                   select new
                                   {
                                       roles.Role
                                   }).First().Role)
                           });
            lbIncluded.Items.Clear();
            // Remove item(s) from Available listbox...
            if (queryIn.Count() > 0)
            {
                foreach (var a in queryIn)
                {
                    lbIncluded.Items.Add(new ListItem(a.Role, a.AdminID.ToString()));
                }
            }

            rsListbox.Sort(ref lbIncluded, rsListbox.SortOrder.Ascending);
            rsListbox.Sort(ref lbExcluded, rsListbox.SortOrder.Ascending);
        }
    }
    private void RemoveSingleFunction(int iIncludedAdminID, int iMenuItemID)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            if (lbIncluded.Items.Count == 0) { return; }
            if (lbIncluded.SelectedIndex == -1) { return; }

            //get the linq recordset...       

            try
            {
                //Remove from Included Roles from Menu Item Access Table...
                MenuItemAccess mia = db.MenuItemAccess.Single(c => c.AdminID == iIncludedAdminID && c.MenuID == iMenuItemID);
                db.MenuItemAccess.DeleteOnSubmit(mia);
                db.SubmitChanges();
            }
            catch (Exception)
            {

            }
            finally
            {
                //Sort the litboxes...
                rsListbox.Sort(ref lbIncluded, rsListbox.SortOrder.Ascending);
                rsListbox.Sort(ref lbExcluded, rsListbox.SortOrder.Ascending);
            }
        }
    }
    private void RemoveSingleFunctionAll(int iIncludedAdminID, int iMenuItemID)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            if (lbIncluded.Items.Count == 0) { return; }

            try
            {
                //Remove from Included Roles from Menu Item Access Table...
                MenuItemAccess mia = db.MenuItemAccess.Single(c => c.AdminID == iIncludedAdminID && c.MenuID == iMenuItemID);
                db.MenuItemAccess.DeleteOnSubmit(mia);
                db.SubmitChanges();
            }
            catch (Exception)
            {

            }
            finally
            {
                //Sort the litboxes...
                rsListbox.Sort(ref lbIncluded, rsListbox.SortOrder.Ascending);
                rsListbox.Sort(ref lbExcluded, rsListbox.SortOrder.Ascending);
            }
        }
    }
    private void RemoveSingleFunction(string sAdmin, string sAdminID)
    {
        int iAdminID = Convert.ToInt32(sAdminID);

        if (lbIncluded.Items.Count == 0) { return; }
        if (lbIncluded.SelectedIndex == -1) { return; }

        lbExcluded.Items.Add(new ListItem(sAdmin, iAdminID.ToString()));//Add to included role...

        // Remove item(s) from Available listbox...
        foreach (ListItem li in lbExcluded.Items)
        {
            if (iAdminID.ToString() == li.Value)
            {
                lbIncluded.Items.RemoveAt(lbIncluded.SelectedIndex);//remove from available items list box...
            }
        }

        rsListbox.Sort(ref lbIncluded, rsListbox.SortOrder.Ascending);
        rsListbox.Sort(ref lbExcluded, rsListbox.SortOrder.Ascending);

    }
    private void AddSingleFunction(int iExcludedAdminID, int iMenuItemID)
    {//For Edit mode...
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            if (lbExcluded.Items.Count == 0) { return; }
            if (lbExcluded.SelectedIndex == -1) { return; }

            try
            {
                //Now add Included to Menu Item Access table...
                if (MenuItemAlreadyExists(iMenuItemID, iExcludedAdminID))
                {
                    return;
                }
                MenuItemAccess mia = new MenuItemAccess();
                mia.MenuID = iMenuItemID;
                mia.AdminID = iExcludedAdminID;
                db.MenuItemAccess.InsertOnSubmit(mia);
                db.SubmitChanges();


            }
            catch (Exception)
            {


            }
            finally
            {
                rsListbox.Sort(ref lbIncluded, rsListbox.SortOrder.Ascending);
                rsListbox.Sort(ref lbExcluded, rsListbox.SortOrder.Ascending);

            }
        }
    }
    private void AddSingleFunctionAll(int iExcludedAdminID, int iMenuItemID)
    {//For Edit mode...
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            if (lbExcluded.Items.Count == 0) { return; }
            try
            {
                //Now add Included to Menu Item Access table...
                if (MenuItemAlreadyExists(iMenuItemID, iExcludedAdminID))
                {
                    return;
                }
                MenuItemAccess mia = new MenuItemAccess();
                mia.MenuID = iMenuItemID;
                mia.AdminID = iExcludedAdminID;
                db.MenuItemAccess.InsertOnSubmit(mia);
                db.SubmitChanges();
            }
            catch (Exception)
            {


            }
            finally
            {
                rsListbox.Sort(ref lbIncluded, rsListbox.SortOrder.Ascending);
                rsListbox.Sort(ref lbExcluded, rsListbox.SortOrder.Ascending);

            }
        }
    }
    private void AddSingleFunction(string sAdmin, string sAdminID)
    {//For add mode...
        //put  item(s) into cart list box... 
        int iAdminID = Convert.ToInt32(sAdminID);
        lbIncluded.Items.Add(new ListItem(sAdmin, iAdminID.ToString()));//Add to included role...

        // Remove item(s) from Available listbox...
        foreach (ListItem li in lbIncluded.Items)
        {
            if (iAdminID.ToString() == li.Value)
            {
                lbExcluded.Items.RemoveAt(lbExcluded.SelectedIndex);//remove from available items list box...
            }
        }

        rsListbox.Sort(ref lbIncluded, rsListbox.SortOrder.Ascending);
        rsListbox.Sort(ref lbExcluded, rsListbox.SortOrder.Ascending);

    }

    #endregion 

    #region Functions

    private Int64 MaxMenuID()
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            Int64 iMaxMenuID = 0;
            var query =
           (from menu in
                (from menu in db.Menu
                 select new
                 {
                     menu.MenuID,
                     Dummy = "x"
                 })
            group menu by new { menu.Dummy } into g
            select new
            {
                MaxMenuID = (System.Int64?)(g.Max(p => p.MenuID) + 1)
            });
            foreach (var max in query)
            {
                iMaxMenuID = max.MaxMenuID.Value;
            }

            return iMaxMenuID;
        }
    }
    private bool IsCirularReference(int iPossibleParentID, int iMenuItemIdOfSubject)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from m in db.Menu
                         where m.MenuID == iPossibleParentID
                         && m.ParentID == iMenuItemIdOfSubject
                         select m);
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
    private bool MenuItemAlreadyExists(int iMenuItemID, int iAdminID)
    {
        using (FELBRO db = new FELBRO(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from m in db.MenuItemAccess
                         where m.MenuID == iMenuItemID
                         && m.AdminID == iAdminID
                         select m);
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
            ViewState["sortColumn"] = " ";
            ViewState["sortDirection"] = " ";
            LoadCurrent();
            LoadParentID();
            LoadImageUrl();
            imgIcon.ImageUrl = @"images\16.jpg";

        }


        ////////////////////////////////////// 

        Button AddButton = btnAdd;
        //Confirm Update...
        AddButton.Attributes["onclick"] = "javascript:return ";
        AddButton.Attributes["onclick"] += "confirm('Are you sure you want to add this menu item";
        AddButton.Attributes["onclick"] += "?')";

        Button DeleteButton = btnDelete;
        //Confirm Update...
        DeleteButton.Attributes["onclick"] = "javascript:return ";
        DeleteButton.Attributes["onclick"] += "confirm('Are you sure you want to delete this menu item";
        DeleteButton.Attributes["onclick"] += "?')";
        ////////////////////////////////////////////////////

        if (rblMode.SelectedItem.Value == "1")//Add Mode...
        {
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            ddlCurrent.Enabled = false;
            txtMenuID.Text = MaxMenuID().ToString();

        }
        else//Edit Mode...
        {
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            ddlCurrent.Enabled = true;

            if (ddlCurrent.SelectedIndex == 0)
            {
                lbExcluded.Items.Clear();
                lbIncluded.Items.Clear();
                btnAddOne.Enabled = false;
                btnAddAll.Enabled = false;
                btnRemoveOne.Enabled = false;
                btnRemoveAll.Enabled = false;
            }
            else
            {
                btnAddOne.Enabled = true;
                btnAddAll.Enabled = true;
                btnRemoveOne.Enabled = true;
                btnRemoveAll.Enabled = true;
            }

        }
    }
    protected void ddlCurrent_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        //Escape if no client is selected...
        if (ddlCurrent.SelectedIndex == 0)
        {
            ResetForm();
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            btnUpdate.Enabled = false;
            return;
        }
        else
        {
            btnDelete.Enabled = true;
            btnUpdate.Enabled = true;
            btnUpdate.Enabled = true;
        }



        BindData(ddlCurrent.SelectedValue);

        //Wouldnt see list if in add mode...
        if (ddlCurrent.SelectedIndex != 0)
        {
            int iMenuID = Convert.ToInt32(ddlCurrent.SelectedValue);

            LoadExcluded(iMenuID);
            LoadIncluded(iMenuID);
        }

    }
    protected void rblMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (rblMode.SelectedItem.Value == "1")//Add Mode...
        {
            ResetForm();
            txtMenuID.Text = MaxMenuID().ToString();
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            ddlCurrent.Visible = false;
            lblCurrentDepts.Visible = false;
            btnAddOne.Enabled = true;
            btnAddAll.Enabled = true;
            btnRemoveOne.Enabled = true;
            btnRemoveAll.Enabled = true;
            LoadExcluded();
            LoadParentID();
        }
        else//Edit Mode...
        {
            lbExcluded.Items.Clear();
            lbIncluded.Items.Clear();
            txtMenuID.Text = "";
            btnAdd.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            ddlCurrent.Visible = true;
            ddlCurrent.Enabled = true;
            lblCurrentDepts.Visible = true;
            LoadCurrent();

        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        ResetForm();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        UpdateRecord(sender, e);
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        DeleteRecord();
        BindData(ddlCurrent.SelectedValue);
        LoadCurrent();
        LoadParentID();
    }
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        AddRecord(sender, e);
        LoadParentID();
    }
    protected void ddlImageUrl_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlImageUrl.SelectedIndex != 0)
        {
            imgIcon.ImageUrl = ddlImageUrl.SelectedItem.Text;
        }
        else
        {
            imgIcon.ImageUrl = @"images\16.jpg";
        }


    }
    protected void btnAddOne_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (lbExcluded.SelectedIndex == -1)
        {
            lblMessage.Text = "**You have not selected a role to add, try again.";
            return;
        }
        if (rblMode.SelectedItem.Value == "1")//Add Mode...
        {
            if (lbExcluded.Items.Count == 0) { return; }
            if (lbExcluded.SelectedIndex == -1) { return; }

            bool bSelected = false;
            foreach (ListItem li in lbExcluded.Items)
            {
                if (li.Selected == false)
                {
                    bSelected = false;
                }
                else
                {
                    bSelected = true;
                    break;
                }
            }
            if (bSelected == false)
            {
                lblMessage.Text = "**You have not selected a role!";
                return;
            }
            else
            {
                lblMessage.Text = "";
            }
            string sAdmin = lbExcluded.SelectedItem.Text;
            string sAdminID = lbExcluded.SelectedValue;
            AddSingleFunction(sAdmin, sAdminID);
        }
        else//Edit Mode...
        {
            int iMenuItemID = 0;
            int iExcludedAdminID = 0;

            if (lbExcluded.Items.Count == 0)
            {
                lblMessage.Text = "**Not roles to Add!";
                return;
            }
            iExcludedAdminID = Convert.ToInt32(lbExcluded.SelectedValue);
            iMenuItemID = Convert.ToInt32(ddlCurrent.SelectedValue);

            AddSingleFunction(iExcludedAdminID, iMenuItemID);

            if (ddlCurrent.SelectedIndex != 0)
            {
                LoadExcluded(iMenuItemID);
                LoadIncluded(iMenuItemID);
            }
        }


    }
    protected void btnRemoveOne_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (lbIncluded.SelectedIndex == -1)
        {
            lblMessage.Text = "**You have not selected a role to remove, try again.";
            return;
        }

        if (rblMode.SelectedItem.Value == "1")//Add Mode...
        {

            string sAdmin = lbIncluded.SelectedItem.Text;
            string sAdminID = lbIncluded.SelectedValue;

            RemoveSingleFunction(sAdmin, sAdminID);

        }
        else//Edit Mode...
        {
            int iMenuItemID = 0;
            int iIncludedAdminID = 0;
            if (lbIncluded.Items.Count == 0)
            {
                lblMessage.Text = "**No roles to remove!";
                return;
            }

            iIncludedAdminID = Convert.ToInt32(lbIncluded.SelectedValue);
            iMenuItemID = Convert.ToInt32(ddlCurrent.SelectedValue);
            RemoveSingleFunction(iIncludedAdminID, iMenuItemID);
            if (ddlCurrent.SelectedIndex != 0)
            {
                int iMenuID = Convert.ToInt32(ddlCurrent.SelectedValue);

                LoadExcluded(iMenuID);
                LoadIncluded(iMenuID);
            }
        }
    }
    protected void btnAddAll_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (lbExcluded.SelectedIndex == -1)
        {
            lblMessage.Text = "**You have not selected a role(s) to Add, try again.";
            return;
        }
        if (rblMode.SelectedItem.Value == "1")//Add Mode...
        {

            ListItemCollection lsc = new ListItemCollection();
            for (int i = 0; i < lbExcluded.Items.Count; i++)
            {
                if (lbExcluded.Items[i].Selected == true)
                {
                    string value = lbExcluded.Items[i].Value;
                    string text = lbExcluded.Items[i].Text;
                    ListItem lst = new ListItem();
                    lst.Text = text;
                    lst.Value = value;
                    lbIncluded.Items.Add(lst);
                    lsc.Add(lst);
                }
            }
            foreach (ListItem ls in lsc)
            {
                lbExcluded.Items.Remove(ls);
            }
            rsListbox.Sort(ref lbIncluded, rsListbox.SortOrder.Ascending);
            rsListbox.Sort(ref lbExcluded, rsListbox.SortOrder.Ascending);
        }
        else//Edit mode...
        {
            int iMenuItemID = 0;
            int iExcludedAdminID = 0;

            if (lbExcluded.Items.Count == 0)
            {
                lblMessage.Text = "**Not roles to Add!";
                return;
            }
            iMenuItemID = Convert.ToInt32(ddlCurrent.SelectedValue);

            ArrayList arRoles = new ArrayList();

            foreach (ListItem li in lbExcluded.Items)
            {
                iExcludedAdminID = Convert.ToInt32(li.Value);
                if (li.Selected)
                {
                    arRoles.Add(iExcludedAdminID);
                }
            }

            foreach (int iAdminID in arRoles)
            {
                AddSingleFunctionAll(iAdminID, iMenuItemID);
            }

            if (ddlCurrent.SelectedIndex != 0)
            {
                LoadExcluded(iMenuItemID);
                LoadIncluded(iMenuItemID);
            }
        }

    }
    protected void btnRemoveAll_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "";
        if (lbIncluded.SelectedIndex == -1)
        {
            lblMessage.Text = "**You have not selected a role(s) to Remove, try again.";
            return;
        }
        if (rblMode.SelectedItem.Value == "1")//Add Mode...
        {

            ListItemCollection lsc = new ListItemCollection();
            for (int i = 0; i < lbIncluded.Items.Count; i++)
            {
                if (lbIncluded.Items[i].Selected == true)
                {
                    string value = lbIncluded.Items[i].Value;
                    string text = lbIncluded.Items[i].Text;
                    ListItem lst = new ListItem();
                    lst.Text = text;
                    lst.Value = value;
                    lbExcluded.Items.Add(lst);
                    lsc.Add(lst);
                }
            }
            foreach (ListItem ls in lsc)
            {
                lbIncluded.Items.Remove(ls);
            }
            rsListbox.Sort(ref lbIncluded, rsListbox.SortOrder.Ascending);
            rsListbox.Sort(ref lbExcluded, rsListbox.SortOrder.Ascending);
        }
        else//Edit Mode...
        {
            int iMenuItemID = 0;
            int iIncludedAdminID = 0;
            if (lbIncluded.Items.Count == 0)
            {
                lblMessage.Text = "**No roles to remove!";
                return;
            }

            iMenuItemID = Convert.ToInt32(ddlCurrent.SelectedValue);

            ArrayList arRoles = new ArrayList();

            foreach (ListItem li in lbIncluded.Items)
            {
                iIncludedAdminID = Convert.ToInt32(li.Value);
                if (li.Selected)
                {
                    arRoles.Add(iIncludedAdminID);
                }

            }
            foreach (int iAdminID in arRoles)
            {
                RemoveSingleFunctionAll(iAdminID, iMenuItemID);
            }


            if (ddlCurrent.SelectedIndex != 0)
            {
                int iMenuID = Convert.ToInt32(ddlCurrent.SelectedValue);

                LoadExcluded(iMenuID);
                LoadIncluded(iMenuID);
            }
        }
    }
    #endregion





}