using backend.Handlers;
using backend.Domain;
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

        public void addAddress(PhysicalAddress newAddress)
        {
            this.handler.addNewAddress(newAddress);
        }

    }
}
