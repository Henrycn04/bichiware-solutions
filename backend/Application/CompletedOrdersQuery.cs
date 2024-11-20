using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using backend.Models;

namespace backend.Commands
{
    public class CompletedOrdersQuery
    {
        private readonly CompletedOrdersReportHandler _handler;

        public CompletedOrdersQuery()
        {
            _handler = new CompletedOrdersReportHandler();
        }
        public CompletedOrdersQuery(CompletedOrdersReportHandler handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public async Task<List<CompletedOrdersModel>> Execute(FiltersCompletedOrdersModel filter)
        {
            if (!await _handler.UserExists(filter.UserID))
            {
                throw new ArgumentException("UserID does not exist.");
            }

            int userType = await _handler.UserIsAdminOrEntrepeneur(filter.UserID);
            if (userType != 2 && userType != 3) 
            {
                throw new UnauthorizedAccessException("User is not authorized to view orders.");
            }

            if (userType == 2 && filter.CompanyID.HasValue && !await _handler.CompanyExists(filter.CompanyID.Value))
            {
                throw new ArgumentException("CompanyID does not exist.");
            }

            var orders = await _handler.GetOrdersByFilterAsync(filter);

            return orders;
        }
    }
}
