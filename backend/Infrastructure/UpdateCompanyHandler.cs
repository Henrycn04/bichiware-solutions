using backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public class UpdateCompanyHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public UpdateCompanyHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public void modifyCompanyData(CompanyProfileModel newData)
        {
            SqlCommand commandForQuery = new SqlCommand("UpdateCompanyData", _connection);
            commandForQuery.CommandType = CommandType.StoredProcedure;
            commandForQuery.Parameters.Add(new SqlParameter("@ID", newData.Id));
            commandForQuery.Parameters.Add(new SqlParameter("@CompanyName", newData.Name));
            commandForQuery.Parameters.Add(new SqlParameter("@Email", newData.Email));
            int phoneNumber = Convert.ToInt32(newData.PhoneNumber);
            int legalId = Convert.ToInt32(newData.LegalId);
            commandForQuery.Parameters.Add(new SqlParameter("@PhoneNumber", phoneNumber));
            commandForQuery.Parameters.Add(new SqlParameter("@LegalID", legalId));
            _connection.Open();
            commandForQuery.ExecuteNonQuery();
            _connection.Close();
        }
    }
}
