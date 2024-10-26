using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace backend.Infrastructure
{
    public class SearchDeliveryHandler
    {
        private readonly SqlConnection _connection;
        private string _ConectionString;

        public SearchDeliveryHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _ConectionString = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_ConectionString);
        }

        public AddDeliveryModel GetSpecificDelivery(int productId, int batchNumber)
        {
            AddDeliveryModel delivery = null;

            string query = "SELECT * FROM Delivery WHERE ProductID = @id AND BatchNumber = @batch";

            SqlCommand queryCommand = new SqlCommand(query, _connection);
            queryCommand.Parameters.AddWithValue("@id", productId);
            queryCommand.Parameters.AddWithValue("@batch", batchNumber);

            try
            {
                _connection.Open();
                using (SqlDataReader reader = queryCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        delivery = new AddDeliveryModel
                        {
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            BatchNumber = reader.GetInt32(reader.GetOrdinal("BatchNumber")),
                            ExpirationDate = reader.GetDateTime(reader.GetOrdinal("ExpirationDate")),
                            ReservedUnits = reader.GetInt32(reader.GetOrdinal("ReservedUnits"))
                        };
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error al ejecutar la consulta: {ex.Message}");
            }
            finally
            {
                _connection.Close();
            }

            return delivery;
        }
    }
}