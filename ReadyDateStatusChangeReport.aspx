<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ReadyDateStatusChangeReport.aspx.cs" Inherits="ReadyDateStatusChangeReport" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .rowColors tr:hover {
            background-color: #05b3f5 !important;
            color: black !important;
            transform-st !important;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
        <ContentTemplate>
            <div style="position: fixed; right: 50px; height: 40px; width: 150px; z-index: 10000;">
                <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="UpdatePanelPromo">
                    <ProgressTemplate>
                        <table style="border: medium solid #000080; width: 100%; background-color: white;">
                            <tr>
                                <td align="right" style="width: 12px;">
                                    <img src="Images/Loader.gif" alt="" />
                                </td>
                                <td><span style="color: #ffffff"><span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing  <span class="">....</span> </strong></span></span></td>
                            </tr>
                        </table>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div>
                <table id="tblPopUpInventoryShortageReport" style="border: 1px solid black; font-size: 7pt; height: 350px; background-color: #FFFFFF;" width="100%" align="center">
                    <tr>
                        <td align="center">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Italic="False" Font-Size="16pt" ForeColor="Navy">Ready Date Status Change Report</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table style="width: 600px">
                                <tr>
                                    <td align="left" style="width: 200px">
                                        <asp:Label ID="lblPeriod" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="Period:"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="10pt" ForeColor="Black" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged">
                                            <asp:ListItem Selected="True">ALL</asp:ListItem>
                                            <asp:ListItem>Range</asp:ListItem>
                                            <asp:ListItem>Last 3 Months</asp:ListItem>
                                            <asp:ListItem>Last 6 Months</asp:ListItem>
                                            <asp:ListItem>Last 9 Months</asp:ListItem>
                                            <asp:ListItem>Last 12 Months</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left" style="width: 5px">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblStartDate" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="Ready Date From:"></asp:Label><br />
                                    </td>
                                    <td align="right">
                                        <asp:TextBox ID="txtStartDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup=""></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgStartDate" TargetControlID="txtStartDate"></ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="txtStartDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtStartDate"></ajaxToolkit:MaskedEditExtender>
                                        <ajaxToolkit:MaskedEditValidator ID="txtStartDateMEV" runat="server" ControlExtender="txtStartDateMEE" ControlToValidate="txtStartDate" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtStartDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtStartDateMEV" />
                                    </td>
                                    <td align="left" style="width: 5px">
                                        <asp:ImageButton ID="imgStartDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" class="auto-style1">
                                        <asp:Label ID="lblEndDate" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="Ready Date To:"></asp:Label><br />
                                    </td>
                                    <td align="right" class="auto-style1">
                                        <asp:TextBox ID="txtEndDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup=""></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgEndDate" TargetControlID="txtEndDate"></ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="txtEndDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtEndDate"></ajaxToolkit:MaskedEditExtender>
                                        <ajaxToolkit:MaskedEditValidator ID="txtEndDateMEV" runat="server" ControlExtender="txtEndDateMEE" ControlToValidate="txtEndDate" Display="None" EmptyValueMessage="Please enter an end date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtEndDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtEndDateMEV" />
                                    </td>
                                    <td align="left" class="auto-style2">
                                        <asp:ImageButton ID="imgEndDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:CheckBox ID="chkSHowAllOpenOrders" runat="server" Checked="True" Font-Size="11pt" ForeColor="Black" Text="&amp;nbsp;Show Open Orders Only" />
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="chkTBD" runat="server" Font-Size="11pt" ForeColor="Black" Text="&amp;nbsp;Show Changed to TBD ONLY" AutoPostBack="True" OnCheckedChanged="chkTBD_CheckedChanged" />
                                        <br />
                                        <asp:CheckBox ID="chkCurrentTBDs" runat="server" Font-Size="11pt" ForeColor="Black" Text="&amp;nbsp;Show Current TBDs" AutoPostBack="True" OnCheckedChanged="chkCurrentTBDs_CheckedChanged" />
                                        <br />
                                        <asp:CheckBox ID="chkShowSentAlerts" runat="server" Font-Size="11pt" ForeColor="Black" Text="&amp;nbsp;Show Sent Alerts" Checked="True" />

                                    </td>
                                    <td align="left"></td>
                                </tr>
                                <tr>
                                    <td align="center">&nbsp;</td>
                                    <td align="center">&nbsp;</td>
                                    <td align="left">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left">&nbsp;<asp:Label ID="lblStockCodeDesc" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Navy"></asp:Label>
                                    </td>
                                    <td align="center">&nbsp;</td>
                                    <td align="left" style="width: 5px">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label72" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="FROM Stock Code"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:TextBox ID="txtStockCodeFrom" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="200px">000000</asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="txtStockCodeFrom_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes" ServicePath="" TargetControlID="txtStockCodeFrom" UseContextKey="True">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label73" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="TO Stock Code"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtStockCodeTo" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="200px">999999</asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="txtStockCodeTo_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes" ServicePath="" TargetControlID="txtStockCodeTo" UseContextKey="True">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="center">
                                        <asp:LinkButton ID="btnRunReport" runat="server" OnClick="btnRunReport_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Click to run report"><i class="fa fa-file-text-o" ></i>&nbsp;Run Report</asp:LinkButton>
                                        <br />
                                        <asp:LinkButton ID="imgExportExcel" runat="server" CssClass="btn btn-success" ForeColor="White" OnClick="imgExportExcel_Click" Width="200px"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>

                                        <asp:LinkButton ID="btnReset" runat="server" CssClass="btn btn-danger" ForeColor="White" OnClick="btnReset_Click" ToolTip="Click to reset form" Width="200px"><i class="fa fa-refresh"></i>&nbsp;Reset Form</asp:LinkButton>

                                    </td>
                                    <td align="left" style="width: 5px">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblRecordCount" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="13pt" ForeColor="Navy"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr class="rowColors">
                        <td>
                            <div style="position: relative">
                                <asp:GridView ID="gvReadyStatusDateChanged" runat="server"
                                    AllowSorting="True"
                                    AutoGenerateColumns="False"
                                    CellPadding="4" Font-Size="9pt"
                                    ForeColor="#333333" GridLines="None"
                                    HorizontalAlign="Center"
                                    OnRowDataBound="gvReadyStatusDateChanged_RowDataBound"
                                    OnSorting="gvReadyStatusDateChanged_Sorting"
                                    PageSize="15" ShowFooter="True" Width="1200px">
                                    <FooterStyle BackColor="Silver" />
                                    <HeaderStyle BackColor="#CCCCCC" Font-Size="8pt" VerticalAlign="Top" ForeColor="Black" />
                                    <RowStyle Height="30px" />
                                    <AlternatingRowStyle Height="30px" />
                                    <EmptyDataRowStyle Font-Bold="true" ForeColor="Red" HorizontalAlign="Center" Font-Size="14pt" />
                                    <EmptyDataTemplate>
                                        No Records found matching the selected criteria
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="No" SortExpression="ID">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Font-Size="9pt" Text='<%# Bind("ID") %>' Width="30px" Font-Bold="True" ToolTip='<%# Bind("DateAdded") %>' Style="cursor: pointer"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sales Order #" SortExpression="SalesOrder">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesOrder" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("SalesOrder") %>' Width="75px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Date" SortExpression="OrderDate">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderDate" runat="server" Text='<%# Bind("OrderDate") %>' Font-Bold="false" Font-Size="9" Width="78px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Status" SortExpression="OrderStatus">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderStatus" runat="server" Text='<%# Bind("OrderStatus") %>' Font-Bold="false" Font-Size="9" Width="115px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stock Code" SortExpression="StockCode">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Font-Bold="True" Font-Size="9" Width="75px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shortage" SortExpression="Shortage">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblShortage" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("Shortage") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shortage Amount" SortExpression="ShortageAmount">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblShortageAmount" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("ShortageAmount") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" SortExpression="JobDescription">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobDescription" runat="server" Text='<%# Bind("JobDescription") %>' Font-Bold="True" Font-Size="8"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Salesperson" SortExpression="SalesPerson">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesPerson" runat="server" Text='<%# Bind("SalesPerson") %>' Font-Bold="false" Font-Size="8pt" Width="125px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer" SortExpression="Customer">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Font-Bold="false" Font-Size="9" Width="195px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CustID" SortExpression="CustomerID">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("CustomerID") %>' Font-Bold="false" Font-Size="9" Width="54px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ready Date Or Status Old" SortExpression="ReadyDateOrStatusOld">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblReadyDateOrStatusOld" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("ReadyDateOrStatusOld") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ready Date Or Status New" SortExpression="ReadyDateOrStatusNew">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblReadyDateOrStatusNew" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("ReadyDateOrStatusNew") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BackColor="Pink" HorizontalAlign="center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sent Alert" SortExpression="SentAlert">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSentAlert" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("SentAlert") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BackColor="Pink" HorizontalAlign="center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Label ID="lblError" runat="server" Font-Size="11pt" ForeColor="Red" Font-Bold="True"></asp:Label>
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

