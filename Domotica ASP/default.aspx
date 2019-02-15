<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Domotica_ASP.index" %>
<%@ Register TagPrefix="Wid" TagName="Widget" Src="Widget.ascx" %>
<%@ Register TagPrefix="Wid" TagName="Overlay" Src="overlay.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
	<head runat="server">
		<meta charset="utf-8" />
		<title>Interface</title>

		<!-- NOTE: jquery libraries:
		<script type="text/javascript" src="//code.jquery.com/jquery-1.6.2.js"></script>
		<script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jqueryui/1.8.14/jquery-ui.js"></script>-->

		<script src="../Properties/main.js"></script>

		<link rel="stylesheet" href="../Properties/base_style.css" />
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />

		<meta name="viewport" content="width=device-width, initial-scale=1" />
	</head>

	<body>
		<div class="grid_parent" id="grid_parent" runat="server"> <!-- Parent of button grid -->
			<!-- DEBUG: needs at least 3 items next to each other. 
				else the grid-gap adds an extra border.
				not sure why this occurs.
			-->
            <form id="global_form" style="width:100%" runat="server">
                <!-- NOTE: for any widget with a setting create an overlay! -->
                <Wid:Widget id="Widget1" name="test" comment="hello!" setting="true" toggle="true" runat="server" />
                <Wid:Widget id="Widget2" name="yes" comment="my dude!" runat="server" />
            </form>
		</div>
        <div class="grid_overlay" id="grid_overlay" onclick="close_overlay(event)">
		    <Wid:Overlay ID="overlay_1" widget_parent="Widget1" runat="server" >
                <content>
                    <Wid:Widget id="Widget3" name="yes" comment="my dude!" runat="server" />
                    <Wid:Widget id="Widget4" name="test" comment="hello!" setting="true" toggle="true" runat="server" />
                    <Wid:Widget id="Widget5" name="test" comment="hello!" toggle="true" runat="server" />
                </content>
            </Wid:Overlay>
            <Wid:Overlay ID="overlay2" widget_parent="Widget4" runat="server" >
                <content>
                    <Wid:Widget id="Widget6" name="yes" comment="my dude!" runat="server" />
                </content>
            </Wid:Overlay>
	    </div>
        
		<!-- div to calculate em -->
		<div id="em_calc" style="height:0;width:0;outline:none;border:none;padding:0;margin:0;"></div>
	</body>
</html>
