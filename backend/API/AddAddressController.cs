using backend.Handlers;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddAddressController : ControllerBase
    {
        private readonly AddAddressHandler handler;

        public AddAddressController()
        {
            this.handler = new AddAddressHandler();
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddAddress(PhysicalAddress newAddress)
        {
            try
            {
                if (newAddress == null)
                {
                    Console.WriteLine("The new address is null");
                    return BadRequest();
                }

                this.handler.addNewAddress(newAddress);
                return Ok("Address registered correctly.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error adding new Address to user: {ex.Message}");
            }
        }
    }
}
