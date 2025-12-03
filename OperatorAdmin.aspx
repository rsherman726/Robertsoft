<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="OperatorAdmin.aspx.cs" Inherits="OperatorAdmin" MaintainScrollPositionOnPostback="true"%>

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
                                        Text="Operator Admin" ForeColor="Black"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="center" class="hdr">
                                    <asp:Panel ID="pnlMain" runat="server" Width="700px">
                                        <table align="center" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite" width="100%">
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td style="width: 250px">&nbsp;</td>
                                                <td style="width: 350px">&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblMode" runat="server" AutoPostBack="True" Font-Bold="True" ForeColor="Black" OnSelectedIndexChanged="rblMode_SelectedIndexChanged" RepeatDirection="Horizontal" Width="200px">
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
                                                <td align="center">
                                                    <asp:Button ID="ibnSearch" runat="server" CssClass="btn btn-primary" OnClick="ibnSearch_Click" Text="search" ToolTip="Fill in any part of the name and click search to find the user." />
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="True" BackColor="LemonChiffon" BorderWidth="2px" CssClass="form-control" ForeColor="Black" OnTextChanged="txtSearch_TextChanged" ToolTip="This is an Auto Complete text box. Just fill in first, first-middle, or first-middle-lastname of the user's name."></asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListUserName" ServicePath="" TargetControlID="txtSearch" UseContextKey="True">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="left">
                                                    <asp:ListBox ID="lbOperators" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="10pt" ForeColor="Black" Height="200px" OnSelectedIndexChanged="lbOperators_SelectedIndexChanged"></asp:ListBox>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="center">
                                                    <asp:Label ID="lblView" runat="server" Font-Bold="True" Font-Italic="True" ForeColor="Black" Text="VIEW OPERATORS:"></asp:Label>
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBoxList ID="cblStatus" runat="server" AutoPostBack="True" Font-Size="9pt" ForeColor="Black" OnSelectedIndexChanged="cblStatus_SelectedIndexChanged" RepeatDirection="Horizontal" Style="text-align: left" Width="225px">
                                                        <asp:ListItem Selected="True" Value="1">&nbsp;Active</asp:ListItem>
                                                        <asp:ListItem Value="0">Not Active</asp:ListItem>
                                                    </asp:CheckBoxList>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="Black" Text="FULL NAME:"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtFullname" runat="server" BackColor="LemonChiffon" BorderWidth="2px" CssClass="form-control" Font-Size="12pt" ForeColor="Black"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtFullname" EnableClientScript="False" ErrorMessage="Full name is required." ValidationGroup="emp"><img src="Images/arrow_alert.gif" alt="arrow"/></asp:RequiredFieldValidator>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label35" runat="server" Font-Bold="True" ForeColor="Black" Text="OPERATOR:"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtOperator" runat="server" BackColor="LemonChiffon" BorderWidth="2px" CssClass="form-control" Font-Size="12pt" ForeColor="Black" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtOperator" EnableClientScript="False" ErrorMessage="Operator is required." ValidationGroup="emp"><img src="Images/arrow_alert.gif" alt="arrow"/></asp:RequiredFieldValidator>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label3" runat="server" Font-Bold="True" ForeColor="Black" Text="EMAIL:"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:TextBox ID="txtEmail" runat="server" BackColor="LemonChiffon" BorderWidth="2px" CssClass="form-control" Font-Size="12pt" ForeColor="Black"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail" EnableClientScript="False" ErrorMessage="Invalid email format." ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="emp" Width="10px"><img src="Images/arrow_alert.gif" 
alt="arrow"/></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label36" runat="server" Font-Bold="True" ForeColor="Black" Text="STATUS:"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlStatus" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Names="Arial" ForeColor="Black" Height="50px">
                                                        <asp:ListItem>Active</asp:ListItem>
                                                        <asp:ListItem>Not Active</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td></td>
                                                <td align="center">
                                                    <asp:Button ID="ibnSave" runat="server" CssClass="btn btn-primary" Enabled="False" OnClick="ibnSave_Click" Text="Save" ValidationGroup="emp" Width="100px" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnSave_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnSave" />
                                                    <asp:Button ID="ibnDelete" runat="server" CssClass="btn btn-danger" Enabled="False" OnClick="ibnDelete_Click" Text="Delete" ValidationGroup="emp" Width="100px" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnDelete" />
                                                    <asp:Button ID="ibnAdd" runat="server" CssClass="btn btn-success" Enabled="False" OnClick="ibnAdd_Click" Text="Add" ValidationGroup="emp" Width="100px" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnAdd_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnAdd" />
                                                </td>
                                                <td align="center"></td>
                                                <td align="center"></td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="center">
                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Size="7pt" ForeColor="Red" HeaderText="Needs Attention" ValidationGroup="emp" />
                                                </td>
                                                <td align="center">&nbsp;</td>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="18pt" ForeColor="Red"></asp:Label>

                                </td>
                            </tr>
                        </table>


                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

