using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for Common
/// </summary>
public class Common
{
    public static string DW_ProductName = ProductInfo.DW_ProductName;
    public static string DW_ProductNameWithoutSpace = ProductInfo.DW_ProductNameWithoutSpace;
    public static string DW_ProductNameAbbreviated = ProductInfo.DW_ProductNameAbbreviated;
    public static string DW_Title = ProductInfo.DW_Title;
    public static string DW_Description = ProductInfo.DW_Description;
    public static string DW_Keyword = ProductInfo.DW_Keyword;
    public static string DW_Overview = ProductInfo.DW_Overview;
    public static string DW_Download = ProductInfo.DW_Download;
    public static string DW_SampleDownload = ProductInfo.DW_SampleDownload;
    public static string DW_OnlineDemo = ProductInfo.DW_OnlineDemo;
    public static string DW_PostHelp = ProductInfo.DW_PostHelp;
    public static string DW_PutHelp = ProductInfo.DW_PutHelp;
    public static string DW_FTPUploadHelp = ProductInfo.DW_FTPUploadHelp;
    public static string DW_LogoName = ProductInfo.DW_LogoName;
    public static string DW_LogoImage = ProductInfo.DW_LogoImage;
    public static string DW_ContainerHeight = ProductInfo.DW_ContainerHeight;



    //Sample:
    public static string DW_TitleLogo = "<div style=\"float:left; padding-top:15px; width:525px; margin-left:25px;\">" +
          "<span><a href=\"http://www.dynamsoft.com/\">" +
          "<img src=\"Images/logo.gif\" alt=\"Dynamsoft: provider of version control solution and TWAIN SDK\" style='padding: 12px 0 0;' " +
          "name=\"logo\" border=\"0\" align=\"left\" id=\"logo\" title=\"Dynamsoft: provider of version control solution and TWAIN SDK\" /></a></span> " +
          "<span style='border-left:1px solid #CCC;margin: 0 0 0 10px;padding: 50px 0 0 10px;'><a href=\"" + Common.DW_Overview + "\"> " +
          "<img alt = \"" + Common.DW_LogoName + "\" style=\"border:none; \" src=\"Images/" + DW_LogoImage + "\"/></a></span> " +
          "</div>";
    public static string DW_LiveChatJS = "";
    //public static string DW_SaveDB = DW_ProductNameWithoutSpace;
    //public static string DW_SaveTable = "tbl" + DW_ProductNameWithoutSpace;
    public static string DW_SaveDB = "DemoWebTwain";
    public static string DW_SaveTable = "tblWebTwain4";
    public static string DW_ConnString = "Server=localhost;Database=" + Common.DW_SaveDB + ";Integrated Security=SSPI";
    //public static string DW_ConnString = "";

}
