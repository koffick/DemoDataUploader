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
                .ForMember(s => s.Id, d => d.MapFrom(m => m.Id ?? Guid.NewGuid()));
        }
    }
}
