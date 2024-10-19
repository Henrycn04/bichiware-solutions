using backend.Handlers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserCompanyListController : Controller
    {
        private readonly UserCompanyListHandler handler;
        private const int ADMINISTRATIVE_ROLE = 3;
        private const int ENTREPRENEUR_ROLE = 2;
        private const int CUSTOMER_ROLE = 1;


        public UserCompanyListController()
        {
            this.handler = new UserCompanyListHandler();
        }


        [HttpGet]
        async public Task<ActionResult<List<UserModel>>> GetUsersList(string userId)
        {
            try
            {
                int userType = handler.GetUserType(userId);
                if (userType == ADMINISTRATIVE_ROLE)
                {
                    var users = this.handler.RequestAllUsers();
                    return users;
                }
                else if (userType == ENTREPRENEUR_ROLE)
                {
                    var users = this.handler.RequestSubordinates(userId);
                    return users;
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "Access Denied");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error at sending users data");
            }
        }

        [HttpGet]
        async public Task<ActionResult<List<CompanyProfileModel>>> GetCompanies(string userId)
        {
            try
            {
                int userType = handler.GetUserType(userId);
                if (userType == ADMINISTRATIVE_ROLE)
                {
                    var companies = this.handler.RequestAllCompanies();
                    return companies;
                }
                else if (userType == ENTREPRENEUR_ROLE)
                {
                    var companies = this.handler.RequestOwnedCompanies(userId);
                    return companies;
                }
                else
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "Access Denied");
                }
            }
            catch (Exception ex1)
            {
                Console.WriteLine(ex1.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Error at sending companies data");
            }
        }
    }
}
