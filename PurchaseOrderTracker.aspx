<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PurchaseOrderTracker.aspx.cs" Inherits="PurchaseOrderTracker" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.4.1/css/all.css" integrity="sha384-5sAR7xN1Nv6T6+dT2mhtzEpVJvfS3NScPQTrOxhwjIuvcA67KV2R5Jz6kr4abQsz" crossorigin="anonymous" />
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
            <table align="Center" width="1200" class="BoxShadow4" style="background-color: #F0F0F0">
                <tr>
                    <td align="center">
                        <table width="100%">
                            <tr>
                                <td class="hdr" align="center">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                        Text="Open Purchase Orders" ForeColor="Black"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlMain" runat="server">
                                        <table width="100%" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                            align="center">
                                            <tr>
                                                <td style="line-height: 15px">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">

                                                    <table width="300" align="center" id="tblTimer">
                                                        <tr>
                                                            <td style="width: 125px">
                                                                <asp:CheckBox ID="chkTimerOnOff" runat="server" Font-Size="9pt" Text="&nbsp;Auto Refresh" AutoPostBack="True" OnCheckedChanged="chkTimerOnOff_CheckedChanged" Checked="True" Width="125px" ForeColor="Black" /></td>
                                                            <td align="center" style="width: 110px">
                                                                <asp:DropDownList ID="ddlInterval" runat="server" Font-Bold="True" Font-Size="9pt" CssClass="form-control input-sm" Width="120px" ForeColor="Black" AutoPostBack="True" OnSelectedIndexChanged="ddlInterval_SelectedIndexChanged">
                                                                    <asp:ListItem Value="20000">20 Seconds</asp:ListItem>
                                                                    <asp:ListItem Value="30000">30 Seconds</asp:ListItem>
                                                                    <asp:ListItem Value="40000">40 Seconds</asp:ListItem>
                                                                    <asp:ListItem Value="50000">50 Seconds</asp:ListItem>
                                                                    <asp:ListItem Value="60000" Selected="True">1 Minute</asp:ListItem>
                                                                    <asp:ListItem Value="90000">1.5 Minutes</asp:ListItem>
                                                                    <asp:ListItem Value="120000">2 Minutes</asp:ListItem>
                                                                    <asp:ListItem Value="300000">5 Minutes</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>


                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="line-height: 5px">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblTimeUpdated" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="9pt" ForeColor="#FF0066" Width="300px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <table style="width: 1300px">
                                                        <tr>
                                                            <td align="left" style="width: 100px">
                                                                <asp:Label ID="lblPeriod" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="Period:"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 300px">
                                                                <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="10pt" ForeColor="Black" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" Width="300px">
                                                                    <asp:ListItem Selected="True">ALL</asp:ListItem>
                                                                    <asp:ListItem>Range</asp:ListItem>
                                                                    <asp:ListItem>Current Week</asp:ListItem>
                                                                    <asp:ListItem>Previous Week</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left" style="width: 5px">&nbsp;</td>
                                                            <td align="left" style="width: 120px">
                                                                <asp:Label ID="lblStartDate" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="PO Date From:"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtStartDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="200px"></asp:TextBox>
                                                                <ajaxToolkit:MaskedEditValidator ID="txtStartDateMEV" runat="server" ControlExtender="txtStartDateMEE" ControlToValidate="txtStartDate" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                                <ajaxToolkit:ValidatorCalloutExtender ID="txtStartDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtStartDateMEV" />
                                                                <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgStartDate" TargetControlID="txtStartDate" />
                                                                <ajaxToolkit:MaskedEditExtender ID="txtStartDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtStartDate" />
                                                            </td>
                                                            <td align="left" style="width: 5px">
                                                                <asp:ImageButton ID="imgStartDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                            </td>
                                                            <td align="left" style="width: 120px">
                                                                <asp:Label ID="lblEndDate" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="PO Date To:"></asp:Label>
                                                            </td>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtEndDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="200px"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgEndDate" TargetControlID="txtEndDate" />
                                                                <ajaxToolkit:MaskedEditExtender ID="txtEndDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtEndDate" />
                                                                <ajaxToolkit:MaskedEditValidator ID="txtEndDateMEV" runat="server" ControlExtender="txtEndDateMEE" ControlToValidate="txtEndDate" Display="None" EmptyValueMessage="Please enter an end date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                                <ajaxToolkit:ValidatorCalloutExtender ID="txtEndDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtEndDateMEV" />
                                                            </td>
                                                            <td align="left" style="width: 5px">
                                                                <asp:ImageButton ID="imgEndDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td>&nbsp;</td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td align="center">
                                                                <br />
                                                            </td>
                                                            <td align="left" style="width: 5px">&nbsp;</td>
                                                            <td align="left" style="width: 5px">&nbsp;</td>
                                                            <td align="left" style="width: 5px">&nbsp;</td>
                                                            <td align="left" style="width: 5px">&nbsp;</td>
                                                            <td align="left" style="width: 5px">&nbsp;</td>
                                                            <td align="left" style="width: 5px">&nbsp;</td>
                                                            <td align="left" style="width: 5px">&nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblNotice" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Red">*To be able to save the PO PDF to the Approved P.O.s in the Approved P.O. Admin you first need to check both the Director and Management Approval check boxes.</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:LinkButton ID="btnRunReport" runat="server" CssClass="btn btn-info" ForeColor="White" OnClick="btnRunReport_Click" ToolTip="Click to run report" Width="175px"><i class="fa fa-file-text-o"></i>&nbsp;Run Report</asp:LinkButton>
                                                                <asp:LinkButton ID="lbnExportExcel" runat="server" CssClass="btn btn-success" ForeColor="White" OnClick="lbnExportExcel_Click" Width="175px"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>
                                                                <asp:LinkButton ID="btnReset" runat="server" CssClass="btn btn-danger" ForeColor="White" OnClick="btnReset_Click" ToolTip="Click to reset form" Width="175px"><i class="fa fa-refresh"></i>&nbsp;Reset Form</asp:LinkButton>
                                                            </td>
                                                            <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                                            <td>
                                                                <asp:CheckBox ID="chkAllSuppliers" runat="server" Text="&nbsp;All Suppliers" AutoPostBack="True" ForeColor="Black" OnCheckedChanged="chkAllSuppliers_CheckedChanged" ToolTip="Show All Suppliers"/>
                                                                <asp:CheckBoxList ID="cblFilters" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cblFilters_SelectedIndexChanged" RepeatDirection="Horizontal" Width="700px" Visible="False">
                                                                    <asp:ListItem Value="Confirmation">&amp;nbsp;Confirmation</asp:ListItem>
                                                                    <asp:ListItem Value="Director Approved">&amp;nbsp;Director Approved</asp:ListItem>
                                                                    <asp:ListItem Value="Manager Approved">&amp;nbsp;Manager Approved</asp:ListItem>
                                                                    <asp:ListItem Value="Non Stock Received">&amp;nbsp;Non Stock Received</asp:ListItem>
                                                                </asp:CheckBoxList>
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
                                                    <asp:Label ID="lblError0" runat="server" Font-Bold="True" Font-Size="22pt" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center">
                                                    <table id="tblAdd" align="center" width="1200">
                                                        <tr class="rowColors">
                                                            <td align="left" valign="middle">

                                                                <!-- *** Begin Header Table *** -->
                                                                <div id="DivHeader" style="vertical-align: top; width: 1325px; text-align: left;">
                                                                    <asp:Table ID="HeaderTable" runat="server"
                                                                        CellPadding="2"
                                                                        CellSpacing="0"
                                                                        Font-Size="11pt"
                                                                        ForeColor="White"
                                                                        BackColor="#333333"
                                                                        Font-Bold="False"
                                                                        Width="1295px">
                                                                    </asp:Table>
                                                                </div>
                                                                <!-- *** End Header Table *** -->
                                                                <asp:Panel ID="pnlGridView" runat="server" Height="500px" ScrollBars="Vertical" Width="1300px">
                                                                    <div id="DivData" class="Container" style="vertical-align: top; height: 500px; width: 1275px;">
                                                                        <asp:GridView ID="gvRecord" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="Black" Width="1275px"
                                                                            BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" OnPageIndexChanging="gvRecord_PageIndexChanging"
                                                                            OnRowDataBound="gvRecord_RowDataBound" OnRowCommand="gvRecord_RowCommand"
                                                                            OnSorting="gvRecord_Sorting" AllowSorting="True" PageSize="50" ShowFooter="true"
                                                                              ShowHeader="false">

                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>' Font-Size="9pt" ForeColor="Black" Width="35px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="PO#" SortExpression="PurchaseOrder">
                                                                                    <HeaderStyle CssClass="CenterAligner" Width="60px" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblQueryStatus" runat="server" ForeColor="Black"></asp:Label>
                                                                                        <asp:Label ID="lblPurchaseOrder" runat="server" Text='<%# Bind("PurchaseOrder") %>' Visible="false"></asp:Label>

                                                                                        <asp:LinkButton ID="lbnPurchaseOrder" runat="server"
                                                                                            CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                                                            CausesValidation="False" ToolTip="Click to view details for this Purchase Order."
                                                                                            CommandName="ViewDetails" ForeColor="#00569D" Font-Size="9" CssClass="NoUnderline"
                                                                                            Text='<%# Bind("PurchaseOrder") %>' Width="50px" Font-Bold="True"></asp:LinkButton>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Supp#" SortExpression="Supplier">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSupplier" runat="server" Text='<%# Bind("Supplier") %>' Font-Bold="false" Font-Size="9" Width="54px" ForeColor="Black"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Supplier" SortExpression="SupplierName">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblSupplierName" runat="server" Text='<%# Bind("SupplierName") %>' Font-Bold="true" Font-Size="9" Width="350px" ForeColor="Black"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Memo Date" SortExpression="MemoDate">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblMemoDate" runat="server" Text='<%# Bind("MemoDate","{0:d}") %>' Font-Bold="true" Font-Size="9" Width="100px" ForeColor="Black"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Buyer" SortExpression="Buyer">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblBuyer" runat="server" Text='<%# Bind("Buyer") %>' Font-Bold="true" Font-Size="9" Width="80px" ForeColor="Black"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="P.O.<br>Date" SortExpression="PurchaseOrderDate">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblPurchaseOrderDate" runat="server" Text='<%# Bind("PurchaseOrderDate") %>' Font-Bold="false" Font-Size="9" Width="78px" ForeColor="Black"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Order<br>Due Date" SortExpression="OrderDueDate">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOrderDueDate" runat="server" Text='<%# Bind("OrderDueDate") %>' Font-Bold="false" Font-Size="9" Width="78px" ForeColor="Black"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Total<br>Value" SortExpression="TotalValue">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblTotalValue" runat="server" Text='<%# Bind("TotalValue") %>' Font-Bold="True" Font-Size="9" Width="70px" ForeColor="Black"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblGrandTotal" runat="server" Font-Bold="True" Font-Size="9"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Confirmed" SortExpression="ConfirmationUserID">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkConfirmed" runat="server" Width="90px" AutoPostBack="true" OnCheckedChanged="chkConfirmed_CheckedChanged" />
                                                                                        <asp:Label ID="lblConfirmationUserID" runat="server" Text='<%# Bind("ConfirmationUserID") %>' Visible="false"></asp:Label>                                                                                     
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Confirmed By" SortExpression="Confirmer">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                         <asp:Label ID="lblConfirmer" runat="server" Text='<%# Bind("Confirmer") %>' ForeColor="Black" Width="250px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Director<br>Approval" SortExpression="DirectorApprovalUserID" Visible="False">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkDirectorApproval" runat="server" Width="75px" AutoPostBack="true" OnCheckedChanged="chkDirectorApproval_CheckedChanged"  ForeColor="Black"/>
                                                                                        <asp:Label ID="lblDirectorApprovalUserID" runat="server" Text='<%# Bind("DirectorApprovalUserID") %>' Visible="false"></asp:Label>
                                                                                        <asp:Label ID="lblApprovalDirector" runat="server" Text='<%# Bind("ApprovalDirector") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Management<br>Approval" SortExpression="ManagementApprovalUserID" Visible="False">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkManagementApproval" runat="server" Width="75px" AutoPostBack="true" OnCheckedChanged="chkManagementApproval_CheckedChanged"  ForeColor="Black"/>
                                                                                        <asp:Label ID="lblManagementApprovalUserID" runat="server" Text='<%# Bind("ManagementApprovalUserID") %>' Visible="false"></asp:Label>
                                                                                        <asp:Label ID="lblApprovalManager" runat="server" Text='<%# Bind("ApprovalManager") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Non<br>Stock<br>Received" SortExpression="NonStockReceived" Visible="False">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkNonStockReceived" runat="server" Width="75px" AutoPostBack="true" OnCheckedChanged="chkNonStockReceived_CheckedChanged"  ForeColor="Black"/>
                                                                                        <asp:Label ID="lblNonStockReceived" runat="server" Text='<%# Bind("NonStockReceived") %>' Visible="false"></asp:Label>
                                                                                        <asp:Label ID="lblNonStockReceivedUser" runat="server" Text='<%# Bind("NonStockReceivedUser") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>


                                                                            </Columns>
                                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" />
                                                                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                            <SortedDescendingHeaderStyle BackColor="#242121" />
                                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </asp:Panel>
                                                            </td>
                                                            <td align="center" valign="middle">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">&nbsp; 
                                                                <asp:Label ID="lblPageNo" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy" Visible="False"></asp:Label>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Timer ID="Timer1" runat="server" Interval="50000" OnTick="Timer1_Tick" />
                                                            </td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="22pt"></asp:Label>
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
            <div class="clearfix">
                <asp:Button ID="Button1" runat="server" Style="display: none" Text="Button" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderPopUp" runat="server" BackgroundCssClass="popup_background"
                    DynamicServicePath="" Enabled="True" PopupControlID="pnlPopUp" TargetControlID="Button1">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlPopUp" runat="server" CssClass="modalPopup2" Visible="true" Style="display: block; padding: 10px" ScrollBars="Vertical" Height="500px" Width="1485px">
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
                            <table id="tblPopUp" style="border: 1px solid black; font-size: 7pt; background-color: #FFFFFF;" width="1185px" align="center">
                                <tr>
                                    <td align="center">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label30" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="Navy">Purchase Order Details for </asp:Label>
                                        <asp:Label ID="lblPurchaseOrderDetails" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="Navy"></asp:Label>
                                        <asp:Label ID="lblPurchaseOrderDetailsHidden" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="lblPurchaseOrderDateDetailsHidden" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="black">Due Date:</asp:Label>
                                        &nbsp;&nbsp;<asp:Label ID="lblDueDatePopup" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Green"></asp:Label>
                                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblSupplierNameHidden" runat="server" Visible="False"></asp:Label>
                                        <asp:Label ID="lblSupplierNumberHidden" runat="server" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnClose0" runat="server" CssClass="btn btn-danger" Text="Close" OnClick="btnClose_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblPopUpError" runat="server" Font-Bold="True" Font-Size="16pt" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvDetails" runat="server" CellPadding="4" ForeColor="#333333"
                                            GridLines="None"
                                            HorizontalAlign="Center"
                                            Font-Size="9pt"
                                            PageSize="15" AllowSorting="True"
                                            OnSorting="gvDetails_Sorting"
                                            Width="1175px"
                                            AutoGenerateColumns="False"
                                            ShowFooter="True" OnRowDataBound="gvDetails_RowDataBound">
                                            <FooterStyle BackColor="Silver" />
                                            <HeaderStyle Font-Size="8pt" VerticalAlign="Top" BackColor="#CCCCCC" />                                             
                                            <Columns>
                                                <asp:TemplateField HeaderText="Purchase Order #" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPurchaseOrder" runat="server" Text='<%# Bind("PurchaseOrder") %>' ForeColor="#00569D"></asp:Label>
                                                        <asp:Label ID="lblPurchaseOrderHidden" runat="server" Text='<%# Bind("PurchaseOrder") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblFlag" runat="server" Text='<%# Bind("Flag") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblPrice" runat="server" Text='<%# Bind("Price") %>' Visible="false"></asp:Label>
                                                        
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Order Qty" SortExpression="OrderQtyInOrderUom">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderQty" runat="server" Text='<%# Bind("OrderQtyInOrderUom") %>' Font-Bold="True" Font-Size="8"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Order Uom" SortExpression="OrderUom">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderUom" runat="server" Text='<%# Bind("OrderUom") %>' Font-Bold="True" Font-Size="8"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Unit Price In Order Uom" SortExpression="UnitPriceInOrderUom">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitPriceInOrderUom" runat="server" Text='<%# Bind("UnitPriceInOrderUom") %>' Font-Bold="True" Font-Size="9"></asp:Label>&nbsp;
                                                        <asp:Label ID="lblLastPrice" runat="server" Text='<%# Bind("LastPrice") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Stock Code" SortExpression="MStockCode">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="hlStockCode" runat="server" Text='<%# Bind("MStockCode") %>' Font-Bold="True" Font-Size="9" CssClass="NoUnderline"></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center"/>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Description" SortExpression="MStockDes">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMStockDes" runat="server" Text='<%# Bind("MStockDes") %>' Font-Bold="True" Font-Size="8"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left"/>
                                                    <FooterTemplate>
                                                        Order Value Total: 
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="PO Value" SortExpression="OrderValue">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderValue" runat="server" Text='<%# Bind("OrderValue") %>' Font-Bold="True" Font-Size="9"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right"/>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblOrderValueTotal" runat="server" Font-Bold="True" Font-Size="12"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>

                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnClose" runat="server" CssClass="btn btn-danger" OnClick="btnClose_Click" Text="Close" />
                                        <asp:LinkButton ID="lbnSave" runat="server" CssClass="btn btn-success" OnClick="lbnSave_Click" Visible="False">SAVE APPROVED PO</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">

                                        <table id="tblExport" runat="server" visible="false">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblOptions" runat="server" Font-Names="Arial" Font-Size="Small" ForeColor="Black" Style="text-align: center" Text="Export Options:" Visible="False"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgbtnExcel" runat="server" Height="40px" ImageUrl="~/Images/ExcelLogoSmall.scale-80.png" OnClick="imgbtnExcel_Click" Text="Button" Visible="false" />
                                                </td>
                                                <td align="center">
                                                    <asp:ImageButton ID="imgbtnPDF" runat="server" Height="20px" ImageUrl="~/Images/pdf-symbol.png" OnClick="imgbtnPDF_Click" Text="Button" Visible="False" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgbtnWord" runat="server" Height="40px" ImageUrl="~/Images/WinWordLogoSmall.scale-80.png" OnClick="imgbtnWord_Click" Text="Button" Visible="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="pnlForm" runat="server" Width="1000px" ScrollBars="Horizontal">
                                            <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />
                                        </asp:Panel>
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
            <asp:PostBackTrigger ControlID="imgbtnExcel" />
            <asp:PostBackTrigger ControlID="imgbtnPDF" />
            <asp:PostBackTrigger ControlID="imgbtnWord" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">

        //This Script is used to maintain Grid Scroll on Partial Postback
        var scrollTop;

        //Register Begin Request and End Request 
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        //Get The Div Scroll Position
        function BeginRequestHandler(sender, args) {
            var m = document.getElementById('DivData');
            scrollTop = m.scrollTop;
        }

        //Set The Div Scroll Position
        function EndRequestHandler(sender, args) {
            var m = document.getElementById('DivData');
            m.scrollTop = scrollTop;
        }
    </script>
</asp:Content>

