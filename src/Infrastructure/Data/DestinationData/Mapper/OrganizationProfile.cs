using AutoMapper;
using DataUploader.Domain.Models.Data;

namespace DestinationData.Mapper
{
    /// <summary>
    /// Класс сопоставления <see cref="Models.Organization"/> c <see cref="Organization"/>
    /// </summary>
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<Models.Organization, Organization>()
                .ReverseMap();
        }
    }
}
