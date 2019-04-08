using Devart.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotica_ASP
{
    public partial class admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Membership.GetUser() != null)
            {
                if (Roles.GetRolesForUser().Contains("admins"))
                {
                    outputUpdatePanel.Attributes["class"] = "updateNotifierParent";
                    Remove_User_UP.Attributes["class"] = "UpdatePanelOverlay";

                    DeleteDevice_UP.Attributes["class"] = "UpdatePanelOverlay";

                    DeleteGroup_UP.Attributes["class"] = "UpdatePanelOverlay";
                    modifyUsers_UP.Attributes["class"] = "UpdatePanelOverlay";
                    modifyDevices_UP.Attributes["class"] = "UpdatePanelOverlay";

                    MySqlCommand userQuery = new MySqlCommand("SELECT username FROM users");
                    List<List<string>> GebruikersTabel = global.ExecuteReader(userQuery, out string error_gebruiker);
                    if (error_gebruiker != "")
                    {
                        /* do something with the error */
                        global.generic_QueryErrorHandler(userQuery, error_gebruiker);
                    }
                    else
                    {
                        foreach (List<string> row in GebruikersTabel)
                        {
                            if (row[0] != Membership.GetUser().UserName)
                            {
                                // Remove user overlay
                                Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                                widget.name = row[0];
                                widget.comment = string.Format("Dit is het account van: {0}", row[0]);
                                widget.toggle = true;
                                widget.ID = row[0];
                                Remove_User_UP.ContentTemplateContainer.Controls.Add(widget);
                            }
                            // insert into group overlay
                            Widget widget2 = (Widget)LoadControl("~/UserControls/Widget.ascx");
                            widget2.name = row[0];
                            widget2.comment = string.Format("Dit is het account van: {0}", row[0]);
                            widget2.toggle = true;
                            widget2.ID = row[0];
                            InsertUsersOverlay.Content.Controls.Add(widget2);
                            // modify users in group overlay
                            Widget widget3 = (Widget)LoadControl("~/UserControls/Widget.ascx");
                            widget3.name = row[0];
                            widget3.comment = string.Format("Dit is het account van: {0}", row[0]);
                            widget3.toggle = true;
                            widget3.ID = row[0];
                            modifyUsers_UP.ContentTemplateContainer.Controls.Add(widget3);
                        }
                        Widget Submit_Remove_User = (Widget)LoadControl("~/UserControls/Widget.ascx");
                        Submit_Remove_User.ID = "Submit_Remove_User";
                        Submit_Remove_User.submittable = true;
                        Submit_Remove_User.name = "verstuur";
                        Submit_Remove_User.submit_function = DeleteUser;
                        Remove_User_UP.ContentTemplateContainer.Controls.Add(Submit_Remove_User);
                    }

                    MySqlCommand get_busy_pins = new MySqlCommand("SELECT pinnr FROM pin");
                    List<List<string>> bezette_pins = global.ExecuteReader(get_busy_pins, out string error_get_pins);
                    if (error_get_pins != "")
                    {
                        /* do something with the error */
                        global.generic_QueryErrorHandler(get_busy_pins, error_get_pins);
                    }
                    else
                    {
                        global.init_pinnr();
                        List<string> bz_pins = new List<string>();
                        foreach(List<string> pin in bezette_pins)
                        {
                            bz_pins.Add(pin[0]);
                        }
                        foreach(string pinnr in global.pinnr)
                        {
                            if (bezette_pins.Count != 0)
                            {
                                if (!bz_pins.Contains(pinnr))
                                {
                                    input_devicepin_list.Items.Add(pinnr);
                                }
                            }
                            else
                            {
                                input_devicepin_list.Items.Add(pinnr);
                            }
                        }
                    }

                    MySqlCommand getTypes = new MySqlCommand("SELECT `Type` FROM apparaattype");
                    List<List<string>> getTypes_result = global.ExecuteReader(getTypes, out string getTypes_error);
                    if (getTypes_error != "")
                    {
                        /* do something with the error */
                        global.generic_QueryErrorHandler(getTypes, getTypes_error);
                    }
                    else
                    {
                        foreach(List<string> type in getTypes_result)
                        {
                            input_devicetype_list.Items.Add(type[0]);
                        }
                    }

                    MySqlCommand DeviceQuery = new MySqlCommand("SELECT naam FROM apparaat");
                    List<List<string>> DeviceTable = global.ExecuteReader(DeviceQuery, out string DeviceQueryError);
                    if (DeviceQueryError != "")
                    {
                        /* do something with the error */
                        global.generic_QueryErrorHandler(DeviceQuery, DeviceQueryError);
                    }
                    else
                    {
                        foreach (List<string> row in DeviceTable)
                        {
                            // Delete Device Overlay
                            Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                            widget.name = row[0];
                            widget.comment = string.Format("Dit is apparaat: {0}", row[0]);
                            widget.toggle = true;
                            widget.ID = row[0];
                            DeleteDevice_UP.ContentTemplateContainer.Controls.Add(widget);
                            // Insert Device into Group overlay
                            Widget widget2 = (Widget)LoadControl("~/UserControls/Widget.ascx");
                            widget2.name = row[0];
                            widget2.comment = string.Format("Dit is apparaat: {0}", row[0]);
                            widget2.toggle = true;
                            widget2.ID = row[0];
                            InsertDevicesOverlay.Content.Controls.Add(widget2);
                            // manage group overlay
                            Widget widget3 = (Widget)LoadControl("~/UserControls/Widget.ascx");
                            widget3.name = row[0];
                            widget3.comment = string.Format("Dit is apparaat: {0}", row[0]);
                            widget3.toggle = true;
                            widget3.ID = row[0];
                            modifyDevices_UP.ContentTemplateContainer.Controls.Add(widget3);
                        }
                        // delete device submit button
                        Widget Submit_Remove_Device = (Widget)LoadControl("~/UserControls/Widget.ascx");
                        Submit_Remove_Device.ID = "Submit_Remove_User";
                        Submit_Remove_Device.submittable = true;
                        Submit_Remove_Device.name = "verstuur";
                        Submit_Remove_Device.submit_function = RemoveDevice;
                        DeleteDevice_UP.ContentTemplateContainer.Controls.Add(Submit_Remove_Device);
                    }

                    MySqlCommand GroupQuery = new MySqlCommand("SELECT groepnaam FROM `group`");
                    List<List<string>> GroupTable = global.ExecuteReader(GroupQuery, out string GroupQueryError);
                    if (GroupQueryError != "")
                    {
                        /* do something with the error */
                        global.generic_QueryErrorHandler(GroupQuery, GroupQueryError);
                    }
                    else
                    {
                        foreach (List<string> row in GroupTable)
                        {
                            Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                            widget.name = row[0];
                            widget.comment = string.Format("Dit is groep: {0}", row[0]);
                            widget.toggle = true;
                            widget.ID = row[0];
                            DeleteGroup_UP.ContentTemplateContainer.Controls.Add(widget);
                            GroupDDlist.Items.Add(row[0]);
                        }
                        Widget Submit_DeleteGroupOID = (Widget)LoadControl("~/UserControls/Widget.ascx");
                        Submit_DeleteGroupOID.ID = " Submit_DeleteGroupOID";
                        Submit_DeleteGroupOID.submittable = true;
                        Submit_DeleteGroupOID.name = "verstuur";
                        Submit_DeleteGroupOID.submit_function = RemoveGroup;
                        DeleteGroup_UP.ContentTemplateContainer.Controls.Add(Submit_DeleteGroupOID);
                        ModifyGroup_Selected(GroupDDlist.SelectedValue);
                    }

                    Submit_AddDeviceOID.submit_function = MakeApparaat;

                    Submit_ManageGroupOID.submit_function = ModifyGroup;

                    Submit_AddGroupOID.submit_function = MakeGroup;
                }
                else
                {                    
                    grid_parent.Controls.Clear();
                    grid_overlay.Controls.Clear();
                }
            }
            else
            {
                grid_parent.Controls.Clear();
                grid_overlay.Controls.Clear();
            }
        }

        public void DeleteUser(object sender, EventArgs e)
        {
            List<string> users = new List<string>();
            List<string> checked_users = new List<string>();
            List<string> removed_users = new List<string>();

            foreach (Control con in Remove_User_UP.ContentTemplateContainer.Controls)
            {
                if (con.GetType() == LoadControl("~/UserControls/Widget.ascx").GetType())
                {
                    Widget wid = (Widget)con;
                    if (wid.name != "verstuur")
                    {
                        users.Add(wid.name);
                        CheckBox ch_box = (CheckBox)wid.FindControl("Toggle_Checkbox");
                        if (ch_box.Checked)
                        {
                            checked_users.Add(wid.name);
                        }
                    }
                }
            }
            
            foreach (string username in checked_users)
            {
                using (MySqlCommand remove_user = new MySqlCommand("DELETE FROM users WHERE username = :gbnaam"))
                {
                    remove_user.Parameters.Add("gbnaam", username);
                    if (!global.ExecuteChanger(remove_user, out string remove_user_error))
                    {
                        /* do something with the error */
                        global.generic_QueryErrorHandler(remove_user, remove_user_error);
                    }
                    else
                    {
                        removed_users.Add(username);
                        users.Remove(username);
                    }
                }
            }
            output.Text = "Verwijderde gebruikers: ";
            for (int i = 0; i < removed_users.Count; i++)
            {
                if (i < removed_users.Count - 1)
                {
                    output.Text += removed_users[i] + ", ";
                }
                else
                {
                    output.Text += removed_users[i] + ".";
                }
            }

            Remove_User_UP.ContentTemplateContainer.Controls.Clear();
            foreach (string username in users)
            {
                Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                widget.name = username;
                widget.comment = string.Format("Dit is het account van: {0}", username);
                widget.toggle = true;
                widget.ID = username;
                Remove_User_UP.ContentTemplateContainer.Controls.Add(widget);
            }
            Widget Submit_Remove_User = (Widget)LoadControl("~/UserControls/Widget.ascx");
            Submit_Remove_User.ID = "Submit_Remove_User";
            Submit_Remove_User.submittable = true;
            Submit_Remove_User.name = "verstuur";
            Submit_Remove_User.submit_function = DeleteUser;
            Remove_User_UP.ContentTemplateContainer.Controls.Add(Submit_Remove_User);
        }

        public void MakeApparaat(object sender, EventArgs e)
        {
            TextBox Device_name = (TextBox)input_devicename.FindControl("TextInput");
            string Device_type = input_devicetype_list.SelectedValue;
            string Device_pin = input_devicepin_list.SelectedValue;

            MySqlCommand query_addapp = new MySqlCommand("INSERT INTO apparaat (`TypeID`, `naam`) SELECT typeID, :naam FROM apparaattype WHERE `type` = :type");
            query_addapp.Parameters.Add("type", Device_type);
            query_addapp.Parameters.Add("naam", Device_name.Text);
            if(!global.ExecuteChanger(query_addapp, out string addapp_error))
            {
                if (addapp_error.Contains("Duplicate entry"))
                {
                    output.Text = "apparaatnaam bestaat al";
                }
                else
                {
                    global.generic_QueryErrorHandler(query_addapp, addapp_error);
                }
            }
            else
            { 
                MySqlCommand query_addpin = new MySqlCommand("INSERT INTO pin (`apparaatID`, `pinnr`) SELECT APPARAATID, :pinnr FROM apparaat WHERE naam = :naam");
                query_addpin.Parameters.Add("naam", Device_name.Text);
                query_addpin.Parameters.Add("pinnr", Device_pin);
                if (!global.ExecuteChanger(query_addpin, out string query_addpin_error))
                {
                    /* do something with the error */
                    global.generic_QueryErrorHandler(query_addpin, query_addpin_error);
                }
                else
                {
                    output.Text = string.Format("Apparaat: {0} toegevoegd", Device_name.Text);
                    Response.Redirect(Request.Url.AbsolutePath, true);
                    Page.ClientScript.RegisterStartupScript(
                        GetType(),
                        "show_output",
                        "OpenUpdater();",
                        true);
                }
            }
        }

        public void RemoveDevice(object sender, EventArgs e)
        {
            List<string> devices = new List<string>();
            List<string> checked_devices = new List<string>();
            List<string> RemovedDevices = new List<string>();
            foreach (Control con in DeleteDevice_UP.ContentTemplateContainer.Controls)
            {
                if (con.GetType() == LoadControl("~/UserControls/Widget.ascx").GetType())
                {
                    Widget wid = (Widget)con;
                    if (wid.name != "verstuur")
                    {
                        devices.Add(wid.name);
                        CheckBox ch_box = (CheckBox)wid.FindControl("Toggle_Checkbox");
                        if (ch_box.Checked)
                        {
                            checked_devices.Add(wid.name);
                        }
                    }
                }
            }

            
            foreach (string device in checked_devices)
            {
                using (MySqlCommand remove_device = new MySqlCommand("DELETE FROM apparaat WHERE naam = :naam"))
                {
                    remove_device.Parameters.Add("naam", device);
                    if (!global.ExecuteChanger(remove_device, out string remove_device_error))
                    {
                        /* do something with the error */
                        global.generic_QueryErrorHandler(remove_device, remove_device_error);
                    }
                    else
                    {
                        RemovedDevices.Add(device);
                        devices.Remove(device);
                    }
                }
            }

            output.Text = "Verwijderde apparaten: ";
            for (int i = 0; i < RemovedDevices.Count; i++)
            {
                if (i < RemovedDevices.Count - 1)
                {
                    output.Text += RemovedDevices[i] + ", ";
                }
                else
                {
                    output.Text += RemovedDevices[i] + ".";
                }
            }
            
            DeleteDevice_UP.ContentTemplateContainer.Controls.Clear();
            foreach (string device in devices)
            {
                Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                widget.name = device;
                widget.comment = string.Format("Dit is apparaat: {0}", device);
                widget.toggle = true;
                widget.ID = device;
                DeleteDevice_UP.ContentTemplateContainer.Controls.Add(widget);
            }
            Widget Submit_Remove_Device = (Widget)LoadControl("~/UserControls/Widget.ascx");
            Submit_Remove_Device.ID = "Submit_Remove_Device";
            Submit_Remove_Device.submittable = true;
            Submit_Remove_Device.name = "verstuur";
            Submit_Remove_Device.submit_function = RemoveDevice;
            DeleteDevice_UP.ContentTemplateContainer.Controls.Add(Submit_Remove_Device);
        }

        public void MakeGroup(object sender, EventArgs e)
        {
            TextBox GroupName = (TextBox)input_groupname.FindControl("TextInput");
            List<string> users = new List<string>();
            List<string> devices = new List<string>();
            foreach(Control con in InsertUsersOverlay.Content.Controls)
            {
                Widget wid = (Widget)con;
                CheckBox ch_box = (CheckBox)wid.FindControl("Toggle_Checkbox");
                if (ch_box.Checked)
                {
                    users.Add(wid.name);
                }
            }
            foreach (Control con in InsertDevicesOverlay.Content.Controls)
            {
                Widget wid = (Widget)con;
                CheckBox ch_box = (CheckBox)wid.FindControl("Toggle_Checkbox");
                if (ch_box.Checked)
                {
                    devices.Add(wid.name);
                }
            }

            bool finished = true;
            MySqlCommand create_group = new MySqlCommand("INSERT INTO `group` (`groepnaam`) VALUES (:gpnaam)");
            create_group.Parameters.Add("gpnaam", GroupName.Text);
            if (!global.ExecuteChanger(create_group, out string create_group_error))
            {
                if (create_group_error.Contains("Duplicate entry"))
                {
                    output.Text = "groepnaam bestaat al";
                }
                else
                {
                    /* do something with the error */
                    global.generic_QueryErrorHandler(create_group, create_group_error);
                }
                finished = false;
            }
            else
            {
                foreach (string user in users)
                {
                    MySqlCommand set_users_ingroup = new MySqlCommand("INSERT INTO neemtdeelaan(`USERID`, `GROUPID`) SELECT users.id, `group`.GROUPID FROM users, `group` WHERE groepnaam = :gpnaam AND users.username = :username");
                    set_users_ingroup.Parameters.Add("username", user);
                    set_users_ingroup.Parameters.Add("gpnaam", GroupName.Text);
                    if (!global.ExecuteChanger(set_users_ingroup, out string set_users_error)){
                        finished = false;
                        /* do something with the error */
                        global.generic_QueryErrorHandler(set_users_ingroup, set_users_error);
                    }
                }
                foreach (string device in devices)
                {
                    MySqlCommand set_devices_access = new MySqlCommand("INSERT INTO heefttoegangtot SELECT `group`.GROUPID, `apparaat`.apparaatid, :bit FROM `group`, apparaat WHERE `group`.groepnaam = :gpnaam AND apparaat.naam = :naam");
                    set_devices_access.Parameters.Add("gpnaam", GroupName.Text);
                    set_devices_access.Parameters.Add("naam", device);
                    set_devices_access.Parameters.Add("bit", true);
                    if (!global.ExecuteChanger(set_devices_access, out string set_devices_error))
                    {
                        finished = false;
                        /* do something with the error */
                        global.generic_QueryErrorHandler(set_devices_access, set_devices_error);
                    }
                }
            }
            if (finished)
            {
                output.Text = string.Format("Groep: {0} toegevoegd", GroupName.Text);
                Response.Redirect(Request.Url.AbsolutePath, true);
                Page.ClientScript.RegisterStartupScript(
                    GetType(),
                    "show_output",
                    "OpenUpdater();",
                    true);
            }
        }

        public void RemoveGroup(object sender, EventArgs e)
        {
            List<string> groups = new List<string>();
            List<string> checked_groups = new List<string>();
            List<string> removed_groups = new List<string>();

            foreach (Control con in DeleteGroup_UP.ContentTemplateContainer.Controls)
            {
                if (con.GetType() == LoadControl("~/UserControls/Widget.ascx").GetType())
                {
                    Widget wid = (Widget)con;
                    if (wid.name != "verstuur")
                    {
                        groups.Add(wid.name);
                        CheckBox ch_box = (CheckBox)wid.FindControl("Toggle_Checkbox");
                        if (ch_box.Checked)
                        {
                            checked_groups.Add(wid.name);
                        }
                    }
                }
            }
            foreach (string group in checked_groups)
            {
                using (MySqlCommand remove_group = new MySqlCommand("DELETE FROM `group` WHERE groepnaam = :naam"))
                {
                    remove_group.Parameters.Add("naam", group);
                    if (!global.ExecuteChanger(remove_group, out string remove_device_error))
                    {
                        /* do something with the error */
                        global.generic_QueryErrorHandler(remove_group, remove_device_error);
                    }
                    else
                    {
                        removed_groups.Add(group);
                        groups.Remove(group);
                    }
                }
            }

            output.Text = "Verwijderde groepen: ";
            for (int i = 0; i < removed_groups.Count; i++)
            {
                if (i < removed_groups.Count - 1)
                {
                    output.Text += removed_groups[i] + ", ";
                }
                else
                {
                    output.Text += removed_groups[i] + ".";
                }
            }

            DeleteGroup_UP.ContentTemplateContainer.Controls.Clear();
            foreach (string group in groups)
            {
                Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                widget.name = group;
                widget.comment = string.Format("Dit is groep: {0}", group);
                widget.toggle = true;
                widget.ID = group;
                DeleteGroup_UP.ContentTemplateContainer.Controls.Add(widget);
            }
            Widget Submit_Remove_Group = (Widget)LoadControl("~/UserControls/Widget.ascx");
            Submit_Remove_Group.ID = "Submit_Remove_Group";
            Submit_Remove_Group.submittable = true;
            Submit_Remove_Group.name = "verstuur";
            Submit_Remove_Group.submit_function = RemoveGroup;
            DeleteGroup_UP.ContentTemplateContainer.Controls.Add(Submit_Remove_Group);
        }

        public void ModifyGroup(object sender, EventArgs e)
        {
            string selected_group = GroupDDlist.SelectedValue;
            MySqlCommand delete_group = new MySqlCommand("DELETE FROM `group` WHERE groepnaam = :gpnaam");
            delete_group.Parameters.Add("gpnaam", selected_group);
            bool finished = true;
            if (!global.ExecuteChanger(delete_group, out string delete_group_error))
            {
                /* do something with the error */
                global.generic_QueryErrorHandler(delete_group, delete_group_error);
            }
            else
            {
                List<string> users = new List<string>();
                List<string> devices = new List<string>();
                foreach (Control con in modifyUsers_UP.ContentTemplateContainer.Controls)
                {
                    if (con.GetType() == LoadControl("~/UserControls/Widget.ascx").GetType())
                    {
                        Widget wid = (Widget)con;
                        CheckBox ch_box = (CheckBox)wid.FindControl("Toggle_Checkbox");
                        if (ch_box.Checked)
                        {
                            users.Add(wid.name);
                        }
                    }
                }
                foreach (Control con in modifyDevices_UP.ContentTemplateContainer.Controls)
                {
                    if (con.GetType() == LoadControl("~/UserControls/Widget.ascx").GetType())
                    {
                        Widget wid = (Widget)con;
                        CheckBox ch_box = (CheckBox)wid.FindControl("Toggle_Checkbox");
                        if (ch_box.Checked)
                        {
                            devices.Add(wid.name);
                        }
                    }
                }
                
                MySqlCommand create_group = new MySqlCommand("INSERT INTO `group` (`groepnaam`) VALUES (:gpnaam)");
                create_group.Parameters.Add("gpnaam", GroupDDlist.SelectedValue);
                if (!global.ExecuteChanger(create_group, out string create_group_error))
                {
                    if (create_group_error.Contains("Duplicate entry"))
                    {
                        output.Text = "groepnaam bestaat al";
                    }
                    else
                    {
                        /* do something with the error */
                        global.generic_QueryErrorHandler(create_group, create_group_error);
                    }
                    finished = false;
                }
                else
                {
                    foreach (string user in users)
                    {
                        MySqlCommand set_users_ingroup = new MySqlCommand("INSERT INTO neemtdeelaan(`USERID`, `GROUPID`) SELECT users.id, `group`.GROUPID FROM users, `group` WHERE groepnaam = :gpnaam AND users.username = :username");
                        set_users_ingroup.Parameters.Add("username", user);
                        set_users_ingroup.Parameters.Add("gpnaam", GroupDDlist.SelectedValue);
                        if (!global.ExecuteChanger(set_users_ingroup, out string set_users_error))
                        {
                            finished = false;
                            /* do something with the error */
                            global.generic_QueryErrorHandler(set_users_ingroup, set_users_error);
                        }
                    }
                    foreach (string device in devices)
                    {
                        MySqlCommand set_devices_access = new MySqlCommand("INSERT INTO heefttoegangtot SELECT `group`.GROUPID, `apparaat`.apparaatid, :bit FROM `group`, apparaat WHERE `group`.groepnaam = :gpnaam AND apparaat.naam = :naam");
                        set_devices_access.Parameters.Add("gpnaam", GroupDDlist.SelectedValue);
                        set_devices_access.Parameters.Add("naam", device);
                        set_devices_access.Parameters.Add("bit", true);
                        if (!global.ExecuteChanger(set_devices_access, out string set_devices_error))
                        {
                            finished = false;
                            /* do something with the error */
                            global.generic_QueryErrorHandler(set_devices_access, set_devices_error);
                        }
                    }
                }
            }
            if (finished)
            {
                output.Text = string.Format("Groep: {0} Veranderd", GroupDDlist.SelectedValue);
                ModifyGroup_Selected(GroupDDlist.SelectedValue);
            }
        }

        protected void changeActiveGroup(object sender, EventArgs e)
        {
            ModifyGroup_Selected(GroupDDlist.SelectedValue);
        }

        protected void ModifyGroup_Selected(string group)
        {
            MySqlCommand changeActiveGroup_query = new MySqlCommand("SELECT username, naam FROM `group` AS g " +
                "LEFT JOIN neemtdeelaan AS NDA ON NDA.GROUPID = g.GROUPID " +
                "LEFT JOIN heefttoegangtot AS HTT ON HTT.GROUPID = g.GROUPID " +
                "LEFT JOIN users AS U ON U.id = NDA.USERID " +
                "LEFT JOIN apparaat AS A ON A.APPARAATID = HTT.APPARAATID " +
                "WHERE groepnaam = :gpnaam");
            changeActiveGroup_query.Parameters.Add("gpnaam", group);
            List<List<string>> changeActiveGroup_result = global.ExecuteReader(changeActiveGroup_query, out string changeActiveGroup_error);
            if (changeActiveGroup_error != "")
            {
                global.generic_QueryErrorHandler(changeActiveGroup_query, changeActiveGroup_error);
                /* do something with the error */
            }
            else
            {
                List<string> users = new List<string>();
                List<string> devices = new List<string>();
                foreach (List<string> row in changeActiveGroup_result)
                {
                    users.Add(row[0]);
                    devices.Add(row[1]);
                }

                // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/linq/walkthrough-writing-queries-linq
                // above is about queries on C# list objects
                IEnumerable<string> usersdistinct = users.Distinct();
                IEnumerable<string> devicesdistinct = devices.Distinct();
                foreach (Control con in modifyUsers_UP.ContentTemplateContainer.Controls)
                {
                    if (con.GetType() == LoadControl("~/UserControls/Widget.ascx").GetType())
                    {
                        Widget wid = (Widget)con;
                        CheckBox ch_box = (CheckBox)wid.FindControl("Toggle_Checkbox");
                        if (usersdistinct.Contains(wid.name))
                        {
                            ch_box.Checked = true;
                        }
                        else
                        {
                            ch_box.Checked = false;
                        }
                    }
                }
                foreach (Control con in modifyDevices_UP.ContentTemplateContainer.Controls)
                {
                    if (con.GetType() == LoadControl("~/UserControls/Widget.ascx").GetType())
                    {
                        Widget wid = (Widget)con;
                        CheckBox ch_box = (CheckBox)wid.FindControl("Toggle_Checkbox");
                        if (devicesdistinct.Contains(wid.name))
                        {
                            ch_box.Checked = true;
                        }
                        else
                        {
                            ch_box.Checked = false;
                        }
                    }
                }
            }
        }

        protected void ReloadPage(object sender, EventArgs e)
        {
            output.Text = "Gebruiker aangemaakt";
            Page.ClientScript.RegisterStartupScript(
                        GetType(),
                        "show_output",
                        "OpenUpdater();",
                        true);
            Response.Redirect(Request.Url.AbsolutePath, true);
        }

        protected void btn_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/default.aspx");
        }
    }
}