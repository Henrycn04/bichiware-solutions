using backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public class CompanyMainDataHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public CompanyMainDataHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public CompanyProfileModel GetCompanyMainData(int companyID)
        {
            CompanyProfileModel companyProfileModel = new CompanyProfileModel();
            string query = "SELECT CompanyName, LegalID, PhoneNumber, EmailAddress FROM [dbo].[Company] WHERE companyID = @companyID AND Deleted = 0";

            SqlCommand commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@companyID", companyID);

            _connection.Open();
            using (SqlDataReader reader = commandForQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    companyProfileModel.Id = companyID;
                    companyProfileModel.Name = reader["CompanyName"].ToString();
                    companyProfileModel.Email = reader["EmailAddress"].ToString();
                    companyProfileModel.LegalId = reader["LegalID"].ToString();
                    companyProfileModel.PhoneNumber = reader["PhoneNumber"].ToString();
                }
            }
            _connection.Close();
            return companyProfileModel;
        }
    }
}
