using backend.Models;

namespace backend.Infrastructure
{
    public interface IUpdateCompanyHandler
    {
        public void modifyCompanyData(CompanyProfileModel newData);
        public bool DeleteCompany(CompaniesIDModel company);
        public bool CheckCompanyExistence(CompaniesIDModel company);
        public bool IsNotHeadquarters(CompaniesIDModel company);
        public bool HasNoPendingOrders(CompaniesIDModel company);
    }
}
