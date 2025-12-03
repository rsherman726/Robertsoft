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

public partial class Viewer : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {


        if (!Page.IsPostBack)
        {
            if (Request.QueryString["folder"] != null && Request.QueryString["file"] != null)
            {
                string sFolder = Request.QueryString["folder"].ToString();

                string sFile = Request.QueryString["file"].ToString();

                string sPath = "";
                sPath = "~/Images/Documents/" + sFolder + "/" + Path.GetFileName(sFile);
                if (Path.GetExtension(sFile).ToUpper() == ".PDF")
                {
                    Literal1.Text = "<iframe frameborder='0' scrolling='no' width='1024' height='800' src='" + "Images/Documents/" + sFolder + "/"+ Path.GetFileName(sFile) + "' name='I1' id='I1'></iframe>";
                }
                else//Images...
                {
                    imgFile.ImageUrl = sPath;
                }

                UpdatePanelPromo.Update();
            }


        }
    }
}