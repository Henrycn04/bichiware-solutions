using backend.Handlers;
using backend.Infrastructure;
using backend.Models;

namespace backend.Commands
{
    public class AddProductToCartCommand
    {
        private readonly AddProductToCartCommandHandler _handler;

        public AddProductToCartCommand(AddProductToCartCommandHandler handler)
        {
            _handler = handler;
        }

        public async Task<bool> Execute(CartProductModel cartProduct)
        {
            if (!await _handler.ProductExists(cartProduct.ProductID, cartProduct.IsPerishable))
            {
                return false;
            }
            if (!await _handler.UserHasShoppingCart(cartProduct.UserID))
            {
                return false;
            }
            if (!cartProduct.IsPerishable)
            {
                cartProduct.CurrentStock = await _handler.GetProductStock(cartProduct.ProductID);
                cartProduct.CurrentCartQuantity = await _handler.GetCurrentCartQuantity(cartProduct.UserID, cartProduct.ProductID);

                if (cartProduct.Quantity + cartProduct.CurrentCartQuantity > cartProduct.CurrentStock)
                {
                    return false;
                }
            }

            return await _handler.HandleAddProductToCart(cartProduct.UserID, cartProduct.ProductID, cartProduct.ProductName, cartProduct.Quantity, cartProduct.ProductPrice, cartProduct.IsPerishable);
        }

    }
}

