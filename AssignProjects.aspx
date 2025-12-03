<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="AssignProjects.aspx.cs" Inherits="AssignProjects" %>

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
                                        Text="Assign Jobs" ForeColor="Black"></asp:Label></td>
                            </tr>
                            <tr><td> 
                                <asp:Label ID="Label10" runat="server" Font-Bold="True" ForeColor="Navy" Text="Supervisors: To Assign a Job to a Manager, first load available jobs either by jobs completed within 30 days or check the 'Show UnAssigned Jobs within 180 Days' then select a warehouse. Next, select the manager from the Managers dropdownlist, then select a job from the Jobs to Assign list."></asp:Label>
                                </td></tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label2235" runat="server" Font-Bold="True" ForeColor="Navy" Text=" To view jobs already assigned, input the date the manager was assigned  the job in the Date of Assigment textbox and select the manager  assigned from the Managers dropdown list."></asp:Label>
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
                                                        <td>
                                                            <asp:CheckBox ID="chkShowUnAssigned" runat="server" AutoPostBack="True" Font-Bold="True" OnCheckedChanged="chkShowUnAssigned_CheckedChanged" Text="Show UnAssigned Jobs within 180 Days" ForeColor="Black" />
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td align="center">
                                                            <asp:Label ID="Label2234" runat="server" Font-Bold="True" Font-Names="arial" Text="Date of Assignment:" ForeColor="Black"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td style="color: black">**Unchecked show jobs completed within 30 days</td>
                                                        <td>&nbsp;</td>
                                                        <td align="center">
                                                            <asp:TextBox ID="txtAssignedDate" runat="server" AutoPostBack="True" BackColor="LemonChiffon" MaxLength="100" OnTextChanged="txtAssignedDate_TextChanged" TabIndex="11" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                            <ajaxToolkit:CalendarExtender ID="AssignedDate_CalendarExtender" runat="server" Animated="False" Enabled="True" Format="MM/dd/yyyy" PopupButtonID="imgAssignedDate" TargetControlID="txtAssignedDate">
                                                            </ajaxToolkit:CalendarExtender>
                                                            <asp:ImageButton ID="imgAssignedDate" runat="server" CausesValidation="False" ImageAlign="AbsMiddle" ImageUrl="~/Images/Calendar_scheduleHS.png" />
                                                            <ajaxToolkit:MaskedEditExtender ID="txtDateMEEAssignedDate" runat="server" InputDirection="LeftToRight" Mask="99/99/9999" MaskType="Date" MessageValidatorTip="true" TargetControlID="txtAssignedDate">
                                                            </ajaxToolkit:MaskedEditExtender>
                                                            <ajaxToolkit:MaskedEditValidator ID="txtDateMEVAssignedDate" runat="server" ControlExtender="txtDateMEEAssignedDate" ControlToValidate="txtAssignedDate" Display="None" EmptyValueMessage="Please enter a Pickup date." ErrorMessage="Invalid date format." InvalidValueMessage="Invalid date format." IsValidEmpty="true" MinimumValue="01/01/1901" MinimumValueMessage="Date must be greater than 01/01/1901" TooltipMessage="Please enter a date." />
                                                            <ajaxToolkit:ValidatorCalloutExtender ID="txtDateMEVEAssignedDate" runat="server" HighlightCssClass="validatorCalloutHighlight" TargetControlID="txtDateMEVAssignedDate" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp; </td>
                                                        <td>&nbsp; 
                                                            <asp:Label ID="Label2236" runat="server" Font-Bold="True" Font-Names="arial" Text="WAREHOUSE" ForeColor="Black"></asp:Label>
                                                        </td>
                                                        <td>&nbsp; </td>
                                                        <td align="center">&nbsp; 
                                                            <asp:Label ID="Label2232" runat="server" Font-Bold="True" Font-Names="arial" Text="MANAGERS" ForeColor="Black"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlWareHouses" runat="server" AutoPostBack="True" BackColor="LemonChiffon" OnSelectedIndexChanged="ddlWareHouses_SelectedIndexChanged" CssClass="form-control" ForeColor="Black">
                                                                <asp:ListItem Value="0">All</asp:ListItem>
                                                                <asp:ListItem Value="1">Warehouse 1</asp:ListItem>
                                                                <asp:ListItem Value="10">Warehouse 10</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td valign="middle">&nbsp;
                                                            </td>
                                                        <td align="center" valign="middle">&nbsp;
                                                            <asp:DropDownList ID="ddlManagers" runat="server" AutoPostBack="True" BackColor="LemonChiffon" OnSelectedIndexChanged="ddlManagers_SelectedIndexChanged" CssClass="form-control" ForeColor="Black">
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
                                                        <td>
                                                            <asp:Label ID="Label2233" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Navy" Text="Jobs to Assign"></asp:Label>
                                                        </td>
                                                        <td>&nbsp; </td>
                                                        <td>
                                                            <asp:Label ID="Label2228" runat="server" Font-Bold="True" Font-Names="arial" ForeColor="Navy" Text="Jobs Assigned to above manager on above assigned date"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td align="center">
                                                            <asp:ListBox ID="lbAvailableJobs" runat="server" BackColor="LemonChiffon" CssClass="form-control" Font-Size="7pt" Height="200px" SelectionMode="Multiple" Font-Bold="False" ForeColor="Black" Width="460px"></asp:ListBox>                                                           
                                                        </td>
                                                        <td align="center"><font face="arial">
                                                            <table id="Table4" border="0" cellpadding="1" cellspacing="1">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:Button ID="btnAddOne" runat="server" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnAddOne_Click" Text="Add &gt;" ToolTip="Click to ADD" Width="100px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnAddOne_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to add this item?" Enabled="True" TargetControlID="btnAddOne">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center"><font face="arial">
                                                                        <asp:Button ID="btnAddAll" runat="server" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnAddAll_Click" Text="Add All &gt;" ToolTip="Click to ADD" Width="100px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnAddAll_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to add all selected?" Enabled="True" TargetControlID="btnAddAll">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                        </font></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center"><font face="arial">
                                                                        <asp:Button ID="btnRemoveOne" runat="server" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnRemoveOne_Click" Text="&lt; Remove" ToolTip="Click to REMOVE" Width="100px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnRemoveOne_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to delete this item?" Enabled="True" TargetControlID="btnRemoveOne">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                        </font></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center"><font face="arial">
                                                                        <asp:Button ID="btnRemoveAll" runat="server" CssClass="btn btn-primary" Enabled="False" Font-Bold="True" Font-Size="8pt" OnClick="btnRemoveAll_Click" Text="&lt; Remove All" ToolTip="Click to REMOVE All" Width="100px" />
                                                                        <ajaxToolkit:ConfirmButtonExtender ID="btnRemoveAll_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure you want to delete all selected items?" Enabled="True" TargetControlID="btnRemoveAll">
                                                                        </ajaxToolkit:ConfirmButtonExtender>
                                                                        </font></td>
                                                                </tr>
                                                            </table>
                                                            </font></td>
                                                        <td align="center"><font face="arial">
                                                            <asp:ListBox ID="lbAssignedJobs" runat="server" BackColor="LemonChiffon" Font-Size="7pt" Height="200px" SelectionMode="Multiple" Font-Bold="True" CssClass="form-control" ForeColor="Black" Width="460px"></asp:ListBox>
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

