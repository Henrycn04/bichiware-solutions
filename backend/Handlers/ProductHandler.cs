using backend.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace backend.Handlers
{
    public class ProductHandler
    {
        private readonly SqlConnection _connection;

        public ProductHandler(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }
        public async Task<int> addPerishableProduct(PerishableModel productData)
        {
            // insert a perishable product
            var consult = @"
                INSERT INTO [dbo].[PerishableProduct] (CompanyID, CompanyName, ProductName, ImageURL, Category, Price, ProductDescription, DeliveryDays, ProductionLimit)
                VALUES (@CompanyID, @CompanyName, @Name, @Image, @Category, @Price, @Description, @DeliveryDays, @Limit);
                SELECT SCOPE_IDENTITY();"; 

            try
            {
                using var consultCommand = new SqlCommand(consult, _connection);

                // add parameteres
                consultCommand.Parameters.AddWithValue("@CompanyID", productData.CompanyID);
                consultCommand.Parameters.AddWithValue("@CompanyName", productData.CompanyName);
                consultCommand.Parameters.AddWithValue("@Name", productData.Name);
                consultCommand.Parameters.AddWithValue("@Image", productData.Image);
                consultCommand.Parameters.AddWithValue("@Category", productData.Category);
                consultCommand.Parameters.AddWithValue("@Price", productData.Price);
                consultCommand.Parameters.AddWithValue("@Description", productData.Description);
                consultCommand.Parameters.AddWithValue("@DeliveryDays", productData.DeliveryDays);
                consultCommand.Parameters.AddWithValue("@Limit", productData.Limit);

                await _connection.OpenAsync();
                var productId = await consultCommand.ExecuteScalarAsync();
                await _connection.CloseAsync();

                // Return ID of the product
                return productId != null ? Convert.ToInt32(productId) : -1; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return -1; 
            }
        }
        public async Task<bool> addNonPerishableProduct(NonPerishableModel productData)
        {
            // insert a non perishable product
            var consult = @"
              INSERT INTO [dbo].[NonPerishableProduct] 
            (CompanyID, CompanyName, ProductName, ImageURL, Category, Price, ProductDescription, Stock)
            VALUES (@CompanyID, @CompanyName, @ProductName, @ImageURL, @Category, @Price, @ProductDescription, @Stock);";
            try
            {
                using var consultCommand = new SqlCommand(consult, _connection);
                // add parameteres
                consultCommand.Parameters.AddWithValue("@CompanyID", productData.CompanyID);
                consultCommand.Parameters.AddWithValue("@CompanyName", productData.CompanyName);
                consultCommand.Parameters.AddWithValue("@ProductName", productData.Name);
                consultCommand.Parameters.AddWithValue("@ImageURL", productData.Image);
                consultCommand.Parameters.AddWithValue("@Category", productData.Category);
                consultCommand.Parameters.AddWithValue("@Price", productData.Price);
                consultCommand.Parameters.AddWithValue("@ProductDescription", productData.Description);
                consultCommand.Parameters.AddWithValue("@Stock", productData.Stock);

                await _connection.OpenAsync();
                var success = await consultCommand.ExecuteNonQueryAsync();
                await _connection.CloseAsync();
                return success > 0 ;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false; 
            }
        }

        public async Task<bool> addDelivery(DeliveryModel deliveryData)
        {
            // insert delivery product
            var consult = @"
             INSERT INTO [dbo].[Delivery] 
            (ProductID, BatchNumber, ExpirationDate, ReservedUnits)
            VALUES (@ProductID, @BatchNumber, @ExpirationDate, @ReservedUnits);";
            try
            {
                using var consultCommand = new SqlCommand(consult, _connection);
                // add parameteres
                consultCommand.Parameters.AddWithValue("@ProductID", deliveryData.ProductID);
                consultCommand.Parameters.AddWithValue("@BatchNumber", deliveryData.BatchNumber);
                consultCommand.Parameters.AddWithValue("@ExpirationDate", deliveryData.ExpirationDate);
                consultCommand.Parameters.AddWithValue("@ReservedUnits", deliveryData.ReservedUnits);
               
                await _connection.OpenAsync();
                var success = await consultCommand.ExecuteNonQueryAsync();
                await _connection.CloseAsync();
                return success > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false; 
            }
        }
        public async Task<bool> SearchDelivery(DeliveryModel deliveryData)
        {   // Search if there are a delivery with the same key
            var consult = @"SELECT COUNT(*) FROM [dbo].[Delivery] 
                     WHERE ProductID = @ProductID AND BatchNumber = @BatchNumber";

            try
            {
                using var consultCommand = new SqlCommand(consult, _connection);
                consultCommand.Parameters.AddWithValue("@ProductID", deliveryData.ProductID);
                consultCommand.Parameters.AddWithValue("@BatchNumber", deliveryData.BatchNumber);

                await _connection.OpenAsync();

                var count = await consultCommand.ExecuteScalarAsync();

                await _connection.CloseAsync();

                if (count != null && (int)count > 0) //  get if there is at least one delivery with the same data
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}