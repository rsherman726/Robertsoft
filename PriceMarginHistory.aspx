<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PriceMarginHistory.aspx.cs" Inherits="PriceMarginHistory" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
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
            <table align="center" width="900">
                <tr>
                    <td align="center">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Price Margin History  Report" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <table align="center" width="900">
                            <tr>
                                <td>
                                    <asp:Panel ID="PnlContent" runat="server">
                                        <table>
                                            <tr>
                                                <td align="center" class="JustRoundedEdgeBothSmall" style="background-color: lightgreen; width: 1100px">
                                                    <asp:Panel ID="pnlSearchCriteriaChart1" runat="server">
                                                        <table width="1000">
                                                            <tr>
                                                                <td align="center">
                                                                    <table>
                                                                        <tr>
                                                                            <td align="center">

                                                                                <asp:RadioButtonList ID="rblIngredientComponent" runat="server" ForeColor="Black" RepeatDirection="Horizontal" Width="410px">
                                                                                    <asp:ListItem Selected="True">&nbsp;Ingredient Stock Code</asp:ListItem>
                                                                                    <asp:ListItem>&nbsp;Component Stock Code</asp:ListItem>
                                                                                </asp:RadioButtonList>

                                                                            </td>
                                                                            <td align="center" style="width: 450px">
                                                                                <asp:Label ID="Label53" runat="server" Font-Bold="True" ForeColor="Black" Text="Finished Stock Code"></asp:Label>
                                                                                &nbsp;<asp:Label ID="lblStockCodeList" runat="server" Font-Bold="True" ForeColor="Black" Text="Stock Code(s)"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center">&nbsp;</td>
                                                                            <td align="center" style="width: 450px">
                                                                                <asp:RadioButtonList ID="rblSourceOfStockCodes" runat="server" AutoPostBack="True" ForeColor="Black" OnSelectedIndexChanged="rblSourceOfStockCodes_SelectedIndexChanged" RepeatDirection="Horizontal" Width="250px">
                                                                                    <asp:ListItem>&nbsp;WipMaster</asp:ListItem>
                                                                                    <asp:ListItem>&nbsp;InvMaster</asp:ListItem>
                                                                                </asp:RadioButtonList>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">&nbsp;</td>
                                                                            <td align="left" style="width: 450px">&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="cursor: pointer" align="center">
                                                                                <table width="100%">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:Label ID="lblStockCodeDescIngredientComponent" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Navy"></asp:Label>
                                                                                            <br />

                                                                                        </td>
                                                                                        <td>&nbsp;</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtStockCodeChartIngredientComponent" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtStockCodeChartIngredient_TextChanged" ValidationGroup="" Width="300px"></asp:TextBox>
                                                                                           
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Font-Bold="True" OnClick="btnSearch_Click" Text="Search" /></td>
                                                                                    </tr>
                                                                                </table>

                                                                            </td>
                                                                            <td style="cursor: pointer" align="left">
                                                                                <asp:ListBox ID="lbParentStockCode" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="9pt" ForeColor="Black" Height="120px" Style="width: 500px !important" AutoPostBack="True" OnSelectedIndexChanged="lbParentStockCode_SelectedIndexChanged"></asp:ListBox>
                                                                                <ajaxToolkit:ListSearchExtender ID="lbParentStockCode_ListSearchExtender" runat="server"
                                                                                    Enabled="True" IsSorted="True" TargetControlID="lbParentStockCode" PromptCssClass="Prompt">
                                                                                </ajaxToolkit:ListSearchExtender>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" style="cursor: pointer">&nbsp;</td>
                                                                            <td align="center" style="cursor: pointer">&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center" style="cursor: pointer">&nbsp;</td>
                                                                            <td align="center" style="cursor: pointer">
                                                                                <asp:Button ID="btnLoadAll" runat="server" CssClass="btn btn-default" Font-Bold="True" OnClick="btnLoadAll_Click" Text="Load All Finished Stock Codes" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top" align="left">&nbsp;</td>
                                                                            <td valign="top" align="left">
                                                                                <asp:Label ID="lblErrorChart1" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="10pt" ForeColor="Red"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                      <asp:LinkButton ID="btnPreviewChart1" runat="server" OnClick="btnPreviewChart1_Click" CssClass="btn btn-info"  Width="200px" ForeColor="White" ToolTip="Click to run"><i class="fa fa-file-text-o" ></i>&nbsp;Run Report</asp:LinkButton> 
 
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label ID="Label49" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Black" Text="Price-Margin History"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Chart ID="Chart1" runat="server" BackColor="#F3DFC1" BackGradientStyle="TopBottom" BorderColor="181, 64, 1" BorderlineDashStyle="Solid" BorderWidth="2" Height="350px" ImageLocation="~/WebCharts/ChartPic_#SEQ(300,3)" ImageType="Png" Palette="BrightPastel" Width="1100px" Visible="False">
                                                        <Legends>
                                                            <asp:Legend IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold"></asp:Legend>
                                                        </Legends>
                                                        <BorderSkin SkinStyle="Emboss" />
                                                        <Series>
                                                            <asp:Series Name="Price" BorderColor="180, 26, 59, 105" BorderWidth="2" ChartType="Line" ShadowColor="64, 0, 0, 0" ShadowOffset="2">
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
                                                            <asp:Series Name="Margin" BorderColor="180, 26, 59, 105" BorderWidth="2" ChartType="Line" ShadowColor="64, 0, 0, 0" ShadowOffset="2">
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
                                                            <asp:Series Name="Amount" BorderColor="180, 26, 59, 105" BorderWidth="2" ChartType="Line" ShadowColor="64, 0, 0, 0" ShadowOffset="2">
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
                                                            <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackColor="WhiteSmoke" ShadowColor="Transparent">
                                                                <AxisY2 LineColor="64, 64, 64, 64" IsLabelAutoFit="False" Enabled="True" Title="Margin">
                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                                                    <MajorGrid Enabled="False" />
                                                                </AxisY2>
                                                                <AxisX2 LineColor="64, 64, 64, 64" IsLabelAutoFit="False" Enabled="False"></AxisX2>
                                                                <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False" WallWidth="0" IsClustered="False"></Area3DStyle>
                                                                <AxisY LineColor="64, 64, 64, 64" IsLabelAutoFit="False" Title="Sales">
                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="{c}" />
                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                </AxisY>
                                                                <AxisX LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
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
                                    </asp:Panel>
                                </td>
                            </tr>

                            <tr>
                                <td align="center">
                                    <asp:GridView ID="gv1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="500px">
                                        <AlternatingRowStyle BackColor="White" />
                                        <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" CssClass="CenterAligner" />
                                        <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
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
        </ContentTemplate>
        <%--        <Triggers>
            <asp:PostBackTrigger ControlID="imgExportExcel1" />
            <asp:PostBackTrigger ControlID="imgExportExcel2" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>

