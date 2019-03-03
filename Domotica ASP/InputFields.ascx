<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputFields.ascx.cs" Inherits="Domotica_ASP.InputFields" %>
<!-- needed to get styling right -->
<div id="InputField" runat="server">
    <script>
        var ParentID = "<%# this.parent_id %>"
        var ClientID = "<%# this.ClientID.ToString() %>"
    </script>

    <div id="Horiz_Range" runat="server" class="Horiz_range">
        <!-- Horizontal range input -->
        <script>
            ChangeSettingsIcon(ParentID, "", "", "27px", "");           
        </script>
        <asp:TextBox ID="Range_Input" runat="server"></asp:TextBox>
        <asp:TextBox ID="Range_Display" runat="server"></asp:TextBox>
    </div>
    <div id="Vert_Range" runat="server" class="Verti_range">
        <!-- Vertical range input -->
        <script>
            ChangeSettingsIcon(ParentID, "5px");
            para = document.getElementById(ParentID+"grid_child_comment");
            para.style.width = "80%";
        </script>
        <asp:TextBox ID="Range_Input_Vert" runat="server"></asp:TextBox>
        <asp:TextBox ID="Range_Display_Vert" runat="server"></asp:TextBox>
    </div>
    <div id="Text" runat="server" class="input_text">
        <!-- Textbox Input -->
        <script>
            ChangeSettingsIcon(ParentID, "", "", "27px", "auto");          
        </script>
         <asp:TextBox ID="TextInput" runat="server"></asp:TextBox>
    </div>
    <div id="Number" runat="server" class="input_number">
        <!-- Number Input -->
        <script>
            ChangeSettingsIcon(ParentID, "5px");
        </script>
        <asp:TextBox ID="NumberInput" runat="server"></asp:TextBox>
    </div>
    <div id="Radio" runat="server" class="input_radio">
        <!-- Radio Input -->
        <script>
            changeTextPlacement(ParentID, ClientID);
        </script>
        <asp:PlaceHolder ID="_Radio" runat="server">

        </asp:PlaceHolder>
        <asp:Button ID="RadioSubBTN" runat="server" Text="Button" />
    </div>
</div>