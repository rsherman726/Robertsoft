<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" EnableEventValidation="false" AutoEventWireup="true" CodeFile="Managers.aspx.cs" Inherits="Managers" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        $(document).ready(function () {
             $('#MainContent_gvRecord tr').click(function (e) {
                $('#MainContent_gvRecord tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });
            window.onbeforeunload = function () { null; }
        });


        function isPostBack() {//Use during postbacks...
             $('#MainContent_gvRecord tr').click(function (e) {
                $('#MainContent_gvRecord tr').removeClass('highlighted');
                $(this).addClass('highlighted');
            });

            window.onbeforeunload = function () { null; }

        }; //End Postback... 

    </script>
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
            height: 35px;
            background-color: #EEEEEE;
            border: 1px solid #CCC;
        }

        .RowStyle2 {
            height: 35px;
            background-color: #f3fcf3;
        }

        .AlternateRowStyle {
            height: 35px;
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


 
    </style>
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <div style="position: fixed; left: 50%; height: 40px; width: 150px; z-index: 10000;">
        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanelPromo2" DisplayAfter="1" ClientIDMode="Static">
            <ProgressTemplate>
                <table>
                    <tbody>
                        <tr>
                            <td align="Left" style="width: 12px">
                                <img src="Images/loader.gif" alt="" />
                            </td>
                            <td align="left"><span style="color: #ffffff">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing <span class="">....</span> </strong></span></span></td>
                        </tr>
                    </tbody>
                </table>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <asp:UpdatePanel ID="UpdatePanelPromo2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <a id="popUpAnchor" runat="server" onclick="return true" onserverclick="fromPopUp"></a>
            <asp:Panel ID="pnlMain" runat="server" DefaultButton="">
                <table align="Center" width="900px" class="BoxShadow4" style="background-color: #F0F0F0">
                    <tr>
                        <td align="center">
                            <table width="100%">
                                <tr>
                                    <td class="hdr" align="center">
                                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                            Text="Manager Page" ForeColor="Black"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                        <a id="A1" runat="server" onclick="return true" onserverclick="fromPopUp"></a>
                                        <asp:Panel ID="Panel1" runat="server">
                                            <table width="100%" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                                align="center">

                                                <tr>
                                                    <td>&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <table id="tblAdd" align="center">
                                                            <tr>
                                                                <td align="char">&nbsp;
                                                                <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Navy" Text="Managers: to record the hours worked for the employees, click on the &quot;Record Hours&quot; link in the grid below."></asp:Label>
                                                                </td>
                                                                <td align="char">&nbsp;</td>
                                                            </tr>
                                                            <tr class="rowColors">
                                                                <td align="center" valign="middle">
                                                                    <asp:Panel ID="pnlManagers" runat="server" ScrollBars="Vertical" Height="600px">
                                                                        <asp:GridView ID="gvRecord" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="Black"
                                                                            GridLines="Horizontal" Width="1100px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px"
                                                                            AllowPaging="false" OnPageIndexChanging="gvRecord_PageIndexChanging" OnRowCommand="gvRecord_RowCommand" OnRowDataBound="gvRecord_RowDataBound" AllowSorting="True" OnSorting="gvRecord_Sorting">
                                                                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="Job#" SortExpression="JobNumber">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblJobNumber" runat="server" Text='<%# Bind("JobNumber") %>' Font-Bold="True"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="StockCode" SortExpression="StockCode">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Font-Bold="True"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Job Description" SortExpression="JobDescription">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblJobDescription" runat="server" Text='<%# Bind("JobDescription") %>' Font-Bold="True"></asp:Label>
                                                                                        <asp:Label ID="lblJob" runat="server" Text='<%# Bind("Job") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Assigned Date" SortExpression="DateAdded">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblAssignedDate" runat="server" Text='<%# Bind("DateAdded") %>' Font-Bold="True"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Production Line" SortExpression="ProductionLine">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lbProductionLine" runat="server" Text='<%# Bind("ProductionLine") %>' Font-Bold="True"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Hours" SortExpression="Status">
                                                                                    <HeaderStyle CssClass="CenterAligner" />
                                                                                    <ItemTemplate>
                                                                                        <asp:HyperLink ID="hlRecord" runat="server" Text="Record Hours/Completed">Record Hours</asp:HyperLink>
                                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" />
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
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </td>
                                                                <td align="center" valign="middle">&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">&nbsp; </td>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp; &nbsp; </td>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                            <tr>
                                                                <td>&nbsp; </td>
                                                                <td>&nbsp;</td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
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
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
