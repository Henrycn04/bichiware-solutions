using backend.Handlers;
using backend.Models;

namespace backend.Commands
{
    public class OrdersCommand
    {
        private readonly OrdersHandler _ordersHandler;

        public OrdersCommand()
        {
            _ordersHandler = new OrdersHandler();
        }

        public List<OrdersModel> getOrderData()
        {
            int amountOfOrders = this._ordersHandler.getAmountOfOrders();
            if (amountOfOrders > 0)
            {
                List<OrdersModel> orders = new List<OrdersModel>();
                orders = this._ordersHandler.getOrdersData();
                return orders;
            } else
            {
                return new List<OrdersModel>();
            }
        }

    }
}
