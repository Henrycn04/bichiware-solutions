using backend.Models;
using Org.BouncyCastle.Crypto;
using System;
using System.Data;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public class UserAddressesHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;

        public UserAddressesHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("CompanyDataContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public List<int> GetAddressesID(int UserID)
        {
            Console.WriteLine($"id usuario: {UserID}");
            List<int> addressesID = new List<int>();
            string query = "SELECT AddressID FROM [dbo].[UserAddress] WHERE UserID = @UserID";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@UserID", UserID);
            _connection.Open();
            using (SqlDataReader reader = commandForQuery.ExecuteReader())
            {
                while (reader.Read()) 
                {
                    int addressID = reader.GetInt32(0);
                    Console.WriteLine($"ID Address asociada: {addressID}");
                    addressesID.Add(addressID);
                }
            }
            _connection.Close();
            Console.WriteLine($"Address IDs en handler: {string.Join(", ", addressesID)}");
            return addressesID;
        }

        public List<UserAddressesModel> GetAddresses(List<int> addressesID)
        {
            List<UserAddressesModel> userAddresses = new List<UserAddressesModel>();
            string query = "SELECT Province, Canton, District, ExactAddress FROM [dbo].[Address] WHERE AddressID = @addressID";
            try
            {
                _connection.Open();

                for (int i = 0; i < addressesID.Count; i++)
                {
                    using (var commandForQuery = new SqlCommand(query, _connection))
                    {
                        commandForQuery.Parameters.AddWithValue("@addressID", addressesID[i]);

                        using (SqlDataReader reader = commandForQuery.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UserAddressesModel address = new UserAddressesModel
                                {
                                    Province = reader["Province"].ToString(),
                                    Canton = reader["Canton"].ToString(),
                                    District = reader["District"].ToString(),
                                    ExactAddress = reader["ExactAddress"].ToString(),
                                };
                                Console.WriteLine($"Address hallada: {address.Province} {address.Canton} {address.ExactAddress}");
                                userAddresses.Add(address);
                            }
                        }
                    }
                }
            }
            finally
            {
                _connection.Close();
            }
            Console.WriteLine($"Address 2: {string.Join(", ", userAddresses)}");
            return userAddresses;
        }




    }
}
