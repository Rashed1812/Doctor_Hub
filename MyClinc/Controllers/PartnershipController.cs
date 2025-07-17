using DAL.Data.Models;
using DAL.Data.Repositories.PartnershipRepo;
using Microsoft.AspNetCore.Mvc;

namespace MyClinc.Controllers
{
    public class PartnershipController : Controller
    {
        private readonly IPartnershipRepository _partnershipRepository;

        public PartnershipController(IPartnershipRepository partnershipRepository)
        {
            _partnershipRepository = partnershipRepository;
        }

        // GET: Partnership
        public IActionResult Index()
        {
            return View();
        }

        // GET: Partnership/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Partnership/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Partnership partnership)
        {
            if (ModelState.IsValid)
            {
                partnership.SubmissionDate = DateTime.Now;
                partnership.Status = PartnershipStatus.Pending;
                
                await _partnershipRepository.AddAsync(partnership);
                await _partnershipRepository.SaveAsync();
                
                TempData["SuccessMessage"] = "تم إرسال طلب الشراكة بنجاح. سيتم التواصل معكم قريباً.";
                return RedirectToAction("Success");
            }
            
            return View(partnership);
        }

        // GET: Partnership/Success
        public IActionResult Success()
        {
            return View();
        }

        // GET: Partnership/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var partnership = await _partnershipRepository.GetByIdAsync(id);
            if (partnership == null)
            {
                return NotFound();
            }
            return View(partnership);
        }

        // GET: Partnership/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var partnership = await _partnershipRepository.GetByIdAsync(id);
            if (partnership == null)
            {
                return NotFound();
            }
            return View(partnership);
        }

        // POST: Partnership/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Partnership partnership)
        {
            if (id != partnership.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _partnershipRepository.Update(partnership);
                    await _partnershipRepository.SaveAsync();
                    TempData["SuccessMessage"] = "تم تحديث بيانات الشراكة بنجاح.";
                    return RedirectToAction("Details", new { id = partnership.Id });
                }
                catch (Exception)
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء تحديث البيانات.";
                }
            }
            return View(partnership);
        }

        // GET: Partnership/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var partnership = await _partnershipRepository.GetByIdAsync(id);
            if (partnership == null)
            {
                return NotFound();
            }
            return View(partnership);
        }

        // POST: Partnership/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var partnership = await _partnershipRepository.GetByIdAsync(id);
            if (partnership != null)
            {
                _partnershipRepository.Delete(partnership);
                await _partnershipRepository.SaveAsync();
                TempData["SuccessMessage"] = "تم حذف طلب الشراكة بنجاح.";
            }
            return RedirectToAction("Index");
        }

        // API للحصول على جميع الشراكات (للداشبورد)
        [HttpGet]
        public async Task<IActionResult> GetAllPartnerships()
        {
            var partnerships = await _partnershipRepository.GetAllAsync();
            return Json(partnerships);
        }

        // API لتحديث حالة الشراكة
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, PartnershipStatus status, string? rejectionReason = null, string? adminNotes = null)
        {
            var partnership = await _partnershipRepository.GetByIdAsync(id);
            if (partnership == null)
            {
                return Json(new { success = false, message = "الشراكة غير موجودة" });
            }

            partnership.Status = status;
            partnership.ResponseDate = DateTime.Now;
            partnership.RejectionReason = rejectionReason;
            partnership.AdminNotes = adminNotes;

            _partnershipRepository.Update(partnership);
            await _partnershipRepository.SaveAsync();

            return Json(new { success = true, message = "تم تحديث حالة الشراكة بنجاح" });
        }
    }
}

