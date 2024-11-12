using backend.Domain;
using backend.Infrastructure;
using backend.Models;
using backend.Queries;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTesting
{
    public class SearchDeliveryQueryTests
    {
        private Mock<SearchDeliveryHandler> _mockDeliveryHandler;
        private Mock<SearchProductHandler> _mockProductHandler;
        private SearchDeliveryQuery _searchDeliveryQuery;

        [SetUp]
        public void Setup()
        {
            _mockDeliveryHandler = new Mock<SearchDeliveryHandler>();
            _mockProductHandler = new Mock<SearchProductHandler>();
            _searchDeliveryQuery = new SearchDeliveryQuery(_mockDeliveryHandler.Object, _mockProductHandler.Object);
        }


        [Test]
        public void GetIndividualDelivery_InvalidProductID()
        {
            // Arrange
            var searchModel = new SearchDeliveryModel { productID = 0, batchNumber = 1 };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _searchDeliveryQuery.GetIndividualDelivery(searchModel));
            Assert.AreEqual("productID has to be greater than 0.", ex.Message);
        }

        [Test]
        public void GetIndividualDelivery_InvalidBatchNumber()
        {
            // Arrange
            var searchModel = new SearchDeliveryModel { productID = 1, batchNumber = 0 };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _searchDeliveryQuery.GetIndividualDelivery(searchModel));
            Assert.AreEqual("batchNumber has to be greater than 0.", ex.Message);
        }


        [Test]
        public void GetlDeliviesFromSpecificProducts_InvalidProductID()
        {
            // Arrange
            var searchModel = new SearchProductListModel { ProductIDs = new List<int> { 1, -1, 2 } };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _searchDeliveryQuery.GetlDeliviesFromSpecificProducts(searchModel));
            Assert.AreEqual("productID has to be greater than 0.", ex.Message);
        }

        [Test]
        public void GetlDeliviesFromSpecificProducts_DuplicateProductIDs()
        {
            // Arrange
            var searchModel = new SearchProductListModel { ProductIDs = new List<int> { 1, 2, 2 } };

            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => _searchDeliveryQuery.GetlDeliviesFromSpecificProducts(searchModel));
            Assert.AreEqual("ProductIDs must be unique.", ex.Message);
        }


    }
}
