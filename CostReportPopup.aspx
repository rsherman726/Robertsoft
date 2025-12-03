<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CostReportPopup.aspx.cs" Inherits="CostReportPopup" %>

<%@ Register src="ucCostReport.ascx" tagname="ucCostReport" tagprefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cost Report</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.js"></script>
    <script type="text/javascript" src="Scripts/kissy-min.js"></script>
</head>
<body>
    <form id="form1" runat="server"> 
        <asp:ScriptManager ID="ScriptManager1" runat="server"  AsyncPostBackTimeout="3600"></asp:ScriptManager>
        <div>
            <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
                <ContentTemplate>
                    <div style="position: fixed; right: 50px; height: 40px; width: 150px; z-index: 10000;">
                        <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" AssociatedUpdatePanelID="UpdatePanelPromo" DisplayAfter="1">

                            <ProgressTemplate>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td align="Left" style="width: 12px">
                                                <img src="Images/loader.gif" alt="" style="border: thin solid #000000" />
                                            </td>
                                            <td align="left"><span style="color: #ffffff">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing <span class="">....</span> </strong></span></span></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>

                    <div>
                        
                        <uc1:ucCostReport ID="ucCostReport1" runat="server" />
                        
                    </div>


                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
