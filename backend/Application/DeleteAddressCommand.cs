using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using System;

namespace backend.Application {
    public class DeleteAddressCommand {
        private DeleteAddressHandler _handler;

        public DeleteAddressCommand() {
            this._handler = new DeleteAddressHandler();
        }

        public void DeleteAddress(int addressID, bool fullDelete) {
            if(fullDelete)this._handler.DeleteAddress(addressID);
            else this._handler.LogicalDeleteAddress(addressID);
        }

    }
}