using backend.Application;
using System.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using backend.Infrastructure;
using backend.Domain;
using backend.Application.Orders;

namespace backend.Handlers
{
    public class AccountActivationHandler
    {
        private SqlConnection sqlConnection;
        private MailHandler mailHandler;
        private SecurityBodyBuilder securityBodyBuilder = null;
        private string connectionPath;
        private int CONFIRMATION_CODE_LENGHT = 6;


        public AccountActivationHandler(IMailService mailService)
        {
            var builder = WebApplication.CreateBuilder();
            this.connectionPath = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            this.sqlConnection = new SqlConnection(this.connectionPath);

            this.mailHandler = new MailHandler(mailService);
            this.securityBodyBuilder = new SecurityBodyBuilder();
            this.mailHandler.SetBodyBuilder(securityBodyBuilder);
            this.securityBodyBuilder.SetReason("Verification");
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


        private bool WriteToDatabase(SqlCommand cmd)
        {
            this.sqlConnection.Open();
            bool success = cmd.ExecuteNonQuery() >= 1;
            this.sqlConnection.Close();
            return success;
        }


        private AccountActivationModel GetAccountActivationData(string userId)
        {
            AccountActivationModel response = new AccountActivationModel();
            string request = @"SELECT UserId,ConfirmationCode,CreationDateTime FROM dbo.Profile WHERE UserId = @userId AND Deleted != 1";
            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("userId", userId);

            DataTable result = ReadFromDatabase(cmd);
            if (result.Rows.Count > 0)
            {
                response.userId = Convert.ToString(result.Rows[0]["UserId"]);
                response.confirmationCode = Convert.ToString(result.Rows[0]["ConfirmationCode"]);
                response.dateTimeLastCode = Convert.ToDateTime(result.Rows[0]["CreationDateTime"]);
            }
            return response;
        }


        private bool UpdateAccountState(string userId)
        {
            string request = @"UPDATE dbo.Profile SET accountStatus = @status WHERE UserID = @userId ";

            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("status", "Active");
            cmd.Parameters.AddWithValue("userId", userId);

            return this.WriteToDatabase(cmd);
        }


        public bool VerifyConfirmationCode(AccountActivationModel accountActivationModel)
        {
            AccountActivationModel accountInDatabase = GetAccountActivationData(accountActivationModel.userId);

            if (accountInDatabase != null)
            {
                TimeSpan timeSinceCreation = System.DateTime.Now - accountInDatabase.dateTimeLastCode;

                if (timeSinceCreation.Minutes <= 15
                 && accountActivationModel.confirmationCode == accountInDatabase.confirmationCode)
                {
                    return this.UpdateAccountState(accountInDatabase.userId);
                }
            }
            return false;
        }


        private DataTable GetUserEmailData(string userId)
        {
            string request = @"SELECT ProfileName,Email FROM dbo.Profile WHERE UserID = @userId AND Deleted != 1";

            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("userId", userId);

            return ReadFromDatabase(cmd);
        }


        private MailMessageModel BuildConfirmationEmail(string userId, string code)
        {
            MailMessageModel mailDataModel = new MailMessageModel();
            DataTable result = this.GetUserEmailData(userId);

            if (result.Rows.Count > 0)
            {
                mailDataModel.ReceiverMailAddress = Convert.ToString(result.Rows[0]["Email"]);
                mailDataModel.ReceiverMailName = Convert.ToString(result.Rows[0]["ProfileName"]);
                this.securityBodyBuilder.SetSecurityCode(code);
            }
            return mailDataModel;
        }


        private string HashCode(string code)
        {
            SHA512 hash = SHA512.Create();

            byte[] hashedCodeBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(code));
            return Convert.ToHexString(hashedCodeBytes);
        }


        public bool SendConfirmationEmail(string userId)
        {
            string code = RandomNumberGenerator.GetHexString(CONFIRMATION_CODE_LENGHT);
            MailMessageModel mailDataModel = this.BuildConfirmationEmail(userId, code);

            if (mailDataModel != null)
            {
                // Uses SHA 512 which is 128 bytes in size, not 64
                string hashedCode = this.HashCode(code);
                DateTime dateTimeNow = DateTime.Now;

                return this.UpdateConfirmationCode(hashedCode, dateTimeNow, userId)
                    && this.mailHandler.SendMail(mailDataModel);
            }
            return false;
        }


        private bool UpdateConfirmationCode(string hashedCode, DateTime dateTimeLastCode, string userId)
        {
            string request = @" UPDATE dbo.Profile SET ConfirmationCode = @hashedCode, CreationDateTime = @dateTimeLastCode 
                WHERE UserID = @userId AND Deleted != 1";

            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("hashedCode", hashedCode);

            // Format for 'u' (Universal): YYYY-MM-DD HH:MM:SSZ'
            cmd.Parameters.AddWithValue("dateTimeLastCode", dateTimeLastCode.ToString("u"));
            cmd.Parameters.AddWithValue("userId", userId);

            return WriteToDatabase(cmd);
        }
    }
}
