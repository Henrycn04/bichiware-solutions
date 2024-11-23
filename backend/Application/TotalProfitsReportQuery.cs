using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using backend.Models;



namespace backend.Queries
{
    public class TotalProfitsQuery
    {
        private readonly ITotalProfitsHandler _totalProfitsHandler;
        const string nullRequest = "The request cant be null";
        const string emptyYearList = "The list of years cant be empty";
        const string emptyIDList = "The list of company ID cant be empty";
        const string validYear = "Year must be positive";
        const string validID = "ID must be positive";


        public TotalProfitsQuery(ITotalProfitsHandler totalProfitsHandler)
        {
            _totalProfitsHandler = totalProfitsHandler;
        }

        public Task<List<TotalProfitsResponseModel>> GetTotalProfits(TotalProftsRequestModel request)
        {
            if (request == null)
            {
                throw new ArgumentException(nullRequest);
            }

            if (request.Years == null || request.Years.Count == 0)
            {
                throw new ArgumentException(emptyYearList);
            }

            if (request.CompanyIDs == null || request.CompanyIDs.Count == 0)
            {
                throw new ArgumentException(emptyIDList);
            }

            ValidateYears(request.Years);
            ValidateCompanyIDs(request.CompanyIDs);

         
            var response=_totalProfitsHandler.GetTotalProfits(request);
            return response;
        }

        private void ValidateYears(List<int> years)
        {
            foreach (var year in years)
            {
                if (year <= 0)
                {
                    throw new ArgumentException(validYear);
                }
            }
        }

        private void ValidateCompanyIDs(List<int> companyIds)
        {
            foreach (var companyId in companyIds)
            {
                if (companyId <= 0)
                {
                    throw new ArgumentException(validID);
                }
            }
        }
    }
}
