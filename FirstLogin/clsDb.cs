using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace FirstLogin
{
    public class clsDb
    {
        MySqlConnection conn = new MySqlConnection("Server=localhost;Database=firstlogin;Uid=root;Pwd=root;");

        public void AddUser(string sUserName, string sPassword)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "insert into users (Username, Password, Highscore) values (@username, @password, '0')";
                cmd.Parameters.AddWithValue("@username", sUserName);
                cmd.Parameters.AddWithValue("@password", sPassword);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
            
            
        }

        public int GetUserID(string sUsername, string sPassword)
        {
            int returnvalue = 0;
            try
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select id from users where Username = @username and Password = @password";
                cmd.Parameters.AddWithValue("@username", sUsername);
                cmd.Parameters.AddWithValue("@password", sPassword);

                string sUserID = cmd.ExecuteScalar().ToString();
                returnvalue = int.Parse(sUserID);
            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return returnvalue;
        }

        public string GetUsername(int iUserID)
        {
            string returnvalue = " ";
            try
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select username from users where id = @id";
                cmd.Parameters.AddWithValue("@id", iUserID.ToString());

                string sUsername = cmd.ExecuteScalar().ToString();
                returnvalue = sUsername;
            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return returnvalue;
        }

        public string GetHighscore(int iUserID)
        {
            string returnvalue = " ";
            try
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select highscore from users where id = @id";
                cmd.Parameters.AddWithValue("@id", iUserID.ToString());

                string sUsername = cmd.ExecuteScalar().ToString();
                returnvalue = sUsername;
            }
            catch (Exception)
            {

            }
            finally
            {
                conn.Close();
            }
            return returnvalue;
        }

        public void AddHighscore(int iHighscore, int iUserID)
        {
            try
            {
                conn.Open();
                MySqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "update users set Highscore = @highscore where ID = @id";
                cmd.Parameters.AddWithValue("@highscore", iHighscore);
                cmd.Parameters.AddWithValue("@id", iUserID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
