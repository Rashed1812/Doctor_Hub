using DAL.Data.Models;
using DAL.Data.Repositories.ConsultationRepo;
using DAL.Data.Repositories.DoctorRequest;
using DAL.Data.Repositories.MedicalRepo;
using DAL.Data.Repositories.PartnershipRepo;
using DAL.Data.Repositories.WhatsAppRepo;
using System.Text.Json;

namespace MyClinc.Services
{
    public class WhatsAppBotService : IWhatsAppBotService
    {
        private readonly IConsultationRepository _consultationRepo;
        private readonly IDoctorJoinRequestRepo _doctorRepo;
        private readonly IPartnershipRepository _partnershipRepo;
        private readonly IMedicalSpecialtyRepository _specialtyRepo;
        private readonly IWhatsAppApiService _whatsAppApiService;
        private readonly IWhatsAppSessionRepository _sessionRepo;
        private readonly IConfiguration _configuration;

        public WhatsAppBotService(
            IConsultationRepository consultationRepo,
            IDoctorJoinRequestRepo doctorRepo,
            IPartnershipRepository partnershipRepo,
            IMedicalSpecialtyRepository specialtyRepo,
            IWhatsAppApiService whatsAppApiService,
            IWhatsAppSessionRepository sessionRepo,
            IConfiguration configuration)
        {
            _consultationRepo = consultationRepo;
            _doctorRepo = doctorRepo;
            _partnershipRepo = partnershipRepo;
            _specialtyRepo = specialtyRepo;
            _whatsAppApiService = whatsAppApiService;
            _sessionRepo = sessionRepo;
            _configuration = configuration;
        }

        public async Task<bool> SendWelcomeMessage(string phoneNumber)
        {
            var welcomeMessage = @"مرحباً بك في عيادة MyClinc! 🏥

يمكنني مساعدتك في:
1️⃣ حجز استشارة طبية
2️⃣ التقديم للانضمام كطبيب
3️⃣ طلب شراكة تجارية

أرسل الرقم المطلوب للبدء:
1 - حجز استشارة
2 - انضمام كطبيب  
3 - طلب شراكة";

            // هنا يتم إرسال الرسالة عبر WhatsApp API
            return await SendWhatsAppMessage(phoneNumber, welcomeMessage);
        }

        public async Task<bool> ProcessConsultationForm(string phoneNumber, string message)
        {
            var session = await GetOrCreateSessionAsync(phoneNumber);
            session.CurrentFormType = "consultation";
            session.LastActivity = DateTime.Now;

            var formData = GetFormData<ConsultationFormData>(session);

            switch (session.CurrentStep)
            {
                case 0: // اختيار نوع الاستشارة
                    return await HandleConsultationSpecialtySelection(phoneNumber, message, session, formData);
                case 1: // الاسم الكامل
                    return await HandleConsultationName(phoneNumber, message, session, formData);
                case 2: // رقم الهاتف
                    return await HandleConsultationPhone(phoneNumber, message, session, formData);
                case 3: // تاريخ الميلاد
                    return await HandleConsultationBirthDate(phoneNumber, message, session, formData);
                case 4: // بلد الولادة
                    return await HandleConsultationBirthCountry(phoneNumber, message, session, formData);
                case 5: // بلد السكن الحالي
                    return await HandleConsultationCurrentCountry(phoneNumber, message, session, formData);
                case 6: // معلومات إضافية
                    return await HandleConsultationAdditionalInfo(phoneNumber, message, session, formData);
                case 7: // تأكيد الحجز
                    return await HandleConsultationConfirmation(phoneNumber, message, session, formData);
                default:
                    return await SendFormInstructions(phoneNumber, "consultation");
            }
        }

        public async Task<bool> ProcessDoctorJoinForm(string phoneNumber, string message)
        {
            var session = await GetOrCreateSessionAsync(phoneNumber);
            session.CurrentFormType = "doctor";
            session.LastActivity = DateTime.Now;

            var formData = GetFormData<DoctorJoinFormData>(session);

            switch (session.CurrentStep)
            {
                case 0: // الاسم الكامل
                    return await HandleDoctorName(phoneNumber, message, session, formData);
                case 1: // البريد الإلكتروني
                    return await HandleDoctorEmail(phoneNumber, message, session, formData);
                case 2: // رقم الهاتف
                    return await HandleDoctorPhone(phoneNumber, message, session, formData);
                case 3: // رقم الترخيص
                    return await HandleDoctorLicense(phoneNumber, message, session, formData);
                case 4: // سنوات الخبرة
                    return await HandleDoctorExperience(phoneNumber, message, session, formData);
                case 5: // المؤهل التعليمي
                    return await HandleDoctorEducation(phoneNumber, message, session, formData);
                case 6: // النبذة الشخصية
                    return await HandleDoctorBiography(phoneNumber, message, session, formData);
                case 7: // تأكيد التقديم
                    return await HandleDoctorConfirmation(phoneNumber, message, session, formData);
                default:
                    return await SendFormInstructions(phoneNumber, "doctor");
            }
        }

        public async Task<bool> ProcessPartnershipForm(string phoneNumber, string message)
        {
            var session = await GetOrCreateSessionAsync(phoneNumber);
            session.CurrentFormType = "partnership";
            session.LastActivity = DateTime.Now;

            var formData = GetFormData<PartnershipFormData>(session);

            switch (session.CurrentStep)
            {
                case 0: // اسم الشركة
                    return await HandlePartnershipCompanyName(phoneNumber, message, session, formData);
                case 1: // اسم الشخص المسؤول
                    return await HandlePartnershipContactPerson(phoneNumber, message, session, formData);
                case 2: // المنصب
                    return await HandlePartnershipPosition(phoneNumber, message, session, formData);
                case 3: // البريد الإلكتروني
                    return await HandlePartnershipEmail(phoneNumber, message, session, formData);
                case 4: // رقم الهاتف
                    return await HandlePartnershipPhone(phoneNumber, message, session, formData);
                case 5: // عنوان الشركة
                    return await HandlePartnershipAddress(phoneNumber, message, session, formData);
                case 6: // نوع الشراكة
                    return await HandlePartnershipType(phoneNumber, message, session, formData);
                case 7: // تفاصيل الشراكة
                    return await HandlePartnershipDetails(phoneNumber, message, session, formData);
                case 8: // تأكيد الطلب
                    return await HandlePartnershipConfirmation(phoneNumber, message, session, formData);
                default:
                    return await SendFormInstructions(phoneNumber, "partnership");
            }
        }

        public async Task<bool> SendFormInstructions(string phoneNumber, string formType)
        {
            string instructions = formType switch
            {
                "consultation" => @"حجز استشارة طبية 📋

أولاً، اختر نوع الاستشارة:
1️⃣ طب عام
2️⃣ طب أسنان
3️⃣ طب نفسي
4️⃣ طب أطفال
5️⃣ طب النساء
6️⃣ طب القلب
7️⃣ طب الجراحة

أرسل الرقم المطلوب:",
                "doctor" => @"انضمام كطبيب 👨‍⚕️

أولاً، أدخل اسمك الكامل:",
                "partnership" => @"طلب شراكة تجارية 🤝

أولاً، أدخل اسم الشركة:",
                _ => "نوع النموذج غير معروف"
            };

            return await SendWhatsAppMessage(phoneNumber, instructions);
        }

        public async Task<bool> SendConfirmationMessage(string phoneNumber, string formType)
        {
            string confirmation = formType switch
            {
                "consultation" => @"✅ تم حجز استشارتك بنجاح!

سيتم التواصل معك قريباً لتأكيد الموعد.
شكراً لثقتك في عيادة MyClinc! 🏥",
                "doctor" => @"✅ تم استلام طلب انضمامك بنجاح!

سيتم مراجعة طلبك والرد عليك خلال 48 ساعة.
شكراً لاهتمامك بالانضمام لفريقنا! 👨‍⚕️",
                "partnership" => @"✅ تم استلام طلب الشراكة بنجاح!

سيتم مراجعة طلبك والرد عليك خلال 24 ساعة.
شكراً لاهتمامك بالشراكة معنا! 🤝",
                _ => "تم إرسال طلبك بنجاح!"
            };

            return await SendWhatsAppMessage(phoneNumber, confirmation);
        }

        // Helper methods
        private async Task<WhatsAppUserSession> GetOrCreateSessionAsync(string phoneNumber)
        {
            var session = await _sessionRepo.GetByPhoneNumberAsync(phoneNumber);
            
            if (session == null)
            {
                session = new WhatsAppUserSession
                {
                    PhoneNumber = phoneNumber,
                    CurrentStep = 0,
                    LastActivity = DateTime.Now,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                await _sessionRepo.AddAsync(session);
            }
            else
            {
                session.LastActivity = DateTime.Now;
                _sessionRepo.Update(session);
            }
            
            await _sessionRepo.SaveAsync();
            return session;
        }

        private T GetFormData<T>(WhatsAppUserSession session) where T : class, new()
        {
            if (string.IsNullOrEmpty(session.FormData))
            {
                return new T();
            }
            return JsonSerializer.Deserialize<T>(session.FormData) ?? new T();
        }

        private void SaveFormData<T>(WhatsAppUserSession session, T data)
        {
            session.FormData = JsonSerializer.Serialize(data);
        }

        // Consultation form handlers
        private async Task<bool> HandleConsultationSpecialtySelection(string phoneNumber, string message, WhatsAppUserSession session, ConsultationFormData formData)
        {
            if (int.TryParse(message, out int specialtyId) && specialtyId >= 1 && specialtyId <= 7)
            {
                formData.MedicalSpecialtyId = specialtyId;
                SaveFormData(session, formData);
                session.CurrentStep = 1;

                return await SendWhatsAppMessage(phoneNumber, "أدخل اسمك الكامل:");
            }
            return await SendWhatsAppMessage(phoneNumber, "يرجى إدخال رقم صحيح من 1 إلى 7:");
        }

        private async Task<bool> HandleConsultationName(string phoneNumber, string message, WhatsAppUserSession session, ConsultationFormData formData)
        {
            formData.PatientName = message;
            SaveFormData(session, formData);
            session.CurrentStep = 2;

            return await SendWhatsAppMessage(phoneNumber, "أدخل رقم هاتفك (مثال: +966501234567):");
        }

        private async Task<bool> HandleConsultationPhone(string phoneNumber, string message, WhatsAppUserSession session, ConsultationFormData formData)
        {
            formData.FullPhoneNumber = message;
            SaveFormData(session, formData);
            session.CurrentStep = 3;

            return await SendWhatsAppMessage(phoneNumber, "أدخل تاريخ ميلادك (مثال: 1990-01-01):");
        }

        private async Task<bool> HandleConsultationBirthDate(string phoneNumber, string message, WhatsAppUserSession session, ConsultationFormData formData)
        {
            if (DateTime.TryParse(message, out DateTime birthDate))
            {
                formData.BirthDate = birthDate;
                SaveFormData(session, formData);
                session.CurrentStep = 4;

                return await SendWhatsAppMessage(phoneNumber, "أدخل بلد الولادة:");
            }
            return await SendWhatsAppMessage(phoneNumber, "يرجى إدخال تاريخ صحيح (مثال: 1990-01-01):");
        }

        private async Task<bool> HandleConsultationBirthCountry(string phoneNumber, string message, WhatsAppUserSession session, ConsultationFormData formData)
        {
            formData.BirthCountry = message;
            SaveFormData(session, formData);
            session.CurrentStep = 5;

            return await SendWhatsAppMessage(phoneNumber, "أدخل بلد السكن الحالي:");
        }

        private async Task<bool> HandleConsultationCurrentCountry(string phoneNumber, string message, WhatsAppUserSession session, ConsultationFormData formData)
        {
            formData.CurrentCountry = message;
            SaveFormData(session, formData);
            session.CurrentStep = 6;

            return await SendWhatsAppMessage(phoneNumber, "هل لديك أي معلومات إضافية تريد إضافتها؟ (أو اكتب 'لا' للمتابعة):");
        }

        private async Task<bool> HandleConsultationAdditionalInfo(string phoneNumber, string message, WhatsAppUserSession session, ConsultationFormData formData)
        {
            if (message.ToLower() != "لا")
            {
                formData.AdditionalInfo = message;
            }
            SaveFormData(session, formData);
            session.CurrentStep = 7;

            var summary = $"📋 ملخص الحجز:\n\n" +
                         $"الاسم: {formData.PatientName}\n" +
                         $"الهاتف: {formData.FullPhoneNumber}\n" +
                         $"تاريخ الميلاد: {formData.BirthDate:yyyy-MM-dd}\n" +
                         $"بلد الولادة: {formData.BirthCountry}\n" +
                         $"بلد السكن: {formData.CurrentCountry}\n\n" +
                         $"هل تريد تأكيد الحجز؟\n" +
                         $"اكتب 'نعم' للتأكيد أو 'لا' للإلغاء:";

            return await SendWhatsAppMessage(phoneNumber, summary);
        }

        private async Task<bool> HandleConsultationConfirmation(string phoneNumber, string message, WhatsAppUserSession session, ConsultationFormData formData)
        {
            if (message.ToLower() == "نعم")
            {
                // Save to database
                var consultation = new Consultation
                {
                    PatientName = formData.PatientName,
                    FullPhoneNumber = formData.FullPhoneNumber,
                    BirthDate = formData.BirthDate,
                    BirthCountry = formData.BirthCountry,
                    CurrentCountry = formData.CurrentCountry,
                    MedicalSpecialtyId = formData.MedicalSpecialtyId,
                    BookingDate = DateTime.Now,
                    Status = "قيد المراجعة",
                    PaymentStatus = PaymentStatus.Pending
                };

                await _consultationRepo.AddAsync(consultation);
                await _consultationRepo.SaveAsync();

                // Clear session
                session.IsActive = false;
                _sessionRepo.Update(session);
                await _sessionRepo.SaveAsync();

                return await SendConfirmationMessage(phoneNumber, "consultation");
            }
            else
            {
                session.IsActive = false;
                _sessionRepo.Update(session);
                await _sessionRepo.SaveAsync();
                return await SendWhatsAppMessage(phoneNumber, "تم إلغاء الحجز. يمكنك البدء من جديد بإرسال '1' لحجز استشارة.");
            }
        }

        // Doctor join form handlers
        private async Task<bool> HandleDoctorName(string phoneNumber, string message, WhatsAppUserSession session, DoctorJoinFormData formData)
        {
            formData.FullName = message;
            SaveFormData(session, formData);
            session.CurrentStep = 1;

            return await SendWhatsAppMessage(phoneNumber, "أدخل بريدك الإلكتروني:");
        }

        private async Task<bool> HandleDoctorEmail(string phoneNumber, string message, WhatsAppUserSession session, DoctorJoinFormData formData)
        {
            formData.Email = message;
            SaveFormData(session, formData);
            session.CurrentStep = 2;

            return await SendWhatsAppMessage(phoneNumber, "أدخل رقم هاتفك:");
        }

        private async Task<bool> HandleDoctorPhone(string phoneNumber, string message, WhatsAppUserSession session, DoctorJoinFormData formData)
        {
            formData.PhoneNumber = message;
            SaveFormData(session, formData);
            session.CurrentStep = 3;

            return await SendWhatsAppMessage(phoneNumber, "أدخل رقم الترخيص:");
        }

        private async Task<bool> HandleDoctorLicense(string phoneNumber, string message, WhatsAppUserSession session, DoctorJoinFormData formData)
        {
            formData.LicenseNumber = message;
            SaveFormData(session, formData);
            session.CurrentStep = 4;

            return await SendWhatsAppMessage(phoneNumber, "أدخل سنوات الخبرة (رقم):");
        }

        private async Task<bool> HandleDoctorExperience(string phoneNumber, string message, WhatsAppUserSession session, DoctorJoinFormData formData)
        {
            if (int.TryParse(message, out int experience))
            {
                formData.ExperienceYears = experience;
                SaveFormData(session, formData);
                session.CurrentStep = 5;

                return await SendWhatsAppMessage(phoneNumber, "أدخل المؤهل التعليمي:");
            }
            return await SendWhatsAppMessage(phoneNumber, "يرجى إدخال رقم صحيح:");
        }

        private async Task<bool> HandleDoctorEducation(string phoneNumber, string message, WhatsAppUserSession session, DoctorJoinFormData formData)
        {
            formData.EducationalDegree = message;
            SaveFormData(session, formData);
            session.CurrentStep = 6;

            return await SendWhatsAppMessage(phoneNumber, "أدخل نبذة شخصية عنك:");
        }

        private async Task<bool> HandleDoctorBiography(string phoneNumber, string message, WhatsAppUserSession session, DoctorJoinFormData formData)
        {
            formData.Biography = message;
            SaveFormData(session, formData);
            session.CurrentStep = 7;

            var summary = $"📋 ملخص الطلب:\n\n" +
                         $"الاسم: {formData.FullName}\n" +
                         $"البريد الإلكتروني: {formData.Email}\n" +
                         $"الهاتف: {formData.PhoneNumber}\n" +
                         $"رقم الترخيص: {formData.LicenseNumber}\n" +
                         $"سنوات الخبرة: {formData.ExperienceYears}\n" +
                         $"المؤهل: {formData.EducationalDegree}\n\n" +
                         $"هل تريد تأكيد الطلب؟\n" +
                         $"اكتب 'نعم' للتأكيد أو 'لا' للإلغاء:";

            return await SendWhatsAppMessage(phoneNumber, summary);
        }

        private async Task<bool> HandleDoctorConfirmation(string phoneNumber, string message, WhatsAppUserSession session, DoctorJoinFormData formData)
        {
            if (message.ToLower() == "نعم")
            {
                // Save to database
                var doctorRequest = new DoctorJoinRequest
                {
                    FullName = formData.FullName,
                    Email = formData.Email,
                    PhoneNumber = formData.PhoneNumber,
                    LicenseNumber = formData.LicenseNumber,
                    ExperienceYears = formData.ExperienceYears,
                    EducationalDegree = formData.EducationalDegree,
                    Biography = formData.Biography,
                    IsTermsAccepted = true,
                    Status = JoinRequestStatus.Pending,
                    SubmissionDate = DateTime.Now
                };

                await _doctorRepo.AddAsync(doctorRequest);
                await _doctorRepo.SaveAsync();

                // Clear session
                session.IsActive = false;
                _sessionRepo.Update(session);
                await _sessionRepo.SaveAsync();

                return await SendConfirmationMessage(phoneNumber, "doctor");
            }
            else
            {
                session.IsActive = false;
                _sessionRepo.Update(session);
                await _sessionRepo.SaveAsync();
                return await SendWhatsAppMessage(phoneNumber, "تم إلغاء الطلب. يمكنك البدء من جديد بإرسال '2' للانضمام كطبيب.");
            }
        }

        // Partnership form handlers
        private async Task<bool> HandlePartnershipCompanyName(string phoneNumber, string message, WhatsAppUserSession session, PartnershipFormData formData)
        {
            formData.CompanyName = message;
            SaveFormData(session, formData);
            session.CurrentStep = 1;

            return await SendWhatsAppMessage(phoneNumber, "أدخل اسم الشخص المسؤول:");
        }

        private async Task<bool> HandlePartnershipContactPerson(string phoneNumber, string message, WhatsAppUserSession session, PartnershipFormData formData)
        {
            formData.ContactPersonName = message;
            SaveFormData(session, formData);
            session.CurrentStep = 2;

            return await SendWhatsAppMessage(phoneNumber, "أدخل المنصب:");
        }

        private async Task<bool> HandlePartnershipPosition(string phoneNumber, string message, WhatsAppUserSession session, PartnershipFormData formData)
        {
            formData.Position = message;
            SaveFormData(session, formData);
            session.CurrentStep = 3;

            return await SendWhatsAppMessage(phoneNumber, "أدخل البريد الإلكتروني:");
        }

        private async Task<bool> HandlePartnershipEmail(string phoneNumber, string message, WhatsAppUserSession session, PartnershipFormData formData)
        {
            formData.Email = message;
            SaveFormData(session, formData);
            session.CurrentStep = 4;

            return await SendWhatsAppMessage(phoneNumber, "أدخل رقم الهاتف:");
        }

        private async Task<bool> HandlePartnershipPhone(string phoneNumber, string message, WhatsAppUserSession session, PartnershipFormData formData)
        {
            formData.FullPhoneNumber = message;
            SaveFormData(session, formData);
            session.CurrentStep = 5;

            return await SendWhatsAppMessage(phoneNumber, "أدخل عنوان الشركة:");
        }

        private async Task<bool> HandlePartnershipAddress(string phoneNumber, string message, WhatsAppUserSession session, PartnershipFormData formData)
        {
            formData.CompanyAddress = message;
            SaveFormData(session, formData);
            session.CurrentStep = 6;

            return await SendWhatsAppMessage(phoneNumber, "أدخل نوع الشراكة المطلوبة:");
        }

        private async Task<bool> HandlePartnershipType(string phoneNumber, string message, WhatsAppUserSession session, PartnershipFormData formData)
        {
            formData.PartnershipType = message;
            SaveFormData(session, formData);
            session.CurrentStep = 7;

            return await SendWhatsAppMessage(phoneNumber, "أدخل تفاصيل الشراكة المطلوبة:");
        }

        private async Task<bool> HandlePartnershipDetails(string phoneNumber, string message, WhatsAppUserSession session, PartnershipFormData formData)
        {
            formData.PartnershipDetails = message;
            SaveFormData(session, formData);
            session.CurrentStep = 8;

            var summary = $"📋 ملخص طلب الشراكة:\n\n" +
                         $"اسم الشركة: {formData.CompanyName}\n" +
                         $"الشخص المسؤول: {formData.ContactPersonName}\n" +
                         $"المنصب: {formData.Position}\n" +
                         $"البريد الإلكتروني: {formData.Email}\n" +
                         $"الهاتف: {formData.FullPhoneNumber}\n" +
                         $"العنوان: {formData.CompanyAddress}\n" +
                         $"نوع الشراكة: {formData.PartnershipType}\n\n" +
                         $"هل تريد تأكيد الطلب؟\n" +
                         $"اكتب 'نعم' للتأكيد أو 'لا' للإلغاء:";

            return await SendWhatsAppMessage(phoneNumber, summary);
        }

        private async Task<bool> HandlePartnershipConfirmation(string phoneNumber, string message, WhatsAppUserSession session, PartnershipFormData formData)
        {
            if (message.ToLower() == "نعم")
            {
                // Save to database
                var partnership = new Partnership
                {
                    CompanyName = formData.CompanyName,
                    ContactPersonName = formData.ContactPersonName,
                    Position = formData.Position,
                    Email = formData.Email,
                    FullPhoneNumber = formData.FullPhoneNumber,
                    CompanyAddress = formData.CompanyAddress,
                    PartnershipType = formData.PartnershipType,
                    PartnershipDetails = formData.PartnershipDetails,
                    SubmissionDate = DateTime.Now,
                    Status = PartnershipStatus.Pending
                };

                await _partnershipRepo.AddAsync(partnership);
                await _partnershipRepo.SaveAsync();

                // Clear session
                session.IsActive = false;
                _sessionRepo.Update(session);
                await _sessionRepo.SaveAsync();

                return await SendConfirmationMessage(phoneNumber, "partnership");
            }
            else
            {
                session.IsActive = false;
                _sessionRepo.Update(session);
                await _sessionRepo.SaveAsync();
                return await SendWhatsAppMessage(phoneNumber, "تم إلغاء الطلب. يمكنك البدء من جديد بإرسال '3' لطلب شراكة.");
            }
        }

        // WhatsApp API integration
        private async Task<bool> SendWhatsAppMessage(string phoneNumber, string message)
        {
            return await _whatsAppApiService.SendMessage(phoneNumber, message);
        }

        public async Task<bool> VerifyWebhook(string mode, string token, string challenge)
        {
            return await _whatsAppApiService.VerifyWebhook(mode, token, challenge);
        }
    }

    // Form data classes
    public class ConsultationFormData
    {
        public string PatientName { get; set; } = "";
        public string FullPhoneNumber { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string BirthCountry { get; set; } = "";
        public string CurrentCountry { get; set; } = "";
        public int MedicalSpecialtyId { get; set; }
        public string? AdditionalInfo { get; set; }
    }

    public class DoctorJoinFormData
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string LicenseNumber { get; set; } = "";
        public int ExperienceYears { get; set; }
        public string EducationalDegree { get; set; } = "";
        public string Biography { get; set; } = "";
    }

    public class PartnershipFormData
    {
        public string CompanyName { get; set; } = "";
        public string ContactPersonName { get; set; } = "";
        public string Position { get; set; } = "";
        public string Email { get; set; } = "";
        public string FullPhoneNumber { get; set; } = "";
        public string CompanyAddress { get; set; } = "";
        public string PartnershipType { get; set; } = "";
        public string PartnershipDetails { get; set; } = "";
    }
} 