using backend.Commands;
using backend.Domain;
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
        private readonly SearchBarCommand _searchBarCommand;


        public ProductosController()
        {
            _productosHandler = new ProductosHandler();
            _searchBarCommand = new SearchBarCommand(_productosHandler);
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
            var productos = _productosHandler.GetNonPerishableProducts(categoria, precioMin, precioMax, empresas);
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
            var productos = _productosHandler.GetPerishableProducts(categoria, precioMin, precioMax, empresas);
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


        // Endpoint to search for the non perishble products that contains the searchTerm 
        [HttpGet("search_non-perishable")]
        public IActionResult SearchNonPerishableProduct([FromQuery] SearchBarModel searchModel)
        {
            var products = _searchBarCommand.SearchNonPerishableProduct(searchModel);
            if (products == null || !products.Any())
            {
                return Ok(); 
            }
            return Ok(products);
        }

        // Endpoint to search for the perishble products that contains the searchTerm 
        [HttpGet("search_perishable")]
        public IActionResult SearchPerishableProduct([FromQuery] SearchBarModel searchModel)
        {
            var products = _searchBarCommand.SearchPerishableProduct(searchModel);
            if (products == null || !products.Any())
            {
                return Ok(); 
            }
            
            return Ok(products);
        }

        // Endpoint to get all deliverys from a Product 
        [HttpGet("getProductDeliveries")]
        public IActionResult getProductDeliveries([FromQuery] SearchBarModel searchModel)
        {
            var products = _searchBarCommand.getProductDeliveries(searchModel);
            if (products == null || !products.Any())
            {
                return Ok();
            }
            return Ok(products);
        }
    }
}



