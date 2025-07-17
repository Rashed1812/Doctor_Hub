using DAL.Data.Repositories.MedicalRepo;
using Microsoft.AspNetCore.Mvc;

namespace MyClinc.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IMedicalSpecialtyRepository _specialtyRepo;

        public ServicesController(IMedicalSpecialtyRepository specialtyRepo)
        {
            _specialtyRepo = specialtyRepo;
        }

        public async Task<IActionResult> Index()
        {
            var specialties = (await _specialtyRepo.GetAllAsync()).Where(s => s.IsVisibleToPatient).ToList();
            return View(specialties);
        }

        public async Task<IActionResult> Details(int id)
        {
            var specialty = await _specialtyRepo.GetByIdAsync(id);
            if (specialty == null)
                return NotFound();

            return View(specialty);
        }
    }
}

