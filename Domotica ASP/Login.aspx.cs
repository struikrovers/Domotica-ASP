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
            // logout
            if(Request.QueryString["Logout"] == "t")
            {
                //source: https://stackoverflow.com/a/6635426
                Session.Clear();
            }

            if(Session["deleteCookie"] == null)
            {
                Session["deleteCookie"] = false;
            }
            PasswordInput.Attributes["type"] = "password";
            HttpCookie verkade = Request.Cookies["verkade"];
            if (verkade != null)
            {
                if (verkade["remembered"] == "true")
                {
                    if (global.checkUserCookie(verkade, out string error))
                    {
                        // put in the username and a false password;
                        UsernameInput.Text = verkade["username"];
                        PasswordInput.Text = "*******";
                    }
                }
            }
        }        

        protected void Button1_Click(object sender, EventArgs e)
        {
            Session["deleteCookie"] = remember.Checked;
            // delete cookie if remember me is unchecked.

            string username = UsernameInput.Text;
            string password = PasswordInput.Text;
            string returned_password = "";

            // check if there is a username
            if (username != "")
            {
                // get a password that corresponds to the given username
                MySqlCommand PasswordQuery = new MySqlCommand("SELECT wachtwoord FROM user WHERE gebruikersnaam = :gebruikersnaam");
                PasswordQuery.Parameters.Add("gebruikersnaam", username);
                List<List<string>> PasswordResult = global.ExecuteReader(PasswordQuery, out string PasswordQueryError, out bool PasswordQueryErrorInd);
                if (PasswordQueryErrorInd) { /* do something if there is an error */ }
                else
                {
                    returned_password = global.getValueFromList(PasswordResult);
                }

                // if there is a cookie from remember me & it is true then redirect to default.aspx
                if (Request.Cookies["verkade"] != null)
                {
                    HttpCookie verkade = Request.Cookies["verkade"];
                    if (verkade["remembered"] == "true")
                    {
                        // check if the userkey is correct
                        if (global.checkUserCookie(verkade, out string verkadeUserkeyError))
                        {
                            MySqlCommand AuthLevelQuery = new MySqlCommand("SELECT toegangslevel FROM user WHERE gebruikersnaam = :gebruikersnaam");
                            AuthLevelQuery.Parameters.Add("gebruikersnaam", verkade["username"]);
                            List<List<string>> AuthLevelQueryInd = global.ExecuteReader(AuthLevelQuery, out string AuthLevelQueryError, out bool AuthLevelQueryErrorInd);
                            int authlvl = 1;
                            if (AuthLevelQueryErrorInd) { /* do something if there is an error */ }
                            else
                            {
                                authlvl = int.Parse(global.getValueFromList(AuthLevelQueryInd));
                            }

                            UsernameInput.Text = verkade["username"];
                            PasswordInput.Text = "*******";
                            Session["user"] = verkade["username"];
                            Session["LoggedIn"] = true;
                            Session["authlvl"] = authlvl;
                            Response.Redirect("default.aspx", true);
                        }
                        else
                        {
                            // if somehow the userkey is wrong then refresh the page.
                            Response.Redirect(Request.Url.AbsolutePath, true);
                        }
                    }
                }
                // check if password is a supported hash
                else if (SecurePasswordHasher.IsHashSupported(returned_password))
                {
                    // check if the password matches
                    if (SecurePasswordHasher.Verify(password, returned_password))
                    {
                        MySqlCommand AuthLevelQuery_nonCookie = new MySqlCommand("SELECT toegangslevel FROM user WHERE gebruikersnaam = :gebruikersnaam");
                        AuthLevelQuery_nonCookie.Parameters.Add("gebruikersnaam", username);
                        List<List<string>> AuthLevelQuery_nonCookie_Result = global.ExecuteReader(AuthLevelQuery_nonCookie, out string AuthLevelQuery_nonCookieError, out bool AuthLevelQuery_nonCookieErrorInd);
                        int authlvl = 1;
                        if (AuthLevelQuery_nonCookieErrorInd) { /* do something if there is an error */ }
                        else
                        {
                            authlvl = int.Parse(global.getValueFromList(AuthLevelQuery_nonCookie_Result));
                        }

                        // if remember me checked then add a cookie if it doesnt exist yet.
                        if (remember.Checked == true)
                        {
                            // create cookie
                            if (Request.Cookies["verkade"] == null)
                            {
                                // get the userID that corresponds to the user
                                MySqlCommand UserIDQuery = new MySqlCommand("SELECT USERID FROM `user` WHERE gebruikersnaam = :gbnaam");
                                UserIDQuery.Parameters.Add("gbnaam", username);
                                List<List<string>> UserIdQueryResult = global.ExecuteReader(UserIDQuery, out string UserIDQueryError, out bool UserIDQueryErrorInd);
                                if (UserIDQueryErrorInd) { /* do something if there is an error */ }
                                else
                                {
                                    int userID = int.Parse(global.getValueFromList(UserIdQueryResult));
                                    if (PasswordQueryErrorInd) { }
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
                        }

                        Session["user"] = username;
                        Session["LoggedIn"] = true;
                        Session["authlvl"] = authlvl;
                        Response.Redirect("default.aspx", true);
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