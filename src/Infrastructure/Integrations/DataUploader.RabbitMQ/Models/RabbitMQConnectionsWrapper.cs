namespace DataUploader.RabbitMQ.Models;

/// <summary>
/// Класс обертки над потокобезопасной коллекцией соединений RabbitMQ
/// </summary>
public class RabbitMQConnectionsWrapper
{
    public RabbitMQConnectionsWrapper(ConnectionDescription[] connections)
    {
        Connections = connections;
    }

    /// <summary>
    /// Коллекция соединений.
    /// </summary>
    public ConnectionDescription[] Connections { get; }
}
