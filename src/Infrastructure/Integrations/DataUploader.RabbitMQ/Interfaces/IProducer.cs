namespace DataUploader.RabbitMQ.Interfaces;

/// <summary>
/// Класс для отправки сообщений в RabbitMQ.
/// </summary>
public interface IProducer
{
    /// <summary>
    /// Метод публикации события
    /// </summary>
    /// <typeparam name="T">Тип события</typeparam>
    /// <param name="event">Событие</param>
    Task PublishAsync<T>(T @event)
        where T : IMessage;
}
