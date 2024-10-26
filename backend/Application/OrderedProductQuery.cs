using backend.Domain;
using backend.Infrastructure;

namespace backend.Application
{
    public class OrderedProductQuery
    {
        private readonly OrderedProductHandler handler;
        public OrderedProductQuery()
        {
            this.handler = new OrderedProductHandler();    
        }

        public List<OrderProductModel> GetOrderedProductList(int orderID)
        {
            return this.handler.GetProducts(orderID);
        }
    }
}
