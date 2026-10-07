using DataUploader.RabbitMQ.Interfaces;
using DataUploader.RabbitMQ.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace DataUploader.RabbitMQ.Services;

/// <summary>
/// Класс реализации публикации события в RabbitMQ
/// </summary>
public class Producer : IProducer
{
    private ILogger<Producer> _logger;
    private readonly RabbitMQConnectionsWrapper _connectionsWrapper;

    /// <summary>
    /// Конструктор класса Producer
    /// </summary>
    /// <param name="connectionsWrapper">Обертка над коллекцией соединений</param>
    public Producer(RabbitMQConnectionsWrapper connectionsWrapper, ILogger<Producer> logger)
    {
        _connectionsWrapper = connectionsWrapper ?? throw new ArgumentNullException(nameof(connectionsWrapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
                _logger.LogWarning($"Подключение к RabbitMQ с параметром ExchangeName = '{@event.ExchangeName}' не настроено.");
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
            var message = $"А процессе отправки сообщения в RabbitMQ получена ошибка '{ex.Message}'.";
            _logger.LogError(message, ex);
        }
    }
}