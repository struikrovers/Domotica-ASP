﻿using Devart.Data.MySql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotica_ASP
{
    public partial class admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
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

            /*
            MySqlCommand query5 = new MySqlCommand("INSERT INTO user (`voornaam`, `achternaam`, `gebruikersnaam`, `wachtwoord`, `email`, `toegangslevel`) VALUES ('joost', 'visserman', 'joost', :wachtwoord, 'test3@hotmail.com', '50')");
            query5.Parameters.Add("wachtwoord", SecurePasswordHasher.Hash("admin123"));
            global.ExecuteReader(query5, out string error5, out bool errorInd5);
            if (errorInd5)
            {
                Label1.Text = error5;
            }
            */
            MySqlCommand userQuery = new MySqlCommand("SELECT gebruikersnaam, voornaam, achternaam FROM user WHERE gebruikersnaam != :gbnaam");
            userQuery.Parameters.Add("gbnaam", Session["user"]);

            List<List<string>> GebruikersTabel = global.ExecuteReader(userQuery, out string error_gebruiker, out bool userErrorYes);
            // List<List<string>> GroepenTabel = global.ExecuteReader(groupQuery, out string error_groep, out bool groupErrorYes);
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
                    Widget widget = (Widget)LoadControl("Widget.ascx");
                    widget.name = row[0];
                    widget.comment = string.Format("Dit is het account van: {0} {1}", row[1], row[2]);
                    widget.toggle = true;
                    widget.ID = row[0];
                    Remove_User_UP.ContentTemplateContainer.Controls.Add(widget);
                }
                Widget Submit_Remove_User = (Widget)LoadControl("Widget.ascx");
                Submit_Remove_User.ID = "Submit_Remove_User";
                Submit_Remove_User.submittable = true;
                Submit_Remove_User.name = "verstuur";
                Submit_Remove_User.submit_function = DeleteUser;
                Remove_User_UP.ContentTemplateContainer.Controls.Add(Submit_Remove_User);
            }

            /* template widget
            Widget SubmitWidget = (Widget)LoadControl("Widget.ascx");
            SubmitWidget.ID = "SubmitWidget";
            SubmitWidget.submittable = true;
            SubmitWidget.name = "verstuur";
            Remove_User.Content.Controls.Add(SubmitWidget);
            */ 

            Widget Submit_DeleteGroupOID = (Widget)LoadControl("Widget.ascx");
            Submit_DeleteGroupOID.ID = " Submit_DeleteGroupOID";
            Submit_DeleteGroupOID.submittable = true;
            Submit_DeleteGroupOID.name = "verstuur";
            DeleteGroupOID.Content.Controls.Add(Submit_DeleteGroupOID);

            Widget Submit_DeleteDeviceOID = (Widget)LoadControl("Widget.ascx");
            Submit_DeleteDeviceOID.ID = "Submit_DeleteDeviceOID";
            Submit_DeleteDeviceOID.submittable = true;
            Submit_DeleteDeviceOID.name = "verstuur";
            DeleteDeviceOID.Content.Controls.Add(Submit_DeleteDeviceOID);

            Widget Submit_Add_User = (Widget)LoadControl("Widget.ascx");
            Submit_Add_User.ID = "SubmitWidget";
            Submit_Add_User.submittable = true;
            Submit_Add_User.name = "verstuur";
            Submit_Add_User.submit_function = MakeUser;
            Add_User.Content.Controls.Add(Submit_Add_User);

            Widget Submit_AddGroupOID = (Widget)LoadControl("Widget.ascx");
            Submit_AddGroupOID.ID = "Submit_AddGroupOID";
            Submit_AddGroupOID.submittable = true;
            Submit_AddGroupOID.name = "verstuur";
            AddGroupOID.Content.Controls.Add(Submit_AddGroupOID);

            Widget Submit_ManageGroupOID = (Widget)LoadControl("Widget.ascx");
            Submit_ManageGroupOID.ID = "Submit_ManageGroupOID";
            Submit_ManageGroupOID.submittable = true;
            Submit_ManageGroupOID.name = "verstuur";
            ManageGroupOID.Content.Controls.Add(Submit_ManageGroupOID);

                
            Widget Submit_AddDeviceOID = (Widget)LoadControl("Widget.ascx");
            Submit_AddDeviceOID.ID = "Submit_AddDeviceOID";
            Submit_AddDeviceOID.submittable = true;
            Submit_AddDeviceOID.name = "verstuur";
            AddDeviceOID.Content.Controls.Add(Submit_AddDeviceOID);

                

            /*PlaceHolder phInput = new PlaceHolder();
            phInput.ID = "placeholder";

            InputFields Input = (InputFields)LoadControl("InputFields.ascx");
            Input.in_type = "radio";
            Input.button_text = "submit";
            Input.ID = "InputField";

            phInput.Controls.Add(Input);

            SubmitWidget.Input = phInput;
            */

        }
        public void MakeUser(object sender, EventArgs e)
        {
            TextBox user_name = (TextBox)input_naam.FindControl("TextInput");
            TextBox user_surname = (TextBox)input_achternaam.FindControl("TextInput");
            TextBox user_username = (TextBox)input_username.FindControl("TextInput");
            TextBox user_password = (TextBox)input_password.FindControl("TextInput");
            TextBox user_email = (TextBox)input_email.FindControl("TextInput");
            CheckBox user_toeganglvl = (CheckBox)_admin.FindControl("Toggle_Checkbox");//TODO: slider

            int user_toegangswaarde = 0;
            if (user_toeganglvl.Checked)
            {
                user_toegangswaarde = 50;

            }

            MySqlCommand query_adduser = new MySqlCommand("INSERT INTO user (`voornaam`, `achternaam`, `gebruikersnaam`, `wachtwoord`, `email`, `toegangslevel`) VALUES (:voornaam, :achternaam, :gebruikersnaam, :wachtwoord, :email, :toeganglvl)");
            query_adduser.Parameters.Add("voornaam", user_name.Text);
            query_adduser.Parameters.Add("achternaam", user_surname.Text);
            query_adduser.Parameters.Add("gebruikersnaam", user_username.Text);
            query_adduser.Parameters.Add("email", user_email.Text);
            query_adduser.Parameters.Add("toeganglvl", user_toegangswaarde);
            query_adduser.Parameters.Add("wachtwoord", SecurePasswordHasher.Hash(user_password.Text));



            global.ExecuteReader(query_adduser, out string error5, out bool errorInd5);
            if (errorInd5)
            {
                if (error5.Contains("Duplicate entry"))
                {
                    output.Text = "user already exists";
                }
                else
                {
                    output.Text = error5;
                }
                /*do something with the error*/
            }
            else
            {
                output.Text = string.Format("User: {0} toegevoegd", user_username.Text);
            }
            Response.Redirect(Request.Url.AbsolutePath, true);
        }

        public void DeleteUser(object sender, EventArgs e)
        {

            MySqlCommand userQuery = new MySqlCommand("SELECT gebruikersnaam, voornaam, achternaam FROM user WHERE gebruikersnaam != :gbnaam");
            userQuery.Parameters.Add("gbnaam", Session["user"]);


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
                            using (MySqlCommand remove_user = new MySqlCommand("DELETE FROM user WHERE gebruikersnaam = :gbnaam"))
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

            MySqlCommand newUserQuery = new MySqlCommand("SELECT gebruikersnaam, voornaam, achternaam FROM user WHERE gebruikersnaam != :gbnaam");
            newUserQuery.Parameters.Add("gbnaam", Session["user"]);
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
                    Widget widget = (Widget)LoadControl("Widget.ascx");
                    widget.name = row[0];
                    widget.comment = string.Format("Dit is het account van: {0} {1}", row[1], row[2]);
                    widget.toggle = true;
                    widget.ID = row[0];
                    Remove_User_UP.ContentTemplateContainer.Controls.Add(widget);
                }
                Widget Submit_Remove_User = (Widget)LoadControl("Widget.ascx");
                Submit_Remove_User.ID = "Submit_Remove_User";
                Submit_Remove_User.submittable = true;
                Submit_Remove_User.name = "verstuur";
                Submit_Remove_User.submit_function = DeleteUser;
                Remove_User_UP.ContentTemplateContainer.Controls.Add(Submit_Remove_User);
            }
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            Label1.Text = "update!";
        }


    }
}