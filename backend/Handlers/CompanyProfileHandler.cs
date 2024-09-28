using backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public class CompanyProfileHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public CompanyProfileHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("CompanyDataContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public List<CompaniesIDModel> getUserCompanies(int UserID)
        {
            List< CompaniesIDModel > userCompanies = new List<CompaniesIDModel>();
            List<int> companiesIDs = new List<int>();
            string query = "SELECT CompanyID FROM [dbo].[CompanyProfiles] WHERE UserID = @UserID";
            SqlCommand commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@UserID", UserID);
            _connection.Open();
            using (SqlDataReader reader = commandForQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    companiesIDs.Add(reader.GetInt32("CompanyID"));
                }
            }
            _connection.Close();
            List<string> companiesNames = new List<string>();
            string query2 = "SELECT CompanyName FROM Company WHERE CompanyID IN (" + string.Join(",", companiesIDs) + ")";
            SqlCommand commandForQuery2 = new SqlCommand(query2, _connection);
            _connection.Open();
            using (SqlDataReader reader = commandForQuery2.ExecuteReader())
            {
                while (reader.Read())
                {
                    companiesNames.Add(reader.GetString("CompanyName"));
                }
            }
            _connection.Close();
            for (int i = 0; i < companiesIDs.Count(); i++)
            {
                CompaniesIDModel companiesIDModel = new CompaniesIDModel
                {
                    CompanyID = companiesIDs[i],
                    CompanyName = companiesNames[i]
                };

                userCompanies.Add(companiesIDModel);
            }

            return userCompanies;
        }
        public CompanyProfileModel getCompanyData(int companyID)
        {
            CompanyProfileModel companyProfileModel = new CompanyProfileModel();
            companyProfileModel.Members = new List<CompanyProfileModel.MembersModel>();
            List<int> membersIDs = new List<int>();
            string queryForMembersIDs = "SELECT UserID FROM CompanyMembers WHERE CompanyID = @companyID";
            SqlCommand commandForQuery = new SqlCommand(queryForMembersIDs, _connection);
            commandForQuery.Parameters.AddWithValue("@companyID", companyID);

            _connection.Open();
            using (SqlDataReader reader = commandForQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    membersIDs.Add(reader.GetInt32(0));
                }
            }
            _connection.Close();

            if (membersIDs.Count > 0)
            {
                string queryForMembersData = "SELECT ProfileName, Email, PhoneNumber FROM Profile WHERE UserID IN (" + string.Join(",", membersIDs) + ")";
                SqlCommand commandForQuery2 = new SqlCommand(queryForMembersData, _connection);

                _connection.Open();
                using (SqlDataReader reader = commandForQuery2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var member = new CompanyProfileModel.MembersModel
                        {
                            Username = reader["ProfileName"].ToString(),
                            Email = reader["Email"].ToString(),
                            PhoneNumber = reader["PhoneNumber"].ToString()
                        };
                        companyProfileModel.Members.Add(member);
                    }
                }
                _connection.Close();
            }

            companyProfileModel.Addresses = new List<CompanyProfileModel.CompanyAddressModel>();
            List<int> companyAddressesIDs = new List<int>();
            string queryForCompanyAddresses = "SELECT AddressID FROM CompanyAddress WHERE CompanyID = @companyID";
            SqlCommand commandForQuery3 = new SqlCommand(queryForCompanyAddresses, _connection);
            commandForQuery3.Parameters.AddWithValue("@companyID", companyID);

            _connection.Open();
            using (SqlDataReader reader = commandForQuery3.ExecuteReader())
            {
                while (reader.Read())
                {
                    companyAddressesIDs.Add(reader.GetInt32(0));
                }
            }
            _connection.Close();

            if (companyAddressesIDs.Count > 0)
            {
                string queryForCompanyAddressesData = "SELECT Province, Canton, District, ExactAddress FROM Address WHERE AddressID IN (" + string.Join(",", companyAddressesIDs) + ")";
                SqlCommand commandForQuery4 = new SqlCommand(queryForCompanyAddressesData, _connection);

                _connection.Open();
                using (SqlDataReader reader = commandForQuery4.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var address = new CompanyProfileModel.CompanyAddressModel
                        {
                            Province = reader["Province"].ToString(),
                            Canton = reader["Canton"].ToString(),
                            District = reader["District"].ToString(),
                            ExactAddress = reader["ExactAddress"].ToString()
                        };
                        companyProfileModel.Addresses.Add(address);
                    }
                }
                _connection.Close();
            }
            string queryForCompanyData = "SELECT CompanyName, PhoneNumber, EmailAddress, LegalID FROM Company WHERE CompanyID = @userID";
            SqlCommand commandForQuery5 = new SqlCommand(queryForCompanyData, _connection);
            commandForQuery5.Parameters.AddWithValue("@userID", companyID);
            _connection.Open();
            using (SqlDataReader reader = commandForQuery5.ExecuteReader())
            {
                while (reader.Read())
                {
                    companyProfileModel.CompanyName = reader["CompanyName"].ToString();
                    companyProfileModel.Cedula = reader["LegalID"].ToString();
                    companyProfileModel.PhoneNumber = reader["PhoneNumber"].ToString();
                    companyProfileModel.Email = reader["EmailAddress"].ToString();
                }
            }
            _connection.Close();

            return companyProfileModel;
        }
    }
}
