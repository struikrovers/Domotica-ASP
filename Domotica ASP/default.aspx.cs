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

                MySqlCommand apparatIDSMetOverlay = new MySqlCommand("SELECT DISTINCT kantemp.apparaatID, kanstand.apparaatID FROM kantemp, kanstand WHERE kantemp.apparaatID = kanstand.apparaatID");
                List<List<string>> apparaatIDSmetOverlayResult = global.ExecuteReader(apparatIDSMetOverlay, out string IDError, out bool IDErrorInd);
                List<string> apparaatIDSmetOverlayList = new List<string>();
                foreach(List<string> row in apparaatIDSmetOverlayResult)
                {
                    apparaatIDSmetOverlayList.Add(row[0]);
                }

                foreach (List<string> row in result)
                {
                    if (apparaatIDSmetOverlayList.Contains(row[0]))
                    {
                        // get multiple values
                        MySqlCommand overlayQuery = new MySqlCommand("SELECT `min`, `max`, `stand` FROM kanstand INNER JOIN kantemp ON kanstand.APPARAATID = kantemp.APPARAATID WHERE kanstand.APPARAATID = :appID");
                        overlayQuery.Parameters.Add("appID", row[0]);
                        List<List<string>> overlayQueryResult = global.ExecuteReader(overlayQuery, out string overlayQueryError, out bool overlayQueryErrorInd);

                        // create the widget
                        Widget widget = (Widget)LoadControl("Widget.ascx");
                        widget.ID = row[1];
                        widget.name = row[1];

                        // create the <Input> place holder
                        PlaceHolder InputPlaceHolder = new PlaceHolder();

                        // horizontal slider InputFields Usercontrol for in the Input place holder
                        InputFields slider = (InputFields)LoadControl("InputFields.ascx");
                        slider.in_type = "ver_slider";
                        slider.minvalue = int.Parse(overlayQueryResult[0][0]);
                        slider.maxvalue = int.Parse(overlayQueryResult[0][1]);
                        slider.stanvalue = int.Parse(overlayQueryResult[0][0]); // change this to value in database
                        
                        // dropdown list Inputfields usercontrol for in the input place holder
                        InputFields dropdownlist = (InputFields)LoadControl("InputFields.ascx");
                        dropdownlist.in_type = "DropDownList";
                        // create the <__DropList> place holder
                        PlaceHolder ddlPH = new PlaceHolder();
                        // create the drop down list
                        DropDownList DDlist = new DropDownList();
                        // add the items for on the dropdown list
                        foreach(List<string> ddlRow in overlayQueryResult)
                        {
                            // add a way to set the currently active state in database as selected
                            DDlist.Items.Add(ddlRow[2]);
                        }
                        // set the dropdown list class ( this is to counter conflicting styling )
                        DDlist.CssClass = "dropDownMulti";

                        // add the dropdownlist to the <__DropList> place holder
                        ddlPH.Controls.Add(DDlist);
                        // set the dropdownlist <__DropList> place holder to previously created one
                        dropdownlist.__DropList = ddlPH;

                        // add the InputField controls to the <Input> place holder
                        InputPlaceHolder.Controls.Add(slider);
                        InputPlaceHolder.Controls.Add(dropdownlist);

                        // set the <Input> place holder from the widget to the created place holder
                        widget.Input = InputPlaceHolder;

                        // add the widget to the grid
                        grid_parent.Controls.Add(widget);
                    }
                    else
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
                                                foreach (List<string> specrow in appSpec)
                                                {
                                                    DPlist.Items.Add(specrow[0]);
                                                }
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
}