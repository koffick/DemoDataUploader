using DataUploader.RabbitMQ.Models;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace DataUploader.RabbitMQ.Extensions;

/// <summary>
/// Расширение IServiceCollection
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет зависимости для работы с RabbitMQ.
    /// </summary>
    /// <param name="services"><inheritdoc cref="IServiceCollection"/></param>
    /// <param name="connectionCredentials">Параметры подключений</param>
    public static void AddRabbitMqDependencies(
        this IServiceCollection services,
        IEnumerable<ConnectionCredentials> connectionCredentials)
    {
        var connections = GetConnectionDescriptions(connectionCredentials);
        services.AddSingleton(new RabbitMQConnectionsWrapper(connections.ToArray()));
    }

    private static IEnumerable<ConnectionDescription> GetConnectionDescriptions(IEnumerable<ConnectionCredentials> connectionCredentials)
    {
        var result = new List<ConnectionDescription>();
        if (connectionCredentials == null)
        {
            return result;
        }

        foreach (var connectionCreditial in connectionCredentials)
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = connectionCreditial.Host,
                Port = connectionCreditial.Port,
                VirtualHost = connectionCreditial.VirtualHost,
                UserName = connectionCreditial.UserName,
                Password = connectionCreditial.Password,
            };

            result.Add(new ConnectionDescription
            {
                Connection = connectionFactory.CreateConnectionAsync().Result,
                ExchangeName = connectionCreditial.ExchangeName,
            });
        }

        return result;
    }
}