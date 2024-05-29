var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>();
builder.Services.AddSingleton<IQueueService, QueueService>();

builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<IRepository<Document>, DocumentRepository>();

var host = builder.Build();
host.Run();