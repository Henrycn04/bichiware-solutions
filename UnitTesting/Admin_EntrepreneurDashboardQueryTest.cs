using backend.Queries;
using backend.Handlers;
using Moq;
using backend.Models;

namespace UnitTesting
{
    class Admin_EntrepreneurDashboardQueryTest
    {
        private readonly Mock<IAdmin_EntrepreneurOrdersHandler> _mockAdminEntrepreneurDahsboardHandler;
        private readonly Admin_EntrepreneurDashboardQuery _admin_EntrepreneurDashboardQuery;
        private readonly List<EntrepreneurOrdersModel> ListForTesting = new List<EntrepreneurOrdersModel>
        {
            new EntrepreneurOrdersModel { },
        };
        public Admin_EntrepreneurDashboardQueryTest()
        {
            this._mockAdminEntrepreneurDahsboardHandler = new Mock<IAdmin_EntrepreneurOrdersHandler>();
            this._admin_EntrepreneurDashboardQuery = new Admin_EntrepreneurDashboardQuery(this._mockAdminEntrepreneurDahsboardHandler.Object);
        }

        [Test]
        public void CheckValidityOfNumber_Integer()
        {
            // Arrange
            int userID = 1;
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserExists(userID)).Returns(1);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserHasCompanies(userID)).Returns(2);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.GetOrdersForEntrepreneur(userID)).Returns(ListForTesting);

            // Act
            List<EntrepreneurOrdersModel> returnList = this._admin_EntrepreneurDashboardQuery.GetOrdersForEntrepreneur(userID);

            // Assert
            Assert.IsTrue(returnList.Count > 0);
        }

        [Test]
        public void CheckValidityOfNumber_NegativeNumber()
        {
            // Arrange
            int userID = -1;
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserExists(userID)).Returns(1);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserHasCompanies(userID)).Returns(2);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.GetOrdersForEntrepreneur(userID)).Returns(ListForTesting);

            // Act
            var returnList = this._admin_EntrepreneurDashboardQuery.GetOrdersForEntrepreneur(userID);

            // Assert
            Assert.IsTrue(returnList.Count == 0);
        }

        [Test]
        public void CheckValidityOfNumber_Zero()
        {
            // Arrange
            int userID = 0;
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserExists(userID)).Returns(1);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserHasCompanies(userID)).Returns(2);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.GetOrdersForEntrepreneur(userID)).Returns(ListForTesting);

            // Act
            var returnList = this._admin_EntrepreneurDashboardQuery.GetOrdersForEntrepreneur(userID);

            // Assert
            Assert.IsTrue(returnList.Count == 0);
        }

        [Test]
        public void CheckIfUserHasCompanies_UserHasCompanies()
        {
            // Arrange
            int userID = 1;
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserExists(userID)).Returns(1);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserHasCompanies(userID)).Returns(2);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.GetOrdersForEntrepreneur(userID)).Returns(ListForTesting);

            // Act
            List<EntrepreneurOrdersModel> returnList = this._admin_EntrepreneurDashboardQuery.GetOrdersForEntrepreneur(userID);

            // Assert
            Assert.IsTrue(returnList.Count > 0);
        }

        [Test]
        public void CheckIfUserHasCompanies_UserHasNoCompanies()
        {
            // Arrange
            int userID = 1;
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserExists(userID)).Returns(1);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserHasCompanies(userID)).Returns(0);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.GetOrdersForEntrepreneur(userID)).Returns(ListForTesting);

            // Act
            List<EntrepreneurOrdersModel> returnList = this._admin_EntrepreneurDashboardQuery.GetOrdersForEntrepreneur(userID);

            // Assert
            Assert.IsTrue(returnList.Count == 0);
        }

        [Test]
        public void CheckIfUserExists_ItExists()
        {
            // Arrange
            int userID = 1;
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserExists(userID)).Returns(1);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserHasCompanies(userID)).Returns(2);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.GetOrdersForEntrepreneur(userID)).Returns(ListForTesting);

            // Act
            List<EntrepreneurOrdersModel> returnList = this._admin_EntrepreneurDashboardQuery.GetOrdersForEntrepreneur(userID);

            // Assert
            Assert.IsTrue(returnList.Count > 0);
        }

        [Test]
        public void CheckIfUserExists_ItDoesNotExists()
        {
            // Arrange
            int userID = 1;
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserExists(userID)).Returns(0);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.CheckIfUserHasCompanies(userID)).Returns(2);
            this._mockAdminEntrepreneurDahsboardHandler.Setup(AdminEntrepreneurDashboardHandler => AdminEntrepreneurDashboardHandler.GetOrdersForEntrepreneur(userID)).Returns(ListForTesting);

            // Act
            List<EntrepreneurOrdersModel> returnList = this._admin_EntrepreneurDashboardQuery.GetOrdersForEntrepreneur(userID);

            // Assert
            Assert.IsTrue(returnList.Count == 0);
        }
    }
}
