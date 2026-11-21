using AutoMapper;
using DataUploader.Domain.Models.Data;

namespace DestinationData.Mapper
{
    /// <summary>
    /// Класс сопоставления <see cref="Models. Field"/> c <see cref=" Fieldl"/>
    /// </summary>
    public class FieldProfile : Profile
    {
        public FieldProfile()
        {
            CreateMap<Models.Field, Field>()
                .ReverseMap();
        }
    }
}
