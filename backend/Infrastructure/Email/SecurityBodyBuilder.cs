using backend.Application;
using backend.Domain;

namespace backend.Infrastructure
{
    public enum SecurityEmailReasons {
        PasswordChange = 0,
        Verification
    }

    public class SecurityBodyBuilder : IMailBodyBuilder
    {
        private string securityCode { get; set; }
        private SecurityEmailReasons reason { get; set; }

        private string pathToConfiguration = @"Configuration/Email/Security";

        /*private const string[] emailSecurityFiles = { };*/

        public MailBody createBody()
        {
            return new MailBody();
        }


        private void GetEmailMessage()
        {
            
        }
    }
}
