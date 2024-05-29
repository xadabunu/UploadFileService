using System.Security.Cryptography;

namespace FileSafetyService;

public static class ChannelExtension
{
    public static EventingBasicConsumer EPSetupConsumer(this IModel channel, IRepository<Document> repository)
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
        
        consumer.Received += async (_, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = new FileMessage(body);

            // message.Display();
            
            var document = await repository.GetById(message.DocumentId);

            var path = $"C:/Eprolex/id_{document.DemandeId}/{document.TypeCode}/{document.Nom}";

            // if (!CheckExtension(path))
            // {
            //     Console.WriteLine("L'extension ne correspond pas.");
            //
            //     document.StatutCode = StatutDocument.Corrompu.Code;
            //     await repository.Update(document);
            //     // suppresion physique ?
            // }
            // else
            // {
            //     ;
            //     // autres vérification (ItextSharp ?)
            //     // mais copilot conseille dans chaque msg de passer par un av)
            // }

            if (!await ScanFile(path))
            {
                Console.WriteLine("Fichier corrompu");
            }

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

    private static async Task<bool> ScanFile(string path)
    {
        const int probability = 1;
        
        Task.Delay(TimeSpan.FromSeconds(5));

        return RandomNumberGenerator.GetInt32(100) < probability;
    }

    private static bool CheckExtension(string path)
    {
        var fileExtension = Path.GetExtension(path).ToLower();
        var fileMagicNumber = ReadFirstBytes(path);

        var fileType = FileExtension.GetNomFromMagicNumber(fileMagicNumber);
        
        return fileType != null && fileType == fileExtension;
    }

    private static string ReadFirstBytes(string path)
    {
        using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        var binaryReader = new BinaryReader(fileStream);
        var bytes = binaryReader.ReadBytes(4);

        return BitConverter.ToString(bytes).Replace("-", string.Empty);
    }
}