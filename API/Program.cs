using API.middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<IRepository<Document>, DocumentRepository>();
builder.Services.AddScoped<IRepository<Demande>, DemandeRepository>();

builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>();
builder.Services.AddSingleton<IQueueService, QueueService>();

builder.Services.AddSignalR();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.MapFileEndpoints();
app.MapDemandeEndpoints();

app.MapHub<MyHub>("/huburl");

app.Run();