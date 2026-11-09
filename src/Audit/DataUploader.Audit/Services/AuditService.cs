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

        /// <inheritdoc cref="IAuditService.FileActionSaveToDBAsync(FileActionHistory)"/>
        public async Task<Guid> FileActionSaveToDBAsync(FileActionHistory fileAction)
        {
            try
            {
                await _auditRepository.SaveFileActionHistory(fileAction);
                return fileAction.Id ?? default;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при вызове метода FileActionSaveToDBAsync в AuditService. {ex.Message}", ex);
                throw;
            }
        }

        /// <inheritdoc cref="IAuditService.UserActionSaveToDBAsync(UserActionHistory)"/>
        public async Task<Guid> UserActionSaveToDBAsync(UserActionHistory userAction)
        {
            try
            {
                await _auditRepository.SaveUserActionHistory(userAction);
                return userAction.Id ?? default;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при вызове метода UserActionSaveToDBAsync в AuditService. {ex.Message}", ex);
                throw;
            }
        }
    }
}
