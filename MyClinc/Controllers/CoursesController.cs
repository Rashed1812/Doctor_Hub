using Microsoft.AspNetCore.Mvc;

namespace MyClinc.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            // This would typically fetch course details from a database
            ViewBag.CourseId = id;
            return View();
        }

        public IActionResult Enroll(int id)
        {
            // This would handle course enrollment
            ViewBag.CourseId = id;
            return View();
        }
    }
}

