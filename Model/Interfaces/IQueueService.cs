using Model.Entities;

namespace Model.Interfaces;

public interface IQueueService
{
    void Send(FileMessage message);
    void Get(IMessage message);
}