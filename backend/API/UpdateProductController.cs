using Microsoft.AspNetCore.Mvc;
using backend.Commands;
using backend.Models;
using backend.Domain;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UpdateProductController : ControllerBase
    {
        private readonly UpdateProductCommand _updateProductCommand;
        private readonly DeleteProductCommand _deleteProductCommand;

        public UpdateProductController(UpdateProductCommand updateProductCommand, DeleteProductCommand deleteProductCommand)
        {
            _updateProductCommand = updateProductCommand;
            _deleteProductCommand = deleteProductCommand;
        }

        [HttpPost("update-perishable")]
        public IActionResult UpdatePerishableProduct([FromBody] UpdatePerishablProductModel updateModel)
        {
            if (updateModel == null)
            {
                return BadRequest("El modelo de producto perecedero no puede ser nulo.");
            }

            try
            {
                _updateProductCommand.UpdatePerishableProduct(updateModel);
                return Ok("Producto perecedero actualizado con éxito.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al actualizar el producto perecedero: " + ex.Message);
            }
        }


        [HttpPost("update-non-perishable")]
        public IActionResult UpdateNonPerishableProduct([FromBody] UpdateNonPerishableProductModel updateModel)
        {
            if (updateModel == null)
            {
                return BadRequest("El modelo de producto no perecedero no puede ser nulo.");
            }

            try
            {
                _updateProductCommand.UpdateNonPerishableProduct(updateModel);
                return Ok("Producto no perecedero actualizado con éxito.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error al actualizar el producto no perecedero: " + ex.Message);
            }
        }
        [HttpDelete("delete-perishable/{productId}")]
        public IActionResult DeletePerishableProduct(int productId)
        {
            try
            {
                _deleteProductCommand.DeletePerishableProduct(productId);
                return Ok("Product deleted success.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error deleting product: " + ex.Message);
            }
        }

        [HttpDelete("delete-non-perishable/{productId}")]
        public IActionResult DeleteNonPerishableProduct(int productId)
        {

            try
            {
                _deleteProductCommand.DeleteNonPerishableProduct(productId);
                return Ok("Product deleted success.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error deleting product: " + ex.Message);
            }
        }
    }
}