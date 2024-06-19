var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<IRepository<Document>, DocumentRepository>();
builder.Services.AddScoped<IRepository<Demande>, DemandeRepository>();

builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>();

builder.Services.AddSignalR();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.MapFileEndpoints();
app.MapDemandeEndpoints();

app.MapHub<MyHub>("/huburl");

app.Run();