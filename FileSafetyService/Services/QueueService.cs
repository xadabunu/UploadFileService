namespace FileSafetyService.Services;

public class QueueService(IConnectionFactory factory) : IQueueService
{
    private readonly IConnection Connection = factory.CreateConnection("localhost");

    public void Send(FileMessage message)
    {
        throw new NotImplementedException();
    }

    public void Get(IMessage message)
    {
        throw new NotImplementedException();
    }
}