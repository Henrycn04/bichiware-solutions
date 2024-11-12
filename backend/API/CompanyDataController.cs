using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Handlers;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyDataController : ControllerBase
    {
        private readonly CompanyDataHandler _companyDataHandler;

        public CompanyDataController()
        {
            this._companyDataHandler = new CompanyDataHandler();
        }

        [HttpPost]
        public int AddCompany(CompanyModel newCompany)
        {
            if (newCompany == null)
            {
                return -1;
            }
            CompanyDataHandler companyDataHandler = new CompanyDataHandler();
            var newCompanyID = companyDataHandler.AddNewCompany(newCompany);
            var newAddressID = companyDataHandler.AddNewAddress(newCompany);
            bool success = companyDataHandler.AddNewCompanyAddress(newCompanyID, newAddressID);
            return newCompanyID;
  
        }

        [HttpGet("getProductOwner")]
        public async Task<ActionResult<string>> GetProductOwner(int companyID)
        {
            try
            {
                var companyName = await this._companyDataHandler.getCompanyName(companyID);
                return Ok(companyName);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error registrando la compañía: {ex.Message}");
            }
        }
    }
}