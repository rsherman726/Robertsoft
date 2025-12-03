<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="ProductionInput.aspx.cs" Inherits="ProductionInput" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="Scripts/jquery-1.7.2.js"></script>
    <script type="text/javascript" src="Scripts/jquery-1.10.2.min.js"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <script type="text/javascript" src="https://kit.fontawesome.com/8f0f960111.js" crossorigin="anonymous"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="Content/MaterialDesign.css" rel="stylesheet" />
    <link href="Content/Site.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/kissy-min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.js"></script>
    <script type="text/javascript" src="Scripts/JqueryValidator.js"></script>
    <script type="text/javascript" src="https://kit.fontawesome.com/8f0f960111.js"></script>


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
    </style>
    <script type="text/javascript">
        window.onunload = function () {
            __doPostBack('btnSubmit', '');
        }
        function isPostBack() {//Use during postbacks...


        }; //End Postback... 
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" AsyncPostBackTimeout="10800" EnablePartialRendering="true" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:HiddenField ID="hdtoSaveData" runat="server" EnableViewState="true" />

        <table align="Center" width="900px" class="BoxShadow4" style="background-color: #F0F0F0">
            <tr>
                <td align="left"></td>
            </tr>
            <tr>
                <td align="center">
                    <table width="100%">
                        <tr>
                            <td class="hdr" align="center">
                                <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                    Text="Record Job Hours" ForeColor="Black"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlMain" runat="server">
                                    <table width="100%" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                        align="center">

                                        <tr>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="lblJob" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="14pt" ForeColor="Navy"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <table id="tblAdd" align="center">
                                                    <tr>
                                                        <td align="char">&nbsp;</td>
                                                        <td align="char">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="char">&nbsp;
                                                        </td>
                                                        <td align="char">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" valign="middle">
                                                            <asp:GridView ID="gvRecord" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="Black" GridLines="Horizontal" Width="500px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" ShowFooter="True" OnRowDataBound="gvRecord_RowDataBound">
                                                                <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                <Columns>
                                                                    <asp:TemplateField HeaderText="Employee">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblEmployee" runat="server" Text='<%# Bind("Employee") %>' Font-Bold="True" ForeColor="Black"></asp:Label>
                                                                            <asp:Label ID="lblEmployeeID" runat="server" Text='<%# Bind("EmployeeID") %>' Visible="false"></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderText="Hours">
                                                                        <ItemTemplate>
                                                                            <table>
                                                                                <tr>
                                                                                    <td align="right">
                                                                                        <asp:TextBox ID="txtHours" runat="server" Width="50px" BackColor="LemonChiffon" Text='<%# Bind("Hours") %>' ForeColor="Black"></asp:TextBox></td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
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
                                                            &nbsp; </td>
                                                        <td align="center" valign="middle">&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:Button ID="btnSubmit" runat="server" Font-Bold="True" OnClick="btnSubmit_Click" Text="Submit" CssClass="btn btn-success" />
                                                            <ajaxToolkit:ConfirmButtonExtender ID="btnSubmit_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Are you sure the hours are correct?" Enabled="True" TargetControlID="btnSubmit"></ajaxToolkit:ConfirmButtonExtender>
                                                        </td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">&nbsp;</td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">&nbsp; </td>
                                                        <td>&nbsp;</td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">&nbsp;</td>
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

    </form>
</body>
</html>

