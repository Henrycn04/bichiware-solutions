using backend.Handlers;
using backend.Queries;
using Moq;
using backend.Domain;
using backend.Models;
using System;

namespace UnitTesting
{
    class CancelledOrdersQueryTest
    {
        private readonly Mock<ICancelledOrdersHandler> _mockCancelledOrdersdHandler;
        private readonly CancelledOrdersQuery _cancelledOrdersQuery;
        private readonly List<CancelledOrdersModel> ListForTesting = new List<CancelledOrdersModel>
        {
            new CancelledOrdersModel { },
        };

        public CancelledOrdersQueryTest()
        {
            this._mockCancelledOrdersdHandler = new Mock<ICancelledOrdersHandler>();
            this._cancelledOrdersQuery = new CancelledOrdersQuery(this._mockCancelledOrdersdHandler.Object);
        }

        [Test]
        public void CheckValidityOfNumber_SuccessWhenUserIDAndCompanyIDAreIntegersGreaterThanZero()
        {
            // Arrange
            FiltersCompletedOrdersModel filters = new FiltersCompletedOrdersModel()
            {
                UserID = 1,
                CompanyID = 1
            };
            int companyID = filters.CompanyID.Value;
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserExists(filters.UserID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfCompanyExists(companyID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserIsAdminOrEntrepeneur(filters.UserID)).Returns(3);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.GetCancelledOrders(filters)).Returns(this.ListForTesting);

            // Act
            var returnList = this._cancelledOrdersQuery.GetCancelledOrders(filters);

            // Assert
            Assert.IsTrue(returnList.Count == 1);
        }

        [Test]
        public void CheckValidityOfNumber_FailsWhenUserIDIsANegativeNumber()
        {
            // Arrange
            FiltersCompletedOrdersModel filters = new FiltersCompletedOrdersModel()
            {
                UserID = -1,
            };
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserExists(filters.UserID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserIsAdminOrEntrepeneur(filters.UserID)).Returns(3);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.GetCancelledOrders(filters)).Returns(this.ListForTesting);

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _cancelledOrdersQuery.GetCancelledOrders(filters));

            // Assert
            Assert.AreEqual("Not valid ID.", exception.Message);
        }

        [Test]
        public void CheckValidityOfNumber_FailsWhenCompanyIDIsANegativeNumber()
        {
            // Arrange
            FiltersCompletedOrdersModel filters = new FiltersCompletedOrdersModel()
            {
                UserID = 1,
                CompanyID = -1
            };
            int companyID = filters.CompanyID.Value;
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserExists(filters.UserID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfCompanyExists(companyID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserIsAdminOrEntrepeneur(filters.UserID)).Returns(3);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.GetCancelledOrders(filters)).Returns(this.ListForTesting);

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _cancelledOrdersQuery.GetCancelledOrders(filters));

            // Assert
            Assert.AreEqual("Not valid ID.", exception.Message);
        }

        [Test]
        public void CheckValidityOfNumber_FailsWhenUserIDIsZero()
        {
            // Arrange
            FiltersCompletedOrdersModel filters = new FiltersCompletedOrdersModel()
            {
                UserID = 0,
            };
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserExists(filters.UserID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserIsAdminOrEntrepeneur(filters.UserID)).Returns(3);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.GetCancelledOrders(filters)).Returns(this.ListForTesting);

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _cancelledOrdersQuery.GetCancelledOrders(filters));

            // Assert
            Assert.AreEqual("Not valid ID.", exception.Message);
        }

        [Test]
        public void CheckValidityOfNumber_FailsWhenCompanyIDIsZero()
        {
            // Arrange
            FiltersCompletedOrdersModel filters = new FiltersCompletedOrdersModel()
            {
                UserID = 1,
                CompanyID = 0
            };
            int companyID = filters.CompanyID.Value;
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserExists(filters.UserID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfCompanyExists(companyID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserIsAdminOrEntrepeneur(filters.UserID)).Returns(3);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.GetCancelledOrders(filters)).Returns(this.ListForTesting);

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _cancelledOrdersQuery.GetCancelledOrders(filters));

            // Assert
            Assert.AreEqual("Not valid ID.", exception.Message);
        }

        [Test]
        public void CheckIfUserExist_SuccessWhenUserDoesExist()
        {
            // Arrange
            FiltersCompletedOrdersModel filters = new FiltersCompletedOrdersModel()
            {
                UserID = 1,
            };
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserExists(filters.UserID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserIsAdminOrEntrepeneur(filters.UserID)).Returns(3);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.GetCancelledOrders(filters)).Returns(this.ListForTesting);

            // Act
            var returnList = _cancelledOrdersQuery.GetCancelledOrders(filters);

            // Assert
            Assert.IsTrue(returnList.Count == 1);
        }

        [Test]
        public void CheckIfUserExist_WhenUserDoesNotExist()
        {
            // Arrange
            FiltersCompletedOrdersModel filters = new FiltersCompletedOrdersModel()
            {
                UserID = 1,
            };
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserExists(filters.UserID)).Returns(0);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserIsAdminOrEntrepeneur(filters.UserID)).Returns(3);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.GetCancelledOrders(filters)).Returns(this.ListForTesting);

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _cancelledOrdersQuery.GetCancelledOrders(filters));

            // Assert
            Assert.AreEqual("UserID does not exist.", exception.Message);
        }

        [Test]
        public void CheckIfCompanyExist_SuccessWhenCompanyDoetExist()
        {
            // Arrange
            FiltersCompletedOrdersModel filters = new FiltersCompletedOrdersModel()
            {
                UserID = 1,
                CompanyID = 1,
            };
            int companyID = filters.CompanyID.Value;
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserExists(filters.UserID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfCompanyExists(companyID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserIsAdminOrEntrepeneur(filters.UserID)).Returns(3);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.GetCancelledOrders(filters)).Returns(this.ListForTesting);

            // Act
            var returnList = _cancelledOrdersQuery.GetCancelledOrders(filters);

            // Assert
            Assert.IsTrue(returnList.Count == 1);
        }

        [Test]
        public void CheckIfCompanyExist_FailsWhenCompanyDoesNotExist()
        {
            // Arrange
            FiltersCompletedOrdersModel filters = new FiltersCompletedOrdersModel()
            {
                UserID = 1,
                CompanyID = 1,
            };
            int companyID = filters.CompanyID.Value;
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserExists(filters.UserID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfCompanyExists(companyID)).Returns(0);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserIsAdminOrEntrepeneur(filters.UserID)).Returns(3);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.GetCancelledOrders(filters)).Returns(this.ListForTesting);

            // Act
            var exception = Assert.Throws<ArgumentException>(() => _cancelledOrdersQuery.GetCancelledOrders(filters));

            // Assert
            Assert.AreEqual("CompanyID does not exist.", exception.Message);
        }

        [Test]
        public void CheckIfUserIsAdminOrEntrepreneur_SuccessIfUserIsAdmin()
        {
            // Arrange
            FiltersCompletedOrdersModel filters = new FiltersCompletedOrdersModel()
            {
                UserID = 1,
                CompanyID = 1,
            };
            int companyID = filters.CompanyID.Value;
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserExists(filters.UserID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfCompanyExists(companyID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserIsAdminOrEntrepeneur(filters.UserID)).Returns(3);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.GetCancelledOrders(filters)).Returns(this.ListForTesting);

            // Act
            var returnList = _cancelledOrdersQuery.GetCancelledOrders(filters);

            // Assert
            Assert.IsTrue(returnList.Count == 1);
        }

        [Test]
        public void CheckIfUserIsAdminOrEntrepreneur_SuccessIfUserIsEntrepreneur()
        {
            // Arrange
            FiltersCompletedOrdersModel filters = new FiltersCompletedOrdersModel()
            {
                UserID = 1,
                CompanyID = 1,
            };
            int companyID = filters.CompanyID.Value;
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserExists(filters.UserID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfCompanyExists(companyID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserIsAdminOrEntrepeneur(filters.UserID)).Returns(2);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.GetCancelledOrders(filters)).Returns(this.ListForTesting);

            // Act
            var returnList = _cancelledOrdersQuery.GetCancelledOrders(filters);

            // Assert
            Assert.IsTrue(returnList.Count == 1);
        }

        [Test]
        public void CheckIfUserIsAdminOrEntrepreneur_FailsIfUserIsNeither()
        {
            // Arrange
            FiltersCompletedOrdersModel filters = new FiltersCompletedOrdersModel()
            {
                UserID = 1,
                CompanyID = 1,
            };
            int companyID = filters.CompanyID.Value;
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserExists(filters.UserID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfCompanyExists(companyID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.CheckIfUserIsAdminOrEntrepeneur(filters.UserID)).Returns(1);
            this._mockCancelledOrdersdHandler.Setup(CancelledOrdersHandler => CancelledOrdersHandler.GetCancelledOrders(filters)).Returns(this.ListForTesting);

            // Act
            var exception = Assert.Throws<UnauthorizedAccessException>(() => _cancelledOrdersQuery.GetCancelledOrders(filters));

            // Assert
            Assert.AreEqual("User is not authorized to view orders.", exception.Message);
        }
    }
}
