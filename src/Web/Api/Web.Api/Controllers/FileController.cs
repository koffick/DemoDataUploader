using DataUploader.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Web.Api.DTO.Requests;
using Web.Api.DTO.Responses;

namespace Web.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IExcelParser _excelParser;
        private IFileProvider _fileProvider;

        public FileController(
            IExcelParser excelParser,
            IFileProvider fileProvider)
        {
            _excelParser = excelParser ?? throw new ArgumentNullException(nameof(excelParser));
            _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
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
