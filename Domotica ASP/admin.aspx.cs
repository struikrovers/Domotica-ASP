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
                    Remove_User_UP.Attributes["style"] =
                    "display: grid; " +
                    "grid-template-columns: repeat(auto-fit, minmax(6em, -webkit-max-content)); " +
                    "grid-template-columns: repeat(auto-fit, minmax(6em, max-content)); " +
                    "justify-content: center; " +
                    "grid-gap: 0.5em; " +
                    "margin-left: -2px; " +
                    "margin-bottom: 0.7em";

                    DeleteDevice_UP.Attributes["style"] =
                    "display: grid; " +
                    "grid-template-columns: repeat(auto-fit, minmax(6em, -webkit-max-content)); " +
                    "grid-template-columns: repeat(auto-fit, minmax(6em, max-content)); " +
                    "justify-content: center; " +
                    "grid-gap: 0.5em; " +
                    "margin-left: -2px; " +
                    "margin-bottom: 0.7em";

                    DeleteGroup_UP.Attributes["style"] =
                    "display: grid; " +
                    "grid-template-columns: repeat(auto-fit, minmax(6em, -webkit-max-content)); " +
                    "grid-template-columns: repeat(auto-fit, minmax(6em, max-content)); " +
                    "justify-content: center; " +
                    "grid-gap: 0.5em; " +
                    "margin-left: -2px; " +
                    "margin-bottom: 0.7em";

                    MySqlCommand userQuery = new MySqlCommand("SELECT username FROM users");

                    List<List<string>> GebruikersTabel = global.ExecuteReader(userQuery, out string error_gebruiker, out bool userErrorYes);
                    if (userErrorYes)
                    {
                        Label label = new Label();
                        label.Text = error_gebruiker;
                        grid_parent.Controls.Add(label);
                    }
                    else
                    {
                        foreach (List<string> row in GebruikersTabel)
                        {
                            if (row[0] != Membership.GetUser().UserName)
                            {
                                Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                                widget.name = row[0];
                                widget.comment = string.Format("Dit is het account van: {0}", row[0]);
                                widget.toggle = true;
                                widget.ID = row[0];
                                Remove_User_UP.ContentTemplateContainer.Controls.Add(widget);
                            }
                            Widget widget2 = (Widget)LoadControl("~/UserControls/Widget.ascx");
                            widget2.name = row[0];
                            widget2.comment = string.Format("Dit is het account van: {0}", row[0]);
                            widget2.toggle = true;
                            widget2.ID = row[0];
                            InsertUsersOverlay.Content.Controls.Add(widget2);
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
                    List<List<string>> bezette_pins = global.ExecuteReader(get_busy_pins, out string error_get_pins, out bool error_pin_ind);
                    if (error_pin_ind)
                    {

                    }
                    else
                    {
                        global.init_pinnr();
                        foreach(string pinnr in global.pinnr)
                        {
                            if (bezette_pins.Count != 0)
                            {
                                if (!bezette_pins[0].Contains(pinnr))
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
                    List<List<string>> getTypes_result = global.ExecuteReader(getTypes, out string getTypes_error, out bool getTypes_error_ind);
                    if (getTypes_error_ind)
                    {

                    }
                    else
                    {
                        foreach(List<string> type in getTypes_result)
                        {
                            input_devicetype_list.Items.Add(type[0]);
                        }
                    }

                    MySqlCommand DeviceQuery = new MySqlCommand("SELECT naam FROM apparaat");
                    List<List<string>> DeviceTable = global.ExecuteReader(DeviceQuery, out string DeviceQueryError, out bool DeviceQueryErrorInd);
                    if (DeviceQueryErrorInd)
                    {
                        /* do something with the error */
                    }
                    else
                    {
                        DeleteDevice_UP.ContentTemplateContainer.Controls.Clear();
                        foreach (List<string> row in DeviceTable)
                        {
                            Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                            widget.name = row[0];
                            widget.comment = string.Format("Dit is apparaat: {0}", row[0]);
                            widget.toggle = true;
                            widget.ID = row[0];
                            DeleteDevice_UP.ContentTemplateContainer.Controls.Add(widget);
                            Widget widget2 = (Widget)LoadControl("~/UserControls/Widget.ascx");
                            widget2.name = row[0];
                            widget2.comment = string.Format("Dit is apparaat: {0}", row[0]);
                            widget2.toggle = true;
                            widget2.ID = row[0];
                            InsertDevicesOverlay.Content.Controls.Add(widget2);
                            Widget widget3 = (Widget)LoadControl("~/UserControls/Widget.ascx");
                            widget3.name = row[0];
                            widget3.comment = string.Format("Dit is apparaat: {0}", row[0]);
                            widget3.toggle = true;
                            widget3.ID = row[0];
                            modifyDevices_UP.ContentTemplateContainer.Controls.Add(widget3);
                        }
                        Widget Submit_Remove_Device = (Widget)LoadControl("~/UserControls/Widget.ascx");
                        Submit_Remove_Device.ID = "Submit_Remove_User";
                        Submit_Remove_Device.submittable = true;
                        Submit_Remove_Device.name = "verstuur";
                        Submit_Remove_Device.submit_function = RemoveDevice;
                        DeleteDevice_UP.ContentTemplateContainer.Controls.Add(Submit_Remove_Device);
                    }

                    MySqlCommand GroupQuery = new MySqlCommand("SELECT groepnaam FROM `group`");
                    List<List<string>> GroupTable = global.ExecuteReader(GroupQuery, out string GroupQueryError, out bool GroupQueryErrorInd);
                    if (GroupQueryErrorInd)
                    {
                        /* do something with the error */
                    }
                    else
                    {
                        DeleteDevice_UP.ContentTemplateContainer.Controls.Clear();
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
                    }


                    /* template widget
                    Widget SubmitWidget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                    SubmitWidget.ID = "SubmitWidget";
                    SubmitWidget.submittable = true;
                    SubmitWidget.name = "verstuur";
                    Remove_User.Content.Controls.Add(SubmitWidget);
                    */
                    Submit_ManageGroupOID.submit_function = ModifyGroup;

                    Submit_AddGroupOID.submit_function = MakeGroup;
                }
                else
                {

                }
            }
        }

        public void DeleteUser(object sender, EventArgs e)
        {

            MySqlCommand userQuery = new MySqlCommand("SELECT username FROM users WHERE username != :gbnaam");
            userQuery.Parameters.Add("gbnaam", Membership.GetUser().UserName);
            
            List<List<string>> GebruikersTabel = global.ExecuteReader(userQuery, out string error_gebruiker, out bool userErrorYes);

            if (userErrorYes)
            {
                Label1.Text = error_gebruiker;
            }
            else
            {
                List<string> removed_users = new List<string>();
                foreach (List<string> row in GebruikersTabel)
                {
                    //Label1.Text += GebruikersTabel[i][0];
                    if (Remove_User_UP.ContentTemplateContainer.FindControl(row[0]) != null)
                    {
                        Widget removeUser = (Widget)Remove_User_UP.ContentTemplateContainer.FindControl(row[0]);
                        CheckBox removable_user = (CheckBox)removeUser.FindControl("Toggle_Checkbox");
                        if (removable_user.Checked)
                        {
                            using (MySqlCommand remove_user = new MySqlCommand("DELETE FROM users WHERE username = :gbnaam"))
                            {
                                remove_user.Parameters.Add("gbnaam", row[0]);
                                if (!global.ExecuteChanger(remove_user, out string remove_user_error))
                                {
                                    /* do something with the error */
                                    output.Text = remove_user_error;
                                }
                                else
                                {
                                    removed_users.Add(row[0]);
                                }
                            }
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

            }

            MySqlCommand newUserQuery = new MySqlCommand("SELECT username FROM users WHERE username != :gbnaam");
            newUserQuery.Parameters.Add("gbnaam", Membership.GetUser().UserName);
            List<List<string>> newUserTable = global.ExecuteReader(newUserQuery, out string newUserTable_error, out bool newUserTable_errorInd);
            if (newUserTable_errorInd)
            {
                /* do something with the error */
            }
            else
            {
                Remove_User_UP.ContentTemplateContainer.Controls.Clear();
                foreach (List<string> row in newUserTable)
                {
                    Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                    widget.name = row[0];
                    widget.comment = string.Format("Dit is het account van: {0}", row[0]);
                    widget.toggle = true;
                    widget.ID = row[0];
                    Remove_User_UP.ContentTemplateContainer.Controls.Add(widget);
                }
                Widget Submit_Remove_User = (Widget)LoadControl("~/UserControls/Widget.ascx");
                Submit_Remove_User.ID = "Submit_Remove_User";
                Submit_Remove_User.submittable = true;
                Submit_Remove_User.name = "verstuur";
                Submit_Remove_User.submit_function = DeleteUser;
                Remove_User_UP.ContentTemplateContainer.Controls.Add(Submit_Remove_User);
            }
        }

        public void MakeApparaat(object sender, EventArgs e)
        {
            TextBox Device_name = (TextBox)input_devicename.FindControl("TextInput");
            string Device_type = input_devicetype_list.SelectedValue;
            string Device_pin = input_devicepin_list.SelectedValue;

            MySqlCommand query_getTypeID = new MySqlCommand("SELECT TypeID FROM apparaattype WHERE `type` = :type");
            query_getTypeID.Parameters.Add("type", Device_type);
            List<List<string>> getTypeID_result = global.ExecuteReader(query_getTypeID, out string getTypeID_error, out bool getTypeID_errorInd);
            if (getTypeID_errorInd)
            {
                /* do something with the error */
            }
            else
            {
                MySqlCommand query_addapp = new MySqlCommand("INSERT INTO apparaat (`TypeID`, `naam`) VALUES (:type, :naam)");
                query_addapp.Parameters.Add("type", getTypeID_result[0][0]);
                query_addapp.Parameters.Add("naam", Device_name.Text);
                if(!global.ExecuteChanger(query_addapp, out string error5))
                {
                    if (error5.Contains("Duplicate entry"))
                    {
                        output.Text = "apparaatnaam bestaat al";
                    }
                    else
                    {
                        output.Text = error5;
                    }
                }
                else
                { 
                    MySqlCommand query_appid = new MySqlCommand("SELECT apparaatID FROM apparaat WHERE naam = :naam");
                    query_appid.Parameters.Add("naam", Device_name.Text);
                    List<List<string>> appid = global.ExecuteReader(query_appid, out string error6, out bool errorInd6);
                    if (errorInd6)
                    {
                        /* do something with the error */
                        output.Text = error6;
                    }
                    else
                    {
                        MySqlCommand query_addpin = new MySqlCommand("INSERT INTO pin VALUES (:appID, :pinnr)");
                        query_addpin.Parameters.Add("appID", appid[0][0]);
                        query_addpin.Parameters.Add("pinnr", Device_pin);
                        if (!global.ExecuteChanger(query_addpin, out string error7))
                        {
                            /* do something with the error */
                            output.Text = error7;
                        }
                        else
                        {
                            output.Text = string.Format("Apparaat: {0} toegevoegd", Device_name.Text);
                            Response.Redirect(Request.Url.AbsolutePath, true);
                        }
                    }
                }
                
            }
        }

        public void RemoveDevice(object sender, EventArgs e)
        {

            MySqlCommand ApparaatQuery = new MySqlCommand("SELECT naam FROM apparaat");

            List<List<string>> ApparaatTable = global.ExecuteReader(ApparaatQuery, out string error_apparaat, out bool apparaatErrorInd);

            if (apparaatErrorInd)
            {
                Label1.Text = error_apparaat;
            }
            else
            {
                List<string> RemovedDevices = new List<string>();
                foreach (List<string> row in ApparaatTable)
                {
                    //Label1.Text += GebruikersTabel[i][0];
                    if (DeleteDevice_UP.ContentTemplateContainer.FindControl(row[0]) != null)
                    {
                        Widget removeDevice = (Widget)DeleteDevice_UP.ContentTemplateContainer.FindControl(row[0]);
                        CheckBox removable_device = (CheckBox)removeDevice.FindControl("Toggle_Checkbox");
                        if (removable_device.Checked)
                        {
                            using (MySqlCommand remove_device = new MySqlCommand("DELETE FROM apparaat WHERE naam = :naam"))
                            {
                                remove_device.Parameters.Add("naam", row[0]);
                                if (!global.ExecuteChanger(remove_device, out string remove_device_error))
                                {
                                    /* do something with the error */
                                    output.Text = remove_device_error;
                                }
                                else
                                {
                                    RemovedDevices.Add(row[0]);
                                }
                            }
                        }
                    }
                }

                output.Text = "Verwijderde gebruikers: ";
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

            }

            MySqlCommand newDeviceQuery = new MySqlCommand("SELECT naam FROM apparaat");
            List<List<string>> newDeviceTable = global.ExecuteReader(newDeviceQuery, out string newDeviceQueryError, out bool newDeviceQueryErrorInd);
            if (newDeviceQueryErrorInd)
            {
                /* do something with the error */
            }
            else
            {
                DeleteDevice_UP.ContentTemplateContainer.Controls.Clear();
                foreach (List<string> row in newDeviceTable)
                {
                    Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                    widget.name = row[0];
                    widget.comment = string.Format("Dit is apparaat: {0}", row[0]);
                    widget.toggle = true;
                    widget.ID = row[0];
                    DeleteDevice_UP.ContentTemplateContainer.Controls.Add(widget);
                }
                Widget Submit_Remove_Device = (Widget)LoadControl("~/UserControls/Widget.ascx");
                Submit_Remove_Device.ID = "Submit_Remove_Device";
                Submit_Remove_Device.submittable = true;
                Submit_Remove_Device.name = "verstuur";
                Submit_Remove_Device.submit_function = RemoveDevice;
                DeleteDevice_UP.ContentTemplateContainer.Controls.Add(Submit_Remove_Device);
            }
        }

        public void MakeGroup(object sender, EventArgs e)
        {
            TextBox GroupName = (TextBox)input_groupname.FindControl("TextInput");
            List<string> users = new List<string>();
            List<string> devices = new List<string>();
            foreach(Control con in InsertUsersOverlay.Content.Controls)
            {
                Widget wid = (Widget)con;
                if (wid.toggle)
                {
                    users.Add(wid.name);
                }
            }
            foreach (Control con in InsertDevicesOverlay.Content.Controls)
            {
                Widget wid = (Widget)con;
                if (wid.toggle)
                {
                    devices.Add(wid.name);
                }
            }

            bool finished = true;
            string error = "";
            MySqlCommand create_group = new MySqlCommand("INSERT INTO `group` (`groepnaam`) VALUES (:gpnaam)");
            create_group.Parameters.Add("gpnaam", GroupName.Text);
            if (global.ExecuteChanger(create_group, out string create_group_error))
            {
                MySqlCommand get_GPid = new MySqlCommand("SELECT GROUPID FROM `group` WHERE groepnaam = :gpnaam");
                get_GPid.Parameters.Add("gpnaam", GroupName.Text);
                List<List<string>> GPid = global.ExecuteReader(get_GPid, out string get_GPnaam_error, out bool get_GPnaamInd);
                if (get_GPnaamInd) {
                    finished = false;
                    error = get_GPnaam_error;
                }
                else
                {
                    foreach (string user in users)
                    {
                        MySqlCommand get_userID = new MySqlCommand("SELECT id FROM users WHERE username = :naam");
                        get_userID.Parameters.Add("naam", user);
                        List<List<string>> UserID = global.ExecuteReader(get_userID, out string get_userID_error, out bool get_userIDInd);
                        if (get_userIDInd) {
                            finished = false;
                            error = get_userID_error;
                        }
                        else
                        {
                            MySqlCommand set_users_ingroup = new MySqlCommand("INSERT INTO neemtdeelaan(`USERID`, `GROUPID`) VALUES(:userid, :gpid)");
                            set_users_ingroup.Parameters.Add("userid", UserID[0][0]);
                            set_users_ingroup.Parameters.Add("gpid", GPid[0][0]);
                            if (!global.ExecuteChanger(set_users_ingroup, out string set_users_error)){
                                finished = false;
                                error = set_users_error;
                            }
                        }
                    }
                    foreach (string device in devices)
                    {
                        MySqlCommand get_appID = new MySqlCommand("SELECT APPARAATID FROM apparaat WHERE naam = :naam");
                        get_appID.Parameters.Add("naam", device);
                        List<List<string>> appID = global.ExecuteReader(get_appID, out string get_appID_error, out bool get_appIDInd);
                        if (get_appIDInd) {
                            finished = false;
                            error = get_appID_error;
                        }
                        else
                        {
                            MySqlCommand set_devices_access = new MySqlCommand("INSERT INTO heefttoegangtot VALUES(:gpid, :appid, :bit)");
                            set_devices_access.Parameters.Add("gpid", GPid[0][0]);
                            set_devices_access.Parameters.Add("appid", appID[0][0]);
                            set_devices_access.Parameters.Add("bit", true);
                            if (!global.ExecuteChanger(set_devices_access, out string set_devices_error))
                            {
                                finished = false;
                                error = set_devices_error;
                            }
                        }
                    }
                }
            }
            else
            {
                if (create_group_error.Contains("Duplicate entry"))
                {
                    error = "apparaatnaam bestaat al";
                }
                else
                {
                    error = create_group_error;
                }
                finished = false;
            }
            if (finished)
            {
                output.Text = string.Format("Groep: {0} toegevoegd", GroupName.Text);
                Response.Redirect(Request.Url.AbsolutePath, true);
            }
            else
            {
                output.Text = error;
            }
        }

        public void RemoveGroup(object sender, EventArgs e)
        {

            MySqlCommand GroupQuery = new MySqlCommand("SELECT groepnaam FROM `group`");

            List<List<string>> GroupTable = global.ExecuteReader(GroupQuery, out string error_GroupQuery, out bool GroupQueryErrorInd);

            if (GroupQueryErrorInd)
            {
                Label1.Text = error_GroupQuery;
            }
            else
            {
                List<string> RemovedGroup = new List<string>();
                foreach (List<string> row in GroupTable)
                {
                    //Label1.Text += GebruikersTabel[i][0];
                    if (DeleteGroup_UP.ContentTemplateContainer.FindControl(row[0]) != null)
                    {
                        Widget removeGroup = (Widget)DeleteGroup_UP.ContentTemplateContainer.FindControl(row[0]);
                        CheckBox removable_Group = (CheckBox)removeGroup.FindControl("Toggle_Checkbox");
                        if (removable_Group.Checked)
                        {
                            using (MySqlCommand remove_group = new MySqlCommand("DELETE FROM `group` WHERE groepnaam = :naam"))
                            {
                                remove_group.Parameters.Add("naam", row[0]);
                                if (!global.ExecuteChanger(remove_group, out string remove_device_error))
                                {
                                    /* do something with the error */
                                    output.Text = remove_device_error;
                                }
                                else
                                {
                                    RemovedGroup.Add(row[0]);
                                }
                            }
                        }
                    }
                }

                output.Text = "Verwijderde groepen: ";
                for (int i = 0; i < RemovedGroup.Count; i++)
                {
                    if (i < RemovedGroup.Count - 1)
                    {
                        output.Text += RemovedGroup[i] + ", ";
                    }
                    else
                    {
                        output.Text += RemovedGroup[i] + ".";
                    }
                }

            }

            MySqlCommand newGroupQuery = new MySqlCommand("SELECT groepnaam FROM `group`");
            List<List<string>> newGroupTable = global.ExecuteReader(newGroupQuery, out string newGroupTableError, out bool newGroupTableErrorInd);
            if (newGroupTableErrorInd)
            {
                /* do something with the error */
            }
            else
            {
                DeleteGroup_UP.ContentTemplateContainer.Controls.Clear();
                foreach (List<string> row in newGroupTable)
                {
                    Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                    widget.name = row[0];
                    widget.comment = string.Format("Dit is groep: {0}", row[0]);
                    widget.toggle = true;
                    widget.ID = row[0];
                    DeleteGroup_UP.ContentTemplateContainer.Controls.Add(widget);
                }
                Widget Submit_Remove_Group = (Widget)LoadControl("~/UserControls/Widget.ascx");
                Submit_Remove_Group.ID = "Submit_Remove_Group";
                Submit_Remove_Group.submittable = true;
                Submit_Remove_Group.name = "verstuur";
                Submit_Remove_Group.submit_function = RemoveGroup;
                DeleteGroup_UP.ContentTemplateContainer.Controls.Add(Submit_Remove_Group);
            }
        }

        public void ModifyGroup(object sender, EventArgs e)
        {
            string selected_group = GroupDDlist.SelectedValue;
            MySqlCommand delete_group = new MySqlCommand("DELETE FROM `group` WHERE groepnaam = :gpnaam");
            delete_group.Parameters.Add("gpnaam", selected_group);
            string error = "";
            bool finished = true;
            if (global.ExecuteChanger(delete_group, out string delete_group_error))
            {
                List<string> users = new List<string>();
                List<string> devices = new List<string>();
                foreach (Control con in modifyUsers.Content.Controls)
                {
                    Widget wid = (Widget)con;
                    if (wid.toggle)
                    {
                        users.Add(wid.name);
                    }
                }
                foreach (Control con in modifyDevices.Content.Controls)
                {
                    Widget wid = (Widget)con;
                    if (wid.toggle)
                    {
                        devices.Add(wid.name);
                    }
                }
                
                MySqlCommand create_group = new MySqlCommand("INSERT INTO `group` (`groepnaam`) VALUES (:gpnaam)");
                create_group.Parameters.Add("gpnaam", GroupDDlist.SelectedValue);
                if (global.ExecuteChanger(create_group, out string create_group_error))
                {
                    MySqlCommand get_GPid = new MySqlCommand("SELECT GROUPID FROM `group` WHERE groepnaam = :gpnaam");
                    get_GPid.Parameters.Add("gpnaam", GroupDDlist.SelectedValue);
                    List<List<string>> GPid = global.ExecuteReader(get_GPid, out string get_GPnaam_error, out bool get_GPnaamInd);
                    if (get_GPnaamInd)
                    {
                        finished = false;
                        error = get_GPnaam_error;
                    }
                    else
                    {
                        foreach (string user in users)
                        {
                            MySqlCommand get_userID = new MySqlCommand("SELECT id FROM users WHERE username = :naam");
                            get_userID.Parameters.Add("naam", user);
                            List<List<string>> UserID = global.ExecuteReader(get_userID, out string get_userID_error, out bool get_userIDInd);
                            if (get_userIDInd)
                            {
                                finished = false;
                                error = get_userID_error;
                            }
                            else
                            {
                                MySqlCommand set_users_ingroup = new MySqlCommand("INSERT INTO neemtdeelaan(`USERID`, `GROUPID`) VALUES(:userid, :gpid)");
                                set_users_ingroup.Parameters.Add("userid", UserID[0][0]);
                                set_users_ingroup.Parameters.Add("gpid", GPid[0][0]);
                                if (!global.ExecuteChanger(set_users_ingroup, out string set_users_error))
                                {
                                    finished = false;
                                    error = set_users_error;
                                }
                            }
                        }
                        foreach (string device in devices)
                        {
                            MySqlCommand get_appID = new MySqlCommand("SELECT APPARAATID FROM apparaat WHERE naam = :naam");
                            get_appID.Parameters.Add("naam", device);
                            List<List<string>> appID = global.ExecuteReader(get_appID, out string get_appID_error, out bool get_appIDInd);
                            if (get_appIDInd)
                            {
                                finished = false;
                                error = get_appID_error;
                            }
                            else
                            {
                                MySqlCommand set_devices_access = new MySqlCommand("INSERT INTO heefttoegangtot VALUES(:gpid, :appid, :bit)");
                                set_devices_access.Parameters.Add("gpid", GPid[0][0]);
                                set_devices_access.Parameters.Add("appid", appID[0][0]);
                                set_devices_access.Parameters.Add("bit", true);
                                if (!global.ExecuteChanger(set_devices_access, out string set_devices_error))
                                {
                                    finished = false;
                                    error = set_devices_error;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (create_group_error.Contains("Duplicate entry"))
                    {
                        error = "apparaatnaam bestaat al";
                    }
                    else
                    {
                        error = create_group_error;
                    }
                    finished = false;
                }
            }
            else
            {
                error = delete_group_error;
            }
            if (finished)
            {
                output.Text = string.Format("Groep: {0} Veranderd", GroupDDlist.SelectedValue);
                Response.Redirect(Request.Url.AbsolutePath, true);
            }
            else
            {
                output.Text = error;
            }
        }

        protected void changeActiveGroup(object sender, EventArgs e)
        {
            Label1.Text = "test";
            //modifyUsers_UP.ContentTemplateContainer.Controls.Clear();
        }

        protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
        {
            output.Text = "Gebruiker aangemaakt";
            Page.ClientScript.RegisterStartupScript(
                        GetType(),
                        "show_output",
                        "OpenUpdater();",
                        true);
        }

        
    }
}