using DataUploader.RabbitMQ.Interfaces;

namespace DataUploader.RabbitMQ.Models
{
    /// <summary>
    /// Класс сообщения о загрузке файла, реализующий <see cref="IMessage"/>
    /// </summary>
    public class FileDataParseMessage : IMessage, ICloneable
    {
        /// <inheritdoc cref="IMessage.RoutingKey"/>
        public string RoutingKey { get; set; }


        /// <inheritdoc cref="IMessage.ExchangeName"/>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Идентификатор файла в хранилище.
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// Идентификатор действия на файлом.
        /// </summary>
        public int ActionTypeId { get; set; }

        /// <summary>
        /// Дата отправки сообщения.
        /// </summary>
        public DateTime EventDate { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
