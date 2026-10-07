using AutoFixture;
using DataUploader.Domain.Interfaces;
using DataUploader.Domain.Models;
using DataUploader.RabbitMQ.Interfaces;
using DataUploader.RabbitMQ.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Web.Api.Controllers;
using Web.Api.DTO.Responses;
using Web.Api.Tests.Helpers;

namespace Web.Api.Tests.Controllers
{
    public class FileControllerTests
    {
        private FileController _controller;
        private UploadFileRequestBuilder _builder;
        private Mock<IExcelParser> _excelParser;
        private Mock<IFileProvider> _fileProvider;
        private Mock<IProducer> _producer;

        public FileControllerTests()
        {
            _builder = new UploadFileRequestBuilder();
            _excelParser = new Mock<IExcelParser>();
            _fileProvider = new Mock<IFileProvider>();
            _producer = new Mock<IProducer>();
            _controller = new FileController(_excelParser.Object, _fileProvider.Object, _producer.Object);
        }

        [Fact]
        public async Task Upload_ShouldReturnOkCorrect()
        {
            IEnumerable<string> errors = new List<string>();
            _excelParser.Setup(s => s.IsValidate(It.IsAny<Stream>(), out errors, It.IsAny<OperationConfiguration>())).Returns(true);
            var data = _builder.Build(true);
            var fileId = Guid.NewGuid();
            _fileProvider.Setup(s => s.SaveFileAsync(It.IsAny<Stream>(), data.File.FileName))
                .Returns(Task.FromResult(fileId));
            var userName = Guid.NewGuid().ToString();
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, userName) };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));
            var controllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext { User = principal },
            };
            _controller.ControllerContext = controllerContext;

            var result = await _controller.UploadAsync(data);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
            var message = (result as OkObjectResult).Value.ToString();
            Assert.Equal("Файл загружен.", message);
            _excelParser.Verify(v => v.IsValidate(It.IsAny<Stream>(), out errors, It.IsAny<OperationConfiguration>()), Times.Once);
            _fileProvider.Verify(v => v.SaveFileAsync(It.IsAny<Stream>(), data.File.FileName), Times.Once);
            _producer.Verify(v => v.PublishAsync(It.Is<FileToProccessMessage>(message =>
                message.ExchangeName == "DataUploader.FileToProccessing" &&
                message.FileName == data.File.FileName &&
                message.FileId == fileId &&
                message.EventId == data.EventId &&
                message.EventName == data.EventName &&
                message.UserName == userName)), Times.Once);
        }

        [Fact]
        public async Task Upload_ShouldReturnBadRequestIfNonValidateFile()
        {
            IEnumerable<string> errors = new Fixture().CreateMany<string>(4);
            _excelParser.Setup(s => s.IsValidate(It.IsAny<Stream>(), out errors, It.IsAny<OperationConfiguration>())).Returns(false);
            var data = _builder.Build(true);

            var result = await _controller.UploadAsync(data);

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            var returnData = (result as BadRequestObjectResult).Value;
            Assert.IsType<ApiFailureResponse>(returnData);
            var response = returnData as ApiFailureResponse;
            var errorsJson = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
            Assert.Equal($"Файл не прошел проверку. Количество ошибок {errors.Count()}. Список ошибок: {errorsJson}", response.ErrorMessage);
        }

        [Fact]
        public async Task Upload_ShouldReturnBadRequestForEmptyParameterValue()
        {
            var result = await _controller.UploadAsync(null);

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            var returnData = (result as BadRequestObjectResult).Value;
            Assert.IsType<ApiFailureResponse>(returnData);
            var response = returnData as ApiFailureResponse;
            Assert.Equal("В процессе загрузки файла произошла непредвиденная ошибка: Value cannot be null. (Parameter 'fileData')", response.ErrorMessage);
        }

        [Fact]
        public async Task Upload_ShouldReturnBadRequestForNonXLSXFile()
        {
            var data = _builder
                .SetFile(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
                .Build();

            var result = await _controller.UploadAsync(data);

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            var returnData = (result as BadRequestObjectResult).Value;
            Assert.IsType<ApiFailureResponse>(returnData);
            var response = returnData as ApiFailureResponse;
            Assert.Equal("Тип файла должен быть \".xlsx\"", response.ErrorMessage);
        }

        [Fact]
        public async Task Upload_ShouldReturnBadRequestIfMEMOTypeNonCorrectly()
        {
            var data = _builder
                .SetFile(Guid.NewGuid().ToString(), Guid.NewGuid().ToString())
                .SetContentType(Guid.NewGuid().ToString())
                .Build();

            var result = await _controller.UploadAsync(data);

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            var returnData = (result as BadRequestObjectResult).Value;
            Assert.IsType<ApiFailureResponse>(returnData);
            var response = returnData as ApiFailureResponse;
            Assert.Equal("Тип файла должен соответствовать формату \"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet\"", response.ErrorMessage);
        }

        [Fact]
        public async Task Upload_ShouldReturnBadRequestIfSaveFileNonCorrectly()
        {
            IEnumerable<string> errors = new List<string>();
            _excelParser.Setup(s => s.IsValidate(It.IsAny<Stream>(), out errors, It.IsAny<OperationConfiguration>())).Returns(true);
            var errorMessage = Guid.NewGuid().ToString();
            _fileProvider.Setup(s => s.SaveFileAsync(It.IsAny<Stream>(), It.IsAny<string>())).Throws(new Exception(errorMessage)); ;
            var data = _builder.Build(true);

            var result = await _controller.UploadAsync(data);

            Assert.NotNull(result);
            Assert.IsType<BadRequestObjectResult>(result);
            var returnData = (result as BadRequestObjectResult).Value;
            Assert.IsType<ApiFailureResponse>(returnData);
            var response = returnData as ApiFailureResponse;
            Assert.Equal($"В процессе загрузки файла произошла непредвиденная ошибка: {errorMessage}", response.ErrorMessage);
        }
    }
}
