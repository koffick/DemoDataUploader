using DataUploader.Audit.Interfaces;
using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models;

namespace DataUploader.Audit.Services
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _auditRepository;
        private readonly ILogger<AuditService> _logger;

        public AuditService(IAuditRepository auditRepository, ILogger<AuditService> logger)
        {
            _auditRepository = auditRepository ?? throw new ArgumentNullException(nameof(auditRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task<Guid> SaveToDBAsync(UserActionHistory userAction)
        {
            try
            {
                await _auditRepository.SaveUserActionHistory(userAction);
                return userAction.Id ?? default;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при вызове метода SaveToDBAsync в AuditService. {ex.Message}", ex);
                throw;
            }
        }
    }
}
