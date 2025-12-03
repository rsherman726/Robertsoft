<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PriceChangesUpdateSystem.aspx.cs" Inherits="PriceChangesUpdateSystem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
        <ContentTemplate>
            <table id="TableStep1" border="0" cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td align="center">
                        <table id="TableStep1a" border="0" cellpadding="1" cellspacing="1" width="1000px">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="16pt" ForeColor="Navy">IMPORT/EXPORT NEW PRICES FOR EMAILER (Step 1)</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Font-Names="Arial" Font-Size="12pt" Font-Bold="True" ForeColor="Red">Please Note: The only statuses you can use in the template are: ACTIVE, TO BE DISCONTINUED, INACTIVE, MARKET PRICE</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="12pt" Width="125px" Font-Bold="True" ForeColor="Black">Pick Excel File:</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table id="TableStep1b" border="0" cellpadding="1" cellspacing="1" width="300">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="verdana" Font-Size="10pt" ForeColor="Red" Width="14px">1.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="FileUploadExcelStep1" runat="server" Width="300px" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="verdana" Font-Size="10pt" ForeColor="Red" Width="14px">2.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:LinkButton ID="btnUploadStep1" runat="server" OnClick="btnUploadStep1_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Click to upload excel file."><i class="fa fa-upload" ></i>&nbsp;Upload File</asp:LinkButton>

                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="verdana" Font-Size="10pt" ForeColor="Red" Width="14px">3.</asp:Label>
                                    &nbsp;
                             <asp:UpdatePanel ID="UpdatePanelStep1" runat="server">
                                 <ContentTemplate>
                                     <asp:LinkButton ID="btnConvertToSQLStep1" runat="server" Enabled="false" OnClick="btnConvertToSQLStep1_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Note: Updating data could take up to a few minutes. Writes to ArNewPriceHolding"><i class="glyphicon glyphicon-ok"></i>&nbsp;Update SQL</asp:LinkButton>

                                     <br />
                                     <table align="left">
                                         <tr>
                                             <td align="left">
                                                 <asp:Label ID="lblErrorSQLStep1" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="Red"></asp:Label></td>
                                         </tr>
                                     </table>
                                     <br />
                                             <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelStep1">
                                                 <ProgressTemplate>
                                                     <table style="width: 125px">
                                                         <tr>
                                                             <td align="center">
                                                                 <img src="Images/uploading.gif" alt="" />
                                                             </td>
                                                             <td align="left"><strong><span style="font-size: 8pt; font-family: Arial; color: Navy;">processing....</span></strong> </td>
                                                         </tr>
                                                     </table>
                                                 </ProgressTemplate>
                                             </asp:UpdateProgress>
                                         </ContentTemplate>
                                     </asp:UpdatePanel>
                                </td>
                            </tr> 
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblErrorUploadStep1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" CssClass="btn btn-success" Width="200px" ForeColor="White" ToolTip="Click to Export Customer New Price List"><i class="glyphicon glyphicon-export" ></i></i>&nbsp;Export New Price List</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:HyperLink ID="hlTaxBillTemplate" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt" ForeColor="Navy" NavigateUrl="~/Images/Templates/FelbroPriceTemplate.xlsx">New Prices Data Excel Template download Step 1</asp:HyperLink>
                    </td>
                </tr>
            </table>
            <table id="TableStep2" border="0" cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td align="center">
                        <table id="TableStep2a" border="0" cellpadding="1" cellspacing="1" width="1000px">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="16pt" ForeColor="Navy">IMPORT NEW PRICES FOR EMAILER (Step 2)</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label3" runat="server" Font-Names="Arial" Font-Size="12pt" Width="125px" Font-Bold="True" ForeColor="Black">Pick Excel File:</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table id="TableStep2b" border="0" cellpadding="1" cellspacing="1" width="300">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="verdana" Font-Size="10pt" ForeColor="Red" Width="14px">4.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="FileUploadExcelStep2" runat="server" Width="300px" BackColor="LemonChiffon" CssClass="form-control" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Names="verdana" Font-Size="10pt" ForeColor="Red" Width="14px">5.</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:LinkButton ID="btnUploadStep2" runat="server" OnClick="btnUploadStep2_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Click to upload excel file."><i class="fa fa-upload" ></i>&nbsp;Upload File</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Names="verdana" Font-Size="10pt" ForeColor="Red" Width="14px">6.</asp:Label>
                                    &nbsp;
                                  <asp:UpdatePanel ID="UpdatePanelStep2" runat="server">
                                    <ContentTemplate>
                                     <asp:LinkButton ID="btnConvertToSQLStep2" runat="server" Enabled="false" OnClick="btnConvertToSQLStep2_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Note: Updating data could take up to a few minutes. Writes to ArNewSalesPrice"><i class="glyphicon glyphicon-ok"></i>&nbsp;Update SQL</asp:LinkButton>

                                     <br />
                                     <table align="left">
                                         <tr>
                                             <td align="left">
                                                 <asp:Label ID="lblErrorSQLStep2" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="Red"></asp:Label></td>
                                         </tr>
                                     </table>
                                     <br />
                                     <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanelStep2">
                                         <ProgressTemplate>
                                             <table style="width: 125px">
                                                 <tr>
                                                     <td style="width: 21px" align="center">
                                                         <img src="Images/uploading.gif" />
                                                     </td>
                                                     <td align="left"><strong><span style="font-size: 8pt; font-family: Arial; color: Navy;">processing....</span></strong> </td>
                                                 </tr>
                                             </table>
                                         </ProgressTemplate>
                                     </asp:UpdateProgress>
                                 </ContentTemplate>
                             </asp:UpdatePanel>
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblErrorUploadStep2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt" ForeColor="Navy" NavigateUrl="~/Images/Templates/FelbroNewPriceTemplate.xlsx">New Prices Data Excel Template download Step 2</asp:HyperLink>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUploadStep1" />
            <asp:PostBackTrigger ControlID="btnExport" />
            <asp:PostBackTrigger ControlID="btnUploadStep2" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

