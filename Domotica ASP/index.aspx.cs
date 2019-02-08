using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotica
{
    public partial class index : System.Web.UI.Page
    {
        
        public static int widgets = 0;

        static string MakeItem(int id)
        {
            /*
            < !--grid item template -->

            < div class="grid_child" id="grid_child_1">
				<!-- -->
				<p class="grid_child_name">Name</p>
				<p class="grid_child_comment">Comment</p>
			</div>
            */

            StringWriter stringWriter = new StringWriter();

            using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
            {
                string classValue = "grid_child";
                string idValue = "grid_child_"+id.ToString();

                writer.AddAttribute(HtmlTextWriterAttribute.Class, classValue);
                writer.AddAttribute(HtmlTextWriterAttribute.Id, idValue);
                // start div
                writer.RenderBeginTag(HtmlTextWriterTag.Div);

                // start name paragraph
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "grid_child_name");
                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write("Name");
                writer.RenderEndTag();
                // start comment paragraph
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "grid_child_name");
                writer.RenderBeginTag(HtmlTextWriterTag.P);
                writer.Write("Comment");
                writer.RenderEndTag();
                // end div
                writer.RenderEndTag();
            }
            return stringWriter.ToString();
        }

        public class WidgetItem : System.Web.UI.Page
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
            public string id;
            public string classname;
            public StringWriter stringWriter = new StringWriter();
            private System.Web.UI.HtmlControls.HtmlGenericControl grid_parent;

            /// <summary>
            /// WidgetItem constructor
            /// </summary>
            /// <param name="name">The name of the widget</param>
            /// <param name="classname">class name of the widget</param>
            /// <param name="comment">extra text for on the widget. default is nothing</param>
            /// <param name="name_class"> class for the name text in the widget. default is grid_child_name</param>
            /// <param name="comment_class"> class for the comment text in the widget. default is grid_child_name</param>
            public WidgetItem(string name, System.Web.UI.HtmlControls.HtmlGenericControl grid_parent, string classname = "grid_child", string comment = "", string name_class = "grid_child_name", string comment_class = "grid_child_name"){
                index.widgets++;
                this.id = "grid_child_" + index.widgets;
                this.classname = classname;
                using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "grid_child");
                    if(this.classname != "grid_child"){writer.AddAttribute(HtmlTextWriterAttribute.Class, classname);} // only add other class if the classname isn't default
                    writer.AddAttribute(HtmlTextWriterAttribute.Id, this.id);
                    // start div
                    writer.RenderBeginTag(HtmlTextWriterTag.Div);

                    // start name paragraph
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "grid_child_name");
                    writer.RenderBeginTag(HtmlTextWriterTag.P);
                    writer.Write("Name");
                    writer.RenderEndTag();
                    // start comment paragraph
                    if(comment != ""){
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "grid_child_name");
                    writer.RenderBeginTag(HtmlTextWriterTag.P);
                    writer.Write("Comment");
                    writer.RenderEndTag();
                    }
                    // end div
                    writer.RenderEndTag();
                }
                grid_parent.InnerHtml += stringWriter.ToString();
            }
            
            /// <summary>
            /// declares if the widget should be a switch that turns on/off
            /// </summary>
            public void isToggable(){
                using (HtmlTextWriter writer = new HtmlTextWriter(stringWriter))
                {
                    writer.AddAttribute(HtmlTextWriterAttribute.Class, "grid_child_name");
                    writer.RenderBeginTag(HtmlTextWriterTag.P);
                    writer.Write("Name");
                    writer.RenderEndTag();
                }
            }

            /// <summary>
            /// declares settings of the widget, standard overlay is created, and innerhtml is implemented into that window
            /// </summary>
            /// <param name="settings">innerHTML for the settings window</param>
            public void hasSettings(StringWriter settings){

            }

            /// <summary>
            /// Declares an input field for on the widget.
            /// </summary>
            /// <param name="type">type of input field eg. number, text</param>
            public void hasInput(string type){

            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            for (int id = 0; id < 7; id++)
            {
                widgets++; // amount of widgets indicator
                grid_parent.InnerHtml += MakeItem(widgets);
            }

            WidgetItem widget1 = new WidgetItem("test", grid_parent);
            widget1.isToggable();
        }
    }
}