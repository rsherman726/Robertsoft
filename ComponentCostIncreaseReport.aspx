<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ComponentCostIncreaseReport.aspx.cs" Inherits="ComponentCostIncreaseReport" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#MainContent_gvReportCondensed tr').click(function (e) {
                $('#MainContent_gvReportCondensed tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });

        });

        function isPostBack() {//Use during postbacks...
            $('#MainContent_gvReportCondensed tr').click(function (e) {
                $('#MainContent_gvReportCondensed tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });



        }; //End Postback... 

    </script>
    <style type="text/css">
        tr.highlighted td {
            background: #ffd77d;
        }

        tbody tr.selected td {
            background: none repeat scroll 0 0 #FFCF8B;
            color: #f00;
        }

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
            <table align="center" width="1100">
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Component Cost Increase Report" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>

                        <table width="1100" class="JustRoundedEdgeBothSmall" style="background-color: lightblue;">
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td align="left">&nbsp;
                                                    <asp:Label ID="Label13" runat="server" Font-Bold="True" ForeColor="Black" Text="Stock Code" Font-Names="Arial"></asp:Label>&nbsp;
                                                    <asp:Label ID="lblDescription" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Navy" Font-Names="Arial"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top" align="left">

                                                <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server"
                                                    CompletionSetCount="25" CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                    DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes"
                                                    ServicePath="" TargetControlID="txtStockCode" UseContextKey="True" CompletionInterval="0">
                                                </ajaxToolkit:AutoCompleteExtender>
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <asp:TextBox ID="txtStockCode" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black"
                                                                OnTextChanged="txtStockCode_TextChanged" ValidationGroup="" Width="350px" placeholder="Enter a Component StockCode"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">&nbsp;</td>
                                                    </tr>

                                                </table>
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
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnPreview" runat="server" CssClass="btn btn-info" OnClick="btnPreview_Click" Text="Preview - Run" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblError" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Panel ID="pnlSummary" runat="server" Width="1100px">
                            <table style="background-color: WhiteSmoke;" width="100%">
                                <tr>
                                    <td align="center">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center" class="rowColors">
                                        <asp:Panel ID="pnlDetailsSummaryGrid" runat="server" Width="1135px" ScrollBars="Vertical" Height="650px">
                                            <asp:GridView ID="gvReportCondensed" runat="server" AutoGenerateColumns="False" Font-Size="10pt"
                                                GridLines="Vertical" Width="1100px"
                                                BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                BorderWidth="1px" ForeColor="Black" OnRowDataBound="gvReportCondensed_RowDataBound">
                                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Stock Code">
                                                        <HeaderStyle Width="149px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' ForeColor="black" Font-Size="8pt" Width="75px" Font-Bold="True"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item Description">
                                                        <HeaderStyle CssClass="CenterAligner" Width="220px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblItemDescription" runat="server" Text='<%# Bind("ItemDescription") %>' ForeColor="black" Font-Size="8pt" Width="220px" Font-Bold="True"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Recipe StockCode">
                                                        <HeaderStyle CssClass="CenterAligner" Width="75" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRecipeStockCode" runat="server" Text='<%# Bind("RecipeStockCode") %>' ForeColor="black" Font-Size="8pt" Width="75px" Font-Bold="True"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Recipe Uom">
                                                        <HeaderStyle CssClass="CenterAligner" Width="60px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRecipeUom" runat="server" Text='<%# Bind("RecipeUom") %>' ForeColor="black"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qty Per Recipe">
                                                        <HeaderStyle CssClass="CenterAligner" Width="88px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQtyPerRecipe" runat="server" Text='<%# Bind("QtyPerRecipe") %>' ForeColor="black"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Qty Per Case">
                                                        <HeaderStyle CssClass="CenterAligner" Width="88px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQtyPerCase" runat="server" Text='<%# Bind("QtyPerCase") %>' ForeColor="black"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Current Cost">
                                                        <HeaderStyle CssClass="CenterAligner" Width="133px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMaterialCost" runat="server" Text='<%# Bind("MaterialCost") %>' ForeColor="black" Width="133px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Estimated Ingredient Cost">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEstimatedIngredientCost" runat="server" Text='<%# Bind("EstimatedIngredientCost") %>' ForeColor="black" Width="82px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cost Increase Per Case">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCostIncreasePerCase" runat="server" Text='<%# Bind("CostIncreasePerCase") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" VerticalAlign="Bottom" HorizontalAlign="Center" />
                                                <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                <SortedDescendingHeaderStyle BackColor="#242121" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                       &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:LinkButton ID="lbnExportExcel" runat="server" CssClass="btn btn-success" OnClick="lbnExportExcel_Click" Width="200px" ForeColor="White" ToolTip="Click to Export to Excel"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>
                    </td>                    
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lbnExportExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

