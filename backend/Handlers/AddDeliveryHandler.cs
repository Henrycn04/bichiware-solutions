using backend.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace backend.Handlers
{
    public class AddDeliveryHandler
    {
        private readonly SqlConnection _connection;

        public AddDeliveryHandler(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public async Task<bool> addDelivery(AddDeliveryModel deliveryData)
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
        public async Task<bool> SearchDelivery(AddDeliveryModel deliveryData)
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