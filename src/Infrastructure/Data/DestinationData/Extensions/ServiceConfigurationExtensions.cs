using DataUploader.Domain.Interfaces;
using DestinationData;
using DestinationData.Mapper;
using Microsoft.EntityFrameworkCore;
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
        /// Метод добавления зависимостей для работы с БД data.
        /// </summary>
        /// <param name="services"><inheritdoc cref="IServiceCollection"/></param>
        /// <param name="configuration"><inheritdoc cref="IConfiguration"/></param>
        public static void AddDestinationDataContextDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration.GetConnectionString("DataDBConnection");
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(connection, x => x.MigrationsAssembly("DestinationData"));
            });
            
            services.AddAutoMapper(cfg => { }, 
                typeof(FieldProfile),
                typeof(OrganizationProfile),
                typeof(WellProfile),
                typeof(TechModeProfile));
            services.AddTransient<IDestinationDataRepository, DestinationDataRepository>();
        }
    }
}
