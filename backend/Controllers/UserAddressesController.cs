using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Handlers;
using System.ComponentModel.Design;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressesController : ControllerBase
    {
        private readonly UserAddressesHandler _userAddressesHandler;

        public UserAddressesController()
        {
            this._userAddressesHandler = new UserAddressesHandler();
        }

        [HttpGet]
        public List<UserAddressesModel> GetUserAddresses(int userID)
        {
            List<int> addressesIDs = _userAddressesHandler.GetAddressesID(userID);
            Console.WriteLine($"Address IDs en controller: {string.Join(", ", addressesIDs)}");
            var userAddresses = _userAddressesHandler.GetAddresses(addressesIDs);
            Console.WriteLine($"Addresses en controller: {string.Join(", ", userAddresses)}");
            return userAddresses;

        }
    }
}