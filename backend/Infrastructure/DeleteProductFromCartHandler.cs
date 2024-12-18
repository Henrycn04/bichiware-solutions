﻿using backend.Commands;
using System.Data;
using System.Data.SqlClient;

namespace backend.Infrastructure
{
    public class DeleteProductFromCartHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;

        public DeleteProductFromCartHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("CompanyDataContext");
            _connection = new SqlConnection(_routeConnection);
        }

        private DataTable CrearTablaConsulta(SqlCommand comandoParaConsulta)
        {
            using (SqlDataAdapter adaptadorParaTabla = new SqlDataAdapter(comandoParaConsulta))
            {
                DataTable consultaFormatoTabla = new DataTable();
                _connection.Open();
                adaptadorParaTabla.Fill(consultaFormatoTabla);
                _connection.Close();
                return consultaFormatoTabla;
            }
        }

        public async Task<bool> ProductExists(int productId, bool isPerishable)
        {
            string query = isPerishable
                ? "SELECT 1 FROM PerishableProduct WHERE ProductID = @ProductID AND Deleted = 0"
                : "SELECT 1 FROM NonPerishableProduct WHERE ProductID = @ProductID AND Deleted = 0";

            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                DataTable resultTable = CrearTablaConsulta(cmd);
                return resultTable.Rows.Count > 0;
            }
        }
        public async Task<bool> ProductIsInCart(int userId, int productId, bool isPerishable)
        {
            string query = isPerishable
                ? "SELECT 1 FROM PerishableCart WHERE ProductID = @ProductID AND UserID = @UserID"
                : "SELECT 1 FROM NonPerishableCart WHERE ProductID = @ProductID AND UserID = @UserID";

            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@UserID", userId);

                DataTable resultTable = CrearTablaConsulta(cmd);
                return resultTable.Rows.Count > 0;
            }
        }

        public async Task<bool> HaveUserProductsInCart(int userId)
        {
            string query =
                "SELECT 1 FROM PerishableCart WHERE UserID = 1 UNION SELECT 1 FROM NonPerishableCart WHERE UserID = 1";

            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);

                DataTable resultTable = CrearTablaConsulta(cmd);
                return resultTable.Rows.Count > 0;
            }
        }


        public async Task<bool> HandleDeleteProductFromCart(int userId, int productId, bool isPerishable)
        {
            string cartTable = isPerishable ? "PerishableCart" : "NonPerishableCart";
            string deleteQuery = $@"
        DELETE FROM {cartTable}
        WHERE ProductID = @ProductID AND UserID = @UserID";

            using (var cmd = new SqlCommand(deleteQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@UserID", userId);

                _connection.Open();
                int affectedRows = await cmd.ExecuteNonQueryAsync();
                _connection.Close();

                return affectedRows > 0;
            }
        }
        public async Task<bool> HandleDeleteAllFromCart(int userId)
        {
            string deletePerishableQuery = $@"
            DELETE FROM PerishableCart
            WHERE UserID = @UserID";

            string deleteNonPerishableQuery = $@"
            DELETE FROM NonPerishableCart
            WHERE UserID = @UserID";
            int affectedRows = 0;

            using (var cmd = new SqlCommand(deletePerishableQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);

                _connection.Open();
                 affectedRows += await cmd.ExecuteNonQueryAsync();
                _connection.Close();

             
            }

            using (var cmd = new SqlCommand(deleteNonPerishableQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);

                _connection.Open();
                 affectedRows += await cmd.ExecuteNonQueryAsync();
                _connection.Close();

               
            } 
            return affectedRows > 0;

        }
    }
}
