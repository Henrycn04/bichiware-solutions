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
    public class ProductController : ControllerBase
    {
        private readonly ProductHandler _productHandler;

        public ProductController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("BichiwareSolutionsContext");
            _productHandler = new ProductHandler(connectionString);
        }

        [HttpPost("addperishableproduct")]
        public async Task<ActionResult<int>> addPerishableProduct([FromBody] PerishableModel productData)
        {
            if (productData == null)
            {
                return BadRequest("Invalid product data.");
            }
            try
            {
                int productID = await _productHandler.addPerishableProduct(productData);
                if (productID > 0)
                {
                    return Ok(productID);
                }
                return BadRequest("Failed to create perishable product.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("addnonperishableproduct")]
        public async Task<ActionResult<bool>> addNonPerishableproduct([FromBody] NonPerishableModel productData)
        {
            if (productData == null)
            {
                return BadRequest("Invalid product data.");
            }
            try
            {
                bool response = await _productHandler.addNonPerishableProduct(productData);
                if (response)
                {
                    return Ok(response);
                }
                return BadRequest("Failed to create non perishable product.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("adddelivery")]
        public async Task<ActionResult<bool>> addDelivery([FromBody] DeliveryModel productData)
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
        public async Task<ActionResult<bool>> SearchDelivery([FromBody] DeliveryModel productData)
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

