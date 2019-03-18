using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotica_ASP
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string User = Session["user"].ToString();
            //SELECT DISTINCT h.APPARAATID, a.naam FROM heefttoegangtot AS h INNER JOIN apparaat AS a ON h.APPARAATID = a.APPARAATID
            //WHERE h.GROUPID IN(
            //    SELECT `GROUPID` FROM neemtdeelaan

            //    WHERE `userid` IN (
            //        SELECT `userid` FROM user WHERE `gebruikersnaam` = :gbnaam
	           // )
            //)
            // query to get the device id's and names of the devices the user has access to.
        }
    }
}