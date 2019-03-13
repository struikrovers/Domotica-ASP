using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Devart.Data.MySql;

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
        public static List<string> ExecuteReader(MySqlCommand query, out string error, out bool errorInd)
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["demotica_conn"].ToString());
            query.Connection = conn;
            error = "";
            errorInd = false;
            List<string> result = new List<string>();
            try
            {
                conn.Open();
                MySqlDataReader myReader = query.ExecuteReader();
                try
                {
                    while (myReader.Read())
                    {
                        foreach (string value in myReader)
                        {
                            result.Add(value.ToString());
                        }
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
    }
}