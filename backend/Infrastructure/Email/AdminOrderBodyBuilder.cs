using backend.Application;
using backend.Domain;
using HtmlAgilityPack;

namespace backend.Infrastructure
{
    public class AdminOrderBodyBuilder : OrdersBodyBuilder, IMailBodyBuilder
    {
        private string customerName;
        private string customerAddress;


        public AdminOrderBodyBuilder()
        {
            this.typeOfMessage = "AdminOrderRequest";
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
                this.InjectCustomerDetails(document);
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


        public void SetCustomerDetails(string customerName, string customerAddress)
        {
            this.customerName = customerName;
            this.customerAddress = customerAddress;
        }


        private void InjectCustomerDetails(HtmlDocument document)
        {
            if (this.customerName.Length <= 0 && this.customerAddress.Length <= 0)
            {
                throw new Exception("Customer details was not provided. Aborting sending email");
            }

            string customerDetails = "<span style=\"font-weight: bold; \">Nombre: </span>" + $"<span> {this.customerName}</span><br />\n" + 
                                     "<span style=\"font-weight: bold; \">Dirección: </span>" + $" <span>{this.customerAddress}</span><br />\n";

            HtmlDocument x = new HtmlDocument();
            x.LoadHtml(customerDetails);

            HtmlNode customerDetailsNode = document.GetElementbyId("CustomerDetails");
            customerDetailsNode.AppendChildren(x.DocumentNode.ChildNodes);
        }
    }
}
