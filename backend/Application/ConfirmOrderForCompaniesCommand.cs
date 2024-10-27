using backend.Application;
using backend.Domain;
using backend.Infrastructure;
using backend.Models;
using backend.Handlers;

namespace backend.Commands
{
    public class ConfirmOrderForCompaniesCommand
    {
        private readonly MailHandler _mailHandler;
        private MailMessageModel mailModel;

        public ConfirmOrderForCompaniesCommand(IMailService mailService)
        {
            this._mailHandler = new MailHandler(mailService);
            this.mailModel = new MailMessageModel();
        }

        public bool SendEmailToCompany(List<ConfirmOrderForCompaniesModel> companiesData)
        {
            bool success = false;
            BusinessOrderBodyBuilder customerOrderBodyBuilder = new BusinessOrderBodyBuilder();
            List<OrderProductModel> products = new List<OrderProductModel>();
            for (int i = 0; i < companiesData.Count; i++)
            {
                customerOrderBodyBuilder = new BusinessOrderBodyBuilder();
                products = companiesData[i].OrderProducts;
                customerOrderBodyBuilder.SetOrderDetails(products, companiesData[i].Taxes, companiesData[i].ShippingCost);
                this._mailHandler.SetBodyBuilder(customerOrderBodyBuilder);
                this.mailModel.ReceiverMailAddress = companiesData[i].CompanyEmail;
                this.mailModel.ReceiverMailName = companiesData[i].CompanyName;
                success = this._mailHandler.SendMail(this.mailModel);
            }
            return success;
        }
    }
}
