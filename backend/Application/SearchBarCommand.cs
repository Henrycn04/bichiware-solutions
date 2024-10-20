using System.Globalization;
using System.Linq;
using System.Text;
using backend.Domain;
using backend.Handlers;
using backend.Models;

namespace backend.Commands
{
    public class SearchBarCommand
    {
        private readonly ProductosHandler _productsHandler;

        public SearchBarCommand(ProductosHandler productsHandler)
        {
            _productsHandler = productsHandler;
        }

        private static string RemoveDiacritics(string text)
        {   if (text != null)
            {
                return string.Concat(
                    text.Normalize(NormalizationForm.FormD)
                        .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                ).Normalize(NormalizationForm.FormC);
            }
            return "";
        }

        public List<NonPerishableProductModel> SearchNonPerishableProduct(SearchBarModel searchModel)
        {
            var products = _productsHandler.GetNonPerishableProducts();
            var searchTermNormalized = RemoveDiacritics(searchModel.SearchTerm);
            var searchTerms = searchTermNormalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var exactMatches = products
                .Where(p =>
                    RemoveDiacritics(p.ProductName).Equals(searchTermNormalized, StringComparison.OrdinalIgnoreCase) ||
                    RemoveDiacritics(p.Category).Equals(searchTermNormalized, StringComparison.OrdinalIgnoreCase) ||
                    RemoveDiacritics(p.CompanyName).Equals(searchTermNormalized, StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (exactMatches.Any())
            {
                return exactMatches;
            }

            return products
                .Where(p =>
                    searchTerms.Any(term =>
                        RemoveDiacritics(p.ProductName).Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        RemoveDiacritics(p.Category).Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        RemoveDiacritics(p.CompanyName).Contains(term, StringComparison.OrdinalIgnoreCase))
                )
                .ToList();
        }
        public List<PerishableProductModel> SearchPerishableProduct(SearchBarModel searchModel)
{
    var products = _productsHandler.GetPerishableProducts();
    var searchTermNormalized = RemoveDiacritics(searchModel.SearchTerm);
    var searchTerms = searchTermNormalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    var exactMatches = products
        .Where(p =>
            RemoveDiacritics(p.ProductName).Equals(searchTermNormalized, StringComparison.OrdinalIgnoreCase) ||
            RemoveDiacritics(p.Category).Equals(searchTermNormalized, StringComparison.OrdinalIgnoreCase) ||
            RemoveDiacritics(p.CompanyName).Equals(searchTermNormalized, StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (exactMatches.Any())
    {
        return exactMatches;
    }

    return products
        .Where(p =>
            searchTerms.Any(term =>
                RemoveDiacritics(p.ProductName).Contains(term, StringComparison.OrdinalIgnoreCase) ||
                RemoveDiacritics(p.Category).Contains(term, StringComparison.OrdinalIgnoreCase) ||
                RemoveDiacritics(p.CompanyName).Contains(term, StringComparison.OrdinalIgnoreCase))
        )
        .ToList();
}

        public List<AddDeliveryModel> getProductDeliveries(SearchBarModel searchModel)
        {
            var deliveries = _productsHandler.GetDeliveries(searchModel.SearchTerm);

            return deliveries.ToList();
        }
    }
}