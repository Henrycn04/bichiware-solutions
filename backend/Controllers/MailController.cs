using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MailController : Controller
    {
        IMailService mailService = null;

        public MailController(IMailService mailService)
        {
            this.mailService = mailService;
        }

        [HttpPost]
        public bool SendMail(MailDataModel mailData)
        {
            return this.mailService.SendMail(mailData);
        }
    }
}
