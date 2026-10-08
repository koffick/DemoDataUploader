using AutoMapper;
using DataUploader.Domain.Models;

namespace AuditData.Mapper
{
    /// <summary>
    /// Класс сопоставления <see cref="Models.FileActionHistory"/> c <see cref="FileActionHistory"/>
    /// </summary>
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<Models.FileActionHistory, FileActionHistory>()
                .ReverseMap()
                .ForMember(s => s.Id, d => d.MapFrom(m => m.Id ?? Guid.NewGuid()));
        }
    }
}
