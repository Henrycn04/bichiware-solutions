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

        public UpdateDeliveryController(UpdateDeliveryCommand updateDeliveryCommand)
        {
            _updateDeliveryCommand = updateDeliveryCommand;
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
    }
}