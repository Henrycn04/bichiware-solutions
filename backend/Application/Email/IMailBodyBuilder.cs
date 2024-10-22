using backend.Domain;

namespace backend.Application
{
    public interface IMailBodyBuilder
    {
        MailBody createBody();
    }
}
