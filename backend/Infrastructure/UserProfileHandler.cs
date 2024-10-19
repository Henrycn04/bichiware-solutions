using backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public class UserProfileHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;

        public UserProfileHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("CompanyDataContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public UserProfileModel getUserData(int UserID)
        {
            UserProfileModel userProfile = null;
            string query = "SELECT ProfileName, Email, CreationDateTime FROM [dbo].[Profile] WHERE UserID = @UserID";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@UserID", UserID);
            _connection.Open();
            using (SqlDataReader reader = commandForQuery.ExecuteReader())
            {
                if (reader.Read())
                {
                    userProfile = new UserProfileModel
                    {
                        UserName = reader["ProfileName"].ToString(),
                        Email = reader["Email"].ToString(),
                        CreationDate = Convert.ToDateTime(reader["CreationDateTime"]).ToString("yyyy-MM-dd")
                    };
                }
            }
            _connection.Close();
            return userProfile;
        }
    }
}
