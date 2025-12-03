<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TopFifteenReport.aspx.cs" Inherits="TopFifteenReport" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .rowColors tr:hover {
            background-color: #05b3f5 !important;
        }

        .rowColors tr:hover {
            color: #FFF !important;
            transform-st !important;
        }

        .Prompt {
            color: navy;
            font-weight: bold;
        }

        .RowStyle {
            height: 50px;
        }

        .AlternateRowStyle {
            height: 50px;
            background-color: LemonChiffon;
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
                    <table align="center" width="1200">
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Top 15 &amp; Goal Pace 250k+ Reports" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Black" Text="Top 15 Report"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="background-color: lightblue;" align="center" class="JustRoundedEdgeBothSmall">

                                <table width="1100">
                                    <tr>
                                        <td align="center">
                                            <table>
                                                <tr>
                                                    <td align="center">
                                                        <asp:Label ID="Label3" runat="server" Width="200px" Font-Bold="True" ForeColor="Black">Rank By</asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:RadioButtonList ID="rblTopTen" runat="server" AutoPostBack="True" ForeColor="Black" OnSelectedIndexChanged="rblTopTen_SelectedIndexChanged" RepeatDirection="Horizontal" Width="600px" Font-Size="13pt">
                                                            <asp:ListItem Selected="True" Value="Y">&nbsp;YTD</asp:ListItem>
                                                            <asp:ListItem Value="S">&nbsp;YTD thru End of Last Month</asp:ListItem>
                                                            <%--  <asp:ListItem Value="L">&nbsp;Last YTD</asp:ListItem>
                                                            <asp:ListItem Value="M">&nbsp;MTD</asp:ListItem>--%>
                                                        </asp:RadioButtonList>
                                                        <table>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:CheckBox ID="chkShowLastColumns" runat="server" AutoPostBack="True" ForeColor="Blue" OnCheckedChanged="chkShowLastColumns_CheckedChanged" Text="Show Open Orders and Ready to Invoice" />
                                                                </td>
                                                            </tr>
                                                            <%-- <tr>
                                                                <td align="left">
                                                                    <asp:CheckBox ID="chkMonthToDate" runat="server" AutoPostBack="True" ForeColor="Black" OnCheckedChanged="chkMonthToDate_CheckedChanged" Text="Show Month as MTD" />
                                                                </td>
                                                            </tr>--%>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:CheckBox ID="chkShowYTDthroughLastMonth" runat="server" AutoPostBack="True" ForeColor="Blue" OnCheckedChanged="chkShowYTDthroughLastMonth_CheckedChanged" Text="Show YTD though End of Last Month" />
                                                                </td>
                                                            </tr>
                                                        </table>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:GridView ID="gvTopFifteen" runat="server" AutoGenerateColumns="False" ForeColor="Black" Width="1450px"
                                                            Font-Size="10pt" BackColor="White" ShowFooter="True" OnRowDataBound="gvTopFifteen_RowDataBound"
                                                            BorderStyle="Solid" BorderWidth="1px"
                                                            BorderColor="Gainsboro"
                                                            AllowSorting="True"
                                                            OnSorting="gvTopFifteen_Sorting">
                                                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                            <RowStyle BackColor="White" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="300px" Font-Bold="true" ToolTip='<%# Bind("Customer") %>' Style="cursor: pointer"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblTotals" runat="server" Text="Totals:" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--<asp:TemplateField HeaderText="Cust#" Visible="false">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>--%>

                                                                <asp:TemplateField HeaderText="LM" SortExpression="PreviousMonthAmount">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPreviousMonthAmountD" runat="server" Text='<%# Bind("PreviousMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblPreviousMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="LYLM" SortExpression="LastYearPreviousMonthAmount">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYearPreviousMonthAmountD" runat="server" Text='<%# Bind("LastYearPreviousMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYearPreviousMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="MTD" SortExpression="CurrentMTDAmount">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentMTDAmountD" runat="server" Text='<%# Bind("CurrentMTDAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentMTDAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="LYMTD" SortExpression="LastYearCurrentMTDAmount">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYearCurrentMTDAmountD" runat="server" Text='<%# Bind("LastYearCurrentMTDAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYearCurrentMTDAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="CM" SortExpression="CurrentMonthAmount">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentMonthAmountD" runat="server" Text='<%# Bind("CurrentMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="LYCM" SortExpression="LastYearCurrentMonthAmount">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYearCurrentMonthAmountD" runat="server" Text='<%# Bind("LastYearCurrentMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYearCurrentMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--YTD--%>
                                                                <asp:TemplateField HeaderText="YTD" SortExpression="CurrentYTDAmount">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentYTDAmountD" runat="server" Text='<%# Bind("CurrentYTDAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentYTDAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="LYTD" SortExpression="LastYTDAmount">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYTDAmountD" runat="server" Text='<%# Bind("LastYTDAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYTDAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="LLYTD" SortExpression="LastYTDMinusOneAmount">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYTDMinusOneAmountD" runat="server" Text='<%# Bind("LastYTDMinusOneAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYTDMinusOneAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="YR to YR DIFF %" SortExpression="YearToYearDiffPercentage" Visible="false">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblYearToYearDiffPercentage" runat="server" Text='<%# Bind("YearToYearDiffPercentage") %>' Width="100px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblYearToYearDiffPercentageWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="YR to YR DIFF MINUS ONE %" SortExpression="YearToYearDiffPercentageMinusOne" Visible="false">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblYearToYearDiffPercentageMinusOne" runat="server" Text='<%# Bind("YearToYearDiffPercentageMinusOne") %>' Width="100px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblYearToYearDiffPercentageMinusOneWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="YTD MARGIN %" SortExpression="CurrentYTDMargin">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentYTDMargin" runat="server" Text='<%# Bind("CurrentYTDMargin") %>'></asp:Label>
                                                                        <asp:Label ID="lblCurrentYTDCost" runat="server" Text='<%# Bind("CurrentYTDCost") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentYTDMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="LYTD MARGIN %" SortExpression="LastYTDMargin">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYTDMargin" runat="server" Text='<%# Bind("LastYTDMargin") %>'></asp:Label>
                                                                        <asp:Label ID="lblLastYTDCost" runat="server" Text='<%# Bind("LastYTDCost") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYTDAmountMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="YTD %" SortExpression="CurrentYTDAmountPercentageOfTotal">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentYTDAmountPercentageOfTotal" runat="server" Text='<%# Bind("CurrentYTDAmountPercentageOfTotal") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentYTDAmountPercentageOfTotalSum" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="LYTD %" SortExpression="LastYTDAmountPercentageOfTotal">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYTDAmountPercentageOfTotal" runat="server" Text='<%# Bind("LastYTDAmountPercentageOfTotal") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYTDAmountPercentageOfTotalSum" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--YTD End of Month--%>
                                                                <asp:TemplateField HeaderText="YTDEM" SortExpression="CurrentYTDEndOfMonthAmountD">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentYTDEndOfMonthAmountD" runat="server" Text='<%# Bind("CurrentYTDEndOfMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentYTDEndOfMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="LYTDEM" SortExpression="LastYTDEndOfMonthAmountD">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYTDEndOfMonthAmountD" runat="server" Text='<%# Bind("LastYTDEndOfMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYTDEndOfMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="LLYTDEM" SortExpression="LastYTDMinusOneYearEndOfMonthAmountD">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYTDMinusOneYearEndOfMonthAmountD" runat="server" Text='<%# Bind("LastYTDMinusOneYearEndOfMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYTDMinusOneYearEndOfMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="YR to YR DIFF %" SortExpression="YearToYearDiffPercentageEndOfMonth" Visible="false">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblYearToYearDiffPercentageEndOfMonth" runat="server" Text='<%# Bind("YearToYearDiffPercentageEndOfMonth") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblYearToYearDiffPercentageEndOfMonthWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="YR to YR DIFF MINUS ONE%" SortExpression="YearToYearDiffPercentageEndOfMonthMinusOne" Visible="false">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblYearToYearDiffPercentageEndOfMonthMinusOne" runat="server" Text='<%# Bind("YearToYearDiffPercentageEndOfMonthMinusOne") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblYearToYearDiffPercentageEndOfMonthMinusOneWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Current Month Margin" SortExpression="CurrentMonthMargin">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentMonthMargin" runat="server" Text='<%# Bind("CurrentMonthMargin") %>' Width="100px"></asp:Label>
                                                                        <asp:Label ID="lblCurrentMonthCost" runat="server" Text='<%# Bind("CurrentMonthCost") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentMonthMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Last Month Margin" SortExpression="LastMonthMargin">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastMonthMargin" runat="server" Text='<%# Bind("LastMonthMargin") %>' Width="100px"></asp:Label>
                                                                        <asp:Label ID="lblPreviousMonthCost" runat="server" Text='<%# Bind("PreviousMonthCost") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastMonthMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="YTD MARGIN %" SortExpression="CurrentYTDEndOfMonthMargin">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentYTDEndOfMonthMargin" runat="server" Text='<%# Bind("CurrentYTDEndOfMonthMargin") %>'></asp:Label>
                                                                        <asp:Label ID="lblCurrentYTDEndOfMonthCost" runat="server" Text='<%# Bind("CurrentYTDEndOfMonthCost") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentYTDEndOfMonthMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="LYTD MARGIN %" SortExpression="LastYTDEndOfMonthMargin">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYTDEndOfMonthMargin" runat="server" Text='<%# Bind("LastYTDEndOfMonthMargin") %>'></asp:Label>
                                                                        <asp:Label ID="lblLastYTDEndOfMonthCost" runat="server" Text='<%# Bind("LastYTDEndOfMonthCost") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYTDEndOfMonthAmountMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="YTD %" SortExpression="CurrentYTDEndOfMonthAmountPercentageOfTotal">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentYTDEndOfMonthAmountPercentageOfTotal" runat="server" Text='<%# Bind("CurrentYTDEndOfMonthAmountPercentageOfTotal") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentYTDEndOfMonthAmountPercentageOfTotalSum" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="LYTD %" SortExpression="LastYTDEndOfMonthAmountPercentageOfTotal">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYTDEndOfMonthAmountPercentageOfTotal" runat="server" Text='<%# Bind("LastYTDEndOfMonthAmountPercentageOfTotal") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYTDEndOfMonthAmountPercentageOfTotalSum" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--Open Orders & Ready To Invoice --%>
                                                                <asp:TemplateField HeaderText="Open&lt;br&gt; Orders<br>Not Ready&lt;br&gt;To&lt;br&gt; Invoice" SortExpression="OpenOrdersAmount">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblOpenOrdersAmountD" runat="server" Text='<%# Bind("OpenOrdersAmountD") %>' Font-Bold="true"></asp:Label>
                                                                        <%-- &nbsp;/&nbsp;<asp:Label ID="lblOpenOrdersAmountPercentageOfTotal" runat="server" Text='<%# Bind("OpenOrdersAmountPercentageOfTotal") %>'></asp:Label>--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblOpenOrdersAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Ready&lt;br&gt;To&lt;br&gt; Invoice" SortExpression="ReadyToInvoiceAmount">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblReadyToInvoiceAmountD" runat="server" Text='<%# Bind("ReadyToInvoiceAmountD") %>' Font-Bold="true"></asp:Label>
                                                                        <%-- &nbsp;/&nbsp;<asp:Label ID="lblReadyToInvoiceAmountPercentageOfTotal" runat="server" Text='<%# Bind("ReadyToInvoiceAmountPercentageOfTotal") %>'></asp:Label>--%>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblReadyToInvoiceAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="UnInvoiced Total" SortExpression="CombinedOpenOrdersAmountD">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCombinedOpenOrdersAmountD" runat="server" Text='<%# Bind("CombinedOpenOrdersAmountD") %>' Font-Bold="true"></asp:Label>
                                                                        <asp:Label ID="lblGrandTotalCombinedOpenOrdersAmount" runat="server" Text='<%# Bind("GrandTotalCombinedOpenOrdersAmount") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCombinedOpenOrdersAmountDTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <FooterStyle BackColor="#CCCCCC" />
                                                            <HeaderStyle BackColor="Black" CssClass="CenterAligner" ForeColor="White" HorizontalAlign="Center" />
                                                            <AlternatingRowStyle HorizontalAlign="Center" />
                                                            <RowStyle CssClass="CenterAligner" ForeColor="Black" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="background-color: #FFFFFF" align="center">
                                                        <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="Large" ForeColor="Black" Text="Goal Pace 250k+ Report"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <asp:GridView ID="gvGoalPace" runat="server" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="Gainsboro" BorderStyle="Solid" BorderWidth="1px" Font-Size="10pt" ForeColor="Black"
                                                            OnRowDataBound="gvGoalPace_RowDataBound" OnSorting="gvGoalPace_Sorting" ShowFooter="True" Width="1450px">
                                                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                            <RowStyle BackColor="White" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="No">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblID" runat="server" Font-Size="9pt" Text='<%# Bind("ID") %>' Width="30px" Font-Bold="True"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Name">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="300px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblName" runat="server" Font-Bold="true" Style="cursor: pointer" Text='<%# Bind("Name") %>' ToolTip='<%# Bind("Customer") %>' Width="300px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblTotals" runat="server" Font-Bold="true" Text="Totals:"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>                                                                
                                                               
                                                                <asp:TemplateField HeaderText="Goal Pace Amount YTD">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="200px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGoalPaceAmountD" runat="server" Font-Bold="true" Text='<%# Bind("GoalPaceAmountD") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Goal Pace Amount Difference">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="200px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGoalPaceAmountDiffD" runat="server" Font-Bold="true" Text='<%# Bind("GoalPaceAmountDiffD") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--Old Stuff--%>
                                                                <%--Last Month Amount--%>
                                                                <asp:TemplateField HeaderText="LM" SortExpression="PreviousMonthAmount">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblPreviousMonthAmountHeader" runat="server" Font-Bold="true"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblPreviousMonthAmountD" runat="server" Text='<%# Bind("PreviousMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblPreviousMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--Last Year last Month--%>
                                                                <asp:TemplateField HeaderText="LYLM" SortExpression="LastYearPreviousMonthAmount">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lbLastYearPreviousMonthAmountHeader" runat="server" Font-Bold="true"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYearPreviousMonthAmountD" runat="server" Text='<%# Bind("LastYearPreviousMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYearPreviousMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--Current month Amount--%>                                                                
                                                                <asp:TemplateField HeaderText="CM" SortExpression="CurrentMonthAmount">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblCurrentMonthAmountHeader" runat="server" Font-Bold="true"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentMonthAmountD" runat="server" Text='<%# Bind("CurrentMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--Last Year Current Month Amount--%>
                                                                <asp:TemplateField HeaderText="LYCM" SortExpression="LastYearCurrentMonthAmount">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblLastYearCurrentMonthAmountHeader" runat="server" Font-Bold="true"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYearCurrentMonthAmountD" runat="server" Text='<%# Bind("LastYearCurrentMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYearCurrentMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>                                                                                                                        

                                                                <%--YTD--%>
                                                                <asp:TemplateField HeaderText="YTD">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblCurrentYTDAmountHeader" runat="server" Font-Bold="true"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle CssClass="CenterAligner" Width="200px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentYTDAmountD" runat="server" Font-Bold="true" Text='<%# Bind("CurrentYTDAmountD") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentYTDAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--Last Year YTD--%>
                                                                <asp:TemplateField HeaderText="LYTD" SortExpression="LastYTDAmount">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblLastYTDAmountHeader" runat="server" Font-Bold="true"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYTDAmountD" runat="server" Text='<%# Bind("LastYTDAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYTDAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--2 year ago YTD--%>
                                                                <asp:TemplateField HeaderText="LLYTD" SortExpression="LastYTDMinusOneAmount">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblLastYTDMinusOneAmountHeader" runat="server" Font-Bold="true"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYTDMinusOneAmountD" runat="server" Text='<%# Bind("LastYTDMinusOneAmountD") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYTDMinusOneAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField> 
                                                                <%--Current year % or total--%>
                                                                <asp:TemplateField HeaderText="YTD %" SortExpression="CurrentYTDAmountPercentageOfTotal">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblCurrentYTDAmountPercentageOfTotalHeader" runat="server" Font-Bold="true"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentYTDAmountPercentageOfTotal" runat="server" Text='<%# Bind("CurrentYTDAmountPercentageOfTotal") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentYTDAmountPercentageOfTotalSum" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--Last year % of total--%>
                                                                <asp:TemplateField HeaderText="LYTD %" SortExpression="LastYTDAmountPercentageOfTotal">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblLastYTDAmountPercentageOfTotalHeader" runat="server" Font-Bold="true"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYTDAmountPercentageOfTotal" runat="server" Text='<%# Bind("LastYTDAmountPercentageOfTotal") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYTDAmountPercentageOfTotalSum" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--Margin Current Month--%>
                                                                <asp:TemplateField HeaderText="Current Month Margin" SortExpression="CurrentMonthMargin">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblCurrentMonthMarginHeader" runat="server" Font-Bold="true"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentMonthMargin" runat="server" Text='<%# Bind("CurrentMonthMargin") %>' Width="100px"></asp:Label>
                                                                        <asp:Label ID="lblCurrentMonthCost" runat="server" Text='<%# Bind("CurrentMonthCost") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentMonthMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--Margin Last Month--%>
                                                                <asp:TemplateField HeaderText="Last Month Margin" SortExpression="LastMonthMargin">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblLastMonthMarginHeader" runat="server" Font-Bold="true"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastMonthMargin" runat="server" Text='<%# Bind("LastMonthMargin") %>' Width="100px"></asp:Label>
                                                                        <asp:Label ID="lblPreviousMonthCost" runat="server" Text='<%# Bind("PreviousMonthCost") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastMonthMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <%--Not Used--%>
                                                                <asp:TemplateField Visible="false">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCurrentYTDMargin" runat="server" Text='<%# Bind("CurrentYTDMargin") %>'></asp:Label>
                                                                        <asp:Label ID="lblCurrentYTDCost" runat="server" Text='<%# Bind("CurrentYTDCost") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblCurrentYTDMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField Visible="false">
                                                                    <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLastYTDMargin" runat="server" Text='<%# Bind("LastYTDMargin") %>'></asp:Label>
                                                                        <asp:Label ID="lblLastYTDCost" runat="server" Text='<%# Bind("LastYTDCost") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Right" />
                                                                    <FooterTemplate>
                                                                        <asp:Label ID="lblLastYTDAmountMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                    </FooterTemplate>
                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <FooterStyle BackColor="#CCCCCC" />
                                                            <HeaderStyle BackColor="Black" CssClass="CenterAligner" ForeColor="White" HorizontalAlign="Center" />
                                                            <AlternatingRowStyle HorizontalAlign="Center" />
                                                            <RowStyle CssClass="CenterAligner" ForeColor="Black" />
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">&nbsp;</td>
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
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table id="Table2" align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:LinkButton ID="btnExportSummary" runat="server" OnClick="btnExportSummary_Click" CssClass="btn btn-success" ForeColor="White" Width="300px"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL Top 15 REPORT</asp:LinkButton>
                                        </td>
                                        <td>&nbsp;</td>
                                        <td align="center">
                                            <asp:LinkButton ID="btnExportSummaryGoalPace" runat="server" OnClick="btnExportSummaryGoalPace_Click" CssClass="btn btn-success" ForeColor="White" Width="300px"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL GOAL PACE 250k+</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                    </table>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportSummary" />
            <asp:PostBackTrigger ControlID="btnExportSummaryGoalPace" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

