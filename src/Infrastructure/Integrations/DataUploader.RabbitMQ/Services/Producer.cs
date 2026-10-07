using DataUploader.RabbitMQ.Interfaces;
using DataUploader.RabbitMQ.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace DataUploader.RabbitMQ.Services;

/// <summary>
/// Класс реализации публикации события в RabbitMQ
/// </summary>
public class Producer : IProducer
{
    private readonly RabbitMQConnectionsWrapper _connectionsWrapper;

    /// <summary>
    /// Конструктор класса Producer
    /// </summary>
    /// <param name="connectionsWrapper">Обертка над коллекцией соединений</param>
    public Producer(RabbitMQConnectionsWrapper connectionsWrapper)
    {
        _connectionsWrapper = connectionsWrapper;
    }

    /// <inheritdoc cref="IProducer.PublishAsync{T}"/>
    public async Task PublishAsync<T>(T @event)
        where T : IMessage
    {
        try
        {
            var connection = _connectionsWrapper.Connections
                .FirstOrDefault(x => x.ExchangeName == @event.ExchangeName)?
                .Connection;
            if (connection == null)
            {
                //TODO отправить в лог сообщение об отсутствии соединения с RabbitMQ
                return;
            }

            var channel = await connection.CreateChannelAsync();

            var messageBody = JsonConvert.SerializeObject(@event, new JsonSerializerSettings { Formatting = Formatting.Indented });
            BasicProperties properties = new BasicProperties();
            await channel.BasicPublishAsync(
                @event.ExchangeName,
                @event.RoutingKey,
                false,
                properties,
                Encoding.UTF8.GetBytes(messageBody));

            channel.Dispose();
        }
        catch (Exception ex)
        {
            //TODO Отправить в лог сообщение, об ошибке при отправке в раббит.
        }
    }
}