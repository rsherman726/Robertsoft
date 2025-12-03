<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="PickupReport.aspx.cs" Inherits="PickupReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <style type="text/css">
        .NoUnderline {
            text-decoration: none;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:UpdatePanel ID="UpdatePanelPromo" runat="server">
        <ContentTemplate>
            <table align="Center" width="900px" class="BoxShadow4" style="background-color: #F0F0F0">
                <tr>
                    <td align="left">

                        <div style="position: fixed; right: 50px; height: 40px; width: 150px; z-index: 10000;">
                            <asp:UpdateProgress ID="UpdateProgressPromo" runat="server" DisplayAfter="1" AssociatedUpdatePanelID="UpdatePanelPromo">
                                <ProgressTemplate>
                                    <table style="border: medium solid #000080; width: 100%; background-color: white;">
                                        <tr>
                                            <td align="right" style="width: 12px;">
                                                <img src="Images/indicator_big.gif" alt="" />
                                            </td>
                                            <td><span style="color: #ffffff"><span style="font-size: 8pt; color: #0000cc; font-family: Verdana"><strong>processing  <span class="">....</span> </strong></span></span></td>
                                        </tr>
                                    </table>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <table width="100%">
                            <tr>
                                <td class="hdr" align="center">
                                    <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Size="X-Large"
                                        Text="Pickup Report"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="pnlMain" runat="server">
                                        <table width="100%" style="border-right: #000000 0px solid; border-top: #000000 0px solid; border-left: #000000 0px solid; border-bottom: #000000 0px solid; background-color: ghostwhite"
                                            align="center">

                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblTimeUpdated" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="9pt" ForeColor="#FF0066"></asp:Label>
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
                                                    <table id="tblAdd" align="center">
                                                        <tr>
                                                            <td align="center">
                                                                &nbsp;</td>
                                                            <td align="char">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" valign="middle">

                                                                <!-- *** Begin Header Table *** -->
                                                                <div id="DivHeader" style="vertical-align: top; width: 1160px; text-align: left;">
                                                                    <asp:Table ID="HeaderTable" runat="server"
                                                                        CellPadding="2"
                                                                        CellSpacing="0"
                                                                        Font-Size="11pt"
                                                                        ForeColor="White"
                                                                        BackColor="#333333"
                                                                        Font-Bold="False"
                                                                        Width="1160">
                                                                    </asp:Table>
                                                                </div>
                                                                <!-- *** End Header Table *** -->
                                                                <asp:Panel ID="pnlGridView" runat="server" Height="500px" ScrollBars="Vertical">
                                                                    <div id="DivData" class="Container" style="vertical-align: top; height: 500px; width: 1150px;">
                                                                        <asp:GridView ID="gvRecord" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="Black" Width="1150px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" OnPageIndexChanging="gvRecord_PageIndexChanging" OnRowDataBound="gvRecord_RowDataBound" OnRowCommand="gvRecord_RowCommand" OnSorting="gvRecord_Sorting" AllowSorting="True" PageSize="50" ShowFooter="True" Font-Size="7pt" ShowHeader="False">
                                                                            <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                            <Columns>                                                                                
                                                                                <asp:TemplateField HeaderText="Internal<br/>PO" SortExpression="InternalPoNumber">
                                                                                    <ItemTemplate>                                                                                            
                                                                                         <asp:Label ID="lblInternalPoNumber" runat="server" Text='<%# Bind("InternalPoNumber") %>' Font-Bold="false" Font-Size="8" Width="55px"></asp:Label>                                                                             
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Customer" SortExpression="Customer">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Font-Bold="false" Font-Size="8" Width="135px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="CustID" SortExpression="CustomerID">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("CustomerID") %>' Font-Bold="false" Font-Size="8" Width="45px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>                                                                                                                                                        
                                                                                                                                                                                                                                   
                                                                              
                                                                                <asp:TemplateField HeaderText="Delivery ID" SortExpression="DeliveryID">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDeliveryID" runat="server" Text='<%# Bind("DeliveryID") %>' Width="45px"  Font-Size="8" CssClass="NoUnderline"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Type" SortExpression="DeliveryTypeID">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDeliveryTypeID" runat="server" Text='<%# Bind("DeliveryTypeID") %>'  Font-Size="8" Width="45px" CssClass="NoUnderline"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Date<br/>Scheduled" SortExpression="DateScheduled">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDateScheduled" runat="server" Text='<%# Bind("DateScheduled") %>'  Font-Size="8" CssClass="NoUnderline" Width="60px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Date<br/>Picked up" SortExpression="DateDelivered">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDateDelivered" runat="server" Text='<%# Bind("DateDelivered") %>' Font-Size="8" CssClass="NoUnderline" Width="60px"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="Pickup<br/>Status" SortExpression="DeliveryStatus">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblDeliveryStatus" runat="server" Text='<%# Bind("DeliveryStatus") %>' Width="62px"  Font-Size="8" CssClass="NoUnderline"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="COD" SortExpression="COD">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCOD" runat="server" Text='<%# Bind("IsCOD") %>'   Font-Size="8" CssClass="NoUnderline"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Check<br/>Number" SortExpression="CheckNumber">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblCheckNumber" runat="server" Text='<%# Bind("CheckNumber") %>' Width="45px"  Font-Size="8" CssClass="NoUnderline"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Amount" SortExpression="Amount">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>'  Width="45px" Font-Size="8" CssClass="NoUnderline"></asp:Label>
                                                                                    </ItemTemplate>                                                                                     
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Truck" SortExpression="Truck">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblTruck" runat="server" Text='<%# Bind("Truck") %>' Width="40px"  Font-Size="8" CssClass="NoUnderline"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                
                                                                                <asp:TemplateField HeaderText="Tracking<br/>Number" SortExpression="TrackingNumber">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblTrackingNumber" runat="server" Text='<%# Bind("TrackingNumber") %>' Width="40px"  Font-Size="8" CssClass="NoUnderline"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField HeaderText="Comments" SortExpression="Comments">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblComments" runat="server" Text='<%# Bind("Comments") %>' Font-Size="8" Width="62px" CssClass="NoUnderline"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                </asp:TemplateField>

                                                                                <asp:TemplateField>
                                                                                    <ItemTemplate>
                                                                                        <asp:Image ID="imgFlaggedDR" runat="server" ImageUrl="~/Images/Flag.gif" />
                                                                                        <asp:Label ID="lblDRFlag" runat="server" Text='<%# Bind("DeliveryReceiptFlag") %>' Visible="false"></asp:Label>
                                                                                    </ItemTemplate>
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField ShowHeader="False">
                                                                                    <ItemTemplate>
                                                                                        <asp:LinkButton ID="lbnScanner" runat="server" CausesValidation="false"
                                                                                            CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                                                            CommandName="Scan" Text="To Scanner"></asp:LinkButton>
                                                                                    </ItemTemplate>
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
                                                                    </div>
                                                                </asp:Panel>
                                                                &nbsp; </td>
                                                            <td align="center" valign="middle">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="center">&nbsp; 
                                                                <asp:Label ID="lblPageNo" runat="server" Font-Bold="True" Font-Size="12pt" ForeColor="Navy" Visible="False"></asp:Label>
                                                            </td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>&nbsp; &nbsp; </td>
                                                            <td>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Timer ID="Timer1" runat="server" Interval="1000000" OnTick="Timer1_Tick" />
                                                            </td>
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
            <div>
                <asp:Button ID="Button1" runat="server" Style="display: none" Text="Button" />
                <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderPopUp" runat="server" BackgroundCssClass="popup_background"
                    CancelControlID="" DynamicServicePath="" Enabled="True" PopupControlID="pnlPopUp" TargetControlID="Button1" Y="100">
                </ajaxToolkit:ModalPopupExtender>
                <asp:Panel ID="pnlPopUp" runat="server" CssClass="modalPopup2" Visible="true" Style="display: none; width: 1200px; height: 700px; padding: 10px" ScrollBars="Vertical">
                    <table id="tblPopUp" style="border: 1px solid black; font-size: 7pt; height: 350px; background-color: #FFFFFF;" width="100%" align="center">
                        <tr>
                            <td align="center">
                                <asp:Label ID="Label232" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="Navy">Sale Order Details</asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:GridView ID="gvDetails" runat="server" CellPadding="4" ForeColor="#333333"
                                    GridLines="None"
                                    HorizontalAlign="Center"
                                    Font-Size="9pt"
                                    PageSize="15" AllowSorting="True"
                                    OnSorting="gvDetails_Sorting"
                                    Width="1200px"
                                    ToolTip=""
                                    AutoGenerateColumns="False"
                                    ShowFooter="True" OnRowDataBound="gvDetails_RowDataBound">
                                    <FooterStyle BackColor="Silver" />
                                    <HeaderStyle Font-Size="8pt" VerticalAlign="Top" BackColor="#CCCCCC" />
                                    <AlternatingRowStyle BackColor="LemonChiffon" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sales Order #" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesOrder" runat="server" Text='<%# Bind("SalesOrder") %>' ForeColor="#00569D" CssClass="NoUnderline"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Order Date" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrderDate" runat="server" Text='<%# Bind("OrderDate") %>' Font-Bold="True" Font-Size="9"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stock Code" SortExpression="StockCode">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStockCode" runat="server" Text='<%# Bind("StockCode") %>' Font-Bold="True" Font-Size="9"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" SortExpression="JobDescription">
                                            <ItemTemplate>
                                                <asp:Label ID="lblJobDescription" runat="server" Text='<%# Bind("JobDescription") %>' Font-Bold="True" Font-Size="8"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="left" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="B.O. Qty" SortExpression="BackOrderQty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBackOrderQty" runat="server" Text='<%# Bind("BackOrderQty") %>' Font-Bold="True" Font-Size="9"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="right" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ship Qty" SortExpression="ShipQty">
                                            <ItemTemplate>
                                                <asp:Label ID="lblShipQty" runat="server" Text='<%# Bind("ShipQty") %>' Font-Bold="True" Font-Size="9"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="right" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Price" SortExpression="Price" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrice" runat="server" Text='<%# Bind("Price") %>' Font-Bold="True" Font-Size="9"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                Order Total: 
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ext. Price" SortExpression="ExtendedPrice" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExtendedPrice" runat="server" Text='<%# Bind("ExtendedPrice") %>' Font-Bold="True" Font-Size="9"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Right" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblPriceTotal" runat="server" Font-Bold="True" Font-Size="12"></asp:Label>
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Sales Person" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSalesPerson" runat="server" Text='<%# Bind("SalesPerson") %>' Font-Size="8"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Customer" SortExpression="">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Font-Bold="True" Font-Size="9"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="CID" SortExpression="CustomerID">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("CustomerID") %>' Font-Bold="True" Font-Size="9" Width="50px"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="right" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Special Instructions">
                                            <ItemTemplate>
                                                <asp:Image ID="imgViewSpecialInstrs" runat="server" ImageUrl="Images/information.png" />
                                                <asp:Panel ID="pnlSpecialInstrs" runat="server">
                                                    <asp:Label ID="lblSpecialInstrs" runat="server" Text='<%# Bind("SpecialInstrs") %>' BackColor="GhostWhite" BorderColor="Black" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                                                    <ajaxToolkit:HoverMenuExtender ID="lblSpecialInstrs_HoverMenuExtender" runat="server" TargetControlID="imgViewSpecialInstrs" PopupControlID="pnlSpecialInstrs" PopupPosition="Center">
                                                    </ajaxToolkit:HoverMenuExtender>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Shipping Instructions">
                                            <ItemTemplate>
                                                <asp:Image ID="imgViewShippingInstrs" runat="server" ImageUrl="Images/information.png" />
                                                <asp:Panel ID="pnlShippingInstrs" runat="server">
                                                    <asp:Label ID="lblShippingInstrs" runat="server" Text='<%# Bind("ShippingInstrs") %>' BackColor="GhostWhite" BorderColor="Black" BorderStyle="Solid" BorderWidth="1"></asp:Label>
                                                    <ajaxToolkit:HoverMenuExtender ID="lblShippingInstrs_HoverMenuExtender" runat="server" TargetControlID="imgViewShippingInstrs" PopupControlID="pnlShippingInstrs" PopupPosition="Center">
                                                    </ajaxToolkit:HoverMenuExtender>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" BackColor="GhostWhite" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btnCancel1" runat="server" Text="Cancel" OnClick="btnCancel1_Click" CssClass="btn btn-primary" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:Label ID="lblErrorUpload" runat="server" Font-Size="8pt" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">

        //This Script is used to maintain Grid Scroll on Partial Postback
        var scrollTop;

        //Register Begin Request and End Request 
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        //Get The Div Scroll Position
        function BeginRequestHandler(sender, args) {
            var m = document.getElementById('DivData');
            scrollTop = m.scrollTop;
        }

        //Set The Div Scroll Position
        function EndRequestHandler(sender, args) {
            var m = document.getElementById('DivData');
            m.scrollTop = scrollTop;
        }
    </script>
</asp:Content>

