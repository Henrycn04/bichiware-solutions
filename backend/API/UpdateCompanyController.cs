using backend.Models;
using backend.Commands;
using Microsoft.AspNetCore.Mvc;
using backend.Handlers;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UpdateCompanyController : Controller
    {
        private readonly UpdateCompanyCommand _updateCompanyCommand;
        private readonly UpdateCompanyQuery _updateCompanyQuery;

        public UpdateCompanyController()
        {
            this._updateCompanyCommand = new UpdateCompanyCommand();
            this._updateCompanyQuery = new UpdateCompanyQuery();
        }

        [HttpGet("CompanyMainData")]
        public CompanyProfileModel GetCompanyMainData(int companyID)
        {
            CompanyProfileModel model = this._updateCompanyQuery.GetCompanyMainData(companyID);
            return model;
        }

        [HttpPost("UpdateCompanyData")]
        public void ModifyCompanyData(CompanyProfileModel newData)
        {
            this._updateCompanyCommand.ModifyCompanyData(newData);
        }
    }
}
