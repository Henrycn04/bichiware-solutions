using backend.Handlers;
using backend.Infrastructure;
using backend.Models;

namespace backend.Commands
{
    public class UpdateAddressCommand
    {
        private readonly UpdateAddressHandler _handler;

        public UpdateAddressCommand(UpdateAddressHandler handler)
        {
            _handler = handler;
        }

        public async Task<bool> ExecuteUser(AddressModelUpdate address)
        {
            if (!await _handler.AddressExists(address.AddressID))
            {
                return false;
            }

            if (!await _handler.AddressLinkedToUser(address.UserID, address.AddressID))
            {
                return false;
            }

            return await _handler.HandleUpdate(address);
        }
        public async Task<bool> ExecuteCompany(AddressModelUpdate address)
        {
            if (!await _handler.AddressExists(address.AddressID))
            {
                return false;
            }

            if (!await _handler.AddressLinkedToCompany(address.CompanyID, address.AddressID))
            {
                return false;
            }

            return await _handler.HandleUpdate(address);
        }


    }
}

