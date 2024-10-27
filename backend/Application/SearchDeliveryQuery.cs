
using backend.Domain;
using backend.Infrastructure;
using backend.Models;



namespace backend.Queries
{
    public class SearchDeliveryQuery
    {
        private readonly SearchDeliveryHandler _deliveryHandler;
        private readonly SearchProductHandler _productHandler;

        public SearchDeliveryQuery(SearchDeliveryHandler deliveryHandler, SearchProductHandler productHandler)
        {
            _deliveryHandler = deliveryHandler;
            _productHandler = productHandler;
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

        public List<AddDeliveryModel> GetlDeliviesFromSpecificProducts(SearchProductListModel searchModel)
        {
            ValidateProductIDs(searchModel);
            CheckForDuplicates(searchModel);
            CheckProductExistence(searchModel);

            var deliveries = _deliveryHandler.GetDeliveriesFromSpecificProducts(searchModel);

            if (deliveries == null)
            {
                throw new InvalidOperationException("Delivery not found.");
            }

            return deliveries;
        }

        private void ValidateProductIDs(SearchProductListModel searchModel)
        {
            foreach (var id in searchModel.ProductIDs)
            {
                if (id <= 0)
                {
                    throw new ArgumentException("productID has to be greater than 0.");
                }
            }
        }

        private void CheckForDuplicates(SearchProductListModel searchModel)
        {
            if (searchModel.ProductIDs.Distinct().Count() != searchModel.ProductIDs.Count)
            {
                throw new ArgumentException("ProductIDs must be unique.");
            }
        }

        private void CheckProductExistence(SearchProductListModel searchModel)
        {
            var existingProductIDs = _productHandler.GetProductsByIds(searchModel.ProductIDs);
            if (existingProductIDs.Count == 0)
            {
                throw new InvalidOperationException("No valid product IDs found.");
            }
        }

    }
}