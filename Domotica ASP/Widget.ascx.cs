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
        public string overlayID { get; set; }
        public bool timeField { get; set; } = false;
        public bool submittable { get; set; } = false;

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

            overlayID = "ContentPlaceHolder1_" + overlayID;

            toggable.Visible = toggle;
            settings.Visible = setting;
            grid_child_name.InnerHtml = name;
            grid_child_comment.InnerHtml = comment;
            settings_icon.Attributes["onclick"] = "open_overlay(event, '" + overlayID + "_overlay_child');";
            Toggle_Checkbox.InputAttributes["class"] = "Toggle_Checkbox";
            Toggle_Checkbox.InputAttributes["name"] = ClientID + "_Toggle_Checkbox";
            timeInput.Attributes["placeholder"] = "HH:MM - DD";
            timeInput.Attributes["onblur"] = "timeValidator(this)";
            timeInput.CssClass = "timeInputField";
            timeInput.Visible = timeField;
            submitBTN.Visible = submittable;
            if (setting)
            {
                ToggleLabel.Attributes["style"] = "right: 30px";
            }


        }

        protected void submitBTN_Click(object sender, EventArgs e)
        {
            
        }
    }
}