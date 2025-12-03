<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ExecutiveSummary.aspx.cs" Inherits="ExecutiveSummary" %>

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
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Executive Summary" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:DetailsView ID="dvES" runat="server" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px"
                            CellPadding="2" ForeColor="Black" GridLines="None" Height="50px" Width="500px" AutoGenerateRows="False">
                            <AlternatingRowStyle BackColor="PaleGoldenrod" HorizontalAlign="Right" />
                            <EditRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                            <EmptyDataTemplate>
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="**No Records Found!!!" ForeColor="Red"></asp:Label></EmptyDataTemplate>
                            <EmptyDataRowStyle HorizontalAlign="Center" />
                            <Fields>
                                <asp:TemplateField HeaderText="Bank Balance">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBankBalance" runat="server" Text='<%# Bind("BankBalance") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cash - Checking">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCashChecking" runat="server" Text='<%# Bind("CashChecking") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cash - MM/CD">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCashMMCD" runat="server" Text='<%# Bind("CashMMCD") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="A/P Balance">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAPBalance" runat="server" Text='<%# Bind("APBalance") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="UnMatched Grn">
                                    <ItemTemplate>
                                        <asp:Label ID="lblUnMatchedGrn" runat="server" Text='<%# Bind("UnMatchedGrn") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="A/R Balance">
                                    <ItemTemplate>
                                        <asp:Label ID="lblARBalance" runat="server" Text='<%# Bind("ARBalance") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Inventory Valuation">
                                    <ItemTemplate>
                                        <asp:Label ID="lblInventoryValuation" runat="server" Text='<%# Bind("InventoryValuation") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="[Purchase Order Commitment]">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPurchaseOrderCommitment" runat="server" Text='<%# Bind("PurchaseOrderCommitment") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sales Orders Taken - Today">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSalesOrdersTakenToday" runat="server" Text='<%# Bind("SalesOrdersTakenToday") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sales Orders Taken - Yesterday">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSalesOrdersTakenYesterday" runat="server" Text='<%# Bind("SalesOrdersTakenYesterday") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Sales Orders Taken - This Week (7 Days back from Today)">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSalesOrdersTakenThisWeek" runat="server" Text='<%# Bind("SalesOrdersTakenThisWeek") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Revenue - This Month">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRevenueThisMonth" runat="server" Text='<%# Bind("RevenueThisMonth") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Revenue - This Year">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRevenueThisYear" runat="server" Text='<%# Bind("RevenueThisYear") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Margin $ - This Month" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMarginMonthDollars" runat="server" Text='<%# Bind("MarginMonthDollars") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Margin % - This Month">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMTD_ProfitPct" runat="server" Text='<%# Bind("MTD_ProfitPct") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Margin $ - This Year" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMarginYearDollars" runat="server" Text='<%# Bind("MarginYearDollars") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Margin % - This Year">
                                    <ItemTemplate>
                                        <asp:Label ID="lblYTD_ProfitPct" runat="server" Text='<%# Bind("YTD_ProfitPct") %>' ForeColor="Black"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>

                            </Fields>
                            <FooterStyle BackColor="Tan" />
                            <HeaderStyle BackColor="Tan" Font-Bold="True" />
                            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
                            <RowStyle HorizontalAlign="Right" />
                            <HeaderTemplate>
                            </HeaderTemplate>
                        </asp:DetailsView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

