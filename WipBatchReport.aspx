<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="WipBatchReport.aspx.cs" Inherits="WipBatchReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .Prompt {
            color: navy;
            font-weight: bold;
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
            <table align="center" width="900">
                <tr>
                    <td align="center">&nbsp;</td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Batch Report" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td style="height: 350px">
                        <asp:Panel ID="pnlSearchCriteria" runat="server">
                            <table width="1100" class="JustRoundedEdgeBothSmall" style="background-color: lightblue;">
                                <tr>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label13" runat="server" Font-Bold="True" ForeColor="Black" Text="Stock Code" Font-Names="Arial"></asp:Label>&nbsp;
                                                    <asp:Label ID="lblDescription" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Navy" Font-Names="Arial"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 450px">
                                                    <asp:Label ID="Label12" runat="server" Text="Date(s)" Font-Bold="True" Font-Names="Arial" ForeColor="Black"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" align="left">

                                                    <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server"
                                                        CompletionSetCount="25" CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes"
                                                        ServicePath="" TargetControlID="txtStockCode" UseContextKey="True" CompletionInterval="0">
                                                    </ajaxToolkit:AutoCompleteExtender>
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtStockCode" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtStockCode_TextChanged" ValidationGroup="" Width="350px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="Label14" runat="server" Font-Bold="True" ForeColor="Black" Text="Job Count" Font-Names="Arial"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:TextBox ID="txtJobCount" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtStockCode_TextChanged" ValidationGroup="" Width="350px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Size="9pt" ForeColor="Red" Text="*Leave blank for Date Range" Font-Names="Arial"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td valign="top" align="left">
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 100px">
                                                                <asp:Label ID="lblPeriod" runat="server" Text="Period:" Font-Bold="True" Font-Names="Arial" ForeColor="Black"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 350px">
                                                                <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="10pt" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" ForeColor="Black">
                                                                    <asp:ListItem Selected="True">All</asp:ListItem>
                                                                    <asp:ListItem>Range</asp:ListItem>
                                                                    <asp:ListItem>Last 3 Months</asp:ListItem>
                                                                    <asp:ListItem>Last 6 Months</asp:ListItem>
                                                                    <asp:ListItem>Last 9 Months</asp:ListItem>
                                                                    <asp:ListItem>Last 12 Months</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100px">
                                                                <asp:Label ID="lblStartDate" runat="server" Text="From:" Font-Bold="True" Font-Names="Arial" ForeColor="Black"></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtStartDate" runat="server"
                                                                    ValidationGroup="" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" AutoPostBack="True" OnTextChanged="txtStartDate_TextChanged"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server"
                                                                    Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgStartDate"
                                                                    TargetControlID="txtStartDate"></ajaxToolkit:CalendarExtender>
                                                                <ajaxToolkit:MaskedEditExtender ID="txtStartDateMEE" runat="server"
                                                                    Mask="99/99/9999"
                                                                    MaskType="Date"
                                                                    MessageValidatorTip="true"
                                                                    InputDirection="LeftToRight"
                                                                    TargetControlID="txtStartDate"></ajaxToolkit:MaskedEditExtender>
                                                                <ajaxToolkit:MaskedEditValidator ID="txtStartDateMEV" runat="server"
                                                                    ControlExtender="txtStartDateMEE"
                                                                    ControlToValidate="txtStartDate"
                                                                    Display="None"
                                                                    ErrorMessage="Invalid date format."
                                                                    InvalidValueMessage="Invalid date format."
                                                                    IsValidEmpty="true"
                                                                    EmptyValueMessage="Please enter a start date."
                                                                    TooltipMessage="Please enter a date."
                                                                    MinimumValue="01/01/1901"
                                                                    MinimumValueMessage="Date must be greater than 01/01/1901" />
                                                                <ajaxToolkit:ValidatorCalloutExtender ID="txtStartDateVCE" runat="server"
                                                                    TargetControlID="txtStartDateMEV"
                                                                    HighlightCssClass="validatorCalloutHighlight" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:ImageButton ID="imgStartDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 100px">
                                                                <asp:Label ID="lblEndDate" runat="server" Text="To:" Font-Bold="True" Font-Names="Arial" ForeColor="Black"></asp:Label>
                                                            </td>
                                                            <td align="right">
                                                                <asp:TextBox ID="txtEndDate" runat="server"
                                                                    ValidationGroup="" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" AutoPostBack="True" OnTextChanged="txtEndDate_TextChanged"></asp:TextBox>
                                                                <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server"
                                                                    Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgEndDate"
                                                                    TargetControlID="txtEndDate"></ajaxToolkit:CalendarExtender>
                                                                <ajaxToolkit:MaskedEditExtender ID="txtEndDateMEE" runat="server"
                                                                    Mask="99/99/9999"
                                                                    MaskType="Date"
                                                                    MessageValidatorTip="true"
                                                                    InputDirection="LeftToRight"
                                                                    TargetControlID="txtEndDate"></ajaxToolkit:MaskedEditExtender>
                                                                <ajaxToolkit:MaskedEditValidator ID="txtEndDateMEV" runat="server"
                                                                    ControlExtender="txtEndDateMEE"
                                                                    ControlToValidate="txtEndDate"
                                                                    Display="None"
                                                                    ErrorMessage="Invalid date format."
                                                                    InvalidValueMessage="Invalid date format."
                                                                    IsValidEmpty="true"
                                                                    EmptyValueMessage="Please enter an end date."
                                                                    TooltipMessage="Please enter a date."
                                                                    MinimumValue="01/01/1901"
                                                                    MinimumValueMessage="Date must be greater than 01/01/1901" />
                                                                <ajaxToolkit:ValidatorCalloutExtender ID="txtEndDateVCE" runat="server"
                                                                    TargetControlID="txtEndDateMEV"
                                                                    HighlightCssClass="validatorCalloutHighlight" />
                                                            </td>
                                                            <td align="left">
                                                                <asp:ImageButton ID="imgEndDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">&nbsp;</td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <br />
                        <asp:RadioButtonList ID="rblPeriodList" runat="server" ForeColor="Navy" RepeatDirection="Horizontal" Visible="False" Width="450px" ToolTip="You must change your period selection before clicking the Preview button.">
                            <asp:ListItem Selected="True">&nbsp;Current Year</asp:ListItem>
                            <asp:ListItem>&nbsp;Previous Year</asp:ListItem>
                            <asp:ListItem>&nbsp;Last 12 Months</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:LinkButton ID="lbnInitializeStockCode" runat="server" CssClass="btn btn-danger input-xs" Font-Names="Arial" Font-Size="10pt" ForeColor="Red" OnClick="lbnInitializeStockCode_Click" Visible="False" Width="77px">Initialize</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td align="center">

                        <asp:LinkButton ID="btnPreview" runat="server" OnClick="btnPreview_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Click to run summary report"><i class="fa fa-file-text-o" ></i>&nbsp;Run Report</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblError" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="10pt" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left"></td>
                </tr>
                <tr>
                    <td align="center">
                        <table>
                            <tr>
                                <td align="left">
                                    <asp:GridView ID="gvReportCondensed" runat="server" AutoGenerateColumns="False" Font-Size="10pt"
                                        GridLines="Vertical" Width="1010px" ShowFooter="false"
                                        BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                        BorderWidth="1px" ForeColor="Black" OnRowDataBound="gvReportCondensed_RowDataBound" OnRowCommand="gvReportCondensed_RowCommand">
                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Stock Code">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbnStockCode" runat="server" Text='<%# Bind("ParentPart") %>'
                                                        CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                        CausesValidation="False" ToolTip="Click to view Details."
                                                        CommandName="Select"
                                                        Width="185px"
                                                        ForeColor="Navy"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("ParentDescription") %>' Width="225px" Font-Size="8pt"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDate" runat="server" Text='<%# Bind("ParentCompleteDate") %>' Width="75px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Parent<br/>Job">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblParentJob" runat="server" Text='<%# Bind("ParentJob") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblParentJobTruncated" runat="server" Text='<%# Bind("ParentJob") %>' Width="70px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Uom">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUom" runat="server" Text='<%# Bind("ParentUom") %>' Width="40px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Bind("ParentQtyManuf") %>' Width="70px"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                        </Columns>
                                        <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" VerticalAlign="Bottom" HorizontalAlign="Center" />
                                        <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div id="divPopup1">
                <asp:Button ID="button1" runat="server" Style="display: none" Text="Button" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderDetails1" runat="server" BackgroundCssClass="popup_Details1"
                    CancelControlID="" DynamicServicePath="" Enabled="True" PopupControlID="pnlDetails" TargetControlID="Button1">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlDetails" runat="server" CssClass="popup_PopupDetails1" Visible="true" Style="display: none; padding: 10px">
                    <table style="border: medium solid #000000; background-color: WhiteSmoke;" width="100%">
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblDetailsForJob" runat="server" ForeColor="Black"></asp:Label></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Panel ID="pnlDetailsgv" runat="server" Width="1075px">
                                    <asp:GridView ID="gvDetails1" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                        BorderWidth="1px" Font-Size="10pt" ForeColor="Black" GridLines="Vertical" OnRowCommand="gvDetails1_RowCommand"
                                        OnRowDataBound="gvDetails1_RowDataBound" ShowFooter="false" Width="1075px">
                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Stock Code">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbnStockCode" runat="server" CausesValidation="False" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>"
                                                        CommandName="Select" Text='<%# Bind("RecipeStockCode") %>' ToolTip="Click to view Details."
                                                        ForeColor="Navy" Width="100px"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("RecipeDescription") %>' Font-Size="8pt"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Recipe<br/>Job">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRecipeJob" runat="server" Text='<%# Bind("RecipeJob") %>' Style="text-align: right"></asp:Label>
                                                    <asp:Label ID="lblRecipeJobHidden" runat="server" Text='<%# Bind("RecipeJob") %>' Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Uom">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUom" runat="server" Text='<%# Bind("RecipeUom") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Qty">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQty" runat="server" Text='<%# Bind("RecipeQtyIssued") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pkg<br/>Uom">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPkgUom" runat="server" Text='<%# Bind("StockUom") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pkg<br/>Qty">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPckQty" runat="server" Text='<%# Bind("PckQty") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <HeaderStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                        </Columns>
                                        <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" />
                                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-info" OnClick="btnCancel_Click" Text="Back" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div id="divPopup2">
                <asp:Button ID="button2" runat="server" Style="display: none" Text="Button" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderDetails2" runat="server" BackgroundCssClass="popup_Details2"
                    CancelControlID="" DynamicServicePath="" Enabled="True" PopupControlID="pnlDetails2" TargetControlID="Button2">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlDetails2" runat="server" CssClass="popup_PopupDetails2" Visible="true" Style="display: none; padding: 10px">
                    <table style="border: medium solid #000000; background-color: WhiteSmoke;" width="100%">
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblDetailsForJob2" runat="server" ForeColor="Black"></asp:Label>
                                <asp:Label ID="lblJobQty" runat="server" ForeColor="Black" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Panel ID="pnlDetailsgv2" runat="server" Width="1050px">
                                    <asp:GridView ID="gvDetails2" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                        BorderWidth="1px" Font-Size="10pt" ForeColor="Black" GridLines="Vertical"
                                        OnRowDataBound="gvDetails2_RowDataBound" ShowFooter="True" Width="1050px">
                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Stock Code">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblStockCode" runat="server" CausesValidation="False" Text='<%# Bind("RecipeStockCode") %>'
                                                        ForeColor="Navy" Width="75px"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblTotal" runat="server" Text=""></asp:Label>
                                                    <br />
                                                    <asp:Label ID="lblCostPerPoundLabel" runat="server" Text=""></asp:Label>

                                                </FooterTemplate>
                                                <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Left" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Description">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("RecipeDescription") %>' Width="180px" Font-Size="8pt"></asp:Label>
                                                </ItemTemplate>
                                                <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Uom">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIngredUom" runat="server" Text='<%# Bind("RecipeUom") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" />
                                            </asp:TemplateField>




                                            <asp:TemplateField HeaderText="LB/GAL">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPanQty1" runat="server" Text='<%# Bind("PanQty1") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Ounces">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPanQty2" runat="server" Text='<%# Bind("PanQty2") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Recipe<br/>Qty<br/>Issued">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIngredQtyIssued" runat="server" Text='<%# Bind("RecipeQtyIssued") %>'></asp:Label>
                                                    <asp:Label ID="lblParentQtyManuf" runat="server" Text='<%# Bind("ParentQtyManuf") %>' ForeColor="Black" Visible="False"></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:Label ID="lblIngredQtyIssuedTotal" runat="server" Text="" Visible="false"></asp:Label>
                                                </FooterTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="What<br>If<br/>Qty<br/>Var.">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCheckQty" runat="server" Text='<%# Bind("CheckQty") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                            <%--Hidden fields--%>
                                            <asp:TemplateField HeaderText="PanSize" Visible="false">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPanSize" runat="server" Text='<%# Bind("PanSize") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="BOM Qty" Visible="False">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBOMQty" runat="server" Text='<%# Bind("BOMQty") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="QtyPer" Visible="False">
                                                <HeaderStyle CssClass="CenterAligner" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblQtyPer" runat="server" Text='<%# Bind("QtyPer") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <FooterStyle BackColor="#333333" ForeColor="White" Font-Bold="True" HorizontalAlign="Right" />
                                            </asp:TemplateField>

                                        </Columns>
                                        <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Bottom" />
                                        <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                        <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                        <SortedDescendingHeaderStyle BackColor="#242121" />
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnCancel2" runat="server" CssClass="btn btn-info" OnClick="btnCancel2_Click" Text="Back" />
                                <asp:Button ID="btnCloseAll" runat="server" CssClass="btn btn-info" OnClick="btnCloseAll_Click" Text="Back to Start" />

                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

