using backend.Domain;
using backend.Models;
using System.Data.SqlClient;

namespace backend.Infrastructure
{
    public class AddOrderHandler
    {
        private readonly string _connectionString;

        public AddOrderHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _connectionString = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
        }

        public async Task<int> InsertOrder(AddOrderModel order)
        {
            string query = @"
                INSERT INTO Orders (UserID, AddressID, FeeID, Tax, ShippingCost, ProductCost, DeliveryDate)
                OUTPUT INSERTED.OrderID
                VALUES (@UserID, @AddressID, @FeeID, @Tax, @ShippingCost, @ProductCost, @DeliveryDate);";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(); 
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", order.UserID);
                    command.Parameters.AddWithValue("@AddressID", order.AddressID);
                    command.Parameters.AddWithValue("@FeeID", order.FeeID);
                    command.Parameters.AddWithValue("@Tax", order.Tax);
                    command.Parameters.AddWithValue("@ShippingCost", order.ShippingCost);
                    command.Parameters.AddWithValue("@ProductCost", order.ProductCost);
                    command.Parameters.AddWithValue("@DeliveryDate", order.DeliveryDate);

                    return (int)await command.ExecuteScalarAsync();
                }
            }
        }

        public async Task<bool> InsertPerishableProduct(AddPerishableProductToOrderModel product)
        {
            string query = @"
                INSERT INTO OrderedPerishable (ProductID, OrderID, BatchNumber, ProductName, Quantity, ProductPrice)
                VALUES (@ProductID, @OrderID, @BatchNumber, @ProductName, @Quantity, @ProductPrice);";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(); 
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", product.ProductID);
                        command.Parameters.AddWithValue("@OrderID", product.OrderID);
                        command.Parameters.AddWithValue("@BatchNumber", product.BatchNumber);
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@Quantity", product.Quantity);
                        command.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar producto perecedero: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> InsertNonPerishableProduct(AddNonPerishableProductToOrderModel product)
        {
            string query = @"
                INSERT INTO OrderedNonPerishable (ProductID, OrderID, ProductName, Quantity, ProductPrice)
                VALUES (@ProductID, @OrderID, @ProductName, @Quantity, @ProductPrice);";

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(); 
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductID", product.ProductID);
                        command.Parameters.AddWithValue("@OrderID", product.OrderID);
                        command.Parameters.AddWithValue("@ProductName", product.ProductName);
                        command.Parameters.AddWithValue("@Quantity", product.Quantity);
                        command.Parameters.AddWithValue("@ProductPrice", product.ProductPrice);

                        int rowsAffected = await command.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar producto no perecedero: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> HasSufficientPerishableStock(AddPerishableProductToOrderModel product)
        {
            string query = @"
                SELECT (pp.ProductionLimit - d.ReservedUnits) AS AvailableStock
                FROM Delivery d
                INNER JOIN PerishableProduct pp ON d.ProductID = pp.ProductID
                WHERE d.ProductID = @ProductID AND d.BatchNumber = @BatchNumber;";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(); // Open the connection
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);
                    command.Parameters.AddWithValue("@BatchNumber", product.BatchNumber);

                    int availableStock = (int)(await command.ExecuteScalarAsync() ?? 0);
                    return availableStock >= product.Quantity;
                }
            }
        }

        public async Task<bool> HasSufficientNonPerishableStock(AddNonPerishableProductToOrderModel product)
        {
            string query = "SELECT Stock FROM NonPerishableProduct WHERE ProductID = @ProductID;";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(); 
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", product.ProductID);

                    int stock = (int)(await command.ExecuteScalarAsync() ?? 0);
                    return stock >= product.Quantity;
                }
            }
        }
    }
}