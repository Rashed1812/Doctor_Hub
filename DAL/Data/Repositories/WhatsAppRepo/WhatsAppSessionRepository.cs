using DAL.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data.Repositories.WhatsAppRepo
{
    public class WhatsAppSessionRepository : IWhatsAppSessionRepository
    {
        private readonly ClincDbContext _dbContext;

        public WhatsAppSessionRepository(ClincDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<WhatsAppUserSession?> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbContext.WhatsAppUserSessions
                .FirstOrDefaultAsync(s => s.PhoneNumber == phoneNumber);
        }

        public async Task<IEnumerable<WhatsAppUserSession>> GetAllAsync()
        {
            return await _dbContext.WhatsAppUserSessions.ToListAsync();
        }

        public async Task<IEnumerable<WhatsAppUserSession>> GetActiveSessionsAsync()
        {
            return await _dbContext.WhatsAppUserSessions
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        public async Task AddAsync(WhatsAppUserSession session)
        {
            await _dbContext.WhatsAppUserSessions.AddAsync(session);
        }

        public void Update(WhatsAppUserSession session)
        {
            _dbContext.WhatsAppUserSessions.Update(session);
        }

        public void Delete(WhatsAppUserSession session)
        {
            _dbContext.WhatsAppUserSessions.Remove(session);
        }

        public async Task SaveAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task CleanupExpiredSessionsAsync(int timeoutMinutes)
        {
            var cutoffTime = DateTime.Now.AddMinutes(-timeoutMinutes);
            var expiredSessions = await _dbContext.WhatsAppUserSessions
                .Where(s => s.LastActivity < cutoffTime && s.IsActive)
                .ToListAsync();

            foreach (var session in expiredSessions)
            {
                session.IsActive = false;
            }

            await SaveAsync();
        }
    }
} 