using backend.Domain;
using backend.Application;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using backend.Commands;

namespace backend.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDataController : ControllerBase
    {
        private readonly UserDataCommand dataUpdater;
        private readonly UserDataQuery informationGatherer;
        private readonly ValidateUserDataService validator;
        private readonly DeleteUserDataCommand _deleteUserDataCommand;
        const string companiesRelatedException = "User cannot be deletead because it is the only memebr of a compnany.";
        const string ordersInProgressRelatedException = "User cannot be deletead because it has orders in progress.";
        public UserDataController(DeleteUserDataCommand deleteUserDataCommand)
        {
            this.dataUpdater = new UserDataCommand();
            this.informationGatherer = new UserDataQuery();
            this.validator = new ValidateUserDataService();
            _deleteUserDataCommand = deleteUserDataCommand;
        }

        [HttpGet("getData")]
        public IActionResult getUserData(int userID)
        {
            try
            {
                UserDataModel result = this.informationGatherer.getData(userID);
                return Ok(result);
            }
            catch (Exception ex)
            {
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
        [HttpDelete("userProfile/{userId}")]
        public IActionResult DeleteUserData(int userId)
        {
            try
            {
                _deleteUserDataCommand.DeleteUserData(userId);
                return Ok(new { message = "User deleted successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "BadRequest", message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                if (ex.Message == companiesRelatedException)
                {
                    return Conflict(new { error = "RelatedCompaniesConflict", message = ex.Message });
                }
                else if(ex.Message == ordersInProgressRelatedException)
                {
                    return Conflict(new { error = "RelatedOrdersInProgressConflict", message = ex.Message });

                }
                return StatusCode(500, new { error = "ServerError", message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "ServerError", message = "Error deleting user: " + ex.Message });
            }


        }
    }
}
