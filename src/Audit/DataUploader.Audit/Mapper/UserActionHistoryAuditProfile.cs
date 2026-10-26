using AutoMapper;
using DataUploader.Domain.Interfaces;
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
                .ForMember(dest => dest.UserId, opt => opt.MapFrom((src, dest, destMember, context) => GetUserId(context, src.UserName)))
                .ForMember(dest => dest.SavedDateTime, opt => opt.Ignore())
                .ForMember(dest => dest.Note, opt => opt.Ignore());
        }

        private Guid GetUserId(ResolutionContext context, string userName)
        {
            // Проверка наличия репозитория в контексте.
            if (context == null || context.State == null || !context.Items.TryGetValue("UserRepository", out var repository))
            {
                return default(Guid);
            }

            // Проверка наличия данных для определения пользователя.
            if (repository == null || string.IsNullOrEmpty(userName) || repository is not IUserRepository)
            {
                return default(Guid);
            }

            var repo = (IUserRepository)repository;
            var user = repo.FindByFullName(userName);
            if (user == null)
            {
                return default(Guid);
            }

            return user.Id;
        }
    }
}