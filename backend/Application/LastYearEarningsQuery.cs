using backend.Domain;
using backend.Infrastructure;

namespace backend.Application
{
    public class LastYearEarningsQuery
    {
        private readonly ILastYearEarningsHandler _handler;

        public LastYearEarningsQuery(ILastYearEarningsHandler handler)
        {
            this._handler = handler;
        }

        public async Task<List<LastYearOrdersModel>> Execute(LastYearEarningsElementsModel filter)
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
