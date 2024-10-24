using backend.Domain;
using backend.Infrastructure;

namespace backend.Application
{
    public class UserDataCommand
    {
        private readonly UserDataHandler _handler;
        public UserDataCommand()
        {
            this._handler = new UserDataHandler();
        }

        public void setData(UserDataModel data)
        {
            this._handler.updateUserData(data);
        }
    }
}
