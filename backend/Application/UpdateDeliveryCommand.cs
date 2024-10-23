using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using backend.Models;

namespace backend.Commands
{
    public class UpdateDeliveryCommand
    {
        private readonly UpdateDeliveryHandler _deliveryUpdateHandler;
        private readonly SearchDeliveryHandler _deliverySearchHandler;

        public UpdateDeliveryCommand(UpdateDeliveryHandler deliveryUpdateHandler, SearchDeliveryHandler deliverySearchHandler)
        {
            _deliveryUpdateHandler = deliveryUpdateHandler;
            _deliverySearchHandler = deliverySearchHandler;
        }


        public void UpdateDelivery(UpdateDeliveryModel updateModel)
            // This error messages are presented to the user through an alert so they have to be in spanish
        {
            if (updateModel.ProductID <= -1)
            {
                throw new ArgumentException("ProductID debe ser mayor a -1.");
            }

            if (updateModel.BatchNumber <= -1)
            {
                throw new ArgumentException("Numero de lote debe ser mayor a -1.");
            }

            if (updateModel.OldBatchNumber <= -1)
            {
                throw new ArgumentException("Numero antiguo de lote debe ser mayor a -1.");
            }

            if (updateModel.ExpirationDate <= DateTime.Now)
            {
                throw new ArgumentException("Fecha de expiracion debe ser en el futuro.");
            }

            var existingDelivery = _deliverySearchHandler.GetSpecificDelivery(updateModel.ProductID, updateModel.OldBatchNumber);
            if (existingDelivery == null)
            {
                throw new InvalidOperationException("No existe el numero de lote antiguo.");
            }

            var conflictingDelivery = _deliverySearchHandler.GetSpecificDelivery(updateModel.ProductID, updateModel.BatchNumber);
            if (conflictingDelivery != null && updateModel.BatchNumber != updateModel.OldBatchNumber)
            {
                throw new InvalidOperationException("Ya existe uan entrega con el mismo numero de lote para el producto escogido.");
            }

            try
            {
                _deliveryUpdateHandler.UpdateDelivery(updateModel.ProductID, updateModel.BatchNumber,
                    updateModel.OldBatchNumber, updateModel.ExpirationDate);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception("Error mientras se actualizaba la entrega.", sqlEx);
            }
        }
    }
}