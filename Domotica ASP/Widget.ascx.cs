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
        public string[] input_types { get; set; } = { };

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

            if (timeField)
            {
                //timeInput.Text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + "-" + DateTime.Now.Day.ToString();
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
            Label output = (Label)Parent.FindControl("output");
            string[] UserInput = new string[3];
            output.Text = "";
            int Timer = 999;
            for(int i = 0; i < input_types.Length; i++)
            {
                if (input_types[i] == "Toggle")
                {
                    UserInput[i] = Toggle_Checkbox.Checked.ToString();
                }
                else
                {
                    if (Input.FindControl(input_types[i]) != null)
                    {
                        InputFields InputField = (InputFields)Input.FindControl(input_types[i]);
                        int type; // the type index from the dictionary
                        if (!global.listTypes.TryGetValue(input_types[i], out type)) { throw new inputTypeException(string.Format("Given Input type does not exist! from widget: {0}", ID)); } // throw an error if the input type from the database does not exist
                        else
                        {
                            switch (type)
                            {
                                case 1:
                                    // horizontal slider
                                    TextBox H_R_Input = (TextBox)InputField.FindControl("Range_Input");
                                    UserInput[i] = H_R_Input.Text;
                                    break;

                                case 2:
                                    // vertical slider
                                    TextBox V_R_Input = (TextBox)InputField.FindControl("Range_Input_Vert");
                                    UserInput[i] = V_R_Input.Text;
                                    break;

                                case 3:
                                    // text
                                    TextBox Text = (TextBox)InputField.FindControl("TextInput");
                                    UserInput[i] = Text.Text;
                                    break;

                                case 4:
                                    // number
                                    TextBox num = (TextBox)InputField.FindControl("NumberInput");
                                    if (i == 2)
                                    {
                                        // the timer input
                                        Timer = int.Parse(num.Text);
                                    }
                                    else
                                    {
                                        UserInput[i] = num.Text;
                                    }
                                    break;

                                case 5:
                                    // radio
                                    RadioButtonList RBList = (RadioButtonList)InputField.__DropList.FindControl("RadioButtonListInput");
                                    UserInput[i] += RBList.SelectedValue;
                                    break;

                                case 6:
                                    // checkbox
                                    CheckBoxList CKList = (CheckBoxList)InputField.__DropList.FindControl("CheckboxListInput");
                                    UserInput[i] += CKList.SelectedValue;
                                    break;

                                case 7:
                                    // dropdownlist
                                    DropDownList DDList = (DropDownList)InputField.__DropList.FindControl("DropDownListInput");
                                    UserInput[i] += DDList.SelectedValue;
                                    break;

                                default:
                                    // nothing
                                    break;
                            }
                        }
                    }
                }
            }
            int subtractor = 2;
            if((timeInput.Text.Length % 2) != 0)
            {
                subtractor = 1;
            }
            DateTime schedule;
            DateTime Month;
            if (timeInput.Text == "")
            {
                schedule = DateTime.Now;
            }
            else {
                if (int.Parse(timeInput.Text.Substring(timeInput.Text.Length - subtractor, subtractor)) < DateTime.Now.Day)
                {
                    Month = DateTime.Now.AddMonths(1);
                }
                else
                {
                    Month = DateTime.Now;
                }
                schedule = new DateTime(DateTime.Now.Year, Month.Month, int.Parse(timeInput.Text.Substring(timeInput.Text.Length - subtractor, subtractor)), int.Parse(timeInput.Text.Substring(0, 2)), int.Parse(timeInput.Text.Substring(3, 2)), DateTime.Now.Second);
            }

            global.updateDevices(UserInput, schedule, Timer, name, out string error);
            if (error != "")
            {
                output.Text += error;
            }
            else
            {
                string time = schedule.ToShortDateString() + " " + schedule.ToShortTimeString();
                if (schedule.DayOfYear > DateTime.Now.DayOfYear)
                {
                    if (schedule.DayOfYear - DateTime.Now.DayOfYear > 1)
                    {
                        time = string.Format("voor {0}-{1}", schedule.Day, schedule.Month);
                    }
                    else
                    {
                        time = string.Format("voor morgen");
                    }
                    time += string.Format(" om {0}", schedule.ToShortTimeString());
                }
                else
                {
                    time = string.Format("voor {0} vandaag", schedule.ToShortTimeString());
                }
                output.Text = string.Format("{0} toegevoegd {1}", name, time);
                if (Timer != 999)
                {
                    output.Text += " voor " + Timer.ToString() + " Minuten";
                }
            }
            output.DataBind();
        }
    }
}