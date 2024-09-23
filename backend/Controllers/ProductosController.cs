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
        // Endpoint para obtener el rango de precios de los productos
        [HttpGet("price-range")]
        public IActionResult GetPriceRange()
        {
            var rangoPrecios = _productosHandler.ObtenerRangoDePrecios();
            return Ok(new { minPrice = rangoPrecios.minPrice, maxPrice = rangoPrecios.maxPrice });
        }


        // Endpoint para obtener productos no perecederos
        [HttpGet("non-perishable")]
        public IActionResult ObtenerProductosNoPerecederos([FromQuery] string categoria, [FromQuery] int precioMin, [FromQuery] int precioMax, [FromQuery] List<int> empresas)
        {
            // Imprimir detalles de la solicitud completa
            Console.WriteLine("Solicitud HTTP:");
            Console.WriteLine($"Método: {Request.Method}");
            Console.WriteLine($"URL Completa: {Request.Path + Request.QueryString}");
            Console.WriteLine("Encabezados:");
            foreach (var header in Request.Headers)
            {
                Console.WriteLine($"{header.Key}: {header.Value}");
            }
            Console.WriteLine("Categoría: " + categoria);
            Console.WriteLine("Precio Mínimo: " + precioMin);
            Console.WriteLine("Precio Máximo: " + precioMax);
            Console.WriteLine("Empresas seleccionadas: " + (empresas != null ? string.Join(", ", empresas) : "No se seleccionaron empresas"));

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
        // Endpoint para obtener los IDs de empresas únicas
        [HttpGet("unique-companies")]
        public IActionResult GetUniqueCompanies()
        {
            var empresas = _productosHandler.ObtenerEmpresasUnicas();
            return Ok(empresas);
        }


    }
}
