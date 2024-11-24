using backend.Models;
using System.Data;
using System.Data.SqlClient;

namespace backend.Infrastructure
{
    public class AccountAddressesHandler
    {
        private SqlConnection sqlConnection;
        private string connectionPath;


        public AccountAddressesHandler()
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


        public List<AddressModel> RequestUserAddresses(string userId)
        {
            List<AddressModel> addresses = new List<AddressModel>();
            string request = @"SELECT *
	                                FROM dbo.Address FULL OUTER JOIN dbo.UserAddress
	                                ON dbo.Address.AddressID = dbo.UserAddress.AddressID
	                                WHERE dbo.UserAddress.UserID = @userId AND dbo.Address.Deleted = 0";
            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("userId", userId);

            DataTable result = this.ReadFromDatabase(cmd);

            foreach (DataRow row in result.Rows)
            {
                addresses.Add(new AddressModel
                {
                    AddressID = Convert.ToInt32(row["AddressID"]),
                    Province = Convert.ToString(row["Province"]),
                    Canton = Convert.ToString(row["Canton"]),
                    District = Convert.ToString(row["District"]),
                    Exact = Convert.ToString(row["ExactAddress"]),
                    Latitude = Convert.ToDecimal(row["Latitude"]), 
                    Longitude = Convert.ToDecimal(row["Longitude"]) 
                });
            }

            return addresses;
        }
    }
}
