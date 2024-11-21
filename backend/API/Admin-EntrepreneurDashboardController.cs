using backend.Commands;
using backend.Queries;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Admin_EntrepreneurDashboardController : ControllerBase
    {
        private readonly Admin_EntrepreneurDashboardQuery _admin_EntrepreneurDashboardQuery;

        public Admin_EntrepreneurDashboardController(Admin_EntrepreneurDashboardQuery admin_EntrepreneurDashboardQuery)
        {
            this._admin_EntrepreneurDashboardQuery = admin_EntrepreneurDashboardQuery;
        }

        [HttpGet("GetOrdersInProgressForAdmin")]
        public IActionResult GetOrdersInProgressForAdmin()
        {
            var orders =  this._admin_EntrepreneurDashboardQuery.GetOrdersForAdmin();
            if (orders == null || orders.Count == 0)
            {
                return Ok("No orders in progress found.");
            }
            return Ok(orders);
        }

        [HttpGet("GetOrdersInProgressForEntrepreneur/{userID}")]
        public IActionResult GetOrdersInProgressForEntrepreneur(int userID)
        {
            var orders = this._admin_EntrepreneurDashboardQuery.GetOrdersForEntrepreneur(userID);
            if (orders == null || orders.Count == 0)
            {
                return Ok("No orders in progress found for the specified user.");
            }
            return Ok(orders);
        }
    }
}
