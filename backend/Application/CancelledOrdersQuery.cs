using backend.Handlers;
using backend.Models;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Reflection.Metadata;

namespace backend.Queries
{
    public class CancelledOrdersQuery
    {
        private readonly ICancelledOrdersHandler _cancelledOrdersHandler;

        public CancelledOrdersQuery(ICancelledOrdersHandler cancelledOrdersHandler)
        {
            this._cancelledOrdersHandler = cancelledOrdersHandler;
        }

        public List<CancelledOrdersModel> GetCancelledOrders(FiltersCompletedOrdersModel filter)
        {

            this.CheckIfUserExists(filter.UserID);
            this.CheckIfUserIsAdminOrEntrepreneur(filter.UserID);
            this.CheckIfCompanyExists(filter.CompanyID);
            return this._cancelledOrdersHandler.GetCancelledOrders(filter);
        }

        private void CheckIfUserExists(int userID)
        {
            if (_cancelledOrdersHandler.CheckIfUserExists(userID) <= 0)
            {
                throw new ArgumentException("UserID does not exist.");
            }
        }

        private void CheckIfCompanyExists(int? companyID)
        {
            if (companyID.HasValue)
            {
                int companyIDValue = companyID.Value;
                if (_cancelledOrdersHandler.CheckIfCompanyExists(companyIDValue) <= 0)
                {
                    throw new ArgumentException("CompanyID does not exist.");
                }
            }
        }

        private void CheckIfUserIsAdminOrEntrepreneur(int userID)
        {
            int userType = _cancelledOrdersHandler.CheckIfUserIsAdminOrEntrepeneur(userID);
            if (userType == 1)
            {
                throw new UnauthorizedAccessException("User is not authorized to view orders.");

            }
        }
    }
}
