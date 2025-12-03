<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ManagersProductionInput.aspx.cs" Inherits="ManagersProductionInput" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

 

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Content/bootstrap.css" rel="stylesheet" />
 
    <script src="https://kit.fontawesome.com/8f0f960111.js"></script>
    <style type="text/css">
        .input-large[class="input-large"] {
            width: 400px !important;
        }

        .CenterAligner {
            text-align: center;
        }

        .line_divider {
            margin-top: 5px;
            margin-bottom: 0px;
            display: block;
            background-color: #51545E;
            height: 4px;
        }

        /*Modal Popup*/
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=20);
            opacity: 0.2;
        }

        .modalPopup {
            background-color: transparent;
            border-width: 3px;
            border-style: solid;
            border-color: transparent;
            padding: 3px;
            width: 250px;
        }

        .popup_background {
            background-color: #4E6662;
            filter: alpha(opacity=70);
            opacity: 0.2;
        }
    </style>
    <script type="text/javascript">
        window.onbeforeunload = function (e) {
            HandleOnClose();
        };
        window.onunload = function () {
            __doPostBack('lbnSaveAndClose', '');
        }
    </script>
    <script type="text/javascript">
        var keyPressed = ""; //Variable to store which key was pressed
        var dataChanged = false;
        var currentElement = "";
        //Function called when the browser is closed.
        function HandleOnClose() {

            var id = getQueryStringValue("id");
            FunctiontoCallSaveData(id);

        }
        function getQueryStringValue(key) {
            return decodeURIComponent(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + encodeURIComponent(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
        }

        function FunctiontoCallSaveData(id) {
            PageMethods.ClosePopup(id);
        }
    </script>
    <script type="text/javascript" src="Scripts/jquery-1.10.2.min.js"></script>
 
    <style type="text/css">
 
        .ItemStyle {
            padding-left: 6px;
            padding-right: 6px;
            padding-top: 1px;
            padding-bottom: 2px;
        }
 

        .left-menu {
            width: 12%;
            float: left;
            margin-left: 3%;
        }

 

        .col-md-12 {
            float: left;
        }

        .nm {
            font-family: arial,sans-serif;
            margin-left: 15px;
            float: left;
        }


        /* Modal Content */
        .modal-content {
            width: 635px;
            height: 700px !important;
            margin-right: 30px;
            background-color: #ffffff;
        }
 
        
            
        .modal-header {
            padding: 2px 16px;
            background-color: #404040;
            color: white;
            height: 40px;
            text-align: center !important;
        }

        .modal-body {
            padding: 2px 16px;
        }

        .modal-footer {
            padding: 2px 16px;
            background-color: #f5f5f5;
            color: white;
        }

        /* Add Animation */
        @-webkit-keyframes slideIn {
            from {
                bottom: -300px;
                opacity: 0
            }

            to {
                bottom: 0;
                opacity: 1
            }
        }

        @keyframes slideIn {
            from {
                bottom: -300px;
                opacity: 0
            }

            to {
                bottom: 0;
                opacity: 1
            }
        }

        @-webkit-keyframes fadeIn {
            from {
                opacity: 0
            }

            to {
                opacity: 1
            }
        }

        @keyframes fadeIn {
            from {
                opacity: 0
            }

            to {
                opacity: 1
            }
        }

        /* no border srounding send mail elements*/

        .form-control {
            border: none;
        } 
        .col {
            width: 100%;
            height: 40px;
            text-align: left;
            border-bottom: 1px solid #dddcdc;
        } 
 
 
    </style>
    <script type="text/javascript">
        $(document).ready(function () {
           

        });

        function isPostBack() {//Use during postbacks...
            

        }; //End Postback... 

    </script>
    <style type="text/css">
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

        tbody tr.selected td {
            background: none repeat scroll 0 0 #FFCF8B;
            color: #f00;
        }
        /*Modal Popup*/
        .modalBackground {
            background-color: black;
            filter: alpha(opacity=20);
            opacity: 0.2;
        }

        .modalPopup {
            background-color: transparent;
            border-width: 3px;
            border-style: solid;
            border-color: transparent;
            padding: 3px;
            width: 250px;
        }

        .popup_background {
            background-color: #4E6662;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
       
    </style>
    
</head>
<body onunload="HandleOnClose();return false;">
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" AsyncPostBackTimeout="10800" EnablePartialRendering="true" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:HiddenField ID="hdtoSaveData" runat="server" EnableViewState="true" />
        <!--Don't Not use Update panel -->
       <%-- <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
            <ContentTemplate>
                <table align="Center" width="900px" class="BoxShadow4" style="background-color: #F0F0F0">
                    <tr>
                        <td align="left">

                            <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" AssociatedUpdatePanelID="UpdatePanelPromo" DisplayAfter="1">
                                <ProgressTemplate>
                                    <table>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <img src="Images/loader.gif" alt="" />
                                                </td>
                                                <td align="left"><span style="color: #ffffff">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing <span class="">....</span> </strong></span></span></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </ProgressTemplate>
                            </asp:UpdateProgress>--%>

                            <table border="0" cellspacing="0" cellpadding="5" width="100%" style="border: 1px solid #C0C0C0; background-color: Whitesmoke;" class="JustRoundedEdgeBothSmall">
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
           <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
    </form>
</body>
</html>
