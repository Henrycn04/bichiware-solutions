using backend.Commands;
using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using Moq;
using System;

namespace UnitTestingDeleteDelivery
{
    public class DeleteDeliveryCommandTests
    {
        private readonly Mock<IUpdateDeliveryHandler> _mockUpdateDeliveryHandler;
        private readonly Mock<ISearchDeliveryHandler> _mockSearchDeliveryHandler;
        private readonly Mock<IOrdersHandler> _mockOrdersHandler;
        private readonly DeleteDeliveryCommand _deleteDeliveryCommand;
        const string invalidIDException = "The ID is incorrect.";
        const string invalidDeliveryException = "The delivery was not found.";

        public DeleteDeliveryCommandTests()
        {
            _mockUpdateDeliveryHandler = new Mock<IUpdateDeliveryHandler>();
            _mockSearchDeliveryHandler = new Mock<ISearchDeliveryHandler>();
            _mockOrdersHandler = new Mock<IOrdersHandler>();

            _deleteDeliveryCommand = new DeleteDeliveryCommand(
                _mockUpdateDeliveryHandler.Object,
                _mockSearchDeliveryHandler.Object,
                _mockOrdersHandler.Object
            );
        }

        [Test]
        public void DeleteDelivery_InvalidProductId_ThrowsArgumentException()
        {
            // Arrange
            var invalidDelivery = new SearchDeliveryModel { productID = -1, batchNumber = 1 };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _deleteDeliveryCommand.DeleteDelivery(invalidDelivery));
            Assert.AreEqual(invalidIDException, exception.Message);
        }

        [Test]
        public void DeleteDelivery_InvalidBatchNumber_ThrowsArgumentException()
        {
            // Arrange
            var invalidDelivery = new SearchDeliveryModel { productID = 1, batchNumber = -1 };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _deleteDeliveryCommand.DeleteDelivery(invalidDelivery));
            Assert.AreEqual(invalidIDException, exception.Message);
        }

        [Test]
        public void DeleteDelivery_DeliveryNotFound_ThrowsArgumentException()
        {
            // Arrange
            var validDelivery = new SearchDeliveryModel { productID = 1, batchNumber = 1 };
            _mockSearchDeliveryHandler
                .Setup(x => x.GetSpecificDelivery(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((backend.Models.AddDeliveryModel)null);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _deleteDeliveryCommand.DeleteDelivery(validDelivery));
            Assert.AreEqual(invalidDeliveryException, exception.Message);
        }

        [Test]
        public void DeleteDelivery_DeliveryHasRelatedOrders_PerformsLogicalDelete()
        {
            // Arrange
            var validDelivery = new SearchDeliveryModel { productID = 1, batchNumber = 1 };
            var deliveryId = new[] { validDelivery.productID, validDelivery.batchNumber };

            _mockSearchDeliveryHandler
                .Setup(x => x.GetSpecificDelivery(deliveryId[0], deliveryId[1]))
                .Returns(new backend.Models.AddDeliveryModel());
            _mockOrdersHandler
                .Setup(x => x.DeliveryHasRelatedOrders(deliveryId))
                .Returns(true);

            // Act
            _deleteDeliveryCommand.DeleteDelivery(validDelivery);

            // Assert
            _mockUpdateDeliveryHandler.Verify(x => x.LogicDeliveryDelete(deliveryId), Times.Once);
        }

        [Test]
        public void DeleteDelivery_NoRelatedOrders_PerformsPhysicalDelete()
        {
            // Arrange
            var validDelivery = new SearchDeliveryModel { productID = 1, batchNumber = 1 };
            var deliveryId = new[] { validDelivery.productID, validDelivery.batchNumber };

            _mockSearchDeliveryHandler
                .Setup(x => x.GetSpecificDelivery(deliveryId[0], deliveryId[1]))
                .Returns(new backend.Models.AddDeliveryModel());
            _mockOrdersHandler
                .Setup(x => x.DeliveryHasRelatedOrders(deliveryId))
                .Returns(false);

            // Act
            _deleteDeliveryCommand.DeleteDelivery(validDelivery);

            // Assert
            _mockUpdateDeliveryHandler.Verify(x => x.DeliveryDelete(deliveryId), Times.Once);
        }
    }
}