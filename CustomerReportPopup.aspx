<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CustomerReportPopup.aspx.cs" Inherits="CustomerReportPopup" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer Report</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.js"></script>
    <script type="text/javascript" src="Scripts/kissy-min.js"></script>
    <style type="text/css">
        .Prompt {
            color: navy;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"  AsyncPostBackTimeout="3600"></asp:ScriptManager>
        <div>
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
                            <table align="center">

                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Customer  Report" Font-Names="Arial"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table width="100%">
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
                                            <tr>
                                                <td align="left">
                                                    <asp:Panel ID="pnlGridView" runat="server" Height="300px" ScrollBars="Vertical" Visible="False" Width="100%">
                                                        <div id="DivData" class="Container" style="vertical-align: top; height: 300px; width: 100%">
                                                            <asp:GridView ID="gvReportCondensed" runat="server" AutoGenerateColumns="False" Font-Size="10pt"
                                                                GridLines="Vertical" Width="1100" ShowFooter="True" ShowHeader="false"
                                                                BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                                BorderWidth="1px" ForeColor="Black" OnRowDataBound="gvReportCondensed_RowDataBound" Font-Names="Arial" OnRowCommand="gvReportCondensed_RowCommand">
                                                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Name">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lbnName" runat="server" Text='<%# Bind("Name") %>'
                                                                                CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                                                CausesValidation="False" ToolTip="Click to view also bought items and Chart."
                                                                                CommandName="View"
                                                                                Width="210px"
                                                                                ForeColor="Navy" Font-Bold="True"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblTotal" runat="server" Text="Totals:" Width="75px"></asp:Label>
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="center" Font-Bold="true" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Cust#">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Width="75px"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Stock<br/>Code">
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
                                                                    <asp:TemplateField HeaderText="Product<br/>Class">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblProductClass" runat="server" Text='<%# Bind("PCDescription") %>' Width="155px" Font-Size="7pt"></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Uom">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblUom" runat="server" Text='<%# Bind("CostUom") %>' Width="50px"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Qty">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblQty" runat="server" Text='<%# Bind("InvoiceQty") %>' Width="65px"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Amount">
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
                                                                    <asp:TemplateField HeaderText="Margin">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblMargin" runat="server" Text='<%# Bind("Margin") %>' Width="75px"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                        <FooterTemplate>
                                                                            <asp:Label ID="lblMarginAvgTotal" runat="server" Text="0.00" Width="75px"></asp:Label>
                                                                        </FooterTemplate>
                                                                        <FooterStyle HorizontalAlign="right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="P<br/>C">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblPriceCode" runat="server" Text='<%# Bind("PriceCode") %>' Width="20px"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="center" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Price">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblAvgPrice" runat="server" Text='<%# Bind("Price") %>' Width="85px"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Price<br/> Change<br/> Date" SortExpression="LastChangeDate">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblLastChangeDate" runat="server" Text='<%# Bind("LastChangeDate") %>' Width="80px"></asp:Label>
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
                                                    <asp:ImageButton ID="imgExportExcel1" runat="server" ImageUrl="~/Images/ExportToExcel.GIF" OnClick="imgExportExcel1_Click" />
                                                </td>
                                            </tr>
                                        </table>

                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblStockCodeLabel" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="16pt" ForeColor="Navy"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <table id="tblChart">
                                            <tr>
                                                <td align="center">
                                                    <asp:Chart ID="Chart1" runat="server" BackColor="#F3DFC1" BackGradientStyle="TopBottom" BorderColor="181, 64, 1" BorderlineDashStyle="Solid" BorderWidth="2" Height="350px" ImageLocation="~/WebCharts/ChartPic_#SEQ(300,3)" ImageType="Png" Palette="BrightPastel" Visible="False" Width="1050px">
                                                        <Legends>
                                                            <asp:Legend BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold" IsTextAutoFit="False" Name="Default">
                                                            </asp:Legend>
                                                        </Legends>
                                                        <BorderSkin SkinStyle="Emboss" />
                                                        <Series>
                                                            <asp:Series BorderColor="180, 26, 59, 105" BorderWidth="2" ChartType="Line" Name="Price" ShadowColor="64, 0, 0, 0" ShadowOffset="2">
                                                                <Points>
                                                                    <asp:DataPoint YValues="45" />
                                                                    <asp:DataPoint YValues="34" />
                                                                    <asp:DataPoint YValues="67" />
                                                                    <asp:DataPoint YValues="31" />
                                                                    <asp:DataPoint YValues="27" />
                                                                    <asp:DataPoint YValues="87" />
                                                                    <asp:DataPoint YValues="45" />
                                                                    <asp:DataPoint YValues="32" />
                                                                </Points>
                                                            </asp:Series>
                                                            <asp:Series BorderColor="180, 26, 59, 105" BorderWidth="2" ChartType="Line" Name="Margin" ShadowColor="64, 0, 0, 0" ShadowOffset="2">
                                                                <Points>
                                                                    <asp:DataPoint YValues="50" />
                                                                    <asp:DataPoint YValues="40" />
                                                                    <asp:DataPoint YValues="70" />
                                                                    <asp:DataPoint YValues="25" />
                                                                    <asp:DataPoint YValues="20" />
                                                                    <asp:DataPoint YValues="77" />
                                                                    <asp:DataPoint YValues="50" />
                                                                    <asp:DataPoint YValues="44" />
                                                                </Points>
                                                            </asp:Series>
                                                            <asp:Series BorderColor="180, 26, 59, 105" BorderWidth="2" ChartType="Line" Name="Amount" ShadowColor="64, 0, 0, 0" ShadowOffset="2">
                                                                <Points>
                                                                    <asp:DataPoint YValues="60" />
                                                                    <asp:DataPoint YValues="50" />
                                                                    <asp:DataPoint YValues="90" />
                                                                    <asp:DataPoint YValues="15" />
                                                                    <asp:DataPoint YValues="50" />
                                                                    <asp:DataPoint YValues="97" />
                                                                    <asp:DataPoint YValues="30" />
                                                                    <asp:DataPoint YValues="24" />
                                                                </Points>
                                                            </asp:Series>
                                                        </Series>
                                                        <ChartAreas>
                                                            <asp:ChartArea BackColor="WhiteSmoke" BorderColor="64, 64, 64, 64" Name="ChartArea1" ShadowColor="Transparent">
                                                                <AxisY2 Enabled="True" IsLabelAutoFit="False" LineColor="64, 64, 64, 64" Title="Margin">
                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                    <MajorGrid Enabled="False" />
                                                                </AxisY2>
                                                                <AxisX2 Enabled="False" IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                </AxisX2>
                                                                <Area3DStyle Inclination="15" IsClustered="False" IsRightAngleAxes="False" Perspective="10" Rotation="10" WallWidth="0" />
                                                                <AxisY IsLabelAutoFit="False" LineColor="64, 64, 64, 64" Title="Sales">
                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="{c}" />
                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                </AxisY>
                                                                <AxisX IsLabelAutoFit="False" LineColor="64, 64, 64, 64">
                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="MM/YYYY" Interval="Auto" />
                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                </AxisX>
                                                            </asp:ChartArea>
                                                        </ChartAreas>
                                                    </asp:Chart>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:CheckBox ID="chk3D" runat="server" AutoPostBack="True" CssClass="Text_Bold" OnCheckedChanged="chk3D_CheckedChanged" Text="&nbsp;Show in 3D" Visible="False" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:GridView ID="gv1" runat="server" Visible="False">
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblCustomerName" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="16pt" ForeColor="Navy"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblAlsoBought" runat="server" Font-Names="arial" Font-Size="12pt" ForeColor="Navy" Font-Bold="True" Font-Italic="True" Visible="False">Items this customer also bought</asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:GridView ID="gvAlsoBought" runat="server" AutoGenerateColumns="False" Font-Size="10pt"
                                            GridLines="Vertical" Width="1050px" ShowFooter="True" ShowHeader="true"
                                            BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                            BorderWidth="1px" ForeColor="Black" OnRowDataBound="gvAlsoBought_RowDataBound" Font-Names="Arial" OnRowCommand="gvAlsoBought_RowCommand" AllowSorting="True" OnSorting="gvAlsoBought_Sorting">
                                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Stock<br/>Code" SortExpression="StockCode">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Width="70px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>' Width="175px" Font-Size="8pt"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Uom">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUom" runat="server" Text='<%# Bind("CostUom") %>' Width="40px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Qty">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQty" runat="server" Text='<%# Bind("InvoiceQty") %>' Width="65px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>' Width="75px"></asp:Label>
                                                        <asp:Label ID="lblCostValue" runat="server" Text='<%# Bind("CostValue") %>' Width="80px" Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="right" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblAmountTotal" runat="server" Text="$0.00" Width="85px"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Margin" SortExpression="Margin">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMargin" runat="server" Text='<%# Bind("Margin") %>' Width="75px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblMarginAvgTotal" runat="server" Text="0.00" Width="75px"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="P<br/>C">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPriceCode" runat="server" Text='<%# Bind("PriceCode") %>' Width="20px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Price">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAvgPrice" runat="server" Text='<%# Bind("Price") %>' Width="75px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Price<br/> Change<br/> Date" SortExpression="LastChangeDate">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLastChangeDate" runat="server" Text='<%# Bind("LastChangeDate") %>' Width="70px"></asp:Label>
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
                                            <FooterStyle BackColor="Wheat" />
                                        </asp:GridView>
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
                                    <td>&nbsp;</td>
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
        </div>
    </form>
</body>
</html>
