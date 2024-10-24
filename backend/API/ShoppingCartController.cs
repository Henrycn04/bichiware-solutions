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
        private readonly DeleteProductFromCartHandler _deleteHandler;

        public ShoppingCartController()
        {
            _addHandler = new AddProductToCartCommandHandler();
            _getAllHandler = new GetAllCartProductsHandler();
            _deleteHandler = new DeleteProductFromCartHandler();
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
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteProductFromCart([FromBody] DeleteCartProductModel cartProduct)
        {
            var deleteProductCommand = new DeleteProductFromCartCommand(_deleteHandler);

            if (!await deleteProductCommand.Execute(cartProduct))
                return Ok("Invalid product data or insufficient stock.");

            return Ok("Product added to cart successfully.");
        }


    }

}
