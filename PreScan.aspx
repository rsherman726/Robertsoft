<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PreScan.aspx.cs" Inherits="PreScan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .auto-style1 {
            height: 22px;
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
                            Text="Pre Scan"></asp:Label></td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>

                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="LabelID2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Blue">Verify your information below!</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td align="left" >
                                    <asp:Label ID="LabelID5" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">Customer:</asp:Label>
                                </td>
                                <td >&nbsp;</td>
                                <td align="right" >
                                    <asp:Label ID="lblCustomer" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy" ReadOnly="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="LabelSalesOrder" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">Sales Order:</asp:Label>
                                </td>
                                <td></td>
                                <td align="right">
                                    <asp:Label ID="lblSO" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy" ReadOnly="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Label ID="LabelPO" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt">Customer PO:</asp:Label>
                                </td>
                                <td>&nbsp;</td>
                                <td align="right">
                                    <asp:Label ID="lblPO" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy" ReadOnly="True"></asp:Label>
                                </td>
                            </tr>
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
                    </td>
                </tr>
                <tr>
                    <td align="center">
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
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" Style="color: red !important"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-primary" OnClick="btnSubmit_Click" Text="Go to Scanner" />
                    </td>
                </tr>
              
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

