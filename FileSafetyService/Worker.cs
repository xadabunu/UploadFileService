namespace FileSafetyService;

public class Worker(ILogger<Worker> logger, IRepository<Document> repository) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory();

        factory.DispatchConsumersAsync = true;

        factory.ConsumerDispatchConcurrency = Environment.ProcessorCount;
        
        using var connection = factory.CreateConnection("localhost");
        
        using var channel = connection.CreateModel();

        channel.EPSetupConsumer(repository);

        Console.ReadKey();
    }
}