<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManageInvMovementsOther.aspx.cs" Inherits="ManageInvMovementsOther" MaintainScrollPositionOnPostback="true" %>

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
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Ingredient Burden Cost" ForeColor="Black"></asp:Label>
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
                                                <table align="center">
                                                    <tr>
                                                        <td style="cursor: pointer; width: 600px;" align="center">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="center">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <table width="100%">
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <table width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table width="100%">

                                                                                                    <tr>
                                                                                                        <td align="center">
                                                                                                            <asp:Label ID="lblStockCodeList" runat="server" Font-Bold="True" ForeColor="Navy" Text="Stock Code(s)"></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td align="center">&nbsp;</td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:ListBox ID="lbParentStockCode" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="9pt" ForeColor="Black" Height="250px" SelectionMode="Multiple"></asp:ListBox>
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
                                                                                                            <asp:LinkButton ID="lbClearAll" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbnClearStockCode_Click" ValidationGroup="nothing">Clear All</asp:LinkButton>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td align="center">&nbsp;</td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td align="center">
                                                                                                            <asp:LinkButton ID="lbnGenerateForm" runat="server" CausesValidation="False" CommandName="Add" CssClass="btn btn-warning" Font-Bold="True" Font-Size="11pt" OnClick="lbnCreateAddFormProducts_Click" Text="Generate Form" ToolTip="Click to Generate Prouct Design"></asp:LinkButton>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td align="center">&nbsp;</td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td align="center">

                                                                                                            <asp:GridView ID="gvForm" runat="server" AutoGenerateColumns="False" CellPadding="4" Font-Bold="True" Font-Size="11pt" ForeColor="#333333" GridLines="Vertical"
                                                                                                                OnRowDataBound="gvForm_RowDataBound" Width="600px">
                                                                                                                <AlternatingRowStyle BackColor="White" />
                                                                                                                <Columns>
                                                                                                                    <asp:TemplateField HeaderText="ID">
                                                                                                                        <HeaderStyle CssClass="CenterAligner" />
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>' ForeColor="Black" Width="40px" Font-Size="9pt"></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Stock Code">
                                                                                                                        <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblStockCode" runat="server" ForeColor="Black" Text='<%# Bind("StockCode") %>' width="100px"></asp:Label>                                                                                                                             
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Description">
                                                                                                                        <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                                        <ItemTemplate>                                                                                                                             
                                                                                                                            <asp:Label ID="lblDescription" runat="server" ForeColor="Black" Text='<%# Bind("Description") %>' Width="400px" Font-Size="9pt" style="margin-left:10px"></asp:Label>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Freight In">
                                                                                                                        <HeaderStyle CssClass="CenterAligner" />
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:TextBox ID="txtFreight" runat="server" Text='<%# Bind("Freight") %>' BackColor="LemonChiffon" CssClass="form-control input-xs" ForeColor="Black" Width="75px"></asp:TextBox>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Tariff">
                                                                                                                        <HeaderStyle CssClass="CenterAligner" />
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:TextBox ID="txtTariff" runat="server" Text='<%# Bind("Tariff") %>' BackColor="LemonChiffon" CssClass="form-control input-xs" ForeColor="Black" Width="75px"></asp:TextBox>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Misc">
                                                                                                                        <HeaderStyle CssClass="CenterAligner" />
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:TextBox ID="txtMisc" runat="server" Text='<%# Bind("Misc") %>' BackColor="LemonChiffon" CssClass="form-control input-xs" ForeColor="Black" Width="75px"></asp:TextBox>
                                                                                                                        </ItemTemplate>
                                                                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                                                                    </asp:TemplateField>
                                                                                                                </Columns>
                                                                                                                <EditRowStyle BackColor="#2461BF" />
                                                                                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                                                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="12pt" ForeColor="White" />
                                                                                                                <PagerStyle BackColor="#2461BF" CssClass="gridViewPager" Font-Bold="true" Font-Size="12pt" ForeColor="white" HorizontalAlign="Center" />
                                                                                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="5" Position="TopAndBottom" />
                                                                                                                <RowStyle BackColor="White" />
                                                                                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                                                                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                                                                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                                                                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                                                                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                                                                                            </asp:GridView>

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
                                                                                <td align="center">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center"></td>
                                                                            </tr>
                                                                        </table>

                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top" align="left">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top">&nbsp;
                                                            <asp:Label ID="lblErrorRange" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="10pt" ForeColor="Red"></asp:Label>
                                                        </td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                    </tr>

                                                    <tr>
                                                        <td align="left" valign="top">
                                                            <br />
                                                        </td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Click to Save"><i class="fa fa-file-text-o" ></i>&nbsp;Save</asp:LinkButton>
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
                        <tr>
                            <td align="center">
                                <table align="center">
                                    <tr class="rowColors">
                                        <td align="center">
                                            <asp:GridView ID="gvReport" runat="server" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" GridLines="Vertical"
                                                OnRowDataBound="gvReport_RowDataBound"
                                                ShowFooter="True" ForeColor="Black" AutoGenerateColumns="False" Width="1000px">
                                                <AlternatingRowStyle />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="#">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>' Width="50px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Stock Code">
                                                        <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStockCode" runat="server" ForeColor="Black" Text='<%# Bind("StockCode") %>' Width="100px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Description">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>' Width="300px" Style="margin-left: 10px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Stock Code">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Width="125px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Freight In">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFreight" runat="server" Text='<%# Bind("Freight") %>' Width="75px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Tariff">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblTariff" runat="server" Text='<%# Bind("Tariff") %>' Width="75px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Misc">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMisc" runat="server" Text='<%# Bind("Misc") %>' Width="75px"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EditRowStyle BackColor="#2461BF" />
                                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" Font-Size="12pt" ForeColor="White" />
                                                <PagerStyle BackColor="#2461BF" CssClass="gridViewPager" Font-Bold="true" Font-Size="12pt" ForeColor="white" HorizontalAlign="Center" />
                                                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="5" Position="TopAndBottom" />
                                                <RowStyle BackColor="White" />
                                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr class="rowColors">
                                        <td align="left">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table id="Table1" align="center">
                                    <tr>
                                        <td align="center">
                                            <asp:LinkButton ID="btnExportReport" runat="server" OnClick="btnExportReport_Click" CssClass="btn btn-success" ForeColor="White" Width="275px"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL REPORT</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportReport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

