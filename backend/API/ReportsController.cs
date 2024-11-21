using backend.Application;
using backend.Commands;
using backend.Domain;
using backend.Infrastructure;
using backend.Models;
using backend.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {

        private ClientReportQuery clientReportQuery;
        private TotalProfitsQuery _totalProfitsReportQuery;
        public ReportsController(TotalProfitsQuery totalProfitsReportQuery)
        {
            clientReportQuery = new ClientReportQuery();
            _totalProfitsReportQuery = totalProfitsReportQuery;
        }
        /*
        [HttpGet("getReport/completedOrders/")]
        public async Task<IActionResult> GetOrdersInProgress([FromQuery] FiltersCompletedOrdersModel filter)
        {
            Console.WriteLine($"User: {filter.UserID}");
            Console.WriteLine($"Company: {filter.CompanyID}");
            Console.WriteLine($"Compañias: {filter.AllCompanies}");
            var getAllOrders = new CompletedOrdersQuery();
            var orders = await getAllOrders.Execute(filter);

            if (orders == null || orders.Count == 0)
            {
                return Ok("No orders in progress found for the specified user.");
            }

            return Ok(orders);
        }*/

        [HttpPost("getReport/clientReport/")]
        public async Task<IActionResult> GetClientReports(ClientReportRequestModel request)
        {
            try 
            {
                if (request == null) throw new Exception("Null request is not accepted");

                var response = this.clientReportQuery.GetReport(request);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost("total-profits")]
        public async Task<IActionResult> GetTotalProfits([FromBody] TotalProftsRequestModel request)
        {
            try
            {
                var result = await _totalProfitsReportQuery.GetTotalProfits(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error inesperado.", Details = ex.Message });
            }
        }

    }
}
