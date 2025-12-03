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

[Serializable]
public class StockCodeDescriptionList
{
    [Serializable]
    public class StockCodeDescription
    {
 
        public string StockCode { get; set; }
        public string Desc { get; set; }
        

    }
}
public class ProductionQuantities
{
    public string StockCode { get; set; }
    public decimal ProductionQuantityAvailables { get; set; }
}