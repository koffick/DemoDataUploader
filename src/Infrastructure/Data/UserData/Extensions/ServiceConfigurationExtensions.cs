using DataUploader.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace UserData.Extensions
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
        public static void AddUserDataContextDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            string connection = configuration.GetConnectionString("UserDBConnection");
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(connection, x => x.MigrationsAssembly("UserData"));
            });

            services.AddTransient<IUserRepository, UserRepository>();
        }
    }
}
