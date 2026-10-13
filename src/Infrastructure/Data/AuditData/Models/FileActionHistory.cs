using System.ComponentModel.DataAnnotations;

namespace AuditData.Models;

/// <summary>
/// Модель для хранения истории действий с файлами.
/// </summary>
public class FileActionHistory
{
    /// <summary>
    /// Уникальный идентификатор в БД.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    ///  Идентификатор файла.
    /// </summary>
    [Required]
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
    [Required]
    public DateTime SavedDateTime { get; set; } = DateTime.Now;

    /// <summary>
    /// Комментарий.
    /// </summary>
    public string Note { get; set; }
}
