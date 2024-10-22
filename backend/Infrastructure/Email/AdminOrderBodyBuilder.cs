using backend.Application;
using backend.Domain;

namespace backend.Infrastructure
{
    public class AdminOrderBodyBuilder : OrdersBodyBuilder, IMailBodyBuilder
    {
        public MailMessageModel createBody(MailMessageModel mail)
        {
            return mail;
        }
    }
}
