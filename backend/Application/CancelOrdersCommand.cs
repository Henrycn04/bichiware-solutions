using backend.Handlers;

namespace backend.Commands
{
    public class CancelOrdersCommand
    {
        private readonly RejectOrderHandler _rejectOrderHandler;
        public CancelOrdersCommand()
        {
            this._rejectOrderHandler = new RejectOrderHandler();
        }
        public int CancelOrderByUser(int orderID)
        {
            int affectedRows = 0;
            if (checkValidityOfNumber(orderID) && checkIfOrderExists(orderID))
            {
                if (CheckStatusOfOrderForUser(orderID))
                {
                    affectedRows = this._rejectOrderHandler.RejectOrder(orderID);
                } else
                {
                    affectedRows = -1;
                }
            }
            return affectedRows;
        }

        public int CancelOrderByEntrepreneur(int orderID)
        {
            int affectedRows = 0;
            if (checkValidityOfNumber(orderID) && checkIfOrderExists(orderID) && CheckStatusOfOrderForEntrepreneur(orderID))
            {
                if (CheckStatusOfOrderForEntrepreneur(orderID))
                {
                    affectedRows = this._rejectOrderHandler.RejectOrder(orderID);
                }
                else
                {
                    affectedRows = -1;
                }
            }
            return affectedRows;
        }

        private bool CheckStatusOfOrderForUser(int orderID)
        {
            return (this._rejectOrderHandler.CheckStatusOfOrder(orderID) == 1);
        }

        private bool CheckStatusOfOrderForEntrepreneur(int orderID)
        {
            int statusOfOrder = this._rejectOrderHandler.CheckStatusOfOrder(orderID);
            return (statusOfOrder == 1 || statusOfOrder == 2);
        }

        private bool checkIfOrderExists(int orderID)
        {
            return (this._rejectOrderHandler.CheckIfOrderExists(orderID) == 1);
        }

        private bool checkValidityOfNumber(int orderID)
        {
            return (orderID > 0);
        }

    }
}
