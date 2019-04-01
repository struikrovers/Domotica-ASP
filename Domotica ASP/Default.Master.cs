using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotica_ASP
{
    public partial class Default : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if(Membership.GetUser() != null)
            {
                // user is logged in
                Label username = (Label)MasterLogin.FindControl("WelcomeLabel");
                username.Text = " " + Membership.GetUser().UserName;
                if (ContentPlaceHolder1.Page.GetType().BaseType.Name == "admin")
                {
                    if (Roles.GetRolesForUser().Contains("admins"))
                    {
                        Button adminpage = (Button)MasterLogin.FindControl("AdminButton");
                        adminpage.Text = "user";
                        adminpage.PostBackUrl = "~/default.aspx";
                    }
                }
            }
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~/Login.aspx");
        }
    }
}