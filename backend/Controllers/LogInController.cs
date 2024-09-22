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

        [HttpPost]
        public async Task<ActionResult<bool>> SearchUser([FromBody] LogInModel userData)
        {   
            bool userExists = await _logInHandler.SearchUser(userData);

            if (userExists)
            {
                return Ok(new { success = true });
            }

            return Ok(new { success = false });
        }
    }
}

