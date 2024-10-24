using backend.Handlers;
using backend.Infrastructure;
using backend.Models;

namespace backend.Commands
{
    public class DeleteProductFromCartCommand
    {
        private readonly DeleteProductFromCartHandler _handler;

        public DeleteProductFromCartCommand(DeleteProductFromCartHandler handler)
        {
            _handler = handler;
        }

        public async Task<bool> Execute(DeleteCartProductModel cartProduct)
        {
            if (!await _handler.ProductExists(cartProduct.ProductID, cartProduct.IsPerishable))
            {
                return false;
            }

            if (!await _handler.ProductIsInCart(cartProduct.UserID, cartProduct.ProductID, cartProduct.IsPerishable))
            {
                return false;
            }

            return await _handler.HandleDeleteProductFromCart(cartProduct.UserID, cartProduct.ProductID, cartProduct.IsPerishable);
        }


    }
}

