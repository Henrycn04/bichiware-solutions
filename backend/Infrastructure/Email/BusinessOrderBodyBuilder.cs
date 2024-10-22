using backend.Application;
using backend.Domain;

namespace backend.Infrastructure
{
    public class BusinessOrderBodyBuilder : OrdersBodyBuilder, IMailBodyBuilder
    {
        public MailMessageModel createBody(MailMessageModel mail)
        {
            return mail;
        }
    }
}
