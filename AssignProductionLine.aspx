<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AssignProductionLine.aspx.cs" Inherits="AssignProductionLine" %>

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
            width: 11px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
        <ContentTemplate>
            <table align="Center" width="1100px" class="BoxShadow4" style="background-color: #F0F0F0">
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
                                        Text="Assign Production Lines" ForeColor="Black"></asp:Label></td>
                            </tr>
                            <tr><td>
                                <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Navy" Text="Managers: Too assign a Job to a specific production line, select the production line from the Production Lines Dropdown list. You may filter Jobs by Stock code with the stock code Dropdown list. The available Jobs will appear. Next select the jobs you want to assign to that production line and click &quot;Add All&quot;. If you selected just one Job you may click &quot;Add&quot; . To remove a job before you have record employees hours. Click the &quot;Remove&quot; or &quot;Remove All&quot; button."></asp:Label>
                                </td></tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2235" runat="server" Font-Bold="True" ForeColor="Navy" Text=" To view jobs already assigned, input the date they were assigned and select the projection line they were assigned to."></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td >
                                    <asp:Panel ID="pnlMain" runat="server" >
                                        <table width="100%" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                            align="center">
                                            
                                            <tr>
                                                <td>&nbsp;<table id="tblAdd" align="center">
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td align="center">
                                                            <asp:Label ID="Label2234" runat="server" Font-Bold="True" Font-Names="arial" Text="DATE OF ASSIGNMENT" ForeColor="Black"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtAssignedDate" runat="server" AutoPostBack="True" BackColor="LemonChiffon" Font-Size="8pt" MaxLength="100" OnTextChanged="txtAssignedDate_TextChanged" TabIndex="11" Width="184px" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="AssignedDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgAssignedDate" TargetControlID="txtAssignedDate">
                                                            </ajaxToolkit:CalendarExtender>
                                                            <asp:ImageButton ID="imgAssignedDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                            <ajaxToolkit:MaskedEditExtender ID="txtDateMEEAssignedDate" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtAssignedDate">
                                                            </ajaxToolkit:MaskedEditExtender>
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td align="center">&nbsp; 
                                                            <asp:Label ID="Label2240" runat="server" Font-Bold="True" Font-Names="arial" Text="STOCK CODES" ForeColor="Black"></asp:Label>
                                                        </td>
                                                        <td>&nbsp; </td>
                                                        <td align="center">&nbsp; 
                                                            <asp:Label ID="Label2232" runat="server" Font-Bold="True" Font-Names="arial" Text="PRODUCTION LINES" ForeColor="Black"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td align="center">
                                                            <asp:DropDownList ID="ddlStockCodes" runat="server" AutoPostBack="True" BackColor="LemonChiffon" Font-Size="8pt" OnSelectedIndexChanged="ddlStockCodes_SelectedIndexChanged" CssClass="form-control" ForeColor="Black">
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td valign="middle">&nbsp;
                                                            </td>
                                                        <td align="center" valign="middle"> 
                                                            <asp:DropDownList ID="ddlProductionLines" runat="server" AutoPostBack="True" BackColor="LemonChiffon" Font-Size="8pt" OnSelectedIndexChanged="ddlProductionLines_SelectedIndexChanged" CssClass="form-control" ForeColor="Black">
                                                            </asp:DropDownList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td>&nbsp;</td>
                                                        <td>&nbsp; </td>
                                                        <td>
                                                            <ajaxToolkit:MaskedEditValidator ID="txtDateMEVAssignedDate" runat="server" ControlExtender="txtDateMEEAssignedDate" ControlToValidate="txtAssignedDate" Display="None" EmptyValueMessage="Please enter a Pickup date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." ForeColor="Black" />
                                                            <ajaxToolkit:ValidatorCalloutExtender ID="txtDateMEVEAssignedDate" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtDateMEVAssignedDate" />
                                                            &nbsp; </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td align="center">
                                                            <asp:Label ID="Label2233" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Navy" Text="JOBS TO ASSIGN"></asp:Label>
                                                        </td>
                                                        <td>&nbsp; </td>
                                                        <td align="center">
                                                            <asp:Label ID="Label2228" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Navy" Text="Jobs Assigned to above production lines on above assigned date"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            <asp:ListBox ID="lbAvailableJobs" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="7pt" Height="200px" SelectionMode="Multiple" Width="450px" Font-Bold="True" ForeColor="Black"></asp:ListBox>                                                           
                                                        </td>
                                                        <td><font face="arial">
                                                            <table id="Table4" border="0" cellpadding="1" cellspacing="1">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Button ID="btnAddOne" runat="server" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnAddOne_Click" Text="Add &gt;" ToolTip="Click to ADD" Width="125px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnAddOne_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to add this item?" Enabled="True" TargetControlID="btnAddOne">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center"><font face="arial">
                                                                        <asp:Button ID="btnAddAll" runat="server" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnAddAll_Click" Text="Add All &gt;" ToolTip="Click to ADD" Width="125px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnAddAll_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to add all selected?" Enabled="True" TargetControlID="btnAddAll">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                        </font></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center"><font face="arial">
                                                                        <asp:Button ID="btnRemoveOne" runat="server" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnRemoveOne_Click" Text="&lt; Remove" ToolTip="Click to REMOVE" Width="125px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnRemoveOne_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to delete this item?" Enabled="True" TargetControlID="btnRemoveOne">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                        </font></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center"><font face="arial">
                                                                        <asp:Button ID="btnRemoveAll" runat="server" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnRemoveAll_Click" Text="&lt; Remove All" ToolTip="Click to REMOVE All" Width="125px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnRemoveAll_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to delete all selected items?" Enabled="True" TargetControlID="btnRemoveAll">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                        </font></td>
                                                                </tr>
                                                            </table>
                                                            </font></td>
                                                        <td><font face="arial">
                                                            <asp:ListBox ID="lbAssignedJobs" runat="server" BackColor="LemonChiffon" Font-Size="8pt" Height="200px" SelectionMode="Multiple" Width="450px" Font-Bold="True" CssClass="form-control" ForeColor="Black"></asp:ListBox>
                                                            </font></td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td style="color: black">(Use CTRL for Multiple Selection) </td>
                                                        <td align="center">&nbsp; </td>
                                                        <td align="center" style="color: black">(Use CTRL for Multiple Selection) </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td align="center">
                                                            <asp:LinkButton ID="lbnSelectAllAssign" runat="server" OnClick="lbnSelectAllAssign_Click" Font-Bold="True">Select All</asp:LinkButton>
                                                            &nbsp;
                                                            <asp:LinkButton ID="lbnClearAssign" runat="server" OnClick="lbnClearAssign_Click" Font-Bold="True">Clear All</asp:LinkButton>
                                                        </td>
                                                        <td>&nbsp; </td>
                                                        <td align="center">&nbsp;
                                                            <asp:LinkButton ID="lbnSelectAllAssigned" runat="server" OnClick="lbnSelectAllAssigned_Click" Font-Bold="True">Select All</asp:LinkButton>
                                                            &nbsp;
                                                            <asp:LinkButton ID="lbnClearAssigned" runat="server" OnClick="lbnClearAssigned_Click" Font-Bold="True">Clear All</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td>&nbsp; </td>
                                                        <td align="right">&nbsp; </td>
                                                        <td>&nbsp; </td>
                                                    </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="hdr" align="center">
                                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="X-Large"
                                        Text="Assign Employees" ForeColor="Black"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label2239" runat="server" Font-Bold="True" ForeColor="Navy" Text="Managers: To assign an employee to a production line, select the production line from the Production Line dropdown list and the Available Employees list will populate with available employees you can assign to the desired production line."></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <table id="tblAdd0" align="center">
                                                        <tr>
                                                            <td>&nbsp; </td>
                                                            <td>&nbsp; </td>
                                                            <td>&nbsp; </td>
                                                            <td align="center">&nbsp;
                                                                <asp:Label ID="Label2236" runat="server" Font-Bold="True" Font-Names="arial" Text="Production Lines:" ForeColor="Black"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp;</td>
                                                            <td valign="middle">&nbsp; </td>
                                                            <td align="center" valign="middle"> 
                                                                <asp:DropDownList ID="ddlProductionLinesEmployees" runat="server" AutoPostBack="True" BackColor="LemonChiffon" Font-Size="8pt" OnSelectedIndexChanged="ddlProductionLinesEmployees_SelectedIndexChanged" CssClass="form-control" ForeColor="Black">
                                                                </asp:DropDownList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp; </td>
                                                            <td>&nbsp;</td>
                                                            <td>&nbsp; </td>
                                                            <td>&nbsp; </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp; </td>
                                                            <td align="center">
                                                                <asp:Label ID="Label2237" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Navy" Text="Available Employees"></asp:Label>
                                                            </td>
                                                            <td>&nbsp; </td>
                                                            <td align="center">
                                                                <asp:Label ID="Label2238" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Navy" Text="Employees assigned to above Production line"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp;</td>
                                                            <td>
                                                                <asp:ListBox ID="lbAvailableEmployees" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" Height="200px" SelectionMode="Multiple" Width="450px" ForeColor="Black"></asp:ListBox>
                                                            </td>
                                                            <td><font face="arial">
                                                                <table id="Table5" border="0" cellpadding="1" cellspacing="1">
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:Button ID="btnAddOneEmployees" runat="server" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnAddOneEmployees_Click" Text="Add &gt;" ToolTip="Click to ADD" Width="125px" />
                                                                            <ajaxToolkit:ConfirmButtonExtender ID="btnAddOneEmployees_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to add this employee?" Enabled="True" TargetControlID="btnAddOneEmployees">
                                                                            </ajaxToolkit:ConfirmButtonExtender>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center"><font face="arial">
                                                                            <asp:Button ID="btnAddAllEmployees" runat="server" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnAddAllEmployees_Click" Text="Add All &gt;" ToolTip="Click to ADD" Width="125px" />
                                                                            <ajaxToolkit:ConfirmButtonExtender ID="btnAddAllEmployees_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to add all selected?" Enabled="True" TargetControlID="btnAddAllEmployees">
                                                                            </ajaxToolkit:ConfirmButtonExtender>
                                                                            </font></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center"><font face="arial">
                                                                            <asp:Button ID="btnRemoveOneEmployees" runat="server" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnRemoveOneEmployees_Click" Text="&lt; Remove" ToolTip="Click to REMOVE" Width="125px" />
                                                                            <ajaxToolkit:ConfirmButtonExtender ID="btnRemoveOneEmployees_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to remove this employee?" Enabled="True" TargetControlID="btnRemoveOneEmployees">
                                                                            </ajaxToolkit:ConfirmButtonExtender>
                                                                            </font></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center"><font face="arial">
                                                                            <asp:Button ID="btnRemoveAllEmployees" runat="server" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnRemoveAllEmployees_Click" Text="&lt; Remove All" ToolTip="Click to REMOVE All" Width="125px" />
                                                                            <ajaxToolkit:ConfirmButtonExtender ID="btnRemoveAllEmployees_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to remove all selected employees?" Enabled="True" TargetControlID="btnRemoveAllEmployees">
                                                                            </ajaxToolkit:ConfirmButtonExtender>
                                                                            </font></td>
                                                                    </tr>
                                                                </table>
                                                                </font></td>
                                                            <td><font face="arial">
                                                                <asp:ListBox ID="lbAssignedEmployees" runat="server" BackColor="LemonChiffon" Font-Bold="True" Height="200px" SelectionMode="Multiple" Width="450px" CssClass="form-control" ForeColor="Black"></asp:ListBox>
                                                                </font></td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp; </td>
                                                            <td align="center" style="color: black">(Use CTRL for Multiple Selection) </td>
                                                            <td align="center">&nbsp; </td>
                                                            <td align="center" style="color: black">(Use CTRL for Multiple Selection) </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp; </td>
                                                            <td align="center">
                                                                <asp:LinkButton ID="lbnSelectAllAssignEmployees" runat="server" Font-Bold="True" OnClick="lbnSelectAllAssignEmployees_Click">Select All</asp:LinkButton>
                                                                &nbsp;
                                                                <asp:LinkButton ID="lbnClearAssignEmployees" runat="server" Font-Bold="True" OnClick="lbnClearAssignEmployees_Click">Clear All</asp:LinkButton>
                                                            </td>
                                                            <td>&nbsp; </td>
                                                            <td align="center">&nbsp;
                                                                <asp:LinkButton ID="lbnSelectAllAssignedEmployees" runat="server" Font-Bold="True" OnClick="lbnSelectAllAssignedEmployees_Click">Select All</asp:LinkButton>
                                                                &nbsp;
                                                                <asp:LinkButton ID="lbnClearAssignedEmployees" runat="server" Font-Bold="True" OnClick="lbnClearAssignedEmployees_Click">Clear All</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp; </td>
                                                            <td>&nbsp; </td>
                                                            <td align="right">&nbsp; </td>
                                                            <td>&nbsp; </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="lblErrorEmployees" runat="server" ForeColor="Red"></asp:Label>
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

