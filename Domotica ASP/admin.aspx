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
        <Wid:Widget id="GroupManage" name="Groepbeheer" comment="Beheer groepen" setting="true" overlayID="AddTo_Group" runat="server" toggle="False" /> 
    </div>
    <div class="grid_overlay" id="grid_overlay" onclick="close_overlay(event, false, this)">
        
        <Wid:Overlay ID="Register_overlay" runat="server">
            <Content>
                <Wid:Widget ID="Add" comment="Voeg een gebruiker toe." name="Toevoegen" runat="server" setting="True" overlayID="Add_User" />
                <Wid:Widget ID="Remove" comment="Verwijder een gebruiker." name="Verwijderen" runat="server" setting="True" overlayID="Remove_User" />
            </Content>
        </Wid:Overlay>
        
        <Wid:Overlay ID="Add_User" runat="server">
            <Content>
                <Wid:Widget ID="user_naam" comment="Naam van gebruiker" name="naam" runat="server">
                    <Input>
                        <Wid:Input ID="input_naam" in_type="text" stantext="naam" runat="server" />
                    </Input>
                </Wid:Widget>
                <Wid:Widget ID="email" comment="email van gebruiker" name="email" runat="server">
                    <Input>
                        <Wid:Input ID="input_email" in_type="text" stantext="email" runat="server" />
                    </Input>
                </Wid:Widget>
                <Wid:Widget ID="username" comment="gebruikersnaam van gebruiker" name="gebruikersnaam" runat="server">
                    <Input>
                        <Wid:Input ID="input_username" in_type="text" stantext="gebruikersnaam" runat="server" />
                    </Input>
                </Wid:Widget>
                <Wid:Widget ID="password" comment="wachtwoord van account" name="wachtwoord" runat="server">
                    <Input>
                        <Wid:Input ID="input_password" in_type="text" stantext="wachtwoord" runat="server" />
                    </Input>
                </Wid:Widget>
                <Wid:Widget ID="_admin" comment="maak de user een admin" name="admin" toggle="true" runat="server" />
            </Content>
        </Wid:Overlay>
        <Wid:Overlay ID="Remove_User" runat="server">
            <Content>
            <!--    <Wid:Widget ID="" -->
            </Content>
        </Wid:Overlay>
        
        <Wid:Overlay ID="AddTo_Group" runat="server">
            <Content>
                <Wid:Widget ID="AddGroup" comment="Maak een groep aan." name="Toevoegen" runat="server" setting="True" overlayID="AddGroupOID" />
                <Wid:Widget ID="ManageGroup" comment="Verander instellingen aan de groepen." name="Wijzigen" runat="server" setting="True" overlayID="ManageGroupOID" />
                <Wid:Widget ID="DeleteGroup" comment="Verwijder groepen." name="Verwijderen" runat="server" setting="True" overlayID="DeleteGroupOID" />
            </Content>
            </Wid:Overlay>
                
                <Wid:overlay ID="AddGroupOID" runat="server">
                <Content>
                    
                    <Wid:Widget ID="Groupname" comment="Voer hier de groepnaam in." name="groepsnaam" runat="server">
                    <Input>
                        <Wid:Input ID="input_groupname" in_type="text" stantext="new group" runat="server" />
                    </Input>
                    </Wid:Widget>
                    
                    <Wid:Widget ID="InsertUsers" comment="voer hier de gebruikers er aan toe die toegang nodig hebben." name="gebruiker toevoegen" runat="server">
                      
                    </Wid:Widget>
                    
                     <Wid:Widget ID="InsertDevices" comment="voer hier de Apparaten toe." name="Apparaat toevoegen" runat="server">
                       
                    </Wid:Widget>
                    </Content>
                </Wid:overlay>
        <Wid:overlay ID="ManageGroupOID" runat="server">
                <Content>
                    <Wid:Widget ID="CurrentGroup" comment="Selecteer de groep die u aan wilt passen." name="Groep" runat="server">
                        
                    </Wid:Widget>
                    <Wid:Widget ID="CurrentUsers" comment="delete/voeg gebruikers toe." name="gebruiker modificeren" runat="server">
                        
                    </Wid:Widget>
                    <Wid:Widget ID="CurrentDevices" comment="delete/voeg Apparaten toe." name="Apparaten modificeren" runat="server">
                        
                    </Wid:Widget>
                </Content>
                </Wid:overlay>
            
        <Wid:overlay ID="DeleteGroupOID" runat="server">
                <Content>
                <Wid:Widget ID="DeleteGroup" comment="Verwijder hier de groepnaam in." name="groepsnaam" runat="server">
                    <Input>
                        <Wid:Input ID="input1" in_type="text" stantext="new group" runat="server" />
                    </Input>
                    </Wid:Widget>
                    
                </Content>
                </Wid:overlay>
    </div>
</asp:Content>
