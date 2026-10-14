using System.Text;
using DataUploader.RabbitMQ.Interfaces;
using DataUploader.RabbitMQ.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace DataUploader.RabbitMQ.Abstractions
{
    /// <summary>
    /// Базовый класс EventHandler
    /// </summary>
    /// <typeparam name="T">Тип события, на который подписывается Handler</typeparam>
    public class BaseEventHandler<T> : BackgroundService
        where T : IMessage
    {
        protected readonly ILogger Logger;
        private readonly string _queueName;
        private IConnection _connection;

        /// <summary>
        /// Конструктор базового Handler
        /// </summary>
        /// <param name="rabbitMQConnectionsWrapper">Обертка подключений к RabbitMQ</param>
        /// <param name="logger">Логгер</param>
        /// <param name="exchangeName">Обменник</param>
        /// <param name="queueName">Очередь сообщений</param>
        protected BaseEventHandler(
            RabbitMQConnectionsWrapper rabbitMQConnectionsWrapper,
            ILogger logger,
            string exchangeName,
            string queueName)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _queueName = queueName;
            _connection = rabbitMQConnectionsWrapper.Connections?
                .Single(x => x.ExchangeName == exchangeName)?
                .Connection ?? throw new ArgumentNullException(nameof(_connection));
        }

        /// <summary>
        /// Метод логики асинхронной обработки события.
        /// </summary>
        /// <param name="event">Событие</param>
        /// <returns>Task</returns>
        public virtual Task HandleAsync(T @event)
        {
            throw new NotImplementedException("Метод логики асинхронной обработки события (необходим к переопределению в классах-наследниках)");
        }

        /// <summary>
        /// Метод BackgroundService
        /// </summary>
        /// <param name="stoppingToken">Токен отмены операции</param>
        /// <returns>Task</returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();
            var channel = await _connection.CreateChannelAsync();
            try
            {
                await channel.QueueDeclarePassiveAsync(_queueName);
                Logger.LogInformation($"Очередь {_queueName} найдена.");

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += (sender, @event) =>
                {
                    var messageBody = Encoding.UTF8.GetString(@event.Body.Span.ToArray());
                    Logger.LogInformation($"Получено сообщение {messageBody}");
                    HandleAsync(JsonConvert.DeserializeObject<T>(messageBody));
                    channel.BasicAckAsync(@event.DeliveryTag, false);
                    return Task.CompletedTask;
                };

                await channel.BasicConsumeAsync(_queueName, false, consumer);

                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (Exception ex)
            {
                Logger.LogError($"При взаимодействии с RabbitMQ получено исключение: {ex.Message}", ex);
            }
            finally
            {
                await channel.CloseAsync();
                Dispose();
            }
        }
    }
}