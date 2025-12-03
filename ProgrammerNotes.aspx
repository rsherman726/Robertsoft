<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProgrammerNotes.aspx.cs" Inherits="ProgrammerNotes" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
        <ContentTemplate>
            <div style="position: fixed; left: 50%; height: 40px; width: 150px; z-index: 10000;">
                <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" AssociatedUpdatePanelID="UpdatePanelPromo" DisplayAfter="1">
                    <ProgressTemplate>
                        <table>
                            <tbody>
                                <tr>
                                    <td align="left">
                                        <img src="Images/indicator_big.gif" alt="" />
                                    </td>
                                    <td align="left"><span style="color: #ffffff">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing <span class="">....</span> </strong></span></span></td>
                                </tr>
                            </tbody>
                        </table>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <table align="Center" style="background-color: Ghostwhite" class="JustRoundedEdgeBothSmall" width="800px">  
                <tr>
                    <td class="hdr" align="center">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Programmer Notes" ForeColor="Black"></asp:Label></td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblModifiedDate" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <CKEditor:CKEditorControl ID="txtNotes" runat="server" BasePath="~/ckeditor" Height="900px" Width="915px" ForeColor="Black">
                        </CKEditor:CKEditorControl></td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:LinkButton ID="lbnSave" runat="server" CssClass="btn btn-success" OnClick="lbnSave_Click">Save Notes</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblErrorNotes" runat="server" Font-Bold="True" Font-Size="20pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>

            </table>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

