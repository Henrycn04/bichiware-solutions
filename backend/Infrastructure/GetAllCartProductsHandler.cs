using backend.Domain;
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
            string query = "SELECT COUNT(1) FROM Profile WHERE UserID = @UserID AND Deleted != 1";
            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                _connection.Open();
                int count = (int)await cmd.ExecuteScalarAsync();
                _connection.Close();
                return count > 0;
            }
        }

        public async Task<List<AllCartProductsModel>> GetAllCartProducts(int userId)
        {
            var allProducts = new List<AllCartProductsModel>();

            string perishableQuery = @"
        SELECT pp.ProductID, pp.ProductName, pp.CompanyID, pp.CompanyName, pp.ImageURL, pp.Category,
               pp.Price, pp.ProductDescription, pc.Quantity, pp.Weight
        FROM PerishableProduct pp
        INNER JOIN PerishableCart pc ON pp.ProductID = pc.ProductID
        WHERE pc.UserID = @UserID";

            _connection.Open();
            using (var cmd = new SqlCommand(perishableQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        allProducts.Add(new AllCartProductsModel
                        {
                            ProductID = (int)reader["ProductID"],
                            UserID = userId,
                            ProductName = reader["ProductName"].ToString(),
                            Quantity = 0,
                            ProductPrice = (decimal)reader["Price"],
                            IsPerishable = true,
                            CurrentCartQuantity = (int)reader["Quantity"],
                            ProductDescription = reader["ProductDescription"].ToString(),
                            Category = reader["Category"].ToString(),
                            CompanyName = reader["CompanyName"].ToString(),
                            ImageURL = reader["ImageURL"].ToString(),
                            Weight = (decimal)reader["Weight"]
                        });
                    }
                }
            }

            string nonPerishableQuery = @"
        SELECT np.ProductID, np.ProductName, np.CompanyID, np.CompanyName, np.ImageURL, np.Category,
               np.Price, np.ProductDescription, np.Stock, nc.Quantity,  np.Weight
        FROM NonPerishableProduct np
        INNER JOIN NonPerishableCart nc ON np.ProductID = nc.ProductID
        WHERE nc.UserID = @UserID";

            using (var cmd = new SqlCommand(nonPerishableQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        allProducts.Add(new AllCartProductsModel
                        {
                            ProductID = (int)reader["ProductID"],
                            UserID = userId,
                            ProductName = reader["ProductName"].ToString(),
                            Quantity = 0,
                            ProductPrice = (decimal)reader["Price"],
                            IsPerishable = false,
                            CurrentCartQuantity = (int)reader["Quantity"],
                            CurrentStock = (int)reader["Stock"],
                            ProductDescription = reader["ProductDescription"].ToString(),
                            Category = reader["Category"].ToString(),
                            CompanyName = reader["CompanyName"].ToString(),
                            ImageURL = reader["ImageURL"].ToString(),
                             Weight = (decimal)reader["Weight"]
                        });
                    }
                }
            }

            _connection.Close();
            return allProducts;
        }


    }
}
