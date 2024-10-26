using backend.Domain;
using backend.Infrastructure;

namespace backend.Application
{
    public class OrderConfirmationCommand
    {
        private readonly MailHandler mailHandler;
        private MailMessageModel message;
        public OrderConfirmationCommand(IMailService mailService)
        {
            this.mailHandler = new MailHandler(mailService);
            this.message = new MailMessageModel();
        }

        public bool makeConfirmationEmail(UserDataModel user, 
                                List<OrderProductModel> products,
                                OrderConfirmationModel order)
        {
            double finalTax = double.Parse(order.tax.ToString());
            double finalDelivery = double.Parse(order.delivery.ToString());

            CustomerOrderBodyBuilder builder = new CustomerOrderBodyBuilder();
            builder.SetState(OrderStates.Confirmed);
            builder.SetOrderDetails(products, finalTax, finalDelivery);
            
            this.message.ReceiverMailAddress = user.emailAddress;
            this.message.ReceiverMailName = user.name;

            this.mailHandler.SetBodyBuilder(builder);
            return this.mailHandler.SendMail(this.message);
        }
    }
}
