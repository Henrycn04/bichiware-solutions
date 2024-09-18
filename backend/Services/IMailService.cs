using backend.Models;

namespace backend.Services
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
