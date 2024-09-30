
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using backend.Models;

namespace backend.Handlers
{
    public class registerUserHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public registerUserHandler() {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("RegisterUser");
            _connection = new SqlConnection(_routeConnection);
        }

        public int addProfile(registerUserModel data)
        {
            var query =
               @"INSERT INTO [dbo].[Profile]
				([ProfileName], [Email], [userPassword], [PhoneNumber])
				OUTPUT INSERTED.UserID
				VALUES (@ProfileName, @EmailAddr, @Passw, @CellNum)";
            var commandForQuery = new SqlCommand(query, _connection);
            string name = data.name + " " + data.lastName;
            commandForQuery.Parameters.AddWithValue("@ProfileName",name);
            commandForQuery.Parameters.AddWithValue("@EmailAddr", data.email);
            commandForQuery.Parameters.AddWithValue("@Passw", data.password);
            commandForQuery.Parameters.AddWithValue("@CellNum", data.phoneNumber);

            _connection.Open();
            int IDResult = (int) commandForQuery.ExecuteScalar();
            _connection.Close();
            return IDResult;
        }

        public void addUser(registerUserModel data, int userID)
        {
            var query =
               @"INSERT INTO [dbo].[UserData]
				([UserName], [UserID], [Email], [IDNumber])
				VALUES (@Name, @UID, @EmailAddr, @IDN)";
            var commandForQuery = new SqlCommand(query, _connection);
            string name = data.name + " " + data.lastName;
            commandForQuery.Parameters.AddWithValue("@Name", name);
            commandForQuery.Parameters.AddWithValue("@UID", userID);
            commandForQuery.Parameters.AddWithValue("@EmailAddr", data.email);
            commandForQuery.Parameters.AddWithValue("@IDN", data.cedula);

            _connection.Open();
            commandForQuery.ExecuteScalar();
            _connection.Close();
        }

        public int addAddr(registerUserModel data, int userID) {
            var query =
               @"INSERT INTO [dbo].[Address]
				([Province], [Canton], [District], [ExactAddress])
                OUTPUT INSERTED.AddressID
				VALUES (@provinceN, @cantonN, @districtN, @exactAddr)";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@provinceN", data.province);
            commandForQuery.Parameters.AddWithValue("@cantonN", data.canton);
            commandForQuery.Parameters.AddWithValue("@districtN", data.district);
            commandForQuery.Parameters.AddWithValue("@exactAddr", data.exactAddress);

            _connection.Open();
            int dirID = (int) commandForQuery.ExecuteScalar();
            _connection.Close();
            return dirID;
        }

        public void addReferencesAddr(int userID, int addrID)
        {
            var query =
               @"INSERT INTO [dbo].[UserAddress]
				([UserID], [AddressID])
				VALUES (@idUser, @idAddr)";
            var commandForQuery = new SqlCommand( query, _connection);
            commandForQuery.Parameters.AddWithValue("@idUser",userID);
            commandForQuery.Parameters.AddWithValue("@idAddr", addrID);

            _connection.Open();
            commandForQuery.ExecuteScalar();
            _connection.Close();
        }
    }
}
