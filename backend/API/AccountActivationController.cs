﻿using Microsoft.AspNetCore.Mvc;
using backend.Handlers;
using backend.Domain;
using backend.Application;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountActivationController : Controller
    {
        private readonly AccountActivationHandler accountActivationHandler;


        public AccountActivationController(IMailService mailService)
        {
            this.accountActivationHandler = new AccountActivationHandler(mailService);
        }


        [HttpPost]
        public async Task<ActionResult<bool>> RequestConfirmationEmail(string userId)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest();
                }

                var response = this.accountActivationHandler.SendConfirmationEmail(userId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error at sending confirmation email");
            }
        }


        [HttpPost]
        public async Task<ActionResult<bool>> ActivateAccount(AccountActivationModel activationRequestModel)
        {
            try
            {
                if (activationRequestModel == null)
                {
                    return BadRequest();
                }

                var response = this.accountActivationHandler.VerifyConfirmationCode(activationRequestModel);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error at activating account");
            }
        }
    }
}
