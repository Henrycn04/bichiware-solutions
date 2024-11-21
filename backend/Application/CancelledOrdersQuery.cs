using backend.Handlers;
using backend.Models;

namespace backend.Queries
{
    public class CancelledOrdersQuery
    {
        private readonly ICancelledOrdersHandler _cancelledOrdersHandler;

        public CancelledOrdersQuery(ICancelledOrdersHandler cancelledOrdersHandler)
        {
            this._cancelledOrdersHandler = cancelledOrdersHandler;
        }
    }
}
