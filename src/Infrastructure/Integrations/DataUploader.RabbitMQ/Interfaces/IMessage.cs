namespace DataUploader.RabbitMQ.Interfaces;

/// <summary>
/// Сообщение RabbitMQ.
/// </summary>
public interface IMessage
{
    /// <summary>
    /// Ключ маршрутизации.
    /// </summary>
    abstract string RoutingKey { get; set; }

    /// <summary>
    /// Обменник.
    /// </summary>
    string ExchangeName { get; set; }
}
