<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Domotica_ASP.admin" %>
<%@ Register TagPrefix="Wid" TagName="Widget" Src="Widget.ascx" %>
<%@ Register TagPrefix="Wid" TagName="Overlay" Src="overlay.ascx" %>
<%@ Register TagPrefix="Wid" TagName="Input" Src="InputFields.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title> Administration </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid_parent" id="grid_parent" runat="server">
		<!-- DEBUG: needs at least 3 items next to each other. 
			else the grid-gap adds an extra border.
			not sure why this occurs.
		-->
        <!-- NOTE: for any widget with a setting create an overlay! -->
        <Wid:Widget id="Register" name="Registratie" comment="Beheer gebruikers" setting="true" overlayID="Register_overlay" runat="server" toggle="False" /> 

    </div>
    <div class="grid_overlay" id="grid_overlay" onclick="close_overlay(event, false, this)">
        <Wid:Overlay ID="Register_overlay" runat="server">
            <Content>
                <Wid:Widget ID="Add" comment="Voeg een gebruiker toe." name="Toevoegen" runat="server" setting="True" overlayID="Add_User">

                  
                </Wid:Widget>

            </Content>

        </Wid:Overlay>
        <Wid:Overlay ID="Overlay1" runat="server">
            <Content>
               

            </Content>

        </Wid:Overlay>

    </div>
</asp:Content>
