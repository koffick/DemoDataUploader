using AutoMapper;
using DataUploader.Domain.Models;

namespace UserData.Mapper
{
    /// <summary>
    /// Класс сопоставления <see cref="Models.User"/> c <see cref="UserInfo"/>
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Models.User, UserInfo>();
        }
    }
}
