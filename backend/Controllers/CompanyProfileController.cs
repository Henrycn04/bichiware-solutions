using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Handlers;
using System.ComponentModel.Design;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyProfileController : ControllerBase
    {
        private readonly CompanyProfileHandler _companyProfileHandler;

        public CompanyProfileController()
        {
            this._companyProfileHandler = new CompanyProfileHandler();
        }

        [HttpGet("UserCompanies")]
        public List<CompaniesIDModel> GetUserCompanies(int userID)
        {
            List<CompaniesIDModel> userCompanies = _companyProfileHandler.getUserCompanies(userID);
            return userCompanies;
        }

        [HttpGet("CompanyData")]
        public CompanyProfileModel GetCompanyData(int companyID)
        {
            Console.WriteLine($"id rcibido: {companyID}");
            CompanyProfileHandler companyProfileHandler = new CompanyProfileHandler();
            CompanyProfileModel companyProfileModel = new CompanyProfileModel();
            companyProfileModel = companyProfileHandler.getCompanyData(companyID);
            return companyProfileModel;
        }
    }
}