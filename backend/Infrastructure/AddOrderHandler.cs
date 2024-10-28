using backend.Application;
using backend.Domain;
using backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace backend.Infrastructure
{
    public class AddOrderHandler
    {
        private readonly string _connectionString;
        private ShippingCostCalculator shippingCalculator;
        private MailHandler mailHandler;
        private DatabaseQuery databaseQuery;
        private IMailBodyBuilder adminBody;
        private IMailBodyBuilder customerBody;

        public AddOrderHandler(IMailService service)
        {
            var builder                 = WebApplication.CreateBuilder();
            this._connectionString      = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");

            this.mailHandler            = new MailHandler(service);
            this.adminBody              = new AdminOrderBodyBuilder();
            this.customerBody           = new CustomerOrderBodyBuilder();
            this.databaseQuery          = new DatabaseQuery();
            this.shippingCalculator     = new ShippingCostCalculator();
        }

        public async Task<int> InsertOrder(AddOrderModel order)
        {
            string query = @"
                INSERT INTO Orders (UserID, AddressID, FeeID, Tax, ShippingCost, ProductCost, DeliveryDate)
                OUTPUT INSERTED.OrderID
                VALUES (@UserID, @AddressID, @FeeID, @Tax, @ShippingCost, @ProductCost, @DeliveryDate);";

            PhysicalAddress destination         = shippingCalculator.GetOrderDestination(order.AddressID);
            //double weight                       = shippingCalculator.SumOrderProductsWeight(order.);
            // double orderCost                    = shippingCalculator.CalculateShippingCost(destination, weight);

            int orderId = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync(); 
                using (SqlCommand command = new SqlCommand(query, connection))
                { 
                    command.Parameters.AddWithValue("@UserID",          order.UserID);
                    command.Parameters.AddWithValue("@AddressID",       order.AddressID);
                    command.Parameters.AddWithValue("@FeeID",           order.FeeID);
                    command.Parameters.AddWithValue("@Tax",             order.Tax);
                    command.Parameters.AddWithValue("@ShippingCost",    0);
                    command.Parameters.AddWithValue("@ProductCost",     order.ProductCost);
                    command.Parameters.AddWithValue("@DeliveryDate",    order.DeliveryDate);

                    orderId = (int) await command.ExecuteScalarAsync();
                }
            }


            return orderId;
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