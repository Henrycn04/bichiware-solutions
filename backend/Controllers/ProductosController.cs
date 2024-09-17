using backend.Handlers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;


namespace backend.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductosController : ControllerBase
    {
        private ProductosHandler _productosHandler;

        public ProductosController()
        {
            _productosHandler = new ProductosHandler();
        }

        // Endpoint para obtener productos no perecederos
        [HttpGet("non-perishable")]
        public IActionResult GetNonPerishableProducts([FromQuery] string category)
        {
            if (category == null) { return Ok(); }
            var productos = _productosHandler.ObtenerProductosNoPerecederos(category);
            if (productos == null || !productos.Any())
            {
                return Ok();
            }
            return Ok(productos);
        }

        // Endpoint para obtener productos perecederos
        [HttpGet("perishable")]
        public IActionResult GetPerishableProducts([FromQuery] string category)
        {
            if (category == null) { return Ok(); }
            var productos = _productosHandler.ObtenerProductosPerecederos(category);
            if (productos == null || !productos.Any())
            {
                return Ok();
            }
            return Ok(productos);
        }
    }
}
