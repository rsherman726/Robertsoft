<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="RecipeComponentReport.aspx.cs" Inherits="RecipeComponentReport" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

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
                                    <td align="left">
                                        <img src="Images/loader.gif" alt="" style="border: thin solid #000000" />
                                    </td>
                                    <td align="left"><span style="color: #ffffff">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing <span class="">....</span> </strong></span></span></td>
                                </tr>
                            </tbody>
                        </table>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div>
                <div>
                    <table align="center" width="1050px">
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Recipe Component Report - Single Selection" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <table width="1000px" style="background-color: lightblue;" align="center" class="JustRoundedEdgeBothSmall">
                                    <tr>
                                        <td align="center">
                                            <table width="950px">
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="lblStockCodeList" runat="server" Font-Bold="True" ForeColor="Navy" Text="Stock Code(s)"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:ListBox ID="lbParentStockCode" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="9pt" ForeColor="Black" Height="250px" AutoPostBack="True" OnSelectedIndexChanged="lbParentStockCode_SelectedIndexChanged"></asp:ListBox>
                                                        <ajaxToolkit:ListSearchExtender ID="lbParentStockCode_ListSearchExtender" runat="server" Enabled="True" IsSorted="True" PromptCssClass="Prompt" TargetControlID="lbParentStockCode">
                                                        </ajaxToolkit:ListSearchExtender>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:LinkButton ID="lbClearAll" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbnClearStockCode_Click" ValidationGroup="nothing">Clear All</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:LinkButton ID="btnPreview" runat="server" CssClass="btn btn-info" ForeColor="White" OnClick="btnPreview_Click" ToolTip="Click to run full report" Width="200px"><i class="fa fa-file-text-o"></i>&nbsp;Full Report</asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">&nbsp;</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblError" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblOptions" runat="server" Font-Names="Arial" Font-Size="Small" ForeColor="Black" Style="text-align: center" Text="Export Options:" Visible="False"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgbtnExcel" runat="server" Height="40px" ImageUrl="~/Images/ExcelLogoSmall.scale-80.png" OnClick="imgbtnExcel_Click" Text="Button" Visible="false" />
                                        </td>
                                        <td align="center">
                                            <asp:ImageButton ID="imgbtnPDF" runat="server" Height="20px" ImageUrl="~/Images/pdf-symbol.png" OnClick="imgbtnPDF_Click" Text="Button" Visible="False" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgbtnWord" runat="server" Height="40px" ImageUrl="~/Images/WinWordLogoSmall.scale-80.png" OnClick="imgbtnWord_Click" Text="Button" Visible="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:Panel ID="pnlForm" runat="server" Width="1000px" ScrollBars="Horizontal">
                                                <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr class="rowColors">
                                        <td align="left">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgbtnExcel" />
            <asp:PostBackTrigger ControlID="imgbtnPDF" />
            <asp:PostBackTrigger ControlID="imgbtnWord" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

