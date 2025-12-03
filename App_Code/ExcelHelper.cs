using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Data.Linq;
using System.Collections.Generic;
using System.Data.Linq.SqlClient;
using System.Drawing;
using System.Text;
using ClosedXML.Excel;

/// <summary>
/// Summary description for ExcelHelper
/// </summary>
public class ExcelHelper
{
    //Row limits older Excel version per sheet
    const int rowLimit = 65000;

    private static string getWorkbookTemplate()
    {
        var sb = new StringBuilder();
        sb.Append("<xml version>\r\n<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"\r\n");
        sb.Append(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"\r\n xmlns:x=\"urn:schemas- microsoft-com:office:excel\"\r\n	xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">\r\n");
        sb.Append(" <Styles>\r\n <Style ss:ID=\"Default\" ss:Name=\"Normal\">\r\n <Alignment ss:Vertical=\"Bottom\"/>\r\n <Borders/>");
        sb.Append("\r\n <Font/>\r\n <Interior/>\r\n <NumberFormat/>\r\n	<Protection/>\r\n </Style>\r\n	<Style ss:ID=\"BoldColumn\">\r\n <Font ");
        sb.Append("x:Family=\"Swiss\" ss:Bold=\"1\"/>\r\n </Style>\r\n<Style ss:ID=\"s62\">\r\n <NumberFormat");
        sb.Append(" ss:Format=\"@\"/>\r\n </Style>\r\n <Style ss:ID=\"Decimal\">\r\n <NumberFormat ss:Format=\"0.0000\"/>\r\n </Style>\r\n ");
        sb.Append("<Style ss:ID=\"Integer\">\r\n <NumberFormat ss:Format=\"0\"/>\r\n </Style>\r\n <Style ss:ID=\"DateLiteral\">\r\n <NumberFormat ");
        sb.Append("ss:Format=\"mm/dd/yyyy;@\"/>\r\n </Style>\r\n <Style ss:ID=\"s28\">\r\n");
        sb.Append("<Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Top\" ss:ReadingOrder=\"LeftToRight\" ss:WrapText=\"1\"/>\r\n");
        sb.Append("<Font x:CharSet=\"1\" ss:Size=\"9\" ss:Color=\"#808080\" ss:Underline=\"Single\"/>\r\n");
        sb.Append("<Interior ss:Color=\"#FFFFFF\" ss:Pattern=\"Solid\"/></Style>\r\n</Styles>\r\n {0}</Workbook>");
        return sb.ToString();
    }

    private static string replaceXmlChar(string input)
    {
        char DoubleQuotes = '"';
        input = input.Replace("&", "&");
        input = input.Replace("<", "<");
        input = input.Replace(">", ">");
        input = input.Replace("\"", DoubleQuotes.ToString());
        input = input.Replace("'", "'");
        return input;
    }

    private static string getWorksheets(DataSet source)
    {
        var sw = new StringWriter();
        if (source == null || source.Tables.Count == 0)
        {
            sw.Write("<Worksheet ss:Name=\"Sheet1\"><Table><Row><Cell  ss:StyleID=\"s62\"><Data ss:Type=\"String\"></Data></Cell></Row></Table></Worksheet>");
            return sw.ToString();
        }
        foreach (DataTable dt in source.Tables)
        {
            if (dt.Rows.Count == 0)
                sw.Write("<Worksheet ss:Name=\"" + replaceXmlChar(dt.TableName) + "\"><Table><Row><Cell  ss:StyleID=\"s62\"><Data ss:Type=\"String\"></Data></Cell></Row></Table></Worksheet>");
            else
            {
                //write each row data
                var sheetCount = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((i % rowLimit) == 0)
                    {
                        //add close tags for previous sheet of the same data table
                        if ((i / rowLimit) > sheetCount)
                        {
                            sw.Write("</Table></Worksheet>");
                        }
                        sheetCount = (i / rowLimit);

                        sw.Write("<Worksheet ss:Name=\"" + replaceXmlChar(dt.TableName) + (((i / rowLimit) == 0) ? "" : Convert.ToString(i / rowLimit)) + "\"><Table>");
                        //write column name row
                        sw.Write("<Row>");
                        foreach (DataColumn dc in dt.Columns)
                            sw.Write(string.Format("<Cell ss:StyleID=\"BoldColumn\"><Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlChar(dc.ColumnName)));
                        sw.Write("</Row>\r\n");

                    }
                    sw.Write("<Row>\r\n");
                    foreach (DataColumn dc in dt.Columns)
                        sw.Write(string.Format("<Cell ss:StyleID=\"s62\"><Data ss:Type=\"String\">{0}</Data></Cell>", replaceXmlChar(dt.Rows[i][dc.ColumnName].ToString())));
                    sw.Write("</Row>\r\n");
                }
                sw.Write("</Table></Worksheet>");
            }
        }

        return sw.ToString();
    }
    public static string GetExcelXml(DataTable dtInput, string filename)
    {
        var excelTemplate = getWorkbookTemplate();
        var ds = new DataSet();
        ds.Tables.Add(dtInput.Copy());
        var worksheets = getWorksheets(ds);
        var excelXml = string.Format(excelTemplate, worksheets);
        return excelXml;
    }

    public static string GetExcelXml(DataSet dsInput, string filename)
    {
        var excelTemplate = getWorkbookTemplate();
        var worksheets = getWorksheets(dsInput);
        var excelXml = string.Format(excelTemplate, worksheets);
        return excelXml;
    }

    public static void ToExcel(DataSet dsInput, string sFileName, HttpResponse Response)
    {
        //var excelXml = GetExcelXml(dsInput, sFileName);

        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dsInput);
            Response.Clear();
            Response.Buffer = true;
            Response.Charset = "";
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + sFileName + ".xlsx");

            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                try
                {
                    wb.SaveAs(MyMemoryStream, false);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.ToString());
                }


                MyMemoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }
    }
    public static void ToExcel(DataSet dsInput, string sFileName, HttpResponse Response, string sOutput)
    {
        //var excelXml = GetExcelXml(dsInput, sFileName);

        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dsInput);


            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                try
                {
                    wb.SaveAs(sOutput);
                }
                catch (Exception ex)
                {

                    Debug.WriteLine(ex.ToString());
                }

            }
        }
    }
    public static MemoryStream ToExcelMemoryStream(DataSet dsInput)
    {
        using (XLWorkbook wb = new XLWorkbook())
        {
            wb.Worksheets.Add(dsInput);

            MemoryStream MyMemoryStream = new MemoryStream();

            try
            {
                wb.SaveAs(MyMemoryStream, false);
                MyMemoryStream.Position = 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            return MyMemoryStream;


        }
    }
    public static void ToExcel(DataTable dtInput, string filename, HttpResponse response)
    {
        var ds = new DataSet();
        ds.Tables.Add(dtInput.Copy());
        ToExcel(ds, filename, response);
    }
}