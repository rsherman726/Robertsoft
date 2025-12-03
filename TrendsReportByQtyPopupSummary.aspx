<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrendsReportByQtyPopupSummary.aspx.cs" Inherits="TrendsReportByQtyPopupSummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchasing Trends Report Summary</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.js"></script>
    <script type="text/javascript" src="Scripts/kissy-min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"  AsyncPostBackTimeout="3600"></asp:ScriptManager>
        <div>
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
                    <div style="text-align: left; padding: 10px; margin: 10px">

                        <asp:Label ID="Label22" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="18pt" ForeColor="Navy">Purchasing Trends Report By Quantity</asp:Label>

                    </div>
                    <div style="text-align: left; padding: 10px; margin: 10px">

                        <asp:Label ID="lblDateRange" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="16pt" ForeColor="Navy"></asp:Label>
                        &nbsp;
                        <asp:CheckBox ID="chkShowColor" runat="server" Font-Names="Arial" Font-Size="12pt" ForeColor="Navy" Text="&nbsp;Show Color" AutoPostBack="True" OnCheckedChanged="chkShowColor_CheckedChanged" Checked="True" />

                    </div>
                    <div>


                        <asp:GridView ID="gvPurchasingTrends" runat="server" AllowSorting="True" BackColor="White" BorderColor="#999999" BorderStyle="None" 
                            BorderWidth="1px" CellPadding="3" GridLines="Vertical" OnRowDataBound="gvPurchasingTrends_RowDataBound" OnSorting="gvPurchasingTrends_Sorting"
                            ShowFooter="True">
                            <AlternatingRowStyle BackColor="#DCDCDC" />
                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#0000A9" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#000065" />
                        </asp:GridView>
                    </div>
                    <asp:Label ID="lblError" runat="server" CssClass="Text_Small" EnableViewState="False" Font-Bold="True" Font-Size="10pt" ForeColor="Red"></asp:Label>
                    <div style="text-align: left; padding: 10px; margin: 10px">
                        <table id="Table1">
                            <tr>
                                <td align="center">
                                    <asp:ImageButton ID="imgExportExcel1" runat="server" ImageUrl="~/Images/ExportToExcel.GIF" OnClick="imgExportExcel1_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="imgExportExcel1" />
                </Triggers>
            </asp:UpdatePanel>

        </div>

    </form>
</body>
</html>
