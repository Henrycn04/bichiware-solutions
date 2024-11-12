using System.Data;
using System.Data.SqlClient;
using backend.Models;

namespace backend.Handlers
{
    public class UserCompanyListHandler {
        private SqlConnection sqlConnection;
        private string connectionPath;


        public UserCompanyListHandler()
        {
            var builder = WebApplication.CreateBuilder();
            this.connectionPath = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            this.sqlConnection = new SqlConnection(this.connectionPath);
        }


        private DataTable ReadFromDatabase(SqlCommand cmd)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable response = new DataTable();

            this.sqlConnection.Open();
            adapter.Fill(response);
            this.sqlConnection.Close();

            return response;
        }


        private bool WriteToDatabase(SqlCommand cmd)
        {
            this.sqlConnection.Open();
            bool success = cmd.ExecuteNonQuery() >= 1;
            this.sqlConnection.Close();
            return success;
        }


        public List<UserModel> RequestAllUsers()
        {
            List<UserModel> users = new List<UserModel>(); 

            string request = @"SELECT dbo.Profile.UserID,dbo.Profile.ProfileName,dbo.UserData.IDNumber,dbo.Profile.Email,dbo.Profile.accountStatus
                                FROM dbo.UserData INNER JOIN dbo.Profile ON dbo.UserData.UserID = dbo.Profile.UserID ";
            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            DataTable result = this.ReadFromDatabase(cmd);

            if (result.Rows.Count > 0)
            {
                foreach (DataRow row in result.Rows)
                {
                    string[] userName = Convert.ToString(row["ProfileName"]).Split(' ');
                    users.Add(new UserModel
                    {
                        UserId = Convert.ToInt16(row["UserID"]),
                        FirstName = userName[0],
                        LastName = userName[1],
                        LegalId = Convert.ToString(row["IDNumber"]),
                        Email = Convert.ToString(row["Email"]),
                        AccountStatus = Convert.ToString(row["accountStatus"]).Trim()
                    });
                }
            }
            return users;
        }


        public List<CompanyProfileModel> RequestAllCompanies()
        {
            List<CompanyProfileModel> companies = new List<CompanyProfileModel>();

            string request = @"SELECT * FROM dbo.Company ";
            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            DataTable result = this.ReadFromDatabase(cmd);

            foreach (DataRow row in result.Rows)
            {
                companies.Add(new CompanyProfileModel
                {
                    Id = Convert.ToInt16(row["CompanyID"]),
                    Name = Convert.ToString(row["CompanyName"]),
                    PhoneNumber = Convert.ToString(row["PhoneNumber"]),
                    Email = Convert.ToString(row["EmailAddress"]),
                    LegalId = Convert.ToString(row["LegalID"])
                });
            }
            return companies;
        }


        public int GetUserType(string userId)
        {
            string request = "SELECT UserType FROM dbo.UserData WHERE UserID = @userId";
            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("userId", userId);
            DataTable result = this.ReadFromDatabase(cmd);

            if (result.Rows.Count > 0)
            {
                int userType = Convert.ToInt32(result.Rows[0]["UserType"]);
                return userType;
            }
            throw new Exception("User does not exists");
        }


        private DataTable GetOwnedCompaniesIDs(string userId)
        {
            string request = "SELECT CompanyID FROM dbo.CompanyProfiles WHERE UserID = @userId";
            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);

            cmd.Parameters.AddWithValue("userId", userId);

            DataTable result = this.ReadFromDatabase(cmd);
            return result;
        }


        private void GetSubordinatesToList(string companyId, List<UserModel> subordinates)
        {
            string request = @"SELECT TEMP.UserID,TEMP.ProfileName,TEMP.IDNumber,TEMP.Email,TEMP.accountStatus
                                    FROM dbo.CompanyMembers FULL OUTER JOIN
                                        (SELECT dbo.Profile.UserID,dbo.Profile.ProfileName,dbo.UserData.IDNumber,dbo.Profile.Email,dbo.Profile.accountStatus
                                            FROM dbo.UserData INNER JOIN dbo.Profile
                                            ON dbo.UserData.UserID = dbo.Profile.UserID)
                                        AS TEMP
                                        ON TEMP.UserID = dbo.CompanyMembers.UserID
                                    WHERE dbo.CompanyMembers.CompanyID = @companyId";
            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("companyId", companyId);

            DataTable result = this.ReadFromDatabase(cmd);

            foreach (DataRow row in result.Rows)
            {
                string[] userName = Convert.ToString(row["ProfileName"]).Split(' ');
                subordinates.Add(new UserModel
                {
                    UserId = Convert.ToInt16(row["UserID"]),
                    FirstName = userName[0],
                    LastName = userName[1],
                    LegalId = Convert.ToString(row["IDNumber"]),
                    Email = Convert.ToString(row["Email"]),
                    AccountStatus = Convert.ToString(row["accountStatus"]).Trim()
                });
            }
        }


        public List<UserModel> RequestSubordinates(string userId)
        {
            DataTable ownedCompaniesIDs = this.GetOwnedCompaniesIDs(userId);
            List<UserModel> subordintes = new List<UserModel>();

            foreach (DataRow companyId in ownedCompaniesIDs.Rows)
            {
                this.GetSubordinatesToList(Convert.ToString(companyId["CompanyID"]), subordintes);
            }
            return subordintes;
        }


        private void GetOwnedCompaniesToList(string companyId, List<CompanyProfileModel> companies)
        {
            string request = @"SELECT * FROM dbo.Company WHERE CompanyID = @companyId";

            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("companyId", companyId);

            DataTable result = this.ReadFromDatabase(cmd);

            foreach (DataRow row in result.Rows)
            {
                companies.Add(new CompanyProfileModel
                {
                    Id = Convert.ToInt16(row["CompanyID"]),
                    Name = Convert.ToString(row["CompanyName"]),
                    PhoneNumber = Convert.ToString(row["PhoneNumber"]),
                    Email = Convert.ToString(row["EmailAddress"]),
                    LegalId = Convert.ToString(row["LegalID"])
                });
            }
        }


        public List<CompanyProfileModel> RequestOwnedCompanies(string userId)
        {
            DataTable ownedCompaniesIDs = this.GetOwnedCompaniesIDs(userId);
            List<CompanyProfileModel> ownedCompanies = new List<CompanyProfileModel>();

            foreach (DataRow companyId in ownedCompaniesIDs.Rows)
            {
                this.GetOwnedCompaniesToList(Convert.ToString(companyId["CompanyID"]), ownedCompanies);
            }
            return ownedCompanies;
        }
    }
}
