using AutoMapper;
using DataUploader.Domain.Models;
using DataUploader.RabbitMQ.Models;

namespace DataUploader.Audit.Mapper
{
    /// <summary>
    /// Класс сопоставления <see cref="FileToProccessMessage"/> c <see cref="FileActionHistory"/>
    /// </summary>
    public class FileActionHistoryAuditProfile : Profile
    {
        public FileActionHistoryAuditProfile()
        {
            CreateMap<FileToProccessMessage, FileActionHistory>()
                .ForMember(dest => dest.Id,opt => opt.MapFrom(m => Guid.NewGuid()))
                .ForMember(dest => dest.ActionDateTime,opt => opt.MapFrom(m => m.EventDate))
                .ForMember(dest => dest.ActionType, opt => opt.MapFrom((src, dest, destMember, context) => GetActionType(context)))
                .ForMember(dest => dest.SavedDateTime, opt => opt.Ignore())
                .ForMember(dest => dest.Note, opt => opt.Ignore());
        }

        private string GetActionType(ResolutionContext context)
        {
            // Проверка наличия типа в контексте.
            if (context == null || context.State == null || !context.Items.TryGetValue("ActionType", out var actionType))
            {
                return null;
            }

            return (string)actionType;
        }
    }
}