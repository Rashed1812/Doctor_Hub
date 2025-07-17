using DAL.Data.Models;
using DAL.Data.Repositories.MedicalRepo;
using Microsoft.AspNetCore.Mvc;

namespace MyClinc.Controllers
{
    public class MedicalSpecialtyController : Controller
    {
        private readonly IMedicalSpecialtyRepository _specialtyRepo;

        public MedicalSpecialtyController(IMedicalSpecialtyRepository specialtyRepo)
        {
            _specialtyRepo = specialtyRepo;
        }

        public async Task<IActionResult> Index()
        {
            var specialties = await _specialtyRepo.GetAllAsync();
            return View(specialties);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] MedicalSpecialty specialty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await _specialtyRepo.AddAsync(specialty);
                    await _specialtyRepo.SaveAsync();
                    return Json(new { success = true, message = "تم إضافة التخصص بنجاح" });
                }
                return Json(new { success = false, message = "البيانات غير صحيحة" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء إضافة التخصص" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] MedicalSpecialty specialty)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingSpecialty = await _specialtyRepo.GetByIdAsync(specialty.Id);
                    if (existingSpecialty != null)
                    {
                        existingSpecialty.Name = specialty.Name;
                        existingSpecialty.Description = specialty.Description;
                        existingSpecialty.Price = specialty.Price;
                        existingSpecialty.IconClass = specialty.IconClass;
                        existingSpecialty.IsVisibleToPatient = specialty.IsVisibleToPatient;

                        _specialtyRepo.Update(existingSpecialty);
                        await _specialtyRepo.SaveAsync();
                        return Json(new { success = true, message = "تم تحديث التخصص بنجاح" });
                    }
                    return Json(new { success = false, message = "التخصص غير موجود" });
                }
                return Json(new { success = false, message = "البيانات غير صحيحة" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء تحديث التخصص" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var specialty = await _specialtyRepo.GetByIdAsync(id);
                if (specialty != null)
                {
                    _specialtyRepo.Delete(specialty);
                    await _specialtyRepo.SaveAsync();
                    return Json(new { success = true, message = "تم حذف التخصص بنجاح" });
                }
                return Json(new { success = false, message = "التخصص غير موجود" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء حذف التخصص" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ToggleVisibility(int id)
        {
            try
            {
                var specialty = await _specialtyRepo.GetByIdAsync(id);
                if (specialty != null)
                {
                    specialty.IsVisibleToPatient = !specialty.IsVisibleToPatient;
                    _specialtyRepo.Update(specialty);
                    await _specialtyRepo.SaveAsync();
                    return Json(new { success = true, message = "تم تحديث حالة الرؤية بنجاح" });
                }
                return Json(new { success = false, message = "التخصص غير موجود" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء تحديث حالة الرؤية" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetSpecialty(int id)
        {
            try
            {
                var specialty = await _specialtyRepo.GetByIdAsync(id);
                if (specialty != null)
                {
                    return Json(new { success = true, data = specialty });
                }
                return Json(new { success = false, message = "التخصص غير موجود" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء جلب بيانات التخصص" });
            }
        }
    }
}
