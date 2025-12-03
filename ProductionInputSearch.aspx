<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ProductionInputSearch.aspx.cs" Inherits="ProductionInputSearch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
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

        .auto-style1 {
            height: 26px;
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
                                <td align="center">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                        Text="Admin Recorded Job Hours" ForeColor="Black"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlMain" runat="server">
                                        <table width="100%" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                            align="center">

                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Navy" Text="Admins: To load a Jobs, start typing the Job number in the text box below and after the autofill displays the job your looking for, select it and it will load the hours and employees who worked on the selected job."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <table>
                                                        <tr>
                                                            <td style="width: 350px">
                                                                <asp:TextBox ID="txtSearch" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" OnTextChanged="txtSearch_TextChanged" ToolTip="AutoComplete Textbox - Type job number, then select the one you want." ForeColor="Black" placeholder="Autofill Jobs..."></asp:TextBox>
                                                                <ajaxToolkit:AutoCompleteExtender ID="txtLocation_AutoCompleteExtender" runat="server" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem" CompletionListItemCssClass="autocomplete_listItem" CompletionSetCount="15" DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListJobs" ServicePath="" TargetControlID="txtSearch" UseContextKey="True" CompletionInterval="0">
                                                                </ajaxToolkit:AutoCompleteExtender>
                                                                 
                                                            </td>
                                                            <td>
                                                                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Font-Bold="True" OnClick="btnSearch_Click" Text="search" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">&nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td align="center" class="auto-style1">
                                                    <asp:Label ID="lblJob" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="14pt" ForeColor="Black"></asp:Label>
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
                                                                <asp:GridView ID="gvRecord" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="Black" GridLines="Horizontal" Width="1000px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" ShowFooter="True" OnRowDataBound="gvRecord_RowDataBound">
                                                                    <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Employee">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblEmployee" runat="server" Text='<%# Bind("Employee") %>' Font-Bold="True"></asp:Label>
                                                                                <asp:Label ID="lblEmployeeID" runat="server" Text='<%# Bind("EmployeeID") %>' Visible="false"></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Left" />
                                                                            <FooterTemplate></FooterTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Hours">
                                                                            <ItemTemplate>

                                                                                <asp:TextBox ID="txtHours" runat="server" Width="100px" BackColor="LemonChiffon" Text='<%# Bind("Hours") %>' CssClass="form-control" ForeColor="Black"></asp:TextBox>

                                                                            </ItemTemplate>
                                                                            <ItemStyle HorizontalAlign="Center" />
                                                                            <FooterTemplate>
                                                                                <table align="center" width="225px" style="border: solid 2px black;">
                                                                                    <tr>
                                                                                        <td align="right">
                                                                                            <asp:Label ID="lblTotalHoursOfficial" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right">
                                                                                            <asp:Label ID="lblTotalHours" runat="server" Font-Bold="True" Font-Size="10pt" ForeColor="Black"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </FooterTemplate>
                                                                            <FooterStyle HorizontalAlign="left" />
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
                                                                &nbsp; </td>
                                                            <td align="center" valign="middle">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">
                                                                <asp:Button ID="btnSubmit" runat="server" Font-Bold="True" OnClick="btnSubmit_Click" Text="Submit" CssClass="btn btn-primary" />
                                                                <ajaxToolkit:ConfirmButtonExtender ID="btnSubmit_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure the hours are correct?" Enabled="True" TargetControlID="btnSubmit"></ajaxToolkit:ConfirmButtonExtender>
                                                                &nbsp; </td>
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

