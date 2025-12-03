<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="IngredientCostTrendsReport.aspx.cs" Inherits="IngredientCostTrendsReport" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript" src="Scripts/jquery-1.10.2.min.js"></script>
    <style type="text/css">
        tbody tr.selected td {
            background: none repeat scroll 0 0 #FFCF8B;
            color: #f00;
        }


        .modalBackground {
            background-color: #000;
            filter: alpha(opacity=50);
            opacity: 0.5;
        }

        .modalPopup {
            background-color: #EEEEEE;
            border-width: 3px;
        }

        .RowStyle {
            height: 20px;
            background-color: #EEEEEE;
            border: 1px solid #CCC;
        }

        .RowStyle2 {
            background-color: #f3fcf3;
        }

        .AlternateRowStyle {
            height: 20px;
            background-color: #e8e6e6;
            border: 1px solid #CCC;
        }

        a {
            color: #000;
            text-decoration: underline;
        }

        .NoUnderline {
            text-decoration: none;
        }

        .block_design {
            background-color: #999;
            display: block;
            height: 5px;
            width: 1500px;
            margin-top: 10px;
        }

        .rowColors tr:hover {
            background-color: white;
            transition-property: background;
            transition-duration: 100ms;
            transition-delay: 5ms;
            transition-timing-function: linear;
        }

        tr.highlighted td {
            background: #ffd77d;
        }



        .auto-style1 {
            width: 334px;
        }

        .auto-style2 {
            width: 200px;
        }

        .auto-style4 {
            height: 21px;
        }
    </style>

    <script type="text/javascript">

        var prevRowIndex;

        function ChangeRowColor(row, rowIndex) {
            if (row == null) {
                return;
            }
            var parent = document.getElementById(row);
            var currentRowIndex = parseInt(rowIndex) + 1;

            if (prevRowIndex == currentRowIndex) {
                return;
            } else if (prevRowIndex != null) {
                parent.rows[prevRowIndex].style.backgroundColor = "#fff";
            }

            parent.rows[currentRowIndex].style.backgroundColor = "#FFCF8B";
            prevRowIndex = currentRowIndex;

            <%--$('#<%= LabelNote.ClientID %>').text(currentRowIndex);--%>

            $('#<%= hfParentContainer.ClientID %>').val(row);
            $('#<%= hfCurrentRowIndex.ClientID %>').val(rowIndex);
        }

        $(function () {
            RetainSelectedRow();
        });

        function RetainSelectedRow() {
            var parent = $('#<%= hfParentContainer.ClientID %>').val();
            var currentIndex = $('#<%= hfCurrentRowIndex.ClientID %>').val();
            if (parent != null) {
                ChangeRowColor(parent, currentIndex);
            }
        }

        $(document).ready(function () {
            $('#MainContent_gvIngredientCostTrends tr').on('click', function () {
                $('#MainContent_gvIngredientCostTrends tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });
        });





        function isPostBack() {//Use during postbacks...
            $('#MainContent_gvIngredientCostTrends tr').on('click', function () {
                $('#MainContent_gvIngredientCostTrends tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });


            var prevRowIndex;

            function ChangeRowColor(row, rowIndex) {
                if (row == null) {
                    return;
                }
                var parent = document.getElementById(row);
                var currentRowIndex = parseInt(rowIndex) + 1;

                if (prevRowIndex == currentRowIndex) {
                    return;
                } else if (prevRowIndex != null) {
                    parent.rows[prevRowIndex].style.backgroundColor = "#fff";
                }

                parent.rows[currentRowIndex].style.backgroundColor = "#FFCF8B";
                prevRowIndex = currentRowIndex;

            <%--$('#<%= LabelNote.ClientID %>').text(currentRowIndex);--%>

                $('#<%= hfParentContainer.ClientID %>').val(row);
                $('#<%= hfCurrentRowIndex.ClientID %>').val(rowIndex);
            }

            $(function () {
                RetainSelectedRow();
            });

            function RetainSelectedRow() {
                var parent = $('#<%= hfParentContainer.ClientID %>').val();
                var currentIndex = $('#<%= hfCurrentRowIndex.ClientID %>').val();
                if (parent != null) {
                    ChangeRowColor(parent, currentIndex);
                }
            }
        }

    </script>
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
                    <asp:Panel ID="pnlMain" runat="server" DefaultButton="btnPreview">
                        <table align="center" width="1600px">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Ingredient Cost Trends Report" ForeColor="Black"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="background-color: lightblue;" align="center" class="JustRoundedEdgeBothSmall">
                                    <asp:Panel ID="pnlSearchCriteria" runat="server" Width="1600px">
                                        <table width="1600px" align="center">
                                            <tr>
                                                <td>
                                                    <table align="center">
                                                        <tr>
                                                            <td valign="top" align="left">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td align="center">&nbsp;</td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Panel ID="pnlStockCodeRange" runat="server">
                                                                                <table width="100%" align="center">
                                                                                    <tr>
                                                                                        <td align="center">&nbsp;</td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <table width="100%" align="center">
                                                                                                <tr>
                                                                                                    <td align="center">
                                                                                                        <asp:Label ID="Label77" runat="server" Font-Bold="True" ForeColor="Black" Text="Stock Code From"></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="center">
                                                                                                        <asp:TextBox ID="txtStockCodeFrom" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="300px" placeholder="StockCode From"></asp:TextBox>
                                                                                                        <ajaxToolkit:AutoCompleteExtender ID="txtStockCodeFrom_AutoCompleteExtender" runat="server" CompletionInterval="0"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                                                            CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes" ServicePath="" TargetControlID="txtStockCodeFrom" UseContextKey="True">
                                                                                                        </ajaxToolkit:AutoCompleteExtender>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="center">
                                                                                                        <asp:Label ID="Label73" runat="server" Font-Bold="True" ForeColor="Black" Text="Stock Code To"></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="center">
                                                                                                        <asp:TextBox ID="txtStockCodeTo" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Font-Size="12pt" ForeColor="Black" ValidationGroup="" Width="300px" placeholder="StockCode To"></asp:TextBox>
                                                                                                        <ajaxToolkit:AutoCompleteExtender ID="txtStockCodeTo_AutoCompleteExtender" runat="server" CompletionInterval="0"
                                                                                                            CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem"
                                                                                                            CompletionSetCount="25" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListStockCodes" ServicePath="" TargetControlID="txtStockCodeTo" UseContextKey="True">
                                                                                                        </ajaxToolkit:AutoCompleteExtender>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </asp:Panel>

                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top">&nbsp;
                                                                <asp:Label ID="Label78" runat="server" Font-Bold="True" ForeColor="Blue" Text="For Single Stock Code Searches leave Stock Code To Blank"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top">&nbsp;
                                                            <asp:Label ID="lblErrorRange" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="14pt" ForeColor="Red"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="top">&nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:LinkButton ID="btnPreview" runat="server" OnClick="btnPreview_Click" CssClass="btn btn-info" Width="200px" ForeColor="White" ToolTip="Click to run full report"><i class="fa fa-file-text-o" ></i>&nbsp;Full Report</asp:LinkButton>
                                                    <asp:LinkButton ID="btnReset" runat="server" OnClick="btnReset_Click" CssClass="btn btn-danger" Width="200px" ForeColor="White" ToolTip="Click to reset form"><i class="fa fa-refresh" ></i>&nbsp;Reset Form</asp:LinkButton>
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
                            <tr>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>

                                <td align="center">
                                    <asp:HiddenField ID="hfCurrentRowIndex" runat="server"></asp:HiddenField>
                                    <asp:HiddenField ID="hfParentContainer" runat="server"></asp:HiddenField>
                                    <asp:GridView ID="gvIngredientCostTrends" runat="server" Width="1650px"
                                        AutoGenerateColumns="true"
                                        OnRowDataBound="gvIngredientCostTrends_RowDataBound"
                                        OnSorting="gvIngredientCostTrends_Sorting"
                                        GridLines="Horizontal"
                                        EnableViewState="false"
                                        AllowSorting="true">
                                        <HeaderStyle BackColor="#333333" Font-Bold="True" HorizontalAlign="Center" ForeColor="White" VerticalAlign="Bottom" CssClass="CenterAligner" />
                                        <RowStyle ForeColor="Black" HorizontalAlign="Center" />
                                        <AlternatingRowStyle BackColor="WhiteSmoke" ForeColor="Black" HorizontalAlign="Center" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table id="Table1" align="center">
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="imgExportExcel1" runat="server" OnClick="imgExportExcel1_Click" CssClass="btn btn-success" ForeColor="White" Width="200px"><i class="glyphicon glyphicon-export"></i></i>&nbsp;EXPORT TO EXCEL</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center"></td>
                            </tr>
                            <tr>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                        </table>
                    </asp:Panel>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="imgExportExcel1" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

