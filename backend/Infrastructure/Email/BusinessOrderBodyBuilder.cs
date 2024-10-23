using backend.Application;
using backend.Domain;
using HtmlAgilityPack;

namespace backend.Infrastructure
{
    public class BusinessOrderBodyBuilder : OrdersBodyBuilder, IMailBodyBuilder
    {
        public BusinessOrderBodyBuilder()
        {
            this.typeOfMessage = "BusinessOrderNotification";
        }


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

                // Inject more values after this line
                this.InjectOrderReceipt(document);

                mail.EmailSubject = message.Subject;
                mail.EmailBody.Html = document;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return mail;
        }
    }
}
