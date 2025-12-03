<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="EmailEditor.aspx.cs" Inherits="EmailEditor" EnableEventValidation="false" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        window.onload = function () {
            var div = document.getElementById("dvScroll");
            var div_position = document.getElementById("div_position");
            var position = parseInt('<%=Request.Form["div_position"] %>');
            if (isNaN(position)) {
                position = 0;
            }
            div.scrollTop = position;
            div.onscroll = function () {
                div_position.value = div.scrollTop;
            };
        };
    </script>
    <div>
        <table align="center">
            <tr>
                <td>
                    <table width="850" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid; background-color: ghostwhite">
                        <tr>
                            <td class="hdr" colspan="2" align="center">
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Email Editor" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>

                            <td align="center">
                                <div id="dvScroll" style="overflow-y: scroll; height: 600px; width: 100%">
                                    <asp:GridView ID="gvButtons" runat="server" AllowPaging="false" AutoGenerateColumns="False"
                                        DataKeyNames="ID" OnPageIndexChanging="gvButtons_PageIndexChanging" OnRowCancelingEdit="gvButtons_RowCancelingEdit"
                                        OnRowCommand="gvButtons_RowCommand" OnRowDataBound="gvButtons_RowDataBound" OnRowDeleting="gvButtons_RowDeleting"
                                        OnRowEditing="gvButtons_RowEditing" OnRowUpdating="gvButtons_RowUpdating" PageSize="14"
                                        ShowFooter="True" Width="900px">
                                        <Columns>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnDeleteRow" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                        CommandName="Delete" Text="Delete" CssClass="btn btn-danger" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="btnDeleteRow_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with Delete?" Enabled="True" TargetControlID="btnDeleteRow"></ajaxToolkit:ConfirmButtonExtender>
                                                </ItemTemplate>
                                                <ItemStyle Width="60px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:Button ID="btnEditRow" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-info" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Button ID="btnUpdateRow" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                        CommandName="Update" Text="Update" CssClass="btn btn-success" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="btnUpdateRow_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with Update?" Enabled="True" TargetControlID="btnUpdateRow"></ajaxToolkit:ConfirmButtonExtender>
                                                    &nbsp;<asp:Button ID="btnCancelEdit" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-info" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <FooterTemplate>
                                                    <asp:Button ID="btnAddRow" runat="server" CommandName="Add" Text="Add" CssClass="btn btn-success" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="btnAddRow_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with Add?" Enabled="True" TargetControlID="btnAddRow"></ajaxToolkit:ConfirmButtonExtender>
                                                </FooterTemplate>
                                                <FooterStyle Width="60px" HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField InsertVisible="False" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>' ForeColor="Black"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblIDEdit" runat="server" Text='<%# Bind("ID") %>' ForeColor="Black"></asp:Label>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Document Name">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDocName" runat="server" Text='<%# Bind("DocName") %>' ForeColor="Black" Font-Names="Arial" Width="300px"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblDocNameEdit" runat="server" Text='<%# Bind("DocName") %>' ForeColor="Black" Width="300px"></asp:Label>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtAddDocName" runat="server" AutoPostBack="True" OnTextChanged="txtAddDocName_TextChanged" CssClass="form-control" BackColor="LemonChiffon"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Load Document" ShowHeader="False">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Button ID="btnLoadDoc" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                        CommandName="Load" Text="Load Text" CssClass="btn btn-warning" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle BackColor="Black" ForeColor="White" />
                                        <PagerStyle Font-Bold="True" Font-Size="11pt" HorizontalAlign="Center" />
                                    </asp:GridView>
                                </div>
                                <input type="hidden" id="div_position" name="div_position" />
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblErr" runat="server" CssClass="Text_Small" EnableViewState="False"
                                    Font-Bold="True" Font-Size="10pt" ForeColor="Red"></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <table class="style1">
                                    <tr>
                                        <td align="left">
                                            <asp:Button ID="btnReset" runat="server" Text="Reset Editor" OnClick="btnReset_Click" CssClass="btn btn-info" />
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btnHome" runat="server" Text="Home" BorderStyle="None"
                                                BackColor="Transparent" Font-Underline="True" OnClick="btnHome_Click"
                                                Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Red"></asp:Button>
                                        </td>
                                        <td align="center">
                                            <asp:Button ID="btnBack" runat="server" Text="Back to Emailer" BorderStyle="None"
                                                BackColor="Transparent" Font-Underline="True" OnClick="btnBack_Click"
                                                Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Red"></asp:Button>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="left">&nbsp;
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtInstructionForHTMLEditor" runat="server" CssClass="Text_Small"
                                    EnableViewState="False" TextMode="MultiLine" ReadOnly="true" Font-Bold="False"
                                    ForeColor="Blue" Font-Size="9pt" Height="110px" Width="900px"></asp:TextBox>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblVariablesWarning" runat="server" ForeColor="Red" Text="WARNING! Do No Overwrite the Variable values! e.g. sContactName, sCustomerName, Etc."
                                    Font-Bold="True" Font-Names="arial" Font-Size="10pt"></asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="lblDocVars" runat="server" Font-Bold="True" CssClass="Text_Bold" ForeColor="Blue"
                                    Font-Size="10pt">DOCUMENT VARIABLES</asp:Label>
                            </td>
                            <td>&nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <table id="tblFields" runat="server" class="Text_Bold">
                        <tr>
                            <td>
                                <asp:Label ID="Label154" runat="server" Text="sCustomerName" ForeColor="Black"></asp:Label>
                            </td>
                            <td style="width: 15px">&nbsp;</td>
                            <td>
                                <asp:Label ID="Label183" runat="server" Text="sDate" ForeColor="Black"></asp:Label>
                            </td>
                            <td style="width: 15px">&nbsp;</td>
                            <td>
                                <asp:Label ID="Label180" runat="server" Text="sCSZ" ForeColor="Black"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label146" runat="server" Text="sContactName" ForeColor="Black"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Label ID="Label182" runat="server" Text="sAddress" ForeColor="Black"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Label ID="Label181" runat="server" Text="sFutureVariable" ForeColor="Black"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <table>
                        <tr>
                            <td valign="top">

                                <CKEditor:CKEditorControl ID="RSEditor" runat="server" BasePath="~/ckeditor" Height="500px" Width="950px" AutoParagraph="false"></CKEditor:CKEditorControl>
                            </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">
                    <asp:Label runat="server" ID="ContentChangedLabel" CssClass="Text_Small" />
                </td>
            </tr>
            <tr>
                <td align="left" valign="top">&nbsp;
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
