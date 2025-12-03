<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserAdmin.aspx.cs" Inherits="UserAdmin" %>

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

        .auto-style2 {
            color: #FF0000;
        }

        .auto-style3 {
            height: 150px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
        <ContentTemplate>
            <table align="Center" width="1100px" class="BoxShadow4" style="background-color: #F0F0F0">
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
                                        Text="User Admin" ForeColor="Black"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table>
                                        <tr>
                                            <td>
                                                <table width="100%" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                                    align="center">
                                                    <tr>
                                                        <td style="width: 5px">&nbsp;</td>
                                                        <td style="width: 250px">&nbsp;</td>
                                                        <td style="width: 350px">&nbsp;</td>
                                                        <td style="width: 5px">&nbsp;</td>
                                                        <td style="width: 5px">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td>
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
                                                        <td align="center">
                                                            <asp:Button ID="ibnSearch" runat="server" Text="search" OnClick="ibnSearch_Click" ToolTip="Fill in any part of the name and click search to find the user." CssClass="btn btn-primary" />
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="True" BorderWidth="2px" OnTextChanged="txtSearch_TextChanged" ToolTip="This is an Auto Complete text box. Just fill in first, first-middle, or first-middle-lastname of the user's name." BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                            <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server"
                                                                CompletionSetCount="25" CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListUserName"
                                                                ServicePath="" TargetControlID="txtSearch" UseContextKey="True" CompletionInterval="0">
                                                            </ajaxToolkit:AutoCompleteExtender>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td></td>
                                                        <td align="left">
                                                            <asp:CheckBoxList ID="cblRoles" runat="server" AutoPostBack="True" Font-Names="Arial" Font-Size="9pt" ForeColor="Black" OnSelectedIndexChanged="cblRoles_SelectedIndexChanged">
                                                            </asp:CheckBoxList>
                                                        </td>
                                                        <td align="left">
                                                            <asp:ListBox ID="lbUsers" runat="server" AutoPostBack="True" Font-Bold="True" OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black" Height="150px"></asp:ListBox>
                                                        </td>
                                                        <td></td>
                                                        <td></td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td align="center">
                                                            <asp:Label ID="lblView" runat="server" Font-Bold="True" Font-Italic="True" ForeColor="Black" Text="VIEW USERS:"></asp:Label>
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
                                                        <td>&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td align="center">
                                                            <asp:LinkButton ID="lbnLoginAs" runat="server" CssClass="btn btn-success btn-sm" Font-Bold="True" OnClick="lbnLoginAs_Click" ToolTip="You can only login to active accounts!" Visible="False">Login as this user</asp:LinkButton>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="FIRST NAME:" ForeColor="Black"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtFirstName" runat="server" BorderWidth="2px" Font-Size="12pt" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" EnableClientScript="False" ErrorMessage="First name is required." ValidationGroup="emp"><img src="Images/arrow_alert.gif" alt="arrow"/></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label34" runat="server" Font-Bold="True" ForeColor="Black" Text="MIDDLE NAME:"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtMiddleInitial" runat="server" BackColor="LemonChiffon" BorderWidth="2px" CssClass="form-control" Font-Size="12pt" ForeColor="Black"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label35" runat="server" Font-Bold="True" ForeColor="Black" Text="LAST NAME:"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtLastName" runat="server" BackColor="LemonChiffon" BorderWidth="2px" CssClass="form-control" Font-Size="12pt" ForeColor="Black"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" EnableClientScript="False" ErrorMessage="Last name is required." ValidationGroup="emp"><img src="Images/arrow_alert.gif" alt="arrow"/></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label2" runat="server" Font-Bold="True" ForeColor="Black" Text="PHONE#:"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtPhone" runat="server" BackColor="LemonChiffon" BorderWidth="2px" CssClass="form-control" Font-Size="12pt" ForeColor="Black"></asp:TextBox>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="EMAIL:" ForeColor="Black"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtEmail" runat="server" BorderWidth="2px" Font-Size="12pt" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" EnableClientScript="False" ErrorMessage="Email is required." ValidationGroup="emp"><img src="Images/arrow_alert.gif" alt="arrow"/></asp:RequiredFieldValidator></td>
                                                        <td>
                                                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail" EnableClientScript="False" ErrorMessage="Invalid email format." ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                ValidationGroup="emp" Width="10px"><img src="Images/arrow_alert.gif" alt="arrow"/></asp:RegularExpressionValidator>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="GENDER:" ForeColor="Black"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlGender" runat="server" Font-Bold="True" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black">
                                                                <asp:ListItem Selected="True">SELECT A GENDER</asp:ListItem>
                                                                <asp:ListItem Value="M">MALE</asp:ListItem>
                                                                <asp:ListItem Value="F">FEMALE</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label28" runat="server" Font-Bold="True" Text="ROLE:" ForeColor="Black"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlRoles" runat="server" Font-Bold="True" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label24" runat="server" Font-Bold="True" Text="ADDRESS:" ForeColor="Black"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtAddress" runat="server" BorderWidth="2px" Font-Size="12pt" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label25" runat="server" Font-Bold="True" Text="CITY:" ForeColor="Black"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtCity" runat="server" BorderWidth="2px" Font-Size="12pt" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label23" runat="server" Font-Bold="True" Text="STATE:" ForeColor="Black"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlState" runat="server" Font-Bold="True" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black">
                                                                <asp:ListItem Value="AL">Alabama</asp:ListItem>
                                                                <asp:ListItem Value="AK">Alaska</asp:ListItem>
                                                                <asp:ListItem Value="AZ">Arizona</asp:ListItem>
                                                                <asp:ListItem Value="AR">Arkansas</asp:ListItem>
                                                                <asp:ListItem Value="CA" Selected="True">California</asp:ListItem>
                                                                <asp:ListItem Value="CO">Colorado</asp:ListItem>
                                                                <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                                                                <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
                                                                <asp:ListItem Value="DE">Delaware</asp:ListItem>
                                                                <asp:ListItem Value="FL">Florida</asp:ListItem>
                                                                <asp:ListItem Value="GA">Georgia</asp:ListItem>
                                                                <asp:ListItem Value="HI">Hawaii</asp:ListItem>
                                                                <asp:ListItem Value="ID">Idaho</asp:ListItem>
                                                                <asp:ListItem Value="IL">Illinois</asp:ListItem>
                                                                <asp:ListItem Value="IN">Indiana</asp:ListItem>
                                                                <asp:ListItem Value="IA">Iowa</asp:ListItem>
                                                                <asp:ListItem Value="KS">Kansas</asp:ListItem>
                                                                <asp:ListItem Value="KY">Kentucky</asp:ListItem>
                                                                <asp:ListItem Value="LA">Louisiana</asp:ListItem>
                                                                <asp:ListItem Value="ME">Maine</asp:ListItem>
                                                                <asp:ListItem Value="MD">Maryland</asp:ListItem>
                                                                <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
                                                                <asp:ListItem Value="MI">Michigan</asp:ListItem>
                                                                <asp:ListItem Value="MN">Minnesota</asp:ListItem>
                                                                <asp:ListItem Value="MS">Mississippi</asp:ListItem>
                                                                <asp:ListItem Value="MO">Missouri</asp:ListItem>
                                                                <asp:ListItem Value="MT">Montana</asp:ListItem>
                                                                <asp:ListItem Value="NE">Nebraska</asp:ListItem>
                                                                <asp:ListItem Value="NV">Nevada</asp:ListItem>
                                                                <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
                                                                <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
                                                                <asp:ListItem Value="NM">New Mexico</asp:ListItem>
                                                                <asp:ListItem Value="NY">New York</asp:ListItem>
                                                                <asp:ListItem Value="NC">North Carolina</asp:ListItem>
                                                                <asp:ListItem Value="ND">North Dakota</asp:ListItem>
                                                                <asp:ListItem Value="OH">Ohio</asp:ListItem>
                                                                <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
                                                                <asp:ListItem Value="OR">Oregon</asp:ListItem>
                                                                <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
                                                                <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
                                                                <asp:ListItem Value="SC">South Carolina</asp:ListItem>
                                                                <asp:ListItem Value="SD">South Dakota</asp:ListItem>
                                                                <asp:ListItem Value="TN">Tennessee</asp:ListItem>
                                                                <asp:ListItem Value="TX">Texas</asp:ListItem>
                                                                <asp:ListItem Value="UT">Utah</asp:ListItem>
                                                                <asp:ListItem Value="VT">Vermont</asp:ListItem>
                                                                <asp:ListItem Value="VA">Virginia</asp:ListItem>
                                                                <asp:ListItem Value="WA">Washington</asp:ListItem>
                                                                <asp:ListItem Value="WV">West Virginia</asp:ListItem>
                                                                <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
                                                                <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Text="POSTALCODE:" ForeColor="Black"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtPostalCode" runat="server" BorderWidth="2px" Font-Size="12pt" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label29" runat="server" Font-Bold="True" Text="USERNAME:" ForeColor="Black"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtUserName" runat="server" BorderWidth="2px" Font-Size="12pt" Enabled="False" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                        </td>
                                                        <td align="center">
                                                            <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName" EnableClientScript="False" ErrorMessage="Username is required." ValidationGroup="emp"><img src="Images/arrow_alert.gif" alt="arrow"/></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label30" runat="server" Font-Bold="True" Text="PASSWORD:" ForeColor="Black"></asp:Label>
                                                            <br />
                                                            <span class="auto-style2">**10 Chars MAX</span></td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtPassword" runat="server" BorderWidth="2px" Font-Size="12pt" ToolTip="Password Strength Advice Enabled" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                            <ajaxToolkit:PasswordStrength ID="PasswordStrengthStandard" runat="server" Enabled="True" TargetControlID="txtPassword"
                                                                DisplayPosition="RightSide"
                                                                StrengthIndicatorType="Text"
                                                                PreferredPasswordLength="8"
                                                                PrefixText="Strength:"
                                                                HelpStatusLabelID="TextBox1_HelpLabel"
                                                                TextStrengthDescriptions="Very Poor;Weak;Average;Strong;Excellent"
                                                                StrengthStyles="TextIndicator_TextBox1_Strength1;TextIndicator_TextBox1_Strength2;TextIndicator_TextBox1_Strength3;TextIndicator_TextBox1_Strength4;TextIndicator_TextBox1_Strength5"
                                                                MinimumNumericCharacters="2"
                                                                MinimumSymbolCharacters="0"
                                                                RequiresUpperAndLowerCaseCharacters="true"></ajaxToolkit:PasswordStrength>
                                                            <br />
                                                            <asp:Label ID="TextBox1_HelpLabel" runat="server" />
                                                        </td>
                                                        <td align="center">
                                                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" EnableClientScript="False" ErrorMessage="Password is required." ValidationGroup="emp"><img src="Images/arrow_alert.gif" alt="arrow"/></asp:RequiredFieldValidator>
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label33" runat="server" Font-Bold="True" Text="STATUS:" ForeColor="Red"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlStatus" runat="server" Font-Bold="True" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black">
                                                                <asp:ListItem Selected="True" Value="-1">Select a Status</asp:ListItem>
                                                                <asp:ListItem Value="1">Active</asp:ListItem>
                                                                <asp:ListItem Value="0">De-Activated</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label36" runat="server" Font-Bold="True" ForeColor="Black" Text="LINK SALES PERSON ID:"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlSalespersonID" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" ForeColor="Black">
                                                                <asp:ListItem Selected="True" Value="0">Select a Salesperson ID</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label37" runat="server" Font-Bold="True" ForeColor="Black" Text="DEPT:"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:DropDownList ID="ddlDept" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" ForeColor="Black">
                                                                <asp:ListItem Selected="True" Value="0">Select a Dept</asp:ListItem>
                                                                <asp:ListItem Value="C">Customer Service</asp:ListItem>
                                                                <asp:ListItem Value="L">Logistics</asp:ListItem>
                                                                <asp:ListItem Value="O">Operations</asp:ListItem>
                                                                <asp:ListItem Value="P">Production</asp:ListItem>
                                                                <asp:ListItem Value="Q">Quality Control</asp:ListItem>
                                                                <asp:ListItem Value="S">System</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label38" runat="server" Font-Bold="True" ForeColor="Black" Text="PICKER:"></asp:Label>
                                                        </td>
                                                        <td align="center">
                                                            <asp:CheckBox ID="chkPicker" runat="server" ForeColor="Black" />
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="Label39" runat="server" Font-Bold="True" ForeColor="Black" Text="COMPTON ONLY:"></asp:Label>
                                                        </td>
                                                        <td align="center">
                                                            <asp:CheckBox ID="chkCompton" runat="server" ForeColor="Black" />
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td></td>
                                                        <td align="center">
                                                            <asp:Button ID="ibnSave" runat="server" Text="Save" OnClick="ibnSave_Click" ValidationGroup="emp" Enabled="False" CssClass="btn btn-primary" Width="100px" />
                                                            <ajaxToolkit:ConfirmButtonExtender ID="ibnSave_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnSave"></ajaxToolkit:ConfirmButtonExtender>
                                                            <asp:Button ID="ibnDelete" runat="server" Enabled="False" Text="Delete" OnClick="ibnDelete_Click" ValidationGroup="emp" CssClass="btn btn-danger" Width="100px" />
                                                            <ajaxToolkit:ConfirmButtonExtender ID="ibnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnDelete"></ajaxToolkit:ConfirmButtonExtender>
                                                            <asp:Button ID="ibnAdd" runat="server" Enabled="False" Text="Add" OnClick="ibnAdd_Click" ValidationGroup="emp" CssClass="btn btn-success" Width="100px" />
                                                            <ajaxToolkit:ConfirmButtonExtender ID="ibnAdd_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnAdd"></ajaxToolkit:ConfirmButtonExtender>
                                                        </td>
                                                        <td align="center"></td>
                                                        <td align="center"></td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td align="center">
                                                            <asp:ValidationSummary ID="vs1" runat="server" Font-Size="10pt" ForeColor="Red" HeaderText="Needs Attention" ValidationGroup="emp" Font-Bold="True" />
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table>
                                                    <tr>
                                                        <td class="hdr" align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="Panel1" runat="server" Width="600px">
                                                                <table width="100%" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                                                    align="center">
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td align="center">
                                                                            <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black" Text="Message Groups Admin"></asp:Label>
                                                                        </td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td align="center">
                                                                            <asp:RadioButtonList ID="rblModeGroup" runat="server" AutoPostBack="True" Font-Bold="True" RepeatDirection="Horizontal" ForeColor="Black" Width="200px" OnSelectedIndexChanged="rblModeGroup_SelectedIndexChanged">
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
                                                                        <td align="left">&nbsp;
                                                                        </td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td align="left">
                                                                            <asp:ListBox ID="lbMessageGroups" runat="server" AutoPostBack="True" Font-Bold="True" OnSelectedIndexChanged="lbMessageGroups_SelectedIndexChanged" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black" Height="100px"></asp:ListBox>
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
                                                                            <asp:Label ID="Label7" runat="server" Font-Bold="True" Text="NAME:" ForeColor="Black"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtGroupName" runat="server" Font-Size="12pt" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RequiredFieldValidator ID="rfvGroupName" runat="server" ControlToValidate="txtGroupName" EnableClientScript="False" ErrorMessage="Group name is required." ValidationGroup="group"><img src="Images/arrow_alert.gif" alt="arrow"/></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td>&nbsp;</td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblErrorMessageGroup" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Red"></asp:Label>
                                                                        </td>
                                                                        <td align="center">&nbsp;</td>
                                                                        <td align="center">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td></td>
                                                                        <td align="center">
                                                                            <asp:Button ID="btnSaveGroup" runat="server" Text="Save" ValidationGroup="group" Enabled="False" CssClass="btn btn-primary" Width="100px" OnClick="btnSaveGroup_Click" />
                                                                            <ajaxToolkit:ConfirmButtonExtender ID="btnSaveGroup_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="btnSaveGroup"></ajaxToolkit:ConfirmButtonExtender>
                                                                            <asp:Button ID="btnDeleteGroup" runat="server" Enabled="False" Text="Delete" ValidationGroup="group" CssClass="btn btn-danger" Width="100px" OnClick="btnDeleteGroup_Click" />
                                                                            <ajaxToolkit:ConfirmButtonExtender ID="btnDeleteGroup_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="btnDeleteGroup"></ajaxToolkit:ConfirmButtonExtender>
                                                                            <asp:Button ID="btnAddGroup" runat="server" Enabled="False" Text="Add" ValidationGroup="group" CssClass="btn btn-success" Width="100px" OnClick="btnAddGroup_Click" />
                                                                            <ajaxToolkit:ConfirmButtonExtender ID="btnAddGroup_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="btnAddGroup"></ajaxToolkit:ConfirmButtonExtender>
                                                                        </td>
                                                                        <td align="center"></td>
                                                                        <td align="center"></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td align="center">
                                                                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" Font-Size="7pt" ForeColor="Red" HeaderText="Needs Attention" ValidationGroup="group" />
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
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align="center" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite" width="100%">
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td align="center">
                                                            <asp:Label ID="Label58" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black" Text="Assign Message Groups"></asp:Label></td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td align="center">

                                                            <table id="tblAdd" align="center">
                                                                <tr>
                                                                    <td>&nbsp; </td>
                                                                    <td>&nbsp; </td>
                                                                    <td>&nbsp; </td>
                                                                    <td align="center">&nbsp;
                                                                    <asp:Label ID="Label66" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Black" Text="Message Groups"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td valign="middle">&nbsp; </td>
                                                                    <td align="center" valign="middle">&nbsp;
                                                                    <asp:DropDownList ID="ddlMessageGroups" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Size="8pt" ForeColor="Black" OnSelectedIndexChanged="ddlMessageGroups_SelectedIndexChanged" Width="350px" Font-Bold="True">
                                                                    </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp; </td>
                                                                    <td align="center">
                                                                        <asp:RadioButtonList ID="rblEvenOdd" runat="server" ForeColor="Black" RepeatDirection="Horizontal" Width="400px">
                                                                            <asp:ListItem Selected="True">NA</asp:ListItem>
                                                                            <asp:ListItem>EVEN ORDERS</asp:ListItem>
                                                                            <asp:ListItem>ODD ORDERS</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                    <td>&nbsp; </td>
                                                                    <td>&nbsp; </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp; </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="Label55" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Navy" Text="Available Employees"></asp:Label>
                                                                    </td>
                                                                    <td>&nbsp; </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="Label57" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Navy" Text="Employees assigned to above Message Group"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>
                                                                        <asp:ListBox ID="lbAvailableEmployees" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black" Height="200px" SelectionMode="Multiple" Width="400px"></asp:ListBox>
                                                                    </td>
                                                                    <td>
                                                                        <table id="Table4" border="0" cellpadding="1" cellspacing="1">
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <asp:Button ID="btnAddOne" runat="server" BackColor="Silver" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnAddOne_Click" Text="Add &gt;" ToolTip="Click to ADD" Width="125px" ValidationGroup="none" />
                                                                                    <ajaxToolkit:ConfirmButtonExtender ID="btnAddOne_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to add this employee?" Enabled="True" TargetControlID="btnAddOne" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <asp:Button ID="btnRemoveOne" runat="server" BackColor="Silver" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnRemoveOne_Click" Text="&lt; Remove" ToolTip="Click to REMOVE" Width="125px" ValidationGroup="none" />
                                                                                    <ajaxToolkit:ConfirmButtonExtender ID="btnRemoveOne_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to remove this employee?" Enabled="True" TargetControlID="btnRemoveOne" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center">&nbsp;</td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ListBox ID="lbAssignedEmployees" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black" Height="200px" SelectionMode="Multiple" Width="400px"></asp:ListBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp; </td>
                                                                    <td style="color: black">(Use CTRL for Multiple Selection) </td>
                                                                    <td align="center">&nbsp; </td>
                                                                    <td align="center" style="color: black">(Use CTRL for Multiple Selection) </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp; </td>
                                                                    <td align="center">
                                                                        <asp:LinkButton ID="lbnSelectAllAssign" runat="server" Font-Bold="True" OnClick="lbnSelectAllAssign_Click">Select All</asp:LinkButton>
                                                                        &nbsp;
                                                                    <asp:LinkButton ID="lbnClearAssign" runat="server" Font-Bold="True" OnClick="lbnClearAssign_Click">Clear All</asp:LinkButton>
                                                                    </td>
                                                                    <td>&nbsp; </td>
                                                                    <td align="center">&nbsp;
                                                                    <asp:LinkButton ID="lbnSelectAllAssigned" runat="server" Font-Bold="True" OnClick="lbnSelectAllAssigned_Click">Select All</asp:LinkButton>
                                                                        &nbsp;
                                                                    <asp:LinkButton ID="lbnClearAssigned" runat="server" Font-Bold="True" OnClick="lbnClearAssigned_Click">Clear All</asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="lblErrorAssignGroup" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                           <tr>
                                            <td align="center">
                                                <table>
                                                    <tr>
                                                        <td class="hdr" align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Panel ID="Panel2" runat="server" Width="600px">
                                                                <table width="100%" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                                                    align="center">
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td align="center">
                                                                            <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black" Text="Security Groups Admin"></asp:Label>
                                                                        </td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td align="center">
                                                                            <asp:RadioButtonList ID="rblModeSecurityGroup" runat="server" AutoPostBack="True" Font-Bold="True" RepeatDirection="Horizontal" ForeColor="Black" Width="200px" OnSelectedIndexChanged="rblModeSecurityGroup_SelectedIndexChanged">
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
                                                                        <td align="left">&nbsp;
                                                                        </td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td align="left">
                                                                            <asp:ListBox ID="lbSecurityGroups" runat="server" AutoPostBack="True" Font-Bold="True" OnSelectedIndexChanged="lbSecurityGroups_SelectedIndexChanged" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black" Height="100px"></asp:ListBox>
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
                                                                            <asp:Label ID="Label10" runat="server" Font-Bold="True" Text="NAME:" ForeColor="Black"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtSecurityGroupName" runat="server" Font-Size="12pt" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:RequiredFieldValidator ID="rfvSecurityGroupName" runat="server" ControlToValidate="txtSecurityGroupName" EnableClientScript="False" ErrorSecurity="Security Group name is required." ValidationSecurityGroup="SecurityGroup"><img src="Images/arrow_alert.gif" alt="arrow"/></asp:RequiredFieldValidator>
                                                                        </td>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblErrorSecurityGroup" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Red"></asp:Label>
                                                                        </td>
                                                                        <td align="center">&nbsp;</td>
                                                                        <td align="center">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td></td>
                                                                        <td align="center">
                                                                            <asp:Button ID="btnSaveSecurityGroup" runat="server" Text="Save" ValidationSecurityGroup="SecurityGroup" Enabled="False" CssClass="btn btn-primary" Width="100px" OnClick="btnSaveSecurityGroup_Click" />
                                                                            <ajaxToolkit:ConfirmButtonExtender ID="btnSaveSecurityGroup_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="btnSaveSecurityGroup"></ajaxToolkit:ConfirmButtonExtender>
                                                                            <asp:Button ID="btnDeleteSecurityGroup" runat="server" Enabled="False" Text="Delete" ValidationSecurityGroup="none" CssClass="btn btn-danger" Width="100px" OnClick="btnDeleteSecurityGroup_Click" />
                                                                            <ajaxToolkit:ConfirmButtonExtender ID="btnDeleteSecurityGroup_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="btnDeleteSecurityGroup"></ajaxToolkit:ConfirmButtonExtender>
                                                                            <asp:Button ID="btnAddSecurityGroup" runat="server" Enabled="False" Text="Add" ValidationSecurityGroup="SecurityGroup" CssClass="btn btn-success" Width="100px" OnClick="btnAddSecurityGroup_Click" />
                                                                            <ajaxToolkit:ConfirmButtonExtender ID="btnAddSecurityGroup_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="btnAddSecurityGroup"></ajaxToolkit:ConfirmButtonExtender>
                                                                        </td>
                                                                        <td align="center"></td>
                                                                        <td align="center"></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td>&nbsp;</td>
                                                                        <td align="center">
                                                                            <asp:ValidationSummary ID="ValidationSummarySG" runat="server" Font-Size="7pt" ForeColor="Red" HeaderText="Needs Attention" ValidationSecurityGroup="SecurityGroup" />
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
                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table align="center" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite" width="100%">
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td align="center">
                                                            <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black" Text="Assign Security Groups"></asp:Label></td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td align="center">

                                                            <table id="tblAddSG" align="center">
                                                                <tr>
                                                                    <td>&nbsp; </td>
                                                                    <td>&nbsp; </td>
                                                                    <td>&nbsp; </td>
                                                                    <td align="center">&nbsp;
                                                                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Black" Text="Security Groups"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                    <td valign="middle">&nbsp; </td>
                                                                    <td align="center" valign="middle">&nbsp;
                                                                    <asp:DropDownList ID="ddlSecurityGroups" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Size="8pt" ForeColor="Black" OnSelectedIndexChanged="ddlSecurityGroups_SelectedIndexChanged" Width="350px" Font-Bold="True">
                                                                    </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp; </td>
                                                                    <td align="center">
                                                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" ForeColor="Black" RepeatDirection="Horizontal" Width="400px" Visible="False">
                                                                            <asp:ListItem Selected="True">NA</asp:ListItem>
                                                                            <asp:ListItem>EVEN ORDERS</asp:ListItem>
                                                                            <asp:ListItem>ODD ORDERS</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                    <td>&nbsp; </td>
                                                                    <td>&nbsp; </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp; </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="Label13" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Navy" Text="Available Employees"></asp:Label>
                                                                    </td>
                                                                    <td>&nbsp; </td>
                                                                    <td align="center">
                                                                        <asp:Label ID="Label14" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Navy" Text="Employees assigned to above Security Group"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td>
                                                                        <asp:ListBox ID="lbAvailableEmployeesSG" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black" Height="200px" SelectionMode="Multiple" Width="400px"></asp:ListBox>
                                                                    </td>
                                                                    <td>
                                                                        <table id="Table5" border="0" cellpadding="1" cellspacing="1">
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <asp:Button ID="btnAddOneSG" runat="server" BackColor="Silver" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnAddOneSG_Click" Text="Add &gt;" ToolTip="Click to ADD" Width="125px" ValidationGroup="none" />
                                                                                    <ajaxToolkit:ConfirmButtonExtender ID="btnAddOneSG_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to add this employee?" Enabled="True" TargetControlID="btnAddOneSG" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <asp:Button ID="btnRemoveOneSG" runat="server" BackColor="Silver" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnRemoveOneSG_Click" Text="&lt; Remove" ToolTip="Click to REMOVE" Width="125px" ValidationGroup="none" />
                                                                                    <ajaxToolkit:ConfirmButtonExtender ID="btnRemoveOneSG_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to remove this employee?" Enabled="True" TargetControlID="btnRemoveOneSG" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center">&nbsp;</td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ListBox ID="lbAssignedEmployeesSG" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black" Height="200px" SelectionMode="Multiple" Width="400px"></asp:ListBox>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp; </td>
                                                                    <td style="color: black">(Use CTRL for Multiple Selection) </td>
                                                                    <td align="center">&nbsp; </td>
                                                                    <td align="center" style="color: black">(Use CTRL for Multiple Selection) </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp; </td>
                                                                    <td align="center">
                                                                        <asp:LinkButton ID="lbnSelectAllAssignSG" runat="server" Font-Bold="True" OnClick="lbnSelectAllAssignSG_Click">Select All</asp:LinkButton>
                                                                        &nbsp;
                                                                    <asp:LinkButton ID="lbnClearAssignSG" runat="server" Font-Bold="True" OnClick="lbnClearAssignSG_Click">Clear All</asp:LinkButton>
                                                                    </td>
                                                                    <td>&nbsp; </td>
                                                                    <td align="center">&nbsp;
                                                                    <asp:LinkButton ID="lbnSelectAllAssignedSG" runat="server" Font-Bold="True" OnClick="lbnSelectAllAssignedSG_Click">Select All</asp:LinkButton>
                                                                        &nbsp;
                                                                    <asp:LinkButton ID="lbnClearAssignedSG" runat="server" Font-Bold="True" OnClick="lbnClearAssignedSG_Click">Clear All</asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td align="left">
                                                            <asp:Label ID="lblErrorAssignSecurityGroup" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="12pt"></asp:Label>
                                                        </td>
                                                        <td align="center">&nbsp;</td>
                                                        <td align="center">&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>




