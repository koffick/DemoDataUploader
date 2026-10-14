using AuditData.Mapper;
using DataUploader.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuditData.Extensions
{
    /// <summary>
    /// Класс расширение для настройки работы с контекстом данных.
    /// </summary>
    public static class ServiceConfigurationExtensions
    {
        /// <summary>
        /// Метод добавления зависимостей для работы с БД user.
        /// </summary>
        /// <param name="services"><inheritdoc cref="IServiceCollection"/></param>
        /// <param name="configuration"><inheritdoc cref="IConfiguration"/></param>
        public static void AddAuditDataContextDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(cfg => { }, typeof(UserActionAuditProfile));
            services.AddTransient<IAuditRepository, AuditRepository>();
        }
    }
}
