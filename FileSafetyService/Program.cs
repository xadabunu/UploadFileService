using FileSafetyService;
using FileSafetyService.Interfaces;
using FileSafetyService.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>();
builder.Services.AddSingleton<IQueueService, QueueService>();

var host = builder.Build();
host.Run();