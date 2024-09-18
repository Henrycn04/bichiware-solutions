

using Microsoft.AspNetCore.Mvc;
using backend.Handlers;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountActivationController : Controller
    {
        private readonly AccountActivationHandler accountActivationHandler;

        public AccountActivationController()
        {
            this.accountActivationHandler = new AccountActivationHandler();
        }

        [HttpGet]
        public void GetHashedConfirmationCode()
        {

        }
    }
}
