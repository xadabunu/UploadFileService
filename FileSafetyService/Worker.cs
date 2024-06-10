namespace FileSafetyService;

public class Worker(ILogger<Worker> logger, IRepository<Document> repository, IHttpClientFactory factory)
    : BackgroundService
{
    private readonly HttpClient client = factory.CreateClient("API");

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        var factory = new ConnectionFactory
        {
            DispatchConsumersAsync = true,
            ConsumerDispatchConcurrency = Environment.ProcessorCount
        };

        using var connection = factory.CreateConnection("localhost");
        using var channel = connection.CreateModel();

        channel.EPSetupConsumer(repository, client);

        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.ReadKey();
        // while (!stoppingToken.IsCancellationRequested)
        // {
        //     await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
        // }
    }
}