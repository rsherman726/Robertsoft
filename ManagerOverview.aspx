<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ManagerOverview.aspx.cs" Inherits="ManagerOverview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .gridViewPager td {
            padding-left: 6px;
            padding-right: 6px;
            padding-top: 1px;
            padding-bottom: 2px;
        }
        /************ PasswordStrength Related Styles ***********************/
        .BoxShadow4 {
            /*Internet Explorer*/
            border-Top-right-radius: 20px;
            border-Top-left-radius: 20px;
            border-bottom-right-radius: 20px;
            border-bottom-left-radius: 20px;
            /*Modzilla*/
            -moz-border-radius-bottomright: 20px;
            -moz-border-radius-bottomleft: 20px;
            -moz-border-radius-topright: 20px;
            -moz-border-radius-topleft: 20px;
            /*Safari*/
            -webkit-border-bottom-left-radius: 20px; /* bottom left corner */
            -webkit-border-bottom-right-radius: 20px; /* bottom right corner */
            -webkit-border-top-left-radius: 20px; /* top left corner */
            -webkit-border-top-right-radius: 20px; /* top right corner */
            /*Opera*/
            -o-border-radius-bottomright: 20px;
            -o-border-radius-bottomleft: 20px;
            -o-border-radius-topright: 20px;
            -o-border-radius-topleft: 20px;
            /* For IE 8 */
            -ms-filter: "progid:DXImageTransform.Microsoft.Shadow(Strength=12, Direction=135, Color='Gray')";
            /* For IE 5.5 - 7 */
            filter: progid:DXImageTransform.Microsoft.Shadow(Strength=12, Direction=135, Color='Gray');
        }

        .TextIndicator_TextBox1 {
            background-color: Gray;
            color: White;
            font-family: Arial;
            font-size: x-small;
            font-style: italic;
            padding: 2px 3px 2px 3px;
        }

        .TextIndicator_TextBox1_Strength1 {
            background-color: Gray;
            color: White;
            font-family: Arial;
            font-size: x-small;
            font-style: italic;
            padding: 2px 3px 2px 3px;
            font-weight: bold;
        }

        .TextIndicator_TextBox1_Strength2 {
            background-color: Gray;
            color: Yellow;
            font-family: Arial;
            font-size: x-small;
            font-style: italic;
            padding: 2px 3px 2px 3px;
            font-weight: bold;
        }

        .TextIndicator_TextBox1_Strength3 {
            background-color: Gray;
            color: #FFCAAF;
            font-family: Arial;
            font-size: x-small;
            font-style: italic;
            padding: 2px 3px 2px 3px;
            font-weight: bold;
        }

        .TextIndicator_TextBox1_Strength4 {
            background-color: Gray;
            color: Aqua;
            font-family: Arial;
            font-size: x-small;
            font-style: italic;
            padding: 2px 3px 2px 3px;
            font-weight: bold;
        }

        .TextIndicator_TextBox1_Strength5 {
            background-color: Gray;
            color: #93FF9E;
            font-family: Arial;
            font-size: x-small;
            font-style: italic;
            padding: 2px 3px 2px 3px;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
        <ContentTemplate>
            <table align="Center" width="900px" class="BoxShadow4" style="background-color: #F0F0F0">
                <tr>
                    <td align="left">

                        <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" AssociatedUpdatePanelID="UpdatePanelPromo" DisplayAfter="1">
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

                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table width="100%">
                            <tr>
                                <td class="hdr" align="center">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                        Text="Manager Overview Page" ForeColor="Black"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlMain" runat="server">
                                        <table width="100%" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                            align="center">

                                            <tr>
                                                <td align="center">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="Label2232" runat="server" Font-Bold="True" Font-Names="arial" Text="Managers:" ForeColor="Black"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlManagers" runat="server" AutoPostBack="True" BackColor="LemonChiffon" OnSelectedIndexChanged="ddlManagers_SelectedIndexChanged" Width="255px" CssClass="form-control" ForeColor="Black">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <table id="tblAdd" align="center">
                                                        <tr>
                                                            <td align="char">&nbsp;
                                                            </td>
                                                            <td align="char">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" valign="middle">
                                                                <asp:GridView ID="gvRecord" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="Black"
                                                                    GridLines="Horizontal" Width="1200px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid"
                                                                    BorderWidth="1px" AllowPaging="True" OnPageIndexChanging="gvRecord_PageIndexChanging" OnRowDataBound="gvRecord_RowDataBound" PageSize="25">
                                                                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Jobs">
                                                                            <HeaderStyle CssClass="CenterAligner" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblJobDescription" runat="server" Text='<%# Bind("JobDescription") %>' Font-Bold="True" Font-Size="8pt"></asp:Label>
                                                                                <asp:Label ID="lblJob" runat="server" Text='<%# Bind("Job") %>' Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Completed Date">
                                                                            <HeaderStyle CssClass="CenterAligner" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblCompletedDate" runat="server" Text='<%# Bind("CompletedDate") %>' Font-Bold="True" Font-Size="9pt"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Assigned Date">
                                                                            <HeaderStyle CssClass="CenterAligner" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblAssignedDate" runat="server" Text='<%# Bind("DateAdded") %>' Font-Bold="True" Font-Size="9pt"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Manager">
                                                                            <HeaderStyle CssClass="CenterAligner" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblManager" runat="server" Text='<%# Bind("Manager") %>' Font-Bold="True" Font-Size="9pt"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Production Line">
                                                                            <HeaderStyle CssClass="CenterAligner" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblProductionLine" runat="server" Text='<%# Bind("ProductionLine") %>' Font-Bold="True" Font-Size="9pt"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Hours">
                                                                            <HeaderStyle CssClass="CenterAligner" />
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRecord" runat="server" Text="Record Hours/Completed" Font-Bold="True"></asp:Label>
                                                                                <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                                                                    <HeaderStyle BackColor="#333333" Font-Bold="False" ForeColor="White" />

                                                                    <PagerStyle BackColor="#2461BF" Font-Bold="true" Font-Size="12pt" ForeColor="white" HorizontalAlign="Center" CssClass="gridViewPager" />
                                                                    <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="First" LastPageText="Last" />
                                                                    <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                                                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                                    <SortedAscendingHeaderStyle BackColor="#4B4B4B" />
                                                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                                    <SortedDescendingHeaderStyle BackColor="#242121" />
                                                                </asp:GridView>
                                                                &nbsp; </td>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


