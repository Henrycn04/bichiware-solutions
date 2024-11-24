using backend.Commands;
using System.Data;
using System.Data.SqlClient;

namespace backend.Infrastructure
{
    public class AddProductToCartCommandHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;

        public AddProductToCartCommandHandler()
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
                ? "SELECT 1 FROM PerishableProduct WHERE ProductID = @ProductID AND Deleted != 1"
                : "SELECT 1 FROM NonPerishableProduct WHERE ProductID = @ProductID AND Deleted != 1";

            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                DataTable resultTable = CrearTablaConsulta(cmd);
                return resultTable.Rows.Count > 0;
            }
        }
        public async Task<bool> UserHasShoppingCart(int userId)
        {
            string checkQuery = "SELECT COUNT(1) FROM ShoppingCart WHERE UserID = @UserID";
            using (var cmd = new SqlCommand(checkQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                _connection.Open();

                int count = (int)await cmd.ExecuteScalarAsync();

                if (count > 0)
                {
                    _connection.Close();
                    return true;
                }

                string insertQuery = "INSERT INTO ShoppingCart (UserID) VALUES (@UserID)";
                using (var insertCmd = new SqlCommand(insertQuery, _connection))
                {
                    insertCmd.Parameters.AddWithValue("@UserID", userId);

                    await insertCmd.ExecuteNonQueryAsync();
                }

                _connection.Close();
                return true;
            }
        }
        public async Task<int> GetProductStock(int productId)
        {
            string stockQuery = "SELECT Stock FROM NonPerishableProduct WHERE ProductID = @ProductID AND Deleted != 1";
            using (var stockCmd = new SqlCommand(stockQuery, _connection))
            {
                stockCmd.Parameters.AddWithValue("@ProductID", productId);
                DataTable stockResultTable = CrearTablaConsulta(stockCmd);
                return stockResultTable.Rows.Count > 0
                    ? Convert.ToInt32(stockResultTable.Rows[0]["Stock"])
                    : 0;
            }
        }

        public async Task<int> GetCurrentCartQuantity(int userId, int productId)
        {
            string cartQuery = "SELECT Quantity FROM NonPerishableCart WHERE ProductID = @ProductID AND UserID = @UserID";
            using (var cartCmd = new SqlCommand(cartQuery, _connection))
            {
                cartCmd.Parameters.AddWithValue("@ProductID", productId);
                cartCmd.Parameters.AddWithValue("@UserID", userId);
                DataTable cartResultTable = CrearTablaConsulta(cartCmd);
                return cartResultTable.Rows.Count > 0
                    ? Convert.ToInt32(cartResultTable.Rows[0]["Quantity"])
                    : 0;
            }
        }

        public async Task<bool> HandleAddProductToCart(int userId, int productId, string productName, int quantity, decimal productPrice, bool isPerishable)
        {
            string cartTable = isPerishable ? "PerishableCart" : "NonPerishableCart";
            string addQuery = $@"
                IF EXISTS (SELECT 1 FROM {cartTable} WHERE ProductID = @ProductID AND UserID = @UserID)
                BEGIN
                    UPDATE {cartTable} SET Quantity = Quantity + @Quantity WHERE ProductID = @ProductID AND UserID = @UserID
                END
                ELSE
                BEGIN
                    INSERT INTO {cartTable} (ProductID, UserID, ProductName, Quantity, ProductPrice)
                    VALUES (@ProductID, @UserID, @ProductName, @Quantity, @ProductPrice)
                END";

            using (var cmd = new SqlCommand(addQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@ProductName", productName);
                cmd.Parameters.AddWithValue("@Quantity", quantity);
                cmd.Parameters.AddWithValue("@ProductPrice", productPrice);

                _connection.Open();
                int affectedRows = await cmd.ExecuteNonQueryAsync();
                _connection.Close();

                return affectedRows > 0;
            }
        }
    }
}
