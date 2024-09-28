using backend.Models;
using System;
using System.ComponentModel.Design;
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
            // La opción OUTPUT permite que el insert retorne un valor
            // En este caso se le está diciendo que retorne el ID generado para la empresa recién añadida a la tabla
            var query =
                @"INSERT INTO [dbo].[Company]
				([CompanyName], [LegalID], [PhoneNumber], [EmailAddress])
				OUTPUT INSERTED.CompanyID
				VALUES (@CompanyName, @LegalID, @PhoneNumber, @EmailAddress)";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@CompanyName", newCompany.CompanyName);
            commandForQuery.Parameters.AddWithValue("@LegalID", newCompany.Cedula);
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
                @"INSERT INTO [dbo].[Address]
				([Province], [Canton], [District], [ExactAddress])
				OUTPUT INSERTED.AddressID
				VALUES (@Province, @Canton, @District, @ExactAddress)";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@Province", newCompany.Provincia);
            commandForQuery.Parameters.AddWithValue("@Canton", newCompany.Canton);
            commandForQuery.Parameters.AddWithValue("@District", newCompany.Distrito);
            commandForQuery.Parameters.AddWithValue("@ExactAddress", newCompany.ExactAddress);

            _connection.Open();
            int addressID = (int)commandForQuery.ExecuteScalar();
            _connection.Close();
            return addressID;
        }

        public bool AddNewCompanyAddress(int newCompanyID, int newAddressID)
        {
            var query =
                @"INSERT INTO [dbo].[CompanyAddress]
				([CompanyID], [AddressID])
				VALUES (@CompanyID, @AddressID)";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@CompanyID", newCompanyID);
            commandForQuery.Parameters.AddWithValue("@AddressID", newAddressID);

            _connection.Open();
            bool success = commandForQuery.ExecuteNonQuery() >= 1;
            _connection.Close();
            return success;
        }

        public async Task<string> getCompanyName(int companyID)
        {
            var query = @"SELECT CompanyName FROM [dbo].[Company] WHERE CompanyID = @companyID";
            try
            {
                using var consultCommand = new SqlCommand(query, _connection);
                consultCommand.Parameters.AddWithValue("@companyID", companyID);
                await _connection.OpenAsync();

                using var reader = await consultCommand.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var companyName = reader.GetString(0);
                    return companyName;
                }

                return ""; 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return "";
            }
            finally
            {
                await _connection.CloseAsync(); 
            }
        }
    }
}