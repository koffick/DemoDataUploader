using AutoFixture;
using AutoMapper;
using DataUploader.Domain.Enums;
using DataUploader.Engine.Handlers;
using DataUploader.Engine.Interfaces;
using DataUploader.RabbitMQ.Abstractions;
using DataUploader.RabbitMQ.Interfaces;
using DataUploader.RabbitMQ.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using Storage.Minio;
using System.Text;

namespace DataUploader.Engine.Tests.Handlers
{
    public class FileToProccessHandlerTests
    {
        private const string exchangeName = "exchangeName";
        private Mock<IProducer> _producer;
        private Mock<IMapper> _mapper;
        private Mock<ILogger<FileToProccessHandler>> _logger;
        private Mock<IConnection> _connection;
        private Mock<IFileStorageProvider> _storage;
        private Mock<IDataProcessingService> _service;
        private FileToProccessHandler _handler;

        public FileToProccessHandlerTests()
        {
            _producer = new Mock<IProducer>();
            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<FileToProccessHandler>>();
            _connection = new Mock<IConnection>();
            _storage = new Mock<IFileStorageProvider>();
            _service = new Mock<IDataProcessingService>();

            var services = new ServiceCollection();

            services.AddSingleton(_producer.Object);
            services.AddSingleton(_storage.Object);
            services.AddSingleton(_service.Object);

            var serviceProvider = services.BuildServiceProvider();
            var conn = new ConnectionDescription
            {
                ExchangeName = exchangeName,
                Connection = _connection.Object
            };
            var connections = new RabbitMQConnectionsWrapper([conn]);

            _handler = new FileToProccessHandler(
                connections,
                _logger.Object,
                exchangeName,
                "",
                _mapper.Object,
                serviceProvider);
        }

        [Fact]
        public void IsInstance()
        {
            Assert.IsAssignableFrom<BaseEventHandler<FileToProccessMessage>>(_handler);
        }

        [Fact]
        public async Task HandleAsync_Comlete()
        {
            var message = new Fixture().Create<FileToProccessMessage>();
            var responseToRun = new Fixture()
                .Build<FileDataParseMessage>()
                .Without(w => w.ActionTypeId)
                .Create();
            _mapper.Setup(s => s.Map<FileDataParseMessage>(message)).Returns(responseToRun);
            var content = Encoding.UTF8.GetBytes(Guid.NewGuid().ToString());
            using var stream = new MemoryStream(content);
            var objectName = string.Join("_", responseToRun.FileId, responseToRun.FileName);
            _storage.Setup(s => s.DownloadAsync(objectName, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(stream);
            var result = ActionTypeEnum.FileDataParseSuccess;
            _service.Setup(s => s.Run(stream))
                .ReturnsAsync(result);

            await _handler.HandleAsync(message);

            Assert.Equal((int)ActionTypeEnum.FileDataParse, responseToRun.ActionTypeId);
            _mapper.Verify(v => v.Map<FileDataParseMessage>(message), Times.Once);
            _producer.Verify(v => v.PublishAsync(responseToRun), Times.Once);
            _storage.Verify(v => v.DownloadAsync(objectName, It.IsAny<string>(),It.IsAny<CancellationToken>()), Times.Once);
            _service.Verify(v => v.Run(stream), Times.Once);
            _producer.Verify(v => v.PublishAsync(It.Is<FileDataParseMessage>(i => i.ActionTypeId == (int)result)), Times.Once);
        }
    }
}
