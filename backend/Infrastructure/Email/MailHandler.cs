using backend.Application;
using backend.Domain;

namespace backend.Infrastructure
{
    public class MailHandler
    {
        IMailService mailService = null;


        public MailHandler(IMailService mailService)
        {
            this.mailService = mailService;
        }


        public bool SendMail(MailMessageModel mailData)
        {
            return mailService.SendMail(mailData);
        }
    }
}
