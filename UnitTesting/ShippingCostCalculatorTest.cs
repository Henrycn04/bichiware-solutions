using backend.Models;
using backend.Infrastructure;

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
        public void NegativeMassTest()
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
        public void UnrealisticAddress()
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
        public void AddressIsOutsideCostaRica()
        {
            // Arrange
            PhysicalAddress address = new PhysicalAddress()
            {
                lat = 33.55513,
                lon = 53.09692
            };
            double mass = 1;
            // Act
            double cost = calculator.CalculateShippingCost(address, mass);
            // Assert
            Assert.AreEqual(cost, 0);
        }

        [Test]
        public void AddressIsNull()
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
        public void MassIsTooSmall()
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
    }
}
