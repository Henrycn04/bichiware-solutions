using System.ComponentModel.Design;
using backend.Application;
using backend.Commands;
using backend.Infrastructure;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientDashboardController : ControllerBase
    {
        private readonly LastBoughtProductsQuery _lastBoughtProductsQuery;
        private readonly SearchProductQuery searchProductQuery;

        public ClientDashboardController(GetOrdersHandler getAllHandler,
            LastBoughtProductsQuery lastBoughtProductsQuery,
            SearchProductQuery searchProductQuery)
        {
            _lastBoughtProductsQuery = lastBoughtProductsQuery;
            this.searchProductQuery = searchProductQuery;
        }

        [HttpGet("getOrdersInProgress/{userID}")]
        public async Task<IActionResult> GetOrdersInProgress(int userID)
        {
            try
            {
                var getAllOrders = new GetOrdersQuery();
                var orders = await getAllOrders.Execute(userID);
                return Ok(orders);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet("getLastProductsOrdered/{userID}")]
        public IActionResult GetLastProductsOrdered(int userID)
        {
            try
            {
                var products = _lastBoughtProductsQuery.GetTop10LastProductsOrdered(userID);
                return Ok(products);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }


        [HttpGet("getPorductsForShowcase")]
        public IActionResult GetProductsForShowcase()
        {
            try
            {
                var response = this.searchProductQuery.GetProductsForShowcase();
                return Ok(response);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}