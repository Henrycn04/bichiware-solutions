using backend.Domain;
using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace backend.Infrastructure
{
    public class SearchProductHandler
    {
        private readonly SqlConnection _connection;
        private string _ConectionString;

        public SearchProductHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _ConectionString = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_ConectionString);
        }

        public GeneralProductModel GetSpecificProduct(int productId)
        {
            GeneralProductModel product = null;

            string query = @"EXEC GetCombinedProducts @ID = @id";


            SqlCommand queryCommand = new SqlCommand(query, _connection);
            queryCommand.Parameters.AddWithValue("@id", productId);

            try
            {
                _connection.Open();
                using (SqlDataReader reader = queryCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        product = new GeneralProductModel()
                        {
                            ProductID = reader.GetInt32(reader.GetOrdinal("ProductID")),
                            CompanyName = reader.GetString(reader.GetOrdinal("CompanyName")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Image = reader.GetString(reader.GetOrdinal("ImageURL")),
                            Category = reader.GetString(reader.GetOrdinal("Category")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                            Stock = reader.IsDBNull(reader.GetOrdinal("Stock")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Stock")),
                            DeliveryDays = reader.IsDBNull(reader.GetOrdinal("DeliveryDays")) ? (string?)null : reader.GetString(reader.GetOrdinal("DeliveryDays")),
                            CompanyID = reader.GetInt32(reader.GetOrdinal("CompanyID")),
                            Limit = reader.IsDBNull(reader.GetOrdinal("ProductionLimit")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ProductionLimit")), // Manejo de null
                            Weight = reader.GetDecimal(reader.GetOrdinal("Weight"))
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

            return product;
        }
        public List<GeneralProductModel> GetProductsByIds(List<int> productIds)
        {
            var products = new List<GeneralProductModel>();

            foreach (var productId in productIds)
            {
                var product = GetSpecificProduct(productId);
                if (product == null)
                {
                    return new List<GeneralProductModel>(); 
                }
                products.Add(product);
            }

            return products;
        }
    }
}
