using backend.Domain;

namespace backend.Application
{
    public interface IMailService
    {
        bool SendMail(MailMessageModel mailData);
    }
}
