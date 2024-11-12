using Microsoft.AspNetCore.Mvc;
using backend.Application;
using backend.Models;
using backend.Domain;
using backend.Infrastructure;

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
        public ActionResult<GeneralProductModel> GetIndividualProduct([FromQuery] int id)
        {
            // Crear el modelo de búsqueda
            var searchModel = new SearcProductModel { ID = id };

            try
            {
                var product = _searchProductQuery.GetIndividualProduct(searchModel);
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
        [HttpPost("specificproducts")]
        public ActionResult<List<GeneralProductModel>> GetSpecificProducts([FromBody] SearchProductListModel searchModel)
        {
            if (searchModel == null)
            {
                return BadRequest("Search model cannot be null.");
            }

            try
            {
                var products = _searchProductQuery.GetSpecificProductList(searchModel);
                return Ok(products);
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