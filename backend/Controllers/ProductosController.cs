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
        // Endpoint to get the price range of non-perishable products
        [HttpGet("price-range/non-perishable")]
        public IActionResult GetNonPerishablePriceRange()
        {
            var rangoPrecios = _productosHandler.ObtenerRangoDePreciosNoPerecederos();
            return Ok(new { minPrice = rangoPrecios.minPrice, maxPrice = rangoPrecios.maxPrice });
        }

        // Endpoint to get the price range of perishable products
        [HttpGet("price-range/perishable")]
        public IActionResult GetPerishablePriceRange()
        {
            var rangoPrecios = _productosHandler.ObtenerRangoDePreciosPerecederos();
            return Ok(new { minPrice = rangoPrecios.minPrice, maxPrice = rangoPrecios.maxPrice });
        }

        // Endpoint to get the price range of all combined products (perishable and non-perishable)
        [HttpGet("price-range")]
        public IActionResult GetCombinedPriceRange()
        {
            var rangoPrecios = _productosHandler.ObtenerRangoDePrecios();
            return Ok(new { minPrice = rangoPrecios.minPrice, maxPrice = rangoPrecios.maxPrice });
        }

        // Endpoint to get non-perishable products
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

        // Endpoint to get perishable products

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
        // Endpoint to get all companies for non-perishable products
        [HttpGet("companies/non-perishable")]
        public IActionResult GetNonPerishableCompanies()
        {
            var empresas = _productosHandler.ObtenerEmpresasNoPerecederos();
            return Ok(empresas);
        }
        // Endpoint to get all companies for perishable products
        [HttpGet("companies/perishable")]
        public IActionResult GetPerishableCompanies()
        {
            var empresas = _productosHandler.ObtenerEmpresasPerecederos();
            return Ok(empresas);
        }


        // Endpoint to get unique company IDs
        [HttpGet("companies")]
        public IActionResult GetUniqueCompanies()
        {
            var empresas = _productosHandler.ObtenerEmpresasUnicas();
            return Ok(empresas);
        }



    }
}
