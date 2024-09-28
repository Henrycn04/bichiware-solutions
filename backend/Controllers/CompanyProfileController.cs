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
        public UserCompaniesModel GetUserCompanies(int userID)
        {
            Console.WriteLine($"ID Usuario recibida {userID}");
            CompanyProfileHandler companyProfileHandler = new CompanyProfileHandler();
            UserCompaniesModel userCompaniesModel = new UserCompaniesModel();
            userCompaniesModel = companyProfileHandler.getUserCompanies(userID);
            Console.WriteLine($"CompanyName: {userCompaniesModel.UserCompanies[0].CompanyName}");
            return userCompaniesModel;
        }

        [HttpGet("CompanyData")]
        public CompanyProfileModel GetCompanyData(int companyID)
        {
            Console.WriteLine($"ID Empresa recibida {companyID}");
            CompanyProfileHandler companyProfileHandler = new CompanyProfileHandler();
            CompanyProfileModel companyProfileModel = new CompanyProfileModel();
            companyProfileModel = companyProfileHandler.getCompanyData(companyID);
 
            Console.WriteLine($"CompanyName: {companyProfileModel.CompanyName}");
            
            return companyProfileModel;
        }
    }
}