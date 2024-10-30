using backend.Commands;
using backend.Domain;
using backend.Infrastructure;
using Moq;
using NUnit.Framework;

namespace backend.Tests.Commands
{
    public class UpdateProductCommandTests
    {
        private Mock<UpdateProductHandler> _mockUpdateProductHandler;
        private Mock<SearchProductHandler> _mockSearchProductHandler;
        private UpdateProductCommand _updateProductCommand;

        [SetUp]
        public void Setup()
        {
            _mockUpdateProductHandler = new Mock<UpdateProductHandler>();
            _mockSearchProductHandler = new Mock<SearchProductHandler>();
            _updateProductCommand = new UpdateProductCommand(_mockUpdateProductHandler.Object, _mockSearchProductHandler.Object);
        }

        [Test]
        public void UpdatePerishableProduct_CheckNullModel()
        {
            // Arrange
            UpdatePerishablProductModel nullModel = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _updateProductCommand.UpdatePerishableProduct(nullModel));
            Assert.That(exception.ParamName, Is.EqualTo("updateModel"));
        }

        [Test]
        public void UpdatePerishableProduct_CheckInvalidNameWhiteSpace()
        {
            // Arrange
            var updateModel = new UpdatePerishablProductModel
            {
                Name = "", 
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _updateProductCommand.UpdatePerishableProduct(updateModel));
            Assert.AreEqual("El nombre del producto es obligatorio.", exception.Message);
        }
        [Test]
        public void UpdatePerishableProduct_CheckInvalidNameNull()
        {
            // Arrange
            var updateModel = new UpdatePerishablProductModel
            {
                Name = null,
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _updateProductCommand.UpdatePerishableProduct(updateModel));
            Assert.AreEqual("El nombre del producto es obligatorio.", exception.Message);
        }
        [Test]
        public void UpdatePerishableProduct_CheckInvalidWeight()
        {
            // Arrange
            var updateModel = new UpdatePerishablProductModel
            {
                Name = "Pan",
                Weight = -1
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _updateProductCommand.UpdatePerishableProduct(updateModel));
            Assert.AreEqual("El peso no puede ser negativo.", exception.Message);
        }
        [Test]
        public void UpdatePerishableProduct_CheckInvalidLimit()
        {
            // Arrange
            var updateModel = new UpdatePerishablProductModel
            {
                Name = "Pan",
                Weight = 1,
                Limit= -1
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _updateProductCommand.UpdatePerishableProduct(updateModel));
            Assert.AreEqual("El limite de produccion para productos perecederos no puede ser negativo.", exception.Message);
        }
        [Test]
        public void UpdatePerishableProduct_ValidModel()
        {
            // Arrange
            var updateModel = new UpdatePerishablProductModel
            {
                Name = "pan",
                Weight = 1,
                Limit = 1,
                ProductID = 2,
                Image = "https://image.com",
                Price = 1,
                Description = "fresh",
                DeliveryDays = "Lunes"

            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _updateProductCommand.UpdatePerishableProduct(updateModel));
            Assert.AreEqual("No perishable product was updated. Please check the provided Product ID.", exception.Message);
        }

        [Test]
        public void UpdateNonPerishableProduct_CheckNullModel()
        {   
            // Arrange
            UpdateNonPerishableProductModel nullModel = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _updateProductCommand.UpdateNonPerishableProduct(nullModel));
            Assert.That(exception.ParamName, Is.EqualTo("updateModel"));
        }


        [Test]
        public void UpdateNonPerishableProduct_CheckInvalidNameWitheSpace()
        {
            // Arrange
            var updateModel = new UpdateNonPerishableProductModel
            {
                Name = "",

            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _updateProductCommand.UpdateNonPerishableProduct(updateModel));
            Assert.AreEqual("El nombre del producto es obligatorio.", exception.Message);
        }
        [Test]
        public void UpdateNonPerishableProduct_CheckInvalidNameNull()
        {
            // Arrange
            var updateModel = new UpdateNonPerishableProductModel
            {
                Name = null,

            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _updateProductCommand.UpdateNonPerishableProduct(updateModel));
            Assert.AreEqual("El nombre del producto es obligatorio.", exception.Message);
        }
        [Test]
        public void UpdateNonPerishableProduct_CheckInvalidWeight()
        {
            // Arrange
            var updateModel = new UpdateNonPerishableProductModel
            {
                Name = "computadora",
                Weight = -1

            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _updateProductCommand.UpdateNonPerishableProduct(updateModel));
            Assert.AreEqual("El peso debe ser mayor que cero.", exception.Message);
        }
        [Test]
        public void UpdateNonPerishableProduct_CheckInvalidStock()
        {
            // Arrange
            var updateModel = new UpdateNonPerishableProductModel
            {
                Name = "computadora",
                Weight = 1,
                Stock = -1

            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _updateProductCommand.UpdateNonPerishableProduct(updateModel));
            Assert.AreEqual("El stock para productos no perecederos no puede ser negativo.", exception.Message);
        }
        [Test]
        public void UpdateNonPerishableProduct_ValidModel()
        {
            // Arrange
            var updateModel = new UpdateNonPerishableProductModel
            {
                Name = "computadora",
                Weight = 1,
                Stock = 1,
                ProductID = 1,
                Image = "https://image.com",
                Price = 1,
                Description = "gamer"

            };

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _updateProductCommand.UpdateNonPerishableProduct(updateModel));
            Assert.AreEqual("No non-perishable product was updated. Please check the provided Product ID.", exception.Message);
        }




    }
}

