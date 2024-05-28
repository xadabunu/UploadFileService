namespace API.Services;

public class QueueService : IQueueService
{
    private readonly IConnectionFactory _factory;
    private readonly IConnection Connection;
    
    public QueueService(IConnectionFactory factory)
    {
        _factory = factory;
        Connection = _factory.CreateConnection("localhost");
    }
    public void Send(IMessage message)
    {
        using var channel = Connection.CreateModel();
        
        channel.EPSetupProducer();

        var bytesMessage = message.Encode();
        
        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: "filesafetyqueue",
            basicProperties: null,
            body: bytesMessage
        );
    }
}