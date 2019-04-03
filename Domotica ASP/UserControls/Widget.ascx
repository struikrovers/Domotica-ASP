<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Widget.ascx.cs" Inherits="Domotica_ASP.Widget" %>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="grid_child" id="grid_child" runat="server">
            <div class="grid_child_header">
                <asp:Label ID="grid_child_name" runat="server" Text="Label" CssClass="grid_child_name"></asp:Label>
            </div>
            <div class="grid_child_commenter" id="grid_child_commenter" runat="server">
                <p class="grid_child_comment" id="grid_child_comment" runat="server"></p>
            </div>
            <asp:TextBox ID="timeInput" runat="server" onclick="setTime(this)"></asp:TextBox>
            <asp:Button ID="submitBTN" runat="server" Text="submit" CssClass="submitbutton" OnClientClick="OpenUpdater()" />

            <div id="input" class="widget_input" runat="server">
                <asp:PlaceHolder ID="_Input" runat="server"></asp:PlaceHolder>
            </div>
            <div id="toggable" runat="server">
                <div class="toggableContainer">
                    <asp:CheckBox ID="Toggle_Checkbox" runat="server" />
                    <asp:Label ID="ToggleLabel" CssClass="toggle_switch" runat="server" AssociatedControlID="Toggle_Checkbox">
                    <span class='toggle_slider round'></span>
                    </asp:Label>
                    <span class="toggableCheckbox_span"></span>
                </div>
            </div>
            <div id="settings" class='settings' runat="server">
                <label class='settings_label'>
                    <span id='settings_icon' class='settings_icon' onclick="onclick is defined in code behind" runat="server">
                        <i style='font-size: 0.87em' class='fa fa-gear'></i>
                    </span>
                </label>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
