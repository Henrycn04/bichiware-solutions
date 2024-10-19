using backend.Handlers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyProductsController : ControllerBase
    {
        private CompanyProductsHandler _companyProductsHandler;

        public CompanyProductsController()
        {
            _companyProductsHandler = new CompanyProductsHandler();
        }

        // Endpoint to get non-perishable products
        [HttpGet("non-perishable")]
        public IActionResult ObtenerProductosNoPerecederos([FromQuery] int empresa)
        {

            var productos = _companyProductsHandler.ObtenerProductosNoPerecederos(empresa);
            if (productos == null || !productos.Any())
            {
                return Ok();
            }
            return Ok(productos);
        }

        // Endpoint to get perishable products
        [HttpGet("perishable")]
        public IActionResult ObtenerProductosPerecederos([FromQuery] int empresa)
        {
            var productos = _companyProductsHandler.ObtenerProductosPerecederos(empresa);
            if (productos == null || !productos.Any())
            {
                return Ok();
            }
            return Ok(productos);
        }

    }
}
