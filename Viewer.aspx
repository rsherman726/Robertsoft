<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Viewer.aspx.cs" Inherits="Viewer" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server" AsyncPostBackTimeout="10800"></asp:ScriptManager>
        <div>
            <asp:UpdatePanel ID="UpdatePanelPromo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="position: fixed; left: 50%; height: 40px; width: 150px; z-index: 10000;">
                        <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" AssociatedUpdatePanelID="UpdatePanelPromo" DisplayAfter="1">
                            <ProgressTemplate>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td align="left">
                                                <img src="Images/BigSpinner.gif" alt="" style="border: thin solid #FF0000; background-color: GhostWhite" />
                                            </td>
                                            <td align="left" style="background-color: GhostWhite"><span style="color: #ffffff; background-color: GhostWhite">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Arial; background-color: GhostWhite"><strong>processing <span class="" style="background-color: GhostWhite">....</span> </strong></span></span></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal><asp:Image ID="imgFile" runat="server" />
                    <table>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
