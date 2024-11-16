using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using System;

namespace backend.Commands
{
    public class DeleteDeliveryCommand
    {
        private readonly IUpdateDeliveryHandler _deliveryUpdateHandler;
        private readonly ISearchDeliveryHandler _deliverySearchHandler;
        private readonly IOrdersHandler _orderHandler;

        public DeleteDeliveryCommand(IUpdateDeliveryHandler deliveryUpdateHandler, ISearchDeliveryHandler deliverySearchHandler, IOrdersHandler orderHandler)
        {
            _deliveryUpdateHandler = deliveryUpdateHandler;
            _deliverySearchHandler = deliverySearchHandler;
            _orderHandler = orderHandler;
        }

        public void DeleteDelivery(SearchDeliveryModel newDeliveryId)
        {
            int[] deliveryId = new int[2];
            deliveryId[0]=newDeliveryId.productID;
            deliveryId[1] = newDeliveryId.batchNumber;
         
            if (!IsValidPerishableProductId(deliveryId))
            {
                throw new ArgumentException("The ID is incorrect.");
            }

            var product = _deliverySearchHandler.GetSpecificDelivery(deliveryId[0], deliveryId[1]);
            if (product == null)
            {
                throw new ArgumentException("The delivery was not found.");
            }

            if (_orderHandler.DeliveryHasRelatedOrders(deliveryId))
            {
                _deliveryUpdateHandler.LogicDeliveryDelete(deliveryId);
            }
            else
            {
                _deliveryUpdateHandler.DeliveryDelete(deliveryId);
            }
        }

        private bool IsValidPerishableProductId(int[] productId)
        {
            if (productId == null)
            {
                return false;
            }
            else if (productId.Length != 2)
            {
                return false;
            }
            else if (productId[0] <= 0)
            {
                return false;
            }
            else if (productId[1] <= 0)
            {
                return false;
            }
            return true;
        }

    }
}