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
    public class AddNonPerishableProductController : ControllerBase
    {
        private readonly AddNonPerishableProductHandler _productHandler;

        public AddNonPerishableProductController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("BichiwareSolutionsContext");
            _productHandler = new AddNonPerishableProductHandler(connectionString);
        }


        [HttpPost("addnonperishableproduct")]
        public async Task<ActionResult<bool>> addNonPerishableproduct([FromBody] AddNonPerishableProductModel productData)
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

    }
}

