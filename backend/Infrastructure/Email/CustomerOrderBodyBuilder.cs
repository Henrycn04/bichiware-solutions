using backend.Application;
using backend.Domain;
using HtmlAgilityPack;

namespace backend.Infrastructure
{
    public enum OrderStates
    {
        Pending,
        Confirmed,
        Rejected
    };

    public class CustomerOrderBodyBuilder : OrdersBodyBuilder, IMailBodyBuilder
    {
        private readonly Dictionary<OrderStates, string> states = new Dictionary<OrderStates, string>()
        {
            { OrderStates.Pending,      "CustomerOrderPending"  },
            { OrderStates.Confirmed,    "CustomerOrderApproved" },
            { OrderStates.Rejected,     "CustomerOrderRejected" }
        };

        public MailMessageModel createBody(MailMessageModel mail)
        {
            try
            {
                if (mail.EmailBody == null)
                {
                    mail.EmailBody = new MailBody();
                }

                HtmlDocument document = this.LoadHtml();
                MailBodyMessagesModel message = this.InjectBodyMessage(document, mail);
                mail.EmailSubject = message.Subject;

                if (this.typeOfMessage.Length <= 0)
                {
                    throw new Exception("State of the order was not specified. Aborting sending email.");
                }

                // Inject more values after this line
                document = this.InjectOrderReceipt(document);
                
                mail.EmailBody.Html = document;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return mail;
        }

        public void SetState(OrderStates state)
        {
            this.typeOfMessage = states[state];
        }
    }
}
