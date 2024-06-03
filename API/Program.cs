using Model.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<IRepository<Document>, DocumentRepository>();
builder.Services.AddScoped<IRepository<Demande>, DemandeRepository>();

builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>();
builder.Services.AddSingleton<IQueueService, QueueService>();

var app = builder.Build();

app.MapFileEndpoints();
app.MapDemandeEndpoints();

app.Run();