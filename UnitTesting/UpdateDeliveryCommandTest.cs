using backend.Commands;
using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using backend.Models;
using Moq;


namespace UnitTesting
{
    public class UpdateDeliveryCommandTests
    {
        private Mock<UpdateDeliveryHandler> _mockUpdateDeliveryHandler;
        private Mock<SearchDeliveryHandler> _mockSearchDeliveryHandler;
        private UpdateDeliveryCommand _updateDeliveryCommand;

        [SetUp]
        public void Setup()
        {
            _mockUpdateDeliveryHandler = new Mock<UpdateDeliveryHandler>();
            _mockSearchDeliveryHandler = new Mock<SearchDeliveryHandler>();
            _updateDeliveryCommand = new UpdateDeliveryCommand(_mockUpdateDeliveryHandler.Object, _mockSearchDeliveryHandler.Object);
        }

        [Test]
        public void UpdateDelivery_CheckNullModel()
        {
            // Arrange
            UpdateDeliveryModel nullModel = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _updateDeliveryCommand.UpdateDelivery(nullModel));
            Assert.That(exception.ParamName, Is.EqualTo("updateModel"));
        }

        [Test]
        public void UpdateDelivery_CheckInvalidProductIDn()
        {
            // Arrange
            var updateModel = new UpdateDeliveryModel
            {
                ProductID = -1,
                BatchNumber = 1,
                OldBatchNumber = 0,
                ExpirationDate = DateTime.Now.AddDays(1)
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _updateDeliveryCommand.UpdateDelivery(updateModel));
            Assert.AreEqual("ProductID debe ser mayor a -1.", exception.Message);
        }

        [Test]
        public void UpdateDelivery_CheckInvalidBatchNumber()
        {
            // Arrange
            var updateModel = new UpdateDeliveryModel
            {
                ProductID = 1,
                BatchNumber = -1,
                OldBatchNumber = 0,
                ExpirationDate = DateTime.Now.AddDays(1)
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _updateDeliveryCommand.UpdateDelivery(updateModel));
            Assert.AreEqual("Número de lote debe ser mayor a -1.", exception.Message);
        }

        [Test]
        public void UpdateDelivery_CheckInvalidOldBatchNumber()
        {
            // Arrange
            var updateModel = new UpdateDeliveryModel
            {
                ProductID = 1,
                BatchNumber = 1,
                OldBatchNumber = -1,
                ExpirationDate = DateTime.Now.AddDays(1)
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _updateDeliveryCommand.UpdateDelivery(updateModel));
            Assert.AreEqual("Número antiguo de lote debe ser mayor a -1.", exception.Message);
        }

        [Test]
        public void UpdateDelivery_CheckExpirationDateInPast()
        {
            // Arrange
            var updateModel = new UpdateDeliveryModel
            {
                ProductID = 1,
                BatchNumber = 1,
                OldBatchNumber = 0,
                ExpirationDate = DateTime.Now.AddDays(-1) 
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _updateDeliveryCommand.UpdateDelivery(updateModel));
            Assert.AreEqual("Fecha de expiración debe ser en el futuro.", exception.Message);
        }


    }
}