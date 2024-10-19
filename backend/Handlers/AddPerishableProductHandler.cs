using backend.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace backend.Handlers
{
    public class AddPerishableProductHandler
    {
        private readonly SqlConnection _connection;

        public AddPerishableProductHandler(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }
        public async Task<int> addPerishableProduct(AddPerishableProductModel productData)
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
    }
}