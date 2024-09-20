using backend.Models;
using System.Data;
using System.Data.SqlClient;
namespace backend.Handlers
{
	public class CompanyDataHandler
	{
		private SqlConnection _connection;
		private string _routeConnection;
		public CompanyDataHandler()
		{
			var builder = WebApplication.CreateBuilder();
			_routeConnection = builder.Configuration.GetConnectionString("companyDataContext");
			_connection = new SqlConnection(_routeConnection);
		}
		public bool AddNewCompany(companyDataModel newCompany)
		{
			// TODO: Agregar consulta
			var query = "a";
			var commandForQuery = new SqlCommand(query, _connection);
			// TODO: Modificar los nombres
			commandForQuery.Parameters.AddWithValue("@Nombre", newCompany.companyName);
			commandForQuery.Parameters.AddWithValue("@Nombre", newCompany.cedula);
			commandForQuery.Parameters.AddWithValue("@Nombre", newCompany.emailAddress);
			commandForQuery.Parameters.AddWithValue("@Nombre", newCompany.phoneNumber);
			commandForQuery.Parameters.AddWithValue("@Nombre", newCompany.provincia);
			commandForQuery.Parameters.AddWithValue("@Nombre", newCompany.canton);
			commandForQuery.Parameters.AddWithValue("@Nombre", newCompany.distrito);
			commandForQuery.Parameters.AddWithValue("@Nombre", newCompany.exactAddress);

			_connection.Open();
			bool success = commandForQuery.ExecuteNonQuery() >= 1;
			_connection.Close();
			return success;
		}
	}
}
