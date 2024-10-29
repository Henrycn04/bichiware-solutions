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
        private readonly UpdateQuantityProductFromCartHandler _updateHandler;

        public ShoppingCartController()
        {
            _addHandler = new AddProductToCartCommandHandler();
            _getAllHandler = new GetAllCartProductsHandler();
            _deleteHandler = new DeleteProductFromCartHandler();
            _updateHandler = new UpdateQuantityProductFromCartHandler();
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

            return Ok("Product deleted to cart successfully.");
        }
        [HttpPost("deleteall")]
        public async Task<IActionResult> DeleteAllProductFromCart([FromBody] DeleteCartProductModel cartProduct)
        {
            var deleteProductCommand = new DeleteProductFromCartCommand(_deleteHandler);
           
            if (!await deleteProductCommand.ExecuteAllProductsDelete(cartProduct))
                return Ok("Invalid user.");
            return Ok("Products deleted to cart successfully.");
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateProductFromCart([FromBody] UpdateCartProductModel cartProduct)
        {
            var updateProductCommand = new UpdateProductFromCartCommand(_updateHandler);

            if (!await updateProductCommand.Execute(cartProduct))
                return Ok("Invalid product data or insufficient stock.");

            return Ok("Product added to cart successfully.");
        }


    }

}
