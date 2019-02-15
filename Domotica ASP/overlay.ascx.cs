using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotica_ASP
{
    public partial class overlay : System.Web.UI.UserControl
    {
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public PlaceHolder Content { get; set; }
        public string widget_parent { get; set; } = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            base.OnInit(e);
            _Content.Controls.Add(Content);
        }
    }
}