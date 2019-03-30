<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Domotica_ASP.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Login page</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:LoginView ID="LoginView1" runat="server">
        <AnonymousTemplate>
            <asp:Login ID="Login1" runat="server">
            </asp:Login>
        </AnonymousTemplate>
        <LoggedInTemplate>
            <asp:LoginStatus ID="LoginStatus1" runat="server" />
        </LoggedInTemplate>
        <RoleGroups>
            <asp:RoleGroup Roles="admin">
                <ContentTemplate>
                    Welkom admin!
                </ContentTemplate>
            </asp:RoleGroup>
        </RoleGroups>
    </asp:LoginView>
    
</asp:Content>
