using backend.Handlers;
using backend.Models;

namespace backend.Application
{
    public class AddAddressCommand
    {
        private readonly AddAddressHandler handler;
        public AddAddressCommand() 
        {
            this.handler = new AddAddressHandler();
        }

        public void addAddress(AddAddressModel newAddress)
        {
            this.handler.addNewAddress(newAddress);
        }

    }
}
