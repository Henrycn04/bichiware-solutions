using backend.Domain;
using backend.Models;
using backend.Commands;
using Microsoft.AspNetCore.Mvc;
using backend.Application;

namespace backend.API
{
        [Route("api/addOrder")]
        [ApiController]
        public class AddOrderController : ControllerBase
        {
            private readonly AddOrderCommand _orderService;

            public AddOrderController(AddOrderCommand orderService)
            {
                _orderService = orderService;
            }

            [HttpPost("add")]
            public async Task<IActionResult> AddOrder([FromBody] AddOrderModel model)
            {
                var orderId = await _orderService.CreateOrder(model);
                return Ok(orderId);
            }

            [HttpPost("addperishable")]
            public async Task<IActionResult> AddPerishableProductsToOrder([FromBody] List<AddPerishableProductToOrderModel> products)
            {
                await _orderService.AddPerishableProducts(products);
                return Ok();
            }

            [HttpPost("addnonperishable")]
            public async Task<IActionResult> AddNonPerishableProductsToOrder([FromBody] List<AddNonPerishableProductToOrderModel> products)
            {
                await _orderService.AddNonPerishableProducts(products);
                return Ok();
            }

            [HttpPost("sendRealizationEmails")]
            public async Task<IActionResult> SendRealizationEmails([FromBody] OrderEmailModel order)
            {
                await _orderService.SendRealizationEmails(order);
                return Ok();
            }

            [HttpPost("calculateShippingCost")]
            public async Task<ActionResult<double>> CalculateShipping([FromBody] OrderEmailModel order)
            {
                double shipping = _orderService.CalculateShipping(order);
                return Ok(shipping);
            }
        }
}

