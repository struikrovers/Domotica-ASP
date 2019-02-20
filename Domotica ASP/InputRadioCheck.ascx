<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InputRadioCheck.ascx.cs" Inherits="Domotica_ASP.InputRadioCheck" %>

<div id="inputs" class="input_radiocheck" runat="server">
    <input <%= this._checked.ToString() %> type="<%= this.type.ToString() %>" name="<%= this.ClientID.ToString() %>" />
    <label for="<%= this.ClientID.ToString() %>"><%= this.value.ToString() %></label ><br />
</div>