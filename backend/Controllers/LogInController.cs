using backend.Handlers;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogInController : ControllerBase
    {
        private readonly LogInHandler _logInHandler;

        public LogInController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("LogInContext");
            _logInHandler = new LogInHandler(connectionString);
        }

        [HttpPost("search")]
        public async Task<ActionResult<bool>> SearchUser([FromBody] LogInModel userData)
        {   // get true if there is at least one user with the data specified
            bool userExists = await _logInHandler.SearchUser(userData);

            if (userExists)
            {
                return Ok(new { success = true });
            }

            return Ok(new { success = false });
        }


        [HttpPost("getData")]
        public async Task<IActionResult> LogIn([FromBody] LogInModel userInformationResponse)
        {   // get the credentials
            var userInfo = await _logInHandler.getUserInformation(userInformationResponse);

            if (userInfo.UserId != null)
            {   
                // create an object for the response
                var response = new
                {
                    UserId = userInfo.UserId,
                    UserType = userInfo.UserType,
                    LoginDate = userInfo.LoginDate
                };
                return Ok(response);
            }
           
            return Unauthorized();
        }

    }
}

