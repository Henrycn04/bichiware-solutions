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
                if (bodyBuilder != null)
                {
                    throw new Exception("No body builder set for email");
                }
                MailboxAddress destination = new MailboxAddress(
                    mailData.ReceiverMailName,
                    mailData.ReceiverMailAddress
                );
                MailboxAddress origin = new MailboxAddress(
                    mailConfiguration.SenderMailName,
                    mailConfiguration.SenderMailAddress
                );

                BodyBuilderRequest(mailData);
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
            // emailBodyBuilder.TextBody = mailData.EmailBody;
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


        private void BodyBuilderRequest(MailMessageModel mailData)
        {
            mailData.EmailBody = bodyBuilder.createBody();
        }


        public void SetBodyBuilder(IMailBodyBuilder builder)
        {
            bodyBuilder = builder;
        }
    }
}
