using backend.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace backend.Handlers
{
    public class AddNonPerishableProductHandler
    {
        private readonly SqlConnection _connection;

        public AddNonPerishableProductHandler(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }
       
        public async Task<bool> addNonPerishableProduct(AddNonPerishableProductModel productData)
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
    }
}