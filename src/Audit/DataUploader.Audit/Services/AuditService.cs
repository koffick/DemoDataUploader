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


        public Task<Guid> SaveToDBAsync(UserActionHistory userAction)
        {
            return Task.FromResult(Guid.NewGuid());
        }
    }
}
