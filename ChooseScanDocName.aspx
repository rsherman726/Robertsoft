<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ChooseScanDocName.aspx.cs" Inherits="ChooseScanDocName" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
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
                                        <img src="Images/loader.gif" alt="" style="border: thin solid #000000" />
                                    </td>
                                    <td align="left"><span style="color: #ffffff">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing <span class="">....</span> </strong></span></span></td>
                                </tr>
                            </tbody>
                        </table>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <table align="center" width="900">
                <tr>
                    <td align="center">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                            Text="Please select a document name"></asp:Label></td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
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
                                    <asp:RadioButtonList ID="rblDocTypeIDSearch" runat="server" Font-Size="8pt" AutoPostBack="True" OnSelectedIndexChanged="rblDocTypeIDSearch_SelectedIndexChanged" Width="400px">
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
                                <td align="right">&nbsp;</td>
                                <td align="center" style="width: 375px">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left">&nbsp;</td>
                                <td align="right">
                                    <asp:Button ID="btnSearchIDs" runat="server" CssClass="btn btn-primary" OnClick="btnSearchIDs_Click" Text="search" ToolTip="Fill in Sales Order, Purchase Order(full or partial), Company(full or partial),Customer#, or Date" />
                                </td>
                                <td align="center" style="width: 375px">
                                    <asp:TextBox ID="txtSearchIDs" runat="server" AutoPostBack="True" Style="background-color: lemonchiffon!important" BorderWidth="2px" CssClass="form-control" OnTextChanged="txtSearchIDs_TextChanged" ToolTip="Options are Sales Order, Purchase Order(full or partial), Company(full or partial),Customer #, or Date"></asp:TextBox>

                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td align="left" style="width: 375px">&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td align="left" style="width: 375px">
                                    <asp:Label ID="Label25" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt" ForeColor="Navy" Style="text-align: left">Use this tool to associate scanned documents to specific Delivery Receipts, POs, and SOs by searching for the doc type you want with criteria such as Customer Name, SO, PO or Date (Delivery Date for Delivery Receipts and Order date for SOs &amp; POs.)</asp:Label>
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td>

                        <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="False" GridLines="None" OnRowCommand="gvResults_RowCommand" OnRowDataBound="gvResults_RowDataBound" Width="800px" AllowPaging="True" OnPageIndexChanging="gvResults_PageIndexChanging" PageSize="15">
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbnSelect" runat="server" CausesValidation="False" CommandArgument='<%# Bind("ID") %>' CommandName="Select" Text="Select" CssClass="NoUnderline" Font-Bold="True"></asp:LinkButton>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Order Date/Delivery Date">
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
                                <asp:TemplateField HeaderText="Sales<br/> Order">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSalesOrder" runat="server" Text='<%# Bind("SalesOrder") %>'></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Purchase<br/>Order">
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
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="LabelID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">ID to be assigned to this document:</asp:Label>
                                </td>
                                <td>&nbsp;</td>
                                <td align="right">
                                    <asp:Label ID="lblID" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy" ReadOnly="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="LabelID1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">Document Type:</asp:Label>
                                </td>
                                <td>&nbsp;</td>
                                <td align="right">
                                    <asp:Label ID="lblDocType" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy" ReadOnly="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="LabelID0" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">Scheduled Delivery/Pickup Date:</asp:Label>
                                </td>
                                <td>&nbsp;</td>
                                <td align="right">
                                    <asp:Label ID="lblScheduledDeliveryDate" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy" ReadOnly="True"></asp:Label>
                                </td>
                            </tr>
                        </table>
                       &nbsp; 
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="label55" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" Style="color: red !important">**If Delivery/Pickup Date was not the date delivered, please select a date from the box below!</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td> 
                                    <asp:TextBox ID="txtDateDelivered" runat="server" BackColor="LemonChiffon" 
                                        CssClass="form-control" ValidationGroup="" Width="150px"></asp:TextBox>                               
                                    <ajaxToolkit:MaskedEditValidator ID="txtDateMEV" runat="server" 
                                        ControlExtender="txtDateMEE" 
                                        ControlToValidate="txtDateDelivered" 
                                        Display="None" EmptyValueMessage="Please enter a start date." 
                                        ErrorMessage="Invalid date format." 
                                        InvalidValueMessage="Invalid date format." 
                                        IsValidEmpty="true" MinimumValue="01/01/1901" 
                                        MinimumValueMessage="Date must be greater than 01/01/1901" 
                                        TooltipMessage="Please enter a date." />
                                    <ajaxToolkit:ValidatorCalloutExtender ID="txtDateMEVE" 
                                        runat="server" HighlightCssClass="validatorCalloutHighlight" 
                                        TargetControlID="txtDateMEV" />
                                   <ajaxToolkit:MaskedEditExtender ID="txtDateMEE" runat="server" 
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
                    <td align="center">
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label22" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">Document Names:</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:RadioButtonList ID="rblDocs" runat="server" Font-Bold="True" Width="300px">
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtCustomName" runat="server" Style="background-color: LemonChiffon !important" CssClass="form-control" Font-Size="8pt"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" Style="color: red !important"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" OnClick="btnSubmit_Click" Text="Go to Scanner" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

