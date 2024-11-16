using Microsoft.AspNetCore.Mvc;
using backend.Commands;
using backend.Models;

using backend.Domain;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UpdateDeliveryController : ControllerBase
    {
        private readonly UpdateDeliveryCommand _updateDeliveryCommand;
        private readonly DeleteDeliveryCommand _deleteDeliveryCommand;

        public UpdateDeliveryController(UpdateDeliveryCommand updateDeliveryCommand, DeleteDeliveryCommand deleteDeliveryCommand)
        {
            _updateDeliveryCommand = updateDeliveryCommand;
            _deleteDeliveryCommand = deleteDeliveryCommand;
        }

        // Método para actualizar una entrega
        [HttpPost("update")]
        public ActionResult UpdateDelivery([FromBody] UpdateDeliveryModel updateModel)
        {
            try
            {

                _updateDeliveryCommand.UpdateDelivery(updateModel);

                return NoContent();
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
        [HttpDelete("delete-delivery")]
        public IActionResult DeleteNonPerishableProduct([FromQuery] SearchDeliveryModel deliveryId)
        {
            try
            {
                _deleteDeliveryCommand.DeleteDelivery(deliveryId);
                return Ok("Delivery deleted success.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error deleting delivery: " + ex.Message);
            }
        }
    }
}