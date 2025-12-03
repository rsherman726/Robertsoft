<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CustomerReportShorted.aspx.cs" Inherits="CustomerReportShorted" %>

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
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Customer  Report Shorted" ForeColor="Black"></asp:Label>
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
                                                        <td align="center" style="cursor: pointer">
                                                            <asp:Label ID="Label76" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Red" Text="Select a Report Type From the 4 options below"></asp:Label>
                                                        </td>
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
                                                                                <td>
                                                                                    <asp:Button ID="btnStockCodeRangePanel" runat="server" CssClass="btn btn-info btn-xs" Text="ALL STOCKCODES" ToolTip="StockCode Range Search" Width="200px" OnClick="btnStockCodeRangePanel_Click" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnSingleStockCodePanel" runat="server" CssClass="btn btn-info" Text="SINGLE STOCKCODE" ToolTip="Single StockCode Search" Width="200px" OnClick="btnSingleStockCodePanel_Click" />
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Button ID="btnEndCustomerPanel" runat="server" CssClass="btn btn-info btn-xs" Text="END CUSTOMER" ToolTip="End Customer Search" Width="200px" OnClick="btnEndCustomerPanel_Click" />
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
                                                                                    <td align="center">
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <asp:Label ID="Label77" runat="server" Font-Bold="True" ForeColor="Black" Text="StockCode From"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <asp:TextBox ID="txtStockCodeFrom" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtStockCodeFrom_TextChanged" ValidationGroup="" Width="300px" placeholder="StockCode From">000000</asp:TextBox>
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
                                                                                                    <asp:TextBox ID="txtStockCodeTo" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="300px" placeholder="StockCode To">999999</asp:TextBox>
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
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblStockCodeDesc" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Navy"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td align="left">
                                                                                                    <asp:Label ID="lblSingleStockCode" runat="server" Font-Bold="True" ForeColor="Black" Text="StockCode:"></asp:Label>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <asp:TextBox ID="txtStockCodeSingle" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtStockCodeSingle_TextChanged" ValidationGroup="" Width="300px" placeholder="Single StockCode"></asp:TextBox>
                                                                                                    <ajaxToolkit:AutoCompleteExtender ID="txtStockCodeSingle_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                                                        CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes"
                                                                                                        ServicePath="" TargetControlID="txtStockCodeFrom" UseContextKey="True">
                                                                                                    </ajaxToolkit:AutoCompleteExtender>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>

                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                        <asp:Panel ID="pnlEndCustomer" runat="server" Visible="false">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="center"></td>
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
                                                                        <asp:TextBox ID="txtStartDate" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtStartDate_TextChanged" ValidationGroup=""></asp:TextBox>
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
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="lblEndDate" runat="server" Font-Bold="True" ForeColor="Black" Text="To:"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:TextBox ID="txtEndDate" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtEndDate_TextChanged" ValidationGroup=""></asp:TextBox>
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
                                                                        <asp:DropDownList ID="ddlCustomers" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black" OnSelectedIndexChanged="ddlCustomers_SelectedIndexChanged">
                                                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                                                        </asp:DropDownList>
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
                                                        <td align="left" valign="top">&nbsp;</td>
                                                    </tr>

                                                    <tr>
                                                        <td align="left" valign="top">&nbsp;
                                                            <asp:Label ID="label69" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="False" Font-Size="8pt" ForeColor="Red">**Date range is controled by setting Period: to Range and filling in: To and From.</asp:Label>
                                                        </td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="btnPreview" runat="server" OnClick="btnPreview_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Click to run full report"><i class="fa fa-file-text-o" ></i>&nbsp;Full Report</asp:LinkButton>
                                                <asp:LinkButton ID="btnSummary" runat="server" OnClick="btnSummary_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Click to run summary report"><i class="fa fa-file-text-o" ></i>&nbsp;Summary Report</asp:LinkButton>
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
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label79" runat="server" Font-Names="Arial" Font-Size="14pt" ForeColor="Navy" Font-Bold="True" Text="Shorted Full"></asp:Label></td>
                        </tr>
                        <tr id="trFullReport" runat="server">
                            <td align="center">

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
                                                <div id="DivData" class="Container" style="vertical-align: top; height: 300px; width: 1500px;">
                                                    <asp:GridView ID="gvReportCondensed" runat="server" AutoGenerateColumns="False" Font-Size="10pt"
                                                        GridLines="Vertical" ShowFooter="True"
                                                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                        BorderWidth="1px" ForeColor="Black" OnRowDataBound="gvReportCondensed_RowDataBound"
                                                        Font-Names="Arial" AllowSorting="false" OnSorting="gvReportCondensed_Sorting" ShowHeader="False" Width="1450px">
                                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="200px" Font-Size="8pt"></asp:Label>
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
                                                                    <asp:Label ID="lblUom" runat="server" Text='<%# Bind("CostUom") %>' Width="60px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="P<br/>C" SortExpression="PriceCode">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPriceCode" runat="server" Text='<%# Bind("PriceCode") %>' Width="45px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Order Qty" SortExpression="OrderedQty">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOrderQty" runat="server" Text='<%# Bind("OrderedQty") %>' Width="65px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblOrderQtyTotal" runat="server" Width="65px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ship Qty" SortExpression="InvoiceQty">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShipQty" runat="server" Text='<%# Bind("InvoiceQty") %>' Width="65px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblShipQtyTotal" runat="server" Width="65px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Shorted Qty" SortExpression="ShortedQty">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblShortedQty" runat="server" Text='<%# Bind("ShortedQty") %>' Width="65px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblShortedQtyTotal" runat="server" Width="65px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Invoice#" SortExpression="Invoice">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoice" runat="server" Text='<%# Bind("Invoice") %>' Width="70px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice Date" SortExpression="InvoiceDate">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Bind("InvoiceDate") %>' Width="70px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Sales Order" SortExpression="SalesOrder">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalesOrder" runat="server" Text='<%# Bind("SalesOrder") %>' Width="70px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Order Date" SortExpression="OrderDate">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOrderDate" runat="server" Text='<%# Bind("OrderDate") %>' Width="70px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Order Status" SortExpression="OrderStatus">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOrderStatus" runat="server" Text='<%# Bind("OrderStatus") %>' Width="80px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Disposition" SortExpression="Disposition">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDisposition" runat="server" Text='<%# Bind("Disposition") %>' Width="65px" Font-Size="9pt"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
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

                                <asp:Label ID="Label80" runat="server" Font-Names="Arial" Font-Size="14pt" ForeColor="Navy" Text="Shorted Summary" Font-Bold="True"></asp:Label>
                                <asp:GridView ID="gvCustomerSummary" runat="server"
                                    AutoGenerateColumns="False" BackColor="White"
                                    BorderColor="#CCCCCC" BorderStyle="Solid"
                                    BorderWidth="1px" Font-Names="Arial"
                                    Font-Size="10pt" ForeColor="Black"
                                    GridLines="Vertical"
                                    OnRowDataBound="gvCustomerSummary_RowDataBound"
                                    OnSorting="gvCustomerSummary_Sorting"
                                    ShowFooter="True" Width="1100px" AllowSorting="True">
                                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" />
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
                                        <asp:TemplateField HeaderText="Stock Code" SortExpression="StockCode">
                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Width="85px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>' Width="100px" Font-Size="7pt"></asp:Label>
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
                                        <asp:TemplateField HeaderText="Order Qty" SortExpression="OrderedQty">
                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderQty" runat="server" Text='<%# Bind("OrderedQty") %>' Width="65px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblOrderQtyTotal" runat="server" Width="65px"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ship Qty" SortExpression="InvoiceQty">
                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipQty" runat="server" Text='<%# Bind("InvoiceQty") %>' Width="65px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblShipQtyTotal" runat="server" Width="65px"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shorted Qty" SortExpression="ShortedQty">
                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblShortedQty" runat="server" Text='<%# Bind("ShortedQty") %>' Width="65px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblShortedQtyTotal" runat="server" Width="65px"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
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
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table id="Table1" align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:LinkButton ID="imgExportExcel1" runat="server" OnClick="imgExportExcel1_Click" CssClass="btn btn-success" ForeColor="White" Width="200px"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center"></td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgExportExcel1" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

