<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="CalendarAdmin.aspx.cs" Inherits="CalendarAdmin" MaintainScrollPositionOnPostback="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
     <style type="text/css">
        .CenterAligner {
            text-align: center;
        }

        .gridViewPager td {
            padding-left: 6px;
            padding-right: 6px;
            padding-top: 1px;
            padding-bottom: 2px;
        }

        .content {
            margin: 0 auto;
            width: 760px;
            background: #ffffff;
            border: 1px solid #dadada;
            border-radius: 6px;
            margin-bottom: 50px;
            padding: 45px;
            padding-top: 25px;
            position: relative;
        }

        #box {
            width: 300px;
            height: 100px;
            text-align: center;
            vertical-align: middle;
            border: 2px solid #ff6a00;
            background-color: #ffd800;
            padding: 15px;
            font-family: Arial;
            font-size: 16px;
            margin-top: 35px;
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
            <table align="Center" width="1000" class="JustRoundedEdgeBoth" style="background-color: GhostWhite">
                <tr>
                    <td align="center">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Calendar Notes Admin" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="pnlMain" runat="server">
                            <table align="center" style="background-color: GhostWhite"> 
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:GridView ID="gvCalendar" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" CellPadding="4"
                                            ForeColor="Black" GridLines="None" OnPageIndexChanging="gvCalendar_PageIndexChanging" OnRowCancelingEdit="gvCalendar_RowCancelingEdit"
                                            OnRowCommand="gvCalendar_RowCommand" OnRowDataBound="gvCalendar_RowDataBound" OnRowDeleting="gvCalendar_RowDeleting"
                                            OnRowEditing="gvCalendar_RowEditing" OnRowUpdating="gvCalendar_RowUpdating" ShowFooter="True" Width="700px" Font-Names="Arial" Font-Size="11pt" PageSize="50">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField ShowHeader="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHolidayCalendarID" runat="server" Font-Size="12pt" Text='<%# Bind("HolidayCalendarID") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Calendar Note">                                                    
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtHoliday" runat="server" Font-Size="12pt" Text='<%# Bind("Holiday") %>' Width="250px" CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblHoliday" runat="server" Text='<%# Bind("Holiday") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:TextBox ID="txtHolidayAdd" runat="server" Font-Size="12pt" Width="250px" CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black"></asp:TextBox>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Calendar Date">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <EditItemTemplate>  
                                                        <table>
                                                            <tr>
                                                                <td style="width: 250px">
                                                                    <asp:TextBox ID="txtHolidayDate" runat="server" Font-Size="12pt" Text='<%# Bind("HolidayDate") %>' Width="250px" CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black"></asp:TextBox>
                                                                    <ajaxToolkit:CalendarExtender ID="txtHolidayDateCE" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgHolidayDate" TargetControlID="txtHolidayDate">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                </td>
                                                                <td style="width: 20px">
                                                                    <asp:ImageButton ID="imgHolidayDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                    <ItemTemplate>
                                                      <asp:Label ID="lblHolidayDate" runat="server" Text='<%# Bind("HolidayDate") %>' ></asp:Label>                                                         
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <table>
                                                            <tr>
                                                                <td style="width: 250px">
                                                                    <asp:TextBox ID="txtHolidayDateAdd" runat="server" Font-Size="12pt"  Width="250px" CssClass="form-control" BackColor="LemonChiffon" ForeColor="Black"></asp:TextBox>
                                                                    <ajaxToolkit:CalendarExtender ID="txtHolidayDateAddCE" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgHolidayDateAdd" TargetControlID="txtHolidayDateAdd">
                                                                    </ajaxToolkit:CalendarExtender>
                                                                </td>
                                                                <td style="width: 20px">
                                                                    <asp:ImageButton ID="imgHolidayDateAdd" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </FooterTemplate>
                                                    <ItemStyle HorizontalAlign="center" />
                                                    <HeaderStyle HorizontalAlign="center" />
                                                    <FooterStyle HorizontalAlign="Left" />
                                                </asp:TemplateField>                                                                                               
                                                <asp:TemplateField ShowHeader="False">
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lbnUpdate" runat="server" CausesValidation="True" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" CommandName="Update" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Update"></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnUpdate_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with update?" Enabled="True" TargetControlID="lbnUpdate">
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
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnDelete_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with delete?" Enabled="True" TargetControlID="lbnDelete">
                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:LinkButton ID="lbnAdd" runat="server" CausesValidation="False" CommandName="Add" Font-Bold="True" Font-Size="11pt" ForeColor="White" Text="Add"></asp:LinkButton>
                                                        <ajaxToolkit:ConfirmButtonExtender ID="lbnAdd_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed with add?" Enabled="True" TargetControlID="lbnAdd">
                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EditRowStyle BackColor="#2461BF" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                             <PagerStyle BackColor="#2461BF" ForeColor="White" Font-Bold="true" Font-Names="arial" Font-Size="12pt" HorizontalAlign="center" CssClass="gridViewPager"/>
                                            <PagerSettings Position="TopAndBottom" Mode="NumericFirstLast" PageButtonCount="5" FirstPageText="First" LastPageText="Last" />       
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
