using System;
using System.Collections.Generic;
using backend.Application;
using backend.Models;
using backend.Handlers;
using backend.Domain;
using backend.Infrastructure;
using Moq;
using static System.Net.Mime.MediaTypeNames;

namespace UnitTestingLastProductsBought
{
    public class LastBoughtProductsQueryTests
    {
        private readonly Mock<IOrderedProductHandler> _mockOrderedProductHandler;
        private readonly Mock<IProductSearchHandler> _mockSearchProductHandler;
        private readonly LastBoughtProductsQuery _query;
        const string greaterThanZeroException = "User ID must be positive and greater than zero.";
        const string noProductsFounException = "No products found for the specified user.";
        const string noValidProductsException = "No valid products found.";
        public LastBoughtProductsQueryTests()
        {
            _mockOrderedProductHandler = new Mock<IOrderedProductHandler>();
            _mockSearchProductHandler = new Mock<IProductSearchHandler>();
            _query = new LastBoughtProductsQuery(_mockOrderedProductHandler.Object, _mockSearchProductHandler.Object);
        }

        [Test]
        public void GetTop10LastProductsOrdered_ShouldThrowArgumentException_WhenUserIdIsZeroOrNegative()
        {
            // Arrange
            int invalidUserId = 0;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _query.GetTop10LastProductsOrdered(invalidUserId));
            Assert.AreEqual(greaterThanZeroException, exception.Message);
        }

        [Test]
        public void GetTop10LastProductsOrdered_ShouldThrowInvalidOperationException_WhenNoProductsFound()
        {
            // Arrange
            int validUserId = 1;
            _mockOrderedProductHandler.Setup(x => x.GetTop10LastProductsOrdered(validUserId)).Returns(new List<int>());

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _query.GetTop10LastProductsOrdered(validUserId));
            Assert.AreEqual(noProductsFounException, exception.Message);
        }

        [Test]
        public void GetTop10LastProductsOrdered_ShouldReturnProducts_WhenValidUserIdAndProductsFound()
        {
            // Arrange
            int validUserId = 1;
            var orderedProductIds = new List<int> { 1, 2, 3 };
            var products = new List<GeneralProductModel>
            {
                new GeneralProductModel { ProductID = 1, Name = "Product 1" },
                new GeneralProductModel { ProductID = 2, Name = "Product 2" },
                new GeneralProductModel { ProductID = 3, Name = "Product 3" }
            };

            _mockOrderedProductHandler.Setup(x => x.GetTop10LastProductsOrdered(validUserId)).Returns(orderedProductIds);
            _mockSearchProductHandler.Setup(x => x.GetProductsByIds(orderedProductIds)).Returns(products);

            // Act
            var result = _query.GetTop10LastProductsOrdered(validUserId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Product 1", result[0].Name);
        }

        [Test]
        public void GetTop10LastProductsOrdered_ShouldThrowInvalidOperationException_WhenInvalidProductsReturned()
        {
            // Arrange
            int validUserId = 1;
            var orderedProductIds = new List<int> { 1, 2, 3 };

            _mockOrderedProductHandler.Setup(x => x.GetTop10LastProductsOrdered(validUserId)).Returns(orderedProductIds);
            _mockSearchProductHandler.Setup(x => x.GetProductsByIds(orderedProductIds)).Returns(new List<GeneralProductModel>());

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _query.GetTop10LastProductsOrdered(validUserId));
            Assert.AreEqual(noValidProductsException, exception.Message);
        }
    }
}
