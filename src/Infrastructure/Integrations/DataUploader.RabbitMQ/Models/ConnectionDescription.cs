using RabbitMQ.Client;

namespace DataUploader.RabbitMQ.Models;

/// <summary>
/// Параметры соединения с RabbitMQ.
/// </summary>
public class ConnectionDescription
{
    /// <summary>
    /// Имя обменника.
    /// </summary>
    public string ExchangeName { get; set; }

    /// <inheritdoc cref="IConnection"/>
    public IConnection Connection { get; set; }
}
