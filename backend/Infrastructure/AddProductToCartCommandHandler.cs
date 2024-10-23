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

        public async Task<bool> Handle(AddProductToCartCommand command)
        {
            if (!command.IsValid())
                return false;
            // Check first if the UserID exist in the ShoppingCart table
            string checkShoppingCartQuery = "IF NOT EXISTS (SELECT 1 FROM ShoppingCart WHERE UserID = @UserID) " +
                                            "BEGIN " +
                                            "INSERT INTO ShoppingCart (UserID) " +
                                            "VALUES (@UserID) " +
                                            "END";

            using (var checkCmd = new SqlCommand(checkShoppingCartQuery, _connection))
            {
                checkCmd.Parameters.AddWithValue("@UserID", command.UserID);

                _connection.Open();
                await checkCmd.ExecuteNonQueryAsync();
                _connection.Close();
            }
            string cartTable = command.IsPerishable ? "PerishableCart" : "NonPerishableCart";
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
                cmd.Parameters.AddWithValue("@ProductID", command.ProductID);
                cmd.Parameters.AddWithValue("@UserID", command.UserID);
                cmd.Parameters.AddWithValue("@ProductName", command.ProductName);
                cmd.Parameters.AddWithValue("@Quantity", command.Quantity);
                cmd.Parameters.AddWithValue("@ProductPrice", command.ProductPrice);

                _connection.Open();
                int affectedRows = await cmd.ExecuteNonQueryAsync();
                _connection.Close();

                return affectedRows > 0;
            }
        }
        public async Task<bool> SetStockAndCartQuantity(AddProductToCartCommand command)
        {
            if (!command.IsPerishable)
            {
                string stockQuery = $"SELECT Stock FROM NonPerishableProduct WHERE ProductID = @ProductID";
                using (var stockCmd = new SqlCommand(stockQuery, _connection))
                {
                    stockCmd.Parameters.AddWithValue("@ProductID", command.ProductID);
                    DataTable stockResultTable = CrearTablaConsulta(stockCmd);
                    if (stockResultTable.Rows.Count == 0)
                        return false;

                    command.CurrentStock = Convert.ToInt32(stockResultTable.Rows[0]["Stock"]);
                }

                string cartQuantityQuery = $"SELECT Quantity FROM NonPerishableCart WHERE ProductID = @ProductID AND UserID = @UserID";
                using (var cartCmd = new SqlCommand(cartQuantityQuery, _connection))
                {
                    cartCmd.Parameters.AddWithValue("@ProductID", command.ProductID);
                    cartCmd.Parameters.AddWithValue("@UserID", command.UserID);
                    DataTable cartResultTable = CrearTablaConsulta(cartCmd);
                    command.CurrentCartQuantity = cartResultTable.Rows.Count > 0
                        ? Convert.ToInt32(cartResultTable.Rows[0]["Quantity"])
                        : 0;
                }
            }

            return true;
        }

    }
}
