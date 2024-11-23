using backend.Handlers;
using backend.Models;

namespace backend.Queries
{
    public class OrdersQuery
    {
        private readonly OrdersHandler _ordersHandler;

        public OrdersQuery()
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

        public List<int> getOrderYears()
        {
           return this._ordersHandler.getDistinctDeliveryYears();
   
        }

    }
}
