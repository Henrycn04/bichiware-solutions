using backend.Domain;
using backend.Application
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly  UserDataCommand dataConnection;
        public UserDataController() { 
            this.dataConnection = new UserDataCommand();
        }
        
        [HttpGet("getData")]
        public UserDataModel getUserData(int userID) { 
            UserDataModel result = new UserDataModel();
            return result;
        }
    }
}
