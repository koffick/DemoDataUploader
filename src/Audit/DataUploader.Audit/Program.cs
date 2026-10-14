using AutoMapper;
using DataUploader.Audit.Interfaces;
using DataUploader.Audit.Services;
using Itsk.Vma.SPekTR.BusinessAudit.Queue.Handlers;
using AuditData.Extensions;
using DataUploader.RabbitMQ.Extensions;
using DataUploader.RabbitMQ.Models;

const string RABBIT_CONNECTIONS_SECTION = "RabbitMQConnections";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IAuditService, AuditService>();
builder.Services.AddAuditDataContextDependencies(builder.Configuration);
var connectionCredentials = builder.Configuration.GetSection(RABBIT_CONNECTIONS_SECTION).Get<List<ConnectionCredentials>>();
builder.Services.AddRabbitMqDependencies(connectionCredentials);

builder.Services.AddHostedService(serviceProvider =>
    new UserActionMessageHandler(
        serviceProvider.GetRequiredService<RabbitMQConnectionsWrapper>(),
        serviceProvider.GetRequiredService<ILogger<UserActionMessageHandler>>(),
        "DataUploader.Audit",
        "DataUploader.Audit.UserAction",
        serviceProvider.GetRequiredService<IMapper>(),
        serviceProvider.GetRequiredService<IServiceProvider>()
        )
    );


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
