using Microsoft.AspNetCore.Http;
using System.Text;
using Web.Api.DTO.Requests;

namespace Web.Api.Tests.Helpers
{

    internal class UploadFileRequestBuilder
    {
        private IFormFile _file { get; set; }
        private string _contentType { get; set; }
        public string? _eventName { get; set; }
        public Guid? _eventId { get; set; }

        public UploadFileRequestBuilder SetFile(string bodyAsString, string fileName)
        {
            var streamData = Encoding.UTF8.GetBytes(bodyAsString);
            var stream = new MemoryStream(streamData);
            _file = new FormFile(stream, 0, streamData.LongLength, string.Empty, fileName)
            {
                Headers = new HeaderDictionary()
            };
            return this;
        }

        public UploadFileRequestBuilder SetContentType(string contentType)
        {
            _contentType = contentType;
            return this;
        }

        public UploadFileRequestBuilder SetEvent(string eventName, Guid eventId)
        {
            _eventId = eventId;
            _eventName = eventName;
            return this;
        }

        public UploadFileRequest Build(bool makeDefaultData = false)
        {
            if (makeDefaultData)
            {
                SetFile(Guid.NewGuid().ToString(), "file.xlsx")
                    .SetContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }

            var result = new UploadFileRequest();
            if (_file != null)
            {
                if (!string.IsNullOrEmpty(_contentType))
                {
                    _file.Headers.ContentType = _contentType;
                }

                result.File = _file;
            }

            result.EventId = _eventId ?? Guid.NewGuid();
            result.EventName = _eventName ?? Guid.NewGuid().ToString();

            return result;
        }
    }
}
