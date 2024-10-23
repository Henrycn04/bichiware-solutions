using backend.Commands;
using backend.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly AddProductToCartCommandHandler _addHandler;
        private readonly GetAllCartProductsHandler _getAllHandler;

        public ShoppingCartController()
        {
            _addHandler = new AddProductToCartCommandHandler();
            _getAllHandler = new GetAllCartProductsHandler();
        }

        [HttpGet("{userID}")]
        public async Task<IActionResult> GetAllCartProducts(int userID)
        {
            try
            {
                var products = await _getAllHandler.GetAllCartProducts(userID);
                if (products == null || products.Count == 0)
                {
                    return Ok("No products found in the cart for the specified user.");
                }
                return Ok(products);
            }
            catch (Exception ex)
            {
                return Ok($"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProductToCart([FromBody] AddProductToCartCommand command)
        {
            bool stockSet = await _addHandler.SetStockAndCartQuantity(command);
            if (!stockSet)
                return Ok("Product not found or stock information unavailable.");

            if (!command.IsValid())
                return Ok("Invalid command data.");

            bool result = await _addHandler.Handle(command);
            if (!result)
                return Ok("Could not add product to cart.");

            return Ok("Product added to cart successfully.");
        }
    }

}
