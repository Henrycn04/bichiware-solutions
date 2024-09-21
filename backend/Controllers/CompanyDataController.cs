using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Handlers;

namespace backend.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CompanyDataController : ControllerBase
    {
        private readonly CompanyDataHandler _companyDataHandler;

        public CompanyDataController()
        {
            _companyDataHandler = new CompanyDataHandler();
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
                var result = companyDataHandler.AddNewCompany(newCompany);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error registrating company: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddAddress(AddressModel newAddress)
        {
            try
            {
                if (newAddress == null)
                {
                    return BadRequest();
                }

                CompanyDataHandler companyDataHandler = new CompanyDataHandler();
                var result = companyDataHandler.AddNewAddress(newAddress);
                return new JsonResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error registrating address: {ex.Message}");
            }
        }

    }
}

