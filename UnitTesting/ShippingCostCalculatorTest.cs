using backend.Models;
using backend.Infrastructure;
using backend.Domain;

namespace UnitTesting
{
    internal class ShippingCostCalculatorTest
    {
        private ShippingCostCalculator calculator;

        [SetUp]
        public void Setup()
        {
            calculator = new ShippingCostCalculator();
        }

        [Test]
        public void CalculateShippingNegativeMassTest()
        {
            // Arrange
            PhysicalAddress address = new PhysicalAddress()
            {
                lat = 9.93135,
                lon = -84.05056
            };
            double mass = -8;
            // Act
            double cost = calculator.CalculateShippingCost(address, mass);
            // Assert
            Assert.AreEqual(cost, 0);
        }

        [Test]
        public void CalculateShippingUnrealisticAddressTest()
        {
            // Arrange
            PhysicalAddress address = new PhysicalAddress()
            {
                lat = 498590.782348,
                lon = -843424.05056
            };
            double mass = 1;
            // Act
            double cost = calculator.CalculateShippingCost(address, mass);
            // Assert
            Assert.AreEqual(cost, 0);
        }

        [Test]
        public void CalculateShippingAddressIsNullTest()
        {
            // Arrange
            PhysicalAddress address = null;
            double mass = 1;
            // Act
            double cost = calculator.CalculateShippingCost(address, mass);
            // Assert
            Assert.AreEqual(cost, 0);
        }

        [Test]
        public void CalculateShippingMassIsTooSmallTest()
        {
            // Arrange
            PhysicalAddress address = new PhysicalAddress()
            {
                lat = 33.55513,
                lon = 53.09692
            };
            double mass = 0.00000000001;
            // Act
            double cost = calculator.CalculateShippingCost(address, mass);
            // Assert
            Assert.AreEqual(cost, 3000);
        }

        [Test]
        public void SumOrderProductsNegativeOrderTest()
        {
            // Arrange
            int orderId = -1;
            // Act
            double weight = calculator.SumOrderProductsWeight(orderId);
            // Assert
            Assert.AreEqual(weight, 0);
        }

        [Test]
        public void SumOrderProductsUnexistentOrderTest()
        {
            // Arrange
            int orderId = 83489494;
            // Act
            double weight = calculator.SumOrderProductsWeight(orderId);
            // Assert
            Assert.AreEqual(weight, 0);
        }

        [Test]
        public void SumOrderProductsBoundaryTest()
        {
            // Arrange
            int orderId = 2147483647;
            // Act
            double weight = calculator.SumOrderProductsWeight(orderId);
            // Assert
            Assert.AreEqual(weight, 0);
        }

        [Test]
        public void SumOrderNormalOrderTest()
        {
            // Arrange
            int orderId = 1;
            // Act
            double weight = calculator.SumOrderProductsWeight(orderId);
            // Assert
            Assert.Greater(weight, 0);
        }

        [Test]
        public void GetOrderProductsNegativeOrderTest()
        {
            // Arrange
            int orderId = -1;
            // Act
            List<OrderProductModel> products = calculator.GetOrderProducts(orderId);
            // Assert
            Assert.IsEmpty(products);
        }
    }
}
