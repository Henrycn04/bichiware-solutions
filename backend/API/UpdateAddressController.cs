using backend.Application;
using backend.Commands;
using backend.Infrastructure;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class UpdateAddressController : ControllerBase
    {
        private readonly UpdateAddressHandler _updateHandler;

        public UpdateAddressController()
        {
            _updateHandler = new UpdateAddressHandler();
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateAddress([FromBody] AddressModelUpdate addressToUpdate)
        {


            var updateAddressCommand = new UpdateAddressCommand(_updateHandler);
            if (!addressToUpdate.IsCompany) {
                Console.WriteLine($"LLEGOOOOOOOO: {addressToUpdate.IsCompany}");
                if (!await updateAddressCommand.ExecuteUser(addressToUpdate))
                    Console.WriteLine("iNVALID USER.");
                    return Ok("Invalid user.");
            } else
            {
                if (!await updateAddressCommand.ExecuteCompany(addressToUpdate))
                    return Ok("Invalid company.");
            }
            Console.WriteLine("Update succesful.");
            return Ok("Update succesful.");
        }

    }

}
