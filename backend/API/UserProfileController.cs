using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Handlers;
using System.ComponentModel.Design;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly UserProfileHandler _userProfileHandler;

        public UserProfileController()
        {
            this._userProfileHandler = new UserProfileHandler();
        }

        [HttpGet]
        public UserProfileModel GetUserData(int userID)
        {
            UserProfileHandler userProfileHandler = new UserProfileHandler();
            UserProfileModel userProfileModel = new UserProfileModel();
            userProfileModel = userProfileHandler.getUserData(userID);
            return userProfileModel;

        }
    }
}