using Moq;
using backend.Commands;
using backend.Handlers;
using backend.Domain;
using System;
using backend.Infrastructure;

namespace UnitTestingDeleteProduct
{
    public class DeleteProductCommandTests
    {
        private readonly Mock<IUpdateProductHandler> _mockProductUpdateHandler;
        private readonly Mock<IProductSearchHandler> _mockProductSearchHandler;
        private readonly Mock<IOrdersHandler> _mockOrderHandler;
        private readonly DeleteProductCommand _deleteProductCommand;
        const string perishableProductException = "The ID has to be positive and even";
        const string nonPerishableProductException = "The ID has to be positive and odd";
        const string notFoundException = "The product was not found.";

        public DeleteProductCommandTests()
        {
            _mockProductUpdateHandler = new Mock<IUpdateProductHandler>();
            _mockProductSearchHandler = new Mock<IProductSearchHandler>();
            _mockOrderHandler = new Mock<IOrdersHandler>();
            _deleteProductCommand = new DeleteProductCommand(
                _mockProductUpdateHandler.Object,
                _mockProductSearchHandler.Object,
                _mockOrderHandler.Object
            );
        }

        [Test]
        public void DeletePerishableProduct_InvalidOddId_ThrowsArgumentException()
        {
            // Arrange
            int invalidProductId = 3;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _deleteProductCommand.DeletePerishableProduct(invalidProductId));
            Assert.AreEqual(perishableProductException, exception.Message);
        }

        [Test]
        public void DeleteNonPerishableProduct_InvalidEvenId_ThrowsArgumentException()
        {
            // Arrange
            int invalidProductId = 2; 

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _deleteProductCommand.DeleteNonPerishableProduct(invalidProductId));
            Assert.AreEqual(nonPerishableProductException, exception.Message);
        }
        [Test]
        public void DeletePerishableProduct_InvalidNegativeId_ThrowsArgumentException()
        {
            // Arrange
            int invalidProductId = -1;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _deleteProductCommand.DeletePerishableProduct(invalidProductId));
            Assert.AreEqual(perishableProductException, exception.Message);
        }

        [Test]
        public void DeleteNonPerishableProduct_InvalidNegativeId_ThrowsArgumentException()
        {
            // Arrange
            int invalidProductId = -1;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _deleteProductCommand.DeleteNonPerishableProduct(invalidProductId));
            Assert.AreEqual(nonPerishableProductException, exception.Message);
        }
        [Test]
        public void DeletePerishableProduct_InvalidZeroId_ThrowsArgumentException()
        {
            // Arrange
            int invalidProductId = 0;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _deleteProductCommand.DeletePerishableProduct(invalidProductId));
            Assert.AreEqual(perishableProductException, exception.Message);
        }

        [Test]
        public void DeleteNonPerishableProduct_InvalidZeroId_ThrowsArgumentException()
        {
            // Arrange
            int invalidProductId = 0;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _deleteProductCommand.DeleteNonPerishableProduct(invalidProductId));
            Assert.AreEqual(nonPerishableProductException, exception.Message);
        }

        [Test]
        public void DeletePerishableProduct_ProductNotFound_ThrowsArgumentException()
        {
            // Arrange
            int validProductId = 4; 
            _mockProductSearchHandler.Setup(x => x.GetSpecificProduct(validProductId)).Returns((GeneralProductModel)null);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _deleteProductCommand.DeletePerishableProduct(validProductId));
            Assert.AreEqual(notFoundException, exception.Message);
        }
        [Test]
        public void DeleteNonPerishableProduct_ProductNotFound_ThrowsArgumentException()
        {
            // Arrange
            int validProductId = 3;
            _mockProductSearchHandler.Setup(x => x.GetSpecificProduct(validProductId)).Returns((GeneralProductModel)null);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _deleteProductCommand.DeleteNonPerishableProduct(validProductId));
            Assert.AreEqual(notFoundException, exception.Message);
        }

        [Test]
        public void DeletePerishableProduct_WithRelatedOrders_CallsLogicPerishableProductDelete()
        {
            // Arrange
            int validProductId = 4; 
            var product = new GeneralProductModel { ProductID = validProductId };
            _mockProductSearchHandler.Setup(x => x.GetSpecificProduct(validProductId)).Returns(product);
            _mockOrderHandler.Setup(x => x.PerishableHasRelatedOrders(validProductId)).Returns(true); 

            // Act
            _deleteProductCommand.DeletePerishableProduct(validProductId);

            // Assert
            _mockProductUpdateHandler.Verify(x => x.LogicPerishableProductDelete(validProductId), Times.Once);
        }
        [Test]
        public void DeleteNonPerishableProduct_WithRelatedOrders_CallsLogicNonPerishableProductDelete()
        {
            // Arrange
            int validProductId = 3; 
            var product = new GeneralProductModel { ProductID = validProductId };
            _mockProductSearchHandler.Setup(x => x.GetSpecificProduct(validProductId)).Returns(product);
            _mockOrderHandler.Setup(x => x.NonPerishableHasRelatedOrders(validProductId)).Returns(true);

            // Act
            _deleteProductCommand.DeleteNonPerishableProduct(validProductId);

            // Assert
            _mockProductUpdateHandler.Verify(x => x.LogicNonPerishableProductDelete(validProductId), Times.Once);
        }

        [Test]
        public void DeleteNonPerishableProduct_WithNoRelatedOrders_CallsNonPerishableProductDelete()
        {
            // Arrange
            int validProductId = 3;
            var product = new GeneralProductModel { ProductID = validProductId };
            _mockProductSearchHandler.Setup(x => x.GetSpecificProduct(validProductId)).Returns(product);
            _mockOrderHandler.Setup(x => x.NonPerishableHasRelatedOrders(validProductId)).Returns(false); 

            // Act
            _deleteProductCommand.DeleteNonPerishableProduct(validProductId);

            // Assert
            _mockProductUpdateHandler.Verify(x => x.NonPerishableProductDelete(validProductId), Times.Once);
        }

        [Test]
        public void DeletePerishableProduct_WithNoRelatedOrders_CallsPerishableProductDelete()
        {
            // Arrange
            int validProductId = 4; 
            var product = new GeneralProductModel { ProductID = validProductId };
            _mockProductSearchHandler.Setup(x => x.GetSpecificProduct(validProductId)).Returns(product);
            _mockOrderHandler.Setup(x => x.PerishableHasRelatedOrders(validProductId)).Returns(false);

            // Act
            _deleteProductCommand.DeletePerishableProduct(validProductId);

            // Assert
            _mockProductUpdateHandler.Verify(x => x.PerishableProductDelete(validProductId), Times.Once);
        }
    }
}