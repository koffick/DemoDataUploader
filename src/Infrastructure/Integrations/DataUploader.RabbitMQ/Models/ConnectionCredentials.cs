namespace DataUploader.RabbitMQ.Models;

/// <summary>
/// Класс параметров подключения к очередям RabbitMQ.
/// </summary>
public class ConnectionCredentials
{
    /// <summary>
    /// Адрес сервера
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Порт
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// VirtualHost
    /// </summary>
    public string VirtualHost { get; set; } = "/";

    /// <summary>
    /// Имя обменника
    /// </summary>
    public string ExchangeName { get; set; }
}
