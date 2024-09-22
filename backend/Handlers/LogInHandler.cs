using backend.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            var consult = @"SELECT COUNT(*) FROM [dbo].[Perfil] 
                     WHERE CorreoElectronico = @Email AND Contasena = @Password";

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
    }
}