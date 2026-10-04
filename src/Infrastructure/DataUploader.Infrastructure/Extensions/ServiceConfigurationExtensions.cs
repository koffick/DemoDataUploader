using DataUploader.Domain.Interfaces;
using DataUploader.Infrastructure.Parsers;
using DataUploader.Infrastructure.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataUploader.Infrastructure.Extensions
{
    /// <summary>
    /// Класс расширение для настройки работы с инфраструктурой.
    /// </summary>
    public static class ServiceConfigurationExtensions
    {
        /// <summary>
        /// Метод добавления зависимостей для работы с классами инфраструктцы.
        /// </summary>
        /// <param name="services"><inheritdoc cref="IServiceCollection"/></param>
        /// <param name="configuration"><inheritdoc cref="IConfiguration"/></param>
        public static void AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IExcelParser, ExcelParser>();
            services.AddTransient<IFileProvider, FileProviderStub>();
        }
    }
}
