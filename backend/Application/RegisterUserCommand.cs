using backend.Handlers;
using backend.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace backend.Application
{
    public class RegisterUserCommand
    {
        private registerUserHandler userHandler;

        public RegisterUserCommand()
        {
            this.userHandler = new registerUserHandler();
        }

        public void addUser(registerUserModel data)
        {
            int IDUser = this.userHandler.addProfile(data);
            this.userHandler.addUser(data, IDUser);
            
            int IDAddr = this.userHandler.addAddr(data);
            this.userHandler.addReferencesAddr(IDUser, IDAddr);
        }
    }
}
