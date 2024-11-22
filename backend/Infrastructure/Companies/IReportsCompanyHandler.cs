using backend.Domain;
using backend.Models;

namespace backend.Infrastructure
{
    public interface IReportsCompanyHandler
    {
        public List<PendingOrderReport> GeneratePendingOrdersReport(FiltersCompletedOrdersModel filter);
        public bool CheckUserType(int userId);
    }
}
