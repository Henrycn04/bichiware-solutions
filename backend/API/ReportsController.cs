using backend.Application;
using backend.Commands;
using backend.Domain;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ReportsCompanyCommand reportsCompany;

        public ReportsController(ReportsCompanyCommand reportsCompany)
        {
            this.reportsCompany = reportsCompany;
        }

        [HttpGet("getReport/completedOrders/")]
        public async Task<IActionResult> GetOrdersInProgress([FromQuery] FiltersCompletedOrdersModel filter)
        {
            var getAllOrders = new CompletedOrdersQuery();
            var orders = await getAllOrders.Execute(filter);

            if (orders == null || orders.Count == 0)
            {
                return Ok("No orders in progress found for the specified user.");
            }

            return Ok(orders);
        }

        [HttpGet("getReport/pendingOrders/")]
        public async Task<ActionResult<List<PendingOrderReport>>> GetPendingOrders([FromQuery] FiltersCompletedOrdersModel filter)
        {
            try
            {
                var response = this.reportsCompany.GetPendingOrdersReport(filter);
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
