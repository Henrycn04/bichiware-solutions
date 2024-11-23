using System.Runtime.CompilerServices;
using backend.Application;
using backend.Commands;
using backend.Domain;
using backend.Infrastructure;
using backend.Models;
using backend.Queries;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private ClientReportQuery clientReportQuery;
        private readonly CancelledOrdersQuery _cancelledOrdersQuery;
        private TotalProfitsQuery _totalProfitsReportQuery;
        private readonly CompletedOrdersQuery _query;
        public ReportsController(CompletedOrdersQuery completedOrdersQuery,TotalProfitsQuery totalProfitsReportQuery, CancelledOrdersQuery cancelledOrdersQuery)
        {
            this._query = completedOrdersQuery;
            clientReportQuery = new ClientReportQuery();
            _totalProfitsReportQuery = totalProfitsReportQuery;
            this._cancelledOrdersQuery = cancelledOrdersQuery;
        }

        [HttpGet("getReport/completedOrders/")]
        public async Task<IActionResult> OrdersInProgress([FromQuery] FiltersCompletedOrdersModel filter)
        {
            var orders = await this._query.Execute(filter);
            if (orders == null || orders.Count == 0)
            {
                return Ok("No orders in progress found for the specified user.");
            }

            return Ok(orders);
        }

        [HttpGet("getReport/cancelledOrders/")]
        public IActionResult CancelledOrders([FromQuery] FiltersCompletedOrdersModel filters)
        {
            if (filters != null)
            {
                List<CancelledOrdersModel> cancelledOrders = new List<CancelledOrdersModel>();
                cancelledOrders = this._cancelledOrdersQuery.GetCancelledOrders(filters);
                if (cancelledOrders.Count == 0)
                {
                    return Ok("No orders in progress found for the specified user.");
                }
                return Ok(cancelledOrders);
            } else
            {
                return BadRequest("Null filters.");
            }
        }


        [HttpPost("getReport/clientReport/")]
        public async Task<IActionResult> ClientReports(ClientReportRequestModel request)
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
        public async Task<IActionResult> TotalProfits([FromBody] TotalProftsRequestModel request)
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
