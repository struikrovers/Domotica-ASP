using Devart.Data.MySql;
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
            // MySqlCommand groupQuery = new MySqlCommand("SELECT GROUPID, groepsnaam FROM group");
            // groupQuery.Parameters.Add("gbnaam", Session["user"]);

            MySqlCommand query_gebruikertoevoegen = new MySqlCommand("SELECT " );


            List<List<string>> GebruikersTabel = global.ExecuteReader(userQuery, out string error_gebruiker, out bool userErrorYes);
           // List<List<string>> GroepenTabel = global.ExecuteReader(groupQuery, out string error_groep, out bool groupErrorYes);
            if (userErrorYes || userErrorYes)
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
                    Remove_User.Content.Controls.Add(widget);
                }

                /* template widget
                Widget SubmitWidget = (Widget)LoadControl("Widget.ascx");
                SubmitWidget.ID = "SubmitWidget";
                SubmitWidget.submittable = true;
                SubmitWidget.name = "verstuur";
                Remove_User.Content.Controls.Add(SubmitWidget);
                */ 

                Widget Submit_Remove_User = (Widget)LoadControl("Widget.ascx");
                Submit_Remove_User.ID = "Submit_Remove_User";
                Submit_Remove_User.submittable = true;
                Submit_Remove_User.name = "verstuur";
                Remove_User.Content.Controls.Add(Submit_Remove_User);


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

        }
        public void MakeUser(object sender, EventArgs e)
        {
            TextBox user_name = (TextBox)input_naam.FindControl("Textinput");
            TextBox user_surname = (TextBox)input_achternaam.FindControl("Textinput");
            TextBox user_username = (TextBox)input_username.FindControl("Textinput");
            TextBox user_password = (TextBox)input_password.FindControl("Textinput");
            TextBox user_email = (TextBox)input_email.FindControl("Textinput");
            CheckBox user_toeganglvl = (CheckBox)input_username.FindControl("Toggle_Checkbox");//TODO: slider

            int user_toegangswaarde = 0;
            if (user_toeganglvl.Checked)
            {
                user_toegangswaarde = 50;

            }

            MySqlCommand query5 = new MySqlCommand("INSERT INTO user (`voornaam`, `achternaam`, `gebruikersnaam`, `wachtwoord`, `email`, `toegangslevel`) VALUES (:voornaam, :achternaam, :gebruikersnaam, :wachtwoord, :email, :toeganglvl)");
            query5.Parameters.Add("voornaam", user_name.Text);
            query5.Parameters.Add("achternaam", user_surname.Text);
            query5.Parameters.Add("gebruikersnaam", user_username.Text);
            query5.Parameters.Add("email", user_email.Text);
            query5.Parameters.Add("toeganglvl", user_toegangswaarde);
            query5.Parameters.Add("wachtwoord", user_toeganglvl.Text);

            global.ExecuteReader(query5, out string error5, out bool errorInd5);
            if (errorInd5)
            {
                Label lbl = new Label();
                lbl.Text = error5;
                UpdatePanel1.ContentTemplateContainer.Controls.Add(lbl);
                UpdatePanel1.DataBind();
                
                /*do something with the error*/
            }
            else
            {
                Label lbl = new Label();
                lbl.Text = "kaas";
                UpdatePanel1.ContentTemplateContainer.Controls.Add(lbl);
                UpdatePanel1.DataBind();



            }
        } 


    }
}