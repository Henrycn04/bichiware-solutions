using backend.Handlers;
using backend.Models;

namespace backend.Queries
{
    public class Admin_EntrepreneurDashboardQuery
    {
        private readonly IAdmin_EntrepreneurOrdersHandler _admin_EntrepreneurOrdersHandler;

        public Admin_EntrepreneurDashboardQuery(IAdmin_EntrepreneurOrdersHandler admin_EntrepreneurOrdersHandler)
        {
            this._admin_EntrepreneurOrdersHandler = admin_EntrepreneurOrdersHandler;
        }

        public List<UserOrdersModel> GetOrdersForAdmin()
        {
            var orders = this._admin_EntrepreneurOrdersHandler.GetOrdersForAdmin();
            return orders;
        }

        public  List<EntrepreneurOrdersModel> GetOrdersForEntrepreneur(int userID)
        {
            List<EntrepreneurOrdersModel> orders = new List<EntrepreneurOrdersModel>();
            if (CheckIfUserHasCompanies(userID) && checkValidityOfNumber(userID) && CheckIfUserExists(userID))
            {
                orders = this._admin_EntrepreneurOrdersHandler.GetOrdersForEntrepreneur(userID);
            }
            return orders;
        }

        private bool CheckIfUserExists(int userID)
        {
            return (this._admin_EntrepreneurOrdersHandler.CheckIfUserExists(userID) > 0);
        }

        private bool CheckIfUserHasCompanies(int userID)
        {
            return (this._admin_EntrepreneurOrdersHandler.CheckIfUserHasCompanies(userID) > 0);
        }

        private bool checkValidityOfNumber(int userID)
        {
            return (userID > 0);
        }
    }
}
