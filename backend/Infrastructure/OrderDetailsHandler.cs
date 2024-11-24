using System.Data;
using System.Data.SqlClient;
using backend.Domain;
using backend.Models;

namespace backend.Handlers
{
    public class OrderDetailsHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public OrderDetailsHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public int CheckIfOrderHasProducts(int orderID)
        {
            string query = @"
                SELECT
                (SELECT COUNT(*) FROM OrderedPerishable op WHERE op.OrderID = @OrderID1) +
                (SELECT COUNT(*) FROM OrderedNonPerishable onp WHERE onp.OrderID = @OrderID2) ";
            SqlCommand commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@OrderID1", orderID);
            commandForQuery.Parameters.AddWithValue("@OrderID2", orderID);
            _connection.Open();
            int numberOfProducts = (int)commandForQuery.ExecuteScalar();
            _connection.Close();
            return numberOfProducts;
        }

        public OrderDetailsModel GetOrderDetails(int orderID)
        {
            OrderDetailsModel orderDetails = new OrderDetailsModel();
            List<OrderProductModel> products = new List<OrderProductModel>();
            orderDetails.OrderProducts = products;
            string query = @"
                SELECT p.ProfileName, p.Email, o.Tax, o.ShippingCost, o.ProductCost
                FROM Orders o
                INNER JOIN Profile p ON p.UserID = o.UserID
                WHERE o.OrderID = @OrderID
            ";
            SqlCommand commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@OrderID", orderID);
            _connection.Open();
            using (SqlDataReader reader = commandForQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    orderDetails.CustomerName = reader["ProfileName"].ToString();
                    orderDetails.CustomerEmail = reader["Email"].ToString();
                    orderDetails.Taxes = (double)reader.GetDecimal("Tax");
                    orderDetails.ShippingCost = (double)reader.GetDecimal("ShippingCost");
                    orderDetails.ProductCost = (double)reader.GetDecimal("ProductCost");
                }
            }
            _connection.Close();
            orderDetails = GetPerishableProductsData(orderDetails, orderID);
            orderDetails = GetNonPerishableProductsData(orderDetails, orderID);
            return orderDetails;
        }

        private OrderDetailsModel GetPerishableProductsData(OrderDetailsModel model, int orderID)
        {
            SqlCommand perishableCommand = new SqlCommand("FindOrderedPerishables", _connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            perishableCommand.Parameters.AddWithValue("@OID", orderID);

            _connection.Open();
            using (SqlDataReader reader = perishableCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    OrderProductModel product = new OrderProductModel
                    {
                        Name = reader["ProductName"].ToString(),
                        Category = reader["Category"].ToString(),
                        Company = reader["CompanyName"].ToString(),
                        PriceInColones = (double)reader.GetDecimal("ProductPrice"),
                        Amount = reader.GetInt32("Quantity"),
                        ImageURL = reader["ImageURL"].ToString()
                    };
                    model.OrderProducts.Add(product);
                }
            }
            _connection.Close();
            return model;
        }

        private OrderDetailsModel GetNonPerishableProductsData(OrderDetailsModel model, int orderID)
        {
            SqlCommand perishableCommand = new SqlCommand("FindOrderedNonPerishables", _connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            perishableCommand.Parameters.AddWithValue("@OID", orderID);

            _connection.Open();
            using (SqlDataReader reader = perishableCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    OrderProductModel product = new OrderProductModel
                    {
                        Name = reader["ProductName"].ToString(),
                        Category = reader["Category"].ToString(),
                        Company = reader["CompanyName"].ToString(),
                        PriceInColones = (double)reader.GetDecimal("ProductPrice"),
                        Amount = reader.GetInt32("Quantity"),
                        ImageURL = reader["ImageURL"].ToString()
                    };
                    model.OrderProducts.Add(product);
                }
            }
            _connection.Close();
            return model;
        }
    }
}
