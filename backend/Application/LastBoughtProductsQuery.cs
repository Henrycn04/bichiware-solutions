using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.Application
{
    public class LastBoughtProductsQuery
    {
        private readonly IOrderedProductHandler _orderedProductHandler;
        private readonly IProductSearchHandler _searchProductHandler;

        public LastBoughtProductsQuery(IOrderedProductHandler orderedProductHandler, IProductSearchHandler searchProductHandler)
        {
            _orderedProductHandler = orderedProductHandler;
            _searchProductHandler = searchProductHandler;
        }

        public List<GeneralProductModel> GetTop10LastProductsOrdered(int userID)
        {
            ValidateUserID(userID);

            var orderedProductIds = _orderedProductHandler.GetTop10LastProductsOrdered(userID);

            if (orderedProductIds == null || orderedProductIds.Count == 0)
            {
                throw new InvalidOperationException("No products found for the specified user.");
            }
            Console.WriteLine(orderedProductIds[0]);
            var products = _searchProductHandler.GetProductsByIds(orderedProductIds);

            ValidateProducts(products);

            return products;
        }

        private void ValidateUserID(int userID)
        {
            if (userID <= 0)
            {
                throw new ArgumentException("User ID must be positive and greater than zero.");
            }
        }

        private void ValidateProducts(List<GeneralProductModel> products)
        {
            if (products == null || products.Count == 0)
            {
                throw new InvalidOperationException("No valid products found.");
            }
        }
    }
}