using backend.Handlers;
using backend.Infrastructure;
using backend.Models;

namespace backend.Commands
{
    public class GetAllCartProductsQuery
    {
        private readonly GetAllCartProductsHandler _handler;

        public GetAllCartProductsQuery(GetAllCartProductsHandler handler)
        {
            _handler = handler;
        }

        public async Task<List<CartProductModel>> Execute(int userId)
        {
            if (!await _handler.UserExists(userId))
            {
                return null;
            }

            var products = await _handler.GetAllCartProducts(userId);

            if (products == null || products.Any(p =>
                p.ProductID <= 0 ||
                string.IsNullOrEmpty(p.ProductName) ||
                p.Quantity < 0 ||
                p.ProductPrice < 0))
            {
                return null;
            }

            return products;
        }
    }
}
