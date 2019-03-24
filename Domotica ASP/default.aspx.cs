using Devart.Data.MySql;
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
            //SELECT DISTINCT h.APPARAATID, a.naam FROM heefttoegangtot AS h INNER JOIN apparaat AS a ON h.APPARAATID = a.APPARAATID
            //WHERE h.GROUPID IN(
            //    SELECT `GROUPID` FROM neemtdeelaan

            //    WHERE `userid` IN (
            //        SELECT `userid` FROM user WHERE `gebruikersnaam` = :gbnaam
            // )
            //)
            // query to get the device id's and names of the devices the user has access to.

            if (Session["LoggedIn"] != null)
            {
                
                MySqlCommand apparaatquery = new MySqlCommand("SELECT DISTINCT h.APPARAATID, a.naam FROM heefttoegangtot AS h INNER JOIN apparaat AS a ON h.APPARAATID = a.APPARAATID WHERE h.GROUPID IN(SELECT `GROUPID` FROM neemtdeelaan WHERE `userid` IN ( SELECT `userid` FROM user WHERE `gebruikersnaam` = :gbnaam))");
                apparaatquery.Parameters.Add("gbnaam", Session["user"]);
                List<List<string>> result = global.ExecuteReader(apparaatquery, out string apparaatError, out bool apparaatErrorInd);
                if (apparaatErrorInd)
                {
                    Label label = new Label();
                    label.Text = apparaatError;
                    grid_parent.Controls.Add(label);
                }

                Dictionary<string, string> tableQueryComb = new Dictionary<string, string>() { { "kantemp", "`min`, `max`" }, { "kanstand", "`stand`"} };

                foreach(List<string> row in result)
                {
                    MySqlCommand InputTypeQuery = new MySqlCommand("SELECT apparaatID, InputType, TypeWaarde FROM apparaattype AS appType INNER JOIN apparaat AS app ON appType.TypeID = app.TypeID WHERE app.apparaatid = :appID");
                    InputTypeQuery.Parameters.Add("appID", row[0]);
                    List<List<string>> result2 = global.ExecuteReader(InputTypeQuery, out string InputQueryError, out bool InputQueryErrorInd);
                    if (InputQueryErrorInd) { }
                    else
                    {
                        
                        List<string> input_type = result2[0];
                        /*
                        Label label = new Label();
                        
                        foreach(string record in input_type)
                        {
                            label.Text += record + " ";
                        }

                        grid_parent.Controls.Add(label);
                        */

                        if (input_type[1] == "Toggle")
                        {
                            Widget widget = (Widget)LoadControl("Widget.ascx");
                            widget.name = row[1];
                            widget.ID = row[1];
                            widget.toggle = true;
                            grid_parent.Controls.Add(widget);
                        }
                        else
                        {
                            Widget widget = (Widget)LoadControl("Widget.ascx");
                            widget.name = row[1];
                            widget.ID = row[1];

                            PlaceHolder inputPH = new PlaceHolder();
                            inputPH.ID = "inputPH";

                            InputFields input = (InputFields)LoadControl("InputFields.ascx");
                            input.ID = input_type[1] + "_" + row[1];

                            if (global.listTypes.ContainsKey(input_type[1]))
                            {
                                PlaceHolder ListPH = new PlaceHolder();
                                ListPH.ID = "ListPH";

                                int type;
                                if (global.listTypes.TryGetValue(input_type[1], out type)) { Console.WriteLine("Input type specified does not exist in class" + this.ID.ToString()); }

                                // get the device specs such as "min/max" or "stand"

                                MySqlCommand getAppSpec = new MySqlCommand();
                                if (input_type[2] == "kantemp")
                                {
                                    getAppSpec.CommandText = "SELECT `min`, `max` FROM kantemp WHERE `apparaatid` = :apparaatid";
                                }
                                else
                                {
                                    getAppSpec.CommandText = "SELECT `stand` FROM kanstand WHERE `apparaatid` = :apparaatid";
                                }
                                getAppSpec.Parameters.Add("apparaatid", input_type[0]);
                                List<List<string>> appSpec = global.ExecuteReader(getAppSpec, out string appSpecError, out bool appSpecErrorInd);
                                if (appSpecErrorInd) { }
                                else
                                {
                                    switch (type)
                                    {
                                        case 1:
                                            // horizontal slider
                                            input.minvalue = int.Parse(appSpec[0][0]);
                                            input.maxvalue = int.Parse(appSpec[0][1]);
                                            input.stanvalue = int.Parse(appSpec[0][0]);
                                            break;
                                        case 2:
                                            // vertical slider
                                            input.minvalue = int.Parse(appSpec[0][0]);
                                            input.maxvalue = int.Parse(appSpec[0][1]);
                                            input.stanvalue = int.Parse(appSpec[0][0]);
                                            break;
                                        case 3:
                                            // text

                                            break;
                                        case 4:
                                            // number
                                            break;
                                        case 5:
                                            // radio
                                            RadioButtonList RBlist = new RadioButtonList();
                                            RBlist.ID = "RadioButtonList";

                                            ListPH.Controls.Add(RBlist);
                                            input.__Radio = ListPH;
                                            break;
                                        case 6:
                                            // checkbox
                                            CheckBoxList CKlist = new CheckBoxList();
                                            CKlist.ID = "RadioButtonList";
                                            input_type[1] = "radio";

                                            ListPH.Controls.Add(CKlist);
                                            input.__Radio = ListPH;
                                            break;
                                        case 7:
                                            // dropdownlist
                                            DropDownList DPlist = new DropDownList();
                                            DPlist.ID = "RadioButtonList";

                                            ListPH.Controls.Add(DPlist);
                                            input.__DropList = ListPH;
                                            break;
                                        default:
                                            break;
                                    }

                                    input.in_type = input_type[1];
                                }
                            }

                            inputPH.Controls.Add(input);
                            widget.Input = inputPH;

                            grid_parent.Controls.Add(widget);
                            
                        }
                        
                    }
                }
                
            }
        }
    }
}