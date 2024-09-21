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
			_routeConnection = builder.Configuration.GetConnectionString("CompanyDataContext");
			_connection = new SqlConnection(_routeConnection);
		}

        public int AddNewCompany(CompanyModel newCompany)
		{
            var query =
                @"INSERT INTO [dbo].[Empresa]
				([NombreEmpresa], [CedulaJuridica], [phoneNumber], [emailAddress])
				VALUES (@NombreEmpresa, @CedulaJuridica, @PhoneNumber, @EmailAddress)";
            var commandForQuery = new SqlCommand(query, _connection);
			// TODO: Modificar los nombres
			commandForQuery.Parameters.AddWithValue("@NombreEmpresa", newCompany.CompanyName);
			commandForQuery.Parameters.AddWithValue("@CedulaJuridica", newCompany.Cedula);
			commandForQuery.Parameters.AddWithValue("@PhoneNumber", newCompany.PhoneNumber);
			commandForQuery.Parameters.AddWithValue("@EmailAddress", newCompany.EmailAddress);

			_connection.Open();
			int companyID = (int)commandForQuery.ExecuteNonQuery();
			_connection.Close();
			return companyID;
		}

		public int AddNewAddress(AddressModel newAddress)
		{
			var query = 
				@"INSERT INTO [dbo].[Direccion]
				([Provincia], [Canton], [Distrito], [Direccion_exacta])
				OUTPUT INSERTED.IDDirecc
				VALUES (@Provincia, @Canton, @Distrito, @ExactAddress)";
			var commandForQuery = new SqlCommand(query, _connection);
			commandForQuery.Parameters.AddWithValue("@Provincia", newAddress.Provincia);
            commandForQuery.Parameters.AddWithValue("@Canton", newAddress.Canton);
            commandForQuery.Parameters.AddWithValue("@Distrito", newAddress.Distrito);
            commandForQuery.Parameters.AddWithValue("@ExactAddress", newAddress.ExactAddress);

            _connection.Open();
            int addressID = (int)commandForQuery.ExecuteNonQuery();
            _connection.Close();
            return addressID;
        }

		public bool AddNewCompanyAddress(CompanyAddressModel newCompanyAddress)
		{
            var query =
                @"INSERT INTO [dbo].[EmpresaDirecc]
				([IDEmpresa], [IDDirecc])
				VALUES (@CompanyID, @AddressID)";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@IDEmpresa", newCompanyAddress.CompanyID);
            commandForQuery.Parameters.AddWithValue("@IDDirecc", newCompanyAddress.AddressID);

            _connection.Open();
            bool success = commandForQuery.ExecuteNonQuery() >= 1;
            _connection.Close();
            return success;
        }
    }
}
