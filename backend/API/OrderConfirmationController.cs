using backend.Application;
using backend.Domain;
using Microsoft.AspNetCore.Mvc;
using backend.Handlers;
using backend.Models;
using backend.Commands;
using backend.Queries;
using Org.BouncyCastle.Security;

namespace backend.Controllers
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
        private readonly ConfirmOrderForCompaniesCommand _confirmOrderForCompaniesCommand;
        private readonly ConfirmOrdersForCompaniesQuery _confirmOrdersForCompaniesQuery;

        public OrderConfirmationController (IMailService mailService)
        {
            this.userDataGetter = new UserDataQuery();
            this.confirmedOrderGetter = new ConfirmedOrderQuery();
            this.orderedProductGetter = new OrderedProductQuery();
            this.orderConfirmationMaker = new OrderConfirmationCommand(mailService);
            this.confirmer = new ConfirmOrderCommand();
            this._confirmOrderForCompaniesCommand = new ConfirmOrderForCompaniesCommand(mailService);
            this._confirmOrdersForCompaniesQuery = new ConfirmOrdersForCompaniesQuery();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> ConfirmOrder([FromBody] OrdersModel orderModel) 
        {
            int OrderID = orderModel.OrderID; 
            try
            {
                this.UpdateOrder(OrderID);
                //OrderConfirmationModel currentOrder = this.GetOrderInfo(OrderID);
                //UserDataModel CurrentUser = this.GetUserData(currentOrder.UserID);
                //List<OrderProductModel> orderedProducts = this.GetOrderedProducts(OrderID);
               // this.ConfirmUser(CurrentUser, orderedProducts, currentOrder);
                this.ConfirmCompanies(OrderID);
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

        private void ConfirmCompanies(int OrderID)
        {
            Console.WriteLine($"ID: {OrderID}");
            List<ConfirmOrderForCompaniesModel> companiesData = new List<ConfirmOrderForCompaniesModel>();
            companiesData = this._confirmOrdersForCompaniesQuery.GetDataForEmails(OrderID);
            if (companiesData.Count == 0) {
                throw new Exception("Error getting companies data for the emails");
            }
            if (!SendEmailToCompanies(companiesData))
            {
                throw new Exception("Error while sending companies confirmation email");
            }
        }

        private bool SendEmailToCompanies(List<ConfirmOrderForCompaniesModel> companiesData)
        {
            bool success = this._confirmOrderForCompaniesCommand.SendEmailToCompany(companiesData);
            return success;
        }
    }
}
