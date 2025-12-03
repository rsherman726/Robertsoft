<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DeliveryAdmin.aspx.cs" Inherits="DeliveryAdmin" %>

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
            <div style="position: fixed; right: 50px; height: 40px; width: 150px; z-index: 10000;">
                <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" AssociatedUpdatePanelID="UpdatePanelPromo" DisplayAfter="1">
                    <ProgressTemplate>
                        <table>
                            <tbody>
                                <tr>
                                    <td align="Left" style="width: 12px">
                                        <img src="Images/loader.gif" alt="" style="border: thin solid #FF0000" />
                                    </td>
                                    <td align="left"><span style="color: #ffffff">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing <span class="">....</span> </strong></span></span></td>
                                </tr>
                            </tbody>
                        </table>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <table align="Center" width="100%">
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td>
                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                        Text="Delivery Admin"></asp:Label></td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:RadioButtonList ID="rblMode" runat="server" AutoPostBack="True" Font-Bold="True" OnSelectedIndexChanged="rblMode_SelectedIndexChanged" RepeatDirection="Horizontal" Width="200px">
                                        <asp:ListItem>Add</asp:ListItem>
                                        <asp:ListItem Selected="True">Edit</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table width="400">
                                        <tr>
                                            <td align="center" style="width: 100px">
                                                <asp:Button ID="ibnSearch" runat="server" CssClass="btn btn-primary" OnClick="ibnSearch_Click" Text="search" ToolTip="Sales Order, Purchase Order, delivery date, vehicle." />
                                            </td>
                                            <td style="width: 350px">
                                                <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" OnTextChanged="txtSearch_TextChanged" ToolTip="This is an Auto Complete text box. Just fill in Sales Order, Purchase Order, delivery date, vehicle."></asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" 
                                                    runat="server" 
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                                                    CompletionListItemCssClass="autocomplete_listItem" 
                                                    CompletionSetCount="25" 
                                                    DelimiterCharacters="" 
                                                    Enabled="True" MinimumPrefixLength="1" 
                                                    ServiceMethod="GetCompletionListDelivery" 
                                                    ServicePath="" TargetControlID="txtSearch" 
                                                    UseContextKey="True"
                                                     CompletionInterval="0">
                                                </ajaxToolkit:AutoCompleteExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <hr />

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="False" GridLines="None" OnRowCommand="gvResults_RowCommand" OnRowDataBound="gvResults_RowDataBound" Width="800px">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Delivery ID">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbnSelect" runat="server" CausesValidation="False" CommandArgument='<%# Bind("DeliveryID") %>' CommandName="Select" Text="Select"></asp:LinkButton>
                                                    &nbsp;<asp:Label ID="lblDeliveryID" runat="server" Text='<%# Bind("DeliveryID") %>'  Font-Bold="True"></asp:Label>
                                                    <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Scheduled Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDateScheduled" runat="server" Text='<%# Bind("DateScheduled") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delivery Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDateDelivered" runat="server" Text='<%# Bind("DateDelivered") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Customer">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Vehicle">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblVehicle" runat="server" Text='<%# Bind("Vehicle") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Driver" Visible="false">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDriver" runat="server" Text='<%# Bind("Driver") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SO#">                                               
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSO" runat="server" Text='<%# Bind("SalesOrder") %>' Font-Bold="True"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="PO#">                                              
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPO" runat="server" Text='<%# Bind("CustomerPoNumber") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td><hr /></td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="pnlMain" runat="server">
                                        <table width="100%" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                            align="center">
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="LabelDeliveryID" runat="server" Font-Bold="True" Text="Delivery ID:"></asp:Label>
                                                </td>
                                                <td style="width: 300px">
                                                    <asp:Label ID="lblDeliveryID" runat="server" Font-Bold="True"></asp:Label>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td style="text-align: left">&nbsp;</td>
                                                <td style="width: 250px">&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Delivery Type:"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 300px">
                                                    <asp:DropDownList ID="ddlDeliveryType" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" OnSelectedIndexChanged="ddlDeliveryType_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td style="text-align: left">&nbsp;</td>
                                                <td style="width: 250px">&nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                                <td>&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="LabelSO" runat="server" Font-Bold="True" Text="Sales Order#" Visible="False"></asp:Label>
                                                </td>
                                                <td style="width: 300px" align="left">
                                                    <asp:TextBox ID="txtSalesOrder" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" OnTextChanged="txtSalesOrder_TextChanged" Visible="False"></asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteSalesOrder" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListSalesOrders" ServicePath="" TargetControlID="txtSalesOrder" UseContextKey="True">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td style="text-align: left">
                                                    &nbsp;</td>
                                                <td style="width: 250px">
                                                    &nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label34" runat="server" Font-Bold="True" Text="Customer:"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 300px">
                                                    <asp:DropDownList ID="ddlCustomer" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True">
                                                    </asp:DropDownList>
                                                     </td>
                                                <td>
                                                    &nbsp;</td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="Delivery Status:"></asp:Label>
                                                </td>
                                                <td style="width: 250px">
                                                    <asp:DropDownList ID="ddlDeliveryStatus" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="LabelPO" runat="server" Font-Bold="True" Text="PO#:" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 300px">
                                                    <asp:TextBox ID="txtPurchaseOrder" runat="server" Font-Size="12pt" BackColor="LemonChiffon" CssClass="form-control" Visible="False"></asp:TextBox>
                                                 <ajaxToolkit:AutoCompleteExtender ID="AutoCompleteExtenderPurchaseOrder" 
                                                    runat="server" 
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" 
                                                    CompletionListItemCssClass="autocomplete_listItem" 
                                                    CompletionSetCount="25" 
                                                    DelimiterCharacters="" 
                                                    Enabled="True" MinimumPrefixLength="1" 
                                                    ServiceMethod="GetCompletionListPurchaseOrders" 
                                                    ServicePath="" TargetControlID="txtPurchaseOrder" 
                                                    UseContextKey="True"
                                                     CompletionInterval="0">
                                                </ajaxToolkit:AutoCompleteExtender>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="LabelInternalPO" runat="server" Font-Bold="True" Text="Internal PO#:" Visible="False"></asp:Label>
                                                </td>
                                                <td style="width: 250px" align="left">
                                                    <asp:TextBox ID="txtInternalPO" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" Visible="False"></asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="txtInternalPO_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListPurchaseOrders" ServicePath="" TargetControlID="txtInternalPO" UseContextKey="True">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label22" runat="server" Font-Bold="True" Text="CS/CT Qty:"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 300px">
                                                    <asp:TextBox ID="txtQtyScheduled" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td style="text-align: left" align="left">
                                                    <asp:Label ID="Label24" runat="server" Font-Bold="True" Text="CS/CT Delivered/Picked Up:" Visible="False"></asp:Label>
                                                </td>
                                                <td style="width: 250px" align="left">
                                                   
                                                    <asp:TextBox ID="txtQtyActual" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" Visible="False"></asp:TextBox>
                                                   
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label25" runat="server" Font-Bold="True" Text="Date Scheduled:"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 300px">
                                                    <asp:TextBox ID="txtDateScheduled" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="DateScheduled_CalendarExtender"
                                                         runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgDateScheduled" 
                                                        TargetControlID="txtDateScheduled">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="txtDateMEEDateScheduled" 
                                                        runat="server" 
                                                        InputDirection="LeftToRight" 
                                                        Mask="99/99/9999"
                                                         MaskType="Date" 
                                                        MessageValidatorTip="true" 
                                                        TargetControlID="txtDateScheduled">
                                                    </ajaxToolkit:MaskedEditExtender> 
                                                    <ajaxToolkit:MaskedEditExtender ID="txtDateMEEDateDelivered" 
                                                        runat="server" 
                                                        InputDirection="LeftToRight" 
                                                        Mask="99/99/9999"
                                                         MaskType="Date" 
                                                        MessageValidatorTip="true" 
                                                        TargetControlID="txtDateDelivered">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    <ajaxToolkit:MaskedEditValidator ID="txtDateMEVDateScheduled" 
                                                        runat="server" 
                                                        ControlExtender="txtDateMEEDateScheduled" 
                                                        ControlToValidate="txtDateScheduled" 
                                                        Display="None" 
                                                        EmptyValueMessage="Please enter a date." 
                                                        ErrorMessage="Invalid date format." 
                                                        InvalidValueMessage="Invalid date format." 
                                                        IsValidEmpty="true" 
                                                        MinimumValue="01/01/1901" 
                                                        MinimumValueMessage="Date must be greater than 01/01/1901" 
                                                        TooltipMessage="Please enter a date." />
                                                    <ajaxToolkit:ValidatorCalloutExtender ID="txtDateMEVEDateScheduled" 
                                                        runat="server" 
                                                        HighlightCssClass="validatorCalloutHighlight" 
                                                        TargetControlID="txtDateMEVDateScheduled" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgDateScheduled" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                </td>
                                                <td align="left" style="text-align: left">
                                                    <asp:Label ID="Label21" runat="server" Font-Bold="True" Text="Date Delivered/Picked Up:"></asp:Label>
                                                </td>
                                                <td style="width: 250px" align="left">
                                                    <asp:TextBox ID="txtDateDelivered" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="DateDelivered_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgDateDelivered" TargetControlID="txtDateDelivered">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditValidator ID="txtDateMEVDateDelivered" runat="server" ControlExtender="txtDateMEEDateDelivered" ControlToValidate="txtDateDelivered" Display="None" EmptyValueMessage="Please enter a date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                    <ajaxToolkit:ValidatorCalloutExtender ID="txtDateMEVEDateDelivered" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtDateMEVDateDelivered" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgDateDelivered" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="LabelInputDate" runat="server" Font-Bold="True" Text="Input Date:"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 300px">
                                                    <asp:TextBox ID="txtDateAdded" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td>&nbsp;</td>
                                                <td style="text-align: left">
                                                    <asp:Label ID="Label29" runat="server" Font-Bold="True" Text="Vehicle:"></asp:Label>
                                                </td>
                                                <td style="width: 250px" align="left">
                                                    <asp:DropDownList ID="ddlVehicle" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" OnSelectedIndexChanged="ddlVehicle_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="LabelAddedBy" runat="server" Font-Bold="True" Text="Added By:"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 300px">
                                                    <asp:TextBox ID="txtAddedBy" runat="server" BackColor="LemonChiffon" CssClass="form-control" Enabled="False" Font-Size="12pt" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                                <td style="text-align: left" align="left">
                                                    <asp:Label ID="LabelTrack" runat="server" Font-Bold="True" Text="Tracking Number:" Visible="False"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 250px">
                                                    <asp:TextBox ID="txtTrackingNumber" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" Visible="False"></asp:TextBox>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="LabelDateModified" runat="server" Font-Bold="True" Text="Date Modified:"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 300px">
                                                    <asp:TextBox ID="txtDateModified" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                                <td align="center" style="text-align: left">
                                                    <asp:Label ID="Label30" runat="server" Font-Bold="True" Text="Check#:"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 250px">
                                                    <asp:TextBox ID="txtCheckNumber" runat="server" BackColor="LightGray" CssClass="form-control" Font-Size="12pt" Enabled="False" ></asp:TextBox>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="LabelModifedBy" runat="server" Font-Bold="True" Text="Modified By:"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 300px">
                                                    <asp:TextBox ID="txtModifiedBy" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" ReadOnly="True"></asp:TextBox>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                                <td align="center" style="text-align: left">
                                                    <asp:Label ID="Label32" runat="server" Font-Bold="True" Text="Check Amount:"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 250px">
                                                    <asp:TextBox ID="txtCheckAmount" runat="server" BackColor="LightGray" CssClass="form-control" Font-Size="12pt" Enabled="False"></asp:TextBox>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                                <td align="left" valign="top">
                                                    <asp:Label ID="Label33" runat="server" Font-Bold="True" Text="Comments:"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 300px">
                                                    <asp:TextBox ID="txtComments" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td align="center">&nbsp;</td>
                                                <td align="center" style="text-align: left">&nbsp;</td>
                                                <td align="center" style="width: 250px">
                                                    <asp:CheckBox ID="chkIsCOD" runat="server" Text="C.O.D." AutoPostBack="True" OnCheckedChanged="chkIsCOD_CheckedChanged" Font-Names="Arial" Font-Size="18pt" ForeColor="Red" />
                                                </td>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                           
                                        </table>
                                    </asp:Panel>

                                </td>
                            </tr>
                            <tr>
                                <td align="left">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" Font-Size="7pt" ForeColor="Red" HeaderText="Needs Attention" ValidationGroup="emp" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="ibnSave" runat="server" CssClass="btn btn-primary" Enabled="False" OnClick="ibnSave_Click" Text="save" ValidationGroup="emp" />
                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnSave_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnSave">
                                    </ajaxToolkit:ConfirmButtonExtender>
                                    &nbsp; 
                                    <asp:Button ID="ibnDelete" runat="server" CssClass="btn btn-primary" Enabled="False" OnClick="ibnDelete_Click" Text="delete" ValidationGroup="emp" />
                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnDelete">
                                    </ajaxToolkit:ConfirmButtonExtender>
                                    &nbsp;<asp:Button ID="ibnAdd" runat="server" CssClass="btn btn-primary" Enabled="False" OnClick="ibnAdd_Click" Text="add" ValidationGroup="emp" />
                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnAdd_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnAdd">
                                    </ajaxToolkit:ConfirmButtonExtender>
                                </td>
                            </tr>
                        </table>


                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
