<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CustomerReportsNew.aspx.cs" Inherits="CustomerReportsNew" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .Prompt {
            color: navy;
            font-weight: bold;
        }
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#MainContent_gvReportCondensed tr').click(function (e) {
                $('#MainContent_gvReportCondensed tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });
            $('#MainContent_gvCustomerSummary tr').click(function (e) {
                $('#MainContent_gvCustomerSummary tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });


        });

        function isPostBack() {//Use during postbacks...
            $('#MainContent_gvReportCondensed tr').click(function (e) {
                $('#MainContent_gvReportCondensed tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });
            $('#MainContent_gvCustomerSummary tr').click(function (e) {
                $('#MainContent_gvCustomerSummary tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });


        }; //End Postback... 

    </script>
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
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Customer  Report" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="background-color: lightblue;" align="center" class="JustRoundedEdgeBothSmall">
                                <asp:Panel ID="pnlSearchCriteria" runat="server" Width="1400px">
                                    <table width="1400">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td style="cursor: pointer; width: 600px;" align="center">&nbsp;</td>
                                                        <td style="cursor: pointer; width: 50px;" align="left">&nbsp;</td>
                                                        <td align="left" style="cursor: pointer">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" style="cursor: pointer">&nbsp;</td>
                                                        <td align="left" style="cursor: pointer; width: 50px;">&nbsp;</td>
                                                        <td align="left" style="cursor: pointer">
                                                            <asp:Label ID="Label12" runat="server" Font-Bold="True" ForeColor="Black" Text="Date(s)"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="left">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td align="center">
                                                                        <table width="730">
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <asp:Button ID="btnStockCodeRangePanel" runat="server" CssClass="btn btn-info" Text="ALL STOCKCODES" ToolTip="StockCode Range Search" Width="200px" OnClick="btnStockCodeRangePanel_Click" />
                                                                                </td>

                                                                                <td align="center">
                                                                                    <asp:Button ID="btnEndCustomerPanel" runat="server" CssClass="btn btn-info" Text="END CUSTOMERS" ToolTip="End Customer Search" Width="200px" OnClick="btnEndCustomerPanel_Click" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="pnlStockCodeRange" runat="server" Visible="false">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="center">&nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">&nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <asp:Label ID="Label77" runat="server" Font-Bold="True" ForeColor="Black" Text="StockCode From"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <asp:TextBox ID="txtStockCodeFrom" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="300px" placeholder="StockCode From" onkeypress="return OnlyNumber(event,this.id);">000000</asp:TextBox>
                                                                                                    <ajaxToolkit:AutoCompleteExtender ID="txtStockCodeFrom_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes" ServicePath="" TargetControlID="txtStockCodeFrom" UseContextKey="True">
                                                                                                    </ajaxToolkit:AutoCompleteExtender>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <asp:Label ID="Label73" runat="server" Font-Bold="True" ForeColor="Black" Text="StockCode To"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <asp:TextBox ID="txtStockCodeTo" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="300px" placeholder="StockCode To" onkeypress="return OnlyNumber(event,this.id);">999999</asp:TextBox>
                                                                                                    <ajaxToolkit:AutoCompleteExtender ID="txtStockCodeTo_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes" ServicePath="" TargetControlID="txtStockCodeTo" UseContextKey="True">
                                                                                                    </ajaxToolkit:AutoCompleteExtender>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                        <asp:Panel ID="pnlSingleStockCode" runat="server" Visible="true">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="center">&nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">&nbsp;</td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                        <asp:Panel ID="pnlEndCustomer" runat="server" Visible="false">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="center"></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">&nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td style="width: 100px">
                                                                                                    <asp:Label ID="Label70" runat="server" Font-Bold="True" ForeColor="Black" Text="End Customer:" Width="110px"></asp:Label>
                                                                                                </td>
                                                                                                <td align="center">
                                                                                                    <asp:DropDownList ID="ddlEndCustomers" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black" OnSelectedIndexChanged="ddlEndCustomers_SelectedIndexChanged" Width="350px">
                                                                                                        <asp:ListItem Selected="True">All</asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="width: 100px">&nbsp;</td>
                                                                                                <td align="center">
                                                                                                    <asp:Label ID="lblStockCodeList" runat="server" Font-Bold="True" ForeColor="Navy" Text="Stock Code(s)"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="width: 100px">
                                                                                                    <asp:Label ID="Label78" runat="server" Font-Bold="True" ForeColor="Black" Text="End Customer StockCodes:" Width="110px"></asp:Label>
                                                                                                </td>
                                                                                                <td align="center">
                                                                                                    <asp:ListBox ID="lbParentStockCodeEndCustomer" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="9pt" ForeColor="Black" Height="250px" SelectionMode="Multiple" Style="width: 350px !important" Width="300px"></asp:ListBox>
                                                                                                    <ajaxToolkit:ListSearchExtender ID="lbParentStockCodeEndCustomer_ListSearchExtender" runat="server" Enabled="True" IsSorted="True" PromptCssClass="Prompt" TargetControlID="lbParentStockCodeEndCustomer">
                                                                                                    </ajaxToolkit:ListSearchExtender>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="width: 100px">&nbsp;</td>
                                                                                                <td align="center">
                                                                                                    <asp:Label ID="Label74" runat="server" Font-Bold="False" Font-Size="9pt" ForeColor="Navy" Text="(Use CTRL for Multiple Selection) "></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td style="width: 100px">&nbsp;</td>
                                                                                                <td align="center">
                                                                                                    <asp:LinkButton ID="lbSelectAllEndCustomer" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbnSelectAllStockCodeEndCustomer_Click" ValidationGroup="nothing">Select All</asp:LinkButton>
                                                                                                    &nbsp;<asp:LinkButton ID="lbClearAllEndCustomer" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbnClearStockCodeEndCustomer_Click" ValidationGroup="nothing">Clear All</asp:LinkButton>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">&nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">&nbsp;</td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top" align="left">&nbsp;</td>
                                                        <td align="left" valign="top">
                                                            <table style="width: 550px">
                                                                <tr>
                                                                    <td style="width: 115px">
                                                                        <asp:Label ID="lblPeriod" runat="server" Font-Bold="True" ForeColor="Black" Text="Period:"></asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="10pt" ForeColor="Black" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged">
                                                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                                                            <asp:ListItem>Range</asp:ListItem>
                                                                            <asp:ListItem>Current Month</asp:ListItem>
                                                                            <asp:ListItem>Last Month</asp:ListItem>
                                                                            <asp:ListItem>Last 3 Months</asp:ListItem>
                                                                            <asp:ListItem>Last 6 Months</asp:ListItem>
                                                                            <asp:ListItem>Last 9 Months</asp:ListItem>
                                                                            <asp:ListItem>Last 12 Months</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="left" style="width: 60px">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100px">
                                                                        <asp:Label ID="lblStartDate" runat="server" Font-Bold="True" ForeColor="Black" Text="From:"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:TextBox ID="txtStartDate" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup=""></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgStartDate" TargetControlID="txtStartDate"></ajaxToolkit:CalendarExtender>
                                                                        <ajaxToolkit:MaskedEditExtender ID="txtStartDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtStartDate"></ajaxToolkit:MaskedEditExtender>
                                                                        <ajaxToolkit:MaskedEditValidator ID="txtStartDateMEV" runat="server" ControlExtender="txtStartDateMEE" ControlToValidate="txtStartDate" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtStartDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtStartDateMEV" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:ImageButton ID="imgStartDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="lblEndDate" runat="server" Font-Bold="True" ForeColor="Black" Text="To:"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:TextBox ID="txtEndDate" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup=""></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgEndDate" TargetControlID="txtEndDate"></ajaxToolkit:CalendarExtender>
                                                                        <ajaxToolkit:MaskedEditExtender ID="txtEndDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtEndDate"></ajaxToolkit:MaskedEditExtender>
                                                                        <ajaxToolkit:MaskedEditValidator ID="txtEndDateMEV" runat="server" ControlExtender="txtEndDateMEE" ControlToValidate="txtEndDate" Display="None" EmptyValueMessage="Please enter an end date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtEndDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtEndDateMEV" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:ImageButton ID="imgEndDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="Label15" runat="server" Font-Bold="True" ForeColor="Black" Text="Year:"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:DropDownList ID="ddlYear" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="10pt" ForeColor="Black">
                                                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                                                            <asp:ListItem>2006</asp:ListItem>
                                                                            <asp:ListItem>2007</asp:ListItem>
                                                                            <asp:ListItem>2008</asp:ListItem>
                                                                            <asp:ListItem>2009</asp:ListItem>
                                                                            <asp:ListItem>2010</asp:ListItem>
                                                                            <asp:ListItem>2011</asp:ListItem>
                                                                            <asp:ListItem>2012</asp:ListItem>
                                                                            <asp:ListItem>2013</asp:ListItem>
                                                                            <asp:ListItem>2014</asp:ListItem>
                                                                            <asp:ListItem>2015</asp:ListItem>
                                                                            <asp:ListItem>2016</asp:ListItem>
                                                                            <asp:ListItem>2017</asp:ListItem>
                                                                            <asp:ListItem>2018</asp:ListItem>
                                                                            <asp:ListItem>2019</asp:ListItem>
                                                                            <asp:ListItem>2020</asp:ListItem>
                                                                            <asp:ListItem>2021</asp:ListItem>
                                                                            <asp:ListItem>2022</asp:ListItem>
                                                                            <asp:ListItem>2023</asp:ListItem>
                                                                            <asp:ListItem>2024</asp:ListItem>
                                                                            <asp:ListItem>2025</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:Label ID="label44" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="9pt" ForeColor="Red" Width="110px">Year Selection Ignores Dates</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="Label14" runat="server" Font-Bold="True" ForeColor="Black" Text="Customer:"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:ListBox ID="lbCustomers" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black" OnSelectedIndexChanged="lbCustomers_SelectedIndexChanged" Height="85px">
                                                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                                                        </asp:ListBox>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:RadioButtonList ID="rblSort" runat="server" AutoPostBack="True" Font-Bold="True" Font-Names="Arial" Font-Size="8pt" ForeColor="Black" OnSelectedIndexChanged="rblSort_SelectedIndexChanged" Width="115px">
                                                                            <asp:ListItem Selected="True" Value="Name">&nbsp;Sort By Name</asp:ListItem>
                                                                            <asp:ListItem Value="Number">&nbsp;Sort By Number</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="Label80" runat="server" Font-Bold="True" ForeColor="Black" Text="Customer Cities:" Width="125px"></asp:Label>
                                                                        <asp:LinkButton ID="lbClearAllCities" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbClearAllCities_Click" ValidationGroup="nothing">Clear All</asp:LinkButton>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:ListBox ID="lbCities" runat="server" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black" Height="100px" SelectionMode="Multiple"></asp:ListBox>
                                                                    </td>
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="Label81" runat="server" Font-Bold="True" ForeColor="Black" Text="Customer Zips:" Width="125px"></asp:Label>
                                                                        <asp:LinkButton ID="lbClearAllZips" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbClearAllZips_Click" ValidationGroup="nothing">Clear All</asp:LinkButton>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:ListBox ID="lbZips" runat="server" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black" Height="75px" SelectionMode="Multiple"></asp:ListBox>
                                                                    </td>
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="Label68" runat="server" Font-Bold="True" ForeColor="Black" Text="Sales Person:"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:DropDownList ID="ddlSalesPerson" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black">
                                                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="Label11" runat="server" Font-Bold="True" ForeColor="Black" Text="Margin:"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:DropDownList ID="ddlMargins" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="10pt" ForeColor="Black" OnSelectedIndexChanged="ddlMargins_SelectedIndexChanged">
                                                                            <asp:ListItem>Single</asp:ListItem>
                                                                            <asp:ListItem>Range</asp:ListItem>
                                                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="lblMarginFrom" runat="server" Font-Bold="True" ForeColor="Black" Text="From:"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:TextBox ID="txtMarginFrom" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup=""></asp:TextBox>
                                                                    </td>
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="lblMarginTo" runat="server" Font-Bold="True" ForeColor="Black" Text="To:"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:TextBox ID="txtMarginTo" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup=""></asp:TextBox>
                                                                    </td>
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="Label67" runat="server" Font-Bold="True" ForeColor="Black" Text="Product Class:" Width="115px"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:DropDownList ID="ddlProductClass" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black">
                                                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top">&nbsp;
                                                            <asp:Label ID="lblErrorRange" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="10pt" ForeColor="Red"></asp:Label>
                                                        </td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                        <td align="left" valign="top">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label79" runat="server" Font-Bold="True" ForeColor="Black" Text="Summary Years Back:" Width="160px"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlYearsBack" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black" Width="100px" AutoPostBack="True" OnSelectedIndexChanged="ddlYearsBack_SelectedIndexChanged">
                                                                            <asp:ListItem Selected="True">1</asp:ListItem>
                                                                            <asp:ListItem>2</asp:ListItem>
                                                                            <asp:ListItem>3</asp:ListItem>
                                                                            <asp:ListItem>4</asp:ListItem>
                                                                            <asp:ListItem>5</asp:ListItem>
                                                                        </asp:DropDownList></td>
                                                                </tr>
                                                            </table>

                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td align="left" valign="top">
                                                            <asp:LinkButton ID="lbnTrendsReport" runat="server" OnClick="lbnTrendsReport_Click" CssClass="btn btn-success" Width="300px" ForeColor="White" ToolTip="Click to run Purchasing Trends Report ($)"><i class="fa fa-file-text-o" ></i>&nbsp;Purchasing Trends Report ($)</asp:LinkButton>

                                                            <asp:LinkButton ID="lbnTrendsReportByQty" runat="server" OnClick="lbnTrendsReportByQty_Click" CssClass="btn btn-success" Width="300px" ForeColor="White" ToolTip="Click to run Purchasing Trends Report (Qty)"><i class="fa fa-file-text-o" ></i>&nbsp;Purchasing Trends Report (Qty)</asp:LinkButton>
                                                            <br />
                                                            <asp:Label ID="label1" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="8pt" ForeColor="Red">**It's recommended you run Trends Report after Customer Report. Running a Summary report then a Trends Report will produce a Summary Trends Report and running a Full Report first will produce a Full Trends Report with StockCodes.</asp:Label>
                                                            <br />
                                                            <asp:Label ID="label66" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="8pt" ForeColor="Red">**Run Trends Report for All Stock Codes with Stock Code From (000000) and Stock Code To (999999).</asp:Label>
                                                            <br />
                                                            <asp:Label ID="label69" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="False" Font-Size="8pt" ForeColor="Red">**Date range is controled by setting Period: to Range and filling in: To and From.</asp:Label>
                                                        </td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                        <td align="left" valign="top">
                                                            <asp:CheckBox ID="chkIncludeEndCustomerInSummary" runat="server" ForeColor="Black" Text="&amp;nbsp;Include End Customer in Summary" />&nbsp;
                                                            <asp:CheckBox ID="chkIncludeGroupingInSummary" runat="server" ForeColor="Black" Text="&amp;nbsp;Include Grouping in Summary" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="btnPreview" runat="server" OnClick="btnPreview_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Click to run full report"><i class="fa fa-file-text-o" ></i>&nbsp;Full Report</asp:LinkButton>
                                                <asp:LinkButton ID="btnRunReportSummary" runat="server" OnClick="btnRunReportSummary_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Click to run summary report"><i class="fa fa-file-text-o" ></i>&nbsp;Summary Report</asp:LinkButton>
                                                <asp:LinkButton ID="btnReset" runat="server" OnClick="btnReset_Click" CssClass="btn btn-danger" Width="200px" ForeColor="White" ToolTip="Click to reset form"><i class="fa fa-refresh" ></i>&nbsp;Reset Form</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblError" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
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
                                                    Width="1574px"
                                                    Visible="false">
                                                </asp:Table>
                                            </div>
                                            <!-- *** End Header Table *** -->
                                        </td>
                                    </tr>
                                    <tr class="rowColors">
                                        <td align="left">
                                            <asp:Panel ID="pnlGridView" runat="server" Height="300px" ScrollBars="Vertical" Visible="False">
                                                <div id="DivData" class="Container" style="vertical-align: top; height: 300px; width: 1350px;">
                                                    <asp:GridView ID="gvReportCondensed" runat="server" AutoGenerateColumns="False" Font-Size="10pt"
                                                        GridLines="Vertical" ShowFooter="True" ShowHeader="False"
                                                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                        BorderWidth="1px" ForeColor="Black" OnRowDataBound="gvReportCondensed_RowDataBound"
                                                        Font-Names="Arial" AllowSorting="True" OnSorting="gvReportCondensed_Sorting">
                                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="100px" Font-Size="7"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotal" runat="server" Text="Totals:" Width="75px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="center" Font-Bold="true" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cust#" SortExpression="Customer">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Width="70px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sales<br/>Person" SortExpression="SalesPerson">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalesPerson" runat="server" Text='<%# Bind("SalesPerson") %>' Width="100px" Font-Size="7"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <%-- <asp:TemplateField HeaderText="Sales#">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalesPersonID" runat="server" Text='<%# Bind("SalesPersonID") %>' Width="70px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField HeaderText="Stock<br/>Code" SortExpression="StockCode">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbnStockCode" runat="server" Text='<%# Bind("StockCode") %>'
                                                                        CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                                        CausesValidation="False" ToolTip="Click to view Details."
                                                                        CommandName="Select"
                                                                        Width="90px"
                                                                        ForeColor="Navy" Font-Bold="True" Font-Size="8"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>' Width="100px" Font-Size="7pt"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Product Class" SortExpression="PCDescription">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductClass" runat="server" Text='<%# Bind("PCDescription") %>' Width="100px" Font-Size="7pt"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Uom">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUom" runat="server" Text='<%# Bind("CostUom") %>' Width="50px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qty">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Bind("InvoiceQty") %>' Width="65px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>' Width="85px"></asp:Label>
                                                                    <asp:Label ID="lblCostValue" runat="server" Text='<%# Bind("CostValue") %>' Width="80px" Visible="false"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblAmountTotal" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Margin" SortExpression="Margin">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMargin" runat="server" Text='<%# Bind("Margin") %>' Width="75px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblMarginAvgTotal" runat="server" Text="0.00" Width="75px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="P<br/>C" SortExpression="PriceCode">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPriceCode" runat="server" Text='<%# Bind("PriceCode") %>' Width="30px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Price">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAvgPrice" runat="server" Text='<%# Bind("Price") %>' Width="85px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Price<br/> Change<br/> Date">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLastChangeDate" runat="server" Text='<%# Bind("LastChangeDate") %>' Width="80px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="YTD" SortExpression="CurrentYTD">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblYTD" runat="server" Text='<%# Bind("CurrentYTD") %>' Width="85px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblYTDTotal" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Year - 1">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblYearMinus1" runat="server" Text='<%# Bind("YearMinus1") %>' Width="85px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblYearMinus1Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Year - 2">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblYearMinus2" runat="server" Text='<%# Bind("YearMinus2") %>' Width="85px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblYearMinus2Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Year - 3">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblYearMinus3" runat="server" Text='<%# Bind("YearMinus3") %>' Width="85px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblYearMinus3Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Year - 4">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblYearMinus4" runat="server" Text='<%# Bind("YearMinus4") %>' Width="85px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblYearMinus4Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Year - 5">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblYearMinus5" runat="server" Text='<%# Bind("YearMinus5") %>' Width="85px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblYearMinus5Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
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
                        <tr id="trSummary" runat="server" class="rowColors">

                            <td align="center">

                                <asp:Label ID="Label3" runat="server" Font-Names="Arial" Font-Size="14pt" ForeColor="Navy" Text="Customer Report Summary" Font-Bold="True"></asp:Label>
                                <asp:Panel ID="pnlSummary" runat="server" Width="1400px" ScrollBars="Horizontal">
                                    <asp:GridView ID="gvCustomerSummary" runat="server"
                                        AutoGenerateColumns="False" BackColor="White"
                                        BorderColor="#CCCCCC" BorderStyle="Solid"
                                        BorderWidth="1px" Font-Names="Arial"
                                        Font-Size="10pt" ForeColor="Black"
                                        GridLines="Vertical"
                                        OnRowDataBound="gvCustomerSummary_RowDataBound"
                                        OnSorting="gvCustomerSummary_Sorting"
                                        ShowFooter="True" Width="1650px" AllowSorting="True">
                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="225px"></asp:Label>
                                                    <asp:Label ID="lblRangeCost" runat="server" Text='<%# Bind("RangeCost") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblCurrentYTDCost" runat="server" Text='<%# Bind("CurrentYTDCost") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblYearMinus1Cost" runat="server" Text='<%# Bind("YearMinus1Cost") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblYearMinus2Cost" runat="server" Text='<%# Bind("YearMinus2Cost") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblYearMinus3Cost" runat="server" Text='<%# Bind("YearMinus3Cost") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblYearMinus4Cost" runat="server" Text='<%# Bind("YearMinus4Cost") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblYearMinus5Cost" runat="server" Text='<%# Bind("YearMinus5Cost") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblPriorYearRangeCost" runat="server" Text='<%# Bind("PriorYearRangeCost") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotal" runat="server" Text="Totals:" Width="75px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle Font-Bold="true" HorizontalAlign="center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Cust#" SortExpression="Customer">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Width="70px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Salesperson" SortExpression="SalesPerson">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSalesPerson" runat="server" Text='<%# Bind("SalesPerson") %>' Width="200px" Font-Size="9px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount<br>(Range)" SortExpression="RangeAmount">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("RangeAmount") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblAmountTotal" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Margin<br>(Range)" SortExpression="RangeMargin">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMargin" runat="server" Text='<%# Bind("RangeMargin") %>' Width="75px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblMarginAvg" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="% of<br>Grand Total<br>(Range)" SortExpression="PercentageOfTotal" Visible="false">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPercentageOfTotal" runat="server" Text='<%# Bind("PercentageOfTotal") %>' Width="75px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="% of<br>Grand Total (Range)<br>Prior" SortExpression="PercentageOfTotalPriorYearRange" Visible="false">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPercentageOfTotalPriorYearRange" runat="server" Text='<%# Bind("PercentageOfTotalPriorYearRange") %>' Width="75px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Amount<br>(Range)<br>Prior" SortExpression="PriorYearRangeAmount">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPriorYearRangeAmount" runat="server" Text='<%# Bind("PriorYearRangeAmount") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblPriorYearRangeAmountTotal" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Margin<br>(Range)<br>Prior" SortExpression="PriorYearRangeMargin">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPriorYearRangeMargin" runat="server" Text='<%# Bind("PriorYearRangeMargin") %>' Width="75px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblPriorYearRangeMarginAvg" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="YTD" SortExpression="CurrentYTD">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYTD" runat="server" Text='<%# Bind("CurrentYTD") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblYTDTotal" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Prior YTD" SortExpression="PriorYTD">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPriorYTD" runat="server" Text='<%# Bind("PriorYTD") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblPriorYTDTotal" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Change %">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChangePercentage" runat="server" Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="YTD Margin" SortExpression="CurrentYTDMargin">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCurrentYTDMargin" runat="server" Text='<%# Bind("CurrentYTDMargin") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblCurrentYTDMarginAvg" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="% of<br>Grand Total YTD" SortExpression="PercentageOfTotalCurrentYTD" Visible="false">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPercentageOfTotalCurrentYTD" runat="server" Text='<%# Bind("PercentageOfTotalCurrentYTD") %>' Width="75px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" SortExpression="YearMinus1">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYearMinus1" runat="server" Text='<%# Bind("YearMinus1") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblYearMinus1Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" SortExpression="YearMinus1Margin">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYearMinus1Margin" runat="server" Text='<%# Bind("YearMinus1Margin") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblYearMinus1MarginAvg" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" SortExpression="YearMinus2">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYearMinus2" runat="server" Text='<%# Bind("YearMinus2") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblYearMinus2Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" SortExpression="YearMinus2Margin">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYearMinus2Margin" runat="server" Text='<%# Bind("YearMinus2Margin") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblYearMinus2MarginAvg" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" SortExpression="YearMinus3">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYearMinus3" runat="server" Text='<%# Bind("YearMinus3") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblYearMinus3Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" SortExpression="YearMinus3Margin">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYearMinus3Margin" runat="server" Text='<%# Bind("YearMinus3Margin") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblYearMinus3MarginAvg" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" SortExpression="YearMinus4">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYearMinus4" runat="server" Text='<%# Bind("YearMinus4") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblYearMinus4Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" SortExpression="YearMinus4Margin">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYearMinus4Margin" runat="server" Text='<%# Bind("YearMinus4Margin") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblYearMinus4MarginAvg" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" SortExpression="YearMinus5">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYearMinus5" runat="server" Text='<%# Bind("YearMinus5") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblYearMinus5Total" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="" SortExpression="YearMinus5Margin">
                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblYearMinus5Margin" runat="server" Text='<%# Bind("YearMinus5Margin") %>' Width="85px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="right" />
                                                <FooterTemplate>
                                                    <asp:Label ID="lblYearMinus5MarginAvg" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                </FooterTemplate>
                                                <FooterStyle HorizontalAlign="right" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                        <FooterStyle BackColor="Wheat" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:LinkButton ID="btnExportFullReport" runat="server" OnClick="btnExportFullReport_Click" CssClass="btn btn-success" ForeColor="White" Width="275px"><i class="glyphicon glyphicon-export"></i>&nbsp;EXPORT TO EXCEL FULL REPORT</asp:LinkButton>
                                <asp:LinkButton ID="btnExportSummary" runat="server" OnClick="btnExportSummary_Click" CssClass="btn btn-success" ForeColor="White" Width="275px"><i class="glyphicon glyphicon-export"></i>&nbsp;EXPORT TO EXCEL SUMMARY</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblErrorPrior" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="10pt" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportFullReport" />
            <asp:PostBackTrigger ControlID="btnExportSummary" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
