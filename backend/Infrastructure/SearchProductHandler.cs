using backend.Domain;
using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;


namespace backend.Infrastructure
{
    public interface IProductSearchHandler
    {
        GeneralProductModel GetSpecificProduct(int productId);
        List<GeneralProductModel> GetProductsByIds(List<int> productIds);
    }
    public class SearchProductHandler : IProductSearchHandler
    {
        private readonly SqlConnection _connection;
        private string _ConectionString;
        private DatabaseQuery databaseQuery;

        public SearchProductHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _ConectionString = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_ConectionString);
            databaseQuery = new DatabaseQuery();
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
                            Limit = reader.IsDBNull(reader.GetOrdinal("ProductionLimit")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("ProductionLimit")), 
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
            Console.WriteLine($"Product {productId} searched.");
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
        public List<GeneralProductModel> GetRandomProductsForShowcase()
        {
            List<GeneralProductModel> products = new List<GeneralProductModel>();
            SqlCommand cmd = new SqlCommand("GetRandomProductsForShowcase", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable result = databaseQuery.ReadFromDatabase(cmd);

            foreach (DataRow row in result.Rows)
            {
                int productId = Convert.ToInt32(row["ProductID"]);
                int stock = 0, limit = 0;
                string deliveryDate = "";
                if (productId % 2 == 0)
                {
                    deliveryDate = Convert.ToString(row["DeliveryDays"]);
                    limit = Convert.ToInt32(row["ProductionLimit"]);
                }
                else
                {
                    stock = Convert.ToInt32(row["Stock"]);
                }
                string description = (row["ProductDescription"] == DBNull.Value) ? "" : Convert.ToString(row["ProductDescription"]);

                products.Add(new GeneralProductModel()
                {
                    ProductID           = productId,
                    CompanyName         = Convert.ToString(row["CompanyName"]),
                    Name                = Convert.ToString(row["ProductName"]),
                    Image               = Convert.ToString(row["ImageURL"]),
                    Category            = Convert.ToString(row["Category"]),
                    Price               = Convert.ToDecimal(row["Price"]),
                    Description         = description,
                    Stock               = stock,
                    DeliveryDays        = deliveryDate,
                    CompanyID           = Convert.ToInt32(row["CompanyID"]),
                    Limit               = limit,
                    Weight              = Convert.ToDecimal(row["Weight"])
                });
            }
            return products;
        }
    }
}
