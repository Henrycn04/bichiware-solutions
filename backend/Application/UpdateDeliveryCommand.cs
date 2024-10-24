using System;
using System.Data.SqlClient;
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
        // the next exceptions are going to be presented to the user thats why they are in spanish

        public void UpdateDelivery(UpdateDeliveryModel updateModel)
        {
            if (updateModel == null)
            {
                throw new ArgumentNullException(nameof(updateModel), "El modelo de actualización no puede ser nulo.");
            }

            ValidateUpdateModel(updateModel);
            CheckExistingDeliveries(updateModel);

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

        private void ValidateUpdateModel(UpdateDeliveryModel model)
        {
            if (model.ProductID <= -1)
            {
                throw new ArgumentException("ProductID debe ser mayor a -1.");
            }

            if (model.BatchNumber <= -1)
            {
                throw new ArgumentException("Número de lote debe ser mayor a -1.");
            }

            if (model.OldBatchNumber <= -1)
            {
                throw new ArgumentException("Número antiguo de lote debe ser mayor a -1.");
            }

            if (model.ExpirationDate <= DateTime.Now)
            {
                throw new ArgumentException("Fecha de expiración debe ser en el futuro.");
            }
        }

        private void CheckExistingDeliveries(UpdateDeliveryModel model)
        {
            var existingDelivery = _deliverySearchHandler.GetSpecificDelivery(model.ProductID, model.OldBatchNumber);
            if (existingDelivery == null)
            {
                throw new InvalidOperationException("No existe el número de lote antiguo.");
            }

            var conflictingDelivery = _deliverySearchHandler.GetSpecificDelivery(model.ProductID, model.BatchNumber);
            if (conflictingDelivery != null && model.BatchNumber != model.OldBatchNumber)
            {
                throw new InvalidOperationException("Ya existe una entrega con el mismo número de lote para el producto escogido.");
            }
        }
    }
}