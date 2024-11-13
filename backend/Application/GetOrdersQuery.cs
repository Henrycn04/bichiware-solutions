using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using backend.Models;

namespace backend.Commands
{
    public class GetOrdersQuery
    {
        private readonly GetOrdersHandler _handler;

        public GetOrdersQuery(GetOrdersHandler handler)
        {
            _handler = handler;
        }

        public async Task<List<UserOrdersModel>> Execute(int userId)
        {
            if (!await _handler.UserExists(userId))
            {
                return null;
            }

            var products = await _handler.GetAllOrdersWithProducts(userId);

            return products;
        }
    }
}
