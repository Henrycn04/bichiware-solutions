using backend.Services;
using backend.Models;

namespace backend.Handlers
{
    public class MailHandler
    {
        IMailService mailService = null;


        public MailHandler(IMailService mailService)
        {
            this.mailService = mailService;
        }


        public bool SendMail(MailDataModel mailData)
        {
            return this.mailService.SendMail(mailData);
        }
    }
}
