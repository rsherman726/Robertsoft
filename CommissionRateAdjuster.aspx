<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CommissionRateAdjuster.aspx.cs" Inherits="CommissionRateAdjuster" %>

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
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Commission Rate Adjuster" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        &nbsp;</td>
                </tr>
                <tr><td align="center">
                    <asp:Label ID="lblErrorEditor" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                    </td></tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="Label37" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="Black" Text="Stockcodes - Rates"></asp:Label>
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
                                                        <asp:ListItem Selected="True">10</asp:ListItem>
                                                        <asp:ListItem>15</asp:ListItem>
                                                        <asp:ListItem>20</asp:ListItem>
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
                                        
                                           <asp:GridView ID="gvStockcodeExclusions" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="Black" GridLines="None" OnPageIndexChanging="gvStockcodeExclusions_PageIndexChanging" OnRowCancelingEdit="gvStockcodeExclusions_RowCancelingEdit"
                                            OnRowCommand="gvStockcodeExclusions_RowCommand" OnRowDataBound="gvStockcodeExclusions_RowDataBound" OnRowDeleting="gvStockcodeExclusions_RowDeleting"
                                            OnRowEditing="gvStockcodeExclusions_RowEditing" OnRowUpdating="gvStockcodeExclusions_RowUpdating" ShowFooter="True" Width="1000px" Font-Names="Arial" Font-Size="11pt" PageSize="25">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField ShowHeader="false">
                                                    <ItemTemplate>                                                      
                                                        <asp:Label ID="lblCommStockCodeOverrideID" runat="server" Font-Size="12pt" Text='<%# Bind("CommStockCodeOverrideID") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stock Code">
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlStockCodes" runat="server" Font-Size="10pt" Width="350px" BackColor="LemonChiffon"  CssClass="form-control" ForeColor="Black" >
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblStockDescription" runat="server" Text='<%# Bind("StockDescription") %>' Visible="false"></asp:Label>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' ForeColor="Black" ></asp:Label>&nbsp;-&nbsp;
                                                        <asp:Label ID="lblStockDescription" runat="server" Text='<%# Bind("StockDescription") %>' ForeColor="Black" ></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlStockCodesAdd" runat="server" Font-Size="10pt" Width="350px" CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black" >
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Commission Percentage">
                                                    <EditItemTemplate>
                                                         <asp:TextBox ID="txtComPercentage" runat="server" Font-Size="12pt" Width="200px" Text='<%# Bind("ComPercentage") %>' BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black" ></asp:TextBox>                                                       
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblComPercentage" runat="server" Text='<%# Bind("ComPercentage") %>' ForeColor="Black" ></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                          <asp:TextBox ID="txtComPercentageAdd" runat="server" Font-Size="12pt" Width="200px" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black" ></asp:TextBox>    
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
                <tr>
                    <td align="center">
                        <asp:Label ID="Label38" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="Black" Text="Salesperson - Customer -  Rates"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table width="100%">
                            <tr>
                                <td align="left" style="width: 100px">
                                    <asp:Label ID="lblResultsPerPage0" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ForeColor="Black" Text="Results per page:" Width="125px"></asp:Label>
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlDisplayCount0" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control input-sm" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" ForeColor="Black" OnSelectedIndexChanged="ddlDisplayCount0_SelectedIndexChanged" Width="100px">
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem Selected="True">10</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
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
                </tr>
                <tr>
                    <td align="left">
                        <asp:GridView ID="gvSalespersonCustomerExclusions" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" CellPadding="4" Font-Names="Arial" Font-Size="11pt" 
                            ForeColor="Black" GridLines="None" OnPageIndexChanging="gvStockcodeExclusions_PageIndexChanging" 
                            OnRowCancelingEdit="gvSalespersonCustomerExclusions_RowCancelingEdit" OnRowCommand="gvSalespersonCustomerExclusions_RowCommand" 
                            OnRowDataBound="gvSalespersonCustomerExclusions_RowDataBound" OnRowDeleting="gvSalespersonCustomerExclusions_RowDeleting" OnRowEditing="gvSalespersonCustomerExclusions_RowEditing" OnRowUpdating="gvSalespersonCustomerExclusions_RowUpdating" PageSize="25" ShowFooter="True" Width="1000px">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField ShowHeader="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCommSalespersonCustomerOverrideID" runat="server" Font-Size="12pt" Text='<%# Bind("CommSalespersonCustomerOverrideID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Salesperson">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlSalesperson" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="10pt" ForeColor="Black" Width="350px">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblSalesperson" runat="server" Text='<%# Bind("Salesperson") %>' Visible="false"></asp:Label>                                        
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSalespersonName" runat="server" ForeColor="Black" Text='<%# Bind("SalespersonName") %>'></asp:Label>                                       
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlSalespersonAdd" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="10pt" ForeColor="Black" Width="350px">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <FooterStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Customer">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddlCustomer" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="10pt" ForeColor="Black" Width="350px">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Visible="false"></asp:Label>                                        
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCustomerName" runat="server" ForeColor="Black" Text='<%# Bind("CustomerName") %>'></asp:Label>                                       
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:DropDownList ID="ddlCustomerAdd" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="10pt" ForeColor="Black" Width="350px">
                                        </asp:DropDownList>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <FooterStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Commission Percentage">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtComPercentage" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" ForeColor="Black" Text='<%# Bind("ComPercentage") %>' Width="200px"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblComPercentage" runat="server" ForeColor="Black" Text='<%# Bind("ComPercentage") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:TextBox ID="txtComPercentageAdd" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="12pt" ForeColor="Black" Width="200px"></asp:TextBox>
                                    </FooterTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                    <HeaderStyle HorizontalAlign="Left" />
                                    <FooterStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lbnUpdate" runat="server" CausesValidation="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Update" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Update"></asp:LinkButton>
                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnUpdate_ConfirmButtonExtender0" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to update?" Enabled="True" TargetControlID="lbnUpdate">
                                        </ajaxToolkit:ConfirmButtonExtender>
                                        &nbsp;<asp:LinkButton ID="lbnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Cancel"></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbnEdit0" runat="server" CausesValidation="False" CommandName="Edit" Font-Bold="True" Font-Size="11pt" ForeColor="Black" Text="Edit"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbnDelete" runat="server" CausesValidation="False" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delete" Font-Bold="True" Font-Size="11pt" ForeColor="Blue" Text="Delete"></asp:LinkButton>
                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnDelete_ConfirmButtonExtender0" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you ABSOLUTELY SURE you want to delete?" Enabled="True" TargetControlID="lbnDelete">
                                        </ajaxToolkit:ConfirmButtonExtender>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:LinkButton ID="lbnAdd" runat="server" CausesValidation="False" CommandName="Add" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Add"></asp:LinkButton>
                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnAdd_ConfirmButtonExtender0" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you Sure you want to add this?" Enabled="True" TargetControlID="lbnAdd">
                                        </ajaxToolkit:ConfirmButtonExtender>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" CssClass="gridViewPager" Font-Bold="true" Font-Size="12pt" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="lblPageNo0" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>         
    </asp:UpdatePanel>
</asp:Content>

