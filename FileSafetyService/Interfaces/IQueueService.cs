namespace FileSafetyService.Interfaces;

public interface IQueueService
{
    void Get(IMessage message);
}