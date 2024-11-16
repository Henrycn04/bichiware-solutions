using backend.Application;
using backend.Commands;
using backend.Infrastructure;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientDashboardController : ControllerBase
    {
        private readonly GetOrdersHandler _getAllHandler;

        public ClientDashboardController()
        {
            _getAllHandler = new GetOrdersHandler();
        }

        [HttpGet("getOrdersInProgress/{userID}")]
        public async Task<IActionResult> GetOrdersInProgress(int userID)
        {
            var getAllOrders = new GetOrdersQuery(_getAllHandler);

            var orders = await getAllOrders.Execute(userID);
            if (orders == null || orders.Count == 0)
            {
                return Ok("No orders in progress found for the specified user.");
            }

            return Ok(orders);
        }

    }
}
