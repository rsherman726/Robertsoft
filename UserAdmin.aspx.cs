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

public partial class UserAdmin : System.Web.UI.Page
{
    #region Subs
    private void AddUserProfile(int iUserID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sFirstName = SharedFunctions.PCase(txtFirstName.Text.Trim());
            string sMiddleInt = SharedFunctions.PCase(txtMiddleInitial.Text.Trim());
            string sLastName = SharedFunctions.PCase(txtLastName.Text.Trim());
            string sAddress = SharedFunctions.PCase(txtAddress.Text.Trim());
            string sCity = SharedFunctions.PCase(txtCity.Text.Trim());
            string sState = ddlState.SelectedValue;
            string sPostalCode = txtPostalCode.Text.Trim();
            string sPhone = txtPhone.Text.Trim();
            string sEmail = txtEmail.Text.Trim();
            string sUserName = txtUserName.Text.Trim();
            string sPassword = txtPassword.Text.Trim();
            string sRoleID = ddlRoles.SelectedValue;
            string sStatus = ddlStatus.SelectedValue;
            string sGender = ddlGender.SelectedValue;
            string sSalesperson = ddlSalespersonID.SelectedValue;
            string sDept = ddlDept.SelectedValue;
            //Clean Phone...

            sPhone = sPhone.Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Trim();
            string sMsg = "";
            //Validate data...
            if (sPhone != "")
            {
                if (SharedFunctions.IsNumeric(sPhone) == false)
                {
                    sMsg += "**Phone number contains non numeric values!<br/>";
                }
            }
            if (ddlGender.SelectedIndex == 0)
            {
                sMsg += "**Please select a gender!<br/>";

            }
            if (ddlRoles.SelectedIndex == 0)
            {
                sMsg += "**Please select a Security Role!<br/>";
            }
            if (txtPassword.Text.Length > 10)
            {
                sMsg += "**Password character limit is 10!<br/>";
            }
            if (ddlStatus.SelectedIndex == 0)
            {
                sMsg += "**Please select a Status!<br/>";
            }
            //Check to see if username already exists
            if (SharedFunctions.UserNameExists(sUserName))
            {
                sMsg += "**UserName already exists, please use another one!<br/>";
            }
            if (IsSalespersonAlreadyLinked(sSalesperson))
            {
                sMsg += "**The SysPro user you selected is already linked to another WipUserID!<br/>";
            }
            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }

            if (Session["UserID"] == null)
            {
                return;
            }


            try
            {

                WipUsers u = new WipUsers();
                u.FirstName = sFirstName;
                if (sMiddleInt != "")
                {
                    u.MiddleName = sMiddleInt;
                }
                else
                {
                    u.MiddleName = null;
                }

                u.LastName = sLastName;
                if (sAddress != "")
                {
                    u.Address = sAddress;
                }
                else
                {
                    u.Address = null;
                }
                if (sCity != "")
                {
                    u.City = sCity;
                }
                else
                {
                    u.City = null;
                }

                u.State = sState;

                if (sPostalCode != "")
                {
                    u.ZipCode = sPostalCode;
                }
                else
                {
                    u.ZipCode = null;
                }

                if (sEmail != "")
                {
                    u.Email = sEmail;
                }
                else
                {
                    u.Email = null;
                }

                if (sPhone != "")
                {
                    u.HomePhone = sPhone;
                }
                else
                {
                    u.HomePhone = null;
                }
                u.Status = Convert.ToInt32(sStatus);
                u.RoleID = Convert.ToInt32(sRoleID);
                u.UserName = sUserName;
                u.Password = sPassword;
                u.DateAdded = DateTime.Now;
                u.AddedBy = iUserID;

                u.Gender = sGender;
                if (ddlSalespersonID.SelectedIndex != 0)
                {
                    u.Salesperson = sSalesperson;
                }
                else
                {
                    u.Salesperson = null;
                }
                if (ddlDept.SelectedIndex != 0)
                {
                    u.Dept = sDept;
                }
                else
                {
                    u.Dept = null;
                }
                if (chkPicker.Checked)
                {
                    u.Picker = "Y";
                }
                else
                {
                    u.Picker = "N";
                }
                if (chkCompton.Checked)
                {
                    u.Compton = "Y";
                }
                else
                {
                    u.Compton = "N";
                }
                db.WipUsers.InsertOnSubmit(u);
                db.SubmitChanges();

                lblError.Text = "**User Added successfully!";
                lblError.ForeColor = Color.Green;

                Reset();
            }
            catch (Exception ex)
            {
                lblError.Text = "**User Added failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void UpdateProfile(int iUserID, int iSelectedUserID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            if (Page.IsValid == false)
            {
                return;
            }

            string sFirstName = SharedFunctions.PCase(txtFirstName.Text.Trim());
            string sMiddleInt = SharedFunctions.PCase(txtMiddleInitial.Text.Trim());
            string sLastName = SharedFunctions.PCase(txtLastName.Text.Trim());
            string sAddress = SharedFunctions.PCase(txtAddress.Text.Trim());
            string sCity = SharedFunctions.PCase(txtCity.Text.Trim());
            string sState = ddlState.SelectedValue;
            string sPostalCode = txtPostalCode.Text.Trim();
            string sPhone = txtPhone.Text.Trim();
            string sEmail = txtEmail.Text.Trim();
            string sUserName = txtUserName.Text.Trim();
            string sPassword = txtPassword.Text.Trim();
            string sRoleID = ddlRoles.SelectedValue;
            string sStatus = ddlStatus.SelectedValue;
            string sGender = ddlGender.SelectedValue;
            string sSalesperson = ddlSalespersonID.SelectedValue;
            string sDept = ddlDept.SelectedValue;
            //Clean Phone...

            sPhone = sPhone.Replace("-", "").Replace("(", "").Replace(")", "").Replace(".", "").Replace(" ", "").Trim();
            string sMsg = "";
            //Validate data...
            if (sPhone != "")
            {
                if (SharedFunctions.IsNumeric(sPhone) == false)
                {
                    sMsg += "**Phone number contains non numeric values!<br/>";
                }
            }
            if (ddlGender.SelectedIndex == 0)
            {
                sMsg += "**Please select a gender!<br/>";

            }
            if (ddlRoles.SelectedIndex == 0)
            {
                sMsg += "**Please select a Security Role!<br/>";
            }
            if (txtPassword.Text.Length > 10)
            {
                sMsg += "**Password character limit is 10!<br/>";
            }
            if (ddlStatus.SelectedIndex == 0)
            {
                sMsg += "**Please select a Status!<br/>";
            }
            if (IsSalespersonAlreadyLinkedForUpdate(sSalesperson, iSelectedUserID))
            {
                sMsg += "**The SysPro user you selected is already linked to another WipUserID!<br/>";
            }
            if (sMsg.Length > 0)
            {
                lblError.Text = sMsg;
                lblError.ForeColor = Color.Red;
                return;
            }

            if (Session["UserID"] == null)
            {
                return;
            }


            try
            {

                WipUsers u = db.WipUsers.Single(p => p.UserID == iSelectedUserID);
                u.FirstName = sFirstName;
                if (sMiddleInt != "")
                {
                    u.MiddleName = sMiddleInt;
                }
                else
                {
                    u.MiddleName = null;
                }

                u.LastName = sLastName;
                if (sAddress != "")
                {
                    u.Address = sAddress;
                }
                else
                {
                    u.Address = null;
                }
                if (sCity != "")
                {
                    u.City = sCity;
                }
                else
                {
                    u.City = null;
                }
                if (sState != "0")
                {
                    u.State = sState;
                }
                else
                {
                    u.State = null;
                }

                if (sPostalCode != "")
                {
                    u.ZipCode = sPostalCode;
                }
                else
                {
                    u.ZipCode = null;
                }

                if (sEmail != "")
                {
                    u.Email = sEmail;
                }
                else
                {
                    u.Email = null;
                }

                if (sPhone != "")
                {
                    u.HomePhone = sPhone;
                }
                else
                {
                    u.HomePhone = null;
                }
                u.Status = Convert.ToInt32(sStatus);
                u.RoleID = Convert.ToInt32(sRoleID);
                // u.UserName = sUserName;
                u.Password = sPassword;
                u.DateModified = DateTime.Now;
                u.ModifiedBy = iUserID;

                u.Gender = sGender;
                if (ddlSalespersonID.SelectedIndex != 0)
                {
                    u.Salesperson = sSalesperson;
                }
                else
                {
                    u.Salesperson = null;
                }
                if (ddlDept.SelectedIndex != 0)
                {
                    u.Dept = sDept;
                }
                else
                {
                    u.Dept = null;
                }
                if (chkPicker.Checked)
                {
                    u.Picker = "Y";
                }
                else
                {
                    u.Picker = "N";
                }
                if (chkCompton.Checked)
                {
                    u.Compton = "Y";
                }
                else
                {
                    u.Compton = "N";
                }
                db.SubmitChanges();


                BindProfile(iSelectedUserID);
                List<string> lStatus = new List<string>();
                foreach (ListItem li in cblStatus.Items)
                {
                    if (li.Selected)
                    {
                        lStatus.Add(li.Value);
                    }
                }
                LoadUserList(txtSearch.Text.Trim(), lStatus);

                lbUsers.SelectedValue = iSelectedUserID.ToString();


                lblError.Text = "**Profile updated successfully!";
                lblError.ForeColor = Color.Green;

            }
            catch (Exception ex)
            {
                lblError.Text = "**Profile updated failed!";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void DeleteProfile(int iUserID, int iSelectedUserID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {
                WipUsers u = db.WipUsers.Single(p => p.UserID == iSelectedUserID);
                db.WipUsers.DeleteOnSubmit(u);
                db.SubmitChanges();

                lblError.Text = "**User Deleted successfully!";
                lblError.ForeColor = Color.Green;

                Reset();
            }
            catch (Exception ex)
            {
                lblError.Text = "**User Delete failed!(You can't delete a user who is associated with another table.e.g. Orders)";
                lblError.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void BindProfile(int iUserID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {

            var query = (from users in db.WipUsers
                         where users.UserID == iUserID
                         select users);
            foreach (var a in query)
            {
                ddlGender.SelectedValue = a.Gender;
                if (a.State != null)
                {
                    ddlState.SelectedValue = a.State;
                }
                else
                {
                    ddlState.SelectedValue = "CA";
                }

                txtFirstName.Text = a.FirstName;
                txtMiddleInitial.Text = a.MiddleName;
                txtLastName.Text = a.LastName;
                txtAddress.Text = a.Address;
                txtCity.Text = a.City;

                if (a.HomePhone != null)
                {
                    if (a.HomePhone.Trim().Length > 9)
                    {
                        txtPhone.Text = SharedFunctions.GetPhoneFormat(a.HomePhone);
                    }
                }

                txtPostalCode.Text = a.ZipCode;
                txtEmail.Text = a.Email;
                txtUserName.Text = a.UserName;
                txtPassword.Text = a.Password;
                ddlRoles.SelectedValue = a.RoleID.ToString();
                ddlStatus.SelectedValue = a.Status.ToString();
                if (a.Salesperson != null)
                {
                    ddlSalespersonID.SelectedValue = a.Salesperson.Trim();
                }
                else
                {
                    ddlSalespersonID.SelectedIndex = 0;
                }
                if (a.Dept != null)
                {
                    ddlDept.SelectedValue = a.Dept.Trim();
                }
                else
                {
                    ddlDept.SelectedIndex = 0;
                }

                if (a.Picker != null)
                {
                    if (a.Picker == "Y")
                    {
                        chkPicker.Checked = true;
                    }
                    else
                    {
                        chkPicker.Checked = false;
                    }
                }
                if (a.Compton != null)
                {
                    if (a.Compton == "Y")
                    {
                        chkCompton.Checked = true;
                    }
                    else
                    {
                        chkCompton.Checked = false;
                    }
                }

                if (a.UserName == "rsherman")//ME
                {
                    if (Session["UserName"].ToString() != "rsherman")//Signed User is not ME...
                    {
                        txtPassword.BackColor = Color.Black;
                        txtUserName.BackColor = Color.Black;
                        ibnSave.Enabled = false;
                    }
                    else
                    {
                        txtPassword.BackColor = Color.WhiteSmoke;
                        txtUserName.BackColor = Color.WhiteSmoke;
                        ibnSave.Enabled = true;
                    }
                }
                else
                {
                    txtPassword.BackColor = Color.WhiteSmoke;
                    txtUserName.BackColor = Color.WhiteSmoke;
                    ibnSave.Enabled = true;
                }

            }
        }
    }
    private void LoadRolesForCheckboxlist()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            cblRoles.Items.Clear();

            var query = (from r in db.WipUsersRoles
                         select r);
            foreach (var a in query)
            {
                cblRoles.Items.Add(new ListItem("&nbsp;" + a.Role, a.RoleID.ToString()));
            }



            foreach (ListItem li in cblRoles.Items)
            {
                li.Selected = true;
            }
        }
    }
    private void LoadUserList(string sSearch, List<string> lStatus)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sFullName = sSearch;
            string sFirstName = null;
            string sMiddleName = null;
            string sLastName = null;
            int iCount = 0;
            string[] name = sFullName.Split(' ');
            sFirstName = name[0].ToString();
            switch (name.Length)
            {
                case 1://Just First Name
                    sFirstName = name[0].ToString().ToUpper();
                    break;
                case 2://No Middle name
                    sFirstName = name[0].ToString().ToUpper();
                    sLastName = name[1].ToString().ToUpper();
                    break;
                case 3:
                    sFirstName = name[0].ToString().ToUpper();
                    sMiddleName = name[1].ToString().ToUpper();
                    sLastName = name[2].ToString().ToUpper();
                    break;
            }

            List<string> Roles = new List<string>();
            foreach (ListItem li in cblRoles.Items)
            {
                if (li.Selected)
                {
                    Roles.Add(li.Value);
                    iCount++;
                }
            }


            lbUsers.Items.Clear();
            var query = (from users in db.WipUsers
                         orderby users.FirstName, users.LastName
                         where Roles.Contains(users.RoleID.ToString()) && lStatus.Contains(users.Status.ToString()) &&
                          (users.FirstName.ToUpper().Contains(sFirstName) || sFirstName == null)
                          &&
                          (users.MiddleName.ToUpper().Contains(sMiddleName) || sMiddleName == null)
                          &&
                          (users.LastName.ToUpper().Contains(sLastName) || sLastName == null)
                         select new
                         {
                             users.FirstName,
                             users.MiddleName,
                             users.LastName,
                             users.UserID,
                             users.WipUsersRoles.Role,
                             users.Status,
                         });
            if (query.Count() > 0)
            {
                string sStatus = "";
                foreach (var a in query)
                {
                    if (a.Status == 1)
                    {
                        sStatus = "Active";
                    }
                    else
                    {
                        sStatus = "Not Active";
                    }
                    sFullName = a.FirstName + " " + (a.MiddleName ?? "") + " " + a.LastName;
                    sFullName = sFullName.Replace("  ", " ") + " (" + a.UserID.ToString() + ") - " + a.Role + " - " + sStatus;
                    lbUsers.Items.Add(new ListItem(sFullName, a.UserID.ToString()));
                }
            }
        }
    }
    private void LoadRoles()
    {
        ddlRoles.Items.Clear();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from r in db.WipUsersRoles
                         select r);
            foreach (var a in query)
            {
                ddlRoles.Items.Add(new ListItem(a.Role, a.RoleID.ToString()));
            }
            ddlRoles.Items.Insert(0, new ListItem("--Select a Role--", "0"));
        }
    }
    private void Reset()
    {
        txtFirstName.Text = "";
        txtMiddleInitial.Text = "";
        txtLastName.Text = "";
        txtAddress.Text = "";
        txtCity.Text = "";
        ddlState.SelectedValue = "CA";
        txtPostalCode.Text = "";
        txtPhone.Text = "";
        txtEmail.Text = "";
        txtUserName.Text = "";
        txtPassword.Text = "";
        ddlRoles.SelectedIndex = 0;
        ddlStatus.SelectedIndex = 0;
        ddlGender.SelectedIndex = 0;
        ddlSalespersonID.SelectedIndex = 0;
        ddlDept.SelectedIndex = 0;
        chkPicker.Checked = false;
        chkCompton.Checked = false;
    }
    private void LoadSalespersons()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddlSalespersonID.Items.Clear();

            var query = (from r in db.SalSalesperson
                         orderby r.Name
                         select r);
            foreach (var a in query)
            {
                ddlSalespersonID.Items.Add(new ListItem(a.Name.Trim() + " - " + a.Salesperson.Trim(), a.Salesperson.Trim()));
            }
            ddlSalespersonID.Items.Insert(0, new ListItem("Select a Salesperson", "0"));
        }
    }

    //Message Groups...
    private void AddMessageGroup()
    {
        lblErrorMessageGroup.Text = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            string sGroupName = txtGroupName.Text.Trim();

            if (GroupNameExists(sGroupName) == true)
            {
                sMsg = "**Group name already exists, please use another name!";
            }
            if (sGroupName == "")
            {
                sMsg = "**Group name is required!";
            }
            if (sMsg.Length > 0)
            {
                lblErrorMessageGroup.Text = sMsg;
                lblErrorMessageGroup.ForeColor = Color.Red;
                return;
            }

            try
            {

                WipMessageGroups g = new WipMessageGroups();
                g.GroupName = sGroupName.ToUpper();
                g.DateAdded = DateTime.Now;
                db.WipMessageGroups.InsertOnSubmit(g);
                db.SubmitChanges();

                lblErrorMessageGroup.Text = "**Message Group Added successfully!";
                lblErrorMessageGroup.ForeColor = Color.Green;

                LoadMessageGroupsList();
                LoadMessageGroups();
                ResetMessageGroup();
            }
            catch (Exception ex)
            {
                lblErrorMessageGroup.Text = "**Message Group Added failed!";
                lblErrorMessageGroup.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void UpdateMessageGroup(int iWipMessageGroupID)
    {
        lblErrorMessageGroup.Text = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";

            if (Page.IsValid == false)
            {
                return;
            }

            string sGroupName = txtGroupName.Text.Trim().ToUpper();
            if (sMsg.Length > 0)
            {
                lblErrorMessageGroup.Text = sMsg;
                lblErrorMessageGroup.ForeColor = Color.Red;
                return;
            }

            try
            {

                WipMessageGroups pl = db.WipMessageGroups.Single(p => p.WipMessageGroupID == iWipMessageGroupID);
                pl.GroupName = sGroupName.ToUpper();
                db.SubmitChanges();

                BindMessageGroup(iWipMessageGroupID);
                LoadMessageGroupsList();
                lbMessageGroups.SelectedValue = iWipMessageGroupID.ToString();

                LoadMessageGroups();

                lblErrorMessageGroup.Text = "**Message Group updated successfully!";
                lblErrorMessageGroup.ForeColor = Color.Green;

            }
            catch (Exception ex)
            {
                lblErrorMessageGroup.Text = "**Message Group updated failed!";
                lblErrorMessageGroup.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void DeleteMessageGroup(int iWipMessageGroupID)
    {
        lblErrorMessageGroup.Text = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {
                WipMessageGroups pl = db.WipMessageGroups.Single(p => p.WipMessageGroupID == iWipMessageGroupID);
                db.WipMessageGroups.DeleteOnSubmit(pl);
                db.SubmitChanges();

                lblErrorMessageGroup.Text = "**Message Group Deleted successfully!";
                lblErrorMessageGroup.ForeColor = Color.Green;
                LoadMessageGroupsList();
                LoadMessageGroups();
                Reset();
            }
            catch (Exception ex)
            {
                lblErrorMessageGroup.Text = "**Message Group Delete failed!(You can't delete a user who is associated with another table.e.g. Orders)";
                lblErrorMessageGroup.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void BindMessageGroup(int iWipMessageGroupID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from w in db.WipMessageGroups
                     where w.WipMessageGroupID == iWipMessageGroupID
                     select w);
        foreach (var a in query)
        {
            txtGroupName.Text = a.GroupName;
        }
    }
    private void LoadMessageGroupsList()
    {

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbMessageGroups.Items.Clear();
            var query = (from g in db.WipMessageGroups
                         orderby g.GroupName
                         select new
                         {
                             g.GroupName,
                             g.WipMessageGroupID
                         });
            foreach (var a in query)
            {
                lbMessageGroups.Items.Add(new ListItem(a.GroupName, a.WipMessageGroupID.ToString()));
            }
        }
    }
    private void ResetMessageGroup()
    {
        txtGroupName.Text = "";
    }

    //Assign Message Group...
    private void LoadMessageGroups()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddlMessageGroups.Items.Clear();
            var query = (from g in db.WipMessageGroups
                         orderby g.GroupName
                         select new
                         {
                             g.WipMessageGroupID,
                             g.GroupName
                         });
            foreach (var a in query)
            {
                ddlMessageGroups.Items.Add(new ListItem(a.GroupName + " - " + a.WipMessageGroupID.ToString(), a.WipMessageGroupID.ToString()));
            }
            ddlMessageGroups.Items.Insert(0, new ListItem("Select a Message Group", "0"));
        }
    }
    private void LoadAssignedEmployees(int iWipMessageGroupID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbAssignedEmployees.Items.Clear();
            var query = (from ea in db.WipMessageGroupAssignments
                         where
                           ea.WipMessageGroupID == iWipMessageGroupID
                         orderby ea.WipUsers.FirstName, ea.WipUsers.LastName
                         select new
                         {
                             ea.WipUserID,
                             EvenOdd = ea.SubGroupOddEven == 1 ? "EVEN ORDERS" : ea.SubGroupOddEven == 2 ? "ODD ORDERS" : "",
                             FullName = (ea.WipUsers.FirstName + " " + (ea.WipUsers.MiddleName ?? "") + " " + ea.WipUsers.LastName).Replace("  ", " ")
                         });
            foreach (var a in query)
            {
                if (a.EvenOdd != "")
                {
                    lbAssignedEmployees.Items.Add(new ListItem(a.FullName + " - " + a.WipUserID.ToString() + " " + a.EvenOdd, a.WipUserID.ToString()));
                }
                else
                {
                    lbAssignedEmployees.Items.Add(new ListItem(a.FullName + " - " + a.WipUserID.ToString(), a.WipUserID.ToString()));
                }
            }
        }
    }
    private void LoadAvailableEmployees(int iWipMessageGroupID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbAvailableEmployees.Items.Clear();
            var query = (from u in db.WipUsers
                         where
                           !
                             (from mga in db.WipMessageGroupAssignments
                              where
                                  mga.WipMessageGroupID == iWipMessageGroupID
                              select new
                              {
                                  mga.WipUserID
                              }).Contains(new { WipUserID = u.UserID })
                              && u.Status != 0
                         orderby
                           u.FirstName,
                           u.LastName
                         select new
                         {
                             FullName = (u.FirstName + " " + (u.MiddleName ?? "") + " " + u.LastName).Replace("  ", " "),
                             u.UserID
                         });
            foreach (var a in query)
            {
                lbAvailableEmployees.Items.Add(new ListItem(a.FullName + " - " + a.UserID.ToString(), a.UserID.ToString()));
            }
        }
    }
    private void AddOne()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lblErrorAssignGroup.Text = "";
            int iManagerID = 0;
            if (Session["UserID"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            iManagerID = Convert.ToInt32(Session["UserID"]);
            int iWipMessageGroupID = 0;
            if (ddlMessageGroups.SelectedIndex == 0)
            {
                lblErrorAssignGroup.Text = "**You have not selected a Message Group, try again.";
                return;
            }
            iWipMessageGroupID = Convert.ToInt32(ddlMessageGroups.SelectedValue);
            if (lbAvailableEmployees.SelectedIndex == -1)
            {
                lblErrorAssignGroup.Text = "**You have not selected a User(s) to Add, try again.";
                return;
            }
            try
            {
                ListItemCollection lsc = new ListItemCollection();
                for (int i = 0; i < lbAvailableEmployees.Items.Count; i++)
                {
                    if (lbAvailableEmployees.Items[i].Selected == true)
                    {
                        string value = lbAvailableEmployees.Items[i].Value;
                        string text = lbAvailableEmployees.Items[i].Text;
                        ListItem lst = new ListItem();
                        lst.Text = text;
                        lst.Value = value;
                        lbAssignedEmployees.Items.Add(lst);
                        lsc.Add(lst);
                    }
                }
                foreach (ListItem ls in lsc)
                {

                }
                //Remove Dups...
                SharedFunctions.RemoveDupsFromListBox(lbAssignedEmployees);

                //Add all items in Assigned listbox...

                int iWipUserID = 0;
                foreach (ListItem li in lbAssignedEmployees.Items)
                {
                    iWipUserID = Convert.ToInt32(li.Value);
                    if (!RecordAlreadyinTable(iWipUserID, iWipMessageGroupID))
                    {
                        WipMessageGroupAssignments ea = new WipMessageGroupAssignments();
                        ea.WipUserID = iWipUserID;
                        ea.WipMessageGroupID = iWipMessageGroupID;
                        ea.AddedBy = iManagerID;
                        ea.DateAdded = DateTime.Now;
                        if (rblEvenOdd.SelectedIndex == 1)
                        {//Even...
                            ea.SubGroupOddEven = 1;
                        }
                        else if (rblEvenOdd.SelectedIndex == 2)
                        {//Odd..
                            ea.SubGroupOddEven = 2;
                        }
                        else
                        {
                            ea.SubGroupOddEven = null;
                        }
                        db.WipMessageGroupAssignments.InsertOnSubmit(ea);
                        db.SubmitChanges();
                    }
                }
                LoadAssignedEmployees(iWipMessageGroupID);
                LoadAvailableEmployees(iWipMessageGroupID);
                rsListbox.Sort(ref lbAssignedEmployees, rsListbox.SortOrder.Ascending);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void RemoveOne()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lblError.Text = "";
            int iWipMessageGroupID = 0;
            if (ddlMessageGroups.SelectedIndex == 0)
            {
                return;
            }
            iWipMessageGroupID = Convert.ToInt32(ddlMessageGroups.SelectedValue);

            //Delete all items in Assigned listbox... 
            try
            {
                int iWipUserID = 0;
                foreach (ListItem li in lbAssignedEmployees.Items)
                {
                    iWipUserID = Convert.ToInt32(li.Value);
                    //Delete Single Agent from Group Assignments...
                    if (li.Selected)
                    {

                        WipMessageGroupAssignments ea = db.WipMessageGroupAssignments.Single(p => p.WipUserID == iWipUserID && p.WipMessageGroupID == iWipMessageGroupID);
                        db.WipMessageGroupAssignments.DeleteOnSubmit(ea);
                        db.SubmitChanges();
                    }

                }
                LoadAssignedEmployees(iWipMessageGroupID);
                LoadAvailableEmployees(iWipMessageGroupID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }


    //Security Groups...
    private void AddSecurityGroup()
    {
        lblErrorSecurityGroup.Text = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";
            string sGroupName = txtSecurityGroupName.Text.Trim();

            if (GroupNameExistsSG(sGroupName) == true)
            {
                sMsg = "**Group name already exists, please use another name!";
            }
            if (sGroupName == "")
            {
                sMsg = "**Group name is required!";
            }
            if (sMsg.Length > 0)
            {
                lblErrorSecurityGroup.Text = sMsg;
                lblErrorSecurityGroup.ForeColor = Color.Red;
                return;
            }

            try
            {

                WipSecurityGroups g = new WipSecurityGroups();
                g.GroupName = sGroupName.ToUpper();
                g.DateAdded = DateTime.Now;
                db.WipSecurityGroups.InsertOnSubmit(g);
                db.SubmitChanges();

                lblErrorSecurityGroup.Text = "**Security Group Added successfully!";
                lblErrorSecurityGroup.ForeColor = Color.Green;

                LoadSecurityGroupsList();
                LoadSecurityGroups();
                ResetSecurityGroup();
            }
            catch (Exception ex)
            {
                lblErrorSecurityGroup.Text = "**Security Group Added failed!";
                lblErrorSecurityGroup.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void UpdateSecurityGroup(int iWipSecurityGroupID)
    {
        lblErrorSecurityGroup.Text = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMsg = "";

            if (Page.IsValid == false)
            {
                return;
            }

            string sGroupName = txtSecurityGroupName.Text.Trim().ToUpper();
            if (sMsg.Length > 0)
            {
                lblErrorSecurityGroup.Text = sMsg;
                lblErrorSecurityGroup.ForeColor = Color.Red;
                return;
            }

            try
            {

                WipSecurityGroups pl = db.WipSecurityGroups.Single(p => p.WipSecurityGroupID == iWipSecurityGroupID);
                pl.GroupName = sGroupName.ToUpper();
                db.SubmitChanges();

                BindSecurityGroup(iWipSecurityGroupID);
                LoadSecurityGroupsList();
                lbSecurityGroups.SelectedValue = iWipSecurityGroupID.ToString();

                LoadSecurityGroups();

                lblErrorSecurityGroup.Text = "**Security Group updated successfully!";
                lblErrorSecurityGroup.ForeColor = Color.Green;

            }
            catch (Exception ex)
            {
                lblErrorSecurityGroup.Text = "**Security Group updated failed!";
                lblErrorSecurityGroup.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void DeleteSecurityGroup(int iWipSecurityGroupID)
    {
        lblErrorSecurityGroup.Text = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {
                WipSecurityGroups pl = db.WipSecurityGroups.Single(p => p.WipSecurityGroupID == iWipSecurityGroupID);
                db.WipSecurityGroups.DeleteOnSubmit(pl);
                db.SubmitChanges();

                lblErrorSecurityGroup.Text = "**Security Group Deleted successfully!";
                lblErrorSecurityGroup.ForeColor = Color.Green;
                LoadSecurityGroupsList();
                LoadSecurityGroups();
                Reset();
            }
            catch (Exception ex)
            {
                lblErrorSecurityGroup.Text = "**Security Group Delete failed!(You can't delete a user who is associated with another table.e.g. Orders)";
                lblErrorSecurityGroup.ForeColor = Color.Red;
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void BindSecurityGroup(int iWipSecurityGroupID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);

        var query = (from w in db.WipSecurityGroups
                     where w.WipSecurityGroupID == iWipSecurityGroupID
                     select w);
        foreach (var a in query)
        {
            txtSecurityGroupName.Text = a.GroupName;
        }
    }
    private void LoadSecurityGroupsList()
    {

        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbSecurityGroups.Items.Clear();
            var query = (from g in db.WipSecurityGroups
                         orderby g.GroupName
                         select new
                         {
                             g.GroupName,
                             g.WipSecurityGroupID
                         });
            foreach (var a in query)
            {
                lbSecurityGroups.Items.Add(new ListItem(a.GroupName, a.WipSecurityGroupID.ToString()));
            }
        }
    }
    private void ResetSecurityGroup()
    {
        txtSecurityGroupName.Text = "";
    }

    //Assign Security Group...
    private void LoadSecurityGroups()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            ddlSecurityGroups.Items.Clear();
            var query = (from g in db.WipSecurityGroups
                         orderby g.GroupName
                         select new
                         {
                             g.WipSecurityGroupID,
                             g.GroupName
                         });
            foreach (var a in query)
            {
                ddlSecurityGroups.Items.Add(new ListItem(a.GroupName + " - " + a.WipSecurityGroupID.ToString(), a.WipSecurityGroupID.ToString()));
            }
            ddlSecurityGroups.Items.Insert(0, new ListItem("Select a Security Group", "0"));
        }
    }
    private void LoadAssignedEmployeesSG(int iWipSecurityGroupID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbAssignedEmployeesSG.Items.Clear();
            var query = (from ea in db.WipSecurityGroupAssignments
                         where
                           ea.WipSecurityGroupID == iWipSecurityGroupID
                         orderby ea.WipUsers.FirstName, ea.WipUsers.LastName
                         select new
                         {
                             ea.WipUserID,
                             EvenOdd = ea.SubGroupOddEven == 1 ? "EVEN ORDERS" : ea.SubGroupOddEven == 2 ? "ODD ORDERS" : "",
                             FullName = (ea.WipUsers.FirstName + " " + (ea.WipUsers.MiddleName ?? "") + " " + ea.WipUsers.LastName).Replace("  ", " ")
                         });
            foreach (var a in query)
            {
                if (a.EvenOdd != "")
                {
                    lbAssignedEmployeesSG.Items.Add(new ListItem(a.FullName + " - " + a.WipUserID.ToString() + " " + a.EvenOdd, a.WipUserID.ToString()));
                }
                else
                {
                    lbAssignedEmployeesSG.Items.Add(new ListItem(a.FullName + " - " + a.WipUserID.ToString(), a.WipUserID.ToString()));
                }
            }
        }
    }
    private void LoadAvailableEmployeesSG(int iWipSecurityGroupID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lbAvailableEmployeesSG.Items.Clear();
            var query = (from u in db.WipUsers
                         where
                           !
                             (from mga in db.WipSecurityGroupAssignments
                              where
                                  mga.WipSecurityGroupID == iWipSecurityGroupID
                              select new
                              {
                                  mga.WipUserID
                              }).Contains(new { WipUserID = u.UserID })
                              && u.Status != 0
                         orderby
                           u.FirstName,
                           u.LastName
                         select new
                         {
                             FullName = (u.FirstName + " " + (u.MiddleName ?? "") + " " + u.LastName).Replace("  ", " "),
                             u.UserID
                         });
            foreach (var a in query)
            {
                lbAvailableEmployeesSG.Items.Add(new ListItem(a.FullName + " - " + a.UserID.ToString(), a.UserID.ToString()));
            }
        }
    }
    private void AddOneSG()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lblErrorAssignSecurityGroup.Text = "";
            int iManagerID = 0;
            if (Session["UserID"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            iManagerID = Convert.ToInt32(Session["UserID"]);
            int iWipSecurityGroupID = 0;
            if (ddlSecurityGroups.SelectedIndex == 0)
            {
                lblErrorAssignSecurityGroup.Text = "**You have not selected a Security Group, try again.";
                return;
            }
            iWipSecurityGroupID = Convert.ToInt32(ddlSecurityGroups.SelectedValue);
            if (lbAvailableEmployeesSG.SelectedIndex == -1)
            {
                lblErrorAssignSecurityGroup.Text = "**You have not selected a User(s) to Add, try again.";
                return;
            }
            try
            {
                ListItemCollection lsc = new ListItemCollection();
                for (int i = 0; i < lbAvailableEmployeesSG.Items.Count; i++)
                {
                    if (lbAvailableEmployeesSG.Items[i].Selected == true)
                    {
                        string value = lbAvailableEmployeesSG.Items[i].Value;
                        string text = lbAvailableEmployeesSG.Items[i].Text;
                        ListItem lst = new ListItem();
                        lst.Text = text;
                        lst.Value = value;
                        lbAssignedEmployeesSG.Items.Add(lst);
                        lsc.Add(lst);
                    }
                }
                foreach (ListItem ls in lsc)
                {

                }
                //Remove Dups...
                SharedFunctions.RemoveDupsFromListBox(lbAssignedEmployeesSG);

                //Add all items in Assigned listbox...

                int iWipUserID = 0;
                foreach (ListItem li in lbAssignedEmployeesSG.Items)
                {
                    iWipUserID = Convert.ToInt32(li.Value);
                    if (!RecordAlreadyinTableSG(iWipUserID, iWipSecurityGroupID))
                    {
                        WipSecurityGroupAssignments ea = new WipSecurityGroupAssignments();
                        ea.WipUserID = iWipUserID;
                        ea.WipSecurityGroupID = iWipSecurityGroupID;
                        ea.AddedBy = iManagerID;
                        ea.DateAdded = DateTime.Now;
                        if (rblEvenOdd.SelectedIndex == 1)
                        {//Even...
                            ea.SubGroupOddEven = 1;
                        }
                        else if (rblEvenOdd.SelectedIndex == 2)
                        {//Odd..
                            ea.SubGroupOddEven = 2;
                        }
                        else
                        {
                            ea.SubGroupOddEven = null;
                        }
                        db.WipSecurityGroupAssignments.InsertOnSubmit(ea);
                        db.SubmitChanges();
                    }
                }
                LoadAssignedEmployeesSG(iWipSecurityGroupID);
                LoadAvailableEmployeesSG(iWipSecurityGroupID);
                rsListbox.Sort(ref lbAssignedEmployeesSG, rsListbox.SortOrder.Ascending);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    private void RemoveOneSG()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            lblError.Text = "";
            int iWipSecurityGroupID = 0;
            if (ddlSecurityGroups.SelectedIndex == 0)
            {
                return;
            }
            iWipSecurityGroupID = Convert.ToInt32(ddlSecurityGroups.SelectedValue);

            //Delete all items in Assigned listbox... 
            try
            {
                int iWipUserID = 0;
                foreach (ListItem li in lbAssignedEmployeesSG.Items)
                {
                    iWipUserID = Convert.ToInt32(li.Value);
                    //Delete Single Agent from Group Assignments...
                    if (li.Selected)
                    {

                        WipSecurityGroupAssignments ea = db.WipSecurityGroupAssignments.Single(p => p.WipUserID == iWipUserID && p.WipSecurityGroupID == iWipSecurityGroupID);
                        db.WipSecurityGroupAssignments.DeleteOnSubmit(ea);
                        db.SubmitChanges();
                    }

                }
                LoadAssignedEmployeesSG(iWipSecurityGroupID);
                LoadAvailableEmployeesSG(iWipSecurityGroupID);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
    #endregion

    #region Functions
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
    private bool IsSalespersonAlreadyLinked(string sSalesperson)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from u in db.WipUsers
                         where u.Salesperson == sSalesperson
                         select u);
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
    private bool IsSalespersonAlreadyLinkedForUpdate(string sSalesperson, int iUserID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from u in db.WipUsers
                         where u.Salesperson == sSalesperson
                         && u.UserID != iUserID
                         select u);
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
    //Message Groups...
    public static bool GroupNameExists(string sGroupName)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from g in db.WipMessageGroups
                         where g.GroupName == sGroupName
                         select g);
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
    }
    public static bool GroupNameExistsSG(string sGroupName)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from g in db.WipSecurityGroups
                         where g.GroupName == sGroupName
                         select g);
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
    }
    private bool RecordAlreadyinTable(int iWipUserID, int iWipMessageGroupID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from ea in db.WipMessageGroupAssignments
                         where ea.WipUserID == iWipUserID
                         && ea.WipMessageGroupID == iWipMessageGroupID
                         select ea);
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
    }
    private bool RecordAlreadyinTableSG(int iWipUserID, int iWipSecurityGroupID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from ea in db.WipSecurityGroupAssignments
                         where ea.WipUserID == iWipUserID
                         && ea.WipSecurityGroupID == iWipSecurityGroupID
                         select ea);
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
            LoadRolesForCheckboxlist();
            LoadRoles();
            LoadSalespersons();
            ibnAdd.Enabled = false;
            ibnDelete.Enabled = false;
            ibnSave.Enabled = false;
            List<string> lStatus = new List<string>();
            foreach (ListItem li in cblStatus.Items)
            {
                if (li.Selected)
                {
                    lStatus.Add(li.Value);
                }
            }
            LoadUserList(txtSearch.Text.Trim(), lStatus);
            LoadMessageGroupsList();
            LoadMessageGroups();

            LoadSecurityGroupsList();
            LoadSecurityGroups();
        }

    }
    protected void cblRoles_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        Reset();
        lbUsers.Items.Clear();
        List<string> lStatus = new List<string>();
        foreach (ListItem li in cblStatus.Items)
        {
            if (li.Selected)
            {
                lStatus.Add(li.Value);
            }
        }

        LoadUserList(txtSearch.Text.Trim(), lStatus);
    }
    protected void ibnSave_Click(object sender, EventArgs e)
    {
        //Save Profile...
        lblError.Text = "";
        int iUserID = 0;
        int iSelectedUserID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (lbUsers.SelectedIndex == -1)
        {
            lblError.Text = "**Please selected a user.";
            lblError.ForeColor = Color.Red;
            return;
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        iSelectedUserID = Convert.ToInt32(lbUsers.SelectedValue);
        UpdateProfile(iUserID, iSelectedUserID);
    }
    protected void ibnAdd_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iUserID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        if (Page.IsValid == false)
        {
            return;
        }

        AddUserProfile(iUserID);
    }
    protected void ibnDelete_Click(object sender, EventArgs e)
    {//Delete Profile...
        lblError.Text = "";
        int iUserID = 0;
        int iSelectedUserID = 0;
        if (Session["UserID"] == null)
        {
            Response.Redirect("Default.aspx");
        }

        if (lbUsers.SelectedIndex == -1)
        {
            lblError.Text = "**Please selected a user.";
            lblError.ForeColor = Color.Red;
            return;
        }
        iUserID = Convert.ToInt32(Session["UserID"]);
        iSelectedUserID = Convert.ToInt32(lbUsers.SelectedValue);
        DeleteProfile(iUserID, iSelectedUserID);
    }
    protected void ibnSearch_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        List<string> lStatus = new List<string>();
        foreach (ListItem li in cblStatus.Items)
        {
            if (li.Selected)
            {
                lStatus.Add(li.Value);
            }
        }
        LoadUserList(txtSearch.Text.Trim(), lStatus);
    }
    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        List<string> lStatus = new List<string>();
        foreach (ListItem li in cblStatus.Items)
        {
            if (li.Selected)
            {
                lStatus.Add(li.Value);
            }
        }
        LoadUserList(txtSearch.Text.Trim(), lStatus);
    }
    protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iUserID = 0;
        if (lbUsers.SelectedIndex != -1)
        {
            iUserID = Convert.ToInt32(lbUsers.SelectedValue);
            ibnSave.Enabled = true;
            ibnDelete.Enabled = true;
            BindProfile(iUserID);
            lbnLoginAs.Visible = true;
        }
        else
        {
            Reset();
            ibnSave.Enabled = false;
            ibnDelete.Enabled = false;
            lbnLoginAs.Visible = false;
        }
    }
    protected void rblMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblError.Text = "";
        int iUserID = 0;
        if (rblMode.SelectedIndex == 0)//Add...
        {
            Reset();
            ibnAdd.Enabled = true;
            ibnSave.Enabled = false;
            ibnDelete.Enabled = false;
            lbnLoginAs.Visible = false;
            lbUsers.Visible = false;
            txtSearch.Visible = false;
            ibnSearch.Visible = false;
            txtUserName.Enabled = true;
            lblView.Visible = false;
            cblStatus.Visible = false;
            cblRoles.Visible = false;
        }
        else//Edit...
        {
            ibnAdd.Enabled = false;
            lbUsers.Visible = true;
            txtSearch.Visible = true;
            ibnSearch.Visible = true;
            lblView.Visible = true;
            cblStatus.Visible = true;
            cblRoles.Visible = true;
            LoadSalespersons();
            if (lbUsers.SelectedIndex != -1)
            {
                iUserID = Convert.ToInt32(lbUsers.SelectedValue);
                ibnSave.Enabled = true;
                ibnDelete.Enabled = true;
                BindProfile(iUserID);
            }
            else
            {
                Reset();
                ibnSave.Enabled = false;
                ibnDelete.Enabled = false;
            }

        }
    }
    protected void lbnLoginAs_Click(object sender, EventArgs e)
    {
        lblError.Text = "";

        if (lbUsers.SelectedIndex == -1)
        {
            lblError.Text = "**Please selected a user.";
            lblError.ForeColor = Color.Red;
            return;
        }

        string sResult = "";
        string sUserName = "";
        string sPassword = "";

        sUserName = txtUserName.Text.Trim();
        sPassword = txtPassword.Text.Trim();

        if (sPassword == "" || sUserName == "")
        {
            lblError.Text = "**UserName and/or Password can not be left blank, try again.";
            return;
        }

        if (ddlStatus.SelectedValue == "0")
        {
            lblError.Text = "**You can only login to active accounts!!";
            lblError.ForeColor = Color.Red;
            return;
        }

        sResult = CheckLogin(sUserName, sPassword);//Change the user....

        if (sResult != "")
        {
            lblError.Text = sResult;

        }
        else
        {

            //Redirect to either Admin or Merchant back office...
            //if Role =1 then (go to Admin back office) else if Role = 2 then (go to User Section)

            if (Session["UserID"] != null)
            {

                int iUserID = Convert.ToInt32(Session["UserID"]);



                //if (Server.MachineName.ToUpper() == "POWERHOUSE")
                //{

                switch (Convert.ToInt32(Session["RoleID"]))
                {
                    //case 1://Admin
                    //    Response.Redirect("Admin.aspx");
                    //    break;
                    //case 2://Customer
                    //    Response.Redirect("CustomerHome.aspx");
                    //    break;
                    //case 3://Card Holder
                    //    Response.Redirect("CardHolderHome.aspx");
                    //    break;
                    //case 4://Accountant...
                    //    Response.Redirect("AccountantHome.aspx");
                    //    break;
                    //case 5://Processor...
                    //    Response.Redirect("ProcessorHome.aspx");
                    //    break;
                    //case 6://Broker
                    //    Response.Redirect("BrokerHome.aspx");
                    //    break;
                    //case 7://Salesperson
                    //    Response.Redirect("SalesHome.aspx");
                    //    break;
                    default:
                        Response.Redirect("Default.aspx");
                        break;
                }

            }
        }
    }
    protected void cblStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        List<string> lStatus = new List<string>();
        foreach (ListItem li in cblStatus.Items)
        {
            if (li.Selected)
            {
                lStatus.Add(li.Value);
            }
        }
        LoadUserList(txtSearch.Text.Trim(), lStatus);
    }
    //Message Groups...
    protected void lbMessageGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        int iWipMessageGroupID = 0;
        if (lbMessageGroups.SelectedIndex != -1)
        {
            iWipMessageGroupID = Convert.ToInt32(lbMessageGroups.SelectedValue);
            btnSaveGroup.Enabled = true;
            btnDeleteGroup.Enabled = true;
            BindMessageGroup(iWipMessageGroupID);
        }
        else
        {
            Reset();
            btnSaveGroup.Enabled = false;
            btnDeleteGroup.Enabled = false;
        }
    }
    protected void rblModeGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrorMessageGroup.Text = "";
        int iWipMessageGroupID = 0;
        if (rblModeGroup.SelectedIndex == 0)//Add...
        {
            ResetMessageGroup();
            btnAddGroup.Enabled = true;
            btnSaveGroup.Enabled = false;
            btnDeleteGroup.Enabled = false;
            lbMessageGroups.Enabled = false;
            lbMessageGroups.BackColor = Color.LightGray;
        }
        else//Edit...
        {
            btnAddGroup.Enabled = false;
            lbMessageGroups.Enabled = true;
            lbMessageGroups.BackColor = Color.LemonChiffon;
            if (lbMessageGroups.SelectedIndex != -1)
            {
                iWipMessageGroupID = Convert.ToInt32(lbMessageGroups.SelectedValue);
                btnSaveGroup.Enabled = true;
                btnDeleteGroup.Enabled = true;
                BindMessageGroup(iWipMessageGroupID);
            }
            else
            {
                Reset();
                btnSaveGroup.Enabled = false;
                btnDeleteGroup.Enabled = false;
            }

        }
    }
    protected void btnSaveGroup_Click(object sender, EventArgs e)
    {
        int iMessageGroupID = 0;
        if (lbMessageGroups.SelectedIndex != -1)
        {
            iMessageGroupID = Convert.ToInt32(lbMessageGroups.SelectedValue);
            UpdateMessageGroup(iMessageGroupID);
        }
    }
    protected void btnDeleteGroup_Click(object sender, EventArgs e)
    {
        int iMessageGroupID = 0;
        if (lbMessageGroups.SelectedIndex != -1)
        {
            iMessageGroupID = Convert.ToInt32(lbMessageGroups.SelectedValue);
            DeleteMessageGroup(iMessageGroupID);
        }
    }
    protected void btnAddGroup_Click(object sender, EventArgs e)
    {
            AddMessageGroup(); 
    }

    //Assign Message Groups...
    protected void ddlMessageGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbAssignedEmployees.Items.Clear();
        int iWipMessageGroupID = 0;

        if (ddlMessageGroups.SelectedIndex == 0)
        {
            btnAddOne.Enabled = false;
            btnRemoveOne.Enabled = false;
            lbAssignedEmployees.Items.Clear();
            lbAvailableEmployees.Items.Clear();
            return;
        }
        else
        {
            btnAddOne.Enabled = true;
        }
        iWipMessageGroupID = Convert.ToInt32(ddlMessageGroups.SelectedValue);
        LoadAvailableEmployees(iWipMessageGroupID);
        LoadAssignedEmployees(iWipMessageGroupID);
        if (lbAssignedEmployees.Items.Count > 0)
        {
            btnRemoveOne.Enabled = true;
        }
    }
    protected void btnAddOne_Click(object sender, EventArgs e)
    {
        AddOne();
    }
    protected void btnRemoveOne_Click(object sender, EventArgs e)
    {
        RemoveOne();
    }

    protected void lbnSelectAllAssign_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAvailableEmployees.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearAssign_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAvailableEmployees.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    protected void lbnSelectAllAssigned_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAssignedEmployees.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearAssigned_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAssignedEmployees.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }

    //Security Groups...
    protected void lbSecurityGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        int iWipSecurityGroupID = 0;
        if (lbSecurityGroups.SelectedIndex != -1)
        {
            iWipSecurityGroupID = Convert.ToInt32(lbSecurityGroups.SelectedValue);
            btnSaveSecurityGroup.Enabled = true;
            btnDeleteSecurityGroup.Enabled = true;
            BindSecurityGroup(iWipSecurityGroupID);
        }
        else
        {
            Reset();
            btnSaveSecurityGroup.Enabled = false;
            btnDeleteSecurityGroup.Enabled = false;
        }
    }
    protected void rblModeSecurityGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblErrorSecurityGroup.Text = "";
        int iWipSecurityGroupID = 0;
        if (rblModeSecurityGroup.SelectedIndex == 0)//Add...
        {
            ResetSecurityGroup();
            btnAddSecurityGroup.Enabled = true;
            btnSaveSecurityGroup.Enabled = false;
            btnDeleteSecurityGroup.Enabled = false;
            lbSecurityGroups.Enabled = false;
            lbSecurityGroups.BackColor = Color.LightGray;
        }
        else//Edit...
        {
            btnAddSecurityGroup.Enabled = false;
            lbSecurityGroups.Enabled = true;
            lbSecurityGroups.BackColor = Color.LemonChiffon;
            if (lbSecurityGroups.SelectedIndex != -1)
            {
                iWipSecurityGroupID = Convert.ToInt32(lbSecurityGroups.SelectedValue);
                btnSaveSecurityGroup.Enabled = true;
                btnDeleteSecurityGroup.Enabled = true;
                BindSecurityGroup(iWipSecurityGroupID);
            }
            else
            {
                Reset();
                btnSaveSecurityGroup.Enabled = false;
                btnDeleteSecurityGroup.Enabled = false;
            }

        }
    }
    protected void btnSaveSecurityGroup_Click(object sender, EventArgs e)
    {
        int iSecurityGroupID = 0;
        if (lbSecurityGroups.SelectedIndex != -1)
        {
            iSecurityGroupID = Convert.ToInt32(lbSecurityGroups.SelectedValue);
            UpdateSecurityGroup(iSecurityGroupID);
        }
    }
    protected void btnDeleteSecurityGroup_Click(object sender, EventArgs e)
    {
        int iSecurityGroupID = 0;
        if (lbSecurityGroups.SelectedIndex != -1)
        {
            iSecurityGroupID = Convert.ToInt32(lbSecurityGroups.SelectedValue);
            DeleteSecurityGroup(iSecurityGroupID);
        }
    }
    protected void btnAddSecurityGroup_Click(object sender, EventArgs e)
    {
        AddSecurityGroup();
    }

    //Assign Security SecurityGroups...
    protected void ddlSecurityGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbAssignedEmployeesSG.Items.Clear();
        int iWipSecurityGroupID = 0;

        if (ddlSecurityGroups.SelectedIndex == 0)
        {
            btnAddOneSG.Enabled = false;
            btnRemoveOneSG.Enabled = false;
            lbAssignedEmployeesSG.Items.Clear();
            lbAvailableEmployeesSG.Items.Clear();
            return;
        }
        else
        {
            btnAddOneSG.Enabled = true;
        }
        iWipSecurityGroupID = Convert.ToInt32(ddlSecurityGroups.SelectedValue);
        LoadAvailableEmployeesSG(iWipSecurityGroupID);
        LoadAssignedEmployeesSG(iWipSecurityGroupID);
        if (lbAssignedEmployeesSG.Items.Count > 0)
        {
            btnRemoveOneSG.Enabled = true;
        }
    }
    protected void btnAddOneSG_Click(object sender, EventArgs e)
    {
        AddOneSG();
    }
    protected void btnRemoveOneSG_Click(object sender, EventArgs e)
    {
        RemoveOneSG();
    }

    protected void lbnSelectAllAssignSG_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAvailableEmployeesSG.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearAssignSG_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAvailableEmployeesSG.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }
    protected void lbnSelectAllAssignedSG_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAssignedEmployeesSG.Items)
        {
            li.Selected = true;
        }
    }
    protected void lbnClearAssignedSG_Click(object sender, EventArgs e)
    {
        foreach (ListItem li in lbAssignedEmployeesSG.Items)
        {
            if (li.Selected)
            {
                li.Selected = false;
            }
        }
    }

    #endregion

    #region WebMethods
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






}