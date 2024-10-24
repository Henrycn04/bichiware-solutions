using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models;
using backend.Application;
using backend.Commands;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RejectOrderController : Controller
    {
        private readonly RejectOrderCommand _rejectOrderCommand;

        public RejectOrderController()
        {
            this._rejectOrderCommand = new RejectOrderCommand();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> RejectOrder([FromBody] OrdersModel orderModel)
        {
            int orderID = orderModel.OrderID;
            int rowsAffected = this._rejectOrderCommand.RejectOrder(orderID);
            if (rowsAffected > 0)
            {
                return Ok();
            }
            else
            {
                return NotFound(new { message = $"Error rejecting order with ID = {orderID}." });
            }
        }
    }
}
