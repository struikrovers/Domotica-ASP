<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Domotica_ASP.index" %>
<%@ Register TagPrefix="Wid" TagName="Widget" Src="Widget.ascx" %>
<%@ Register TagPrefix="Wid" TagName="Overlay" Src="overlay.ascx" %>
<%@ Register TagPrefix="Wid" TagName="Input" Src="InputFields.ascx" %>

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
        <!-- https://fontawesome.com/v4.7.0/icons/ -->
        <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css" />
        <!-- https://fonts.google.com/specimen/Exo+2?selection.family=Exo+2:200|Open+Sans -->
        <link href="https://fonts.googleapis.com/css?family=Exo+2:300|Roboto+Slab" rel="stylesheet" />

		<meta name="viewport" content="width=device-width, initial-scale=1" />
	</head>

	<body>
        <form runat="server">
		    <div class="grid_parent" id="grid_parent" runat="server"> <!-- Parent of button grid -->
			    <!-- DEBUG: needs at least 3 items next to each other. 
				    else the grid-gap adds an extra border.
				    not sure why this occurs.
			    -->
                <!-- NOTE: for any widget with a setting create an overlay! -->
                <Wid:Widget id="Widget1" name="Vertical" comment="This is a vertical slider!" setting="true" overlayID="overlay1" toggle="true" runat="server" >
                    <Input>
                        <Wid:Input ID="Vertical" in_type="ver_slider" runat="server"/>
                    </Input>
                </Wid:Widget>
                <Wid:Widget id="Widget2" name="Horizontal" comment="This is a horizontal slider!" setting="true" overlayID="overlay1" runat="server" >
                    <Input>
                        <Wid:Input ID="Horizontal" in_type="hor_slider" runat="server"/>
                    </Input>
                </Wid:Widget>
                <Wid:Widget ID="Widget3" name="Number" comment="This is a number input!" setting="true" overlayID="overlay1" runat="server"  >
                    <Input>
                        <Wid:Input ID="Number" in_type="number" runat="server"/>
                    </Input>
                </Wid:Widget>
                <Wid:Widget ID="Widget4" name="Text" comment="This is a text input!" setting="true" overlayID="overlay1" runat="server"  >
                    <Input>
                        <Wid:Input ID="Text" in_type="text" stantext="zo te zien werkt t!" setting="true" overlayID="overlay1" runat="server"/>
                    </Input>
                </Wid:Widget>
                <Wid:Widget ID="Widget5" name="Radio" comment="This is a radio input!" setting="true" overlayID="overlay1" runat="server"  >
                    <Input>
                        <Wid:Input ID="Radio" in_type="radio" runat="server" button_text="submit">
                            <__Radio>
                                <asp:RadioButtonList ID="RadioButton_Input" runat="server">
                                    <asp:ListItem>Cat</asp:ListItem>
                                    <asp:ListItem Selected="True">Dog</asp:ListItem>
                                    <asp:ListItem>Fish</asp:ListItem>
                                    <asp:ListItem>Zebra</asp:ListItem>
                                </asp:RadioButtonList>
                            </__Radio>
                        </Wid:Input>
                    </Input>
                </Wid:Widget>
                <Wid:Widget ID="Widget6" name="Checkbox" comment="This is a checkbox input!" setting="true" overlayID="overlay1" runat="server"  >
                    <Input>
                        <Wid:Input ID="Checkbox" in_type="radio" runat="server">
                            <__Radio>
                                <asp:CheckBoxList ID="CheckBoxList1" runat="server">
                                    <asp:ListItem>1</asp:ListItem>
                                    <asp:ListItem Selected="True">2</asp:ListItem>
                                    <asp:ListItem>3</asp:ListItem>
                                    <asp:ListItem>4</asp:ListItem>
                                </asp:CheckBoxList>
                            </__Radio>
                        </Wid:Input>
                    </Input>
                </Wid:Widget>
                <Wid:Widget ID="Widget13" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
		    </div>

            <div class="grid_overlay" id="grid_overlay" onclick="close_overlay(event, false, this)">
		        <Wid:Overlay ID="overlay1" runat="server" >
                    <Content>
                        <Wid:Widget id="Widget7" name="Vertical" comment="This is a vertical slider!" setting="true" overlayID="overlay1" toggle="true" runat="server" >
                            <Input>
                                <Wid:Input ID="Input1" in_type="ver_slider" runat="server"/>
                            </Input>
                        </Wid:Widget>
                        <Wid:Widget id="Widget8" name="Horizontal" comment="This is a horizontal slider!" setting="true" overlayID="overlay1" runat="server" >
                            <Input>
                                <Wid:Input ID="Input2" in_type="hor_slider" runat="server"/>
                            </Input>
                        </Wid:Widget>
                        <Wid:Widget ID="Widget9" name="Number" comment="This is a number input!" setting="true" overlayID="overlay1" runat="server"  >
                            <Input>
                                <Wid:Input ID="Input3" in_type="number" runat="server"/>
                            </Input>
                        </Wid:Widget>
                        <Wid:Widget ID="Widget10" name="Text" comment="This is a text input!" setting="true" overlayID="overlay1" runat="server"  >
                            <Input>
                                <Wid:Input ID="Input4" in_type="text" stantext="werkt t eindelijk?" setting="true" overlayID="overlay1" runat="server"/>
                            </Input>
                        </Wid:Widget>
                        <Wid:Widget ID="Widget11" name="Radio" comment="This is a radio input!" setting="true" overlayID="overlay1" runat="server"  >
                            <Input>
                                <Wid:Input ID="Input5" in_type="radio" runat="server" button_text="submit">
                                    <__Radio>
                                        <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                                            <asp:ListItem>Cat</asp:ListItem>
                                            <asp:ListItem Selected="True">Dog</asp:ListItem>
                                            <asp:ListItem>Fish</asp:ListItem>
                                            <asp:ListItem>Zebra</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </__Radio>
                                </Wid:Input>
                            </Input>
                        </Wid:Widget>
                        <Wid:Widget ID="Widget12" name="Checkbox" comment="This is a checkbox input!" setting="true" overlayID="overlay1" runat="server"  >
                            <Input>
                                <Wid:Input ID="Input6" in_type="radio" runat="server">
                                    <__Radio>
                                        <asp:CheckBoxList ID="CheckBoxList2" runat="server">
                                            <asp:ListItem>1</asp:ListItem>
                                            <asp:ListItem Selected="True">2</asp:ListItem>
                                            <asp:ListItem>3</asp:ListItem>
                                            <asp:ListItem>4</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </__Radio>
                                </Wid:Input>
                            </Input>
                        </Wid:Widget>
                        <Wid:Widget ID="Widget14" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
                    </content>
                </Wid:Overlay>
                <Wid:Overlay ID="overlay2" runat="server" >
                    <content>
                    
                    </content>
                </Wid:Overlay>
	        </div>
        </form>
		<!-- div to calculate em -->
		<div id="em_calc" style="height:0;width:0;outline:none;border:none;padding:0;margin:0;"></div>
	</body>
</html>
