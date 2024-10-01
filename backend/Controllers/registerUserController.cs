using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using backend.Handlers;
using backend.Models;


namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class registerUserController : ControllerBase {
    
       private registerUserHandler userHandler;

        public registerUserController() { 
            this.userHandler = new registerUserHandler();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> registerUser(registerUserModel data)
        {
            try
            {
                if (data == null) return BadRequest();
                int IDUser = this.userHandler.addProfile(data);
                this.userHandler.addUser(data, IDUser);
                int IDAddr = this.userHandler.addAddr(data);
                this.userHandler.addReferencesAddr(IDUser, IDAddr);
                return Ok("User registered correctly");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error registering new user: {ex.Message}");
            }
        }
    }
}
