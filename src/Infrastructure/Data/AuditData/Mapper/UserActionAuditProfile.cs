using AutoMapper;
using DataUploader.Domain.Models;

namespace AuditData.Mapper
{
    /// <summary>
    /// Класс сопоставления <see cref="Models.UserActionHistory"/> c <see cref="UserActionHistory"/>
    /// </summary>
    public class UserActionAuditProfile : Profile
    {
        public UserActionAuditProfile()
        {
            CreateMap<Models.UserActionHistory, UserActionHistory>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(m => m.Id ?? Guid.NewGuid()))
                .ForMember(dest => dest.SavedDateTime, opt => opt.Condition((src, dest) => src.SavedDateTime != null));
        }
    }
}
