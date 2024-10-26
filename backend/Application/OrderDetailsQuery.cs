using backend.Application;
using backend.Domain;
using backend.Infrastructure;
using backend.Models;
using backend.Handlers;

namespace backend.Queries
{
    public class OrderDetailsQuery
    {
        private readonly OrderDetailsHandler _orderDetailsHandler;

        public OrderDetailsQuery()
        {
            this._orderDetailsHandler = new OrderDetailsHandler();
        }


        public OrderDetailsModel GetOrderDetails(int orderID)
        {
            OrderDetailsModel orderDetails = new OrderDetailsModel();
            if (CheckIfOrderHasProducts(orderID))
            {
                orderDetails = this._orderDetailsHandler.GetOrderDetails(orderID);
            }
            return orderDetails;
        }

        private bool CheckIfOrderHasProducts(int orderID)
        {
            return (this._orderDetailsHandler.CheckIfOrderHasProducts(orderID) > 0);
        }
    }
}
