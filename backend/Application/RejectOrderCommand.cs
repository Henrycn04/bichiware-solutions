using backend.Handlers;

namespace backend.Commands
{
    public class RejectOrderCommand
    {
        private readonly RejectOrderHandler _rejectOrderHandler;

        public RejectOrderCommand()
        {
            this._rejectOrderHandler = new RejectOrderHandler();
        }

        public int RejectOrder(int orderID)
        {
            int affectedRows = 0;
            if (checkValidityOfNumber(orderID) && checkIfOrderExists(orderID) && CheckStatusOfOrder(orderID))
            {
                affectedRows = this._rejectOrderHandler.RejectOrder(orderID);
            }
            return affectedRows;
        }

        private bool CheckStatusOfOrder(int orderID)
        {
            if (this._rejectOrderHandler.CheckStatusOfOrder(orderID) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkIfOrderExists(int orderID)
        {
            if (this._rejectOrderHandler.CheckIfOrderExists(orderID) == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool checkValidityOfNumber(int orderID)
        {
            if (orderID > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
