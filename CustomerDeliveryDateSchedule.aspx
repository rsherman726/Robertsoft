<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CustomerDeliveryDateSchedule.aspx.cs" Inherits="CustomerDeliveryDateSchedule" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .CenterAligner {
            text-align: center;
        }

        .gridViewPager td {
            padding-left: 6px;
            padding-right: 6px;
            padding-top: 1px;
            padding-bottom: 2px;
        }

        .content {
            margin: 0 auto;
            width: 760px;
            background: #ffffff;
            border: 1px solid #dadada;
            border-radius: 6px;
            margin-bottom: 50px;
            padding: 45px;
            padding-top: 25px;
            position: relative;
        }

        #box {
            width: 300px;
            height: 100px;
            text-align: center;
            vertical-align: middle;
            border: 2px solid #ff6a00;
            background-color: #ffd800;
            padding: 15px;
            font-family: Arial;
            font-size: 16px;
            margin-top: 35px;
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
            <table align="Center" width="1000" class="JustRoundedEdgeBoth" style="background-color: GhostWhite">
                <tr>
                    <td align="center">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Customer Delivery Schedule" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblError0" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-info" OnClick="btnUpdate_Click" Text="Update" />
                      <ajaxToolkit:ConfirmButtonExtender ID="btnUpdate_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with Update?" Enabled="True" TargetControlID="btnUpdate">
                         </ajaxToolkit:ConfirmButtonExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server">
                            <table align="center" style="background-color: GhostWhite">
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:GridView ID="gvCustomerDeliverySchedule" runat="server"
                                            AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="Black" GridLines="None"
                                            OnRowDataBound="gvCustomerDeliverySchedule_RowDataBound"
                                            OnRowDeleting="gvCustomerDeliverySchedule_RowDeleting"
                                            ShowFooter="True" Width="1050px" Font-Names="Arial" Font-Size="11pt" OnRowCommand="gvCustomerDeliverySchedule_RowCommand">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField ShowHeader="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomerDeliveryScheduleID" runat="server" Font-Size="12pt" Text='<%# Bind("CustomerDeliveryScheduleID") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Customer">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>'></asp:Label>
                                                    </ItemTemplate>                                                   
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                    
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                     <FooterTemplate>
                                                        <asp:DropDownList ID="ddlCustomers" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Left" />
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" width="300px"/>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Schedule">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:CheckBoxList ID="cblSchedule" runat="server" RepeatDirection="Horizontal" Width="500px">
                                                            <asp:ListItem>&nbsp;Monday</asp:ListItem>
                                                            <asp:ListItem>&nbsp;Tuesday</asp:ListItem>
                                                            <asp:ListItem>&nbsp;Wednesday</asp:ListItem>
                                                            <asp:ListItem>&nbsp;Thursday</asp:ListItem>
                                                            <asp:ListItem>&nbsp;Friday</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                        <asp:Label ID="lblMonday" runat="server" Text='<%# Bind("Monday") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblTuesday" runat="server" Text='<%# Bind("Tuesday") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblWednesday" runat="server" Text='<%# Bind("Wednesday") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblThursday" runat="server" Text='<%# Bind("Thursday") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblFriday" runat="server" Text='<%# Bind("Friday") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:CheckBoxList ID="cblSchedule" runat="server" RepeatDirection="Horizontal" Width="500px">
                                                            <asp:ListItem>&nbsp;Monday</asp:ListItem>
                                                            <asp:ListItem>&nbsp;Tuesday</asp:ListItem>
                                                            <asp:ListItem>&nbsp;Wednesday</asp:ListItem>
                                                            <asp:ListItem>&nbsp;Thursday</asp:ListItem>
                                                            <asp:ListItem>&nbsp;Friday</asp:ListItem>
                                                        </asp:CheckBoxList>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="center" />
                                                </asp:TemplateField>

                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbnDelete" runat="server" CausesValidation="False" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delete" Font-Bold="True" Font-Size="11pt" Text="Delete" ForeColor="Blue"></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with delete?" Enabled="True" TargetControlID="lbnDelete">
                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lbnAdd" runat="server" CausesValidation="False" CommandName="Add" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Add"></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnAdd_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with add?" Enabled="True" TargetControlID="lbnAdd">
                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#2461BF" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" Font-Bold="true" Font-Names="arial" Font-Size="12pt" HorizontalAlign="center" CssClass="gridViewPager" />
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
                        <asp:Button ID="btnUpdate0" runat="server" CssClass="btn btn-info" OnClick="btnUpdate_Click" Text="Update" />
                        <ajaxToolkit:ConfirmButtonExtender ID="btnUpdate0_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with Update?" Enabled="True" TargetControlID="btnUpdate0">
                         </ajaxToolkit:ConfirmButtonExtender>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Arial" Font-Size="12pt"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

