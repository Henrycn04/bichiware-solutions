using backend.Commands;
using backend.Handlers;
using Moq;


namespace UnitTesting
{
    class CancelOrdersCommandTest
    {
        private readonly Mock<IRejectOrderHandler> _mockRejectOrderHandler;
        private readonly CancelOrdersCommand _cancelOrdersCommand;
        public CancelOrdersCommandTest()
        {
            this._mockRejectOrderHandler = new Mock<IRejectOrderHandler>();
            this._cancelOrdersCommand = new CancelOrdersCommand(this._mockRejectOrderHandler.Object);
        }

        [Test]
        public void CheckValidityOfNumber_Integer()
        {
            // Arrange
            int orderID = 1;
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckIfOrderExists(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckStatusOfOrder(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.RejectOrder(orderID)).Returns(10);

            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByUser(orderID);

            // Assert
            Assert.AreEqual(10, returnValue);
        }

        [Test]
        public void CheckValidityOfNumber_NegativeNumber()
        {
            // Arrange
            int orderID = -1;

            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByUser(orderID);

            // Assert
            Assert.AreEqual(0, returnValue);
        }

        [Test]
        public void CheckValidityOfNumber_Zero()
        {
            // Arrange
            int orderID = 0;

            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByUser(orderID);

            // Assert
            Assert.AreEqual(0, returnValue);
        }

        [Test]
        public void CheckExistentOrder()
        {
            // Arrange
            int orderID = 10;
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckIfOrderExists(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckStatusOfOrder(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.RejectOrder(orderID)).Returns(10);

            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByUser(orderID);

            // Assert
            Assert.AreEqual(10, returnValue);
        }

        [Test]
        public void CheckInexistentOrder()
        {
            // Arrange
            int orderID = 10;
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckIfOrderExists(orderID)).Returns(0);

            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByUser(orderID);

            // Assert
            Assert.AreEqual(0, returnValue);
        }

        [Test]
        public void CancelOrderByUser_CheckOrderStatus_PendingOrder()
        {
            // Arrange
            int orderID = 1;
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckIfOrderExists(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckStatusOfOrder(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.RejectOrder(orderID)).Returns(10);
            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByUser(orderID);

            // Assert
            Assert.AreEqual(10, returnValue);
        }

        [Test]
        public void CancelOrderByUser_CheckOrderStatus_OrderAlreadyConfirmed()
        {
            // Arrange
            int orderID = 1;
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckIfOrderExists(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckStatusOfOrder(orderID)).Returns(2);

            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByUser(orderID);

            // Assert
            Assert.AreEqual(-1, returnValue);
        }

        [Test]
        public void CancelOrderByUser_CheckOrderStatus_OrderAlreadyRejected()
        {
            // Arrange
            int orderID = 1;
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckIfOrderExists(orderID)).Returns(1);

            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckStatusOfOrder(orderID)).Returns(3);

            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByUser(orderID);

            // Assert
            Assert.AreEqual(-1, returnValue);
        }

        [Test]
        public void CancelOrderByUser_CheckOrderStatus_OrderAlreadySent()
        {
            // Arrange
            int orderID = 1;
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckIfOrderExists(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckStatusOfOrder(orderID)).Returns(4);

            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByUser(orderID);

            // Assert
            Assert.AreEqual(-1, returnValue);
        }

        [Test]
        public void CancelOrderByUser_CheckOrderStatus_OrderAlreadyDone()
        {
            // Arrange
            int orderID = 1;
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckIfOrderExists(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckStatusOfOrder(orderID)).Returns(5);

            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByUser(orderID);

            // Assert
            Assert.AreEqual(-1, returnValue);
        }

        [Test]
        public void CancelOrderByEntrepreneur_CheckOrderStatus_PendingOrder()
        {
            // Arrange
            int orderID = 1;
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckIfOrderExists(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckStatusOfOrder(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.RejectOrder(orderID)).Returns(10);
            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByEntrepreneur(orderID);

            // Assert
            Assert.AreEqual(10, returnValue);
        }

        [Test]
        public void CancelOrderByEntrepreneur_CheckOrderStatus_OrderAlreadyConfirmed()
        {
            // Arrange
            int orderID = 1;
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckIfOrderExists(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckStatusOfOrder(orderID)).Returns(2);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.RejectOrder(orderID)).Returns(10);

            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByEntrepreneur(orderID);

            // Assert
            Assert.AreEqual(10, returnValue);
        }

        [Test]
        public void CancelOrderByEntrepreneur_CheckOrderStatus_OrderAlreadyRejected()
        {
            // Arrange
            int orderID = 1;
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckIfOrderExists(orderID)).Returns(1);

            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckStatusOfOrder(orderID)).Returns(3);

            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByEntrepreneur(orderID);

            // Assert
            Assert.AreEqual(-1, returnValue);
        }

        [Test]
        public void CancelOrderByEntrepreneur_CheckOrderStatus_OrderAlreadySent()
        {
            // Arrange
            int orderID = 1;
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckIfOrderExists(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckStatusOfOrder(orderID)).Returns(4);

            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByEntrepreneur(orderID);

            // Assert
            Assert.AreEqual(-1, returnValue);
        }

        [Test]
        public void CancelOrderByEntrepreneur_CheckOrderStatus_OrderAlreadyDone()
        {
            // Arrange
            int orderID = 1;
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckIfOrderExists(orderID)).Returns(1);
            this._mockRejectOrderHandler.Setup(rejectOrderHandler => rejectOrderHandler.CheckStatusOfOrder(orderID)).Returns(5);

            // Act
            int returnValue = this._cancelOrdersCommand.CancelOrderByEntrepreneur(orderID);

            // Assert
            Assert.AreEqual(-1, returnValue);
        }
    }
}
