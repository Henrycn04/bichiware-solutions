﻿using backend.Models;
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

        public void addNewAddress(PhysicalAddress data)
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

        private int createAddress(PhysicalAddress data)
        {
            var query =
                @"INSERT INTO [dbo].[Address]
				([Province], [Canton], [District], [ExactAddress], [Latitude], [Longitude])
				OUTPUT INSERTED.AddressID
				VALUES (@prov, @cant, @dist, @exactAddr, @lat, @lon)";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@prov", data.province);
            commandForQuery.Parameters.AddWithValue("@cant", data.canton);
            commandForQuery.Parameters.AddWithValue("@dist", data.district);
            commandForQuery.Parameters.AddWithValue("@exactAddr", data.exactAddress);
            commandForQuery.Parameters.AddWithValue("@lat", data.lat);
            commandForQuery.Parameters.AddWithValue("@lon", data.lon);

            _connection.Open();
            int addressID = (int)commandForQuery.ExecuteScalar();
            _connection.Close();
            return addressID;
        }
    }
}
