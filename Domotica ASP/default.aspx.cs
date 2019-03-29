﻿using Devart.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Domotica_ASP
{
    public partial class _default : System.Web.UI.Page
    {

        // source: http://www.alexandre-gomes.com/?p=137
        private class ProgressTemplate : ITemplate
        {
            #region ITemplate Members

            public void InstantiateIn(Control container)
            { }

            #endregion
        }

        public void delete_row(string naam)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            outputUpdatePanel.Attributes["class"] = "updateNotifierParent";
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

                MySqlCommand apparaatquery = new MySqlCommand("SELECT DISTINCT h.APPARAATID, a.naam, atype.`Type` FROM heefttoegangtot AS h INNER JOIN apparaat AS a ON h.APPARAATID = a.APPARAATID INNER JOIN apparaattype AS atype ON atype.TypeID = a.TypeID WHERE h.GROUPID IN( SELECT `GROUPID` FROM neemtdeelaan WHERE `userid` IN ( SELECT `userid` FROM user WHERE `gebruikersnaam` = :gbnaam))");
                apparaatquery.Parameters.Add("gbnaam", Session["user"]);
                List<List<string>> result = global.ExecuteReader(apparaatquery, out string apparaatError, out bool apparaatErrorInd);
                if (apparaatErrorInd)
                {
                    /* do something if there is an error */
                    Label label = new Label();
                    label.Text = apparaatError;
                    grid_parent.Controls.Add(label);
                }
                else
                {
                    Dictionary<string, string> tableQueryComb = new Dictionary<string, string>() { { "kantemp", "`min`, `max`" }, { "kanstand", "`stand`" } };

                    List<string> apparaatIDSmetOverlayList = new List<string>();
                    MySqlCommand apparatIDSMetOverlay = new MySqlCommand("SELECT DISTINCT kantemp.apparaatID, kanstand.apparaatID FROM kantemp, kanstand WHERE kantemp.apparaatID = kanstand.apparaatID");
                    List<List<string>> apparaatIDSmetOverlayResult = global.ExecuteReader(apparatIDSMetOverlay, out string IDError, out bool IDErrorInd);
                    if (IDErrorInd) { /* do something if there is an error */ }
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
                            List<List<string>> overlayQueryResult = global.ExecuteReader(overlayQuery, out string overlayQueryError, out bool overlayQueryErrorInd);
                            if (overlayQueryErrorInd) { /* do something if there is an error */ }
                            else
                            {
                                // create the widget
                                Widget widget = (Widget)LoadControl("Widget.ascx");
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
                                InputFields slider = (InputFields)LoadControl("InputFields.ascx");
                                slider.in_type = "ver_slider";
                                slider.minvalue = int.Parse(overlayQueryResult[0][0]);
                                slider.maxvalue = int.Parse(overlayQueryResult[0][1]);
                                slider.stanvalue = int.Parse(overlayQueryResult[0][0]); // change this to value in database
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
                                    InputFields dropdownlist = (InputFields)LoadControl("InputFields.ascx");
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
                                    InputFields num = (InputFields)LoadControl("InputFields.ascx");
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

                                // ajax:
                                // create the updatepanel
                                //UpdatePanel UpdatePanelWidget = new UpdatePanel();
                                //UpdatePanelWidget.ID = "UpdatePanel_" + row[1];
                                //UpdatePanelWidget.ContentTemplateContainer.Controls.Add(widget);

                                // add the widget to the grid
                                grid_parent.Controls.Add(widget);
                            }
                        }
                        else
                        {
                            MySqlCommand InputTypeQuery = new MySqlCommand("SELECT apparaatID, InputType, TypeWaarde FROM apparaattype AS appType INNER JOIN apparaat AS app ON appType.TypeID = app.TypeID WHERE app.apparaatid = :appID");
                            InputTypeQuery.Parameters.Add("appID", row[0]);
                            List<List<string>> inputTypeQueryResult = global.ExecuteReader(InputTypeQuery, out string InputQueryError, out bool InputQueryErrorInd);
                            if (InputQueryErrorInd) { /* do something if there is an error */ }
                            else
                            {
                                List<string> input_type = inputTypeQueryResult[0];

                                // create a toggable widget instead of special input widget
                                if (input_type[1] == "Toggle")
                                {
                                    // create widget
                                    Widget widget = (Widget)LoadControl("Widget.ascx");
                                    if (!global.dingenDieGeenDatumInputMogen.Contains(row[2]))
                                    {
                                        widget.timeField = true;
                                    }
                                    widget.name = row[1];
                                    widget.ID = row[1];
                                    widget.toggle = true;
                                    widget.submittable = true;
                                    widget.input_types = new string[] { input_type[1] };

                                    // ajax:
                                    // create the updatepanel
                                    UpdatePanel UpdatePanelWidget = new UpdatePanel();
                                    UpdatePanelWidget.ID = "UpdatePanel_" + row[1];
                                    UpdatePanelWidget.ContentTemplateContainer.Controls.Add(widget);
                                    UpdateProgress UpdateProgressControl = new UpdateProgress();
                                    // create updateprogress control
                                    UpdateProgressControl.ID = "UpdateProgress_" + row[1];
                                    UpdateProgressControl.AssociatedUpdatePanelID = "UpdatePanel_" + row[1];
                                    // add the widget to the grid
                                    grid_parent.Controls.Add(UpdatePanelWidget);
                                    grid_parent.Controls.Add(UpdateProgressControl);
                                }
                                // create a widget with a special input type
                                else
                                {
                                    // create widget
                                    Widget widget = (Widget)LoadControl("Widget.ascx");
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
                                    InputFields input = (InputFields)LoadControl("InputFields.ascx");
                                    input.ID = input_type[1]; //input_type[1] + "_" + 

                                    int type; // the type index from the dictionary
                                    if (!global.listTypes.TryGetValue(input_type[1], out type)) { throw new inputTypeException(string.Format("Given Input type does not exist! from apparaat: {0}", row[0])); } // throw an error if the input type from the database does not exist
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
                                        List<List<string>> appSpec = global.ExecuteReader(getAppSpec, out string appSpecError, out bool appSpecErrorInd);
                                        if (appSpecErrorInd) { /* do something if there is an error */}
                                        else
                                        {
                                            // create the <__radio> or <__DropList> placeholder
                                            PlaceHolder ListPH = new PlaceHolder();
                                            //ListPH.ID = "ListPH"; needed?

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
                                    // ajax:
                                    // create the updatepanel
                                    //UpdatePanel UpdatePanelWidget = new UpdatePanel();
                                    //UpdatePanelWidget.ID = "UpdatePanel_" + row[1];
                                    //UpdatePanelWidget.ContentTemplateContainer.Controls.Add(widget);

                                    // add the widget to the grid
                                    grid_parent.Controls.Add(widget);

                                }

                            }
                        }
                    }
                }
            }

            // gridview 
            DataTable dt = new DataTable("ScheduleTable");
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("apparaat", typeof(string)));
            dt.Columns.Add(new DataColumn("tijd", typeof(string)));
            dt.Columns.Add(new DataColumn("stand", typeof(string)));
            dt.Columns.Add(new DataColumn("temp", typeof(string)));
            dt.Columns.Add(new DataColumn("hidden", typeof(DateTime)));

            // get the devices with multiple values
            MySqlCommand getSchedule;
            if (Session["LoggedIn"] != null)
            {
                getSchedule = new MySqlCommand("SELECT naam, tijd, temperatuur, `stand` " +
                    "FROM temp " +
                    "INNER JOIN stand ON temp.SCHAKELID = stand.SCHAKELID " +
                    "INNER JOIN schakelschema AS ss ON temp.SCHAKELID = ss.SCHAKELID " +
                    "INNER JOIN apparaat ON ss.APPARAATID = apparaat.APPARAATID " +
                    "WHERE apparaat.apparaatID IN (" +
                        "SELECT DISTINCT h.APPARAATID " +
                        "FROM heefttoegangtot AS h " +
                        "INNER JOIN apparaat AS a ON h.APPARAATID = a.APPARAATID " +
                        "INNER JOIN apparaattype AS atype ON atype.TypeID = a.TypeID " +
                        "WHERE h.GROUPID IN( " +
                            "SELECT `GROUPID` " +
                            "FROM neemtdeelaan " +
                            "WHERE `userid` IN(" +
                                "SELECT `userid` " +
                                "FROM user " +
                                "WHERE `gebruikersnaam` = :gbnaam)))");
                getSchedule.Parameters.Add("gbnaam", Session["user"].ToString());
            }
            else
            {
                getSchedule = new MySqlCommand(
                    "SELECT naam, tijd, temperatuur, `stand` " +
                    "FROM temp " +
                    "INNER JOIN stand ON temp.SCHAKELID = stand.SCHAKELID " +
                    "INNER JOIN schakelschema AS ss ON temp.SCHAKELID = ss.SCHAKELID " +
                    "INNER JOIN apparaat ON ss.APPARAATID = apparaat.APPARAATID");
            }
            List<List<string>> getSchedule_result = global.ExecuteReader(getSchedule, out string getSchedule_error, out bool getSchedule_errorInd);
            if (getSchedule_errorInd)
            {
                /* do something with the error */
            }
            else
            {
                foreach (List<string> row in getSchedule_result)
                {
                    dr = dt.NewRow();
                    dr["apparaat"] = row[0].ToString();
                    string date = "";
                    if (row[1].Substring(4) == "-")
                    {
                        date = row[1].Substring(0, 3);
                    }
                    else
                    {
                        date = row[1].Substring(0, 4);
                    }
                    dr["tijd"] = date + " om " + row[1].Substring(row[1].Length - 8, 5);
                    dr["temp"] = row[2].ToString();
                    dr["stand"] = row[3].ToString();
                    dr["hidden"] = Convert.ToDateTime(row[1]);
                    dt.Rows.Add(dr);
                }
            }

            // get the devices with single temp value
            MySqlCommand getSchedule_single_temp;
            if (Session["LoggedIn"] != null)
            {
                getSchedule_single_temp = new MySqlCommand("SELECT naam, tijd, temperatuur " +
                    "FROM temp " +
                    "INNER JOIN schakelschema AS ss ON temp.SCHAKELID = ss.SCHAKELID " +
                    "INNER JOIN apparaat ON ss.APPARAATID = apparaat.APPARAATID " +
                    "WHERE naam NOT IN(" +
                        "SELECT naam " +
                        "FROM temp " +
                        "INNER JOIN stand ON temp.SCHAKELID = stand.SCHAKELID " +
                        "INNER JOIN schakelschema AS ss ON temp.SCHAKELID = ss.SCHAKELID " +
                        "INNER JOIN apparaat ON ss.APPARAATID = apparaat.APPARAATID)" +
                    "AND apparaat.apparaatID IN (" +
                        "SELECT DISTINCT h.APPARAATID " +
                        "FROM heefttoegangtot AS h " +
                        "INNER JOIN apparaat AS a ON h.APPARAATID = a.APPARAATID " +
                        "INNER JOIN apparaattype AS atype ON atype.TypeID = a.TypeID " +
                        "WHERE h.GROUPID IN( " +
                            "SELECT `GROUPID` " +
                            "FROM neemtdeelaan " +
                            "WHERE `userid` IN(" +
                                "SELECT `userid` " +
                                "FROM user " +
                                "WHERE `gebruikersnaam` = :gbnaam)))");
                getSchedule_single_temp.Parameters.Add("gbnaam", Session["user"].ToString());
            }
            else
            {
                getSchedule_single_temp = new MySqlCommand("SELECT naam, tijd, temperatuur " +
                    "FROM temp " +
                    "INNER JOIN schakelschema AS ss ON temp.SCHAKELID = ss.SCHAKELID " +
                    "INNER JOIN apparaat ON ss.APPARAATID = apparaat.APPARAATID " +
                    "WHERE naam NOT IN ( " +
                        "SELECT naam " +
                        "FROM temp " +
                        "INNER JOIN stand ON temp.SCHAKELID = stand.SCHAKELID " +
                        "INNER JOIN schakelschema AS ss ON temp.SCHAKELID = ss.SCHAKELID " +
                        "INNER JOIN apparaat ON ss.APPARAATID = apparaat.APPARAATID)");
            }
            List<List<string>> getSchedule_single_temp_result = global.ExecuteReader(getSchedule_single_temp, out string getSchedule_single_temp_error, out bool getSchedule_single_temp_errorInd);
            if (getSchedule_single_temp_errorInd)
            {
                /* do something with the error */
            }
            else
            {
                foreach (List<string> row in getSchedule_single_temp_result)
                {
                    dr = dt.NewRow();
                    dr["apparaat"] = row[0].ToString();
                    string date = "";
                    if (row[1].Substring(4) == "-")
                    {
                        date = row[1].Substring(0, 3);
                    }
                    else
                    {
                        date = row[1].Substring(0, 4);
                    }
                    dr["tijd"] = date + " om " + row[1].Substring(row[1].Length - 8, 5);
                    dr["temp"] = row[2].ToString();
                    dr["hidden"] = Convert.ToDateTime(row[1]);
                    dt.Rows.Add(dr);
                }
            }

            // get the devices with single stand value
            MySqlCommand getSchedule_single_stand;
            if (Session["LoggedIn"] != null)
            {
                getSchedule_single_stand = new MySqlCommand("SELECT naam, tijd, `stand` " +
                    "FROM stand INNER JOIN schakelschema AS ss ON stand.SCHAKELID = ss.SCHAKELID " +
                    "INNER JOIN apparaat ON ss.APPARAATID = apparaat.APPARAATID " +
                    "WHERE naam NOT IN ( " +
                        "SELECT naam FROM temp " +
                        "INNER JOIN stand ON temp.SCHAKELID = stand.SCHAKELID " +
                        "INNER JOIN schakelschema AS ss ON temp.SCHAKELID = ss.SCHAKELID " +
                        "INNER JOIN apparaat ON ss.APPARAATID = apparaat.APPARAATID)" +
                    "AND apparaat.apparaatID IN (" +
                        "SELECT DISTINCT h.APPARAATID " +
                        "FROM heefttoegangtot AS h " +
                        "INNER JOIN apparaat AS a ON h.APPARAATID = a.APPARAATID " +
                        "INNER JOIN apparaattype AS atype ON atype.TypeID = a.TypeID " +
                        "WHERE h.GROUPID IN( " +
                            "SELECT `GROUPID` " +
                            "FROM neemtdeelaan " +
                            "WHERE `userid` IN(" +
                                "SELECT `userid` " +
                                "FROM user " +
                                "WHERE `gebruikersnaam` = :gbnaam)))");
                getSchedule_single_stand.Parameters.Add("gbnaam", Session["user"].ToString());
            }
            else
            {
                getSchedule_single_stand = new MySqlCommand("SELECT naam, tijd, `stand` " +
                    "FROM stand " +
                    "INNER JOIN schakelschema AS ss ON stand.SCHAKELID = ss.SCHAKELID " +
                    "INNER JOIN apparaat ON ss.APPARAATID = apparaat.APPARAATID " +
                    "WHERE naam NOT IN ( " +
                        "SELECT naam " +
                        "FROM temp " +
                        "INNER JOIN stand ON temp.SCHAKELID = stand.SCHAKELID " +
                        "INNER JOIN schakelschema AS ss ON temp.SCHAKELID = ss.SCHAKELID " +
                        "INNER JOIN apparaat ON ss.APPARAATID = apparaat.APPARAATID)");
            }
            List<List<string>> getSchedule_single_stand_result = global.ExecuteReader(getSchedule_single_stand, out string getSchedule_single_stand_error, out bool getSchedule_single_standInd);
            if (getSchedule_single_standInd)
            {
                /* do something with the error */
            }
            else
            {
                foreach (List<string> row in getSchedule_single_stand_result)
                {
                    dr = dt.NewRow();
                    dr["apparaat"] = row[0].ToString();
                    string date = "";
                    if (row[1].Substring(4) == "-")
                    {
                        date = row[1].Substring(0, 3);
                    }
                    else
                    {
                        date = row[1].Substring(0, 4);
                    }
                    dr["tijd"] = date + " om " + row[1].Substring(row[1].Length - 8, 5);
                    dr["stand"] = row[2].ToString();
                    dr["hidden"] = Convert.ToDateTime(row[1]);
                    dt.Rows.Add(dr);
                }
            }

            dt.DefaultView.Sort = "tijd asc";
            dt = dt.DefaultView.ToTable();

            GridView GR = new GridView();
            GR.ID = "ScheduleDisplayer";
            GR.DataSource = dt;
            GR.RowDataBound += hide_hidden;
            if (Session["LoggedIn"] != null)
            {
                CommandField remove = new CommandField();
                remove.ButtonType = ButtonType.Button;
                remove.HeaderText = "Verwijderen";
                remove.ShowDeleteButton = true;
                remove.ShowHeader = true;
                remove.DeleteText = "Delete";

                GR.Columns.Add(remove);
                GR.RowDeleting += Schedule_Notify_delete;
            }
            ScheduleUpdatePanel.ContentTemplateContainer.Controls.Add(GR);
            GR.DataBind();
        }

        protected void hide_hidden(object sender, GridViewRowEventArgs e)
        {
            if (Session["LoggedIn"] != null)
            {
                e.Row.Cells[5].Visible = false;
            }
            else
            {
                e.Row.Cells[4].Visible = false;
            }
        }

        protected void Schedule_Notify_delete (object sender, GridViewDeleteEventArgs e)
        {
            Label lbl = new Label();
            if(sender.GetType() == typeof(GridView))
            {
                GridView GR = (GridView)sender;
                GR.SelectRow(e.RowIndex);
                TableCellCollection DC = GR.SelectedRow.Cells;
                MySqlCommand removeSchedule = new MySqlCommand("DELETE FROM schakelschema WHERE apparaatid IN (SELECT apparaatid FROM apparaat WHERE naam = :naam) AND tijd = :tijd");
                removeSchedule.Parameters.Add("naam", DC[1].Text);
                removeSchedule.Parameters.Add("tijd", Convert.ToDateTime(DC[5].Text));
                if (global.ExecuteChanger(removeSchedule, out string error))
                {
                    /* do something with the error */
                    lbl.Text = error;
                }
                else
                {
                    lbl.Text += DC[1].Text + " om " + DC[2].Text + " verwijdert";
                    GR.DeleteRow(e.RowIndex);
                    GR.DataBind();
                }
            }
            ScheduleUpdatePanel.ContentTemplateContainer.Controls.Add(lbl);
            ScheduleUpdatePanel.DataBind();
        }
    }
}