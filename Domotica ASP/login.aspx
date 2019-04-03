<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Domotica_ASP.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Login page</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="lbl2" runat="server"></asp:Label>
    <asp:LoginView ID="LoginView1" runat="server">
        <AnonymousTemplate>
            <asp:Login ID="Login1" runat="server" CssClass="LoginView" EnableTheming="False" DestinationPageUrl="~/default.aspx">
            </asp:Login>
        </AnonymousTemplate>
        <LoggedInTemplate>
            <asp:label ID="lbl" Text="U bent al ingelogt, ga verder naar de standaard pagina:" runat="server"></asp:label>
            <asp:Button ID="btn" Text="Terug" OnClick="btn_Click" runat="server" />
        </LoggedInTemplate>
    </asp:LoginView>
    
</asp:Content>
