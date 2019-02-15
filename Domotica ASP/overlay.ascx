<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="overlay.ascx.cs" Inherits="Domotica_ASP.overlay" %>
<div class="overlay_child" id="<%= this.widget_parent.ToString() %>_overlay_child">
    <div class="overlay_content">
        <asp:PlaceHolder ID="_Content" runat="server">

        </asp:PlaceHolder>
    </div>
</div>