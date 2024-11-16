using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend.Infrastructure
{
    public class DeleteAddressHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public DeleteAddressHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public bool checkAddress(int addressID)
        {
            int orderExists;
            var query =
               @"SELECT CASE 
                WHEN EXISTS (
                    SELECT 1
                    FROM [dbo].[Orders]
                    WHERE AddressID = @ID
                ) THEN 1
                ELSE 0
                END AS RowExists;";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@ID", addressID);

            _connection.Open();
            orderExists = (int)commandForQuery.ExecuteScalar();
            _connection.Close();
            return 0 == orderExists;
        }

        public void DeleteAddress(int addressID)
        {
            int rowsAffected;
            var query =
               @"DELETE FROM [dbo].[Address]
                    WHERE AddressID = @ID";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@ID", addressID);

            _connection.Open();
            rowsAffected = commandForQuery.ExecuteNonQuery();
            _connection.Close();
            if (rowsAffected == 0) throw new Exception("Address deletion unsuccesful: No rows affected with address " + addressID);
        }

        public void LogicalDeleteAddress(int addressID)
        {
            int rowsAffected;
            var query =
               @"UPDATE [dbo].[Address]
                    SET Deleted = 1
                    WHERE AddressID = @ID";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@ID", addressID);

            _connection.Open();
            rowsAffected = commandForQuery.ExecuteNonQuery();
            _connection.Close();
            if (rowsAffected == 0) throw new Exception("Logical address deletion unsuccesful: No rows affected with address " + addressID);
        }
    }
}
