<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="InventoryShortageReport.aspx.cs" Inherits="InventoryShortageReport" MaintainScrollPositionOnPostback="true" %>

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
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Italic="False" Font-Size="16pt" ForeColor="Navy">Inventory Shortage Report</asp:Label>
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
                                            <asp:ListItem Selected="True">Range</asp:ListItem>
                                            <asp:ListItem>Single</asp:ListItem>
                                            <asp:ListItem>Up To Date</asp:ListItem>
                                            <asp:ListItem>ALL</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left" style="width: 5px">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblStartDate" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="Shipping Date From:"></asp:Label><br />
                                    </td>
                                    <td align="right">
                                        <asp:TextBox ID="txtStartDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtStartDate_TextChanged" ValidationGroup=""></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgStartDate" TargetControlID="txtStartDate">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="txtStartDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtStartDate">
                                        </ajaxToolkit:MaskedEditExtender>
                                        <ajaxToolkit:MaskedEditValidator ID="txtStartDateMEV" runat="server" ControlExtender="txtStartDateMEE" ControlToValidate="txtStartDate" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtStartDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtStartDateMEV" />
                                    </td>
                                    <td align="left" style="width: 5px">
                                        <asp:ImageButton ID="imgStartDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblEndDate" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="Shipping Date To:"></asp:Label><br />
                                    </td>
                                    <td align="right">
                                        <asp:TextBox ID="txtEndDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtEndDate_TextChanged" ValidationGroup=""></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgEndDate" TargetControlID="txtEndDate">
                                        </ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="txtEndDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtEndDate">
                                        </ajaxToolkit:MaskedEditExtender>
                                        <ajaxToolkit:MaskedEditValidator ID="txtEndDateMEV" runat="server" ControlExtender="txtEndDateMEE" ControlToValidate="txtEndDate" Display="None" EmptyValueMessage="Please enter an end date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtEndDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtEndDateMEV" />
                                    </td>
                                    <td align="left" style="width: 5px">
                                        <asp:ImageButton ID="imgEndDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">&nbsp;</td>
                                    <td align="center">&nbsp;</td>
                                    <td align="left" style="width: 5px">&nbsp;</td>
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

                                        <table width="500px">
                                            <tr>
                                                <td align="right">
                                                    
                                                    <asp:LinkButton ID="btnRunReport" runat="server" OnClick="btnRunReport_Click" CssClass="btn btn-info"  Width="200px" ForeColor="White" ToolTip="Click to run full report"><i class="fa fa-file-text-o" ></i>&nbsp;Run Full Report</asp:LinkButton>

                                                </td>
                                                <td align="left">
                                                     <asp:LinkButton ID="btnRunReportSummary" runat="server" OnClick="btnRunReportSummary_Click" CssClass="btn btn-info"  Width="200px" ForeColor="White" ToolTip="Click to run summary report"><i class="fa fa-file-text-o" ></i>&nbsp;Run Summary Report</asp:LinkButton>                                                    
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkRunBoth" runat="server" Checked="True" ForeColor="Black" Text="&amp;nbsp;Both" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <asp:LinkButton ID="imgExportExcelShortage" runat="server" OnClick="imgExportExcelShortage_Click" CssClass="btn btn-success" ForeColor="White" Width="200px"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>
                                                     
                                                </td>
                                                <td align="left">
                                                    <asp:LinkButton ID="imgExportExcelShortageSummary" runat="server" OnClick="imgExportExcelShortageSummary_Click" CssClass="btn btn-success" ForeColor="White" Width="200px"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>
                                                     
                                                </td>
                                                <td>&nbsp;</td>
                                            </tr>
                                        </table>
                                        <br />
                                        <br />
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
                    <tr class="rowColors">
                        <td>
                            <div style="position: relative">
                                <asp:GridView ID="gvInventoryShortageReport" runat="server" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" Font-Size="9pt" ForeColor="#333333" GridLines="None" HorizontalAlign="Center" 
                                    OnRowDataBound="gvInventoryShortageReport_RowDataBound" 
                                    OnSorting="gvInventoryShortageReport_Sorting" 
                                    OnRowCancelingEdit="gvInventoryShortageReport_RowCancelingEdit" 
                                    OnRowCommand="gvInventoryShortageReport_RowCommand" 
                                    OnRowEditing="gvInventoryShortageReport_RowEditing" 
                                    OnRowUpdating="gvInventoryShortageReport_RowUpdating"
                                    PageSize="15" ShowFooter="True" Width="1200px">
                                    <FooterStyle BackColor="Silver" />
                                    <HeaderStyle BackColor="#CCCCCC" Font-Size="8pt" VerticalAlign="Top" ForeColor="Black" />
                                    <Columns>
                                         <asp:TemplateField HeaderText="No" SortExpression="ID">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblID" runat="server" Font-Size="9pt" Text='<%# Bind("ID") %>' Width="30px" Font-Bold="True"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Font-Bold="True" Font-Size="9" ForeColor="Black" Text='<%# Bind("Name") %>' Width="235px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CustID" SortExpression="Customer">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerID" runat="server" Font-Bold="true" Font-Size="9" ForeColor="Black" Text='<%# Bind("Customer") %>' Width="54px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sales Order #" SortExpression="SalesOrder">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesOrder" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("SalesOrder") %>'></asp:Label>
                                                <asp:Panel ID="pnlNotes0" runat="server" BackColor="white" Visible="true" Width="250px" BorderStyle="Solid" BorderWidth="1" BorderColor="Black">
                                                    <div style="position: relative">
                                                        <asp:Label ID="lblSalesOrderNotes" runat="server" Text='<%# Bind("SalesOrderNotes") %>' ForeColor="Black" Font-Bold="True" Width="200px"></asp:Label>
                                                    </div>
                                                </asp:Panel>
                                                <ajaxToolkit:HoverMenuExtender ID="hmeNotes0" runat="server" PopupControlID="pnlNotes0" PopupPosition="left" TargetControlID="lblSalesOrder">
                                                </ajaxToolkit:HoverMenuExtender>
                                            </ItemTemplate>
                                            <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Order Date" SortExpression="OrderDate">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderDate" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("OrderDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />                                            
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="StockCode" SortExpression="MStockCode">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblStockCode" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("MStockCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotal" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="black" Text="Total:"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Qty" SortExpression="MBackOrderQty">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblQty" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("MOrderQty") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblQtyTotal" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shortage" SortExpression="Shortage">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblShortage" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("Shortage") %>'></asp:Label>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:Label ID="lblShortageTotal" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D"></asp:Label>
                                            </FooterTemplate>
                                            <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer<br>Delivery<br>Date" SortExpression="CustomerDeliveryDate">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerDeliveryDate" runat="server" Text='<%# Bind("CustomerDeliveryDate") %>' CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shipping Date" SortExpression="MLineShipDate">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipDate" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("MLineShipDate") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<img src='images/information.png'/>">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Image ID="imgNotes" runat="server" ImageUrl="Images/information.png" Style="position: relative" />
                                                <asp:Panel ID="pnlNotes" runat="server" BackColor="GhostWhite" Visible="true">
                                                    <div style="position: relative">
                                                        <asp:GridView ID="gvProductionSchedule" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvProductionSchedule_RowDataBound" Width="350px"
                                                            Font-Size="12" BorderStyle="Solid" BorderWidth="1" BorderColor="Black" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White">
                                                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                            <EmptyDataTemplate>
                                                                <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Red" Text="No Records Found"></asp:Label>
                                                            </EmptyDataTemplate>
                                                            <EmptyDataRowStyle HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Scheduled Date">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblScheduledDate" runat="server" Text='<%# Bind("ScheduledDate") %>' Font-Bold="True" ForeColor="Black"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Scheduled Qty">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblScheduledQty" runat="server" Text='<%# Bind("ScheduledQty") %>' ForeColor="Black" Font-Bold="True" Width="50px"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle BackColor="#666666" Font-Names="Arial" ForeColor="White" Height="25px" HorizontalAlign="Center" BorderWidth="1" BorderStyle="Solid" VerticalAlign="Bottom" />
                                                            <FooterStyle BackColor="#666666" ForeColor="White" />
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                                <ajaxToolkit:HoverMenuExtender ID="hmeNotes" runat="server" PopupControlID="pnlNotes" PopupPosition="left" TargetControlID="imgNotes">
                                                </ajaxToolkit:HoverMenuExtender>
                                            </ItemTemplate>
                                            <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Notes">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtNotes" runat="server" Text='<%# Bind("Notes") %>' ForeColor="Black" Font-Bold="True" Width="200px" CssClass="form-control input-sm" BackColor="LemonChiffon"></asp:TextBox>
                                                <asp:CheckBox ID="chkDuplicate" runat="server" Text="&nbsp;Duplicate" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Image ID="imgNotes2" runat="server" ImageUrl="Images/pencil.png" Style="position: relative" />
                                                <asp:Panel ID="pnlNotes2" runat="server" BackColor="white" Visible="true" Width="250px" BorderStyle="Solid" BorderWidth="1" BorderColor="Black">
                                                    <div style="position: relative">
                                                        <asp:Label ID="lblNotes" runat="server" Text='<%# Bind("Notes") %>' ForeColor="Black" Font-Bold="True" Width="200px"></asp:Label>
                                                    </div>
                                                </asp:Panel>
                                                <ajaxToolkit:HoverMenuExtender ID="hmeNotes2" runat="server" PopupControlID="pnlNotes2" PopupPosition="left" TargetControlID="imgNotes2">
                                                </ajaxToolkit:HoverMenuExtender>
                                            </ItemTemplate>
                                            <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                         <asp:TemplateField ShowHeader="False">
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="lbnUpdate" runat="server" CausesValidation="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Update"
                                                    Font-Bold="false" Font-Size="9pt" Text="Update" ForeColor="Navy"></asp:LinkButton>
                                                <ajaxToolkit:ConfirmButtonExtender ID="lbnUpdate_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True"
                                                    ConfirmText="Are you sure you want to update this record?" Enabled="True" TargetControlID="lbnUpdate">
                                                </ajaxToolkit:ConfirmButtonExtender>
                                                <br />
                                                <asp:LinkButton ID="lbnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Font-Bold="false" Font-Size="9pt" Text="Cancel" ForeColor="Navy"></asp:LinkButton>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbnEdit" runat="server" CausesValidation="False" CommandName="Edit" Font-Bold="false" Font-Size="9pt" Text="Edit" ForeColor="Navy"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblRecordCount0" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="13pt" ForeColor="Navy"></asp:Label>
                        </td>
                    </tr>
                    <tr class="rowColors">
                        <td align="center">
                            <asp:GridView ID="gvInventoryShortageReportSummary" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                CellPadding="4" Font-Size="9pt" ForeColor="#333333" GridLines="None" HorizontalAlign="Center"
                                OnRowDataBound="gvInventoryShortageReportSummary_RowDataBound"
                                OnSorting="gvInventoryShortageReportSummary_Sorting" PageSize="15"
                                ShowFooter="True" Width="700px">
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Size="8pt" ForeColor="White" VerticalAlign="Top" Font-Bold="True" />
                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                <RowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderText="StockCode" SortExpression="MStockCode">
                                        <HeaderStyle CssClass="CenterAligner" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblStockCode" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("MStockCode") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotal" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="black" Text="Total:"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description" SortExpression="Description">
                                        <HeaderStyle CssClass="CenterAligner" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("Description") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Shortage" SortExpression="Shortage">
                                        <HeaderStyle CssClass="CenterAligner" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblShortage" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("Shortage") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblShortageTotal" runat="server" CssClass="NoUnderline" Font-Bold="true" ForeColor="#00569D"></asp:Label>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                        <FooterStyle HorizontalAlign="center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<img src='images/information.png'/>">
                                        <HeaderStyle CssClass="CenterAligner" />
                                        <ItemTemplate>
                                            <asp:Image ID="imgNotes" runat="server" ImageUrl="Images/information.png" Style="position: relative" />
                                            <asp:Panel ID="pnlNotes" runat="server" BackColor="GhostWhite" Visible="true">
                                                <div style="position: relative">
                                                    <asp:GridView ID="gvProductionSchedule" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvProductionSchedule_RowDataBound" Width="350px"
                                                        Font-Size="12" BorderStyle="Solid" BorderWidth="1" BorderColor="Black" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White">
                                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                        <EmptyDataTemplate>
                                                            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Red" Text="No Records Found"></asp:Label>
                                                        </EmptyDataTemplate>
                                                        <EmptyDataRowStyle HorizontalAlign="Center" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Scheduled Date">
                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblScheduledDate" runat="server" Text='<%# Bind("ScheduledDate") %>' Font-Bold="True" ForeColor="Black"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Scheduled Qty">
                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblScheduledQty" runat="server" Text='<%# Bind("ScheduledQty") %>' ForeColor="Black" Font-Bold="True" Width="50px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle BackColor="#666666" Font-Names="Arial" ForeColor="White" Height="25px" HorizontalAlign="Center" BorderWidth="1" BorderStyle="Solid" VerticalAlign="Bottom" />
                                                        <FooterStyle BackColor="#666666" ForeColor="White" />
                                                    </asp:GridView>
                                                </div>
                                            </asp:Panel>
                                            <ajaxToolkit:HoverMenuExtender ID="hmeNotes" runat="server" PopupControlID="pnlNotes" PopupPosition="left" TargetControlID="imgNotes">
                                            </ajaxToolkit:HoverMenuExtender>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </asp:GridView>
                        </td>
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
            <asp:PostBackTrigger ControlID="imgExportExcelShortage" />
            <asp:PostBackTrigger ControlID="imgExportExcelShortageSummary" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

