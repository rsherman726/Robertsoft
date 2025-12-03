<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="IngredientCostHistory.aspx.cs" Inherits="IngredientCostHistory" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 226px;
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
            <table align="center" width="1100px">
                <tr>
                    <td align="center">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Ingredient Cost History  Report" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <table align="center" width="1100px">
                            <tr>
                                <td>
                                    <asp:Panel ID="PnlContent" runat="server">
                                        <table width="1100px">
                                            <tr>
                                                <td align="center" class="JustRoundedEdgeBothSmall" style="background-color: lightgreen; width: 1100px">
                                                    <asp:Panel ID="pnlSearchCriteriaChart1" runat="server">
                                                        <table width="1100px">
                                                            <tr>
                                                                <td align="center">
                                                                    <table>
                                                                        <tr>
                                                                            <td align="center">&nbsp;</td>
                                                                            <td align="center" style="width: 450px">&nbsp;</td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center">&nbsp;</td>
                                                                            <td align="center" style="width: 450px">
                                                                                <table width="1000px">
                                                                                    <tr>
                                                                                        <td align="left" style="width: 200px">
                                                                                            <asp:Label ID="lblPeriod" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="Period:"></asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="10pt" ForeColor="Black" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged" Width="300px">
                                                                                                <asp:ListItem Selected="True">ALL</asp:ListItem>
                                                                                                <asp:ListItem>Range</asp:ListItem>
                                                                                                <asp:ListItem>Last 3 Months</asp:ListItem>
                                                                                                <asp:ListItem>Last 6 Months</asp:ListItem>
                                                                                                <asp:ListItem>Last 9 Months</asp:ListItem>
                                                                                                <asp:ListItem>Last 12 Months</asp:ListItem>
                                                                                                <asp:ListItem>Last 2 Years</asp:ListItem>
                                                                                                <asp:ListItem>Last 3 Years</asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblStartDate" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="Date From:" Width="90px"></asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <ajaxToolkit:MaskedEditValidator ID="txtStartDateMEV" runat="server" ControlExtender="txtStartDateMEE" ControlToValidate="txtStartDate" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                                                            <ajaxToolkit:ValidatorCalloutExtender ID="txtStartDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtStartDateMEV" />
                                                                                            <asp:TextBox ID="txtStartDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="275px"></asp:TextBox>
                                                                                            <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgStartDate" TargetControlID="txtStartDate" />
                                                                                            <ajaxToolkit:MaskedEditExtender ID="txtStartDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtStartDate" />
                                                                                        </td>
                                                                                        <td align="left" style="width: 5px">
                                                                                            <asp:ImageButton ID="imgStartDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblEndDate" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Text="Date To:" Width="70px"></asp:Label>
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <ajaxToolkit:MaskedEditValidator ID="txtEndDateMEV" runat="server" ControlExtender="txtEndDateMEE" ControlToValidate="txtEndDate" Display="None" EmptyValueMessage="Please enter an end date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                                                            <ajaxToolkit:ValidatorCalloutExtender ID="txtEndDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtEndDateMEV" />
                                                                                            <asp:TextBox ID="txtEndDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="275px"></asp:TextBox>
                                                                                            <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgEndDate" TargetControlID="txtEndDate" />
                                                                                            <ajaxToolkit:MaskedEditExtender ID="txtEndDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtEndDate" />
                                                                                        </td>
                                                                                        <td align="left" style="width: 5px">
                                                                                            <asp:ImageButton ID="imgEndDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left">
                                                                                            <br />
                                                                                        </td>
                                                                                        <td align="right">&nbsp;</td>
                                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                                        <td align="left" style="width: 5px">&nbsp;</td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center">&nbsp;</td>
                                                                            <td align="center" style="width: 450px">
                                                                                <table width="1150px">
                                                                                    <tr>
                                                                                        <td style="width: 400px"> <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" OnTextChanged="txtStockCodeChartIngredient_TextChanged" placeholder="Enter a StockCode or Partial Description" ValidationGroup="" Width="500px"></asp:TextBox></td>
                                                                                        <td align="left"> <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Font-Bold="True" OnClick="btnSearch_Click" Text="Search" />
                                                                                <ajaxToolkit:AutoCompleteExtender ID="txtStockCodeSingle_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes" ServicePath="" TargetControlID="txtSearch" UseContextKey="True">
                                                                                </ajaxToolkit:AutoCompleteExtender></td>
                                                                                        <td></td>
                                                                                        <td align="center">
                                                                                            <asp:CheckBox ID="chkShowChart" runat="server" AutoPostBack="True" ForeColor="Black" OnCheckedChanged="chkShowChart_CheckedChanged" Text="&nbsp;Show in Chart" />
                                                                                        </td>
                                                                                        <td align="center">
                                                                                            <asp:LinkButton ID="btnPreviewChart1" runat="server" CssClass="btn btn-info" ForeColor="White" OnClick="btnPreviewChart1_Click" ToolTip="Click to run" Width="200px"><i class="fa fa-file-text-o"></i>&nbsp;Run Report</asp:LinkButton>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:ListBox ID="lbStockCode" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Size="9pt" ForeColor="Black" Height="150px" OnSelectedIndexChanged="lbStockCode_SelectedIndexChanged" Style="width: 500px !important" Width="400px"></asp:ListBox>
                                                                                            <ajaxToolkit:ListSearchExtender ID="lbStockCode_ListSearchExtender" runat="server" Enabled="True" IsSorted="True" PromptCssClass="Prompt" TargetControlID="lbStockCode">
                                                                                            </ajaxToolkit:ListSearchExtender>
                                                                                        </td>
                                                                                        <td>&nbsp;</td>
                                                                                        <td>&nbsp;</td>
                                                                                        <td>&nbsp;</td>
                                                                                        <td>&nbsp;</td>
                                                                                    </tr>
                                                                                </table>
                                                                               
                                                                               
                                                                            </td>
                                                                        </tr>                                                                        
                                                                        <tr>
                                                                            <td valign="top" align="left">&nbsp;</td>
                                                                            <td valign="top" align="left">
                                                                                <asp:Label ID="lblError" runat="server" EnableViewState="False" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">&nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Panel ID="pnlChart" runat="server" Visible="false">
                                                        <table align="center">
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Label ID="Label49" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Black" Text="Ingredient Cost History Chart"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:Chart ID="Chart1" runat="server" BackColor="#F3DFC1" BackGradientStyle="TopBottom" BorderColor="181, 64, 1" BorderlineDashStyle="Solid" BorderWidth="2" Height="500px"
                                                                        ImageLocation="~/WebCharts/ChartPic_#SEQ(300,3)" ImageType="Png" Palette="BrightPastel" Width="1100px" Visible="False">
                                                                        <Legends>
                                                                            <asp:Legend IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold" Enabled="False"></asp:Legend>
                                                                        </Legends>
                                                                        <BorderSkin SkinStyle="Emboss" />
                                                                        <Series>
                                                                            <asp:Series Name="UnitCost" BorderColor="180, 26, 59, 105" BorderWidth="11" ChartType="Line" ShadowColor="64, 0, 0, 0" ShadowOffset="2">
                                                                                <Points>
                                                                                    <asp:DataPoint YValues="10" />
                                                                                    <asp:DataPoint YValues="24" />
                                                                                    <asp:DataPoint YValues="27" />
                                                                                    <asp:DataPoint YValues="41" />
                                                                                    <asp:DataPoint YValues="57" />
                                                                                    <asp:DataPoint YValues="107" />
                                                                                    <asp:DataPoint YValues="65" />
                                                                                    <asp:DataPoint YValues="42" />
                                                                                </Points>
                                                                            </asp:Series>
                                                                            <asp:Series Name="LastCost" BorderColor="120, 26, 22, 165" BorderWidth="11" ChartType="Line" ShadowColor="64, 0, 0, 0" ShadowOffset="2">
                                                                                <Points>
                                                                                    <asp:DataPoint YValues="45" />
                                                                                    <asp:DataPoint YValues="34" />
                                                                                    <asp:DataPoint YValues="67" />
                                                                                    <asp:DataPoint YValues="31" />
                                                                                    <asp:DataPoint YValues="27" />
                                                                                    <asp:DataPoint YValues="87" />
                                                                                    <asp:DataPoint YValues="45" />
                                                                                    <asp:DataPoint YValues="32" />
                                                                                </Points>
                                                                            </asp:Series>
                                                                        </Series>
                                                                        <ChartAreas>
                                                                            <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackColor="WhiteSmoke" ShadowColor="Transparent">
                                                                                <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False" WallWidth="0" IsClustered="False"></Area3DStyle>
                                                                                <AxisY LineColor="64, 64, 64, 64" IsLabelAutoFit="False" Title="Cost">
                                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="{c}" />
                                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                </AxisY>
                                                                                <AxisY2 LineColor="64, 64, 64, 64" IsLabelAutoFit="False" Title="Cost">
                                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="{c}" />
                                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                </AxisY2>
                                                                                <AxisX LineColor="64, 64, 64, 64" IsLabelAutoFit="False">
                                                                                    <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" Format="MM/YYYY" Interval="Auto" />
                                                                                    <MajorGrid LineColor="64, 64, 64, 64" />
                                                                                </AxisX>
                                                                            </asp:ChartArea>
                                                                        </ChartAreas>
                                                                    </asp:Chart>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center"></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:CheckBox ID="chk3D" runat="server" AutoPostBack="True" OnCheckedChanged="chk3D_CheckedChanged" Text="&nbsp;Show in 3D" Visible="False" ForeColor="Black" />&nbsp;
                                                                    <asp:CheckBox ID="chkShowLabels" runat="server" AutoPostBack="True" ForeColor="Black" OnCheckedChanged="chkShowLabels_CheckedChanged" Text="&nbsp;Show in Labels" Visible="False" /></td>
                                                            </tr>
                                                        </table>

                                                    </asp:Panel>

                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="500px">
                                        <asp:GridView ID="gv1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="1000px" AutoGenerateColumns="False" OnRowDataBound="gv1_RowDataBound">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Stock Code">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="UOM">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockUOM" runat="server" Text='<%# Bind("StockUOM") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("EntryDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Qty">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTrnQty" runat="server" Text='<%# Bind("TrnQty") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Avg Cost">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnitCost" runat="server" Text='<%# Bind("UnitCost") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Cost">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLastCost" runat="server" Text='<%# Bind("LastCost") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Alt UOM">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAlternateUom" runat="server" Text='<%# Bind("AlternateUom") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Alt UOM Cost">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAltUOMCost" runat="server" Text='<%# Bind("AltUOMCost") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Freight In">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFreight" runat="server" Text='<%# Bind("Freight") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Tariff" Visible="false">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblTariff" runat="server" Text='<%# Bind("Tariff") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Misc">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblMisc" runat="server" Text='<%# Bind("Misc") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Grand Total">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblGrandTotal" runat="server" Text='<%# Bind("GrandTotal") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" CssClass="CenterAligner" />
                                            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                            <SortedAscendingCellStyle BackColor="#FDF5AC" />
                                            <SortedAscendingHeaderStyle BackColor="#4D0000" />
                                            <SortedDescendingCellStyle BackColor="#FCF6C0" />
                                            <SortedDescendingHeaderStyle BackColor="#820000" />
                                        </asp:GridView>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:LinkButton ID="imgExportExcel" runat="server" CssClass="btn btn-success" ForeColor="White" OnClick="imgExportExcel_Click" Width="200px"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgExportExcel" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

