using backend.Domain;
using backend.Application;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly UserDataCommand dataUpdater;
        private readonly UserDataQuery informationGatherer;
        private readonly ValidateUserDataService validator;
        public UserDataController() { 
            this.dataUpdater = new UserDataCommand();
            this.informationGatherer = new UserDataQuery();
            this.validator = new ValidateUserDataService();
        }
        
        [HttpGet("getData")]
        public IActionResult getUserData(int userID) {
            try
            {
                UserDataModel result = this.informationGatherer.getData(userID);
                return Ok(result);
            }
            catch (Exception ex) { 
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("updateData")]
        public async Task<ActionResult<bool>> updateData(UserDataModel newData)
        {
            try
            {
                if (newData == null) return BadRequest();
                if (!this.validator.ValidateUserUpdate(newData))
                {
                    throw new Exception("Invalid data, cannot update");
                }
                this.dataUpdater.setData(newData);
                return Ok("User registered correctly");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating data: {ex.Message}");
            }
        }

    }
}
