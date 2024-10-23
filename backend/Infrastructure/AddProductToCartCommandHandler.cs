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

            string tableName = command.IsPerishable ? "PerishableProduct" : "NonPerishableProduct";

            if (!command.IsPerishable)
            {
                string query = $"SELECT Stock FROM NonPerishableProduct WHERE ProductID = @ProductID";

                using (var cmd = new SqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@ProductID", command.ProductID);
                    DataTable resultTable = CrearTablaConsulta(cmd);

                    if (resultTable.Rows.Count == 0) // Product does not exist
                        return false;

                    int stock = Convert.ToInt32(resultTable.Rows[0]["Stock"]);
                    if (!command.IsPerishable && command.Quantity > stock) // Check stock for non-perishable products
                        return false;
                }
            }
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
    }
}
