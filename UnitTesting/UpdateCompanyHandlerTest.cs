using backend.Handlers;
using backend.Infrastructure;
using Moq;
using backend.Application;
using backend.Models;

namespace UnitTesting
{
    public class UpdateCompanyHandlerTest
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
        public void DeleteNullCompany()
        {
            // Arrange
            CompaniesIDModel company = null;
            // Act and Assert
            var exception = Assert.Throws<NullReferenceException>(() => deleteCompanyCommand.DeleteCompany(company));
            Assert.AreEqual("La compañia a borrar no puede ser nula.", exception.Message);
        }

        [Test]
        public void DeleteInvalidCompany()
        {
            // Arrange
            CompaniesIDModel company = new CompaniesIDModel()
            {
                CompanyID = -4,
                CompanyName = "Vault-Tek"
            };
            // Act and assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(() => deleteCompanyCommand.DeleteCompany(company));
            Assert.AreEqual("La identificacion de la empresa debe ser un numero valido.", exception.ParamName);
        }

        [Test]
        public void DeleteBichiwareSolutions()
        {
            // Arrange
            CompaniesIDModel company = new CompaniesIDModel()
            {
                CompanyID = 1,
                CompanyName = "Bichiware Solutions"
            };
            // Act and assert
            var exception = Assert.Throws<InvalidOperationException>(() => deleteCompanyCommand.DeleteCompany(company));
            Assert.AreEqual("La compañia Bichiware Solutions no se puede eliminar porque es la casa matriz.", exception.Message);
        }

        [Test]
        public void DeleteNotExisitingCompany()
        {
            // Arrange
            CompaniesIDModel company = new CompaniesIDModel()
            {
                CompanyID = 787,
                CompanyName = "Night Corp"
            };
            // Act and assert
            var exception = Assert.Throws<ArgumentException>(() => deleteCompanyCommand.DeleteCompany(company));
            Assert.AreEqual("El nombre de la compañia no fue encontrado.", exception.Message);
        }
    }
}
