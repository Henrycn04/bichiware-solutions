using backend.Application;
using backend.Domain;
using backend.Infrastructure;
using backend.Models;
using Moq;

namespace UnitTesting
{
    public class ReportsCompanyCommandTest
    {
        private ReportsCompanyCommand reportsCompanyCommand;
        private ReportsCompanyHandler reportsCompanyHandler;
        private Mock<IUpdateCompanyHandler> updateCompanyHandler;

        [SetUp]
        public void SetUp()
        {
            reportsCompanyHandler = new ReportsCompanyHandler();
            updateCompanyHandler = new Mock<IUpdateCompanyHandler>();
            this.reportsCompanyCommand = new ReportsCompanyCommand(reportsCompanyHandler, updateCompanyHandler.Object);
        }

        [Test]
        public void ReportsCompanyCommandInvalidUserIdTest()
        {
            // Arrange
            FiltersCompletedOrdersModel filter = new FiltersCompletedOrdersModel()
            {
                UserID = -3,
                CompanyID = 1,
            };
            // Act and assert
            var exception = Assert.Throws<ArgumentException>(() => reportsCompanyCommand.GetPendingOrdersReport(filter));
            Assert.AreEqual("La identificacion de usuario no es valida.", exception.Message);
        }

        [Test]
        public void ReportsCompanyCommandInvalidCompanyIdTest()
        {
            // Arrange
            FiltersCompletedOrdersModel filter = new FiltersCompletedOrdersModel()
            {
                UserID = 1,
                CompanyID = -56,
            };
            // Act and assert
            var exception = Assert.Throws<ArgumentException>(() => reportsCompanyCommand.GetPendingOrdersReport(filter));
            Assert.AreEqual("La identificacion de compañia no es valida.", exception.Message);
        }

        [Test]
        public void ReportsCompanyCommandIncorrectUserTypeTest()
        {
            // Arrange
            FiltersCompletedOrdersModel filter = new FiltersCompletedOrdersModel()
            {
                UserID = 2,
                CompanyID = 1,
            };
            // Act and assert
            var exception = Assert.Throws<UnauthorizedAccessException>(() => reportsCompanyCommand.GetPendingOrdersReport(filter));
            Assert.AreEqual("Usted no es un administrador o empresario para realizar esta operacion.", exception.Message);
        }

        [Test]
        public void ReportsCompanyCommandNoPendingOrders()
        {
            // Arrange
            FiltersCompletedOrdersModel filter = new FiltersCompletedOrdersModel()
            {
                UserID = 1,
                CompanyID = 1,
            };
            // Act and assert
            var exception = Assert.Throws<KeyNotFoundException>(() => reportsCompanyCommand.GetPendingOrdersReport(filter));
            Assert.AreEqual("No se encontraron pedidos pendientes para esta compañia.", exception.Message);
        }

        [Test]
        public void ReportsCompanyCommandNullFilter()
        {
            // Arrange
            FiltersCompletedOrdersModel filter = null;
            // Act and assert
            var exception = Assert.Throws<NullReferenceException>(() => reportsCompanyCommand.GetPendingOrdersReport(filter));
            Assert.AreEqual("La identificación de usuario no puede ser nula.", exception.Message);
        }
    }
}
