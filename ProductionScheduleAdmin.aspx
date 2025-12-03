<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProductionScheduleAdmin.aspx.cs" Inherits="ProductionScheduleAdmin" %>

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
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Production Schedule Admin" ForeColor="Black"></asp:Label>
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

                                        <asp:GridView ID="gvProductionSchedule" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="Black" GridLines="None" OnPageIndexChanging="gvProductionSchedule_PageIndexChanging" OnRowCancelingEdit="gvProductionSchedule_RowCancelingEdit"
                                            OnRowCommand="gvProductionSchedule_RowCommand" OnRowDataBound="gvProductionSchedule_RowDataBound" OnRowDeleting="gvProductionSchedule_RowDeleting"
                                            OnRowEditing="gvProductionSchedule_RowEditing" OnRowUpdating="gvProductionSchedule_RowUpdating" ShowFooter="True" Width="1000px" Font-Names="Arial" Font-Size="11pt" PageSize="25">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField ShowHeader="false">
                                                    <ItemTemplate>

                                                        <asp:Label ID="lblProductionScheduleID" runat="server" Font-Size="12pt" Text='<%# Bind("ProductionScheduleID") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Stock Code">
                                                    <EditItemTemplate>
                                                        <asp:DropDownList ID="ddlStockCodes" runat="server" Font-Size="12pt" Width="350px" BackColor="LemonChiffon" ForeColor="Black">
                                                        </asp:DropDownList>
                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblStockDescription" runat="server" Text='<%# Bind("StockDescription") %>' Visible="false"></asp:Label>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' ForeColor="Black"></asp:Label>&nbsp;-&nbsp;
                                                        <asp:Label ID="lblStockDescription" runat="server" Text='<%# Bind("StockDescription") %>' ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:DropDownList ID="ddlStockCodesAdd" runat="server" Font-Size="12pt" Width="350px" CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black">
                                                        </asp:DropDownList>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Line">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtLine" runat="server" Text='<%# Bind("Line") %>' CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLine" runat="server" Text='<%# Bind("Line") %>' ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtLineAdd" runat="server" Text='<%# Bind("Line") %>' CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Quantity">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtQuantity" runat="server" Text='<%# Bind("Quantity") %>' CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("Quantity") %>' ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtQuantityAdd" runat="server" Text='<%# Bind("Quantity") %>' CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Comment">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtComment" runat="server" Text='<%# Bind("Comment") %>' CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblComment" runat="server" Text='<%# Bind("Comment") %>' ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtCommentAdd" runat="server" Text='<%# Bind("Comment") %>' CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black"></asp:TextBox>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Schedule Date">
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtScheduledDate" runat="server" Text='<%# Bind("ScheduledDate") %>' CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="txtScheduledDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgScheduledDate" TargetControlID="txtScheduledDate">
                                                        </ajaxToolkit:CalendarExtender>
                                                        <ajaxToolkit:MaskedEditExtender ID="txtScheduledDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtScheduledDate">
                                                        </ajaxToolkit:MaskedEditExtender>
                                                        <ajaxToolkit:MaskedEditValidator ID="txtScheduledDateMEV" runat="server" ControlExtender="txtScheduledDateMEE" ControlToValidate="txtScheduledDate" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtScheduledDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtScheduledDateMEV" />
                                                        <asp:ImageButton ID="imgScheduledDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblScheduledDate" runat="server" Text='<%# Bind("ScheduledDate") %>' ForeColor="Black"></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtScheduledDateAdd" runat="server" Text='<%# Bind("ScheduledDate") %>' CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black"></asp:TextBox>
                                                        <ajaxToolkit:CalendarExtender ID="txtScheduledDateAdd_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgScheduledDateAdd" TargetControlID="txtScheduledDateAdd">
                                                        </ajaxToolkit:CalendarExtender>
                                                        <ajaxToolkit:MaskedEditExtender ID="txtScheduledDateAddMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtScheduledDateAdd">
                                                        </ajaxToolkit:MaskedEditExtender>
                                                        <ajaxToolkit:MaskedEditValidator ID="txtScheduledDateAddMEV" runat="server" ControlExtender="txtScheduledDateAddMEE" ControlToValidate="txtScheduledDateAdd" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtScheduledDateAddVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtScheduledDateAddMEV" />
                                                        <asp:ImageButton ID="imgScheduledDateAdd" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                    </FooterTemplate>
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

