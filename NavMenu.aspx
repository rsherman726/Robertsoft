<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="NavMenu.aspx.cs"
    Inherits="NavMenu" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
        <ContentTemplate>
            <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="UpdatePanelPromo">
                <ProgressTemplate>
                    <table style="width: 100%">
                        <tbody>
                            <tr>
                                <td style="width: 12px" align="right">
                                    <img src="Images/loader.gif" alt="" />
                                </td>
                                <td>
                                    <span style="color: #ffffff">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing
                                        <span class="">....</span> </strong></span></span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div>
                <table align="Center" width="900" class="BoxShadow4" style="background-color: #F0F0F0">
                    <tr>
                        <td class="hdr" align="center">
                             <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Menu Admin" ForeColor="Black"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <table width="100%">
                                <tr>
                                    <td valign="top">
                                        &nbsp;
                                    </td>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <table id="Table3" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td align="left" style="width: 250px">
                                                                <asp:Label ID="lblCurrentDepts" runat="server" Font-Bold="True" Font-Names="Arial"
                                                                    Font-Size="8pt" ForeColor="Black">Current Menu Items:</asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 425px">
                                                                <asp:DropDownList ID="ddlCurrent" runat="server" AutoPostBack="True" Enabled="False" OnSelectedIndexChanged="ddlCurrent_SelectedIndexChanged" BackColor="LemonChiffon"
                                                                    Font-Size="10pt" CssClass="form-control" Font-Bold="True" ForeColor="Black">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td align="left">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 250px;">
                                                            </td>
                                                            <td align="center" style="width: 425px;">
                                                                <asp:RadioButtonList ID="rblMode" runat="server" AutoPostBack="True" Font-Bold="False"
                                                                    Font-Names="verdana" Font-Size="8pt" RepeatDirection="Horizontal" Width="252px"
                                                                    OnSelectedIndexChanged="rblMode_SelectedIndexChanged" ForeColor="Black">
                                                                    <asp:ListItem Value="1">&nbsp;New</asp:ListItem>
                                                                    <asp:ListItem Selected="True" Value="2">&nbsp;Existing</asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                            <td align="left">
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 250px">
                                                                <asp:Label ID="lblMenuID" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                                                                    Width="165px" ForeColor="Black">Menu ID:</asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 425px">
                                                                <asp:TextBox ID="txtMenuID" runat="server" MaxLength="50" 
                                                                    BackColor="LemonChiffon" ReadOnly="True" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtMenuID"
                                                                    ErrorMessage="Menu ID is required." ValidationGroup="NONE">*</asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 250px">
                                                                <asp:Label ID="Label12" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                                                                    Width="165px" ForeColor="Black">Text(Link Display):</asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 425px">
                                                                <asp:TextBox ID="txtText" runat="server" MaxLength="50" 
                                                                    BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtText"
                                                                    ErrorMessage="Text is required." ValidationGroup="NONE">*</asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 250px">
                                                                <asp:Label ID="Label13" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                                                                    Width="165px" ForeColor="Black">Description(Mouse Over):</asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 425px">
                                                                <asp:TextBox ID="txtDescription" runat="server" BackColor="LemonChiffon" MaxLength="255" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                            </td>
                                                            <td align="left">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtDescription"
                                                                    ErrorMessage="Description is required." ValidationGroup="NONE">*</asp:RequiredFieldValidator>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 250px">
                                                                <asp:Label ID="Label35" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                                                                    Width="165px" ForeColor="Black">Order:</asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 425px;height:80px">
                                                               <asp:TextBox ID="txtRankingNumUpDown" runat="server"  Width="75px" BackColor="LemonChiffon" Font-Names="Arial" Font-Size="9pt" ForeColor="Black">1.0</asp:TextBox>
                                                <ajaxToolkit:NumericUpDownExtender ID="NumericUpDownExtender1" runat="server" TargetControlID="txtRankingNumUpDown"
                                                    Width="150" RefValues="" ServiceDownMethod="" ServiceUpMethod="" TargetButtonDownID=""
                                                    TargetButtonUpID="" Minimum="1" Maximum="100" Step="1" />
                                                            </td>
                                                            <td align="left">
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 250px">
                                                                <asp:Label ID="Label14" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt" Width="165px" ForeColor="Black">NavigateUrl:</asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 425px">
                                                                <asp:TextBox ID="txtNavigateUrl" runat="server" BackColor="LemonChiffon" MaxLength="100" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                            </td>
                                                            <td align="left">&nbsp; </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px">
                                                                &nbsp;</td>
                                                            <td style="width: 425px">
                                                                <asp:Label ID="Label15" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="7pt"
                                                                    ForeColor="Blue">*Leave NavigateUrl blank to make header link or sub menu header link. **For Excel Files use Images\Excel\(Name of file)</asp:Label>
                                                            </td>
                                                            <td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 250px">
                                                                <asp:Label ID="Label20" runat="server" Font-Bold="True" Font-Names="Arial" 
                                                                    Font-Size="8pt" ForeColor="Black">ImageUrl:</asp:Label>  &nbsp;
                                                                <asp:Image ID="imgIcon" runat="server" />
                                                            </td>
                                                            <td style="width: 425px">
                                                                <asp:DropDownList ID="ddlImageUrl" runat="server" BackColor="LemonChiffon" AutoPostBack="True" 
                                                                    onselectedindexchanged="ddlImageUrl_SelectedIndexChanged" CssClass="form-control" ForeColor="Black">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px">
                                                                &nbsp;</td>
                                                            <td style="width: 425px">
                                                                <asp:Label ID="Label21" runat="server" Font-Bold="False" Font-Names="Arial" 
                                                                    Font-Size="7pt" ForeColor="Blue">*Leave ImageUrl blank to make header link or sub menu header link. </asp:Label>
                                                            </td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 250px">
                                                                <asp:Label ID="Label16" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                                                                    Width="165px" ForeColor="Black">Target (Where to Open Link):</asp:Label>
                                                            </td>
                                                            <td style="width: 425px">
                                                                <asp:TextBox ID="txtTarget" runat="server" BackColor="LemonChiffon" MaxLength="100" CssClass="form-control" ForeColor="Black"></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px">
                                                                &nbsp;
                                                            </td>
                                                            <td style="width: 425px">
                                                                <asp:Label ID="Label17" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="7pt"
                                                                    ForeColor="Blue">*Leave Target text box blank to open page within current browser and use _Blank to open page in it's own browser window.</asp:Label>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 250px">
                                                                <asp:Label ID="Label18" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt"
                                                                    Width="165px" ForeColor="Black">Parent ID:</asp:Label>
                                                            </td>
                                                            <td style="width: 425px">
                                                                <asp:DropDownList ID="ddlParentID" runat="server" 
                                                                    BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px">
                                                                &nbsp;
                                                            </td>
                                                            <td style="width: 425px">
                                                                <asp:Label ID="Label19" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="7pt"
                                                                    ForeColor="Blue">Parent ID is the ID of the header link, the link that the menu item will appear under. If the header links themselves don't have a parent ID, they are then Top level links. Select NONE to create a new Top level menu item.</asp:Label>
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 250px">
                                                                <asp:Label ID="Label36" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="8pt" Width="165px" Visible="False" ForeColor="Black">Show on menu:</asp:Label>
                                                            </td>
                                                            <td align="center" style="width: 425px">
                                                                <asp:CheckBox ID="chkShowOnMenu" runat="server" Checked="True" Visible="False" />
                                                            </td>
                                                            <td>
                                                                &nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="width: 250px">&nbsp;</td>
                                                            <td align="center" style="width: 425px">
                                                                <asp:Label ID="Label37" runat="server" Font-Bold="False" Font-Names="Arial" Font-Size="7pt" ForeColor="Blue" Visible="False">*This is checked by default. If you need to add a page that does not actually need to be shown on the menu, then uncheck.</asp:Label>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px">&nbsp; </td>
                                                            <td style="width: 425px">&nbsp; <font face="arial">
                                                                <asp:TextBox ID="txtOldName" runat="server" Visible="False"></asp:TextBox>
                                                                </font></td>
                                                            <td>&nbsp; </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px">
                                                                &nbsp;
                                                            </td>
                                                            <td align="center" style="width: 425px">
                                                                <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-primary" OnClick="btnUpdate_Click"
                                                                    Text="Update" Width="100px" ValidationGroup="NONE" />
                                                                <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-primary" OnClick="btnDelete_Click"
                                                                    Text="Delete" Width="100px" />
                                                                <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" OnClick="btnAdd_Click"
                                                                    Text="Add" Width="100px" ValidationGroup="NONE" />
                                                                <asp:Button ID="btnReset" runat="server" CssClass="btn btn-primary" OnClick="btnReset_Click"
                                                                    Text="Reset" Width="100px" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Names="Arial" 
                                                        Font-Size="9pt" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="top" >
                                                    <asp:Label ID="Label34" runat="server" Font-Bold="False" Font-Names="Arial" 
                                                        Font-Size="8pt" ForeColor="Blue" Width="470px">Note: Updates to the security take place instantly, independent of updating other menu item data!</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" valign="top">
                                                    <font face="arial">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="center">
                                                                <font face="arial">
                                                                <asp:Label ID="labelAssignedProducts" runat="server" Font-Bold="True" 
                                                                    Font-Names="verdana" Font-Size="9pt" Width="170px" ForeColor="Black">Excluded Roles</asp:Label>
                                                                </font>
                                                            </td>
                                                            <td>
                                                                &nbsp;</td>
                                                            <td align="center" class="auto-style1" style="width: 300px">
                                                                <font face="arial">
                                                                <asp:Label ID="labelAvailableProducts" runat="server" Font-Bold="True" 
                                                                    Font-Names="verdana" Font-Size="9pt" Width="150px" ForeColor="Black">Included Roles</asp:Label>
                                                                </font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="right">
                                                                <font face="arial">
                                                                <asp:ListBox ID="lbExcluded" runat="server" BackColor="LemonChiffon" 
                                                                    Font-Size="8pt" Height="200px" SelectionMode="Multiple" CssClass="form-control" ForeColor="Black">
                                                                </asp:ListBox>
                                                                </font>
                                                            </td>
                                                            <td align="center">
                                                                <table ID="Table4" border="0" cellpadding="1" cellspacing="1">
                                                                    <tr>
                                                                        <td align="center">
                                                                            <asp:Button ID="btnAddOne" runat="server" 
                                                                                CssClass="btn btn-primary" Font-Bold="True" Font-Size="9pt" 
                                                                                OnClick="btnAddOne_Click" Text="Add &gt;" ToolTip="Click to ADD" Width="125px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <font face="arial">
                                                                            <asp:Button ID="btnAddAll" runat="server"  
                                                                                CssClass="btn btn-primary" Font-Bold="True" Font-Size="9pt" 
                                                                                OnClick="btnAddAll_Click" Text="Add All &gt;" ToolTip="Click to ADD" 
                                                                                Width="125px" />
                                                                            </font>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <font face="arial">
                                                                            <asp:Button ID="btnRemoveOne" runat="server" 
                                                                                CssClass="btn btn-primary" Font-Bold="True" Font-Size="9pt" 
                                                                                OnClick="btnRemoveOne_Click" Text="&lt; Remove" ToolTip="Click to REMOVE" 
                                                                                Width="125px" />
                                                                            </font>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <font face="arial">
                                                                            <asp:Button ID="btnRemoveAll" runat="server" 
                                                                                CssClass="btn btn-primary" Font-Bold="True" Font-Size="9pt" 
                                                                                OnClick="btnRemoveAll_Click" Text="&lt; Remove All" 
                                                                                ToolTip="Click to REMOVE All" Width="125px" />
                                                                            </font>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td style="width: 350px" >
                                                                <font face="arial">
                                                                <asp:ListBox ID="lbIncluded" runat="server" BackColor="LemonChiffon" 
                                                                    Font-Size="8pt" Height="200px" SelectionMode="Multiple" CssClass="form-control" ForeColor="Black">
                                                                </asp:ListBox>
                                                                </font>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center" style="color: black" >
                                                                (Use CTRL for Multiple Selection)</td>
                                                            <td >
                                                            </td>
                                                            <td style="width: 350px; color: black;" align="center">
                                                                (Use CTRL for Multiple Selection)</td>
                                                        </tr>
                                                    </table>
                                                    </font>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" HeaderText="The following items need attention:"
                                Font-Names="Arial" Font-Size="8pt" ValidationGroup="NONE" ForeColor="Red" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td align="center">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
<asp:Content ID="Content2" runat="server" contentplaceholderid="HeadContent">

</asp:Content>

