using System;
using System.IO;
using System.Collections.Generic;

namespace Domotica
{
    public partial class index : System.Web.UI.Page
    {
        
        public static int widgets = 0;

        public class WidgetItem
        {
            /** NOTE: & TODO:
             * Widget creator class
             * needs to be able to create a widget item for on the grid.
             * widget creator has special functions to make widget special
             * list of functions needed:
             * - input field. arguments: specified what type of input
             * - on/off switch: indication if widget is currently active or not. arguments: boolean true or false? 
             * - settings field: window that opens on mobile to add specifics for that widget function. arguments: html string for in the window
             */

            public string name;
            public string comment;
            public string id;
            public string classname;
            public string name_class;
            public string comment_class;
            public StringWriter strWriter = new StringWriter();

            private string HTML_string;

            private System.Web.UI.HtmlControls.HtmlGenericControl grid_parent;

            public bool Toggable = false;

            /// <summary>
            /// WidgetItem constructor
            /// </summary>
            /// <param name="name">The name of the widget</param>
            /// <param name="classname">class name of the widget</param>
            /// <param name="comment">extra text for on the widget. default is nothing</param>
            /// <param name="name_class"> class for the name text in the widget. default is grid_child_name</param>
            /// <param name="comment_class"> class for the comment text in the widget. default is grid_child_name</param>
            public WidgetItem(string name, System.Web.UI.HtmlControls.HtmlGenericControl grid_parent, string classname = "grid_child", string comment = "", string name_class = "grid_child_name", string comment_class = "grid_child_name")
            {
                index.widgets++;
                this.id = "grid_child_" + index.widgets;
                this.classname = classname;
                this.name_class = name_class;
                this.comment_class = comment_class;

                this.name = name;
                this.comment = comment;

                this.grid_parent = grid_parent;

                HTML_string = @"
                    <p class="+name_class+">"+name+@"</p>
                    <p class="+comment_class+@">"+comment+@"</p>
                ";
            }

            /// <summary>
            /// declares if the widget should be a switch that turns on/off
            /// </summary>
            public void isToggable(string function)
            {
                // TODO: ask database if function is on.
                string _checked = ""; //"checked";
                HTML_string += @"
                <label class=toggableContainer>
                    <input class='Toggle_checkbox' type='checkbox' "+ _checked +@">
                    <span class=toggableCheckbox onclick="+ function + @"></span>
                    <label class='toggle_switch'>
                        <span class='toggle_slider round'></span>
                    </label>
                </label>
                ";
                // NOTE: https://stackoverflow.com/questions/7896402/how-can-i-replace-text-with-css
                classname += " toggable_widget grid_child_toggable";
                Toggable = true;
            }

            /// <summary>
            /// declares settings of the widget, standard overlay is created, and innerhtml is implemented into that window
            /// </summary>
            /// <param name="settings">innerHTML for the settings window</param>
            /// <param name="grid_overlay">HTML tag ID of overlay_child</param>
            public void hasSettings(string settings, System.Web.UI.HtmlControls.HtmlGenericControl overlay_child, System.Web.UI.HtmlControls.HtmlGenericControl grid_overlay_control)
            {
                grid_overlay_control.InnerHtml += "<input id='settings_checkbox_" + this.id + @"' type='checkbox'>";
                HTML_string += @"
                <div class='settings'>
                    <label for='settings_checkbox_"+this.id+@"' class='settings_label'>
                        <span class='settings_icon'>
                            <i style='font-size: 0.87em' class='fa fa-gear'></i>
                        </span>
                    </label>
                </div>";
                overlay_child.InnerHtml += @"";
            }

            /// <summary>
            /// Declares an input field for on the widget.
            /// </summary>
            /// <param name="type">type of input field eg. number, text</param>
            public void hasInput(string type)
            {

            }

            public void renderWidget()
            {                
                HTML_string = @"
                    <div class='"+ classname + "' id="+ id +@">
                        " + HTML_string + @"
                    </div>
                ";
                grid_parent.InnerHtml += HTML_string;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            index.widgets = 0;
            Dictionary<string, WidgetItem> widgets = new Dictionary<string, WidgetItem>();
            for (int id = 0; id < 7; id++)
            {
                widgets.Add("widget_" + id, new WidgetItem("name", grid_parent, comment: "comment"));
            }

            foreach( KeyValuePair<string, WidgetItem> kvp in widgets)
            {
                kvp.Value.isToggable("");
                kvp.Value.hasSettings("", overlay_child, grid_overlay_control);
                kvp.Value.renderWidget();
            }
            
            WidgetItem Test = new WidgetItem("test_test", grid_parent, comment: "test2");
            Test.renderWidget();
        }
    }
}