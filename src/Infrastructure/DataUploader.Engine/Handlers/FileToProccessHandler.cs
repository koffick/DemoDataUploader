using AutoMapper;
using DataUploader.Domain.Enums;
using DataUploader.Engine.Interfaces;
using DataUploader.RabbitMQ.Abstractions;
using DataUploader.RabbitMQ.Interfaces;
using DataUploader.RabbitMQ.Models;
using Newtonsoft.Json;
using Storage.Minio;

namespace DataUploader.Engine.Handlers
{
    /// <summary>
    /// Хэндлер сообщений о действиях с файлом.
    /// </summary>
    public class FileToProccessHandler : BaseEventHandler<FileToProccessMessage>
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;

        /// <inheritdoc cref="BaseEventHandler{T}"/>
        public FileToProccessHandler(
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
                    var producer = scope.ServiceProvider.GetRequiredService<IProducer>();
                    var message = _mapper.Map<FileDataParseMessage>(@event);
                    message.ActionTypeId = (int)ActionTypeEnum.FileDataParse;
                    // TODO настроить отправку в правильный exchange
                    // await producer.PublishAsync(message);

                    var fileStorage = scope.ServiceProvider.GetRequiredService<IFileStorageProvider>();
                    var objectName = string.Join("_", message.FileId, message.FileName);
                    var fileData = await fileStorage.DownloadAsync(objectName);

                    Logger.LogInformation($"Данные из файла '{objectName}' взяты в обработку.");

                    var dataProcessingService = scope.ServiceProvider.GetRequiredService<IDataProcessingService>();
                    var result = await dataProcessingService.Run(fileData);

                    Logger.LogInformation($"Данные из файла '{objectName}' обработаны.");

                    var completeMessage = (FileDataParseMessage)message.Clone();
                    completeMessage.ActionTypeId = (int)result;
                    // TODO настроить отправку в правильный exchange
                    // await producer.PublishAsync(completeMessage);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Ошибка при обработке данных из файла {JsonConvert.SerializeObject(@event)}. {ex.Message}", ex);
            }
        }
    }
}