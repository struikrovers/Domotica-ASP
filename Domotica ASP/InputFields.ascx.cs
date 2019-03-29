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

        public int stanvalue { get; set; }
        public string stantext { get; set; } = "placeholder";
        public int maxvalue { get; set; } = 100;
        public int minvalue { get; set; } = 0;
        public string in_type { get; set; } = "hor_slider";
        public string innerHTML { get; set; } = "";

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public PlaceHolder __Radio { get; set; } = null;
        public PlaceHolder __DropList { get; set; } = null;
        public string button_text { get; set; } = "submit";
        protected string parent_id;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            parent_id = ClientID.ToString().Remove(ClientID.ToString().IndexOf(ID.ToString()), (ID.ToString()).Length);
            Horiz_Range.Visible = false;
            Vert_Range.Visible = false;
            Text.Visible = false;
            Number.Visible = false;
            Radio.Visible = false;
            DropDownList.Visible = false;

            if (__Radio != null)
            {
                base.OnInit(e);
                _Radio.Controls.Add(__Radio);
            }

            if (__DropList != null)
            {
                base.OnInit(e);
                _DropDownList.Controls.Add(__DropList);
            }

            int type;
            // give an error when the given type does not exist.
            if (!global.listTypes.TryGetValue(in_type, out type)) { throw new inputTypeException(string.Format("Input type does not exist! From widget: {0}", parent_id)); }
            switch (type) {
                case 1:
                    // horizontal slider
                    Horiz_Range.Visible = true;
                    Range_Input.Attributes["type"] = "range";
                    Range_Display.Attributes["type"] = "number";
                    Range_Input.Attributes["value"] = stanvalue.ToString();
                    Range_Display.Attributes["value"] = stanvalue.ToString();
                    Range_Input.Attributes["min"] = minvalue.ToString();
                    Range_Display.Attributes["min"] = minvalue.ToString();
                    Range_Input.Attributes["max"] = maxvalue.ToString();
                    Range_Display.Attributes["max"] = maxvalue.ToString();
                    Range_Input.Attributes["oninput"] = "document.getElementById('" + ClientID + "_Range_Display').value = document.getElementById('" + ClientID + "_Range_Input').value";
                    Range_Display.Attributes["oninput"] = "document.getElementById('" + ClientID + "_Range_Input').value = document.getElementById('" + ClientID + "_Range_Display').value";
                    Range_Display.Attributes["onchange"] = "checkuserinput('" + ClientID + "')";
                    break;
                case 2:
                    // vertical slider
                    Vert_Range.Visible = true;
                    Range_Input_Vert.Attributes["type"] = "range";
                    Range_Display_Vert.Attributes["type"] = "number";
                    Range_Input_Vert.Attributes["value"] = stanvalue.ToString();
                    Range_Display_Vert.Attributes["value"] = stanvalue.ToString();
                    Range_Input_Vert.Attributes["min"] = minvalue.ToString();
                    Range_Display_Vert.Attributes["min"] = minvalue.ToString();
                    Range_Input_Vert.Attributes["max"] = maxvalue.ToString();
                    Range_Display_Vert.Attributes["max"] = maxvalue.ToString();
                    Range_Input_Vert.Attributes["oninput"] = "document.getElementById('" + ClientID + "_Range_Display_Vert').value = document.getElementById('" + ClientID + "_Range_Input_Vert').value";
                    Range_Display_Vert.Attributes["oninput"] = "document.getElementById('" + ClientID + "_Range_Input_Vert').value = document.getElementById('" + ClientID + "_Range_Display_Vert').value";
                    Range_Display_Vert.Attributes["onchange"] = "checkuserinput('" + ClientID + "')";
                    break;
                case 3:
                    // Text input
                    Text.Visible = true;
                    TextInput.Attributes["type"] = "Text";
                    TextInput.Attributes["placeholder"] = stantext;
                    break;
                case 4:
                    // number input
                    Number.Visible = true;
                    NumberInput.Attributes["type"] = "number";
                    NumberInput.Attributes["value"] = stanvalue.ToString();
                    NumberInput.Attributes["min"] = minvalue.ToString();
                    NumberInput.Attributes["max"] = maxvalue.ToString();
                    break;
                case 5:
                    // radio button
                    Radio.Visible = true;
                    RadioSubBTN.Text = button_text;
                    break;
                case 7:
                    // DropDownList button
                    DropDownList.Visible = true;
                    break;
                default:
                    break;
            }

            InputField.DataBind();
        }
    }
}