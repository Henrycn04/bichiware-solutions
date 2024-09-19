using System.Data.SqlClient;
using System.Data;
using backend.Models;
using backend.Services;

namespace backend.Handlers
{
    public class AccountActivationHandler
    {
        private SqlConnection sqlConnection;
        //private MailHandler mailHandler;
        private string connectionPath;

        public AccountActivationHandler()
        {
            var builder = WebApplication.CreateBuilder();
            this.connectionPath = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            this.sqlConnection = new SqlConnection(this.connectionPath);
            //this.mailHandler = new MailHandler(mailService);
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

        private AccountActivationModel GetAccountActivationData(string userId)
        {
            AccountActivationModel response = new AccountActivationModel();
            string request = @"SELECT IDUsuario,CodigoConfirmacion,FechaHoraDeUltimoCodigo FROM dbo.Perfil WHERE IDUsuario = @userId ";
            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("userId", userId);

            DataTable result = ReadFromDatabase(cmd);
            if (result.Rows.Count > 0)
            {
                response.userId = Convert.ToString(result.Rows[0]["IDUsuario"]);
                response.confirmationCode = Convert.ToString(result.Rows[0]["CodigoConfirmacion"]);
                response.dateTimeLastCode = Convert.ToDateTime(result.Rows[0]["FechaHoraDeUltimoCodigo"]);
            }
            return response;
        }


        public bool VerifyConfirmationCode(AccountActivationModel accountActivationModel)
        {
            AccountActivationModel accountInDatabase = GetAccountActivationData(accountActivationModel.userId);
            
            if (accountInDatabase != null)
            {
                TimeSpan timeSinceCreation = accountInDatabase.dateTimeLastCode - System.DateTime.Now;

                if (timeSinceCreation.Minutes <= 15
                 && accountActivationModel.confirmationCode == accountInDatabase.confirmationCode)
                {
                    return true;
                }
            }
            return false;
        }


        private MailDataModel GetAccountMainDetails(string userId)
        {
            MailDataModel mailDataModel = new MailDataModel();
            string request = @"SELECT NombrePerfil,CorreoElectronico FROM dbo.Perfil WHERE IDUsuario = @userId ";
            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("userId", userId);

            DataTable result = ReadFromDatabase(cmd);
            if (result.Rows.Count > 0)
            {
                mailDataModel.DestinationEmailAddress = Convert.ToString(result.Rows[0]["CorreoElectronico"]);
                mailDataModel.DestinationEmailName = Convert.ToString(result.Rows[0]["NombrePerfil"]);
            }
            return mailDataModel;
        }


        public bool SendConfirmationEmail(string userId)
        {
            MailDataModel mailDataModel = this.GetAccountMainDetails(userId);

            if (mailDataModel != null)
            {
                //return this.mailHandler.SendMail(mailDataModel);
            }
            return false;
        }
    }
}
