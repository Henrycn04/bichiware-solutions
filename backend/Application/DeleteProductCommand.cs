using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using System;

namespace backend.Commands
{
    public class DeleteProductCommand
    {
        private readonly UpdateProductHandler _productUpdateHandler;
        private readonly SearchProductHandler _productSearchHandler;
        private readonly OrdersHandler _orderHandler;

        public DeleteProductCommand(UpdateProductHandler productUpdateHandler, SearchProductHandler productSearchHandler, OrdersHandler orderHandler)
        {
            _productUpdateHandler = productUpdateHandler;
            _productSearchHandler = productSearchHandler;
            _orderHandler = orderHandler;
        }

        public void DeletePerishableProduct(int productId)
        {
            if (!IsValidPerishableProductId(productId))
            {
                throw new ArgumentException("The ID has to be positive and even.");
            }

            var product = _productSearchHandler.GetSpecificProduct(productId);
            if (product == null)
            {
                throw new ArgumentException("The product was not found.");
            }

            if (_orderHandler.PerishableHasRelatedOrders(productId))
            {
                _productUpdateHandler.LogicPerishableProductDelete(productId);
            }
            else
            {
                _productUpdateHandler.PerishableProductDelete(productId);
            }
        }

        public void DeleteNonPerishableProduct(int productId)
        {
            if (!IsValidNonPerishableProductId(productId))
            {
                throw new ArgumentException("The ID has to be positive and odd");
            }
            var product = _productSearchHandler.GetSpecificProduct(productId);
            if (product == null)
            {
                throw new ArgumentException("The product was not found.");
            }



            if (_orderHandler.NonPerishableHasRelatedOrders(productId))
            {
                _productUpdateHandler.LogicNonPerishableProductDelete(productId);
            }
            else
            {
                _productUpdateHandler.NonPerishableProductDelete(productId);
            }
        }

        private bool IsValidPerishableProductId(int productId)
        {
            return productId > 0 && productId % 2 == 0;
        }

        private bool IsValidNonPerishableProductId(int productId)
        {
            return productId > 0 && productId % 2 != 0;
        }

    }
}