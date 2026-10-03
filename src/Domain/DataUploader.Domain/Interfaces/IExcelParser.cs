using DataUploader.Domain.Models;

namespace DataUploader.Domain.Interfaces;

/// <summary>
/// Класс для работы с данными в файле Excel.
/// </summary>
public interface IExcelParser
{
    /// <summary>
    /// Определеяет, является ли файл Excel-ем и шаблонизирован ли он под загрузку данных.
    /// </summary>
    /// <param name="data">Файл Excel в виде потока.</param>
    /// <param name="errors">Список ошибок валидации.</param>
    /// <param name="configuration">Настройка обработки файла.</param>
    /// <returns></returns>
    bool IsValidate(Stream data, out IEnumerable<string> errors, OperationConfiguration? configuration = null);
}
