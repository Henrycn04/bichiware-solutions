using backend.Queries;
using backend.Domain;
using Microsoft.AspNetCore.Mvc;
using backend.Infrastructure;
using backend.Models;


namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchDeliveryController : ControllerBase
    {
        private readonly SearchDeliveryQuery _searchDeliveryQuery;

        public SearchDeliveryController(SearchDeliveryQuery searchDeliveryQuery)
        {
            _searchDeliveryQuery = searchDeliveryQuery;
        }
        [HttpGet("individualDelivery")]
        public IActionResult GetIndividualDelivery([FromQuery] SearchDeliveryModel searchModel)
        {
            if (searchModel == null)
            {
                return BadRequest("Search model cannot be null.");
            }

            try
            {
                var delivery = _searchDeliveryQuery.GetIndividualDelivery(searchModel);
                return Ok(delivery);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
        [HttpPost("specificDeliveries")]
        public ActionResult<List<AddDeliveryModel>> GetDeliveriesFromSpecificProducts([FromBody] SearchProductListModel searchModel)
        {
            if (searchModel == null)
            {
                return BadRequest("Search model cannot be null.");
            }

            try
            {
                var deliveries = _searchDeliveryQuery.GetlDeliviesFromSpecificProducts(searchModel);
                return Ok(deliveries);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}