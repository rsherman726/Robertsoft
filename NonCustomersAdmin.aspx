<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="NonCustomersAdmin.aspx.cs" Inherits="NonCustomersAdmin" MaintainScrollPositionOnPostback="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
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
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="End Customer Admin" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table width="600" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: lightblue"
                                            align="center" class="JustRoundedEdgeBothSmall">
                                            <tr>
                                                <td style="width: 5px">&nbsp;</td>
                                                <td style="width: 210px">&nbsp;</td>
                                                <td style="width: 300px">&nbsp;</td>
                                                <td style="width: 300px">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 5px">&nbsp;</td>
                                                <td style="width: 210px">&nbsp;</td>
                                                <td style="width: 300px">
                                                    <asp:RadioButtonList ID="rblMode" runat="server" AutoPostBack="True" Font-Bold="True" RepeatDirection="Horizontal" OnSelectedIndexChanged="rblMode_SelectedIndexChanged" Width="200px" ForeColor="Black">
                                                        <asp:ListItem>&nbsp;Add</asp:ListItem>
                                                        <asp:ListItem Selected="True">&nbsp;Edit</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td style="width: 300px">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 5px">&nbsp;</td>
                                                <td align="center" style="width: 210px">
                                                    &nbsp;</td>
                                                <td align="left" style="width: 300px">
                                                    <asp:DropDownList ID="ddlNonCustomersMain" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" ForeColor="Black" OnSelectedIndexChanged="ddlNonCustomers_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Value="0">Select a Non Customer</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" style="width: 300px">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 5px">&nbsp;</td>
                                                <td style="width: 210px">&nbsp;</td>
                                                <td align="center" style="width: 300px">
                                                    <asp:Label ID="lblNonCustomerIDDisplay" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                                </td>
                                                <td align="center" style="width: 300px">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 5px">&nbsp;</td>
                                                <td align="left" style="width: 210px">
                                                    <asp:Label ID="Label36" runat="server" Font-Bold="True" Text="END CUSTOMER NAME:" ForeColor="Black" Width="175px"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 300px">
                                                    <asp:TextBox ID="txtNonCustomerName" runat="server" BackColor="LemonChiffon"  CssClass="form-control" Font-Size="12pt" ForeColor="Black"></asp:TextBox>
                                                </td>
                                                <td align="left" style="width: 300px">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="left" style="width: 5px">&nbsp;</td>
                                                <td align="left" style="width: 210px">
                                                    <asp:Label ID="Label68" runat="server" Font-Bold="True" ForeColor="Black" Text="SALESPERSON"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 300px">
                                                    <asp:DropDownList ID="ddlSalesPerson" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black">
                                                        <asp:ListItem Selected="True">All</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td align="left" style="width: 300px">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 5px">&nbsp;</td>
                                                <td style="width: 210px"></td>
                                                <td align="center" style="width: 300px">
                                                    <asp:Button ID="ibnSave" runat="server" Text="save" OnClick="ibnSave_Click" ValidationGroup="emp" Enabled="False" CssClass="btn btn-primary" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnSave_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnSave">
                                                    </ajaxToolkit:ConfirmButtonExtender>
                                                    <asp:Button ID="ibnDelete" runat="server" Enabled="False" Text="delete" OnClick="ibnDelete_Click" ValidationGroup="emp" CssClass="btn btn-primary" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnDelete">
                                                    </ajaxToolkit:ConfirmButtonExtender>
                                                    <asp:Button ID="ibnAdd" runat="server" Enabled="False" Text="add" OnClick="ibnAdd_Click" ValidationGroup="emp" CssClass="btn btn-primary" />
                                                    <ajaxToolkit:ConfirmButtonExtender ID="ibnAdd_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Is all the information correct?" Enabled="True" TargetControlID="ibnAdd">
                                                    </ajaxToolkit:ConfirmButtonExtender>
                                                </td>
                                                <td align="center" style="width: 300px">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td style="width: 5px">&nbsp;</td>
                                                <td style="width: 210px">&nbsp;</td>
                                                <td align="center" style="width: 300px">
                                                    &nbsp;</td>
                                                <td align="center" style="width: 300px">&nbsp;</td>
                                            </tr>
                                        </table>
                    </td>
                </tr>
                <tr><td align="center">
                    <asp:Label ID="lblErrorEditor" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                    </td></tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="Label37" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="Black" Text="Assign Stock Codes"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server">
                            <table align="center" style="background-color: GhostWhite">
                                <tr>
                                    <td>
                                        <asp:Button ID="btnDisableEnter" runat="server" Text="" OnClientClick="return false;" Style="display: none;" />
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
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        
                                           <asp:GridView ID="gvNonCustomer" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="Black" GridLines="None" OnPageIndexChanging="gvNonCustomer_PageIndexChanging" OnRowCancelingEdit="gvNonCustomer_RowCancelingEdit"
                                            OnRowCommand="gvNonCustomer_RowCommand" OnRowDataBound="gvNonCustomer_RowDataBound" OnRowDeleting="gvNonCustomer_RowDeleting"
                                            OnRowEditing="gvNonCustomer_RowEditing" OnRowUpdating="gvNonCustomer_RowUpdating" ShowFooter="True" Width="1000px" Font-Names="Arial" Font-Size="11pt" PageSize="25">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField ShowHeader="false">
                                                    <ItemTemplate>
                                                     <%--   <asp:Label ID="lblNonCustomerID" runat="server" Font-Size="12pt" Text='<%# Bind("NonCustomerID") %>' Visible="false"></asp:Label>--%>
                                                        <asp:Label ID="lblNonCustomerStockCodeAssignID" runat="server" Font-Size="12pt" Text='<%# Bind("NonCustomerStockCodeAssignID") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Non Customer Name">
                                                    <EditItemTemplate>
                                                         <asp:DropDownList ID="ddlNonCustomers" runat="server" Font-Size="12pt" Width="350px" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black" ></asp:DropDownList>
                                                        <asp:Label ID="lblNonCustomerID" runat="server" Text='<%# Bind("NonCustomerID") %>' Visible="false"></asp:Label>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' ForeColor="Black" ></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                          <asp:DropDownList ID="ddlNonCustomersAdd" runat="server" Font-Size="12pt" Width="350px" BackColor="LemonChiffon" CssClass="form-control"  ForeColor="Black" ></asp:DropDownList>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stock Code">
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlStockCodes" runat="server" Font-Size="12pt" Width="350px" BackColor="LemonChiffon" ForeColor="Black"  CssClass="form-control">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblStockDescription" runat="server" Text='<%# Bind("StockDescription") %>' Visible="false"></asp:Label>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' ForeColor="Black" ></asp:Label>&nbsp;-&nbsp;
                                                        <asp:Label ID="lblStockDescription" runat="server" Text='<%# Bind("StockDescription") %>' ForeColor="Black" ></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlStockCodesAdd" runat="server" Font-Size="12pt" Width="350px" CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black" >
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lbnUpdate" runat="server" CausesValidation="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Update" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Update"></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnUpdate_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to update?" Enabled="True" TargetControlID="lbnUpdate">
                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                        &nbsp;<asp:LinkButton ID="lbnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Cancel"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbnEdit" runat="server" CausesValidation="False" CommandName="Edit" Font-Bold="True" Font-Size="11pt" Text="Edit" ForeColor="Black"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbnDelete" runat="server" CausesValidation="False" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delete" Font-Bold="True" Font-Size="11pt" Text="Delete" ForeColor="Blue"></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you ABSOLUTELY SURE you want to delete?" Enabled="True" TargetControlID="lbnDelete">
                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lbnAdd" runat="server" CausesValidation="False" CommandName="Add" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Add"></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnAdd_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you Sure you want to add this?" Enabled="True" TargetControlID="lbnAdd">
                                                        </ajaxToolkit:ConfirmButtonExtender>
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

