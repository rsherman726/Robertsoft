<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EmployeeAdmin.aspx.cs" Inherits="EmployeeAdmin" %>

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
                        <div style="position: fixed; right: 50px; height: 40px; width: 150px; z-index: 10000;">
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
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td class="hdr" align="center">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                        Text="Employee Admin" ForeColor="Black"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlMain" runat="server" Width="900px">
                                        <table width="900" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                            align="center">
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
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
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="True" Font-Size="12pt" OnTextChanged="txtSearch_TextChanged" ToolTip="This is an Auto Complete text box. Just fill in first, first-middle, or first-middle-lastname of the user's name." BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server"
                                                        CompletionSetCount="25" CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListUserName"
                                                        ServicePath="" TargetControlID="txtSearch" UseContextKey="True">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                </td>
                                                <td align="left">
                                                    <asp:Button ID="ibnSearch" runat="server" CssClass="btn btn-primary" OnClick="ibnSearch_Click" Text="search" ToolTip="Fill in any part of the name and click search to find the user." />
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="center">
                                                    <asp:ListBox ID="lbEmployees" runat="server" AutoPostBack="True" Font-Bold="True" OnSelectedIndexChanged="lbEmployees_SelectedIndexChanged" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black" Height="175px">
                                                        <asp:ListItem Selected="True" Value="0">--First search for user--</asp:ListItem>
                                                    </asp:ListBox>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="FIRST NAME:" ForeColor="Black"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtFirstName" runat="server" Font-Size="12pt" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtFirstName" EnableClientScript="False" ErrorMessage="First name is required." ValidationGroup="emp"><img src="Images/arrow_alert.gif" alt="arrow"/></asp:RequiredFieldValidator>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtLastName" EnableClientScript="False" ErrorMessage="Last name is required." ValidationGroup="emp"><img src="Images/arrow_alert.gif" alt="arrow"/></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label2234" runat="server" Font-Bold="True" Text="MIDDLE NAME:" ForeColor="Black"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtMiddleInitial" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" ForeColor="Black"></asp:TextBox>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label2235" runat="server" Font-Bold="True" Text="LAST NAME:" ForeColor="Black"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtLastName" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" ForeColor="Black"></asp:TextBox>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">&nbsp;</td>
                                                <td align="center">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label2233" runat="server" Font-Bold="True" Text="STATUS:" ForeColor="Black"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:DropDownList ID="ddlStatus" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" ForeColor="Black">
                                                        <asp:ListItem Selected="True" Value="-1">--Select a Status--</asp:ListItem>
                                                        <asp:ListItem Value="1">Active</asp:ListItem>
                                                        <asp:ListItem Value="0">De-Activated</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="8pt" ForeColor="Red"></asp:Label>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td></td>
                                                <td style="text-align: center">
                                                    <asp:Button ID="ibnSave" runat="server" Text="save" OnClick="ibnSave_Click" ValidationGroup="emp" Enabled="False" CssClass="btn btn-primary" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnSave_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnSave"></ajaxToolkit:ConfirmButtonExtender>
                                                    <asp:Button ID="ibnDelete" runat="server" Enabled="False" Text="delete" OnClick="ibnDelete_Click" ValidationGroup="emp" CssClass="btn btn-primary" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnDelete"></ajaxToolkit:ConfirmButtonExtender>
                                                    <asp:Button ID="ibnAdd" runat="server" Enabled="False" Text="add" OnClick="ibnAdd_Click" ValidationGroup="emp" CssClass="btn btn-primary" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnAdd_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnAdd"></ajaxToolkit:ConfirmButtonExtender>
                                                </td>
                                                <td align="center"></td>
                                                <td align="center"></td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="center">
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Size="9pt" ForeColor="Red" HeaderText="Needs Attention" ValidationGroup="emp" />
                                                </td>
                                                <td align="center">&nbsp;</td>
                                                <td align="center">&nbsp;</td>
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
