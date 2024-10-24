
using backend.Domain;
using backend.Infrastructure;
using backend.Models;

namespace backend.Application
{
    public class SearchProductQuery
    {
        private readonly SearchProductHandler _productHandler;

        public SearchProductQuery(SearchProductHandler productHandler)
        {
            _productHandler = productHandler;
        }


        public GeneralProductModel GetIndividualDelivery(SearcProductModel searchModel)
        {
            if (searchModel.ID <= 0)
            {
                throw new ArgumentException("productID has to be greater than 0.");
            }
            var product = _productHandler.GetSpecificProduct(searchModel.ID);

            if (product == null)
            {
                throw new InvalidOperationException("Delivery not found.");
            }

            return product;
        }


    }
}
