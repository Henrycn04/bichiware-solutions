using backend.Application;
using backend.Domain;
using backend.Infrastructure;
using backend.Models;
using backend.Handlers;

namespace backend.Commands
{
    public class RejectOrderEmailCommand
    {
        private readonly MailHandler _mailHandler;
        private MailMessageModel mailModel;

        public RejectOrderEmailCommand(IMailService mailService)
        {
            this._mailHandler = new MailHandler(mailService);
            this.mailModel = new MailMessageModel();
        }

        public bool SendEmailToUser(OrderDetailsModel orderDetailsModel)
        {
            CustomerOrderBodyBuilder customerOrderBodyBuilder = new CustomerOrderBodyBuilder();
            customerOrderBodyBuilder.SetState(OrderStates.Rejected);
            List<OrderProductModel> products = orderDetailsModel.OrderProducts;
            customerOrderBodyBuilder.SetOrderDetails(products, orderDetailsModel.Taxes, orderDetailsModel.ShippingCost);
            this._mailHandler.SetBodyBuilder(customerOrderBodyBuilder);
            this.mailModel.ReceiverMailAddress = orderDetailsModel.CustomerEmail;
            this.mailModel.ReceiverMailName = orderDetailsModel.CustomerName;
            bool success = this._mailHandler.SendMail(this.mailModel);
            return success;
        }
    }
}
