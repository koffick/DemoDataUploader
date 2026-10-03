namespace DataUploader.Domain.Interfaces;

/// <summary>
/// Класс для работы хранилищем файлов.
/// </summary>
public interface IFileProvider
{
    /// <summary>
    /// Сохраняет файл в хранилище.
    /// </summary>
    /// <param name="fileData"></param>
    /// <param name="fileName"></param>
    /// <returns></returns>
    Task<Guid> SaveFileAsync(Stream fileData, string fileName);
}
