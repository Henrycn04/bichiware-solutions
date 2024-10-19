using backend.Handlers;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddDeliveryController : ControllerBase
    {
        private readonly AddDeliveryHandler _productHandler;

        public AddDeliveryController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("BichiwareSolutionsContext");
            _productHandler = new AddDeliveryHandler(connectionString);
        }

        [HttpPost("adddelivery")]
        public async Task<ActionResult<bool>> addDelivery([FromBody] AddDeliveryModel productData)
        {
            if (productData == null)
            {
                return BadRequest("Invalid product data.");
            }
            try
            {
                bool response = await _productHandler.addDelivery(productData);
                if (response)
                {
                    return Ok(response);
                }
                return BadRequest("Failed to create delivery.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }


        [HttpPost("searchdelivery")]
        public async Task<ActionResult<bool>> SearchDelivery([FromBody] AddDeliveryModel productData)
        {   // get true if there is at least one user with the data specified
            bool deliveryExists = await _productHandler.SearchDelivery(productData);

            if (deliveryExists)
            {
                    return Ok(new { success = true });
            }

            return Ok(new { success = false });
        }

    }
}

