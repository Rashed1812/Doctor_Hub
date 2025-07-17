using Microsoft.AspNetCore.Mvc;

namespace MyClinc.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string name, string email, string message)
        {
            // Handle contact form submission
            if (ModelState.IsValid)
            {
                // Process the contact form (e.g., send email, save to database)
                ViewBag.Message = "تم إرسال رسالتك بنجاح. سنتواصل معك قريباً.";
            }
            return View();
        }
    }
}

