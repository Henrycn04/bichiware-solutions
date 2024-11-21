using backend.Handlers;
using backend.Infrastructure;
using Moq;
using backend.Application;
using backend.Models;

namespace UnitTesting
{
    public class DeleteCompanyCommandTest
    {
        private Mock<IOrdersHandler>            mockOrdersHandler;
        private Mock<IUpdateProductHandler>     mockUpdateProductHandler;
        private IUpdateCompanyHandler           updateCompanyHandler;
        private DeleteCompanyCommand            deleteCompanyCommand;

        [SetUp]
        public void SetUp()
        {
            mockOrdersHandler = new Mock<IOrdersHandler>();
            mockUpdateProductHandler = new Mock<IUpdateProductHandler>();
            updateCompanyHandler = new UpdateCompanyHandler(mockUpdateProductHandler.Object, mockOrdersHandler.Object);
            deleteCompanyCommand = new DeleteCompanyCommand(updateCompanyHandler);
        }

        [Test]
        public void DeleteInvalidCompany()
        {
            // Arrange
            int companyId = -4;
            // Act and assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => deleteCompanyCommand.DeleteCompany(companyId));
            Assert.AreEqual("La identificacion de empresa debe ser un numero valido.", exception.ParamName);
        }

        [Test]
        public void DeleteBichiwareSolutions()
        {
            // Arrange
            int companyId = 1;
            // Act and assert
            var exception = Assert.Throws<InvalidOperationException>(() => deleteCompanyCommand.DeleteCompany(companyId));
            Assert.AreEqual("La compañia no se puede eliminar porque es la casa matriz.", exception.Message);
        }

        [Test]
        public void DeleteNotExisitingCompany()
        {
            // Arrange
            int companyId = 787;
            // Act and assert
            var exception = Assert.Throws<ArgumentException>(() => deleteCompanyCommand.DeleteCompany(companyId));
            Assert.AreEqual("El nombre de la compañia no fue encontrado.", exception.Message);
        }
    }
}
