using DAL.Data.Models;
using DAL.Data.Repositories.ConsultationRepo;
using DAL.Data.Repositories.MedicalRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Add this using statement
using System.Linq;
using System.Threading.Tasks;

namespace MyClinc.Controllers
{
    public class ConsultationController : Controller
    {
        private readonly IMedicalSpecialtyRepository _specialtyRepo;
        private readonly IConsultationRepository _consultationRepo;

        public ConsultationController(IMedicalSpecialtyRepository specialtyRepo, IConsultationRepository consultationRepo)
        {
            _specialtyRepo = specialtyRepo;
            _consultationRepo = consultationRepo;
        }

        public async Task<IActionResult> Book()
        {
            var specialties = (await _specialtyRepo.GetAllAsync()).Where(s => s.IsVisibleToPatient).ToList();
            // Create a SelectList using 'Id' for value and 'Name' for text
            ViewBag.MedicalSpecialties = new SelectList(specialties, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Book(Consultation model)
        {
            if (ModelState.IsValid)
            {
                // Get the selected specialty to set the consultation fee
                var specialty = await _specialtyRepo.GetByIdAsync(model.MedicalSpecialtyId);
                if (specialty != null)
                {
                    model.ConsultationFee = specialty.Price ?? 0;
                }

                model.BookingDate = DateTime.Now; // Set booking date
                model.PaymentStatus = PaymentStatus.Pending; // Initial status
                model.Status = "قيد المراجعة"; // Initial consultation status

                await _consultationRepo.AddAsync(model);
                await _consultationRepo.SaveAsync();

                // Redirect to confirmation page with WhatsApp link
                return RedirectToAction("BookingConfirmation", new { phoneNumber = model.PhoneNumber });
            }

            var specialties = (await _specialtyRepo.GetAllAsync()).Where(s => s.IsVisibleToPatient).ToList();
            // Re-create the SelectList if ModelState is not valid
            ViewBag.MedicalSpecialties = new SelectList(specialties, "Id", "Name");
            return View(model);
        }

        public IActionResult BookingConfirmation(string phoneNumber)
        {
            ViewBag.WhatsAppNumber = phoneNumber;
            return View();
        }
    }
}