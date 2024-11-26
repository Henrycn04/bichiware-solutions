using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using backend.Domain;
using MailKit.Security;

namespace backend.Application
{
    public class MailService : IMailService
    {
        MailConfiguration mailConfiguration = null;
        IMailBodyBuilder bodyBuilder = null;

        public MailService(IOptions<MailConfiguration> options)
        {
            mailConfiguration = options.Value;
        }

        public bool SendMail(MailMessageModel mailData)
        {
            try
            {
                this.ValidateMailMessage(mailData);

                MailboxAddress destination = new MailboxAddress(
                    mailData.ReceiverMailName,
                    mailData.ReceiverMailAddress
                );
                MailboxAddress origin = new MailboxAddress(
                    mailConfiguration.SenderMailName,
                    mailConfiguration.SenderMailAddress
                );

                mailData = bodyBuilder.createBody(mailData);
                MimeMessage emailMessage = FormatMessage(origin, destination, mailData);
                SmtpClientRequest(emailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private MimeMessage FormatMessage(MailboxAddress origin, MailboxAddress destination, MailMessageModel mailData)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(origin);
            emailMessage.To.Add(destination);
            emailMessage.Subject = mailData.EmailSubject;

            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.HtmlBody = mailData.EmailBody.ToString();
            emailMessage.Body = emailBodyBuilder.ToMessageBody();

            return emailMessage;
        }

        private void SmtpClientRequest(MimeMessage emailMessage)
        {
            // This smtp client is from the Mailkit.Net.Smtp namespace, not System.Net.Mail
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Connect(mailConfiguration.Host, mailConfiguration.Port, SecureSocketOptions.StartTls);
            smtpClient.Authenticate(mailConfiguration.Username, mailConfiguration.Password);

            string serverResponse = smtpClient.Send(emailMessage);
            Console.WriteLine("SMTP server response: \n" + serverResponse);

            smtpClient.Disconnect(true);
            smtpClient.Dispose();
        }


        public void SetBodyBuilder(IMailBodyBuilder builder)
        {
            bodyBuilder = builder;
        }

        public bool ValidateAllowedDomains(MailMessageModel mail)
        {
            List<string> allowedDomains = new List<string>()
            {
                "@gmail",
                "@icloud",
                "@yahoo",
                "@outlook"
            };

            bool isAllowed = false;
            foreach (string domain in allowedDomains)
            {
                isAllowed = mail.ReceiverMailAddress.Contains(domain);
                if (isAllowed)
                {
                    break;
                }
            }
            
            if (!isAllowed)
            {
                throw new ArgumentException("The domain provided for the email address is not allowed");
            }
            return true;
        }

        public bool ValidateMailMessage(MailMessageModel mail)
        {
            return this.ValidateNotNullMailMessage(mail)
                && this.ValidateAllowedDomains(mail);
        }
        
        public bool ValidateNotNullMailMessage(MailMessageModel mail)
        {
            if (mail == null)
            {
                throw new NullReferenceException("The mail message is null");
            }
            return true;
        }
    }
}
