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
    public partial class Login1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PasswordInput.Attributes["type"] = "password";
            HttpCookie verkade = Request.Cookies["verkade"];
            if (verkade != null)
            {
                if (verkade["remembered"] == "true")
                {
                    if (global.checkUserCookie(verkade, out string error, out bool errorInd))
                    {
                        // put in the username and a false password;
                        UsernameInput.Text = verkade["username"];
                        PasswordInput.Text = "*******";
                    }
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            // delete cookie if remember me is unchecked.
            void delete_cookie()
            {
                if (remember.Checked == false && Request.Cookies["verkade"] != null)
                {
                    base.Response.Cookies["verkade"].Expires = DateTime.Now.AddDays(-1);
                    UsernameInput.Text = "";
                    PasswordInput.Text = "";
                }
            }

            string username = UsernameInput.Text;
            string password = PasswordInput.Text;
            string returned_password = "";

            // check if there is a username
            if (username != "")
            {
                // get a password that corresponds to the given username
                MySqlCommand query = new MySqlCommand("SELECT wachtwoord FROM user WHERE gebruikersnaam = :gebruikersnaam");
                query.Parameters.Add("gebruikersnaam", username);
                List<dynamic> result = global.ExecuteReader(query, out string error, out bool errorInd);
                if (errorInd) { }
                else
                {
                    returned_password = global.getValueFromList(result);
                }

                // if there is a cookie from remember me & it is true then redirect to default.aspx
                if (Request.Cookies["verkade"] != null)
                {
                    HttpCookie verkade = Request.Cookies["verkade"];
                    if (verkade["remembered"] == "true")
                    {
                        // check if the userkey is correct
                        if (global.checkUserCookie(verkade, out string error3, out bool errorInd3))
                        {
                            UsernameInput.Text = verkade["username"];
                            PasswordInput.Text = "*******";
                            Session["user"] = verkade["username"];
                        }
                        else
                        {
                            // if somehow the userkey is wrong then refresh the page.
                            Response.Redirect(Request.Url.AbsolutePath, false);
                        }
                    }
                    delete_cookie();
                }
                // check if password is a supported hash
                else if (SecurePasswordHasher.IsHashSupported(returned_password))
                {
                    // check if the password matches
                    if (SecurePasswordHasher.Verify(password, returned_password))
                    {
                        // TODO: redirect to default;
                        // if remember me checked then add a cookie if it doesnt exist yet.
                        if (remember.Checked == true)
                        {
                            // create cookie
                            if (Request.Cookies["verkade"] == null)
                            {
                                // get the userID that corresponds to the user
                                MySqlCommand query2 = new MySqlCommand("SELECT USERID FROM `user` WHERE gebruikersnaam = :gbnaam");
                                query2.Parameters.Add("gbnaam", username);
                                int userID = global.getValueFromList(global.ExecuteReader(query2, out string error2, out bool errorInd2));
                                if (errorInd) { }
                                else
                                {
                                    // turn the username into a number ( for user key )
                                    int username_num = global.stringToInt(username);
                                    // get a random salt number
                                    Random random = new Random(); int ran = random.Next(465780, 898724); ran = ran * random.Next(465790, 898734);
                                    // create the cookie
                                    HttpCookie verkade = new HttpCookie("verkade");
                                    DateTime now = DateTime.Now;
                                    verkade.Expires = now.AddDays(7);
                                    verkade.Values.Add("remembered", "true"); // remembered value ( kinda obsolute but it's a nice esthetic
                                    verkade.Values.Add("username", username); // the username of the user.
                                    verkade.Values.Add("salt", ran.ToString()); // the salt used for the unique userkey
                                    verkade.Values.Add("userkey", ((ran * (userID * 10)) * username_num).ToString()); // the userkey

                                    Response.Cookies.Add(verkade);
                                }
                            }
                        }
                        // delete the cookie if the remember me is left unchecked after the user is already logged in
                        delete_cookie();
                    }
                    else
                    {
                        ResponseLabel.Text = "wrong password!";
                    }
                }
                else
                {
                    ResponseLabel.Text = "User does not exist!";
                }
            }
            else
            {
                ResponseLabel.Text = "Enter a username!";
            }
        }
    }
}