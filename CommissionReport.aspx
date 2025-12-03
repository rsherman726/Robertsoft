<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CommissionReport.aspx.cs" Inherits="CommissionReport" %>

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
        .RowStyle {
            height: 50px;
        }

        .AlternateRowStyle {
            height: 50px;
            background-color: LemonChiffon;
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
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Commission  Report" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="background-color: lightblue;" align="center" class="JustRoundedEdgeBothSmall">
                                <asp:Panel ID="pnlSearchCriteria" runat="server" Width="1100px">
                                    <table width="1100">
                                        <tr>
                                            <td align="center">
                                                <table>
                                                    <tr>
                                                        <td align="left" style="cursor: pointer">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top">
                                                            <table style="width: 550px">
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
                                                                    <td align="left" style="width: 100px">&nbsp;</td>
                                                                    <td align="right">&nbsp;</td>
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                 <asp:LinkButton ID="btnPreviewDetails" runat="server" OnClick="btnPreviewDetails_Click" CssClass="btn btn-info"  Width="200px" ForeColor="White" ToolTip="Click to run details report"><i class="fa fa-file-text-o" ></i>&nbsp;Details Report</asp:LinkButton>      
                                                 <asp:LinkButton ID="btnPreview" runat="server" OnClick="btnPreview_Click" CssClass="btn btn-info"  Width="200px" ForeColor="White" ToolTip="Click to run summary report"><i class="fa fa-file-text-o" ></i>&nbsp;Summary Report</asp:LinkButton>                                                   
                                                 <asp:LinkButton ID="btnReset" runat="server" OnClick="btnReset_Click" CssClass="btn btn-danger"  Width="200px" ForeColor="White" ToolTip="Click to reset form"><i class="fa fa-refresh" ></i>&nbsp;Reset Form</asp:LinkButton>  
                                                 
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
                        <tr id="trSummaryReport" runat="server">
                            <td align="center">
                                <asp:Label ID="lblTotalSalesLabel" runat="server" Font-Names="Arial" Font-Size="14pt" ForeColor="Navy" Font-Bold="True"></asp:Label>&nbsp;
                                <asp:Label ID="lblTotalCommissionLabel" runat="server" Font-Names="Arial" Font-Size="14pt" ForeColor="Navy" Font-Bold="True"></asp:Label>
                                <table align="center">
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
                                                    Visible="false">
                                                </asp:Table>
                                            </div>
                                            <!-- *** End Header Table *** -->
                                        </td>
                                    </tr>
                                     <tr class="rowColors">
                                        <td align="left">
                                            <asp:Panel ID="pnlGridView" runat="server" Height="300px" ScrollBars="Vertical" Visible="False">
                                                <div id="DivData" class="Container" style="vertical-align: top; height: 300px; width: 100%;">
                                                    <asp:GridView ID="gvReportSummary" runat="server" AutoGenerateColumns="False" Font-Size="10pt"
                                                        GridLines="Vertical" ShowFooter="True"
                                                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                        BorderWidth="1px" ForeColor="Black" OnRowDataBound="gvReportSummary_RowDataBound"
                                                        Font-Names="Arial" AllowSorting="false" OnSorting="gvReportSummary_Sorting" Width="1100px" ShowHeader="False">
                                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sales#" SortExpression="SalespersonID">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalesPersonID" runat="server" Text='<%# Bind("SalespersonID") %>' Width="70px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Salesperson" SortExpression="SalespersonName">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalesPerson" runat="server" Text='<%# Bind("SalespersonName") %>' Width="200px" Font-Size="9"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotal" runat="server" Text="Totals:" Width="75px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="center" Font-Bold="true" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Cust#" SortExpression="Customer">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Width="70px" Font-Size="9"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="CustomerName" SortExpression="CustomerName">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>' Width="200px" Font-Size="9"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="Sales" SortExpression="Sales">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSales" runat="server" Text='<%# Bind("Sales") %>' Width="85px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotalSales" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Commission" SortExpression="Commission">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCommission" runat="server" Text='<%# Bind("Commission") %>' Width="75px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotalCommission" runat="server" Text="$0.00" Width="75px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="%" SortExpression="ComPer">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblComPer" runat="server" Text='<%# Bind("ComPer") %>' Width="75px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <%-- <FooterTemplate>
                                                             <asp:Label ID="lblComPerAvgTotal" runat="server" Text="0.00" Width="75px"></asp:Label>
                                                                 </FooterTemplate>--%>
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
                        <tr>
                            <td align="center">
                                <table id="Table1" align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="imgExportExcel1" runat="server" ImageUrl="~/Images/ExportToExcel.GIF" OnClick="imgExportExcel1_Click" Visible="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr id="trDetailsReport" runat="server">
                            <td align="center">
                                <asp:Label ID="Label1" runat="server" Font-Names="Arial" Font-Size="14pt" ForeColor="Navy" Font-Bold="True"></asp:Label>&nbsp;
                                <asp:Label ID="Label2" runat="server" Font-Names="Arial" Font-Size="14pt" ForeColor="Navy" Font-Bold="True"></asp:Label>
                                <table align="center">
                                    <tr>
                                        <td align="left">
                                            <!-- *** Begin Header Table *** -->
                                            <div id="divHeader2" runat="server" style="vertical-align: top; text-align: left;">
                                                <asp:Table ID="tblHeaderTable2" runat="server"
                                                    CellPadding="2"
                                                    CellSpacing="0"
                                                    Font-Size="11pt"
                                                    ForeColor="White"
                                                    BackColor="#333333"
                                                    Font-Bold="False"
                                                    Visible="false">
                                                </asp:Table>
                                            </div>
                                            <!-- *** End Header Table *** -->
                                        </td>
                                    </tr>
                                     <tr class="rowColors">
                                        <td align="left">
                                            <asp:Panel ID="pnlGridView2" runat="server" Height="300px" ScrollBars="Vertical" Visible="False">
                                                <div id="DivData2" class="Container" style="vertical-align: top; height: 300px; width: 100%;">
                                                    <asp:GridView ID="gvReportDetails" runat="server" AutoGenerateColumns="False" Font-Size="10pt"
                                                        GridLines="Vertical" ShowFooter="True"
                                                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                        BorderWidth="1px" ForeColor="Black" OnRowDataBound="gvReportDetails_RowDataBound"
                                                        Font-Names="Arial" AllowSorting="True" OnSorting="gvReportDetails_Sorting" Width="1300px">                                                        
                                                        <RowStyle CssClass="RowStyle" />
                                                        <AlternatingRowStyle CssClass="AlternateRowStyle"  BackColor="WhiteSmoke" />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="Sales#">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalesPersonID" runat="server" Text='<%# Bind("SalespersonID") %>' Width="50px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Salesperson">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSalesPerson" runat="server" Text='<%# Bind("SalespersonName") %>' Width="125px" Font-Size="9"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                                <FooterTemplate>
                                                                    <asp:Label ID="lblTotal" runat="server" Text="Totals:" Width="75px"></asp:Label>
                                                                </FooterTemplate>
                                                                <FooterStyle HorizontalAlign="center" Font-Bold="true" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cust#">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Width="70px" Font-Size="9"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Customer<br>Name">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("CustomerName") %>' Width="125px" Font-Size="9"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice<br>Number" SortExpression="InvoiceNumber">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# Bind("InvoiceNumber") %>' Width="60px" Font-Size="9"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice<br>Date" SortExpression="InvoiceDate">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblInvoiceDate" runat="server" Text='<%# Bind("InvoiceDate") %>' Width="60px" Font-Size="9"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Stock<br>Code" SortExpression="StockCode">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner"/>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Width="60px" Font-Size="9"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Stock<br>Description" SortExpression="StockDescription">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblStockDescription" runat="server" Text='<%# Bind("StockDescription") %>' Width="125px" Font-Size="9"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Qty" SortExpression="Qty">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty") %>' Width="60px" Font-Size="9"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="UoM" SortExpression="UoM">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUoM" runat="server" Text='<%# Bind("UoM") %>' Width="60px" Font-Size="9"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Unit<br>Price" SortExpression="UnitPrice">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Bind("UnitPrice") %>' Width="65px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Line<br>Price" SortExpression="LinePrice">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblLinePrice" runat="server" Text='<%# Bind("LinePrice") %>' Width="75px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Cash<br>Disc Amt" SortExpression="CashDiscAmt">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCashDiscAmt" runat="server" Text='<%# Bind("CashDiscAmt") %>' Width="65px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Net<br>Sale Amt" SortExpression="NetSaleAmt">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblNetSaleAmt" runat="server" Text='<%# Bind("NetSaleAmt") %>' Width="75px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Com<br>Amt" SortExpression="ComAmt">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblComAmt" runat="server" Text='<%# Bind("ComAmt") %>' Width="75px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:TemplateField>

                                                            <asp:TemplateField HeaderText="%">
                                                                <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblComPer" runat="server" Text='<%# Bind("ComPercentage") %>' Width="45px"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" />
                                                                <%-- <FooterTemplate>
                                                             <asp:Label ID="lblComPerAvgTotal" runat="server" Text="0.00" Width="75px"></asp:Label>
                                                                 </FooterTemplate>--%>
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
                        <tr>
                            <td align="center">
                                <table id="Table2" align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="imgExportExcel2" runat="server" ImageUrl="~/Images/ExportToExcel.GIF" OnClick="imgExportExcel2_Click" Visible="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                    </table>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgExportExcel1" />
            <asp:PostBackTrigger ControlID="imgExportExcel2" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
