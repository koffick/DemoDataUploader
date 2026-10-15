using AutoMapper;
using DataUploader.Domain.Models;

namespace AuditData.Mapper
{
    /// <summary>
    /// Класс сопоставления <see cref="Models.FileActionHistory"/> c <see cref="FileActionHistory"/>
    /// </summary>
    public class FileActionAuditProfile : Profile
    {
        public FileActionAuditProfile()
        {
            CreateMap<Models.FileActionHistory, FileActionHistory>()
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(m => m.Id ?? Guid.NewGuid()));
        }
    }
}
