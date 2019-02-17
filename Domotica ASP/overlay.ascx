<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="overlay.ascx.cs" Inherits="Domotica_ASP.overlay" %>
<div class="overlay_child" id="<%= this.widget_parent.ToString() %>_overlay_child">
    <div class="overlay_closer" onclick="close_overlay(event, true)">
        <span class='close_icon'>
            <i class='fa fa-times'></i>
        </span>
    </div>
    <div class="overlay_closer icon_back" onclick="close_overlay(event, 'back')">
        <span class="close_icon" style="font-size:0.65em">
            <i class="fa fa-long-arrow-left"></i>
        </span>
    </div>
    <div class="overlay_content">
        <asp:PlaceHolder ID="_Content" runat="server">

        </asp:PlaceHolder>
    </div>
</div>