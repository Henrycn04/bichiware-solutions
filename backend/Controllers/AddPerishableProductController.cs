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
    public class AddPerishableProductController : ControllerBase
    {
        private readonly AddPerishableProductHandler _productHandler;

        public AddPerishableProductController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("BichiwareSolutionsContext");
            _productHandler = new AddPerishableProductHandler(connectionString);
        }

        [HttpPost("addperishableproduct")]
        public async Task<ActionResult<int>> addPerishableProduct([FromBody] AddPerishableProductModel productData)
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

    }
}

