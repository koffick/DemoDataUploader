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
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    ///  Идентификатор файла.
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// Имя файла.
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// Идентификатор события.
    /// </summary>
    public Guid EventId { get; set; }

    /// <summary>
    /// Наименование события.
    /// </summary>
    public string EventName { get; set; }

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
