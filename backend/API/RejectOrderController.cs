using Microsoft.AspNetCore.Mvc;
using backend.Queries;
using backend.Services;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RejectOrderController : Controller
    {
        private readonly RejectOrderQuery _rejectOrderQuery;

        public RejectOrderController()
        {
            this._rejectOrderQuery = new RejectOrderQuery();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> RejectOrder([FromBody] OrdersModel orderModel)
        {
            Console.WriteLine(orderModel.OrderID);
            int orderID = orderModel.OrderID;
            try
            {
                int rowsAffected = this._rejectOrderQuery.RejectOrder(orderID);

                if (rowsAffected > 0)
                {
                    return Ok();
                }
                else
                {
                    return NotFound(new { message = "This order does not exists." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error rejecting the order.", error = ex.Message });
            }
        }


    }
}
