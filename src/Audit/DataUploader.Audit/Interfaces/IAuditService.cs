using DataUploader.RabbitMQ.Interfaces;

namespace DataUploader.Audit.Interfaces
{
    public interface IAuditService
    {
        public Task<Guid> SaveToDBAsync(IMessage message);
    }
}
