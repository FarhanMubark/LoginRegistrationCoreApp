using System.Data;
using System.Data.SqlClient;

namespace LoginRegistrationnetCoreApp.Models
{
    public class DAL
    {
        public Response Registration(Users users, SqlConnection connection)
        {
            Response response = new Response();
            try
            {
                SqlCommand cmd = new SqlCommand("sp_register", connection);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", users.Username);
                cmd.Parameters.AddWithValue("@Email", users.Email);
                cmd.Parameters.AddWithValue("@Password", users.Password);
                cmd.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                cmd.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                string message = (string)cmd.Parameters["@ErrorMessage"].Value;
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = message;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = message;
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }

        public Response Login(Users users, SqlConnection connection)
        {
            Response response = new Response();
            try
            {
                SqlDataAdapter da = new SqlDataAdapter("sp_Login", connection);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Email", users.Email);
                da.SelectCommand.Parameters.AddWithValue("@Password", users.Password);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "User is valid";
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Wrong Email or Password"; // User is invalid
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = 100;
                response.StatusMessage = ex.Message;
            }
            return response;
        }
    }
}
