using backend.Models;
using backend.Services;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;


namespace backend.Handlers
{
    public class ChangePasswordHandler
    {
        private SqlConnection sqlConnection;
        private string connectionPath;
        private MailHandler mailHandler;
        private int CONFIRMATION_CODE_LENGHT = 6;


        public ChangePasswordHandler(IMailService mailService)
        {
            var builder = WebApplication.CreateBuilder();
            this.connectionPath = builder.Configuration.GetConnectionString("BichiwareSolutionsContext");
            this.sqlConnection = new SqlConnection(this.connectionPath);
            this.mailHandler = new MailHandler(mailService);
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


        private DataTable GetUserEmailData(string email)
        {
            string request = @"SELECT ProfileName FROM dbo.Profile WHERE Email = @email ";

            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("email", email);

            return ReadFromDatabase(cmd);
        }


        private MailDataModel BuildConfirmationEmail(string email, string code)
        {
            MailDataModel mailDataModel = new MailDataModel();
            DataTable result = this.GetUserEmailData(email);

            if (result.Rows.Count > 0)
            {
                mailDataModel.DestinationEmailAddress = email;
                mailDataModel.DestinationEmailName = Convert.ToString(result.Rows[0]["ProfileName"]);
                mailDataModel.EmailBody = @"Hola, se ha solicitado un cambio de contraseña para su perfíl
                    . Tu código de seguridad es: " + code + @". No le des este código a nadie.";
                mailDataModel.EmailSubject = @"Bichiware: Alerta. Su contraseña está siendo actualizada.";
            }
            return mailDataModel;
        }


        private AccountSecurityDataModel GetAccountSecurityData(string email)
        {
            AccountSecurityDataModel accountSecurityDataModel = null;
            string request = @"SELECT UserID,ConfirmationCode,CreationDateTime,userPassword FROM dbo.Profile WHERE Email = @email ";

            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("email", email);

            DataTable result = ReadFromDatabase(cmd);
            if (result.Rows.Count > 0)
            {
                accountSecurityDataModel = new AccountSecurityDataModel();
                accountSecurityDataModel.userId = Convert.ToString(result.Rows[0]["UserID"]);
                accountSecurityDataModel.securityCode = Convert.ToString(result.Rows[0]["ConfirmationCode"]);
                accountSecurityDataModel.dateTimeLastCode = Convert.ToDateTime(result.Rows[0]["CreationDateTime"]);
                accountSecurityDataModel.password = Convert.ToString(result.Rows[0]["userPassword"]);
            }
            return accountSecurityDataModel;
        }


        private bool VerifySecurityCode(string email, string securityCode)
        {
            AccountSecurityDataModel securityDataFromDatabase = this.GetAccountSecurityData(email);

            if (securityDataFromDatabase != null)
            {
                Console.WriteLine("securityData retrieved from database");
                TimeSpan timeSinceLastCode = DateTime.Now - securityDataFromDatabase.dateTimeLastCode;

                Console.WriteLine("timeSinceLastCode: " + timeSinceLastCode.Minutes);

                return securityCode == securityDataFromDatabase.securityCode
                    && timeSinceLastCode.Minutes <= 15;
            }
            return false;
        }


        private bool UpdatePassword(string email, string newPassword)
        {
            string request = @" UPDATE dbo.Profile SET userPassword = @newPassword WHERE Email = @email";

            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("newPassword", newPassword);
            cmd.Parameters.AddWithValue("email", email);

            return WriteToDatabase(cmd);
        }


        public bool AttemptChangePassword(ChangePasswordModel changePasswordModel)
        {
            if (this.VerifySecurityCode(changePasswordModel.email, changePasswordModel.securityCode))
            {
                Console.WriteLine("Verifying success");
                return this.UpdatePassword(changePasswordModel.email, changePasswordModel.newPassword);
            }
            Console.WriteLine("Error at verifying code");
            return false;
        }


        private string HashCode(string code)
        {
            SHA512 hash = SHA512.Create();

            byte[] hashedCodeBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(code));
            return Convert.ToHexString(hashedCodeBytes);
        }


        public bool SendConfirmationEmail(string email)
        {
            string code = RandomNumberGenerator.GetHexString(CONFIRMATION_CODE_LENGHT);
            MailDataModel mailDataModel = this.BuildConfirmationEmail(email, code);

            if (mailDataModel != null)
            {
                // Uses SHA 512 which is 128 bytes in size, not 64
                string hashedCode = this.HashCode(code);
                DateTime dateTimeNow = DateTime.Now;

                return this.UpdateConfirmationCode(hashedCode, dateTimeNow, email)
                    && this.mailHandler.SendMail(mailDataModel);
            }
            return false;
        }


        private bool UpdateConfirmationCode(string hashedCode, DateTime dateTimeLastCode, string email)
        {
            string request = @" UPDATE dbo.Profile SET ConfirmationCode = @hashedCode, CreationDateTime = @dateTimeLastCode 
                WHERE Email = @email ";

            SqlCommand cmd = new SqlCommand(request, this.sqlConnection);
            cmd.Parameters.AddWithValue("hashedCode", hashedCode);

            // Format for 'u' (Universal): YYYY-MM-DD HH:MM:SSZ'
            cmd.Parameters.AddWithValue("dateTimeLastCode", dateTimeLastCode.ToString("u"));
            cmd.Parameters.AddWithValue("email", email);

            return WriteToDatabase(cmd);
        }
    }
}
