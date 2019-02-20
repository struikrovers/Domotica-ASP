using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotica_ASP
{
    public partial class InputRadioCheck : System.Web.UI.UserControl
    {
        public int amount { get; set; } = 0;
        public string type { get; set; } = "radio";
        public string value { get; set; } = "";
        public bool check { get; set; } = false;
        protected string _checked = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (check)
            {
                _checked = "checked";
            }
        }
    }
}