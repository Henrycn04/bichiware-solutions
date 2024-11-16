using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Infrastructure;
using backend.Application;

namespace backend.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class AccountAddressesController : Controller
    {
        private readonly AccountAddressesHandler handler;
        private readonly DeleteAddressCommand deleteCommand;


        public AccountAddressesController()
        {
            this.handler = new AccountAddressesHandler();
            this.deleteCommand = new DeleteAddressCommand();
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

        [HttpPost]
        public async Task<ActionResult<bool>> DeleteAddresess(int[] addressList)
        {
            try
            {

                for (int i = 0; i < addressList.Length; i++)
                {
                    this.deleteCommand.DeleteAddress(addressList[i]);
                }
                return Ok("Addresses deleted correctly.");
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error on delete address:" + ex.Message);
            }
        }
    }
}
