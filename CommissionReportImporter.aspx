<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CommissionReportImporter.aspx.cs" Inherits="CommissionReportImporter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function ShowProgressPopup() {
            //Calculate...
            $find("ModalPopupActionButtons").show();
        }
        function ShowProgressPopup1() {
            //Calculate...
            $find("ModalPopupActionButtons1").show();
        }
    </script>
    <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
        <ContentTemplate>
            <div style="position: fixed; right: 50px; height: 40px; width: 150px; z-index: 10000;">
                <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" AssociatedUpdatePanelID="UpdatePanelPromo" DisplayAfter="1">

                    <ProgressTemplate>
                        <table>
                            <tbody>
                                <tr>
                                    <td align="Left"  >
                                        <img src="Images/loader.gif" alt="" style="border: thin solid #000000" />
                                    </td>
                                    <td align="left"><span style="color: #ffffff">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing <span class="">....</span> </strong></span></span></td>
                                </tr>
                            </tbody>
                        </table>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <table id="Table2" border="0" cellpadding="1" cellspacing="1" width="500" align="center" style="background-color: white" class="JustRoundedEdgeBothSmall">
                <tr>
                    <td align="center">
                        <table id="Table1" border="0" cellpadding="1" cellspacing="1" width="500">
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="XX-Large" Width="725px" ForeColor="Black">Commission Report Importer</asp:Label>
                                </td>
                            </tr>
                           
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="10pt" Width="125px" Font-Bold="True" ForeColor="Black">Pick a File:</asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table id="Table3" border="0" cellpadding="1" cellspacing="1" width="300">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Width="75px">Step 1.</asp:Label>
                                            </td>
                                            <td>
                                                <asp:FileUpload ID="FileUpload1" runat="server" Width="400px" BorderColor="LemonChiffon" Height="35px" ForeColor="Black" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <div style="font-family: arial, Helvetica, sans-serif; font-size: 10pt; font-weight: bold; color: #333333;">
                                    </div>
                                </td>
                            </tr>                          
                          
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" Width="75px">Step 2.</asp:Label>
                                    <asp:UpdatePanel ID="UpdatePanelRunTop" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnUpload" runat="server" Text="Upload File" ToolTip="Click to upload a data file." Font-Bold="True" OnClientClick="ShowProgressPopup()" OnClick="btnUpload_Click" CssClass="btn btn-primary" Font-Names="arial" />
                                            <asp:Button runat="server" ID="btnHidden" Style="display: none;" />
                                            <ajaxToolkit:ModalPopupExtender
                                                ID="ModalPopupActionButtons"
                                                runat="server" TargetControlID="btnHidden"
                                                PopupControlID="PanelProgressPopup"
                                                BehaviorID="ModalPopupActionButtons" BackgroundCssClass="modalBackground">
                                            </ajaxToolkit:ModalPopupExtender>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <asp:Panel runat="server"
                                        ID="PanelProgressPopup"
                                        Style="display: none;">
                                        <div class="ModalProgressPopup">
                                            <p style="border: medium solid #000000; padding: 10px; margin: 10px; font-family: arial, Helvetica, sans-serif; font-size: 12pt; font-weight: bold; color: red; text-align: left; width: 400px; background-color: whitesmoke;">
                                                <asp:Image ID="ImageProgress"
                                                    runat="server" AlternateText="Loading Please Wait"
                                                    ImageUrl="~/Images/loader.gif" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" /><br />
                                                Please wait while we import your data...
                                            </p>
                                        </div>
                                    </asp:Panel>

                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblErrorSQL" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gvResults" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" Font-Names="Arial" Font-Size="8pt" GridLines="Vertical" Width="740px">
                                        <AlternatingRowStyle BackColor="#DCDCDC" />
                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                        <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" CssClass="CenterAligner" />
                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                        <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                        <SortedDescendingHeaderStyle BackColor="#000065" />
                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                        <RowStyle HorizontalAlign="Center" />
                                    </asp:GridView>
                                </td>
                            </tr>                           
                        </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

