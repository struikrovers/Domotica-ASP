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
            List<List<string>> result = global.ExecuteReader(userQuery, out string apparaatError, out bool apparaatErrorInd);
            if (apparaatErrorInd)
            {
                Label label = new Label();
                label.Text = apparaatError;
                grid_parent.Controls.Add(label);
            }
            else
            {
                foreach (List<string> row in result)
                {
                    Widget widget = (Widget)LoadControl("Widget.ascx");
                    widget.name = row[0];
                    widget.comment = string.Format("Dit is het account van: {0} {1}", row[1], row[2]);
                    widget.toggle = true;
                    Remove_User.Content.Controls.Add(widget);
                }

                Widget SubmitWidget = (Widget)LoadControl("Widget.ascx");
                SubmitWidget.ID = "SubmitWidget";

                PlaceHolder phInput = new PlaceHolder();
                phInput.ID = "placeholder";

                InputFields Input = (InputFields)LoadControl("InputFields.ascx");
                Input.in_type = "radio";
                Input.button_text = "submit";
                Input.ID = "InputField";

                phInput.Controls.Add(Input);

                SubmitWidget.Input = phInput;

                Remove_User.Content.Controls.Add(SubmitWidget);

            }

        }
    }
}