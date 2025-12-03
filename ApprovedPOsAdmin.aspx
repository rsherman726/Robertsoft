<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ApprovedPOsAdmin.aspx.cs" Inherits="ApprovedPOsAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <table align="Center" width="1000px" class="BoxShadow4" style="background-color: #F0F0F0">
        <tr>
            <td align="right"></td>
        </tr>
        <tr>
            <td class="hdr" align="center">
                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Approved P.O. Admin" ForeColor="Black"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">&nbsp;<img src="Images/Menus/pdf.gif" style="width: 16px; height: 16px" />
            </td>
        </tr>
         
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            <table border="0" bordercolor="black" bordercolordark="#000000" bordercolorlight="#000000">
                                <tr>
                                    <td>
                                        <table id="tblGrid" border="0" cellpadding="0" cellspacing="0" align="center">
                                            <tr>
                                                <td align="left" style="color: #2f3632"></td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:GridView ID="gvPDFs" runat="server" AutoGenerateColumns="False"
                                                        Width="1000px" AllowPaging="True"
                                                        OnPageIndexChanging="gvPDFs_PageIndexChanging" OnRowCommand="gvPDFs_RowCommand"
                                                        OnRowDataBound="gvPDFs_RowDataBound" OnRowDeleting="gvPDFs_RowDeleting" ForeColor="Black">
                                                        <FooterStyle CssClass="DHTR_Grid_Row td" />
                                                        <RowStyle CssClass="DHTR_Grid_Row td" />
                                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                        <Columns>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgDelete" runat="server" CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                                        CommandName="Delete" Text="Delete" ImageUrl="Images/Delete.gif" />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="FileName">
                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="lnkFileName" runat="server" Text='<%# Bind("FileName") %>'></asp:HyperLink>
                                                                    <asp:Label ID="lblFullPath" runat="server" Visible="false" Text='<%# Bind("FullPath") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center"
                                                                    VerticalAlign="Middle" />
                                                                <ItemStyle CssClass="Text_Small" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Creation Date">
                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDateCreated" runat="server" Text='<%# Bind("DateCreated") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center"
                                                                    VerticalAlign="Middle" />
                                                                <ItemStyle CssClass="Text_Small" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="File Size">
                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFileSize" runat="server" Text='<%# Bind("FileSize") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle BackColor="WhiteSmoke" Font-Names="Verdana" Font-Size="XX-Small" HorizontalAlign="Center"
                                                                    VerticalAlign="Middle" />
                                                                <ItemStyle CssClass="Text_Small" HorizontalAlign="Right" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Names="Arial"
                                                        Font-Size="9pt" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>


                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">&nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

    </table>
</asp:Content>

