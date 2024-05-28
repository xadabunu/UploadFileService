namespace API;

public interface IQueueService
{
    void Send(IMessage message);
}