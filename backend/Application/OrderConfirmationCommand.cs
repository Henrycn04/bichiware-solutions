using backend.Domain;

namespace backend.Application
{
    public class OrderConfirmationCommand
    {
        public OrderConfirmationCommand() { }

        public string makeConfirmationEmail(UserDataModel user, 
                                List<OrderedProductModel> products,
                                OrderConfirmationModel order)
        {
            // TODO Finish Method
            return "Placeholder";
        }
    }
}
