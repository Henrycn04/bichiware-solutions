using System.Data.SqlClient;
using backend.Domain;

namespace backend.Infrastructure
{
    public class ConfirmedOrderHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public ConfirmedOrderHandler() 
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public OrderConfirmationModel GetOrder(int orderId)
        {
            OrderConfirmationModel data = new OrderConfirmationModel();
            var getter =
                @"Select [UserID], [Tax], [ShippingCost], [ProductCost]
                FROM [dbo].[Orders]			
                WHERE [OrderID] = @OID";
            var commandGetter = new SqlCommand(getter, _connection);
            commandGetter.Parameters.AddWithValue("@OID", orderId);
            _connection.Open();
            var reader = commandGetter.ExecuteReader();

            if (reader.Read())
            {
                data.UserID = Int32.Parse(reader["UserID"].ToString());
                data.tax = float.Parse(reader["Tax"].ToString());
                data.delivery = float.Parse(reader["ShippingCost"].ToString());
                data.productCost = float.Parse(reader["ProductCost"].ToString());
                _connection.Close();
                return data;
            }
            else
            {
                _connection.Close();
                throw new Exception("No User with that ID found");
            }


        }
    }
}
