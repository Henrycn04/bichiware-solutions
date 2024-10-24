using backend.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace backend.Infrastructure
{
    public class GetAllCartProductsHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;

        public GetAllCartProductsHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("CompanyDataContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public async Task<bool> UserExists(int userId)
        {
            string query = "SELECT COUNT(1) FROM Profile WHERE UserID = @UserID";
            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                _connection.Open();
                int count = (int)await cmd.ExecuteScalarAsync();
                _connection.Close();
                return count > 0;
            }
        }

        public async Task<List<CartProductModel>> GetAllCartProducts(int userId)
        {
            var allProducts = new List<CartProductModel>();

            string perishableQuery = "SELECT * FROM PerishableCart WHERE UserID = @UserID";
            _connection.Open();
            using (var cmd = new SqlCommand(perishableQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        allProducts.Add(new CartProductModel
                        {
                            ProductID = (int)reader["ProductID"],
                            UserID = (int)reader["UserID"], 
                            ProductName = reader["ProductName"].ToString(),
                            Quantity = 0,
                            ProductPrice = (decimal)reader["ProductPrice"],
                            IsPerishable = true,
                            CurrentCartQuantity = (int)reader["Quantity"]
                        });
                    }
                }
            }

            string nonPerishableQuery = "SELECT * FROM NonPerishableCart WHERE UserID = @UserID";
            using (var cmd = new SqlCommand(nonPerishableQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        allProducts.Add(new CartProductModel
                        {
                            ProductID = (int)reader["ProductID"],
                            UserID = (int)reader["UserID"],
                            ProductName = reader["ProductName"].ToString(),
                            Quantity = 0,
                            ProductPrice = (decimal)reader["ProductPrice"],
                            IsPerishable = false,
                            CurrentCartQuantity = (int)reader["Quantity"] 
                        });
                    }
                }
            }
            _connection.Close();
            return allProducts;
        }

    }
}
