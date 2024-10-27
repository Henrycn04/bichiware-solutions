using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using backend.Handlers;
using backend.Models;
using backend.Services;
using backend.Application;


namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class registerUserController : ControllerBase {
    
        private RegisterUserCommand userCommand;
        private ValidateUserDataService validator;
        
        public registerUserController() { 
            this.userCommand = new RegisterUserCommand();
            this.validator = new ValidateUserDataService();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> registerUser(registerUserModel data)
        {
            try
            {
                if (data == null) return BadRequest();
                if(!this.validator.ValidateUser(data)) throw new Exception("Invalid data, cannot register user");
                this.userCommand.addUser(data);

                return Ok("User registered correctly");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error registering new user: {ex.Message}");
            }
        }
    }
}
