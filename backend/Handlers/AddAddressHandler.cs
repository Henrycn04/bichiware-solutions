using backend.Models;
using System;
using System.ComponentModel.Design;
using System.Data;
using System.Data.SqlClient;

namespace backend.Handlers
{
    public class AddAddressHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public AddAddressHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("addAddressContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public void addNewAddress(AddAddressModel data)
        {
            int addrID = this.createAddress(data);
            var query =
                @"INSERT INTO [dbo].[UserAddress]
				([UserID], [AddressID])
				VALUES (@User, @Address)";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@User", data.userID);
            commandForQuery.Parameters.AddWithValue("@Address", addrID);

            _connection.Open();
            commandForQuery.ExecuteScalar();
            _connection.Close();
        }

        private int createAddress(AddAddressModel data)
        {
            var query =
                @"INSERT INTO [dbo].[Address]
				([Province], [Canton], [District], [ExactAddress])
				OUTPUT INSERTED.AddressID
				VALUES (@prov, @cant, @dist, @exactAddr)";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@prov", data.province);
            commandForQuery.Parameters.AddWithValue("@cant", data.canton);
            commandForQuery.Parameters.AddWithValue("@dist", data.district);
            commandForQuery.Parameters.AddWithValue("@exactAddr", data.exactAddress);

            _connection.Open();
            int addressID = (int)commandForQuery.ExecuteScalar();
            _connection.Close();
            return addressID;
        }
    }
}
