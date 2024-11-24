using backend.Commands;
using System.Data;
using System.Data.SqlClient;

namespace backend.Infrastructure
{
    public class UpdateQuantityProductFromCartHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;

        public UpdateQuantityProductFromCartHandler()
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
                ? "SELECT 1 FROM PerishableProduct WHERE ProductID = @ProductID WHERE Deleted = 0"
                : "SELECT 1 FROM NonPerishableProduct WHERE ProductID = @ProductID WHERE Deleted = 0";

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


        public async Task<bool> HandleUpdateProductFromCart(int userId, int productId, bool isPerishable, int newQuantity)
        {
            string cartTable = isPerishable ? "PerishableCart" : "NonPerishableCart";
            string updateQuery = $@"
        UPDATE {cartTable}
        SET Quantity = @NewQuantity
        WHERE ProductID = @ProductID AND UserID = @UserID";

            using (var cmd = new SqlCommand(updateQuery, _connection))
            {
                cmd.Parameters.AddWithValue("@NewQuantity", newQuantity);
                cmd.Parameters.AddWithValue("@ProductID", productId);
                cmd.Parameters.AddWithValue("@UserID", userId);

                _connection.Open();
                int affectedRows = await cmd.ExecuteNonQueryAsync();
                _connection.Close();

                return affectedRows > 0;
            }
        }


    }
}
