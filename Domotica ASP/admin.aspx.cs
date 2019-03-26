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

                Widget Submit_Add_User = (Widget)LoadControl("Widget.ascx");
                Submit_Add_User.ID = "SubmitWidget";
                Submit_Add_User.submittable = true;
                Submit_Add_User.name = "verstuur";
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

                Widget Submit_DeleteGroupOID = (Widget)LoadControl("Widget.ascx");
                Submit_DeleteGroupOID.ID = " Submit_DeleteGroupOID";
                Submit_DeleteGroupOID.submittable = true;
                Submit_DeleteGroupOID.name = "verstuur";
                DeleteGroupOID.Content.Controls.Add(Submit_DeleteGroupOID);

                



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
    }
}