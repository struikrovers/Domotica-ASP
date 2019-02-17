using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotica_ASP
{
    public partial class InputFields : System.Web.UI.UserControl
    {

        public int stanvalue { get; set; } = 1;
        public int maxvalue { get; set; } = 100;
        public int minvalue { get; set; } = 0;
        public string in_type { get; set; } = "hor_slider";
        private Dictionary<string, int> in_type_dict = new Dictionary<string, int>() { { "hor_slider", 1 }, { "ver_slider", 2 }, { "text", 3 }, { "number", 4 }, { "radio", 5 }, { "checkbox" , 6}};
        protected void Page_Load(object sender, EventArgs e)
        {
            Horiz_Range.Visible = false;
            Vert_Range.Visible = false;
            Text.Visible = false;
            Number.Visible = false;
            Radio.Visible = false;
            Checkbox.Visible = false;

            int type;
            if (in_type_dict.TryGetValue(in_type, out type)) { Console.WriteLine("Input type specified does not exist in class" + this.ID.ToString()); }
            switch (type) {
                case 1:
                    // horizontal slider
                    Horiz_Range.Visible = true;
                    break;
                case 2:
                    // vertical slider
                    Vert_Range.Visible = true;
                    break;
                case 3:
                    // Text input
                    Text.Visible = true;
                    break;
                case 4:
                    // number input
                    Number.Visible = true;
                    break;
                case 5:
                    // radio button
                    Radio.Visible = true;
                    break;
                case 6:
                    // checkbox
                    Checkbox.Visible = true;
                    break;
                default:
                    break;
            }
        }
    }
}