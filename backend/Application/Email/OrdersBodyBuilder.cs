using backend.Domain;

namespace backend.Application
{
    public class OrdersBodyBuilder
    {
        protected MailBody mailBody { get; set; }
        protected List<OrderProductModel> products = null;
        protected decimal taxInColones;
        protected decimal shippingFee;

        protected string pathToConfiguration = @"Configuration/Email/Orders";
        protected readonly string[] emailSecurityFiles =
        {
            "OrderMessages.json",
            "Orders.cshtml"
        };

        public void SetOrderDetails(List<OrderProductModel> products, decimal taxInColones, decimal shippingFee)
        {
            products = products;
            taxInColones = taxInColones;
            shippingFee = shippingFee;
        }
    }
}
