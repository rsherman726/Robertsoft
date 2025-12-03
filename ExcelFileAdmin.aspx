<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ExcelFileAdmin.aspx.cs" Inherits="ExcelFileAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 6px;
        }

        .auto-style3 {
            height: 20px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <table align="Center" width="600" class="BoxShadow4" style="background-color: #F0F0F0">
        <tr>
            <td align="right"></td>
        </tr>
        <tr>
            <td class="hdr" align="center">
                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Excel File Admin" ForeColor="Black"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">&nbsp;<img src="Images/Menus/Excel.GIF" style="width: 16px; height: 16px" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label2" runat="server" Font-Names="verdana" Font-Size="10pt" Width="125px" Font-Bold="True" ForeColor="Black">Pick Excel File:</asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
                <table id="Table3" border="0" cellpadding="1" cellspacing="1" width="300">
                    <tr>
                        <td class="auto-style1">
                            <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Names="verdana" Font-Size="10pt" ForeColor="Red">1st.</asp:Label>
                        </td>
                        <td>
                            <asp:FileUpload ID="FileUploadExcel" runat="server" Width="300px" />
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
                <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="verdana" Font-Size="10pt" ForeColor="Red">2nd.</asp:Label>
                <asp:Button ID="btnUpload" runat="server" Font-Bold="False" OnClick="btnUpload_Click" Text="Upload Excel File" ToolTip="Click to upload a rate schedule excel file." CssClass="btn btn-primary" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Label ID="lblErrorUpload" runat="server" Font-Bold="True" Font-Names="verdana" Font-Size="9pt" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="center">
            </td>
        </tr>
        <tr>
            <td align="left" >
                <asp:Label ID="Label11" runat="server"  Font-Names="verdana" Font-Size="9pt" ForeColor="Blue">Note: Make sure you don&#39;t have any spaces between the words in the name of your file. Use "_" underscore if necessary.</asp:Label>
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
                                                                <ItemStyle CssClass="Text_Small" HorizontalAlign="Center"/>
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

