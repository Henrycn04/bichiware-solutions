using Microsoft.AspNetCore.Http;
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
            _companyDataHandler = new CompanyDataHandler();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddCompanyData(CompanyDataModel newCompany)
        {
            try
            {
                if (newCompany == null)
                {
                    return BadRequest();
                }

                CompanyDataHandler companyDataHandler = new CompanyDataHandler();
                var result = companyDataHandler.AddCompanyData(newCompany);
                return new JsonResult(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error registrating company");
            }
        }

    }
}

