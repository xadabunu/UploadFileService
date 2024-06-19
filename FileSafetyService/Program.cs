var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<IRepository<Document>, DocumentRepository>();

builder.Services.AddHttpClient("API", client => client.BaseAddress = new Uri(builder.Configuration["ApiUrl"]));

builder.Services.AddMassTransit(configure =>
{
    configure.AddConsumer<FileMessageConsumer>();
    
    configure.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        
        cfg.ConfigureEndpoints(context);
    });
});

var host = builder.Build();
host.Run();