using DAL.Data.Models;

namespace MyClinc.Services
{
    public interface IWhatsAppBotService
    {
        Task<bool> SendWelcomeMessage(string phoneNumber);
        Task<bool> ProcessConsultationForm(string phoneNumber, string message);
        Task<bool> ProcessDoctorJoinForm(string phoneNumber, string message);
        Task<bool> ProcessPartnershipForm(string phoneNumber, string message);
        Task<bool> SendFormInstructions(string phoneNumber, string formType);
        Task<bool> SendConfirmationMessage(string phoneNumber, string formType);
        Task<bool> VerifyWebhook(string mode, string token, string challenge);
    }
} 