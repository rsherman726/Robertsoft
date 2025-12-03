<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AssignEmployees.aspx.cs" Inherits="AssignEmployees" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        /************ PasswordStrength Related Styles ***********************/
        .BoxShadow4 {
            /*Internet Explorer*/
            border-Top-right-radius: 20px;
            border-Top-left-radius: 20px;
            border-bottom-right-radius: 20px;
            border-bottom-left-radius: 20px;
            /*Modzilla*/
            -moz-border-radius-bottomright: 20px;
            -moz-border-radius-bottomleft: 20px;
            -moz-border-radius-topright: 20px;
            -moz-border-radius-topleft: 20px;
            /*Safari*/
            -webkit-border-bottom-left-radius: 20px; /* bottom left corner */
            -webkit-border-bottom-right-radius: 20px; /* bottom right corner */
            -webkit-border-top-left-radius: 20px; /* top left corner */
            -webkit-border-top-right-radius: 20px; /* top right corner */
            /*Opera*/
            -o-border-radius-bottomright: 20px;
            -o-border-radius-bottomleft: 20px;
            -o-border-radius-topright: 20px;
            -o-border-radius-topleft: 20px;
            /* For IE 8 */
            -ms-filter: "progid:DXImageTransform.Microsoft.Shadow(Strength=12, Direction=135, Color='Gray')";
            /* For IE 5.5 - 7 */
            filter: progid:DXImageTransform.Microsoft.Shadow(Strength=12, Direction=135, Color='Gray');
        }

        .TextIndicator_TextBox1 {
            background-color: Gray;
            color: White;
            font-family: Arial;
            font-size: x-small;
            font-style: italic;
            padding: 2px 3px 2px 3px;
        }

        .TextIndicator_TextBox1_Strength1 {
            background-color: Gray;
            color: White;
            font-family: Arial;
            font-size: x-small;
            font-style: italic;
            padding: 2px 3px 2px 3px;
            font-weight: bold;
        }

        .TextIndicator_TextBox1_Strength2 {
            background-color: Gray;
            color: Yellow;
            font-family: Arial;
            font-size: x-small;
            font-style: italic;
            padding: 2px 3px 2px 3px;
            font-weight: bold;
        }

        .TextIndicator_TextBox1_Strength3 {
            background-color: Gray;
            color: #FFCAAF;
            font-family: Arial;
            font-size: x-small;
            font-style: italic;
            padding: 2px 3px 2px 3px;
            font-weight: bold;
        }

        .TextIndicator_TextBox1_Strength4 {
            background-color: Gray;
            color: Aqua;
            font-family: Arial;
            font-size: x-small;
            font-style: italic;
            padding: 2px 3px 2px 3px;
            font-weight: bold;
        }

        .TextIndicator_TextBox1_Strength5 {
            background-color: Gray;
            color: #93FF9E;
            font-family: Arial;
            font-size: x-small;
            font-style: italic;
            padding: 2px 3px 2px 3px;
            font-weight: bold;
        }

        .auto-style1 {
            width: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
        <ContentTemplate>
            <table align="Center" width="900px" class="BoxShadow4" style="background-color: #F0F0F0">
                <tr>
                    <td align="left">

                        <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" AssociatedUpdatePanelID="UpdatePanelPromo" DisplayAfter="1">
                            <ProgressTemplate>
                                <table>
                                    <tbody>
                                        <tr>
                                            <td align="Left" style="width: 12px">
                                                <img src="Images/loader.gif" alt="" />
                                            </td>
                                            <td align="left"><span style="color: #ffffff">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing <span class="">....</span> </strong></span></span></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </ProgressTemplate>
                        </asp:UpdateProgress>

                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table width="100%">
                            <tr>
                                <td class="hdr">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                        Text="Assign Employees" ForeColor="Black"></asp:Label></td>
                            </tr>
                            <tr><td>
                                <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Navy" Text="Managers: To assign an employee to a production line, select the production line from the Production Line dropdown list and the Available Employees list will populate with available employees you can assign to the desired production line."></asp:Label>
                                </td></tr>
                            <tr>
                                <td >
                                    <asp:Panel ID="pnlMain" runat="server" >
                                        <table width="100%" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                            align="center">
                                            
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;<table id="tblAdd" align="center">
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td>&nbsp; </td>
                                                        <td>&nbsp; </td>
                                                        <td align="char">&nbsp; 
                                                            <asp:Label ID="Label2232" runat="server" Font-Bold="True" Font-Names="arial" Text="Production Lines:" ForeColor="Black"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td valign="middle">&nbsp;
                                                            </td>
                                                        <td align="center" valign="middle">&nbsp;
                                                            <asp:DropDownList ID="ddlProductionLines" runat="server" AutoPostBack="True" BackColor="LemonChiffon" Font-Size="8pt" OnSelectedIndexChanged="ddlProductionLines_SelectedIndexChanged" Width="255px" CssClass="form-control" ForeColor="Black">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp; </td>
                                                        <td>&nbsp; </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td>
                                                            <asp:Label ID="Label2233" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Navy" Text="Available Employees"></asp:Label>
                                                        </td>
                                                        <td>&nbsp; </td>
                                                        <td>
                                                            <asp:Label ID="Label2228" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Navy" Text="Employees assigned to above Production line"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            <asp:ListBox ID="lbAvailableEmployees" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="7pt" Height="200px" SelectionMode="Multiple" Width="350px" Font-Bold="True" ForeColor="Black"></asp:ListBox>                                                           
                                                        </td>
                                                        <td><font face="arial">
                                                            <table id="Table4" border="0" cellpadding="1" cellspacing="1">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Button ID="btnAddOne" runat="server" BackColor="Silver" CssClass="btn btn-primary btn-sm" Enabled="False" Font-Bold="True" Font-Size="8pt" ForeColor="Black" OnClick="btnAddOne_Click" Text="Add &gt;" ToolTip="Click to ADD" Width="125px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnAddOne_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to add this employee?" Enabled="True" TargetControlID="btnAddOne">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center"><font face="arial">
                                                                        <asp:Button ID="btnAddAll" runat="server" BackColor="Silver" CssClass="btn btn-primary btn-sm" Enabled="False" Font-Bold="True" Font-Size="8pt" ForeColor="Black" OnClick="btnAddAll_Click" Text="Add All &gt;" ToolTip="Click to ADD" Width="125px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnAddAll_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to add all selected?" Enabled="True" TargetControlID="btnAddAll">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                        </font></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center"><font face="arial">
                                                                        <asp:Button ID="btnRemoveOne" runat="server" BackColor="Silver" CssClass="btn btn-primary btn-sm" Enabled="False" Font-Bold="True" Font-Size="8pt" ForeColor="Black" OnClick="btnRemoveOne_Click" Text="&lt; Remove" ToolTip="Click to REMOVE" Width="125px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnRemoveOne_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to remove this employee?" Enabled="True" TargetControlID="btnRemoveOne">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                        </font></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center"><font face="arial">
                                                                        <asp:Button ID="btnRemoveAll" runat="server" BackColor="Silver" CssClass="btn btn-primary btn-sm" Enabled="False" Font-Bold="True" Font-Size="8pt" ForeColor="Black" OnClick="btnRemoveAll_Click" Text="&lt; Remove All" ToolTip="Click to REMOVE All" Width="125px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnRemoveAll_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to remove all selected employees?" Enabled="True" TargetControlID="btnRemoveAll">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                        </font></td>
                                                                </tr>
                                                            </table>
                                                            </font></td>
                                                        <td><font face="arial">
                                                            <asp:ListBox ID="lbAssignedEmployees" runat="server" BackColor="LemonChiffon" Font-Size="8pt" Height="200px" SelectionMode="Multiple" Width="350px" Font-Bold="True" CssClass="form-control" ForeColor="Black"></asp:ListBox>
                                                            </font></td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td style="color: black">(Use CTRL for Multiple Selection) </td>
                                                        <td align="center">&nbsp; </td>
                                                        <td align="center" style="color: black">(Use CTRL for Multiple Selection) </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td>
                                                            <asp:LinkButton ID="lbnSelectAllAssign" runat="server" OnClick="lbnSelectAllAssign_Click" Font-Bold="True">Select All</asp:LinkButton>
                                                            &nbsp;
                                                            <asp:LinkButton ID="lbnClearAssign" runat="server" OnClick="lbnClearAssign_Click" Font-Bold="True">Clear All</asp:LinkButton>
                                                        </td>
                                                        <td>&nbsp; </td>
                                                        <td>&nbsp;
                                                            <asp:LinkButton ID="lbnSelectAllAssigned" runat="server" OnClick="lbnSelectAllAssigned_Click" Font-Bold="True">Select All</asp:LinkButton>
                                                            &nbsp;
                                                            <asp:LinkButton ID="lbnClearAssigned" runat="server" OnClick="lbnClearAssigned_Click" Font-Bold="True">Clear All</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td>&nbsp; </td>
                                                        <td align="right">&nbsp; </td>
                                                        <td>&nbsp; </td>
                                                    </tr>
                                                    </table>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td class="auto-style1">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                                <td align="center" class="auto-style1">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                </td>
                            </tr>
                        </table>


                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
