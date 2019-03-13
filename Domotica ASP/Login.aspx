<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Domotica_ASP.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label ID="username" runat="server" AssociatedControlID="UsernameInput" Text="Username"></asp:Label>
        <asp:TextBox ID="UsernameInput" runat="server"></asp:TextBox>
        <div>
            <asp:Label ID="password" runat="server" AssociatedControlID="PasswordInput" Text="Password"></asp:Label>
            <asp:TextBox ID="PasswordInput" runat="server"></asp:TextBox>
        </div>
        <asp:CheckBox ID="CheckBox1" runat="server" Text="Remember me" />
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Login" />
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    </form>
</body>
</html>
