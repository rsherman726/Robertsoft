<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CustomerPurchasingBehavior.aspx.cs" Inherits="CustomerPurchasingBehavior" %>

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
            <div>
                <div>
                    <table align="center" width="900">

                        <tr>
                            <td align="center">
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Purchasing Behavior Reports" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="background-color: lightblue;" align="center" class="JustRoundedEdgeBothSmall">
                                <asp:Panel ID="pnlSearchCriteria" runat="server" Width="1100px">
                                    <table width="1050">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td style="cursor: pointer" align="left">&nbsp;</td>
                                                        <td style="cursor: pointer" align="left">&nbsp;</td>
                                                        <td align="left" style="cursor: pointer">
                                                            <asp:Label ID="Label12" runat="server" Font-Bold="True" ForeColor="Black" Text="Date(s)"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="left">

                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label67" runat="server" Font-Bold="True" Font-Italic="True" ForeColor="Black" Text="Reports:"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:RadioButtonList ID="rblReport" runat="server" ForeColor="Black">
                                                                            <asp:ListItem Selected="True">&nbsp;Purchasing Alert</asp:ListItem>
                                                                            <asp:ListItem>&nbsp;<font color='#660066'>Last 24 Months Sales Trends</font></asp:ListItem>
                                                                        </asp:RadioButtonList></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" ForeColor="#660066" Text="Threshold % for Last 24 months"></asp:Label></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlThreshold" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="Black">
                                                                            <asp:ListItem>5</asp:ListItem>
                                                                            <asp:ListItem>10</asp:ListItem>
                                                                            <asp:ListItem>15</asp:ListItem>
                                                                            <asp:ListItem>20</asp:ListItem>
                                                                            <asp:ListItem>25</asp:ListItem>
                                                                            <asp:ListItem>30</asp:ListItem>
                                                                        </asp:DropDownList></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <asp:RadioButtonList ID="rblIncreaseDecrease" runat="server" Font-Bold="True" Font-Names="Arial" ForeColor="#660066" RepeatDirection="Horizontal" Width="225px">
                                                                            <asp:ListItem Value="0" Selected="True">&nbsp;Below</asp:ListItem>
                                                                            <asp:ListItem Value="1">&nbsp;Above</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                            </table>

                                                            <br />




                                                        </td>
                                                        <td valign="top" align="left">&nbsp;</td>
                                                        <td align="left" valign="top">
                                                            <table style="width: 550px">
                                                                <tr>
                                                                    <td style="width: 100px">
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
                                                                    <td align="left">&nbsp;</td>
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
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top">
                                                            <asp:Label ID="Label13" runat="server" Text="Stock Code" Font-Bold="True" ForeColor="Black"></asp:Label>&nbsp;
                                                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Red" Text="Leave blank for all stock codes..."></asp:Label>
                                                        </td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                        <td align="left" valign="top">
                                                            <asp:Label ID="label44" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="10pt" ForeColor="Red">**Year Selection Ignores Dates</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top">
                                                            <asp:Label ID="lblStockCodeDesc" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Navy"></asp:Label>
                                                        </td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top">
                                                            <asp:TextBox ID="txtStockCode" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtStockCode_TextChanged" ValidationGroup=""></asp:TextBox>
                                                            <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes" ServicePath="" TargetControlID="txtStockCode" UseContextKey="True">
                                                            </ajaxToolkit:AutoCompleteExtender>
                                                        </td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                        <td align="left" valign="top">
                                                            <table>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="Label14" runat="server" Font-Bold="True" ForeColor="Black" Text="Customer:"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 350px">
                                                                        <asp:DropDownList ID="ddlCustomers" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black">
                                                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                        <asp:RadioButtonList ID="rblSort" runat="server" AutoPostBack="True" Font-Bold="True" Font-Names="Arial" Font-Size="8pt" ForeColor="Black" OnSelectedIndexChanged="rblSort_SelectedIndexChanged" Width="115px">
                                                                            <asp:ListItem Selected="True" Value="Name">&nbsp;Sort By Name</asp:ListItem>
                                                                            <asp:ListItem Value="Number">&nbsp;Sort By Number</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top">
                                                            <asp:LinkButton ID="lbnTrendsReport" runat="server" Font-Underline="True" ForeColor="#333333" OnClick="lbnTrendsReport_Click">Purchasing Trends Report</asp:LinkButton>&nbsp;
                                                            <asp:Label ID="label66" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="False" Font-Size="8pt" ForeColor="Red">**Run after any Report - Uses date filters</asp:Label>
                                                        </td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                        <td align="right" valign="top">&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="btnPreview" runat="server" OnClick="btnPreview_Click" CssClass="btn btn-info"  Width="200px" ForeColor="White" ToolTip="Click to run Full report"><i class="fa fa-file-text-o" ></i>&nbsp;Full Report</asp:LinkButton> 

                                                <asp:LinkButton ID="imgExportExcel" runat="server" CssClass="btn btn-success" ForeColor="White" OnClick="imgExportExcel_Click" Width="200px"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>

                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblError" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="10pt" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr class="rowColors">
                            <td align="center">
                                <asp:GridView ID="gvPurchasingTrends" runat="server" AllowSorting="True" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical" OnRowDataBound="gvPurchasingTrends_RowDataBound" OnSorting="gvPurchasingTrends_Sorting">
                                    <AlternatingRowStyle BackColor="#DCDCDC" />
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                    <SortedAscendingHeaderStyle BackColor="#0000A9" />
                                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                    <SortedDescendingHeaderStyle BackColor="#000065" />
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table id="Table1" align="center">
                                    <tr>
                                        <td align="center">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                    </table>

                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgExportExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

