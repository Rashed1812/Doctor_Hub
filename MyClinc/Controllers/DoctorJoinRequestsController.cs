using DAL.Data.Models;
using DAL.Data.Repositories.DoctorRequest;
using DAL.Data.Repositories.MedicalRepo;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace MyClinc.Controllers
{
    public class DoctorJoinRequestsController : Controller
    {
        private readonly IDoctorJoinRequestRepo _doctorRepo;
        private readonly IMedicalSpecialtyRepository _specialtyRepo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DoctorJoinRequestsController(IDoctorJoinRequestRepo doctorRepo, IMedicalSpecialtyRepository specialtyRepo, IWebHostEnvironment webHostEnvironment)
        {
            _doctorRepo = doctorRepo;
            _specialtyRepo = specialtyRepo;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Specialties = await _specialtyRepo.GetAllAsync();
            return View();
        }

        //POST: /DoctorJoinRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoctorJoinRequest model, int[] SelectedSpecialties)
        {
            if (ModelState.IsValid)
            {
                model.DoctorSpecialties = SelectedSpecialties.Select(id => new DoctorSpecialty
                {
                    MedicalSpecialtyId = id
                }).ToList();

                // Handle file uploads
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "doctors");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                if (model.GraduationCertificateFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.GraduationCertificateFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.GraduationCertificateFile.CopyToAsync(fileStream);
                    }
                    model.GraduationCertificatePath = "/uploads/doctors/" + fileName;
                }

                if (model.CertificateFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.CertificateFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.CertificateFile.CopyToAsync(fileStream);
                    }
                    model.CertificateFilePath = "/uploads/doctors/" + fileName;
                }

                if (model.LicenseFile != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.LicenseFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.LicenseFile.CopyToAsync(fileStream);
                    }
                    model.LicenseFilePath = "/uploads/doctors/" + fileName;
                }

                await _doctorRepo.AddAsync(model);
                await _doctorRepo.SaveAsync();

                return RedirectToAction("ThankYou");
            }

            ViewBag.Specialties = await _specialtyRepo.GetAllAsync();
            return View(model);
        }

        public IActionResult ThankYou()
        {
            return View();
        }

        // GET: /DoctorJoinRequests
        // Admin
        public async Task<IActionResult> Index()
        {
            var data = await _doctorRepo.GetAllAsync();
            return View(data);
        }

        //GET: /DoctorJoinRequests/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var doctor = await _doctorRepo.GetByIdAsync(id);
            if (doctor == null)
                return NotFound();

            return View("~/Views/Admin/DoctorDetails.cshtml", doctor);
        }

        //GET: /DoctorJoinRequests/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var doctor = await _doctorRepo.GetByIdAsync(id);
            if (doctor == null)
                return NotFound();

            return View(doctor);
        }
        //POST: /DoctorJoinRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _doctorRepo.GetByIdAsync(id);
            if (doctor != null)
            {
                _doctorRepo.Delete(doctor);
                await _doctorRepo.SaveAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}


