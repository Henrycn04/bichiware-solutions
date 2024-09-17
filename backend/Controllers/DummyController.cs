using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    public class DummyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
