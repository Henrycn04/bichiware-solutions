using System.Data.SqlClient;

namespace backend.Infrastructure
{
    public class SetToConfirmedOrderHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public SetToConfirmedOrderHandler() 
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public int SetToConfirmed(int orderID)
        {
            string query = "UPDATE [dbo].[Orders] SET OrderStatus = 2 WHERE OrderID = @OrderID";
            SqlCommand commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@OrderID", orderID);
            _connection.Open();
            int rows = commandForQuery.ExecuteNonQuery();
            _connection.Close();
            return rows;
        }
    }
}
