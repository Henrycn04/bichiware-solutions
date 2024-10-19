using System.Data.SqlClient;
using System.Xml.Linq;
using backend.Models;

namespace backend.Handlers
{
    public class SetTypeHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public SetTypeHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("SetType");
            _connection = new SqlConnection(_routeConnection);
        }

        public void setNewType(SetTypeModel data)
        {
            var query =
               @"UPDATE [dbo].[UserData]
				SET [UserType] = @NewType
				WHERE [UserID] = @UID";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@NewType", data.newType);
            commandForQuery.Parameters.AddWithValue("@UID", data.userID);

            _connection.Open();
            commandForQuery.ExecuteNonQuery();
            _connection.Close();
        }

    }
}
