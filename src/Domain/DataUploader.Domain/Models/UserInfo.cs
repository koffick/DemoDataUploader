namespace DataUploader.Domain.Models;

/// <summary>
/// Информация о пользователе приложения.
/// </summary>
public class UserInfo
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Логин.
    /// </summary>
    public string LoginName { get; set; }

    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия пользователя.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Полное имя.
    /// </summary>
    public string FullName => string.Join(" ", [ FirstName, LastName ]);
}
