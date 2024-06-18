namespace FileSafetyService.Extensions;

public static class ChannelExtension
{
    public static void EPSetupConsumer(this IModel channel, IRepository<Document> repository, HttpClient client)
    {
        channel.QueueDeclare(
            queue: "file-checking-queue",
            durable: false,
            exclusive: false,
            autoDelete: false
        );

        channel.BasicQos(
            prefetchSize: 0,
            prefetchCount: 12,
            global: false
        );

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (_, eventArgs) =>
        {
            var message = GetMessageFromArgs(eventArgs);

            var document = await repository.GetById(message.DocumentId);

            if (document is not null) // si le déposant retire le document avant le traitement du message
            {
                var scanResultMessage = await CheckDocument(document, repository, message.ConnectionId);

                await client.PostAsJsonAsync("/scanresult", scanResultMessage);
            }

            channel.BasicAck(
                deliveryTag: eventArgs.DeliveryTag,
                multiple: false
            );
        };

        channel.BasicConsume(
            queue: "file-checking-queue",
            autoAck: false,
            consumer: consumer
        );
    }

    private static FileMessage GetMessageFromArgs(BasicDeliverEventArgs eventArgs)
    {
        var body = eventArgs.Body.ToArray();
        var messageStr = Encoding.UTF8.GetString(body);

        return JsonSerializer.Deserialize<FileMessage>(messageStr);
    }

    private static async Task<ScanResultMessage> CheckDocument(Document document, IRepository<Document> repository, string connectionId)
    {
        var path = $"C:/Eprolex/id_{document.DemandeId}/{document.TypeCode}/{document.Nom}";

        var fileIsSafe = await ScanFile(path);

        document.StatutCode = fileIsSafe ? StatutDocument.Valide.Code : StatutDocument.Corrompu.Code;

        if (!fileIsSafe)
        {
            File.Delete(path);
        }

        // if (!CheckExtension(path))
        // {
        //     Console.WriteLine("Pas un pdf.");
        //
        //     // conversion
        // }

        await repository.Update(document);

        return new ScanResultMessage(document.DemandeId, document.Id, document.StatutCode, connectionId);
    }

    private static async Task<bool> ScanFile(string path)
    {
        const int safetyProbability = 99;

        await Task.Delay(TimeSpan.FromSeconds(3));

        return RandomNumberGenerator.GetInt32(100) < safetyProbability;
    }

    private static bool CheckExtension(string path)
    {
        var fileExtension = Path.GetExtension(path).ToLower();

        return fileExtension.Equals("pdf");
    }
}