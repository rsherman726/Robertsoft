<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DocSearch.aspx.cs" Inherits="DocSearch" %>

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
                                <td align="center">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                        Text="Document Search"></asp:Label></td>
                            </tr>
                            <tr>
                                <td></td>
                            </tr>
                            <tr>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:RadioButtonList ID="rblDocType" runat="server" RepeatDirection="Horizontal" Width="600px">
                                        <asp:ListItem Selected="True">&nbsp;Delivery/Will Call Receipts</asp:ListItem>
                                        <asp:ListItem>&nbsp;Pickup</asp:ListItem>
                                        <asp:ListItem>&nbsp;Purchase Orders</asp:ListItem>
                                        <asp:ListItem>&nbsp;All Documents</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table>
                                        <tr>
                                            <td align="center" style="width: 100px">
                                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" OnClick="ibnSearch_Click" Text="search" ToolTip="Just fill in date, PO (full or partial),customer name(full or partial) or SO." />
                                            </td>
                                            <td style="width: 350px">
                                                <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" OnTextChanged="txtSearch_TextChanged" ToolTip="This is an Auto Complete text box. Just fill in date,PO(full or partial),customer Name(full or partial) or SO." Width="320px"></asp:TextBox>
                                                <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender"
                                                    runat="server"
                                                    CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    CompletionListItemCssClass="autocomplete_listItem"
                                                    CompletionSetCount="25"
                                                    DelimiterCharacters=""
                                                    Enabled="True" MinimumPrefixLength="1"
                                                    ServiceMethod="GetCompletionListIDs"
                                                    ServicePath="" TargetControlID="txtSearch"
                                                    UseContextKey="True"
                                                    CompletionInterval="0">
                                                </ajaxToolkit:AutoCompleteExtender>
                                            </td>
                                            <td style="width: 350px">
                                                <table id="tblRange" align="center" border="0" cellpadding="1" cellspacing="1">
                                                    <tr>
                                                        <td style="float: right !important">
                                                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="8pt" Width="65px">Start Date:</asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:TextBox ID="txtStartDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" ValidationGroup="" Width="150px"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgStartDate" TargetControlID="txtStartDate">
                                                            </ajaxToolkit:CalendarExtender>
                                                            <ajaxToolkit:MaskedEditExtender ID="txtDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtStartDate">
                                                            </ajaxToolkit:MaskedEditExtender>
                                                            <ajaxToolkit:MaskedEditValidator ID="txtDateMEV" runat="server" ControlExtender="txtDateMEE" ControlToValidate="txtStartDate" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                            <ajaxToolkit:ValidatorCalloutExtender ID="txtDateMEVE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtDateMEV" />
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="imgStartDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                        </td>
                                                        <td align="right" style="float: right !important">
                                                            <asp:Label ID="Label26" runat="server" Font-Bold="True" Font-Size="8pt" Width="65px">End Date:</asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:TextBox ID="txtEndDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" ValidationGroup="" Width="150px"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgEndDate" TargetControlID="txtEndDate">
                                                            </ajaxToolkit:CalendarExtender>
                                                            <ajaxToolkit:MaskedEditExtender ID="txtDateMEE2" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtEndDate">
                                                            </ajaxToolkit:MaskedEditExtender>
                                                            <ajaxToolkit:MaskedEditValidator ID="txtDateMEV2" runat="server" ControlExtender="txtDateMEE2" ControlToValidate="txtEndDate" Display="None" EmptyValueMessage="Please enter an end date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                            <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtDateMEV2" />
                                                        </td>
                                                        <td>
                                                            <asp:ImageButton ID="imgEndDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" style="width: 100px">&nbsp;</td>
                                            <td align="left" style="width: 350px">
                                                <asp:Label ID="Label24" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="8pt" ForeColor="Red" Width="250px">**Give autofill at least 5 or 6 seconds to populate.(For POs, no spaces please)</asp:Label>
                                            </td>
                                            <td align="left" style="width: 350px">&nbsp;</td>
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
                                <td align="center">
                                    <asp:Panel ID="pnlMain" runat="server">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="PnlContentEditPDFs" runat="server">
                                                        <table align="center" border="0" bordercolor="black" bordercolordark="#000000" bordercolorlight="#000000">
                                                            <tr>
                                                                <td>
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <table border="0" bordercolor="black" bordercolordark="#000000" bordercolorlight="#000000">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table id="tblGrid" align="center" border="0" cellpadding="0" cellspacing="0">
                                                                                                <tr>
                                                                                                    <td align="left" style="color: #2f3632"></td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="left">
                                                                                                        <asp:GridView ID="gvPDFs" runat="server" AllowPaging="True" AutoGenerateColumns="False" Font-Size="8pt" OnPageIndexChanging="gvPDFs_PageIndexChanging" OnRowCommand="gvPDFs_RowCommand" OnRowDataBound="gvPDFs_RowDataBound" OnRowDeleting="gvPDFs_RowDeleting" Width="1000px" AllowSorting="True" OnSorting="gvPDFs_Sorting">
                                                                                                            <FooterStyle CssClass="DHTR_Grid_Row td" />
                                                                                                            <RowStyle CssClass="DHTR_Grid_Row td" />
                                                                                                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                                                            <Columns>
                                                                                                                <asp:TemplateField ShowHeader="False">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:ImageButton ID="imgDeletePDF" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delete" ImageUrl="Images/Delete.gif" Text="Delete" />
                                                                                                                        <ajaxToolkit:ConfirmButtonExtender ID="imgDeletePDF_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to delete this file?" Enabled="True" TargetControlID="imgDeletePDF">
                                                                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                                    <ItemStyle CssClass="Text_Small" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Company" SortExpression="Company">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:LinkButton ID="lbnCompany" runat="server"
                                                                                                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Company"
                                                                                                                            Text='<%# Bind("Company") %>'></asp:LinkButton>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                                    <ItemStyle CssClass="Text_Small" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Sales Order" SortExpression="SalesOrder">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:LinkButton ID="lbnSalesOrder" runat="server" Text='<%# Bind("SalesOrder") %>'
                                                                                                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="SalesOrder"></asp:LinkButton>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                                    <ItemStyle CssClass="Text_Small" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Purchase Order" SortExpression="CustomerPoNumber">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:LinkButton ID="lbnPurchaseOrder" runat="server" Text='<%# Bind("CustomerPoNumber") %>'
                                                                                                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="PurchaseOrder"></asp:LinkButton>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                                    <ItemStyle CssClass="Text_Small" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="DeliveryID" SortExpression="DeliveryID">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:LinkButton ID="lbnDeliveryID" runat="server" Text='<%# Bind("DeliveryID") %>'
                                                                                                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delivery"></asp:LinkButton>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                                    <ItemStyle CssClass="Text_Small" HorizontalAlign="center" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Delivery Date" SortExpression="DateDelivered">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblDateDelivered" runat="server" Text='<%# Bind("DateDelivered") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                                    <ItemStyle CssClass="Text_Small" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="FileName" SortExpression="FileName">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:LinkButton ID="lbnFileName" runat="server" Text='<%# Bind("FileName") %>'
                                                                                                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Select" Font-Bold="True" ForeColor="Navy"></asp:LinkButton>
                                                                                                                        <asp:Label ID="lblFullPath" runat="server" Text='<%# Bind("FullPath") %>' Visible="false"></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                                    <ItemStyle CssClass="Text_Small" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Creation Date" SortExpression="DateCreated">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblDateCreated" runat="server" Text='<%# Bind("DateCreated") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                                    <ItemStyle CssClass="Text_Small" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Doc Type" SortExpression="DocType">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblDocType" runat="server" Text='<%# Bind("DocType") %>'></asp:Label>
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                                    <ItemStyle CssClass="Text_Small" />
                                                                                                                </asp:TemplateField>
                                                                                                                <asp:TemplateField HeaderText="Files Type" SortExpression="Extension">
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:Label ID="lblExtension" runat="server" Text='<%# Bind("Extension") %>' Visible="false"></asp:Label>
                                                                                                                        <asp:Image ID="imgExtension" runat="server" />
                                                                                                                    </ItemTemplate>
                                                                                                                    <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                                    <ItemStyle CssClass="Text_Small" HorizontalAlign="Center" />
                                                                                                                </asp:TemplateField>
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="center">
                                                                                                        <hr />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="center">
                                                                                                        <asp:Label ID="Label23" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt">Document Upload</asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="center">&nbsp;</td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="left">
                                                                                                        <asp:Label ID="lblMessageUpload" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Red"></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td align="left">&nbsp;</td>
                                                                                                    <td align="left">
                                                                                                        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">Type of Doc?</asp:Label>
                                                                                                    </td>
                                                                                                    <td align="left" style="width: 375px">
                                                                                                        <asp:RadioButtonList ID="rblDocTypeIDSearch" runat="server" Font-Size="8pt" AutoPostBack="True" OnSelectedIndexChanged="rblDocTypeIDSearch_SelectedIndexChanged">
                                                                                                            <asp:ListItem Selected="True">&nbsp;Delivery/Will Call Receipts IDs</asp:ListItem>
                                                                                                            <asp:ListItem>&nbsp;Pickup Receipt IDs</asp:ListItem>
                                                                                                            <asp:ListItem>&nbsp;Purchase Orders #</asp:ListItem>
                                                                                                            <asp:ListItem>&nbsp;Other</asp:ListItem>
                                                                                                        </asp:RadioButtonList>
                                                                                                    </td>
                                                                                                    <td>&nbsp;</td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="left">&nbsp;</td>
                                                                                                    <td align="right">
                                                                                                        <asp:Button ID="btnSearchIDs" runat="server" CssClass="btn btn-primary" OnClick="btnSearchIDs_Click" Text="search" ToolTip="Fill in Sales Order, Purchase Order(full or partial), Company(full or partial), or Date" />
                                                                                                    </td>
                                                                                                    <td align="center" style="width: 375px">
                                                                                                        <asp:TextBox ID="txtSearchIDs" runat="server" AutoPostBack="True" BackColor="LemonChiffon" BorderWidth="2px" CssClass="form-control" OnTextChanged="txtSearchIDs_TextChanged" ToolTip="Options are Sales Order, Purchase Order(full or partial), Company(full or partial), or Date"></asp:TextBox>

                                                                                                    </td>
                                                                                                    <td>&nbsp;</td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td>&nbsp;</td>
                                                                                                    <td>&nbsp;</td>
                                                                                                    <td align="left" style="width: 375px">
                                                                                                        <asp:Label ID="Label25" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt" ForeColor="Navy">Use this tool to associate uploaded documents to specific Delivery Receipts, POs, and SOs by searching for the doc type you want with criteria such as Customer Name, SO, PO or Date (Delivery Date for Delivery Receipts and Order date for SOs &amp; POs.)</asp:Label>
                                                                                                    </td>
                                                                                                    <td>&nbsp;</td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <hr />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="False" GridLines="None" OnRowCommand="gvResults_RowCommand" OnRowDataBound="gvResults_RowDataBound" Width="800px" AllowPaging="True" OnPageIndexChanging="gvResults_PageIndexChanging">
                                                                                                <Columns>
                                                                                                    <asp:TemplateField ShowHeader="False">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton ID="lbnSelect" runat="server" CausesValidation="False" CommandArgument='<%# Bind("ID") %>' CommandName="Select" Text="Select"></asp:LinkButton>
                                                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>' Visible="false"></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Date">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Company">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblCompany" runat="server" Text='<%# Bind("Company") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Sales Order">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblSalesOrder" runat="server" Text='<%# Bind("SalesOrder") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Purchase Order">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblPurchaseOrder" runat="server" Text='<%# Bind("CustomerPoNumber") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle HorizontalAlign="Left" />
                                                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                                                    </asp:TemplateField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <hr />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="LabelID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">ID to be assigned to this document:</asp:Label>
                                                                                            <asp:Label ID="lblID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Navy"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="LabelID1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">Scheduled Delivery/Pickup Date:</asp:Label>
                                                                                            &nbsp;
                                                                                            <asp:Label ID="lblScheduledDeliveryDate" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy" ReadOnly="True"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="label55" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="color: red !important">**If Delivery Date was not the date delivered, please select a date from the box below!</asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:TextBox ID="txtDateDelivered" runat="server" BackColor="LemonChiffon"
                                                                                                            CssClass="form-control" ValidationGroup="" Width="150px"></asp:TextBox>
                                                                                                        <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidator1" runat="server"
                                                                                                            ControlExtender="txtDateMEE"
                                                                                                            ControlToValidate="txtDateDelivered"
                                                                                                            Display="None" EmptyValueMessage="Please enter a start date."
                                                                                                            ErrorMessage="Invalid date format."
                                                                                                            InvalidValueMessage="Invalid date format."
                                                                                                            IsValidEmpty="true" MinimumValue="01/01/1901"
                                                                                                            MinimumValueMessage="Date must be greater than 01/01/1901"
                                                                                                            TooltipMessage="Please enter a date." />
                                                                                                        <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender2"
                                                                                                            runat="server" HighlightCssClass="validatorCalloutHighlight"
                                                                                                            TargetControlID="txtDateMEV" />
                                                                                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server"
                                                                                                            InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date"
                                                                                                            MessageValidatorTip="true" TargetControlID="txtDateDelivered">
                                                                                                        </ajaxToolkit:MaskedEditExtender>
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:ImageButton ID="imgDateDelivered" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                            <ajaxToolkit:CalendarExtender ID="txtDateDelivered_CalendarExtender" runat="server"
                                                                                                Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgDateDelivered" TargetControlID="txtDateDelivered">
                                                                                            </ajaxToolkit:CalendarExtender>

                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">&nbsp;</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">&nbsp;</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td valign="top">
                                                                                                        <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" CssClass="btn btn-primary" />
                                                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnUpload_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to upload this file?" Enabled="True" TargetControlID="btnUpload">
                                                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                                                    </td>
                                                                                                    <td valign="top">
                                                                                                        <table>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="lbl2121" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="Red" Width="225px">*Max Upload Size: 20MB, ZIP to 50MB</asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <asp:Label ID="uploadResult" runat="server" Text="&nbsp;" />
                                                                                                                    <asp:FileUpload ID="fuDocuments" runat="server" Width="300px" />
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                    <td align="left">
                                                                                                        <table>
                                                                                                            <tr>
                                                                                                                <td>&nbsp; </td>
                                                                                                                <td align="left">
                                                                                                                    <asp:Label ID="Label22" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">Document Names:</asp:Label>
                                                                                                                </td>
                                                                                                                <td>&nbsp; </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; </td>
                                                                                                                <td>
                                                                                                                    <asp:RadioButtonList ID="rblDocs" runat="server" Font-Bold="True" Width="300px">
                                                                                                                    </asp:RadioButtonList>
                                                                                                                </td>
                                                                                                                <td></td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td valign="top">&nbsp;</td>
                                                                                                                <td>
                                                                                                                    <asp:TextBox ID="txtCustomName" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="8pt" Width="200px"></asp:TextBox>
                                                                                                                </td>
                                                                                                                <td>&nbsp; </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left">
                                                                                            <table id="clientSide" runat="server" cellpadding="3" style="border-collapse: collapse; border-left: solid 1px #aaaaff; border-top: solid 1px #aaaaff;">
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblUploadErr" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Red"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:GridView ID="gvPDFsUploads" runat="server" AllowPaging="True" AutoGenerateColumns="False" Font-Size="8pt" OnPageIndexChanging="gvPDFsUploads_PageIndexChanging" OnRowCommand="gvPDFsUploads_RowCommand" OnRowDataBound="gvPDFsUploads_RowDataBound" OnRowDeleting="gvPDFsUploads_RowDeleting" Width="900px">
                                                                                                <FooterStyle CssClass="DHTR_Grid_Row td" />
                                                                                                <RowStyle CssClass="DHTR_Grid_Row td" />
                                                                                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                                                <Columns>
                                                                                                    <asp:TemplateField ShowHeader="False">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:ImageButton ID="imgDeletePDF" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delete" ImageUrl="Images/Delete.gif" Text="Delete" />
                                                                                                            <ajaxToolkit:ConfirmButtonExtender ID="imgDeletePDF_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to delete this file?" Enabled="True" TargetControlID="imgDeletePDF">
                                                                                                            </ajaxToolkit:ConfirmButtonExtender>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="FileName">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton ID="lbnFileName" runat="server" Text='<%# Bind("FileName") %>'
                                                                                                                CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Select"></asp:LinkButton>
                                                                                                            <asp:Label ID="lblFullPath" runat="server" Text='<%# Bind("FullPath") %>' Visible="false"></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                        <ItemStyle CssClass="Text_Small" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Creation Date">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblDateCreated" runat="server" Text='<%# Bind("DateCreated") %>'></asp:Label>
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                        <ItemStyle CssClass="Text_Small" />
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:TemplateField HeaderText="Files Type">
                                                                                                        <ItemTemplate>
                                                                                                            <asp:Label ID="lblExtension" runat="server" Text='<%# Bind("Extension") %>' Visible="false"></asp:Label>
                                                                                                            <asp:Image ID="imgExtension" runat="server" />
                                                                                                        </ItemTemplate>
                                                                                                        <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                                                                        <ItemStyle CssClass="Text_Small" HorizontalAlign="Center" />
                                                                                                    </asp:TemplateField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                </td>
                            </tr>
                            <tr>
                                <td align="left">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">&nbsp; &nbsp;</td>
                            </tr>
                        </table>


                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
            <asp:PostBackTrigger ControlID="gvPDFs" />
            <asp:PostBackTrigger ControlID="gvPDFsUploads" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

