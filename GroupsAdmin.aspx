<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="GroupsAdmin.aspx.cs" Inherits="GroupsAdmin" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
        <ContentTemplate>
            <div style="position: fixed; left: 50%; height: 40px; width: 150px; z-index: 10000;">
                <asp:UpdateProgress ID="Progress" runat="server" AssociatedUpdatePanelID="UpdatePanelPromo" DisplayAfter="1">
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
            <table align="center">
                <tr>
                    <td align="center">
                        <asp:Label ID="Label26" runat="server" Font-Bold="True" Font-Size="20pt" ForeColor="Black" Text="GROUPS ADMIN"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server">
                            <table align="center">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnDisableEnter" runat="server" Text="" OnClientClick="return false;" Style="display: none;" />
                                    </td>
                                    <td></td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;
                                    </td>
                                    <td>
                                        <asp:Panel ID="pnlPageCount" runat="server" Width="100%">
                                            <table width="100%">
                                                <tr>
                                                    <td align="left" style="width: 100px">
                                                        <asp:Label ID="lblResultsPerPage" runat="server" Font-Names="Arial" Font-Size="10pt" Text="Results per page:" Width="125px" ForeColor="Black" Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td align="left">
                                                        <asp:DropDownList ID="ddlDisplayCount" runat="server" AutoPostBack="True" Font-Names="Arial" Font-Size="12pt"
                                                            OnSelectedIndexChanged="ddlDisplayCount_SelectedIndexChanged" CssClass="form-control input-sm" Width="100px" BackColor="LemonChiffon" Font-Bold="True" ForeColor="Black">
                                                            <asp:ListItem>5</asp:ListItem>
                                                            <asp:ListItem>10</asp:ListItem>
                                                            <asp:ListItem>15</asp:ListItem>
                                                            <asp:ListItem Selected="True">20</asp:ListItem>
                                                            <asp:ListItem>50</asp:ListItem>
                                                            <asp:ListItem>100</asp:ListItem>
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
                                        </asp:Panel>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:GridView ID="gvGroups"
                                            runat="server"
                                            AllowPaging="True"
                                            AutoGenerateColumns="False"
                                            CellPadding="4"
                                            ForeColor="#333333"
                                            GridLines="Vertical"
                                            OnPageIndexChanging="gvGroups_PageIndexChanging"
                                            OnRowCancelingEdit="gvGroups_RowCancelingEdit"
                                            OnRowCommand="gvGroups_RowCommand"
                                            OnRowDataBound="gvGroups_RowDataBound"
                                            OnRowDeleting="gvGroups_RowDeleting"
                                            OnRowEditing="gvGroups_RowEditing"
                                            OnRowUpdating="gvGroups_RowUpdating"
                                            ShowFooter="True"
                                            Width="800px" Font-Bold="True" Font-Size="11pt">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField ShowHeader="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGroupID" runat="server" Font-Size="7pt" Text='<%# Bind("GroupID") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Group Name">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtGroupName" runat="server" BackColor="LemonChiffon"  Text='<%# Bind("GroupName") %>' CssClass="form-control" Width="300px" ForeColor="Black"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGroupName" runat="server" Text='<%# Bind("GroupName") %>' ForeColor="Black" Font-Size="10pt"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lbnUpdate" runat="server" CausesValidation="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Update" CssClass="btn btn-success" Font-Bold="True" Text="Update"></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnUpdate_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to update this record?" Enabled="True" TargetControlID="lbnUpdate">
                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                        &nbsp;<asp:LinkButton ID="lbnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Font-Bold="True" ForeColor="White" Text="Cancel" CssClass="btn btn-danger"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbnEdit" runat="server" CausesValidation="False" CommandName="Edit" Font-Bold="True" Text="Edit" CssClass="btn btn-primary"></asp:LinkButton>
                                                    </ItemTemplate>
                                                     <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbnDelete" runat="server" CausesValidation="False" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delete" CssClass="btn btn-danger" Font-Bold="True" Text="Delete" ToolTip="Delete Group"></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to delete this record?" Enabled="True" TargetControlID="lbnDelete">
                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#2461BF" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" Font-Bold="true" Font-Size="12pt" ForeColor="white" HorizontalAlign="Center" CssClass="gridViewPager" />
                                            <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="First" LastPageText="Last" />
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
                        <table>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="label4" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy">Group Name</asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtGroupName" runat="server" BackColor="LemonChiffon" CssClass="form-control" Width="300px" ForeColor="Black"></asp:TextBox>
                                </td>
                                <td></td>
                                <td align="center">
                                    <asp:LinkButton ID="lbnAdd" runat="server" CausesValidation="False" CommandName="Add" Font-Bold="True" Font-Size="11pt" Text="ADD GROUP" ToolTip="Click Add Group" CssClass="btn btn-success" OnClick="lbnAdd_Click"></asp:LinkButton>
                                    <ajaxToolkit:ConfirmButtonExtender ID="lbnAdd_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you Sure you want to add this record?" Enabled="True" TargetControlID="lbnAdd">
                                    </ajaxToolkit:ConfirmButtonExtender>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblPageNo" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="14pt"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label27" runat="server" Font-Bold="True" Font-Size="16pt" ForeColor="Black" Text="GROUP MEMBERS"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Navy" Text="**To assign a Customer to a  Group, select the Group Name from the Group Name list and the Available Customers list will populate with the available Customer you can assign to the selected Group." Width="1100px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table align="center">
                                        <tr>
                                            <td>
                                                <table id="tblAdd" align="center">
                                                    <tr>
                                                        <td class="auto-style1">&nbsp; </td>
                                                        <td class="auto-style1">&nbsp; </td>
                                                        <td align="center">&nbsp; </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td valign="middle">&nbsp; </td>
                                                        <td align="center" valign="middle">&nbsp;<asp:Label ID="Label22" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="12pt" ForeColor="Navy" Text=" Group Name"></asp:Label>
                                                            <asp:ListBox ID="lbGroups" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" ForeColor="Black" Height="100px" OnSelectedIndexChanged="lbGroups_SelectedIndexChanged"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp; </td>
                                                        <td>&nbsp; </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Label ID="Label33" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="12pt" ForeColor="Navy" Text="Available Customer"></asp:Label>
                                                        </td>
                                                        <td>&nbsp; </td>
                                                        <td align="center">
                                                            <asp:Label ID="Label28" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="12pt" ForeColor="Navy" Text="Customers assigned to above  Group"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:ListBox ID="lbAvailable" runat="server" BackColor="LemonChiffon" Font-Bold="True" Font-Size="9pt" ForeColor="Navy" Height="200px" SelectionMode="Multiple" Width="500px"></asp:ListBox>
                                                        </td>
                                                        <td>
                                                            <table id="Table4" align="center" border="0" cellpadding="1" cellspacing="1">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Button ID="btnAddOne" runat="server" CssClass="btn btn-info" Enabled="False" Font-Bold="True" ForeColor="White" OnClick="btnAddOne_Click" Text="Add &gt;" ToolTip="Click to ADD" Width="125px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnAddOne_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with assignment?" Enabled="True" TargetControlID="btnAddOne">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Button ID="btnAddAll" runat="server" CssClass="btn btn-info" Enabled="False" Font-Bold="True" ForeColor="White" OnClick="btnAddAll_Click" Text="Add All &gt;" ToolTip="Click to ADD" Width="125px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnAddAll_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with assignments?" Enabled="True" TargetControlID="btnAddAll">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Button ID="btnRemoveOne" runat="server" CssClass="btn btn-info" Enabled="False" Font-Bold="True" ForeColor="White" OnClick="btnRemoveOne_Click" Text="&lt; Remove" ToolTip="Click to REMOVE" Width="125px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnRemoveOne_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with assignment removal?" Enabled="True" TargetControlID="btnRemoveOne">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Button ID="btnRemoveAll" runat="server" CssClass="btn btn-info" Enabled="False" Font-Bold="True" ForeColor="White" OnClick="btnRemoveAll_Click" Text="&lt; Remove All" ToolTip="Click to REMOVE All" Width="125px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnRemoveAll_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with assignment removals?" Enabled="True" TargetControlID="btnRemoveAll">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td align="center">
                                                            <asp:ListBox ID="lbAssigned" runat="server" BackColor="LemonChiffon" Font-Bold="True" Font-Size="9pt" ForeColor="Navy" Height="200px" SelectionMode="Multiple" Width="500px"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" style="font-family: arial, Helvetica, sans-serif; color: black; font-size: 9pt">(Use CTRL for Multiple Selection) </td>
                                                        <td align="center">&nbsp; </td>
                                                        <td align="center" style="font-family: arial, Helvetica, sans-serif; color: black; font-size: 9pt">(Use CTRL for Multiple Selection) </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:LinkButton ID="lbnSelectAllAssign" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black" OnClick="lbnSelectAllAssign_Click">Select All</asp:LinkButton>
                                                            &nbsp;
                                                            <asp:LinkButton ID="lbnClearAssign" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black" OnClick="lbnClearAssign_Click">Clear All</asp:LinkButton>
                                                        </td>
                                                        <td>&nbsp; </td>
                                                        <td align="center">&nbsp;
                                                            <asp:LinkButton ID="lbnSelectAllAssigned" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black" OnClick="lbnSelectAllAssigned_Click">Select All</asp:LinkButton>
                                                            &nbsp;
                                                            <asp:LinkButton ID="lbnClearAssigned" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black" OnClick="lbnClearAssigned_Click">Clear All</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td align="right">&nbsp; </td>
                                                        <td>&nbsp; </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

