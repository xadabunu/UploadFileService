namespace API.Services;

public class QueueService(IConnectionFactory factory) : IQueueService
{
    private readonly IConnection Connection = factory.CreateConnection("localhost");

    public void Send(FileMessage message)
    {
        using var channel = Connection.CreateModel();
        
        channel.EPSetupProducer();
        
        var messageStr = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageStr);
        
        channel.BasicPublish(
            exchange: string.Empty,
            routingKey: "file-checking-queue",
            basicProperties: null,
            body: body
        );
    }

    public void Get(IMessage message)
    {
        throw new NotImplementedException();
    }
}