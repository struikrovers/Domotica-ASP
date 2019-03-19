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
        <Wid:Widget id="Widget1" name="Vertical" comment="This is a vertical slider!" setting="true" overlayID="overlay1" toggle="true" runat="server" >
            <Input>
                <Wid:Input ID="Vertical" in_type="ver_slider" runat="server"/>
            </Input>
        </Wid:Widget>
        <Wid:Widget id="Widget2" name="Horizontal" comment="This is a horizontal slider!" setting="false" toggle="true" overlayID="overlay1" runat="server" >
            <Input>
                <Wid:Input ID="Horizontal" in_type="hor_slider"  runat="server"/>
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
        <Wid:Widget ID="Widget15" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
        <Wid:Widget ID="Widget16" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
        <Wid:Widget ID="Widget17" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
        <Wid:Widget ID="Widget18" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
        <Wid:Widget ID="Widget19" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
        <Wid:Widget ID="Widget20" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
        <Wid:Widget ID="Widget22" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
        <Wid:Widget ID="Widget21" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
    </div>

    <div class="grid_overlay" id="grid_overlay" onclick="close_overlay(event, false, this)">
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
	    <Wid:Overlay ID="overlay1" runat="server" >
            <Content>
                <Wid:Widget id="Widget7" name="Vertical" comment="This is a vertical slider!" setting="true" overlayID="overlay2" toggle="true" runat="server" >
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
                <Wid:Widget ID="Widget23" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
                <Wid:Widget ID="Widget24" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
                <Wid:Widget ID="Widget25" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
                <Wid:Widget ID="Widget26" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
                <Wid:Widget ID="Widget27" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
                <Wid:Widget ID="Widget28" name="Header" comment="This is the comment box." setting="true" overlayID="overlay1" runat="server" />
            </content>
        </Wid:Overlay>
        <Wid:Overlay ID="overlay2" runat="server" >
            <content>
                    
            </content>
        </Wid:Overlay>
    </div>
</asp:Content>
