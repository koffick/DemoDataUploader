namespace DataUploader.Domain.Models;

/// <summary>
/// Модель истории действий с файлами.
/// </summary>
public class FileActionHistory
{
    /// <summary>
    /// Уникальный идентификатор в БД.
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    ///  Идентификатор файла.
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// Наименование действия.
    /// </summary>
    public string ActionType { get; set; }

    /// <summary>
    /// Дата действия.
    /// </summary>
    public DateTime? ActionDateTime { get; set; }

    /// <summary>
    /// Дата создания.
    /// </summary>
    public DateTime SavedDateTime { get; set; }

    /// <summary>
    /// Комментарий.
    /// </summary>
    public string Note { get; set; }
}
