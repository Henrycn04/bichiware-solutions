using backend.Application;
using backend.Domain;

namespace backend.Infrastructure
{
    public class CustomerOrderBodyBuilder : OrdersBodyBuilder, IMailBodyBuilder
    {
        public MailBody createBody()
        {
            return new MailBody();
        }
    }
}
