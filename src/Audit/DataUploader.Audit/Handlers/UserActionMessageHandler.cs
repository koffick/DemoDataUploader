using AutoMapper;
using Newtonsoft.Json;
using DataUploader.Audit.Interfaces;
using DataUploader.RabbitMQ.Abstractions;
using DataUploader.RabbitMQ.Models;
using DataUploader.Audit.Models;

namespace Itsk.Vma.SPekTR.BusinessAudit.Queue.Handlers
{
    /// <summary>
    /// Хэндлер сообщений о действиях с файлом.
    /// </summary>
    public class UserActionMessageHandler : BaseEventHandler<UserActionMessage>
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        /// <inheritdoc cref="BaseEventHandler{T}"/>
        public UserActionMessageHandler(
            RabbitMQConnectionsWrapper connectionDictionaryWrapperDictionary,
            ILogger logger,
            string exchangeName,
            string routingKey,
            IMapper mapper,
            IServiceProvider serviceProvider)
            : base(connectionDictionaryWrapperDictionary, logger, exchangeName, routingKey)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <inheritdoc cref="BaseEventHandler{T}.HandleAsync"/>
        public override async Task HandleAsync(UserActionMessage @event)
        {
            try
            {
                var userAction = _mapper.Map<UserAction>(@event);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var businessService =
                        scope.ServiceProvider.GetRequiredService<IAuditService>();
                    var fileActionId = await businessService.SaveToDBAsync(@event);
                    Logger.LogInformation($"Информация о действии с файлом успешно сохранена с ID = {fileActionId}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Ошибка при сохранении информации о действии с файлом {JsonConvert.SerializeObject(@event)}. {ex.Message}", ex);
            }
        }
    }
}