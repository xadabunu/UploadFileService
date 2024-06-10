var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>();
builder.Services.AddSingleton<IQueueService, QueueService>();

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<IRepository<Document>, DocumentRepository>();

builder.Services.AddHttpClient("API", client => client.BaseAddress = new Uri(builder.Configuration["ApiUrl"]));

var host = builder.Build();
host.Run();