using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Handlers;
using System.ComponentModel.Design;

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
        public async Task<ActionResult<bool>> AddCompany(CompanyModel newCompany)
        {
            try
            {
                if (newCompany == null)
                {
                    return BadRequest();
                }

                CompanyDataHandler companyDataHandler = new CompanyDataHandler();
                int newCompanyID = companyDataHandler.AddNewCompany(newCompany);
                int newAddressID = companyDataHandler.AddNewAddress(newCompany);
                bool success = companyDataHandler.AddNewCompanyAddress(newCompanyID, newAddressID);
                return Ok("Company registered correctly.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error registrating company: {ex.Message}");
            }
        }
    }
}