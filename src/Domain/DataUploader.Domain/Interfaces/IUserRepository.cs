using DataUploader.Domain.Models;

namespace DataUploader.Domain.Interfaces;

/// <summary>
/// Класс-хранилище данных о пользователеях.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Ищет информацию о пользователе по логину.
    /// </summary>
    /// <param name="login"></param>
    /// <returns></returns>
    UserInfo Find(string login);
}
