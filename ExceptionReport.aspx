<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ExceptionReport.aspx.cs" Inherits="ExceptionReport" %>

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
                                        Text="Exception Report"></asp:Label></td>
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
                                                    <table id="tblAdd" align="center">
                                                        <tr>
                                                            <td align="center">
                                                                <table id="tblPageCt" width="1050">
                                                                    <tr>
                                                                        <td align="left" width="100">
                                                                            <asp:Label ID="lblResultsPerPage" runat="server" Font-Names="Arial" Font-Size="10pt" Text="Results per page:" Width="125px" Visible="False"></asp:Label>
                                                                        </td>
                                                                        <td align="left">
                                                                            <asp:DropDownList ID="ddlDisplayCount" runat="server" AutoPostBack="True" BackColor="LemonChiffon" Font-Names="Arial" Font-Size="10pt" OnSelectedIndexChanged="ddlDisplayCount_SelectedIndexChanged" Visible="False">
                                                                                <asp:ListItem>10</asp:ListItem>
                                                                                <asp:ListItem>20</asp:ListItem>
                                                                                <asp:ListItem Selected="True">50</asp:ListItem>
                                                                                <asp:ListItem>100</asp:ListItem>
                                                                                <asp:ListItem>250</asp:ListItem>
                                                                                <asp:ListItem>500</asp:ListItem>
                                                                                <asp:ListItem>750</asp:ListItem>
                                                                                <asp:ListItem>1000</asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                        <td align="left" valign="middle">&nbsp;</td>
                                                                    </tr>
                                                                </table>
                                                            </td>
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
                                                                        width="1160" 
                                                                        >
                                                                         
                                                                    </asp:Table>
                                                                </div>
                                                                <!-- *** End Header Table *** -->
                                                                  <asp:Panel ID="pnlGridView" runat="server" Height="500px" ScrollBars="Vertical">
                                                                      <div id="DivData" class="Container" style="vertical-align: top; height: 500px; width: 1150px;">
                                                                          <asp:GridView ID="gvRecord" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="Black" Width="1150px" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" OnPageIndexChanging="gvRecord_PageIndexChanging" OnRowDataBound="gvRecord_RowDataBound" OnRowCommand="gvRecord_RowCommand" OnSorting="gvRecord_Sorting" AllowSorting="True" PageSize="50" ShowFooter="True" ShowHeader="False">
                                                                              <AlternatingRowStyle BackColor="WhiteSmoke" />
                                                                              <Columns>
                                                                                  <asp:TemplateField ShowHeader="False">
                                                                                      <ItemTemplate>
                                                                                          <asp:LinkButton ID="lbnSelect" runat="server"
                                                                                              CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                                                              CausesValidation="False" ToolTip="Click to view Details."
                                                                                              CommandName="Select" ForeColor="#00569D"  Font-Size="8" CssClass="NoUnderline"
                                                                                              Text="Select" Width="32"></asp:LinkButton>
                                                                                      </ItemTemplate>
                                                                                      <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                  </asp:TemplateField>
                                                                                  <asp:TemplateField HeaderText="SO #" SortExpression="SalesOrder">
                                                                                      <ItemTemplate>
                                                                                          <asp:Label ID="lblQueryStatus" runat="server"></asp:Label>
                                                                                          <asp:LinkButton ID="lbnSalesOrder" runat="server" Text='<%# Bind("SalesOrder") %>'
                                                                                              CommandName="GetSO" ToolTip="Click to view Docs associated with this Sales Order."
                                                                                              CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                                                              ForeColor="#00569D" CssClass="NoUnderline" Font-Size="7" Font-Bold="True" Width="40"></asp:LinkButton>
                                                                                      </ItemTemplate>
                                                                                      <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                  </asp:TemplateField>
                                                                                  <asp:TemplateField HeaderText="Customer" SortExpression="Customer">
                                                                                      <ItemTemplate>
                                                                                          <asp:Label ID="lblCustomer" runat="server" Text='<%# Bind("Customer") %>' Font-Bold="false" Font-Size="8" Width="215px"></asp:Label>
                                                                                      </ItemTemplate>
                                                                                      <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                  </asp:TemplateField>
                                                                                  <asp:TemplateField HeaderText="CustID" SortExpression="CustomerID">
                                                                                      <ItemTemplate>
                                                                                          <asp:Label ID="lblCustomerID" runat="server" Text='<%# Bind("CustomerID") %>' Font-Bold="false" Font-Size="8" Width="54px"></asp:Label>
                                                                                      </ItemTemplate>
                                                                                      <ItemStyle HorizontalAlign="right" VerticalAlign="Top" />
                                                                                  </asp:TemplateField>
                                                                                  <asp:TemplateField HeaderText="Order Date" SortExpression="OrderDate">
                                                                                      <ItemTemplate>
                                                                                          <asp:Label ID="lblOrderDate" runat="server" Text='<%# Bind("OrderDate") %>' Font-Bold="false" Font-Size="8" Width="70px"></asp:Label>
                                                                                      </ItemTemplate>
                                                                                      <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                  </asp:TemplateField>
                                                                                  <asp:TemplateField HeaderText="Total Value" SortExpression="TotalValue">
                                                                                      <ItemTemplate>
                                                                                          <asp:Label ID="lblTotalValue" runat="server" Text='<%# Bind("TotalValue") %>' Font-Bold="True" Font-Size="8" Width="67px"></asp:Label>
                                                                                      </ItemTemplate>
                                                                                      <ItemStyle HorizontalAlign="Right" VerticalAlign="Top" />
                                                                                      <FooterTemplate>
                                                                                          Grand Total:    
                                                                                          <asp:Label ID="lblGrandTotal" runat="server" Font-Bold="True" Font-Size="9"></asp:Label>
                                                                                      </FooterTemplate>
                                                                                      <FooterStyle HorizontalAlign="Right" />
                                                                                  </asp:TemplateField>
                                                                                  <asp:TemplateField HeaderText="Order Status" SortExpression="OrderStatus">
                                                                                      <ItemTemplate>
                                                                                          <asp:Label ID="lblOrderStatus" runat="server" Text='<%# Bind("OrderStatus") %>' Font-Bold="false" Font-Size="8" Width="62px"></asp:Label>
                                                                                      </ItemTemplate>
                                                                                      <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                  </asp:TemplateField>
                                                                                  <asp:TemplateField HeaderText="Requested Ship Date" SortExpression="reqShipDate">
                                                                                      <ItemTemplate>
                                                                                          <asp:Label ID="lblShipDate" runat="server" Text='<%# Bind("reqShipDate") %>' Font-Bold="false" Font-Size="8" Width="90px"></asp:Label>
                                                                                      </ItemTemplate>
                                                                                      <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                  </asp:TemplateField>
                                                                                  <asp:TemplateField HeaderText="Sales Person" SortExpression="SalesPerson">
                                                                                      <ItemTemplate>
                                                                                          <asp:Label ID="lblSalesPerson" runat="server" Text='<%# Bind("SalesPerson") %>' Font-Bold="false" Font-Size="8" Width="125px"></asp:Label>
                                                                                      </ItemTemplate>
                                                                                      <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                  </asp:TemplateField>
                                                                                  <asp:TemplateField HeaderText="Purchase Order" SortExpression="CustomerPoNumber">
                                                                                      <ItemTemplate>
                                                                                          <asp:LinkButton ID="lbnPurchaseOrder" runat="server" Text='<%# Bind("CustomerPoNumber") %>' ToolTip="Search for docs with this Purchase Order."
                                                                                              CommandName="GetPO"
                                                                                              CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                                                              ForeColor="#00569D" CssClass="NoUnderline" Font-Size="7" Width="140px"></asp:LinkButton>
                                                                                      </ItemTemplate>
                                                                                      <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                  </asp:TemplateField>
                                                                                  <asp:TemplateField>
                                                                                      <ItemTemplate>
                                                                                          <asp:Image ID="imgFlaggedPO" runat="server" ImageUrl="~/Images/Flag.gif" />
                                                                                           <asp:Label ID="lblPOFlag" runat="server" Text='<%# Bind("PurchaseOrderFlag") %>' visible="false"></asp:Label>
                                                                                      </ItemTemplate>
                                                                                  </asp:TemplateField>
                                                                                  <asp:TemplateField HeaderText="Del">
                                                                                      <ItemTemplate>
                                                                                          <asp:ImageButton ID="ibnDeliveryStatus" runat="server" CommandName="View"
                                                                                              CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'></asp:ImageButton>
                                                                                          <asp:Label ID="lblDeliveryStatus" runat="server" Text='<%# Bind("DeliveryStatus") %>' Visible="false"></asp:Label>
                                                                                      </ItemTemplate>
                                                                                      <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                                                  </asp:TemplateField>
                                                                                  <asp:TemplateField HeaderText="DelID" SortExpression="DeliveryID">
                                                                                      <ItemTemplate>
                                                                                          <asp:LinkButton ID="lbnDeliveryID" runat="server" Text='<%# Bind("DeliveryID") %>' ToolTip="Search for docs with this DeliveryID."
                                                                                              CommandName="GetDR"
                                                                                              CommandArgument='<%# ((GridViewRow) Container).RowIndex %>'
                                                                                              ForeColor="#00569D" CssClass="NoUnderline" Font-Size="8" Width="40px"></asp:LinkButton>
                                                                                      </ItemTemplate>
                                                                                      <ItemStyle HorizontalAlign="center" VerticalAlign="Top" />
                                                                                  </asp:TemplateField>
                                                                                  <asp:TemplateField>
                                                                                      <ItemTemplate>
                                                                                          <asp:Image ID="imgFlaggedDR" runat="server" ImageUrl="~/Images/Flag.gif" />
                                                                                          <asp:Label ID="lblDRFlag" runat="server" Text='<%# Bind("DeliveryReceiptFlag") %>' visible="false"></asp:Label>
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
                        <tr><td align="center">
                            <asp:Label ID="Label232" runat="server" Font-Bold="True" Font-Italic="True" Font-Size="14pt" ForeColor="Navy">Sale Order Details</asp:Label>
                            </td></tr>
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
                                        <asp:TemplateField HeaderText="Price" SortExpression="Price">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrice" runat="server" Text='<%# Bind("Price") %>' Font-Bold="True" Font-Size="9"></asp:Label>                                                
                                            </ItemTemplate>   
                                            <ItemStyle HorizontalAlign="Right" BackColor="GhostWhite" />
                                            <FooterTemplate>
                                                Order Total: 
                                            </FooterTemplate>
                                            <FooterStyle HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Ext. Price" SortExpression="ExtendedPrice">
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
                        <tr><td align="center">
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

