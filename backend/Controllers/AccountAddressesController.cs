using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Handlers;

namespace backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class AccountAddressesController : Controller
    {
        private readonly AccountAddressesHandler handler;


        public AccountAddressesController()
        {
            this.handler = new AccountAddressesHandler();
        }


        [HttpGet]
        public async Task<ActionResult<List<AddressModel>>> GetUserAddresses(string userId)
        {
            try
            {
                if (userId == null)
                {
                    return BadRequest();
                }

                List<AddressModel> addresses = this.handler.RequestUserAddresses(userId);
                return addresses;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error at getting user addresses: " + ex.Message);
            }
        }
    }
}
