using DataUploader.Domain.Models;

namespace DataUploader.Audit.Interfaces
{
    public interface IAuditService
    {
        public Task<Guid> SaveToDBAsync(UserActionHistory userAction);
    }
}
