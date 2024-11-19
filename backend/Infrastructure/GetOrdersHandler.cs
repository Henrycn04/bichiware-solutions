using backend.Domain;
using backend.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace backend.Infrastructure
{
    public class GetOrdersHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;

        public GetOrdersHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("CompanyDataContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public async Task<bool> UserExists(int userId)
        {
            string query = "SELECT COUNT(1) FROM Profile WHERE UserID = @UserID";
            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                _connection.Open();
                int count = (int)await cmd.ExecuteScalarAsync();
                _connection.Close();
                return count > 0;
            }
        }

        public async Task<List<UserOrdersModel>> GetAllOrdersWithProducts(int userId)
        {
            var orders = await GetOrdersByUserId(userId);

            foreach (var order in orders)
            {
                var products = await GetProductsByOrderId(order.OrderID);
                order.Products.AddRange(products);
            }

            return orders;
        }

        private async Task<List<UserOrdersModel>> GetOrdersByUserId(int userId)
        {
            var orders = new List<UserOrdersModel>();
            string getOrdersQuery = "EXEC GetActiveOrdersByUserID @UserID";

            _connection.Open();
            try
            {
                using (var ordersCmd = new SqlCommand(getOrdersQuery, _connection))
                {
                    ordersCmd.Parameters.AddWithValue("@UserID", userId);

                    using (var ordersReader = await ordersCmd.ExecuteReaderAsync())
                    {
                        while (await ordersReader.ReadAsync())
                        {
                            var order = new UserOrdersModel
                            {
                                OrderID = (int)ordersReader["OrderID"],
                                UserID = ordersReader["UserID"] as int?,
                                AddressID = ordersReader["AddressID"] as int?,
                                FeeID = ordersReader["FeeID"] as int?,
                                Tax = (decimal)ordersReader["Tax"],
                                ShippingCost = (decimal)ordersReader["ShippingCost"],
                                ProductCost = (decimal)ordersReader["ProductCost"],
                                OrderStatus = ordersReader["OrderStatus"] as int?,
                                DeliveryDate = (DateTime)ordersReader["DeliveryDate"]
                            };

                            orders.Add(order);
                        }
                    }
                }
            }
            finally
            {
                _connection.Close();
            }

            return orders;
        }

        private async Task<List<UserOrderProductModel>> GetProductsByOrderId(int orderId)
        {
            var products = new List<UserOrderProductModel>();
            string getProductsQuery = "EXEC GetProductsByOrderID @OrderID";

            _connection.Open();
            try
            {
                using (var productsCmd = new SqlCommand(getProductsQuery, _connection))
                {
                    productsCmd.Parameters.AddWithValue("@OrderID", orderId);

                    using (var productsReader = await productsCmd.ExecuteReaderAsync())
                    {
                        while (await productsReader.ReadAsync())
                        {
                            var product = new UserOrderProductModel
                            {
                                ProductID = (int)productsReader["ProductID"],
                                ProductName = productsReader["ProductName"].ToString(),
                                Quantity = (int)productsReader["Quantity"],
                                ProductPrice = (decimal)productsReader["ProductPrice"],
                                BatchNumber = productsReader["BatchNumber"] as int?,
                                ProductType = productsReader["ProductType"].ToString()
                            };

                            products.Add(product);
                        }
                    }
                }
            }
            finally
            {
                _connection.Close();
            }

            return products;
        }
    }
}
