using DataUploader.Domain.Interfaces;
using DataUploader.RabbitMQ.Interfaces;
using DataUploader.RabbitMQ.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Web.Api.DTO.Requests;
using Web.Api.DTO.Responses;

namespace Web.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private IExcelParser _excelParser;
        private IFileProvider _fileProvider;
        private IProducer _producer;

        public FileController(
            IExcelParser excelParser,
            IFileProvider fileProvider,
            IProducer producer)
        {
            _excelParser = excelParser ?? throw new ArgumentNullException(nameof(excelParser));
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
            _producer = producer ?? throw new ArgumentNullException(nameof(producer));
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(ApiFailureResponse), 400)]
        public async Task<IActionResult> UploadAsync(UploadFileRequest fileData)
        {
            try
            {
                if (fileData == null || fileData.File == null)
                {
                    throw new ArgumentNullException(nameof(fileData));
                }

                string contentType = GetContentType(fileData.File);

                if (contentType == string.Empty)
                {
                    if (!fileData.File.FileName.Contains(".xlsx"))
                    {
                        var message = $"Тип файла должен быть \".xlsx\"";
                        return BadRequest(new ApiFailureResponse(message));
                    }
                }
                else
                {
                    if (!contentType.Contains("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
                    {
                        var message = $"Тип файла должен соответствовать формату \"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet\"";
                        return BadRequest(new ApiFailureResponse(message));
                    }
                }

                var stream = new MemoryStream();
                await fileData.File.CopyToAsync(stream);
                if (!_excelParser.IsValidate(stream, out var errors))
                {
                    var errorsJson = Newtonsoft.Json.JsonConvert.SerializeObject(errors);
                    var message = $"Файл не прошел проверку. Количество ошибок {errors.Count()}. Список ошибок: {errorsJson}";
                    return BadRequest(new ApiFailureResponse(message));
                }

                var fileId = await _fileProvider.SaveFileAsync(stream, fileData.File.FileName);
                var userName = User?.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Name)?.Value ?? "Пользователь не идентифицирован";

                var eventMessage = new FileToProccessMessage()
                {
                    EventName = fileData.EventName,
                    EventId = fileData.EventId,
                    FileId = fileId,
                    FileName = fileData.File.FileName,
                    EventDate = DateTime.Now,
                    ExchangeName = "DataUploader.FileToProccessing",
                    RoutingKey = string.Empty,
                    UserName = userName,
                };

                await _producer.PublishAsync(eventMessage);
                return Ok("Файл загружен.");
            }
            catch (Exception ex)
            {
                var message = $"В процессе загрузки файла произошла непредвиденная ошибка: {ex.Message}";
                return BadRequest(new ApiFailureResponse(message));
            }
        }

        private static string GetContentType(IFormFile file)
        {
            string contentType = string.Empty;
            try
            {
                contentType = file.ContentType;
            }
            catch (Exception)
            {
            }

            return contentType;
        }
    }
}
