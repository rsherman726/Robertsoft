<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PriceAnalysisSpreadsheet.aspx.cs" Inherits="PriceAnalysisSpreadsheet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#MainContent_gvSpreadsheet tr').click(function (e) {
                $('#MainContent_gvSpreadsheet tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            }); 

        });

        function isPostBack() {//Use during postbacks...
            $('#MainContent_gvSpreadsheet tr').click(function (e) {
                $('#MainContent_gvSpreadsheet tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            }); 

        }; //End Postback... 

    </script>
    <style type="text/css">
        tr.highlighted td {
            background: #ffd77d;
        }

        tbody tr.selected td {
            background: none repeat scroll 0 0 #FFCF8B;
            color: #f00;
        }

        .rowColors tr:hover {
            background-color: #05b3f5 !important;
        }

        .rowColors tr:hover {
            color: #FFF !important;
            transform-st !important;
        }

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
            <div>
                <div>
                    <table align="center" width="1200">
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Price Analysis Export" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="background-color: lightblue;" align="center" class="JustRoundedEdgeBothSmall">
                                <asp:Panel ID="pnlSearchCriteria" runat="server" Width="1000px">
                                    <table width="1000px">
                                        <tr>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td valign="top" align="left">
                                                            <table width="100%">
                                                                <tr>
                                                                    <td>
                                                                        <table>
                                                                            <tr>
                                                                                <td align="center">&nbsp;</td>
                                                                                <td align="center">
                                                                                    <asp:Label ID="lblStockCodeList" runat="server" Font-Bold="True" ForeColor="Navy" Text="Key Ingredient Stock Code(s)"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center">&nbsp;</td>
                                                                                <td align="center">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center" style="cursor: pointer" valign="top">&nbsp;</td>
                                                                                <td align="left" style="cursor: pointer" valign="top">
                                                                                    <asp:ListBox ID="lbKeyIngredientStockCodes" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="9pt" ForeColor="Black" Height="250px" Style="width: 350px !important"></asp:ListBox>
                                                                                    <ajaxToolkit:ListSearchExtender ID="lbKeyIngredientStockCodes_ListSearchExtender" runat="server" Enabled="True" IsSorted="True" PromptCssClass="Prompt" TargetControlID="lbKeyIngredientStockCodes">
                                                                                    </ajaxToolkit:ListSearchExtender>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center" valign="top"></td>
                                                                                <td align="center" valign="top">
                                                                                    <asp:LinkButton ID="lbClearAll" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbClearAll_Click" ValidationGroup="nothing">Clear Selection</asp:LinkButton>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center" valign="top">&nbsp;</td>
                                                                                <td align="center" valign="top">&nbsp;</td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center" valign="top">&nbsp;</td>
                                                                                <td align="center" valign="top">
                                                                                    <asp:Label ID="label22" runat="server" Font-Bold="True" ForeColor="Black" Text="Estimated Cost Filters"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center" valign="top">&nbsp;</td>
                                                                                <td align="center" valign="top">
                                                                                    <asp:Label ID="label23" runat="server" Font-Bold="False" ForeColor="Black" Text="Shrinkage %"></asp:Label>

                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center" valign="top">&nbsp;</td>
                                                                                <td align="center" valign="top">
                                                                                    <asp:TextBox ID="txtShrinkagePercentage" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" placeholder="Shrinkage %"
                                                                                        onkeypress="return OnlyNumberAftertwoDigits(event,this.id);" ValidationGroup=""></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center" valign="top">&nbsp;</td>
                                                                                <td align="center" valign="top">
                                                                                    <asp:Label ID="label75" runat="server" Font-Bold="False" ForeColor="Black" Text="Labor % Increase"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center" valign="top">&nbsp;</td>
                                                                                <td align="center" valign="top">
                                                                                    <asp:TextBox ID="txtLaborPercentage" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" placeholder="Labor % Increase"
                                                                                        onkeypress="return OnlyNumberAftertwoDigits(event,this.id);" ValidationGroup=""></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                        <td valign="top" align="left">&nbsp;</td>
                                                        <td align="left" valign="top">
                                                            <table style="width: 550px">
                                                                <tr>
                                                                    <td style="width: 115px">
                                                                        <asp:Label ID="lblPeriod" runat="server" Font-Bold="True" ForeColor="Black" Text="Period:"></asp:Label>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="10pt" ForeColor="Black" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged">
                                                                            <asp:ListItem Selected="True">Range</asp:ListItem>
                                                                            <asp:ListItem>Last 3 Months</asp:ListItem>
                                                                            <asp:ListItem>Last 6 Months</asp:ListItem>
                                                                            <asp:ListItem>Last 9 Months</asp:ListItem>
                                                                            <asp:ListItem>Last 12 Months</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="left" style="width: 60px">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="width: 100px">
                                                                        <asp:Label ID="lblStartDate" runat="server" Font-Bold="True" ForeColor="Black" Text="From:"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:TextBox ID="txtStartDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup=""></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgStartDate" TargetControlID="txtStartDate"></ajaxToolkit:CalendarExtender>
                                                                        <ajaxToolkit:MaskedEditExtender ID="txtStartDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtStartDate"></ajaxToolkit:MaskedEditExtender>
                                                                        <ajaxToolkit:MaskedEditValidator ID="txtStartDateMEV" runat="server" ControlExtender="txtStartDateMEE" ControlToValidate="txtStartDate" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtStartDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtStartDateMEV" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:ImageButton ID="imgStartDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="lblEndDate" runat="server" Font-Bold="True" ForeColor="Black" Text="To:"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:TextBox ID="txtEndDate" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup=""></asp:TextBox>
                                                                        <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgEndDate" TargetControlID="txtEndDate"></ajaxToolkit:CalendarExtender>
                                                                        <ajaxToolkit:MaskedEditExtender ID="txtEndDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtEndDate"></ajaxToolkit:MaskedEditExtender>
                                                                        <ajaxToolkit:MaskedEditValidator ID="txtEndDateMEV" runat="server" ControlExtender="txtEndDateMEE" ControlToValidate="txtEndDate" Display="None" EmptyValueMessage="Please enter an end date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtEndDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtEndDateMEV" />
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:ImageButton ID="imgEndDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="Label14" runat="server" Font-Bold="True" ForeColor="Black" Text="Customer:"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:ListBox ID="lbCustomers" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black" Height="200px" SelectionMode="Multiple"></asp:ListBox>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:RadioButtonList ID="rblSort" runat="server" AutoPostBack="True" Font-Bold="True" Font-Names="Arial" Font-Size="8pt" ForeColor="Black" OnSelectedIndexChanged="rblSort_SelectedIndexChanged" Width="115px">
                                                                            <asp:ListItem Selected="True" Value="Name">&nbsp;Sort By Name</asp:ListItem>
                                                                            <asp:ListItem Value="Number">&nbsp;Sort By Number</asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">&nbsp;</td>
                                                                    <td align="center">
                                                                        <asp:Label ID="Label74" runat="server" Font-Bold="False" Font-Size="9pt" ForeColor="Navy" Text="(Use CTRL for Multiple Selection) "></asp:Label>
                                                                    </td>
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">&nbsp;</td>
                                                                    <td align="center">
                                                                        <asp:LinkButton ID="lbSelectAllCustomer" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbSelectAllCustomer_Click" ValidationGroup="nothing">Select All</asp:LinkButton>
                                                                        &nbsp;<asp:LinkButton ID="lbClearAllCustomer" runat="server" Font-Size="11pt" ForeColor="Navy" OnClick="lbClearAllCustomer_Click" ValidationGroup="nothing">Clear All</asp:LinkButton>
                                                                    </td>
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="Label70" runat="server" Font-Bold="True" ForeColor="Black" Text="End Customer:" Width="110px"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:DropDownList ID="ddlEndCustomers" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="9pt" ForeColor="Black" OnSelectedIndexChanged="ddlEndCustomers_SelectedIndexChanged">
                                                                            <asp:ListItem Selected="True">All</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" style="width: 100px">
                                                                        <asp:Label ID="Label71" runat="server" Font-Bold="True" ForeColor="Black" Text="End Customer&lt;br&gt;StockCodes:" Width="125px"></asp:Label>
                                                                    </td>
                                                                    <td align="right">
                                                                        <asp:ListBox ID="lbParentStockCode" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="9pt" ForeColor="Black" Height="200px" SelectionMode="Multiple" Style="width: 350px !important"></asp:ListBox>
                                                                        <ajaxToolkit:ListSearchExtender ID="lbParentStockCode_ListSearchExtender" runat="server" Enabled="True" IsSorted="True" PromptCssClass="Prompt" TargetControlID="lbParentStockCode">
                                                                        </ajaxToolkit:ListSearchExtender>
                                                                    </td>
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>

                                                    <tr>
                                                        <td align="center" valign="top">
                                                            <asp:CheckBox ID="chkEndCustomer" runat="server" Font-Size="11pt" ForeColor="Black" Text="End Customer" ToolTip="For the Price Analysis with Customer" AutoPostBack="True" OnCheckedChanged="chkEndCustomer_CheckedChanged" />
                                                            &nbsp;
                                                            <br />
                                                        </td>
                                                        <td align="left" valign="top">&nbsp;</td>
                                                        <td align="left" valign="top">
                                                            <asp:Label ID="Label1" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="9pt" ForeColor="Red">** Recommendation: short date ranges, i.e. 3 Month period take 3.5 minutes to run</asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table width="600px">
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="btnPreview" runat="server" OnClick="btnPreview_Click" CssClass="btn btn-info" Width="250px" ForeColor="White" ToolTip="Click to run Price Analysis"><i class="fa fa-file-text-o" ></i>&nbsp;Price Analysis</asp:LinkButton>
                                                        </td>
                                                        <td>

                                                            <asp:LinkButton ID="btnPreviewWithCost" runat="server" CssClass="btn btn-info" ForeColor="White" OnClick="btnPreviewWithCost_Click" ToolTip="Click to run Price Analysis With Estimated Cost" Width="250px"><i class="fa fa-file-text-o"></i>&nbsp;Price Analysis w/Estimated Cost</asp:LinkButton>

                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="btnCustomerReport" runat="server" OnClick="btnCustomerReport_Click" CssClass="btn btn-info" Width="250px" ForeColor="White" ToolTip="Click to run Price Analysis with Customer"><i class="fa fa-file-text-o" ></i>&nbsp;Price Analysis with Customer</asp:LinkButton>

                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="btnReset" runat="server" OnClick="btnReset_Click" CssClass="btn btn-danger" Width="125px" ForeColor="White" ToolTip="Click to reset form"><i class="fa fa-refresh" ></i>&nbsp;Reset Form</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="imgExportExcelPriceAnalysis" runat="server" CssClass="btn btn-success" OnClick="imgExportExcelPriceAnalysis_Click" ToolTip="Export Full Price Analysis Report" Width="250px"><i class="glyphicon glyphicon-export"></i>&nbsp;Export To Excel</asp:LinkButton>
                                                        </td>
                                                        <td>

                                                            <asp:LinkButton ID="imgExportExcelPriceAnalysisWithCost" runat="server" CssClass="btn btn-success" OnClick="imgExportExcelPriceAnalysisWithCost_Click"   ToolTip="Export Full Price Analysis Report" Width="250px"><i class="glyphicon glyphicon-export"></i>&nbsp;Export To Excel</asp:LinkButton>

                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="imgExportExcelPriceAnalysisWithCustomer" runat="server" CssClass="btn btn-success" OnClick="imgExportExcelPriceAnalysisWithCustomer_Click" ToolTip="Export Price Analysis Report With Customers" Width="250px"><i class="glyphicon glyphicon-export"></i>&nbsp;Export To Excel</asp:LinkButton>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                </table>





                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblError" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trFullReport" runat="server">
                            <td align="center">

                                <asp:Label ID="lblRecordCount" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="14pt" ForeColor="Navy"></asp:Label>

                            </td>
                        </tr>
                        <tr id="trSummary" runat="server" class="rowColors">

                            <td align="center">

                                <asp:GridView ID="gvSpreadsheet" runat="server" ForeColor="Black" Font-Size="8pt">
                                </asp:GridView>
                            </td>
                        </tr>

                    </table>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgExportExcelPriceAnalysis" />
            <asp:PostBackTrigger ControlID="imgExportExcelPriceAnalysisWithCost" />
            <asp:PostBackTrigger ControlID="imgExportExcelPriceAnalysisWithCustomer" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

