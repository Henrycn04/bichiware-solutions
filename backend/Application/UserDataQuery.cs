using System.Security.Cryptography.X509Certificates;
using backend.Infrastructure;
using backend.Domain;

namespace backend.Application
{
    public class UserDataQuery
    {
        private readonly UserDataHandler _handler;
        public UserDataQuery()
        {
            this._handler = new UserDataHandler();
        }

        public UserDataModel getData(int userId)
        {
            return this._handler.getUserData(userId);
        }

    }
}
