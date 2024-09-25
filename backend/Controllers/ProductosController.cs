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
        // Endpoint para obtener el rango de precios de los productos no perecederos
        [HttpGet("price-range/non-perishable")]
        public IActionResult GetNonPerishablePriceRange()
        {
            var rangoPrecios = _productosHandler.ObtenerRangoDePreciosNoPerecederos();
            return Ok(new { minPrice = rangoPrecios.minPrice, maxPrice = rangoPrecios.maxPrice });
        }

        // Endpoint para obtener el rango de precios de los productos perecederos
        [HttpGet("price-range/perishable")]
        public IActionResult GetPerishablePriceRange()
        {
            var rangoPrecios = _productosHandler.ObtenerRangoDePreciosPerecederos();
            return Ok(new { minPrice = rangoPrecios.minPrice, maxPrice = rangoPrecios.maxPrice });
        }

        // Endpoint para obtener el rango de precios de todos los productos combinados (perecederos y no perecederos)
        [HttpGet("price-range")]
        public IActionResult GetCombinedPriceRange()
        {
            var rangoPrecios = _productosHandler.ObtenerRangoDePrecios();
            return Ok(new { minPrice = rangoPrecios.minPrice, maxPrice = rangoPrecios.maxPrice });
        }

        // Endpoint para obtener productos no perecederos
        [HttpGet("non-perishable")]
        public IActionResult ObtenerProductosNoPerecederos([FromQuery] string categoria, [FromQuery] int precioMin, [FromQuery] int precioMax, [FromQuery] List<int> empresas)
        {

            if (categoria == null) { return Ok(); }
            var productos = _productosHandler.ObtenerProductosNoPerecederos(categoria, precioMin, precioMax, empresas);
            if (productos == null || !productos.Any())
            {
                return Ok();
            }
            return Ok(productos);
        }

        // Endpoint para obtener productos perecederos
        [HttpGet("perishable")]
        public IActionResult ObtenerProductosPerecederos([FromQuery] string categoria, [FromQuery] int precioMin, [FromQuery] int precioMax, [FromQuery] List<int> empresas)
        {
            if (categoria == null) { return Ok(); }
            var productos = _productosHandler.ObtenerProductosPerecederos(categoria, precioMin, precioMax, empresas);
            if (productos == null || !productos.Any())
            {
                return Ok();
            }
            return Ok(productos);
        }
        // Endpoint para obtener todas las empresas de productos no perecederos
        [HttpGet("companies/non-perishable")]
        public IActionResult GetNonPerishableCompanies()
        {
            var empresas = _productosHandler.ObtenerEmpresasNoPerecederos();
            return Ok(empresas);
        }
        // Endpoint para obtener todas las empresas de productos perecederos
        [HttpGet("companies/perishable")]
        public IActionResult GetPerishableCompanies()
        {
            var empresas = _productosHandler.ObtenerEmpresasPerecederos();
            return Ok(empresas);
        }


        // Endpoint para obtener los IDs de empresas únicas

        [HttpGet("companies")]
        public IActionResult GetUniqueCompanies()
        {
            var empresas = _productosHandler.ObtenerEmpresasUnicas();
            return Ok(empresas);
        }



    }
}
