<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Widget.ascx.cs" Inherits="Domotica_ASP.Widget" %>

<div class=grid_child id=grid_child runat="server">
    <p class="grid_child_name"><%= this.name.ToString() %></p>
    <p class="grid_child_name"><%= this.comment.ToString() %></p>
    <div id="toggable" runat="server">
        <label class=toggableContainer onclick="">
            <input class='Toggle_checkbox' type='checkbox'>
            <span class=toggableCheckbox></span>
            <label class='toggle_switch'>
                <span class='toggle_slider round'></span>
            </label>
        </label>
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