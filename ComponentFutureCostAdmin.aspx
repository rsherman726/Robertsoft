<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ComponentFutureCostAdmin.aspx.cs" Inherits="ComponentFutureCostAdmin" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#MainContent_gvIngredients tr').click(function (e) {
                $('#MainContent_gvIngredients tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });
            $('#MainContent_gvPackaging tr').click(function (e) {
                $('#MainContent_gvPackaging tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });

        });

        function isPostBack() {//Use during postbacks...
            $('#MainContent_gvIngredients tr').click(function (e) {
                $('#MainContent_gvIngredients tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });
            $('#MainContent_gvPackaging tr').click(function (e) {
                $('#MainContent_gvPackaging tr').removeClass('highlighted');
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
                        <table style="border: medium solid #000080; width: 100%; background-color: white;">
                            <tr>
                                <td>
                                    <img src="Images/Loader.gif" alt="" />
                                </td>
                                <td><span style="color: #ffffff"><span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing  <span class="">....</span> </strong></span></span></td>
                            </tr>
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
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Component Future Cost Admin" ForeColor="Black"></asp:Label>
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
                                                    ServicePath="" TargetControlID="txtSearch" UseContextKey="True" CompletionInterval="0">
                                                </ajaxToolkit:AutoCompleteExtender>
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left" style="width: 375px">
                                                            <asp:TextBox ID="txtSearch" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="350px"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 75px">
                                                            <asp:LinkButton ID="lbnSearch" runat="server" CssClass="btn btn-success btn-lg" Font-Bold="True" OnClick="lbnSearch_Click" ToolTip="Search with Autofill"><i class="fa fa-search"></i></asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="lbnReset" runat="server" CssClass="btn btn-danger btn-lg" Font-Bold="True" OnClick="lbnReset_Click" ToolTip="Reload Default Search...Could take up to 25 seconds">LOAD ALL&nbsp;<i class="fa fa-refresh" ></i></asp:LinkButton>
                                                        </td>
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
                    <td align="center">
                        <asp:Label ID="lblError" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <table style="background-color: WhiteSmoke;" width="100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black" Text="Packaging"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table width="500px">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtStockCodePackaging" runat="server" CssClass="form-control input-sm"
                                                    BackColor="LemonChiffon" placeholder="StockCode"
                                                    ForeColor="Black" Width="250px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPackagingCost" runat="server" CssClass="form-control input-sm"
                                                    BackColor="LemonChiffon" Style="text-align: right" placeholder="Packaging Cost" onkeypress="return OnlyNumberAftertwoDigits(event,this.id);"
                                                    ForeColor="Black" Width="250px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lbnAddPackaging" runat="server" CssClass="btn btn-info btn-sm" OnClick="lbnAddPackaging_Click" ToolTip="Click to Add Packaging">Add</asp:LinkButton>
                                                <ajaxToolkit:ConfirmButtonExtender ID="lbnAddPackaging_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="lbnAddPackaging"></ajaxToolkit:ConfirmButtonExtender>

                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="pnlPackagingGrid" runat="server">
                                        <table align="center">
                                            <tr class="rowColors">
                                                <td align="center">
                                                    <asp:Panel ID="pnlGridPackaging" runat="server" Width="1125px">
                                                        <asp:GridView ID="gvPackaging" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                            BorderWidth="1px" Font-Size="10pt" ForeColor="Black" GridLines="Vertical"
                                                            OnRowDataBound="gvPackaging_RowDataBound" Width="1100px" OnRowCommand="gvPackaging_RowCommand" OnRowDeleting="gvPackaging_RowDeleting">
                                                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Stock Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("MStockCode") %>' Font-Size="10pt" Width="75px"></asp:Label>
                                                                        <asp:Label ID="lblEstimatedPackagingCostID" runat="server" Text='<%# Bind("EstimatedPackagingCostID") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Component Description">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblComponentDescription" runat="server" Text='<%# Bind("ComponentDesc") %>' Font-Size="10pt"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Uom">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStockUom" runat="server" Text='<%# Bind("StockUom") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Landed Cost">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblActualCost" runat="server" Text='<%# Bind("ActualCost") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="What If Cost">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemStyle HorizontalAlign="center" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtPackagingCost" runat="server" Text='<%# Bind("EstimatedPackagingCost") %>' CssClass="form-control input-sm"
                                                                            AutoPostBack="True" BackColor="LemonChiffon" Style="text-align: right" OnTextChanged="txtPackagingCost_TextChanged"
                                                                            ForeColor="Black"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Burden Cost %">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBurdenCost" runat="server" Text='<%# Bind("BurdenPercentage") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Landed What If Cost">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLandedWhatIfCost" runat="server" Text='<%# Bind("LandedWhatIfCost") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ShowHeader="False">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lbnDelete" runat="server" CausesValidation="False" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delete" Font-Bold="True" Font-Size="11pt" Text="Delete" ForeColor="Blue"></asp:LinkButton>
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with delete?" Enabled="True" TargetControlID="lbnDelete"></ajaxToolkit:ConfirmButtonExtender>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                            <SortedDescendingHeaderStyle BackColor="#242121" />
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:LinkButton ID="btnExportPackaging" runat="server" CssClass="btn btn-success" ForeColor="White" OnClick="btnExportPackaging_Click" Width="275px"><i class="glyphicon glyphicon-export"></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>

                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="Label14" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black" Text="Recipe"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="background-color: WhiteSmoke;" width="100%">

                            <tr>
                                <td align="center">
                                    <table width="500px">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtStockCodeIngredient" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black" placeholder="StockCode" Width="250px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtIngredientCost" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black" onkeypress="return OnlyNumberAftertwoDigits(event,this.id);" placeholder="Ingredient Cost" Style="text-align: right" Width="250px"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lbnAddIngredient" runat="server" CssClass="btn btn-info btn-sm" OnClick="lbnAddIngredient_Click" ToolTip="Click to Add Ingredient">Add</asp:LinkButton>
                                                <ajaxToolkit:ConfirmButtonExtender ID="lbnAddIngredient_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="lbnAddIngredient" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="pnlDetailsIngredientsGrid" runat="server">
                                        <table align="center">
                                            <tr class="rowColors">
                                                <td align="center">
                                                    <asp:Panel ID="pnlGridIngredients" runat="server" Width="1125px">
                                                        <asp:GridView ID="gvIngredients" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Font-Size="10pt" ForeColor="Black" GridLines="Vertical" OnRowDataBound="gvIngredients_RowDataBound" Width="1100px" OnRowCommand="gvIngredients_RowCommand" OnRowDeleting="gvIngredients_RowDeleting">
                                                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Stock Code">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStockCode" runat="server" Font-Size="10pt" Text='<%# Bind("MStockCode") %>' Width="75px"></asp:Label>
                                                                        <asp:Label ID="lblEstimatedIngredientCostID" runat="server" Text='<%# Bind("EstimatedIngredientCostID") %>' Visible="false"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Component Description">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblComponentDescription" runat="server" Font-Size="10pt" Text='<%# Bind("ComponentDesc") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Uom">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStockUom" runat="server" Text='<%# Bind("StockUom") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Landed Cost">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblActualCost" runat="server" Text='<%# Bind("ActualCost") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="What If Cost">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemStyle HorizontalAlign="center" />
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtMaterialCost" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control input-sm" 
                                                                            ForeColor="Black" OnTextChanged="txtMaterialCost_TextChanged" Style="text-align: right" Text='<%# Bind("EstimatedIngredientCost") %>'></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Burden Cost %">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblBurdenCost" runat="server" Text='<%# Bind("BurdenPercentage") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Landed What If Cost">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblLandedWhatIfCost" runat="server" Text='<%# Bind("LandedWhatIfCost") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ShowHeader="False">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lbnDelete" runat="server" CausesValidation="False" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delete" Font-Bold="True" Font-Size="11pt" ForeColor="Blue" Text="Delete"></asp:LinkButton>
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with delete?" Enabled="True" TargetControlID="lbnDelete" />
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" />
                                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                            <SortedDescendingHeaderStyle BackColor="#242121" />
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:LinkButton ID="btnExportIngredients" runat="server" CssClass="btn btn-success" ForeColor="White" OnClick="btnExportIngredients_Click" Width="275px"><i class="glyphicon glyphicon-export"></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>
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
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportPackaging" />
            <asp:PostBackTrigger ControlID="btnExportIngredients" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

