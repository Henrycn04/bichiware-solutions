using backend.Handlers;
using backend.Models;

namespace backend.Queries
{
    public class ConfirmOrdersForCompaniesQuery
    {
        private readonly ConfirmOrdersToCompaniesHandler _confirmOrdersToCompaniesHandler;
        public ConfirmOrdersForCompaniesQuery()
        {
            this._confirmOrdersToCompaniesHandler = new ConfirmOrdersToCompaniesHandler();
        }

        public List<ConfirmOrderForCompaniesModel> GetDataForEmails(int orderID)
        {
            List<ConfirmOrderForCompaniesModel> companiesData = new List<ConfirmOrderForCompaniesModel>();
            companiesData = this._confirmOrdersToCompaniesHandler.GetDataForEmails(orderID);
            return companiesData;
        }
    }
}
