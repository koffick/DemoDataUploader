using DataUploader.RabbitMQ.Interfaces;

namespace DataUploader.RabbitMQ.Models
{
    /// <summary>
    /// Класс сообщения о загрузке файла, реализующий <see cref="IMessage"/>
    /// </summary>
    public class FileToProccessMessage : IMessage
    {
        /// <inheritdoc cref="IMessage.RoutingKey"/>
        public string RoutingKey { get; set; }


        /// <inheritdoc cref="IMessage.ExchangeName"/>
        public string ExchangeName { get; set; }

        /// <summary>
        /// Наименование события.
        /// </summary>
        public string EventName { get; set; }

        /// <summary>
        /// Идентификатор события.
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Имя файла.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Идентификатор файла в хранилище.
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Дата отправки сообщения.
        /// </summary>
        public DateTime EventDate { get; set; }
    }
}
