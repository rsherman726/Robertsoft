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
using System.Data.Linq.SqlClient;
using System.Transactions;
using ClosedXML.Excel;

[Serializable()]
public sealed class SharedFunctions
{



    #region Functions

    public static List<string> removeDuplicates(List<string> inputList)
    {
        Dictionary<string, int> uniqueStore = new Dictionary<string, int>();
        List<string> finalList = new List<string>();
        foreach (string currValue in inputList)
        {
            if (!uniqueStore.ContainsKey(currValue))
            {
                uniqueStore.Add(currValue, 0);
                finalList.Add(currValue);
            }
        }
        return finalList;
    }
    public static ListBox RemoveDupsFromListBox(ListBox lbInput)
    {
        int iRowCount = lbInput.Items.Count;
        ArrayList newListText = new ArrayList();
        ArrayList newListValues = new ArrayList();

        //Load all Items into temp array
        string[] tempText = new string[iRowCount];
        string[] tempValues = new string[iRowCount];


        for (int iRow = 0; iRow < iRowCount; iRow++)
        {
            tempText[iRow] = lbInput.Items[iRow].Text;
        }
        for (int iRow = 0; iRow < iRowCount; iRow++)
        {
            tempValues[iRow] = lbInput.Items[iRow].Value;
        }
        //Add unique items to new ArrayList
        foreach (string ts in tempText)
        {
            if (!newListText.Contains(ts))
            {
                newListText.Add(ts);
            }
        }
        foreach (string ts in tempValues)
        {
            if (!newListValues.Contains(ts))
            {
                newListValues.Add(ts);
            }
        }

        ListItemCollection lsc = new ListItemCollection();
        lbInput.Items.Clear();
        for (int i = 0; i < newListText.Count; i++)
        {
            string value = newListValues[i].ToString();
            string text = newListText[i].ToString();
            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem();
            lst.Text = text;
            lst.Value = value;
            lbInput.Items.Add(lst);
            lsc.Add(lst);
        }

        return lbInput;
    }
    public static DataSet getDataSet(string sSQL, SqlConnection conn, string sDataSetName)
    {

        SqlCommand command = new SqlCommand();

        DataSet ds = new DataSet();
        //create a new dataset...
        ds.Clear();

        SqlDataAdapter objDataAdapter = new SqlDataAdapter(sSQL, conn);
        objDataAdapter.SelectCommand.CommandTimeout = 900;
        try
        {
            objDataAdapter.Fill(ds, sDataSetName); //fill dataSet with data...
            return ds;
        }
        catch (System.Exception e)
        {
            Debug.WriteLine(e.ToString());
            return null;
        }
        finally
        {
            objDataAdapter.Dispose();
            ds.Dispose();
            conn.Close();
        }

    }
    public static DataSet ExecuteQueryWithRetVal(string sqlCmd, SqlConnection conn)
    {
        // connect to the  database

        conn.Open();

        DataSet dataSet = new DataSet();
        SqlCommand command = new SqlCommand();
        SqlDataAdapter sqlAdapter = new SqlDataAdapter(); ;

        command.CommandText = sqlCmd;
        command.CommandType = System.Data.CommandType.Text;
        command.Connection = conn;
        command.CommandTimeout = 900;

        try
        {
            //set the command for data adapter

            sqlAdapter.SelectCommand = command;
            sqlAdapter.Fill(dataSet);
            command.Connection.Close();
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                //Debug.WriteLine(dataSet.Tables[0].Rows[0][0].ToString());
                return dataSet;
            }
            else
            {

                return null;
            }

        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.ToString());
            return null;

        }
        finally
        {
            sqlAdapter.Dispose();
            dataSet.Dispose();
            command.Dispose();
            conn.Close();
        }
    }
    public static bool IsNullableType(Type type)
    {//Work in conjuntion with the getDataTableLinq...
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static DataTable getDataTable(string sSQL, SqlConnection conn, string sTableName)
    {
        DataTable dtGeneric = new DataTable();
        SqlCommand command = new SqlCommand();

        DataSet oDataSet = new DataSet();
        //create a new dataset...
        oDataSet.Clear();

        SqlDataAdapter objDataAdapter = new SqlDataAdapter(sSQL, conn);
        objDataAdapter.SelectCommand.CommandTimeout = 100000;
        try
        {
            objDataAdapter.Fill(oDataSet, sTableName); //fill dataSet with data...
            dtGeneric = oDataSet.Tables[sTableName];
            return dtGeneric;
        }
        catch (System.Exception e)
        {

            Debug.WriteLine(e.ToString());
            return null;
        }
        finally
        {
            if (dtGeneric != null)
            {
                dtGeneric.Dispose();
            }

            objDataAdapter.Dispose();
            command.Dispose();
            conn.Close();
        }

    }
    public static DataTable CloneTable(DataTable originalTable)
    {

        DataTable newTable;
        newTable = originalTable.Clone();

        return newTable;

    }
    public static int ExecuteQuery(string sqlCmd, SqlConnection conn)
    {
        // connect to the  database

        conn.Open();

        // call the update and rebind the datagrid
        SqlCommand command = new SqlCommand();
        command.CommandText = sqlCmd;
        command.Connection = conn;
        command.CommandTimeout = 600;
        try
        {
            command.ExecuteNonQuery();
            return 1;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return 0;
        }
        finally
        {
            conn.Close();
            conn.Dispose();
            command.Dispose();
        }
    }
    public static string GetUserName(int iUserID, SqlConnection conn)
    {
        string SQL = "Select UserName From Customers";
        SQL += " Where CustomerID=" + iUserID;

        DataTable dt = new DataTable();

        dt = getDataTable(SQL, conn, "Customers");


        if (dt.Rows.Count > 0)
        {

            return dt.Rows[0]["UserName"].ToString();
        }
        else
        {

            return "";
        }
    }
    public static string CleanInput(string strIn)
    {
        // Replace invalid characters with empty strings.
        return Regex.Replace(strIn, @"[^\w\-]", "");
    }
    public static bool IsEmail(string email)
    {
        bool result;

        result = Regex.IsMatch(email.Trim(), @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");


        if (result != true)
        {
            //bad email...
            return false;
        }
        else
        {
            //good email..
            return true;
        }
    }
    public static string PCase(String strParam)
    {
        if (strParam == "")
        {
            return "";
        }
        String strProper = strParam.Substring(0, 1).ToUpper();
        strParam = strParam.Substring(1).ToLower();
        String strPrev = "";

        for (int iIndex = 0; iIndex < strParam.Length; iIndex++)
        {
            if (iIndex > 1)
            {
                strPrev = strParam.Substring(iIndex - 1, 1);
            }
            if (strPrev.Equals(" ") ||
                strPrev.Equals("\t") ||
                strPrev.Equals("\n") ||
                strPrev.Equals("."))
            {
                strProper += strParam.Substring(iIndex, 1).ToUpper();
            }
            else
            {
                strProper += strParam.Substring(iIndex, 1);
            }
        }
        return strProper;
    }
    public static bool IsAlphaCharacters(string strIn)
    {
        if (Regex.IsMatch(strIn, "([a-z])", RegexOptions.IgnoreCase) == true)
        {
            return true;
        }
        else
        {
            return false;
        }

    }
    public static bool IsNumeric(string inputData)
    {
        try
        {
            Convert.ToInt32(inputData);
            return true;
        }
        catch
        {
            try
            {
                Convert.ToDouble(inputData);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
    public static bool IsDate(string inputData)
    {
        try
        {
            Convert.ToDateTime(inputData);
            return true;
        }
        catch
        {
            return false;
        }
    }
    public static bool IsZip(string strIn)
    {
        if (Regex.IsMatch(strIn, @"\d{5}(-\d{4})?", RegexOptions.IgnoreCase) == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static string RemoveQuotes(string s)
    {
        //Remove single Quotes from string....

        string s2;

        s = s.Trim();


        s2 = "";

        for (int i = 0; i < s.Length; i++)
        {
            if (s.Substring(i, 1) == "'")
            //if(Mid(s, i, 1) == "'" )
            {
                s2 = s2 + "''";
            }
            else
            {
                s2 = s2 + s.Substring(i, 1);
                //s2 = s2 & Mid(s, i, 1);
            }
        }


        return s2;

    }
    public static bool IsAdmin(int iUserID)
    {

        // SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        int iRoleID = 0;
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        var query = (from u in db.WipUsers
                     where u.UserID == iUserID
                     select new { u.RoleID });

        foreach (var a in query)
        {
            iRoleID = a.RoleID;
        }
        if (iRoleID == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool IsValidEmail(String inputData)
    {
        bool bMatch = false;
        Regex regEmailPattern = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        if (regEmailPattern.IsMatch(inputData))
        {
            bMatch = true;
        }
        else
        {
            bMatch = false;
        }
        return bMatch;
    }
    public static string GetPhoneFormat(string sNumber)
    {
        string sFormated = "";
        string sInput = sNumber.Trim();
        string sFirstThree = "";
        string sMiddleThree = "";
        string sLastFour = "";
        if (sInput.Length > 0)
        {
            sFirstThree = sInput.Substring(0, 3);
            sMiddleThree = sInput.Substring(3, 3);
            sLastFour = sInput.Substring(6, 4);

            sFormated = "(" + sFirstThree + ") " + sMiddleThree + "-" + sLastFour;

        }
        return sFormated;

    }
    public static string GetLongStateName(string sShortStateName)
    {
        string sLongStateName = "";
        string SQL = "Select Description From States";
        SQL += " Where CountryID='US'";
        SQL += " And ShortState='" + sShortStateName + "'";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        DataTable dt = new DataTable();

        dt = getDataTable(SQL, conn, "States");

        if (dt.Rows.Count > 0)
        {
            sLongStateName = dt.Rows[0]["Description"].ToString();
        }

        //always will return a value...

        return sLongStateName;
    }
    public static string GetShortStateName(string sLongStateName)
    {
        string sShortStateName = "";
        string SQL = "Select ShortState From States";
        SQL += " Where CountryID='US'";
        SQL += " And Description='" + sLongStateName + "'";
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        DataTable dt = new DataTable();

        dt = getDataTable(SQL, conn, "States");

        if (dt.Rows.Count > 0)
        {
            sShortStateName = dt.Rows[0]["ShortState"].ToString();
        }

        //always will return a value...

        return sShortStateName;
    }
    public static int GetSelIndex(string ListValue, DataTable dt, string fieldName, string type)
    {

        if (type == "INT")
        {

            //Loop through each row in the DataSet
            for (int iLoop = 0; iLoop < dt.Rows.Count - 1; iLoop++)
            {

                int iListValue = Int32.Parse(ListValue);

                if (iListValue == Int32.Parse(dt.Rows[iLoop][fieldName].ToString().Trim()))
                {
                    return iLoop;
                }

            }

        }

        if (type == "STRING")
        {
            //Loop through each row in the DataSet
            for (int iLoop = 0; iLoop < dt.Rows.Count - 1; iLoop++)
            {

                // Debug.WriteLine(dt.Rows[iLoop][fieldName].ToString());

                if (ListValue == dt.Rows[iLoop][fieldName].ToString().Trim())
                {
                    return iLoop;
                }

            }

        }

        return -1;
    }
    public static int GetSelIndex(string ListValue, System.Web.UI.WebControls.DropDownList dl, string Type)
    {


        if (Type == "Value")
        {
            //Loop through each row in the dropDownList...
            for (int iLoop = 0; iLoop < dl.Items.Count; iLoop++)
            {


                //Debug.WriteLine(dl.Items[iLoop].Value);
                if (ListValue == dl.Items[iLoop].Value.Trim())
                {
                    return iLoop;
                }

            }

        }
        if (Type == "Text")
        {
            //Loop through each row in the dropDownList...
            for (int iLoop = 0; iLoop < dl.Items.Count; iLoop++)
            {


                //Debug.WriteLine(dl.Items[iLoop].ToString());
                if (ListValue == dl.Items[iLoop].Text.Trim())
                {
                    return iLoop;
                }

            }


        }
        return -1;

    }
    public static int GetSelIndex(string ListValue, System.Web.UI.WebControls.ListBox dl, string Type)
    {


        if (Type == "Value")
        {
            //Loop through each row in the dropDownList...
            for (int iLoop = 0; iLoop < dl.Items.Count; iLoop++)
            {


                //Debug.WriteLine(dl.Items[iLoop].Value);
                if (ListValue == dl.Items[iLoop].Value.Trim())
                {
                    return iLoop;
                }

            }

        }
        if (Type == "Text")
        {
            //Loop through each row in the dropDownList...
            for (int iLoop = 0; iLoop < dl.Items.Count; iLoop++)
            {


                //Debug.WriteLine(dl.Items[iLoop].ToString());
                if (ListValue == dl.Items[iLoop].Text.Trim())
                {
                    return iLoop;
                }

            }


        }
        return -1;

    }
    public static string getShortMonth(int month)
    {
        string ShortMonthName = "";
        switch (month)
        {
            case 1:
                ShortMonthName = "Jan";
                break;
            case 2:
                ShortMonthName = "Feb";
                break;
            case 3:
                ShortMonthName = "Mar";
                break;
            case 4:
                ShortMonthName = "Apr";
                break;
            case 5:
                ShortMonthName = "May";
                break;
            case 6:
                ShortMonthName = "Jun";
                break;
            case 7:
                ShortMonthName = "Jul";
                break;
            case 8:
                ShortMonthName = "Aug";
                break;
            case 9:
                ShortMonthName = "Sep";
                break;
            case 10:
                ShortMonthName = "Oct";
                break;
            case 11:
                ShortMonthName = "Nov";
                break;
            case 12:
                ShortMonthName = "Dec";
                break;

            default:
                break;

        }
        return ShortMonthName;
    }
    public static bool UserNameExists(string sUserName)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from users in db.WipUsers
                         where users.UserName == sUserName
                         select users);
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
    public static string GetNodeTextWithString(string NodeName, string xmlString)
    {
        XmlNode obj_Node;
        XmlDocument doc = new XmlDocument();

        doc.LoadXml(xmlString);

        XmlElement root = doc.DocumentElement;
        obj_Node = root.SelectSingleNode("//" + NodeName);


        if (obj_Node == null)
        {
            Debug.WriteLine("no node with that name found!");
            return "";
        }
        else
        {
            Debug.WriteLine(obj_Node.InnerText);
            return obj_Node.InnerText;
        }
    }
    //public static string GetEmployeeName(int iEmpID)
    //{
    //    string sEmpName = "";

    //    FNF db = new FNF();
    //    var query = (from e in db.Employees
    //                 where
    //                   e.EmployeeID == iEmpID
    //                 select new
    //                 {
    //                     Name = (e.Fname + " " + e.MName + " " + e.LName)
    //                 });
    //    foreach (var a in query)
    //    {
    //        sEmpName = a.Name;
    //    }

    //    return sEmpName;
    //}
    //public static bool IsRespAlreadyInActiveGroup(int iRespID, int iProjID)
    //{
    //    bool bInGroup = false;
    //    FNF db = new FNF();

    //    try
    //    {
    //        var query = (from rtp in db.RespToProjects
    //                     join p in db.Projects on new { ProjID = rtp.ProjID } equals new { ProjID = p.ProjectID }
    //                     join rg in db.RespGroup on rtp.RespID equals rg.RespID
    //                     where
    //                       rtp.RespID == iRespID &&
    //                       rtp.ProjID != iProjID &&
    //                       rg.ProjID != iProjID &&
    //                       p.Status == "Active"
    //                     group rg by new
    //                     {
    //                         rg.GroupID,
    //                         rg.ProjID
    //                     } into g
    //                     select new
    //                     {
    //                         GroupID = (System.Int32?)g.Key.GroupID,
    //                         ProjOfGroup = (System.Int32?)g.Key.ProjID,
    //                         GroupName =
    //                           ((from groups in db.Groups
    //                             where
    //                               groups.GroupID == g.Key.GroupID
    //                             select new
    //                             {
    //                                 groups.GroupName
    //                             }).First().GroupName)
    //                     });
    //        int iCount = query.Count();
    //        if (iCount > 0)
    //        {
    //            bInGroup = true;
    //        }
    //        else
    //        {
    //            bInGroup = false;
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine(ex.ToString());
    //    }


    //    return bInGroup;

    //}
    public static string FromDecimalTimeToStandardTime(double dTotalTime)
    {
        string sResult = "";
        string sMinutes = "";
        double Hours = Math.Floor(dTotalTime);
        int Minutes = 0;//not lunch minutes...
        //Calc Minutes
        Minutes = Convert.ToInt32((dTotalTime - Hours) * 60f);
        if (Minutes.ToString().Length < 2)
        {
            sMinutes = "0" + Minutes.ToString();
        }
        else
        {
            sMinutes = Minutes.ToString();
        }

        sResult = Hours.ToString() + ":" + sMinutes;

        return sResult;
    }
    public static double FromStandardTimeToDecimalTime(string sTotalHours)
    {
        try
        {
            double dTotalHours = 0;
            string sMinutes = "";
            double dMinutes = 0;
            string sHours = "";
            int iHours = 0;
            int iPositionOfColon = 0;
            iPositionOfColon = sTotalHours.IndexOf(":");
            sMinutes = sTotalHours.Substring(iPositionOfColon + 1);
            sHours = sTotalHours.Substring(0, iPositionOfColon);
            dMinutes = Convert.ToDouble(sMinutes);
            dMinutes = dMinutes / 60f;
            iHours = Convert.ToInt32(sHours);

            dTotalHours = iHours + dMinutes;

            return dTotalHours;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
            return 0;
        }

    }
    public static double FromStandardTimeToDecimalTimeWithSeconds(string sTotalHours)
    {
        double dTotalHours = 0;
        string sMinutes = "";
        double dMinutes = 0;
        string sSeconds = "";
        double dSeconds = 0;
        string sHours = "";
        int iHours = 0;
        int iPositionOfColon = 0;
        int iLastPositionOfColon = 0;
        iLastPositionOfColon = sTotalHours.LastIndexOf(":");
        sSeconds = sTotalHours.Substring(iLastPositionOfColon + 1);
        iPositionOfColon = sTotalHours.IndexOf(":");
        sMinutes = sTotalHours.Substring(iPositionOfColon + 1, 2);
        sHours = sTotalHours.Substring(0, iPositionOfColon);
        dSeconds = Convert.ToDouble(sSeconds);
        dMinutes = Convert.ToDouble(sMinutes);
        dSeconds = dSeconds / 3600f;
        dMinutes = dMinutes / 60f;
        iHours = Convert.ToInt32(sHours);
        dTotalHours = iHours + dMinutes + dSeconds;

        return dTotalHours;
    }
    public static double ConvertMinutesToHours(int iMinutes)
    {
        double dHours = 0;

        dHours = iMinutes / 60f;

        return dHours;

    }
    public static double ConvertHoursToMinutes(double dHours)
    {
        double dMinutes = 0;

        dMinutes = dHours * 60f;

        return dMinutes;

    }
    public static double ConvertMinutesToHours(double dMinutes)
    {
        double dHours = 0;

        dHours = dMinutes / 60f;

        return dHours;

    }
    public static DateTime GetFirstDayOfMonth(DateTime dtDate)
    {
        // set return value to the first day of the month 
        // for any date passed in to the method 
        // create a datetime variable set to the passed in date 

        DateTime dtFrom = dtDate;


        // remove all of the days in the month 
        // except the first day and set the 
        // variable to hold that date 
        dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));

        // return the first day of the month 

        return dtFrom;

    }
    public static int? GetRole(int iUserID)
    {
        FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        int? iRole = null;

        var query = (from users in db.WipUsers
                     where users.UserID == iUserID
                     select new
                     {
                         users.RoleID
                     });
        foreach (var a in query)
        {
            iRole = a.RoleID;
        }
        return iRole;

    }
    public static DataTable GetJobDescriptionAndStockCode(Int64 iJob)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = null;

            try
            {
                var query = (from m in db.WipMaster
                             join pa in db.WipProductionLineAssigns on m.Job equals pa.Job into pa_join
                             from pa in pa_join.DefaultIfEmpty()
                             join ja in db.WipProductionLines on pa.ProLineID equals ja.ProLineID into ja_join
                             from ja in ja_join.DefaultIfEmpty()
                             where
                               Convert.ToInt64(m.Job) == iJob
                             select new
                             {
                                 m.Job,
                                 m.JobDescription,
                                 m.StockCode,
                                 LineName = ja.LineName == null ? "UnAssigned" : ja.LineName
                             });

                dt = ToDataTable(db, query);
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
    }
    public static String GetEmployeeName(int iEmployeeID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sName = null;

            var query = (from ea in db.WipEmployees
                         where ea.EmployeeID == iEmployeeID
                         select new
                         {
                             FullName = (ea.FirstName + " " + (ea.MiddleName ?? "") + " " + ea.LastName).Replace("  ", " ")
                         });
            foreach (var a in query)
            {
                sName = a.FullName;
            }
            return sName;
        }
    }
    public static Decimal GetJobHours(string sJob)
    {
        decimal dcHours = 0;
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from wipjobpost in db.WipJobPost
                         where
                           wipjobpost.Job == sJob
                         group wipjobpost by new
                         {
                             wipjobpost.Job
                         } into g
                         orderby
                           g.Key.Job descending
                         select new
                         {
                             g.Key.Job,
                             Hours = (System.Decimal?)g.Sum(p => p.LRunTimeHours)
                         });
            foreach (var a in query)
            {
                dcHours = Convert.ToDecimal(a.Hours);
            }

            return dcHours;
        }
    }
    public static string GetDept(int iUserID)
    {
        string sDept = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from u in db.WipUsers
                         where u.UserID == iUserID
                         select new
                         {
                             u.Dept
                         });
            foreach (var a in query)
            {
                sDept = (a.Dept ?? "");
            }
        }

        return sDept;
    }
    ///NEW LINQ Functions...
    public static DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
    {
        DataTable dtReturn = new DataTable();

        // column names 
        System.Reflection.PropertyInfo[] oProps = null;

        if (varlist == null) return dtReturn;

        foreach (T rec in varlist)
        {
            // Use reflection to get property names, to create table, Only first time, others 
            // will follow 
            if (oProps == null)
            {
                oProps = ((Type)rec.GetType()).GetProperties();
                foreach (System.Reflection.PropertyInfo pi in oProps)
                {
                    Type colType = pi.PropertyType;

                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                    == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }

                    dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                }
            }

            DataRow dr = dtReturn.NewRow();

            foreach (System.Reflection.PropertyInfo pi in oProps)
            {
                dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                (rec, null);
            }

            dtReturn.Rows.Add(dr);
        }
        return dtReturn;
    }
    public static DataTable ToDataTable(System.Data.Linq.DataContext ctx, object query)
    {//For Queries that use non declare columns
        if (query == null)
        {
            throw new ArgumentNullException("query");
        }

        IDbCommand cmd = ctx.GetCommand(query as IQueryable);
        System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter();
        adapter.SelectCommand = (System.Data.SqlClient.SqlCommand)cmd;
        DataTable dt = new DataTable("sd");

        try
        {
            if (cmd.Connection.State == ConnectionState.Open)
            {
                cmd.Connection.Close();
            }
            cmd.Connection.Open();
            adapter.FillSchema(dt, SchemaType.Source);
            adapter.Fill(dt);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.ToString());
        }
        finally
        {
            cmd.Connection.Close();
        }
        return dt;
    }
    //public static int? GetRole(int iUserID)
    //{
    //    MOOSEPLANET db = new MOOSEPLANET();
    //    int? iRole = null;

    //    var query =( from users in db.Users
    //                 where users.UserID == iUserID
    //                 select new 
    //                 {
    //                     users.RoleID
    //                 });
    //    foreach (var a in query)
    //    {
    //        iRole = a.RoleID;
    //    }
    //    return iRole;

    //}
    //public static bool LoadStates(DropDownList ddl)
    //{


    //    MOOSEPLANET db = new MOOSEPLANET();
    //    var query = (from states in db.States
    //                 where states.CountryID =="US"
    //                 select new
    //                 {
    //                     states.Description,
    //                     states.ShortState
    //                 });


    //    try
    //    {
    //        foreach (var a in query)
    //        {
    //            ddl.Items.Add(new System.Web.UI.WebControls.ListItem(a.Description, a.ShortState));
    //        }

    //        //string sFieldName = "Description";
    //        ddl.SelectedIndex = GetSelIndex("CA", ddl, "Value");
    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine(ex.ToString());
    //        return false;
    //    }
    //}
    //public static string GetCompanyFolderName(int iUserID)
    //{
    //    MOOSEPLANET db = new MOOSEPLANET();
    //    string sFolderName = "";

    //    var query = (from users in db.Users
    //                 where users.UserID == iUserID
    //                 select new
    //                 {
    //                     users.CompanyFolder
    //                 });

    //    foreach (var a in query)//should return only one folder name...
    //    {
    //        sFolderName = a.CompanyFolder;
    //    }

    //    return sFolderName;
    //}
    //public static bool UserAlreadyExists(string sUserName)
    //{
    //    MOOSEPLANET db = new MOOSEPLANET();
    //    var query = (from u in db.Users
    //                 where u.UserName == sUserName
    //                 select u);
    //    int iRowCount = query.Count();

    //    if (iRowCount > 0)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    public static int Calculate_Age(int year, int month, int day)
    {
        int diffYears = DateTime.Now.Year - year;
        if ((DateTime.Now.Month < month) ||
               (DateTime.Now.Month == month && DateTime.Now.Day < day))
            diffYears--;
        return diffYears;
    }
    public static bool MenuItemAlreadyExists(string sText)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                // Your LINQ to SQL query goes here 
                var query = (from menu in db.Menu
                             where menu.Text == sText
                             select new
                             {
                                 menu.Text
                             }).ToArray();
                int iRowCount = query.Count();

                if (iRowCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
    public static bool MenuItemIDAlreadyExists(int iMenuID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                // Your LINQ to SQL query goes here 
                var query = (from menu in db.Menu
                             where menu.MenuID == iMenuID
                             select new
                             {
                                 menu.MenuID
                             }).ToArray();
                int iRowCount = query.Count();

                if (iRowCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
    public static int? GetAdminID(int iUserID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            int? iAdminID = null;
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                // Your LINQ to SQL query goes here 
                try
                {
                    var query = (from u in db.WipUsers
                                 where
                                   u.UserID == iUserID
                                 select new
                                 {
                                     u.RoleID
                                 });
                    foreach (var adminid in query)
                    {
                        iAdminID = adminid.RoleID;
                    }
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.ToString());
                }


                return iAdminID;
            }
        }
    }
    public static string GetMenuItemText(string sNavigateUrl)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            string sMenuItemText = "";
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                // Your LINQ to SQL query goes here 
                var query = (from menu in db.Menu
                             where
                               menu.NavigateUrl == sNavigateUrl
                             select new
                             {
                                 menu.Text
                             });
                foreach (var text in query)
                {
                    sMenuItemText = text.Text;
                }

                return sMenuItemText;
            }
        }
    }
    public static bool IsAccessGranted(int? iAdminID, string sPageName)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            using (var txn = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                // Your LINQ to SQL query goes here 
                try
                {
                    var query = (from menuitemaccess in db.MenuItemAccess
                                 where
                                   menuitemaccess.AdminID == iAdminID &&
                                   menuitemaccess.MenuID ==
                                     ((from menu in db.Menu
                                       where
                                         menu.Text == sPageName
                                       select new
                                       {
                                           menu.MenuID
                                       }).First().MenuID)
                                 select new
                                 {
                                     menuitemaccess.MIRID,
                                     menuitemaccess.MenuID,
                                     menuitemaccess.AdminID
                                 });

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
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.ToString());
                    return false;
                }
                finally
                {

                }
            }
        }
    }
    public static string GetUserFullName(int iUserID)
    {
        string sFullName = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from u in db.WipUsers
                       where u.UserID == iUserID
                       select new
                       {
                           FullName = (u.FirstName + " " + (u.MiddleName ?? "") + " " + u.LastName).Replace("  ", " ")
                       });
            foreach (var a in qry)
            {
                sFullName = a.FullName;
            }
            return sFullName;
        }
    }
    public static string GetUserEmail(int iUserID)
    {
        string sEmail = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from u in db.WipUsers
                       where u.UserID == iUserID
                       select new
                       {
                           u.Email,
                       });
            foreach (var a in qry)
            {
                sEmail = a.Email;
            }
            return sEmail;
        }
    }
    public static string GetPurchaseOrderWithSalesOrder(string sSalesOrder)
    {
        string sPurchaseOrder = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from p in db.SorMaster
                       where p.SalesOrder == sSalesOrder
                       select new
                       {
                           p.CustomerPoNumber
                       });
            foreach (var a in qry)
            {
                sPurchaseOrder = a.CustomerPoNumber;
            }
            return sPurchaseOrder;
        }
    }
    public static string GetCustomerIDWithSalesOrder(string sSalesOrder)
    {
        string sCustomerID = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from p in db.SorMaster
                       where p.SalesOrder == sSalesOrder
                       select new
                       {
                           p.Customer
                       });
            foreach (var a in qry)
            {
                sCustomerID = a.Customer.Trim();
            }
            return sCustomerID;
        }
    }
    public static DataTable GetCompanyInfoFromDeliveryIDInDeliveries(int iDeliveryID)
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {
                var qry = (from dm in db.DelMaster
                           join c in db.ArCustomer on dm.Customer equals c.Customer
                           where
                             dm.DeliveryID == iDeliveryID
                           select new
                           {
                               dm.DeliveryID,
                               dm.Customer,
                               dm.SalesOrder,
                               dm.CustomerPoNumber,
                               dm.DeliveryTypeID,
                               dm.QtyScheduled,
                               dm.QtyActual,
                               dm.DateScheduled,
                               dm.DateDelivered,
                               dm.DateAdded,
                               dm.AddedBy,
                               dm.DateModified,
                               dm.ModifiedBy,
                               dm.DriverID,
                               dm.VehicleID,
                               dm.DeliveryStatus,
                               dm.CheckNumber,
                               dm.Amount,
                               dm.IsCOD,
                               dm.Comments,
                               c.Name,
                               c.ShortName,
                               c.ExemptFinChg,
                               c.MaintHistory,
                               c.CustomerType,
                               c.MasterAccount,
                               c.StoreNumber,
                               c.PrtMasterAdd,
                               c.CreditStatus,
                               c.CreditLimit,
                               c.InvoiceCount,
                               c.Salesperson,
                               c.Salesperson1,
                               c.Salesperson2,
                               c.Salesperson3,
                               c.PriceCode,
                               c.CustomerClass,
                               c.Branch,
                               c.TermsCode,
                               c.InvDiscCode,
                               c.BalanceType,
                               c.Area,
                               c.LineDiscCode,
                               c.TaxStatus,
                               c.TaxExemptNumber,
                               c.SpecialInstrs,
                               c.PriceCategoryTable,
                               c.DateLastSale,
                               c.DateLastPay,
                               c.OutstOrdVal,
                               c.NumOutstOrd,
                               c.Telephone,
                               c.Contact,
                               c.AddTelephone,
                               c.Fax,
                               c.Telex,
                               c.TelephoneExtn,
                               c.Currency,
                               c.UserField1,
                               c.UserField2,
                               c.GstExemptFlag,
                               c.GstExemptNum,
                               c.GstLevel,
                               c.DetailMoveReqd,
                               c.InterfaceFlag,
                               c.ContractPrcReqd,
                               c.BuyingGroup1,
                               c.BuyingGroup2,
                               c.BuyingGroup3,
                               c.BuyingGroup4,
                               c.BuyingGroup5,
                               c.StatementReqd,
                               c.BackOrdReqd,
                               c.ShippingInstrs,
                               c.StateCode,
                               c.DateCustAdded,
                               c.StockInterchange,
                               c.MaintLastPrcPaid,
                               c.IbtCustomer,
                               c.SoDefaultDoc,
                               c.CounterSlsOnly,
                               c.PaymentStatus,
                               c.Nationality,
                               c.HighestBalance,
                               c.CustomerOnHold,
                               c.InvCommentCode,
                               c.EdiSenderCode,
                               c.RelOrdOsValue,
                               c.EdiFlag,
                               c.SoDefaultType,
                               c.Email,
                               c.ApplyOrdDisc,
                               c.ApplyLineDisc,
                               c.FaxInvoices,
                               c.FaxStatements,
                               c.HighInvDays,
                               c.HighInv,
                               c.DocFax,
                               c.DocFaxContact,
                               c.SoldToAddr1,
                               c.SoldToAddr2,
                               c.SoldToAddr3,
                               c.SoldToAddr4,
                               c.SoldToAddr5,
                               c.SoldPostalCode,
                               c.ShipToAddr1,
                               c.ShipToAddr2,
                               c.ShipToAddr3,
                               c.ShipToAddr4,
                               c.ShipToAddr5,
                               c.ShipPostalCode,
                               c.State,
                               c.CountyZip,
                               c.City,
                               c.State1,
                               c.CountyZip1,
                               c.City1,
                               c.DefaultOrdType,
                               c.PoNumberMandatory,
                               c.CreditCheckFlag,
                               c.CompanyTaxNumber,
                               c.DeliveryTerms,
                               c.TransactionNature,
                               c.DeliveryTermsC,
                               c.TransactionNatureC,
                               c.RouteCode,
                               c.FaxQuotes,
                               c.RouteDistance,
                               c.TpmCustomerFlag,
                               c.SalesWarehouse,
                               c.TpmPricingFlag,
                               c.ArStatementNo,
                               c.TpmCreditCheck,
                               c.WholeOrderShipFlag,
                               c.MinimumOrderValue,
                               c.MinimumOrderChgCod,
                               c.UkVatFlag,
                               c.UkCurrency,
                               c.TimeStamp
                           });
                dt = SharedFunctions.ToDataTable(db, qry);
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
    }
    public static DataTable GetCompanyInfoFromPurchaseOrder(string sCustomerPoNumber)
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {
                var qry = (from c in db.ArCustomer
                           join sm in db.SorMaster on c.Customer equals sm.Customer
                           where
                             sm.CustomerPoNumber.Trim().Replace(" ", "") == sCustomerPoNumber
                           select new
                           {
                               sm.SalesOrder,
                               sm.CustomerPoNumber,
                               c.Name,
                               c.ShortName,
                               c.ExemptFinChg,
                               c.MaintHistory,
                               c.CustomerType,
                               c.MasterAccount,
                               c.StoreNumber,
                               c.PrtMasterAdd,
                               c.CreditStatus,
                               c.CreditLimit,
                               c.InvoiceCount,
                               c.Salesperson,
                               c.Salesperson1,
                               c.Salesperson2,
                               c.Salesperson3,
                               c.PriceCode,
                               c.CustomerClass,
                               c.Branch,
                               c.TermsCode,
                               c.InvDiscCode,
                               c.BalanceType,
                               c.Area,
                               c.LineDiscCode,
                               c.TaxStatus,
                               c.TaxExemptNumber,
                               c.SpecialInstrs,
                               c.PriceCategoryTable,
                               c.DateLastSale,
                               c.DateLastPay,
                               c.OutstOrdVal,
                               c.NumOutstOrd,
                               c.Telephone,
                               c.Contact,
                               c.AddTelephone,
                               c.Fax,
                               c.Telex,
                               c.TelephoneExtn,
                               c.Currency,
                               c.UserField1,
                               c.UserField2,
                               c.GstExemptFlag,
                               c.GstExemptNum,
                               c.GstLevel,
                               c.DetailMoveReqd,
                               c.InterfaceFlag,
                               c.ContractPrcReqd,
                               c.BuyingGroup1,
                               c.BuyingGroup2,
                               c.BuyingGroup3,
                               c.BuyingGroup4,
                               c.BuyingGroup5,
                               c.StatementReqd,
                               c.BackOrdReqd,
                               c.ShippingInstrs,
                               c.StateCode,
                               c.DateCustAdded,
                               c.StockInterchange,
                               c.MaintLastPrcPaid,
                               c.IbtCustomer,
                               c.SoDefaultDoc,
                               c.CounterSlsOnly,
                               c.PaymentStatus,
                               c.Nationality,
                               c.HighestBalance,
                               c.CustomerOnHold,
                               c.InvCommentCode,
                               c.EdiSenderCode,
                               c.RelOrdOsValue,
                               c.EdiFlag,
                               c.SoDefaultType,
                               c.Email,
                               c.ApplyOrdDisc,
                               c.ApplyLineDisc,
                               c.FaxInvoices,
                               c.FaxStatements,
                               c.HighInvDays,
                               c.HighInv,
                               c.DocFax,
                               c.DocFaxContact,
                               c.SoldToAddr1,
                               c.SoldToAddr2,
                               c.SoldToAddr3,
                               c.SoldToAddr4,
                               c.SoldToAddr5,
                               c.SoldPostalCode,
                               c.ShipToAddr1,
                               c.ShipToAddr2,
                               c.ShipToAddr3,
                               c.ShipToAddr4,
                               c.ShipToAddr5,
                               c.ShipPostalCode,
                               c.State,
                               c.CountyZip,
                               c.City,
                               c.State1,
                               c.CountyZip1,
                               c.City1,
                               c.DefaultOrdType,
                               c.PoNumberMandatory,
                               c.CreditCheckFlag,
                               c.CompanyTaxNumber,
                               c.DeliveryTerms,
                               c.TransactionNature,
                               c.DeliveryTermsC,
                               c.TransactionNatureC,
                               c.RouteCode,
                               c.FaxQuotes,
                               c.RouteDistance,
                               c.TpmCustomerFlag,
                               c.SalesWarehouse,
                               c.TpmPricingFlag,
                               c.ArStatementNo,
                               c.TpmCreditCheck,
                               c.WholeOrderShipFlag,
                               c.MinimumOrderValue,
                               c.MinimumOrderChgCod,
                               c.UkVatFlag,
                               c.UkCurrency,
                               c.TimeStamp
                           });
                dt = SharedFunctions.ToDataTable(db, qry);
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
    }
    public static DataTable GetCompanyInfoFromSalesOrder(string sSalesOrder)
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {
                var qry = (from sm in db.SorMaster
                           join c in db.ArCustomer on sm.Customer equals c.Customer
                           where
                             sm.SalesOrder == sSalesOrder
                           select new
                           {
                               sm.SalesOrder,
                               sm.CustomerPoNumber,
                               c.Customer,
                               c.Name,
                               c.ShortName,
                               c.ExemptFinChg,
                               c.MaintHistory,
                               c.CustomerType,
                               c.MasterAccount,
                               c.StoreNumber,
                               c.PrtMasterAdd,
                               c.CreditStatus,
                               c.CreditLimit,
                               c.InvoiceCount,
                               c.Salesperson,
                               c.Salesperson1,
                               c.Salesperson2,
                               c.Salesperson3,
                               c.PriceCode,
                               c.CustomerClass,
                               c.Branch,
                               c.TermsCode,
                               c.InvDiscCode,
                               c.BalanceType,
                               c.Area,
                               c.LineDiscCode,
                               c.TaxStatus,
                               c.TaxExemptNumber,
                               c.SpecialInstrs,
                               c.PriceCategoryTable,
                               c.DateLastSale,
                               c.DateLastPay,
                               c.OutstOrdVal,
                               c.NumOutstOrd,
                               c.Telephone,
                               c.Contact,
                               c.AddTelephone,
                               c.Fax,
                               c.Telex,
                               c.TelephoneExtn,
                               c.Currency,
                               c.UserField1,
                               c.UserField2,
                               c.GstExemptFlag,
                               c.GstExemptNum,
                               c.GstLevel,
                               c.DetailMoveReqd,
                               c.InterfaceFlag,
                               c.ContractPrcReqd,
                               c.BuyingGroup1,
                               c.BuyingGroup2,
                               c.BuyingGroup3,
                               c.BuyingGroup4,
                               c.BuyingGroup5,
                               c.StatementReqd,
                               c.BackOrdReqd,
                               c.ShippingInstrs,
                               c.StateCode,
                               c.DateCustAdded,
                               c.StockInterchange,
                               c.MaintLastPrcPaid,
                               c.IbtCustomer,
                               c.SoDefaultDoc,
                               c.CounterSlsOnly,
                               c.PaymentStatus,
                               c.Nationality,
                               c.HighestBalance,
                               c.CustomerOnHold,
                               c.InvCommentCode,
                               c.EdiSenderCode,
                               c.RelOrdOsValue,
                               c.EdiFlag,
                               c.SoDefaultType,
                               c.Email,
                               c.ApplyOrdDisc,
                               c.ApplyLineDisc,
                               c.FaxInvoices,
                               c.FaxStatements,
                               c.HighInvDays,
                               c.HighInv,
                               c.DocFax,
                               c.DocFaxContact,
                               c.SoldToAddr1,
                               c.SoldToAddr2,
                               c.SoldToAddr3,
                               c.SoldToAddr4,
                               c.SoldToAddr5,
                               c.SoldPostalCode,
                               c.ShipToAddr1,
                               c.ShipToAddr2,
                               c.ShipToAddr3,
                               c.ShipToAddr4,
                               c.ShipToAddr5,
                               c.ShipPostalCode,
                               c.State,
                               c.CountyZip,
                               c.City,
                               c.State1,
                               c.CountyZip1,
                               c.City1,
                               c.DefaultOrdType,
                               c.PoNumberMandatory,
                               c.CreditCheckFlag,
                               c.CompanyTaxNumber,
                               c.DeliveryTerms,
                               c.TransactionNature,
                               c.DeliveryTermsC,
                               c.TransactionNatureC,
                               c.RouteCode,
                               c.FaxQuotes,
                               c.RouteDistance,
                               c.TpmCustomerFlag,
                               c.SalesWarehouse,
                               c.TpmPricingFlag,
                               c.ArStatementNo,
                               c.TpmCreditCheck,
                               c.WholeOrderShipFlag,
                               c.MinimumOrderValue,
                               c.MinimumOrderChgCod,
                               c.UkVatFlag,
                               c.UkCurrency,
                               c.TimeStamp
                           });
                dt = SharedFunctions.ToDataTable(db, qry);
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
    }
    public static DataTable GetCompanyInfoFromAllDocs(string sDocID)
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {
                var qry = (from sm in db.SorMaster
                           join c in db.ArCustomer on sm.Customer equals c.Customer
                           join dm in db.DelMaster on c.Customer equals dm.Customer into dm_join
                           from dm in dm_join.DefaultIfEmpty()
                           where sm.SalesOrder == sDocID
                           || (sm.CustomerPoNumber).Trim().Replace(" ", "") == sDocID
                           || db.Udf_IsNumeric(sDocID) == 1 && dm.DeliveryID.ToString() == sDocID
                           select new
                           {
                               sm.SalesOrder,
                               sm.CustomerPoNumber,
                               Customer = c.Customer,
                               Name = c.Name,
                               ShortName = c.ShortName,
                               ExemptFinChg = c.ExemptFinChg,
                               MaintHistory = c.MaintHistory,
                               CustomerType = c.CustomerType,
                               MasterAccount = c.MasterAccount,
                               StoreNumber = c.StoreNumber,
                               PrtMasterAdd = c.PrtMasterAdd,
                               CreditStatus = c.CreditStatus,
                               CreditLimit = (decimal?)c.CreditLimit,
                               InvoiceCount = (decimal?)c.InvoiceCount,
                               Salesperson = c.Salesperson,
                               Salesperson1 = c.Salesperson1,
                               Salesperson2 = c.Salesperson2,
                               Salesperson3 = c.Salesperson3,
                               PriceCode = c.PriceCode,
                               CustomerClass = c.CustomerClass,
                               Branch = c.Branch,
                               TermsCode = c.TermsCode,
                               InvDiscCode = c.InvDiscCode,
                               BalanceType = c.BalanceType,
                               Area = c.Area,
                               LineDiscCode = c.LineDiscCode,
                               TaxStatus = c.TaxStatus,
                               TaxExemptNumber = c.TaxExemptNumber,
                               SpecialInstrs = c.SpecialInstrs,
                               PriceCategoryTable = c.PriceCategoryTable,
                               DateLastSale = (DateTime?)c.DateLastSale,
                               DateLastPay = (DateTime?)c.DateLastPay,
                               OutstOrdVal = (decimal?)c.OutstOrdVal,
                               NumOutstOrd = (decimal?)c.NumOutstOrd,
                               Telephone = c.Telephone,
                               Contact = c.Contact,
                               AddTelephone = c.AddTelephone,
                               Fax = c.Fax,
                               Telex = c.Telex,
                               TelephoneExtn = c.TelephoneExtn,
                               Currency = c.Currency,
                               UserField1 = c.UserField1,
                               UserField2 = (decimal?)c.UserField2,
                               GstExemptFlag = c.GstExemptFlag,
                               GstExemptNum = c.GstExemptNum,
                               GstLevel = c.GstLevel,
                               DetailMoveReqd = c.DetailMoveReqd,
                               InterfaceFlag = c.InterfaceFlag,
                               ContractPrcReqd = c.ContractPrcReqd,
                               BuyingGroup1 = c.BuyingGroup1,
                               BuyingGroup2 = c.BuyingGroup2,
                               BuyingGroup3 = c.BuyingGroup3,
                               BuyingGroup4 = c.BuyingGroup4,
                               BuyingGroup5 = c.BuyingGroup5,
                               StatementReqd = c.StatementReqd,
                               BackOrdReqd = c.BackOrdReqd,
                               ShippingInstrs = c.ShippingInstrs,
                               StateCode = c.StateCode,
                               DateCustAdded = (DateTime?)c.DateCustAdded,
                               StockInterchange = c.StockInterchange,
                               MaintLastPrcPaid = c.MaintLastPrcPaid,
                               IbtCustomer = c.IbtCustomer,
                               SoDefaultDoc = c.SoDefaultDoc,
                               CounterSlsOnly = c.CounterSlsOnly,
                               PaymentStatus = c.PaymentStatus,
                               Nationality = c.Nationality,
                               HighestBalance = (decimal?)c.HighestBalance,
                               CustomerOnHold = c.CustomerOnHold,
                               InvCommentCode = c.InvCommentCode,
                               EdiSenderCode = c.EdiSenderCode,
                               RelOrdOsValue = (decimal?)c.RelOrdOsValue,
                               EdiFlag = c.EdiFlag,
                               SoDefaultType = c.SoDefaultType,
                               Email = c.Email,
                               ApplyOrdDisc = c.ApplyOrdDisc,
                               ApplyLineDisc = c.ApplyLineDisc,
                               FaxInvoices = c.FaxInvoices,
                               FaxStatements = c.FaxStatements,
                               HighInvDays = (decimal?)c.HighInvDays,
                               HighInv = c.HighInv,
                               DocFax = c.DocFax,
                               DocFaxContact = c.DocFaxContact,
                               SoldToAddr1 = c.SoldToAddr1,
                               SoldToAddr2 = c.SoldToAddr2,
                               SoldToAddr3 = c.SoldToAddr3,
                               SoldToAddr4 = c.SoldToAddr4,
                               SoldToAddr5 = c.SoldToAddr5,
                               SoldPostalCode = c.SoldPostalCode,
                               ShipToAddr1 = c.ShipToAddr1,
                               ShipToAddr2 = c.ShipToAddr2,
                               ShipToAddr3 = c.ShipToAddr3,
                               ShipToAddr4 = c.ShipToAddr4,
                               ShipToAddr5 = c.ShipToAddr5,
                               ShipPostalCode = c.ShipPostalCode,
                               State = c.State,
                               CountyZip = c.CountyZip,
                               City = c.City,
                               State1 = c.State1,
                               CountyZip1 = c.CountyZip1,
                               City1 = c.City1,
                               DefaultOrdType = c.DefaultOrdType,
                               PoNumberMandatory = c.PoNumberMandatory,
                               CreditCheckFlag = c.CreditCheckFlag,
                               CompanyTaxNumber = c.CompanyTaxNumber,
                               DeliveryTerms = c.DeliveryTerms,
                               TransactionNature = (decimal?)c.TransactionNature,
                               DeliveryTermsC = c.DeliveryTermsC,
                               TransactionNatureC = (decimal?)c.TransactionNatureC,
                               RouteCode = c.RouteCode,
                               FaxQuotes = c.FaxQuotes,
                               RouteDistance = (decimal?)c.RouteDistance,
                               TpmCustomerFlag = c.TpmCustomerFlag,
                               SalesWarehouse = c.SalesWarehouse,
                               TpmPricingFlag = c.TpmPricingFlag,
                               ArStatementNo = c.ArStatementNo,
                               TpmCreditCheck = c.TpmCreditCheck,
                               WholeOrderShipFlag = c.WholeOrderShipFlag,
                               MinimumOrderValue = (decimal?)c.MinimumOrderValue,
                               MinimumOrderChgCod = c.MinimumOrderChgCod,
                               UkVatFlag = c.UkVatFlag,
                               UkCurrency = c.UkCurrency,
                               TimeStamp = c.TimeStamp,
                               DateDelivered =
                               dm.DateDelivered == null ? "" : null
                           }).Take(1);
                dt = SharedFunctions.ToDataTable(db, qry);
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
    }
    public static DataTable GetDeliveryInfo(int iDeliveryID)
    {
        DataTable dt = new DataTable();
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from dm in db.DelMaster
                       where dm.DeliveryID == iDeliveryID
                       select dm);

            try
            {
                dt = ToDataTable(db, qry);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                dt.Dispose();
            }
            return dt;
        }
    }
    public static bool DocumentAlreadyExists(string sDocumentName)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from sd in db.DocScanUploadHistory
                       where sd.DocName == sDocumentName
                       select sd);
            if (qry.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public static string GetCustomerName(string sCustomer)
    {
        string sFullName = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from c in db.ArCustomer
                       where c.Customer == sCustomer
                       select new
                       {
                           c.Name
                       });
            foreach (var a in qry)
            {
                sFullName = a.Name;
            }
            return sFullName;
        }
    }
    public static string GetCustomerEmail(string sCustomer)
    {
        string sEmail = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from c in db.ArCustomer
                       where c.Customer == sCustomer
                       && c.Email != ""
                       && c.Email.Contains("@")
                       select new
                       {
                           c.Email
                       });
            foreach (var a in qry)
            {
                if (a.Email.Contains(";"))
                {
                    sEmail = a.Email.Substring(0, a.Email.IndexOf(";"));
                }
                else
                {
                    sEmail = a.Email;
                }
               
            }
            return sEmail;
        }
    }
    public static string GetInternalPO(int iDeliveryID)
    {
        string sInternalPO = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from dm in db.DelMaster
                       where dm.DeliveryID == iDeliveryID
                       select new
                       {
                           dm.InternalPoNumber
                       });
            foreach (var a in qry)
            {
                sInternalPO = a.InternalPoNumber;
            }
            return sInternalPO;
        }
    }
    public static string GetOldestTrnDateInDatabase()
    {
        string sOldestTrnDate = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from s in
                    (from s in db.ArSalesMove
                     select new
                     {
                         s.TrnDate,
                         Dummy = "x"
                     })
                       group s by new { s.Dummy } into g
                       select new
                       {
                           MaxDate = (DateTime?)g.Min(p => p.TrnDate)
                       });
            foreach (var a in qry)
            {
                sOldestTrnDate = ((DateTime)a.MaxDate).ToShortDateString();
            }
            return sOldestTrnDate;
        }
    }
    public static string GetStockCodeDesc(string _StockCode)
    {
        string sDesc = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query =
            ((from wm in db.WipMaster
              where
                   wm.StockCode.Replace("'", "") == _StockCode.Replace("'", "")
              group wm by new
              {
                  wm.StockCode,
                  wm.StockDescription
              } into g
              orderby
                g.Max(p => p.JobTenderDate) descending
              select new
              {
                  g.Key.StockCode,
                  g.Key.StockDescription
              }).Take(1));
            foreach (var r in query)
            {
                sDesc = r.StockDescription;
            }
            int iCount = query.Count();

            if (iCount < 1)
            {
                var query1 =
                    (from im in db.InvMaster
                     where
                       im.StockCode.Replace("'", "") == _StockCode.Replace("'", "")
                     group im by new
                     {
                         im.StockCode,
                         im.Description
                     } into g
                     select new
                     {
                         g.Key.StockCode,
                         g.Key.Description
                     });
                foreach (var r in query1)
                {
                    sDesc = r.Description;
                }
            }

            return sDesc;
        }
    }
    public static DataTable GetProductionSchedule(string sStockCode)
    {//Changed from JobStartDate to JobDeliveryDate...10-3-2017...
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = new DataTable();
            try
            {

                var query = (from w in db.WipMaster
                             where
                               w.StockCode.Trim() == sStockCode &&
                               w.ActCompleteDate == null
                             group w by new
                             {
                                 w.StockCode,
                                 w.JobDeliveryDate,
                                 w.QtyToMake
                             } into g
                             orderby g.Key.JobDeliveryDate descending
                             select new
                             {
                                 g.Key.StockCode,
                                 ScheduledDate = g.Key.JobDeliveryDate,
                                 ScheduledQty = g.Key.QtyToMake
                             });
                dt = SharedFunctions.ToDataTable(db, query);

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
    }
    public static DataTable GetProductionScheduleUnGrouped(string sStockCode)
    {//Changed from JobStartDate to JobDeliveryDate...10-3-2017...
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = new DataTable();
            try
            {

                var query = (from w in db.WipMaster
                             where
                               w.StockCode.Trim() == sStockCode &&
                               w.ActCompleteDate == null
                             orderby w.JobDeliveryDate
                             select new
                             {
                                 w.StockCode,
                                 ScheduledDate = w.JobDeliveryDate,
                                 ScheduledQty = w.QtyToMake
                             });
                dt = SharedFunctions.ToDataTable(db, query);

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
    }
    public static DataTable GetSaleOrderComments(int iSalesOrder)
    {//Changed from JobStartDate to JobDeliveryDate...10-3-2017...
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = new DataTable();
            try
            {
                var query = ( from sc in db.SorComments
                              where Convert.ToInt32(sc.SalesOrder) == iSalesOrder
                              orderby sc.DateAdded descending
                              select new
                              {
                                  sc.Comment
                              }).Take(1);
                dt = SharedFunctions.ToDataTable(db, query);

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
    }
    public static DataTable GetTotalQtyBreakdown(int iSalesOrder)
    {//Changed from JobStartDate to JobDeliveryDate...10-3-2017...
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            DataTable dt = new DataTable();
            try
            {
                var query = (from sd in db.SorDetail
                             where
                               Convert.ToInt32(sd.SalesOrder) == iSalesOrder
                               && (sd.MShipQty + sd.MBackOrderQty) > 0
                             group sd by new
                             {
                                 sd.MStockCode,
                                 sd.MStockDes
                             } into g
                             orderby
                               g.Key.MStockCode
                             select new
                             {
                                 StockCode = g.Key.MStockCode,
                                 Description = g.Key.MStockDes,
                                 Qty = (decimal?)Convert.ToDecimal(g.Sum(p => p.MShipQty + p.MBackOrderQty))
                             });
                dt = SharedFunctions.ToDataTable(db, query);

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
    }
    public static string GetEmailOfFirstOperator(string sSalesOrder)
    {
        string sEmail = "";
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var qry = (from u in db.AdmOperator_
                       join s in db.SorFirstOperator on u.Operator equals s.FirstOperator
                       where s.SalesOrder == sSalesOrder
                       select new
                       {
                           u.Email,

                       });
            foreach (var a in qry)
            {
                sEmail = a.Email.Trim();
            }
            return sEmail;
        }
    }
    public static DateTime? GetMaxProductionDateBySalesOrder(int iSalesOrder)
    {//Changed from JobStartDate to JobDeliveryDate...10-3-2017...
        DateTime? dtMaxProductionDate = null;
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {
                var query = (from wm in
                                    (from wm in db.WipMaster
                                     where
                                       wm.ActCompleteDate == null &&
                                         (from sd in db.SorDetail
                                          where
                                           Convert.ToInt32(sd.SalesOrder) == iSalesOrder
                                          select new
                                          {
                                              sd.MStockCode
                                          }).Contains(new { MStockCode = wm.StockCode })
                                     select new
                                     {
                                         wm.JobDeliveryDate,
                                         Dummy = "x"
                                     })
                             group wm by new { wm.Dummy } into g
                             select new
                             {
                                 ProductionDate = (DateTime?)g.Max(p => p.JobDeliveryDate)
                             });
                foreach (var a in query)
                {
                    dtMaxProductionDate = a.ProductionDate;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return dtMaxProductionDate;
        }
    }
    public static DateTime? GetMaxProductionDateBySalesOrderNew(int iSalesOrder)
    {
        DateTime? dtMaxProductionDate = null;
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            try
            {
                var query = (from sr in
                            (from sr in db.SorReadyStatusAlerts
                             where
                               Convert.ToInt32(sr.SalesOrder) == iSalesOrder
                              &&  db.Udf_IsDate(sr.ReadyDateOrStatusNew) == true
                             select new
                             {
                                 sr.ReadyDateOrStatusNew,
                                 Dummy = "x"
                             })
                             group sr by new { sr.Dummy } into g
                             
                             select new
                             {
                                 MaxProductionDate = g.Max(p => p.ReadyDateOrStatusNew)
                             });
                foreach (var a in query)
                {
                    dtMaxProductionDate =  Convert.ToDateTime(a.MaxProductionDate);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return dtMaxProductionDate;
        }
    }
    public static bool IsOrderStatusTBD(int iSalesOrder, string sStockCode)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from sr in db.SorReadyStatusAlerts
                         where
                           Convert.ToInt32(sr.SalesOrder) == iSalesOrder
                           && sr.StockCode == sStockCode
                           && sr.ReadyDateOrStatusNew == "TBD"
                         select new
                         {
                             sr.ReadyDateOrStatusNew
                         });

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
    public static bool DoesOrderContainTBDs(int iSalesOrder)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from sr in db.SorReadyStatusAlerts
                         join sd in db.SorDetail on new { sr.SalesOrder, sr.StockCode }  equals new { sd.SalesOrder, StockCode = sd.MStockCode }
                         where
                           Convert.ToInt32(sr.SalesOrder) == iSalesOrder &&
                           sr.ReadyDateOrStatusNew == "TBD"
                         select new
                         {
                             sr.ReadyDateOrStatusNew
                         });

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
    public static bool IsCompton(int iUserID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            var query = (from u in db.WipUsers
                         where u.UserID == iUserID
                         && u.Compton == "Y"
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
    public static List<string> GetEndCustomer()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            List<string> lEndCustomers = new List<string>();
            var query = (from e in db.ArNonCustomer
                         select new
                         {
                             e.NonCustomerID
                         });

            foreach (var a in query)
            {
                lEndCustomers.Add(a.NonCustomerID.ToString());
            }
            return lEndCustomers;
        }
    }
    public static List<string> GetGrouping()
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            List<string> lGroupMembers = new List<string>();
            var query = (from e in db.ArGroups
                         select new
                         {
                             e.GroupID
                         }).Distinct();

            foreach (var a in query)
            {
                lGroupMembers.Add(a.GroupID.ToString());
            }
            return lGroupMembers;
        }
    }
    public static List<string> GetMessageGroupEmails(int iWipMessageGroupID, int iSubGroupEvenOdd)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            List<string> lGroupMembers = new List<string>();
            switch (iSubGroupEvenOdd)
            {
                case 0://NA..
                    var query = (from mga in db.WipMessageGroupAssignments
                                 where
                                   mga.WipMessageGroupID == iWipMessageGroupID &&
                                   mga.SubGroupOddEven == null &&
                                   mga.WipUsers.Status == 1
                                 select new
                                 {
                                     mga.WipUsers.Email
                                 });

                    foreach (var a in query)
                    {
                        lGroupMembers.Add(a.Email);
                    }
                    break;
                case 1://Even Orders...
                    var query1 = (from mga in db.WipMessageGroupAssignments
                                 where
                                   mga.WipMessageGroupID == iWipMessageGroupID &&
                                   mga.SubGroupOddEven == 1 &&
                                   mga.WipUsers.Status == 1
                                  select new
                                 {
                                     mga.WipUsers.Email
                                 });

                    foreach (var a in query1)
                    {
                        lGroupMembers.Add(a.Email);
                    }
                    break;
                case 2://Odd Orders...
                    var query2 = (from mga in db.WipMessageGroupAssignments
                                 where
                                   mga.WipMessageGroupID == iWipMessageGroupID &&
                                   mga.SubGroupOddEven == 2 &&
                                   mga.WipUsers.Status == 1
                                  select new
                                 {
                                     mga.WipUsers.Email
                                 });

                    foreach (var a in query2)
                    {
                        lGroupMembers.Add(a.Email);
                    }

                    break;
            }
            
            return lGroupMembers;
        }
    }
    public static List<string> GetSecurityGroupMembers(int iWipSecurityGroupID)
    {
        using (FELBRO db = new FELBRO(System.Configuration.ConfigurationManager.ConnectionStrings["SQL"].ConnectionString))
        {
            List<string> lGroupMembers = new List<string>();
           
                    var query = (from mga in db.WipSecurityGroupAssignments
                                 where
                                   mga.WipSecurityGroupID == iWipSecurityGroupID
                                   &&  mga.WipUsers.Status == 1
                                 select new
                                 {
                                     mga.WipUserID
                                 });

                    foreach (var a in query)
                    {
                        lGroupMembers.Add(a.WipUserID.ToString());
                    }                  

            return lGroupMembers;
        }
    }

    #endregion

    #region Subs
    public static void ResizeImage(string OriginalFile, string NewFile, int NewWidth, int MaxHeight, bool OnlyResizeIfWider)
    {
        try
        {


            System.Drawing.Image FullsizeImage = System.Drawing.Image.FromFile(OriginalFile);

            // Prevent using images internal thumbnail
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
            FullsizeImage.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);

            if (OnlyResizeIfWider)
            {
                if (FullsizeImage.Width <= NewWidth)
                {
                    NewWidth = FullsizeImage.Width;
                }
            }

            int NewHeight = FullsizeImage.Height * NewWidth / FullsizeImage.Width;
            if (NewHeight > MaxHeight)
            {
                // Resize with height instead
                NewWidth = FullsizeImage.Width * MaxHeight / FullsizeImage.Height;
                NewHeight = MaxHeight;
            }

            System.Drawing.Image NewImage = FullsizeImage.GetThumbnailImage(NewWidth, NewHeight, null, IntPtr.Zero);

            // Clear handle to original file so that we can overwrite it if necessary
            FullsizeImage.Dispose();

            // Save resized picture
            NewImage.Save(NewFile);
        }
        catch (Exception ex)
        {

            Debug.WriteLine(ex.ToString());
        }
    }
    public static void Check_Session(string sessionName)
    {
        if (System.Web.HttpContext.Current.Session[sessionName] == null)
            System.Web.HttpContext.Current.Response.Redirect("Default.aspx");
    }
    public static void SetupMenu(System.Web.UI.WebControls.MenuEventArgs e)
    {

        int iUserID = 0;
        if (HttpContext.Current.Session["UserID"] != null)
        {
            iUserID = Convert.ToInt32(HttpContext.Current.Session["UserID"]);

        }
        else
        {
            e.Item.Enabled = false;
            e.Item.Text = "";
            e.Item.ImageUrl = "";
            return;
        }
        string sText = e.Item.Text;
        int? iAdminID = GetAdminID(iUserID);

        if (!IsAccessGranted(iAdminID, sText))
        {
            e.Item.Enabled = false;
            e.Item.Text = "";
            e.Item.ImageUrl = "";
        }

    }
 

    #endregion
}

