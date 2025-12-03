<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="StockCodeListCustomerReport.aspx.cs" Inherits="StockCodeListCustomerReport" MaintainScrollPositionOnPostback="true" %>

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
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Customer  Report - Stock Code List" ForeColor="Black"></asp:Label>
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
                                                            <asp:Label ID="Label76" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Red" Text="Select a Report Type From the 4 options below" Visible="False"></asp:Label>
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
                                                                    <td>
                                                                        <asp:Panel ID="pnlStockCodeList" runat="server">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="center">&nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="Label75" runat="server" Font-Bold="False" Font-Size="14pt" ForeColor="Red" Text="Stock Code List has 3 Modes: Ingredient Based search, Component based search and  All StockCodes, select from WipMaster or InvMaster"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <table width="100%">
                                                                                            <tr>
                                                                                                <td valign="top">
                                                                                                    <table width="100%">
                                                                                                        <tr>
                                                                                                            <td align="center">&nbsp;</td>
                                                                                                            <td>&nbsp;</td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="center">
                                                                                                                <asp:RadioButtonList ID="rblIngredientComponent" runat="server" Font-Size="7pt" ForeColor="Black" RepeatDirection="Horizontal" Width="350px" AutoPostBack="True" OnSelectedIndexChanged="rblIngredientComponent_SelectedIndexChanged">
                                                                                                                    <asp:ListItem Selected="True">&nbsp;Ingredient Stock Code</asp:ListItem>
                                                                                                                    <asp:ListItem>&nbsp;Component/Packaging Stock Code</asp:ListItem>
                                                                                                                </asp:RadioButtonList>
                                                                                                            </td>
                                                                                                            <td>&nbsp;</td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="center">&nbsp;</td>
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
                                                                                                        <tr>
                                                                                                            <td align="center">&nbsp;</td>
                                                                                                            <td>&nbsp;</td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="center">
                                                                                                                <asp:Button ID="btnLoadAll" runat="server" CssClass="btn btn-default" Font-Bold="True" OnClick="btnLoadAll_Click" Text="Load All Finished Stock Code" />
                                                                                                            </td>
                                                                                                            <td>&nbsp;</td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                                <td>
                                                                                                    <table width="100%">
                                                                                                        <tr>
                                                                                                            <td align="center">
                                                                                                                <asp:Label ID="lblStockCodeList" runat="server" Font-Bold="True" ForeColor="Navy" Text="Stock Code(s)"></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="center">
                                                                                                                <asp:Label ID="Label53" runat="server" Font-Bold="True" ForeColor="Black" Text="Finished Stock Code Source"></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="center">
                                                                                                                <asp:RadioButtonList ID="rblSourceOfStockCodes" runat="server" AutoPostBack="True" ForeColor="Black" OnSelectedIndexChanged="rblSourceOfStockCodes_SelectedIndexChanged" RepeatDirection="Horizontal" Width="250px">
                                                                                                                    <asp:ListItem Selected="True">&nbsp;WipMaster</asp:ListItem>
                                                                                                                    <asp:ListItem>&nbsp;InvMaster</asp:ListItem>
                                                                                                                </asp:RadioButtonList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="center">&nbsp;</td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <asp:ListBox ID="lbParentStockCode" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="9pt" ForeColor="Black" Height="250px" SelectionMode="Multiple" Style="width: 350px !important"></asp:ListBox>
                                                                                                                <ajaxToolkit:ListSearchExtender ID="lbParentStockCode_ListSearchExtender" runat="server" Enabled="True" IsSorted="True" PromptCssClass="Prompt" TargetControlID="lbParentStockCode">
                                                                                                                </ajaxToolkit:ListSearchExtender>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="center">
                                                                                                                <asp:Label ID="Label1" runat="server" Font-Bold="False" Font-Size="9pt" ForeColor="Navy" Text="(Use CTRL for Multiple Selection) "></asp:Label></td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="center">
                                                                                                                <asp:LinkButton ID="lbSelectAll" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbnSelectAllStockCode_Click" ValidationGroup="nothing">Select All</asp:LinkButton>
                                                                                                                &nbsp;<asp:LinkButton ID="lbClearAll" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbnClearStockCode_Click" ValidationGroup="nothing">Clear All</asp:LinkButton>
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
                                                                                <tr>
                                                                                    <td align="center"></td>
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
                                                        <td align="left" valign="top">&nbsp;</td>
                                                    </tr>

                                                    <tr>
                                                        <td align="left" valign="top">
                                                             <asp:LinkButton ID="lbnTrendsReport" runat="server" OnClick="lbnTrendsReport_Click" CssClass="btn btn-success"  Width="300px" ForeColor="White" ToolTip="Click to run Purchasing Trends Report ($)"><i class="fa fa-file-text-o" ></i>&nbsp;Purchasing Trends Report ($)</asp:LinkButton> 
                                                           
                                                             <asp:LinkButton ID="lbnTrendsReportByQty" runat="server" OnClick="lbnTrendsReportByQty_Click" CssClass="btn btn-success"  Width="300px" ForeColor="White" ToolTip="Click to run Purchasing Trends Report (Qty)"><i class="fa fa-file-text-o" ></i>&nbsp;Purchasing Trends Report (Qty)</asp:LinkButton>
                                                            <asp:Label ID="label66" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="8pt" ForeColor="Red">**Run after Customer Report - Will Return all Stock Codes since the Trends Report either takes a single Stock Code or All Stock Codes</asp:Label>
                                                            <br />
                                                            <asp:Label ID="label69" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="8pt" ForeColor="Red">**Date range is controled by setting Period: to Range and filling in: To and From.</asp:Label>
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
                                                <asp:LinkButton ID="btnReportSummary" runat="server" OnClick="btnSummary_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Click to run summary report"><i class="fa fa-file-text-o" ></i>&nbsp;Summary Report</asp:LinkButton>
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
                                                        Font-Names="Arial" AllowSorting="false" OnSorting="gvReportCondensed_Sorting">
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
                                        <asp:TemplateField HeaderText="Amount" SortExpression="SubTotal">
                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("SubTotal") %>' Width="85px"></asp:Label>
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
                                            <%-- <FooterTemplate>
                                                <asp:Label ID="lblMarginAvgTotal" runat="server" Text="0.00" Width="75px"></asp:Label>
                                            </FooterTemplate>--%>
                                            <FooterStyle HorizontalAlign="right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="% of Grand Total" SortExpression="PercentageOfTotal">
                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblPercentageOfTotal" runat="server" Text='<%# Bind("PercentageOfTotal") %>' Width="75px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="right" />
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
                                            <asp:ImageButton ID="imgExportExcel1" runat="server" ImageUrl="~/Images/ExportToExcel.GIF" OnClick="imgExportExcel1_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center" style="background-color: #CCCCCC;" class="JustRoundedEdgeBothSmall">
                                <asp:Panel ID="pnlSearchCriteria0" runat="server" Visible="False">
                                    <table width="900px">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td align="left">
                                                            <asp:Label ID="Label47" runat="server" Text="Stock Code" Font-Bold="True" ForeColor="Black"></asp:Label>&nbsp;
                                                        </td>
                                                        <td align="left" style="width: 450px">
                                                            <asp:Label ID="Label48" runat="server" Text="Customer:" Font-Bold="True" ForeColor="Black"></asp:Label>
                                                        </td>
                                                        <td align="left" style="width: 450px">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left">&nbsp;</td>
                                                        <td align="left" style="width: 450px">&nbsp;</td>
                                                        <td align="left" style="width: 450px">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td style="cursor: pointer" align="left">
                                                            <asp:ListBox ID="lbStockCode0" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="9pt" Height="200px" SelectionMode="Multiple" Style="width: 500px !important" ToolTip="Start typing a stock code to search" ForeColor="Black"></asp:ListBox>
                                                            <ajaxToolkit:ListSearchExtender ID="ddlClientCode_ListSearchExtender" runat="server"
                                                                Enabled="True" IsSorted="True" TargetControlID="lbStockCode0" PromptCssClass="Prompt">
                                                            </ajaxToolkit:ListSearchExtender>
                                                        </td>
                                                        <td style="cursor: pointer" align="left">
                                                            <asp:DropDownList ID="ddlCustomers0" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" Width="425px" ForeColor="Black">
                                                                <asp:ListItem Selected="True">All</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td align="left" style="cursor: pointer">
                                                            <asp:RadioButtonList ID="rblSort0" runat="server" AutoPostBack="True" Font-Names="Arial" Font-Size="8pt" OnSelectedIndexChanged="rblSort0_SelectedIndexChanged" Width="115px" Font-Bold="True" ForeColor="Black">
                                                                <asp:ListItem Selected="True" Value="Name">&nbsp;Sort By Name</asp:ListItem>
                                                                <asp:ListItem Value="Number">&nbsp;Sort By Number</asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="top">&nbsp;
                                        <asp:Label ID="Label65" runat="server" Font-Bold="False" Font-Size="9pt" ForeColor="Navy" Text="(Use CTRL for Multiple Selection) "></asp:Label>
                                                        </td>
                                                        <td valign="top" align="left">&nbsp;</td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="top">
                                                            <asp:LinkButton ID="lbSelectAll0" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbnSelectAllStockCode0_Click" ValidationGroup="nothing">Select All</asp:LinkButton>&nbsp;
                                        <asp:LinkButton ID="lbClearAll0" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbnClearStockCode0_Click" ValidationGroup="nothing">Clear All</asp:LinkButton>
                                                        </td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btnPreview0" runat="server" CssClass="btn btn-info" Text="Preview" OnClick="btnPreview0_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label61" runat="server" Text="Price Increase History" Font-Names="Arial" Font-Size="14pt" ForeColor="black"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center"></td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblErrorPrior" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="10pt" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table align="center" width="100%">
                                    <tr>
                                        <td align="center">
                                            <!-- *** Begin Header Table *** -->
                                            <div id="DivHeader2" style="vertical-align: top; text-align: center; width: 1030px">
                                                <asp:Table ID="HeaderTable2" runat="server"
                                                    CellPadding="2"
                                                    CellSpacing="0"
                                                    Font-Size="11pt"
                                                    ForeColor="White"
                                                    BackColor="#333333"
                                                    Font-Bold="False"
                                                    Width="1055px"
                                                    Visible="false">
                                                </asp:Table>
                                            </div>
                                            <!-- *** End Header Table *** -->
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <%--  <asp:Panel ID="pnlGridView2" runat="server" Height="300px" ScrollBars="Vertical" Visible="False">
                                                <div id="DivData2" class="Container" style="vertical-align: top; height: 300px; width: 100%;">
                                                    <asp:GridView ID="gvDatePriorToPriceChange" runat="server" AutoGenerateColumns="False" Font-Size="10pt"
                                                        GridLines="Vertical" Width="1005px" ShowHeader="False"
                                                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                        BorderWidth="1px" ForeColor="Black" OnRowDataBound="gvDatePriorToPriceChange_RowDataBound" AllowSorting="True" OnSorting="gvDatePriorToPriceChange_Sorting">
                                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="200px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cust#" SortExpression="Customer">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Width="80px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Stock<br/>Code" SortExpression="StockCode">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Width="70px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Description">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>' Width="200px" Font-Size="8pt"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unit<br/>Price" SortExpression="UnitPrice">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUnitCost" runat="server" Text='<%# Bind("UnitPrice") %>' Width="50px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Margin" SortExpression="Margin">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblMargin" runat="server" Text='<%# Bind("Margin") %>' Width="50px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Price<br/> Change<br/> Date" SortExpression="LastChangeDate">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLastPriceIncreaseDate" runat="server" Text='<%# Bind("LastChangeDate") %>' Width="80px"></asp:Label>
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
                                                </div>
                                            </asp:Panel>--%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="Table2" align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="imgExportExcel2" runat="server" ImageUrl="~/Images/ExportToExcel.GIF" OnClick="imgExportExcel2_Click" Visible="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgExportExcel1" />
            <asp:PostBackTrigger ControlID="imgExportExcel2" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

