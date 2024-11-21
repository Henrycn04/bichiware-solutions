using backend.Models;

namespace backend.Infrastructure
{
    public interface IUpdateCompanyHandler
    {
        public void modifyCompanyData(CompanyProfileModel newData);
        public bool DeleteCompany(int companyId);
        public bool CheckCompanyExistence(CompaniesIDModel company);
        public bool IsHeadquarters(CompaniesIDModel company);
        public bool HasPendingOrders(CompaniesIDModel company);
        public void DeleteCompanyProducts(int companyId);
    }
}
