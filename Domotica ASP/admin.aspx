<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="Domotica_ASP.admin" %>

<%@ Register TagPrefix="Wid" TagName="Widget" Src="~/UserControls/Widget.ascx" %>
<%@ Register TagPrefix="Wid" TagName="Overlay" Src="~/UserControls/overlay.ascx" %>
<%@ Register TagPrefix="Wid" TagName="Input" Src="~/UserControls/InputFields.ascx" %>

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
        <asp:UpdatePanel ID="outputUpdatePanel" runat="server">
            <ContentTemplate>
                <div id="update_panel_div" class="updateNotifier">
                    <asp:Label ID="output" runat="server"></asp:Label>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

        <Wid:Widget ID="Register" name="Registratie" comment="Beheer gebruikers" setting="true" overlayID="Register_overlay" runat="server" toggle="False" />
        <Wid:Widget ID="GroupManage" name="Groepbeheer" comment="Beheer groepen" setting="true" overlayID="AddTo_Group" runat="server" toggle="False" />
        <Wid:Widget ID="DeviceManage" name="App Beheer" comment="Beheer Apparaten" setting="true" overlayID="DeviceManager" runat="server" toggle="False" />
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
        
        <!-- User Overlay -->
        <Wid:Overlay ID="Register_overlay" runat="server">
            <Content>
                <Wid:Widget ID="Add" comment="Voeg een gebruiker toe." name="Toevoegen" runat="server" setting="True" overlayID="Add_User" />
                <Wid:Widget ID="Remove" comment="Verwijder een gebruiker." name="Verwijderen" runat="server" setting="True" overlayID="Remove_User" />
            </Content>
        </Wid:Overlay>
            <Wid:Overlay ID="Add_User" runat="server">
                <Content>
                    <asp:CreateUserWizard ID="CreateUserWizard1" runat="server" LoginCreatedUser="False" EmailRegularExpression="^(.{1,})+@(.{1,})\.+(.{3,5})$" autocomplete="off" OnCreatedUser="ReloadPage">
                        <WizardSteps>
                            <asp:CreateUserWizardStep ID="CreateUserWizardStep1" runat="server">
                                <ContentTemplate>
                                    <table>
                                        <tr>
                                            <td align="center" colspan="2">Sign Up for Your New Account</td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">User Name:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="UserName" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName" ErrorMessage="User Name is required." ToolTip="User Name is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Password" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password" ErrorMessage="Password is required." ToolTip="Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="ConfirmPasswordLabel" runat="server" AssociatedControlID="ConfirmPassword">Confirm Password:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="ConfirmPasswordRequired" runat="server" ControlToValidate="ConfirmPassword" ErrorMessage="Confirm Password is required." ToolTip="Confirm Password is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <asp:Label ID="EmailLabel" runat="server" AssociatedControlID="Email">E-mail:</asp:Label>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="Email" runat="server"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="EmailRequired" runat="server" ControlToValidate="Email" ErrorMessage="E-mail is required." ToolTip="E-mail is required." ValidationGroup="CreateUserWizard1">*</asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:CompareValidator ID="PasswordCompare" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" Display="Dynamic" ErrorMessage="The Password and Confirmation Password must match." ValidationGroup="CreateUserWizard1"></asp:CompareValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2">
                                                <asp:RegularExpressionValidator ID="EmailRegExp" runat="server" ControlToValidate="Email" Display="Dynamic" ErrorMessage="Please enter a different e-mail." ValidationExpression="^(.{1,})+@(.{1,})\.+(.{3,5})$" ValidationGroup="CreateUserWizard1"></asp:RegularExpressionValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" colspan="2" style="color:Red;">
                                                <asp:Literal ID="ErrorMessage" runat="server" EnableViewState="False"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:CreateUserWizardStep>
                            <asp:CompleteWizardStep ID="CompleteWizardStep1" runat="server">
                            </asp:CompleteWizardStep>
                        </WizardSteps>
                    </asp:CreateUserWizard>
                </Content>
            </Wid:Overlay>

            <Wid:Overlay ID="Remove_User" runat="server" cssClass="overlay_content ajaxContent">
                <Content>
                    <asp:UpdatePanel ID="Remove_User_UP" runat="server">
                        <ContentTemplate>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </Wid:Overlay>


        <Wid:Overlay ID="AddTo_Group" runat="server">
            <Content>
                <Wid:Widget ID="AddGroup" comment="Maak een groep aan." name="Toevoegen" runat="server" setting="True" overlayID="AddGroupOID" />
                <Wid:Widget ID="ManageGroup" comment="Verander instellingen aan de groepen." name="Wijzigen" runat="server" setting="True" overlayID="ManageGroupOID" />
                <Wid:Widget ID="DeleteGroup" comment="Verwijder groepen." name="Verwijderen" runat="server" setting="True" overlayID="DeleteGroupOID" />
            </Content>
        </Wid:Overlay>

        <!-- Group Overlay -->
        <Wid:Overlay ID="AddGroupOID" runat="server">
            <Content>

                <Wid:Widget ID="Groupname" comment="Voer hier de groepnaam in." name="Groepsnaam" runat="server">
                    <Input>
                        <Wid:Input ID="input_groupname" in_type="text" stantext="new group" runat="server" />
                    </Input>
                </Wid:Widget>

                <Wid:Widget ID="InsertUsers" comment="voeg hier de gebruikers toe aan de groep." name="Gebruiker" setting="true" overlayID="InsertUsersOverlay" runat="server" />
                <Wid:Widget ID="InsertDevices" comment="voeg hier de Apparaten toe waartoe de groep toegang heeft." setting="true" overlayID="InsertDevicesOverlay" name="Apparaat" runat="server" />
                <Wid:Widget ID="Submit_AddGroupOID" name="verstuur" submittable="true" runat="server" show_notifier="False" />
            </Content>
        </Wid:Overlay>

            <Wid:Overlay ID="InsertUsersOverlay" runat="server">
                <Content>

                </Content>
            </Wid:Overlay>
            <Wid:Overlay ID="InsertDevicesOverlay" runat="server">
                <Content>

                </Content>
            </Wid:Overlay>

        <Wid:Overlay ID="ManageGroupOID" runat="server">
            <Content>
                <Wid:Widget ID="CurrentGroup" comment="Selecteer de groep die u aan wilt passen." name="Groep" runat="server">
                    <Input>
                        <Wid:Input ID="GroupDDL" in_type="DropDownList" runat="server">
                            <__DropList>
                                <asp:DropDownList ID="GroupDDlist" runat="server" OnSelectedIndexChanged="changeActiveGroup" CausesValidation="true" AutoPostBack="True">

                                </asp:DropDownList>
                            </__DropList>
                        </Wid:Input>
                    </Input>
                </Wid:Widget>
                <Wid:Widget ID="CurrentUsers" comment="delete/voeg gebruikers toe." name="Gebruiker" setting="true" overlayID="modifyUsers" runat="server" />
                <Wid:Widget ID="CurrentDevices" comment="delete/voeg Apparaten toe." name="Apparaten" setting="true" overlayID="modifyDevices" runat="server" />
                <Wid:Widget ID="Submit_ManageGroupOID" name="verstuur" submittable="true" runat="server"/>
            </Content>
        </Wid:Overlay>

            <Wid:Overlay ID="modifyUsers" runat="server">
                <Content>
                    <asp:UpdatePanel ID="modifyUsers_UP" runat="server">
                        <ContentTemplate>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </Wid:Overlay>
            <Wid:Overlay ID="modifyDevices" runat="server">
                <Content>
                    <asp:UpdatePanel ID="modifyDevices_UP" runat="server">
                        <ContentTemplate>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </Content>
            </Wid:Overlay>

        <Wid:Overlay ID="DeleteGroupOID" runat="server" cssClass="overlay_content ajaxContent">
            <Content>
                <asp:UpdatePanel ID="DeleteGroup_UP" runat="server">
                    <ContentTemplate>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </Content>
        </Wid:Overlay>

        <Wid:Overlay ID="DeviceManager" runat="server">
            <Content>
                <Wid:Widget ID="AddDevice" comment="Voeg apparaat toe." name="Toevoegen" runat="server" setting="true" overlayID="AddDeviceOID" />
                <Wid:Widget ID="DeleteDevice" comment="Verwijder apparaten." name="Verwijderen" runat="server" setting="True" overlayID="DeleteDeviceOID" />
            </Content>
        </Wid:Overlay>

        <!-- Apparaten Overlay -->
        <Wid:Overlay ID="AddDeviceOID" runat="server">
            <Content>
                <Wid:Widget ID="DeviceName" comment="Voer hier de apparaatnaam in." name="Apparaatnaam" runat="server">
                    <Input>
                        <Wid:Input ID="input_devicename" in_type="text" stantext="apparaat naam" runat="server" />
                    </Input>
                </Wid:Widget>

                <Wid:Widget ID="DeviceType" comment="Kies uw apparaat." name="Type" runat="server">
                    <Input>
                        <Wid:Input ID="input_devicetype" in_type="DropDownList" runat="server">
                            <__DropList>
                                <asp:dropdownlist ID="input_devicetype_list" runat="server">
                                    
                                </asp:dropdownlist>
                            </__DropList>
                        </Wid:Input>
                    </Input>
                </Wid:Widget>

                <Wid:Widget ID="DevicePin" comment="Op welke pin is het apparaat aangesloten?" name="Aansluiting" runat="server">
                    <Input>
                        <Wid:Input ID="input_devicepin" in_type="DropDownList" runat="server">
                            <__DropList>
                                <asp:dropdownlist ID="input_devicepin_list" runat="server">
                                    
                                </asp:dropdownlist>
                            </__DropList>
                        </Wid:Input>
                    </Input>
                </Wid:Widget>
                <Wid:Widget ID="Submit_AddDeviceOID" submittable="true" name="verstuur" runat="server" />
            </Content>
        </Wid:Overlay>

        <Wid:Overlay ID="DeleteDeviceOID" runat="server" cssClass="overlay_content ajaxContent">
            <Content>
                <asp:UpdatePanel ID="DeleteDevice_UP" runat="server">
                    <ContentTemplate>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </Content>
        </Wid:Overlay>
    </div>
</asp:Content>
