<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PriceMarginHistoryFull.aspx.cs" Inherits="PriceMarginHistoryFull" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
      <style type="text/css">
                .rowColors tr:hover {
            background-color: #05b3f5 !important;
        }

        .rowColors tr:hover {
            color: #FFF !important;
            transform-st !important;
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
            <table align="center" width="900">
                <tr>
                    <td align="center">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Price Margin History  Report" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <table align="center" width="900">
                            <tr>
                                <td>
                                    <asp:Panel ID="PnlContent" runat="server">
                                        <table>
                                            <tr>
                                                <td align="center" class="JustRoundedEdgeBothSmall" style="background-color: lightgreen; width: 1100px">
                                                    <asp:Panel ID="pnlSearchCriteriaChart1" runat="server">
                                                        <table width="1000">
                                                            <tr>
                                                                <td align="center">
                                                                    <table>
                                                                        <tr>
                                                                            <td align="center">

                                                                                &nbsp;</td>
                                                                            <td align="center" style="width: 450px">
                                                                                <asp:Label ID="Label53" runat="server" Font-Bold="True" ForeColor="Black" Text="Finished Stock Code"></asp:Label>
                                                                                &nbsp;<asp:Label ID="lblStockCodeList" runat="server" Font-Bold="True" ForeColor="Black" Text="Stock Code(s)"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center">
                                                                                <asp:RadioButtonList ID="rblIngredientComponent" runat="server" ForeColor="Black"  RepeatDirection="Horizontal" Width="410px">
                                                                                    <asp:ListItem Selected="True">&nbsp;Ingredient Stock Code</asp:ListItem>
                                                                                    <asp:ListItem>&nbsp;Component Stock Code</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                            <td align="center" style="width: 450px">
                                                                                <asp:RadioButtonList ID="rblSourceOfStockCodes" runat="server" AutoPostBack="True" ForeColor="Black" OnSelectedIndexChanged="rblSourceOfStockCodes_SelectedIndexChanged" RepeatDirection="Horizontal" Width="250px">
                                                                                    <asp:ListItem Selected="True">&nbsp;WipMaster</asp:ListItem>
                                                                                    <asp:ListItem>&nbsp;InvMaster</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">&nbsp;</td>
                                                                            <td align="left" style="width: 450px">&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="cursor: pointer" align="center">
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblStockCodeDescIngredientComponent" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Navy"></asp:Label>
                                                                                            <br />

                                                                                        </td>
                                                                                        <td>&nbsp;</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtStockCodeChartIngredientComponent" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtStockCodeChartIngredient_TextChanged" ValidationGroup="" Width="300px"></asp:TextBox>
                                                                                           
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Font-Bold="True" OnClick="btnSearch_Click" Text="Search" /></td>
                                                                                    </tr>
                                                                                </table>

                                                                            </td>
                                                                            <td style="cursor: pointer" align="left">
                                                                                <asp:ListBox ID="lbParentStockCode" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="9pt" ForeColor="Black" Height="120px" Style="width: 500px !important" SelectionMode="Multiple"></asp:ListBox>
                                                                                <ajaxToolkit:ListSearchExtender ID="lbParentStockCode_ListSearchExtender" runat="server"
                                                                                    Enabled="True" IsSorted="True" TargetControlID="lbParentStockCode" PromptCssClass="Prompt">
                                                                                </ajaxToolkit:ListSearchExtender>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" style="cursor: pointer">&nbsp;</td>
                                                                            <td align="center" style="cursor: pointer">
                                                                                <asp:Label ID="Label65" runat="server" Font-Bold="False" Font-Size="9pt" ForeColor="Navy" Text="(Use CTRL for Multiple Selection) "></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" style="cursor: pointer">&nbsp;</td>
                                                                            <td align="center" style="cursor: pointer">
                                                                                <asp:LinkButton ID="lbSelectAll" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbnSelectAllStockCode_Click" ValidationGroup="nothing">Select All</asp:LinkButton>
                                                                                &nbsp;<asp:LinkButton ID="lbClearAll" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbnClearStockCode_Click" ValidationGroup="nothing">Clear All</asp:LinkButton>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" style="cursor: pointer">&nbsp;</td>
                                                                            <td align="center" style="cursor: pointer">
                                                                                <asp:Button ID="btnLoadAll" runat="server" CssClass="btn btn-default" Font-Bold="True" OnClick="btnLoadAll_Click" Text="Load All Finished Stock Codes" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top" align="left">&nbsp;</td>
                                                                            <td valign="top" align="left">
                                                                                <asp:Label ID="lblError" runat="server" EnableViewState="False" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:LinkButton ID="btnPreviewChart1" runat="server" OnClick="btnPreviewChart1_Click" CssClass="btn btn-info"  Width="200px" ForeColor="White" ToolTip="Click to run"><i class="fa fa-file-text-o" ></i>&nbsp;Run Report</asp:LinkButton> 
 
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label ID="Label49" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Black" Text="Price-Margin History"></asp:Label>
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
                                                <td align="center">

                                                    <tr id="trFullReport" runat="server">
                                                        <td align="center">
                                                            <asp:Label ID="lblPriceTotal" runat="server" Font-Names="Arial" Font-Size="14pt" ForeColor="Navy" Font-Bold="True"></asp:Label>&nbsp;
                                <asp:Label ID="lblMarginWeightedAvg" runat="server" Font-Names="Arial" Font-Size="14pt" ForeColor="Navy" Font-Bold="True"></asp:Label>
                                                            <table align="left">
                                                                <tr>
                                                                    <td align="left">
                                                                        <!-- *** Begin Header Table *** -->
                                                                        <div id="divHeader" runat="server" style="vertical-align: top; text-align: left;">
                                                                            <asp:Table ID="tblHeaderTable" runat="server"
                                                                                CellPadding="2"
                                                                                CellSpacing="0"
                                                                                Font-Size="11pt"
                                                                                ForeColor="White"
                                                                                BackColor="#333333"
                                                                                Font-Bold="False"
                                                                                Visible="false">
                                                                            </asp:Table>
                                                                        </div>
                                                                        <!-- *** End Header Table *** -->
                                                                    </td>
                                                                </tr>
                                                                 <tr class="rowColors">
                                                                    <td align="left">
                                                                        <asp:Panel ID="pnlGridView" runat="server" Height="300px" ScrollBars="Vertical" Visible="False">
                                                                            <div id="DivData" class="Container" style="vertical-align: top; height: 300px; width: 100%;">
                                                                                <asp:GridView ID="gvReportCondensed" runat="server" AutoGenerateColumns="False" Font-Size="10pt"
                                                                                    GridLines="Vertical" ShowFooter="True"
                                                                                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                                                    BorderWidth="1px" ForeColor="Black" OnRowDataBound="gvReportCondensed_RowDataBound"
                                                                                    Font-Names="Arial" AllowSorting="false" OnSorting="gvReportCondensed_Sorting" ShowHeader="False">
                                                                                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Stock<br/>Code" SortExpression="StockCode">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Width="90px"   ForeColor="Navy" Font-Bold="True" Font-Size="8"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>' Width="150px" Font-Size="8pt"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="center" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText=" Margin<br/>YTD" SortExpression="MarginCurrentYTD">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblMarginYTD" runat="server" Text='<%# Bind("MarginCurrentYTD") %>' Width="85px"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="right" />
                                                                                           <%-- <FooterTemplate>
                                                                                                <asp:Label ID="lblMarginYTDTotal" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                            <FooterStyle HorizontalAlign="right" />--%>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Year - 1">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblMarginYearMinus1" runat="server" Text='<%# Bind("MarginYearMinus1") %>' Width="85px"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="right" />
                                                                                            <%--<FooterTemplate>
                                                                                                <asp:Label ID="lblMarginYearMinus1Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                            <FooterStyle HorizontalAlign="right" />--%>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Year - 2">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblMarginYearMinus2" runat="server" Text='<%# Bind("MarginYearMinus2") %>' Width="85px"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="right" />
                                                                                           <%-- <FooterTemplate>
                                                                                                <asp:Label ID="lblMarginYearMinus2Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                            <FooterStyle HorizontalAlign="right" />--%>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Year - 3">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblMarginYearMinus3" runat="server" Text='<%# Bind("MarginYearMinus3") %>' Width="85px"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="right" />
                                                                                            <%--<FooterTemplate>
                                                                                                <asp:Label ID="lblMarginYearMinus3Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                            <FooterStyle HorizontalAlign="right" />--%>
                                                                                        </asp:TemplateField>

                                                                                        <asp:TemplateField HeaderText="Amount<br/>YTD" SortExpression="AmountCurrentYTD">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAmountYTD" runat="server" Text='<%# Bind("AmountCurrentYTD") %>' Width="85px"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="right" />
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblAmountYTDTotal" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                            <FooterStyle HorizontalAlign="right" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Year - 1">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAmountYearMinus1" runat="server" Text='<%# Bind("AmountYearMinus1") %>' Width="85px"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="right" />
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblAmountYearMinus1Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                            <FooterStyle HorizontalAlign="right" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Year - 2">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAmountYearMinus2" runat="server" Text='<%# Bind("AmountYearMinus2") %>' Width="85px"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="right" />
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblAmountYearMinus2Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                            <FooterStyle HorizontalAlign="right" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Year - 3">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblAmountYearMinus3" runat="server" Text='<%# Bind("AmountYearMinus3") %>' Width="85px"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="right" />
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblAmountYearMinus3Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                            <FooterStyle HorizontalAlign="right" />
                                                                                        </asp:TemplateField>

                                                                                        <asp:TemplateField HeaderText="Price<br/>YTD" SortExpression="PriceCurrentYTD">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPriceYTD" runat="server" Text='<%# Bind("PriceCurrentYTD") %>' Width="85px"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="right" />
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblPriceYTDTotal" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                            <FooterStyle HorizontalAlign="right" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Year - 1">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPriceYearMinus1" runat="server" Text='<%# Bind("PriceYearMinus1") %>' Width="85px"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="right" />
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblPriceYearMinus1Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                            <FooterStyle HorizontalAlign="right" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Year - 2">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPriceYearMinus2" runat="server" Text='<%# Bind("PriceYearMinus2") %>' Width="85px"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="right" />
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblPriceYearMinus2Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                            <FooterStyle HorizontalAlign="right" />
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Year - 3">
                                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblPriceYearMinus3" runat="server" Text='<%# Bind("PriceYearMinus3") %>' Width="85px"></asp:Label>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="right" />
                                                                                            <FooterTemplate>
                                                                                                <asp:Label ID="lblPriceYearMinus3Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                                            </FooterTemplate>
                                                                                            <FooterStyle HorizontalAlign="right" />
                                                                                        </asp:TemplateField>


                                                                                    </Columns>
                                                                                    <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" VerticalAlign="Bottom" HorizontalAlign="Center" />
                                                                                    <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                                                                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                                    <SortedDescendingHeaderStyle BackColor="#242121" />
                                                                                    <FooterStyle BackColor="Wheat" />
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>


                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <%--        <Triggers>
            <asp:PostBackTrigger ControlID="imgExportExcel1" />
            <asp:PostBackTrigger ControlID="imgExportExcel2" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
