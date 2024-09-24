
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
            _routeConnection = builder.Configuration.GetConnectionString("CompanyDataContext");
            _connection = new SqlConnection(_routeConnection);
        }

        public int addProfile(registerUserModel data)
        {
            var query =
               @"INSERT INTO [dbo].[Perfil]
				([NombrePerfil], [Correo], [Contrasena], [NumeroTelefono])
				OUTPUT INSERTED.IDUsuario
				VALUES (@NombrePerfil, @Correo, @Contrasena, @NumeroTelefono)";
            var commandForQuery = new SqlCommand(query, _connection);
            string name = data.name + " " + data.lastName;
            commandForQuery.Parameters.AddWithValue("@NombrePerfil",name);
            commandForQuery.Parameters.AddWithValue("@Correo", data.email);
            commandForQuery.Parameters.AddWithValue("@Contrasena", data.password);
            commandForQuery.Parameters.AddWithValue("@NumeroTelefono", data.phoneNumber);

            _connection.Open();
            int IDResult = (int) commandForQuery.ExecuteScalar();
            _connection.Close();
            return IDResult;
        }

        public void addUser(registerUserModel data, int userID)
        {
            var query =
               @"INSERT INTO [dbo].[Usuario]
				([NombreUsuario], [IDUsuario], [CorreoElectronico], [Cedula])
				VALUES (@Nombre, @IDUser, @Correo, @Cedula)";
            var commandForQuery = new SqlCommand(query, _connection);
            string name = data.name + " " + data.lastName;
            commandForQuery.Parameters.AddWithValue("@NombrePerfil", name);
            commandForQuery.Parameters.AddWithValue("@IDUser", userID);
            commandForQuery.Parameters.AddWithValue("@Correo", data.email);
            commandForQuery.Parameters.AddWithValue("@Cedula", data.cedula);

            _connection.Open();
            commandForQuery.ExecuteScalar();
            _connection.Close();
        }

        public int addDirecc(registerUserModel data, int userID) {
            var query =
               @"INSERT INTO [dbo].[Direccion]
				([Provincia], [Canton], [Distrito], [Direccion_exacta])
                OUTPUT INSERTED.IDDireccion
				VALUES (@province, @canton, @distrito, @exactDir)";
            var commandForQuery = new SqlCommand(query, _connection);
            commandForQuery.Parameters.AddWithValue("@province", data.province);
            commandForQuery.Parameters.AddWithValue("@canton", data.canton);
            commandForQuery.Parameters.AddWithValue("@distrito", data.district);
            commandForQuery.Parameters.AddWithValue("@exactDir", data.exactAddress);

            _connection.Open();
            int dirID = (int) commandForQuery.ExecuteScalar();
            _connection.Close();
            return dirID;
        }

        public void addReferencesDirecc(int userID, int direccID)
        {
            var query =
               @"INSERT INTO [dbo].[UserDirecc]
				([IDUsuario], [IDDirecc])
				VALUES (@idUser, @idAddr)";
            var commandForQuery = new SqlCommand( query, _connection);
            commandForQuery.Parameters.AddWithValue("@idUser",userID);
            commandForQuery.Parameters.AddWithValue("@idAddr", direccID);
        }
    }
}
