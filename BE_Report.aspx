<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="BE_Report.aspx.cs" Inherits="BE_Report" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        function ShowProgressPopup() {
            //Calculate...
            $find("ModalPopupActionButtons").show();
        }
       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <%-- <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
        <ContentTemplate> --%>
    <div>
        <div>
            <table align="center" width="1200">
                <tr>
                    <td align="center">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="BE Report Export" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="background-color: lightblue;" align="center" class="JustRoundedEdgeBothSmall">
                        <asp:Panel ID="pnlSearchCriteria" runat="server" Width="1000px">
                            <table width="1000px">
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:UpdatePanel ID="UpdatePanelRunTop" runat="server">
                                            <ContentTemplate>
                                                <asp:LinkButton ID="imgExportExcel1" runat="server" CssClass="btn btn-success" OnClientClick="ShowProgressPopup()"
                                                    Text="&lt;i class=&quot;glyphicon glyphicon-export&quot;&gt;&lt;/i&gt;&lt;/i&gt;&amp;nbsp;CREATE EXCEL BE REPORT"
                                                    OnClick="imgExportExcel1_Click" ToolTip="Export Full Price Analysis Report" Width="250px"></asp:LinkButton>
                                                <asp:Button runat="server" ID="btnHidden" Style="display: none;" />
                                                <ajaxToolkit:ModalPopupExtender
                                                    ID="ModalPopupActionButtons"
                                                    runat="server" TargetControlID="btnHidden"
                                                    PopupControlID="PanelProgressPopup"
                                                    BehaviorID="ModalPopupActionButtons" BackgroundCssClass="modalBackground">
                                                </ajaxToolkit:ModalPopupExtender>
                                            </ContentTemplate>
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="imgExportExcel1" />
                                            </Triggers>
                                        </asp:UpdatePanel>
                                        <asp:Panel runat="server"
                                            ID="PanelProgressPopup"
                                            Style="display: none;">
                                            <div class="ModalProgressPopup">
                                                <div style="border: medium solid #000080; width: 100%; background-color: white; padding: 10px; margin: 10px; text-align: center">
                                                    <asp:Image ID="ImageProgress"
                                                        runat="server" AlternateText="Loading Please Wait"
                                                        ImageUrl="~/Images/timersmall.gif" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" />
                                                    <span class="modaltext">Your report will be ready in about a minute.                                                 
                                                    </span>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="background-color: lightblue;" align="center" class="JustRoundedEdgeBothSmall">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:HyperLink ID="hlExcel" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Navy"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblError" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr id="trFullReport" runat="server">
                    <td align="center">&nbsp;</td>
                </tr>
                <tr id="trSummary" runat="server">

                    <td align="center">

                        <asp:GridView ID="gvSpreadsheet" runat="server" ForeColor="Black" Font-Size="7pt">
                        </asp:GridView>
                    </td>
                </tr>

            </table>
        </div>
    </div>

    <%--    </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgExportExcel1" />
        </Triggers>
    </asp:UpdatePanel>--%>
</asp:Content>
