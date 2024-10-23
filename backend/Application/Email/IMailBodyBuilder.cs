using backend.Domain;

namespace backend.Application
{
    public interface IMailBodyBuilder
    {
        MailMessageModel createBody(MailMessageModel mail);
    }
}
