using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;
using backend.Domain;
using backend.Models;

namespace backend.Infrastructure
{
    public class UserDataHandler
    {
        private SqlConnection _connection;
        private string _routeConnection;
        public UserDataHandler()
        {
            var builder = WebApplication.CreateBuilder();
            _routeConnection = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public UserDataModel getUserData(int userID)
        {
            var getter =
                @"Select [ProfileName], [Email], [PhoneNumber]
                FROM [dbo].[Profile]				
                WHERE [UserID] = @UID";
            var commandGetter = new SqlCommand(getter, _connection);
            commandGetter.Parameters.AddWithValue("@UID", userID);
            _connection.Open();
            var reader = commandGetter.ExecuteReader();
            
            string profileName, email, phoneNumber;
            if (reader.Read())
            {
                profileName = reader["ProfileName"].ToString();
                email = reader["Email"].ToString();
                phoneNumber = reader["PhoneNumber"].ToString();

                UserDataModel userData = new UserDataModel();
                userData.name = profileName;
                userData.emailAddress = email;
                userData.phoneNumber = Int32.Parse(phoneNumber);
                _connection.Close();
                return userData;
            } else
            {
                _connection.Close();
                throw new Exception("No User with that ID found");
            }
        }

        public void updateUserData(UserDataModel data) {
            SqlCommand commandSetter = new SqlCommand("UpdateProfileData", _connection);
            commandSetter.CommandType = CommandType.StoredProcedure;
            commandSetter.Parameters.AddWithValue("@NewName", data.name);
            commandSetter.Parameters.AddWithValue("@NewEmail", data.emailAddress);
            commandSetter.Parameters.AddWithValue("@Newnumber", data.phoneNumber);
            commandSetter.Parameters.AddWithValue("@UID", data.UserID);
            _connection.Open( );
            int rowsAffected = commandSetter.ExecuteNonQuery();
            _connection.Close();
            if (rowsAffected <= 0) throw new Exception("Update failed on Profile");
            else
            {
                var setter2 =
                    @"UPDATE [dbo].[UserData]
                    SET [UserName] = @NewName,
                    [Email] = @NewEmail
				    WHERE [UserID] = @UID";
                var commandSetter2 = new SqlCommand(setter2, _connection);
                commandSetter2.Parameters.AddWithValue("@NewName", data.name);
                commandSetter2.Parameters.AddWithValue("@NewEmail", data.emailAddress);
                commandSetter2.Parameters.AddWithValue("@UID", data.UserID);

                _connection.Open();
                rowsAffected = commandSetter2.ExecuteNonQuery();
                _connection.Close();
                if (rowsAffected <= 0) throw new Exception("Update failed on UserData");
            }
        }
    }
}
