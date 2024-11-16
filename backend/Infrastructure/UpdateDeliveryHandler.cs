using System;
using System.Data;
using System.Data.SqlClient;
using backend.Domain;
using backend.Models;

namespace backend.Infrastructure
{
    public interface IUpdateDeliveryHandler
    {
        void UpdateDelivery(int id, int batchNumber, int oldBatchNumber, DateTime expirationDate);
        void LogicDeliveryDelete(int[] deliveryId);
        void DeliveryDelete(int[] deliveryId);
    }

    public class UpdateDeliveryHandler: IUpdateDeliveryHandler
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
        public void LogicDeliveryDelete(int[] deliveryID)
        {
            using (var command = new SqlCommand("logicDeliveryDelete", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", deliveryID[0]);
                command.Parameters.AddWithValue("@BatchNumber", deliveryID[1]);

                try
                {
                    _connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("No delivery was logically deleted. Please check the provided Delivery ID.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while performing a logical delete on the delivery.", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
        public void DeliveryDelete(int[] deliveryID)
        {
            using (var command = new SqlCommand("DeliveryDelete", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ProductID", deliveryID[0]);
                command.Parameters.AddWithValue("@BatchNumber", deliveryID[1]);

                try
                {
                    _connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("No delivery was deleted. Please check the provided Delivery ID.");
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("An error occurred while performing a delete on the delivery.", ex);
                }
                finally
                {
                    _connection.Close();
                }
            }
        }
    }
}