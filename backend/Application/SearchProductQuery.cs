
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


        public GeneralProductModel GetIndividualProduct(SearcProductModel searchModel)
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

        public List<GeneralProductModel> GetSpecificProductList(SearchProductListModel searchModel)
        {
            ValidateProductIDs(searchModel);
            CheckForDuplicates(searchModel);
            CheckValidProductIDs(searchModel);

            var products = _productHandler.GetProductsByIds(searchModel.ProductIDs);

            if (products == null)
            {
                throw new InvalidOperationException("Products not found.");
            }

            return products;
        }

        private void ValidateProductIDs(SearchProductListModel searchModel)
        {
            if (searchModel.ProductIDs == null || !searchModel.ProductIDs.Any())
            {
                throw new ArgumentException("ProductIDs cannot be null or empty.");
            }
        }

        private void CheckValidProductIDs(SearchProductListModel searchModel)
        {
            if (searchModel.ProductIDs.Any(id => id <= 0))
            {
                throw new ArgumentException("All product IDs must be greater than 0.");
            }
        }

        private void CheckForDuplicates(SearchProductListModel searchModel)
        {
            var duplicateIds = searchModel.ProductIDs.GroupBy(id => id)
                                                     .Where(group => group.Count() > 1)
                                                     .Select(group => group.Key)
                                                     .ToList();

            if (duplicateIds.Any())
            {
                throw new InvalidOperationException($"Duplicate product IDs found: {string.Join(", ", duplicateIds)}.");
            }
        }

        public List<GeneralProductModel> GetProductsForShowcase()
        {
            return this._productHandler.GetRandomProductsForShowcase();
        }
    }
}
