<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LogOnUserControl.ascx.cs"
    Inherits="LogOnUserControl" %>
<script type="text/javascript">
    function ShowHidePassword(ID) {
        if (document.getElementById($("#" + ID).prev().attr('id')).type == "password") {
            $("#" + ID).attr("data-hint", "Hide");
            $("#" + ID).find("i").removeClass("icon-eye").addClass("icon-eye-slash");
            document.getElementById($("#" + ID).prev().attr('id')).type = "text";
        }
        else {
            $("#" + ID).attr("data-hint", "Show");
            $("#" + ID).find("i").removeClass("icon-eye-slash").addClass("icon-eye");
            document.getElementById($("#" + ID).prev().attr('id')).type = "password";
        }
    }
</script>
<style type="text/css">
    .textAreaBoxInputs {
        padding: 7px 10px;
        outline: medium none;
        line-height: 30px;
        float: left;
    }

    #LogOnUserControl1_txtUname {
        background-color: lemonchiffon !important;
    }

    .dvShowHidePassword {
        margin-left: -300px;
        padding: 7px 10px;
        cursor: pointer;
        line-height: 40px;
        ser-select: none;
        -webkit-user-select: none; /* webkit (safari, chrome) */
        -moz-user-select: none; /* mozilla */
        -khtml-user-select: none; /* webkit (konqueror) */
        -ms-user-select: none; /* IE10+ */
    }

    @font-face {
        font-family: 'FontAwesome';
        src: url('fonts/fontawesome-webfont.eot?v=4.1.0');
        src: url('fonts/fontawesome-webfont1fc6.eot?#iefix&v=4.1.0') format('embedded-opentype'), url('fonts/fontawesome-webfont1fc6.woff?v=4.1.0') format('woff'), url('fonts/fontawesome-webfont1fc6.ttf?v=4.1.0') format('truetype'), url('fonts/fontawesome-webfont1fc6.svg?v=4.1.0#fontawesomeregular') format('svg');
        font-weight: normal;
        font-style: normal;
    }

    .icon {
        font-size: 12pt;
        display: inline-block;
        font-family: FontAwesome;
        font-style: normal;
        font-weight: normal;
        line-height: 1;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
    }

    /* makes the font 33% larger relative to the icon container */
    .icon-lg {
        font-size: 1.33333333em;
        line-height: 0.75em;
        vertical-align: -15%;
    }

    .icon-eye:before {
        content: "\f06e";
    }

    .icon-eye-slash:before {
        content: "\f070";
    }
</style>
<script src="https://kit.fontawesome.com/8f0f960111.js"></script>
<table width="100%" align="left">
    <tr>
        <td align="left">
            <asp:UpdatePanel ID="upLogin" runat="server">
                <ContentTemplate>
                    <table align="right">
                        <tr>
                            <td>
                                <asp:Label ID="lblError" runat="server" Font-Size="8pt" ForeColor="Red"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblWelcome" runat="server" Style="color: lightyellow !important"></asp:Label>
                                <asp:UpdateProgress ID="udpLogin" runat="server" AssociatedUpdatePanelID="upLogin">
                                    <ProgressTemplate>
                                        <table style="width: 100%">
                                            <tr>
                                                <td>
                                                    <img src="Images/progressbar.gif" />
                                                </td>
                                                <td><strong><span style="font-size: 8pt; font-family: Arial; color: white;">verifying credentials ....</span></strong> </td>
                                            </tr>
                                        </table>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                            <td>&nbsp;<asp:Button ID="btnLogOff" runat="server" OnClick="btnLogOff_Click" Text="Log Off"
                                Visible="False" ValidationGroup="none" Font-Names="Arial" CssClass="btn btn-danger" />
                                &nbsp;
                            </td>
                            <td>
                                <asp:Panel ID="pnlSignOn" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkRememberMe" runat="server" Font-Names="Arial" Font-Size="8pt" Text="Remember Me" ForeColor="White" />
                                            </td>
                                            <td>&nbsp;</td>

                                            <td>
                                                <asp:TextBox ID="txtUname" runat="server" Width="200px" Font-Size="12pt" BackColor="LemonChiffon" CssClass="form-control" placeholder="Username"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPwd" runat="server" CssClass="form-control textAreaBoxInputs" TextMode="Password" placeholder="Password" ForeColor="Black" BackColor="LemonChiffon"></asp:TextBox>
                                                <span id="ShowHidePassword" class="dvShowHidePassword hint--top hint--bounce hint--rounded" data-hint="Show" onclick="ShowHidePassword(this.id);" style="margin-left: -40px"><i class="icon icon-eye"></i></span>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" Text="Log In" Font-Names="Arial" ValidationGroup="none" CssClass="btn btn-danger" />
                                            </td>


                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="lbnForgotPassword" runat="server" CssClass="NoUnderline" Font-Bold="True" Font-Names="arial" Font-Size="9pt" ForeColor="White" OnClick="lbnForgotPassword_Click">Email me Log In</asp:LinkButton>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                    </td>
                </ContentTemplate>
            </asp:UpdatePanel>
    </tr>
</table>
<div class="clear">
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderForgotPwd" runat="server" DynamicServicePath=""
        Enabled="True" TargetControlID="Button0" PopupControlID="pnlForgotPwd" X="850" Y="60">
    </ajaxToolkit:ModalPopupExtender>
    <asp:Button ID="Button0" runat="server" Style="display: none" Text="Button" />
    <asp:Panel runat="server" ID="pnlForgotPwd" Style="display: none; padding: 10px" Width="420px">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <table align="center" style="background-color: Whitesmoke; padding: 10px; margin: 10px; border: solid 1px black" class="JustRoundedEdgeBothSmall">
                    <tr>
                        <td>
                            <table border="0" cellspacing="0" cellpadding="5" width="100%" style="background-color: Whitesmoke; padding: 10px; margin: 10px; width: 350px" class="JustRoundedEdgeBothSmall">
                                <tr>
                                    <td align="left">
                                        <table width="100%" style="background-color: whitesmoke" class="JustRoundedEdgeBothSmall">
                                            <tr>
                                                <td>
                                                    <strong style="font-size: 12pt; font-family: arial, Helvetica, sans-serif; color: black;">Forgot Password?</strong> </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <table style="background-color: whitesmoke">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="Label29" runat="server" Text="Enter Email:" Font-Size="8pt" Width="60px" Font-Names="arial" ForeColor="Black"></asp:Label>
                                                            </td>

                                                            <td align="left">
                                                                <asp:TextBox ID="txtResetEmail" runat="server" Width="250px" Font-Size="10pt" BorderStyle="Inset" ForeColor="Black" Height="25px" CssClass="form-control input-xs"></asp:TextBox>
                                                                <ajaxToolkit:TextBoxWatermarkExtender ID="txtResetEmail_TextBoxWatermarkExtender" runat="server" Enabled="True" TargetControlID="txtResetEmail" WatermarkText="EMAIL HERE"></ajaxToolkit:TextBoxWatermarkExtender>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                                            <ProgressTemplate>
                                                <table style="width: 100%; background-color: GhostWhite">
                                                    <tr>
                                                        <td style="width: 21px">
                                                            <img src="Images/uploading.gif" style="border: Thin solid Black" />
                                                        </td>
                                                        <td align="left"><strong><span style="font-size: 8pt; font-family: Arial; color: black;">verifying....</span></strong> </td>
                                                    </tr>
                                                </table>
                                            </ProgressTemplate>
                                        </asp:UpdateProgress>
                                        <asp:Label ID="lblResetPwdError" runat="server" Font-Size="8pt" ForeColor="Red" Font-Names="arial"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <table style="background-color: GhostWhite">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton ID="lbnResetPwd" runat="server" CssClass="btn btn-info btn-sm" Font-Bold="True" OnClick="lbnResetPwd_Click" 
                                                        ToolTip="Click to have your password sent to your original registered email.">Send&nbsp;<i class="fa fa-paper-plane"></i></asp:LinkButton>
                                                </td>
                                                <td style="width: 5px">&nbsp;</td>
                                                <td>
                                                    <asp:Button ID="lbnCancel0" runat="server" Text="Cancel" OnClick="lbnCancel0_Click" CssClass="btn btn-info btn-sm" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</div>
