
using System.Data;
using System.Data.SqlClient;
using backend.Models;

namespace backend.Handlers
{
    public class registerUserHandler
    {
        public registerUserHandler() { 
            
        }

        public int addUser(registerUserModel data)
        {
            var query =
               @"INSERT INTO [dbo].[Empresa]
				([NombreEmpresa], [CedulaJuridica], [phoneNumber], [emailAddress])
				OUTPUT INSERTED.IDEmpresa
				VALUES (@NombreEmpresa, @CedulaJuridica, @PhoneNumber, @EmailAddress)";

        }
    }
}
