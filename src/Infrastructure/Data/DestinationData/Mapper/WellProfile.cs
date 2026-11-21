using AutoMapper;
using DataUploader.Domain.Models.Data;

namespace DestinationData.Mapper
{
    /// <summary>
    /// Класс сопоставления <see cref="Models.Well"/> c <see cref="Well"/>
    /// </summary>
    public class WellProfile : Profile
    {
        public WellProfile()
        {
            CreateMap<Models.Well, Well>()
                .ReverseMap();
        }
    }
}
