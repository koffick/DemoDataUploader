using AutoMapper;
using DataUploader.Engine.Handlers;
using DataUploader.Engine.Interfaces;
using DataUploader.Engine.Mapper;
using DataUploader.Engine.Services;
using DataUploader.RabbitMQ.Models;
using DataUploader.RabbitMQ.Extensions;

const string RABBIT_CONNECTIONS_SECTION = "RabbitMQConnections";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(cfg => { },
    typeof(FileDataParseProfile));

builder.Services.AddTransient<IDataProcessingService, DataProcessingService>();
builder.Services.AddMinioFileStorage(builder.Configuration);

var connectionCredentials = builder.Configuration.GetSection(RABBIT_CONNECTIONS_SECTION).Get<List<ConnectionCredentials>>();
builder.Services.AddRabbitMqDependencies(connectionCredentials);

builder.Services.AddHostedService(serviceProvider =>
    new FileToProccessHandler(
        serviceProvider.GetRequiredService<RabbitMQConnectionsWrapper>(),
        serviceProvider.GetRequiredService<ILogger<FileToProccessHandler>>(),
        "DataUploader.Engine",
        "DataUploader.Engine.FileToProccessing",
        serviceProvider.GetRequiredService<IMapper>(),
        serviceProvider.GetRequiredService<IServiceProvider>()
        )
    );

var app = builder.Build();

app.Run();