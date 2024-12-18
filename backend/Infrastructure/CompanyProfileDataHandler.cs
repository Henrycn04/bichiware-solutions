using backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public interface ICompanyProfileDataHandler
    {
        List<CompaniesIDModel> getUserCompanies(int userID);
        CompanyProfileDataModel getCompanyData(int companyID);
        List<ProductForDeliveriesModel> getCompanyProducts(int companyID);
        bool userHasRelatedCompanies(int userID);
    }
    public class CompanyProfileDataHandler : ICompanyProfileDataHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public CompanyProfileDataHandler()
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
            if (companiesIDs.Count > 0)
            {
                string query2 = "SELECT CompanyName FROM Company WHERE CompanyID IN (" + string.Join(",", companiesIDs) + ") AND Deleted = 0";
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
            }
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
        public CompanyProfileDataModel getCompanyData(int companyID)
        {
            CompanyProfileDataModel companyProfileModel = new CompanyProfileDataModel();
            companyProfileModel.Members = new List<CompanyProfileDataModel.MembersModel>();
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
                string queryForMembersData = "SELECT ProfileName, Email, PhoneNumber FROM Profile WHERE UserID IN (" + string.Join(",", membersIDs) + ") AND Deleted != 1";
                SqlCommand commandForQuery2 = new SqlCommand(queryForMembersData, _connection);

                _connection.Open();
                using (SqlDataReader reader = commandForQuery2.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var member = new CompanyProfileDataModel.MembersModel
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

            companyProfileModel.Addresses = new List<CompanyProfileDataModel.CompanyAddressModel>();
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
                string queryForCompanyAddressesData = "SELECT AddressID, Province, Canton, District, ExactAddress FROM Address WHERE AddressID IN (" + string.Join(",", companyAddressesIDs) + ") AND Deleted = 0";
                SqlCommand commandForQuery4 = new SqlCommand(queryForCompanyAddressesData, _connection);

                _connection.Open();
                using (SqlDataReader reader = commandForQuery4.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var address = new CompanyProfileDataModel.CompanyAddressModel
                        {
                            AddressID = Convert.ToInt32(reader["AddressID"]),
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
            string queryForCompanyData = "SELECT CompanyName, PhoneNumber, EmailAddress, LegalID FROM Company WHERE CompanyID = @userID AND Deleted = 0";
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

        public List<ProductForDeliveriesModel> getCompanyProducts(int companyID)
        {
            List<ProductForDeliveriesModel> companyProducts = new List<ProductForDeliveriesModel>();
            string query = "SELECT ProductName, ProductID FROM [dbo].[PerishableProduct] WHERE companyID = @companyID AND Deleted = 0";

            SqlCommand commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@companyID", companyID);

           _connection.Open();
            using (SqlDataReader reader = commandForQuery.ExecuteReader())
            {
                while (reader.Read())
                {
                    ProductForDeliveriesModel companyProduct = new ProductForDeliveriesModel();
                    companyProduct.productName = reader["ProductName"].ToString();
                    companyProduct.productID = reader.GetInt32("ProductID");
                    companyProducts.Add(companyProduct);
                }
            }
            _connection.Close();
            return companyProducts;
        }

        public bool userHasRelatedCompanies(int userID)
        {
            string query = @"
             SELECT COUNT(*)
                FROM CompanyMembers o
                WHERE o.UserID = @UserID
                  AND NOT EXISTS (
                      SELECT 1
                      FROM CompanyMembers o2
                      WHERE o2.CompanyID = o.CompanyID
                        AND o2.UserID != o.UserID
                  );
            ";

            using (SqlCommand command = new SqlCommand(query, _connection))
            {
                command.Parameters.AddWithValue("@UserID", userID);

                _connection.Open();
                int count = (int)command.ExecuteScalar();
                _connection.Close();

                return count > 0;
            }
        }
    }
}
