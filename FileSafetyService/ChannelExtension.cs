namespace FileSafetyService;

public static class ChannelExtension
{
    public static EventingBasicConsumer EPSetupConsumer(this IModel channel)
    {
        channel.QueueDeclare(
            queue: "filesafetyqueue",
            durable: false,
            exclusive: false,
            autoDelete: false
        );

        channel.BasicQos(
            prefetchSize: 0,
            prefetchCount: 1,
            global: false
        );

        var consumer = new EventingBasicConsumer(channel);
        
        consumer.Received += (_, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();

            var message = new FileMessage(body);

            Console.WriteLine(message);
            
            channel.BasicAck(
                deliveryTag: eventArgs.DeliveryTag,
                multiple: false
            );
        };
        
        channel.BasicConsume(
            queue: "filesafetyqueue",
            autoAck: false,
            consumer: consumer
        );

        return consumer;
    }
}