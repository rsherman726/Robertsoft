<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UnitCostReport.aspx.cs" Inherits="UnitCostReport" MaintainScrollPositionOnPostback="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#MainContent_gvIngredients tr').click(function (e) {
                $('#MainContent_gvIngredients tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });
            $('#MainContent_gvComponents tr').click(function (e) {
                $('#MainContent_gvComponents tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });

        });

        function isPostBack() {//Use during postbacks...
            $('#MainContent_gvIngredients tr').click(function (e) {
                $('#MainContent_gvIngredients tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });
            $('#MainContent_gvComponents tr').click(function (e) {
                $('#MainContent_gvComponents tr').removeClass('highlighted');
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
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Unit Cost Report" ForeColor="Black"></asp:Label>
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
                                                            <asp:TextBox ID="txtStockCode" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtStockCode_TextChanged" ValidationGroup="" Width="350px"></asp:TextBox>
                                                        </td>
                                                        <td align="left">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="Black" Text="Period:"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="10pt" ForeColor="Black" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" Width="450px">
                                                                            <asp:ListItem>Range</asp:ListItem>
                                                                            <asp:ListItem>Last Month</asp:ListItem>
                                                                            <asp:ListItem>Last 30 Days</asp:ListItem>
                                                                            <asp:ListItem>Last 60 Days</asp:ListItem>
                                                                            <asp:ListItem>Last 3 Months</asp:ListItem>
                                                                            <asp:ListItem>Last 6 Months</asp:ListItem>
                                                                            <asp:ListItem>Last 9 Months</asp:ListItem>
                                                                            <asp:ListItem>Last 12 Months</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>&nbsp;</td>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left">
                                                            <table>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblStartDate" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="Black" Text="From:" Width="45px"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td style="width: 200px">
                                                                                    <asp:TextBox ID="txtStartDate" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup=""></asp:TextBox>
                                                                                    <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgStartDate" TargetControlID="txtStartDate" />
                                                                                    <ajaxToolkit:MaskedEditExtender ID="txtStartDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtStartDate" />
                                                                                </td>
                                                                                <td style="width: 10px">
                                                                                    <asp:ImageButton ID="imgStartDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="lblEndDate" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="Black" Text="To:"></asp:Label>
                                                                    </td>
                                                                    <td align="center">
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td style="width: 200px">
                                                                                    <asp:TextBox ID="txtEndDate" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup=""></asp:TextBox>
                                                                                    <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgEndDate" TargetControlID="txtEndDate" />
                                                                                    <ajaxToolkit:MaskedEditExtender ID="txtEndDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtEndDate" />
                                                                                </td>
                                                                                <td style="width: 10px">
                                                                                    <asp:ImageButton ID="imgEndDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td align="right">&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblNote" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="11pt" ForeColor="Red">Note: All time is used if no dates is entered</asp:Label>
                                </td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnPreview" runat="server" CssClass="btn btn-info" OnClick="btnPreview_Click" Text="Preview - Reload" />
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
                                    <td align="center">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black" Text="Case"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="pnlDetailsSummaryGrid" runat="server" Width="1100px">
                                            <asp:GridView ID="gvReportCondensed" runat="server" AutoGenerateColumns="False" Font-Size="10pt"
                                                GridLines="Vertical" Width="1100px"
                                                BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                BorderWidth="1px" ForeColor="Black" OnRowDataBound="gvReportCondensed_RowDataBound">
                                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Stock Code">
                                                        <HeaderStyle Width="149px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("FinishedStockCode") %>' Font-Size="8pt" Width="75px" Font-Bold="True"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Component Description">
                                                        <HeaderStyle CssClass="CenterAligner" Width="354px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblParentDescription" runat="server" Text='<%# Bind("ParentDescription") %>' Font-Size="8pt" Width="180px" Font-Bold="True"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Uom">
                                                        <HeaderStyle CssClass="CenterAligner" Width="60px" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStockUom" runat="server" Text='<%# Bind("StockUom") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="QtyPer">
                                                        <HeaderStyle CssClass="CenterAligner" Width="88px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQtyPer" runat="server" Text='<%# Bind("QtyPer") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bom<br/>Recipe<br/>Cost">
                                                        <HeaderStyle CssClass="CenterAligner" Width="88px" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblActualUnitCost" runat="server" Text='<%# Bind("ActualCost") %>' ForeColor="Red"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Bom<br/>Packaging<br/>Cost">
                                                        <HeaderStyle CssClass="CenterAligner" Width="133px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPackagingCost" runat="server" Text='<%# Bind("PackagingCost") %>' ForeColor="Red" Width="133px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Actual<br/>Labor<br/>Cost">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalLaborCost" runat="server" Text='<%# Bind("TotalLaborCost") %>' ForeColor="Red" Width="82px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<br/>Total">
                                                        <HeaderStyle CssClass="CenterAligner" Width="149px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotal" runat="server" ForeColor="Red" Width="149px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="What If<br/>Recipe<br/>Cost" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWhatIfUnitCost" runat="server" Text='<%# Bind("WhatIfCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="What If<br/>Packaging<br/>Cost" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWhatIfPkgCost" runat="server" Text='<%# Bind("WhatIfPkgCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="What If<br/>Labor<br/>Cost" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWhatIfLaborCost" runat="server" Text='<%# Bind("WhatIfLabCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="What If<br/>Total" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWhatIfTotal" runat="server"></asp:Label>
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
                                            <asp:GridView ID="gvEstimatedCost" runat="server" AutoGenerateColumns="False" Font-Size="10pt"
                                                GridLines="Vertical" Width="1100px"
                                                BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                BorderWidth="1px" ForeColor="Black" OnRowDataBound="gvEstimatedCost_RowDataBound">
                                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <HeaderStyle Width="149px" BackColor="White" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label21" runat="server" Width="75px">&nbsp;</asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle Width="354px" BackColor="White" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label22" runat="server" Width="180px">&nbsp;</asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle Width="60px" BackColor="White" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label23" runat="server" Width="60px">&nbsp;</asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle Width="88px" BackColor="White" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="Label24" runat="server" Width="88px">&nbsp;</asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Est.Rec.Cost">
                                                        <HeaderStyle CssClass="CenterAligner" Width="88px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEstimatedRecipeCost" runat="server" Text='<%# Bind("EstimatedRecipeCost") %>' ForeColor="Red"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Est.Pck.Cost">
                                                        <HeaderStyle CssClass="CenterAligner" Width="133px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblEstimatedPackagingCost" runat="server" Text='<%# Bind("EstimatedPackagingCost") %>' ForeColor="Red" Width="133px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Est.Lab.Cost">
                                                        <HeaderStyle CssClass="CenterAligner" Width="82px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLaborCost" runat="server" Text='<%# Bind("LaborCost") %>' ForeColor="Red" Width="82px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total">
                                                        <HeaderStyle CssClass="CenterAligner" Width="149px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotal" runat="server" Text='<%# Bind("Total") %>' ForeColor="Red" Width="149px"></asp:Label>
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
                    <td>
                        <table width="1100px">
                            <tr style="background-color: #333333; color: white">
                                <td align="center" style="width: 183.3px">
                                    <asp:Label ID="LabelPrice" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="White" Text="Price"></asp:Label>
                                </td>
                                <td align="center" style="width: 183.3px">
                                    <asp:Label ID="LabelShrinkage" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="White" Text="Total With Shrinkage"></asp:Label>
                                </td>
                                <td align="center" style="width: 183.3px">
                                    <asp:Label ID="LabelShrinkagePercentage" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="White" Text="Shrinkage %"></asp:Label>
                                </td>
                                <td align="center" style="width: 183.3px">
                                    <asp:Label ID="LabelMargin" runat="server" Text="Margin %" ForeColor="White" Font-Names="Arial" Font-Size="10pt" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="center" style="width: 183.3px">
                                    <asp:Label ID="LabelMarkup" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="White" Text="Markup %"></asp:Label>
                                </td>
                                <td align="center" style="width: 183.3px"></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtPrice" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black" Style="text-align: right"></asp:TextBox>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblShrinkage" runat="server" ForeColor="Black" Style="text-align: right"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtShrinkagePercentage" runat="server" CssClass="form-control input-sm" BackColor="LemonChiffon" ForeColor="Black" Style="text-align: right">2</asp:TextBox>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblMargin" runat="server" Font-Size="11pt" ForeColor="Black"></asp:Label>
                                </td>
                                <td align="center">
                                    <asp:Label ID="lblMarkup" runat="server" Font-Size="11pt" ForeColor="Black"></asp:Label>
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-info" Text="Submit" OnClick="btnSubmit_Click" />
                                </td>
                            </tr>

                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="background-color: pink">

                        <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black" Text="Cost Report"></asp:Label>

                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="gvReportWipCost" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" Font-Size="10pt" ForeColor="Black" GridLines="Vertical" OnRowDataBound="gvReportCost_RowDataBound" ShowFooter="false" Width="1100px">
                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                            <Columns>
                                <%-- <asp:TemplateField HeaderText="Stock Code">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStockCode" runat="server" ForeColor="Navy" Text='<%# Bind("ParentPart") %>' Width="185px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <HeaderStyle CssClass="CenterAligner" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" runat="server" Font-Size="8pt" Text='<%# Bind("ParentDescription") %>' Width="225px"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date">
                                    <HeaderStyle CssClass="CenterAligner" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("ParentCompleteDate") %>' Width="75px"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Parent&lt;br/&gt;Job">
                                    <HeaderStyle CssClass="CenterAligner" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblParentJob" runat="server" Text='<%# Bind("ParentJob") %>' Width="70px"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Uom">
                                    <HeaderStyle CssClass="CenterAligner" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblUom" runat="server" Text='<%# Bind("ParentUom") %>' Width="40px"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Qty">
                                    <HeaderStyle CssClass="CenterAligner" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblQty" runat="server" Text='<%# Bind("ParentQtyManuf") %>' Width="70px"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Recipe&lt;br/&gt;Cost">
                                    <HeaderStyle CssClass="CenterAligner" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecipeCost" runat="server" Text='<%# Bind("RecipeCost") %>' Width="80px"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pkg&lt;br/&gt;Cost">
                                    <HeaderStyle CssClass="CenterAligner" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblPkgCost" runat="server" Text='<%# Bind("PkgCost") %>' Width="85px"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Labor&lt;br/&gt;OH">
                                    <HeaderStyle CssClass="CenterAligner" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblLaborCost" runat="server" Text='<%# Bind("LaborOH") %>' Width="85px"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Total">
                                    <HeaderStyle CssClass="CenterAligner" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Font-Bold="true" Text='<%# Bind("Total") %>' Width="85px"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" />
                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#242121" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr class="rowColors">
                    <td>

                        <asp:Panel ID="pnlComponents" runat="server" Width="1100px">
                            <table style="background-color: WhiteSmoke;" width="100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black" Text="Packaging"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="pnlComponentsGrid" runat="server" Width="1100px">
                                            <asp:GridView ID="gvComponents" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                BorderWidth="1px" Font-Size="10pt" ForeColor="Black" GridLines="Vertical"
                                                OnRowDataBound="gvComponents_RowDataBound" Width="1100px" ShowFooter="True">
                                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Stock Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("MStockCode") %>' Font-Size="8pt" Width="75px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotal" runat="server" Text="TOTALS"></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Component Description">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblComponentDescription" runat="server" Text='<%# Bind("ComponentDesc") %>' Font-Size="8pt" Width="250px"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Uom">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStockUom" runat="server" Text='<%# Bind("StockUom") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="QtyPer">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQtyPer" runat="server" Text='<%# Bind("QtyPer") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cost<br/>Per<br/>Uom" Visible="False">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCostPerUom" runat="server" Text='<%# Bind("CostPerUom") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblCostPerUomTotal" runat="server" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Landed<br/>Packaging<br/>Cost">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMaterialCost" runat="server" Text='<%# Bind("MaterialCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total<br/>Packaging<br/>Cost">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblActualUnitCost" runat="server" Text='<%# Bind("ActualCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblActualUnitCostTotal" runat="server" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="What If<br/>Packaging<br>Cost" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemStyle HorizontalAlign="center" />
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkOverwrite" runat="server" ToolTip="Overwrite Database value when checked (Check first!)" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtPackagingCost" runat="server" Text='<%# Bind("EstimatedPackagingCost") %>' CssClass="form-control input-sm"
                                                                            AutoPostBack="True" BackColor="LemonChiffon" Style="text-align: right" OnTextChanged="txtPackagingCost_TextChanged"
                                                                            Width="95" ForeColor="Black"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Landed What If<br/>Packagikng<br/> Cost">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLandedWhatIfCost" runat="server" Text='<%# Bind("LandedWhatIfCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                          <FooterTemplate>
                                                           &nbsp;
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total<br/>What If<br/>Cost">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNewCost" runat="server" Text='<%# Bind("TotalNewCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblNewCostTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="What If<br/>Packaging<br/>Cost" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWhatIfUnitCost" runat="server" Text='<%# Bind("WhatIfCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblWhatIfUnitCostTotal" runat="server" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="What If<br/>Labor<br/>Cost" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWhatIfLaborCost" runat="server" Text='<%# Bind("WhatIfLabCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblWhatIfLaborCostTotal" runat="server" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="What If<br/>Mat<br/>Cost" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWhatIfMatCost" runat="server" Text='<%# Bind("WhatIfMatCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblWhatIfMatCostTotal" runat="server" ForeColor="white" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Labor<br/>Cost<br/>Mat" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLaborCostMat" runat="server" Text='<%# Bind("LabCostMat") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblLaborCostMatTotal" runat="server" ForeColor="white" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Actual Pkg<br/>Labor<br/>Cost">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLaborCostPkg" runat="server" Text='<%# Bind("LabCostPkg") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblLaborCostPkgTotal" runat="server" ForeColor="white" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Total<br/>Labor<br/>Cost" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalLaborCost" runat="server" Text='<%# Bind("TotalLaborCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotalLaborCostTotal" runat="server" ForeColor="white" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
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
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr class="rowColors">
                    <td>

                        <asp:Panel ID="pnlIngredients" runat="server" Width="1100px">
                            <table style="background-color: WhiteSmoke;" width="100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label14" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Black" Text="Recipe"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="pnlDetailsIngredientsGrid" runat="server" Width="1100px">
                                            <asp:GridView ID="gvIngredients" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                BorderWidth="1px" Font-Size="10pt" ForeColor="Black" GridLines="Vertical"
                                                OnRowDataBound="gvIngredients_RowDataBound" ShowFooter="True" Width="1100px">
                                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Stock Code">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("MStockCode") %>' Font-Size="8pt" Width="75px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotal" runat="server" Text="TOTALS"></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Component Description">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblComponentDescription" runat="server" Text='<%# Bind("ComponentDesc") %>' Font-Size="8pt" Width="250px"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Uom">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStockUom" runat="server" Text='<%# Bind("StockUom") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Right" />
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="QtyPer">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQtyPer" runat="server" Text='<%# Bind("QtyPer") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Cost<br/>Per<br/>Uom" Visible="False">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCostPerUom" runat="server" Text='<%# Bind("CostPerUom") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblCostPerUomTotal" runat="server" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Actual<br/>Ingredient<br/>Cost">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMaterialCost" runat="server" Text='<%# Bind("MaterialCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Actual<br/>Uom<br/>Cost">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblActualUnitCost" runat="server" Text='<%# Bind("ActualCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblActualUnitCostTotal" runat="server" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="What If<br/>Ingredient<br>Cost" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemStyle HorizontalAlign="center" />
                                                        <ItemTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:CheckBox ID="chkOverwrite" runat="server" ToolTip="Overwrite Database value when checked (Check first!)" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtMaterialCost" runat="server" Text='<%# Bind("EstimatedIngredientCost") %>' CssClass="form-control input-sm"
                                                                            AutoPostBack="True" BackColor="LemonChiffon" Style="text-align: right" OnTextChanged="txtMaterialCost_TextChanged"
                                                                            Width="95" ForeColor="Black"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            &nbsp;
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />                                                        
                                                         <FooterTemplate>
                                                            
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Landed What If<br/>Ingredient <br/>Cost">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                           <asp:Label ID="lblLandedWhatIfCost" runat="server" Text='<%# Bind("LandedWhatIfCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />                                                        
                                                         
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Total<br/>What If<br/>Cost">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNewCost" runat="server" Text='<%# Bind("TotalNewCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblNewCostTotal" runat="server"></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="What If<br/>Uom<br/>Cost" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWhatIfUnitCost" runat="server" Text='<%# Bind("WhatIfCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblWhatIfUnitCostTotal" runat="server" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="What If<br/>Labor<br/>Cost" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWhatIfLaborCost" runat="server" Text='<%# Bind("WhatIfLabCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblWhatIfLaborCostTotal" runat="server" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="What If<br/>Ingredient<br/>Cost" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblWhatIfMatCost" runat="server" Text='<%# Bind("WhatIfMatCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />

                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Actual<br/>Recipe<br/>Labor Cost">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLaborCostMat" runat="server" Text='<%# Bind("LabCostMat") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblLaborCostMatTotal" runat="server" ForeColor="white" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Labor<br/>Cost<br/>Pkg" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLaborCostPkg" runat="server" Text='<%# Bind("LabCostPkg") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblLaborCostPkgTotal" runat="server" ForeColor="white" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Total<br/>Labor<br/>Cost" Visible="false">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTotalLaborCost" runat="server" Text='<%# Bind("TotalLaborCost") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <FooterTemplate>
                                                            <asp:Label ID="lblTotalLaborCostTotal" runat="server" ForeColor="white" Text=""></asp:Label>
                                                        </FooterTemplate>
                                                        <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" VerticalAlign="Top" />
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

                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

