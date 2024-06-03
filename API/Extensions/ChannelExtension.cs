namespace API.Repositories;

public static class ChannelExtension
{
    public static void EPSetupProducer(this IModel channel)
    {
        channel.QueueDeclare(
            queue: "file-checking-queue",
            durable: false,
            exclusive: false,
            autoDelete: false
        );
    }
}