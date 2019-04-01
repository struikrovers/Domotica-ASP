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

    public class global
    {
        // dictrionaries & Lists ( static data )
        public static Dictionary<string, int> listTypes { get; set; } = new Dictionary<string, int>() {
            { "hor_slider", 1 }, { "ver_slider", 2 }, { "text", 3 }, { "number", 4 }, { "radio", 5 }, { "checkbox", 6 }, { "DropDownList", 7 }
        };
        public static List<string> dingenDieGeenDatumInputMogen = new List<string>(){ "Verwarming" };
        public static List<string> dingenDieEenTimerHebben = new List<string>() { "Oven" };
        public static List<string> pinnr = new List<string>();
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
        /// Executs the mysqlcommand given, query generation should be done beforehand
        /// </summary>
        /// <param name="query"> the mysqlcommand query </param>
        /// <param name="error"> the error message </param>
        /// <param name="errorInd"> indicator of the error </param>
        /// <returns></returns>
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
                        for(int i = 0; i < myReader.FieldCount; i++)
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
                // myReader.Close(); // < -nodig ?
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
            catch(Exception exc)
            {
                error = exc.Message;
            }
            finally
            {
                conn.Close();
            }

            return finished;
        }

        public static string getValueFromList(List<List<string>> Result)
        {
            string returned = "";
            foreach (List<string> list in Result)
            {
                foreach(string value in list)
                {
                    returned = value;
                }
            }
            return returned;
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
            MySqlCommand getSchedule;
            if (Membership.GetUser() != null)
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
                                "FROM users " +
                                "WHERE `username` = :gbnaam)))");
                getSchedule.Parameters.Add("gbnaam", Membership.GetUser().UserName);
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
            List<List<string>> getSchedule_result = ExecuteReader(getSchedule, out string getSchedule_error);
            if (getSchedule_error != "")
            {
                /* do something with the error */
                generic_QueryErrorHandler(getSchedule_error);
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
                    if(date.Hour < 10)
                    {
                        hour_add = "0";
                    }
                    if(date.Minute < 10)
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

            // get the devices with single temp value
            MySqlCommand getSchedule_single_temp;
            if (Membership.GetUser() != null)
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
                                "FROM users " +
                                "WHERE `username` = :gbnaam)))");
                getSchedule_single_temp.Parameters.Add("gbnaam", Membership.GetUser().UserName);
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
            List<List<string>> getSchedule_single_temp_result = global.ExecuteReader(getSchedule_single_temp, out string getSchedule_single_temp_error);
            if (getSchedule_single_temp_error != "")
            {
                /* do something with the error */
                global.generic_QueryErrorHandler(getSchedule_single_temp_error);
            }
            else
            {
                foreach (List<string> row in getSchedule_single_temp_result)
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
                    dr["hidden"] = Convert.ToDateTime(row[1]);
                    dt.Rows.Add(dr);
                }
            }

            // get the devices with single stand value
            MySqlCommand getSchedule_single_stand;
            if (Membership.GetUser() != null)
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
                                "FROM users " +
                                "WHERE `username` = :gbnaam)))");
                getSchedule_single_stand.Parameters.Add("gbnaam", Membership.GetUser().UserName);
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
            List<List<string>> getSchedule_single_stand_result = global.ExecuteReader(getSchedule_single_stand, out string getSchedule_single_stand_error);
            if (getSchedule_single_stand_error != "")
            {
                /* do something with the error */
                global.generic_QueryErrorHandler(getSchedule_single_stand_error);
            }
            else
            {
                foreach (List<string> row in getSchedule_single_stand_result)
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
                    dr["stand"] = row[2].ToString();
                    dr["hidden"] = Convert.ToDateTime(row[1]);
                    dt.Rows.Add(dr);
                }
            }

            dt.DefaultView.Sort = "tijd asc";
            dt = dt.DefaultView.ToTable();
            return dt;
        }
        public static bool show_delete_btn = false;

        public static void generic_QueryErrorHandler(string error)
        {
            System.Diagnostics.Trace.WriteLine(error);
        }

        // https://stackoverflow.com/a/20044767
        public static int stringToInt(string text)
        {
            int num = 0;
            for (int i = 0; i < text.Length; i++)
            {
                num += text.ToUpper()[i] - 64;
            }
            return num;
        }

        public static void updateDevices(string[] userInput, DateTime schedule, int Timer, string name, out string error)
        {
            error = "";
            // query looks like: INSERT INTO schakelschema (`apparaatid`, `tijd`) VALUES('apparaatid', 'tijd');
            MySqlCommand get_appid = new MySqlCommand("SELECT apparaatid FROM apparaat WHERE naam = :naam");
            get_appid.Parameters.Add(":naam", name);
            List<List<string>> appid = ExecuteReader(get_appid, out string get_appid_error);
            if (get_appid_error != "")
            {
                /* do something with the error */
                error = "get_app_id error: " + get_appid_error;
            }
            else
            {
                //apparaatid = appid[0][0]
                MySqlCommand create_schedule = new MySqlCommand("INSERT INTO schakelschema (`apparaatid`, `tijd`) VALUES(:apparaatid, :tijd)");
                create_schedule.Parameters.Add(":apparaatid", appid[0][0]);
                create_schedule.Parameters.Add(":tijd", schedule);
                if (!ExecuteChanger(create_schedule, out string schedule_error))
                {
                    /* do something with the error */
                    error = "create_schedule error: " + schedule_error;
                }
                else
                {
                    // get schakelID: SELECT schakelID FROM schakelschema WHERE apparaatid = apparaatid AND tijd = tijd;
                    MySqlCommand get_schakelid = new MySqlCommand("SELECT schakelid FROM schakelschema WHERE apparaatid = :appid AND tijd = :tijd");
                    get_schakelid.Parameters.Add(":appid", appid[0][0]);
                    get_schakelid.Parameters.Add(":tijd", schedule);
                    List<List<string>> schakelid = ExecuteReader(get_schakelid, out string get_schakelid_error);
                    if (get_schakelid_error != "")
                    {
                        /* do something with the error */
                        error = "get_schakelid error: " + get_schakelid_error;
                    }
                    else
                    {
                        if(Timer != 999)
                        {
                            MySqlCommand setTimer = new MySqlCommand("INSERT INTO timer(schakelid, uit_tijd) VALUES(:schakelid, :tijd)");
                            setTimer.Parameters.Add(":schakelid", schakelid[0][0]);
                            setTimer.Parameters.Add(":tijd", schedule.AddMinutes(Timer));
                            if(!ExecuteChanger(setTimer, out string setTimerError)){
                                /* do something with the error */
                                error = "setTimer error: " + setTimerError;
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
                                        MySqlCommand add_temp = new MySqlCommand("INSERT INTO temp (`schakelid`, `temperatuur`) VALUES (:schakelid, :temp)");
                                        add_temp.Parameters.Add(":schakelid", schakelid[0][0]);
                                        add_temp.Parameters.Add(":temp", num);
                                        if (!ExecuteChanger(add_temp, out string add_temp_error))
                                        {
                                            /* do something with the error */
                                            error = "add_temp error: " + add_temp_error;
                                        }
                                    }
                                    else
                                    {
                                        // it's a setting
                                        // set setting: INSERT INTO stand (`schakelid`, `stand`) VALUES ('schakelid', 'stand')
                                        MySqlCommand add_stand = new MySqlCommand("INSERT INTO stand (`schakelid`, `stand`) VALUES (:schakelid, :stand)");
                                        add_stand.Parameters.Add(":schakelid", schakelid[0][0]);
                                        add_stand.Parameters.Add(":stand", input);
                                        if (!ExecuteChanger(add_stand, out string add_stand_error))
                                        {
                                            /* do something with the error */
                                            error = "add_stand error: " + add_stand_error;
                                        }
                                    }
                                }
                                else
                                {
                                    // it's a toggle
                                    // set stand to "aan": INSERT INTO stand (`schakelid`, `stand`) VALUES ('schakelid', 'toggle')
                                    MySqlCommand add_toggle = new MySqlCommand("INSERT INTO stand (`schakelid`, `stand`) VALUES (:schakelid, :stand)");
                                    add_toggle.Parameters.Add(":schakelid", schakelid[0][0]);
                                    add_toggle.Parameters.Add(":stand", toggle);
                                    if (!ExecuteChanger(add_toggle, out string add_toggle_error))
                                    {
                                        /* do something with the error */
                                        error = "add_toggle error: " + add_toggle_error;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // password protection by: https://stackoverflow.com/a/32191537
    public static class SecurePasswordHasher
    {
        /// <summary>
        /// Size of salt.
        /// </summary>
        private const int SaltSize = 16;

        /// <summary>
        /// Size of hash.
        /// </summary>
        private const int HashSize = 20;

        /// <summary>
        /// Creates a hash from a password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="iterations">Number of iterations.</param>
        /// <returns>The hash.</returns>
        public static string Hash(string password, int iterations)
        {
            // Create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            // Combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            // Format hash with extra information
            return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
        }

        /// <summary>
        /// Creates a hash from a password with 10000 iterations
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>The hash.</returns>
        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }

        /// <summary>
        /// Checks if hash is supported.
        /// </summary>
        /// <param name="hashString">The hash.</param>
        /// <returns>Is supported?</returns>
        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$MYHASH$V1$");
        }

        /// <summary>
        /// Verifies a password against a hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="hashedPassword">The hash.</param>
        /// <returns>Could be verified?</returns>
        public static bool Verify(string password, string hashedPassword)
        {
            // Check hash
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            // Extract iteration and Base64 string
            var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            // Get hash bytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            // Get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}