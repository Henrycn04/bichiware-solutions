using backend.Application;
using backend.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.Design;
using backend.Handlers;
using backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderConfirmationController : ControllerBase
    {

        private readonly UserDataQuery userDataGetter;
        private readonly ConfirmedOrderQuery confirmedOrderGetter;
        private readonly OrderedProductQuery orderedProductGetter;
        private readonly OrderConfirmationCommand orderConfirmationMaker;
        private readonly ConfirmOrderCommand confirmer;
        public OrderConfirmationController (IMailService mailService)
        {
            this.userDataGetter = new UserDataQuery();
            this.confirmedOrderGetter = new ConfirmedOrderQuery();
            this.orderedProductGetter = new OrderedProductQuery();
            this.orderConfirmationMaker = new OrderConfirmationCommand(mailService);
            this.confirmer = new ConfirmOrderCommand();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> ConfirmOrder(int OrderID) 
        {
            try
            {
                this.UpdateOrder(OrderID);
                OrderConfirmationModel currentOrder = this.GetOrderInfo(OrderID);
                UserDataModel CurrentUser = this.GetUserData(currentOrder.UserID);
                List<OrderProductModel> orderedProducts = this.GetOrderedProducts(OrderID);
                this.ConfirmUser(CurrentUser, orderedProducts, currentOrder);
                this.ConfirmCompanies();
                return Ok("Order confirmed correctly");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error confirming order: {ex.Message}");
            }
        }

        private void UpdateOrder(int orderID)
        {
            this.confirmer.UpdateOrder(orderID);
        }

        private OrderConfirmationModel GetOrderInfo(int orderID)
        {
            return this.confirmedOrderGetter.GetOrderData(orderID);
        }

        private UserDataModel GetUserData(int userID)
        {
            UserDataModel result = this.userDataGetter.getData(userID);
            return result;
        }

        private List<OrderProductModel> GetOrderedProducts(int orderID)
        {
            return this.orderedProductGetter.GetOrderedProductList(orderID);
        }

        private void ConfirmUser(UserDataModel user, List<OrderProductModel> products,
                                OrderConfirmationModel order) 
        {
            if (!this.orderConfirmationMaker.makeConfirmationEmail(user, products, order))
                throw new Exception("Error while sending user confirmation email");
        }

        private void ConfirmCompanies()
        {
        }


    }
}
