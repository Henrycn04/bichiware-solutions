using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.Models;
using backend.Application;
using backend.Commands;
using backend.Queries;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RejectOrderController : Controller
    {
        private readonly RejectOrderCommand _rejectOrderCommand;
        private readonly RejectOrderEmailCommand _rejectOrderEmailCommand;
        private readonly OrderDetailsQuery _orderDetailsQuery;
        private OrderDetailsModel orderDetailsModel;

        public RejectOrderController(IMailService mailService)
        {
            this._rejectOrderCommand = new RejectOrderCommand();
            this._rejectOrderEmailCommand = new RejectOrderEmailCommand(mailService);
            this._orderDetailsQuery = new OrderDetailsQuery();
            this.orderDetailsModel = new OrderDetailsModel();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> RejectOrder([FromBody] OrdersModel orderModel)
        {
            int orderID = orderModel.OrderID;

            if (SendEmailToCustomer(orderID))
            {
                int rowsAffected = this._rejectOrderCommand.RejectOrder(orderID);
                if (rowsAffected > 0)
                {
                    return Ok();
                }
                else {
                    return UnprocessableEntity(new { message = $"Error rejecting order with ID = {orderID}." });
                }
            }
            else
            {
                // This returns error code 422, business rules are not followed
                return UnprocessableEntity(new { message = "Error sending notification of rejection email" });
            }
        }

        private bool SendEmailToCustomer(int orderID)
        {
            if (GetOrderDetails(orderID) != null)
            {
                return (this._rejectOrderEmailCommand.SendEmailToUser(this.orderDetailsModel));
            }
            else {
                return false;
            }
        }

        private string GetOrderDetails(int orderID)
        {
            this.orderDetailsModel = this._orderDetailsQuery.GetOrderDetails(orderID);
            return this.orderDetailsModel.CustomerEmail;
        }

    }
}
