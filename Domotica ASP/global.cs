using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Devart.Data.MySql;
using System.Security.Cryptography;

namespace Domotica_ASP
{
    public class global
    {
        /// <summary>
        /// Executs the mysqlcommand given, query generation should be done beforehand
        /// </summary>
        /// <param name="query"> the mysqlcommand query </param>
        /// <param name="error"> the error message </param>
        /// <param name="errorInd"> indicator of the error </param>
        /// <returns></returns>
        public static List<dynamic> ExecuteReader(MySqlCommand query, out string error, out bool errorInd)
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["demotica_conn"].ToString());
            query.Connection = conn;
            error = "";
            errorInd = false;
            // why dynamic you might ask? well because there are different data types in the database ofcourse! let the compiler fix the datatypes for you! source: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/dynamic
            List<dynamic> result = new List<dynamic>();
            try
            {
                conn.Open();
                MySqlDataReader myReader = query.ExecuteReader();
                try
                {
                    while (myReader.Read())
                    {
                        // creating a list of the current columns
                        List<dynamic> result_inner = new List<dynamic>();
                        for(int i = 0; i < myReader.FieldCount; i++)
                        {
                            result_inner.Add(myReader.GetValue(i));
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

        public static dynamic getValueFromList(List<dynamic> Result)
        {
            dynamic returned = "";
            foreach (dynamic list in Result)
            {
                foreach(dynamic value in list)
                {
                    returned = value;
                }
            }
            return returned;
        }

        public static bool checkUserCookie(HttpCookie verkade, out string error, out bool errorInd)
        {
            MySqlCommand query2 = new MySqlCommand("SELECT USERID FROM `user` WHERE gebruikersnaam = :gbnaam");
            query2.Parameters.Add("gbnaam", verkade["username"]);
            List<dynamic> result = global.ExecuteReader(query2, out string error2, out bool errorInd2);
            if (errorInd2)
            {
                error = error2;
                errorInd = true;
                return false;
            }
            else {
                error = "";
                errorInd = false;
                int userID = global.getValueFromList(result);
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