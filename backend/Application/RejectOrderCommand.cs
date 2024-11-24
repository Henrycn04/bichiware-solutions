using backend.Handlers;

namespace backend.Commands
{
    public class RejectOrderCommand
    {
        private readonly IRejectOrderHandler _rejectOrderHandler;

        public RejectOrderCommand(IRejectOrderHandler rejectOrderHandler)
        {
            this._rejectOrderHandler = rejectOrderHandler;
        }

        public int RejectOrder(int orderID)
        {
            int affectedRows = 0;
            if (checkValidityOfNumber(orderID) && checkIfOrderExists(orderID) && CheckStatusOfOrder(orderID))
            {
                int userType = 3;
                affectedRows = this._rejectOrderHandler.RejectOrder(orderID, userType);
            }
            return affectedRows;
        }

        private bool CheckStatusOfOrder(int orderID)
        {
            return (this._rejectOrderHandler.CheckStatusOfOrder(orderID) == 1);
        }

        private bool checkIfOrderExists(int orderID)
        {
            return (this._rejectOrderHandler.CheckIfOrderExists(orderID) == 1) ;
        }

        private bool checkValidityOfNumber(int orderID)
        {
            return (orderID > 0) ;
        }

    }
}
