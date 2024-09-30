using backend.Handlers;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteFromCompanyController : ControllerBase
    {
        private DeleteFromCompanyHandler handler;

        public DeleteFromCompanyController()
        {
            this.handler = new DeleteFromCompanyHandler();
        }
        
        [HttpPost]
        public async Task<ActionResult<bool>> deleteFromCompany(DeleteFromCompanyModel data)
        {
            try
            {
                if (data == null)
                {
                    return BadRequest();
                }

                this.handler.deleteUserCompany(data);
                return Ok("Address registered correctly.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding new Address to user: {ex.Message}");
            }
        }
    }
}
