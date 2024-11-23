using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using backend.Models;

namespace backend.Commands
{
    public class GetOrdersQuery
    {
        private readonly GetOrdersHandler _handler;

        public GetOrdersQuery()
        {
            _handler = new GetOrdersHandler();
        }
        public GetOrdersQuery(GetOrdersHandler handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public async Task<List<UserOrdersModel>> Execute(int userId)
        {
            if (!await _handler.UserExists(userId))
            {
                throw new ArgumentException("UserID doesnt exist.");
            }

            if (!await _handler.UserExists(userId) )
            {
                throw new ArgumentException("UserID doesnt exist.");
            }

            var products = await _handler.GetAllOrdersWithProducts(userId);

            return products;
        }
    }
}
