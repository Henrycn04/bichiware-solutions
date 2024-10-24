using backend.Application;
using backend.Commands;
using backend.Infrastructure;
using backend.Models;
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

        [HttpGet("getAllCartProducts/{userID}")]
        public async Task<IActionResult> GetAllCartProducts(int userID)
        {
            var getAllProductsCommand = new GetAllCartProductsQuery(_getAllHandler);

            var products = await getAllProductsCommand.Execute(userID);
            if (products == null || products.Count == 0)
            {
                return Ok("No products found in the cart for the specified user.");
            }

            return Ok(products);
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddProductToCart([FromBody] CartProductModel cartProduct)
        {
            var addProductCommand = new AddProductToCartCommand(_addHandler);

            if (!await addProductCommand.Execute(cartProduct))
                return Ok("Invalid product data or insufficient stock.");

            return Ok("Product added to cart successfully.");
        }


    }

}
