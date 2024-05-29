using FileSafetyService.Interfaces;

namespace FileSafetyService.Services;

public class QueueService : IQueueService
{
    private readonly IConnectionFactory _factory;
    private readonly IConnection Connection;
    
    public QueueService(IConnectionFactory factory)
    {
        _factory = factory;
        Connection = _factory.CreateConnection("localhost");
    }
    
    public void Get(IMessage message)
    {
        throw new NotImplementedException();
    }
}