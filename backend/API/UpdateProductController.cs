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

        public UpdateProductController(UpdateProductCommand updateProductCommand)
        {
            _updateProductCommand = updateProductCommand;
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
    }
}