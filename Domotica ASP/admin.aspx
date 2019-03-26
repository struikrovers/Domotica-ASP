<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Domotica_ASP.admin" %>

<%@ Register TagPrefix="Wid" TagName="Widget" Src="Widget.ascx" %>
<%@ Register TagPrefix="Wid" TagName="Overlay" Src="overlay.ascx" %>
<%@ Register TagPrefix="Wid" TagName="Input" Src="InputFields.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Administration </title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid_parent" id="grid_parent" runat="server">
        <!-- DEBUG: needs at least 3 items next to each other. 
			else the grid-gap adds an extra border.
			not sure why this occurs.
		-->
        <!-- NOTE: for any widget with a setting create an overlay! -->
        <Wid:Widget ID="Register" name="Registratie" comment="Beheer gebruikers" setting="true" overlayID="Register_overlay" runat="server" toggle="False" />
        <Wid:Widget ID="GroupManage" name="Groepbeheer" comment="Beheer groepen" setting="true" overlayID="AddTo_Group" runat="server" toggle="False" />
        <Wid:Widget ID="DeviceManage" name="Apparaatbeheer" comment="Beheer Apparaten" setting="true" overlayID="DeviceManager" runat="server" toggle="False" />
    </div>
    <div class="grid_overlay" id="grid_overlay" onclick="close_overlay(event, false, this)" runat="server">
        <div id="close_overlay_icon" class="overlay_closer" onclick="close_overlay(event, 'close', this)">
            <span class='close_icon'>
                <i class='fa fa-times'></i>
            </span>
        </div>
        <div id="close_overlay_back" class="overlay_closer icon_back" onclick="close_overlay(event, 'back', this)">
            <span class="close_icon" style="font-size: 0.65em">
                <i class="fa fa-long-arrow-left"></i>
            </span>
        </div>
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
            </Content>
        </Wid:Overlay>

        <Wid:Overlay ID="AddTo_Group" runat="server">
            <Content>
                <Wid:Widget ID="AddGroup" comment="Maak een groep aan." name="Toevoegen" runat="server" setting="True" overlayID="AddGroupOID" />
                <Wid:Widget ID="ManageGroup" comment="Verander instellingen aan de groepen." name="Wijzigen" runat="server" setting="True" overlayID="ManageGroupOID" />
                <Wid:Widget ID="DeleteGroup" comment="Verwijder groepen." name="Verwijderen" runat="server" setting="True" overlayID="DeleteGroupOID" />
            </Content>
        </Wid:Overlay>

        <Wid:Overlay ID="AddGroupOID" runat="server">
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
        </Wid:Overlay>
        <Wid:Overlay ID="ManageGroupOID" runat="server">
            <Content>
                <Wid:Widget ID="CurrentGroup" comment="Selecteer de groep die u aan wilt passen." name="Groep" runat="server">
                </Wid:Widget>
                <Wid:Widget ID="CurrentUsers" comment="delete/voeg gebruikers toe." name="Gebruiker" runat="server">
                </Wid:Widget>
                <Wid:Widget ID="CurrentDevices" comment="delete/voeg Apparaten toe." name="Apparaten" runat="server">
                </Wid:Widget>
            </Content>
        </Wid:Overlay>

        <Wid:Overlay ID="DeleteGroupOID" runat="server">
            <Content>
                <Wid:Widget ID="RemoveGroup" comment="Verwijder hier de groep." name="verwijder groep" runat="server">
                    <Input>
                        <Wid:Input ID="input_deletegroup" in_type="DropDownList" runat="server">
                        </Wid:Input>
                    </Input>
                </Wid:Widget>

            </Content>
        </Wid:Overlay>

        <Wid:Overlay ID="DeviceManager" runat="server">
            <Content>
                <Wid:Widget ID="AddDevice" comment="Voeg apparaat toe." name="Toevoegen" runat="server" setting="true" overlayID="AddDeviceOID" />
                <Wid:Widget ID="ManageDevice" comment="Verander instellingen aan de apparaten." name="Wijzigen" runat="server" setting="True" overlayID="ManageDeviceOID" />
                <Wid:Widget ID="DeleteDevice" comment="Verwijder apparaten." name="Verwijderen" runat="server" setting="True" overlayID="DeleteDeviceOID" />
            </Content>
        </Wid:Overlay>
        <Wid:Overlay ID="Overlay1" runat="server">
            <Content>

                <Wid:Widget ID="Widget1" comment="Voer hier de groepnaam in." name="groepsnaam" runat="server">
                    <Input>
                        <Wid:Input ID="input1" in_type="text" stantext="new group" runat="server" />
                    </Input>
                </Wid:Widget>

                <Wid:Widget ID="Widget2" comment="voer hier de gebruikers er aan toe die toegang nodig hebben." name="gebruiker toevoegen" runat="server">
                </Wid:Widget>

                <Wid:Widget ID="Widget3" comment="voer hier de Apparaten toe." name="Apparaat toevoegen" runat="server">
                </Wid:Widget>
            </Content>
        </Wid:Overlay>
        <Wid:Overlay ID="Overlay2" runat="server">
            <Content>
                <Wid:Widget ID="Widget4" comment="Selecteer de groep die u aan wilt passen." name="Groep" runat="server">
                </Wid:Widget>
                <Wid:Widget ID="Widget5" comment="delete/voeg gebruikers toe." name="Gebruiker" runat="server">
                </Wid:Widget>
                <Wid:Widget ID="Widget6" comment="delete/voeg Apparaten toe." name="Apparaten" runat="server">
                </Wid:Widget>
            </Content>
        </Wid:Overlay>

        <Wid:Overlay ID="Overlay3" runat="server">
            <Content>
                <Wid:Widget ID="Widget7" comment="Verwijder hier de groep." name="verwijder groep" runat="server">
                    <Input>
                        <Wid:Input ID="input2" in_type="DropDownList" runat="server">
                        </Wid:Input>
                    </Input>

                </Wid:Widget>
               </Content>
              </Wid:Overlay>
    </div>
</asp:Content>
