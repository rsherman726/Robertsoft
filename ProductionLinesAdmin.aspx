<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProductionLinesAdmin.aspx.cs" Inherits="ProductionLinesAdmin" %>

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
            <table align="Center" width="600" class="BoxShadow4" style="background-color: #F0F0F0">
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
                        <table>
                            <tr>
                                <td class="hdr" align="center">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                        Text="Production Lines Admin" ForeColor="Black"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlMain" runat="server" Width="600px">
                                        <table width="100%" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                            align="center">
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td >&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="center">
                                                    <asp:RadioButtonList ID="rblMode" runat="server" AutoPostBack="True" Font-Bold="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblMode_SelectedIndexChanged" ForeColor="Black" Width="200px">
                                                        <asp:ListItem>&nbsp;Add</asp:ListItem>
                                                        <asp:ListItem Selected="True">&nbsp;Edit</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td >&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Button ID="ibnSearch" runat="server" Text="search" OnClick="ibnSearch_Click" ToolTip="Fill in any part of the name and click search to find the line name." CssClass="btn btn-primary" Width="100px" />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="True" Font-Size="12pt" OnTextChanged="txtSearch_TextChanged" ToolTip="This is an Auto Complete text box. Just fill in part of line name." BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server"
                                                        CompletionSetCount="25" CompletionListItemCssClass="autocomplete_listItem" 
                                                        CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListLineName"
                                                        ServicePath="" TargetControlID="txtSearch" UseContextKey="True">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td >&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlProductionLines" runat="server" AutoPostBack="True" Font-Bold="True" OnSelectedIndexChanged="ddlProductionLines_SelectedIndexChanged" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black">
                                                        <asp:ListItem Selected="True" Value="0">--First search for Production Line--</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td class="auto-style1">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td class="auto-style1">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="NAME:"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtLineName" runat="server" Font-Size="12pt" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtLineName" EnableClientScript="False" ErrorMessage="Line name is required." ValidationGroup="emp"><img src="Images/arrow_alert.gif" alt="arrow"/></asp:RequiredFieldValidator>
                                                </td>
                                                <td >
                                                    &nbsp;</td>
                                            </tr>
                                            
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="8pt" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                                <td align="center" class="auto-style1">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td></td>
                                                <td align="center">
                                                    <asp:Button ID="ibnSave" runat="server" Text="save" OnClick="ibnSave_Click" ValidationGroup="emp" Enabled="False" CssClass="btn btn-primary" Width="100px" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnSave_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnSave">
                                                    </ajaxToolkit:ConfirmButtonExtender>
                                                    <asp:Button ID="ibnDelete" runat="server" Enabled="False" Text="delete" OnClick="ibnDelete_Click" ValidationGroup="emp" CssClass="btn btn-primary" Width="100px" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnDelete">
                                                    </ajaxToolkit:ConfirmButtonExtender>
                                                    <asp:Button ID="ibnAdd" runat="server" Enabled="False" Text="add" OnClick="ibnAdd_Click" ValidationGroup="emp" CssClass="btn btn-primary" Width="100px" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnAdd_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnAdd">
                                                    </ajaxToolkit:ConfirmButtonExtender>
                                                </td>
                                                <td align="center"></td>
                                                <td align="center" class="auto-style1"></td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="center">
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Size="7pt" ForeColor="Red" HeaderText="Needs Attention" ValidationGroup="emp" />
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

