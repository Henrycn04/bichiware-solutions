using backend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

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
        public async Task<List<object>> GetAllCartProducts(int userID)
        {
            var allProducts = new List<object>();

            string perishableQuery = "SELECT * FROM PerishableCart WHERE UserID = @UserID";
            _connection.Open();
            using (var cmd = new SqlCommand(perishableQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        allProducts.Add(new
                        {
                            ProductID = reader["ProductID"],
                            ProductName = reader["ProductName"],
                            Quantity = reader["Quantity"],
                            ProductPrice = reader["ProductPrice"],
                            IsPerishable = true
                        });
                    }
                }
            }

            string nonPerishableQuery = "SELECT * FROM NonPerishableCart WHERE UserID = @UserID";
            using (var cmd = new SqlCommand(nonPerishableQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        allProducts.Add(new
                        {
                            ProductID = reader["ProductID"],
                            ProductName = reader["ProductName"],
                            Quantity = reader["Quantity"],
                            ProductPrice = reader["ProductPrice"],
                            IsPerishable = false
                        });
                    }
                }
            }
            _connection.Close();
            return allProducts;
        }

    }

}
