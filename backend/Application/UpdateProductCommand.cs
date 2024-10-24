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
    public class UpdateProductCommand
    {
        private readonly UpdateProductHandler _productUpdateHandler;
        private readonly SearchProductHandler _productSearchHandler;

        public UpdateProductCommand(UpdateProductHandler productUpdateHandler, SearchProductHandler productSearchHandler)
        {
            _productUpdateHandler = productUpdateHandler;
            _productSearchHandler = productSearchHandler;
        }
        // the next exceptions are going to be presented to the user thats why they are in spanish

        public void UpdatePerishableProduct(UpdatePerishablProductModel updateModel) {
            if (updateModel == null)
            {
                throw new ArgumentNullException(nameof(updateModel), "El modelo de producto perecedero no puede ser nulo.");
            }

            ValidatePerishableProduct(updateModel);
            _productUpdateHandler.UpdatePerishableProduct(updateModel);
          
        }
        public void UpdateNonPerishableProduct(UpdateNonPerishableProductModel updateModel) {
            if (updateModel == null)
            {
                throw new ArgumentNullException(nameof(updateModel), "El modelo de producto perecedero no puede ser nulo.");
            }

            ValidateNonPerishableProduct(updateModel);
            _productUpdateHandler.UpdateNonPerishableProduct(updateModel);
          
            }
      
        private void ValidatePerishableProduct(UpdatePerishablProductModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("El nombre del producto es obligatorio.");
            }

            ValidateWeight(model.Weight);
            ValidateStock(model.Limit);
        }

        private void ValidateNonPerishableProduct(UpdateNonPerishableProductModel model)
        {
       
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("El nombre del producto es obligatorio.");
            }

            ValidateWeight(model.Weight, isPerishable: false); 
            ValidateStock(model.Stock, isPerishable: false); 
        }

        private void ValidateWeight(decimal weight, bool isPerishable = true)
        {
            if (isPerishable && weight < 0)
            {
                throw new ArgumentException("El peso no puede ser negativo.");
            }
            else if (!isPerishable && weight <= 0)
            {
                throw new ArgumentException("El peso debe ser mayor que cero.");
            }
        }

        private void ValidateStock(int stock, bool isPerishable = true)
        {
            if (isPerishable && stock < 0)
            {
                throw new ArgumentException("El limite de produccion para productos perecederos no puede ser negativo.");
            }
            else if (!isPerishable && stock < 0)
            {
                throw new ArgumentException("El stock para productos no perecederos no puede ser negativo.");
            }
        }

      
    }
}