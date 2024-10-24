using Microsoft.AspNetCore.Mvc;
using backend.Application;
using backend.Models;
using backend.Domain;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchProductController : ControllerBase
    {
        private readonly SearchProductQuery _searchProductQuery;

        public SearchProductController(SearchProductQuery searchProductQuery)
        {
            _searchProductQuery = searchProductQuery;
        }

        [HttpGet("individualproduct")]
        public ActionResult<GeneralProductModel> GetIndividualDelivery([FromQuery] int id)
        {
            // Crear el modelo de búsqueda
            var searchModel = new SearcProductModel { ID = id };

            try
            {
                var product = _searchProductQuery.GetIndividualDelivery(searchModel);
                return Ok(product);
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
                // Manejo genérico de errores
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}