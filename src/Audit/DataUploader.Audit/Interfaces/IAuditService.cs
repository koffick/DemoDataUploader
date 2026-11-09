using DataUploader.Domain.Models;

namespace DataUploader.Audit.Interfaces
{
    public interface IAuditService
    {
        /// <summary>
        /// Сохранение действия пользователя.
        /// </summary>
        /// <param name="userAction">Действие пользователя.</param>
        /// <returns></returns>
        public Task<Guid> UserActionSaveToDBAsync(UserActionHistory userAction);

        /// <summary>
        /// Сохранение действия с файлом.
        /// </summary>
        /// <param name="fileAction">Действие с файлом.</param>
        /// <returns></returns>
        public Task<Guid> FileActionSaveToDBAsync(FileActionHistory fileAction);
    }
}
