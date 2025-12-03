<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeFile="Emailer.aspx.cs" Inherits="Emailer" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="HeadContent">
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        function CountItems(lb) {
            var count = 0;
            var lbl = document.getElementById('lbl_selected');
            for (var i = 0; i < lb.options.length; i++) {
                if (lb.options[i].selected) {
                    count++;
                }
            }
            document.getElementById('<% =lblSelectedCount.ClientID %>').innerHTML = 'Select emails: ' + count;

        }

    </script>
    <asp:UpdatePanel ID="UpdatePanelContact" runat="server">
        <ContentTemplate>
            <div style="position: fixed; right: 50px; height: 40px; width: 150px; z-index: 10000;">
                <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" AssociatedUpdatePanelID="UpdatePanelContact" DisplayAfter="1">

                    <ProgressTemplate>
                        <table>
                            <tbody>
                                <tr>
                                    <td align="Left">
                                        <img src="Images/loader.gif" alt="" style="border: thin solid #000000" />
                                    </td>
                                    <td align="left"><span style="color: #ffffff">&nbsp;<span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing <span class="">....</span> </strong></span></span></td>
                                </tr>
                            </tbody>
                        </table>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <table width="1000px" style="border-right: #000000 thin solid; border-top: #000000 thin solid; border-left: #000000 thin solid; border-bottom: #000000 thin solid; background-color: ghostwhite" align="center">
                <tr>
                    <td class="hdr" align="center">
                        <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large" Text="Emailer" ForeColor="Black"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="Table3" border="0" cellpadding="1" cellspacing="1" width="1000px" align="center">
                            <tr>
                                <td align="center">
                                    <table>
                                        <tr>

                                            <td align="left">
                                                <asp:RadioButtonList ID="rblCustomerSource" runat="server" ForeColor="Black" Width="300px" AutoPostBack="True" OnSelectedIndexChanged="rblCustomerSource_SelectedIndexChanged">
                                                    <asp:ListItem>&nbsp;All Customers</asp:ListItem>
                                                    <asp:ListItem Selected="True">&nbsp;New Price Table Customers</asp:ListItem>
                                                    <asp:ListItem>&nbsp;Vendors</asp:ListItem>
                                                    <asp:ListItem>&nbsp;Misc</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td align="center">
                                                <asp:Label ID="Label13" runat="server" Font-Size="9pt" ForeColor="Black" Text="Starting Letter of Customer's Name:"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:DropDownList ID="ddlStartingLetter" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" Font-Bold="True" ForeColor="Black" OnSelectedIndexChanged="ddlStartingLetter_SelectedIndexChanged" Width="100px">
                                                    <asp:ListItem Selected="True">ALL</asp:ListItem>
                                                    <asp:ListItem>A</asp:ListItem>
                                                    <asp:ListItem>B</asp:ListItem>
                                                    <asp:ListItem>C</asp:ListItem>
                                                    <asp:ListItem>D</asp:ListItem>
                                                    <asp:ListItem>E</asp:ListItem>
                                                    <asp:ListItem>F</asp:ListItem>
                                                    <asp:ListItem>G</asp:ListItem>
                                                    <asp:ListItem>H</asp:ListItem>
                                                    <asp:ListItem>I</asp:ListItem>
                                                    <asp:ListItem>J</asp:ListItem>
                                                    <asp:ListItem>K</asp:ListItem>
                                                    <asp:ListItem>L</asp:ListItem>
                                                    <asp:ListItem>M</asp:ListItem>
                                                    <asp:ListItem>N</asp:ListItem>
                                                    <asp:ListItem>O</asp:ListItem>
                                                    <asp:ListItem>P</asp:ListItem>
                                                    <asp:ListItem>Q</asp:ListItem>
                                                    <asp:ListItem>R</asp:ListItem>
                                                    <asp:ListItem>S</asp:ListItem>
                                                    <asp:ListItem>T</asp:ListItem>
                                                    <asp:ListItem>U</asp:ListItem>
                                                    <asp:ListItem>V</asp:ListItem>
                                                    <asp:ListItem>W</asp:ListItem>
                                                    <asp:ListItem>X</asp:ListItem>
                                                    <asp:ListItem>Y</asp:ListItem>
                                                    <asp:ListItem>Z</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td align="center">
                                                <asp:Button ID="btnLoadEmails" runat="server" CssClass="btn btn-info" OnClick="btnLoadEmails_Click" Text="Load Emails" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="center">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblEmailAddressCount" runat="server" Font-Bold="True" Font-Names="Arial"
                                        Font-Size="10pt" Style="color: #000000" Width="500px"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center" class="style6">
                                    <strong style="font-family: arial, Helvetica, sans-serif; color: navy; font-weight: bold; font-size: 12pt">Seachable Listbox...When populated, select any name within the listbox and start typing the Customer name of your choice and it will appear above the listbox.</strong></td>
                            </tr>
                            <tr>
                                <td align="center">&nbsp;</td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <asp:Panel ID="pnlEmail" runat="server" Height="300" ScrollBars="Vertical" BackColor="#FFF8E7" BorderColor="#DCDCDC" BorderStyle="Solid" BorderWidth="2">
                                        <asp:CheckBoxList ID="cblEmailAddresses" runat="server" SelectionMode="Multiple" Font-Bold="True" Font-Size="10pt" ForeColor="Black" OnSelectedIndexChanged="cblEmailAddresses_SelectedIndexChanged"></asp:CheckBoxList>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <strong style="color: black">(Use CTRL for Multiple Selection) </strong>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkSelectAll_CheckedChanged"
                                        Text="&nbsp;Select All" ForeColor="Black" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <table>
                                        <tr>
                                            <td align="left" valign="top">
                                                <asp:Label ID="Label1" runat="server" CssClass="style5" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" ForeColor="Black" Width="300px">Attachment:(Max. Size: 2048kb)(2MB)</asp:Label>
                                            </td>
                                            <td align="center">
                                                <asp:FileUpload ID="fuAttachment" runat="server" BackColor="LemonChiffon" CssClass="form-control" Width="350px" ForeColor="Black" />
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblEmailFormatError" runat="server" ForeColor="Red" Font-Bold="True" Font-Names="Arial" Font-Size="16pt"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblSelectedCount" runat="server" Font-Bold="True" Font-Names="Arial"
                                        Font-Size="10pt" Style="color: #0000FF" Width="150px"></asp:Label>
                                </td>
                            </tr>

                            <tr>
                                <td align="center"></td>
                            </tr>

                            <tr>
                                <td align="center"></td>
                            </tr>

                            <tr>
                                <td align="center">
                                    <table width="700px">
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="Label18" runat="server" CssClass="style5" Font-Bold="True" Font-Names="Arial" Font-Size="12pt" ForeColor="Black">CC:</asp:Label></td>
                                            <td>
                                                <asp:TextBox ID="txtTestEmailReciepientCC" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black" placeholder="Enter a CC email or leave blank to ignore" Width="400px">qa@felbro.com</asp:TextBox></td>
                                            <td></td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:CheckBox ID="chkTesting" runat="server" ForeColor="Red" Text="&amp;nbsp;Test Email:" Width="125px" /></td>
                                            <td>
                                                <asp:TextBox ID="txtTestEmailReciepient" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black" placeholder="Enter Your Email" Width="400px"></asp:TextBox></td>
                                            <td>&nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:CheckBox ID="chkSendItemSpecsToVendors" runat="server" ForeColor="Blue" Visible="False" />
                                            </td>
                                            <td align="left" style="width: 400px">
                                                <asp:Label ID="LabelSpecsToVender" runat="server" Font-Bold="True" ForeColor="Black" Text="&amp;nbsp;Include Excel Vendor Item Specs Attachment" Visible="False" Width="400px"></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                        </tr>
                                    </table>

                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                            </tr>

                            <tr>
                                <td align="center" id="tblNewPrice" runat="server" style="background-color: lightblue">
                                    <table width="100%">
                                        <tr>
                                            <td align="center"></td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkSendNewPrices" runat="server" ForeColor="Blue" Text="&amp;nbsp;Include New Price Grid on bottom of email" />&nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Label ID="Label15" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="Black" Text="Email Destination"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:RadioButtonList ID="rblEmailDestination" runat="server" ForeColor="Navy" RepeatDirection="Horizontal" Width="650px">
                                                    <asp:ListItem Selected="True">&nbsp;Send to Sales Rep Below</asp:ListItem>
                                                    <asp:ListItem>&nbsp;Send to New Price Customer List selected  Above</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td align="center">
                                                <asp:TextBox ID="txtSalesRepEmail" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black" Width="300px" placeholder="Enter Sales Rep Email"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label17" runat="server" Font-Bold="True" ForeColor="Black" Text="Subject"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:TextBox ID="txtSubject" runat="server" BackColor="LemonChiffon" CssClass="form-control input-sm" ForeColor="Black" placeholder="Enter Email Subject or leave blank for default" Width="300px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="Label14" runat="server" Font-Bold="True" ForeColor="Black" Text="Select a Letter to Email"></asp:Label>
                                    <asp:LinkButton ID="btnReloadPriceGrid" runat="server" CssClass="btn-primary active" OnClick="btnReloadPriceGrid_Click" Text="Reload Price Grid" Width="125px"><i class="fa fa-refresh"></i>&nbsp;Reload Grid</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:DropDownList ID="ddlLetters" runat="server" AutoPostBack="True" BackColor="LemonChiffon" CssClass="form-control" ForeColor="Black" OnSelectedIndexChanged="ddlLetters_SelectedIndexChanged" Width="350px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <CKEditor:CKEditorControl ID="RSEditor" runat="server" BasePath="~/ckeditor" Height="500px" Width="1000px" AutoParagraph="false">		                               
                                    </CKEditor:CKEditorControl>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Panel ID="pnlPreview" runat="server" Height="400px" ScrollBars="Vertical">
                                        <asp:GridView ID="gvPreview" runat="server" AllowSorting="True" BackColor="White"
                                            BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3"
                                            GridLines="Vertical" OnRowDataBound="gvPreview_RowDataBound" OnSorting="gvPreview_Sorting" PageSize="1000" AutoGenerateColumns="False" Width="950px" Font-Size="10pt">
                                            <AlternatingRowStyle BackColor="#DCDCDC" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Customer">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Name">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblName" runat="server" Text='<%# Bind("Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Price Code">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPriceCode" runat="server" Text='<%# Bind("PriceCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="StockCode">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Current Price">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCurrentPrice" runat="server" Text='<%# Bind("CurrentPrice") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="New Price">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblNewPrice" runat="server" Text='<%# Bind("NewPrice") %>'></asp:Label>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("Status") %>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Sale Date">
                                                    <HeaderStyle CssClass="CenterAligner" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLastSaleDate" runat="server" Text='<%# Bind("LastSaleDate") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
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
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:LinkButton ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" CssClass="btn btn-info" TabIndex="13" Width="125px" ForeColor="White" ToolTip="Click to send emails"><i class="fa fa-envelope-o" ></i>&nbsp;Send Emails</asp:LinkButton>
                                    <ajaxToolkit:ConfirmButtonExtender ID="btnSubmit_ConfirmButtonExtender" runat="server" ConfirmOnFormSubmit="True" ConfirmText="Proceed to send out emails?" Enabled="True" TargetControlID="btnSubmit"></ajaxToolkit:ConfirmButtonExtender>
                                    <asp:LinkButton ID="btnReset" runat="server" OnClick="btnReset_Click" CssClass="btn btn-danger" TabIndex="14" Width="125px" ForeColor="White" ToolTip="Click to reset form"><i class="fa fa-refresh" ></i>&nbsp;Reset</asp:LinkButton>

                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lblMessage" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="16pt" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left">&nbsp;</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="right" style="height: 20px">
                        <asp:Button ID="btnToEmailEditor" runat="server" BackColor="Transparent" BorderStyle="None"
                            Font-Bold="True" Font-Underline="True" OnClick="btnToEmailEditor_Click" Text="To Email Editor" ForeColor="Black" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                </tr>
                </caption>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSubmit" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

