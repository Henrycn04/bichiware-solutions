using Moq;
using backend.Application;
using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;


namespace UnitTestingDeleteUserData
{
    public class DeleteUserDataCommandTests
    {
        private readonly Mock<IUserDataHandler> _userDataHandlerMock;
        private readonly Mock<ICompanyProfileDataHandler> _companyProfileDataHandlerMock;
        private readonly Mock<IOrdersHandler> _ordersHandlerMock;
        private readonly DeleteUserDataCommand _command;
        const string validIDException = "The ID has to be positive.";
        const string userNotFoundException = "User not found";
        const string companiesRelatedException = "User cannot be deletead because it is the only memebr of a compnany.";
        const string ordersInProgressRelatedException = "User cannot be deletead because it has orders in progress.";
        const int testID = 1;
        const int negativeTestID = -1;

        public DeleteUserDataCommandTests()
        {
            _userDataHandlerMock = new Mock<IUserDataHandler>();
            _companyProfileDataHandlerMock = new Mock<ICompanyProfileDataHandler>();
            _ordersHandlerMock = new Mock<IOrdersHandler>();

            _command = new DeleteUserDataCommand(
                _userDataHandlerMock.Object,
                _companyProfileDataHandlerMock.Object,
                _ordersHandlerMock.Object);
        }

        [Test]
        public void DeleteUserData_ShouldThrowArgumentException_WhenUserIdIsInvalid()
        {
            // Arrange
            int invalidUserId = negativeTestID;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _command.DeleteUserData(invalidUserId));
            Assert.AreEqual(validIDException, exception.Message);
        }

        [Test]
        public void DeleteUserData_ShouldThrowArgumentException_WhenUserNotFound()
        {
            // Arrange
            int userId = testID;
            _userDataHandlerMock.Setup(x => x.getUserData(userId)).Returns((UserDataModel)null);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _command.DeleteUserData(userId));
            Assert.AreEqual(userNotFoundException, exception.Message);
        }

        [Test]
        public void DeleteUserData_ShouldThrowInvalidOperationException_WhenUserHasRelatedCompanies()
        {
            // Arrange
            int userId = testID;
            var user = new UserDataModel();  // User exists
            _userDataHandlerMock.Setup(x => x.getUserData(userId)).Returns(user);
            _companyProfileDataHandlerMock.Setup(x => x.userHasRelatedCompanies(userId)).Returns(true);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _command.DeleteUserData(userId));
            Assert.AreEqual(companiesRelatedException, exception.Message);
        }

        [Test]
        public void DeleteUserData_ShouldThrowInvalidOperationException_WhenUserHasOrdersInProgress()
        {
            // Arrange
            int userId = testID;
            var user = new UserDataModel();  // User exists
            _userDataHandlerMock.Setup(x => x.getUserData(userId)).Returns(user);
            _companyProfileDataHandlerMock.Setup(x => x.userHasRelatedCompanies(userId)).Returns(false);
            _ordersHandlerMock.Setup(x => x.userHasRelatedOrdersInProgress(userId)).Returns(true);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _command.DeleteUserData(userId));
            Assert.AreEqual(ordersInProgressRelatedException, exception.Message);
        }

        [Test]
        public void DeleteUserData_ShouldCallLogicDelete_WhenUserHasOrders()
        {
            // Arrange
            int userId = testID;
            var user = new UserDataModel();
            _userDataHandlerMock.Setup(x => x.getUserData(userId)).Returns(user);
            _companyProfileDataHandlerMock.Setup(x => x.userHasRelatedCompanies(userId)).Returns(false);
            _ordersHandlerMock.Setup(x => x.userHasRelatedOrdersInProgress(userId)).Returns(false);
            _ordersHandlerMock.Setup(x => x.userHasRelatedOrders(userId)).Returns(true);

            // Act
            _command.DeleteUserData(userId);

            // Assert
            _userDataHandlerMock.Verify(x => x.logicDeleteUserData(userId), Times.Once);
        }

        [Test]
        public void DeleteUserData_ShouldCallDelete_WhenUserHasNoOrders()
        {
            // Arrange
            int userId = testID;
            var user = new UserDataModel();
            _userDataHandlerMock.Setup(x => x.getUserData(userId)).Returns(user);
            _companyProfileDataHandlerMock.Setup(x => x.userHasRelatedCompanies(userId)).Returns(false);
            _ordersHandlerMock.Setup(x => x.userHasRelatedOrdersInProgress(userId)).Returns(false);
            _ordersHandlerMock.Setup(x => x.userHasRelatedOrders(userId)).Returns(false);

            // Act
            _command.DeleteUserData(userId);

            // Assert
            _userDataHandlerMock.Verify(x => x.deleteUserData(userId), Times.Once);
        }
    }
}