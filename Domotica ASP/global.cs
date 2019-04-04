using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Devart.Data.MySql;
using System.Security.Cryptography;
using System.Data;
using System.Web.Security;

namespace Domotica_ASP
{
    public class inputTypeException : Exception
    {
        public inputTypeException()
        {

        }

        public inputTypeException(string message) : base(message)
        {
        }

        public inputTypeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class QueryErrorException : Exception
    {
        public QueryErrorException()
        {

        }

        public QueryErrorException(string message) : base(message)
        {
        }

        public QueryErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class global
    {
        // dictrionaries & Lists ( static data )
        public static Dictionary<string, int> listTypes { get; set; } = new Dictionary<string, int>() {
            { "hor_slider", 1 }, { "ver_slider", 2 }, { "text", 3 }, { "number", 4 }, { "radio", 5 }, { "checkbox", 6 }, { "DropDownList", 7 }
        };
        public static List<string> dingenDieGeenDatumInputMogen { get; set; } = new List<string>() { "Verwarming" };
        public static List<string> dingenDieEenTimerHebben { get; set; } = new List<string>() { "Oven" };
        public static List<string> pinnr { get; set; } = new List<string>();
        public static void init_pinnr()
        {
            if (pinnr.Count == 0)
            {
                for (int i = 1; i < 21; i++)
                {
                    pinnr.Add("D" + i.ToString());
                }
            }
        }

        /// <summary>
        /// Executes the mysqlcommand given, query generation should be done beforehand
        /// </summary>
        /// <param name="query"> the mysqlcommand query </param>
        /// <param name="error"> the error message </param>
        /// <returns>Returns a List<list<string>> which corresponds to the outputted table, in form of: row -> column</returns>
        public static List<List<string>> ExecuteReader(MySqlCommand query, out string error)
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["demotica_conn"].ToString());
            query.Connection = conn;
            error = "";

            List<List<string>> result = new List<List<string>>();
            try
            {
                conn.Open();
                MySqlDataReader myReader = query.ExecuteReader();
                try
                {
                    while (myReader.Read())
                    {
                        // creating a list of the current columns
                        List<string> result_inner = new List<string>();
                        for (int i = 0; i < myReader.FieldCount; i++)
                        {
                            result_inner.Add(myReader.GetValue(i).ToString());
                        }
                        result.Add(result_inner);
                    }
                }
                catch (Exception exc)
                {
                    error = "Error while reading query: " + exc.Message;
                }
            }
            catch (Exception exc)
            {
                error = "Error while connecting: " + exc.Message;
            }
            finally
            {
                // always call Close when done reading.
                conn.Close();
            }
            return result;
        }

        public static bool ExecuteChanger(MySqlCommand query, out string error)
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["demotica_conn"].ToString());
            query.Connection = conn;
            bool finished = false;
            error = "";
            try
            {
                conn.Open();
                query.ExecuteNonQuery();
                finished = true;
            }
            catch (Exception exc)
            {
                error = exc.Message;
            }
            finally
            {
                conn.Close();
            }

            return finished;
        }

        public static DataTable GetScheduleTable()
        {
            // gridview 
            DataTable dt = new DataTable("ScheduleTable");
            DataRow dr = null;
            dt.Columns.Add(new DataColumn("apparaat", typeof(string)));
            dt.Columns.Add(new DataColumn("tijd", typeof(string)));
            dt.Columns.Add(new DataColumn("stand", typeof(string)));
            dt.Columns.Add(new DataColumn("temp", typeof(string)));
            dt.Columns.Add(new DataColumn("hidden", typeof(DateTime)));

            // get the devices with multiple values
            MySqlCommand getSchedule = new MySqlCommand(
                    "SELECT naam, ss.tijd, temp.temperatuur, `stand`.stand " +
                    "FROM `stand` " +
                    "RIGHT JOIN schakelschema AS ss ON ss.SCHAKELID = `stand`.SCHAKELID " +
                    "INNER JOIN apparaat ON ss.APPARAATID = apparaat.APPARAATID " +
                    "LEFT JOIN temp ON temp.SCHAKELID = ss.SCHAKELID");
            List<List<string>> getSchedule_result = ExecuteReader(getSchedule, out string getSchedule_error);
            if (getSchedule_error != "")
            {
                /* do something with the error */
                generic_QueryErrorHandler(getSchedule, getSchedule_error);
            }
            else
            {
                foreach (List<string> row in getSchedule_result)
                {
                    dr = dt.NewRow();
                    dr["apparaat"] = row[0].ToString();
                    DateTime date = Convert.ToDateTime(row[1]);
                    string hour_add = "";
                    string minute_add = "";
                    if (date.Hour < 10)
                    {
                        hour_add = "0";
                    }
                    if (date.Minute < 10)
                    {
                        minute_add = "0";
                    }

                    dr["tijd"] = date.Day.ToString() + "-" + date.Month.ToString() + " om " + hour_add + date.Hour.ToString() + ":" + minute_add + date.Minute.ToString();
                    dr["temp"] = row[2].ToString();
                    dr["stand"] = row[3].ToString();
                    dr["hidden"] = Convert.ToDateTime(row[1]);
                    dt.Rows.Add(dr);
                }
            }

            dt.DefaultView.Sort = "tijd asc";
            dt = dt.DefaultView.ToTable();
            return dt;
        }
        public static bool show_delete_btn { get; set; } = false;

        public static void generic_QueryErrorHandler(MySqlCommand error_command, string error)
        {
            System.Diagnostics.Trace.WriteLine(error);
            throw new QueryErrorException(string.Format("Error in query: {0} \n Error Feedback: {1}", error_command.CommandText, error));
        }

        public static void updateDevices(string[] userInput, DateTime schedule, int Timer, string name, out string error)
        {
            error = "";
            // query looks like: INSERT INTO schakelschema (`apparaatid`, `tijd`) VALUES('apparaatid', 'tijd');
            MySqlCommand create_schedule = new MySqlCommand("INSERT INTO schakelschema (`apparaatid`, `tijd`) SELECT apparaatid, :tijd FROM apparaat WHERE naam = :naam");
            create_schedule.Parameters.Add(":naam", name);
            create_schedule.Parameters.Add(":tijd", schedule);
            if (!ExecuteChanger(create_schedule, out string schedule_error))
            {
                /* do something with the error */
                error = "create_schedule error: " + schedule_error;
                generic_QueryErrorHandler(create_schedule, error);
            }
            else
            {
                if (Timer != 999)
                {
                    MySqlCommand setTimer = new MySqlCommand("INSERT INTO timer(schakelid, uit_tijd) " +
                        "SELECT schakelschema.SCHAKELID, :tijd_uit " +
                        "FROM schakelschema " +
                        "WHERE apparaatid IN ( " +
                            "SELECT apparaatid " +
                            "FROM apparaat " +
                            "WHERE naam = :naam ) " +
                        "AND tijd = :tijd");
                    setTimer.Parameters.Add(":naam", name);
                    setTimer.Parameters.Add(":tijd_uit", schedule.AddMinutes(Timer));
                    setTimer.Parameters.Add(":tijd", schedule);
                    if (!ExecuteChanger(setTimer, out string setTimerError))
                    {
                        /* do something with the error */
                        error = "setTimer error: " + setTimerError;
                        generic_QueryErrorHandler(setTimer, error);
                    }

                }
                //schakelid = schakelid[0][0];
                foreach (string input in userInput)
                {
                    if (input != null)
                    {
                        if (!bool.TryParse(input, out bool toggle))
                        {
                            // not a toggle
                            if (int.TryParse(input, out int num))
                            {
                                // it's a temperature
                                // set temp: INSERT INTO temp (`schakelid`, `temp`) VALUES ('schakelid', 'temp')
                                MySqlCommand add_temp = new MySqlCommand("INSERT INTO temp (`schakelid`, `temperatuur`) " +
                                    "SELECT schakelschema.SCHAKELID, :temp " +
                                    "FROM schakelschema " +
                                    "WHERE apparaatid IN ( " +
                                        "SELECT apparaatid " +
                                        "FROM apparaat " +
                                        "WHERE naam = :naam ) " +
                                    "AND tijd = :tijd");
                                add_temp.Parameters.Add(":naam", name);
                                add_temp.Parameters.Add(":temp", num);
                                add_temp.Parameters.Add(":tijd", schedule);
                                if (!ExecuteChanger(add_temp, out string add_temp_error))
                                {
                                    /* do something with the error */
                                    error = "add_temp error: " + add_temp_error;
                                    generic_QueryErrorHandler(add_temp, error);
                                }
                            }
                            else
                            {
                                // it's a setting
                                // set setting: INSERT INTO stand (`schakelid`, `stand`) VALUES ('schakelid', 'stand')
                                MySqlCommand add_stand = new MySqlCommand("INSERT INTO stand (`schakelid`, `stand`) " +
                                    "SELECT schakelschema.SCHAKELID, :stand " +
                                    "FROM schakelschema " +
                                    "WHERE apparaatid IN ( " +
                                        "SELECT apparaatid " +
                                        "FROM apparaat " +
                                        "WHERE naam = :naam ) " +
                                    "AND tijd = :tijd");
                                add_stand.Parameters.Add(":naam", name);
                                add_stand.Parameters.Add(":stand", input);
                                add_stand.Parameters.Add(":tijd", schedule);
                                if (!ExecuteChanger(add_stand, out string add_stand_error))
                                {
                                    /* do something with the error */
                                    error = "add_stand error: " + add_stand_error;
                                    generic_QueryErrorHandler(add_stand, error);
                                }
                            }
                        }
                        else
                        {
                            // it's a toggle
                            // set stand to "aan": INSERT INTO stand (`schakelid`, `stand`) VALUES ('schakelid', 'toggle')
                            MySqlCommand add_toggle = new MySqlCommand("INSERT INTO stand (`schakelid`, `stand`) " +
                                    "SELECT schakelschema.SCHAKELID, :stand " +
                                    "FROM schakelschema " +
                                    "WHERE apparaatid IN ( " +
                                        "SELECT apparaatid " +
                                        "FROM apparaat " +
                                        "WHERE naam = :naam ) " +
                                    "AND tijd = :tijd");
                            add_toggle.Parameters.Add(":naam", name);
                            add_toggle.Parameters.Add(":stand", toggle);
                            add_toggle.Parameters.Add(":tijd", schedule);
                            if (!ExecuteChanger(add_toggle, out string add_toggle_error))
                            {
                                /* do something with the error */
                                error = "add_toggle error: " + add_toggle_error;
                                generic_QueryErrorHandler(add_toggle, error);
                            }
                        }
                    }
                }
            }
        }
    }
}