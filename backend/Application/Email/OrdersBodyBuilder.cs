using backend.Domain;

namespace backend.Application
{
    public class OrdersBodyBuilder
    {
        private List<OrderProductModel> products = null;
        private decimal taxInColones;
        private decimal shippingFee;

        public void SetOrderDetails(List<OrderProductModel> products, decimal taxInColones, decimal shippingFee)
        {
            products = products;
            taxInColones = taxInColones;
            shippingFee = shippingFee;
        }
    }
}
