using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Queries;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private OrdersQuery _ordersQuery;

        public OrdersController()
        {
            this._ordersQuery = new OrdersQuery();
        }

        [HttpGet]
        public List<OrdersModel> GetOrderData()
        {
            List<OrdersModel> orders = new List<OrdersModel>();
            orders = this._ordersQuery.getOrderData();
            return orders;
        }

        [HttpGet("years")]
        public List<int> GetOrderYears()
        {
            List<int> orders = this._ordersQuery.getOrderYears();
            return orders;
        }
    }
}
