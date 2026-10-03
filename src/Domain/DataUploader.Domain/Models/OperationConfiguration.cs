namespace DataUploader.Domain.Models;

/// <summary>
/// Настройки для обработки данных для вида операции. 
/// </summary>
public class OperationConfiguration
{
    /// <summary>
    /// Номер страницы с данными.
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    /// Номер строки с сопоставлением колонок и свойств в модели хранения.
    /// </summary>
    public int? ColumnKeysRowNumber { get; set; } = 1;

    /// <summary>
    /// Номер строки с которой начинаются данные.
    /// </summary>
    public int DataRowNumber { get; set; } = 2;
}
