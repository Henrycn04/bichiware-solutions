using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Handlers;
using backend.Models;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class registerUserController : ControllerBase {
    
       private registerUserHandler userHandler;

        registerUserController() { 
            this.userHandler = new registerUserHandler();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> registerUser(registerUserModel data)
        {
            try
            {
                if (data == null) return BadRequest();
                int IDUser = this.userHandler.addUser(data);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error registering new user: {ex.Message}");
            }
        }
    }
}
