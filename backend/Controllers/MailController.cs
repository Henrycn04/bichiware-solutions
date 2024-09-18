using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : Controller
    {
        IMailService mailService = null;

        public MailController(IMailService mailService)
        {
            this.mailService = mailService;
        }

        [HttpPost]
        public bool SendMail(MailData mailData)
        {
            return this.mailService.SendMail(mailData);
        }
    }
}
