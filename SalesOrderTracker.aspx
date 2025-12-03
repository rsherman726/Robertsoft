<%@ Page Title="Sales Order Tracker" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SalesOrderTracker.aspx.cs" Inherits="SalesOrderTracker" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.4.1/css/all.css" integrity="sha384-5sAR7xN1Nv6T6+dT2mhtzEpVJvfS3NScPQTrOxhwjIuvcA67KV2R5Jz6kr4abQsz" crossorigin="anonymous" />
    <style type="text/css">
        .NoUnderline {
            text-decoration: none;
        }

        .CenterAligner {
            text-align: center !important;
        }

        .autocomplete_highlightedListItem {
            background-color: #ffff99;
            color: black;
            padding: 1px;
            font-size: 14pt;
            font-weight: bold;
        }
        /* AutoComplete item */
        .autocomplete_listItem {
            background-color: window;
            color: windowtext;
            padding: 1px;
            font-size: 14pt;
            font-weight: bold;
        }

        .gridview td {
            padding: 5px;
        }
    </style>
    <script type="text/javascript">
        // It is important to place this JavaScript code after ScriptManager1
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function BeginRequestHandler(sender, args) {
            if ($get('<%=pnlGridView.ClientID%>') != null) {
                // Get X and Y positions of scrollbar before the partial postback
                xPos = $get('<%=pnlGridView.ClientID%>').scrollLeft;
                yPos = $get('<%=pnlGridView.ClientID%>').scrollTop;
            }
        }

        function EndRequestHandler(sender, args) {
            if ($get('<%=pnlGridView.ClientID%>') != null) {
                // Set X and Y positions back to the scrollbar
                // after partial postback
                $get('<%=pnlGridView.ClientID%>').scrollLeft = xPos;
                $get('<%=pnlGridView.ClientID%>').scrollTop = yPos;
            }
        }

        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler);
    </script>
    <script type="text/javascript">


        function isPostBack() {//Use during postbacks...
            $('#MainContent_gvRecord tr').click(function (e) {
                $('#MainContent_gvRecord tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });
            $('#MainContent_gvDetails tr').click(function (e) {
                $('#MainContent_gvDetails tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });


        }; //End Postback... 

    </script>
    <style type="text/css">
        .rowColors tr:hover {
            background-color: #05b3f5 !important;
            transition-property: background;
            transition-duration: 100ms;
            transition-delay: 5ms;
            transition-timing-function: linear;
            transform-st !important;
        }

        tr.highlighted td {
            background: #05b3f5;
        }

        tbody tr.selected td {
            background: none repeat scroll 0 0 #FFCF8B;
            color: #f00;
        }
    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
        <ContentTemplate>
            <div style="position: fixed; right: 50px; height: 40px; width: 150px; z-index: 10000;">
                <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="UpdatePanelPromo">
                    <ProgressTemplate>
                        <table style="border: medium solid #000080; width: 100%; background-color: white;">
                            <tr>
                                <td align="right" style="width: 12px;">
                                    <img src="Images/Loader.gif" alt="" />
                                </td>
                                <td><span style="color: #ffffff"><span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing  <span class="">....</span> </strong></span></span></td>
                            </tr>
                        </table>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <table id="tblMain" runat="server" align="Center">
                <tr>
                    <td align="center">
                        <table width="100%">
                            <tr>
                                <td class="hdr" align="center">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                        Text="Open Orders - The Sales Order Tracker" ForeColor="Black"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlMain" runat="server">
                                        <table width="100%" align="center">
                                            <tr>
                                                <td style="line-height: 15px">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">

                                                    <table width="300" align="center" id="tblTimer">
                                                        <tr>
                                                            <td style="width: 125px">
                                                                <asp:CheckBox ID="chkTimerOnOff" runat="server" Font-Size="9pt" Text="&nbsp;Auto Refresh" AutoPostBack="True" OnCheckedChanged="chkTimerOnOff_CheckedChanged" Checked="True" Width="125px" ForeColor="Black" /></td>
                                                            <td align="center" style="width: 110px">
                                                                <asp:DropDownList ID="ddlInterval" runat="server" Font-Bold="True" Font-Size="9pt" CssClass="form-control input-sm" Width="120px" ForeColor="Black" AutoPostBack="True" OnSelectedIndexChanged="ddlInterval_SelectedIndexChanged">
                                                                    <asp:ListItem Value="20000">20 Seconds</asp:ListItem>
                                                                    <asp:ListItem Value="30000">30 Seconds</asp:ListItem>
                                                                    <asp:ListItem Value="40000">40 Seconds</asp:ListItem>
                                                                    <asp:ListItem Value="50000">50 Seconds</asp:ListItem>
                                                                    <asp:ListItem Value="60000">1 Minute</asp:ListItem>
                                                                    <asp:ListItem Value="90000">1.5 Minutes</asp:ListItem>
                                                                    <asp:ListItem Value="120000">2 Minutes</asp:ListItem>
                                                                    <asp:ListItem Value="300000">5 Minutes</asp:ListItem>
                                                                    <asp:ListItem Value="600000">10 Minutes</asp:ListItem>
                                                                    <asp:ListItem Value="1800000" Selected="True">30 Minutes</asp:ListItem>
                                                                    <asp:ListItem Value="3600000">60 Minutes</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>


                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="line-height: 5px">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblTimeUpdated" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="9pt" ForeColor="#FF0066" Width="300px"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:RadioButtonList ID="rblCompton" runat="server" ForeColor="Navy" RepeatDirection="Horizontal" Width="325px" AutoPostBack="True" OnSelectedIndexChanged="rblCompton_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Value="Standard View">&amp;nbsp;Standard View</asp:ListItem>
                                                        <asp:ListItem Value="Compton View">&amp;nbsp;Compton View</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <span style="font-weight: bold; font-family: arial, Helvetica, sans-serif; color: black;">Projected Delivery Month Totals</span></td>
                                            </tr>
                                            <tr>
                                                <td>&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:GridView ID="gvSummary" runat="server" AutoGenerateColumns="true" ForeColor="Black" BackColor="White">
                                                        <HeaderStyle CssClass="CenterAligner" BackColor="Black" ForeColor="White" HorizontalAlign="Center" />
                                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                                        <RowStyle CssClass="CenterAligner" ForeColor="Black" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblGrandTotalSummary" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="16pt" ForeColor="Navy"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Panel ID="pnlPlacedOrdersSummary" runat="server">
                                                        <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Navy" Text="These counts are for Orders Placed reguardless of status"></asp:Label>
                                                        <table width="700px">
                                                            <tr>
                                                                <td align="left" style="width: 250px">&nbsp;</td>
                                                                <td align="center" style="width: 95px">
                                                                    <asp:Label ID="Label26" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Navy" Text="Order Count"></asp:Label>
                                                                </td>
                                                                <td style="width: 95px" align="center">
                                                                    <asp:Label ID="Label31" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Navy" Text="Line Count"></asp:Label>
                                                                </td>
                                                                <td align="center" style="width: 125px">
                                                                    <asp:Label ID="Label27" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Navy" Text="Total Value"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label23" runat="server" Font-Size="11pt" ForeColor="Black" Text="Today's Orders:"></asp:Label>
                                                                    <asp:Label ID="lblToday" runat="server" Font-Bold="True" Font-Size="9pt" ForeColor="Black"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblOrderCountToday" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblLineCountToday" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblOrderAmountToday" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label24" runat="server" Font-Size="11pt" ForeColor="Black" Text="Yesterday's Orders:"></asp:Label>
                                                                    <asp:Label ID="lblYesterday" runat="server" Font-Bold="True" Font-Size="9pt" ForeColor="Black"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblOrderCountYesterday" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblLineCountYesterday" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblOrderAmountYesterday" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:Label ID="Label25" runat="server" Font-Size="11pt" ForeColor="Black" Text="This Month's Orders:"></asp:Label>
                                                                    <asp:Label ID="lblThisMonth" runat="server" Font-Bold="True" Font-Size="9pt" ForeColor="Black"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblOrderCountThisMonth" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblLineCountThisMonth" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black"></asp:Label>
                                                                </td>
                                                                <td align="center">
                                                                    <asp:Label ID="lblOrderAmountThisMonth" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black"></asp:Label>
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
                                                    <asp:LinkButton ID="btnPlacedOrdersByDateRange" runat="server" CssClass="btn btn-info" ForeColor="White" OnClick="btnPlacedOrdersByDateRange_Click" ToolTip="Click to run Placed Order by Date Range report" Width="300px"><i class="fa fa-file-text-o"></i>&nbsp;Placed Order by Date Range Report</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblError0" runat="server" Font-Bold="True" Font-Size="16pt" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Panel ID="pnlTopFifteen" runat="server">
                                                        <ajaxToolkit:CollapsiblePanelExtender ID="CollapsiblePanelExtenderAddCards" runat="server"
                                                            CollapseControlID="PnlTitle" Collapsed="True" CollapsedImage="~/Images/Add-icon.png"
                                                            CollapsedText="" ExpandControlID="PnlTitle" ExpandedImage="~/Images/minus-icon.png"
                                                            ExpandedText="" ImageControlID="ImageExpand1" SuppressPostBack="true"
                                                            TargetControlID="PnlContent" TextLabelID="Label1"></ajaxToolkit:CollapsiblePanelExtender>
                                                        <asp:Panel ID="PnlTitle" runat="server" CssClass="collapsePanelHeader" Width="100%"
                                                            BorderColor="Gray" ForeColor="black" BackColor="LightGray" Height="30px" HorizontalAlign="Center">
                                                            <div style="float: left">
                                                                <asp:Image ID="ImageExpand1" runat="server" ImageUrl="images/add-icon.png" Width="16px" />
                                                                &nbsp;<span style="font-family: arial, Helvetica, sans-serif; font-weight: bold; font-size: 12pt">Top 15 List</span> &nbsp;        
                                                            </div>
                                                        </asp:Panel>
                                                        <asp:Panel ID="PnlContent" runat="server">
                                                            <table align="center" width="1200">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Top 15  Report" ForeColor="Black"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td style="background-color: lightblue;" align="center" class="JustRoundedEdgeBothSmall">

                                                                        <table width="1100">
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <asp:Label ID="Label10" runat="server" Width="200px" Font-Bold="True" ForeColor="Black">Rank By</asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <asp:RadioButtonList ID="rblTopTen" runat="server" AutoPostBack="True" ForeColor="Black" OnSelectedIndexChanged="rblTopTen_SelectedIndexChanged" RepeatDirection="Horizontal" Width="600px" Font-Size="13pt">
                                                                                                    <asp:ListItem Selected="True" Value="Y">&nbsp;YTD</asp:ListItem>
                                                                                                    <asp:ListItem Value="S">&nbsp;YTD thru End of Last Month</asp:ListItem>
                                                                                                    <%--<asp:ListItem Value="L">&nbsp;Last YTD</asp:ListItem>
                                                                                                    <asp:ListItem Value="M">&nbsp;MTD</asp:ListItem>--%>
                                                                                                </asp:RadioButtonList>
                                                                                                <table>
                                                                                                    <tr>
                                                                                                        <td align="left">
                                                                                                            <asp:CheckBox ID="chkShowLastColumns" runat="server" AutoPostBack="True" ForeColor="Blue" OnCheckedChanged="chkShowLastColumns_CheckedChanged" Text="Show Open Orders and Ready to Invoice" Checked="True" Visible="False" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <%--<tr>
                                                                                                        <td align="left">
                                                                                                            <asp:CheckBox ID="chkMonthToDate" runat="server" AutoPostBack="True" ForeColor="Black" OnCheckedChanged="chkMonthToDate_CheckedChanged" Text="Show Month as MTD" />
                                                                                                        </td>
                                                                                                    </tr>--%>
                                                                                                    <tr>
                                                                                                        <td align="left">
                                                                                                            <asp:CheckBox ID="chkShowYTDthroughLastMonth" runat="server" AutoPostBack="True" ForeColor="Blue" OnCheckedChanged="chkShowYTDthroughLastMonth_CheckedChanged" Text="Show YTD though End of Last Month" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>

                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <asp:GridView ID="gvTopFifteen" runat="server" AutoGenerateColumns="False" ForeColor="Black" Width="1350px"
                                                                                                    Font-Size="10pt" BackColor="White" ShowFooter="True" OnRowDataBound="gvTopFifteen_RowDataBound"
                                                                                                    BorderStyle="Solid" BorderWidth="1px"
                                                                                                    BorderColor="Gainsboro"
                                                                                                    AllowSorting="True"
                                                                                                    OnSorting="gvTopFifteen_Sorting">
                                                                                                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                                                    <RowStyle BackColor="White" />
                                                                                                    <Columns>
                                                                                                        <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                                                                                            <HeaderStyle CssClass="CenterAligner" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="200px" Font-Bold="true" ToolTip='<%# Bind("Customer") %>' Style="cursor: pointer"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblTotals" runat="server" Text="Totals:" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>
                                                                                                        <%--<asp:TemplateField HeaderText="Cust#" Visible="false">
                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Font-Bold="true"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>--%>

                                                                                                        <asp:TemplateField HeaderText="LM" SortExpression="PreviousMonthAmount">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblPreviousMonthAmountD" runat="server" Text='<%# Bind("PreviousMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblPreviousMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="LYLM" SortExpression="LastYearPreviousMonthAmount">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblLastYearPreviousMonthAmountD" runat="server" Text='<%# Bind("LastYearPreviousMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblLastYearPreviousMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="MTD" SortExpression="CurrentMTDAmount">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblCurrentMTDAmountD" runat="server" Text='<%# Bind("CurrentMTDAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblCurrentMTDAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="LYMTD" SortExpression="LastYearCurrentMTDAmount">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblLastYearCurrentMTDAmountD" runat="server" Text='<%# Bind("LastYearCurrentMTDAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblLastYearCurrentMTDAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="CM" SortExpression="CurrentMonthAmount">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblCurrentMonthAmountD" runat="server" Text='<%# Bind("CurrentMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblCurrentMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="LYCM" SortExpression="LastYearCurrentMonthAmount">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblLastYearCurrentMonthAmountD" runat="server" Text='<%# Bind("LastYearCurrentMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblLastYearCurrentMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>
                                                                                                        <%--YTD--%>
                                                                                                        <asp:TemplateField HeaderText="YTD" SortExpression="CurrentYTDAmount">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblCurrentYTDAmountD" runat="server" Text='<%# Bind("CurrentYTDAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblCurrentYTDAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="LYTD" SortExpression="LastYTDAmount">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblLastYTDAmountD" runat="server" Text='<%# Bind("LastYTDAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblLastYTDAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="LLYTD" SortExpression="LastYTDMinusOneAmount">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblLastYTDMinusOneAmountD" runat="server" Text='<%# Bind("LastYTDMinusOneAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblLastYTDMinusOneAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="YR to YR DIFF %" SortExpression="YearToYearDiffPercentage" Visible="false">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblYearToYearDiffPercentage" runat="server" Text='<%# Bind("YearToYearDiffPercentage") %>' Width="100px"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblYearToYearDiffPercentageWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="YR to YR DIFF MINUS ONE %" SortExpression="YearToYearDiffPercentageMinusOne" Visible="false">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblYearToYearDiffPercentageMinusOne" runat="server" Text='<%# Bind("YearToYearDiffPercentageMinusOne") %>' Width="100px"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblYearToYearDiffPercentageMinusOneWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="YTD MARGIN %" SortExpression="CurrentYTDMargin">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblCurrentYTDMargin" runat="server" Text='<%# Bind("CurrentYTDMargin") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblCurrentYTDCost" runat="server" Text='<%# Bind("CurrentYTDCost") %>' Visible="false"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblCurrentYTDMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="LYTD MARGIN %" SortExpression="LastYTDMargin">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblLastYTDMargin" runat="server" Text='<%# Bind("LastYTDMargin") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblLastYTDCost" runat="server" Text='<%# Bind("LastYTDCost") %>' Visible="false"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblLastYTDAmountMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="YTD %" SortExpression="CurrentYTDAmountPercentageOfTotal">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblCurrentYTDAmountPercentageOfTotal" runat="server" Text='<%# Bind("CurrentYTDAmountPercentageOfTotal") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblCurrentYTDAmountPercentageOfTotalSum" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="LYTD %" SortExpression="LastYTDAmountPercentageOfTotal">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblLastYTDAmountPercentageOfTotal" runat="server" Text='<%# Bind("LastYTDAmountPercentageOfTotal") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblLastYTDAmountPercentageOfTotalSum" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>
                                                                                                        <%--YTD End of Month--%>
                                                                                                        <asp:TemplateField HeaderText="YTDEM" SortExpression="CurrentYTDEndOfMonthAmountD">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblCurrentYTDEndOfMonthAmountD" runat="server" Text='<%# Bind("CurrentYTDEndOfMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblCurrentYTDEndOfMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="LYTDEM" SortExpression="LastYTDEndOfMonthAmountD">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblLastYTDEndOfMonthAmountD" runat="server" Text='<%# Bind("LastYTDEndOfMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblLastYTDEndOfMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="LLYTDEM" SortExpression="LastYTDMinusOneYearEndOfMonthAmountD">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblLastYTDMinusOneYearEndOfMonthAmountD" runat="server" Text='<%# Bind("LastYTDMinusOneYearEndOfMonthAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblLastYTDMinusOneYearEndOfMonthAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="YR to YR DIFF %" SortExpression="YearToYearDiffPercentageEndOfMonth" Visible="false">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblYearToYearDiffPercentageEndOfMonth" runat="server" Text='<%# Bind("YearToYearDiffPercentageEndOfMonth") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblYearToYearDiffPercentageEndOfMonthWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="YR to YR DIFF MINUS ONE%" SortExpression="YearToYearDiffPercentageEndOfMonthMinusOne" Visible="false">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblYearToYearDiffPercentageEndOfMonthMinusOne" runat="server" Text='<%# Bind("YearToYearDiffPercentageEndOfMonthMinusOne") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblYearToYearDiffPercentageEndOfMonthMinusOneWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Current Month Margin" SortExpression="CurrentMonthMargin">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblCurrentMonthMargin" runat="server" Text='<%# Bind("CurrentMonthMargin") %>' Width="100px"></asp:Label>
                                                                                                                <asp:Label ID="lblCurrentMonthCost" runat="server" Text='<%# Bind("CurrentMonthCost") %>' Visible="false"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblCurrentMonthMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Last Month Margin" SortExpression="LastMonthMargin">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblLastMonthMargin" runat="server" Text='<%# Bind("LastMonthMargin") %>' Width="100px"></asp:Label>
                                                                                                                <asp:Label ID="lblPreviousMonthCost" runat="server" Text='<%# Bind("PreviousMonthCost") %>' Visible="false"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblLastMonthMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="YTD MARGIN %" SortExpression="CurrentYTDEndOfMonthMargin">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblCurrentYTDEndOfMonthMargin" runat="server" Text='<%# Bind("CurrentYTDEndOfMonthMargin") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblCurrentYTDEndOfMonthCost" runat="server" Text='<%# Bind("CurrentYTDEndOfMonthCost") %>' Visible="false"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblCurrentYTDEndOfMonthMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="LYTD MARGIN %" SortExpression="LastYTDEndOfMonthMargin">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblLastYTDEndOfMonthMargin" runat="server" Text='<%# Bind("LastYTDEndOfMonthMargin") %>'></asp:Label>
                                                                                                                <asp:Label ID="lblLastYTDEndOfMonthCost" runat="server" Text='<%# Bind("LastYTDEndOfMonthCost") %>' Visible="false"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblLastYTDEndOfMonthAmountMarginWeighted" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="YTD %" SortExpression="CurrentYTDEndOfMonthAmountPercentageOfTotal">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblCurrentYTDEndOfMonthAmountPercentageOfTotal" runat="server" Text='<%# Bind("CurrentYTDEndOfMonthAmountPercentageOfTotal") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblCurrentYTDEndOfMonthAmountPercentageOfTotalSum" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="LYTD %" SortExpression="LastYTDEndOfMonthAmountPercentageOfTotal">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="100px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblLastYTDEndOfMonthAmountPercentageOfTotal" runat="server" Text='<%# Bind("LastYTDEndOfMonthAmountPercentageOfTotal") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblLastYTDEndOfMonthAmountPercentageOfTotalSum" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>
                                                                                                        <%--Open Orders & Ready To Invoice --%>
                                                                                                        <asp:TemplateField HeaderText="Open&lt;br&gt; Orders<br>Not Ready&lt;br&gt;To&lt;br&gt; Invoice" SortExpression="OpenOrdersAmount">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblOpenOrdersAmountD" runat="server" Text='<%# Bind("OpenOrdersAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                                <%-- &nbsp;/&nbsp;<asp:Label ID="lblOpenOrdersAmountPercentageOfTotal" runat="server" Text='<%# Bind("OpenOrdersAmountPercentageOfTotal") %>'></asp:Label>--%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblOpenOrdersAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>

                                                                                                        <asp:TemplateField HeaderText="Ready&lt;br&gt;To&lt;br&gt; Invoice" SortExpression="ReadyToInvoiceAmount">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblReadyToInvoiceAmountD" runat="server" Text='<%# Bind("ReadyToInvoiceAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                                <%-- &nbsp;/&nbsp;<asp:Label ID="lblReadyToInvoiceAmountPercentageOfTotal" runat="server" Text='<%# Bind("ReadyToInvoiceAmountPercentageOfTotal") %>'></asp:Label>--%>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblReadyToInvoiceAmountTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="UnInvoiced Total" SortExpression="CombinedOpenOrdersAmountD">
                                                                                                            <HeaderStyle CssClass="CenterAligner" Width="150px" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblCombinedOpenOrdersAmountD" runat="server" Text='<%# Bind("CombinedOpenOrdersAmountD") %>' Font-Bold="true"></asp:Label>
                                                                                                                <asp:Label ID="lblGrandTotalCombinedOpenOrdersAmount" runat="server" Text='<%# Bind("GrandTotalCombinedOpenOrdersAmount") %>' Visible="false"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblCombinedOpenOrdersAmountDTotal" runat="server" Font-Bold="true"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                            <FooterStyle HorizontalAlign="Right" />
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                    <FooterStyle BackColor="#CCCCCC" />
                                                                                                    <HeaderStyle BackColor="Black" CssClass="CenterAligner" ForeColor="White" HorizontalAlign="Center" />
                                                                                                    <AlternatingRowStyle HorizontalAlign="Center" />
                                                                                                    <RowStyle CssClass="CenterAligner" ForeColor="Black" />
                                                                                                </asp:GridView>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>&nbsp;</td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="center">&nbsp;</td>
                                                                            </tr>
                                                                        </table>

                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">&nbsp;</td>
                                                                </tr>


                                                                <tr>
                                                                    <td align="center">&nbsp;</td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <table id="Table2" align="center">
                                                                            <tr>
                                                                                <td align="center">
                                                                                    <asp:LinkButton ID="btnExportTopFifteenList" runat="server" OnClick="btnExportTopFifteenList_Click" CssClass="btn btn-success" ForeColor="White" Width="275px"><i class="glyphicon glyphicon-export"></i>&nbsp;EXPORT TO EXCEL TOP 15 LIST</asp:LinkButton>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">&nbsp;</td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control input-lg" OnTextChanged="txtSearch_TextChanged" placeholder="Search - Enter SO, PO , Customer Name or Customer #" Width="500px" ForeColor="Black"></asp:TextBox>
                                                                <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server" CompletionInterval="0" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                                                                    CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="15" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1"
                                                                    ServiceMethod="GetCompletionListSalesOrders" ServicePath=""
                                                                    TargetControlID="txtSearch" UseContextKey="True">
                                                                </ajaxToolkit:AutoCompleteExtender>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lbnSearch" runat="server" CssClass="btn btn-success btn-lg" Font-Bold="True" OnClick="lbnSearch_Click" ToolTip="Search with Autofill"><i class="fa fa-search"></i></asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lbnReset" runat="server" CssClass="btn btn-danger btn-lg" Font-Bold="True" OnClick="lbnReset_Click" ToolTip="Reload Default Search"><i class="fa fa-refresh" ></i></asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlSort1" runat="server" ForeColor="Black" Width="200px" BackColor="LemonChiffon" CssClass="form-control">
                                                                    <asp:ListItem>Sort Option 1</asp:ListItem>
                                                                    <asp:ListItem Value="AppointmentTime ASC">Appointment Date Lo-Hi</asp:ListItem>
                                                                    <asp:ListItem Value="AppointmentTime DESC">Appointment Date Hi-Lo</asp:ListItem>
                                                                    <asp:ListItem Value="Customer ASC">Customer A-Z</asp:ListItem>
                                                                    <asp:ListItem Value="Customer DESC">Customer Z-A</asp:ListItem>
                                                                    <asp:ListItem Value="CustomerID ASC">Customer # Lo-Hi</asp:ListItem>
                                                                    <asp:ListItem Value="CustomerID DESC">Customer # Hi-Lo</asp:ListItem>
                                                                    <asp:ListItem Value="OrderStatus ASC">Order Status A-Z</asp:ListItem>
                                                                    <asp:ListItem Value="OrderStatus DESC">Order Status Z-A</asp:ListItem>
                                                                   <asp:ListItem Value="ShipViaDescription ASC">Ship Via A-Z</asp:ListItem>
                                                                    <asp:ListItem Value="ShipViaDescription DESC">Ship Via Z-A</asp:ListItem>
                                                                    <asp:ListItem Value="ReqShipDate ASC">Ship Date Lo-Hi</asp:ListItem>
                                                                    <asp:ListItem Value="ReqShipDate DESC">Ship Date Hi-Lo</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlSort2" runat="server" ForeColor="Black" Width="200px" BackColor="LemonChiffon" CssClass="form-control">
                                                                    <asp:ListItem>Sort Option 2</asp:ListItem>
                                                                    <asp:ListItem Value="AppointmentTime ASC">Appointment Date Lo-Hi</asp:ListItem>
                                                                    <asp:ListItem Value="AppointmentTime DESC">Appointment Date Hi-Lo</asp:ListItem>
                                                                    <asp:ListItem Value="Customer ASC">Customer A-Z</asp:ListItem>
                                                                    <asp:ListItem Value="Customer DESC">Customer Z-A</asp:ListItem>
                                                                    <asp:ListItem Value="CustomerID ASC">Customer # Lo-Hi</asp:ListItem>
                                                                    <asp:ListItem Value="CustomerID DESC">Customer # Hi-Lo</asp:ListItem>
                                                                    <asp:ListItem Value="OrderStatus ASC">Order Status A-Z</asp:ListItem>
                                                                    <asp:ListItem Value="OrderStatus DESC">Order Status Z-A</asp:ListItem>
                                                                   <asp:ListItem Value="ShipViaDescription ASC">Ship Via A-Z</asp:ListItem>
                                                                    <asp:ListItem Value="ShipViaDescription DESC">Ship Via Z-A</asp:ListItem>
                                                                    <asp:ListItem Value="ReqShipDate ASC">Ship Date Lo-Hi</asp:ListItem>
                                                                    <asp:ListItem Value="ReqShipDate DESC">Ship Date Hi-Lo</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlSort3" runat="server" ForeColor="Black" Width="200px" BackColor="LemonChiffon" CssClass="form-control">
                                                                    <asp:ListItem>Sort Option 3</asp:ListItem>
                                                                    <asp:ListItem Value="AppointmentTime ASC">Appointment Date Lo-Hi</asp:ListItem>
                                                                    <asp:ListItem Value="AppointmentTime DESC">Appointment Date Hi-Lo</asp:ListItem>
                                                                    <asp:ListItem Value="Customer ASC">Customer A-Z</asp:ListItem>
                                                                    <asp:ListItem Value="Customer DESC">Customer Z-A</asp:ListItem>
                                                                    <asp:ListItem Value="CustomerID ASC">Customer # Lo-Hi</asp:ListItem>
                                                                    <asp:ListItem Value="CustomerID DESC">Customer # Hi-Lo</asp:ListItem>
                                                                    <asp:ListItem Value="OrderStatus ASC">Order Status A-Z</asp:ListItem>
                                                                    <asp:ListItem Value="OrderStatus DESC">Order Status Z-A</asp:ListItem>
                                                                    <asp:ListItem Value="ShipViaDescription ASC">Ship Via A-Z</asp:ListItem>
                                                                    <asp:ListItem Value="ShipViaDescription DESC">Ship Via Z-A</asp:ListItem>
                                                                    <asp:ListItem Value="ReqShipDate ASC">Ship Date Lo-Hi</asp:ListItem>
                                                                    <asp:ListItem Value="ReqShipDate DESC">Ship Date Hi-Lo</asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:LinkButton ID="lbnFilter" runat="server" CssClass="btn btn-success" Font-Bold="True" OnClick="lbnSearch_Click" ToolTip="Load Sales Order Tracker with chosen filters">Run Filters</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:LinkButton ID="btnUpdate" runat="server" BorderColor="Black" BorderStyle="Solid" BorderWidth="5px" CssClass="btn btn-primary" Font-Bold="True" Font-Size="14pt" OnClick="btnUpdate_Click" ToolTip="Click to update database. Don't forget to restart Timer!" Width="200px">SAVE</asp:LinkButton>
                                                    <ajaxToolkit:ConfirmButtonExtender ID="btnUpdate_ConfirmButtonExtender" runat="server" BehaviorID="btnUpdate_ConfirmButtonExtender" ConfirmOnFormSubmit="True" ConfirmText="Update Database now? Remember to Reset Timer!" TargetControlID="btnUpdate" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">

                                                    <table id="tblColorsLegend" width="1050px">
                                                        <tr>
                                                            <td style="background-color: lightgreen; text-align: center">
                                                                <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black" Text="Order is Ready" Width="125px"></asp:Label></td>
                                                            <td style="background-color: lemonchiffon; text-align: center">
                                                                <asp:Label ID="Label6" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black" Text="Production Dates Confirmed" Width="175px"></asp:Label></td>
                                                            <td style="background-color: pink; text-align: center">
                                                                <asp:Label ID="Label22" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black" Text="Short or Late Shipping" Width="175px"></asp:Label></td>
                                                            <td style="background-color: lightblue; text-align: center">
                                                                <asp:Label ID="Label21" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black" Text="Order Contains TBDs" Width="200px"></asp:Label></td>
                                                            <td style="background-color: red; text-align: center">
                                                                <asp:Label ID="Label34" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black" Text="Hold" Width="125px"></asp:Label></td>
                                                            <td style="background-color: orange; text-align: center">
                                                                <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black" Text="Suspended" Width="125px"></asp:Label></td>
                                                        </tr>
                                                    </table>

                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <table id="tblAdd" align="center" width="auto">
                                                        <tr class="rowColors">
                                                            <td align="left" valign="middle">

                                                                <!-- *** Begin Header Table *** -->
                                                                <div id="DivHeader" style="vertical-align: top; width: auto; text-align: left;">
                                                                    <asp:Table ID="HeaderTable" runat="server"
                                                                        CellPadding="2"
                                                                        CellSpacing="0"
                                                                        Font-Size="11pt"
                                                                        ForeColor="White"
                                                                        BackColor="#333333"
                                                                        Font-Bold="False">
                                                                    </asp:Table>
                                                                </div>
                                                                <!-- *** End Header Table *** -->
                                                                <asp:Panel ID="pnlGridView" runat="server" Height="500px" ScrollBars="Vertical" Style="overflow-y: scroll; overflow-x: hidden;">
                                                                    <div id="DivData" class="Container" style="vertical-align: top; height: 500px; width: 100%;">
                                                                        <asp:GridView ID="gvRecord" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="Black"
                                                                            BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" OnPageIndexChanging="gvRecord_PageIndexChanging"
                                                                            OnRowDataBound="gvRecord_RowDataBound" OnRowCommand="gvRecord_RowCommand"
                                                                            OnSorting="gvRecord_Sorting" AllowSorting="True" PageSize="50" ShowFooter="true"
                                                                            OnSelectedIndexChanged="gvRecord_OnSelectedIndexChanged" ShowHeader="false">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="#">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>' Font-Size="9pt" ForeColor="Black" Width="35px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="F/L<br>Oper." SortExpression="FirstOperator">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblFirstOperator" runat="server" Text='<%# Bind("FirstOperator") %>' Font-Bold="true" Font-Size="8pt" Width="70px" ForeColor="black"></asp:Label>
                                                                                        <asp:Label ID="lblLastOperator" runat="server" Text='<%# Bind("LastOperator") %>' Font-Bold="false" Font-Size="8pt" Width="70px" Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="CustID" SortExpression="CustomerID">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("CustomerID") %>' Font-Bold="false" Font-Size="9pt" Width="54px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Customer" SortExpression="Customer">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Font-Bold="false" Font-Size="9pt" Width="195px"></asp:Label>
                                                                                        <asp:Label ID="lblShortage" runat="server" Text='<%# Bind("Shortage") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="N<br/>O<br/>T<br>E">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgNotes" runat="server" ImageUrl="Images/information.png" />
                                                                                        <asp:Panel ID="pnlNotes" runat="server" Width="250px" Visible="true">
                                                                                            <div style="padding: 10px; margin: 10px; text-align: justify">
                                                                                                <asp:GridView ID="gvComments" runat="server" AutoGenerateColumns="False" Width="250px"
                                                                                                    Font-Size="9pt" BorderStyle="Solid" BorderWidth="1" BorderColor="Black" ShowHeader="false" ShowFooter="false" BackColor="White"
                                                                                                    CssClass="gridview">
                                                                                                    <EmptyDataTemplate>
                                                                                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="9pt" ForeColor="Red" Text="No Records Found"></asp:Label>
                                                                                                    </EmptyDataTemplate>
                                                                                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                                                                                    <Columns>
                                                                                                        <asp:TemplateField>
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblComments" runat="server" Text='<%# Bind("Comment") %>' Font-Size="9pt" ForeColor="Black"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle Wrap="true" />
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                    <HeaderStyle BackColor="#666666" Font-Names="Arial" ForeColor="White" Height="25px" HorizontalAlign="Center" BorderWidth="1" BorderStyle="Solid" VerticalAlign="Bottom" />
                                                                                                </asp:GridView>
                                                                                                <asp:Label ID="lblDept" runat="server" Text='<%# Bind("Dept") %>' ForeColor="Transparent" Width="25px"></asp:Label>
                                                                                            </div>
                                                                                            <br />
                                                                                        </asp:Panel>
                                                                                        <ajaxToolkit:HoverMenuExtender ID="hmeNotes" runat="server" PopupControlID="pnlNotes" PopupPosition="left" TargetControlID="imgNotes">
                                                                                        </ajaxToolkit:HoverMenuExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" Width="25px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="A<br>C<br>K">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgAck" runat="server" ImageUrl="~/Images/Ack.png" Style="cursor: pointer" />
                                                                                        <asp:Label ID="lblAck" runat="server" Text='<%# Bind("Ack") %>' Visible="false"></asp:Label>
                                                                                        <asp:Label ID="lblAckDateTime" runat="server" Text='<%# Bind("AckDateTime") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Order<br>Date" SortExpression="OrderDate">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOrderDate" runat="server" Text='<%# Bind("OrderDateTime") %>' Font-Bold="false" Font-Size="9pt" Width="78px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="SO#" SortExpression="SalesOrder">
                                                                                    <HeaderStyle CssClass="CenterAligner" Width="60px" />
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lbnSalesOrder" runat="server"
                                                                                            CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                                                            CausesValidation="False" ToolTip="Click to view Details."
                                                                                            CommandName="ViewDetails" ForeColor="blue" Font-Size="9pt" CssClass="NoUnderline"
                                                                                            Text='<%# Bind("SalesOrder") %>' Width="55px" Font-Bold="True"></asp:LinkButton>
                                                                                        <asp:Label ID="lblQueryStatus" runat="server"></asp:Label>
                                                                                        <asp:Label ID="lblSalesOrder" runat="server" Text='<%# Bind("SalesOrder") %>' Visible="false"></asp:Label>
                                                                                        <asp:Panel ID="pnlTotalBreakdown" runat="server" BackColor="GhostWhite" Visible="true">
                                                                                            <div style="position: relative">
                                                                                                <asp:GridView ID="gvTotalQtyBreakdown" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvTotalQtyBreakdown_RowDataBound" Width="350px"
                                                                                                    Font-Size="9pt" BorderStyle="Solid" BorderWidth="1" BorderColor="Black" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White" ShowFooter="True">
                                                                                                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                                                    <EmptyDataTemplate>
                                                                                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="9pt" ForeColor="Red" Text="No Records Found"></asp:Label>
                                                                                                    </EmptyDataTemplate>
                                                                                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                                                                                    <Columns>
                                                                                                        <asp:TemplateField HeaderText="StockCode">
                                                                                                            <HeaderStyle CssClass="CenterAligner" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' ForeColor="Black"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Description">
                                                                                                            <HeaderStyle CssClass="CenterAligner" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>' ForeColor="Black" Width="200px"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Qty">
                                                                                                            <HeaderStyle CssClass="CenterAligner" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblQty" runat="server" Text='<%# Bind("Qty", "{0:n2}") %>' ForeColor="Black" Width="75px"></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                            <ItemStyle HorizontalAlign="right" />
                                                                                                            <FooterTemplate>
                                                                                                                <asp:Label ID="lblQtyTotal" runat="server" ForeColor="white" Width="50px"></asp:Label>
                                                                                                            </FooterTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                    <HeaderStyle BackColor="#666666" Font-Names="Arial" ForeColor="White" Height="25px" HorizontalAlign="Center" BorderWidth="1" BorderStyle="Solid" VerticalAlign="Bottom" />
                                                                                                    <FooterStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Right" />
                                                                                                </asp:GridView>
                                                                                            </div>
                                                                                        </asp:Panel>
                                                                                        <ajaxToolkit:HoverMenuExtender ID="hmeTotalBreakdown" runat="server" PopupControlID="pnlTotalBreakdown" PopupPosition="left" TargetControlID="lbnSalesOrder">
                                                                                        </ajaxToolkit:HoverMenuExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Customer PO" SortExpression="CustomerPoNumber">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblPurchaseOrder" runat="server" Text='<%# Bind("CustomerPoNumber") %>' Font-Size="9pt" Width="155px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Total<br>Qty" SortExpression="TotalQuantity" Visible="false">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblTotalQuantity" runat="server" Text='<%# Bind("TotalQuantity") %>' Font-Bold="True" Font-Size="9pt" Width="50px" ForeColor="blue" Style="cursor: pointer"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Total<br>Value" SortExpression="TotalValue">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblTotalValue" runat="server" Text='<%# Bind("TotalValue") %>' Font-Bold="True" Font-Size="9pt" Width="105px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                                                                    <FooterTemplate>
                                                                                        <asp:Label ID="lblGrandTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                                                                    </FooterTemplate>
                                                                                    <FooterStyle HorizontalAlign="Right" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Order<br>Status" SortExpression="OrderStatus">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblOrderStatus" runat="server" Text='<%# Bind("OrderStatus") %>' Font-Bold="false" Font-Size="9pt" Width="115px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Assigned<br>Picker" SortExpression="Picker">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:DropDownList ID="ddlPicker" runat="server" Width="105px" ForeColor="Black" Font-Size="8pt" BackColor="LemonChiffon"></asp:DropDownList>
                                                                                        <asp:Label ID="lblPickerUserID" runat="server" Text='<%# Bind("PickerUserID") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Assigned<br>Pick<br>Date" SortExpression="PickDate">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtPickDate" runat="server" Text='<%# Bind("PickDate") %>' ForeColor="Black" BackColor="LemonChiffon" Font-Bold="false" Font-Size="8pt" Width="90px"></asp:TextBox>
                                                                                        <ajaxToolkit:CalendarExtender ID="txtPickDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgPickDate" TargetControlID="txtPickDate"></ajaxToolkit:CalendarExtender>
                                                                                        <ajaxToolkit:MaskedEditExtender ID="txtPickDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtPickDate"></ajaxToolkit:MaskedEditExtender>
                                                                                        <ajaxToolkit:MaskedEditValidator ID="txtPickDateMEV" runat="server" ControlExtender="txtPickDateMEE" ControlToValidate="txtPickDate" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtPickDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtPickDateMEV" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Start<br>Pick<br>Time" SortExpression="StartPickTime">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="ddlStartHours" runat="server" Width="45px" ForeColor="Black" BackColor="LemonChiffon" Font-Size="8pt" ToolTip="Only changing minutes triggers the update">
                                                                                                        <asp:ListItem Selected="True" Value="HH">HH</asp:ListItem>
                                                                                                        <asp:ListItem Value="00">00</asp:ListItem>
                                                                                                        <asp:ListItem Value="01">01</asp:ListItem>
                                                                                                        <asp:ListItem Value="02">02</asp:ListItem>
                                                                                                        <asp:ListItem Value="03">03</asp:ListItem>
                                                                                                        <asp:ListItem Value="04">04</asp:ListItem>
                                                                                                        <asp:ListItem Value="05">05</asp:ListItem>
                                                                                                        <asp:ListItem Value="06">06</asp:ListItem>
                                                                                                        <asp:ListItem Value="07">07</asp:ListItem>
                                                                                                        <asp:ListItem Value="08">08</asp:ListItem>
                                                                                                        <asp:ListItem Value="09">09</asp:ListItem>
                                                                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                                                                        <asp:ListItem Value="11">11</asp:ListItem>
                                                                                                        <asp:ListItem Value="12">12</asp:ListItem>
                                                                                                        <asp:ListItem Value="13">13</asp:ListItem>
                                                                                                        <asp:ListItem Value="14">14</asp:ListItem>
                                                                                                        <asp:ListItem Value="15">15</asp:ListItem>
                                                                                                        <asp:ListItem Value="16">16</asp:ListItem>
                                                                                                        <asp:ListItem Value="17">17</asp:ListItem>
                                                                                                        <asp:ListItem Value="19">19</asp:ListItem>
                                                                                                        <asp:ListItem Value="20">20</asp:ListItem>
                                                                                                        <asp:ListItem Value="21">21</asp:ListItem>
                                                                                                        <asp:ListItem Value="22">22</asp:ListItem>
                                                                                                        <asp:ListItem Value="23">23</asp:ListItem>
                                                                                                    </asp:DropDownList></td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="ddlStartMinutes" runat="server" Width="45px" ForeColor="Black" BackColor="LemonChiffon" Font-Size="8pt" ToolTip="Only changing minutes triggers the update">
                                                                                                        <asp:ListItem Selected="True">MM</asp:ListItem>
                                                                                                        <asp:ListItem Value="00">00</asp:ListItem>
                                                                                                        <asp:ListItem Value="01">01</asp:ListItem>
                                                                                                        <asp:ListItem Value="02">02</asp:ListItem>
                                                                                                        <asp:ListItem Value="03">03</asp:ListItem>
                                                                                                        <asp:ListItem Value="04">04</asp:ListItem>
                                                                                                        <asp:ListItem Value="05">05</asp:ListItem>
                                                                                                        <asp:ListItem Value="06">06</asp:ListItem>
                                                                                                        <asp:ListItem Value="07">07</asp:ListItem>
                                                                                                        <asp:ListItem Value="08">08</asp:ListItem>
                                                                                                        <asp:ListItem Value="09">09</asp:ListItem>
                                                                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                                                                        <asp:ListItem Value="11">11</asp:ListItem>
                                                                                                        <asp:ListItem Value="12">12</asp:ListItem>
                                                                                                        <asp:ListItem Value="13">13</asp:ListItem>
                                                                                                        <asp:ListItem Value="14">14</asp:ListItem>
                                                                                                        <asp:ListItem Value="15">15</asp:ListItem>
                                                                                                        <asp:ListItem Value="16">16</asp:ListItem>
                                                                                                        <asp:ListItem Value="17">17</asp:ListItem>
                                                                                                        <asp:ListItem Value="18">18</asp:ListItem>
                                                                                                        <asp:ListItem Value="19">19</asp:ListItem>
                                                                                                        <asp:ListItem Value="20">20</asp:ListItem>
                                                                                                        <asp:ListItem Value="21">21</asp:ListItem>
                                                                                                        <asp:ListItem Value="22">22</asp:ListItem>
                                                                                                        <asp:ListItem Value="23">23</asp:ListItem>
                                                                                                        <asp:ListItem Value="24">24</asp:ListItem>
                                                                                                        <asp:ListItem Value="25">25</asp:ListItem>
                                                                                                        <asp:ListItem Value="26">26</asp:ListItem>
                                                                                                        <asp:ListItem Value="27">27</asp:ListItem>
                                                                                                        <asp:ListItem Value="28">28</asp:ListItem>
                                                                                                        <asp:ListItem Value="29">29</asp:ListItem>
                                                                                                        <asp:ListItem Value="30">30</asp:ListItem>
                                                                                                        <asp:ListItem Value="31">31</asp:ListItem>
                                                                                                        <asp:ListItem Value="32">32</asp:ListItem>
                                                                                                        <asp:ListItem Value="33">33</asp:ListItem>
                                                                                                        <asp:ListItem Value="34">34</asp:ListItem>
                                                                                                        <asp:ListItem Value="35">35</asp:ListItem>
                                                                                                        <asp:ListItem Value="36">36</asp:ListItem>
                                                                                                        <asp:ListItem Value="37">37</asp:ListItem>
                                                                                                        <asp:ListItem Value="38">38</asp:ListItem>
                                                                                                        <asp:ListItem Value="39">39</asp:ListItem>
                                                                                                        <asp:ListItem Value="40">40</asp:ListItem>
                                                                                                        <asp:ListItem Value="41">41</asp:ListItem>
                                                                                                        <asp:ListItem Value="42">42</asp:ListItem>
                                                                                                        <asp:ListItem Value="43">43</asp:ListItem>
                                                                                                        <asp:ListItem Value="44">44</asp:ListItem>
                                                                                                        <asp:ListItem Value="45">45</asp:ListItem>
                                                                                                        <asp:ListItem Value="46">46</asp:ListItem>
                                                                                                        <asp:ListItem Value="47">47</asp:ListItem>
                                                                                                        <asp:ListItem Value="48">48</asp:ListItem>
                                                                                                        <asp:ListItem Value="49">49</asp:ListItem>
                                                                                                        <asp:ListItem Value="50">50</asp:ListItem>
                                                                                                        <asp:ListItem Value="51">51</asp:ListItem>
                                                                                                        <asp:ListItem Value="52">52</asp:ListItem>
                                                                                                        <asp:ListItem Value="53">53</asp:ListItem>
                                                                                                        <asp:ListItem Value="54">54</asp:ListItem>
                                                                                                        <asp:ListItem Value="55">55</asp:ListItem>
                                                                                                        <asp:ListItem Value="56">56</asp:ListItem>
                                                                                                        <asp:ListItem Value="57">57</asp:ListItem>
                                                                                                        <asp:ListItem Value="58">58</asp:ListItem>
                                                                                                        <asp:ListItem Value="59">59</asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <asp:Label ID="lblStartPickTime" runat="server" Text='<%# Bind("StartPickTime") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="End<br>Pick<br>Time" SortExpression="EndPickTime">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="ddlEndHours" runat="server" Width="45px" Font-Size="8pt" ForeColor="Black" BackColor="LemonChiffon" ToolTip="Only changing minutes triggers the update">
                                                                                                        <asp:ListItem Selected="True" Value="HH">HH</asp:ListItem>
                                                                                                        <asp:ListItem Value="00">00</asp:ListItem>
                                                                                                        <asp:ListItem Value="01">01</asp:ListItem>
                                                                                                        <asp:ListItem Value="02">02</asp:ListItem>
                                                                                                        <asp:ListItem Value="03">03</asp:ListItem>
                                                                                                        <asp:ListItem Value="04">04</asp:ListItem>
                                                                                                        <asp:ListItem Value="05">05</asp:ListItem>
                                                                                                        <asp:ListItem Value="06">06</asp:ListItem>
                                                                                                        <asp:ListItem Value="07">07</asp:ListItem>
                                                                                                        <asp:ListItem Value="08">08</asp:ListItem>
                                                                                                        <asp:ListItem Value="09">09</asp:ListItem>
                                                                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                                                                        <asp:ListItem Value="11">11</asp:ListItem>
                                                                                                        <asp:ListItem Value="12">12</asp:ListItem>
                                                                                                        <asp:ListItem Value="13">13</asp:ListItem>
                                                                                                        <asp:ListItem Value="14">14</asp:ListItem>
                                                                                                        <asp:ListItem Value="15">15</asp:ListItem>
                                                                                                        <asp:ListItem Value="16">16</asp:ListItem>
                                                                                                        <asp:ListItem Value="17">17</asp:ListItem>
                                                                                                        <asp:ListItem Value="18">18</asp:ListItem>
                                                                                                        <asp:ListItem Value="19">19</asp:ListItem>
                                                                                                        <asp:ListItem Value="20">20</asp:ListItem>
                                                                                                        <asp:ListItem Value="21">21</asp:ListItem>
                                                                                                        <asp:ListItem Value="22">22</asp:ListItem>
                                                                                                        <asp:ListItem Value="23">23</asp:ListItem>
                                                                                                    </asp:DropDownList></td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="ddlEndMinutes" runat="server" Width="45px" Font-Size="8pt" ForeColor="Black" BackColor="LemonChiffon" ToolTip="Only changing minutes triggers the update">
                                                                                                        <asp:ListItem Selected="True">MM</asp:ListItem>
                                                                                                        <asp:ListItem Value="00">00</asp:ListItem>
                                                                                                        <asp:ListItem Value="01">01</asp:ListItem>
                                                                                                        <asp:ListItem Value="02">02</asp:ListItem>
                                                                                                        <asp:ListItem Value="03">03</asp:ListItem>
                                                                                                        <asp:ListItem Value="04">04</asp:ListItem>
                                                                                                        <asp:ListItem Value="05">05</asp:ListItem>
                                                                                                        <asp:ListItem Value="06">06</asp:ListItem>
                                                                                                        <asp:ListItem Value="07">07</asp:ListItem>
                                                                                                        <asp:ListItem Value="08">08</asp:ListItem>
                                                                                                        <asp:ListItem Value="09">09</asp:ListItem>
                                                                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                                                                        <asp:ListItem Value="11">11</asp:ListItem>
                                                                                                        <asp:ListItem Value="12">12</asp:ListItem>
                                                                                                        <asp:ListItem Value="13">13</asp:ListItem>
                                                                                                        <asp:ListItem Value="14">14</asp:ListItem>
                                                                                                        <asp:ListItem Value="15">15</asp:ListItem>
                                                                                                        <asp:ListItem Value="16">16</asp:ListItem>
                                                                                                        <asp:ListItem Value="17">17</asp:ListItem>
                                                                                                        <asp:ListItem Value="18">18</asp:ListItem>
                                                                                                        <asp:ListItem Value="19">19</asp:ListItem>
                                                                                                        <asp:ListItem Value="20">20</asp:ListItem>
                                                                                                        <asp:ListItem Value="21">21</asp:ListItem>
                                                                                                        <asp:ListItem Value="22">22</asp:ListItem>
                                                                                                        <asp:ListItem Value="23">23</asp:ListItem>
                                                                                                        <asp:ListItem Value="24">24</asp:ListItem>
                                                                                                        <asp:ListItem Value="25">25</asp:ListItem>
                                                                                                        <asp:ListItem Value="26">26</asp:ListItem>
                                                                                                        <asp:ListItem Value="27">27</asp:ListItem>
                                                                                                        <asp:ListItem Value="28">28</asp:ListItem>
                                                                                                        <asp:ListItem Value="29">29</asp:ListItem>
                                                                                                        <asp:ListItem Value="30">30</asp:ListItem>
                                                                                                        <asp:ListItem Value="31">31</asp:ListItem>
                                                                                                        <asp:ListItem Value="32">32</asp:ListItem>
                                                                                                        <asp:ListItem Value="33">33</asp:ListItem>
                                                                                                        <asp:ListItem Value="34">34</asp:ListItem>
                                                                                                        <asp:ListItem Value="35">35</asp:ListItem>
                                                                                                        <asp:ListItem Value="36">36</asp:ListItem>
                                                                                                        <asp:ListItem Value="37">37</asp:ListItem>
                                                                                                        <asp:ListItem Value="38">38</asp:ListItem>
                                                                                                        <asp:ListItem Value="39">39</asp:ListItem>
                                                                                                        <asp:ListItem Value="40">40</asp:ListItem>
                                                                                                        <asp:ListItem Value="41">41</asp:ListItem>
                                                                                                        <asp:ListItem Value="42">42</asp:ListItem>
                                                                                                        <asp:ListItem Value="43">43</asp:ListItem>
                                                                                                        <asp:ListItem Value="44">44</asp:ListItem>
                                                                                                        <asp:ListItem Value="45">45</asp:ListItem>
                                                                                                        <asp:ListItem Value="46">46</asp:ListItem>
                                                                                                        <asp:ListItem Value="47">47</asp:ListItem>
                                                                                                        <asp:ListItem Value="48">48</asp:ListItem>
                                                                                                        <asp:ListItem Value="49">49</asp:ListItem>
                                                                                                        <asp:ListItem Value="50">50</asp:ListItem>
                                                                                                        <asp:ListItem Value="51">51</asp:ListItem>
                                                                                                        <asp:ListItem Value="52">52</asp:ListItem>
                                                                                                        <asp:ListItem Value="53">53</asp:ListItem>
                                                                                                        <asp:ListItem Value="54">54</asp:ListItem>
                                                                                                        <asp:ListItem Value="55">55</asp:ListItem>
                                                                                                        <asp:ListItem Value="56">56</asp:ListItem>
                                                                                                        <asp:ListItem Value="57">57</asp:ListItem>
                                                                                                        <asp:ListItem Value="58">58</asp:ListItem>
                                                                                                        <asp:ListItem Value="59">59</asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <asp:Label ID="lblEndPickTime" runat="server" Text='<%# Bind("EndPickTime") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Standard<br>Pick<br>Time<br>Minutes" SortExpression="StdPickingTimeInMinutes">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblStdPickingTimeInMinutes" runat="server" Text='<%# Bind("StdPickingTimeInMinutes") %>' ForeColor="Black" Font-Bold="false" Font-Size="8pt" Width="73px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Standard<br>Cases<br>Per<br>Minute" SortExpression="StdCasesPerMinute">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblStdCasesPerMinute" runat="server" Text='<%# Bind("StdCasesPerMinute") %>' ForeColor="Black" Font-Bold="false" Font-Size="8pt" Width="73px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Actual<br>Picking<br>Time<br>Minutes" SortExpression="ActualPickingTimeInMinutes">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblActualPickingTimeInMinutes" runat="server" Text='<%# Bind("ActualPickingTimeInMinutes") %>' Font-Bold="false" Font-Size="9pt" Width="53px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Actual<br>Cases<br>Per<br>Minute" SortExpression="ActualCasesPerMinutes">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblActualCasesPerMinutes" runat="server" Text='<%# Bind("ActualCasesPerMinutes") %>' Font-Bold="false" Font-Size="9pt" Width="53px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Cases<br>Picked" SortExpression="CasesPicked">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtCasesPicked" runat="server" Text='<%# Bind("CasesPicked") %>' ForeColor="Black" BackColor="LemonChiffon" Font-Bold="false" Font-Size="8pt" Width="50px" OnSelectedIndexChanged="txtCasesPicked_TextChanged"></asp:TextBox>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Projected<br>Finish<br>Time" SortExpression="ProjectedFinishTime">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblProjectedFinishTime" runat="server" Text='<%# Bind("ProjectedFinishTime") %>' ForeColor="Black" Font-Bold="false" Font-Size="8pt" Width="73px" AutoPostBack="true" OnSelectedIndexChanged="txtCasesPicked_TextChanged"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Felbro<br>Ship<br>Date" SortExpression="reqShipDate">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblShipDate" runat="server" Text='<%# Bind("reqShipDate") %>' Font-Bold="false" Font-Size="9pt" Width="90px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Default<br>Ship<br>Via" SortExpression="ShipViaDescription">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDefaultShipVia" runat="server" Text='<%# Bind("ShipViaDescription") %>' Font-Bold="false" Font-Size="7pt" Width="95px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="SP" SortExpression="SalesPerson" Visible="false">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgSalesPerson" runat="server" ImageUrl="~/Images/People.png" ToolTip='<%# Bind("SalesPerson") %>' Style="cursor: pointer" Width="20px"></asp:Image>
                                                                                        <asp:Label ID="lblSalesPerson" runat="server" Text='<%# Bind("SalesPerson") %>' Visible="false"></asp:Label>
                                                                                        <asp:Label ID="lblSalesPersonID" runat="server" Text='<%# Bind("SalesPersonID") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Appoint<br> Date" SortExpression="AppointmentDateTime">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblAppointmentDateTime" runat="server" Text='<%# Bind("AppointmentTime","{0:d}") %>' ToolTip='<%# Bind("AppointmentTime","{0:t}") %>' Style="cursor:pointer" Font-Bold="false" Font-Size="9pt" Width="95px"></asp:Label>
                                                                                   
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="S<br>T<br>A<br>G<br>E<br>D" SortExpression="Staged">
                                                                                    <HeaderStyle CssClass="CenterAligner" Font-Size="7pt" />
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtStaged" runat="server" Text='<%# Bind("Staged") %>' BackColor="LemonChiffon"  style="text-transform: uppercase;"
                                                                                            ToolTip="P for Partial, F for Full" Width="20px" MaxLength="1" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="P<br>A<br>L<br>L<br>E<br>T<br>S" SortExpression="PalletsRequired">
                                                                                    <HeaderStyle CssClass="CenterAligner" Font-Size="7pt" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblPalletsRequired" runat="server" Text='<%# Bind("PalletsRequired") %>' ToolTip="Pallets Needed" Font-Bold="true" ForeColor="Black" Width="35px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="S<br>C<br>A<br>N" SortExpression="ReadyForPickup">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkScanned" runat="server" ToolTip="Scanned if Checked" Width="35px" />
                                                                                        <asp:Label ID="lblScanned" runat="server" Text='<%# Bind("ReadyForPickup") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="H<br>O<br>L<br>D" SortExpression="CreditHold">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:CheckBox ID="chkCreditHold" runat="server" AutoPostBack="True" OnCheckedChanged="chkCreditHold_CheckedChanged" ToolTip="Credit Hold if Checked" Width="35px" />
                                                                                        <asp:Label ID="lblCreditHold" runat="server" Text='<%# Bind("CreditHold") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Requested<br>Delivery<br>Date" SortExpression="CustomerDeliveryDate">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCustomerDeliveryDate" runat="server" Text='<%# Bind("CustomerDeliveryDate") %>' Font-Bold="false" Font-Size="9pt" Width="70px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Actual<br>Ship<br>Date" SortExpression="ActualDeliveryDate">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtActualDeliveryDate" runat="server" Text='<%# Bind("ActualDeliveryDate") %>' Font-Bold="false" Font-Size="8pt" Width="70px" ForeColor="Black" BackColor="LemonChiffon"></asp:TextBox>
                                                                                        <ajaxToolkit:CalendarExtender ID="txtActualDeliveryDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgActualDeliveryDate" TargetControlID="txtActualDeliveryDate"></ajaxToolkit:CalendarExtender>
                                                                                        <ajaxToolkit:MaskedEditExtender ID="txtActualDeliveryDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtActualDeliveryDate"></ajaxToolkit:MaskedEditExtender>
                                                                                        <ajaxToolkit:MaskedEditValidator ID="txtActualDeliveryDateMEV" runat="server" ControlExtender="txtActualDeliveryDateMEE" ControlToValidate="txtActualDeliveryDate" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtActualDeliveryDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtActualDeliveryDateMEV" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField>
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <HeaderTemplate>
                                                                                        <asp:Image ID="imgLeadTimes" runat="server" ImageUrl="Images/Appointment24.png" />
                                                                                    </HeaderTemplate>
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgLeadTimes" runat="server" ImageUrl="Images/Appointment24.png" />
                                                                                        <asp:Panel ID="pnlLeadTimes" runat="server" BackColor="GhostWhite" BorderColor="Black" BorderStyle="Solid" BorderWidth="1" Width="500px" Visible="true">
                                                                                            <div style="padding: 10px; margin: 10px; text-align: justify">
                                                                                                <asp:Label ID="lblContactType" runat="server" Text='<%# Bind("ContactType") %>' Visible="false"></asp:Label>
                                                                                                <asp:Label ID="lblLeadTimeValue" runat="server" Text='<%# Bind("LeadTimeValue") %>' Font-Size="9pt" ForeColor="Black"></asp:Label>
                                                                                                <asp:Label ID="lblLeadTimeValueType" runat="server" Text='<%# Bind("LeadTimeValueType") %>' Font-Size="9pt" ForeColor="Black"></asp:Label>
                                                                                                <asp:HyperLink ID="hlAppointmentInfo" runat="server" ForeColor="purple" Width="300px" Font-Bold="True"></asp:HyperLink>
                                                                                                <asp:Label ID="lblLeadTimeSource" runat="server" Text='<%# Bind("LeadTimeSource") %>' Visible="false"></asp:Label>
                                                                                                <asp:Label ID="lblAddressCode" runat="server" Text='<%# Bind("AddrCode") %>' Visible="false"></asp:Label>
                                                                                            </div>
                                                                                            <br />
                                                                                        </asp:Panel>
                                                                                        <ajaxToolkit:HoverMenuExtender ID="hmeLeadTimes" runat="server" PopupControlID="pnlLeadTimes" PopupPosition="left" TargetControlID="imgLeadTimes">
                                                                                        </ajaxToolkit:HoverMenuExtender>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" Width="25px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Appoint<br>Date" SortExpression="AppointmentTime">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:TextBox ID="txtAppointmentDateTime" runat="server" Text='<%# Bind("AppointmentTime","{0:d}") %>' Font-Bold="false" Font-Size="8pt" Width="70px" ForeColor="Black" BackColor="LemonChiffon"></asp:TextBox>
                                                                                        <ajaxToolkit:CalendarExtender ID="txtAppointmentDateTimeCalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="txtAppointmentDateTime" TargetControlID="txtAppointmentDateTime"></ajaxToolkit:CalendarExtender>
                                                                                        <ajaxToolkit:MaskedEditExtender ID="txtAppointmentDateTimeMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtAppointmentDateTime"></ajaxToolkit:MaskedEditExtender>
                                                                                        <ajaxToolkit:MaskedEditValidator ID="txtAppointmentDateTimeMEV" runat="server" ControlExtender="txtAppointmentDateTimeMEE" ControlToValidate="txtAppointmentDateTime" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                                                        <ajaxToolkit:ValidatorCalloutExtender ID="txtAppointmentDateTimeVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtAppointmentDateTimeMEV" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Appoint<br>Time" SortExpression="AppointmentTime">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <table>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="ddlAppointmentTimeHours" runat="server" Width="45px" Font-Size="8pt" ForeColor="Black" BackColor="LemonChiffon" ToolTip="Only changing minutes triggers the update">
                                                                                                        <asp:ListItem Selected="True" Value="HH">HH</asp:ListItem>
                                                                                                        <asp:ListItem Value="00">00</asp:ListItem>
                                                                                                        <asp:ListItem Value="01">01</asp:ListItem>
                                                                                                        <asp:ListItem Value="02">02</asp:ListItem>
                                                                                                        <asp:ListItem Value="03">03</asp:ListItem>
                                                                                                        <asp:ListItem Value="04">04</asp:ListItem>
                                                                                                        <asp:ListItem Value="05">05</asp:ListItem>
                                                                                                        <asp:ListItem Value="06">06</asp:ListItem>
                                                                                                        <asp:ListItem Value="07">07</asp:ListItem>
                                                                                                        <asp:ListItem Value="08">08</asp:ListItem>
                                                                                                        <asp:ListItem Value="09">09</asp:ListItem>
                                                                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                                                                        <asp:ListItem Value="11">11</asp:ListItem>
                                                                                                        <asp:ListItem Value="12">12</asp:ListItem>
                                                                                                        <asp:ListItem Value="13">13</asp:ListItem>
                                                                                                        <asp:ListItem Value="14">14</asp:ListItem>
                                                                                                        <asp:ListItem Value="15">15</asp:ListItem>
                                                                                                        <asp:ListItem Value="16">16</asp:ListItem>
                                                                                                        <asp:ListItem Value="17">17</asp:ListItem>
                                                                                                        <asp:ListItem Value="18">18</asp:ListItem>
                                                                                                        <asp:ListItem Value="19">19</asp:ListItem>
                                                                                                        <asp:ListItem Value="20">20</asp:ListItem>
                                                                                                        <asp:ListItem Value="21">21</asp:ListItem>
                                                                                                        <asp:ListItem Value="22">22</asp:ListItem>
                                                                                                        <asp:ListItem Value="23">23</asp:ListItem>
                                                                                                    </asp:DropDownList></td>
                                                                                                <td>
                                                                                                    <asp:DropDownList ID="ddlAppointmentTimeMinutes" runat="server" Width="45px" Font-Size="8pt" ForeColor="Black" BackColor="LemonChiffon" ToolTip="Only changing minutes triggers the update">
                                                                                                        <asp:ListItem Selected="True">MM</asp:ListItem>
                                                                                                        <asp:ListItem Value="00">00</asp:ListItem>
                                                                                                        <asp:ListItem Value="01">01</asp:ListItem>
                                                                                                        <asp:ListItem Value="02">02</asp:ListItem>
                                                                                                        <asp:ListItem Value="03">03</asp:ListItem>
                                                                                                        <asp:ListItem Value="04">04</asp:ListItem>
                                                                                                        <asp:ListItem Value="05">05</asp:ListItem>
                                                                                                        <asp:ListItem Value="06">06</asp:ListItem>
                                                                                                        <asp:ListItem Value="07">07</asp:ListItem>
                                                                                                        <asp:ListItem Value="08">08</asp:ListItem>
                                                                                                        <asp:ListItem Value="09">09</asp:ListItem>
                                                                                                        <asp:ListItem Value="10">10</asp:ListItem>
                                                                                                        <asp:ListItem Value="11">11</asp:ListItem>
                                                                                                        <asp:ListItem Value="12">12</asp:ListItem>
                                                                                                        <asp:ListItem Value="13">13</asp:ListItem>
                                                                                                        <asp:ListItem Value="14">14</asp:ListItem>
                                                                                                        <asp:ListItem Value="15">15</asp:ListItem>
                                                                                                        <asp:ListItem Value="16">16</asp:ListItem>
                                                                                                        <asp:ListItem Value="17">17</asp:ListItem>
                                                                                                        <asp:ListItem Value="18">18</asp:ListItem>
                                                                                                        <asp:ListItem Value="19">19</asp:ListItem>
                                                                                                        <asp:ListItem Value="20">20</asp:ListItem>
                                                                                                        <asp:ListItem Value="21">21</asp:ListItem>
                                                                                                        <asp:ListItem Value="22">22</asp:ListItem>
                                                                                                        <asp:ListItem Value="23">23</asp:ListItem>
                                                                                                        <asp:ListItem Value="24">24</asp:ListItem>
                                                                                                        <asp:ListItem Value="25">25</asp:ListItem>
                                                                                                        <asp:ListItem Value="26">26</asp:ListItem>
                                                                                                        <asp:ListItem Value="27">27</asp:ListItem>
                                                                                                        <asp:ListItem Value="28">28</asp:ListItem>
                                                                                                        <asp:ListItem Value="29">29</asp:ListItem>
                                                                                                        <asp:ListItem Value="30">30</asp:ListItem>
                                                                                                        <asp:ListItem Value="31">31</asp:ListItem>
                                                                                                        <asp:ListItem Value="32">32</asp:ListItem>
                                                                                                        <asp:ListItem Value="33">33</asp:ListItem>
                                                                                                        <asp:ListItem Value="34">34</asp:ListItem>
                                                                                                        <asp:ListItem Value="35">35</asp:ListItem>
                                                                                                        <asp:ListItem Value="36">36</asp:ListItem>
                                                                                                        <asp:ListItem Value="37">37</asp:ListItem>
                                                                                                        <asp:ListItem Value="38">38</asp:ListItem>
                                                                                                        <asp:ListItem Value="39">39</asp:ListItem>
                                                                                                        <asp:ListItem Value="40">40</asp:ListItem>
                                                                                                        <asp:ListItem Value="41">41</asp:ListItem>
                                                                                                        <asp:ListItem Value="42">42</asp:ListItem>
                                                                                                        <asp:ListItem Value="43">43</asp:ListItem>
                                                                                                        <asp:ListItem Value="44">44</asp:ListItem>
                                                                                                        <asp:ListItem Value="45">45</asp:ListItem>
                                                                                                        <asp:ListItem Value="46">46</asp:ListItem>
                                                                                                        <asp:ListItem Value="47">47</asp:ListItem>
                                                                                                        <asp:ListItem Value="48">48</asp:ListItem>
                                                                                                        <asp:ListItem Value="49">49</asp:ListItem>
                                                                                                        <asp:ListItem Value="50">50</asp:ListItem>
                                                                                                        <asp:ListItem Value="51">51</asp:ListItem>
                                                                                                        <asp:ListItem Value="52">52</asp:ListItem>
                                                                                                        <asp:ListItem Value="53">53</asp:ListItem>
                                                                                                        <asp:ListItem Value="54">54</asp:ListItem>
                                                                                                        <asp:ListItem Value="55">55</asp:ListItem>
                                                                                                        <asp:ListItem Value="56">56</asp:ListItem>
                                                                                                        <asp:ListItem Value="57">57</asp:ListItem>
                                                                                                        <asp:ListItem Value="58">58</asp:ListItem>
                                                                                                        <asp:ListItem Value="59">59</asp:ListItem>
                                                                                                    </asp:DropDownList>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <asp:Label ID="lblAppointmentTime" runat="server" Text='<%# Bind("AppointmentTime") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Actual<br>Ship<br>Via" SortExpression="ActualShipViaID">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:DropDownList ID="ddlShipVia" runat="server" Width="85px" Font-Size="8pt" ForeColor="Black" BackColor="LemonChiffon"></asp:DropDownList>
                                                                                        <asp:Label ID="lblActualShipViaID" runat="server" Text='<%# Bind("ActualShipViaID") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="D<br/>A<br/>Y<br>S" Visible="false">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgDeliveryInfoUnScheduled" runat="server" ImageUrl="~/Images/DelTruckUnScheduled.png" ToolTip='<%# Bind("CustomerDeliverySchedule") %>' Style="cursor: pointer" Width="20px"></asp:Image>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="35px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="S<br/>H<br/>I<br/>P" Visible="false">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgDeliveryInfoScheduled" runat="server" ImageUrl="~/Images/DelTruckScheduled.png" Style="cursor: pointer" Width="20px"></asp:Image>
                                                                                        <asp:Label ID="lblDeliveryStatus" runat="server" Text='<%# Bind("DeliveryStatus") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" Width="35px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<span style='color:GoldenRod' class='fa fa-sticky-note'  title='Special Instructions'></span>" Visible="false">
                                                                                    <ItemTemplate>
                                                                                        <asp:HyperLink ID="imgViewSpecialInstrs" runat="server" ForeColor="GoldenRod" Style="position: relative"><span class='fa fa-sticky-note'  title=''></span></asp:HyperLink>
                                                                                        <asp:Panel ID="pnlSpecialInstrs" runat="server">
                                                                                            <div style="position: relative; right: 0px">
                                                                                                <asp:Label ID="lblSpecialInstrs" runat="server" Text='<%# Bind("SpecialInstrs") %>' BackColor="GhostWhite" BorderColor="Black" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                                                                                                <ajaxToolkit:HoverMenuExtender ID="lblSpecialInstrs_HoverMenuExtender" runat="server" TargetControlID="imgViewSpecialInstrs" PopupControlID="pnlSpecialInstrs" PopupPosition="Left">
                                                                                                </ajaxToolkit:HoverMenuExtender>
                                                                                            </div>
                                                                                        </asp:Panel>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="20px" />
                                                                                    <FooterTemplate></FooterTemplate>
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                            <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" />
                                                                            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
                                                                            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                            <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                            <SortedDescendingHeaderStyle BackColor="#242121" />
                                                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                                                        </asp:GridView>
                                                                    </div>
                                                                </asp:Panel>
                                                            </td>
                                                            <td align="center" valign="middle">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">&nbsp; 
                                                                <asp:Label ID="lblPageNo" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy" Visible="False"></asp:Label>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table id="Table1" align="center">

                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:LinkButton ID="btnExportExcel" runat="server" OnClick="btnExportExcel_Click" CssClass="btn btn-success" ForeColor="White"><i class="glyphicon glyphicon-export"></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>

                                                                        </td>
                                                                </table>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Timer ID="Timer1" runat="server" Interval="50000" OnTick="Timer1_Tick" />
                                                            </td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="16pt"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div class="clearfix">
                <asp:Button ID="btnShowDetails" runat="server" Style="display: none" Text="Button" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderPopUp" runat="server" BackgroundCssClass="popup_background" CancelControlID="btnClose"
                    DynamicServicePath="" Enabled="True" PopupControlID="pnlPopUpDetails" TargetControlID="btnShowDetails">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlPopUpDetails" runat="server" CssClass="modalPopup2" Visible="true" Style="display: none; padding: 10px" ScrollBars="Vertical" Height="500px" Width="1485px">
                    <asp:UpdatePanel ID="UpdatePanelDetails" runat="server">
                        <ContentTemplate>
                            <div style="position: fixed; right: 50px; height: 40px; width: 150px; z-index: 10000;">
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="UpdatePanelDetails">
                                    <ProgressTemplate>
                                        <table style="border: medium solid #000080; width: 100%; background-color: white;">
                                            <tr>
                                                <td align="right" style="width: 12px;">
                                                    <img src="Images/Loader.gif" alt="" />
                                                </td>
                                                <td><span style="color: #ffffff"><span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing  <span class="">....</span> </strong></span></span></td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>

                            <table id="tblPopUp" style="border: 1px solid black; font-size: 7pt; background-color: #FFFFFF;" width="1450px" align="center">
                                <tr>
                                    <td style="float: left">
                                        <asp:LinkButton ID="btnClose1" runat="server" CssClass="btn" Font-Bold="True" ForeColor="Red" Font-Size="16pt"><span class="close" style="color:red !important">&times</span></asp:LinkButton>
                                    </td>
                                    <td style="float: right">
                                        <asp:LinkButton ID="btnClose" runat="server" CssClass="btn" Font-Bold="True" ForeColor="Red" Font-Size="16pt"><span class="close" style="color:red !important">&times</span></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label30" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="Navy">Sales Order Details for </asp:Label>
                                        <asp:Label ID="lblSaleOrderDetails" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="Navy"></asp:Label>
                                        <asp:Label ID="lblSaleOrderDetailsHidden" runat="server" Visible="false"></asp:Label>
                                        &nbsp;&nbsp;<asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="black">Ship Date:</asp:Label>
                                        &nbsp;&nbsp;<asp:Label ID="lblFelbroShipDateDetails" runat="server" Font-Bold="True" Font-Size="14pt" ForeColor="Green"></asp:Label>
                                        <asp:Label ID="lblSalespersonUserID" runat="server" Visible="false"></asp:Label>&nbsp;&nbsp;
                                        <asp:Label ID="lblCustomerName" runat="server" ForeColor="Navy" Font-Bold="True" Font-Size="14pt"></asp:Label>&nbsp;&nbsp;
                                     <span style="color: navy; font-weight: bold; font-size: 14pt;">(</span>
                                        <asp:Label ID="lblCustomerNumber" runat="server" ForeColor="Navy" Font-Bold="True" Font-Size="14pt"></asp:Label><span style="color: navy; font-weight: bold; font-size: 14pt;">)</span>&nbsp;&nbsp;
                                        <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="black">PO#:</asp:Label>
                                        <asp:Label ID="lblPO" runat="server" ForeColor="Navy" Font-Bold="True" Font-Size="14pt"></asp:Label>
                                    </td>
                                </tr>
                                <%--  <tr>
                                    <td align="center">
                                        <asp:Button ID="btnClose0" runat="server" CssClass="btn btn-danger" Text="Close" OnClick="btnClose_Click" />
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td align="right">
                                        <asp:CheckBox ID="chkLoadSchedules" runat="server" Text="&nbsp;Load Schedules" AutoPostBack="true" OnCheckedChanged="chkLoadSchedules_CheckedChanged" ForeColor="Blue" Font-Bold="True" />
                                        <asp:CheckBox ID="chkLoadMatrixes" runat="server" Text="&nbsp;Load Matrixes" AutoPostBack="true" OnCheckedChanged="chkLoadMatrixes_CheckedChanged" ForeColor="Blue" Font-Bold="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:GridView ID="gvDetails" runat="server" CellPadding="4" ForeColor="#333333"
                                            GridLines="None"
                                            HorizontalAlign="Center"
                                            Font-Size="9pt"
                                            PageSize="15" AllowSorting="True"
                                            OnSorting="gvDetails_Sorting"
                                            Width="1450px"
                                            AutoGenerateColumns="False"
                                            ShowFooter="True" OnRowDataBound="gvDetails_RowDataBound">
                                            <FooterStyle BackColor="Silver" />
                                            <HeaderStyle Font-Size="8pt" VerticalAlign="Top" BackColor="#CCCCCC" />
                                            <AlternatingRowStyle BackColor="LemonChiffon" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="#" SortExpression="ID" Visible="false">
                                                    <HeaderStyle CssClass="CenterAligner" Width="35px" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRowID" runat="server" Style="cursor: pointer" Text='<%# Bind("RowID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sales Order #" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSalesOrder" runat="server" Text='<%# Bind("SalesOrder") %>' ForeColor="#00569D" CssClass="NoUnderline"></asp:Label>
                                                        <asp:Label ID="lblSalesOrderHidden" runat="server" Text='<%# Bind("SalesOrder") %>' Visible="false"></asp:Label>
                                                        <asp:Panel ID="pnlNotes0" runat="server" BackColor="white" Visible="true" Width="250px" BorderStyle="Solid" BorderWidth="1" BorderColor="Black">
                                                            <div style="position: relative">
                                                                <asp:Label ID="lblSalesOrderNotes" runat="server" Text='<%# Bind("SalesOrderNotes") %>' ForeColor="Black" Font-Bold="True" Width="200px"></asp:Label>
                                                            </div>
                                                        </asp:Panel>
                                                        <ajaxToolkit:HoverMenuExtender ID="hmeNotes0" runat="server" PopupControlID="pnlNotes0" PopupPosition="right" TargetControlID="lblSalesOrder">
                                                        </ajaxToolkit:HoverMenuExtender>
                                                        <asp:Label ID="lblShortage" runat="server" Text='<%# Bind("Shortage") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Order Date" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrderDate" runat="server" Text='<%# Bind("OrderDate") %>' Font-Bold="True" Font-Size="9pt"></asp:Label>
                                                        <asp:Label ID="lblOrderStatus" runat="server" Text='<%# Bind("OrderStatus") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Stock Code" SortExpression="StockCode">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Font-Bold="True" Font-Size="9pt"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" SortExpression="JobDescription">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblJobDescription" runat="server" Text='<%# Bind("JobDescription") %>' Font-Bold="True" Font-Size="8"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Long<br/>Desc" SortExpression="JobDescriptionLong" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblJobDescriptionLong" runat="server" Text='<%# Bind("JobDescriptionLong") %>' Font-Bold="True" Font-Size="8"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="B.O. Qty" SortExpression="BackOrderQty">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblBackOrderQty" runat="server" Text='<%# Bind("BackOrderQty") %>' Font-Bold="True" Font-Size="9pt"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ship Qty" SortExpression="ShipQty">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblShipQty" runat="server" Text='<%# Bind("ShipQty") %>' Font-Bold="True" Font-Size="9pt"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Price" SortExpression="Price">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPrice" runat="server" Text='<%# Bind("Price") %>' Font-Bold="True" Font-Size="9pt"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" BackColor="GhostWhite" />
                                                    <FooterTemplate>
                                                        Order Total: 
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Ext. Price" SortExpression="ExtendedPrice">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblExtendedPrice" runat="server" Text='<%# Bind("ExtendedPrice") %>' Font-Bold="True" Font-Size="9pt"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" BackColor="GhostWhite" />
                                                    <FooterTemplate>
                                                        <asp:Label ID="lblPriceTotal" runat="server" Font-Bold="True" Font-Size="12"></asp:Label>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        &nbsp;&nbsp;&nbsp;
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <%-- <asp:TemplateField HeaderText="Sales Person" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSalesPerson" runat="server" Text='<%# Bind("SalesPerson") %>' Font-Size="8" ForeColor="Navy"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Customer" SortExpression="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Font-Bold="True" Font-Size="9pt"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CID" SortExpression="CustomerID" Visible="true">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("CustomerID") %>' Font-Bold="True" Font-Size="9pt" Width="50px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField HeaderText="Ready Date" SortExpression="ReadyDate" Visible="true">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLineItemOrderStatus" runat="server" Text='<%# Bind("LineItemOrderStatus") %>' Font-Bold="True" Font-Size="9pt" Width="70px"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<span style='color:Navy' class='fa fa-clock-o'  title='Production Schedule'></span>">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lbnNotes" runat="server" Style="position: relative" ForeColor="Navy"><span class='fa fa-clock-o'  title='Production Schedule'></span></asp:HyperLink>
                                                        <asp:Panel ID="pnlNotes" runat="server" BackColor="GhostWhite" Visible="true">
                                                            <div style="position: relative">
                                                                <asp:GridView ID="gvProductionSchedule" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvProductionSchedule_RowDataBound" Width="350px"
                                                                    Font-Size="12" BorderStyle="Solid" BorderWidth="1" BorderColor="Black" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White">
                                                                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                    <EmptyDataTemplate>
                                                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Red" Text="No Records Found"></asp:Label>
                                                                    </EmptyDataTemplate>
                                                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Scheduled Date">
                                                                            <HeaderStyle CssClass="CenterAligner" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblScheduledDate" runat="server" Text='<%# Bind("ScheduledDate") %>' Font-Bold="True" ForeColor="Black"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Scheduled Qty">
                                                                            <HeaderStyle CssClass="CenterAligner" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblScheduledQty" runat="server" Text='<%# Bind("ScheduledQty") %>' ForeColor="Black" Font-Bold="True" Width="50px"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle BackColor="#666666" Font-Names="Arial" ForeColor="White" Height="25px" HorizontalAlign="Center" BorderWidth="1" BorderStyle="Solid" VerticalAlign="Bottom" />
                                                                    <FooterStyle BackColor="#666666" ForeColor="White" />
                                                                </asp:GridView>
                                                            </div>
                                                        </asp:Panel>
                                                        <ajaxToolkit:HoverMenuExtender ID="hmeNotes" runat="server" PopupControlID="pnlNotes" PopupPosition="left" TargetControlID="lbnNotes">
                                                        </ajaxToolkit:HoverMenuExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<span style='color:Blue' class='fa fa-calendar'  title='Production Matrix'></span>">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="lbnMatrix" runat="server" Style="position: relative" ForeColor="Blue"><span class='fa fa-calendar'  title='Production Matrix'></span></asp:HyperLink>
                                                        <asp:Panel ID="pnlMatrix" runat="server" BackColor="GhostWhite" Visible="true">
                                                            <div style="position: relative">
                                                                <asp:Panel ID="ProMatrixScroll" runat="server" ScrollBars="Vertical" Height="200px" Width="1025px">
                                                                    <asp:GridView ID="gvProductionMatrix" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvProductionMatrix_RowDataBound" Width="1000px"
                                                                        Font-Size="9pt" BorderStyle="Solid" BorderWidth="1" BorderColor="Black" HeaderStyle-BackColor="#666666" HeaderStyle-ForeColor="White">
                                                                        <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                        <EmptyDataTemplate>
                                                                            <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Red" Text="No Records Found"></asp:Label>
                                                                        </EmptyDataTemplate>
                                                                        <EmptyDataRowStyle HorizontalAlign="Center" />
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="#">
                                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblID" runat="server" Text='<%# Bind("ID") %>' Font-Bold="True" Font-Size="8"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Stock Code">
                                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Font-Bold="True" Font-Size="9pt"></asp:Label>
                                                                                    <asp:Label ID="lblSelectedOrder" runat="server" Text='<%# Bind("SelectedOrder") %>' Visible="false"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Description">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblStockDescription" runat="server" Text='<%# Bind("StockDescription") %>' Font-Bold="True" Font-Size="8" Width="250px"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="left" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Production Date">
                                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblScheduledDate" runat="server" Text='<%# Bind("ScheduledDate") %>' Font-Bold="True" ForeColor="Black" Font-Size="8"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Production Qty">
                                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblScheduledQty" runat="server" Text='<%# Bind("ScheduledQty") %>' ForeColor="Black" Font-Size="8"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="On Hand">
                                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblOnHand" runat="server" Text='<%# Bind("OnHand") %>' ForeColor="Black" Font-Size="8"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="B.O. Qty">
                                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblBackOrderQty" runat="server" Text='<%# Bind("MBackOrderQty") %>' ForeColor="Black" Font-Size="8"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Ship Qty">
                                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblShipQty" runat="server" Text='<%# Bind("MShipQty") %>' ForeColor="Black" Font-Size="9pt"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="right" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Sales Order">
                                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSalesOrder" runat="server" Text='<%# Bind("SalesOrder") %>' ForeColor="Black" Font-Size="8pt"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Order<br>Date">
                                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblOrderDate" runat="server" Text='<%# Bind("OrderDate") %>' Font-Size="8pt" ForeColor="Black"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="Customer">
                                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("CustomerName") %>' ForeColor="Black" Font-Size="8pt" Width="195px"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="CustID">
                                                                                <HeaderStyle CssClass="CenterAligner" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("Customer") %>' Font-Size="8pt" Width="54px"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle HorizontalAlign="right" VerticalAlign="Top" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle BackColor="#666666" Font-Names="Arial" ForeColor="White" Height="25px" HorizontalAlign="Center" BorderWidth="1" BorderStyle="Solid" VerticalAlign="Bottom" />
                                                                    </asp:GridView>
                                                                </asp:Panel>
                                                            </div>
                                                        </asp:Panel>
                                                        <ajaxToolkit:HoverMenuExtender ID="hmeMatrix" runat="server" PopupControlID="pnlMatrix" PopupPosition="left" TargetControlID="lbnMatrix">
                                                        </ajaxToolkit:HoverMenuExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<span style='color:GoldenRod' class='fa fa-sticky-note'  title='Special Instructions'></span>" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="imgViewSpecialInstrs" runat="server" ForeColor="GoldenRod" Style="position: relative"><span class='fa fa-sticky-note'  title='Special Instructions'></span></asp:HyperLink>
                                                        <asp:Panel ID="pnlSpecialInstrs" runat="server">
                                                            <div style="position: relative">
                                                                <asp:Label ID="lblSpecialInstrs" runat="server" Text='<%# Bind("SpecialInstrs") %>' BackColor="GhostWhite" BorderColor="Black" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                                                                <ajaxToolkit:HoverMenuExtender ID="lblSpecialInstrs_HoverMenuExtender" runat="server" TargetControlID="imgViewSpecialInstrs" PopupControlID="pnlSpecialInstrs" PopupPosition="Center">
                                                                </ajaxToolkit:HoverMenuExtender>
                                                            </div>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<span style='color:Black' class='fa fa-truck'  title='Shipping Instructions'></span>" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="imgViewShippingInstrs" runat="server" ForeColor="Black" Style="position: relative"><span class='fa fa-truck'  title='Shipping Instructions'></span></asp:HyperLink>
                                                        <asp:Panel ID="pnlShippingInstrs" runat="server">
                                                            <div style="position: relative">
                                                                <asp:Label ID="lblShippingInstrs" runat="server" Text='<%# Bind("ShippingInstrs") %>' BackColor="GhostWhite" BorderColor="Black" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                                                                <ajaxToolkit:HoverMenuExtender ID="lblShippingInstrs_HoverMenuExtender" runat="server" TargetControlID="imgViewShippingInstrs" PopupControlID="pnlShippingInstrs" PopupPosition="Center">
                                                                </ajaxToolkit:HoverMenuExtender>
                                                            </div>
                                                        </asp:Panel>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" BackColor="GhostWhite" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Notes">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Image ID="imgNotes2" runat="server" ImageUrl="Images/pencil.png" Style="position: relative" />
                                                        <asp:Panel ID="pnlNotes2" runat="server" BackColor="white" Visible="true" Width="250px" BorderStyle="Solid" BorderWidth="1" BorderColor="Black">
                                                            <div style="position: relative">
                                                                <asp:Label ID="lblNotes" runat="server" Text='<%# Bind("Notes") %>' ForeColor="Black" Font-Bold="True" Width="200px"></asp:Label>
                                                            </div>
                                                        </asp:Panel>
                                                        <ajaxToolkit:HoverMenuExtender ID="hmeNotes2" runat="server" PopupControlID="pnlNotes2" PopupPosition="left" TargetControlID="imgNotes2">
                                                        </ajaxToolkit:HoverMenuExtender>
                                                    </ItemTemplate>
                                                    <ItemStyle BackColor="GhostWhite" HorizontalAlign="center" VerticalAlign="Top" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label28" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="Navy">Sales Order Comments</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:LinkButton ID="btnAddNotes" runat="server" CssClass="btn btn-info btn-s" Font-Bold="True" Font-Size="10pt" OnClick="btnAddNotes_Click">Add Note</asp:LinkButton>
                                        <ajaxToolkit:ConfirmButtonExtender ID="btnAddNotes_ConfirmButtonExtender" runat="server" BehaviorID="btnAddNotes_ConfirmButtonExtender" ConfirmText="Add Comment?" TargetControlID="btnAddNotes" ConfirmOnFormSubmit="True" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:TextBox ID="txtSaleOrderComment" runat="server" BackColor="LemonChiffon" CssClass="form-control text-uppercase" Font-Bold="True" Font-Size="12pt" ForeColor="Black" Height="75px" TextMode="MultiLine" ValidationGroup=""></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="Label33" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="12pt" ForeColor="Red">Please select a recipient</asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:CheckBox ID="chkCCtoAccountsReceivable" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black" Text="&amp;nbsp; Accounts Receivable" />
                                        <asp:CheckBox ID="chkCCtoCustomerService" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black" Text="&amp;nbsp; Customer Service" />&nbsp;
                                        <asp:CheckBox ID="chkCCtoLogistics" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black" Text="&amp;nbsp; Logistics" />&nbsp; 
                                        <asp:CheckBox ID="chkCCtoOperations" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black" Text="&amp;nbsp; Operations" />&nbsp; 
                                        <asp:CheckBox ID="chkCCtoProduction" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black" Text="&amp;nbsp; Production" />&nbsp; 
                                        <asp:CheckBox ID="chkCCtoQualityControl" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black" Text="&amp;nbsp; Quality Control" />
                                        <asp:CheckBox ID="chkCCtoTransfer" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Black" Text="&amp;nbsp; Transfer" />

                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblErrorNotes" runat="server" Font-Size="18pt" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:GridView ID="gvNotes" runat="server" AutoGenerateColumns="False" BackColor="#EEEEEE" Font-Bold="True" ForeColor="black" GridLines="None"
                                            OnRowCancelingEdit="gvNotes_RowCancelingEdit"
                                            OnRowCommand="gvNotes_RowCommand"
                                            OnRowDataBound="gvNotes_RowDataBound"
                                            OnRowEditing="gvNotes_RowEditing"
                                            OnRowUpdating="gvNotes_RowUpdating"
                                            OnRowDeleting="gvNotes_RowDeleting"
                                            Width="1375px">
                                            <AlternatingRowStyle BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" />
                                            <EmptyDataTemplate>
                                                <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Red" Text="No Comments Found"></asp:Label>
                                            </EmptyDataTemplate>
                                            <EmptyDataRowStyle HorizontalAlign="Center" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ibnDelete" runat="server" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Delete" CssClass="cursor" ImageUrl="~/Images/Delete.gif" ToolTip="Click to delete note." />
                                                        <ajaxToolkit:ConfirmButtonExtender ID="ibnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to delete this note?" Enabled="True" TargetControlID="ibnDelete" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Font-Underline="false" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSOCommentID" runat="server" Text='<%# Bind("SOCommentID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Left By">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Font-Size="10pt"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Comment">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtNote" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" Font-Size="9pt" Text='<%# Bind("Comment") %>' Width="1000px" TextMode="MultiLine" ForeColor="Black"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNote" runat="server" Text='<%# Bind("Comment") %>' Width="800px" Font-Size="10pt"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Date & Time">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("DateAdded") %>' Font-Size="10pt"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Dept">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDept" runat="server" Text='<%# Bind("Dept") %>' Font-Size="10pt"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="False">
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lbnUpdate" runat="server" CausesValidation="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Update" Font-Size="9pt" Text="Update" CssClass="btn btn-success btn-xs"></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnUpdate_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to update this note?" Enabled="True" TargetControlID="lbnUpdate" />
                                                        &nbsp;<asp:LinkButton ID="lbnCancel" runat="server" CausesValidation="False" CommandName="Cancel" Font-Size="9pt" Text="Cancel" CssClass="btn btn-danger btn-xs"></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbnEdit" runat="server" CausesValidation="False" CommandName="Edit" Font-Size="9pt" Text="Edit" CssClass="btn btn-warning btn-xs"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <HeaderStyle BackColor="Black" Font-Size="12pt" ForeColor="White" />
                                        </asp:GridView>
                                    </td>
                                </tr>

                                <tr>
                                    <td align="left">&nbsp;</td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </div>
            <div>
                <asp:Button ID="btnDeliveries" runat="server" Style="display: none" Text="Button" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderDeliveries" runat="server" BackgroundCssClass="popup_background"
                    CancelControlID="" DynamicServicePath="" Enabled="True" PopupControlID="pnlDeliveries" TargetControlID="btnDeliveries" Y="100">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlDeliveries" runat="server" CssClass="modalPopup2" Visible="true" Style="display: none; width: 1000px; height: 700px; padding: 10px" ScrollBars="Vertical">
                    <table id="tblPopUpDeliveries" style="border: 1px solid black; font-size: 7pt; height: 350px; background-color: #FFFFFF;" width="100%" align="center">
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="Navy">Delivery Details</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvDeliveries" runat="server" CellPadding="4" ForeColor="#333333"
                                    GridLines="None"
                                    HorizontalAlign="Center"
                                    Font-Size="9pt"
                                    PageSize="15" AllowSorting="True"
                                    OnSorting="gvDetails_Sorting"
                                    Width="975px"
                                    AutoGenerateColumns="False"
                                    ShowFooter="True" OnRowDataBound="gvDeliveries_RowDataBound" OnRowCommand="gvDeliveries_RowCommand">
                                    <FooterStyle BackColor="Silver" />
                                    <HeaderStyle Font-Size="8pt" VerticalAlign="Top" BackColor="#CCCCCC" />
                                    <AlternatingRowStyle BackColor="LemonChiffon" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sales Order #" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesOrder" runat="server" Text='<%# Bind("SalesOrder") %>' ForeColor="#00569D" CssClass="NoUnderline"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delivery ID" SortExpression="DeliveryID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeliveryID" runat="server" Text='<%# Bind("DeliveryID") %>' ForeColor="#00569D" CssClass="NoUnderline"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Type" SortExpression="DeliveryTypeID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeliveryTypeID" runat="server" Text='<%# Bind("DeliveryTypeID") %>' ForeColor="#00569D" CssClass="NoUnderline"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date<br/>Scheduled" SortExpression="DateScheduled">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDateScheduled" runat="server" Text='<%# Bind("DateScheduled") %>' ForeColor="#00569D" CssClass="NoUnderline"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Date<br/>Delivered" SortExpression="DateDelivered">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDateDelivered" runat="server" Text='<%# Bind("DateDelivered") %>' ForeColor="#00569D" CssClass="NoUnderline"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delivery/Will Call<br/>Status" SortExpression="DeliveryStatus">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDeliveryStatus" runat="server" Text='<%# Bind("DeliveryStatus") %>' ForeColor="#00569D" CssClass="NoUnderline"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="COD" SortExpression="COD">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCOD" runat="server" Text='<%# Bind("IsCOD") %>' ForeColor="#00569D" CssClass="NoUnderline"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Check<br/>Number" SortExpression="CheckNumber">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCheckNumber" runat="server" Text='<%# Bind("CheckNumber") %>' ForeColor="#00569D" CssClass="NoUnderline"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>' ForeColor="#00569D" CssClass="NoUnderline"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Truck" SortExpression="Truck">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTruck" runat="server" Text='<%# Bind("Truck") %>' ForeColor="#00569D" CssClass="NoUnderline"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Comments" SortExpression="Comments">
                                            <ItemTemplate>
                                                <asp:Label ID="lblComments" runat="server" Text='<%# Bind("Comments") %>' ForeColor="#00569D" CssClass="NoUnderline"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>

                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Image ID="imgFlaggedDR" runat="server" ImageUrl="~/Images/Flag.gif" />
                                                <asp:Label ID="lblDRFlag" runat="server" Text='<%# Bind("DeliveryReceiptFlag") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbnViewDocs" runat="server" CausesValidation="false"
                                                    CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                    CommandName="ViewDocs" Text="View Docs"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbnScanner" runat="server" CausesValidation="false"
                                                    CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                    CommandName="Scan" Text="To Scanner"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbnEdit" runat="server" CausesValidation="false"
                                                    CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                    CommandName="Edit" Text="Edit"></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="Cancel2" runat="server" CssClass="btn btn-primary" OnClick="btnCancel2_Click" Text="Cancel" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblErrorDeliveries" runat="server" Font-Size="8pt" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div>
                <asp:Button ID="btnShowPlacedOrders" runat="server" Style="display: none" Text="Button" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderPlacedOrders" runat="server" BackgroundCssClass="popup_background"
                    CancelControlID="" DynamicServicePath="" Enabled="True" PopupControlID="pnlPlacedOrders" TargetControlID="btnShowPlacedOrders" Y="100">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlPlacedOrders" runat="server" CssClass="modalPopup2" Visible="true" Style="display: block; width: 1150px; height: 600px; padding: 10px" ScrollBars="Vertical">

                    <table id="tblPlacedOrders" style="border: 1px solid black; font-size: 7pt; background-color: #FFFFFF;" width="1120px" align="center">
                        <tr>
                            <td align="center">
                                <asp:LinkButton ID="lbnCancelPlacedOrders" runat="server" Font-Bold="True" OnClick="bntCancel4_Click" Style="float: right; position: relative; top: 5px; right: 15px;" ForeColor="Gray"><span class="close">&times</span></asp:LinkButton></td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="Navy">Place Orders By Date Range</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table width="500px">
                                    <tr>
                                        <td align="left" style="width: 200px">
                                            <asp:Label ID="lblPeriod" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black" Text="Period:"></asp:Label>
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddlPeriod" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="10pt" ForeColor="Black" OnSelectedIndexChanged="ddlPeriod_SelectedIndexChanged">
                                                <asp:ListItem>Range</asp:ListItem>
                                                <asp:ListItem>Today</asp:ListItem>
                                                <asp:ListItem>Yesterday</asp:ListItem>
                                                <asp:ListItem Selected="True">Current Month</asp:ListItem>
                                                <asp:ListItem>Last Month</asp:ListItem>
                                                <asp:ListItem>Last 3 Months</asp:ListItem>
                                                <asp:ListItem>Last 6 Months</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td align="left" style="width: 5px">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblStartDate" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black" Text="Date From:"></asp:Label><br />
                                        </td>
                                        <td align="right" style="width: 275px">
                                            <asp:TextBox ID="txtStartDate" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup=""></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="txtStartDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgStartDate" TargetControlID="txtStartDate"></ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="txtStartDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtStartDate"></ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:MaskedEditValidator ID="txtStartDateMEV" runat="server" ControlExtender="txtStartDateMEE" ControlToValidate="txtStartDate" Display="None" EmptyValueMessage="Please enter a start date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                            <ajaxToolkit:ValidatorCalloutExtender ID="txtStartDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtStartDateMEV" />
                                        </td>
                                        <td align="left" style="width: 5px">
                                            <asp:ImageButton ID="imgStartDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <asp:Label ID="lblEndDate" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black" Text="Date To:"></asp:Label><br />
                                        </td>
                                        <td align="right">
                                            <asp:TextBox ID="txtEndDate" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup=""></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="txtEndDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgEndDate" TargetControlID="txtEndDate"></ajaxToolkit:CalendarExtender>
                                            <ajaxToolkit:MaskedEditExtender ID="txtEndDateMEE" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtEndDate"></ajaxToolkit:MaskedEditExtender>
                                            <ajaxToolkit:MaskedEditValidator ID="txtEndDateMEV" runat="server" ControlExtender="txtEndDateMEE" ControlToValidate="txtEndDate" Display="None" EmptyValueMessage="Please enter an end date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                            <ajaxToolkit:ValidatorCalloutExtender ID="txtEndDateVCE" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtEndDateMEV" />
                                        </td>
                                        <td align="left" style="width: 5px">
                                            <asp:ImageButton ID="imgEndDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" Visible="False" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:LinkButton ID="btnRunPlacedOrders" runat="server" CssClass="btn btn-info btn-sm" ForeColor="White" OnClick="btnRunPlacedOrders_Click" ToolTip="Click to run Placed Order by Date Range report" Width="150px"><i class="fa fa-file-text-o"></i>&nbsp;Run Summary Report</asp:LinkButton>
                                <asp:LinkButton ID="btnRunPlacedOrdersDetails" runat="server" CssClass="btn btn-info btn-sm" ForeColor="White" OnClick="btnRunPlacedOrdersDetails_Click" ToolTip="Click to run Placed Orders Details by Date Range report" Width="150px"><i class="fa fa-file-text-o"></i>&nbsp;Run Details Report</asp:LinkButton>
                                <asp:LinkButton ID="btnExportExcelPlacedOrdersSummary" runat="server" CssClass="btn btn-success btn-sm" ForeColor="White" OnClick="btnExportExcelPlacedOrdersSummary_Click"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT SUMMARY</asp:LinkButton>
                                <asp:LinkButton ID="btnExportExcelPlacedOrdersDetails" runat="server" CssClass="btn btn-success btn-sm" ForeColor="White" OnClick="btnExportExcelPlacedOrdersDetails_Click"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT DETAILS</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblOrdersPlacedError" runat="server" Font-Size="16pt" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:GridView ID="gvPlacedOrders" runat="server" CellPadding="4" ForeColor="#333333"
                                    GridLines="None"
                                    HorizontalAlign="Center"
                                    Font-Size="9pt"
                                    PageSize="30" AllowSorting="True"
                                    OnSorting="gvPlacedOrders_Sorting"
                                    Width="1000px"
                                    AutoGenerateColumns="False"
                                    ShowFooter="True" OnRowDataBound="gvPlacedOrders_RowDataBound">
                                    <FooterStyle BackColor="Silver" />
                                    <HeaderStyle Font-Size="8pt" VerticalAlign="Top" BackColor="#CCCCCC" />
                                    <AlternatingRowStyle BackColor="LemonChiffon" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Red" Text="No Records Found"></asp:Label>
                                    </EmptyDataTemplate>
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Date" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMonthDayYear" runat="server" Text='<%# Bind("MonthDayYear") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="65px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Value Original" SortExpression="TotalValue">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalValue" runat="server" Text='<%# Bind("TotalValue") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalValueTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Value Current" SortExpression="TotalValueUnchanged">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalValueUnchanged" runat="server" Text='<%# Bind("TotalValueUnchanged") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalValueUnchangedTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Total Value Diff" SortExpression="TotalValueDiff">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalValueDiff" runat="server" Text='<%# Bind("TotalValueDiff") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="150px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalValueDiffTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Line Count Original" SortExpression="LineCount">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblLineCount" runat="server" Text='<%# Bind("LineCount") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="75px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblLineCountTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Line Count Current" SortExpression="LineCountUnchanged">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblLineCountUnchanged" runat="server" Text='<%# Bind("LineCountUnchanged") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="75px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblLineCountUnchangedTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Line Count Diff" SortExpression="LineCountDiff">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblLineCountDiff" runat="server" Text='<%# Bind("LineCountDiff") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="75px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblLineCountDiffTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Count Original" SortExpression="OrderCount">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderCount" runat="server" Text='<%# Bind("OrderCount") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="75px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblOrderCountTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Count Current" SortExpression="OrderCountUnchanged">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderCountUnchanged" runat="server" Text='<%# Bind("OrderCountUnchanged") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="75px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblOrderCountUnchangedTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Count Diff" SortExpression="OrderCountUnchanged">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderCountDiff" runat="server" Text='<%# Bind("OrderCountDiff") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="75px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblOrderCountDiffTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:LinkButton ID="lbnUpdateNotes" runat="server" CssClass="btn btn-info btn-sm" ForeColor="White" OnClick="lbnUpdateNotes_Click"
                                    ToolTip="Click to Update Place Orders Note" Width="150px"><i class="fa fa-pencil"></i>&nbsp;Update Details Report</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvPlacedOrderDetails" runat="server" CellPadding="4" ForeColor="#333333"
                                    GridLines="None"
                                    HorizontalAlign="Center"
                                    Font-Size="9pt"
                                    PageSize="30" AllowSorting="True"
                                    OnSorting="gvPlacedOrderDetails_Sorting"
                                    Width="1000px"
                                    AutoGenerateColumns="False"
                                    ShowFooter="True" OnRowDataBound="gvPlacedOrderDetails_RowDataBound">
                                    <FooterStyle BackColor="Silver" />
                                    <HeaderStyle Font-Size="8pt" VerticalAlign="Top" BackColor="#CCCCCC" />
                                    <AlternatingRowStyle BackColor="LemonChiffon" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="lblError" runat="server" Font-Bold="True" Font-Size="11pt" ForeColor="Red" Text="No Records Found"></asp:Label>
                                    </EmptyDataTemplate>
                                    <EmptyDataRowStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Date" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMonthDayYear" runat="server" Text='<%# Bind("MonthDayYear") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="65px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sales Order" SortExpression="SalesOrder">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesOrder" runat="server" Text='<%# int.Parse(Eval("SalesOrder").ToString()) %>'></asp:Label>
                                                <asp:Label ID="lblSalesOrderLong" runat="server" Text='<%# Bind("SalesOrder") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer" SortExpression="Customer">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" SortExpression="Name">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>' Width="300px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Total<br>Value<br>Original" SortExpression="TotalValue">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalValue" runat="server" Text='<%# Bind("TotalValue", "{0:n2}") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="100px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalValueTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total<br> Value<br>Current" SortExpression="TotalValueUnchanged">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalValueUnchanged" runat="server" Text='<%# Bind("TotalValueUnchanged", "{0:n2}") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="75px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalValueUnchangedTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>


                                        <asp:TemplateField HeaderText="Total<br>Value<br>Diff" SortExpression="TotalValueDiff">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalValueDiff" runat="server" Text='<%# Bind("TotalValueDiff", "{0:n2}") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="75px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalValueDiffTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Line Count Original" SortExpression="LineCount">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblLineCount" runat="server" Text='<%# Bind("LineCount") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblLineCountTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Line Count Current" SortExpression="LineCountUnchanged">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblLineCountUnchanged" runat="server" Text='<%# Bind("LineCountUnchanged") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblLineCountUnchangedTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Line<br>Count<br> Diff" SortExpression="LineCountDiff">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblLineCountDiff" runat="server" Text='<%# Bind("LineCountDiff") %>' ForeColor="#00569D" CssClass="NoUnderline" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblLineCountDiffTotal" runat="server" Font-Bold="True" Font-Size="9pt"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Note">
                                            <HeaderStyle CssClass="CenterAligner" />
                                            <ItemTemplate>
                                                <asp:DropDownList ID="ddlPlaceOrdersNote" runat="server" BackColor="LemonChiffon" ForeColor="Black"></asp:DropDownList>
                                                <asp:Label ID="lblSorPlacedOrdersNotesCommentOptionsID" runat="server" Text='<%# Bind("SorPlacedOrdersNotesCommentOptionsID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="center" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="bntCancel4" runat="server" CssClass="btn btn-primary" Text="Cancel" OnClick="bntCancel4_Click" />

                            </td>
                        </tr>
                        <tr>
                            <td align="center">&nbsp;</td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportExcel" />
            <asp:PostBackTrigger ControlID="btnExportTopFifteenList" />
            <asp:PostBackTrigger ControlID="btnExportExcelPlacedOrdersSummary" />
            <asp:PostBackTrigger ControlID="btnExportExcelPlacedOrdersDetails" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">

        //This Script is used to maintain Grid Scroll on Partial Postback
        var scrollTop;

        //Register Begin Request and End Request 
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        //Get The Div Scroll Position
        function BeginRequestHandler(sender, args) {
            var m = document.getElementById('DivData');
            scrollTop = m.scrollTop;
        }

        //Set The Div Scroll Position
        function EndRequestHandler(sender, args) {
            var m = document.getElementById('DivData');
            m.scrollTop = scrollTop;
        }

        $(document).ready(function () {
            $('#MainContent_gvRecord tr').click(function (e) {
                $('#MainContent_gvRecord tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });
            $('#MainContent_gvDetails tr').click(function (e) {
                $('#MainContent_gvDetails tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });

        });

    </script>
</asp:Content>

