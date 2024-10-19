using backend.Handlers;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetTypeController : ControllerBase
    {

        private readonly SetTypeHandler handler;

        public SetTypeController()
        {
            this.handler = new SetTypeHandler();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> SetNewType(SetTypeModel newType)
        {
            try
            {
                if (newType == null)
                {
                    return BadRequest();
                }

                this.handler.setNewType(newType);
                return Ok("Type changed correctly.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding new Address to user: {ex.Message}");
            }
        }
    }
}
