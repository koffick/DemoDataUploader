using AutoMapper;
using DataUploader.Domain.Models;
using DataUploader.RabbitMQ.Models;

namespace DataUploader.Audit.Mapper
{
    /// <summary>
    /// Класс сопоставления <see cref="UserActionMessage"/> c <see cref="UserActionHistory"/>
    /// </summary>
    public class UserActionHistoryAuditProfile : Profile
    {
        public UserActionHistoryAuditProfile()
        {
            CreateMap<UserActionMessage, UserActionHistory>()
                .ForMember(dest => dest.Id,opt => opt.MapFrom(m => Guid.NewGuid()))
                .ForMember(dest => dest.ActionDateTime,opt => opt.MapFrom(m => m.EventDate))
                .ForMember(dest => dest.EventId,opt => opt.MapFrom(m => m.EventId))
                .ForMember(dest => dest.FileId,opt => opt.MapFrom(m => m.FileId))
                .ForMember(dest => dest.EventName,opt => opt.MapFrom(m => m.EventName))
                .ForMember(dest => dest.FileName,opt => opt.MapFrom(m => m.FileName))
                .ForAllMembers(dest => dest.Ignore());
        }
    }
}
