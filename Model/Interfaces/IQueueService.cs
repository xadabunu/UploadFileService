namespace Model.Interfaces;

public interface IQueueService
{
    void Send(IMessage message);
    void Get(IMessage message);
}