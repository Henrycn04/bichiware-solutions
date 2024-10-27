using backend.Infrastructure;

namespace backend.Application
{
    public class ConfirmOrderCommand
    {
        private readonly SetToConfirmedOrderHandler handler;
         
        public ConfirmOrderCommand ()
        {
            this.handler = new SetToConfirmedOrderHandler();
        } 

        public void UpdateOrder(int orderID)
        {
            int rows = this.handler.SetToConfirmed(orderID);
            if (rows <= 0) throw new Exception("No rows were affected, order was already confirmed");
        }
    }
}
