using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotica_ASP
{
    public partial class Default : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            void delete_cookie()
            {
                if ((bool)Session["deleteCookie"] == false && Request.Cookies["verkade"] != null)
                {
                    Response.Cookies["verkade"].Expires = DateTime.Now.AddDays(-1);
                }
            }

            if (Session["deleteCookie"] != null)
            {
                delete_cookie();
            }

            if (Session["LoggedIn"] != null)
            {
                if ((bool)Session["LoggedIn"] == true)
                {
                    Welcome.Text = "welkom ";
                    WelcomeLabel.Text = (string)Session["user"];
                    LoginButton.Visible = false;
                    LogoutButton.Visible = true;
                }
                else
                {

                }
            }

            // do not show the login header at the login page. source: https://stackoverflow.com/a/3215399
            if (ContentPlaceHolder1.Page.GetType().BaseType.Name == "Login")
            {
                Header.Visible = false;
            }
        }
    }
}