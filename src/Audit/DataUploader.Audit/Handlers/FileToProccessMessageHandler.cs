using AutoMapper;
using Newtonsoft.Json;
using DataUploader.Audit.Interfaces;
using DataUploader.RabbitMQ.Abstractions;
using DataUploader.RabbitMQ.Models;
using DataUploader.Domain.Models;
using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Enums;

namespace DataUploader.Audit.Handlers
{
    /// <summary>
    /// Хэндлер сообщений о действиях с файлом.
    /// </summary>
    public class FileToProccessMessageHandler : BaseEventHandler<FileToProccessMessage>
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        /// <inheritdoc cref="BaseEventHandler{T}"/>
        public FileToProccessMessageHandler(
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
        public override async Task HandleAsync(FileToProccessMessage @event)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var fileAction = _mapper.Map<FileActionHistory>(@event, opt => opt.Items["ActionType"] = nameof(ActionTypeEnum.FileToProccess));
                    var businessService =
                        scope.ServiceProvider.GetRequiredService<IAuditService>();
                    var fileActionId = await businessService.FileActionSaveToDBAsync(fileAction);
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