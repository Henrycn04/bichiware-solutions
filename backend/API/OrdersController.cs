using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Queries;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : Controller
    {
        private OrdersCommand _ordersCommand;

        public OrdersController()
        {
            this._ordersCommand = new OrdersCommand();
        }

        [HttpGet]
        public List<OrdersModel> GetOrderData()
        {
            List<OrdersModel> orders = new List<OrdersModel>();
            orders = this._ordersCommand.getOrderData();
            return orders;
        }
    }
}
