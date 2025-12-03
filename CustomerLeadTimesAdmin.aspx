<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CustomerLeadTimesAdmin.aspx.cs" Inherits="CustomerLeadTimesAdmin" MaintainScrollPositionOnPostback="true" %>

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
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Customer Lead Times Admin" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" ForeColor="Red">If you enter a phone number do not use dashes, spaces or brackets</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" ForeColor="Red">If you enter a website address, do not start it with http:// or https://</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server">
                            <table align="center" style="background-color: GhostWhite">
                                <tr>
                                    <td>
                                        <asp:Panel ID="pnlLeadTimes" runat="server">
                                            <asp:GridView ID="gvLeadTimes" runat="server"
                                                AutoGenerateColumns="False" CellPadding="4"
                                                ForeColor="Black" GridLines="None" OnRowCancelingEdit="gvLeadTimes_RowCancelingEdit"
                                                OnRowCommand="gvLeadTimes_RowCommand" OnRowDataBound="gvLeadTimes_RowDataBound" OnRowDeleting="gvLeadTimes_RowDeleting"
                                                OnRowEditing="gvLeadTimes_RowEditing" OnRowUpdating="gvLeadTimes_RowUpdating" ShowFooter="True" Width="1000px" Font-Names="Arial" Font-Size="10pt">
                                                <AlternatingRowStyle BackColor="White" />
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="false">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblCustomerLeadTimeID" runat="server" Text='<%# Bind("CustomerLeadTimeID") %>' Visible="false"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Customer Name">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' ForeColor="Black"></asp:Label>&nbsp;-&nbsp;
                                                            <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' ForeColor="Black"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlCustomers" runat="server" Font-Size="10pt" Width="350px" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black" AutoPostBack="true" OnSelectedIndexChanged="ddlCustomers_SelectedIndexChanged"></asp:DropDownList>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <FooterStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Adress Code">                                                       
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAddrCode" runat="server" Text='<%# Bind("AddrCode") %>' ForeColor="Black"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlAddressCodes" runat="server" Font-Size="10pt" Width="175px" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black"></asp:DropDownList>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                        <FooterStyle HorizontalAlign="Left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Lead<br>Time Value<br> Type">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlLeadTimeValueTypes" runat="server" Font-Size="10pt" Width="130px" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem>HOURS</asp:ListItem>
                                                                <asp:ListItem>DAYS</asp:ListItem>
                                                                <asp:ListItem>WEEKS</asp:ListItem>
                                                                <asp:ListItem>MONTHS</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lblLeadTimeValueType" runat="server" Text='<%# Bind("LeadTimeValueType") %>' Visible="false"></asp:Label>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeadTimeValueType" runat="server" Text='<%# Bind("LeadTimeValueType") %>' ForeColor="Black"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlLeadTimeValueTypes" runat="server" Font-Size="10pt" Width="130px" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem>HOURS</asp:ListItem>
                                                                <asp:ListItem>DAYS</asp:ListItem>
                                                                <asp:ListItem>WEEKS</asp:ListItem>
                                                                <asp:ListItem>MONTHS</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <FooterStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Lead <br>Time <br>Value">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtLeadTimeValue" runat="server" Text='<%# Bind("LeadTimeValue") %>' BackColor="LemonChiffon" ForeColor="Black" CssClass="form-control input-sm" Width="100px" placeholder="Time Amount"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeadTimeValue" runat="server" Text='<%# Bind("LeadTimeValue") %>' ForeColor="Black"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtLeadTimeValue" runat="server" Text='<%# Bind("LeadTimeValue") %>' BackColor="LemonChiffon" ForeColor="Black" CssClass="form-control input-sm" Width="100px" placeholder="Time Amount"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <FooterStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Contact Type">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <EditItemTemplate>
                                                            <asp:DropDownList ID="ddlContactTypes" runat="server" Font-Size="10pt" Width="130px" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem>EMAIL</asp:ListItem>
                                                                <asp:ListItem>PHONE</asp:ListItem>
                                                                <asp:ListItem>WEBSITE</asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:Label ID="lblContactType" runat="server" Text='<%# Bind("ContactType") %>' Visible="false"></asp:Label>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblContactType" runat="server" Text='<%# Bind("ContactType") %>' ForeColor="Black"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:DropDownList ID="ddlContactTypes" runat="server" Font-Size="10pt" Width="130px" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black">
                                                                <asp:ListItem Value="0">Select</asp:ListItem>
                                                                <asp:ListItem>EMAIL</asp:ListItem>
                                                                <asp:ListItem>PHONE</asp:ListItem>
                                                                <asp:ListItem>WEBSITE</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <FooterStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Lead <br>Time <br>Source">
                                                        <HeaderStyle CssClass="CenterAligner" />
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="txtLeadTimeSource" runat="server" Text='<%# Bind("LeadTimeSource") %>' BackColor="LemonChiffon" ForeColor="Black" CssClass="form-control input-sm" Width="250px" placeholder="Enter email, website or phone"></asp:TextBox>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblLeadTimeSource" runat="server" Text='<%# Bind("LeadTimeSource") %>' ForeColor="Black"></asp:Label>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:TextBox ID="txtLeadTimeSource" runat="server" Text='<%# Bind("LeadTimeSource") %>' BackColor="LemonChiffon" ForeColor="Black" CssClass="form-control input-sm" Width="250px" placeholder="Enter email, website or phone"></asp:TextBox>
                                                        </FooterTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <FooterStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <EditItemTemplate>
                                                            <asp:LinkButton ID="lbnUpdate" runat="server" CausesValidation="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Update" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Update" CssClass="btn btn-success btn-xs"></asp:LinkButton>
                                                            <ajaxToolkit:ConfirmButtonExtender ID="lbnUpdate_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to update?" Enabled="True" TargetControlID="lbnUpdate"></ajaxToolkit:ConfirmButtonExtender>
                                                             <asp:LinkButton ID="lbnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Cancel" CssClass="btn btn-warning btn-xs"></asp:LinkButton>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbnEdit" runat="server" CausesValidation="False" CommandName="Edit" Font-Bold="True" Font-Size="11pt" Text="Edit" CssClass="btn btn-info btn-sm"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbnDelete" runat="server" CausesValidation="False" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delete" Font-Bold="True" Font-Size="11pt" Text="Delete" CssClass="btn btn-danger btn-sm"></asp:LinkButton>
                                                            <ajaxToolkit:ConfirmButtonExtender ID="lbnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you ABSOLUTELY SURE you want to delete?" Enabled="True" TargetControlID="lbnDelete"></ajaxToolkit:ConfirmButtonExtender>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <asp:LinkButton ID="lbnAdd" runat="server" CausesValidation="False" CommandName="Add" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Add" CssClass="btn btn-success btn-sm"></asp:LinkButton>
                                                            <ajaxToolkit:ConfirmButtonExtender ID="lbnAdd_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you Sure you want to add this?" Enabled="True"
                                                                TargetControlID="lbnAdd"></ajaxToolkit:ConfirmButtonExtender>
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
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Arial" Font-Size="12pt"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
