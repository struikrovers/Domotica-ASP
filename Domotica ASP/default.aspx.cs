using Devart.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotica_ASP
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            outputUpdatePanel.Attributes["class"] = "updateNotifierParent";
            ScheduleUpdatePanel.Attributes["class"] = "ScheduleUpdatePanel";

            if (Membership.GetUser() != null)
            {

                MySqlCommand apparaatquery = new MySqlCommand("SELECT DISTINCT h.APPARAATID, a.naam, atype.`Type` FROM heefttoegangtot AS h INNER JOIN apparaat AS a ON h.APPARAATID = a.APPARAATID INNER JOIN apparaattype AS atype ON atype.TypeID = a.TypeID WHERE h.GROUPID IN( SELECT `GROUPID` FROM neemtdeelaan WHERE `userid` IN ( SELECT `id` FROM users WHERE `username` = :gbnaam))");
                apparaatquery.Parameters.Add("gbnaam", Membership.GetUser().UserName);
                List<List<string>> result = global.ExecuteReader(apparaatquery, out string apparaatError);
                if (apparaatError != "")
                {
                    /* do something with the error */
                    global.generic_QueryErrorHandler(apparaatquery, apparaatError);
                }
                else
                {
                    Dictionary<string, string> tableQueryComb = new Dictionary<string, string>() { { "kantemp", "`min`, `max`" }, { "kanstand", "`stand`" } };

                    List<string> apparaatIDSmetOverlayList = new List<string>();
                    MySqlCommand apparatIDSMetOverlay = new MySqlCommand("SELECT DISTINCT kantemp.apparaatID, kanstand.apparaatID FROM kantemp, kanstand WHERE kantemp.apparaatID = kanstand.apparaatID");
                    List<List<string>> apparaatIDSmetOverlayResult = global.ExecuteReader(apparatIDSMetOverlay, out string IDError);
                    if (IDError != "")
                    { 
                        /* do something with the error */
                        global.generic_QueryErrorHandler(apparatIDSMetOverlay, IDError);
                    }
                    else
                    {
                        foreach (List<string> row in apparaatIDSmetOverlayResult)
                        {
                            apparaatIDSmetOverlayList.Add(row[0]);
                        }
                    }

                    foreach (List<string> row in result)
                    {
                        if (apparaatIDSmetOverlayList.Contains(row[0]))
                        {
                            // get multiple values
                            MySqlCommand overlayQuery = new MySqlCommand("SELECT `min`, `max`, `stand` FROM kanstand INNER JOIN kantemp ON kanstand.APPARAATID = kantemp.APPARAATID WHERE kanstand.APPARAATID = :appID");
                            overlayQuery.Parameters.Add("appID", row[0]);
                            List<List<string>> overlayQueryResult = global.ExecuteReader(overlayQuery, out string overlayQueryError);
                            if (overlayQueryError != "")
                            { 
                                /* do something with the error */
                                global.generic_QueryErrorHandler(overlayQuery, overlayQueryError);
                            }
                            else
                            {
                                // create the widget
                                Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                                widget.ID = row[1];
                                widget.name = row[1];
                                if (!global.dingenDieGeenDatumInputMogen.Contains(row[2]))
                                {
                                    widget.timeField = true;
                                }
                                widget.submittable = true;

                                // create the <Input> place holder
                                PlaceHolder InputPlaceHolder = new PlaceHolder();

                                // horizontal slider InputFields Usercontrol for in the Input place holder
                                InputFields slider = (InputFields)LoadControl("~/UserControls/InputFields.ascx");
                                slider.in_type = "ver_slider";
                                slider.minvalue = int.Parse(overlayQueryResult[0][0]);
                                slider.maxvalue = int.Parse(overlayQueryResult[0][1]);
                                slider.stanvalue = int.Parse(overlayQueryResult[0][0]);
                                slider.ID = "ver_slider";
                                // add the slider to the <Input> place holder
                                InputPlaceHolder.Controls.Add(slider);

                                if (overlayQueryResult[0].Contains("toggle"))
                                {
                                    widget.toggle = true;
                                    widget.input_types = new string[] { "Toggle", "ver_slider" };
                                }
                                else
                                {
                                    widget.input_types = new string[] { "ver_slider", "DropDownList", "number" };
                                    // dropdown list Inputfields usercontrol for in the input place holder
                                    InputFields dropdownlist = (InputFields)LoadControl("~/UserControls/InputFields.ascx");
                                    dropdownlist.in_type = "DropDownList";
                                    dropdownlist.ID = "DropDownList";

                                    // create the <__DropList> place holder
                                    PlaceHolder ddlPH = new PlaceHolder();
                                    // create the drop down list
                                    DropDownList DDlist = new DropDownList();
                                    // add the items for on the dropdown list
                                    foreach (List<string> ddlRow in overlayQueryResult)
                                    {
                                        // add a way to set the currently active state in database as selected
                                        DDlist.Items.Add(ddlRow[2]);
                                    }
                                    // set the dropdown list class ( this is to counter conflicting styling )
                                    DDlist.CssClass = "dropDownMulti";
                                    DDlist.ID = "DropDownListInput";

                                    // add the dropdownlist to the <__DropList> place holder
                                    ddlPH.Controls.Add(DDlist);
                                    // set the dropdownlist <__DropList> place holder to previously created one
                                    dropdownlist.__DropList = ddlPH;
                                    // add the dropdownlist
                                    InputPlaceHolder.Controls.Add(dropdownlist);
                                }

                                if (global.dingenDieEenTimerHebben.Contains(row[2]))
                                {
                                    InputFields num = (InputFields)LoadControl("~/UserControls/InputFields.ascx");
                                    num.in_type = "number";
                                    num.minvalue = 0;
                                    num.maxvalue = 60;
                                    num.ID = "number";
                                    TextBox numb = (TextBox)num.FindControl("NumberInput");
                                    numb.Attributes["onchange"] = "fixValue(this)";
                                    numb.Attributes["class"] = "inputTimer";

                                    InputPlaceHolder.Controls.Add(num);
                                }

                                // set the <Input> place holder from the widget to the created place holder
                                widget.Input = InputPlaceHolder;

                                // add the widget to the grid
                                grid_parent.Controls.Add(widget);
                            }
                        }
                        else
                        {
                            MySqlCommand InputTypeQuery = new MySqlCommand("SELECT apparaatID, InputType, TypeWaarde FROM apparaattype AS appType INNER JOIN apparaat AS app ON appType.TypeID = app.TypeID WHERE app.apparaatid = :appID");
                            InputTypeQuery.Parameters.Add("appID", row[0]);
                            List<List<string>> inputTypeQueryResult = global.ExecuteReader(InputTypeQuery, out string InputQueryError);
                            if (InputQueryError != "")
                            {
                                /* do something with the error */
                                global.generic_QueryErrorHandler(InputTypeQuery, InputQueryError);
                            }
                            else
                            {
                                List<string> input_type = inputTypeQueryResult[0];

                                // create a toggable widget instead of special input widget
                                if (input_type[1] == "Toggle")
                                {
                                    // create widget
                                    Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                                    if (!global.dingenDieGeenDatumInputMogen.Contains(row[2]))
                                    {
                                        widget.timeField = true;
                                    }
                                    widget.name = row[1];
                                    widget.ID = row[1];
                                    widget.toggle = true;
                                    widget.submittable = true;
                                    widget.input_types = new string[] { input_type[1] };

                                    // add the widget to the grid
                                    grid_parent.Controls.Add(widget);
                                }
                                // create a widget with a special input type
                                else
                                {
                                    // create widget
                                    Widget widget = (Widget)LoadControl("~/UserControls/Widget.ascx");
                                    if (!global.dingenDieGeenDatumInputMogen.Contains(row[2]))
                                    {
                                        widget.timeField = true;
                                    }
                                    widget.submittable = true;
                                    widget.name = row[1];
                                    widget.ID = row[1];
                                    widget.input_types = new string[] { input_type[1] };

                                    // create <Input> placeholder tag
                                    PlaceHolder inputPH = new PlaceHolder();

                                    // create InputFields user control
                                    InputFields input = (InputFields)LoadControl("~/UserControls/InputFields.ascx");
                                    input.ID = input_type[1];

                                    int type; // the type index from the dictionary
                                    // throw an error if the input type from the database does not exist
                                    if (!global.listTypes.TryGetValue(input_type[1], out type)) { throw new inputTypeException(string.Format("Given Input type does not exist! from apparaat: {0}", row[0])); } 
                                    else
                                    {
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
                                        // get the apparaat specs
                                        List<List<string>> appSpec = global.ExecuteReader(getAppSpec, out string appSpecError);
                                        if (appSpecError != "")
                                        {
                                            /* do something with the error */
                                            global.generic_QueryErrorHandler(getAppSpec, appSpecError);
                                        }
                                        else
                                        {
                                            // create the <__radio> or <__DropList> placeholder
                                            PlaceHolder ListPH = new PlaceHolder();
                                            
                                            // set the special input parameters of the widget
                                            switch (type)
                                            {
                                                case 1:
                                                    // horizontal slider
                                                    input.minvalue = int.Parse(appSpec[0][0]);
                                                    input.maxvalue = int.Parse(appSpec[0][1]);
                                                    input.stanvalue = int.Parse(appSpec[0][0]);
                                                    ListPH.Dispose(); // delete the ListPH placeholder if it is not used
                                                    break;

                                                case 2:
                                                    // vertical slider
                                                    input.minvalue = int.Parse(appSpec[0][0]);
                                                    input.maxvalue = int.Parse(appSpec[0][1]);
                                                    input.stanvalue = int.Parse(appSpec[0][0]);
                                                    ListPH.Dispose(); // delete the ListPH placeholder if it is not used
                                                    break;

                                                case 3:
                                                    // text
                                                    ListPH.Dispose(); // delete the ListPH placeholder if it is not used
                                                    break;

                                                case 4:
                                                    // number
                                                    ListPH.Dispose(); // delete the ListPH placeholder if it is not used
                                                    break;

                                                case 5:
                                                    // radio
                                                    RadioButtonList RBlist = new RadioButtonList(); // create a radio
                                                    RBlist.ID = "RadioButtonListInput";
                                                    foreach (List<string> specrow in appSpec)
                                                    {
                                                        RBlist.Items.Add(specrow[0]);
                                                    }
                                                    ListPH.Controls.Add(RBlist);
                                                    input.__Radio = ListPH;
                                                    break;

                                                case 6:
                                                    // checkbox
                                                    CheckBoxList CKlist = new CheckBoxList();
                                                    CKlist.ID = "CheckboxListInput";
                                                    input_type[1] = "radio";
                                                    foreach (List<string> specrow in appSpec)
                                                    {
                                                        CKlist.Items.Add(specrow[0]);
                                                    }
                                                    ListPH.Controls.Add(CKlist);
                                                    input.__Radio = ListPH;
                                                    break;

                                                case 7:
                                                    // dropdownlist
                                                    DropDownList DPlist = new DropDownList();
                                                    DPlist.ID = "DropDownListInput";
                                                    foreach (List<string> specrow in appSpec)
                                                    {
                                                        DPlist.Items.Add(specrow[0]);
                                                    }
                                                    ListPH.Controls.Add(DPlist);
                                                    input.__DropList = ListPH;
                                                    break;

                                                default:
                                                    ListPH.Dispose(); // delete the ListPH placeholder if it is not used
                                                    break;
                                            }
                                            // set the in_type of the input control
                                            input.in_type = input_type[1];
                                        }
                                    }
                                    // add the input control to the <input> placeholder
                                    inputPH.Controls.Add(input);
                                    // set the widget <input> placeholder to inputPH
                                    widget.Input = inputPH;

                                    // add the widget to the grid
                                    grid_parent.Controls.Add(widget);

                                }

                            }
                        }
                    }
                }
            }

            ScheduleDisplayer.RowDataBound += hide_hidden;
            ScheduleDisplayer.RowDeleting += Schedule_delete;

            DataTable dt = global.GetScheduleTable();
            if (dt.Rows.Count == 0)
            {
                DataRow emp_dr = dt.NewRow();
                emp_dr["apparaat"] = "Currently";
                emp_dr["tijd"] = "No";
                emp_dr["stand"] = "Devices";
                emp_dr["temp"] = "Scheduled";
                emp_dr["hidden"] = DateTime.Now;
                dt.Rows.Add(emp_dr);
                global.show_delete_btn = false;
            }
            else
            {
                global.show_delete_btn = true;
            }
            ScheduleDisplayer.DataSource = dt;
            ScheduleDisplayer.DataBind();
        }

        protected void hide_hidden(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[5].Visible = false;
            if (Membership.GetUser() == null || !global.show_delete_btn)
            {
                e.Row.Cells[0].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((Button)e.Row.Cells[0].Controls[0]).OnClientClick = "OpenUpdater();"; // add any JS you want here
            }
        }

        protected void Schedule_delete (object sender, GridViewDeleteEventArgs e)
        {
            output.Text = "";
            if (sender.GetType() == typeof(GridView))
            {
                GridView GR = (GridView)sender;
                GR.SelectRow(e.RowIndex);
                TableCellCollection DC = GR.SelectedRow.Cells;
                MySqlCommand removeSchedule = new MySqlCommand("DELETE FROM schakelschema WHERE (`apparaatid` IN (SELECT `apparaatid` FROM apparaat WHERE `naam` = :naam)) AND (`tijd` = :tijd)");
                removeSchedule.Parameters.Add("naam", DC[1].Text);
                removeSchedule.Parameters.Add("tijd", Convert.ToDateTime(DC[5].Text));
                if (!global.ExecuteChanger(removeSchedule, out string removeSchedule_error))
                {
                    global.generic_QueryErrorHandler(removeSchedule, removeSchedule_error);
                    /* do something with the error */
                }
                else
                {
                    output.Text = DC[1].Text + " om " + DC[2].Text + " verwijdert";
                    DataTable dt = global.GetScheduleTable();
                    if (dt.Rows.Count == 0)
                    {
                        DataRow emp_dr = dt.NewRow();
                        emp_dr["apparaat"] = "Currently";
                        emp_dr["tijd"] = "No";
                        emp_dr["stand"] = "Devices";
                        emp_dr["temp"] = "Scheduled";
                        emp_dr["hidden"] = DateTime.Now;
                        dt.Rows.Add(emp_dr);
                        global.show_delete_btn = false;
                    }
                    else
                    {
                        global.show_delete_btn = true;
                    }
                    ScheduleDisplayer.DataSource = dt;
                    ScheduleDisplayer.DataBind();
                }
            }
        }
    }
}