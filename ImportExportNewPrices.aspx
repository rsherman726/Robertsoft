<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ImportExportNewPrices.aspx.cs" Inherits="ImportExportNewPrices" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <table id="Table2" border="0" cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td align="center">
                        <table id="Table1" border="0" cellpadding="1" cellspacing="1" width="500">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="16pt" ForeColor="Navy">IMPORT/EXPORT NEW PRICES FOR EMAILER</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="12pt" Width="125px" Font-Bold="True" ForeColor="Black">Pick Excel File:</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table id="Table3" border="0" cellpadding="1" cellspacing="1" width="300">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="verdana" Font-Size="10pt" ForeColor="Red" Width="14px">1.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="FileUploadExcel" runat="server" Width="300px" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black" />
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
                                    <asp:LinkButton ID="btnUpload" runat="server" OnClick="btnUpload_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Click to upload excel file."><i class="fa fa-upload" ></i>&nbsp;Upload File</asp:LinkButton>

                                </td>
                            </tr> 
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Names="verdana" Font-Size="10pt" ForeColor="Red" Width="14px">3.</asp:Label>
                                    &nbsp;
                             <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                 <ContentTemplate>
                                     <asp:LinkButton ID="btnConvertToSQL" runat="server" Enabled="false" OnClick="btnConvertToSQL_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Note: Updating data could take up to a few minutes."><i class="glyphicon glyphicon-ok"></i>&nbsp;Update SQL</asp:LinkButton>

                                     <br />
                                     <table align="left">
                                         <tr>
                                             <td align="left">
                                                 <asp:Label ID="lblErrorSQL" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="Red"></asp:Label></td>
                                         </tr>
                                     </table>
                                     <br />
                                     <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
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
                        <asp:Label ID="lblErrorUpload" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>                
                <tr>
                    <td align="center">
                        <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" CssClass="btn btn-success" Width="200px" ForeColor="White" ToolTip="Click to Export Customer New Price List"><i class="glyphicon glyphicon-export" ></i></i>&nbsp;Export New Price List</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:HyperLink ID="hlTaxBillTemplate" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt" ForeColor="Navy" NavigateUrl="~/Images/Templates/FelbroPriceTemplate.xlsx">New Prices Data Excel Template download</asp:HyperLink>
                    </td>
                </tr>
            </table>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
            <asp:PostBackTrigger ControlID="btnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

