using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Devart.Data.MySql;

namespace Domotica_ASP
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordInput.Attributes["type"] = "password";
            HttpCookie verkade = Request.Cookies["verkade"];
            if (verkade != null)
            {
                if (verkade["remembered"] == "true")
                {
                    MySqlCommand query2 = new MySqlCommand("SELECT USERID FROM `user` WHERE gebruikersnaam = :gbnaam");
                    query2.Parameters.Add("gbnaam", verkade["username"]);
                    int userID = global.getValueFromList(global.ExecuteReader(query2, out string error2, out bool errorInd2));
                    int username_num = global.stringToInt(verkade["username"]);
                    if (verkade["userkey"] == ((int.Parse(verkade["salt"]) * (userID * 10)) * username_num).ToString())
                    {
                        Label1.Text = "already logged in";
                        if (!remember.Checked)
                        {
                            Response.Cookies.Remove("verkade");
                        }
                    }
                    else
                    {
                        Label1.Text = "wrong password!";
                    }

                }
            }

            /*
            MySqlCommand query2 = new MySqlCommand("INSERT INTO user (`voornaam`, `achternaam`, `gebruikersnaam`, `wachtwoord`, `email`, `toegangslevel`) VALUES ('imre', 'korf', 'admin3', :wachtwoord, 'test3@hotmail.com', '50')");
            query2.Parameters.Add("wachtwoord", SecurePasswordHasher.Hash("admin123"));
            global.ExecuteReader(query2, out string error2, out bool errorInd2);
            if (errorInd2)
            {
                Label1.Text = error2;
            }
            */
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string username = UsernameInput.Text;
            string password = PasswordInput.Text;
            string returned_password = "";

            MySqlCommand query = new MySqlCommand("SELECT wachtwoord FROM user WHERE gebruikersnaam = :gebruikersnaam");
            query.Parameters.Add("gebruikersnaam", username);
            string error; bool errorInd;
            List<dynamic> result = global.ExecuteReader(query, out error, out errorInd);
            if (errorInd)
            {
                Label1.Text = error;
            }
            else
            {
                returned_password = global.getValueFromList(result);
            }
            if(SecurePasswordHasher.Verify(password, returned_password))
            {
                Label1.Text = "logged in!";
                if (remember.Checked == true) {
                    if (Request.Cookies["verkade"] == null)
                    {
                        MySqlCommand query2 = new MySqlCommand("SELECT USERID FROM `user` WHERE gebruikersnaam = :gbnaam");
                        query2.Parameters.Add("gbnaam", username);
                        int userID = global.getValueFromList(global.ExecuteReader(query2, out string error2, out bool errorInd2));
                        int username_num = global.stringToInt(username); ;
                        if (errorInd2)
                        {
                            Label2.Text = error2;
                        }
                        Random random = new Random(); int ran = random.Next(465780, 898724); ran = ran * random.Next(465790, 898734);

                        HttpCookie verkade = new HttpCookie("verkade");
                        DateTime now = DateTime.Now;
                        verkade.Expires = now.AddDays(7);
                        verkade.Values.Add("remembered", "true");
                        verkade.Values.Add("username", username);
                        verkade.Values.Add("salt", ran.ToString());
                        verkade.Values.Add("userkey", ((ran * (userID * 10)) * username_num).ToString());
                        
                        Response.Cookies.Add(verkade);
                        Label2.Text = ((ran * (userID * 10)) * username_num).ToString();
                    }
                }
            }
            else
            {
                Label1.Text = "wrong password!";
            }
        }
    }
}