using backend.Domain;
using backend.Infrastructure;
using backend.Services;

namespace backend.Application
{
    public class MonthlyShippingCostQuery
    {
        private MonthlyShippingCostHandler handler;
        private ValidateMonthlyShippingCostRequestService validator;
        public MonthlyShippingCostQuery() 
        {
            handler = new MonthlyShippingCostHandler();
            validator = new ValidateMonthlyShippingCostRequestService();
        }

        public List<MonthlyShippingResponseModel> GetMonthlyShippingCost(MonthlyShippingRequestModel request)
        {
            if (!validator.ValidateData(request)) throw new Exception("Invalid data in request");
            return handler.GetMonthlyShippingCost(request);
        }
    }
}
