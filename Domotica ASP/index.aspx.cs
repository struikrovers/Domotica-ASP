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

        protected void Page_Load(object sender, EventArgs e)
        {
            for (int id = 0; id < 7; id++)
            {
                grid_parent.InnerHtml += MakeItem(id);
            }
        }
    }
}