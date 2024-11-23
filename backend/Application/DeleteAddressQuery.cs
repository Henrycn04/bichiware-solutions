using backend.Infrastructure;

namespace backend.Application
{
    public class DeleteAddressQuery
    {
        private readonly DeleteAddressHandler handler;
        public DeleteAddressQuery() {
            handler = new DeleteAddressHandler();
        }

        public bool CheckDelete(int addressID)
        {
           return this.handler.checkAddress(addressID);
        }
    }
}
