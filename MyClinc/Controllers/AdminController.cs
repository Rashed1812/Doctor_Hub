using DAL.Data.Models;
using DAL.Data.Repositories.ConsultationRepo;
using DAL.Data.Repositories.DoctorRequest;
using DAL.Data.Repositories.MedicalRepo;
using DAL.Data.Repositories.PartnershipRepo;
using Microsoft.AspNetCore.Mvc;
using MyClinc.Services;

namespace MyClinc.Controllers
{
    public class AdminController : Controller
    {
        private readonly IDoctorJoinRequestRepo _doctorRepo;
        private readonly IMedicalSpecialtyRepository _specialtyRepo;
        private readonly IConsultationRepository _consultationRepo;
        private readonly IPartnershipRepository _partnershipRepo;
        private readonly IPdfService _pdfService;
        
        // كلمة المرور الثابتة للمسؤول (يمكن تغييرها من هنا)
        private const string ADMIN_PASSWORD = "Admin@2025";

        public AdminController(IDoctorJoinRequestRepo doctorRepo, IMedicalSpecialtyRepository specialtyRepo, IConsultationRepository consultationRepo, IPartnershipRepository partnershipRepo, IPdfService pdfService)
        {
            _doctorRepo = doctorRepo;
            _specialtyRepo = specialtyRepo;
            _consultationRepo = consultationRepo;
            _partnershipRepo = partnershipRepo;
            _pdfService = pdfService;
        }

        // التحقق من تسجيل دخول المسؤول
        private bool IsAdminLoggedIn()
        {
            return HttpContext.Session.GetString("IsAdmin") == "true";
        }

        // إعادة توجيه لصفحة تسجيل الدخول إذا لم يكن مسجل دخول
        private IActionResult RedirectToLoginIfNotAdmin()
        {
            if (!IsAdminLoggedIn())
            {
                return RedirectToAction("Login");
            }
            return null;
        }

        // صفحة تسجيل الدخول
        [HttpGet]
        public IActionResult Login()
        {
            // إذا كان مسجل دخول بالفعل، توجه للوحة التحكم
            if (IsAdminLoggedIn())
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        // معالجة تسجيل الدخول
        [HttpPost]
        public IActionResult Login(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                TempData["ErrorMessage"] = "يرجى إدخال كلمة المرور";
                return View();
            }

            if (password == ADMIN_PASSWORD)
            {
                // تسجيل الدخول بنجاح
                HttpContext.Session.SetString("IsAdmin", "true");
                HttpContext.Session.SetString("LoginTime", DateTime.Now.ToString());
                TempData["SuccessMessage"] = "تم تسجيل الدخول بنجاح";
                return RedirectToAction("Dashboard");
            }
            else
            {
                // كلمة مرور خاطئة
                TempData["ErrorMessage"] = "كلمة المرور غير صحيحة";
                return View();
            }
        }

        // تسجيل الخروج
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "تم تسجيل الخروج بنجاح";
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Dashboard()
        {
            var redirectResult = RedirectToLoginIfNotAdmin();
            if (redirectResult != null) return redirectResult;

            var doctorRequests = await _doctorRepo.GetAllAsync();
            var specialties = await _specialtyRepo.GetAllAsync();
            var consultations = await _consultationRepo.GetAllAsync();
            var partnerships = await _partnershipRepo.GetAllAsync();

            ViewBag.TotalDoctorRequests = doctorRequests.Count();
            ViewBag.PendingRequests = doctorRequests.Count(r => r.Status == JoinRequestStatus.Pending);
            ViewBag.ApprovedRequests = doctorRequests.Count(r => r.Status == JoinRequestStatus.Approved);
            ViewBag.TotalSpecialties = specialties.Count();
            ViewBag.TotalConsultations = consultations.Count();
            ViewBag.PaidConsultations = consultations.Count(c => c.PaymentStatus == PaymentStatus.Paid);
            ViewBag.TotalPartnerships = partnerships.Count();
            ViewBag.PendingPartnerships = partnerships.Count(p => p.Status == PartnershipStatus.Pending);

            return View();
        }

        public async Task<IActionResult> DoctorRequests()
        {
            var redirectResult = RedirectToLoginIfNotAdmin();
            if (redirectResult != null) return redirectResult;
            
            var requests = await _doctorRepo.GetAllAsync();
            return View(requests);
        }

        public async Task<IActionResult> Specialties()
        {
            var redirectResult = RedirectToLoginIfNotAdmin();
            if (redirectResult != null) return redirectResult;
            
            var specialties = await _specialtyRepo.GetAllAsync();
            return View(specialties);
        }

        public async Task<IActionResult> Consultations()
        {
            var redirectResult = RedirectToLoginIfNotAdmin();
            if (redirectResult != null) return redirectResult;
            
            var consultations = await _consultationRepo.GetAllAsync();
            return View(consultations);
        }

        public async Task<IActionResult> Partnerships()
        {
            var redirectResult = RedirectToLoginIfNotAdmin();
            if (redirectResult != null) return redirectResult;
            
            var partnerships = await _partnershipRepo.GetAllAsync();
            ViewBag.TotalPartnerships = partnerships.Count();
            ViewBag.PendingPartnerships = partnerships.Count(p => p.Status == PartnershipStatus.Pending);
            ViewBag.ApprovedPartnerships = partnerships.Count(p => p.Status == PartnershipStatus.Approved);
            ViewBag.RejectedPartnerships = partnerships.Count(p => p.Status == PartnershipStatus.Rejected);
            return View(partnerships);
        }

        public async Task<IActionResult> PartnershipDetails(int id)
        {
            var redirectResult = RedirectToLoginIfNotAdmin();
            if (redirectResult != null) return redirectResult;
            
            var partnership = await _partnershipRepo.GetByIdAsync(id);
            if (partnership == null)
            {
                return NotFound();
            }
            return View(partnership);
        }

        public async Task<IActionResult> Invoices()
        {
            var redirectResult = RedirectToLoginIfNotAdmin();
            if (redirectResult != null) return redirectResult;
            
            var paidConsultations = (await _consultationRepo.GetAllAsync()).Where(c => c.PaymentStatus == PaymentStatus.Paid).ToList();
            ViewBag.TotalInvoices = paidConsultations.Count();
            ViewBag.PaidInvoices = paidConsultations.Count(); // All are paid in this view
            ViewBag.TotalAmount = paidConsultations.Sum(c => c.ConsultationFee);
            ViewBag.ThisMonthInvoices = paidConsultations.Count(c => c.BookingDate.Month == DateTime.Now.Month && c.BookingDate.Year == DateTime.Now.Year);
            return View(paidConsultations);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateInvoicePDF([FromBody] ConsultationIdRequest request)
        {
            try
            {
                var consultation = await _consultationRepo.GetByIdAsync(request.ConsultationId);
                if (consultation == null)
                {
                    return Json(new { success = false, message = "الاستشارة غير موجودة" });
                }

                // Generate PDF invoice
                var pdfBytes = await _pdfService.GenerateInvoicePdfAsync(consultation);
                var fileName = $"Invoice-INV-{request.ConsultationId:D4}-{consultation.PatientName}.pdf";

                // Return PDF as base64 for download
                var base64Pdf = Convert.ToBase64String(pdfBytes);
                
                return Json(new
                {
                    success = true,
                    message = "تم إنشاء الفاتورة بنجاح",
                    pdfData = base64Pdf,
                    fileName = fileName
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء إنشاء الفاتورة: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GenerateInvoiceHTML([FromBody] ConsultationIdRequest request)
        {
            try
            {
                var consultation = await _consultationRepo.GetByIdAsync(request.ConsultationId);
                if (consultation == null)
                {
                    return Json(new { success = false, message = "الاستشارة غير موجودة" });
                }

                // Generate HTML invoice content
                var invoiceHtml = GenerateInvoiceHtml(consultation);

                return Json(new
                {
                    success = true,
                    message = "تم إنشاء الفاتورة بنجاح",
                    invoiceHtml = invoiceHtml,
                    fileName = $"Invoice-INV-{request.ConsultationId:D4}-{consultation.PatientName}.html"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء إنشاء الفاتورة" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> InvoiceDisplay(int consultationId)
        {
            var consultation = await _consultationRepo.GetByIdAsync(consultationId);
            if (consultation == null)
            {
                return NotFound();
            }

            return View(consultation);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadInvoicePDF(int consultationId)
        {
            try
            {
                var consultation = await _consultationRepo.GetByIdAsync(consultationId);
                if (consultation == null)
                {
                    return NotFound();
                }

                var pdfBytes = await _pdfService.GenerateInvoicePdfAsync(consultation);
                var fileName = $"Invoice-INV-{consultationId:D4}-{consultation.PatientName}.pdf";

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل الفاتورة: " + ex.Message;
                return RedirectToAction("Invoices");
            }
        }

        [HttpGet]
        public async Task<IActionResult> DownloadConfirmationPDF(int consultationId)
        {
            try
            {
                var consultation = await _consultationRepo.GetByIdAsync(consultationId);
                if (consultation == null)
                {
                    return NotFound();
                }

                var pdfBytes = await _pdfService.GenerateConfirmationPdfAsync(consultation);
                var fileName = $"Confirmation-{consultationId:D4}-{consultation.PatientName}.pdf";

                return File(pdfBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "حدث خطأ أثناء تحميل رسالة التأكيد: " + ex.Message;
                return RedirectToAction("Consultations");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendInvoiceWhatsApp([FromBody] ConsultationIdRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Json(new { success = false, message = "بيانات الطلب فارغة." });
                }

                var consultation = await _consultationRepo.GetByIdAsync(request.ConsultationId);
                if (consultation == null)
                {
                    return Json(new { success = false, message = "الاستشارة غير موجودة" });
                }

                // تحقق إضافي: تأكد أن رقم الهاتف ليس فارغاً أو مسافات فقط
                var phoneToUse = !string.IsNullOrWhiteSpace(consultation.FullPhoneNumber) 
                    ? consultation.FullPhoneNumber 
                    : consultation.PhoneNumber;

                if (string.IsNullOrWhiteSpace(phoneToUse))
                {
                    return Json(new { success = false, message = "رقم الهاتف غير موجود أو فارغ للاستشارة." });
                }

                // تنظيف رقم الهاتف: إزالة أي رموز غير الأرقام أو +
                var cleanPhoneNumber = new string(phoneToUse
                    .Where(c => char.IsDigit(c) || c == '+')
                    .ToArray());

                // تحقق إضافي: تأكد أن رقم الهاتف ليس فارغاً بعد التنظيف
                if (string.IsNullOrEmpty(cleanPhoneNumber))
                {
                    return Json(new { success = false, message = "رقم الهاتف غير صالح بعد التنظيف." });
                }

                // تحقق أن الرقم يبدأ بـ "+"، وإن لم يكن كذلك أعد توجيه المستخدم لتنسيق الرقم بشكل صحيح
                if (!cleanPhoneNumber.StartsWith("+"))
                {
                    return Json(new
                    {
                        success = false,
                        message = "يرجى إدخال رقم الهاتف مع كود الدولة، مثلاً: +9647715513344"
                    });
                }

                // تأكد من أن PatientName ليس null
                var patientName = consultation.PatientName ?? "المريض";

                // تأكد من أن MedicalSpecialty.Name ليس null
                var medicalSpecialtyName = consultation.MedicalSpecialty?.Name ?? "غير محدد";

                // إنشاء رسالة واتساب
                var message = $"مرحباً {patientName}،\n\n" +
                              $"تم إنشاء فاتورتكم الطبية:\n" +
                              $"📋 رقم الفاتورة: INV-{request.ConsultationId:D4}\n" +
                              $"🏥 التخصص: {medicalSpecialtyName}\n" +
                              $"📅 تاريخ الحجز: {consultation.BookingDate:dd/MM/yyyy}\n" +
                              $"💰 المبلغ: ${consultation.ConsultationFee}\n" +
                              $"✅ حالة الدفع: مدفوع\n\n" +
                              $"شكراً لثقتكم بـ Doctor Hub 🙏";

                // توليد رابط الواتساب
                var whatsappUrl = $"https://wa.me/{cleanPhoneNumber.Replace("+", "")}?text={Uri.EscapeDataString(message)}";

                return Json(new
                {
                    success = true,
                    message = "تم تحضير رسالة الواتساب",
                    whatsappUrl = whatsappUrl
                });
            }
            catch (Exception ex)
            {
                // يمكنك هنا تسجيل الخطأ في Logs أو System Console
                return Json(new { success = false, message = "حدث خطأ أثناء تحضير رسالة الواتساب." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendConfirmationWhatsApp([FromBody] ConsultationIdRequest request)
        {
            try
            {
                if (request == null)
                {
                    return Json(new { success = false, message = "بيانات الطلب فارغة." });
                }

                var consultation = await _consultationRepo.GetByIdAsync(request.ConsultationId);
                if (consultation == null)
                {
                    return Json(new { success = false, message = "الاستشارة غير موجودة" });
                }

                // تحقق إضافي: تأكد أن رقم الهاتف ليس فارغاً أو مسافات فقط
                var phoneToUse = !string.IsNullOrWhiteSpace(consultation.FullPhoneNumber) 
                    ? consultation.FullPhoneNumber 
                    : consultation.PhoneNumber;

                if (string.IsNullOrWhiteSpace(phoneToUse))
                {
                    return Json(new { success = false, message = "رقم الهاتف غير موجود أو فارغ للاستشارة." });
                }

                // تنظيف رقم الهاتف: إزالة أي رموز غير الأرقام أو +
                var cleanPhoneNumber = new string(phoneToUse
                    .Where(c => char.IsDigit(c) || c == '+')
                    .ToArray());

                // تحقق إضافي: تأكد أن رقم الهاتف ليس فارغاً بعد التنظيف
                if (string.IsNullOrEmpty(cleanPhoneNumber))
                {
                    return Json(new { success = false, message = "رقم الهاتف غير صالح بعد التنظيف." });
                }

                // تحقق أن الرقم يبدأ بـ "+"، وإن لم يكن كذلك أعد توجيه المستخدم لتنسيق الرقم بشكل صحيح
                if (!cleanPhoneNumber.StartsWith("+"))
                {
                    return Json(new
                    {
                        success = false,
                        message = "يرجى إدخال رقم الهاتف مع كود الدولة، مثلاً: +9647715513344"
                    });
                }

                // تأكد من أن PatientName ليس null
                var patientName = consultation.PatientName ?? "المريض";

                // تأكد من أن MedicalSpecialty.Name ليس null
                var medicalSpecialtyName = consultation.MedicalSpecialty?.Name ?? "غير محدد";

                // إنشاء رسالة تأكيد واتساب
                var message = $"مرحباً {patientName}،\n\n" +
                              $"✅ تم تأكيد حجزك بنجاح!\n\n" +
                              $"📋 رقم الحجز: #{request.ConsultationId:D4}\n" +
                              $"🏥 نوع الاستشارة: {medicalSpecialtyName}\n" +
                              $"📅 تاريخ الحجز: {consultation.BookingDate:dd/MM/yyyy}\n" +
                              $"👨‍⚕️ الطبيب: الدكتور محمد أحمد\n" +
                              $"💰 الرسوم: ${consultation.ConsultationFee}\n\n" +
                              $"سيتم التواصل معك قريباً لتحديد موعد الاستشارة.\n\n" +
                              $"شكراً لثقتكم بـ Doctor Hub 🙏";

                // توليد رابط الواتساب
                var whatsappUrl = $"https://wa.me/{cleanPhoneNumber.Replace("+", "")}?text={Uri.EscapeDataString(message)}";

                return Json(new
                {
                    success = true,
                    message = "تم تحضير رسالة التأكيد",
                    whatsappUrl = whatsappUrl
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء تحضير رسالة التأكيد." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePaymentStatus([FromBody] UpdatePaymentStatusRequest request)
        {
            try
            {
                var consultation = await _consultationRepo.GetByIdAsync(request.ConsultationId);
                if (consultation == null)
                {
                    return Json(new { success = false, message = "الاستشارة غير موجودة" });
                }

                consultation.PaymentStatus = request.Status;
                await _consultationRepo.SaveAsync();

                return Json(new
                {
                    success = true,
                    message = "تم تحديث حالة الدفع بنجاح"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء تحديث حالة الدفع" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmConsultation([FromBody] ConsultationIdRequest request)
        {
            try
            {
                var consultation = await _consultationRepo.GetByIdAsync(request.ConsultationId);
                if (consultation == null)
                {
                    return Json(new { success = false, message = "الاستشارة غير موجودة" });
                }

                consultation.Status = "مؤكد";
                await _consultationRepo.SaveAsync();

                // Generate confirmation PDF
                var pdfBytes = await _pdfService.GenerateConfirmationPdfAsync(consultation);
                var fileName = $"Confirmation-{request.ConsultationId:D4}-{consultation.PatientName}.pdf";
                var base64Pdf = Convert.ToBase64String(pdfBytes);

                return Json(new
                {
                    success = true,
                    message = "تم تأكيد الاستشارة بنجاح وإنشاء رسالة التأكيد",
                    pdfData = base64Pdf,
                    fileName = fileName
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء تأكيد الاستشارة: " + ex.Message });
            }
        }

        public IActionResult GenerateInvoice(int consultationId)
        {
            // TODO: Implement PDF generation logic here
            // For now, just a placeholder
            TempData["SuccessMessage"] = $"تم إنشاء الفاتورة للاستشارة رقم {consultationId}";
            return RedirectToAction("Invoices");
        }

        private string GenerateInvoiceHtml(Consultation consultation)
        {
            return $@"
<!DOCTYPE html>
<html dir='rtl' lang='ar'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>فاتورة رقم INV-{consultation.ConsultationId:D4}</title>
    <style>
        body {{ font-family: 'Arial', sans-serif; direction: rtl; margin: 0; padding: 20px; background: #f8f9fa; }}
        .invoice-container {{ max-width: 800px; margin: 0 auto; background: white; border-radius: 10px; box-shadow: 0 0 20px rgba(0,0,0,0.1); overflow: hidden; }}
        .invoice-header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; }}
        .invoice-body {{ padding: 30px; }}
        .invoice-details {{ display: flex; justify-content: space-between; margin-bottom: 30px; }}
        .detail-section {{ flex: 1; margin-left: 20px; }}
        .detail-section h4 {{ color: #333; border-bottom: 2px solid #667eea; padding-bottom: 10px; margin-bottom: 15px; }}
        .detail-item {{ margin-bottom: 10px; }}
        .detail-label {{ font-weight: bold; color: #666; }}
        .detail-value {{ color: #333; }}
        .invoice-table {{ width: 100%; border-collapse: collapse; margin: 20px 0; }}
        .invoice-table th, .invoice-table td {{ padding: 15px; text-align: right; border-bottom: 1px solid #eee; }}
        .invoice-table th {{ background: #f8f9fa; font-weight: bold; color: #333; }}
        .total-section {{ background: #f8f9fa; padding: 20px; border-radius: 8px; margin-top: 20px; }}
        .total-amount {{ font-size: 24px; font-weight: bold; color: #667eea; text-align: center; }}
        .footer {{ text-align: center; margin-top: 30px; padding-top: 20px; border-top: 1px solid #eee; color: #666; }}
        .status-badge {{ display: inline-block; padding: 5px 15px; border-radius: 20px; background: #28a745; color: white; font-size: 12px; }}
        .print-button {{ background: #667eea; color: white; border: none; padding: 10px 20px; border-radius: 5px; cursor: pointer; margin: 20px auto; display: block; font-size: 16px; }}
        .print-button:hover {{ background: #5a6fd8; }}
        @media print {{ .print-button {{ display: none; }} }}
    </style>
</head>
<body>
    <div class='invoice-container'>
        <div class='invoice-header'>
            <h1>فاتورة طبية</h1>
            <h2>رقم الفاتورة: INV-{consultation.ConsultationId:D4}</h2>
            <p>Doctor Hub - عيادة طبية متخصصة</p>
        </div>
        
        <div class='invoice-body'>
            <div class='invoice-details'>
                <div class='detail-section'>
                    <h4>بيانات المريض</h4>
                    <div class='detail-item'>
                        <span class='detail-label'>الاسم:</span>
                        <span class='detail-value'>{consultation.PatientName}</span>
                    </div>
                    <div class='detail-item'>
                        <span class='detail-label'>الجنس:</span>
                        <span class='detail-value'>{consultation.Gender}</span>
                    </div>
                    <div class='detail-item'>
                        <span class='detail-label'>رقم الهاتف:</span>
                        <span class='detail-value'>{consultation.PhoneNumber}</span>
                    </div>
                </div>
                
                <div class='detail-section'>
                    <h4>بيانات الفاتورة</h4>
                    <div class='detail-item'>
                        <span class='detail-label'>تاريخ الإصدار:</span>
                        <span class='detail-value'>{DateTime.Now:dd/MM/yyyy}</span>
                    </div>
                    <div class='detail-item'>
                        <span class='detail-label'>تاريخ الحجز:</span>
                        <span class='detail-value'>{consultation.BookingDate:dd/MM/yyyy}</span>
                    </div>
                    <div class='detail-item'>
                        <span class='detail-label'>حالة الدفع:</span>
                        <span class='detail-value'><span class='status-badge'>✅ مدفوع</span></span>
                    </div>
                </div>
            </div>
            
            <table class='invoice-table'>
                <thead>
                    <tr>
                        <th>الخدمة</th>
                        <th>التخصص</th>
                        <th>الكمية</th>
                        <th>السعر</th>
                        <th>المجموع</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>استشارة طبية</td>
                        <td>{consultation.MedicalSpecialty?.Name ?? "غير محدد"}</td>
                        <td>1</td>
                        <td>${consultation.ConsultationFee}</td>
                        <td>${consultation.ConsultationFee}</td>
                    </tr>
                </tbody>
            </table>
            
            <div class='total-section'>
                <div class='total-amount'>
                    💰 المبلغ الإجمالي: ${consultation.ConsultationFee}
                </div>
            </div>
            
            <button class='print-button' onclick='window.print()'>🖨️ طباعة الفاتورة</button>
            
            <div class='footer'>
                <p>شكراً لثقتكم بـ Doctor Hub</p>
                <p>للاستفسارات: +964 771 551 3344</p>
                <p>البريد الإلكتروني: info@doctorhub.com</p>
                <p>الموقع الإلكتروني: www.doctorhub.com</p>
            </div>
        </div>
    </div>
</body>
</html>";
        }

        [HttpPost]
        public async Task<IActionResult> UpdateConsultationStatus([FromBody] UpdateStatusRequest request)
        {
            try
            {
                var consultation = await _consultationRepo.GetByIdAsync(request.ConsultationId);
                if (consultation == null)
                {
                    return Json(new { success = false, message = "الاستشارة غير موجودة" });
                }

                consultation.Status = request.Status;
                await _consultationRepo.SaveAsync();

                return Json(new { success = true, message = "تم تحديث حالة الاستشارة بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء تحديث حالة الاستشارة" });
            }
        }

        public async Task<IActionResult> ConsultationDetails(int id)
        {
            var consultation = await _consultationRepo.GetByIdAsync(id);
            if (consultation == null)
            {
                return NotFound();
            }
            return View(consultation);
        }

        public async Task<IActionResult> GenerateConsultationReport(int id)
        {
            var consultation = await _consultationRepo.GetByIdAsync(id);
            if (consultation == null)
            {
                return NotFound();
            }
            return View(consultation);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateConsultationReport([FromBody] Consultation model)
        {

                var consultation = await _consultationRepo.GetByIdAsync(model.ConsultationId);
                if (consultation == null)
                    return Json(new { success = false });

                consultation.Diagnosis = model.Diagnosis;
                consultation.RecommendedTreatment = model.RecommendedTreatment;

                _consultationRepo.Update(consultation);
                await _consultationRepo.SaveAsync();

                return Json(new { success = true });
        }



        public async Task<IActionResult> DoctorDetails(int id)
        {
            var doctor = await _doctorRepo.GetByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            return View("DoctorDetails", doctor);
        }

        public async Task<IActionResult> PatientDetails(int id)
        {
            // Assuming patient details are part of Consultation model for now
            var consultation = await _consultationRepo.GetByIdAsync(id);
            if (consultation == null)
            {
                return NotFound();
            }
            return View(consultation);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveDoctor(int id)
        {
            var doctor = await _doctorRepo.GetByIdAsync(id);
            if (doctor != null)
            {
                doctor.Status = JoinRequestStatus.Approved;
                doctor.JoinDate = DateTime.Now;
                await _doctorRepo.SaveAsync();
                TempData["SuccessMessage"] = "تم قبول طلب الطبيب بنجاح";
            }
            return RedirectToAction("DoctorRequests");
        }

        [HttpPost]
        public async Task<IActionResult> RejectDoctor(int id)
        {
            var doctor = await _doctorRepo.GetByIdAsync(id);
            if (doctor != null)
            {
                doctor.Status = JoinRequestStatus.Rejected;
                await _doctorRepo.SaveAsync();
                TempData["SuccessMessage"] = "تم رفض طلب الطبيب";
            }
            return RedirectToAction("DoctorRequests");
        }

        [HttpPost]
        public async Task<IActionResult> ApprovePartnership(int id)
        {
            var partnership = await _partnershipRepo.GetByIdAsync(id);
            if (partnership != null)
            {
                partnership.Status = PartnershipStatus.Approved;
                await _partnershipRepo.SaveAsync();
                TempData["SuccessMessage"] = "تم قبول طلب الشراكة بنجاح";
            }
            return RedirectToAction("Partnerships");
        }

        [HttpPost]
        public async Task<IActionResult> RejectPartnership(int id, string reason)
        {
            var partnership = await _partnershipRepo.GetByIdAsync(id);
            if (partnership != null)
            {
                partnership.Status = PartnershipStatus.Rejected;
                partnership.RejectionReason = reason;
                await _partnershipRepo.SaveAsync();
                TempData["SuccessMessage"] = "تم رفض طلب الشراكة";
            }
            return RedirectToAction("Partnerships");
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteDoctorRequest(int id)
        {
            try
            {
                var doctorRequest = await _doctorRepo.GetByIdAsync(id);
                if (doctorRequest == null)
                {
                    return Json(new { success = false, message = "طلب الطبيب غير موجود" });
                }

                _doctorRepo.Delete(doctorRequest);
                await _doctorRepo.SaveAsync();

                return Json(new { success = true, message = "تم حذف طلب الطبيب بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء حذف طلب الطبيب: " + ex.Message });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteConsultation(int id)
        {
            try
            {
                var consultation = await _consultationRepo.GetByIdAsync(id);
                if (consultation == null)
                {
                    return Json(new { success = false, message = "الاستشارة غير موجودة" });
                }

                _consultationRepo.Delete(consultation);
                await _consultationRepo.SaveAsync();

                return Json(new { success = true, message = "تم حذف الاستشارة بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ أثناء حذف الاستشارة: " + ex.Message });
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> InvoiceDisplay(int consultationId)
        //{
        //    var consultation = await _consultationRepo.GetByIdAsync(consultationId);
        //    if (consultation == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewBag.InvoiceNumber = $"INV-{consultation.ConsultationId:D4}";
        //    ViewBag.InvoiceDate = consultation.BookingDate.ToString("dd/MM/yyyy");
        //    ViewBag.PaymentStatus = consultation.PaymentStatus == DAL.Data.Models.PaymentStatus.Paid ? "مدفوع" : "غير مدفوع";
        //    ViewBag.PatientName = consultation.PatientName;
        //    ViewBag.Gender = consultation.Gender;
        //    ViewBag.PhoneNumber = consultation.PhoneNumber;
        //    ViewBag.MedicalSpecialty = consultation.MedicalSpecialty?.Name ?? "غير محدد";
        //    ViewBag.BookingDate = consultation.BookingDate.ToString("dd/MM/yyyy");
        //    ViewBag.ConsultationFee = consultation.ConsultationFee;

        //    return View();
        //}

        // WhatsApp Bot Management
        public IActionResult WhatsAppBot()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendTestMessage(string phoneNumber, string message)
        {
            try
            {
                // This would use the WhatsApp bot service
                // For now, just return success
                TempData["SuccessMessage"] = "تم إرسال الرسالة التجريبية بنجاح";
                return RedirectToAction("WhatsAppBot");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"خطأ في إرسال الرسالة: {ex.Message}";
                return RedirectToAction("WhatsAppBot");
            }
        }

        public IActionResult BotSettings()
        {
            return View();
        }

    }
}

// Request models
public class ConsultationIdRequest
{
    public int ConsultationId { get; set; }
}

public class UpdatePaymentStatusRequest
{
    public int ConsultationId { get; set; }
    public PaymentStatus Status { get; set; }
}

public class UpdateStatusRequest
{
    public int ConsultationId { get; set; }
    public string Status { get; set; }
}





