using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using backend.Configuration;
using backend.Models;
using MailKit.Security;

namespace backend.Services
{
    public class MailService : IMailService
    {
        MailConfiguration mailConfiguration = null;

        public MailService(IOptions<MailConfiguration> options)
        {
            mailConfiguration = options.Value;
        }

        public bool SendMail(MailData mailData)
        {
            try
            {
                MailboxAddress origin = new MailboxAddress(
                    mailConfiguration.OriginEmailName,
                    mailConfiguration.OriginEmailAddress
                );
                MailboxAddress destination = new MailboxAddress(
                    mailData.DestinationEmailName,
                    mailData.DestinationEmailAddress
                );
                MimeMessage emailMessage = this.FormatMessage(origin, destination, mailData);
                this.SmtpClientRequest(emailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public MimeMessage FormatMessage(MailboxAddress origin, MailboxAddress destination, MailData mailData)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(origin);
            emailMessage.To.Add(destination);
            emailMessage.Subject = mailData.EmailSubject;

            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.TextBody = mailData.EmailBody;
            emailMessage.Body = emailBodyBuilder.ToMessageBody();

            return emailMessage;
        }

        public void SmtpClientRequest(MimeMessage emailMessage)
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
    }
}
