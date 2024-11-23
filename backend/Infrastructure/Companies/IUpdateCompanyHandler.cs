using backend.Models;

namespace backend.Infrastructure
{
    public interface IUpdateCompanyHandler
    {
        public void modifyCompanyData(CompanyProfileModel newData);
        public bool DeleteCompany(int companyId);
        public bool CheckCompanyExistence(int companyId);
        public bool IsHeadquarters(int companyId);
        public bool HasPendingOrders(int companyId);
        public void DeleteCompanyProducts(int companyId);
    }
}
