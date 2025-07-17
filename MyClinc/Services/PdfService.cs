using DAL.Data.Models;
using System.Text;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Tables;
using System.Data;

namespace MyClinc.Services
{
    public interface IPdfService
    {
        Task<byte[]> GenerateInvoicePdfAsync(Consultation consultation);
        Task<byte[]> GenerateConfirmationPdfAsync(Consultation consultation);
        string GenerateInvoiceHtml(Consultation consultation);
        string GenerateConfirmationHtml(Consultation consultation);
    }

    public class PdfService : IPdfService
    {
        private readonly IWebHostEnvironment _env;

        public PdfService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<byte[]> GenerateInvoicePdfAsync(Consultation consultation)
        {
            //Create a new PDF document
            PdfDocument document = new PdfDocument();
            //Add a page to the document
            PdfPage page = document.Pages.Add();
            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;

            //Load the font from the file
            FileStream fontStream = new FileStream(Path.Combine(_env.WebRootPath, "fonts", "Amiri-Regular.ttf"), FileMode.Open, FileAccess.Read);
            PdfTrueTypeFont font = new PdfTrueTypeFont(fontStream, 12);

            // Draw header
            graphics.DrawString("Doctor Hub", new PdfTrueTypeFont(fontStream, 24, PdfFontStyle.Bold), PdfBrushes.Blue, new PointF(200, 20));
            graphics.DrawString($"Invoice #INV-{consultation.ConsultationId:D4}", new PdfTrueTypeFont(fontStream, 18, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(200, 60));

            // Patient Information
            graphics.DrawString("Patient Information:", new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(10, 100));
            graphics.DrawString($"Name: {consultation.PatientName ?? "N/A"}", font, PdfBrushes.Black, new PointF(10, 120));
            graphics.DrawString($"Phone: {consultation.FullPhoneNumber ?? consultation.PhoneNumber ?? "N/A"}", font, PdfBrushes.Black, new PointF(10, 140));
            graphics.DrawString($"Gender: {consultation.Gender ?? "Not specified"}", font, PdfBrushes.Black, new PointF(10, 160));
            graphics.DrawString($"Birth Date: {consultation.BirthDate.ToString("dd/MM/yyyy") ?? "Not specified"}", font, PdfBrushes.Black, new PointF(10, 180));

            // Consultation Information
            graphics.DrawString("Consultation Information:", new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(10, 220));
            graphics.DrawString($"Specialty: {consultation.MedicalSpecialty?.Name ?? "Not specified"}", font, PdfBrushes.Black, new PointF(10, 240));
            graphics.DrawString($"Date: {consultation.BookingDate:dd/MM/yyyy}", font, PdfBrushes.Black, new PointF(10, 260));
            graphics.DrawString($"Fee: ${consultation.ConsultationFee}", font, PdfBrushes.Black, new PointF(10, 280));
            graphics.DrawString($"Payment Status: {GetPaymentStatusText(consultation.PaymentStatus)}", font, PdfBrushes.Black, new PointF(10, 300));

            // Total
            graphics.DrawString($"Total Amount: ${consultation.ConsultationFee}", new PdfTrueTypeFont(fontStream, 16, PdfFontStyle.Bold), PdfBrushes.Green, new PointF(200, 340));

            // Footer
            graphics.DrawString("Thank you for choosing Doctor Hub!", new PdfTrueTypeFont(fontStream, 12), PdfBrushes.Gray, new PointF(200, 380));

            //Save the document into stream
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            document.Close(true);

            return stream.ToArray();
        }

        public async Task<byte[]> GenerateConfirmationPdfAsync(Consultation consultation)
        {
            //Create a new PDF document
            PdfDocument document = new PdfDocument();
            //Add a page to the document
            PdfPage page = document.Pages.Add();
            //Create PDF graphics for the page
            PdfGraphics graphics = page.Graphics;

            //Load the font from the file
            FileStream fontStream = new FileStream(Path.Combine(_env.WebRootPath, "fonts", "Amiri-Regular.ttf"), FileMode.Open, FileAccess.Read);
            PdfTrueTypeFont font = new PdfTrueTypeFont(fontStream, 12);

            // Header
            graphics.DrawString("Doctor Hub", new PdfTrueTypeFont(fontStream, 24, PdfFontStyle.Bold), PdfBrushes.Blue, new PointF(200, 20));
            graphics.DrawString("Appointment Confirmation", new PdfTrueTypeFont(fontStream, 18, PdfFontStyle.Bold), PdfBrushes.Green, new PointF(200, 60));

            graphics.DrawString($"Dear {consultation.PatientName ?? "Patient"},", font, PdfBrushes.Black, new PointF(10, 100));
            graphics.DrawString("Your medical appointment has been confirmed successfully!", font, PdfBrushes.Black, new PointF(10, 120));

            // Appointment Details
            graphics.DrawString("Appointment Details:", new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(10, 160));
            graphics.DrawString($"Booking ID: #{consultation.ConsultationId:D4}", font, PdfBrushes.Black, new PointF(10, 180));
            graphics.DrawString($"Date: {consultation.BookingDate:dd/MM/yyyy}", font, PdfBrushes.Black, new PointF(10, 200));
            graphics.DrawString($"Consultation Type: {consultation.MedicalSpecialty?.Name ?? "Not specified"}", font, PdfBrushes.Black, new PointF(10, 220));
            graphics.DrawString($"Fee: ${consultation.ConsultationFee}", font, PdfBrushes.Black, new PointF(10, 240));

            // Doctor Information
            graphics.DrawString("Doctor Information:", new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(10, 280));
            graphics.DrawString("Doctor Name: Dr. Mohammed Ahmed", font, PdfBrushes.Black, new PointF(10, 300));
            graphics.DrawString($"Specialty: {consultation.MedicalSpecialty?.Name ?? "Not specified"}", font, PdfBrushes.Black, new PointF(10, 320));
            graphics.DrawString("Experience: 15+ years", font, PdfBrushes.Black, new PointF(10, 340));

            // Important Notes
            graphics.DrawString("Important Notes:", new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(10, 380));
            graphics.DrawString("• Please arrive 15 minutes before your appointment", font, PdfBrushes.Black, new PointF(10, 400));
            graphics.DrawString("• Bring all previous medical reports", font, PdfBrushes.Black, new PointF(10, 420));
            graphics.DrawString("• Contact us for rescheduling at least 24 hours in advance", font, PdfBrushes.Black, new PointF(10, 440));

            // Contact Information
            graphics.DrawString("Contact Information:", new PdfTrueTypeFont(fontStream, 14, PdfFontStyle.Bold), PdfBrushes.Black, new PointF(10, 480));
            graphics.DrawString("Phone: +9647715513344", font, PdfBrushes.Black, new PointF(10, 500));
            graphics.DrawString("WhatsApp: +9647715513344", font, PdfBrushes.Black, new PointF(10, 520));
            graphics.DrawString("Email: info@doctorhub.com", font, PdfBrushes.Black, new PointF(10, 540));

            //Save the document into stream
            MemoryStream stream = new MemoryStream();
            document.Save(stream);
            document.Close(true);

            return stream.ToArray();
        }

        public string GenerateInvoiceHtml(Consultation consultation)
        {
            var html = new StringBuilder();

            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html lang=\"ar\" dir=\"rtl\">");
            html.AppendLine("<head>");
            html.AppendLine("    <meta charset=\"UTF-8\">");
            html.AppendLine("    <title>فاتورة استشارة</title>");
            html.AppendLine("    <style>");
            html.AppendLine("        body {");
            html.AppendLine("            font-family: \'Amiri\', sans-serif;");
            html.AppendLine("            direction: rtl;");
            html.AppendLine("            text-align: right;");
            html.AppendLine("            font-size: 12px;");
            html.AppendLine("        }");
            html.AppendLine("        .container {");
            html.AppendLine("            width: 80%;");
            html.AppendLine("            margin: auto;");
            html.AppendLine("            padding: 20px;");
            html.AppendLine("            border: 1px solid #eee;");
            html.AppendLine("            box-shadow: 0 0 10px rgba(0,0,0,0.1);");
            html.AppendLine("        }");
            html.AppendLine("        h1, h2, h3 {");
            html.AppendLine("            color: #333;");
            html.AppendLine("        }");
            html.AppendLine("        .header {");
            html.AppendLine("            text-align: center;");
            html.AppendLine("            margin-bottom: 20px;");
            html.AppendLine("        }");
            html.AppendLine("        .details table {");
            html.AppendLine("            width: 100%;");
            html.AppendLine("            border-collapse: collapse;");
            html.AppendLine("            margin-bottom: 20px;");
            html.AppendLine("        }");
            html.AppendLine("        .details th, .details td {");
            html.AppendLine("            border: 1px solid #ddd;");
            html.AppendLine("            padding: 8px;");
            html.AppendLine("            text-align: right;");
            html.AppendLine("        }");
            html.AppendLine("        .footer {");
            html.AppendLine("            text-align: center;");
            html.AppendLine("            margin-top: 30px;");
            html.AppendLine("            font-size: 10px;");
            html.AppendLine("            color: #777;");
            html.AppendLine("        }");
            html.AppendLine("    </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine("    <div class=\"container\">");
            html.AppendLine("        <div class=\"header\">");
            html.AppendLine("            <h1>Doctor Hub</h1>");
            html.AppendLine("            <h2>فاتورة استشارة</h2>");
            html.AppendLine($"            <p>تاريخ الفاتورة: {DateTime.Now:dd/MM/yyyy}</p>");
            html.AppendLine("        </div>");
            html.AppendLine("        <div class=\"details\">");
            html.AppendLine("            <h3>بيانات المريض:</h3>");
            html.AppendLine("            <table>");
            html.AppendLine($"                <tr><th>الاسم</th><td>{consultation.PatientName ?? "غير محدد"}</td></tr>");
            html.AppendLine($"                <tr><th>رقم الهاتف</th><td>{consultation.FullPhoneNumber ?? consultation.PhoneNumber ?? "غير محدد"}</td></tr>");
            html.AppendLine($"                <tr><th>تاريخ الميلاد</th><td>{consultation.BirthDate.ToString("dd/MM/yyyy") ?? "غير محدد"}</td></tr>");
            html.AppendLine($"                <tr><th>الجنس</th><td>{consultation.Gender ?? "غير محدد"}</td></tr>");
            html.AppendLine("            </table>");
            html.AppendLine("            <h3>تفاصيل الاستشارة:</h3>");
            html.AppendLine("            <table>");
            html.AppendLine($"                <tr><th>نوع الاستشارة</th><td>{consultation.MedicalSpecialty?.Name ?? "غير محدد"}</td></tr>");
            html.AppendLine($"                <tr><th>تاريخ الحجز</th><td>{consultation.BookingDate:dd/MM/yyyy}</td></tr>");
            html.AppendLine($"                <tr><th>الرسوم</th><td>${consultation.ConsultationFee}</td></tr>");
            html.AppendLine($"                <tr><th>حالة الدفع</th><td>{GetPaymentStatusText(consultation.PaymentStatus)}</td></tr>");
            html.AppendLine("            </table>");
            html.AppendLine("        </div>");
            html.AppendLine("        <div class=\"footer\">");
            html.AppendLine("            <p>شكراً لاختياركم Doctor Hub!</p>");
            html.AppendLine("        </div>");
            html.AppendLine("    </div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html.ToString();
        }


        public string GenerateConfirmationHtml(Consultation consultation)
        {
            var html = new StringBuilder();

            html.AppendLine("<!DOCTYPE html>");
            html.AppendLine("<html lang=\"ar\" dir=\"rtl\">");
            html.AppendLine("<head>");
            html.AppendLine("    <meta charset=\"UTF-8\">");
            html.AppendLine("    <title>تأكيد الحجز</title>");
            html.AppendLine("    <style>");
            html.AppendLine("        body {");
            html.AppendLine("            font-family: \'Amiri\', sans-serif;");
            html.AppendLine("            direction: rtl;");
            html.AppendLine("            text-align: right;");
            html.AppendLine("            font-size: 12px;");
            html.AppendLine("        }");
            html.AppendLine("        .container {");
            html.AppendLine("            width: 80%;");
            html.AppendLine("            margin: auto;");
            html.AppendLine("            padding: 20px;");
            html.AppendLine("            border: 1px solid #eee;");
            html.AppendLine("            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);");
            html.AppendLine("        }");
            html.AppendLine("        h1, h2, h3 {");
            html.AppendLine("            color: #333;");
            html.AppendLine("        }");
            html.AppendLine("        .header {");
            html.AppendLine("            text-align: center;");
            html.AppendLine("            margin-bottom: 20px;");
            html.AppendLine("        }");
            html.AppendLine("        .details table {");
            html.AppendLine("            width: 100%;");
            html.AppendLine("            border-collapse: collapse;");
            html.AppendLine("            margin-bottom: 20px;");
            html.AppendLine("        }");
            html.AppendLine("        .details th, .details td {");
            html.AppendLine("            border: 1px solid #ddd;");
            html.AppendLine("            padding: 8px;");
            html.AppendLine("            text-align: right;");
            html.AppendLine("        }");
            html.AppendLine("        .footer {");
            html.AppendLine("            text-align: center;");
            html.AppendLine("            margin-top: 30px;");
            html.AppendLine("            font-size: 10px;");
            html.AppendLine("            color: #777;");
            html.AppendLine("        }");
            html.AppendLine("    </style>");
            html.AppendLine("</head>");
            html.AppendLine("<body>");
            html.AppendLine("    <div class=\"container\">");
            html.AppendLine("        <div class=\"header\">");
            html.AppendLine("            <h1>Doctor Hub</h1>");
            html.AppendLine("            <h2>تأكيد موعد الاستشارة</h2>");
            html.AppendLine($"            <p>عزيزي/عزيزتي {consultation.PatientName},</p>");
            html.AppendLine("            <p>تم تأكيد موعد استشارتك بنادي Doctor Hub!</p>");
            html.AppendLine("        </div>");
            html.AppendLine("        <div class=\"details\">");
            html.AppendLine("            <h3>تفاصيل الموعد:</h3>");
            html.AppendLine("            <table>");
            html.AppendLine($"                <tr><th>اسم المريض</th><td>{consultation.PatientName}</td></tr>");
            html.AppendLine($"                <tr><th>نوع الاستشارة</th><td>{consultation.MedicalSpecialty?.Name ?? "غير محدد"}</td></tr>");
            html.AppendLine($"                <tr><th>تاريخ ووقت الموعد</th><td>{consultation.BookingDate:dd/MM/yyyy HH:mm}</td></tr>");
            html.AppendLine("            </table>");
            html.AppendLine("            <h3>بيانات الطبيب:</h3>");
            html.AppendLine("            <table>");
            html.AppendLine("                <tr><th>اسم الطبيب</th><td>الدكتور محمد أحمد</td></tr>");
            html.AppendLine("                <tr><th>التخصص</th><td>استشاري طب عام</td></tr>");
            html.AppendLine("            </table>");
            html.AppendLine("        </div>");
            html.AppendLine("        <div class=\"footer\">");
            html.AppendLine("            <p>نتطلع لخدمتكم في Doctor Hub!</p>");
            html.AppendLine("        </div>");
            html.AppendLine("    </div>");
            html.AppendLine("</body>");
            html.AppendLine("</html>");

            return html.ToString();
        }


        private string GenerateInvoiceText(Consultation consultation)
        {
            var sb = new StringBuilder();
            sb.AppendLine("===============================================");
            sb.AppendLine("                Doctor Hub");
            sb.AppendLine("              Medical Invoice");
            sb.AppendLine("===============================================");
            sb.AppendLine();
            sb.AppendLine($"Invoice Number: INV-{consultation.ConsultationId:D4}");
            sb.AppendLine($"Issue Date: {DateTime.Now:dd/MM/yyyy}");
            sb.AppendLine();
            sb.AppendLine("-----------------------------------------------");
            sb.AppendLine("Patient Information:");
            sb.AppendLine("-----------------------------------------------");
            sb.AppendLine($"Name: {consultation.PatientName}");
            sb.AppendLine($"Phone: {consultation.FullPhoneNumber ?? consultation.PhoneNumber}");
            sb.AppendLine($"Gender: {consultation.Gender ?? "Not specified"}");
            sb.AppendLine($"Birth Date: {consultation.BirthDate.ToString("dd/MM/yyyy") ?? "Not specified"}");
            sb.AppendLine();
            sb.AppendLine("-----------------------------------------------");
            sb.AppendLine("Service Details:");
            sb.AppendLine("-----------------------------------------------");
            sb.AppendLine($"Consultation Type: {consultation.MedicalSpecialty?.Name ?? "Not specified"}");
            sb.AppendLine($"Booking Date: {consultation.BookingDate:dd/MM/yyyy}");
            sb.AppendLine($"Fee: ${consultation.ConsultationFee}");
            sb.AppendLine($"Payment Status: {GetPaymentStatusText(consultation.PaymentStatus)}");
            sb.AppendLine();
            sb.AppendLine("===============================================");
            sb.AppendLine($"Total Amount: ${consultation.ConsultationFee}");
            sb.AppendLine("===============================================");
            sb.AppendLine();
            sb.AppendLine("For inquiries:");
            sb.AppendLine("Phone: +9647715513344");
            sb.AppendLine("Email: info@doctorhub.com");
            sb.AppendLine();
            sb.AppendLine("Thank you for trusting Doctor Hub");

            return sb.ToString();
        }

        private string GenerateConfirmationText(Consultation consultation)
        {
            var sb = new StringBuilder();
            sb.AppendLine("===============================================");
            sb.AppendLine("                Doctor Hub");
            sb.AppendLine("         Appointment Confirmation");
            sb.AppendLine("===============================================");
            sb.AppendLine();
            sb.AppendLine($"Dear {consultation.PatientName}");
            sb.AppendLine("Your medical appointment has been confirmed successfully!");
            sb.AppendLine();
            sb.AppendLine("-----------------------------------------------");
            sb.AppendLine("Appointment Details:");
            sb.AppendLine("-----------------------------------------------");
            sb.AppendLine($"Booking ID: #{consultation.ConsultationId:D4}");
            sb.AppendLine($"Date: {consultation.BookingDate:dd/MM/yyyy}");
            sb.AppendLine($"Consultation Type: {consultation.MedicalSpecialty?.Name ?? "Not specified"}");
            sb.AppendLine($"Fee: ${consultation.ConsultationFee}");
            sb.AppendLine();
            sb.AppendLine("-----------------------------------------------");
            sb.AppendLine("Doctor Information:");
            sb.AppendLine("-----------------------------------------------");
            sb.AppendLine("Doctor Name: Dr. Mohammed Ahmed");
            sb.AppendLine($"Specialty: {consultation.MedicalSpecialty?.Name ?? "Not specified"}");
            sb.AppendLine("Experience: 15+ years");
            sb.AppendLine();
            sb.AppendLine("-----------------------------------------------");
            sb.AppendLine("Important Notes:");
            sb.AppendLine("-----------------------------------------------");
            sb.AppendLine("• Please arrive 15 minutes before your appointment");
            sb.AppendLine("• Bring all previous medical reports");
            sb.AppendLine("• If you\\\\\'re more than 30 minutes late, the appointment may be cancelled");
            sb.AppendLine();
            sb.AppendLine("For inquiries or rescheduling:");
            sb.AppendLine("Phone: +9647715513344");
            sb.AppendLine("WhatsApp: +9647715513344");
            sb.AppendLine();
            sb.AppendLine("We look forward to serving you at Doctor Hub");

            return sb.ToString();
        }

        private string GetPaymentStatusText(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "معلق",
                PaymentStatus.Paid => "مدفوع",
                _ => "غير معروف",
            };
        }


    }
}




