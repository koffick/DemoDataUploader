using Microsoft.AspNetCore.Http;

namespace Web.Api.DTO.Requests
{
    /// <summary>
    /// Данные загружаемого файла.
    /// </summary>
    public class UploadFileRequest
    {
        /// <summary>
        /// Загружаемый файл.
        /// </summary>
        public IFormFile File { get; set; }

        /// <summary>
        /// Наименование события.
        /// </summary>
        public string EventName { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор события.
        /// </summary>
        public Guid EventId { get; set; } = default;
    }
}
