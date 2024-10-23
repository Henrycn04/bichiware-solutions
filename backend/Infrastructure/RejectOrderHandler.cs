using backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public class RejectOrderHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public RejectOrderHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public int CheckIfOrderExists(int orderID)
        {
            Console.WriteLine(orderID);

            string query = "SELECT COUNT(*) FROM Orders WHERE OrderID = @OrderID";
            SqlCommand commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@OrderID", orderID);
            _connection.Open();
            int order = (int)commandForQuery.ExecuteScalar();
            _connection.Close();
            return order;
        }

        public int RejectOrder(int orderID)
        {
            Console.WriteLine(orderID);

            string query = "UPDATE [dbo].[Orders] SET OrderStatus = 3 WHERE OrderID = @OrderID";
            SqlCommand commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@OrderID", orderID);
            _connection.Open();
            int rows = commandForQuery.ExecuteNonQuery();
            _connection.Close();
            return rows;
        }
    }
}
