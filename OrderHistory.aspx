<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="OrderHistory.aspx.cs" Inherits="OrderHistory" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .rowColors tr:hover {
            background-color: #05b3f5 !important;
            color: black !important;
            transform-st !important;
        }
        /* The Close Button */
        .close {
            color: white;
            float: right;
            font-size: 28px;
            font-weight: bold;
            padding-right: 10px;
            margin-right: 10px;
        }

            .close:hover,
            .close:focus {
                color: #ff0000;
                text-decoration: none;
                cursor: pointer;
            }
    </style>
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" integrity="sha384-mzrmE5qonljUremFsqc01SB46JvROS7bZs3IO2EmfFsd15uHvIt+Y8vEf7N7fWAU" crossorigin="anonymous">
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
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Italic="False" Font-Size="16pt" ForeColor="Navy">Order History (Invoiced) </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table style="width: 700px">
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
                                    <td align="left">
                                        <asp:Label ID="lblEndDate" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="Shipping Date To:"></asp:Label><br />
                                    </td>
                                    <td align="right">
                                        <asp:TextBox ID="txtEndDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtEndDate_TextChanged" ValidationGroup=""></asp:TextBox>
                                        <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgEndDate" TargetControlID="txtEndDate"></ajaxToolkit:CalendarExtender>
                                        <ajaxToolkit:MaskedEditExtender ID="txtEndDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtEndDate"></ajaxToolkit:MaskedEditExtender>
                                        <ajaxToolkit:MaskedEditValidator ID="txtEndDateMEV" runat="server" ControlExtender="txtEndDateMEE" ControlToValidate="txtEndDate" Display="None" EmptyValueMessage="Please enter an end date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtEndDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtEndDateMEV" />
                                    </td>
                                    <td align="left" style="width: 5px">
                                        <asp:ImageButton ID="imgEndDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label14" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="Customer:"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:DropDownList ID="ddlCustomers" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black" OnSelectedIndexChanged="ddlCustomers_SelectedIndexChanged">
                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td align="left" style="width: 5px">
                                        <asp:RadioButtonList ID="rblSort" runat="server" AutoPostBack="True" Font-Bold="True" Font-Names="Arial" Font-Size="8pt" ForeColor="Black" OnSelectedIndexChanged="rblSort_SelectedIndexChanged" Width="115px">
                                            <asp:ListItem Selected="True" Value="Name">&nbsp;Sort By Name</asp:ListItem>
                                            <asp:ListItem Value="Number">&nbsp;Sort By Number</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label74" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="Sales Order#:"></asp:Label>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtSaleOrder" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" ForeColor="Black" placeholder="Enter Sales Order # with or without leading zeros"></asp:TextBox>
                                        <ajaxToolkit:AutoCompleteExtender ID="txtSaleOrder_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes" ServicePath="" TargetControlID="txtSaleOrder" UseContextKey="True">
                                        </ajaxToolkit:AutoCompleteExtender>
                                    </td>
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
                                                    <asp:TextBox ID="txtStockCodeFrom" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="200px" placeholder="Stock Code From"></asp:TextBox>
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
                                                    <asp:TextBox ID="txtStockCodeTo" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="200px" placeholder="Stock Code To"></asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender ID="txtStockCodeTo_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes" ServicePath="" TargetControlID="txtStockCodeTo" UseContextKey="True">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="center">
                                        <asp:LinkButton ID="btnRunReport" runat="server" CssClass="btn btn-info" OnClick="btnRunReport_Click" Width="200px" ForeColor="White" ToolTip="Click to run report"><i class="fa fa-file-text-o"></i>&nbsp;Run Report</asp:LinkButton>
                                        <br />
                                        <asp:LinkButton ID="lbnExportExcel" runat="server" CssClass="btn btn-success" OnClick="lbnExportExcel_Click" Width="200px" ForeColor="White" ToolTip="Click to Export to Excel"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>
                                        <br />
                                        <asp:LinkButton ID="btnReset" runat="server" CssClass="btn btn-danger" OnClick="btnReset_Click" Width="200px" ForeColor="White" ToolTip="Click to reset form"><i class="fa fa-refresh" ></i>&nbsp;Reset Form</asp:LinkButton>
                                    </td>
                                    <td align="left" style="width: 5px">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="20pt" ForeColor="Red"></asp:Label>
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
                                <asp:Panel ID="pnlReport" runat="server" ScrollBars="Vertical" Height="600px">
                                    <asp:GridView ID="gvOrderHistory" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                        CellPadding="4" Font-Size="9pt" ForeColor="#333333" GridLines="None" HorizontalAlign="Center"
                                        OnRowDataBound="gvOrderHistory_RowDataBound" OnSorting="gvOrderHistory_Sorting" PageSize="15" ShowFooter="false" Width="1250px" OnRowCommand="gvOrderHistory_RowCommand">
                                        <FooterStyle BackColor="Silver" />
                                        <HeaderStyle BackColor="#CCCCCC" Font-Size="11pt" VerticalAlign="Top" ForeColor="Black" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="No" SortExpression="ID">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Font-Size="9pt" Text='<%# Bind("ID") %>' Width="30px" Font-Bold="True"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="30px" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Name" SortExpression="Name">                                               
                                                <ItemTemplate>
                                                    <asp:Label ID="lblName" runat="server" Font-Bold="True" Font-Size="9" ForeColor="Black" Text='<%# Bind("Name") %>' Width="235px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CustID" SortExpression="Customer">                                                 
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCustomerID" runat="server" Font-Bold="true" Font-Size="9" ForeColor="Black" Text='<%# Bind("Customer") %>' Width="54px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Customer PO" SortExpression="CustomerPoNumber">                                                
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPurchaseOrder" runat="server" Text='<%# Bind("CustomerPoNumber") %>' Font-Size="8pt" Width="155px" ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="left" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Sales Order #" SortExpression="SalesOrder">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSalesOrder" runat="server" Font-Bold="true" ForeColor="#00569D" Text='<%# Bind("SalesOrder") %>'></asp:Label>
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

                                            <asp:TemplateField HeaderText="Documents">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbnInvoice" runat="server" CommandName="ShowInvoice" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'> <i class="fa fa-file-pdf" style="color:red;font-size:12pt"></i>&nbsp;<i class="glyphicon glyphicon-picture"style="color:blue;font-size:12pt"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="S.O. Notes">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblNotes" runat="server" Style="position: relative; color: black; font-size: 12pt"><i class="fa fa-file-text-o"></i></asp:Label>
                                                    <asp:Panel ID="pnlNotes0" runat="server" BackColor="white" Visible="true" Width="950px" BorderStyle="Solid" BorderWidth="1" BorderColor="Black">
                                                        <div style="position: relative">
                                                            <asp:Label ID="lblSalesOrderNotes" runat="server" Text='<%# Bind("SalesOrderNotes") %>' ForeColor="Black" Font-Bold="True" Width="900px" Style="text-align: left"></asp:Label>
                                                        </div>
                                                    </asp:Panel>
                                                    <ajaxToolkit:HoverMenuExtender ID="hmeNotes0" runat="server" PopupControlID="pnlNotes0" PopupPosition="left" TargetControlID="lblNotes">
                                                    </ajaxToolkit:HoverMenuExtender>
                                                </ItemTemplate>
                                                <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">&nbsp;</td>
                    </tr>
                    <tr>
                        <td align="left">&nbsp;</td>
                    </tr>
                </table>
            </div>
            <div class="clearfix">
                <asp:Button ID="Button1" runat="server" Style="display: none" Text="Button" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderPopUp" runat="server" BackgroundCssClass="popup_background"
                    DynamicServicePath="" Enabled="True" PopupControlID="pnlPopUp" TargetControlID="Button1">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlPopUp" runat="server" CssClass="modalPopup2" Visible="true" Style="display: block; padding: 10px" ScrollBars="Vertical" Height="600px" Width="980px">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div style="position: fixed; right: 50px; height: 40px; width: 150px; z-index: 10000;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="UpdatePanel1">
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
                            <table id="tblPopUp" style="border: 1px solid black; font-size: 7pt; background-color: #FFFFFF;" width="950px" align="center">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Italic="False" Font-Size="16pt" ForeColor="Navy">Sales Order#&nbsp;</asp:Label>
                                        <asp:Label ID="lblSalesOrderPopup" runat="server" Font-Bold="True" Font-Italic="False" Font-Size="16pt" ForeColor="Navy"></asp:Label>
                                        <asp:LinkButton ID="lbnClose" runat="server" OnClick="btnClose_Click" CssClass="close" ForeColor="Red">&times</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:GridView ID="gvPDFs"
                                            runat="server"
                                            AutoGenerateColumns="False"
                                            BackColor="GhostWhite"
                                            Font-Size="8pt"
                                            ForeColor="Black"
                                            OnRowCommand="gvPDFs_RowCommand"
                                            OnRowDataBound="gvPDFs_RowDataBound"
                                            OnRowDeleting="gvPDFs_RowDeleting"
                                            Width="900px">
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Red" Text="No Records Found"></asp:Label>
                                            </EmptyDataTemplate>
                                            <EmptyDataRowStyle HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField ShowHeader="False">
                                                    <HeaderStyle CssClass="CenterAligner" Width="50px" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbnDeletePDF" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delete" ToolTip="Click to delete this file"><i class="glyphicon glyphicon-trash" style="color:purple;font-size:16pt"></i></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnDeletePDF_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True"
                                                            ConfirmText="Are you sure you want to delete this file?" Enabled="True" TargetControlID="lbnDeletePDF"></ajaxToolkit:ConfirmButtonExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="ID">
                                                    <HeaderStyle CssClass="CenterAligner" Width="40px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbnFileName" runat="server" Text='<%# Bind("FileName") %>' Visible="false"></asp:LinkButton>
                                                        <asp:Label ID="lblFullPath" runat="server" Text='<%# Bind("FullPath") %>' Visible="false"></asp:Label>
                                                        <asp:LinkButton ID="imgDownLoad" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                            CommandName="Select" ToolTip='<%# Bind("FileName") %>'><i class="glyphicon glyphicon-floppy-save" style="color:blue;font-size:16pt"></i></asp:LinkButton>
                                                        <asp:Label ID="lblExtension" runat="server" Text='<%# Bind("Extension") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Creation Date">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDateCreated" runat="server" Text='<%# Bind("DateAdded") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Document Name">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDocName" runat="server" Text='<%# Bind("DocName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="View Document">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlView" runat="server"><i class="glyphicon glyphicon-eye-open" style="color:red;font-size:16pt"></i></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                    <HeaderStyle CssClass="CenterAligner" BackColor="" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center" VerticalAlign="Middle" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle BackColor="GhostWhite" CssClass="CenterAligner" Font-Names="Arial" Font-Size="10pt" VerticalAlign="Middle" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:LinkButton ID="btnDeleteAll" runat="server" CssClass="btn btn-danger btn-sm" Font-Bold="False" OnClick="btnDeleteAll_Click" Visible="False" ToolTip="Select the files you want to delete then click here"><i class="glyphicon glyphicon-trash" style="font-size:12pt"></i>&nbsp;Delete All Checked</asp:LinkButton>
                                        <ajaxToolkit:ConfirmButtonExtender ID="btnDeleteAll_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Delete All Selected?" Enabled="True" TargetControlID="btnDeleteAll"></ajaxToolkit:ConfirmButtonExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Red">SELECT THE DOCUMENT FOLDER</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="padding: 10px; margin: 10px">
                                        <asp:RadioButtonList ID="rblFolderName" runat="server" Font-Bold="False" Width="900px" ForeColor="Black" Font-Size="11pt" AutoPostBack="True"
                                            OnSelectedIndexChanged="rblFolderName_SelectedIndexChanged" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="POs"><i class="glyphicon glyphicon-floppy-open" style="color:blue;font-size:16pt"></i>&nbsp;Purchase Orders</asp:ListItem>
                                            <asp:ListItem Value="PickingTickets"><i class="glyphicon glyphicon-floppy-open" style="color:blue;font-size:16pt"></i>&nbsp;Picking Tickets</asp:ListItem>
                                            <asp:ListItem Value="DeliveryReceipts"><i class="glyphicon glyphicon-floppy-open" style="color:blue;font-size:16pt"></i>&nbsp;Delivery Receipts</asp:ListItem>
                                            <asp:ListItem Value="Invoices"><i class="glyphicon glyphicon-floppy-open" style="color:blue;font-size:16pt"></i>&nbsp;Invoices</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="background-color: lightblue">
                                        <table style="background-color: lightblue">
                                            <tr>
                                                <td valign="top" style="background-color: lightblue">&nbsp;</td>
                                                <td valign="top">
                                                    <table style="background-color: lightblue">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl21" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="Red" Width="225px">*Max Upload Size: 20MB, ZIP to 50MB</asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="11pt" ForeColor="navy">
                                                                    <span style="color:red">*When uploading a document please follow this naming convention:</span><br />
                                                                    <span style="color:navy">POs = Sales Order Number + "_PO"<br />
                                                                    Picking Ticket = Sales Order Number + "_PT"<br />
                                                                    Delivery Receipts = Sales Order Number + "_DR"<br />
                                                                    Invoices = Sales Order Number + "_INV"<br />
                                                                    Example: 52892_PO<br />
                                                                    </span>
                                                                    <span style="color:black;font-size:10pt">If you have multiple invoices use for example: 52892_PO1, 52892_PO2, 52892_PO3</span>
                                                                </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="uploadResult" runat="server" Text="&nbsp;" />
                                                                <asp:FileUpload ID="fuDocuments" runat="server" Width="400px" ForeColor="Black" Font-Size="14pt" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="background-color: lightblue">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center" style="background-color: lightblue">
                                        <asp:LinkButton ID="btnUpload" runat="server" CssClass="btn btn-info" OnClick="btnUpload_Click" Width="200px" ToolTip="First Select a Folder and then after selecting a file click here to Upload"><i class="glyphicon glyphicon-floppy-open" style="font-size:16pt"></i>&nbsp;Upload</asp:LinkButton>
                                        <ajaxToolkit:ConfirmButtonExtender ID="btnUpload_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to upload this file?" Enabled="True" TargetControlID="btnUpload"></ajaxToolkit:ConfirmButtonExtender>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblUploadError" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:LinkButton ID="btnClose" runat="server" CssClass="btn btn-danger" OnClick="btnClose_Click" Width="200px" ToolTip="Click to close pop up"><i class="glyphicon glyphicon-remove" style="font-size:16pt"></i>&nbsp;Close</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:GridView ID="GridView1" runat="server"></asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lbnExportExcel" />
            <asp:PostBackTrigger ControlID="btnUpload" />
            <asp:PostBackTrigger ControlID="gvPDFs" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

