<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Domotica_ASP.Login1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Login page</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Label ID="username" runat="server" AssociatedControlID="UsernameInput" Text="Username"></asp:Label>
    <asp:TextBox ID="UsernameInput" runat="server" ViewStateMode="Disabled" EnableViewState="False"></asp:TextBox>
    <br />
    <asp:Label ID="password" runat="server" AssociatedControlID="PasswordInput" Text="Password"></asp:Label>
    <asp:TextBox ID="PasswordInput" runat="server" EnableViewState="False" ViewStateMode="Disabled"></asp:TextBox>
    <br />
    <asp:CheckBox ID="remember" runat="server" Text="Remember me" />
    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Login" />
    <asp:Label ID="ResponseLabel" runat="server"></asp:Label>
    <asp:Label ID="Label2" runat="server"></asp:Label>
    <br />
</asp:Content>
