using backend.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Data;
using System.Data.SqlClient;
namespace backend.Handlers
{
    public class LogInHandler
    {
        private readonly SqlConnection _conexion;

        public LogInHandler(string connectionString)
        {
            _conexion = new SqlConnection(connectionString);
        }

        public async Task<bool> SearchUser(LogInModel logInData)
        {   // Search if there are a user with the same email and password in the database
            var consult = @"SELECT COUNT(*) FROM [dbo].[Profile] 
                     WHERE Email = @Email AND userPassword = @Password AND Deleted != 1";

            try
            {
                using var consultCommand = new SqlCommand(consult, _conexion);
                consultCommand.Parameters.AddWithValue("@Email", logInData.Email);
                consultCommand.Parameters.AddWithValue("@Password", logInData.Password);

                await _conexion.OpenAsync(); 

                var count = await consultCommand.ExecuteScalarAsync();

                await _conexion.CloseAsync(); 

                if (count != null && (int)count > 0) //  get if there is at least one user
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false; 
            }
        }
        public async Task<(string UserId, string UserType, DateTime LoginDate)> getUserInformation(LogInModel logInResponse)
        {
            string userId = null;
            string userType = null;
            DateTime loginDate = DateTime.MinValue;
            // get the credentials data
            string consult = @"SELECT UserID, UserType, GETDATE() AS LoginDate
                       FROM dbo.UserData
                       WHERE Email = @Email";

          
            await _conexion.OpenAsync();

            using (var cmd = new SqlCommand(consult, _conexion))
            {
                cmd.Parameters.AddWithValue("@Email", logInResponse.Email);

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        userId = reader["UserID"].ToString();
                        userType = reader["UserType"].ToString();
                        loginDate = (DateTime)reader["LoginDate"];
                    }
                }
            }

            // close conection
            await _conexion.CloseAsync();

            return (userId, userType, loginDate);
        }
    }
}