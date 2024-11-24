using backend.Commands;
using backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace backend.Infrastructure
{
    public class UpdateAddressHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;

        public UpdateAddressHandler()
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

        public async Task<bool> AddressExists(int addressID)
        {
            string query =
                "SELECT 1 FROM Address WHERE AddressID = @AddressID AND Deleted = 0"
                ;

            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@AddressID", addressID);
                DataTable resultTable = CrearTablaConsulta(cmd);
                return resultTable.Rows.Count > 0;
            }
        }
        public async Task<bool> AddressLinkedToUser(int userId, int addressID)
        {
            string query =
                "SELECT 1 FROM UserAddress WHERE AddressID = @AddressID AND UserID = @UserID"
                ;

            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@AddressID", addressID);
                cmd.Parameters.AddWithValue("@UserID", userId); 

                DataTable resultTable = CrearTablaConsulta(cmd);
                return resultTable.Rows.Count > 0;
            }
        }
        public async Task<bool> AddressLinkedToCompany(int companyId, int addressID)
        {
            string query =
                "SELECT 1 FROM CompanyAddress WHERE AddressID = @AddressID AND CompanyID = @CompanyID"
                ;

            using (var cmd = new SqlCommand(query, _connection))
            {
                cmd.Parameters.AddWithValue("@AddressID", addressID);
                cmd.Parameters.AddWithValue("@CompanyID", companyId);

                DataTable resultTable = CrearTablaConsulta(cmd);
                return resultTable.Rows.Count > 0;
            }
        }


        public async Task<bool> HandleUpdate(AddressModelUpdate address)
        {
            string updateQuery = "UpdateAddressData";

            using (var cmd = new SqlCommand(updateQuery, _connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AddressID", address.AddressID);
                cmd.Parameters.AddWithValue("@Province", address.Province);
                cmd.Parameters.AddWithValue("@Canton", address.Canton);
                cmd.Parameters.AddWithValue("@District", address.District);
                cmd.Parameters.AddWithValue("@ExactAddress", address.Exact);
                cmd.Parameters.AddWithValue("@Latitude", address.Latitude);
                cmd.Parameters.AddWithValue("@Longitude", address.Longitude);

                _connection.Open();
                int affectedRows = await cmd.ExecuteNonQueryAsync();
                _connection.Close();

                return affectedRows > 0;
            }
        }


    }
}
