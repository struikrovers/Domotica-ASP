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
        }
    }
}