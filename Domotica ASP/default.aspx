<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Domotica_ASP._default" %>
<%@ Register TagPrefix="Wid" TagName="Widget" Src="Widget.ascx" %>
<%@ Register TagPrefix="Wid" TagName="Overlay" Src="overlay.ascx" %>
<%@ Register TagPrefix="Wid" TagName="Input" Src="InputFields.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="grid_parent" id="grid_parent" runat="server">
		<!-- DEBUG: needs at least 3 items next to each other. 
			else the grid-gap adds an extra border.
			not sure why this occurs.
		-->
        <!-- NOTE: for any widget with a setting create an overlay! -->
    </div>

    <div class="grid_overlay" id="grid_overlay" onclick="close_overlay(event, false, this)" runat="server">
        <div id="close_overlay_icon" class="overlay_closer" onclick="close_overlay(event, 'close', this)">
        <span class='close_icon'>
            <i class='fa fa-times'></i>
        </span>
        </div>
        <div id="close_overlay_back" class="overlay_closer icon_back" onclick="close_overlay(event, 'back', this)">
            <span class="close_icon" style="font-size:0.65em">
                <i class="fa fa-long-arrow-left"></i>
            </span>
        </div>
        <!-- put overlays here! -->
	    
    </div>
</asp:Content>
