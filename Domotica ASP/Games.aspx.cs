using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Domotica_ASP
{
    public partial class Games : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = DropDownList1.SelectedIndex;
        }

        protected void snake_Init(object sender, EventArgs e)
        {
            HtmlGenericControl body = (HtmlGenericControl)Master.FindControl("MasterBody");
            body.Attributes["onload"] = "initGame()";
            body.Attributes["onkeydown"] = "enterKey(event)";
        }
    }
}