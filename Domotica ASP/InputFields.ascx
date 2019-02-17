<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputFields.ascx.cs" Inherits="Domotica_ASP.InputFields" %>
<form>
    <div id="Horiz_Range" runat="server" class="Horiz_range">
        <!-- Horizontal range input -->
        <script>
            waitForElementToDisplay("#<%= this.ClientID.ToString().Remove(this.ClientID.ToString().IndexOf(this.ID.ToString()), (this.ID.ToString()).Length) %>settings", 10, () => {
                var widget_settings = document.getElementById("<%= this.ClientID.ToString().Remove(this.ClientID.ToString().IndexOf(this.ID.ToString()), (this.ID.ToString()).Length) %>settings");
                widget_settings.style.top = "27px";
                widget_settings.style.bottom = "auto";
            }, 0);            
        </script>
        <input type="range" id="<%= this.ClientID.ToString() %>_Range_Input" value="<%= this.stanvalue.ToString() %>" min="<%= this.minvalue.ToString() %>" max="<%= this.maxvalue.ToString() %>" oninput="document.getElementById('<%= this.ClientID.ToString() %>_Range_Display').value = document.getElementById('<%= this.ClientID.ToString() %>_Range_Input').value" />
        <input type="number" id="<%= this.ClientID.ToString() %>_Range_Display" value="<%= this.stanvalue.ToString() %>" min="<%= this.minvalue.ToString() %>" max="<%= this.maxvalue.ToString() %>" oninput="document.getElementById('<%= this.ClientID.ToString() %>_Range_Input').value = document.getElementById('<%= this.ClientID.ToString() %>_Range_Display').value" />
    </div>
    <div id="Vert_Range" runat="server" class="Verti_range">
        <!-- Vertical range input -->
        <script>
            waitForElementToDisplay("#<%= this.ClientID.ToString().Remove(this.ClientID.ToString().IndexOf(this.ID.ToString()), (this.ID.ToString()).Length) %>settings", 10, () => {
                var widget_settings = document.getElementById("<%= this.ClientID.ToString().Remove(this.ClientID.ToString().IndexOf(this.ID.ToString()), (this.ID.ToString()).Length) %>settings");
                widget_settings.style.left = "5px";
                widget_settings.style.right = "auto";
                document.getElementById("<%= this.ClientID.ToString().Remove(this.ClientID.ToString().IndexOf(this.ID.ToString()), (this.ID.ToString()).Length) %>grid_child").children.<%= this.ClientID.ToString().Remove(this.ClientID.ToString().IndexOf(this.ID.ToString()), (this.ID.ToString()).Length) %>grid_child_commenter.children.<%= this.ClientID.ToString().Remove(this.ClientID.ToString().IndexOf(this.ID.ToString()), (this.ID.ToString()).Length) %>grid_child_comment.style.width = "80%"
            }, 0);            
        </script>
        <input type="range" id="<%= this.ClientID.ToString() %>_Range_Input_vert" value="<%= this.stanvalue.ToString() %>" min="<%= this.minvalue.ToString() %>" max="<%= this.maxvalue.ToString() %>" oninput="document.getElementById('<%= this.ClientID.ToString() %>_Range_Display_vert').value = document.getElementById('<%= this.ClientID.ToString() %>_Range_Input_vert').value" />
        <input type="number" id="<%= this.ClientID.ToString() %>_Range_Display_vert" value="<%= this.stanvalue.ToString() %>" min="<%= this.minvalue.ToString() %>" max="<%= this.maxvalue.ToString() %>" oninput="document.getElementById('<%= this.ClientID.ToString() %>_Range_Input_vert').value = document.getElementById('<%= this.ClientID.ToString() %>_Range_Display_vert').value" />
    </div>
    <div id="Text" runat="server" class="input_text">
        <!-- Textbox Input -->
        <script>
            waitForElementToDisplay("#<%= this.ClientID.ToString().Remove(this.ClientID.ToString().IndexOf(this.ID.ToString()), (this.ID.ToString()).Length) %>settings", 10, () => {
                var widget_settings = document.getElementById("<%= this.ClientID.ToString().Remove(this.ClientID.ToString().IndexOf(this.ID.ToString()), (this.ID.ToString()).Length) %>settings");
                widget_settings.style.top = "27px";
                widget_settings.style.bottom = "auto";
            }, 0);            
        </script>
        <input type="text" id="<%= this.ClientID.ToString() %>_text" placeholder="<%= this.stanvalue.ToString() %>"/>
    </div>
    <div id="Number" runat="server" class="input_number">
        <!-- Number Input -->
         <input type="number" id="<%= this.ClientID.ToString() %>_number" value="<%= this.stanvalue.ToString() %>"/>
    </div>
    <div id="Radio" runat="server" class="input_radio">
        <!-- Radio Input -->

    </div>
    <div id="Checkbox" runat="server" class="input_checkbox">
        <!-- checkbox Input -->

    </div>
</form>