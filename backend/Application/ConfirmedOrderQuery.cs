using backend.Domain;
using backend.Models;
using backend.Infrastructure;

namespace backend.Application
{
    public class ConfirmedOrderQuery
    {
        private readonly ConfirmedOrderHandler handler;
        public ConfirmedOrderQuery() 
        {
            this.handler = new ConfirmedOrderHandler();
        }

        public OrderConfirmationModel GetOrderData(int orderID)
        {
            return this.handler.GetOrder(orderID);     
        }
    }
}
