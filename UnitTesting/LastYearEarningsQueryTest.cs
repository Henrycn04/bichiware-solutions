using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Application;
using backend.Domain;
using backend.Infrastructure;
using Moq;
using NUnit.Framework;

namespace UnitTesting
{
    public class LastYearEarningsQueryTest
    {
        private Mock<ILastYearEarningsHandler> _handlerMock;
        private LastYearEarningsQuery _query;

        [SetUp]
        public void SetUp()
        {
            _handlerMock = new Mock<ILastYearEarningsHandler>();
            _query = new LastYearEarningsQuery(_handlerMock.Object);
        }

        [Test]
        public void Execute_ThrowsArgumentException_WhenUserDoesNotExist()
        {
            // Arrange
            var filter = new LastYearEarningsElementsModel { UserID = 1 };
            _handlerMock.Setup(h => h.UserExists(filter.UserID)).ReturnsAsync(false);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _query.Execute(filter));
            Assert.AreEqual("UserID does not exist.", exception.Message);
        }

        [Test]
        public void Execute_ThrowsUnauthorizedAccessException_WhenUserNotAuthorized()
        {
            // Arrange
            var filter = new LastYearEarningsElementsModel { UserID = 1 };
            _handlerMock.Setup(h => h.UserExists(filter.UserID)).ReturnsAsync(true);
            _handlerMock.Setup(h => h.UserIsAdminOrEntrepeneur(filter.UserID)).ReturnsAsync(1);

            // Act & Assert
            var exception = Assert.ThrowsAsync<UnauthorizedAccessException>(() => _query.Execute(filter));
            Assert.AreEqual("User is not authorized to view orders.", exception.Message);
        }

        [Test]
        public void Execute_ThrowsArgumentException_WhenCompanyDoesNotExist_ForAdmin()
        {
            // Arrange
            var filter = new LastYearEarningsElementsModel { UserID = 1, CompanyID = 100 };
            _handlerMock.Setup(h => h.UserExists(filter.UserID)).ReturnsAsync(true);
            _handlerMock.Setup(h => h.UserIsAdminOrEntrepeneur(filter.UserID)).ReturnsAsync(2);
            _handlerMock.Setup(h => h.CompanyExists(filter.CompanyID.Value)).ReturnsAsync(false);

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(() => _query.Execute(filter));
            Assert.AreEqual("CompanyID does not exist.", exception.Message);
        }

        [Test]
        public async Task Execute_ReturnsOrders_WhenFiltersAreValid()
        {
            // Arrange
            var filter = new LastYearEarningsElementsModel { UserID = 1, CompanyID = 100 };
            var expectedOrders = new List<LastYearOrdersModel>
            {
                new LastYearOrdersModel { OrderID = 1, Total = 500 },
                new LastYearOrdersModel { OrderID = 2, Total = 1000 }
            };

            _handlerMock.Setup(h => h.UserExists(filter.UserID)).ReturnsAsync(true);
            _handlerMock.Setup(h => h.UserIsAdminOrEntrepeneur(filter.UserID)).ReturnsAsync(2);
            _handlerMock.Setup(h => h.CompanyExists(filter.CompanyID.Value)).ReturnsAsync(true);
            _handlerMock.Setup(h => h.GetOrdersByFilterAsync(filter)).ReturnsAsync(expectedOrders);

            // Act
            var result = await _query.Execute(filter);

            // Assert
            Assert.AreEqual(expectedOrders.Count, result.Count);
            Assert.AreEqual(expectedOrders[0].OrderID, result[0].OrderID);
            Assert.AreEqual(expectedOrders[1].Total, result[1].Total);
        }
    }
}
