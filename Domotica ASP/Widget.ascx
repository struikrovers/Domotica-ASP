<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Widget.ascx.cs" Inherits="Domotica_ASP.Widget" %>

<div class=grid_child id=grid_child runat="server">
    <div class="grid_child_header">
        <p class="grid_child_name"><%= this.name.ToString() %></p>
    </div>
    <div class="grid_child_commenter" id="grid_child_commenter" runat="server">
        <p class="grid_child_comment" id="grid_child_comment" runat="server"><%= this.comment.ToString() %></p>
    </div>
    <div id="input" class="widget_input" runat="server">
        <asp:PlaceHolder ID="_Input" runat="server">

        </asp:PlaceHolder>
    </div>
    <div id="toggable" runat="server">
        <div class=toggableContainer>
            <input id=Toggle_checkbox class='Toggle_checkbox' type='checkbox' runat="server">
            <span class=toggableCheckbox_span></span>
            <label for="<%= this.ClientID.ToString() %>_Toggle_checkbox" class='toggle_switch' onclick="">
                <span class='toggle_slider round'></span>
            </label>
        </div>
    </div>
    <div id="settings" class='settings' runat="server">
        <label class='settings_label'>
            <input type="checkbox" id="<%= this.ID.ToString() %>_settings_checkbox" class="settings_checkbox"/>
            <span class='settings_icon' onclick="open_overlay(event, '<%= this.ID.ToString() %>_overlay_child');">
                <i style='font-size: 0.87em' class='fa fa-gear'></i>
            </span>
        </label>
    </div>
</div>