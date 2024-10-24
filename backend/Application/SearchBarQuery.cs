using System.Globalization;
using System.Linq;
using System.Text;
using backend.Domain;
using backend.Handlers;
using backend.Models;

namespace backend.Queries
{
    public class SearchBarQuery
    {
        private readonly ProductosHandler _productsHandler;

        public SearchBarQuery(ProductosHandler productsHandler)
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
                    WordMatches(RemoveDiacritics(p.ProductName), searchTerms) ||
                    WordMatches(RemoveDiacritics(p.Category), searchTerms) ||
                    WordMatches(RemoveDiacritics(p.CompanyName), searchTerms))
                .ToList();

            return exactMatches;
        }

        public List<PerishableProductModel> SearchPerishableProduct(SearchBarModel searchModel)
        {
            var products = _productsHandler.GetPerishableProducts();
            var searchTermNormalized = RemoveDiacritics(searchModel.SearchTerm);
            var searchTerms = searchTermNormalized.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var exactMatches = products
                .Where(p =>
                    WordMatches(RemoveDiacritics(p.ProductName), searchTerms) ||
                    WordMatches(RemoveDiacritics(p.Category), searchTerms) ||
                    WordMatches(RemoveDiacritics(p.CompanyName), searchTerms))
                .ToList();

            return exactMatches;
        }

        // Método auxiliar para comparar palabras completas
        private bool WordMatches(string text, string[] searchTerms)
        {
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return searchTerms.All(term => words.Contains(term, StringComparer.OrdinalIgnoreCase));
        }

        public List<AddDeliveryModel> getProductDeliveries(SearchBarModel searchModel)
        {
            var deliveries = _productsHandler.GetDeliveries(searchModel.SearchTerm);

            return deliveries.ToList();
        }
    }
}