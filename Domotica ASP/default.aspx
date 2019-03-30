﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Domotica_ASP._default" %>

<%@ Register TagPrefix="Wid" TagName="Widget" Src="UserControls/Widget.ascx" %>
<%@ Register TagPrefix="Wid" TagName="Overlay" Src="UserControls/overlay.ascx" %>
<%@ Register TagPrefix="Wid" TagName="Input" Src="UserControls/InputFields.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    </div>
    <asp:UpdatePanel ID="ScheduleUpdatePanel" runat="server">
        <ContentTemplate>
            <asp:GridView ID="ScheduleDisplayer" runat="server">
                <Columns>
                    <asp:CommandField ButtonType="Button" HeaderText="Verwijderen" ShowDeleteButton="True" ShowHeader="True" />
                </Columns>
            </asp:GridView>
            </ContentTemplate>
    </asp:UpdatePanel>

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
