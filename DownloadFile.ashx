<%@ WebHandler Language="C#" Class="DownloadFile" %>
<%@ Assembly Name="System.Data, Version=4.0.0.0, Culture=neutral, PublicKeyToken=B77A5C561934E089" %>

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
using System.Web.SessionState;

public class DownloadFile : IHttpHandler, IRequiresSessionState
{


    public void ProcessRequest(HttpContext context)
    {
        System.Web.HttpRequest request = context.Request;
        string sFileName = request.QueryString["fn"];

        DataSet dsInput = (DataSet)context.Session["ds"];
        using (XLWorkbook wb = new XLWorkbook())
        {

            wb.Worksheets.Add(dsInput);
            context.Response.Clear();
            context.Response.Buffer = true;
            context.Response.Charset = "";
            context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            context.Response.AddHeader("content-disposition", "attachment;filename=" + sFileName + ".xlsx");

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


                MyMemoryStream.WriteTo(context.Response.OutputStream);
                context.Response.Flush();
                //context.Response.End();
            }
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}


