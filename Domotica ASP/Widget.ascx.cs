using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotica_ASP
{
    public partial class Widget : System.Web.UI.UserControl
    {
        // NOTE: setting up properties for tag helper 
        public string name { get; set; } = "name";
        public string comment { get; set; } = "";
        public bool toggle { get; set; } = false;
        public bool setting { get; set; } = false;
        

        // NOTE: input fields
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public PlaceHolder Input { get; set; } = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Input != null)
            {
                base.OnInit(e);
                _Input.Controls.Add(Input);
            }
            else
            {
                input.Visible = false;
            }
            toggable.Visible = toggle;
            settings.Visible = setting;
        }
    }
}