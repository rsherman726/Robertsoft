<%@ Page Title="Price analysis" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PriceAnalysis.aspx.cs" Inherits="PriceAnalysis" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .rowColors tr:hover {
            background-color: #05b3f5 !important;
            transition-property: background;
            transition-duration: 100ms;
            transition-delay: 5ms;
            transition-timing-function: linear;
            transform-st !important;
        }

        tr.highlighted td {
            background: #05b3f5;
        }

        tbody tr.selected td {
            background: none repeat scroll 0 0 #FFCF8B;
            color: #f00;
        }
    </style>
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
            <div style="text-align: left">
                <table style="float: left !important" width="1220">
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Price Analysis Report" ForeColor="Black"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="900" class="JustRoundedEdgeBothSmall" style="background-color: lightblue;" align="center">
                                <tr>
                                    <td style="width: 30px">&nbsp;
                                    </td>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td align="center" style="width: 400px">
                                                    &nbsp;</td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblStockCodeDescIngredientComponent" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Navy"></asp:Label>
                                                    <br />
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtStockCodeChartIngredientComponent" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtStockCodeChartIngredient_TextChanged" ValidationGroup=""></asp:TextBox>

                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Font-Bold="True" OnClick="btnSearch_Click" Text="Search" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td align="left">&nbsp;</td>
                                    <td align="left">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblStockCodeList" runat="server" ForeColor="Black" Text="Stock Code(s)" Font-Bold="True"></asp:Label></td>
                                                <td>
                                                    <asp:RadioButtonList ID="rblSourceOfStockCodes" runat="server" ForeColor="Black" RepeatDirection="Horizontal" AutoPostBack="True" OnSelectedIndexChanged="rblSourceOfStockCodes_SelectedIndexChanged" Width="250px">
                                                        <asp:ListItem Selected="True">&nbsp;WipMaster</asp:ListItem>
                                                        <asp:ListItem>&nbsp;InvMaster</asp:ListItem>
                                                    </asp:RadioButtonList></td>
                                            </tr>
                                        </table>



                                    </td>
                                    <td>
                                        <asp:Label ID="Label12" runat="server" Text="Date(s)" CssClass="auto-style1" ForeColor="Black"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">&nbsp;</td>
                                    <td align="center">
                                        <asp:Button ID="btnLoadAll" runat="server" CssClass="btn btn-default" Font-Bold="True" OnClick="btnLoadAll_Click" Text="Load All Finished Stock Code" />
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left">&nbsp;</td>
                                    <td align="left">&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">&nbsp;</td>
                                    <td align="center" valign="top">
                                        <asp:ListBox ID="lbParentStockCode" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="9pt" Height="200px" SelectionMode="Multiple" Style="width: 500px !important" ToolTip="Start typing a stock code to search" ForeColor="Black" Width="475px"></asp:ListBox>
                                        <ajaxToolkit:ListSearchExtender ID="ddlClientCode_ListSearchExtender" runat="server"
                                            Enabled="True" IsSorted="True" TargetControlID="lbParentStockCode">
                                        </ajaxToolkit:ListSearchExtender>
                                    </td>
                                    <td valign="top">
                                        <table>
                                            <tr>
                                                <td style="width: 50px">
                                                    <asp:Label ID="lblPeriod" runat="server" Text="Period:" Font-Bold="True" Font-Names="Arial" ForeColor="Black"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" ForeColor="Black">
                                                        <asp:ListItem Selected="true">Select/Clear</asp:ListItem>
                                                        <asp:ListItem>Range</asp:ListItem>
                                                        <asp:ListItem>Last Month</asp:ListItem>
                                                        <asp:ListItem>Last 3 Months</asp:ListItem>
                                                        <asp:ListItem>Last 6 Months</asp:ListItem>
                                                        <asp:ListItem>Last 9 Months</asp:ListItem>
                                                        <asp:ListItem>Last 12 Months</asp:ListItem>
                                                        <asp:ListItem>Previous Year</asp:ListItem>
                                                        <asp:ListItem>Current Year</asp:ListItem>
                                                        <asp:ListItem>Previous 5 Years</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 50px">
                                                    <asp:Label ID="lblStartDate" runat="server" Font-Bold="True" ForeColor="Black" Text="From:"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:TextBox ID="txtStartDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ValidationGroup="" Width="250px" ForeColor="Black"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgStartDate" TargetControlID="txtStartDate">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="txtStartDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtStartDate">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    <ajaxToolkit:MaskedEditValidator ID="txtStartDateMEV" runat="server" ControlExtender="txtStartDateMEE" ControlToValidate="txtStartDate" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                    <ajaxToolkit:ValidatorCalloutExtender ID="txtStartDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtStartDateMEV" />
                                                </td>
                                                <td align="left">
                                                    <asp:ImageButton ID="imgStartDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 50px">
                                                    <asp:Label ID="lblEndDate" runat="server" Text="To:" Font-Bold="True" ForeColor="Black"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:TextBox ID="txtEndDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ValidationGroup="" Width="250px" ForeColor="Black"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgEndDate" TargetControlID="txtEndDate">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="txtEndDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtEndDate">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    <ajaxToolkit:MaskedEditValidator ID="txtEndDateMEV" runat="server" ControlExtender="txtEndDateMEE" ControlToValidate="txtEndDate" Display="None" EmptyValueMessage="Please enter an end date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                    <ajaxToolkit:ValidatorCalloutExtender ID="txtEndDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtEndDateMEV" />
                                                </td>
                                                <td align="left">
                                                    <asp:ImageButton ID="imgEndDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 50px">&nbsp;</td>
                                                <td align="right">&nbsp;</td>
                                                <td align="left">&nbsp;</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">&nbsp;</td>
                                    <td align="center" valign="top">
                                       
                                    </td>
                                    <td valign="top">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">&nbsp;</td>
                                    <td align="center" valign="top">&nbsp;
                                        <asp:Label ID="Label65" runat="server" Font-Bold="False" Font-Size="9pt" ForeColor="Navy" Text="(Use CTRL for Multiple Selection) "></asp:Label>
                                    </td>
                                    <td valign="top">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">&nbsp;</td>
                                    <td align="center" valign="top">
                                        <asp:LinkButton ID="lbSelectAll" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbnSelectAllStockCode_Click" ValidationGroup="nothing">Select All</asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="lbClearAll" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbnClearStockCode_Click" ValidationGroup="nothing">Clear All</asp:LinkButton>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="Label74" runat="server" CssClass="auto-style1" ForeColor="Black" Text="Cost Per Unit Data Source"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">&nbsp;</td>
                                    <td align="center" valign="top">
                                        <asp:RadioButtonList ID="rblSingleOrStockCodeRange" runat="server" Font-Size="10.5pt" ForeColor="Black" RepeatDirection="Horizontal" Width="400px">
                                            <asp:ListItem Selected="True">Stock Code List</asp:ListItem>
                                            <asp:ListItem>&nbsp;Stock Code Range</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                    <td valign="top">
                                        <asp:RadioButtonList ID="rblCostPerUnitDataSource" runat="server" Font-Size="10.5pt" ForeColor="Black" RepeatDirection="Horizontal" Width="525px">
                                            <asp:ListItem Selected="True">Cost Report</asp:ListItem>
                                            <asp:ListItem Value="Unit Cost Report (3 sec. per Stock Code)">Unit Cost Report (3 sec. per Stock Code)</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" valign="top">&nbsp;</td>
                                    <td align="center" valign="top">
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label72" runat="server" Font-Bold="True" ForeColor="Black" Text="FROM Stock Code"></asp:Label>
                                                </td>
                                                <td style="font-family: arial, Helvetica, sans-serif; color: black; font-weight: bold; font-size: 12px">&nbsp;</td>
                                                <td align="left">
                                                    <asp:Label ID="Label73" runat="server" Font-Bold="True" ForeColor="Black" Text="TO Stock Code"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtStockCodeFrom" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="200px" onkeypress="return OnlyNumber(event,this.id);">000000</asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="txtStockCodeFrom_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes" ServicePath="" TargetControlID="txtStockCodeFrom" UseContextKey="True">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                </td>
                                                <td style="font-family: arial, Helvetica, sans-serif; color: black; font-weight: bold; font-size: 12px">To</td>
                                                <td>
                                                    <asp:TextBox ID="txtStockCodeTo" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="200px">999999</asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="txtStockCodeTo_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes" ServicePath="" TargetControlID="txtStockCodeTo" UseContextKey="True">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                           <asp:LinkButton ID="btnPreview" runat="server" OnClick="btnPreview_Click" CssClass="btn btn-info"  Width="200px" ForeColor="White" ToolTip="Click to run report"><i class="fa fa-file-text-o" ></i>&nbsp;Run Report</asp:LinkButton> 

                            <asp:LinkButton ID="imgExportExcel" runat="server" CssClass="btn btn-success" ForeColor="White" OnClick="imgExportExcel_Click" Width="200px"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>

                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblError" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">&nbsp;
                            <asp:Label ID="lblRecordCount" runat="server" EnableViewState="False" Font-Bold="True" Font-Size="12pt" ForeColor="Blue"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">

                            <table id="tblGridview" align="center">
                                <tr>
                                    <td align="left">
                                        <!-- *** Begin Header Table *** -->
                                        <div id="DivHeader" style="vertical-align: top; width: 1450px; text-align: left;">
                                            <asp:Table ID="HeaderTable" runat="server"
                                                CellPadding="2"
                                                CellSpacing="0"
                                                Font-Size="11pt"
                                                ForeColor="White"
                                                BackColor="#333333"
                                                Font-Bold="False"
                                                Width="1505px"
                                                Visible="false">
                                            </asp:Table>
                                        </div>
                                        <!-- *** End Header Table *** -->
                                    </td>
                                </tr>
                                <tr class="rowColors">
                                    <td align="center">
                                        <asp:Panel ID="pnlGridView" runat="server" ScrollBars="Vertical" Visible="False" Width="100%">
                                            <div id="DivData" class="Container" style="vertical-align: top; width: 1505px;">
                                                <asp:GridView ID="gvReportCondensed" runat="server" AutoGenerateColumns="False"
                                                    GridLines="Vertical" Width="1505px" ShowFooter="True" ShowHeader="false"
                                                    BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                    BorderWidth="1px" ForeColor="Black" OnRowDataBound="gvReport_RowDataBound" OnSorting="gvReport_Sorting" AllowSorting="True">
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="lblError" runat="server" Text='NO RECORDS FOUND!!' Font-Size="12"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Stock<br/>Code" SortExpression="StockCode">
                                                            <HeaderStyle CssClass="CenterAligner" />
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbnStockCode" runat="server" Text='<%# Bind("StockCode") %>'
                                                                    CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                                    CausesValidation="False" ToolTip="Click to view Details."
                                                                    CommandName="Select"
                                                                    Width="70px"
                                                                    ForeColor="Navy" Font-Bold="True"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <FooterTemplate>
                                                                <asp:Label ID="LabelTotal" runat="server" Width="70px" Font-Bold="True" ForeColor="White" Text="Totals:"></asp:Label>
                                                            </FooterTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description">
                                                            <HeaderStyle CssClass="CenterAligner" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>' Width="250px" ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Uom">
                                                            <HeaderStyle CssClass="CenterAligner" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUom" runat="server" Text='<%# Bind("Uom") %>' Width="40px" ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Qty<br/> Sold">
                                                            <HeaderStyle CssClass="CenterAligner" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQtySold" runat="server" Text='<%# Bind("QtySold") %>' ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sales<br/>Date<br/>Range">
                                                            <HeaderStyle CssClass="CenterAligner" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSalesByDateRange" runat="server" Text='<%# Bind("SalesDateRange") %>' ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblSalesByDateRangeTotal" runat="server" Width="100px" Font-Bold="false" ForeColor="White" Font-Size="8pt"></asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Sales<br/>Last 12<br/>Month" SortExpression="SalesLast12Months">
                                                            <HeaderStyle CssClass="CenterAligner" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblSalesLast12Months" runat="server" Text='<%# Bind("SalesLast12Months") %>' ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                            <FooterTemplate>
                                                                <asp:Label ID="lblSalesLast12MonthsTotal" runat="server" Width="100px" Font-Bold="false" ForeColor="White" Font-Size="8pt"></asp:Label>
                                                            </FooterTemplate>
                                                            <FooterStyle HorizontalAlign="Right" />
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="Avg<br>Price">
                                                            <HeaderStyle CssClass="CenterAligner" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblAvgPrice" runat="server" Text='<%# Bind("AvgPrice") %>' ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Gross<br/>Margin %" SortExpression="Margin">
                                                            <HeaderStyle CssClass="CenterAligner" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblGrossMargin" runat="server" Text='<%# Bind("Margin") %>' ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Recipe<br/>Cost<br/>Per<br/>Unit">
                                                            <HeaderStyle CssClass="CenterAligner" BackColor="tan" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRecipeCostPerUnit" runat="server" Text='<%# Bind("RecipeCostPerUnit") %>' ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Pkg<br/>Cost<br/>Per<br/>Unit">
                                                            <HeaderStyle CssClass="CenterAligner" BackColor="tan" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblPkgCostPerUnit" runat="server" Text='<%# Bind("PkgCostPerUnit") %>' ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Labor<br/>OH<br/>Per<br/>Unit">
                                                            <HeaderStyle CssClass="CenterAligner" BackColor="tan" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLaborCostPerUnit" runat="server" Text='<%# Bind("LaborCostPerUnit") %>' ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Total<br/>Per<br/>Unit">
                                                            <HeaderStyle CssClass="CenterAligner" BackColor="tan" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblTotalPerUnit" runat="server" Text='<%# Bind("TotalCostPerUnit") %>' Font-Bold="false" ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:TemplateField>


                                                        <asp:TemplateField HeaderText="What If<br/>Recipe<br/>Cost<br/>Per<br/>Unit">
                                                            <HeaderStyle CssClass="CenterAligner" BackColor="Teal" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWiRecipeCostPerUnit" runat="server" Text='<%# Bind("WiRecipeCostPerUnit") %>' ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="What If<br/>Pkg<br/>Cost<br/>Per<br/>Unit">
                                                            <HeaderStyle CssClass="CenterAligner" BackColor="Teal" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWiPkgCostPerUnit" runat="server" Text='<%# Bind("WiPkgCostPerUnit") %>' ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="What If<br/>Labor<br/>OH<br/>Per<br/>Unit">
                                                            <HeaderStyle CssClass="CenterAligner" BackColor="Teal" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWiLaborCostPerUnit" runat="server" Text='<%# Bind("WiLaborCostPerUnit") %>' ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="What If<br/>Total<br/>Per<br/>Unit">
                                                            <HeaderStyle CssClass="CenterAligner" BackColor="Teal" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblWiTotalPerUnit" runat="server" Text='<%# Bind("wiTotalCostPerUnit") %>' Font-Bold="false" ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Last<br/>Price<br/>Change<br/>Date A">
                                                            <HeaderStyle CssClass="CenterAligner" BackColor="Teal" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLastPriceChangeDateA" runat="server" Text='<%# Bind("LastPriceChangeDateA") %>' Font-Bold="false" ForeColor="Black" Font-Size="8pt" Width="60px"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Last<br/>Price<br/>Change<br/>Date B">
                                                            <HeaderStyle CssClass="CenterAligner" BackColor="Teal" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLastPriceChangeDateB" runat="server" Text='<%# Bind("LastPriceChangeDateB") %>' Font-Bold="false" ForeColor="Black" Font-Size="8pt" Width="60px"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Last<br/>Price<br/>Change<br/>Date C">
                                                            <HeaderStyle CssClass="CenterAligner" BackColor="Teal" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLastPriceChangeDateC" runat="server" Text='<%# Bind("LastPriceChangeDateC") %>' Font-Bold="false" ForeColor="Black" Font-Size="8pt" Width="60px"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Last<br/>Price<br/>Change<br/>Date D">
                                                            <HeaderStyle CssClass="CenterAligner" BackColor="Teal" />
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblLastPriceChangeDateD" runat="server" Text='<%# Bind("LastPriceChangeDateD") %>' Font-Bold="false" ForeColor="Black" Font-Size="8pt" Width="60px"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="70px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle BackColor="#333333" ForeColor="White" VerticalAlign="Bottom" Font-Size="9pt" />
                                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                    <FooterStyle BackColor="Gray" />
                                                </asp:GridView>
                                            </div>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>                    
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgExportExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

