<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CustomerDailyNotes.aspx.cs" Inherits="CustomerDailyNotes" %>

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
            <div style="position: fixed; left: 50%; height: 40px; width: 150px; z-index: 10000;">
                <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" AssociatedUpdatePanelID="UpdatePanelPromo" DisplayAfter="1">
                    <ProgressTemplate>
                        <table>
                            <tbody>
                                <tr>
                                    <td align="left">
                                        <img src="Images/indicator_big.gif" alt="" />
                                    </td>
                                    <td align="left"><span style="color: #ffffff">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing <span class="">....</span> </strong></span></span></td>
                                </tr>
                            </tbody>
                        </table>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <table align="Center" style="background-color: Ghostwhite" class="JustRoundedEdgeBothSmall" width="800">
                <tr>
                    <td align="center">
                        <table width="700px">
                            <tr>
                                <td class="hdr" align="center">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                        Text="Customer Notes" ForeColor="Black"></asp:Label></td>
                            </tr>
                            <tr>
                                <td align="center" class="hdr">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center" class="hdr">
                                    <asp:Panel ID="pnlMain" runat="server">
                                        <table>
                                            <tr>
                                                <td align="center">
                                                    <table align="center" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite" width="700px">
                                                        <tr>
                                                            <td align="left" style="width: 5px">&nbsp;</td>
                                                            <td align="left">
                                                                <table width="400px">
                                                                    <tr>
                                                                        <td style="width: 350px">
                                                                            <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black" OnTextChanged="txtSearch_TextChanged" ToolTip="This is an Auto Complete text box. Just fill in first, first-middle, or first-middle-lastname of the user's name."></asp:TextBox>
                                                                            <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListUserName" ServicePath="" TargetControlID="txtSearch" UseContextKey="True">
                                                                            </ajaxToolkit:AutoCompleteExtender>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="ibnSearch" runat="server" CssClass="btn btn-primary" Font-Names="Arial" OnClick="ibnSearch_Click" Text="Search" ToolTip="Fill in any part of the name and click search to find the user." ValidationGroup="none" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td style="width: 15px">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 5px">&nbsp;</td>
                                                            <td align="left">
                                                                <asp:ListBox ID="lbCustomers" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control input-sm" Font-Bold="True" Font-Names="arial" ForeColor="Black" Height="300px" OnSelectedIndexChanged="lbCustomers_SelectedIndexChanged" Width="600px" Font-Size="10pt"></asp:ListBox>
                                                            </td>
                                                            <td style="width: 15px">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 5px">&nbsp;</td>
                                                            <td align="center">
                                                                <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="20pt" ForeColor="Red"></asp:Label>
                                                            </td>
                                                            <td style="width: 15px">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 5px">&nbsp;</td>
                                                                        <td style="width: 5px">&nbsp;</td>
                                                                        <td align="left">&nbsp;</td>
                                                                        <td align="center">
                                                                            <asp:Label ID="Label41" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Black" Text="Customer Information"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 5px">&nbsp;</td>
                                                                        <td style="width: 5px">&nbsp;</td>
                                                                        <td align="left">&nbsp;</td>
                                                                        <td align="center">
                                                                            &nbsp;</td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 5px">&nbsp;</td>
                                                                        <td style="width: 5px">&nbsp;</td>
                                                                        <td align="left">
                                                                            <asp:Label ID="LabelUserID" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" Text="CUSTOMER:"></asp:Label>
                                                                        </td>
                                                                        <td align="center">
                                                                            <asp:Label ID="lblCustomerID" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Blue"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 300px">
                                                                            <asp:Label ID="Label36" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" Text="COMPANY NAME:"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblCompanyName" runat="server" Font-Size="12pt" ForeColor="Black"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 300px">
                                                                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" Text="ADDRESS:" Width="150px"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblAddress" runat="server" Font-Size="12pt" ForeColor="Black"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 300px">
                                                                            <asp:Label ID="Label35" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" Text="CITY:"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblCity" runat="server" Font-Size="12pt" ForeColor="Black"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 300px">
                                                                            <asp:Label ID="Label37" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" Text="STATE:"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblState" runat="server" Font-Size="12pt" ForeColor="Black"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 300px">
                                                                            <asp:Label ID="Label38" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" Text="POSTAL CODE:"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblPostalCode" runat="server" Font-Size="12pt" ForeColor="Black"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 300px">
                                                                            <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" Text="PHONE#:"></asp:Label>
                                                                            &nbsp; <span style="color: red; font-family: arial, Helvetica, sans-serif; font-size: 7pt;">i.e. 8185551212</span> </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblPhone" runat="server" Font-Size="12pt" ForeColor="Black"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 300px">
                                                                            <asp:Label ID="Label43" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" Text="EXTENSION:"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblExtension" runat="server" Font-Size="12pt" ForeColor="Black"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 300px">
                                                                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" Text="EMAIL:"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblEmail" runat="server" Font-Size="12pt" ForeColor="Black"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                        <td style="width: 15px">&nbsp;</td>
                                                                    </tr>
                                                                    <tr align="left">
                                                                        <td style="width: 5px">&nbsp;</td>
                                                                        <td style="width: 5px">&nbsp;</td>
                                                                        <td align="left" style="width: 300px">
                                                                            <asp:Label ID="Label40" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" Text="CONTACT:"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:Label ID="lblContact" runat="server" Font-Size="12pt" ForeColor="Black"></asp:Label>
                                                                        </td>
                                                                        <td align="center" style="width: 15px">&nbsp;</td>
                                                                        <td align="center" style="width: 15px">&nbsp;</td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                        </tr>

                                                        <tr>
                                                            <td style="width: 5px">&nbsp;</td>
                                                            <td align="center">
                                                                &nbsp;</td>
                                                            <td style="width: 15px">&nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblError0" runat="server" Font-Bold="True" Font-Size="20pt" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table id="tblNotes" runat="server" visible="false" style="background-color: lightblue">
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label42" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Black" Text="Customer Daily Notes"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table width="1200">
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lblNoteDate" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" Text="Note Date"></asp:Label>
                                                        </td>

                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <table id="tblNoteDate" width="250px">
                                                                <tr>
                                                                    <td align="right" style="width: 150px">
                                                                        <asp:TextBox ID="txtNoteDate" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" Font-Size="12pt" ForeColor="Black"></asp:TextBox>
                                                                        <ajaxToolkit:TextBoxWatermarkExtender ID="txtNoteDate_TextBoxWatermarkExtender" runat="server" Enabled="True" TargetControlID="txtNoteDate" WatermarkCssClass="WatermarkDates" WatermarkText="Closing Date To">
                                                                        </ajaxToolkit:TextBoxWatermarkExtender>
                                                                        <ajaxToolkit:CalendarExtender ID="txtNoteDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgNoteDate" TargetControlID="txtNoteDate">
                                                                        </ajaxToolkit:CalendarExtender>
                                                                        <ajaxToolkit:MaskedEditExtender ID="txtNoteDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtNoteDate">
                                                                        </ajaxToolkit:MaskedEditExtender>
                                                                        <ajaxToolkit:MaskedEditValidator ID="txtNoteDateMEV" runat="server" ControlExtender="txtNoteDateMEE" ControlToValidate="txtNoteDate" Display="None" EmptyValueMessage="Please enter a Date card was opened." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtNoteDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtNoteDateMEV" />
                                                                    </td>
                                                                    <td align="left" style="width: 5px">
                                                                        <asp:ImageButton ID="imgNoteDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="lblNotes" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black" Text="Notes"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>

                                                        <td align="center">
                                                            <asp:TextBox ID="txtNotes" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" Font-Size="12pt" ForeColor="Black" Height="75px" TextMode="MultiLine" Width="900px"></asp:TextBox></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="lbnAdd" runat="server" CssClass="btn btn-success" Font-Bold="True" OnClick="lbnAdd_Click" Text="Add Note" ToolTip="Add Note for Selected" Enabled="False"></asp:LinkButton>
                                                <ajaxToolkit:ConfirmButtonExtender ID="lbnAdd_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to add this record?" Enabled="True" TargetControlID="lbnAdd">
                                                </ajaxToolkit:ConfirmButtonExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblErrorNotes" runat="server" Font-Bold="True" Font-Size="20pt" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:GridView ID="gvNotes" runat="server" AutoGenerateColumns="False" CellPadding="4" Font-Bold="True" Font-Size="11pt" ForeColor="#333333"
                                                    GridLines="Vertical" OnRowCancelingEdit="gvNotes_RowCancelingEdit"
                                                    OnRowCommand="gvNotes_RowCommand" OnRowDataBound="gvNotes_RowDataBound" OnRowDeleting="gvNotes_RowDeleting" OnRowEditing="gvNotes_RowEditing" OnRowUpdating="gvNotes_RowUpdating" PageSize="20" Width="950px">
                                                    <AlternatingRowStyle BackColor="White" />
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Red" Text="No Records Found"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                                    <Columns>
                                                        <asp:TemplateField Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNoteID" runat="server" Font-Size="7pt" Text='<%# Bind("NoteID") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <FooterStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Customer">
                                                            <HeaderStyle CssClass="CenterAligner" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblCustomer" runat="server" Font-Size="9pt" ForeColor="Black" Text='<%# Bind("Customer") %>' Width="100px"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Name">
                                                            <HeaderStyle CssClass="CenterAligner" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblName" runat="server" Font-Size="9pt" ForeColor="Black" Text='<%# Bind("Name") %>' Width="200px"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Note Date">
                                                            <HeaderStyle CssClass="CenterAligner" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNoteDate" runat="server" Font-Size="9pt" ForeColor="Black" Text='<%# Bind("NoteDate") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Notes">
                                                            <HeaderStyle CssClass="CenterAligner" />
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="txtNotes" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" Text='<%# Bind("Notes") %>' Font-Size="9pt" ForeColor="Black" Height="75px" TextMode="MultiLine" Width="900px"></asp:TextBox>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNotes" runat="server" Font-Size="9pt" ForeColor="Black" Text='<%# Bind("Notes") %>' Width="500px"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="center" />

                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <EditItemTemplate>
                                                                <asp:LinkButton ID="lbnUpdate" runat="server" CausesValidation="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Update" CssClass="btn btn-success btn-xs"
                                                                    Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Update"></asp:LinkButton>
                                                                <ajaxToolkit:ConfirmButtonExtender ID="lbnUpdate_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to update this record?" Enabled="True" TargetControlID="lbnUpdate">
                                                                </ajaxToolkit:ConfirmButtonExtender>
                                                                &nbsp;<asp:LinkButton ID="lbnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Cancel" CssClass="btn btn-warning btn-xs"></asp:LinkButton>
                                                            </EditItemTemplate>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbnEdit" runat="server" CausesValidation="False" CommandName="Edit" Font-Bold="True" Font-Size="11pt" Text="Edit" CssClass="btn btn-primary btn-xs"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbnDelete" runat="server" CausesValidation="False" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                                    CommandName="Delete" Font-Bold="True" Font-Size="11pt" Text="Delete" ToolTip="Delete Issuer" CssClass="btn btn-danger btn-xs"></asp:LinkButton>
                                                                <ajaxToolkit:ConfirmButtonExtender ID="lbnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to delete this record?" Enabled="True" TargetControlID="lbnDelete">
                                                                </ajaxToolkit:ConfirmButtonExtender>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EditRowStyle BackColor="#2461BF" />
                                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                    <RowStyle BackColor="#EFF3FB" />
                                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table id="Table1" align="center">
                                        <tr>
                                            <td align="center">
                                                <asp:ImageButton ID="imgExportExcel" runat="server" ImageUrl="~/Images/ExportToExcel.GIF" OnClick="imgExportExcel_Click" ToolTip="Click to export all selected customer's notes for selected month/year." />
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
        <Triggers>
            <asp:PostBackTrigger ControlID="imgExportExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

