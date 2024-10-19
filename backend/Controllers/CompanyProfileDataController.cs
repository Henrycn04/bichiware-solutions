using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Handlers;
using System.ComponentModel.Design;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyProfileDataController : ControllerBase
    {
        private readonly CompanyProfileDataHandler _companyProfileHandler;

        public CompanyProfileDataController()
        {
            this._companyProfileHandler = new CompanyProfileDataHandler();
        }

        [HttpGet("UserCompanies")]
        public List<CompaniesIDModel> GetUserCompanies(int userID)
        {
            List<CompaniesIDModel> userCompanies = _companyProfileHandler.getUserCompanies(userID);
            return userCompanies;
        }

        [HttpGet("CompanyData")]
        public CompanyProfileDataModel GetCompanyData(int companyID)
        {
            CompanyProfileDataHandler companyProfileHandler = new CompanyProfileDataHandler();
            CompanyProfileDataModel companyProfileModel = new CompanyProfileDataModel();
            companyProfileModel = companyProfileHandler.getCompanyData(companyID);
            return companyProfileModel;
        }

        [HttpGet("CompanyProducts")]
        public List<ProductForDeliveriesModel> GetCompanyProducts(int companyID)
        {
            CompanyProfileDataHandler companyProfileHandler = new CompanyProfileDataHandler();
            List<ProductForDeliveriesModel> companyProducts = new List<ProductForDeliveriesModel>();
            companyProducts = companyProfileHandler.getCompanyProducts(companyID);
            return companyProducts;
        }

    }
}