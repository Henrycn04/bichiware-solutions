using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Commands;
using backend.Infrastructure;
using backend.Models;

namespace UnitTesting
{
    public class GetOrdersQueryTests
    {
        private Mock<GetOrdersHandler> _mockHandler;
        private GetOrdersQuery _getOrdersQuery;

        [SetUp]
        public void Setup()
        {
            _mockHandler = new Mock<GetOrdersHandler>();
            _getOrdersQuery = new GetOrdersQuery(_mockHandler.Object);  // Inyectando el mock del handler
        }

        [Test]
        public async Task Execute_UserDoesNotExist_ThrowsArgumentException()
        {
            // Arrange
            var userId = -1;
            // Act & Assert
            var ex = Assert.ThrowsAsync<ArgumentException>(async () => await _getOrdersQuery.Execute(userId));
            Assert.AreEqual("UserID doesnt exist.", ex.Message);
        }


    }
}
