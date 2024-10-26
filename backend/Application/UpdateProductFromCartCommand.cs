using backend.Handlers;
using backend.Infrastructure;
using backend.Models;

namespace backend.Commands
{
    public class UpdateProductFromCartCommand
    {
        private readonly UpdateQuantityProductFromCartHandler _handler;

        public UpdateProductFromCartCommand(UpdateQuantityProductFromCartHandler handler)
        {
            _handler = handler;
        }

        public async Task<bool> Execute(UpdateCartProductModel cartProduct)
        {
            if (!await _handler.ProductExists(cartProduct.ProductID, cartProduct.IsPerishable))
            {
                return false;
            }

            if (!await _handler.ProductIsInCart(cartProduct.UserID, cartProduct.ProductID, cartProduct.IsPerishable))
            {
                return false;
            }

            return await _handler.HandleUpdateProductFromCart(cartProduct.UserID, cartProduct.ProductID, cartProduct.IsPerishable, cartProduct.CurrentCartQuantity);
        }


    }
}

