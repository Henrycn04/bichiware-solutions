using backend.Handlers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using backend.Services;


namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ChangePasswordController : Controller
    {
        private readonly ChangePasswordHandler changePasswordHandler;

        public ChangePasswordController (IMailService mailService)
        {
            this.changePasswordHandler = new ChangePasswordHandler(mailService);
        }


        [HttpPost]
        public async Task<ActionResult<bool>> SendConfirmationEmail(string email)
        {
            try
            {
                var response = this.changePasswordHandler.SendConfirmationEmail(email);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Error at resending code through email");
            }
        }


            [HttpPost]
        public async Task<ActionResult<bool>> ChangePassword(ChangePasswordModel changePasswordModel)
        {
            try
            {
                if (changePasswordModel == null)
                {
                    return BadRequest();
                }

                var response = this.changePasswordHandler.AttemptChangePassword(changePasswordModel);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Error at changing password");
            }
        }
    }
}
