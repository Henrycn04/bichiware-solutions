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
            // La opci�n OUTPUT permite que el insert retorne un valor
            // En este caso se le est� diciendo que retorne el ID generado para la empresa reci�n a�adida a la tabla
            var query =
                @"INSERT INTO [dbo].[Empresa]
				([NombreEmpresa], [CedulaJuridica], [phoneNumber], [emailAddress])
				OUTPUT INSERTED.IDEmpresa
				VALUES (@NombreEmpresa, @CedulaJuridica, @PhoneNumber, @EmailAddress)";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@NombreEmpresa", newCompany.CompanyName);
            commandForQuery.Parameters.AddWithValue("@CedulaJuridica", newCompany.Cedula);
            commandForQuery.Parameters.AddWithValue("@PhoneNumber", newCompany.PhoneNumber);
            commandForQuery.Parameters.AddWithValue("@EmailAddress", newCompany.EmailAddress);

            _connection.Open();
            // ExecuteScalar ejecuta el comando SQL y atrapa el valor de retorno
            // Ese valor se convierte a int y se retorna
            int companyID = (int)commandForQuery.ExecuteScalar();
            _connection.Close();
            return companyID;
        }

        public int AddNewAddress(CompanyModel newCompany)
        {
            // Se hace lo mismo que con el AddCompany
            var query =
                @"INSERT INTO [dbo].[Direccion]
				([Provincia], [Canton], [Distrito], [Direccion_exacta])
				OUTPUT INSERTED.IDDireccion
				VALUES (@Provincia, @Canton, @Distrito, @ExactAddress)";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@Provincia", newCompany.Provincia);
            commandForQuery.Parameters.AddWithValue("@Canton", newCompany.Canton);
            commandForQuery.Parameters.AddWithValue("@Distrito", newCompany.Distrito);
            commandForQuery.Parameters.AddWithValue("@ExactAddress", newCompany.ExactAddress);

            _connection.Open();
            int addressID = (int)commandForQuery.ExecuteScalar();
            _connection.Close();
            return addressID;
        }

        public bool AddNewCompanyAddress(int newCompanyID, int newAddressID)
        {
            var query =
                @"INSERT INTO [dbo].[EmpresaDirecc]
				([IDEmpresa], [IDDirecc])
				VALUES (@IDEmpresa, @IDDirecc)";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@IDEmpresa", newCompanyID);
            commandForQuery.Parameters.AddWithValue("@IDDirecc", newAddressID);

            _connection.Open();
            bool success = commandForQuery.ExecuteNonQuery() >= 1;
            _connection.Close();
            return success;
        }
    }
}