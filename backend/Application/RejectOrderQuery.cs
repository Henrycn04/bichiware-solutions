using backend.Handlers;

namespace backend.Queries
{
    public class RejectOrderQuery
    {
        private readonly RejectOrderHandler _rejectOrderHandler;

        public RejectOrderQuery()
        {
            this._rejectOrderHandler = new RejectOrderHandler();
        }

        public int RejectOrder(int orderID)
        {
            Console.WriteLine(orderID);

            if (checkIfOrderExists(orderID))
            {
                int rows = this._rejectOrderHandler.RejectOrder(orderID);
                return rows;
            } else
            {
                return 0;
            }
        }

        private bool checkIfOrderExists(int orderID)
        {
            if (this._rejectOrderHandler.CheckIfOrderExists(orderID) == 1)
            {
                return true;
            } else
            {
                return false;
            }
        }

    }
}
