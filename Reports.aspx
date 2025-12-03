<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Reports.aspx.cs" Inherits="Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .auto-style1 {
            height: 22px;
        }
        .auto-style2 {
            width: 450px;
            height: 22px;
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
                                        <img src="Images/loader.gif" alt="" />
                                    </td>
                                    <td align="left"><span style="color: #ffffff">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing <span class="">....</span> </strong></span></span></td>
                                </tr>
                            </tbody>
                        </table>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <div>
                <table align="center">
                    <tr>
                        <td>
                            <table width="1000" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid; background-color: ghostwhite">
                                <tr>
                                    <td class="hdr" align="center">
                                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                            Text="Reports" ForeColor="Black"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left"></td>
                                </tr>
                                <tr>
                                    <td>
                                        <table id="tblReports" align="center">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label23" runat="server" Font-Bold="True"
                                                        Font-Size="8pt" ForeColor="Black">Select a Report</asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlReports" runat="server"
                                                        Width="255px" BackColor="LemonChiffon" Font-Bold="True" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="ddlReports_SelectedIndexChanged" ForeColor="Black">
                                                        <asp:ListItem Value="0">Select a Report...</asp:ListItem>
                                                        <asp:ListItem>Labor Productivity Report</asp:ListItem>
                                                        <asp:ListItem>Labor Hours Report</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="Label27" runat="server"  Font-Bold="True"
                                            Font-Size="8pt" ForeColor="Navy">**Leave dates blank for all time.</asp:Label>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table id="tblRange" border="0" cellpadding="1" cellspacing="1"
                                            class="Text_Small" width="700" align="center">
                                            <tr>
                                                <td align="right">
                                                    <asp:Label ID="Label2" runat="server"  Font-Bold="True"
                                                        Font-Size="8pt" Width="65px" ForeColor="Black">Start Date:</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:TextBox ID="txtStartDate" runat="server"
                                                        ValidationGroup="" Width="150px" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server"
                                                        Animated="False" Enabled="True" Format="MM/dd/yyyy"
                                                        PopupButtonID="imgStartDate" TargetControlID="txtStartDate">
                                                    </ajaxToolkit:CalendarExtender>

                                                    <ajaxToolkit:MaskedEditExtender ID="txtDateMEE" runat="server"
                                                        Mask="99/99/9999"
                                                        MaskType="Date"
                                                        MessageValidatorTip="true"
                                                        InputDirection="LeftToRight"
                                                        TargetControlID="txtStartDate">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    <ajaxToolkit:MaskedEditValidator ID="txtDateMEV" runat="server"
                                                        ControlExtender="txtDateMEE"
                                                        ControlToValidate="txtStartDate"
                                                        Display="None"
                                                        ErrorMessage="Invalid date format."
                                                        InvalidValueMessage="Invalid date format."
                                                        IsValidEmpty="true"
                                                        EmptyValueMessage="Please enter a start date."
                                                        TooltipMessage="Please enter a date."
                                                        MinimumValue="01/01/1901"
                                                        MinimumValueMessage="Date must be greater than 01/01/1901" />
                                                    <ajaxToolkit:ValidatorCalloutExtender ID="txtDateMEVE" runat="server"
                                                        TargetControlID="txtDateMEV"
                                                        HighlightCssClass="validatorCalloutHighlight" />

                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgStartDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="Label22" runat="server"  Font-Bold="True"
                                                        Font-Size="8pt" Width="65px" ForeColor="Black">End Date:</asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:TextBox ID="txtEndDate" runat="server"
                                                        ValidationGroup="" Width="150px" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server"
                                                        Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgEndDate"
                                                        TargetControlID="txtEndDate">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <ajaxToolkit:MaskedEditExtender ID="txtDateMEE2" runat="server"
                                                        Mask="99/99/9999"
                                                        MaskType="Date"
                                                        MessageValidatorTip="true"
                                                        InputDirection="LeftToRight"
                                                        TargetControlID="txtEndDate">
                                                    </ajaxToolkit:MaskedEditExtender>
                                                    <ajaxToolkit:MaskedEditValidator ID="txtDateMEV2" runat="server"
                                                        ControlExtender="txtDateMEE2"
                                                        ControlToValidate="txtEndDate"
                                                        Display="None"
                                                        ErrorMessage="Invalid date format."
                                                        InvalidValueMessage="Invalid date format."
                                                        IsValidEmpty="true"
                                                        EmptyValueMessage="Please enter an end date."
                                                        TooltipMessage="Please enter a date."
                                                        MinimumValue="01/01/1901"
                                                        MinimumValueMessage="Date must be greater than 01/01/1901" />
                                                    <ajaxToolkit:ValidatorCalloutExtender ID="ValidatorCalloutExtender1" runat="server"
                                                        TargetControlID="txtDateMEV2"
                                                        HighlightCssClass="validatorCalloutHighlight" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgEndDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnRun" runat="server" Text="Run Report"
                                                        OnClick="btnRun_Click" ToolTip="Click here to run the report!" Font-Bold="True" CssClass="btn btn-primary" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblError" runat="server"  EnableViewState="False"
                                            Font-Bold="True" Font-Size="10pt" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label25" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black">STOCK CODES</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:ListBox ID="lbStockCode" runat="server" BackColor="LemonChiffon" Height="200px" SelectionMode="Multiple" CssClass="form-control" Style="width: 900px !important" Font-Size="7pt" ForeColor="Black"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="color: black">(Use CTRL for Multiple Selection)</td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:LinkButton ID="lbnSelectAllStockCode" runat="server" Font-Bold="True" OnClick="lbnSelectAllStockCode_Click">Select All</asp:LinkButton>
                                        &nbsp;
                                                            <asp:LinkButton ID="lbnClearStockCode" runat="server" Font-Bold="True" OnClick="lbnClearStockCode_Click">Clear All</asp:LinkButton></td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="pnlLaborControls" runat="server" Width="900">
                                            <table>
                                                <tr>
                                                    <td align="center">
                                                        <table width="900">

                                                            <tr>
                                                                <td align="left">&nbsp;</td>
                                                                <td align="center" style="width: 450px">&nbsp;</td>
                                                                <td align="center">&nbsp;</td>
                                                                <td align="center" style="width: 450px">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">&nbsp;</td>
                                                                <td align="center" style="width: 450px">
                                                                    <asp:Label ID="Label24" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black">EMPLOYEES</asp:Label>
                                                                </td>
                                                                <td align="center">&nbsp;</td>
                                                                <td align="center" style="width: 450px">
                                                                    <asp:Label ID="Label26" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black">PRODUCTION LINES</asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">&nbsp;</td>
                                                                <td style="width: 450px" align="center">
                                                                    <asp:ListBox ID="lbEmployees" runat="server" Height="250px" BackColor="LemonChiffon" SelectionMode="Multiple" CssClass="form-control" ForeColor="Black"></asp:ListBox>
                                                                </td>
                                                                <td>&nbsp;</td>
                                                                <td style="width: 450px" align="center">
                                                                    <asp:ListBox ID="lbProductionLine" runat="server" Height="250px" BackColor="LemonChiffon" SelectionMode="Multiple" CssClass="form-control" ForeColor="Black"></asp:ListBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">&nbsp;</td>
                                                                <td align="center" style="width: 450px; color: black;">(Use CTRL for Multiple Selection)</td>
                                                                <td align="center">&nbsp;</td>
                                                                <td align="center" style="width: 450px; color: black;">(Use CTRL for Multiple Selection)</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" class="auto-style1"></td>
                                                                <td align="center" class="auto-style2">
                                                                    <asp:LinkButton ID="lbnSelectAllEmployee" runat="server" Font-Bold="True" OnClick="lbnSelectAllEmployee_Click">Select All</asp:LinkButton>
                                                                    &nbsp;
                                                            <asp:LinkButton ID="lbnClearEmployee" runat="server" Font-Bold="True" OnClick="lbnClearEmployee_Click">Clear All</asp:LinkButton>
                                                                </td>
                                                                <td align="center" class="auto-style1"></td>
                                                                <td align="center" class="auto-style2">
                                                                    <asp:LinkButton ID="lbnSelectAllProLine" runat="server" Font-Bold="True" OnClick="lbnSelectAllProLine_Click">Select All</asp:LinkButton>
                                                                    &nbsp;
                                                            <asp:LinkButton ID="lbnClearProLine" runat="server" Font-Bold="True" OnClick="lbnClearProLine_Click">Clear All</asp:LinkButton></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">&nbsp;</td>
                                                                <td align="left" style="width: 450px">
                                                                    <asp:CheckBox ID="chkGroup" runat="server" Font-Bold="True" Text="&nbsp;Group Excel Export Report by employee" Width="345px" ToolTip="You must select one or more Employees to use Group Function" ForeColor="Black" />
                                                                    
                                                                     
                                                                </td>
                                                                <td>&nbsp;</td>
                                                                <td style="width: 450px">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">&nbsp;</td>
                                                                <td align="left" style="width: 450px">
                                                                    <asp:Label ID="Label56" runat="server" EnableViewState="False" Font-Bold="True" Font-Size="10pt" ForeColor="Red">Please Note: Choosing Grouping Option above requires you select one or more employees. The results will only properly display in the exported Excel spreadsheet, not the grid below..</asp:Label>
                                                                </td>
                                                                <td>&nbsp;</td>
                                                                <td style="width: 450px">&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <table>
                                            <tr>
                                                <td align="left" width="100">
                                                    <asp:Label ID="lblResultsPerPage" runat="server" Font-Names="Arial" Font-Size="10pt" Text="Results per page:" Width="125px" ForeColor="Black"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlDisplayCount" runat="server" AutoPostBack="True" Font-Names="Arial" Font-Size="10pt" OnSelectedIndexChanged="ddlDisplayCount_SelectedIndexChanged" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black" Width="75px" >
                                                        <asp:ListItem>10</asp:ListItem>
                                                        <asp:ListItem Selected="True">20</asp:ListItem>
                                                        <asp:ListItem>50</asp:ListItem>
                                                        <asp:ListItem>100</asp:ListItem>
                                                        <asp:ListItem>250</asp:ListItem>
                                                        <asp:ListItem>500</asp:ListItem>
                                                        <asp:ListItem>750</asp:ListItem>
                                                        <asp:ListItem>1000</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="right" style="width: 350px" valign="middle">
                                                    <asp:Label ID="lblRecordCount" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black"></asp:Label>
                                                    </td>
                                                <td></td>
                                                <td>
                                                    <asp:Button ID="btnRun0" runat="server" CssClass="btn btn-primary" Font-Bold="True" OnClick="btnRun_Click" Text="Run Report" ToolTip="Click here to run the report!" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Panel ID="pnlLaborReport" runat="server">
                                            <asp:GridView ID="gvReports" runat="server" CellPadding="4" ForeColor="#333333"
                                                GridLines="None" HorizontalAlign="Center" Font-Size="9pt" AllowPaging="True" OnPageIndexChanging="gvReports_PageIndexChanging" PageSize="15" AllowSorting="True" OnSorting="gvReports_Sorting" Width="950px" ToolTip="Click headers link to sort asending or decending." OnRowDataBound="gvReports_RowDataBound">
                                                <AlternatingRowStyle BackColor="White" />
                                                <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" HorizontalAlign="Left" />
                                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                                <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                                <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                                <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                                <SortedDescendingHeaderStyle BackColor="#820000" />
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:GridView ID="gvReportsLaborHours" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" Font-Size="9pt" ForeColor="#333333" GridLines="None" HorizontalAlign="Center" OnPageIndexChanging="gvReportsLaborHours_PageIndexChanging" OnRowDataBound="gvReportsLaborHours_RowDataBound" OnSorting="gvReports_Sorting" PageSize="20" ToolTip="Click headers link to sort asending or decending." Width="950px" ShowFooter="True">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Date">                                                  
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Production Line">                                                    
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblProductionLine" runat="server" Text='<%# Bind("ProductionLine") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="StockCode">                                                   
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stock<br/>Description">                                                   
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockDescription" runat="server" Text='<%# Bind("StockDescription") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Qty">                                                   
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQtyManufactured" runat="server" Text='<%# Bind("QtyManufactured") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Job">                                                    
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblJob" runat="server" Text='<%# Bind("Job") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="UOM">                                                   
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUOM" runat="server" Text='<%# Bind("UOM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Employee Name">                                                   
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmployeeName" runat="server" Text='<%# Bind("EmployeeName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                          <FooterTemplate>
                                                        <asp:Label ID="LabelTotal" runat="server" Text="Total Hours:"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Hours">                                                    
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHours" runat="server" Text='<%# Bind("Hours") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblTotalHours" runat="server"></asp:Label>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" HorizontalAlign="Left" />
                                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                            <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                            <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                            <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                            <SortedDescendingHeaderStyle BackColor="#820000" />
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <table id="Table1" align="center">
        <tr>
            <td align="center">&nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:LinkButton ID="imgExportExcel" runat="server" OnClick="imgExportExcel_Click" CssClass="btn btn-success" ForeColor="White" Width="200px"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>
                 
            </td>
        </tr>
    </table>
</asp:Content>

