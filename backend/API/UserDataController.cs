using backend.Domain;
using backend.Application;
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
        public UserDataController() { 
            this.dataUpdater = new UserDataCommand();
            this.informationGatherer = new UserDataQuery();
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
