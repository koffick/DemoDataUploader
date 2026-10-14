using DataUploader.Audit.Interfaces;
using DataUploader.RabbitMQ.Interfaces;

namespace DataUploader.Audit.Services
{
    public class AuditService : IAuditService
    {
        public Task<Guid> SaveToDBAsync(IMessage message)
        {
            return Task.FromResult(Guid.NewGuid());
        }
    }
}
