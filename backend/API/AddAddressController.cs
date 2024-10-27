using backend.Application;
using backend.Handlers;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddAddressController : ControllerBase
    {
        private readonly AddAddressCommand command;
        private readonly ValidateUserDataService validator;

        public AddAddressController()
        {
            this.command = new AddAddressCommand();
            this.validator = new ValidateUserDataService();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddAddress(AddAddressModel newAddress)
        {
            try
            {
                if (newAddress == null)
                {
                    Console.WriteLine("The new address is null");
                    return BadRequest();
                }
                if (!this.validator.ValidateAddress(newAddress))
                {
                    throw new Exception("Invalid data, cannot add new address");
                }
                this.command.addAddress(newAddress);
                return Ok("Address registered correctly.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding new Address to user: {ex.Message}");
            }
        }
    }
}
