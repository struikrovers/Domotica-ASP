using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using Devart.Data.MySql;

namespace Domotica_ASP
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string username = UsernameInput.Text;
            string password = PasswordInput.Text;

            MySqlConnection conn = new MySqlConnection();
            conn.ConnectionString = ConfigurationManager.ConnectionStrings["demotica_conn"].ToString();

            MySqlCommand query = new MySqlCommand("SELECT * FROM user", conn);
            conn.Open();
            MySqlDataReader myReader = query.ExecuteReader();
            try
            {
                while (myReader.Read())
                {
                    Label1.Text = myReader.GetInt64(0).ToString();
                }
            }
            finally
            {
                // always call Close when done reading.
                myReader.Close();
                // always call Close when done reading.
                conn.Close();
            }

            /*
            string error; bool errorInd;
            List<string> result = global.ExecuteReader(query, out error, out errorInd);
            if (errorInd)
            {
                Label1.Text = error;
            }
            else
            {
                foreach (string value in result)
                {
                    Label1.Text += value + " <br> \n";
                }
            }
            */
        }
    }
}