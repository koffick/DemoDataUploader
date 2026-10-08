using DataUploader.Domain.Models;

namespace DataUploader.Domain.Interfaces;

/// <summary>
/// Класс-хранилище данных аудита.
/// </summary>
public interface IAuditRepository
{
    /// <summary>
    /// Сохраняет информацию о действие с файлом.
    /// </summary>
    /// <param name="action">Параметры действия с файлом.</param>
    /// <returns></returns>
    Task SaveFileActionHistory(FileActionHistory action);
}
