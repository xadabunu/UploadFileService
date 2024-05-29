namespace API.Services;

public class QueueService(IConnectionFactory factory) : IQueueService
{
    private readonly IConnection Connection = factory.CreateConnection("localhost");

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