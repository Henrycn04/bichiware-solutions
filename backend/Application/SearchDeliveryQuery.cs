
using backend.Domain;
using backend.Infrastructure;
using backend.Models;



namespace backend.Queries
{
    public class SearchDeliveryQuery
    {
        private readonly SearchDeliveryHandler _deliveryHandler;

        public SearchDeliveryQuery(SearchDeliveryHandler deliveryHandler)
        {
            _deliveryHandler = deliveryHandler;
        }


        public AddDeliveryModel GetIndividualDelivery(SearchDeliveryModel searchModel)
        {
            if (searchModel.productID <= 0)
            {
                throw new ArgumentException("productID has to be greater than 0.");
            }

            if (searchModel.batchNumber <= 0)
            {
                throw new ArgumentException("batchNumber has to be greater than 0.");
            }
            var delivery = _deliveryHandler.GetSpecificDelivery(searchModel.productID, searchModel.batchNumber);

            if (delivery == null)
            {
                throw new InvalidOperationException("Delivery not found.");
            }

            return delivery;
        }


    }
}