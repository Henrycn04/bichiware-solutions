using backend.Models;
using backend.Commands;
using Microsoft.AspNetCore.Mvc;
using backend.Application;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UpdateCompanyController : Controller
    {
        private readonly UpdateCompanyCommand _updateCompanyCommand;
        private readonly UpdateCompanyQuery _updateCompanyQuery;
        private readonly DeleteCompanyCommand deleteCompanyCommand;

        public UpdateCompanyController(
            UpdateCompanyCommand updateCompanyCommand,
            DeleteCompanyCommand deleteCompanyCommand)
        {
            this._updateCompanyCommand = updateCompanyCommand;
            this._updateCompanyQuery = new UpdateCompanyQuery();
            this.deleteCompanyCommand = deleteCompanyCommand;
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

        [HttpDelete("DeleteCompany")]
        public ActionResult<string> DeleteCompany(CompaniesIDModel company)
        {
            try
            {
                var response = this.deleteCompanyCommand.DeleteCompany(company);
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
