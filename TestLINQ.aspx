<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="TestLINQ.aspx.cs" Inherits="TestLINQ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:GridView ID="GridView1" runat="server">
    </asp:GridView>
    <br />

    <asp:TextBox ID="txtUpload" runat="server" AutoPostBack="True" OnTextChanged="txtUpload_TextChanged"></asp:TextBox>
    <ajaxToolkit:AutoCompleteExtender ID="txtSearch_AutoCompleteExtender" runat="server"
        CompletionSetCount="25" CompletionListItemCssClass="autocomplete_listItem" CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
        DelimiterCharacters="" Enabled="True" MinimumPrefixLength="1" ServiceMethod="GetCompletionListUserName"
        ServicePath="" TargetControlID="txtUpload" UseContextKey="True" CompletionInterval="0">
    </ajaxToolkit:AutoCompleteExtender>





    
</asp:Content>

