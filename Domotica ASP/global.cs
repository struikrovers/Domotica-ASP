using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Devart.Data.MySql;
using System.Security.Cryptography;

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


        /// <summary>
        /// Executs the mysqlcommand given, query generation should be done beforehand
        /// </summary>
        /// <param name="query"> the mysqlcommand query </param>
        /// <param name="error"> the error message </param>
        /// <param name="errorInd"> indicator of the error </param>
        /// <returns></returns>
        public static List<List<string>> ExecuteReader(MySqlCommand query, out string error, out bool errorInd)
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["demotica_conn"].ToString());
            query.Connection = conn;
            error = "";
            errorInd = false;
            
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

            if(error != "")
            {
                errorInd = true;
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

        public static bool checkUserCookie(HttpCookie verkade, out string error)
        {
            MySqlCommand UserIDQuery = new MySqlCommand("SELECT USERID FROM `user` WHERE gebruikersnaam = :gbnaam");
            UserIDQuery.Parameters.Add("gbnaam", verkade["username"]);
            List<List<string>> UserIDqueryResult = ExecuteReader(UserIDQuery, out string UserIDQueryError, out bool UserIDQueryErrorInd);
            if (UserIDQueryErrorInd)
            {
                /* do something if there is an error */
                error = UserIDQueryError;
                return false;
            }
            else {
                error = "";
                int userID = int.Parse(getValueFromList(UserIDqueryResult));
                int username_num = global.stringToInt(verkade["username"]);
                if (verkade["userkey"] == ((int.Parse(verkade["salt"]) * (userID * 10)) * username_num).ToString())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
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
            List<List<string>> appid = ExecuteReader(get_appid, out string get_appid_error, out bool get_appid_error_ind);
            if (get_appid_error_ind)
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
                    List<List<string>> schakelid = ExecuteReader(get_schakelid, out string get_schakelid_error, out bool get_schakelid_error_ind);
                    if (get_schakelid_error_ind)
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