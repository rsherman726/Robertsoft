<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CustomItemsAdmin.aspx.cs" Inherits="CustomItemsAdmin" MaintainScrollPositionOnPostback="true" %>

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
            <table align="Center" width="1000" class="JustRoundedEdgeBoth" style="background-color: GhostWhite">
                <tr>
                    <td align="center">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Custom Items Admin" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblErrorEditor" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server">
                            <table align="center" style="background-color: GhostWhite">
                                <tr>
                                    <td>&nbsp; 
                                    </td>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td align="left" style="width: 100px">
                                                    <asp:Label ID="lblResultsPerPage" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="Black" Text="Results per page:" Width="125px"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddlDisplayCount" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control input-sm" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" ForeColor="Black" OnSelectedIndexChanged="ddlDisplayCount_SelectedIndexChanged" Width="100px">
                                                        <asp:ListItem>5</asp:ListItem>
                                                        <asp:ListItem>10</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>20</asp:ListItem>
                                                        <asp:ListItem>50</asp:ListItem>
                                                        <asp:ListItem Selected="True">100</asp:ListItem>
                                                        <asp:ListItem>200</asp:ListItem>
                                                        <asp:ListItem>300</asp:ListItem>
                                                        <asp:ListItem>400</asp:ListItem>
                                                        <asp:ListItem>500</asp:ListItem>
                                                        <asp:ListItem>600</asp:ListItem>
                                                        <asp:ListItem>700</asp:ListItem>
                                                        <asp:ListItem>800</asp:ListItem>
                                                        <asp:ListItem>900</asp:ListItem>
                                                        <asp:ListItem>1000</asp:ListItem>
                                                        <asp:ListItem>ALL</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" valign="middle">&nbsp; </td>
                                            </tr>
                                        </table>

                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>

                                        <asp:GridView ID="gvCustomItems" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="Black" GridLines="None" OnPageIndexChanging="gvCustomItems_PageIndexChanging" OnRowCancelingEdit="gvCustomItems_RowCancelingEdit"
                                            OnRowCommand="gvCustomItems_RowCommand" OnRowDataBound="gvCustomItems_RowDataBound" OnRowDeleting="gvCustomItems_RowDeleting"
                                            OnRowEditing="gvCustomItems_RowEditing" OnRowUpdating="gvCustomItems_RowUpdating" ShowFooter="True" Width="1000px" Font-Names="Arial" Font-Size="11pt" PageSize="100">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField ShowHeader="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomItemID" runat="server" Font-Size="12pt" Text='<%# Bind("CustomItemID") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stock Code">
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlStockCodes" runat="server" Font-Size="9pt" Width="350px" CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Visible="false"></asp:Label>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlStockCodes" runat="server" Font-Size="9pt" Width="350px" CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>' ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lbnUpdate" runat="server" CausesValidation="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Update" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Update"></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnUpdate_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to update?" Enabled="True" TargetControlID="lbnUpdate"></ajaxToolkit:ConfirmButtonExtender>
                                                        &nbsp;<asp:LinkButton ID="lbnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Cancel"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbnEdit" runat="server" CausesValidation="False" CommandName="Edit" Font-Bold="True" Font-Size="11pt" Text="Edit" ForeColor="Black"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbnDelete" runat="server" CausesValidation="False" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delete" Font-Bold="True" Font-Size="11pt" Text="Delete" ForeColor="Blue"></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you ABSOLUTELY SURE you want to delete?" Enabled="True" TargetControlID="lbnDelete"></ajaxToolkit:ConfirmButtonExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lbnAdd" runat="server" CausesValidation="False" CommandName="Add" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Add"></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnAdd_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you Sure you want to add this?" Enabled="True" TargetControlID="lbnAdd"></ajaxToolkit:ConfirmButtonExtender>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#2461BF" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" Font-Bold="true" Font-Size="12pt" ForeColor="White" HorizontalAlign="Center" CssClass="gridViewPager" />
                                            <RowStyle BackColor="#EFF3FB" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                        </asp:GridView>

                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>

                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblPageNo" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Arial" Font-Size="12pt"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

