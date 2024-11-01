using backend.Application;
using backend.Commands;
using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using backend.Models;
using Moq;


namespace UnitTesting
{
    public class SearchProductQueryTest
    {
        private Mock<SearchProductHandler> _mockSearchProductHandler;
        private SearchProductQuery _searchProductQuery;

        [SetUp]
        public void Setup()
        {
            _mockSearchProductHandler = new Mock<SearchProductHandler>();
            _searchProductQuery = new SearchProductQuery(_mockSearchProductHandler.Object);
        }

        [Test]
        public void SearchProduct_CheckInvalidProductID()
        {
            // Arrange
            var updateModel = new SearcProductModel
            {
                ID = -1,
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _searchProductQuery.GetIndividualProduct(updateModel));
            Assert.AreEqual("productID has to be greater than 0.", exception.Message);
        }

        [Test]
        public void GetSpecificProductList_NullOrEmptyProductIDs()
        {
            // Arrange
            var searchModel = new SearchProductListModel { ProductIDs = null };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _searchProductQuery.GetSpecificProductList(searchModel));
            Assert.AreEqual("ProductIDs cannot be null or empty.", exception.Message);

            searchModel.ProductIDs = new List<int>();
            exception = Assert.Throws<ArgumentException>(() => _searchProductQuery.GetSpecificProductList(searchModel));
            Assert.AreEqual("ProductIDs cannot be null or empty.", exception.Message);
        }

        [Test]
        public void GetSpecificProductList_InvalidProductIDs()
        {
            // Arrange
            var searchModel = new SearchProductListModel
            {
                ProductIDs = new List<int> { 1, -2, 3 }
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _searchProductQuery.GetSpecificProductList(searchModel));
            Assert.AreEqual("All product IDs must be greater than 0.", exception.Message);
        }

        [Test]
        public void GetSpecificProductList_DuplicateProductIDs()
        {
            // Arrange
            var searchModel = new SearchProductListModel
            {
                ProductIDs = new List<int> { 1, 2, 2 }
            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _searchProductQuery.GetSpecificProductList(searchModel));
            Assert.AreEqual("Duplicate product IDs found: 2.", exception.Message);
        }

    }
}