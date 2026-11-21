using AutoMapper;
using DataUploader.Domain.Models.Data;

namespace DestinationData.Mapper
{
    /// <summary>
    /// Класс сопоставления <see cref="Models.TechMode"/> c <see cref="TechMode"/>
    /// </summary>
    public class TechModeProfile : Profile
    {
        public TechModeProfile()
        {
            CreateMap<Models.TechMode, TechMode>()
                .ReverseMap();
        }
    }
}
