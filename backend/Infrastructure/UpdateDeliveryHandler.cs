using System;
using System.Data;
using System.Data.SqlClient;
using backend.Models;

namespace backend.Infrastructure
{
    public class UpdateDeliveryHandler
    {
        private readonly SqlConnection _connection;
        private string _ConectionString;

        public UpdateDeliveryHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _ConectionString = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_ConectionString);
        }

        public void UpdateDelivery(int id, int batchNumber, int oldBatchNumber, DateTime expirationDate)
        {
            using (var command = new SqlCommand("UpdateDeliveryData", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ID", id);
                command.Parameters.AddWithValue("@BatchNumber", batchNumber);
                command.Parameters.AddWithValue("@OldBatchNumber", oldBatchNumber);
                command.Parameters.AddWithValue("@ExpirationDate", expirationDate);

                try
                {
                    _connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("No delivery was updated. Please check the provided IDs and batch numbers.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while updating the delivery.", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
    }
}