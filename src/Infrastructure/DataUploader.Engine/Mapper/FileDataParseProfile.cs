using AutoMapper;
using DataUploader.RabbitMQ.Models;

namespace DataUploader.Engine.Mapper
{
    /// <summary>
    /// Класс сопоставления <see cref="FileToProccessMessage"/> c <see cref="FileDataParseMessage"/>
    /// </summary>
    public class FileDataParseProfile : Profile
    {
        public FileDataParseProfile()
        {
            CreateMap<FileToProccessMessage, FileDataParseMessage>();
        }
    }
}
