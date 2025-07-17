using Microsoft.AspNetCore.Mvc;

namespace MyClinc.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

