using backend.Domain;
using backend.Handlers;
using backend.Infrastructure;
using System;

namespace backend.Application {
    public class DeleteAddressCommand {
        private deleteAddressHandler _handler;

        public DeleteAddressCommand() {
            this._handler = new deleteAddressHandler();
        }

        public void DeleteAddress(int addressID) {
            
        }

    }
}