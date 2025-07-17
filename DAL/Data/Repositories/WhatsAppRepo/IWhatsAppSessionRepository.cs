using DAL.Data.Models;

namespace DAL.Data.Repositories.WhatsAppRepo
{
    public interface IWhatsAppSessionRepository
    {
        Task<WhatsAppUserSession?> GetByPhoneNumberAsync(string phoneNumber);
        Task<IEnumerable<WhatsAppUserSession>> GetAllAsync();
        Task<IEnumerable<WhatsAppUserSession>> GetActiveSessionsAsync();
        Task AddAsync(WhatsAppUserSession session);
        void Update(WhatsAppUserSession session);
        void Delete(WhatsAppUserSession session);
        Task SaveAsync();
        Task CleanupExpiredSessionsAsync(int timeoutMinutes);
    }
} 