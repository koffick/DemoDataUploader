using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Storage.Minio;

public static class MinioServiceCollectionExtensions
{
    public static IServiceCollection AddMinioFileStorage(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection(MinioOptions.SectionName));

        services.AddSingleton<IMinioClient>(sp =>
        {
            var opts = configuration
                .GetSection(MinioOptions.SectionName)
                .Get<MinioOptions>() ?? throw new InvalidOperationException("Опции Minio не заданы!");

            var builder = new MinioClient()
                .WithEndpoint(opts.Endpoint)
                .WithCredentials(opts.AccessKey, opts.SecretKey);

            if (opts.UseSsl)
                builder = builder.WithSSL();

            return builder.Build();
        });

        services.AddSingleton<IFileStorageProvider, MinioFileStorageProvider>();
        return services;
    }
}